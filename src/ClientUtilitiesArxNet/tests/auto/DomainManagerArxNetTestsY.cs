// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.28修改：测试NUnit.Util.ArxNet.DomainManagerArxNet类
// 2013.6.1
//  1.已经改DomainManager为DomainManagerArxNet
// ****************************************************************

using System;
using System.Collections;
using System.IO;
using NUnit.Framework;

namespace NUnit.Util.ArxNet.Tests
{
    public class DomainManagerArxNetTests
    {
        static string path1 = TestPath("/test/bin/debug/test1.dll");
        static string path2 = TestPath("/test/bin/debug/test2.dll");
        static string path3 = TestPath("/test/utils/test3.dll");

        [Test]
        public void GetPrivateBinPath()
        {
            string[] assemblies = new string[] { path1, path2, path3 };

            Assert.AreEqual(
                TestPath("bin/debug") + Path.PathSeparator + TestPath("utils"),
                DomainManagerArxNet.GetPrivateBinPath(TestPath("/test"), assemblies));
        }

        [Test]
        public void GetCommonAppBase_OneElement()
        {
            string[] assemblies = new string[] { path1 };

            Assert.AreEqual(
                TestPath("/test/bin/debug"),
                DomainManagerArxNet.GetCommonAppBase(assemblies));
        }

        [Test]
        public void GetCommonAppBase_TwoElements_SameDirectory()
        {
            string[] assemblies = new string[] { path1, path2 };

            Assert.AreEqual(
                TestPath("/test/bin/debug"),
                DomainManagerArxNet.GetCommonAppBase(assemblies));
        }

        [Test]
        public void GetCommonAppBase_TwoElements_DifferentDirectories()
        {
            string[] assemblies = new string[] { path1, path3 };

            Assert.AreEqual(
                TestPath("/test"),
                DomainManagerArxNet.GetCommonAppBase(assemblies));
        }

        [Test]
        public void GetCommonAppBase_ThreeElements_DiferentDirectories()
        {
            string[] assemblies = new string[] { path1, path2, path3 };

            Assert.AreEqual(
                TestPath("/test"),
                DomainManagerArxNet.GetCommonAppBase(assemblies));
        }

        [Test]
        public void UnloadUnloadedDomain()
        {
            AppDomain domain = AppDomain.CreateDomain("DomainManagerArxNetTests-domain");
            AppDomain.Unload(domain);

            DomainManagerArxNet manager = new DomainManagerArxNet();
            manager.Unload(domain);
        }

        /// <summary>
        /// Take a valid Linux filePath and make a valid windows filePath out of it
        /// if we are on Windows. Change slashes to backslashes and, if the
        /// filePath starts with a slash, add C: in front of it.
        /// </summary>
        private static string TestPath(string path)
        {
            if (Path.DirectorySeparatorChar != '/')
            {
                path = path.Replace('/', Path.DirectorySeparatorChar);
                if (path[0] == Path.DirectorySeparatorChar)
                    path = "C:" + path;
            }

            return path;
        }
    }
}
