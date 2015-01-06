// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.29修改：
//      1.在nunit2.6.2基础上修改
//      2.NUnit.Util.AddinManager改为NUnit.Util.ArxNet.AddinManagerArxNet类
//      3.改Services改为ServicesArxNet
//  2014.8.22：
//      在NUnit2.6.3基础上修改
//  2015.1.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnit.Util.ArxNet
{
	public class AddinManagerArxNet : IService
	{
		static Logger log = InternalTrace.GetLogger(typeof(AddinManagerArxNet));

		#region Instance Fields
		IAddinRegistry addinRegistry;
		#endregion

		#region Constructor
		public AddinManagerArxNet()
		{
		}
		#endregion

		#region Addin Registration
		public void RegisterAddins()
		{
			// Load any extensions in the addins directory
			DirectoryInfo dir = new DirectoryInfo( NUnitConfiguration.AddinDirectory );
			if ( dir.Exists )
				foreach( FileInfo file in dir.GetFiles( "*.dll" ) )
					Register( file.FullName );
		}

		public void Register( string path )
		{
			try
			{
				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = Path.GetFileNameWithoutExtension(path);
				assemblyName.CodeBase = path;
				Assembly assembly = Assembly.Load(assemblyName);
				log.Debug( "Loaded " + Path.GetFileName(path) );

				foreach ( Type type in assembly.GetExportedTypes() )
				{
					if ( type.GetCustomAttributes(typeof(NUnitAddinAttribute), false).Length == 1 )
					{
						Addin addin = new Addin( type );
                        if ( addinRegistry.IsAddinRegistered(addin.Name) )
                            log.Error( "Addin {0} was already registered", addin.Name );
                        else
                        {
						    addinRegistry.Register( addin );
						    log.Debug( "Registered addin: {0}", addin.Name );
                        }
					}
				}
			}
			catch( Exception ex )
			{
				// NOTE: Since the gui isn't loaded at this point, 
				// the trace output will only show up in Visual Studio
				log.Error( "Failed to load" + path, ex  );
			}
		}
		#endregion

		#region IService Members

		public void InitializeService()
		{
			addinRegistry = ServicesArxNet.AddinRegistry;
			RegisterAddins();
		}

		public void UnloadService()
		{
		}

		#endregion
	}
}
