//-----------------------------------------------------------------------------
//  log.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Interface for log -
//-----------------------------------------------------------------------------
using System;

namespace LightFireCS.Log
{
	/// <summary>
	/// Summary description for ILog.
	/// </summary>
	public interface ILog
	{
		void Text(string message, string category);
		void Info(string message, string category);
		void Warn(string message, string category);
		void Error(string message, string category);
	}
}
