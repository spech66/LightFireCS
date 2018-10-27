//-----------------------------------------------------------------------------
//  SceneNodeQ3Bsp.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Load and Render Quake3 BSP Maps -
//-----------------------------------------------------------------------------
using System;
using System.IO;
using LightFireCS.Math;
using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	/// <summary>
	/// Description of Q3BspHeader.
	/// </summary>
	public class Q3BspHeader
	{
		public char[] magic;
		public int version;
		public Q3BspDirEntry[] dirEntries = new Q3BspDirEntry[17];
		
		public Q3BspHeader(BinaryReader reader)
		{
			magic = reader.ReadChars(4);
			version = reader.ReadInt32();
			
			if(version != 0x2e)
				throw(new System.Exception("Not a valid Quake3 Map!"));
			
			for(int i = 0; i < dirEntries.Length; i++)
				dirEntries[i] = new Q3BspDirEntry(reader);
		}
	}

	/// <summary>
	/// Description of Q3BspDirEntry.
	/// </summary>
	public class Q3BspDirEntry
	{
		public int offset;
		public int length;
		
		public Q3BspDirEntry(BinaryReader reader)
		{
			offset = reader.ReadInt32();
			length = reader.ReadInt32();
		}
	}

	public class Q3BspEntities
	{
		public char[] entities;
		
		public Q3BspEntities(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			entities = reader.ReadChars(dirEntry.length);
		}
		
		public void Print()
		{
			Console.Write("Q3bspEntities => Entities: ");
			for(int i = 0; i < entities.Length; i++)
				Console.Write(entities[i]);
			Console.WriteLine();
		}
	}
	
	public class Q3BspTexture
	{
		public char[] name;
		public int flags;
		public int contents;
		
		public string strName;
		
		public Q3BspTexture(BinaryReader reader, string textureDir)
		{
			name = reader.ReadChars(64);
			flags = reader.ReadInt32();
			contents = reader.ReadInt32();
			
			strName = textureDir;
			for(int i = 0; i < name.Length; i++)
			{
				if(name[i] != 0)
					strName += name[i];
				else
					break;
			}
						
			if(0 != TextureManager.Get().LoadTextureFromFile(strName + ".jpg"))
			{
				if(0 != TextureManager.Get().LoadTextureFromFile(strName + ".tga"))
				{
					strName = "";
				} else {
					strName += ".tga";
				}
			} else {
				strName += ".jpg";
			}
			
		}
		
		public void Print()
		{
			Console.Write("Q3bspTexture => Name: ");
			for(int i = 0; i < name.Length; i++)
				Console.Write(name[i]);
			Console.WriteLine();
			
			Console.WriteLine("Q3bspTexture => Flags: {0}", flags);
			Console.WriteLine("Q3bspTexture => Contents: {0}", contents);
		}
		
		public static int GetSize()
		{
			int size = 0;
			size += 64; //System.Runtime.InteropServices.Marshal.SizeOf(Magic);
			size += 4; //System.Runtime.InteropServices.Marshal.SizeOf(System.Int32);
			size += 4; //System.Runtime.InteropServices.Marshal.SizeOf(Contents);
			return size;
		}
	}
	
	public class Q3BspTextures
	{
		public Q3BspTexture[] textures;
		
		public Q3BspTextures(BinaryReader reader, Q3BspDirEntry dirEntry, string textureDir)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			int cSize = dirEntry.length / Q3BspTexture.GetSize();
			textures = new Q3BspTexture[cSize];
			
			for(int i = 0; i < cSize; i++)
				textures[i] = new Q3BspTexture(reader, textureDir);
		}
		
		public void Print()
		{
			for(int i = 0; i < textures.Length; i++)
				textures[i].Print();
		}
	}
	
	public class Q3BspPlane
	{
		public float[] normals = new float[3];
		public float distance;
		
		public Q3BspPlane(BinaryReader reader)
		{
			normals[0] = reader.ReadSingle();
			normals[1] = reader.ReadSingle();
			normals[2] = reader.ReadSingle();
			distance = reader.ReadSingle();
		}
		
		public void Print()
		{
			Console.WriteLine("Q3BspPlane => Normal: {0} {1} {2}", normals[0], normals[1], normals[2]);
			Console.WriteLine("Q3BspPlane => Distance: {0}", distance);
		}
		
		public static int GetSize()
		{
			return 16; // 4*sizeof(float)=4*4=16
		}
	}
	
	public class Q3BspPlanes
	{
		public Q3BspPlane[] planes;
		
		public Q3BspPlanes(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);

			int cSize = dirEntry.length / Q3BspPlane.GetSize();
			planes = new Q3BspPlane[cSize];
			
			for(int i = 0; i < cSize; i++)
				planes[i] = new Q3BspPlane(reader);
		}
		
		public void Print()
		{
			for(int i = 0; i < planes.Length; i++)
				planes[i].Print();
		}
	}
	
	public class Q3BspVertex
	{
		public float[] position = new float[3];
		public float[,] texcoords = new float[2, 2];
		public float[] normal = new float[3];
		public byte[] color = new byte[4];
		
		public Q3BspVertex(BinaryReader reader)
		{
			position[0] = reader.ReadSingle();
			position[1] = reader.ReadSingle();
			position[2] = reader.ReadSingle();
			texcoords[0, 0] = reader.ReadSingle();
			texcoords[0, 1] = reader.ReadSingle();
			texcoords[1, 0] = reader.ReadSingle();
			texcoords[1, 1] = reader.ReadSingle();
			normal[0] = reader.ReadSingle();
			normal[1] = reader.ReadSingle();
			normal[2] = reader.ReadSingle();
			color[0] = reader.ReadByte();
			color[1] = reader.ReadByte();
			color[2] = reader.ReadByte();
			color[3] = reader.ReadByte();
		}
		
		public static int GetSize()
		{
			return 44;
		}
	}
	
	public class Q3BspVertices
	{
		public Q3BspVertex[] vertices;
	
		public Q3BspVertices(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			int cSize = dirEntry.length / Q3BspVertex.GetSize();
			vertices = new Q3BspVertex[cSize];
			
			for(int i = 0; i < cSize; i++)
				vertices[i] = new Q3BspVertex(reader);			
		}
	}
	
	public class Q3BspMeshVerts
	{
		public int[] offsets;
		
		public Q3BspMeshVerts(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			int cSize = dirEntry.length / 4;
			offsets = new int[cSize];
			
			for(int i = 0; i < cSize; i++)
				offsets[i] = reader.ReadInt32();
		}
	}	
	
	public class Q3BspFace
	{
		public int textureIndx;
		public int effect;
		public int faceType;
		public int vertexIndx;
		public int vertexNum;
		public int meshIndx;
		public int meshNum;
		public int lightmapIndx;
		public int[] lightmapStart = new int[2];
		public int[] lightmapSize = new int[2];
		public float[] lightmapOrigin = new float[3];
		public float[,] lightmapVect = new float[2, 3];
		public float[] normal = new float[3];
		public int[] size = new int[2];
		
		public Q3BspFace(BinaryReader reader)
		{
			textureIndx = reader.ReadInt32();
			effect = reader.ReadInt32();
			faceType = reader.ReadInt32();
			vertexIndx = reader.ReadInt32();
			vertexNum = reader.ReadInt32();
			meshIndx = reader.ReadInt32();
			meshNum = reader.ReadInt32();
			lightmapIndx = reader.ReadInt32();
			lightmapStart[0] = reader.ReadInt32();
			lightmapStart[1] = reader.ReadInt32();
			lightmapSize[0] = reader.ReadInt32();
			lightmapSize[1] = reader.ReadInt32();
			lightmapOrigin[0] = reader.ReadSingle();
			lightmapOrigin[1] = reader.ReadSingle();
			lightmapOrigin[2] = reader.ReadSingle();
			lightmapVect[0, 0] = reader.ReadSingle();
			lightmapVect[0, 1] = reader.ReadSingle();
			lightmapVect[0, 2] = reader.ReadSingle();
			lightmapVect[1, 0] = reader.ReadSingle();
			lightmapVect[1, 1] = reader.ReadSingle();
			lightmapVect[1, 2] = reader.ReadSingle();
			normal[0] = reader.ReadSingle();
			normal[1] = reader.ReadSingle();
			normal[2] = reader.ReadSingle();
			size[0] = reader.ReadInt32();
			size[1] = reader.ReadInt32();
		}
		
		public static int GetSize()
		{
			return 104;
		}
	}
	
	public class Q3BspFaces
	{
		public Q3BspFace[] faces;
	
		public Q3BspFaces(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			int cSize = dirEntry.length / Q3BspFace.GetSize();
			faces = new Q3BspFace[cSize];
			
			for(int i = 0; i < cSize; i++)
				faces[i] = new Q3BspFace(reader);			
		}
	}
	
	public class Q3BspLightmap
	{
		public byte[,,] map = new byte[128, 128, 3];

		public int texId = 0;
		
		public Q3BspLightmap(BinaryReader reader)
		{
			for(int i = 0; i < 128; i++)
				for(int j = 0; j < 128; j++)
					for(int k = 0; k < 3; k++)
					{
						byte b = reader.ReadByte();
						double bi = b * 2.0; // Gamma correcture
						if(bi > 255.0)
							bi = 255.0;		
						map[i, j, k] = (byte)bi;
					}
					
			BindTexture();	
		}

		public void BindTexture()
		{
			if (texId == 0)
			{
				//Gl.glGenTextures(1, new int[] { texId });
				int[] textureAr = new int[1];
				Gl.glGenTextures(1, textureAr);
				texId = textureAr[0];
			}
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texId);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
			int err = Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGB, 128, 128, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, map);
			if(err != 0)
			{
				EngineLog.Get().Error("Error binding texture (" + texId + "): " + Glu.gluErrorString(err), ToString());
			}
		}
		
		public static int GetSize()
		{
			return 49152;
		}
	}
	
	public class Q3BspLightmaps
	{
		public Q3BspLightmap[] lightmaps;
	
		public Q3BspLightmaps(BinaryReader reader, Q3BspDirEntry dirEntry)
		{
			reader.BaseStream.Seek(Convert.ToInt64(dirEntry.offset), SeekOrigin.Begin);
			
			int cSize = dirEntry.length / Q3BspLightmap.GetSize();
			lightmaps = new Q3BspLightmap[cSize];
			
			for(int i = 0; i < cSize; i++)
				lightmaps[i] = new Q3BspLightmap(reader);			
		}
	}
	
	/// <summary>
	/// Description of Q3Bsp.
	/// </summary>
	public class Q3Bsp
	{	
		private BinaryReader reader;
		
		private Q3BspHeader header;
		private Q3BspEntities entities;
		private Q3BspTextures textures;
		private Q3BspPlanes planes;
		private Q3BspVertices vertices;
		private Q3BspMeshVerts meshVerts;
		private Q3BspFaces faces;
		private Q3BspLightmaps lightmaps;
		
		public Q3Bsp(string map, string textureDir)
		{
			reader = new BinaryReader(File.Open(map, FileMode.Open));
			
			header = new Q3BspHeader(reader);
			
			entities = new Q3BspEntities(reader, header.dirEntries[0]);
			//entities.Print();
			
			textures = new Q3BspTextures(reader, header.dirEntries[1], textureDir);
			//textures.Print();
			
			planes = new Q3BspPlanes(reader, header.dirEntries[2]);
			//planes.Print();
			
			vertices = new Q3BspVertices(reader, header.dirEntries[10]);
			
			meshVerts = new Q3BspMeshVerts(reader, header.dirEntries[11]);
			
			faces = new Q3BspFaces(reader, header.dirEntries[13]);
			
			lightmaps = new Q3BspLightmaps(reader, header.dirEntries[14]);
			
			/*nodes
			leafs
			leaffaces
			leafbrushes
			models
			brushes
			brushsides
				vertexes\vertices
			meshverts
			effects
				faces
				lightmaps
			lightvols
			visdata*/
		}

		public void Resize()
		{
			foreach(Q3BspLightmap lm in lightmaps.lightmaps)
				lm.BindTexture();
		}

		public void Render(Frustum frustum)
		{
			for(int fi = 0; fi < faces.faces.Length; fi++)
			{
				Q3BspFace curFace = faces.faces[fi];
				//int offset = faces.faces[fi].vertexIndx;
				
				// 1=polygon, 2=patch, 3=mesh, 4=billboard
				if(faces.faces[fi].faceType == 1 || faces.faces[fi].faceType == 3) // Polygon
				{
					TextureManager.Get().SetTexture(textures.textures[curFace.textureIndx].strName);
					Gl.glActiveTextureARB(Gl.GL_TEXTURE0_ARB);

					if(curFace.lightmapIndx >= 0)
					{
						Gl.glActiveTextureARB(Gl.GL_TEXTURE1_ARB);
						Gl.glEnable(Gl.GL_TEXTURE_2D);
						Gl.glBindTexture(Gl.GL_TEXTURE_2D, lightmaps.lightmaps[curFace.lightmapIndx].texId);
					}
									
					Gl.glBegin(Gl.GL_TRIANGLES);
					for(int i = 0 ; i < curFace.meshNum; i++)
					{
						Q3BspVertex vert = vertices.vertices[curFace.vertexIndx + meshVerts.offsets[curFace.meshIndx + i]];
						
						Gl.glMultiTexCoord2fARB(Gl.GL_TEXTURE0_ARB, vert.texcoords[0, 0], vert.texcoords[0, 1]);
						
						if(curFace.lightmapIndx >= 0)
						{
							Gl.glMultiTexCoord2fARB(Gl.GL_TEXTURE1_ARB, vert.texcoords[1, 0], vert.texcoords[1, 1]);
						}
						
						Gl.glColor3ub(vert.color[0] , vert.color[1], vert.color[2]);
						// Quake3 uses different coordinate system
						Gl.glVertex3f(vert.position[0], vert.position[2], -vert.position[1]);
					}
					Gl.glEnd();
					
					Gl.glActiveTextureARB(Gl.GL_TEXTURE0_ARB);

					//ToDo: Get this run in an safe enviroment
					//Gl.glVertexPointer(3, Gl.GL_FLOAT, Q3BspVertex.GetSize(), vertices.vertices[offset].position);
					//stride == Q3BspVertex.GetSize()
					//Gl.glClientActiveTextureARB(Gl.GL_TEXTURE0_ARB);
					//Gl.glTexCoordPointer(2, Gl.GL_FLOAT, stride, &(vertex[offset].textureCoord));
					//glClientActiveTextureARB(GL_TEXTURE1_ARB);
					//glTexCoordPointer(2, GL_FLOAT, stride, &(vertex[offset].lightmapCoord));
					//Gl.glDrawElements(Gl.GL_TRIANGLES, curFace.vertexNum, Gl.GL_UNSIGNED_INT, meshVerts.offsets[curFace.vertexIndx]);
				}
			}
		}
	}

	public class SceneNodeQ3Bsp : SceneNode
	{
		private Vector3	position = new Vector3();
		private Vector3	rotation = new Vector3();
		private BoundingBox boundingBox = new BoundingBox();
		private Q3Bsp bsp;
		
		public SceneNodeQ3Bsp(Q3Bsp bsp)
		{
			Visible = true;
			this.bsp = bsp;
		}
		
		public override void SetPosition(Vector3 pos)
		{
		}
		
		public override void SetRotation(Vector3 rot)
		{
			rotation = rot;
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
			bsp.Resize();
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
			
			Gl.glPushMatrix();
			Gl.glTranslated(position.x, position.y, position.z);
			Gl.glRotated(rotation.x, 1, 0, 0);
			Gl.glRotated(rotation.y, 0, 1, 0);
			Gl.glRotated(rotation.z, 0, 0, 1);
			
			Gl.glCullFace(Gl.GL_FRONT);
			Gl.glScaled(0.03, 0.03, 0.03);
			bsp.Render(frustum);
			Gl.glCullFace(Gl.GL_BACK);		
			
			foreach(SceneNode n in nodes)
				n.Render(frustum);

			Gl.glPopMatrix();
		}
	}
}
