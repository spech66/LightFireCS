using System;
using LightFireCS;
using LightFireCS.Graphics;
using LightFireCS.Graphics.Gui;
using LightFireCS.Math;

public class MainMenu
{
	private StyleFactory style;
	private Box background;
	private Window window;
	private Button btnGameCPU, btnGame2, btnExit;

	private bool select;
	private int selectedItem;

	public MainMenu()
	{
		style = new StyleFactory("gui/default.xml");
		background = new Box(new Rect(0, 0, 640, 480), "gui/back.tga");
		
		WindowManager.Get().UnregisterAll();
		
		window = new Window(new Rect(180, 200, 460, 400), "Chess# v1.0", style);
 		WindowManager.Get().RegisterWindow(window);

		btnGameCPU = new Button(window, new Rect(12, 32, 268, 64), "P1 vs AI", style);
		btnGameCPU.LeftDown += new EventHandler(OnButtonGameCPUDown);
		btnGame2 = new Button(window, new Rect(12, 96, 268, 128), "P1 vs P2", style);
		btnGame2.LeftDown += new EventHandler(OnButtonGame2Down);
		btnExit = new Button(window, new Rect(12, 160, 268, 192), "Exit", style);
		btnExit.LeftDown += new EventHandler(OnButtonExitDown);
		
		select = true;                
	}

   public int Render()
   {
		while(select)
		{
			LightFireCS.MessageHandler.Get().ProcessEvents();
			GDevice gDevice = GDevice.Get();
			
			gDevice.BeginRender();
			gDevice.SetOrthoView();
			background.Render();
			WindowManager.Get().Render();
			gDevice.EndRender();
		}
		
		return selectedItem;
   }
   
	public void OnButtonGameCPUDown(object o, EventArgs e)
	{
		selectedItem = 0;
		select = false;
	}

	public void OnButtonGame2Down(object o, EventArgs e)
	{
		selectedItem = 1;
		select = false;
	}

	public void OnButtonExitDown(object o, EventArgs e)
	{
		selectedItem = 2;
		select = false;
	}

}