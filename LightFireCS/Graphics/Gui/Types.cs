//-----------------------------------------------------------------------------
//  Types.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Basic types needed by the GUI -
//-----------------------------------------------------------------------------
using System;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public enum Gradient
	{
		NONE,
		HORIZONTAL,
		VERTICAL,
		TBEAM
	}

	public class Rect
	{
		private double posX1, posY1;
		private double posX2, posY2;

		public double X1 { get { return posX1; } set { posX1 = value; } }
		public double Y1 { get { return posY1; } set { posY1 = value; } }
		public double X2 { get { return posX2; } set { posX2 = value; } }
		public double Y2 { get { return posY2; } set { posY2 = value; } }
		public double Width { get { return posX2 - posX1; } set { posX2 = posX1 + value; } }
		public double Height { get { return posY2 - posY1; } set { posY2 = posY1 + value; } }

		public Rect(double x, double y, double x2, double y2)
		{
			posX1 = x;
			posY1 = y;
			posX2 = x2;
			posY2 = y2;
		}
		
		public Rect(Rect r)
		{
			posX1 = r.X1;
			posY1 = r.Y1;
			posY2 = r.X2;
			posY2 = r.Y2;
		}
		
		public void Shrink(double s)
		{
			posX1 += s;
			posY1 += s;
			posX2 -= s;
			posY2 -= s;
		}
	}

	public class Color
	{
		public byte r, g, b, a;

		public Color()
		{
			this.r = 255;
			this.g = 255;
			this.b = 255;
			this.a = 255;
		}

		public Color(byte r, byte g, byte b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		public Color(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		
		public Color(string color)
		{
			r = 255;
			g = 255;
			b = 255;
			a = 255;
			
			if("" == color)
				return;
			
			try
			{
				string[] colors = color.Split(' ');
				if(colors.Length >= 3)
				{
					r = Convert.ToByte(colors[0]);
					g = Convert.ToByte(colors[1]);
					b = Convert.ToByte(colors[2]);
					if(colors.Length == 4)
						this.a = Convert.ToByte(colors[3]);
				}
			} catch {
				EngineLog.Get().Error("Error converting "+color+" to Color", "Color");	
			}
		}
	}
}
