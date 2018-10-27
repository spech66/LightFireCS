using System;
using LightFireCS.Graphics;
using LightFireCS.Graphics.Gui;
using LightFireCS.Math;
using Tao.OpenGl;

public enum GameState { GS_MENU, GS_GAME, GS_EDITOR }

public class MainClass
{
	static void Main(string[] args)
	{
		Main main = new Main();
		main.Loop();
	}
}

public class Main
{
	private GameState gameState;

	public Main()
	{
		GDevice gDevice = LightFireCS.Graphics.GDevice.Get();
		Console.WriteLine(gDevice);
		gDevice.CreateDevice(640, 480, 32, false);
		//gDevice.CreateDevice(1280, 1024, 32, true);
		gDevice.SetWindowText("Grid");
	}

	public void Loop()
	{
		gameState = GameState.GS_GAME;

		// Each render methode must use its own Loop
		while (LightFireCS.Graphics.GDevice.Get().IsRunning)
		{
			switch (gameState)
			{
				case GameState.GS_GAME:
					{
						WindowManager.Get().UnregisterAll();
						Game game = new Game();
						game.Render();
						gameState = GameState.GS_MENU;
						if (gameState == GameState.GS_MENU)
						{
							LightFireCS.Graphics.GDevice.Get().Quit();
						}
					} break;
			}
		}
	}
}
