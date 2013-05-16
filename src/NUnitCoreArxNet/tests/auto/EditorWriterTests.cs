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

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Framework;

using Com.Utility.UnitTest;

namespace NUnit.Core.ArxNet.Tests
{
    [TestFixture]
    public class EditorWriterTests
    {
        //public EditorWriter()
        [Test]
        public void Constructor_Default()
        {
            EditorWriter editorWriter = new EditorWriter();
            Assert.That(editorWriter, Is.Not.Null);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Assert.That(editorWriter.Editor, Is.EqualTo(ed));
            UnicodeEncoding encoding = new UnicodeEncoding(false, false);
            Assert.That(editorWriter.Encoding.CodePage, Is.EqualTo(encoding.CodePage));
        }

        //public EditorWriter(Editor ed)
        [Test]
        [Category("Constructor_Editor")]
        public void Constructor_EditorIsActive()
        {
            Editor ed= Application.DocumentManager.MdiActiveDocument.Editor;
            EditorWriter editorWriter = new EditorWriter(ed);
            Assert.That(editorWriter, Is.Not.Null);
            Assert.That(editorWriter.Editor, Is.EqualTo(ed));
            UnicodeEncoding encoding = new UnicodeEncoding(false, false);
            Assert.That(editorWriter.Encoding.CodePage, Is.EqualTo(encoding.CodePage));
        }

        [Test]
        [Category("Constructor_Editor")]
        public void Constructor_EditorNotActive()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Application.DocumentManager.Add(null);
            //ed.WriteMessage("\n原窗口");
            EditorWriter editorWriter = new EditorWriter(ed);
            Assert.That(editorWriter, Is.Not.Null);
            ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Assert.That(editorWriter.Editor, Is.EqualTo(ed));
            UnicodeEncoding encoding = new UnicodeEncoding(false, false);
            Assert.That(editorWriter.Encoding.CodePage, Is.EqualTo(encoding.CodePage));
        }

        [Test]
        [Category("Constructor_Editor")]
        public void Constructor_EditorNull()
        {
            EditorWriter editorWriter = new EditorWriter(null);
            Assert.That(editorWriter, Is.Not.Null);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Assert.That(editorWriter.Editor, Is.EqualTo(ed));
            UnicodeEncoding encoding = new UnicodeEncoding(false, false);
            Assert.That(editorWriter.Encoding.CodePage, Is.EqualTo(encoding.CodePage));
        }

        //public override void Close()
        [Test]
        public void Close()
        {
            EditorWriter editorWriter = new EditorWriter();
            editorWriter.Close();
            object obj = UnitTestHelper.GetNonPublicField(editorWriter, "m_Encoding");
            UnicodeEncoding encoding = (obj != null) ? obj as UnicodeEncoding : null;
            Assert.That(encoding, Is.Null);
            obj = UnitTestHelper.GetNonPublicField(editorWriter, "m_Editor");
            Editor ed = (obj != null) ? obj as Editor : null;
            Assert.That(ed, Is.Null);
        }        

        //protected override void Dispose(bool disposing)
        [Test]
        public void Dispose_bool()
        {
            EditorWriter editorWriter = new EditorWriter();
            UnitTestHelper.CallNonPublicMethod(editorWriter, "Dispose", true);
            object obj = UnitTestHelper.GetNonPublicField(editorWriter, "m_Encoding");
            UnicodeEncoding encoding = (obj != null) ? obj as UnicodeEncoding : null;
            Assert.That(encoding, Is.Null);
            obj = UnitTestHelper.GetNonPublicField(editorWriter, "m_Editor");
            Editor ed = (obj != null) ? obj as Editor : null;
            Assert.That(ed, Is.Null);
        }        

        //public Editor Editor
        [Test]
        public void Get_Editor()
        {
            EditorWriter editorWriter = new EditorWriter();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Assert.That(editorWriter.Editor, Is.Not.Null);
            Assert.That(editorWriter.Editor, Is.EqualTo(ed));
        }

        //public override Encoding Encoding
        [Test]
        public void Get_Encoding()
        {
            EditorWriter editorWriter = new EditorWriter();
            UnicodeEncoding encoding = new UnicodeEncoding(false, false);
            Assert.That(editorWriter.Encoding, Is.Not.Null);
            Assert.That(editorWriter.Encoding.CodePage, Is.EqualTo(encoding.CodePage));

        }
        
    }
}
