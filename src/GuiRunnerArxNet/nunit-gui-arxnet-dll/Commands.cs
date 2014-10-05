// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.28：
//      在NUnit2.6.3基础上修改
// ****************************************************************

using System;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

[assembly: CommandClass(typeof(NUnit.Gui.ArxNet.Commands))]

namespace NUnit.Gui.ArxNet
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

        // Define Command
        [CommandMethod("nunit", CommandFlags.Session)]
        public void Cmd_NUnit() // This method can have any name
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

                //NUnit.Gui.ArxNet.AppEntryArxNet.Main(args);
                AppEntryArxNet.Init();
                AppEntryArxNet.Main(args);
                //AppEntryArxNet.CleanUp();
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Application.ShowAlertDialog(e.Message);
                //ed.WriteMessage(e.Message);
            }
            catch (ApplicationException e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            catch (System.Exception e)
            {
                Application.ShowAlertDialog(e.Message);
                //ed.WriteMessage(e.Message);
            }
        }
        [CommandMethod("nunit-arxnet", CommandFlags.Session)]
        public void Cmd_NUnit_ArxNet() 
        {
            // Put your command code here
            Cmd_NUnit();
        }
    }
}