// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// 2013.5.31修改：
//  1.在nunit2.6.2基础上修改
//  2.NUnit.UiKit.TextBoxDisplay改为NUnit.UiKit.ArxNet.TextBoxDisplayArxNet类
//  3.改Services为ServicesArxNet
// ****************************************************************

using System;
using System.Diagnostics;
using System.Text;
using NUnit.Core;
using NUnit.Util;

using NUnit.Util.ArxNet;

namespace NUnit.UiKit
{
    public class TextDisplayContentArxNet
    {
        private char[] content = new char[] { '0', '0', '0', '0', '0' };

        public TextDisplayContentArxNet()
        {
        }

        public bool Out 
        {
            get { return content[0] == '1'; }
            set { content[0] = value ? '1' : '0'; } 
        }

        public bool Error
        {
            get { return content[1] == '1'; }
            set { content[1] = value ? '1' : '0'; } 
        }

        public bool Trace
        {
            get { return content[2] == '1'; }
            set { content[2] = value ? '1' : '0'; }
        }

        public LoggingThreshold LogLevel
        {
            get { return (LoggingThreshold)(content[3] - '0'); }
            set { content[3] = (char)((int)value + '0'); }
        }

        public TestLabelLevel Labels
        {
            get { return (TestLabelLevel)(content[4] - '0'); }
            set { content[4] = (char)((int)value + '0'); }
        }

        public static TextDisplayContentArxNet FromSettings(string name)
        {
            TextDisplayContentArxNet content = new TextDisplayContentArxNet();
            content.LoadSettings(name);
            return content;
        }

        public void LoadSettings(string name)
        {
            ISettings settings = ServicesArxNet.UserSettings;
            string prefix = "Gui.TextOutput." + name;

            string rep = settings.GetSetting(prefix + ".Content", "00000");

            // Assume new format but if it isn't try the old one
            if (!LoadUsingNewFormat(rep))
                LoadUsingOldFormat(rep);                
        }

        private bool LoadUsingNewFormat(string rep)
        {
            if (rep.Length != 5) return false;

            foreach (char c in rep)
                if (!char.IsDigit(c))
                    return false;

            this.content = rep.ToCharArray();

            return true;
        }

        private void LoadUsingOldFormat(string content)
        {
            ContentType contentType = (ContentType)System.Enum.Parse(typeof(ContentType), content, false);
            this.Out = (contentType & ContentType.Out) != 0;
            this.Error = (contentType & ContentType.Error) != 0;
            this.Trace = (contentType & ContentType.Trace) != 0;
            this.LogLevel = (contentType & ContentType.Log) != 0
                ? LoggingThreshold.All
                : LoggingThreshold.Off;
            this.Labels = (contentType & ContentType.Labels) != 0
                ? (contentType & ContentType.LabelOnlyOnOutput) != 0
                    ? TestLabelLevel.All
                    : TestLabelLevel.On
                : TestLabelLevel.Off;
        }

        [Flags]
        private enum ContentType
        {
            Empty = 0,
            Out = 1,
            Error = 2,
            Trace = 4,
            Log = 8,
            Labels = 64,
            LabelOnlyOnOutput = 128
        }

        public void SaveSettings(string name)
        {
            ISettings settings = ServicesArxNet.UserSettings;
            string prefix = "Gui.TextOutput." + name;

            settings.SaveSetting(prefix + ".Content", new string(content));
        }
    }
}
