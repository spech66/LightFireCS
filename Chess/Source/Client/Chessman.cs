using System;
using LightFireCS;
using LightFireCS.Graphics;
using LightFireCS.Math;

public abstract class Chessman
{
	private double rX, rY;
	private string model;
	private SceneNode sceneNode;
	protected int positionX, positionY;
	protected bool isWhite;
	protected bool killed;	
	public bool[,] moveTo = new bool[8,8];
	
	public bool White { get { return isWhite; } set { isWhite = value; } }
	public bool Kill { set { killed = value; } }
	public bool Killed { get { return killed; } }
	public int X { get { return positionX; } }
	public int Y { get { return positionY; } }

	public abstract bool CanMoveTo(int x, int y, ref char[,] board);
	public abstract char GetType();
	
	public Chessman(int x, int y, bool white)
	{
		MoveTo(x, y);
		isWhite = white;
		killed = false;

		LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile(@"G:\Programmieren\Chess\Models\WHITE.TGA");
		LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile(@"G:\Programmieren\Chess\Models\BLACK.TGA");
	}
	
	public void SetModel(string file)
	{
		ModelLoader modelLoader = LightFireCS.Graphics.ModelLoader.Get();
		model = file;
		modelLoader.LoadModel(file);
		sceneNode = new SceneNodeModel(modelLoader.GetModel(file));
		//BoundingBox b = sceneNode.GetBoundingBox();
	}

	public void MoveTo(int x, int y)
	{
		positionX = x;
		positionY = y;
		
		if(positionX <= 4)
			rX = -120-(4-positionX)*240;
		else
			rX = 120+(positionX-5)*240;
		
		if(positionY <= 4)
			rY = -120-(4-positionY)*240;
		else
			rY = 120+(positionY-5)*240;
	}
	
	public void BuildMoveList(ref char[,] board)
	{
		for(int x = 0; x < 8; x++)
		{
			for(int y = 0; y < 8; y++)
			{
				if(CanMoveTo(x+1, y+1, ref board) && !killed)
					moveTo[x, y] = true;
				else
					moveTo[x, y] = false;				
			}
		}
	}

	public void Render(Camera cam)
	{	
		if(killed)
			return;
		
		sceneNode.SetRotation(new Vector3(-90, 0, 0));
		sceneNode.SetPosition(new Vector3(rX, 0, rY));
	
		if(isWhite)
			LightFireCS.Graphics.TextureManager.Get().SetTexture(@"G:\Programmieren\Chess\Models\WHITE.TGA");
		else
			LightFireCS.Graphics.TextureManager.Get().SetTexture(@"G:\Programmieren\Chess\Models\BLACK.TGA");

		sceneNode.Render(cam.ViewFrustum);
	}
	
	public BoundingBox GetBoundingBox()
	{
		if(killed)
			return null;
		
		sceneNode.SetPosition(new Vector3(rX, 0, rY));
		BoundingBox bb = new BoundingBox(sceneNode.GetBoundingBox());
		return bb;
	}
}

public class King : Chessman
{
	public King(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\koenig.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'K';
		else
			return 'k';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		return true;
	}
}

public class Queen : Chessman
{
	public Queen(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\dame.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'Q';
		else
			return 'q';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		return true;
	}
}

public class Bishop : Chessman
{
	public Bishop(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\laeufer.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'B';
		else
			return 'b';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		return true;
	}
}

public class Knight : Chessman
{
	public Knight(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\springer.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'N';
		else
			return 'n';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		return true;
	}
}

public class Rock : Chessman
{
	public Rock(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\turm.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'R';
		else
			return 'r';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		return true;
	}
}

public class Pawn : Chessman
{
	public Pawn(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\bauer.3DS");
	}
	
	public override char GetType()
	{
		if(isWhite)
			return 'P';
		else
			return 'p';
	}

	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		if(positionX == x && positionY == y)
			return false;
			
		if(isWhite && positionX - x == 0 && y - positionY == 1)
			return true;
			
		if(isWhite && positionX - x == 0 && y - positionY == 2 && positionY == 2)
			return true;
			
		/*if(isWhite && positionX - x == 1 && positionY - y == 0 && board[x, y] == ' ')
			return true;
		if(!isWhite && x - positionX == 1 && positionY - y == 0 && board[x, y] == ' ')
			return true;
		if(isWhite && positionX - x == 2 && positionY - y == 0 && positionX == 6)
			return true;
		if(!isWhite && x - positionX == 2 && positionY - y == 0 && positionX == 1)
			return true;
		if(isWhite && Char.IsLower(board[x, y]) && board[x, y] != ' ' &&
			positionX - x == 1 && Math.Abs(positionY - y) == 1)
			return true;
		if(!isWhite && Char.IsUpper(board[x, y]) && board[x, y] != ' ' &&
			x - positionX == 1 && Math.Abs(positionY - y) == 1)
			return true;
		*/
		return false;
	}
}

public class Arrow : Chessman
{
	public Arrow(int x, int y, bool white): base(x, y, white)
	{
		SetModel(@"G:\Programmieren\Chess\Models\arrow.3DS");
	}
	
	public override char GetType()
	{
		return '*';
	}
	
	public override bool CanMoveTo(int x, int y, ref char[,] board)
	{
		return false;
	}
}
