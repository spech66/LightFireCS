//-----------------------------------------------------------------------------
//  VfsReader.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Reads files from virtual file system -
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections;

namespace LightFireCS.IO
{
	public class VfsReader
	{
		private string fileName;
		private ArrayList list = new ArrayList();
		private VfsHeader header = new VfsHeader();
		private BinaryReader binReader;

		public int Open(string file)
		{
			fileName = file;
			binReader = new BinaryReader(File.Open(fileName, FileMode.Open));

			try
			{
				header.Read(binReader);
				header.ConsoleWrite();
				
				for(int i = 0; i < header.files; i++)
				{
					VfsReaderFile vfsFi = new VfsReaderFile();
					vfsFi.offset = binReader.ReadInt64();
					vfsFi.lenght = binReader.ReadInt32();
					vfsFi.file = binReader.ReadString();
					list.Add(vfsFi);
					
					Console.WriteLine("Dat: {0} {1} {2}", vfsFi.offset, vfsFi.lenght, vfsFi.file);
				}
			}
			catch(EndOfStreamException e)
			{
				Console.WriteLine(e.StackTrace);
				return -1;
			}
			finally
			{
				binReader.Close();
			}
			
			binReader.Close();
			return 0;
		}
		
		public byte[] GetFile(string file)
		{
			/*binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
			
			foreach(VfsReaderFile vfsFile in list)
			{
				if(vfsFile.file == file)
				{
					Console.WriteLine(binReader.BaseStream.Length);
					binReader.BaseStream.Position = vfsFile.offset;
					Console.WriteLine("len:" + vfsFile.lenght);
					Console.WriteLine("off:" + vfsFile.offset);
					byte[] dat = new byte[vfsFile.lenght];
					binReader.ReadB
					dat = binReader.ReadBytes(vfsFile.lenght);
					binReader.Close();
					return dat;
				}
			}
			binReader.Close();*/
			return null;
		}
	}
}
