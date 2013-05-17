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

        //public override void Write(char[] buffer, int index, int count)
        [Test]
        [Category("Write_chars_int_int")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Write_charsNull_int_int()
        {
            EditorWriter editorWriter = new EditorWriter();
            editorWriter.Write(null, 7, 6);
            editorWriter.Close();
        }

        [Test]
        [Category("Write_chars_int_int")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Write_chars_intNegative_int()
        {
            EditorWriter editorWriter = new EditorWriter();
            char[] buffer = new char[3];
            buffer[0] = 't';
            buffer[1] = 'r';
            buffer[2] = 'y';
            editorWriter.Write(buffer, -1, 3);
            editorWriter.Close();
        }

        [Test]
        [Category("Write_chars_int_int")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Write_chars_int_intNegative()
        {
            EditorWriter editorWriter = new EditorWriter();
            char[] buffer = new char[3];
            buffer[0] = 't';
            buffer[1] = 'r';
            buffer[2] = 'y';
            editorWriter.Write(buffer, 0, -3);
            editorWriter.Close();
        }

        [Test]
        [Category("Write_chars_int_int")]
        [ExpectedException(typeof(ArgumentException))]
        public void Write_chars_int_int_InvalidOffLen()
        {
            EditorWriter editorWriter = new EditorWriter();
            char[] buffer = new char[3];
            buffer[0] = 't';
            buffer[1] = 'r';
            buffer[2] = 'y';
            editorWriter.Write(buffer, 1, 3);
            editorWriter.Close();
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
