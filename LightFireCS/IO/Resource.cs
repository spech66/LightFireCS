//-----------------------------------------------------------------------------
//  Resource.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Resource interface -
//-----------------------------------------------------------------------------
using System;

namespace LightFireCS.IO
{
	public abstract class Resource
	{
		public abstract bool Load(string fileName);
	}
}