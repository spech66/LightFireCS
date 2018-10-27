//-----------------------------------------------------------------------------
//  NameGenerator.cs
//  Copyright (C) 2008 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Generate various (pseudo) unique names -
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace LightFireCS.Utilities
{
	public class NameGenerator
	{
		/// <summary>
		/// Generates a unique filename based on the <seealso cref="GenerateGGUID"/> function and adds
		/// a <paramref name="extension"/> to it.
		/// </summary>
		/// <param name="extension">The extension added to the filename</param>
		/// <returns>Unique file name</returns>
		public static string GenerateFileName(string extension)
		{
			if (extension != "")
				return GenerateGGUID() + "." + extension;
			else
				return GenerateGGUID();
		}

		/// <summary>
		/// Generate GGUID Version 4 as defined in http://www.ietf.org/rfc/rfc4122.txt
		/// </summary>
		/// <returns>Unique ID</returns>
		public static string GenerateGGUID()
		{
			Random rnd = new Random();
			StringBuilder sb = new StringBuilder();
			//%04x%04x-%04x-%03x4-%04x-%04x%04x%04x
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));
			sb.Append('-');
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));
			sb.Append('-');
			sb.Append(string.Format("{0:x3}", rnd.Next(0, 4095)));
			sb.Append("4-");
			string bin = Convert.ToString(rnd.Next(0, 65535), 2);
			bin.Remove(6, 2);
			bin.Insert(6, "01");
			sb.Append(string.Format("{0:x4}", Convert.ToInt32(bin, 2)));
			sb.Append('-');
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));
			sb.Append(string.Format("{0:x4}", rnd.Next(0, 65535)));

			return sb.ToString();
		}

		/*public static string GenerateName()//bitmask with flags e.g. [A-Z][a-z][0-9][filechars]...
		{
			return "";
		}*/
	}
}
