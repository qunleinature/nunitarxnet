// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.1.25：
//  在NUnit2.6.2基础上修改
// ****************************************************************

using System;
using System.IO;
using System.Reflection;
using NUnit.Core;
using NUnit.Util;

using NUnit.ConsoleRunner;

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

        /// <summary>
        /// AutoCad .net应用程序入口点
        /// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
            CommandOptionsArxNet options = new CommandOptionsArxNet(args);//2013.1.25改

            // Create SettingsService early so we know the trace level right at the start
            SettingsService settingsService = new SettingsService();
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

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加

            if (options.cleanup)
            {
                log.Info("Performing cleanup of shadow copy cache");
                DomainManager.DeleteShadowCopyPath();
                ed.WriteMessage("\nShadow copy cache emptied");//2013.1.25改
                return CommandUiArxNet.OK;//2013.1.25改
            }

            if (options.NoArgs) 
			{				
                ed.WriteMessage("\nfatal error: no inputs specified");//2013.1.25改
                options.Help();
                return CommandUiArxNet.OK;//2013.1.25改
			}
			
			if(!options.Validate())
			{
                foreach (string arg in options.InvalidArguments)
                    ed.WriteMessage("\nfatal error: invalid argument: {0}", arg);//2013.1.25改
                options.Help();
                return CommandUiArxNet.INVALID_ARG;//2013.1.25改
			}

			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( settingsService );
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			ServiceManager.Services.AddService( new ProjectService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );
            ServiceManager.Services.AddService( new TestAgency() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

            foreach (string parm in options.Parameters)
            {
                if (!Services.ProjectService.CanLoadProject(parm) && !PathUtils.IsAssemblyFileType(parm))
                {
                    ed.WriteMessage("\nFile type not known: {0}", parm);//2013.1.25改
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
                ed.WriteMessage("\n" + ex.Message);//2013.1.25改
                return CommandUiArxNet.FILE_NOT_FOUND;//2013.1.25改
			}
            /*2013.1.25加*/
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nAutoCAD Exception:\n{0}", ex.ToString());
                return CommandUiArxNet.UNEXPECTED_ERROR;
            }
            /*2013.1.25加*/
            catch (System.Exception ex)//2013.1.25改
			{
                ed.WriteMessage("\nUnhandled Exception:\n{0}", ex.ToString());//2013.1.25改
                return CommandUiArxNet.UNEXPECTED_ERROR;//2013.1.25改
			}
			finally
			{
				if(options.wait)
				{
                    ed.GetString(new PromptStringOptions("\n\nHit <enter> key to continue"));//2013.1.25改
				}

                log.Info("nunit-command-arxnet.dll terminating");//2013.1.25改
			}

		}

		private static void WriteCopyright()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string versionText = executingAssembly.GetName().Version.ToString();

#if CLR_1_0
            string productName = "NUnit-Console (.NET 1.0)";
#elif CLR_1_1
            string productName = "NUnit-Console (.NET 1.1)";
#else
            string productName = "NUnit-Command-ArxNet";//2013.1.25改
#endif
            string copyrightText = "Copyright (C) 2013 Lei Qun.\r\nCopyright (C) 2002-2012 Charlie Poole.\r\nCopyright (C) 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov.\r\nCopyright (C) 2000-2002 Philip Craig.\r\nAll Rights Reserved.";

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
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage(String.Format("\n{0} version {1}", productName, versionText));
            ed.WriteMessage("\n" + copyrightText);
            ed.WriteMessage("\n");

            ed.WriteMessage("\nRuntime Environment - ");
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;
            ed.WriteMessage(string.Format("\n   OS Version: {0}", Environment.OSVersion));
            ed.WriteMessage(string.Format("\n  CLR Version: {0} ( {1} )",
                Environment.Version, framework.DisplayName));
            ed.WriteMessage(string.Format("\n ACAD Version: {0} ", Application.Version));//AutoCAD版本

            ed.WriteMessage("\n");
            /*2013.1.25改*/
		}

        /// <summary>
        /// 初始化类
        /// </summary>
        public static void Init()
        {
            throw new System.NotImplementedException();
        }
	}
}
