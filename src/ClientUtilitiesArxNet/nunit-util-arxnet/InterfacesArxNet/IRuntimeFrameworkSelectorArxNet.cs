// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.27修改：
//      1.IRuntimeFrameworkSelector改为IRuntimeFrameworkSelectorArxNet
//  2014.8.22：
//      在NUnit2.6.3基础上修改
//  2015.1.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using NUnit.Core;

namespace NUnit.Util.ArxNet
{
    interface IRuntimeFrameworkSelectorArxNet
    {
        /// <summary>
        /// Selects a target runtime framework for a TestPackage based on
        /// the settings in the package and the assemblies themselves.
        /// The package RuntimeFramework setting may be updated as a 
        /// result and the selected runtime is returned.
        /// </summary>
        /// <param name="package">A TestPackage</param>
        /// <returns>The selected RuntimeFramework</returns>
        RuntimeFramework SelectRuntimeFramework(TestPackage package);
    }
}
