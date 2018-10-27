//-----------------------------------------------------------------------------
//  Device.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Handles mouse and keyboard -
//-----------------------------------------------------------------------------
using System;
using Tao.Sdl;
using LightFireCS.Log;

namespace LightFireCS.Input
{
	/// <summary>
	/// Handle mouse and keyboard.
	/// </summary>
	public class IDevice
	{
		private static IDevice instance;
		//! Contains the state of every key on the keyboard
		private bool[]			keyboard = new bool[323];
		//! The x and y coordinates of the mouse
		private int				mouseX, mouseY;
		//! Contains the state of every mouse button
		private bool[]			mouseB = new bool[8];
		
		//! EventHandler
		public event EventHandler MouseMove;
		public event EventHandler MouseLeftDown;
		public event KeyEventHandler KeyDown;

		private IDevice()
		{
			for(int keys = 0; keys < 323; keys++)
				keyboard[keys] = false;

			for(int buttons = 0; buttons < 8; buttons++)
				mouseB[buttons] = false;
		
			//Sdl.SDL_ShowCursor(0);
			//Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_ON);
			Sdl.SDL_EnableUNICODE(1);
		}
		
		public static IDevice Get()
		{
			if(null == instance)
				instance = new IDevice();

			return instance;
		}

		public void ProcessSdlEvent(Sdl.SDL_Event sdlEvent)
		{
			switch(sdlEvent.type)
			{
				case Sdl.SDL_KEYDOWN:
				{
					string textInput = "";
					
					switch(sdlEvent.key.keysym.sym)
					{
						case Sdl.SDLK_SPACE:
							textInput = " ";
							break;
					
						case Sdl.SDLK_TAB:
							textInput = "    ";
							break;
							
						case Sdl.SDLK_RETURN:
							textInput = "\n";
							break;
						
						case Sdl.SDLK_BACKSPACE:
							textInput = "\b";
							break;
					
						default:
						{
							if(sdlEvent.key.keysym.unicode > 0 && sdlEvent.key.keysym.unicode < 0x80)
							{
								textInput = Convert.ToString(Convert.ToChar(sdlEvent.key.keysym.unicode));
								if(sdlEvent.key.keysym.mod == Sdl.KMOD_ALT)
									textInput = textInput.ToUpper();
							}
						}	break;
					}

					keyboard[sdlEvent.key.keysym.sym] = true;
					if(null != KeyDown)
					{
						KeyEventArgs keyEvent = new KeyEventArgs(sdlEvent.key.keysym.sym, textInput);
						KeyDown(this, keyEvent);
					}
				} break;
			
				case Sdl.SDL_KEYUP:
				{
					keyboard[sdlEvent.key.keysym.sym] = false;
				} break;
		
				case Sdl.SDL_MOUSEMOTION:
				{
					mouseY = sdlEvent.motion.y;
					mouseX = sdlEvent.motion.x;
					if(null != MouseMove)
						MouseMove(this, EventArgs.Empty);
				} break;
		
				case Sdl.SDL_MOUSEBUTTONDOWN:
				{
					if(sdlEvent.button.button-1 < 8)
					{
						mouseB[sdlEvent.button.button-1] = true;
						if(0 == (sdlEvent.button.button-1) && null != MouseLeftDown)
							MouseLeftDown(this, EventArgs.Empty);
					}
				} break;
		
				case Sdl.SDL_MOUSEBUTTONUP:
				{
					if(sdlEvent.button.button-1 < 8)
						mouseB[sdlEvent.button.button-1] = false;
				} break;
			}
		}

		public bool GetKeyState(int key)
		{
			return keyboard[key];
		}

		public bool GetMouseButtonState(int button)
		{
			return mouseB[button];
		}

		public void GetMousePos(out int x, out int y)
		{
			Sdl.SDL_GetMouseState(out x, out y);
		}
	}
}
