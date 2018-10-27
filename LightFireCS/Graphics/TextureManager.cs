//-----------------------------------------------------------------------------
//  TextureManager.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Loads and stores textures -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.IO;

using Tao.OpenGl;
using Tao.DevIl;

namespace LightFireCS.Graphics
{
	public class Texture
	{
		public int id = -1;
		public int width, height, bpp;
		//public byte[] data;
	}

	public class TextureManager
	{
		private static TextureManager instance;
		private Hashtable textureList = new Hashtable();
		private int lastTexture = 0;
	
		private TextureManager()
		{
			Il.ilInit();
			Ilu.iluInit(); 
		}
		
		public static TextureManager Get()
		{
			if(null == instance)
				instance = new TextureManager();

			return instance;
		}

		public int LoadTextureFromFile(string file)
		{
			return LoadTextureFromFile(file, -1);
		}

		private int LoadTextureFromFile(string file, int oldId)
		{
			if(textureList.ContainsKey(file) && oldId == -1)
				return 0;
				
			int imageId = 0;
			Il.ilGenImages(1, out imageId);
			Il.ilBindImage(imageId);
			
			if(!Il.ilLoadImage(file))
			{
				Il.ilDeleteImages(1, ref imageId);
				EngineLog.Get().Error("Error loading "+file+" (DevIL: "+Il.ilGetError()+")", "Texture manager");
				return -1;
			}
			
			if(file.ToUpper().LastIndexOf(".TGA") == 0)
				Ilu.iluFlipImage();
			
			Texture tex = new Texture();

			tex.width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
			tex.height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
			tex.bpp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
			IntPtr Ptr = Il.ilGetData();

			Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
			if(oldId == -1)
			{
				//Gl.glGenTextures(1, new int[] { tex.id });				
				int[] texture = new int[1];
				Gl.glGenTextures(1, texture);
				tex.id = texture[0];
			} else
			{
				tex.id = oldId;
			}

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex.id);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
						
			int texType = Gl.GL_RGB;
			if(file.ToUpper().LastIndexOf(".TGA") != -1)
				texType = Gl.GL_BGR;
			if(tex.bpp == 32)
				texType = Gl.GL_RGBA;
			if(file.ToUpper().LastIndexOf(".TGA") != -1 && tex.bpp == 32)
				texType = Gl.GL_BGRA;
				
			int texDestType = Gl.GL_RGB;
			if(tex.bpp == 32)
				texDestType = Gl.GL_RGBA;
			
			Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, texDestType, tex.width,
				tex.height, texType, Gl.GL_UNSIGNED_BYTE, Ptr);
				
			Gl.glPopAttrib();

			if(oldId == -1)
				textureList.Add(file, tex);
			EngineLog.Get().Info(file+" loaded", "Texture manager");
			
			return 0;
		}

		public void SetTexture()
		{
			SetTexture("");
		}

		public void SetTexture(string file)
		{
			if("0" == file || "" == file)
			{
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
				lastTexture = 0;
			}
			else if(textureList.ContainsKey(file) &&
				((Texture)(textureList[file])).id != lastTexture) 
			{
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, ((Texture)(textureList[file])).id);
				lastTexture = ((Texture)(textureList[file])).id;
			}
		}
		
		public Texture GetTexture(string file)
		{
			if(textureList.ContainsKey(file))
				return (Texture)textureList[file];
				
			return null;
		}
		
		public void FreeImage(string file)
		{
			if(textureList.ContainsKey(file))
			{
				textureList.Remove(file);
			}
		}

		public void ReloadAll()
		{
			foreach(DictionaryEntry tex in textureList)
				LoadTextureFromFile((string)tex.Key, ((Texture)tex.Value).id);
		}
	}
}
