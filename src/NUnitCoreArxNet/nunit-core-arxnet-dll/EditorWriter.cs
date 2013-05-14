// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.5.9新建：EditorWriter类
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace NUnit.Core.ArxNet
{
    [Serializable, ComVisible(true)]
    public class EditorWriter : TextWriter
    {
        private static UnicodeEncoding m_encoding;

        public override Encoding Encoding
        {
            get 
            { 
                //throw new NotImplementedException(); 
                if (m_encoding == null)
                {
                    m_encoding = new UnicodeEncoding(false, false);
                }
                return m_encoding;
            }
        }
    }
}
