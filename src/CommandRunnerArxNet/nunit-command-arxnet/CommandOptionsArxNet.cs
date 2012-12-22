// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
//2012年1月6日，雷群修改
// ****************************************************************

namespace NUnit.CommandRunner.ArxNet
{
    using System;
    using System.Reflection;
    using System.Collections;
    using System.Text;
    using Codeblast;
    using NUnit.Util;
    using NUnit.Core;

    using NUnit.ConsoleRunner;

    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.ApplicationServices;

    public class CommandOptionsArxNet : ConsoleOptions
    {
        private bool allowForwardSlash;

        public CommandOptionsArxNet( params string[] args ) : this( System.IO.Path.DirectorySeparatorChar != '/', args ) {}        

        public CommandOptionsArxNet(bool allowForwardSlash, params string[] args) : base(allowForwardSlash, args)
        {
            this.allowForwardSlash = allowForwardSlash;
            process = ProcessModel.Single;
            domain = DomainUsage.None;
            nothread = true;
        }

        public override string GetHelpText()
        {
            StringBuilder helpText = new StringBuilder();

            Type t = this.GetType();
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            char optChar = allowForwardSlash ? '/' : '-';
            foreach (FieldInfo field in fields)
            {
                if (!field.Name.Equals("process") && !field.Name.Equals("domain") && !field.Name.Equals("nothread"))
                {                   
                    object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
                    if (atts.Length > 0)
                    {
                        OptionAttribute att = (OptionAttribute)atts[0];
                        if (att.Description != null)
                        {
                            string valType = "";
                            if (att.Value == null)
                            {
                                if (field.FieldType == typeof(float)) valType = "=FLOAT";
                                else if (field.FieldType == typeof(string)) valType = "=STR";
                                else if (field.FieldType != typeof(bool)) valType = "=X";
                            }

                            helpText.AppendFormat("{0}{1,-20}\t{2}", optChar, field.Name + valType, att.Description);
                            if (att.Short != null)
                                helpText.AppendFormat(" (Short format: {0}{1}{2})", optChar, att.Short, valType);
                            helpText.Append(Environment.NewLine);
                        }
                    }
                }
            }
            return helpText.ToString();
        }

        public override void Help()
        {            
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage("\n");
            ed.WriteMessage("\nNUNIT-COMMAND");
            ed.WriteMessage("\nargs:[inputfiles] [options]");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nRuns a set of NUnit tests from the AutoCad command lines.");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nYou may specify one or more assemblies or a single");
            ed.WriteMessage("\nproject file of type .nunit.");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nOptions:");
            ed.WriteMessage("\n" + GetHelpText());
            ed.WriteMessage("\n");
            ed.WriteMessage("\nOptions that take values may use an equal sign, a colon");
            ed.WriteMessage("\nor a space to separate the option from its value.");
            ed.WriteMessage("\n");
        }
    }
}
