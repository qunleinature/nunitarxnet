// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.25�޸ģ�
//      1.��nunit2.6.3�������޸�
//      2.��NUnit.Util.ArxNet.TestServerArxNet
//      3.��TestDomainArxNet
// ****************************************************************


using System;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Core;

namespace NUnit.Util.ArxNet
{
	/// <summary>
	/// Base class for servers
	/// </summary>
	public class TestServerArxNet : ServerBase
	{
		private TestRunner runner;

		public TestServerArxNet( string uri, int port ) : base( uri, port )
		{
			this.runner = new TestDomainArxNet();
		}

		public TestRunner TestRunner
		{
			get { return runner; }
		}
	}
}