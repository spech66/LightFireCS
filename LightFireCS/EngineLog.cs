//-----------------------------------------------------------------------------
//  EngineLog.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Singleton log -
//-----------------------------------------------------------------------------
using System;
using LightFireCS.Log;

namespace LightFireCS
{
	/// <summary>
	/// Summary description for EngineLog3.
	/// </summary>
	class EngineLog : ILog
	{
		private static EngineLog instance;
		private HtmlFile htmlFile;
		private TextFile textFile;
		
		private EngineLog()
		{
			htmlFile = new HtmlFile("lfcs.htm");
			textFile = new TextFile("lfcs.txt");
		}

		public static EngineLog Get()
		{
			if(null == instance)
				instance = new EngineLog();

			return instance;
		}
		
		public void Text(string message, string category)
		{
			htmlFile.Text(message, category);
			textFile.Text(message, category);
		}
		
		public void Info(string message, string category)
		{
			htmlFile.Info(message, category);
			textFile.Info(message, category);
		}
		
		public void Warn(string message, string category)
		{
			htmlFile.Warn(message, category);
			textFile.Warn(message, category);
		}
		
		public void Error(string message, string category)
		{
			htmlFile.Error(message, category);
			textFile.Error(message, category);
		}
	}
}
