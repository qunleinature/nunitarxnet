// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.7.29
//      1.在nunit2.6.2基础上
//      2.改ServicesArxNet
//  2014.8.22：
//      在NUnit2.6.3基础上修改
//  2015.1.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;

namespace NUnit.Util.ArxNet
{
	using System.Diagnostics;
	using System.Security.Policy;
	using System.Reflection;
	using System.Collections;
	using System.Configuration;
	using System.IO;

	using NUnit.Core;

	public class TestDomainArxNet : ProxyTestRunner, TestRunner
	{
        static Logger log = InternalTrace.GetLogger(typeof(TestDomainArxNet));

		#region Instance Variables

		/// <summary>
		/// The appdomain used  to load tests
		/// </summary>
		private AppDomain domain; 

		/// <summary>
		/// The TestAgent in the domain
		/// </summary>
		private DomainAgent agent;

		#endregion

		#region Constructors
		public TestDomainArxNet() : base( 0 ) { }

		public TestDomainArxNet( int runnerID ) : base( runnerID ) { }
		#endregion

		#region Properties
		public AppDomain AppDomain
		{
			get { return domain; }
		}
		#endregion

		#region Loading and Unloading Tests
		public override bool Load( TestPackage package )
		{
			Unload();

            log.Info("Loading " + package.Name);
			try
			{
				if ( this.domain == null )
					this.domain = ServicesArxNet.DomainManager.CreateDomain( package );

                if (this.agent == null)
                {
                    this.agent = DomainAgent.CreateInstance(domain);
                    this.agent.Start();
                }
            
				if ( this.TestRunner == null )
					this.TestRunner = this.agent.CreateRunner( this.ID );

                log.Info(
                    "Loading tests in AppDomain, see {0}_{1}.log", 
                    domain.FriendlyName, 
                    Process.GetCurrentProcess().Id);

				return TestRunner.Load( package );
			}
			catch
			{
                log.Error("Load failure");
				Unload();
				throw;
			}
		}

		public override void Unload()
		{
            if (this.TestRunner != null)
            {
                log.Info("Unloading");
                this.TestRunner.Unload();
                this.TestRunner = null;
            }

            if (this.agent != null)
            {
                log.Info("Stopping DomainAgent");
                this.agent.Dispose();
                this.agent = null;
            }

			if(domain != null) 
			{
                log.Info("Unloading AppDomain " + domain.FriendlyName);
				ServicesArxNet.DomainManager.Unload(domain);
				domain = null;
			}
		}
		#endregion

        #region Running Tests
        public override void BeginRun(EventListener listener, ITestFilter filter, bool tracing, LoggingThreshold logLevel)
        {
            log.Info("BeginRun in AppDomain {0}", domain.FriendlyName);
            base.BeginRun(listener, filter, tracing, logLevel);
        }
        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();

            Unload();
        }

        #endregion
    }
}
