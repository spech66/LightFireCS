using System;
using LightFireCS.Graphics;
using LightFireCS.Math;
using LightFireCS.Graphics.Gui;
using Tao.OpenGl;
using LightFireCS.Input;

public class Game
{
	private bool running;
	
	private SceneNode octtreeNode;
	private Camera camera;
	private SceneNodeBlockGrid blockgNode;
	private SceneNodeModel xyzModelNode;

	public Game()
	{
		octtreeNode = new SceneNodeOcttree();
		blockgNode = new SceneNodeBlockGrid();

		ModelLoader.Get().LoadModel("models/xyz.3ds");
		xyzModelNode = new SceneNodeModel(ModelLoader.Get().GetModel("models/xyz.3ds"));
		octtreeNode.AddNode(xyzModelNode);
		((SceneNodeOcttree)(octtreeNode)).BuildTree(6);

		camera = new Camera();
		camera.SetPosition(10, 10, 10);
		camera.SetTarget(0, 0, 0);
		
		running = true;
	}

	public void Render()
	{
		while(running)
		{
			IDevice iDevice = IDevice.Get();
			GDevice gDevice = GDevice.Get();
			LightFireCS.MessageHandler.Get().ProcessEvents();
						
			if(iDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_ESCAPE))
				running = false;
								
			gDevice.BeginRender();
			gDevice.SetPerspectiveView();
			camera.UpdatePOCamera();

			blockgNode.Render(camera.ViewFrustum);
            octtreeNode.Render(camera.ViewFrustum);

			//gDevice.SetOrthoView();
			//hud.Render();
			gDevice.EndRender();
		}
	}
}
