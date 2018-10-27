using System;
using System.Collections.Generic;
using System.Text;
using Tao.Sdl;

namespace LightFireCS.Core
{
	public class MessageHandler
	{
		private static MessageHandler instance;

		private MessageHandler()
		{

		}

		public static MessageHandler Get()
		{
			if(null == instance)
				instance = new MessageHandler();

			return instance;
		}

		public void ProcessEvents()
		{
			Sdl.SDL_Event sdlEvent;
			while(0 != Sdl.SDL_PollEvent(out sdlEvent))
			{
				Graphics.GDevice.Get().ProcessSdlEvent(sdlEvent);
				Input.IDevice.Get().ProcessSdlEvent(sdlEvent);
			}
		}
	}
}
