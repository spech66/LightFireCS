//-----------------------------------------------------------------------------
//  Box.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Renders a quad with texture coordinates -
//-----------------------------------------------------------------------------
using System;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public class Box
	{
		private Rect rect;
		private Color color;
		private Color color2;
		private string texture;
		private string[] texturescale = new string[9];
		private bool scalemode;
		private Gradient gradient;
		
		public Rect Position
		{
			set { rect = value; }
			get { return rect; }
		}
		
		public Color Color1
		{
			set { color = value; }
			get { return color; }
		}
		
		public Color Color2
		{
			set { color2 = value; }
			get { return color2; }
		}

		public Box(Rect rect, Color color, string texture)
		{
			Position = rect;
			Color1 = color;
			SetTexture(texture);
		}
		
		public Box(Rect rect, Color color)
		{
			Position = rect;
			Color1 = color;
			SetTexture("");
		}
		
		public Box(Rect rect, string texture)
		{
			Position = rect;
			Color1 = new Color();
			SetTexture(texture);
		}
		
		public void SetTexture(string texture)
		{
			if("" == texture)
			{
				this.texture = "";
				return;
			}

			if(-1 != texture.IndexOf('*'))
			{
				scalemode = true;
				texturescale[0] = texture.Replace("*", "tl");
				texturescale[1] = texture.Replace("*", "t");
				texturescale[2] = texture.Replace("*", "tr");
				texturescale[3] = texture.Replace("*", "l");
				texturescale[4] = texture.Replace("*", "m");
				texturescale[5] = texture.Replace("*", "r");
				texturescale[6] = texture.Replace("*", "bl");
				texturescale[7] = texture.Replace("*", "b");
				texturescale[8] = texture.Replace("*", "br");
				
				for(int i = 0; i < 9; i++)
					TextureManager.Get().LoadTextureFromFile(texturescale[i]);
			} else {
				scalemode = false;
				this.texture = texture;
				TextureManager.Get().LoadTextureFromFile(texture);
			}
		}
		
		public void SetGradient(Gradient gradient, Color color)
		{
			this.gradient = gradient;
			this.color2 = color;
		}
		
		public void Render()
		{
			if(scalemode)
			{
				Gl.glColor4ub(color.r, color.g, color.b, color.a);
				
				Texture tex = TextureManager.Get().GetTexture(texturescale[0]);
				int h = tex.height;
				int w = tex.width;
				
				//Top
				TextureManager.Get().SetTexture(texturescale[0]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[1]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[2]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y1+h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1);
				Gl.glEnd();

				//Mid
				TextureManager.Get().SetTexture(texturescale[3]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1+h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1+h);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[4]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y1+h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1+h);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[5]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y1+h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2-h);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1+h);
				Gl.glEnd();

				//Bottom
				TextureManager.Get().SetTexture(texturescale[6]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y2-h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2-h);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[7]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2-h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1+w, rect.Y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2-h);
				Gl.glEnd();
				
				TextureManager.Get().SetTexture(texturescale[8]);
				Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2-h);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X2-w, rect.Y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y2-h);
				Gl.glEnd();				
			} else {
				if("" != texture)
					TextureManager.Get().SetTexture(texture);

				Gl.glBegin(Gl.GL_QUADS);
				
				if(gradient == Gradient.HORIZONTAL)
				{
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2);
					Gl.glColor4ub(color2.r, color2.g, color2.b, color2.a);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1);
				} else if(gradient == Gradient.VERTICAL) {
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1);
					Gl.glColor4ub(color2.r, color2.g, color2.b, color2.a);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2);
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1);			
				} else if(gradient == Gradient.TBEAM) {
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1);
					Gl.glColor4ub(color2.r, color2.g, color2.b, color2.a);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2);
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2);
					Gl.glColor4ub(color2.r, color2.g, color2.b, color2.a);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1);			
				} else {
					Gl.glColor4ub(color.r, color.g, color.b, color.a);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(rect.X1, rect.Y1);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(rect.X1, rect.Y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(rect.X2, rect.Y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(rect.X2, rect.Y1);			
				}
				
				Gl.glColor4ub(255, 255, 255, 255);
				
				Gl.glEnd();
			}
		}
	}
}
