// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.12.21�޸ģ�TestLoader��ΪTestLoaderArxNet��
// 2013.1.7�޸ģ�
//  1.��Ϊ����NUnit.Util.ArxNet.SettingsServiceArxNet��
// 2013.5.27�޸ģ�
//  1.��nunit2.6.2�������޸�
//  2.����Init��������ʼ����̬��Ա
// 2013.5.29�޸ģ�
//  1.��AddinManagerΪAddinManagerArxNet
//  2.��ProjectServiceΪProjectServiceArxNet
// 2013.6.1
//  1.�Ѿ���ServiceManagerΪServiceManagerArxNet
// 2013.6.2
//  1.�Ѿ���DomainManagerΪDomainManagerArxNet
// ****************************************************************

using System;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnit.Util.ArxNet
{
	/// <summary>
	/// Services is a utility class, which is used to provide access
	/// to services in a more simple way than is supported by te
	/// ServiceManager class itself.
	/// </summary>
	public class ServicesArxNet
	{
		#region AddinManager
		private static AddinManagerArxNet addinManager;
		public static AddinManagerArxNet AddinManager
		{
			get 
			{
				if (addinManager == null )
					addinManager = (AddinManagerArxNet)ServiceManagerArxNet.Services.GetService( typeof( AddinManagerArxNet ) );

				return addinManager; 
			}
		}
		#endregion

		#region AddinRegistry
		private static IAddinRegistry addinRegistry;
		public static IAddinRegistry AddinRegistry
		{
			get 
			{
				if (addinRegistry == null)
					addinRegistry = (IAddinRegistry)ServiceManagerArxNet.Services.GetService( typeof( IAddinRegistry ) );
                
				return addinRegistry;
			}
		}
		#endregion

		#region DomainManager
		private static DomainManagerArxNet domainManager;
        public static DomainManagerArxNet DomainManager
		{
			get
			{
				if ( domainManager == null )
                    domainManager = (DomainManagerArxNet)ServiceManagerArxNet.Services.GetService(typeof(DomainManagerArxNet));

				return domainManager;
			}
		}
		#endregion

		#region UserSettings
		private static ISettings userSettings;
		public static ISettings UserSettings
		{
			get 
			{ 
				if ( userSettings == null )
					userSettings = (ISettings)ServiceManagerArxNet.Services.GetService( typeof( ISettings ) );

				// Temporary fix needed to run TestDomain tests in test AppDomain
				// TODO: Figure out how to set up the test domain correctly
				if ( userSettings == null )
                    userSettings = new SettingsServiceArxNet();

				return userSettings; 
			}
		}
		
		#endregion

		#region RecentFilesService
#if CLR_2_0 || CLR_4_0
		private static RecentFiles recentFiles;
		public static RecentFiles RecentFiles
		{
			get
			{
				if ( recentFiles == null )
					recentFiles = (RecentFiles)ServiceManagerArxNet.Services.GetService( typeof( RecentFiles ) );

				return recentFiles;
			}
		}
#endif
		#endregion

		#region TestLoader
#if CLR_2_0 || CLR_4_0
        private static TestLoaderArxNet loader;
        public static TestLoaderArxNet TestLoader
		{
			get
			{
				if ( loader == null )
                    loader = (TestLoaderArxNet)ServiceManagerArxNet.Services.GetService(typeof(TestLoaderArxNet));

				return loader;
			}
		}
#endif
		#endregion

		#region TestAgency
		private static TestAgency agency;
		public static TestAgency TestAgency
		{
			get
			{
				if ( agency == null )
					agency = (TestAgency)ServiceManagerArxNet.Services.GetService( typeof( TestAgency ) );

				// Temporary fix needed to run ProcessRunner tests in test AppDomain
				// TODO: Figure out how to set up the test domain correctly
//				if ( agency == null )
//				{
//					agency = new TestAgency();
//					agency.Start();
//				}

				return agency;
			}
		}
		#endregion

		#region ProjectLoader
		private static ProjectServiceArxNet projectService;
		public static ProjectServiceArxNet ProjectService
		{
			get
			{
				if ( projectService == null )
					projectService = (ProjectServiceArxNet)
						ServiceManagerArxNet.Services.GetService( typeof( ProjectServiceArxNet ) );

				return projectService;
			}
		}
		#endregion

        /*2013-5-27lq��*/
        //��ʼ����̬��Ա
        public static void Init()
        {
            addinManager = null;
            addinRegistry = null;
            domainManager = null;
            userSettings = null;
            recentFiles = null;
            loader = null;
            agency = null;
            projectService = null;
        }
	}
}