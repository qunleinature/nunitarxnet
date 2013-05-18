// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.5.17新建：测试EditorWriter类
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Framework;

using Com.Utility.UnitTest;

namespace NUnit.Core.ArxNet.Tests
{
    [TestFixture]
    public class EditorWriterTests
    {
        //public override void Write(char value)
        [Test]
        public void Write_char()
        {
            EditorWriter editorWriter = new EditorWriter();
            editorWriter.Write('A');
            editorWriter.Close();
        }

        //public override void Write(string value)
        [Test]
        public void Write_string()
        {
            EditorWriter editorWriter = new EditorWriter();
            editorWriter.Write("\nHello EditorWriter!");
            editorWriter.Close();
        }

        //public override void Write(char[] buffer, int index, int count)
        [Test]
        public void Write_chars_int_int()
        {
            EditorWriter editorWriter = new EditorWriter();
            char[] buffer = new char[20];
            string str = "\nHello EditorWriter!";
            str.CopyTo(0, buffer, 0, 20);
            editorWriter.Write(buffer, 7, 6);
            editorWriter.Close();
        }
    }

    [TestFixture]
    public class ConsoleSetOutEditorWriterTests
    {
        private TextWriter m_StdOut;
        private EditorWriter m_EditorWriter;

        [TestFixtureSetUp]
        public void Init()
        {
            m_StdOut = Console.Out;
            m_EditorWriter = new EditorWriter();
            Console.SetOut(m_EditorWriter);
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            Console.SetOut(m_StdOut);
            m_EditorWriter.Close();
            m_EditorWriter = null;
        }

        [Test]
        public void ConsoleWriteLine()
        {
            Console.WriteLine("Console SetOut EditorWriter，Test Console WriteLine：");
            Console.WriteLine();
            
        }
    }
}
