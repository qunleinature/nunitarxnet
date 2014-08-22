// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2012.12.19修改
// 2013.5.29修改
//   1.改ProjectService为ProjectServiceArxNet
// 2013.6.1
//   1.已经改ServiceManager为ServiceManagerArxNet
// 2013.6.2  
//   1.已经改DomainManager为DomainManagerArxNet
// 2013.6.7  
//   1.已经改SettingsGroup为SettingsGroupArxNet
//   2.已改在nunit2.6.2基础
// 2013.7.29
//   1.已改ServicesArxNet
// 2014.7.18
//   1.可能是在CAD环境下不支持程序域下的测试？
//   2.ServiceManagerArxNet不使用代理
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Util;

using NUnit.Util.ArxNet;

namespace NUnit.Util.ArxNet.Tests
{
	/// <summary>
	/// This fixture is used to set up a distinct ServiceMangager with its
	/// own services in the test domain when testing NUnit. We start and
	/// stop services selectively. In particular, we don't want to 
	/// do a StopAllServices command, because that would cause any 
	/// changes to UserSettings to be saved. 
	/// 
	/// TODO: Refactor SettingsService so we can use it without actually
	/// touching the backup storage.
	/// </summary>
	[SetUpFixture]
	public class ServiceManagerArxNetSetUpFixture
	{
		[SetUp]
		public void CreateServicesForTestDomain()
		{
			ServiceManagerArxNet.Services.AddService( new DummySettingsService() );
			ServiceManagerArxNet.Services.AddService( new DomainManagerArxNet() );
            ServiceManagerArxNet.Services.AddService( new ProjectServiceArxNet() );
            //2014.7.17 Lei Qun修改,ServiceManagerArxNet不使用代理
			//ServiceManagerArxNet.Services.AddService( new TestAgency( "TestDomain_TestAgency", 0 ) );
			//ServicesArxNet.TestAgency.Start();
		}

		[TearDown]
		public void ClearServices()
		{
            //2014.7.17 Lei Qun修改,ServiceManagerArxNet不使用代理
			//ServicesArxNet.TestAgency.Stop();
			ServiceManagerArxNet.Services.ClearServices();
		}
	}

    class DummySettingsService : SettingsGroupArxNet, NUnit.Core.IService
    {
        public DummySettingsService()
        {
            this.storage = new MemorySettingsStorage();
        }

        #region IService Members

        public void InitializeService()
        {
        }

        public void UnloadService()
        {
        }

        #endregion
    }
}
