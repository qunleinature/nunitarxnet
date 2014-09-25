// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.25修改：
//      1.在nunit2.6.3基础上修改
//      2.改NUnit.Util.ArxNet.TestServerArxNet
//      3.改TestDomainArxNet
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
