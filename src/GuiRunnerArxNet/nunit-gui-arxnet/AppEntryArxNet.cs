// ****************************************************************
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
// 2012.12.24修改
//  1.利用反射将ServiceManagerArxNet、ServicesArxNet类复位
//  2.public int NUnit.Gui.ArxNet.AppEntryArxNet.Main (string[] args)去掉static
//  3.增加：static public bool nunitRunned = false;//nunit测试命令是否已运行过了
// 2012.12.23修改
//  1.NUnit.Gui.ArxNet.AppEntryArxNet改为调用NUnit.Util.ArxNet.SettingsServiceArxNet
// 2013.1.23修改
//  1.NUnit.Gui.ArxNet.AppEntryArxNet改为调用NUnit.Gui.ArxNet.GuiOptionsArxNet
// 2013.5.27：
//  1.DomainManager改为DomainManagerArxNet
//  2.ServiceManager改为ServiceManagerArxNet
//  3.Services改为ServicesArxNet
//  4.使用EditorWritor类在Editor输出
//  5.增加Init、CleanUp方法
// ****************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

using NUnit.UiKit;
using NUnit.Util;
using NUnit.Core;
using NUnit.Core.Extensibility;

using NUnit.Util.ArxNet;
using NUnit.UiKit.ArxNet;
using NUnit.Core.ArxNet;

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
    public class AppEntryArxNet
    {
        static internal Logger log = InternalTrace.GetLogger(typeof(AppEntryArxNet));
        private static EditorWriter m_EditorWriter = null;
        private static TextWriter m_SavedOut = null;
        private static TextWriter m_SavedError = null;

        //static public bool nunitRunned = false;//nunit测试命令是否已运行过了

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            /*if (nunitRunned)
            {
                Type type;
                FieldInfo field;
                //利用反射将含静态成员的类复位
                //ServiceManagerArxNet类复位
                type = typeof(ServiceManagerArxNet);
                field = type.GetField("services", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(ServiceManagerArxNet.Services, new ArrayList());
                field = type.GetField("serviceIndex", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(ServiceManagerArxNet.Services, new Hashtable());
                //ServicesArxNet类复位
                type = typeof(ServicesArxNet);
                field = type.GetField("addinManager", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("addinRegistry", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("agency", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("domainManager", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("loader", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("projectService", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("recentFiles", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);
                field = type.GetField("userSettings", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.SetField);
                field.SetValue(null, null);                
            }*/

            // Create SettingsService early so we know the trace level right at the start
            SettingsServiceArxNet settingsService = new SettingsServiceArxNet();
            InternalTrace.Initialize("nunit-gui_%p.log", (InternalTraceLevel)settingsService.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default));

            log.Info("Starting NUnit GUI");

            GuiOptionsArxNet guiOptions = new GuiOptionsArxNet(args);

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
                DomainManagerArxNet.DeleteShadowCopyPath();
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
                // Add Standard Services to ServiceManagerArxNet
                log.Info("Adding Services");
                ServiceManagerArxNet.Services.AddService(settingsService);
                ServiceManagerArxNet.Services.AddService(new DomainManagerArxNet());
                ServiceManagerArxNet.Services.AddService(new RecentFilesService());
                ServiceManagerArxNet.Services.AddService(new ProjectService());
                ServiceManagerArxNet.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcherArxNet()));
                ServiceManagerArxNet.Services.AddService(new AddinRegistry());
                ServiceManagerArxNet.Services.AddService(new AddinManager());
                ServiceManagerArxNet.Services.AddService(new TestAgency());

                // Initialize Services
                log.Info("Initializing Services");
                ServiceManagerArxNet.Services.InitializeServices();
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
                ServiceManagerArxNet.Services.StopAllServices();
                ServiceManagerArxNet.Services.ClearServices();
                //2012.12.23改

                throw;
            }
            /*finally
            {
                log.Info("Stopping Services");
                ServiceManagerArxNet.Services.StopAllServices();
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

        /// <summary>
        /// 初始化类
        /// </summary>
        public static void Init()//2013.5.27lq加
        {
            //throw new System.NotImplementedException();
            log = InternalTrace.GetLogger(typeof(AppEntryArxNet));

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
        public static void CleanUp()//2013.5.27lq加
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
