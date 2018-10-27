using System;
using System.Text.RegularExpressions;
using LightFireCS;
using LightFireCS.Math;
using Tao.OpenGl;

public class Board
{
	private LightFireCS.Graphics.GDevice GDevice;
	private LightFireCS.Graphics.Camera Cam;
	private LightFireCS.Graphics.TextureManager TexMan;
	private LightFireCS.Graphics.ModelLoader ModelLoader;
	private LightFireCS.Input.IDevice InputDevice;

	private Chessman[] chessman;

	private char[,] board = new char[8,8];
	private string activeColor;		// w/b
	private string castling;		// Rochade
	private string enPassant;
	private int halfDrafts;
	private int drafts;
	
	private Chessman chessmanInHand;
	private Chessman arrow;

	public Board()
	{
		GDevice = LightFireCS.Graphics.GDevice.Get();
		Cam = new LightFireCS.Graphics.Camera();
		TexMan = LightFireCS.Graphics.TextureManager.Get();
		ModelLoader = LightFireCS.Graphics.ModelLoader.Get();
		InputDevice = LightFireCS.Input.IDevice.Get();
		TexMan.LoadTextureFromFile(@"g:\Programmieren\Chess\Models\WHITE.TGA");
		TexMan.LoadTextureFromFile(@"g:\Programmieren\Chess\Models\BLACK.TGA");
		TexMan.LoadTextureFromFile(@"g:\Programmieren\Chess\Models\BOARD.TGA");
		Cam.SetPosition(0, 20, 30);
		Cam.SetRotation(0, 0, 45);

		Cam.SetPosition(0, 30, 30);
		Cam.SetRotation(0, 0, 120);

		HomePosition();

		arrow = new Arrow(1, 1, true);
		
		/*ParseFen("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Render();
		ParseFen("rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2");
		Render();
		ParseFen("rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2");
		Render();*/
		//MoveFigure(1, 7, 2, 5);
		/*MoveFigure(2, 6, 2, 4);
		MoveFigure(1, 1, 1, 3);
		MoveFigure(1, 3, 2, 4);
		RenderAscii();*/
	}
	
	public void HomePosition()
	{
		ParseFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq _ 0 1");
		
		chessmanInHand = null;
		
		chessman = new Chessman[32];
		chessman[0] = new King(5, 1, true);
		chessman[1] = new King(5, 8, false);
		chessman[2] = new Queen(4, 1, true);
		chessman[3] = new Queen(4, 8, false);
		chessman[4] = new Bishop(3, 1, true);
		chessman[5] = new Bishop(6, 1, true);
		chessman[6] = new Bishop(3, 8, false);
		chessman[7] = new Bishop(6, 8, false);
		chessman[8] = new Knight(2, 1, true);
		chessman[9] = new Knight(7, 1, true);
		chessman[10] = new Knight(2, 8, false);
		chessman[11] = new Knight(7, 8, false);
		chessman[12] = new Rock(1, 1, true);
		chessman[13] = new Rock(8, 1, true);
		chessman[14] = new Rock(1, 8, false);
		chessman[15] = new Rock(8, 8, false);
		for(int i = 16; i < 24; i++)
		{
			chessman[i] = new Pawn(i-15, 2, true);
			chessman[i+8] = new Pawn(i-15, 7, false);
		}
		
		foreach(Chessman cmBuild in chessman)
			cmBuild.BuildMoveList(ref board);
	}

	public void ParseFen(string fen)
	{
		string fenNonBoard = fen.Substring(fen.IndexOf(" ")+1);
		fen = fen.Replace("/", "");
		
		string rep = " ";
		for(int i = 1; i < 9; i++)
		{
			fen = fen.Replace(Convert.ToString(i), rep);
			rep += " ";	
		}
		
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				board[i, j] = fen[i*8 + j];
			}
		}

		string[] tokens = fenNonBoard.Split(' ');
		activeColor = tokens[0];
		castling = tokens[1];
		enPassant = tokens[2];
		halfDrafts = Convert.ToInt32(tokens[3]);
		drafts = Convert.ToInt32(tokens[4]);
	}
	
	public bool MoveFigure(int x, int y, int toX, int toY)
	{
		Console.WriteLine("Move - From ({0}/{1}) To ({2}/{3})", x, y, toX, toY);
		
		foreach(Chessman cm in chessman)
		{
			if(cm.X == x && cm.Y == y)
			{
				if(cm.CanMoveTo(toX, toY, ref board))
				{
					// Remove chessman at target
					foreach(Chessman cmRem in chessman)
					{
						if(cmRem.X == toX && cmRem.Y == toY)
							cmRem.Kill = true;
					}
					
					cm.MoveTo(toX, toY);
					
					board[y-1, x-1] = ' ';
					board[toY-1, toX-1] = cm.GetType();
										
					foreach(Chessman cmBuild in chessman)
						cmBuild.BuildMoveList(ref board);
					
					if(activeColor == "w")
						activeColor = "b";
					else
						activeColor = "w";
						
					RenderAscii();	
						
					return true;
				}
			}
		}

		return false;
	}

	public void RenderAscii()
	{
		Console.WriteLine(activeColor + " " + castling + " " + enPassant + " " +
							halfDrafts + " " + drafts);
		Console.WriteLine("  A B C D E F G H");
		for(int i = 0; i < 8; i++)
		{
			Console.Write(8-i + " ");
			for(int j = 0; j < 8; j++)
			{
				Console.Write(board[i, j] + " ");
			}
			Console.WriteLine(8-i);
		}
		Console.WriteLine("  A B C D E F G H");
		Console.WriteLine();
	}
	
	public void RenderBoard()
	{
		LightFireCS.Graphics.TextureManager.Get().SetTexture(@"g:\Programmieren\Chess\Models\BOARD.TGA");
	
		Gl.glBegin(Gl.GL_QUADS);
		
		Gl.glTexCoord2d(0.0, 0.0);
		Gl.glVertex3d(-1000.0, -200.0, -1000.0);
		
		Gl.glTexCoord2d(0.0, 1.0);
		Gl.glVertex3d(-1000.0, -200.0, 1000.0);
		
		Gl.glTexCoord2d(1.0, 1.0);
		Gl.glVertex3d(1000.0, -200.0, 1000.0);
		
		Gl.glTexCoord2d(1.0, 0.0);
		Gl.glVertex3d(1000.0, -200.0, -1000.0);
		
		Gl.glEnd();
	}
	
	public bool Render()
	{
		LightFireCS.MessageHandler.Get().ProcessEvents();

		if(InputDevice.GetKeyState(Tao.Sdl.Sdl.SDLK_ESCAPE))
			GDevice.Quit();
			
		int mouseX, mouseY;
		InputDevice.GetMousePos(out mouseX, out mouseY);
			
		GDevice.BeginRender();
		GDevice.SetPerspectiveView();
		Cam.UpdateCamera();

		Gl.glPushMatrix();
		Gl.glScaled(0.01, 0.01, 0.01);

		if(InputDevice.GetMouseButtonState(0))
        {
			Vector3 pos, dir;
			double[] hit;
			GDevice.RayFromPoint(mouseX, mouseY, out pos, out dir);
			
			foreach(Chessman cm in chessman)
			{
				BoundingBox cmbb = cm.GetBoundingBox();
				if (cmbb != null && Intersection.RayBoundingBox(pos, dir, cmbb, out hit))
				{			
					if((cm.White && activeColor == "w") || (!cm.White && activeColor == "b"))
					{
						chessmanInHand = cm;
						Console.WriteLine("Hit. x:{0} y:{1}", cm.X , cm.Y);
					} else {
						Console.WriteLine("Hit Wrong Color. x:{0} y:{1}", cm.X , cm.Y);
					}
				}
			}
        }

		/*Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
		Gl.glDepthMask(Gl.GL_FALSE);
		Gl.glEnable(Gl.GL_STENCIL_TEST);
		Gl.glStencilFunc(Gl.GL_ALWAYS, 1, 0xFFFFFFFF);
		Gl.glStencilOp(Gl.GL_REPLACE, Gl.GL_REPLACE, Gl.GL_REPLACE);
		RenderBoard();
		
		// reflektion nur im bereich
		Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
		Gl.glDepthMask(Gl.GL_TRUE);
		Gl.glStencilFunc(Gl.GL_EQUAL, 1, 0xFFFFFFFF);
		Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_KEEP);
		
		Gl.glPushMatrix();
		Gl.glScaled(1.0, -1.0, 1.0);
		foreach(Chessman cm in chessman)
			cm.Render();
		Gl.glPopMatrix();*/
		
		RenderBoard();
		
		foreach(Chessman cm in chessman)
		{
			if(chessmanInHand == cm)
				Gl.glColor3ub(0, 255, 0);
			cm.Render(Cam);
			if (!cm.Killed)
				cm.GetBoundingBox().Render();
			if(chessmanInHand == cm)
				Gl.glColor3ub(255, 255, 255);
		}
		
		if(chessmanInHand != null)
		{
			for(int x = 0; x < 8; x++)
			{
				for(int y = 0; y < 8; y++)
				{
					if(chessmanInHand != null && chessmanInHand.moveTo[x, y])
					{
						Gl.glColor3ub(0, 255, 0);
						arrow.MoveTo(x+1, y+1);
						arrow.Render(Cam);
						Gl.glColor3ub(255, 255, 255);
						if(InputDevice.GetMouseButtonState(0))
						{
							Vector3 pos, dir;
							double[] hit;
							GDevice.RayFromPoint(mouseX, mouseY, out pos, out dir);
							if(Intersection.RayBoundingBox(pos, dir, arrow.GetBoundingBox(), out hit))
							{
								if(MoveFigure(chessmanInHand.X, chessmanInHand.Y, x+1, y+1))
								{
									chessmanInHand = null;
								}
							}
						}
					}
				}
			}
		}

		Gl.glPopMatrix();
		GDevice.EndRender();

		return GDevice.IsRunning;
	}
}