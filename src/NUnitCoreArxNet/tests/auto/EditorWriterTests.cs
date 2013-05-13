// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.5.13新建：测试EditorWriter类
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace NUnit.Core.ArxNet.Tests
{
    [TestFixture]
    public class EditorWriterTests
    {
        //public override Encoding Encoding
        [Test]
        public void Get_Encoding()
        {
            EditorWriter editorWriter = new EditorWriter();
            Assert.That(editorWriter.Encoding, Is.Not.Null);
        }
    }
}
