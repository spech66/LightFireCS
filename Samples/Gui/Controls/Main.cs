using System;
using System.IO;
using LightFireCS;
using LightFireCS.Graphics;
using LightFireCS.Graphics.Gui;
using LightFireCS.Input;

public class Controls
{
	private StyleFactory style;
	private Box boxBackground;
	private Window windowQuit, windowText;
	private Button btnWindowQuitQuit;
	private TextCtrl txtWindowTextText;
	private ProgressBar progressBar;

	public static void Main()
	{
		Controls c = new Controls();
		c.Init();
		c.Run();
	}
	
	public void Init()
	{
		GDevice.Get().CreateDevice(640, 480, 32, false);
		GDevice.Get().SetWindowText("Gui Controls");	
	
		style = new StyleFactory("res/gui/default.xml");
	
		boxBackground = new Box(new Rect(0, 0, 640, 480), new Color(160, 196, 255));
		
		windowQuit = new Window(new Rect(100, 100, 380, 364), "Main Window", style, "testwindow");
		windowText = new Window(new Rect(300, 120, 640, 400), "Text Window", style);
		WindowManager.Get().RegisterWindow(windowText);
		WindowManager.Get().RegisterWindow(windowQuit);

		btnWindowQuitQuit = new Button(windowQuit, new Rect(12, 32, 268, 64), "Quit", style);
		btnWindowQuitQuit.LeftDown += new EventHandler(OnButtonWindowQuitQuit);
		progressBar = new ProgressBar(windowQuit, new Rect(12, 96, 268, 128), "", style);
		txtWindowTextText = new TextCtrl(windowText, new Rect(12, 32, 328, 260), "", style);
		
		progressBar.SetBorder(10, 50);
		progressBar.Value = 30;
		//progressBar.ShowText = false;
		
		StreamReader sr = new StreamReader(File.Open("res/sample.txt", FileMode.Open));
		txtWindowTextText.AppendText(sr.ReadToEnd());
		sr.Close();
	}
	
	public void Run()
	{
		GDevice gDevice = GDevice.Get();

        while (gDevice.IsRunning)
		{
			MessageHandler.Get().ProcessEvents();
			
			if(IDevice.Get().GetKeyState(Tao.Sdl.Sdl.SDLK_ESCAPE))
				gDevice.Quit();
			
			gDevice.BeginRender();
			gDevice.SetOrthoView();
			boxBackground.Render();
			WindowManager.Get().Render();
            windowQuit.WindowFont.Render("FPS: " + gDevice.Fps, new Color(255, 255, 255), new Rect(0, 0, 60, 40));
			gDevice.EndRender();
		}	
	}
	
	public void OnButtonWindowQuitQuit(object o, EventArgs e)
	{
		GDevice.Get().Quit();
	}
}