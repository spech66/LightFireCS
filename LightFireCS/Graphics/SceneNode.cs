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

namespace LightFireCS.Graphics
{
	public abstract class SceneNode
	{
		protected ArrayList nodes = new ArrayList();

		public bool Visible { get; set; }

		public SceneNode()
		{
			Visible = true;
		}
	
		public void AddNode(SceneNode node)
		{
			nodes.Add(node);
		}
		
		public void DeleteNode(SceneNode node)
		{
			node.DeleteNodes();
			nodes.Remove(node);
		}
		
		public void DeleteNodes()
		{
			foreach(SceneNode n in nodes)
				n.DeleteNodes();
			nodes.Clear();
		}

		public ArrayList GetNodes()
		{
			return nodes;
		}
		
		public abstract void SetPosition(Vector3 pos);
		public abstract Vector3 GetPosition();		
		public abstract void SetRotation(Vector3 rot);
		public abstract Vector3 GetRotation();		
		public abstract BoundingBox GetBoundingBox();
		public abstract void Resize();
		public abstract void Update();
		public abstract void Render(Frustum frustum);
	}
}