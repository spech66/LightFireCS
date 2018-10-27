//-----------------------------------------------------------------------------
//  BoundingBox.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire#  Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Manage bounding boxes -
//-----------------------------------------------------------------------------
using System;
using LightFireCS.Math;
using Tao.OpenGl;

namespace LightFireCS.Math
{
	public class BoundingBox
	{
		public Vector3 min;
		public Vector3 max;
		
		public BoundingBox()
		{
			min = new Vector3(1000000, 1000000, 1000000);
			max = new Vector3(-1000000, -1000000, -1000000);
		}
		
		public BoundingBox(double maxX, double maxY, double maxZ,
							double minX, double minY, double minZ)
		{
			min = new Vector3(minX, minY, minZ);
			max = new Vector3(maxX, maxY, maxZ);
		}
		
		public BoundingBox(BoundingBox bb)
		{
			min = new Vector3(bb.min.x, bb.min.y, bb.min.z);
			max = new Vector3(bb.max.x, bb.max.y, bb.max.z);
		}
		
		public void Render()
		{
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glBegin(Gl.GL_LINES);
				Gl.glColor3ub(255, 0, 0);			
				Gl.glVertex3d(min.x, min.y, max.z); //front bottom
				Gl.glVertex3d(max.x, min.y, max.z);		
				Gl.glVertex3d(min.x, max.y, max.z); //front top
				Gl.glVertex3d(max.x, max.y, max.z);			
				Gl.glVertex3d(min.x, min.y, max.z); //front left
				Gl.glVertex3d(min.x, max.y, max.z);
				Gl.glVertex3d(max.x, min.y, max.z); //front right
				Gl.glVertex3d(max.x, max.y, max.z);	
				Gl.glVertex3d(min.x, min.y, min.z); //back bottom
				Gl.glVertex3d(max.x, min.y, min.z);		
				Gl.glVertex3d(min.x, max.y, min.z); //back top
				Gl.glVertex3d(max.x, max.y, min.z);			
				Gl.glVertex3d(min.x, min.y, min.z); //back left
				Gl.glVertex3d(min.x, max.y, min.z);
				Gl.glVertex3d(max.x, min.y, min.z); //back right
				Gl.glVertex3d(max.x, max.y, min.z);			
				Gl.glVertex3d(min.x, min.y, min.z); //left bottom
				Gl.glVertex3d(min.x, min.y, max.z);
				Gl.glVertex3d(min.x, max.y, min.z); //left top
				Gl.glVertex3d(min.x, max.y, max.z);	
				Gl.glVertex3d(max.x, min.y, min.z); //right bottom
				Gl.glVertex3d(max.x, min.y, max.z);
				Gl.glVertex3d(max.x, max.y, min.z); //right top
				Gl.glVertex3d(max.x, max.y, max.z);
				Gl.glColor3ub(255, 255, 255);
			Gl.glEnd();
			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}
		
		public bool Collision(BoundingBox checkBox)
		{
			if(max.x < checkBox.min.x || min.x > checkBox.max.x)
				return false;
				
			if(max.y < checkBox.min.y || min.y > checkBox.max.y)
				return false;
				
			if(max.z < checkBox.min.z || min.z > checkBox.max.z)
				return false;

			return true;
		}
		
		public void GetFarPointsPN(Vector3 normal, out Vector3 p, out Vector3 n)
		{
			p = new Vector3();
			n = new Vector3();
		
			if(normal.x > 0)
			{
				p.x = max.x;
				n.x = min.x;
			} else {
				p.x = min.x;
				n.x = max.x;
			}
			
			if(normal.y > 0)
			{
				p.y = max.y;
				n.y = min.y;
			} else {
				p.y = min.y;
				n.y = max.y;
			}
			
			if(normal.z > 0)
			{
				p.z = max.z;
				n.z = min.z;
			} else {
				p.z = min.z;
				n.z = max.z;
			}
		}
	}
}