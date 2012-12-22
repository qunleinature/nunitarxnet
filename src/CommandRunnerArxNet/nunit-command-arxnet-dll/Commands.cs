// (C) Copyright 2002-2007 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

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
        static public void CMD_NUnit_Command() // This method can have any name
        {
            // Put your command code here
            //Application.ShowAlertDialog("nunit-command");
            try
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

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

                RunnerArxNet.Main(args);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            catch (System.Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
        }

        // Define Command "nunit-console"
        [CommandMethod("nunit-command-arxnet", CommandFlags.Session)]
        static public void Cmd_NUnit_Command_Arxnet() // This method can have any name
        {
            // Put your command code here
            CMD_NUnit_Command();
        }

    }
}