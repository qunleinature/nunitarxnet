// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.1.25：
//      在NUnit2.6.2基础上修改
//  2013.5.25：
//      使用EditorWritor类在Editor输出
//  2013.5.27：
//      1.ServiceManager改为ServiceManagerArxNet
//      2.Init方法增加ServiceManagerArxNet初始化
//      3.DomainManager改为DomainManagerArxNet
//      4.Init方法增加DomainManagerArxNet初始化
//      5.Services改为ServicesArxNet
//      6.Init方法增加ServicesArxNet初始化
//  2013.5.29：
//      1.改AddinManager为AddinManagerArxNet
//      2.改ProjectService为ProjectServiceArxNet
//  2013.6.7
//      1.已改SettingsServiceArxNet
//  2013.6.28
//      1.已改CommandOptionsArxNet
//  2014.8.21：
//      在NUnit2.6.3基础上修改
//  2015.1.3：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.IO;
using System.Reflection;

using NUnit.Core;
using NUnit.Util;

using NUnit.ConsoleRunner;
using NUnit.Core.ArxNet;
using NUnit.Util.ArxNet;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

namespace NUnit.CommandRunner.ArxNet
{
	/// <summary>
	/// Summary description for Runner.
	/// </summary>
	public class RunnerArxNet
	{
		static Logger log = InternalTrace.GetLogger(typeof(RunnerArxNet));
        /*2013.5.27lq加*/
        private static EditorWriter m_EditorWriter = null;
        private static TextWriter m_SavedOut = null;
        private static TextWriter m_SavedError = null;

        /// <summary>
        /// AutoCad .net应用程序入口点
        /// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
            CommandOptionsArxNet options = new CommandOptionsArxNet(args);//2013.1.25改

            // Create SettingsService early so we know the trace level right at the start
            SettingsServiceArxNet settingsService = new SettingsServiceArxNet();
            InternalTraceLevel level = (InternalTraceLevel)settingsService.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default);
            if (options.trace != InternalTraceLevel.Default)
                level = options.trace;

            InternalTrace.Initialize("nunit-command_%p.log", level);//2013.1.25改

            log.Info("nunit-command-arxnet.dll starting");//2013.1.25改

			if(!options.nologo)
				WriteCopyright();

			if(options.help)
			{
				options.Help();
                return CommandUiArxNet.OK;//2013.1.25改
			}

            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加

            if (options.cleanup)
            {
                log.Info("Performing cleanup of shadow copy cache");
                DomainManagerArxNet.DeleteShadowCopyPath();
                //ed.WriteMessage("\nShadow copy cache emptied");//2013.1.25改
                Console.WriteLine("Shadow copy cache emptied");//2013.5.25lq改
                return CommandUiArxNet.OK;//2013.1.25改
            }

            if (options.NoArgs) 
			{				
                //ed.WriteMessage("\nfatal error: no inputs specified");//2013.1.25改
                //Console.WriteLine("fatal error: no inputs specified");//2013.5.25lq改
                Console.Error.WriteLine("fatal error: no inputs specified");//2013.5.25lq改
                options.Help();
                return CommandUiArxNet.OK;//2013.1.25改
			}
			
			if(!options.Validate())
			{
                foreach (string arg in options.InvalidArguments)
                    //ed.WriteMessage("\nfatal error: invalid argument: {0}", arg);//2013.1.25改
                    //Console.WriteLine("fatal error: invalid argument: {0}", arg);//2013.5.25lq改
                    Console.Error.WriteLine("fatal error: invalid argument: {0}", arg);//2013.5.25lq改
                options.Help();
                return CommandUiArxNet.INVALID_ARG;//2013.1.25改
			}

			// Add Standard Services to ServiceManager
			ServiceManagerArxNet.Services.AddService( settingsService );
			ServiceManagerArxNet.Services.AddService( new DomainManagerArxNet() );
			//ServiceManagerArxNet.Services.AddService( new RecentFilesService() );
			ServiceManagerArxNet.Services.AddService( new ProjectServiceArxNet() );
			//ServiceManagerArxNet.Services.AddService( new TestLoaderArxNet() );
			ServiceManagerArxNet.Services.AddService( new AddinRegistry() );
			ServiceManagerArxNet.Services.AddService( new AddinManagerArxNet() );
            ServiceManagerArxNet.Services.AddService( new TestAgency() );

			// Initialize Services
			ServiceManagerArxNet.Services.InitializeServices();

            foreach (string parm in options.Parameters)
            {
                if (!ServicesArxNet.ProjectService.CanLoadProject(parm) && !PathUtils.IsAssemblyFileType(parm))
                {
                    //ed.WriteMessage("\nFile type not known: {0}", parm);//2013.1.25改
                    Console.WriteLine("File type not known: {0}", parm);//2013.5.25lq改
                    return CommandUiArxNet.INVALID_ARG;//2013.1.25改
                }
            }

			try
			{
                CommandUiArxNet commandUiArxNet = new CommandUiArxNet();//2013.1.25改
                return commandUiArxNet.Execute(options);//2013.1.25改
			}
			catch( FileNotFoundException ex )
			{
                //ed.WriteMessage("\n" + ex.Message);//2013.1.25改
                Console.WriteLine(ex.Message);//2013.5.25lq改
                return CommandUiArxNet.FILE_NOT_FOUND;//2013.1.25改
			}
            /*2013.1.25加*/
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                //ed.WriteMessage("\nAutoCAD Exception:\n{0}", ex.ToString());
                Console.WriteLine("AutoCAD Exception:\n{0}", ex.Message);//2013.5.25lq改
                return CommandUiArxNet.UNEXPECTED_ERROR;
            }
            /*2013.1.25加*/
            catch (System.Exception ex)//2013.1.25改
			{
                //ed.WriteMessage("\nUnhandled Exception:\n{0}", ex.ToString());//2013.1.25改
                Console.WriteLine("Unhandled Exception:\n{0}", ex.ToString());//2013.5.25改
                return CommandUiArxNet.UNEXPECTED_ERROR;//2013.1.25改
			}
			finally
			{
				if(options.wait)
				{
                    //Console.Out.WriteLine("\nHit <enter> key to continue");
                    //Console.ReadLine();
                    //ed.GetString(new PromptStringOptions("\n\nHit <enter> key to continue"));//2013.1.25改/
                    m_EditorWriter.Editor.GetString(new PromptStringOptions("\n\nHit <enter> key to continue"));//2013.5.25lq改
				}

                log.Info("nunit-command-arxnet.dll terminating");//2013.1.25改
			}

		}

		private static void WriteCopyright()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string versionText = executingAssembly.GetName().Version.ToString();

#if CLR_1_0
            string productName = "NUnit-Command-ArxNet (.NET 1.0)";//2013.9.27lq改
#elif CLR_1_1
            string productName = "NUnit-Command-ArxNet (.NET 1.1)";//2013.9.27lq改
#else
            string productName = "NUnit-Command-ArxNet";//2013.1.25改
#endif
            string copyrightText = "Copyright (C) 2015 Lei Qun.\r\nCopyright (C) 2002-2012 Charlie Poole.\r\nCopyright (C) 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov.\r\nCopyright (C) 2000-2002 Philip Craig.\r\nAll Rights Reserved.";//2015.1.3lq改//2013.9.27lq改
                        
            //object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            //if ( objectAttrs.Length > 0 )
            //    productName = ((AssemblyProductAttribute)objectAttrs[0]).Product;

			object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			if ( objectAttrs.Length > 0 )
				copyrightText = ((AssemblyCopyrightAttribute)objectAttrs[0]).Copyright;

			objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
            if (objectAttrs.Length > 0)
            {
                string configText = ((AssemblyConfigurationAttribute)objectAttrs[0]).Configuration;
                if (configText != "")
                    versionText += string.Format(" ({0})", configText);
            }
            
            /*2013.1.25改*/
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage(String.Format("\n{0} version {1}", productName, versionText));
            //ed.WriteMessage("\n" + copyrightText);
            //ed.WriteMessage("\n");

            //ed.WriteMessage("\nRuntime Environment - ");
            /*2013.5.25改*/
            Console.WriteLine(String.Format("{0} version {1}", productName, versionText));
            Console.WriteLine(copyrightText);
            Console.WriteLine();

            Console.WriteLine("Runtime Environment - ");
            /*2013.5.25改*/
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;            
            //ed.WriteMessage(string.Format("\n   OS Version: {0}", Environment.OSVersion));
            //ed.WriteMessage(string.Format("\n  CLR Version: {0} ( {1} )",
                //Environment.Version, framework.DisplayName));
            //ed.WriteMessage(string.Format("\n ACAD Version: {0} ", Application.Version));//AutoCAD版本

            //ed.WriteMessage("\n");
            /*2013.5.25改*/
            Console.WriteLine(string.Format("   OS Version: {0}", Environment.OSVersion));
            Console.WriteLine(string.Format("  CLR Version: {0} ( {1} )",
                Environment.Version, framework.DisplayName));
            Console.WriteLine(string.Format(" ACAD Version: {0} ", Application.Version));//AutoCAD版本

            Console.WriteLine();
            /*2013.5.25改*/
            /*2013.1.25改*/
		}

        /// <summary>
        /// 初始化类
        /// </summary>
        public static void Init()//2013.5.24加
        {
            //throw new System.NotImplementedException();
            log = InternalTrace.GetLogger(typeof(RunnerArxNet));

            ServiceManagerArxNet.Init();
            DomainManagerArxNet.Init();
            ServicesArxNet.Init();

            m_EditorWriter = new EditorWriter();
            m_SavedOut = Console.Out;
            m_SavedError = Console.Error;
            Console.SetOut(m_EditorWriter);
            Console.SetError(m_EditorWriter);
        }

        /// <summary>
        /// 清除类
        /// </summary>
        public static void CleanUp()//2013.5.25加
        {
            //throw new System.NotImplementedException();
            //恢复错误输出流
            if (m_SavedError != null)
            {
                Console.SetError(m_SavedError);
            }
            else//设置标准错误输出流
            {
                StreamWriter standardError = new StreamWriter(Console.OpenStandardError());
                standardError.AutoFlush = true;
                Console.SetError(standardError);
            }

            //恢复输出流
            if (m_SavedOut != null)
            {
                Console.SetOut(m_SavedOut);
            }
            else//设置标准输出流
            {
                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }

            //清除EditorWriter
            if (m_EditorWriter != null)
            {
                m_EditorWriter.Close();
                m_EditorWriter = null;
            }
        }
	}
}
