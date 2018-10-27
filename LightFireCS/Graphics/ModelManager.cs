//-----------------------------------------------------------------------------
//  ModelManager.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire#  Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Loads and stores models -
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections;
using LightFireCS.Math;

using Tao.OpenGl;
using Tao.DevIl;

namespace LightFireCS.Graphics
{
	public class ModelLoader
	{
		private static ModelLoader instance;
		private Hashtable modelList = new Hashtable();
		
		private ModelLoader()
		{
		}
		
		public static ModelLoader Get()
		{
			if(null == instance)
				instance = new ModelLoader();

			return instance;
		}
		
		private string ReadNames(ref BinaryReader binReader, int maxLen)
		{
			int i = 0;
			string s = "";
			char c;
			do
			{
				c = binReader.ReadChar();
				s += c;
				i++;
			}
			while(c != '\0' && i < maxLen);
			
			return s;
		}
		
		public int LoadModel(string file)
		{
			if(modelList.ContainsKey(file))
				return 0;
			
			BinaryReader binReader;
			binReader = new BinaryReader(File.Open(file, FileMode.Open));
			// ToDo: Error handling
			
			Model model = new Model();
			model.file = file;
			
			ModelMaterial modelMaterial = new ModelMaterial();
			ModelObject modelObject = new ModelObject();
			
			long fileLenght = binReader.BaseStream.Length;
			while(binReader.BaseStream.Position < fileLenght)
			{
				ushort 	chunkId = binReader.ReadUInt16();
				uint	chunkLen = binReader.ReadUInt32();

				switch(chunkId)
				{
					case 0x4d4d: // Main
					break;
					
					case 0x3d3d: // Editor
		        	break;
		        	
		        	case 0x4000: // Object
		        		model.objectList.Add(modelObject);
		        		modelObject = new ModelObject();
		        		modelObject.vertexList.Clear();
		        		modelObject.faceList.Clear();
		        		modelObject.normalList.Clear();
		        		modelObject.texcoordList.Clear();
		        		modelObject.name = ReadNames(ref binReader, 20);
		        	break;
		        	
		        	case 0x4100:
					break;
		 
					case 0x4110: // Vertices
						ushort lenv = binReader.ReadUInt16();
						for(int i = 0; i < lenv; i++)
						{
							Vector3 vec = new Vector3();
							vec.x = binReader.ReadSingle();
							vec.y = binReader.ReadSingle();
							vec.z = binReader.ReadSingle();				
							modelObject.vertexList.Add(vec);
						}
					break;
					
					case 0x4120: // Faces
						ushort lenf = binReader.ReadUInt16();	
						for(int i = 0; i < lenf; i++)
						{
							ModelFace modelFace = new ModelFace();
							modelFace.vertexIndex[0] = binReader.ReadUInt16();
							modelFace.vertexIndex[1] = binReader.ReadUInt16();
							modelFace.vertexIndex[2] = binReader.ReadUInt16();
							binReader.ReadUInt16();
							modelObject.faceList.Add(modelFace);
						}
					break;
					
					case 0x4130: // Material information
						modelObject.materialName = ReadNames(ref binReader, 20);
						binReader.BaseStream.Seek(chunkLen-6-
												modelObject.materialName.Length,
												SeekOrigin.Current);
					break;
					
					case 0x4140: // Texture coordinates
						ushort lent = binReader.ReadUInt16();
						for(int i = 0; i < lent; i++)
						{
							ModelTextureCoord modelTc = new ModelTextureCoord();
							modelTc.u = binReader.ReadSingle();
							modelTc.v = binReader.ReadSingle();
							modelObject.texcoordList.Add(modelTc);
						}
					break;
					
					case 0xAFFF: // Material
						model.materialList.Add(modelMaterial);
						modelMaterial = new ModelMaterial();
					break;
					
					case 0xA000:
						modelMaterial.name = ReadNames(ref binReader, 20);
					break;
					
					case 0xA200:
					break;

					case 0xA300:
						modelMaterial.textureName = ReadNames(ref binReader, 20);
					break;
					
					default:
						binReader.BaseStream.Seek(chunkLen-6,
													SeekOrigin.Current);
					break;
        		}
			}
			binReader.Close();
			
			model.objectList.Add(modelObject);
			model.materialList.Add(modelMaterial);
			
			model.objectList.RemoveAt(0);
			model.materialList.RemoveAt(0);
			
			foreach(ModelMaterial modelMat in model.materialList)
			{
				int pos = file.LastIndexOf("/", file.Length);
				string tmpStr = file.Substring(0, pos+1) + modelMat.textureName;
				modelMat.textureName = tmpStr;
				LightFireCS.Graphics.TextureManager.Get().LoadTextureFromFile(modelMat.textureName);
			}
		
			Console.WriteLine("Objects: " + model.objectList.Count);
			Console.WriteLine("Materials: " + model.materialList.Count);
			
            int faces = 0;
            foreach (ModelObject mdlObj in model.objectList)
                faces += mdlObj.faceList.Count;
            Console.WriteLine("Faces: " + faces);

			modelList.Add(file, model);
			EngineLog.Get().Info(file+" loaded", "Model manager");
					
			return 0;
		}
		
		public Model GetModel(string file)
		{
			return ((Model)(modelList[file]));
		}
		
			//void		FreeModel(string FileName);
			//void		RenderModel(string FileName);
	}
}