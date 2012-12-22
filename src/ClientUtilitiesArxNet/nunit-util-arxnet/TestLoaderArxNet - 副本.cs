// ****************************************************************
// Copyright 2002-2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2012.12.19修改
// ****************************************************************

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using System.Reflection;

using NUnit.Core;
using NUnit.Core.Filters;
using NUnit.Util;

using Com.Utility.UnitTest;

namespace NUnit.Util.ArxNet
{
    public class TestLoaderArxNet : TestLoader, NUnit.Core.EventListener, ITestLoader, IService
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestLoaderArxNet));

        #region Constructors

		public TestLoaderArxNet()
			: base() { }

		public TestLoaderArxNet(TestEventDispatcher eventDispatcher)
			: base(eventDispatcher) { }

		public TestLoaderArxNet(IAssemblyWatcher assemblyWatcher)
			: base(assemblyWatcher) { }

        public TestLoaderArxNet(TestEventDispatcher eventDispatcher, IAssemblyWatcher assemblyWatcher)
            : base(eventDispatcher, assemblyWatcher) { }

        /*public TestLoaderArxNet(TestLoader loader)//复制构造函数
        {
            this.events = loader.
            this.watcher = assemblyWatcher;
            this.factory = new DefaultTestRunnerFactory();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
        }*/

        /*
        public TestLoader(TestEventDispatcher eventDispatcher, IAssemblyWatcher assemblyWatcher)
		{
			this.events = eventDispatcher;
			this.watcher = assemblyWatcher;
			this.factory = new DefaultTestRunnerFactory();
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
		}
        */

        #endregion

        #region Methods for Loading and Unloading Projects

        /// <summary>
        /// Create a new project with default naming
        /// </summary>
        //调用重写方法： private void OnProjectLoad(NUnitProject testProject)
        new public void NewProject()
        {
            log.Info("Creating empty project");

            object value;
            TestEventDispatcher events;
            Exception lastException;
            try
            {
                //private TestEventDispatcher events;
                //events.FireProjectLoading("New Project");
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoading("New Project");                

                OnProjectLoad(ServicesArxNet.ProjectService.NewProject());
            }
            catch (Exception exception)
            {
                log.Error("Project creation failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireProjectLoadFailed("New Project", exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoadFailed("New Project", exception);                
            }
        }

        /// <summary>
        /// Create a new project using a given path
        /// </summary>
        ////调用重写方法： private void OnProjectLoad(NUnitProject testProject)
        new public void NewProject(string filePath)
        {
            log.Info("Creating project " + filePath);

            object value;
            TestEventDispatcher events;
            Exception lastException;
            try
            {
                //private TestEventDispatcher events;
                //events.FireProjectLoading(filePath);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoading(filePath);                

                NUnitProject project = new NUnitProject(filePath);

                project.Configs.Add("Debug");
                project.Configs.Add("Release");
                project.IsDirty = false;

                OnProjectLoad(project);
            }
            catch (Exception exception)
            {
                log.Error("Project creation failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireProjectLoadFailed(filePath, exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoadFailed(filePath, exception);                
            }
        }

        /// <summary>
        /// Load a new project, optionally selecting the config and fire events
        /// </summary>
        //调用重写方法： private void OnProjectLoad(NUnitProject testProject) 
        new public void LoadProject(string filePath, string configName)
        {
            object value;
            TestEventDispatcher events;
            Exception lastException;
            try
            {
                log.Info("Loading project {0}, {1} config", filePath, configName == null ? "default" : configName);

                //private TestEventDispatcher events;
                //events.FireProjectLoading(filePath);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoading(filePath);                

                NUnitProject newProject = ServicesArxNet.ProjectService.LoadProject(filePath);
                if (configName != null)
                {
                    newProject.SetActiveConfig(configName);
                    newProject.IsDirty = false;
                }

                OnProjectLoad(newProject);
            }
            catch (Exception exception)
            {
                log.Error("Project load failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireProjectLoadFailed(filePath, exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoadFailed(filePath, exception);                
            }
        }

        /// <summary>
        /// Load a new project using the default config and fire events
        /// </summary>
        new public void LoadProject(string filePath)
        {
            LoadProject(filePath, null);
        }

        /// <summary>
        /// Load a project from a list of assemblies and fire events
        /// </summary>
        //调用重写方法： private void OnProjectLoad(NUnitProject testProject)
        new public void LoadProject(string[] assemblies)
        {
            object value;
            TestEventDispatcher events;
            Exception lastException;
            try
            {
                log.Info("Loading multiple assemblies as new project");

                //private TestEventDispatcher events;
                //events.FireProjectLoading("New Project");
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoading("New Project");                

                NUnitProject newProject = ServicesArxNet.ProjectService.WrapAssemblies(assemblies);

                OnProjectLoad(newProject);
            }
            catch (Exception exception)
            {
                log.Error("Project load failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireProjectLoadFailed("New Project", exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectLoadFailed("New Project", exception);
            }
        }

        /// <summary>
        /// Unload the current project and fire events
        /// </summary>
        //调用new public void UnloadTest()
        new public void UnloadProject()
        {
            string testFileName = TestFileName;

            log.Info("Unloading project " + testFileName);

            object value;
            TestEventDispatcher events;
            NUnitProject testProject;
            Exception lastException;
            try
            {
                //private TestEventDispatcher events;
                //events.FireProjectUnloading(testFileName);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectUnloading(testFileName);

                if (IsTestLoaded)
                    UnloadTest();//调用新方法

                //private NUnitProject testProject = null;
                //testProject = null;
                testProject = null;
                SetBaseNoPublicField("testProject", testProject);

                //private TestEventDispatcher events;
                //events.FireProjectUnloaded(testFileName);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectUnloaded(testFileName);
            }
            catch (Exception exception)
            {
                log.Error("Project unload failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireProjectUnloadFailed(testFileName, exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireProjectUnloadFailed(testFileName, exception);
            }

        }

        /// <summary>
        /// Common operations done each time a project is loaded
        /// </summary>
        /// <param name="testProject">The newly loaded project</param>
        //调用new public void UnloadProject()
        new private void OnProjectLoad(NUnitProject testProject)
        {
            if (IsProjectLoaded)
                UnloadProject();

            object value;
            TestEventDispatcher events;
            //private NUnitProject testProject = null;
            //this.testProject = testProject;            
            SetBaseNoPublicField("testProject", testProject);

            //private TestEventDispatcher events;
            //events.FireProjectLoaded(TestFileName);
            events = null;
            value = GetBaseNoPublicField("events");
            if (value != null)  events = (TestEventDispatcher)value;
            if (events != null) events.FireProjectLoaded(TestFileName);
        }

        #endregion

        #region Methods for Loading and Unloading Tests

        new public void LoadTest()
        {
            LoadTest(null);
        }

        //在CAD环境下加载的测试须是单线程、异步
        new public void LoadTest(string testName)
        {
            log.Info("Loading tests for " + Path.GetFileName(TestFileName));

            long startTime = DateTime.Now.Ticks;

            object value;
            TestEventDispatcher events;
            TestRunner testRunner;
            ITestRunnerFactory factory;
            ITest loadedTest;
            string loadedTestName;
            TestResult testResult;
            bool reloadPending;
            RuntimeFramework currentFramework;
            NUnitProject testProject;
            Exception lastException;

            try
            {
                //private TestEventDispatcher events;
                //events.FireTestLoading(TestFileName);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireTestLoading(TestFileName);

                TestPackage package = MakeTestPackage(testName);

                //private TestRunner testRunner = null;
                //if (testRunner != null)
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null) testRunner = (TestRunner)value;               
                if (testRunner != null)
                    testRunner.Dispose();

                //private ITestRunnerFactory factory;
                //testRunner = factory.MakeTestRunner(package);
                factory = null;
                value = GetBaseNoPublicField("factory");
                if (value != null) factory = (ITestRunnerFactory)value;
                //testRunner = factory.MakeTestRunner(package);                
                if (factory != null)
                {
                    testRunner = factory.MakeTestRunner(package);
                    SetBaseNoPublicField("testRunner", testRunner);
                }

                //bool loaded = testRunner.Load(package);
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null) testRunner = (TestRunner)value;
                bool loaded = false;
                if (testRunner != null) loaded = testRunner.Load(package);

                //private ITest loadedTest = null;
                //loadedTest = testRunner.Test;
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null) testRunner = (TestRunner)value;
                loadedTest = null;
                if (testRunner != null)
                {
                    loadedTest = testRunner.Test;
                    SetBaseNoPublicField("loadedTest", loadedTest);
                }

                //private string loadedTestName = null;
                //loadedTestName = testName;
                loadedTestName = testName;
                SetBaseNoPublicField("loadedTestName", loadedTestName);

                //private TestResult testResult = null;
                //testResult = null;
                testResult = null;
                SetBaseNoPublicField("testResult", testResult);

                //private bool reloadPending = false;
                //reloadPending = false;
                reloadPending = false;
                SetBaseNoPublicField("reloadPending", reloadPending);                

                if (ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.ReloadOnChange", true))
                {
                    InstallWatcher();//调用新方法
                }
                
                if (loaded)
                {
                    //private RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
                    /*this.currentFramework = package.Settings.Contains("RuntimeFramework")
                        ? package.Settings["RuntimeFramework"] as RuntimeFramework
                        : RuntimeFramework.CurrentFramework;*/
                    currentFramework = package.Settings.Contains("RuntimeFramework")
                        ? package.Settings["RuntimeFramework"] as RuntimeFramework
                        : RuntimeFramework.CurrentFramework;
                    SetBaseNoPublicField("currentFramework", currentFramework);

                    //private NUnitProject testProject = null;
                    //testProject.HasChangesRequiringReload = false;
                    testProject = null;
                    value = GetBaseNoPublicField("testProject");
                    if (value != null) testProject = (NUnitProject)value;
                    if (testProject != null) testProject.HasChangesRequiringReload = false;

                    //private TestEventDispatcher events;
                    //events.FireTestLoaded(TestFileName, loadedTest);
                    events = null;
                    value = GetBaseNoPublicField("events");
                    if (value != null)  events = (TestEventDispatcher)value;
                    if (events != null) events.FireTestLoaded(TestFileName, loadedTest);
                }
                else
                {
                    //private Exception lastException = null;
                    //lastException = new ApplicationException(string.Format("Unable to find test {0} in assembly", testName));
                    lastException = null;
                    lastException = new ApplicationException(string.Format("Unable to find test {0} in assembly", testName));
                    SetBaseNoPublicField("lastException", lastException);

                    //private TestEventDispatcher events;
                    //events.FireTestLoadFailed(TestFileName, lastException);
                    events = null;
                    value = GetBaseNoPublicField("events");
                    if (value != null)  events = (TestEventDispatcher)value;
                    if (events != null) events.FireTestLoadFailed(TestFileName, lastException);
                }
            }
            catch (FileNotFoundException exception)
            {
                log.Error("File not found", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                foreach (string assembly in TestProject.ActiveConfig.Assemblies)
                {
                    //private NUnitProject testProject = null;
                    /*if (Path.GetFileNameWithoutExtension(assembly) == exception.FileName &&
                        !PathUtils.SamePathOrUnder(testProject.ActiveConfig.BasePath, assembly))
                    {
                        lastException = new ApplicationException(string.Format("Unable to load {0} because it is not located under the AppBase", exception.FileName), exception);
                        break;
                    }*/
                    testProject = null;
                    value = GetBaseNoPublicField("testProject");
                    if (value != null) testProject = (NUnitProject)value;
                    if (testProject != null)
                    {
                        if (Path.GetFileNameWithoutExtension(assembly) == exception.FileName &&
                            !PathUtils.SamePathOrUnder(testProject.ActiveConfig.BasePath, assembly))
                        {
                            //private Exception lastException = null;
                            //lastException = new ApplicationException(string.Format("Unable to load {0} because it is not located under the AppBase", exception.FileName), exception);
                            lastException = new ApplicationException(string.Format("Unable to load {0} because it is not located under the AppBase", exception.FileName), exception);
                            SetBaseNoPublicField("lastException", lastException);
                            break;
                        }
                    }
                }

                //private TestEventDispatcher events;
                //events.FireTestLoadFailed(TestFileName, lastException);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireTestLoadFailed(TestFileName, lastException);

                double loadTime = (double)(DateTime.Now.Ticks - startTime) / (double)TimeSpan.TicksPerSecond;
                log.Info("Load completed in {0} seconds", loadTime);
            }
            catch (Exception exception)
            {
                log.Error("Failed to load test", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireTestLoadFailed(TestFileName, exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireTestLoadFailed(TestFileName, exception);
            }
        }

        /// <summary>
        /// Unload the current test suite and fire the Unloaded event
        /// </summary>
        //调用重写方法：private void RemoveWatcher ()
        new public void UnloadTest()
        {
            if (IsTestLoaded)
            {
                log.Info("Unloading tests for " + Path.GetFileName(TestFileName));

                // Hold the name for notifications after unload
                string fileName = TestFileName;

                object value;
                TestEventDispatcher events;
                TestRunner testRunner;
                ITest loadedTest;
                string loadedTestName;
                TestResult testResult;
                bool reloadPending;
                Exception lastException;
                try
                {
                    //private TestEventDispatcher events;
                    //events.FireTestUnloading(fileName);
                    events = null;
                    value = GetBaseNoPublicField("events");
                    if (value != null)  events = (TestEventDispatcher)value;
                    if (events != null) events.FireTestUnloading(fileName);

                    RemoveWatcher();

                    //private TestRunner testRunner = null;
                    /*
                     testRunner.Unload();
                    testRunner.Dispose();
                    testRunner = null;
                     */
                    testRunner = null;
                    value = GetBaseNoPublicField("testRunner");
                    if (value != null) testRunner = (TestRunner)value;
                    if (testRunner != null)
                    {
                        testRunner.Unload();
                        testRunner.Dispose();
                        testRunner = null;
                        SetBaseNoPublicField("testRunner", testRunner);
                    }

                    //private ITest loadedTest = null;
                    //loadedTest = null;
                    loadedTest = null;
                    SetBaseNoPublicField("loadedTest", loadedTest);

                    //private string loadedTestName = null;
                    //loadedTestName = null;
                    loadedTestName = null;
                    SetBaseNoPublicField("loadedTestName", loadedTestName);

                    //private TestResult testResult = null;
                    //testResult = null;
                    testResult = null;
                    SetBaseNoPublicField("testResult", testResult);

                    //private bool reloadPending = false;
                    //reloadPending = false;
                    reloadPending = false;
                    SetBaseNoPublicField("reloadPending", reloadPending);

                    //private TestEventDispatcher events;
                    //events.FireTestUnloaded(fileName);
                    events = null;
                    value = GetBaseNoPublicField("events");
                    if (value != null)  events = (TestEventDispatcher)value;
                    if (events != null) events.FireTestUnloaded(fileName);
                    
                    log.Info("Unload complete");
                }
                catch (Exception exception)
                {
                    log.Error("Failed to unload tests", exception);

                    //private Exception lastException = null;
                    //lastException = exception;
                    lastException = exception;
                    SetBaseNoPublicField("lastException", lastException);

                    //private TestEventDispatcher events;
                    //events.FireTestUnloadFailed(fileName, exception);
                    events = null;
                    value = GetBaseNoPublicField("events");
                    if (value != null)  events = (TestEventDispatcher)value;
                    if (events != null) events.FireTestUnloadFailed(fileName, exception);
                }
            }
        }

        /// <summary>
        /// Return true if the current project can be reloaded under
        /// the specified CLR version.
        /// </summary>
        /*public bool CanReloadUnderRuntimeVersion(Version version)
        {
            if (!ServicesArxNet.TestAgency.IsRuntimeVersionSupported(version))
                return false;

            foreach (TestAssemblyInfo info in AssemblyInfo)
                if (info.ImageRuntimeVersion > version)
                    return false;

            return true;
        }*/

        /// <summary>
        /// Reload the current test on command
        /// </summary>
        //在CAD环境下重载的测试须是单线程、异步
        new public void ReloadTest(RuntimeFramework framework)
        {
            log.Info("Reloading tests for " + Path.GetFileName(TestFileName));

            object value;
            TestEventDispatcher events;
            string loadedTestName;
            TestRunner testRunner;
            ITestRunnerFactory factory;
            RuntimeFramework currentFramework;
            ITest loadedTest;
            bool reloadPending;
            NUnitProject testProject;
            Exception lastException;

            try
            {
                //private TestEventDispatcher events;
                //events.FireTestReloading(TestFileName);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null) events = (TestEventDispatcher)value;
                if (events != null) events.FireTestReloading(TestFileName);

                //private string loadedTestName = null;
                //TestPackage package = MakeTestPackage(loadedTestName);
                loadedTestName = null;
                value = GetBaseNoPublicField("loadedTestName");
                if (value != null) loadedTestName = (string)value;
                TestPackage package = MakeTestPackage(loadedTestName);

                if (framework != null)
                    package.Settings["RuntimeFramework"] = framework;

                RemoveWatcher();//调用新方法

                //private TestRunner testRunner = null;
                //testRunner.Unload();
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null)      testRunner = (TestRunner)value;
                if (testRunner != null) testRunner.Unload();

                //private ITestRunnerFactory factory;
                //if (!factory.CanReuse(testRunner, package))
                factory = null;
                value = GetBaseNoPublicField("factory");
                if (value != null) factory = (ITestRunnerFactory)value;
                if (factory != null)
                {
                    if (!factory.CanReuse(testRunner, package))
                    {
                        testRunner.Dispose();

                        //private TestRunner testRunner = null;
                        //testRunner = factory.MakeTestRunner(package);
                        testRunner = factory.MakeTestRunner(package);
                        SetBaseNoPublicField("testRunner", testRunner);
                    }
                }

                //private TestRunner testRunner = null;
                //if (testRunner.Load(package))
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null) testRunner = (TestRunner)value;
                if (testRunner != null)
                {
                    if (testRunner.Load(package))
                    {
                        //private RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
                        /*this.currentFramework = package.Settings.Contains("RuntimeFramework")
                            ? package.Settings["RuntimeFramework"] as RuntimeFramework
                            : RuntimeFramework.CurrentFramework;*/
                        currentFramework = package.Settings.Contains("RuntimeFramework")
                            ? package.Settings["RuntimeFramework"] as RuntimeFramework
                            : RuntimeFramework.CurrentFramework;
                        SetBaseNoPublicField("currentFramework", currentFramework);
                    }
                }

                //private ITest loadedTest = null;
                //loadedTest = testRunner.Test;
                testRunner = null;
                value = GetBaseNoPublicField("testRunner");
                if (value != null) testRunner = (TestRunner)value;                
                if (testRunner != null)
                {
                    loadedTest = testRunner.Test;
                    SetBaseNoPublicField("loadedTest", loadedTest);
                }

                //private RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
                //currentRuntime = framework;
                currentFramework = framework;
                SetBaseNoPublicField("currentFramework", currentFramework);

                //private bool reloadPending = false;
                //reloadPending = false;
                reloadPending = false;
                SetBaseNoPublicField("reloadPending", reloadPending);
                
                if (ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.ReloadOnChange", true))
                {
                    InstallWatcher();//调用新方法
                }

                //private NUnitProject testProject = null;
                //testProject.HasChangesRequiringReload = false;
                testProject = null;
                value = GetBaseNoPublicField("testProject");
                if (value != null)          testProject = (NUnitProject)value;
                if (testProject != null)    testProject.HasChangesRequiringReload = false;


                //private ITest loadedTest = null;
                //events.FireTestReloaded(TestFileName, loadedTest);
                loadedTest = null;
                value = GetBaseNoPublicField("loadedTest");
                if (loadedTest != null) loadedTest = (ITest)value;
                //private TestEventDispatcher events;
                //events.FireTestReloaded(TestFileName, loadedTest);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null) events = (TestEventDispatcher)value;
                if (events != null) events.FireTestReloaded(TestFileName, loadedTest);

                log.Info("Reload complete");
            }
            catch (Exception exception)
            {
                log.Error("Reload failed", exception);

                //private Exception lastException = null;
                //lastException = exception;
                lastException = exception;
                SetBaseNoPublicField("lastException", lastException);

                //private TestEventDispatcher events;
                //events.FireTestReloadFailed(TestFileName, exception);
                events = null;
                value = GetBaseNoPublicField("events");
                if (value != null)  events = (TestEventDispatcher)value;
                if (events != null) events.FireTestReloadFailed(TestFileName, exception);
            }
        }

        new public void ReloadTest()
        {
            RuntimeFramework currentRuntime;
            object value;
            //private RuntimeFramework currentRuntime;
            //ReloadTest(currentRuntime);
            currentRuntime = null;
            value = GetBaseNoPublicField("currentRuntime");
            if (value != null) currentRuntime = (RuntimeFramework)value;

            ReloadTest(currentRuntime);
        }

        /// <summary>
        /// Handle watcher event that signals when the loaded assembly
        /// file has changed. Make sure it's a real change before
        /// firing the SuiteChangedEvent. Since this all happens
        /// asynchronously, we use an event to let ui components
        /// know that the failure happened.
        /// </summary>
        //在CAD环境下重载的测试须是单线程、异步 ，需调用新ReloadTest()，且异步运行
        new public void OnTestChanged(string testFileName)
        {
            log.Info("Assembly changed: {0}", testFileName);

            object value;
            bool reloadPending;
            ITestFilter lastFilter;
            TestRunner testRunner;
            
            if (Running)
            {
                //private bool reloadPending = false;
                /*if (Running)
                    reloadPending = true;*/
                reloadPending = true;
                SetBaseNoPublicField("reloadPending", reloadPending);
            }
            else
            {
                ReloadTest();//新方法，保证在CAD环境下重载的测试须是单线程、异步

                //private ITestFilter lastFilter;
                //if (lastFilter != null && ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.RerunOnChange", false))
                lastFilter = null;
                value = GetBaseNoPublicField("lastFilter");
                if (value != null) lastFilter = (ITestFilter)value;
                if (lastFilter != null && ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.RerunOnChange", false))
                {
                    //private TestRunner testRunner = null;
                    //testRunner.BeginRun(this, lastFilter);
                    testRunner = null;
                    value = GetBaseNoPublicField("TestRunner");
                    if (value != null) testRunner = (TestRunner)value;
                    if (testRunner != null) testRunner.Run(this, lastFilter);//异步运行测试
                }
            }
        }
        #endregion

        #region Methods for Running Tests
        /// <summary>
        /// Run all the tests
        /// </summary>
        new public void RunTests()
        {
            RunTests(TestFilter.Empty);
        }

        /// <summary>
        /// Run selected tests using a filter
        /// </summary>
        /// <param name="filter">The filter to be used</param>
        //在CAD环境下应单线程、异步运行测试
        new public void RunTests(ITestFilter filter)
        {
            //throw (new NotImplementedException());  
            bool reloadPending;
            ITestFilter lastFilter;
            TestRunner testRunner;
            
            if (!Running)
            {
                //private bool reloadPending = false;
                //if (reloadPending || ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.ReloadOnRun", false))
                reloadPending = false;
                object value;
                value = GetBaseNoPublicField("reloadPending");
                if (value != null) reloadPending = (bool)value;
                if (reloadPending || ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.ReloadOnRun", false))
                    ReloadTest();//新方法，保证在CAD环境下重载的测试须是单线程、异步

                //private ITestFilter lastFilter;
                //this.lastFilter = filter;
                lastFilter = filter;
                SetBaseNoPublicField("lastFilter", lastFilter);

                //private TestRunner testRunner = null;
                //testRunner.BeginRun(this, filter);
                testRunner = null;
                value = GetBaseNoPublicField("TestRunner");
                if (value != null) testRunner = (TestRunner)value;
                if (testRunner != null) testRunner.Run(this, filter);//异步运行测试
            }           
        }              

        /*
        /// <summary>
        /// Cancel the currently running test.
        /// Fail silently if there is none to
        /// allow for latency in the UI.
        /// </summary>
        public void CancelTestRun()
        {
            if (Running)
                testRunner.CancelRun();
        }

        public IList GetCategories()
        {
            CategoryManager categoryManager = new CategoryManager();
            categoryManager.AddAllCategories(this.loadedTest);
            ArrayList list = new ArrayList(categoryManager.Categories);
            list.Sort();
            return list;
        }

        public void SaveLastResult(string fileName)
        {
            new XmlResultWriter(fileName).SaveTestResult(this.testResult);
        }
        */
        #endregion

        #region Helper Methods

        /// <summary>
        /// Install our watcher object so as to get notifications
        /// about changes to a test.
        /// </summary>
        //调用new public void OnTestChanged(string testFileName) 
        private void InstallWatcher()
        {
            object value;
            IAssemblyWatcher watcher;
            //private IAssemblyWatcher watcher;
            /*
            if (watcher != null)
            {
                watcher.Stop();
                watcher.FreeResources();

                watcher.Setup(1000, TestProject.ActiveConfig.Assemblies.ToArray());
                watcher.AssemblyChanged += new AssemblyChangedHandler(OnTestChanged);
                watcher.Start();
            }
            */
            watcher = null;
            value = GetBaseNoPublicField("watcher");
            if (value != null) watcher = (IAssemblyWatcher)value;
            if (watcher != null)
            {
                watcher.Stop();
                watcher.FreeResources();

                watcher.Setup(1000, TestProject.ActiveConfig.Assemblies.ToArray());
                watcher.AssemblyChanged += new AssemblyChangedHandler(OnTestChanged);
                watcher.Start();
            }
        }

        /// <summary>
        /// Stop and remove our current watcher object.
        /// </summary>
        //调用new public void OnTestChanged(string testFileName) 
        private void RemoveWatcher()
        {
            object value;
            IAssemblyWatcher watcher;
            //private IAssemblyWatcher watcher;
            /*
             if (watcher != null)
            {
                watcher.Stop();
                watcher.FreeResources();
                watcher.AssemblyChanged -= new AssemblyChangedHandler(OnTestChanged);
            }
             */
            watcher = null;
            value = GetBaseNoPublicField("watcher");
            if (value != null) watcher = (IAssemblyWatcher)value;
            if (watcher != null)
            {
                watcher.Stop();
                watcher.FreeResources();
                watcher.AssemblyChanged -= new AssemblyChangedHandler(OnTestChanged);
            }
        }

        //在CAD环境下的测试包是单线程、异步
        private TestPackage MakeTestPackage(string testName)
        {
            TestPackage package = TestProject.ActiveConfig.MakeTestPackage();
            package.TestName = testName;

            ISettings settings = ServicesArxNet.UserSettings;
            package.Settings["MergeAssemblies"] = settings.GetSetting("Options.TestLoader.MergeAssemblies", false);
            package.Settings["AutoNamespaceSuites"] = settings.GetSetting("Options.TestLoader.AutoNamespaceSuites", true);
            package.Settings["ShadowCopyFiles"] = settings.GetSetting("Options.TestLoader.ShadowCopyFiles", true);
            
            package.Settings["ProcessModel"] = ProcessModel.Single;//单线程

            package.Settings["DomainUsage"] = DomainUsage.None;//不新建应用域

            package.Settings["UseThreadedRunner"] = false;//不使用线程

            if (!package.Settings.Contains("WorkDirectory"))
                package.Settings["WorkDirectory"] = Environment.CurrentDirectory;

            return package;
        }

        //获得基类对象的私有字段的值
        private object GetBaseNoPublicField(string fieldName)
        {
            try
            {
                //利用反射访问基类对象的私有字段
                Type baseType = this.GetType().BaseType;
                FieldInfo field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
                object value = field.GetValue(this);
                return value;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        //设置基类对象的私有字段的值
        private void SetBaseNoPublicField(string fieldName, object value)
        {
            try
            {
                //利用反射访问基类对象的私有字段
                Type baseType = this.GetType().BaseType;
                FieldInfo field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.SetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
                if (field != null) field.SetValue(this, value);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        } 
 
        //运行基类对象的私有函数
        private object CallBaseNoPublicMethod(string methodName, object[] param)
        {
            try
            {                
                //利用反射访问基类对象的私有函数
                Type baseType = this.GetType().BaseType;
                MethodInfo method = baseType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                object result = method.Invoke(this, param);
                return result;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }            
        }
        #endregion
    }
}
