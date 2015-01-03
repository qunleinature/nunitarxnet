// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.8.21：
//      在NUnit2.6.3基础上修改
//  2015.1.3：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Core;

[assembly: CommandClass(typeof(NUnit.CommandRunner.ArxNet.Commands))]

namespace NUnit.CommandRunner.ArxNet
{
    /// <summary>
    /// Summary description for Commands.
    /// </summary>
    public class Commands
    {
        public Commands()
        {
            //
            // TODO: Add constructor logic here
            //
        }        

        // Define Command "nunit-command"
        [CommandMethod("nunit-command", CommandFlags.Session)]
        static public void NUnit_Command() // This method can have any name
        {
            // Put your command code here
            //Application.ShowAlertDialog("nunit-command");
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            { 
                PromptStringOptions opt = new PromptStringOptions("args:");
                opt.AllowSpaces = true;
                PromptResult res = ed.GetString(opt);
                string[] args = new string[0];
                switch (res.Status)
                {
                    case PromptStatus.OK:
                        if (res.StringResult.Trim() != "")
                        {
                            args = res.StringResult.Split(' ');
                        }
                        break;
                    default:
                        break;
                }

                RunnerArxNet.Init();//2013.5.25加                
                RunnerArxNet.Main(args);
                RunnerArxNet.CleanUp();//2013.5.25加
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                //Application.ShowAlertDialog(e.Message);
                ed.WriteMessage(e.Message);
            }
            catch (System.Exception e)
            {
                //Application.ShowAlertDialog(e.Message);
                ed.WriteMessage(e.Message);
            }
        }

        // Define Command "nunit-console"
        [CommandMethod("nunit-command-arxnet", CommandFlags.Session)]
        static public void NUnit_Command_Arxnet() // This method can have any name
        {
            // Put your command code here
            NUnit_Command();
        }

    }
}