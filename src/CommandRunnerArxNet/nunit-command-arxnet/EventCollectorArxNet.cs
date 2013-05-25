// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.1.25：
//  在NUnit2.6.2基础上修改
// 2013.5.25：
//  使用EditorWritor类在Editor输出
// ****************************************************************

using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using NUnit.Core;
using NUnit.Util;

using NUnit.ConsoleRunner;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

namespace NUnit.CommandRunner.ArxNet
{
	/// <summary>
	/// Summary description for EventCollector.
	/// </summary>
	public class EventCollectorArxNet : MarshalByRefObject, EventListener
	{
		private int testRunCount;
		private int testIgnoreCount;
		private int failureCount;
		private int level;

        private CommandOptionsArxNet options;//2013.1.25改
		private TextWriter outWriter;
		private TextWriter errorWriter;

		StringCollection messages;
		
		private bool progress = false;
		private string currentTestName;

		private ArrayList unhandledExceptions = new ArrayList();

        public EventCollectorArxNet(CommandOptionsArxNet options, TextWriter outWriter, TextWriter errorWriter)//2013.1.25改
		{
			level = 0;
			this.options = options;
			this.outWriter = outWriter;
			this.errorWriter = errorWriter;
			this.currentTestName = string.Empty;
			this.progress = !options.xmlConsole && !options.labels && !options.nodots;

			AppDomain.CurrentDomain.UnhandledException += 
				new UnhandledExceptionEventHandler(OnUnhandledException);
		}

		public bool HasExceptions
		{
			get { return unhandledExceptions.Count > 0; }
		}

		public void WriteExceptions()
		{
            /*2013.1.25改*/
            /*
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n");
            ed.WriteMessage("\nUnhandled exceptions:");
            int index = 1;
            foreach (string msg in unhandledExceptions)
                ed.WriteMessage("\n{0}) {1}", index++, msg);
            */
            /*2013.1.25改*/

            /*2013.5.25lq改*/
            Console.WriteLine();
            Console.WriteLine("Unhandled exceptions:");
            int index = 1;
            foreach (string msg in unhandledExceptions)
                Console.WriteLine("{0}) {1}", index++, msg);
            /*2013.5.25lq改*/
		}

		public void RunStarted(string name, int testCount)
		{
		}

		public void RunFinished(TestResult result)
		{
		}

        public void RunFinished(System.Exception exception)//2013.1.25改
		{
		}

		public void TestFinished(TestResult testResult)
		{
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            switch( testResult.ResultState )
            {
                case ResultState.Error:
                case ResultState.Failure:
                case ResultState.Cancelled:
                    testRunCount++;
			        failureCount++;
    					
			        if ( progress )
                        //ed.WriteMessage("F");//2013.1.25改
                        Console.Write("F");//2013.5.25lq改
    					
			        messages.Add( string.Format( "{0}) {1} :", failureCount, testResult.Test.TestName.FullName ) );
			        messages.Add( testResult.Message.Trim( Environment.NewLine.ToCharArray() ) );

			        string stackTrace = StackTraceFilter.Filter( testResult.StackTrace );
			        if ( stackTrace != null && stackTrace != string.Empty )
			        {
				        string[] trace = stackTrace.Split( System.Environment.NewLine.ToCharArray() );
				        foreach( string s in trace )
				        {
					        if ( s != string.Empty )
					        {
						        string link = Regex.Replace( s.Trim(), @".* in (.*):line (.*)", "$1($2)");
						        messages.Add( string.Format( "at\n{0}", link ) );
					        }
				        }
			        }
                    break;

                case ResultState.Inconclusive:
                case ResultState.Success:
                    testRunCount++;
                    break;

                case ResultState.Ignored:
                case ResultState.Skipped:
                case ResultState.NotRunnable:
    				testIgnoreCount++;
					
	    			if ( progress )
                        //ed.WriteMessage("N");//2013.1.25改
                        Console.Write("N");//2013.5.25改
                    break;
			}

			currentTestName = string.Empty;
		}

		public void TestStarted(TestName testName)
		{
            currentTestName = testName.FullName;

            if (options.labels)
                outWriter.WriteLine("***** {0}", currentTestName);

            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加
            if (progress)
                //ed.WriteMessage(".");//2013.1.25改
                Console.Write(".");//2013.5.25lq改
		}

		public void SuiteStarted(TestName testName)
		{
            if (level++ == 0)
            {
                messages = new StringCollection();
                testRunCount = 0;
                testIgnoreCount = 0;
                failureCount = 0;
                Trace.WriteLine("################################ UNIT TESTS ################################");
                Trace.WriteLine("Running tests in '" + testName.FullName + "'...");
            }            
		}

		public void SuiteFinished(TestResult suiteResult) 
		{
			if ( --level == 0) 
			{
				Trace.WriteLine( "############################################################################" );

				if (messages.Count == 0) 
				{
					Trace.WriteLine( "##############                 S U C C E S S               #################" );
				}
				else 
				{
					Trace.WriteLine( "##############                F A I L U R E S              #################" );
						
					foreach ( string s in messages ) 
					{
						Trace.WriteLine(s);
					}
				}

				Trace.WriteLine( "############################################################################" );
				Trace.WriteLine( "Executed tests       : " + testRunCount );
				Trace.WriteLine( "Ignored tests        : " + testIgnoreCount );
				Trace.WriteLine( "Failed tests         : " + failureCount );
				Trace.WriteLine( "Unhandled exceptions : " + unhandledExceptions.Count);
				Trace.WriteLine( "Total time           : " + suiteResult.Time + " seconds" );
				Trace.WriteLine( "############################################################################");
			}
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject.GetType() != typeof(System.Threading.ThreadAbortException))
			{
                this.UnhandledException((System.Exception)e.ExceptionObject);//2013.1.25改
			}
		}

        public void UnhandledException(System.Exception exception)//2013.1.25改
		{
			// If we do labels, we already have a newline
			unhandledExceptions.Add(currentTestName + " : " + exception.ToString());
			//if (!options.labels) outWriter.WriteLine();
			string msg = string.Format("##### Unhandled Exception while running {0}", currentTestName);
			//outWriter.WriteLine(msg);
			//outWriter.WriteLine(exception.ToString());

			Trace.WriteLine(msg);
			Trace.WriteLine(exception.ToString());
		}

		public void TestOutput( TestOutput output)
		{
			switch ( output.Type )
			{
				case TestOutputType.Out:
					outWriter.Write( output.Text );
					break;
				case TestOutputType.Error:
					errorWriter.Write( output.Text );
					break;
			}
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
