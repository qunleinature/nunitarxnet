// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// 2013.5.31�޸ģ�
//  1.��nunit2.6.2�������޸�
//  2.NUnit.UiKit.TextDisplay��ΪNUnit.UiKit.ArxNet.TextDisplayArxNet�ӿ�
//  3.��TextDisplayContentΪTextDisplayContentArxNet
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