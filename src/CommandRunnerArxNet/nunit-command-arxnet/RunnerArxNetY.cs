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
    /// Summary description for RunnerArxNet
    /// </summary>
    public class RunnerArxNet : Runner
    {
        static Logger log = InternalTrace.GetLogger(typeof(RunnerArxNet));

        /// <summary>
        /// AutoCad .net应用程序入口点
        /// </summary>
        [STAThread]
        new public static int Main(string[] args)
        {            
            CommandOptionsArxNet options = new CommandOptionsArxNet(args);

            // Create SettingsService early so we know the trace level right at the start
            SettingsService settingsService = new SettingsService();
            InternalTraceLevel level = (InternalTraceLevel)settingsService.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default);
            if (options.trace != InternalTraceLevel.Default)
                level = options.trace;

            InternalTrace.Initialize("nunit-command-arxnet_%p.log", level);

            log.Info("NUnit-command-arxnet.dll starting");

            if (!options.nologo)
                WriteCopyright();

            if (options.help)
            {
                options.Help();
                return CommandUiArxNet.OK;
            }

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            if (options.cleanup)
            {
                log.Info("Performing cleanup of shadow copy cache");
                DomainManager.DeleteShadowCopyPath();
                ed.WriteMessage("\nShadow copy cache emptied");
                return CommandUiArxNet.OK;
            }

            if (options.NoArgs)
            {
                ed.WriteMessage("\nfatal error: no inputs specified");
                options.Help();
                return CommandUiArxNet.OK;
            }

            if (!options.Validate())
            {
                foreach (string arg in options.InvalidArguments)
                    ed.WriteMessage("\nfatal error: invalid argument: {0}", arg);
                options.Help();
                return CommandUiArxNet.INVALID_ARG;
            }

            // Add Standard Services to ServiceManager
            ServiceManager.Services.AddService(new SettingsService());
            ServiceManager.Services.AddService(new DomainManager());
            //ServiceManager.Services.AddService( new RecentFilesService() );
            ServiceManager.Services.AddService(new ProjectService());
            //ServiceManager.Services.AddService( new TestLoader() );
            ServiceManager.Services.AddService(new AddinRegistry());
            ServiceManager.Services.AddService(new AddinManager());
            // Hack: Resolves conflict with gui testagency when running
            // console tests under the gui.
            if (!AppDomain.CurrentDomain.FriendlyName.StartsWith("test-domain-"))
                ServiceManager.Services.AddService(new TestAgency());

            // Initialize Services
            ServiceManager.Services.InitializeServices();

            foreach (string parm in options.Parameters)
            {
                if (!Services.ProjectService.CanLoadProject(parm) && !PathUtils.IsAssemblyFileType(parm))
                {
                    ed.WriteMessage("\nFile type not known: {0}", parm);
                    return CommandUiArxNet.INVALID_ARG;
                }
            }

            try
            {
                CommandUiArxNet commandUiArxNet = new CommandUiArxNet();
                return commandUiArxNet.Execute(options);
            }
            catch (FileNotFoundException ex)
            {
                ed.WriteMessage("\n" + ex.Message);
                return CommandUiArxNet.FILE_NOT_FOUND;
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nAutoCAD Exception:\n{0}", ex.ToString());
                return CommandUiArxNet.UNEXPECTED_ERROR;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nUnhandled Exception:\n{0}", ex.ToString());
                return CommandUiArxNet.UNEXPECTED_ERROR;
            }
            finally
            {
                if (options.wait)
                {
                    ed.GetString(new PromptStringOptions("\n\nHit <enter> key to continue"));
                }

                log.Info("NUnit-command-arxnet.dll terminating");
            }
        }

        private static void WriteCopyright()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string versionText = executingAssembly.GetName().Version.ToString();

            string productName = "NUnit-Command-ArxNet";
            string copyrightText = "Copyright (C) 2012 Lei Qun.\r\nCopyright (C) 2002-2011 Charlie Poole.\r\nCopyright (C) 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov.\r\nCopyright (C) 2000-2002 Philip Craig.\r\nAll Rights Reserved.";

            //object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            //if (objectAttrs.Length > 0)
            //    productName = ((AssemblyProductAttribute)objectAttrs[0]).Product;

            object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (objectAttrs.Length > 0)
                copyrightText = ((AssemblyCopyrightAttribute)objectAttrs[0]).Copyright;

            objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
            if (objectAttrs.Length > 0)
            {
                string configText = ((AssemblyConfigurationAttribute)objectAttrs[0]).Configuration;
                if (configText != "")
                    versionText += string.Format(" ({0})", configText);
            }
            
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage(String.Format("\n{0} version {1}", productName, versionText));
            ed.WriteMessage("\n" + copyrightText);
            ed.WriteMessage("\n");

            ed.WriteMessage("\nRuntime Environment - ");
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;
            ed.WriteMessage(string.Format("\n   OS Version: {0}", Environment.OSVersion));
            ed.WriteMessage(string.Format("\n  CLR Version: {0} ( {1} )",
                Environment.Version, framework.DisplayName));
            ed.WriteMessage(string.Format("\n ACAD Version: {0} ",Application.Version));//AutoCAD版本

            ed.WriteMessage("\n");


        }
    }
}
