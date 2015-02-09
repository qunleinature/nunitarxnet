// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun 
//  2012.12.23修改:
//      private void InvokeHandler( MulticastDelegate handlerList, EventArgs e )中
//      control.Invoke( handler, args );死锁,改为 
//      control.BeginInvoke(handler, args);
//  2013.6.8
//      1.改在nunit2.6.2基础
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;

using NUnit.Util;
using NUnit.Core;
using NUnit.UiKit;
using NUnit.Util.ArxNet;

namespace NUnit.UiKit.ArxNet
{
	[Serializable]
	public class TestEventInvocationException : Exception
	{
		public TestEventInvocationException( Exception inner )
			: base( "Exception invoking TestEvent handler", inner )
		{
		}
	}	

	/// <summary>
	/// Summary description for GuiTestEventDispatcher.
	/// </summary>
	public class GuiTestEventDispatcherArxNet : TestEventDispatcher
	{
		protected override void Fire(TestEventHandler handler, TestEventArgs e)
		{
			if ( handler != null )
				InvokeHandler( handler, e );
		}

		private void InvokeHandler( MulticastDelegate handlerList, EventArgs e )
		{
			object[] args = new object[] { this, e };
			foreach( Delegate handler in handlerList.GetInvocationList() )
			{
				object target = handler.Target;
				System.Windows.Forms.Control control 
					= target as System.Windows.Forms.Control;
				try 
				{
					if ( control != null && control.InvokeRequired )
						//control.Invoke( handler, args );
                        control.BeginInvoke(handler, args);
					else
						handler.Method.Invoke( target, args );
				}
				catch( Exception ex )
				{
					// TODO: Stop rethrowing this since it goes back to the
					// Test domain which may not know how to handle it!!!
					Console.WriteLine( "Exception:" );
					Console.WriteLine( ex );
					//throw new TestEventInvocationException( ex );
					//throw;
				}
			}
		}

	}
}

