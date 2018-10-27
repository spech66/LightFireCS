//-----------------------------------------------------------------------------
//  Button.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Button controls -
//-----------------------------------------------------------------------------
using System;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public class Button : Window
	{
		public Button(Window parent, Rect rect, string title, StyleFactory factory, string style):
					base(rect, title, factory, style)
		{
			parent.RegisterChild(((Window)this));
			
			if(null == font)
			{
				font = parent.WindowFont;
				fontColor = parent.FontColor;
			}
			
			rect.X1 += parent.Size.X1;
			rect.Y1 += parent.Size.Y1;
			rect.X2 += parent.Size.X1;
			rect.Y2 += parent.Size.Y1;
		}
		
		public Button(Window parent, Rect rect, string title, StyleFactory factory): this(parent, rect, title, factory, "button") {}
	}
}