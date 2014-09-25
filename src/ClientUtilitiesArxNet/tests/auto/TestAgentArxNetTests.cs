// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.25
//      1.��NUnit2.6.3����
//      2.��NUnit.Util.ArxNet.Tests.RemoteTestAgentArxNetTests
//      3.��RemoteTestAgentArxnet
// ****************************************************************

using System;
using System.Diagnostics;
using NUnit.Framework;

namespace NUnit.Util.ArxNet.Tests
{
    [TestFixture]
    public class RemoteTestAgentArxNetTests
    {
        [Test]
        public void AgentReturnsProcessId()
        {
            RemoteTestAgentArxnet agent = new RemoteTestAgentArxnet(Guid.NewGuid(), null);
            Assert.AreEqual(Process.GetCurrentProcess().Id, agent.ProcessId);
        }
    }
}