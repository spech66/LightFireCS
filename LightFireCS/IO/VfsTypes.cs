//-----------------------------------------------------------------------------
//  VfsTypes.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Classes and structs needed by the Vfs -
//-----------------------------------------------------------------------------
using System;
using System.IO;

namespace LightFireCS.IO
{
	class VfsHeader
	{
		protected char[] header; //LFCSP
		protected char[] version; //X.XXX
		public int files;
		
		public int Read(BinaryReader binReader)
		{
			try
			{
				header = binReader.ReadChars(5);
				version = binReader.ReadChars(4);
				files = binReader.ReadInt32();
			} catch(EndOfStreamException e) {
				Console.WriteLine(e.StackTrace);
				return -1;
			}
			
			return 0;
		}
		
		public void ConsoleWrite()
		{
			Console.WriteLine("Header: " + header);
			Console.WriteLine("Version: {0}.{1}{2}{3}" + version[0], version[1], version[2], version[3]);
			Console.WriteLine("Files: {0}", files);
		}
	}
	
	public class VfsReaderFile
	{
		public long offset;
		public long lenght;
		public string file;
	};	
}