//-----------------------------------------------------------------------------
//  Model.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Model for ModelManager -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.Math;
using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	
	public class ModelTextureCoord
	{
		public float u, v;
	}
	
	[CLSCompliant(false)]
	public class ModelFace
	{
		public ushort[] vertexIndex = new ushort[3];
		public ushort[] textureCoordIndex = new ushort[3];
		public ushort[] normalIndex = new ushort[3];
	}
	
	public class ModelObject
	{
		public string		name;
		public ArrayList 	vertexList = new ArrayList();
		public ArrayList 	faceList = new ArrayList();
		public ArrayList 	normalList = new ArrayList();
		public ArrayList 	texcoordList = new ArrayList();
		public string		materialName;
	}
	
	public class ModelMaterial
	{
		public string	name;
		public string	textureName;
	}

	public class Model
	{
		public string		file;
		public ArrayList 	objectList = new ArrayList();
		public ArrayList 	materialList = new ArrayList();
		
		public Model()
		{
		}
		
		public void Render()
		{
			foreach(ModelObject mdlObj in objectList)
			{
				try
				{
					foreach(ModelMaterial mdlMat in materialList)
					{
						if(mdlMat.name == mdlObj.materialName)
							LightFireCS.Graphics.TextureManager.Get().SetTexture(mdlMat.textureName);
					}

					Gl.glBegin(Gl.GL_TRIANGLES);
					foreach(ModelFace mdlFace in mdlObj.faceList)
					{
						int[] vertIndex = new int[3];
						vertIndex[0] = mdlFace.vertexIndex[0];
						vertIndex[1] = mdlFace.vertexIndex[1];
						vertIndex[2] = mdlFace.vertexIndex[2];

						Gl.glTexCoord2f(((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[0]])).u,
							((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[0]])).v);
						Gl.glVertex3d(((Vector3)(mdlObj.vertexList[vertIndex[0]])).x,
									((Vector3)(mdlObj.vertexList[vertIndex[0]])).y,
									((Vector3)(mdlObj.vertexList[vertIndex[0]])).z);

						Gl.glTexCoord2f(((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[1]])).u,
							((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[1]])).v);
						Gl.glVertex3d(((Vector3)(mdlObj.vertexList[vertIndex[1]])).x,
									((Vector3)(mdlObj.vertexList[vertIndex[1]])).y,
									((Vector3)(mdlObj.vertexList[vertIndex[1]])).z);
						
						Gl.glTexCoord2f(((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[2]])).u,
							((ModelTextureCoord)(mdlObj.texcoordList[vertIndex[2]])).v);
						Gl.glVertex3d(((Vector3)(mdlObj.vertexList[vertIndex[2]])).x,
									((Vector3)(mdlObj.vertexList[vertIndex[2]])).y,
									((Vector3)(mdlObj.vertexList[vertIndex[2]])).z);
					}
					Gl.glEnd();
				} catch {
					Console.WriteLine("Error in " + file + " - Object " + mdlObj.name);
				}
			}
		}
	}
}