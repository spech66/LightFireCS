//-----------------------------------------------------------------------------
//  ProgressBar.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Progress bar control -
//-----------------------------------------------------------------------------
using System;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public class ProgressBar : Window
	{
		private double valueMin = 0;
		private double valueMax = 100;
		private double valueCurrent = 100;
		private bool showText = true;
		
		public double Value
		{
			get { return valueCurrent; }
			set
			{
				if(value >= valueMin && value <= valueMax)
					valueCurrent = value;
				else
					valueCurrent = valueMax;
			}
		}

		public bool ShowText { get { return showText; } set { showText = value; } }

		public ProgressBar(Window parent, Rect rect, string title, StyleFactory factory, string style):
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
		
		public ProgressBar(Window parent, Rect rect, string title, StyleFactory factory): this(parent, rect, title, factory, "progressBar") {}

		public void SetBorder(double vMin, double vMax)
		{
			if(vMin > vMax)
				return;

			valueMin = vMin;
			valueMax = vMax;
		}

		public override void Render()
		{
			if(!Visible)
				return;

			double x1 = background.Position.X1;
			double x2 = background.Position.X2;
			double cX = (valueCurrent-valueMin)/(valueMax-valueMin)*(x2-x1) + x1;

			if(Disabled && !Hovered)
			{
				backgroundDisabled.Position.X2 = cX; 
				backgroundDisabled.Render();
				backgroundDisabled.Position.X2 = x2;
			} else if(Hovered) {
				backgroundHover.Position.X2 = cX; 
				backgroundHover.Render();
				backgroundHover.Position.X2 = x2;
			} else {
				background.Position.X2 = cX; 
				background.Render();
				background.Position.X2 = x2;
			}

			if(font != null && showText)
			{
				double perc = (valueCurrent-valueMin)/(valueMax-valueMin) * 100;
				if(title != "")
					font.Render(title + " " + perc + "%", fontColor, background.Position, FontVAlign.Center, FontHAlign.Middle);
				else
					font.Render(perc + "%", fontColor, background.Position, FontVAlign.Center, FontHAlign.Middle);
			}
			
			foreach(Window w in childs)
			{
				w.Render();
			}
		}
	}
}