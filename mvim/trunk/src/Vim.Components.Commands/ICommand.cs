/*
 * Created by SharpDevelop.
 * User: rubiojr
 * Date: 14/02/2005
 * Time: 12:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace Vim.Components.Commands
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public interface ICommand
	{
		void Execute (object[] parameters);
		
		ArrayList Aliases { get; }
		string Name { get; }
	}
}
