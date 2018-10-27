// http://bandodalua.lua.inf.puc-rio.br/luanet/
using System;

public class ChessClient
{
	static void Main()
	{
		LightFireCS.Graphics.GDevice.Get().CreateDevice(640, 480, 32, false);
		LightFireCS.Graphics.GDevice.Get().SetWindowText("Chess#");
	
		MainMenu menu = new MainMenu();
				
		int mode = menu.Render();
		if(mode == 2)
		{
			LightFireCS.Graphics.GDevice.Get().Quit();
			return;
		}
		
		Board board = new Board();
		bool running = true;
		while(running)
		{
			running = board.Render();
		}
		
		LightFireCS.Graphics.GDevice.Get().Quit();
	}
}
