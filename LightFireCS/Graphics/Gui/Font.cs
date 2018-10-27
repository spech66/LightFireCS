//-----------------------------------------------------------------------------
//  FontManager.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  -  -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using Tao.Sdl;

namespace LightFireCS.Graphics.Gui
{
	public enum FontVAlign
	{
		Left,
		Center,
		Right
	}
	
	public enum FontHAlign
	{
		Top,
		Middle,
		Bottom
	}

	public class Font
	{
		private IntPtr font;
		private Rect size;
		
		public Rect Size { get { return new Rect(size); } }
		
		public Font()
		{
			size = new Rect(0, 0, 0, 0);
		}
		
		public int LoadFont(string ttfFile, int size)
		{
			font = SdlTtf.TTF_OpenFont(ttfFile, size);
			
			if(IntPtr.Zero == font)
			{
				EngineLog.Get().Error("Error loading " + ttfFile + ": " + SdlTtf.TTF_GetError(), "Font");
				return 1;
			}
			
			EngineLog.Get().Info(ttfFile+" loaded", "Font");
			return 0;
		}
		
		public int SizeText(string text)
		{
			int w, h;
			SdlTtf.TTF_SizeUNICODE(font, text, out w, out h);
			return w;
		}
		
		public void Render(string text, Color color, Rect rect)
		{
			Render(text, color, rect, FontVAlign.Left, FontHAlign.Top);
		}
		
		public void Render(string text, Color color, Rect rect, FontVAlign vAlign, FontHAlign hAlign)
		{
			if(text == "")
				return;
		
			Sdl.SDL_Color sdlColor = new Sdl.SDL_Color();
			sdlColor.r = color.r;
			sdlColor.g = color.g;
			sdlColor.b = color.b;
			
			int w, h;			

			IntPtr surfaceP = SdlTtf.TTF_RenderText_Solid(font, text, sdlColor);

			Sdl.SDL_Surface surface = (Sdl.SDL_Surface)Marshal.PtrToStructure(surfaceP, typeof(Sdl.SDL_Surface));
			
			double wT = System.Math.Pow(2, System.Math.Ceiling(System.Math.Log(surface.w) / System.Math.Log(2)));
			w = (int)(wT + 0.5);
			double hT = System.Math.Pow(2, System.Math.Ceiling(System.Math.Log(surface.h) / System.Math.Log(2)));
			h = (int)(hT + 0.5);

			IntPtr surfaceP2 = Sdl.SDL_CreateRGBSurface(0, w, h, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, unchecked((int)0xff000000));
			Sdl.SDL_Surface surface2 = (Sdl.SDL_Surface)Marshal.PtrToStructure(surfaceP2, typeof(Sdl.SDL_Surface));

			Sdl.SDL_BlitSurface(surfaceP, ref surface.clip_rect, surfaceP2, ref surface2.clip_rect);
			Sdl.SDL_Surface surface3 = (Sdl.SDL_Surface)Marshal.PtrToStructure(surfaceP2, typeof(Sdl.SDL_Surface));

			int texture = 0;
			//Gl.glGenTextures(1, new int[] { texture });
			int[] textureAr = new int[1];
			Gl.glGenTextures(1, textureAr);
			texture = textureAr[0];

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
			Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA, w, h, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, surface3.pixels);

			Gl.glColor3ub(255, 255, 255);

			double x1, x2;
			double y1, y2;

			if(vAlign == FontVAlign.Left)
			{
				x1 = rect.X1;
				x2 = rect.X1+w;			
			} else if(vAlign == FontVAlign.Center) {
				double rX = (rect.X2 - rect.X1)/2 + rect.X1;
				double wMod = (surface3.w - surface.w)/2;
				x1 = rX - surface3.w/2 + wMod;
				x2 = rX + surface3.w/2 + wMod;
			} else /* Right */ {
				x1 = rect.X2-w;
				x2 = rect.X1;
			}

			if(hAlign == FontHAlign.Top)
			{
				y1 = rect.Y1;
				y2 = rect.Y1+h;
			} else if(hAlign == FontHAlign.Middle) {
				double rY = (rect.Y2 - rect.Y1)/2 + rect.Y1;
				double hMod = (surface3.h - surface.h)/2;
				y1 = rY - surface3.h/2 + hMod;
				y2 = rY + surface3.h/2 + hMod;
			} else /* Bottom */{
				y1 = rect.Y2-h;
				y2 = rect.Y2;
			}
			
			Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0.0f, 0.0f);	Gl.glVertex2d(x1, y1);
					Gl.glTexCoord2f(0.0f, 1.0f);	Gl.glVertex2d(x1, y2);
					Gl.glTexCoord2f(1.0f, 1.0f);	Gl.glVertex2d(x2, y2);
					Gl.glTexCoord2f(1.0f, 0.0f);	Gl.glVertex2d(x2, y1);
			Gl.glEnd();
			
			size.X1 = surface.w;
			size.Y1 = surface.h;
			
			Sdl.SDL_FreeSurface(surfaceP);
			Sdl.SDL_FreeSurface(surfaceP2);
			Gl.glDeleteTextures(1, new int[] { texture });
			TextureManager.Get().SetTexture("");
		}
	}
}
