﻿// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
//  2013.5.27修改：
//      1.Services改为ServicesArxNet
//  2013.7.29
//      1.已改NUnit2.6.2基础
//  2013.8.1
//      1.已改IRuntimeFrameworkSelectorArxNet
//  2014.8.22：
//      在NUnit2.6.3基础上修改
//  2015.1.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Util.ArxNet
{
    public class RuntimeFrameworkSelectorArxNet : IRuntimeFrameworkSelectorArxNet
    {
        static Logger log = InternalTrace.GetLogger(typeof(RuntimeFrameworkSelectorArxNet));

        /// <summary>
        /// Selects a target runtime framework for a TestPackage based on
        /// the settings in the package and the assemblies themselves.
        /// The package RuntimeFramework setting may be updated as a 
        /// result and the selected runtime is returned.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        /// <returns>The selected RuntimeFramework</returns>
        public RuntimeFramework SelectRuntimeFramework(TestPackage package)
        {
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework requestedFramework = package.Settings["RuntimeFramework"] as RuntimeFramework;

            log.Debug("Current framework is {0}", currentFramework);
            if (requestedFramework == null)
                log.Debug("No specific framework requested");
            else
                log.Debug("Requested framework is {0}", requestedFramework);

            RuntimeType targetRuntime = requestedFramework == null
                ? RuntimeType.Any 
                : requestedFramework.Runtime;
            Version targetVersion = requestedFramework == null
                ? RuntimeFramework.DefaultVersion
                : requestedFramework.FrameworkVersion;

            if (targetRuntime == RuntimeType.Any)
                targetRuntime = currentFramework.Runtime;

            if (targetVersion == RuntimeFramework.DefaultVersion)
            {
                if (ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.RuntimeSelectionEnabled", true))
                    foreach (string assembly in package.Assemblies)
                    {
                        using (AssemblyReader reader = new AssemblyReader(assembly))
                        {
                            string vString = reader.ImageRuntimeVersion;
                            if (vString.Length > 1) // Make sure it's a valid dot net assembly
                            {
                                Version v = new Version(vString.Substring(1));
                                log.Debug("Assembly {0} uses version {1}", assembly, v);
                                if (v > targetVersion) targetVersion = v;
                            }
                        }
                    }
                else
                    targetVersion = RuntimeFramework.CurrentFramework.ClrVersion;

                RuntimeFramework checkFramework = new RuntimeFramework(targetRuntime, targetVersion);
                if (!checkFramework.IsAvailable || !ServicesArxNet.TestAgency.IsRuntimeVersionSupported(targetVersion))
                {
                    log.Debug("Preferred version {0} is not installed or this NUnit installation does not support it", targetVersion);
                    if (targetVersion < currentFramework.FrameworkVersion)
                        targetVersion = currentFramework.FrameworkVersion;
                }
            }

            RuntimeFramework targetFramework = new RuntimeFramework(targetRuntime, targetVersion);
            package.Settings["RuntimeFramework"] = targetFramework;

            log.Debug("Test will use {0} framework", targetFramework);

            return targetFramework;
        }
    }
}
