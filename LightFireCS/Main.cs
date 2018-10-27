// <copyright file="Main.cs" company="Sebastian Pech">
// Copyright (C) Sebastian Pech
// </copyright>
//-----------------------------------------------------------------------------
// LightFire# - A free Game Engine
//
// Copyright (C) 2004-2009 Sebastian Pech
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 2.1 of the License, or any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//-----------------------------------------------------------------------------
using System;

[assembly: CLSCompliant(true)]
namespace LightFireCS
{
	public delegate void KeyEventHandler(object sender, KeyEventArgs fe);

	public class KeyEventArgs : System.EventArgs
	{
		public int keyIndex;
		public string input;
		
		public KeyEventArgs(int keyIndex, string input)
		{
			this.keyIndex = keyIndex;
			this.input = input;
		}
	}
}
