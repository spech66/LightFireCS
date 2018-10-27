//-----------------------------------------------------------------------------
//  Terrain.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Terrain -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using Tao.OpenGl;
using LightFireCS.Math;

namespace LightFireCS.Graphics
{
	public class TerrainLayer
	{
		private string texture, detailTexture;
		private int max, min;
		private int index;

		public string Texture { get { return texture; } }
		public string DetailTexture { get { return detailTexture; } }
		public int Min { get { return min; } }
		public int Max { get { return max; } }
		public int Index { get { return index; } }

		public TerrainLayer(string texture, string detailTexture, int max, int min, int index)
		{
			this.texture = texture;
			this.detailTexture = detailTexture;
			this.max = max;
			this.min = min;
			this.index = index;
		}
	}

	public class TerrainTree
	{
		private Terrain terrain;
		private int indexStartX;
		private int indexEndX;
		private int indexStartZ;
		private int indexEndZ;
		private TerrainTree[] childs;
		private BoundingBox boundingBox;

		public TerrainTree(int level, int indexStartX, int indexEndX, int indexStartZ, int indexEndZ, Terrain t)
		{
			terrain = t;
			childs = null;

			double minY = 99999999, maxY = -99999999;
			for(int i = indexStartX; i < indexEndX; i++)
			{
				for(int j = indexStartZ; j < indexEndZ; j++)
				{
					double h = terrain.terrain[i, j]*terrain.ScaleY;
					if(h < minY)
						minY = h;
					if(h > maxY)
						maxY = h;
				}
			}

			boundingBox = new BoundingBox(indexEndX*terrain.ScaleXZ, maxY, indexEndZ*terrain.ScaleXZ,
											indexStartX*terrain.ScaleXZ, minY, indexStartZ*terrain.ScaleXZ);

			// Smallest Patch Size: 8x8
			if(indexEndX-indexStartX <= 8 || indexEndZ-indexStartZ <= 8)
			{
				this.indexStartX = indexStartX;
				this.indexEndX = indexEndX;
				this.indexStartZ = indexStartZ;
				this.indexEndZ = indexEndZ;
				return;
			}

			childs = new TerrainTree[4];
			level++;

			int midX = Convert.ToInt32((indexEndX-indexStartX)/2 + indexStartX);
			int midZ = Convert.ToInt32((indexEndZ-indexStartZ)/2 + indexStartZ);
			childs[0] = new TerrainTree(level, indexStartX, midX, indexStartZ, midZ, terrain);
			childs[1] = new TerrainTree(level, midX, indexEndX, indexStartZ, midZ, terrain);
			childs[2] = new TerrainTree(level, indexStartX, midX, midZ, indexEndZ, terrain);
			childs[3] = new TerrainTree(level, midX, indexEndX, midZ, indexEndZ, terrain);
		}

		public void Render(Frustum frustum)
		{
			if(!frustum.BoundingBoxInside(boundingBox))
				return;

			if(childs != null)
			{
				childs[0].Render(frustum);
				childs[1].Render(frustum);
				childs[2].Render(frustum);
				childs[3].Render(frustum);
				return;
			}

			Gl.glDisable(Gl.GL_BLEND);

			foreach(TerrainLayer layer in terrain.layers)
			{
				TextureManager.Get().SetTexture(layer.Texture);
				Gl.glBegin(Gl.GL_QUADS);
				for(int x = indexStartX; x < indexEndX; x++)
				{
					for(int z = indexStartZ; z < indexEndZ; z++)
					{
						if(layer.Index == terrain.layersIndex[x, z])
						{
							Gl.glTexCoord2d(1.0/8.0*(x-indexStartX), 1.0/8.0*(z-indexStartZ));
							Gl.glVertex3d(terrain.ScaleXZ*x, terrain.ScaleY*terrain.terrain[x,z], terrain.ScaleXZ*z);
							Gl.glTexCoord2d(1.0/8.0*(x-indexStartX), 1.0/8.0*(z-indexStartZ+1));
							Gl.glVertex3d(terrain.ScaleXZ*x, terrain.ScaleY*terrain.terrain[x,z+1], terrain.ScaleXZ*(z+1));
							Gl.glTexCoord2d(1.0/8.0*(x-indexStartX+1), 1.0/8.0*(z-indexStartZ+1));
							Gl.glVertex3d(terrain.ScaleXZ*(x+1), terrain.ScaleY*terrain.terrain[x+1,z+1], terrain.ScaleXZ*(z+1));
							Gl.glTexCoord2d(1.0/8.0*(x-indexStartX+1), 1.0/8.0*(z-indexStartZ));
							Gl.glVertex3d(terrain.ScaleXZ*(x+1), terrain.ScaleY*terrain.terrain[x+1,z], terrain.ScaleXZ*z);
						}
					}
				}
				Gl.glEnd();
			}
			
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glEnable(Gl.GL_BLEND);
			//Hack: For windows because of glDepthFunc(GL_LEQUAL)
			Gl.glDisable(Gl.GL_DEPTH_TEST);
			
			int AlphaTL, AlphaTR, AlphaBL, AlphaBR;
			foreach(TerrainLayer layer in terrain.layers)
			{
				TextureManager.Get().SetTexture(layer.Texture);
				Gl.glBegin(Gl.GL_QUADS);
				for(int x = indexStartX; x < indexEndX; x++)
				{
					for(int z = indexStartZ; z < indexEndZ; z++)
					{
						if(layer.Index == terrain.layersIndex[x, z+1])
							AlphaTL = 1;
						else
							AlphaTL = 0;

						if(layer.Index == terrain.layersIndex[x+1, z+1])
							AlphaTR = 1;
						else
							AlphaTR = 0;

						if(layer.Index == terrain.layersIndex[x, z])
							AlphaBL = 1;
						else
							AlphaBL = 0;

						if(layer.Index == terrain.layersIndex[x+1, z])
							AlphaBR = 1;
						else
							AlphaBR = 0;

						Gl.glColor4d(1.0, 1.0, 1.0, AlphaBL);
						Gl.glTexCoord2d(1.0/8.0*(x-indexStartX), 1.0/8.0*(z-indexStartZ));
						Gl.glVertex3d(terrain.ScaleXZ*x, terrain.ScaleY*terrain.terrain[x,z], terrain.ScaleXZ*z);
						Gl.glColor4d(1.0, 1.0, 1.0, AlphaTL);
						Gl.glTexCoord2d(1.0/8.0*(x-indexStartX), 1.0/8.0*(z-indexStartZ+1));
						Gl.glVertex3d(terrain.ScaleXZ*x, terrain.ScaleY*terrain.terrain[x,z+1], terrain.ScaleXZ*(z+1));
						Gl.glColor4d(1.0, 1.0, 1.0, AlphaTR);
						Gl.glTexCoord2d(1.0/8.0*(x-indexStartX+1), 1.0/8.0*(z-indexStartZ+1));
						Gl.glVertex3d(terrain.ScaleXZ*(x+1), terrain.ScaleY*terrain.terrain[x+1,z+1], terrain.ScaleXZ*(z+1));
						Gl.glColor4d(1.0, 1.0, 1.0, AlphaBR);
						Gl.glTexCoord2d(1.0/8.0*(x-indexStartX+1), 1.0/8.0*(z-indexStartZ));
						Gl.glVertex3d(terrain.ScaleXZ*(x+1), terrain.ScaleY*terrain.terrain[x+1,z], terrain.ScaleXZ*z);
					}
				}
				Gl.glEnd();
			}
			
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glDisable(Gl.GL_BLEND);

			/*for(int x = 0; x < 31; x++)
			{
				Gl.glBegin(Gl.GL_QUADS);
				for(int z = 0; z < 31; z++)
				{
					Gl.glTexCoord2d(1.0/32*x, 1.0/32*z);
					Gl.glVertex3d(scaleXZ*x, scaleY*terrain[x,z], scaleXZ*z);
					Gl.glTexCoord2d(1.0/32*x, 1.0/32*(z+1));
					Gl.glVertex3d(scaleXZ*x, scaleY*terrain[x,z+1], scaleXZ*(z+1));
					Gl.glTexCoord2d(1.0/32*(x+1), 1.0/32*(z+1));
					Gl.glVertex3d(scaleXZ*(x+1), scaleY*terrain[x+1,z+1], scaleXZ*(z+1));
					Gl.glTexCoord2d(1.0/32*(x+1), 1.0/32*z);
					Gl.glVertex3d(scaleXZ*(x+1), scaleY*terrain[x+1,z], scaleXZ*z);
				}
				Gl.glEnd();

				Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
			    for(x = 0; x < 31; x++)
			    {
			        for(z = 0; z < 31; z++)
			        {
			            glVertex3d(scaleXZ*(x+1), scaleY*terrain[x+1][z], scaleXZ*z);
			            glVertex3d(scaleXZ*x, scaleY*terrain[x][z], scaleXZ*z);
			        }
			    }
			    glEnd();
			}*/
		}
	}

	public class Terrain
	{
		public byte[,] terrain;
		public ArrayList layers = new ArrayList();
		public int[,] layersIndex;

		private int size;
		private double scaleXZ;
		private double scaleY;
		private TerrainTree terrainTree;

		public double ScaleXZ { get { return scaleXZ; } }
		public double ScaleY { get { return scaleY; } }
		public int Size { get { return size; } }

		public Terrain(int sizeFactor, double scaleXZ, double scaleY)
		{
			size = Convert.ToInt32(System.Math.Pow(2, sizeFactor));
			int realSize = size + 1;
			terrain = new byte[realSize, realSize];
			layersIndex = new int[realSize, realSize];

			for(int x = 0; x < realSize; x++)
			{
				for(int y = 0; y < realSize; y++)
				{
					terrain[x, y] = 0;
				}
			}

			this.scaleXZ = scaleXZ;
			this.scaleY = scaleY;
			terrainTree = new TerrainTree(0, 0, size, 0, size, this);
		}

		public Terrain(string heightMap, int sizeFactor, double scaleXZ, double scaleY)
		{
			size = Convert.ToInt32(System.Math.Pow(2, sizeFactor));
			int realSize = size + 1;
			terrain = new byte[realSize, realSize];
			layersIndex = new int[realSize, realSize];

			BinaryReader binReader = new BinaryReader(File.Open(heightMap, FileMode.Open));
			for(int x = 0; x < realSize; x++)
			{
				for(int y = 0; y < realSize; y++)
				{
					terrain[x, y] = binReader.ReadByte();
				}
			}
			binReader.Close();

			this.scaleXZ = scaleXZ;
			this.scaleY = scaleY;
			terrainTree = new TerrainTree(0, 0, size, 0, size, this);
		}

		public Terrain(int sizeFactor)
			: this(sizeFactor, 4.0, 0.5)
		{
		}

		public Terrain(string heightMap, int sizeFactor)
			: this(heightMap, sizeFactor, 4.0, 0.5)
		{
		}

		public void InsertTexture(string texture, string detailTexture, int min, int max)
		{
			TextureManager.Get().LoadTextureFromFile(texture);
			TextureManager.Get().LoadTextureFromFile(detailTexture);
			TerrainLayer tmpLayer = new TerrainLayer(texture, detailTexture, min, max, layers.Count + 1);
			layers.Add(tmpLayer);

			for(int x = 0; x < size; x++)
			{
				for(int y = 0; y < size; y++)
				{
					if(terrain[x, y] >= min && terrain[x, y] <= max)
						layersIndex[x, y] = layers.Count;
				}
			}
		}

		public void Render(Frustum frustum)
		{
			terrainTree.Render(frustum);
		}

		public double GetHeight(double x, double z)
		{
			if(x > 0 && x < scaleXZ * size && z > 0 && z < scaleXZ * size)
			{
				int indexX = Convert.ToInt32(System.Math.Round(x/scaleXZ));
				int indexZ = Convert.ToInt32(System.Math.Round(z/scaleXZ));

				if(indexX > size-2)	indexX = size-2;
				if(indexZ > size-2)	indexZ = size-2;

				double h0 = terrain[indexX, indexZ]*scaleY;
				double h1 = terrain[indexX, indexZ+1]*scaleY;
				double h2 = terrain[indexX+1, indexZ+1]*scaleY;
				double h3 = terrain[indexX+1, indexZ]*scaleY;

				double dX = (x - indexX * scaleXZ) / scaleXZ;
				double dZ = (z - indexZ * scaleXZ) / scaleXZ;

				return h0 + dX * (h1-h0) + dZ * (h2-h0) + dX * dZ * (h0 - h1 - h2 + h3);
			}
			return -1;
		}

		public double GetHeight(int x, int y)
		{
			if(x < 0 || x > size-1 || y < 0 || y > size-1)
				return -1;

			return terrain[x, y]*scaleY;
		}

		public void SetHeight(int x, int y, byte h)
		{
			terrain[x, y] = h;
		}

		public int GetTextureIndex(int x, int y)
		{
			if(x < 0 || x > size-1 || y < 0 || y > size-1)
				return -1;
			return layersIndex[x, y];
		}

		public void SetTextureIndex(int x, int y, int index)
		{
			if(x < 0 || x > size-1 || y < 0 || y > size-1)
				return;
			layersIndex[x, y] = index;
		}

		public int[] PickPoint()
		{
			int mx, my;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mx, out my);

			Vector3 pos, dir;
			GDevice.Get().RayFromPoint(mx, my, out pos, out dir);

			for(int x = 0; x < size-1; x++)
			{
				for(int z = 0; z < size-1; z++)
				{
					Vector3 v1 = new Vector3(x * scaleXZ, terrain[x, z]*scaleY, z * scaleXZ);
					Vector3 v2 = new Vector3(x * scaleXZ, terrain[x, z+1]*scaleY, (z+1) * scaleXZ);
					Vector3 v3 = new Vector3((x+1) * scaleXZ, terrain[x+1, z+1]*scaleY, (z+1) * scaleXZ);

					if(Intersection.RayTriangle(pos, dir, v1, v2, v3))
					{
						return new int[]{x, z};
					}

					Vector3 v4 = new Vector3((x+1) * scaleXZ, terrain[x+1, z]*scaleY, z * scaleXZ);
					if(Intersection.RayTriangle(pos, dir, v2, v3, v4))
					{
						return new int[]{x, z};
					}
				}
			}

			return new int[]{-1, -1};
		}
	}
}
