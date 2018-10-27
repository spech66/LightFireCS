//-----------------------------------------------------------------------------
//  StyleFactory.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Handle Style Files for the GUI -
//-----------------------------------------------------------------------------
using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Specialized;

namespace LightFireCS.Graphics.Gui
{
	public class StyleFactory
	{
		private NameValueCollection styles = new NameValueCollection();
	
		public StyleFactory(string styleFile)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(styleFile);
			
			XmlNodeList xnl;
			xnl = doc.SelectNodes("styles/style");
			
			foreach(XmlNode node in xnl)
			{
				string nodename = node.Attributes.GetNamedItem("name").Value;
				
				foreach(XmlNode subnode in node.ChildNodes)
				{
					string subnodename = nodename + "@" + subnode.Name;
					
					XmlAttributeCollection col = subnode.Attributes;
					foreach(XmlAttribute attr in col)
					{
						styles.Add(subnodename + "@" + attr.Name, attr.Value);
					}
				}
			}
		}
				
		public string GetValue(string style, string node, string attribute)
		{
			string s = styles[style + "@" + node + "@" + attribute];
			if(null != s)
			{
				return s;
			} else {
				//Console.WriteLine("Key \"{0} {1} {2}\" not found", style, node, attribute);
				return "";
			}
		}
	}
}