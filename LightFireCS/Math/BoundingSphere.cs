//-----------------------------------------------------------------------------
//  BoundingSphere.cs
//  Copyright (C) 2006 by Sebastian Pech
//  This file is part of the "LightFire#  Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Manage bounding spheres -
//-----------------------------------------------------------------------------
using System;
using LightFireCS.Math;
using Tao.OpenGl;

namespace LightFireCS.Math
{
	public class BoundingSphere
	{	
		public Vector3 center;
		public double radius;
		
		public BoundingSphere()
		{
			center = new Vector3(0, 0, 0);
			radius = 0.0;
		}
		
		public BoundingSphere(double cX, double cY, double cZ, double r)
		{
			center = new Vector3(cX, cY, cZ);
			radius = r;
		}
		
		public BoundingSphere(BoundingSphere bs)
		{
			center = new Vector3(bs.center.x, bs.center.y, bs.center.z);
			radius = bs.radius;
		}
		
		public BoundingSphere(BoundingBox bb)
		{
			center = new Vector3(0, 0, 0);
			center.x = (bb.min.x + bb.max.x)/2.0;
			center.y = (bb.min.y + bb.max.y)/2.0;
			center.z = (bb.min.z + bb.max.z)/2.0;
			
			Vector3 diag = new Vector3(0, 0, 0);
			diag.x = (bb.max.x - bb.min.x)/2.0;
			diag.y = (bb.max.y - bb.min.y)/2.0;
			diag.z = (bb.max.z - bb.min.z)/2.0;
			radius = diag.Length();
		}
		
		public void Render()
		{
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glBegin(Gl.GL_LINES);
				Gl.glColor3ub(255, 0, 0);		
				Gl.glVertex3d(center.x, center.y, center.z); // x-dir
				Gl.glVertex3d(center.x+radius, center.y, center.z);
				Gl.glVertex3d(center.x, center.y, center.z);
				Gl.glVertex3d(center.x-radius, center.y, center.z);
				Gl.glVertex3d(center.x, center.y, center.z); // y-dir
				Gl.glVertex3d(center.x, center.y+radius, center.z);
				Gl.glVertex3d(center.x, center.y, center.z);
				Gl.glVertex3d(center.x, center.y-radius, center.z);
				Gl.glVertex3d(center.x, center.y, center.z); // z-dir
				Gl.glVertex3d(center.x, center.y, center.z+radius);
				Gl.glVertex3d(center.x, center.y, center.z);
				Gl.glVertex3d(center.x, center.y, center.z-radius);
				Gl.glColor3ub(255, 255, 255);
			Gl.glEnd();
			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}
		
		public bool Collision(BoundingSphere checkSphere)
		{
			Vector3 diff = center - checkSphere.center;
			if(diff.Length() < (radius + checkSphere.radius))
				return false;
			else
				return true;
		}
	}
}