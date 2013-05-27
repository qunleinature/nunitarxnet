// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2013.5.27修改：
//  1.在nunit2.6.2基础上修改
//  2.NUnit.Util.ServiceManger改为NUnit.Util.ArxNet.ServiceMangerArxNet类
//  3.增加Init方法，初始化静态成员
// ****************************************************************

using System;
using System.Collections;
using NUnit.Core;

namespace NUnit.Util.ArxNet
{
	/// <summary>
	/// Summary description for ServiceManger.
	/// </summary>
	public class ServiceManagerArxNet
	{
		private ArrayList services = new ArrayList();
		private Hashtable serviceIndex = new Hashtable();

		private static ServiceManagerArxNet defaultServiceManager = new ServiceManagerArxNet();

		static Logger log = InternalTrace.GetLogger(typeof(ServiceManagerArxNet));

        /*2013-5-27lq加*/
        //初始化静态成员
        public static void Init()
        {
		    defaultServiceManager = new ServiceManagerArxNet();
		    log = InternalTrace.GetLogger(typeof(ServiceManagerArxNet));
        }

		public static ServiceManagerArxNet Services
		{
			get { return defaultServiceManager; }
		}

		public void AddService( IService service )
		{
			services.Add( service );
			log.Debug( "Added " + service.GetType().Name );
		}

		public IService GetService( Type serviceType )
		{
			IService theService = (IService)serviceIndex[serviceType];
			if ( theService == null )
				foreach( IService service in services )
				{
					// TODO: Does this work on Mono?
					if( serviceType.IsInstanceOfType( service ) )
					{
						serviceIndex[serviceType] = service;
						theService = service;
						break;
					}
				}

			if ( theService == null )
				log.Error( string.Format( "Requested service {0} was not found", serviceType.FullName ) );
			else
				log.Debug( string.Format( "Request for service {0} satisfied by {1}", serviceType.Name, theService.GetType().Name ) );
			
			return theService;
		}

		public void InitializeServices()
		{
			foreach( IService service in services )
			{
				log.Info( "Initializing " + service.GetType().Name );
                try
                {
                    service.InitializeService();
                }
                catch (Exception ex)
                {
                    log.Error("Failed to initialize service", ex);
                }
			}
		}

		public void StopAllServices()
		{
			// Stop services in reverse of initialization order
			// TODO: Deal with dependencies explicitly
			int index = services.Count;
            while (--index >= 0)
            {
                IService service = services[index] as IService;
                log.Info( "Stopping " + service.GetType().Name );
                try
                {
                    service.UnloadService();
                }
                catch (Exception ex)
                {
                    log.Error("Failure stopping service", ex);
                }
            }
		}

		public void ClearServices()
		{
            log.Info("Clearing Service list");
			services.Clear();
		}

		private ServiceManagerArxNet() { }
	}
}
