// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.24：
//  1.利用cad命令直接测试
// ****************************************************************

using System;
using System.IO;
using NUnit.Framework;
using NUnit.TestUtilities;
using NUnit.Util.ProjectConverters;
using NUnit.Util.Tests.resources;
using NUnit.Util.ArxNet;
using NUnit.Util.ArxNet.ProjectConvertersArxNet;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.VisualStudioConverterArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class VisualStudioConverterArxNetTestsCommands
    {
        //public void FromCSharpProject()
        [CommandMethod("FromCSharpProject")]
        public void FromCSharpProject()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromCSharpProject();
        }

        //public void FromVBProject()
        [CommandMethod("FromVBProject")]
        public void FromVBProject()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromVBProject();
        }

        //public void FromJsharpProject()
        [CommandMethod("FromJsharpProject")]
        public void FromJsharpProject()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromJsharpProject();
        }

        //public void FromCppProject()
        [CommandMethod("FromCppProject")]
        public void FromCppProject()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromCppProject();
        }

        //public void FromProjectWithHebrewFileIncluded()
        [CommandMethod("FromProjectWithHebrewFileIncluded")]
        public void FromProjectWithHebrewFileIncluded()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromProjectWithHebrewFileIncluded();
        }

        //public void FromVSSolution2003()
        [CommandMethod("FromVSSolution2003")]
        public void FromVSSolution2003()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromVSSolution2003();
        }

        //public void FromVSSolution2005()
        [CommandMethod("FromVSSolution2005")]
        public void FromVSSolution2005()
        {
            VisualStudioConverterArxNetTests tests = new VisualStudioConverterArxNetTests();
            tests.CreateImporter();
            tests.FromVSSolution2005();
        }
    }
}
