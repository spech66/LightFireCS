//-----------------------------------------------------------------------------
//  SceneNode.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Interface for scene nodes -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.Math;

using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	public class SceneNodeModel : SceneNode
	{
		private Vector3	position = new Vector3();
		private Vector3	rotation = new Vector3();
		private BoundingBox mdlBoundingBox = new BoundingBox();
		private BoundingBox boundingBox = new BoundingBox();
		private Model model;
		
		public SceneNodeModel(Model model)
		{
			Visible = true;
			this.model = model;
			
			Vector3 bbMax = new Vector3(-1000000, -1000000, -1000000);
			Vector3 bbMin = new Vector3(1000000, 1000000, 1000000);
			foreach(ModelObject obj in model.objectList)
			{
				foreach(Vector3 vec in obj.vertexList)
				{
					if(vec.x > bbMax.x) bbMax.x = vec.x;
					if(vec.y > bbMax.y) bbMax.y = vec.y;
					if(vec.z > bbMax.z) bbMax.z = vec.z;
					if(vec.x < bbMin.x) bbMin.x = vec.x;
					if(vec.y < bbMin.y) bbMin.y = vec.y;
					if(vec.z < bbMin.z) bbMin.z = vec.z;
				}
			}
			mdlBoundingBox = new BoundingBox(bbMax.x, bbMax.y, bbMax.z,
											bbMin.x, bbMin.y, bbMin.z); 
			boundingBox = new BoundingBox(bbMax.x, bbMax.y, bbMax.z,
											bbMin.x, bbMin.y, bbMin.z);
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
			if(false == Visible)
				return;
			
			Gl.glPushMatrix();
			Gl.glTranslated(position.x, position.y, position.z);
			Gl.glRotated(rotation.x, 1, 0, 0);
			Gl.glRotated(rotation.y, 0, 1, 0);
			Gl.glRotated(rotation.z, 0, 0, 1);
			
			model.Render();			
			
			foreach(SceneNode n in nodes)
				n.Render(frustum);

			Gl.glPopMatrix();
		}
	}
}