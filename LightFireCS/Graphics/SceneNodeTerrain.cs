//-----------------------------------------------------------------------------
//  SceneNodeTerrain.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Terrain scene node -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.Math;
using System.IO;

using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	public class SceneNodeTerrain : SceneNode
	{
		private Vector3	position = new Vector3();
		private Vector3	rotation = new Vector3();
		private BoundingBox mdlBoundingBox = new BoundingBox();
		private BoundingBox boundingBox = new BoundingBox();
		public Terrain terrain;
		
		public SceneNodeTerrain(Terrain terrain)
		{
			Vector3 bbMax = new Vector3(-1000000, -1000000, -1000000);
			Vector3 bbMin = new Vector3(1000000, 1000000, 1000000);

			this.terrain = terrain;
			for(int x = 0; x < terrain.Size; x++)
			{
				for(int y = 0; y < terrain.Size; y++)
				{
					double h = terrain.GetHeight(x, y);
					if(h > bbMax.y) bbMax.y = h;
					if(h < bbMin.y) bbMin.y = h;
				}
			}
			
			double scaleXZ = terrain.ScaleXZ;
			
			mdlBoundingBox = new BoundingBox(scaleXZ*terrain.Size, bbMax.y, scaleXZ*terrain.Size,
											0, bbMin.y, 0); 
			boundingBox = new BoundingBox(scaleXZ*terrain.Size, bbMax.y, scaleXZ*terrain.Size,
											0, bbMin.y, 0);
		}
		
		public override void SetPosition(Vector3 pos)
		{
			position = pos;
			boundingBox.max = mdlBoundingBox.max + pos;
			boundingBox.min = mdlBoundingBox.min + pos;
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
		}
				
		public override void Update()
		{
			foreach(SceneNode node in nodes)
				node.Update();
		}
		
		public override void Render(Frustum frustum)
		{
			if(!Visible)
				return;
			
			Gl.glPushMatrix();
			Gl.glTranslated(position.x, position.y, position.z);
			Gl.glRotated(rotation.x, 1, 0, 0);
			Gl.glRotated(rotation.y, 0, 1, 0);
			Gl.glRotated(rotation.z, 0, 0, 1);
			
			terrain.Render(frustum);			
			
			foreach(SceneNode n in nodes)
				n.Render(frustum);

			Gl.glPopMatrix();
		}
	}
}