/*
 * Created by SharpDevelop.
 * User: rubiojr
 * Date: 14/02/2005
 * Time: 13:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;

namespace Vim.Components.Commands
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class TestCommand : ICommand
	{
		
		private ArrayList aliases;
		public TestCommand (){
		}
	
		public string Name {
			get {
				return "TestCommand";
			}
		}
		
		public ArrayList Aliases {
			get {
				return aliases;
			}
		}
		
		public void Execute (object[] parameters)
		{
			
		}
	}
}
