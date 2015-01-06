// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.9.25修改：
//      1.在nunit2.6.3基础上修改
//      2.NUnit.Util.RemoteTestAgent改NUnit.Util.ArxNet.RemoteTestAgentArxNet
//      3.InProcessTestRunnerFactory改InProcessTestRunnerFactoryArxNet
//  2015.1.6修改：
//      1.在nunit2.6.4基础上修改
// ****************************************************************

using System;
using System.Threading;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Util.ArxNet
{
	/// <summary>
	/// RemoteTestAgent represents a remote agent executing in another process
	/// and communicating with NUnit by TCP. Although it is similar to a
	/// TestServer, it does not publish a Uri at which clients may connect 
	/// to it. Rather, it reports back to the sponsoring TestAgency upon 
	/// startup so that the agency may in turn provide it to clients for use.
	/// </summary>
	public class RemoteTestAgentArxnet : TestAgent
	{
		static Logger log = InternalTrace.GetLogger(typeof(RemoteTestAgentArxnet));

		#region Fields

        private ManualResetEvent stopSignal = new ManualResetEvent(false);
		
		#endregion

		#region Constructor
		/// <summary>
		/// Construct a RemoteTestAgentArxNet
		/// </summary>
		public RemoteTestAgentArxnet( Guid agentId, TestAgency agency )
            : base(agentId, agency) { }
		#endregion

		#region Properties
		public int ProcessId
		{
			get { return System.Diagnostics.Process.GetCurrentProcess().Id; }
		}
		#endregion

		#region Public Methods
		public override TestRunner CreateRunner(int runnerID)
		{
			return new AgentRunner(runnerID);
		}

        public override bool Start()
		{
			log.Info("Agent starting");

			try
			{
				this.Agency.Register( this );
				log.Debug( "Registered with TestAgency" );
			}
			catch( Exception ex )
			{
				log.Error( "RemoteTestAgent: Failed to register with TestAgency", ex );
                return false;
			}

            return true;
		}

        [System.Runtime.Remoting.Messaging.OneWay]
        public override void Stop()
		{
			log.Info( "Stopping" );
            // This causes an error in the client because the agent 
            // database is not thread-safe.
            //if ( agency != null )
            //    agency.ReportStatus(this.ProcessId, AgentStatus.Stopping);


            stopSignal.Set();
		}

		public void WaitForStop()
		{
            stopSignal.WaitOne();
		}
		#endregion

        #region Nested AgentRunner class
        class AgentRunner : ProxyTestRunner
        {
            private ITestRunnerFactory factory;

            public AgentRunner(int runnerID)
                : base(runnerID) 
            {
                this.factory = new InProcessTestRunnerFactoryArxNet();
            }

            public override bool Load(TestPackage package)
            {
                this.TestRunner = factory.MakeTestRunner(package);
                
                return base.Load(package);
            }
			
			public override IList AssemblyInfo 
			{
				get 
				{
					IList result = base.AssemblyInfo;
					string name = Path.GetFileName(Assembly.GetEntryAssembly().Location);
					
					foreach( TestAssemblyInfo info in result )
						info.ModuleName = name;
					
					return result;
				}
			}
        }
        #endregion
    }
}
