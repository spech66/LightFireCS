//-----------------------------------------------------------------------------
//  HtmlFile.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Log messages to HTML files -
//-----------------------------------------------------------------------------
using System;
using System.IO;

namespace LightFireCS.Log
{
	/// <summary>
	/// Summary description for Text.
	/// </summary>
	public class HtmlFile : ILog
	{
		private string fileName;
			
		public HtmlFile(string file)
		{
			if("" == file)
				file = "logfile.htm";

			fileName = file;
			StreamWriter Save = new StreamWriter(fileName, false);
			Save.WriteLine("<html><head>");
			Save.WriteLine("<style type=\"text/css\"><!--");
			Save.WriteLine("body {background-color: #ffffff; color: #000000;}");
			Save.WriteLine("table {border-collapse: collapse;}");
			Save.WriteLine("td, th { border: 1px solid #000000; font-size: 75%; vertical-align: baseline;}");
			Save.WriteLine(".h {background-color: #99E5FF; font-weight: bold;}");
			Save.WriteLine(".high {background-color: #C04618; color: #FFFFFF; font-weight: bold; }");
			Save.WriteLine(".med {background-color: #EDAC00; }");
			Save.WriteLine(".low {background-color: #008000;color: #FFFFFF; }");
			Save.WriteLine(".info {background-color: #dddddd; }");
			Save.WriteLine("//--></style></head><body><center>");
			Save.WriteLine("<table border=\"0\" cellpadding=\"3\" width=\"80%\">");
			Save.Close();
		}

		~HtmlFile()
		{
			StreamWriter Save = new StreamWriter(fileName, true);
			Save.WriteLine("</table></center></body></html>");
			Save.Close();
		}

		public void Text(string message, string category)
		{
			Write("info", "", message, category);
		}
		
		public void Info(string message, string category)
		{
			Write("low", "Info", message, category);
		}
		
		public void Warn(string message, string category)
		{
			Write("med", "Warning", message, category);
		}
		
		public void Error(string message, string category)
		{
			Write("high", "Error", message, category);
		}

		public void Write(string format, string type, string message, string category)
		{
			StreamWriter Save = new StreamWriter(fileName, true);
			Save.WriteLine("<tr>");
			Save.WriteLine("<td class=\"" + format + "\">" + System.DateTime.Now.ToString("HH:mm:ss") + "</td>");
			Save.WriteLine("<td class=\"" + format + "\">" + type + "</td>");
			Save.WriteLine("<td class=\"" + format + "\">" + category + "</td>");
			Save.WriteLine("<td class=\"" + format + "\">" + message + "</td>");
			Save.WriteLine("</tr>");

			Save.WriteLine();
			Save.Close();
		}
	}
}
