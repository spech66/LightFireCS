using System;
using LightFireCS;
using LightFireCS.Graphics;
using LightFireCS.Input;

public class Quake3Map
{
	private Camera camera;
	private SceneNode bspNode;
	
	public static void Main()
	{
		Quake3Map q = new Quake3Map();
		q.Init();
		q.Run();
	}
	
	public void Init()
	{
		//LightFireCS.Graphics.GDevice.Get().CreateDevice(1280, 1024, 32, true);
		LightFireCS.Graphics.GDevice.Get().CreateDevice(640, 480, 32, false);
		LightFireCS.Graphics.GDevice.Get().SetWindowText("Quake3 Map");
		
		camera = new Camera();
		camera.SetPosition(32, 40, 32);
		camera.RotateCamera(0, 0, 0);
		//camera.Position.y = 20;
		camera.Position.y = 1;
		//camera.Rotation.z = 20;
		camera.Position.z = 1;
			
		//Q3Bsp q3bsp = new Q3Bsp("res/q3maps/q3dm17.bsp", "res/q3maps/");
		Q3Bsp q3bsp = new Q3Bsp("res/q3maps/aedesert.bsp", "res/q3maps/");
		bspNode = new SceneNodeQ3Bsp(q3bsp);
	}
	
	public void Run()
	{
		GDevice gDevice = GDevice.Get();
        IDevice iDevice = IDevice.Get();

		iDevice.MouseMove += new EventHandler(OnMouseMove);
		gDevice.Resize += new EventHandler(OnResizeWindow);

		while(gDevice.IsRunning)
		{
			MessageHandler.Get().ProcessEvents();

			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_ESCAPE))
				gDevice.Quit();
				
			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_w))
			{
				camera.Position.x -= gDevice.DeltaTime * 0.01 * Math.Sin(camera.Rotation.y * Math.PI / 180.0);
				camera.Position.z -= gDevice.DeltaTime * 0.01 * Math.Cos(camera.Rotation.y * Math.PI / 180.0);
			}
			
			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_s))
			{
				camera.Position.x += gDevice.DeltaTime * 0.01 * Math.Sin(camera.Rotation.y * Math.PI / 180.0);
				camera.Position.z += gDevice.DeltaTime * 0.01 * Math.Cos(camera.Rotation.y * Math.PI / 180.0);
			}
			
			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_a))
				camera.Rotation.y += gDevice.DeltaTime * 0.05;
				
			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_d))
				camera.Rotation.y -= gDevice.DeltaTime * 0.05;

			gDevice.BeginRender();
			gDevice.SetPerspectiveView();
			camera.UpdateCamera();
			bspNode.Render(camera.ViewFrustum);
			gDevice.EndRender();
		}	
	}

	public void OnMouseMove(Object o, EventArgs e)
	{
		/*int x, y;
		//IDevice.Get().GetMouseMotion(out x, out y);
		//IDevice.Get().MouseMove
		camera.Rotation.y += GDevice.Get().DeltaTime * x * 0.001;
		camera.Rotation.z += GDevice.Get().DeltaTime * y * 0.001;*/
	}

	public void OnResizeWindow(object o, EventArgs e)
	{
		bspNode.Resize();
	}
}