// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.5.9新建：
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

        public override Encoding Encoding
        {
            get { throw new NotImplementedException(); }
        }
    }
}
