// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2012.12.19�޸�
// 2013.5.29�޸�
//   1.��ProjectServiceΪProjectServiceArxNet
// 2013.6.1
//   1.�Ѿ���ServiceManagerΪServiceManagerArxNet
// 2013.6.2  
//   1.�Ѿ���DomainManagerΪDomainManagerArxNet
// 2013.6.7  
//   1.�Ѿ���SettingsGroupΪSettingsGroupArxNet
//   2.�Ѹ���nunit2.6.2����
// 2013.7.29
//   1.�Ѹ�ServicesArxNet
// 2014.7.18
//   1.��������CAD�����²�֧�ֳ������µĲ��ԣ�
//   2.ServiceManagerArxNet��ʹ�ô���
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.8.22��
//  ��NUnit2.6.3�������޸�
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
            //2014.9.17����
            ServiceManagerArxNet.Init();
            DomainManagerArxNet.Init();
            ServicesArxNet.Init();

            ServiceManagerArxNet.Services.AddService( new DummySettingsService() );
			ServiceManagerArxNet.Services.AddService( new DomainManagerArxNet() );
            ServiceManagerArxNet.Services.AddService( new ProjectServiceArxNet() );
            ServiceManagerArxNet.Services.AddService( new TestAgency( "TestDomain_TestAgency", 0 ) );
			ServicesArxNet.TestAgency.Start();
		}

		[TearDown]
		public void ClearServices()
		{
            //2014.7.17 Lei Qun�޸�,ServiceManagerArxNet��ʹ�ô���
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