//-----------------------------------------------------------------------------
//  Vector3.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - 3D Vector -
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Text;

namespace LightFireCS.Log
{
	/// <summary>
	/// Summary description for Text.
	/// </summary>
	public class TextFile : ILog
	{
		private string fileName;
			
		public TextFile(string file)
		{
			if("" == file)
				file = "logfile.txt";

			fileName = file;
			StreamWriter Save = new StreamWriter(fileName, false);
			Save.WriteLine("File created: " + System.DateTime.Now.ToString("F"));
			Save.Write(Save.NewLine);
			Save.Close();
		}

		~TextFile()
		{
			StreamWriter Save = new StreamWriter(fileName, true);
			Save.Write(Save.NewLine);
			Save.WriteLine("File closed: " + System.DateTime.Now.ToString("F"));
			Save.Close();
		}

		public void Text(string message, string category)
		{
			Write("", message, category);
		}
		
		public void Info(string message, string category)
		{
			Write("Info", message, category);
		}
		
		public void Warn(string message, string category)
		{
			Write("Warning", message, category);
		}
		
		public void Error(string message, string category)
		{
			Write("Error", message, category);
		}

		public void Write(string type, string message, string category)
		{
			StreamWriter Save = new StreamWriter(fileName, true);
			StringBuilder Builder = new StringBuilder();
			Builder.AppendFormat("{0,-10}", System.DateTime.Now.ToString("HH:mm:ss"));
			Builder.AppendFormat("{0,-10}", type);
			Builder.AppendFormat("{0,-20}", category);
			Builder.AppendFormat("{0,-60}", message);
			Save.WriteLine(Builder.ToString());
			Save.Close();
		}
	}
}
