// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2012.5.17新建：测试EditorWriter类
//  2014.9.27：
//      在NUnit2.6.3基础上调试
//  2015.1.4：
//      在NUnit2.6.4基础上调试
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
        [Category("WriteLine")]
        public void WriteLine_NewLine()
        {
            Console.WriteLine();
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_bool()
        {
            Console.WriteLine(true);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_char()
        {
            Console.WriteLine('A');
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_chars()
        {
            char[] buffer = new char[3];
            buffer[0] = 't';
            buffer[1] = 'r';
            buffer[2] = 'y';
            Console.WriteLine(buffer);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_decimal()
        {
            decimal dividend = Decimal.One;
            decimal divisor = 3;
            Console.WriteLine(dividend / divisor * divisor);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_double()
        {
            Console.WriteLine(1.23456789d);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_int()
        {
            Console.WriteLine(-2147483648);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_long()
        {
            Console.WriteLine(9223372036854775807L);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_object()
        {
            Console.WriteLine(m_EditorWriter);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_float()
        {
            Console.WriteLine(0.123456789f);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string()
        {
            Console.WriteLine("string");
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_uint()
        {
            Console.WriteLine(4294967295u);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_ulong()
        {
            Console.WriteLine(18446744073709551615ul);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string_object()
        {
            DateTime thisDate = DateTime.Now;
            Console.WriteLine(
                "(d) Short date: . . . . . . . {0:d}\n" +
                "(D) Long date:. . . . . . . . {0:D}\n" +
                "(t) Short time: . . . . . . . {0:t}\n" +
                "(T) Long time:. . . . . . . . {0:T}\n" +
                "(f) Full date/short time: . . {0:f}\n" +
                "(F) Full date/long time:. . . {0:F}\n" +
                "(g) General date/short time:. {0:g}\n" +
                "(G) General date/long time: . {0:G}\n" +
                "    (default):. . . . . . . . {0} (default = 'G')\n" +
                "(M) Month:. . . . . . . . . . {0:M}\n" +
                "(R) RFC1123:. . . . . . . . . {0:R}\n" +
                "(s) Sortable: . . . . . . . . {0:s}\n" +
                "(u) Universal sortable: . . . {0:u} (invariant)\n" +
                "(U) Universal full date/time: {0:U}\n" +
                "(Y) Year: . . . . . . . . . . {0:Y}\n",
                thisDate);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string_objects()
        {
            object[] arg = new object[5] { "arg0", "arg1", "arg2", "arg3", "arg4" };
            Console.WriteLine("arg0：{0}\narg1：{1}\narg2：{2}\narg3：{3}\narg4：{4}", arg);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_chars_int_int()
        {
            char[] buffer = new char[20];
            string str = "\nHello EditorWriter!";
            str.CopyTo(0, buffer, 0, 20);
            Console.WriteLine(buffer, 7, 6);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string_object_object()
        {
            Console.WriteLine(
                "(C) Currency: . . . . . . . . {0:C}\n" +
                "(D) Decimal:. . . . . . . . . {0:D}\n" +
                "(E) Scientific: . . . . . . . {1:E}\n" +
                "(F) Fixed point:. . . . . . . {1:F}\n" +
                "(G) General:. . . . . . . . . {0:G}\n" +
                "    (default):. . . . . . . . {0} (default = 'G')\n" +
                "(N) Number: . . . . . . . . . {0:N}\n" +
                "(P) Percent:. . . . . . . . . {1:P}\n" +
                "(R) Round-trip: . . . . . . . {1:R}\n" +
                "(X) Hexadecimal:. . . . . . . {0:X}\n",
                -123, -123.45f);
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string_object_object_object()
        {
            Console.WriteLine("arg0：{0}\narg1：{1}\narg2：{2}", "arg0", "arg1", "arg2");
        }

        [Test]
        [Category("WriteLine")]
        public void WriteLine_string_object_object_object_object()
        {
            Console.WriteLine("arg0：{0}\narg1：{1}\narg2：{2}\narg3：{3}", "arg0", "arg1", "arg2", "arg3");
        }

    }
}
