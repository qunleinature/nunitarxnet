﻿// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2012.8.24修改
// 2012.12.23修改:
//  1.改GuiTestEventDispatcher为GuiTestEventDispatcherArxNet  
//  2.改主窗体为无模式对话框
// ****************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using NUnit.UiKit;
using NUnit.Util;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Util.ArxNet;
using NUnit.UiKit.ArxNet;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using FormsApplication = System.Windows.Forms.Application;
using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;
using SystemException = System.Exception;
using CADException = Autodesk.AutoCAD.Runtime.Exception;

namespace NUnit.Gui.ArxNet
{
    /// <summary>
    /// Class to manage application startup.
    /// </summary>
    public class AppEntryArxNet : AppEntry
    {
        static internal Logger log = InternalTrace.GetLogger(typeof(AppEntryArxNet));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        new public static int Main(string[] args)
        {
            // Create SettingsService early so we know the trace level right at the start
            SettingsService settingsService = new SettingsService();
            InternalTrace.Initialize("nunit-gui_%p.log", (InternalTraceLevel)settingsService.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default));

            log.Info("Starting NUnit GUI");

            GuiOptions guiOptions = new GuiOptions(args);

            GuiAttachedConsole attachedConsole = null;
            if (guiOptions.console)
            {
                log.Info("Creating attached console");
                attachedConsole = new GuiAttachedConsole();
            }

            if (guiOptions.help)
            {
                MessageDisplay.Display(guiOptions.GetHelpText());
                return 0;
            }

            if (!guiOptions.Validate())
            {
                string message = "Error in command line";
                MessageDisplay.Error(message + Environment.NewLine + Environment.NewLine + guiOptions.GetHelpText());
                log.Error(message);
                return 2;
            }

            if (guiOptions.cleanup)
            {
                log.Info("Performing cleanup of shadow copy cache");
                DomainManager.DeleteShadowCopyPath();
                return 0;
            }

            if (!guiOptions.NoArgs)
            {
                if (guiOptions.lang != null)
                {
                    log.Info("Setting culture to " + guiOptions.lang);
                    Thread.CurrentThread.CurrentUICulture =
                        new CultureInfo(guiOptions.lang);
                }
            }

            try
            {
                // Add Standard Services to ServiceManager
                log.Info("Adding Services");
                ServiceManager.Services.AddService(settingsService);
                ServiceManager.Services.AddService(new DomainManager());
                ServiceManager.Services.AddService(new RecentFilesService());
                ServiceManager.Services.AddService(new ProjectService());
                ServiceManager.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcherArxNet()));
                ServiceManager.Services.AddService(new AddinRegistry());
                ServiceManager.Services.AddService(new AddinManager());
                ServiceManager.Services.AddService(new TestAgency());

                // Initialize Services
                log.Info("Initializing Services");
                ServiceManager.Services.InitializeServices();
            }
            catch (SystemException ex)
            {
                MessageDisplay.FatalError("Service initialization failed.", ex);
                log.Error("Service initialization failed", ex);
                return 2;
            }

            // Create container in order to allow ambient properties
            // to be shared across all top-level forms.
            log.Info("Initializing AmbientProperties");
            AppContainer c = new AppContainer();
            AmbientProperties ambient = new AmbientProperties();
            c.Services.AddService(typeof(AmbientProperties), ambient);

            log.Info("Constructing Form");
            NUnitFormArxNet form = new NUnitFormArxNet(guiOptions);
            c.Add(form);

            try
            {
                log.Info("Starting Gui Application");
                //FormsApplication.Run(form);
                Document doc = CADApplication.DocumentManager.MdiActiveDocument;
                CADApplication.ShowModelessDialog(doc.Window.Handle, form);                
                //CADApplication.ShowModalDialog(form);
                //CADApplication.ShowModelessDialog(form);
                //log.Info("Application Exit");
            }
            catch (SystemException ex)
            {
                log.Error("Gui Application threw an excepion", ex);

                //2012.12.23改
                log.Info("Stopping Services");
                ServiceManager.Services.StopAllServices();
                //2012.12.23改

                throw;
            }
            /*finally
            {
                log.Info("Stopping Services");
                ServiceManager.Services.StopAllServices();
            }

            if (attachedConsole != null)
            {
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
                attachedConsole.Close();
            }

            log.Info("Exiting NUnit GUI");
            InternalTrace.Close();*/

            return 0;
        }

        private static IMessageDisplay MessageDisplay
        {
            get { return new MessageDisplay("NUnit"); }
        }
    }
}
