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

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

namespace NUnit.Core.ArxNet
{
    [Serializable, ComVisible(true)]
    public class EditorWriter : TextWriter
    {
        private static UnicodeEncoding m_Encoding;
        private Editor m_Editor;

        public EditorWriter()
        {
            //throw new System.NotImplementedException();
            m_Encoding = new UnicodeEncoding(false, false);
            m_Editor = Application.DocumentManager.MdiActiveDocument.Editor;
        }

        public EditorWriter(Editor ed)
        {
            //throw new System.NotImplementedException();            
            m_Encoding = new UnicodeEncoding(false, false);

            //总保持当前活动Editor
            Editor activeEditor = Application.DocumentManager.MdiActiveDocument.Editor;
            if (ed !=null && ed == activeEditor)
                m_Editor = ed;
            else
                m_Editor = activeEditor;
        }

        public override Encoding Encoding
        {
            get 
            { 
                //throw new NotImplementedException();
                if (m_Encoding == null)
                {
                    m_Encoding = new UnicodeEncoding(false, false);
                }
                return m_Encoding;
            }
        }

        public Editor Editor
        {
            get
            {
                //throw new System.NotImplementedException();
                //总保持当前活动Editor
                Editor activeEditor = Application.DocumentManager.MdiActiveDocument.Editor;
                if (m_Editor == null || m_Editor != activeEditor)
                {
                    m_Editor = activeEditor;
                }
                return m_Editor;
            }
        }

        protected override void Dispose(bool disposing)
        {
            //throw new System.NotImplementedException();
            m_Encoding = null;
            m_Editor = null;
        }
    }
}
