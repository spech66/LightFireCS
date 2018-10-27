//-----------------------------------------------------------------------------
//  Frustum.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Plane -
//-----------------------------------------------------------------------------

using System;
using Tao.OpenGl;

namespace LightFireCS.Math
{
	/// <summary>
	/// Plane
	/// </summary>
	public class Plane
	{
		public double a, b, c, d;
		public Vector3 normal;
		
		public void Normalize()
		{
			normal = new Vector3(a, b, c);
			d = d / normal.Length();
			normal.Normalize();
		}
		
		public double Distance(Vector3 p)
		{
			return d + Vector3.DotProduct(normal, p);
		}
		
		public bool PointInside(Vector3 p)
		{
			if((Vector3.DotProduct(normal, p)) + d < 0)
				return false; // Outside
			
			return true;
		}
	}
	
	/// <summary>
	/// Plane
	/// </summary>
	public class Frustum
	{
		private Plane[] planes = new Plane[6];
		
		public Frustum(Matrix4 mat)
		{		
			planes[0] = new Plane();
			planes[0].a = mat[4,1] + mat[1,1]; // Left
			planes[0].b = mat[4,2] + mat[1,2];
			planes[0].c = mat[4,3] + mat[1,3];
			planes[0].d = mat[4,4] + mat[1,4];
			
			planes[1] = new Plane();
			planes[1].a = mat[4,1] - mat[1,1]; // Right
			planes[1].b = mat[4,2] - mat[1,2];
			planes[1].c = mat[4,3] - mat[1,3];
			planes[1].d = mat[4,4] - mat[1,4];
			
			planes[2] = new Plane();	
			planes[2].a = mat[4,1] - mat[2,1]; // Top
			planes[2].b = mat[4,2] - mat[2,2];
			planes[2].c = mat[4,3] - mat[2,3];
			planes[2].d = mat[4,4] - mat[2,4];
			
			planes[3] = new Plane();
			planes[3].a = mat[4,1] + mat[2,1]; // Bottom
			planes[3].b = mat[4,2] + mat[2,2];
			planes[3].c = mat[4,3] + mat[2,3];
			planes[3].d = mat[4,4] + mat[2,4];
			
			planes[4] = new Plane();		
			planes[4].a = mat[4,1] + mat[3,1]; // Near
			planes[4].b = mat[4,2] + mat[3,2];
			planes[4].c = mat[4,3] + mat[3,3];
			planes[4].d = mat[4,4] + mat[3,4];
			
			planes[5] = new Plane();
			planes[5].a = mat[4,1] + mat[3,1]; // Far
			planes[5].b = mat[4,2] + mat[3,2];
			planes[5].c = mat[4,3] + mat[3,3];
			planes[5].d = mat[4,4] + mat[3,4];
			
			for(int i = 0; i < 6; i++)
				planes[i].Normalize();
		}
		
		public bool PointInside(Vector3 p)
		{
			for(int i = 0; i < 6; i++)
			{
				if(!planes[i].PointInside(p))
					return false;
			}
			
			return true;
		}
		
		public bool BoundingBoxInside(BoundingBox bb)
		{
			bool inside = true; //TODO: replace by enum out\in\intersect
			
			for(int i = 0; i < 6; i++)
			{
				Vector3 p, n;
				bb.GetFarPointsPN(planes[i].normal, out p, out n);
				
				if(planes[i].PointInside(p) == false)
					return false;
					
				//if(planes[i].PointInside(p) == false)
				//	inside = true; // intersect
			}
						
			return inside;
		}
		
		public bool BoundingSphereInside(BoundingSphere bs)
		{
			double distance;
			bool inside = true;
			
			for(int i = 0; i < 6; i++)
			{
				distance = planes[i].Distance(bs.center);
				if(distance < -bs.radius)
					return false;
				//else if(distance < bs.radius)
				//	inseide = true; // intersect
			}
			
			return inside;
		}
	}
}