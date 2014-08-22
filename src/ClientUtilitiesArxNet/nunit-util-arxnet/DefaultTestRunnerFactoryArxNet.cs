// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.27修改：
//  1.selector改为RuntimeFrameworkSelectorArxNet
// 2013.5.29修改：
//  1.改ProcessRunner为ProcessRunnerArxNet
//  2.CAD环境下测试包是单进程、无应用域
// 2013.7.2
//  1.已改NUnit2.6.2基础
// 2013.7.19
//  1.改InProcessTestRunnerFactoryArxNet
// 2013.7.29
//  1.已改MultipleTestProcessRunnerArxNet
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.8.22：
//  在NUnit2.6.3基础上修改
// ****************************************************************

using System;
using NUnit.Core;

namespace NUnit.Util.ArxNet
{
    /// <summary>
    /// DefaultTestRunnerFactory handles creation of a suitable test 
    /// runner for a given package to be loaded and run either in a 
    /// separate process or within the same process. 
    /// </summary>
    public class DefaultTestRunnerFactoryArxNet : InProcessTestRunnerFactoryArxNet, ITestRunnerFactory
    {
#if CLR_2_0 || CLR_4_0
        private RuntimeFrameworkSelectorArxNet selector = new RuntimeFrameworkSelectorArxNet();//RuntimeFrameworkSelectorArxNet    
        
        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>A TestRunner</returns>
        public override TestRunner MakeTestRunner(TestPackage package)
        {
            package.Settings["ProcessModel"] = ProcessModel.Single;//2013.5.29lq改，单进程
            package.Settings["DomainUsage"] = DomainUsage.None;//2013.5.29lq改，无应用域
            
            ProcessModel processModel = GetTargetProcessModel(package);

            switch (processModel)
            {
                case ProcessModel.Multiple:
                    package.Settings.Remove("ProcessModel");
                    return new MultipleTestProcessRunnerArxNet();
                case ProcessModel.Separate:
                    package.Settings.Remove("ProcessModel");
                    return new ProcessRunnerArxNet();
                default:
                    return base.MakeTestRunner(package);
            }
        }

        public override bool CanReuse(TestRunner runner, TestPackage package)
        {
            package.Settings["ProcessModel"] = ProcessModel.Single;//2013.5.29lq改，单进程
            package.Settings["DomainUsage"] = DomainUsage.None;//2013.5.29lq改，无应用域

            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework targetFramework = selector.SelectRuntimeFramework(package);

            ProcessModel processModel = (ProcessModel)package.GetSetting("ProcessModel", ProcessModel.Default);
            if (processModel == ProcessModel.Default)
                if (!currentFramework.Supports(targetFramework))
                    processModel = ProcessModel.Separate;

            switch (processModel)
            {
                case ProcessModel.Multiple:
                    return runner is MultipleTestProcessRunnerArxNet;
                case ProcessModel.Separate:
                    ProcessRunnerArxNet processRunner = runner as ProcessRunnerArxNet;
                    return processRunner != null && processRunner.RuntimeFramework == targetFramework;
                default:
                    return base.CanReuse(runner, package);
            }
        }

        private ProcessModel GetTargetProcessModel(TestPackage package)
        {                        
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework targetFramework = selector.SelectRuntimeFramework(package);
            
            ProcessModel processModel = (ProcessModel)package.GetSetting("ProcessModel", ProcessModel.Default);
            if (processModel == ProcessModel.Default)
                if (!currentFramework.Supports(targetFramework))
                    processModel = ProcessModel.Separate;
            return processModel;
        }
#endif
    }
}
