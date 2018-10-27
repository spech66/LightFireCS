//-----------------------------------------------------------------------------
//  SceneNodeQuadtree.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Quadtree scene node -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using LightFireCS.Math;
using System.IO;

using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	public class QuadtreeNode
	{
		private ArrayList childNodes = new ArrayList();
		private ArrayList sceneNodes = new ArrayList();
		private BoundingBox nodeBox;
		
		public QuadtreeNode(ArrayList sceneNodes, int depth, int maxDepth)
		{
			this.sceneNodes = sceneNodes;
			
			depth++;
			if(depth == maxDepth)
				return;

			nodeBox = new BoundingBox(-1000000, -1000000, -1000000, 1000000, 1000000, 1000000);
			foreach(SceneNode node in sceneNodes)
			{
				BoundingBox tempBB = node.GetBoundingBox();

				if(tempBB.min.x < nodeBox.min.x) nodeBox.min.x = tempBB.min.x;
				if(tempBB.min.y < nodeBox.min.y) nodeBox.min.y = tempBB.min.y;
				if(tempBB.min.z < nodeBox.min.z) nodeBox.min.z = tempBB.min.z;
			
				if(tempBB.max.x > nodeBox.max.x) nodeBox.max.x = tempBB.max.x;
				if(tempBB.max.y > nodeBox.max.y) nodeBox.max.y = tempBB.max.y;
				if(tempBB.max.z > nodeBox.max.z) nodeBox.max.z = tempBB.max.z;
			}
			
			ArrayList[] childSceneNodes = new ArrayList[4];
			for(int i = 0; i < 4; i++)
				childSceneNodes[i] = new ArrayList();
			
			double halfX = ((nodeBox.max.x - nodeBox.min.x)/2) + nodeBox.min.x;
			double halfY = ((nodeBox.max.y - nodeBox.min.y)/2) + nodeBox.min.y;
			foreach(SceneNode node in sceneNodes)
			{
				BoundingBox tempBB = node.GetBoundingBox();
					
				if(tempBB.min.x <= halfX && tempBB.min.y <= halfY)
					childSceneNodes[0].Add(node);

				if(tempBB.min.x > halfX && tempBB.min.y <= halfY)
					childSceneNodes[1].Add(node);
					
				if(tempBB.min.x <= halfX && tempBB.min.y > halfY)
					childSceneNodes[2].Add(node);

				if(tempBB.min.x > halfX && tempBB.min.y > halfY)
					childSceneNodes[3].Add(node);
			}
			
			for(int i = 0; i < 4; i++)
			{
				if(childSceneNodes[i].Count > 0)
				{
					QuadtreeNode tempNode = new QuadtreeNode(childSceneNodes[i], depth, maxDepth);
					childNodes.Add(tempNode);
				}
			}
		}
		
		public void DeleteChilds()
		{
			foreach(QuadtreeNode node in childNodes)
			{
				node.DeleteChilds();
			}
			childNodes.Clear();
		}
		
		public BoundingBox GetBoundingBox()
		{
			return nodeBox;
		}
		
		public void Render(Frustum frustum)
		{
			if(childNodes.Count == 0)
			{
				foreach(SceneNode sNode in sceneNodes)
					sNode.Render(frustum);
			} else {
				foreach(QuadtreeNode cNode in childNodes)
					cNode.Render(frustum);
			}
		}
	}

	public class SceneNodeQuadtree : SceneNode
	{
		private BoundingBox boundingBox = new BoundingBox();
		private QuadtreeNode rootNode;
		
		public SceneNodeQuadtree()
		{
			Visible = true;
		}

		public void BuildTree(int maxDepth)
		{
			rootNode = new QuadtreeNode(nodes, 0, maxDepth);
			boundingBox = rootNode.GetBoundingBox();
		}
		
		public void BuildTree()
		{
			BuildTree(10);
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
			return boundingBox.min;
		}
		
		public override Vector3 GetRotation()
		{
			return new Vector3();
		}

		public override void Resize()
		{
		}		

		public override void Update()
		{
			BuildTree();
		}
		
		public override void Render(Frustum frustum)
		{
			if(false == Visible)
				return;
				
			Gl.glPushMatrix();
			if(null != rootNode)
				rootNode.Render(frustum);	
			Gl.glPopMatrix();
		}
	}
}
