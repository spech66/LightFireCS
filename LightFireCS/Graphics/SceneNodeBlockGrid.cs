//-----------------------------------------------------------------------------
//  SceneNodeBlockGrid.cs
//  Copyright (C) 2008 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Block grid scene node -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.Math;

using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	public class SceneNodeBlockGrid : SceneNode
	{
		private Vector3 position = new Vector3();
		private Vector3 rotation = new Vector3();
		private BoundingBox boundingBox = new BoundingBox();

		private int[][] grid;

		public SceneNodeBlockGrid()
		{
			grid = new int[][] {
				new int[]{0,0, 0,0, 0,0, 1,1, 0,0, 1,1},
				new int[]{0,0, 0,0, 0,0, 1,1, 0,0, 1,1},

				new int[]{1,1, 1,0, 0,0, 0,0, 0,0, 0,0},
				new int[]{1,1, 1,0, 0,0, 0,0, 0,0, 0,0},

				new int[]{1,1, 1,0, 0,0, 0,0, 0,0, 0,0},
				new int[]{1,1, 1,0, 0,0, 0,0, 0,0, 0,0},

				new int[]{0,0, 0,0, 0,0, 0,0, 0,0, 0,0},
				new int[]{0,0, 0,0, 0,0, 0,0, 0,0, 0,0}
			};

			LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile("models\\mauerski.jpg");
			LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile("models\\floor.jpg");
			LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile("models\\color.jpg");
		}

		public override void SetPosition(Vector3 pos)
		{
		}

		public override void SetRotation(Vector3 rot)
		{
		}

		public override BoundingBox GetBoundingBox()
		{
			return boundingBox;
		}

		public override Vector3 GetPosition()
		{
			return position;
		}

		public override Vector3 GetRotation()
		{
			return rotation;
		}

		public override void Resize()
		{
		}

		public override void Update()
		{
			foreach(SceneNode node in nodes)
				node.Update();
		}

		public override void Render(Frustum frustum)
		{
			if(false == Visible)
				return;

			LightFireCS.Graphics.TextureManager.Get().SetTexture("models\\floor.jpg");
			Gl.glBegin(Gl.GL_QUADS);
			//Gl.glColor3ub(0, 255, 0);
			for(int i = 0; i < grid.Length / 2; i++)
			{
				for(int j = 0; j < grid[i].Length / 2; j++)
				{
					Gl.glTexCoord2f(0, 0);
					Gl.glVertex3i(i, grid[(i * 2)][(j * 2)], j);
					Gl.glTexCoord2f(1, 0);
					Gl.glVertex3i(i + 1, grid[(i * 2) + 1][(j * 2)], j);
					Gl.glTexCoord2f(1, 1);
					Gl.glVertex3i(i + 1, grid[(i * 2) + 1][(j * 2) + 1], j + 1);
					Gl.glTexCoord2f(0, 1);
					Gl.glVertex3i(i, grid[(i * 2)][(j * 2) + 1], j + 1);
				}
			}
			Gl.glEnd();

			LightFireCS.Graphics.TextureManager.Get().SetTexture("models\\mauerski.jpg");
			Gl.glBegin(Gl.GL_QUADS);
			for(int i = 0; i < (grid.Length-1) / 2; i++)
			{
				for(int j = 0; j < grid[i].Length / 2; j++)
				{
					// Front
					if(true) // TODO: Check if next node is not higher or lower
					{
						Gl.glTexCoord2f(0, 0);
						Gl.glVertex3i(i + 1, grid[(i * 2) + 1][(j * 2)], j);
						Gl.glTexCoord2f(1, 0);
						Gl.glVertex3i(i + 1, grid[(i * 2) + 2][(j * 2) + 1], j);
						Gl.glTexCoord2f(1, 1);
						Gl.glVertex3i(i + 1, grid[(i * 2) + 2][(j * 2) + 1], j + 1);
						Gl.glTexCoord2f(0, 1);
						Gl.glVertex3i(i + 1, grid[(i * 2 + 1)][(j * 2)], j + 1);
					}
				}
			}
			Gl.glEnd();
		}
	}
}
