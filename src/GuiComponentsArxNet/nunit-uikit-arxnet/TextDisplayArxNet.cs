// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.31修改：
//      1.在nunit2.6.2基础上修改
//      2.NUnit.UiKit.TextDisplay改为NUnit.UiKit.ArxNet.TextDisplayArxNet接口
//      3.改TextDisplayContent为TextDisplayContentArxNet
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using NUnit.Util;
using NUnit.Core;

namespace NUnit.UiKit.ArxNet
{
	/// <summary>
	/// The TextDisplay interface is implemented by object - generally
	/// controls - that display text.
	/// </summary>
	public interface TextDisplayArxNet : TestObserver
	{
		/// <summary>
		///  The output types handled by this display
		/// </summary>
		TextDisplayContentArxNet Content { get; set; }

		/// <summary>
		/// Clears the display
		/// </summary>
		void Clear();

		/// <summary>
		/// Appends text to the display
		/// </summary>
		/// <param name="text">The text to append</param>
		void Write( string text );

		/// <summary>
		/// Appends text to the display followed by a newline
		/// </summary>
		/// <param name="text">The text to append</param>
		void WriteLine( string text );

		void Write( NUnit.Core.TestOutput output );

		/// <summary>
		/// Gets the current text - used mainly for testing
		/// </summary>
		string GetText();
	}
}
