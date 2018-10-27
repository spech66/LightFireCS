//-----------------------------------------------------------------------------
//  Device.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - The graphic device initialises SDL and OpenGL and provides basic config -
//-----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using Tao.Sdl;
using LightFireCS.Log;
using LightFireCS.Math;

namespace LightFireCS.Graphics
{
	/// <summary>
	/// Summary description for Device.
	/// </summary>
	public class GDevice
	{
		private static GDevice instance = null;
		private bool isRunning;
		private int width, height, colorDepth;
		private bool fullscreen;
		private bool isOrtho;
		private int videoFlags;
		private IntPtr sdlSurface;
		private int frameTime;
		private int deltaTime;
		private int fpsCurrent;
		private int fpsCount;
		private int fpsTime;

		public int DeltaTime { get { return deltaTime; } }
		public int Fps { get { return fpsCurrent; } }
		public bool IsRunning { get { return isRunning; } }

		//! EventHandler
		public event EventHandler Resize;

		private GDevice()
		{
		}
		
		public static GDevice Get()
		{
			if(null == instance)
				instance = new GDevice();

			return instance;
		}

		public int CreateDevice(int width, int height, int colorDepth, bool fullscreen)
		{
			this.width = width;
			this.height = height;
			this.colorDepth = colorDepth;
			this.fullscreen = fullscreen;
            videoFlags = Sdl.SDL_OPENGL | Sdl.SDL_GL_DOUBLEBUFFER | Sdl.SDL_RESIZABLE | Sdl.SDL_HWPALETTE;

            if(Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO) < 0)
            {
                EngineLog.Get().Error("Error init SDL: " + Sdl.SDL_GetError(), "Graphics");
                Sdl.SDL_Quit();
                return -1;
            }
            
			if(-1 == SdlTtf.TTF_Init())
			{
				EngineLog.Get().Error("Error init SDL TTF: " + SdlTtf.TTF_GetError(), "Graphics");
                return -1;
			} else {
				EngineLog.Get().Info("SDL TTF initialized", "Graphics");
			}			

			if(this.fullscreen)
			{
				videoFlags |= Sdl.SDL_FULLSCREEN | Sdl.SDL_ANYFORMAT;
				EngineLog.Get().Info("Switching to Fullscreen", "Graphics");
			} else {
				EngineLog.Get().Info("Switching to Window", "Graphics");
			}

			IntPtr videoInfoPtr = Sdl.SDL_GetVideoInfo();
			if(IntPtr.Zero == videoInfoPtr)
			{
				EngineLog.Get().Error("GetVideoInfo failed", "Graphics");
				return 1;
			}

			Sdl.SDL_VideoInfo videoInfo = (Sdl.SDL_VideoInfo)Marshal.PtrToStructure(
                                            videoInfoPtr, typeof(Sdl.SDL_VideoInfo));

			// This doesn't work
			/*if(1 == videoInfo.hw_available)
			{
				EngineLog.Get().Info("Hardware Surface", "Graphics");
				videoFlags |= Sdl.SDL_HWSURFACE;
			} else 	{
				EngineLog.Get().Warn("Software Surface", "Graphics");
				videoFlags |= Sdl.SDL_SWSURFACE;
			}*/
			videoFlags |= Sdl.SDL_HWSURFACE;

			if(1 == videoInfo.blit_hw)
			{
				EngineLog.Get().Info("Hardware Blitting", "Graphics");
				videoFlags |= Sdl.SDL_HWACCEL;
			} else 	{
				EngineLog.Get().Warn("No Hardware Blitting", "Graphics");
			}

            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_RED_SIZE, 5);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_GREEN_SIZE, 5);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_BLUE_SIZE, 5);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DEPTH_SIZE, 16);
            Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);

			sdlSurface = Sdl.SDL_SetVideoMode(width, height, colorDepth, videoFlags);
			if(0 == sdlSurface.ToInt32())
			{
				EngineLog.Get().Error("Set video mode: " + Sdl.SDL_GetError(), "Graphics");
				return 1;
			} else {
				EngineLog.Get().Info("Set video mode to " + width + "x" + height + "x" + colorDepth, "Graphics");
			}

			Gl.glShadeModel(Gl.GL_SMOOTH);
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

			Gl.glClearDepth(1.0f);
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glDepthFunc(Gl.GL_LEQUAL);

			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			Gl.glEnable(Gl.GL_TEXTURE_2D);

			Gl.glEnable(Gl.GL_CULL_FACE);
			Gl.glCullFace(Gl.GL_BACK);

			string glVendor = Gl.glGetString(Gl.GL_VENDOR);
			string glVersion = Gl.glGetString(Gl.GL_VERSION);
			string glVideoCard = Gl.glGetString(Gl.GL_RENDERER);
			string glExt = Gl.glGetString(Gl.GL_EXTENSIONS);
			EngineLog.Get().Info("OpenGL Vendor: " + glVendor, "Graphics");
			EngineLog.Get().Info("OpenGL Version " + glVersion, "Graphics");
			EngineLog.Get().Info("OpenGL Renderer: " + glVideoCard, "Graphics");
			EngineLog.Get().Info("OpenGL Extensions: " + glExt, "Graphics");
			
			if(!Gl.IsExtensionSupported("GL_ARB_multitexture"))
			{
				EngineLog.Get().Error("Extension GL_ARB_multitexture not supported!", "Graphics");
				return 1;
			}

			// New TAO Framwork loads all extensions on startup
			/*if(false == GlExtensionLoader.LoadExtension("GL_ARB_multitexture"))
			{
				EngineLog.Get().Error("Load OpenGL Extension: GL_ARB_multitexture", "Graphics");
				return 1;
			}*/

			isRunning = true;
			isOrtho = false;
			fpsTime	= Sdl.SDL_GetTicks();
			frameTime = Sdl.SDL_GetTicks();

			ResizeWindow(width, height);

			return 0;
		}

		public void ProcessSdlEvent(Sdl.SDL_Event sdlEvent)
		{
			switch(sdlEvent.type)
			{
				case Sdl.SDL_QUIT:
				{
					isRunning = false;
				} break;
				case Sdl.SDL_VIDEORESIZE:
				{
					ResizeWindow(sdlEvent.resize.w, sdlEvent.resize.h);
					TextureManager.Get().ReloadAll();
					if(null != Resize)
						Resize(this, EventArgs.Empty);
				} break;
			}
		}

		public void BeginRender()
		{
			deltaTime = Sdl.SDL_GetTicks() - frameTime;
			frameTime = Sdl.SDL_GetTicks();
			
			TextureManager.Get().SetTexture();
			
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
		}

		public void EndRender()
		{
			Gl.glFlush();
			Sdl.SDL_GL_SwapBuffers();

			if((Sdl.SDL_GetTicks() - fpsTime) >= 1000)
			{
				fpsTime	= Sdl.SDL_GetTicks();
				fpsCurrent = fpsCount;
				fpsCount = 0;
			}
			fpsCount++;
		}

		public void ResizeWindow(int width, int height)
		{
			this.width = width;
			this.height = height;
			Sdl.SDL_SetVideoMode(width, height, colorDepth, videoFlags);
			Gl.glEnable(Gl.GL_TEXTURE_2D);
	
			if(isOrtho)
				SetOrthoView();
			else
				SetPerspectiveView();
		}

		public void SetPerspectiveView()
		{
			float Ratio = width / height;
			Gl.glViewport(0, 0, width, height);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();

			Glu.gluPerspective(45.0f, Ratio, 0.1f, 4096.0f);
			isOrtho = false;

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			
			Gl.glEnable(Gl.GL_DEPTH_TEST);
		}

		public void SetOrthoView()
		{
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();

			Glu.gluOrtho2D(0, width, height, 0);
			isOrtho = true;

			Gl.glMatrixMode(Gl.GL_MODELVIEW);								
			Gl.glLoadIdentity();

			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			
			Gl.glDisable(Gl.GL_DEPTH_TEST);
		}

		public void SetWindowText(string text)
		{
			Sdl.SDL_WM_SetCaption(text, "");
		}
		
		public void Quit()
		{
			isRunning = false;
		}
		
		public void RayFromPoint(int mouseX, int mouseY, out Vector3 pos, out Vector3 dir)
		{
			double[] modelview = new double[16];
			double[] projection = new double[16];
			int[] viewport = new int[4];
			float winX, winY;
			double posX, posY, posZ;
			double posX2, posY2, posZ2;
			
			Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
			Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
			Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
			
			winX = mouseX;
			winY = viewport[3] - mouseY - 1;
			Glu.gluUnProject(winX, winY, 0.0, modelview, projection, viewport, out posX, out posY, out posZ);		
			Glu.gluUnProject(winX, winY, 1.0, modelview, projection, viewport, out posX2, out posY2, out posZ2);
			
			pos = new Vector3(posX, posY, posZ);
			Vector3 pos2 = new Vector3(posX2, posY2, posZ2);
			dir = new Vector3(pos2 - pos);
			dir.Normalize();
		}
	}
}
