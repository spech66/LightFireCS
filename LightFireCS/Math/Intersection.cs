//-----------------------------------------------------------------------------
//  Intersection.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Intersection between objects -
//-----------------------------------------------------------------------------

using System;

namespace LightFireCS.Math
{
	/// <summary>
	/// Intersection between Objects
	/// </summary>
	public class Intersection
	{
		public static bool RayTriangle(Vector3 rO, Vector3 rD, Vector3 v0, Vector3 v1, Vector3 v2)
		{
			Vector3 edge1 = v1 - v0;
			Vector3 edge2 = v2 - v0;
			
			Vector3 p = Vector3.CrossProduct(rD, edge2);
			double determinant = Vector3.DotProduct(edge1, p);
	
			double epsilon = 0.00001;
			if(determinant > -epsilon && determinant < epsilon)
				return false;
	
			double invDet = 1.0 / determinant;
			Vector3 dist = rO - v1;
			
			double u = Vector3.DotProduct(dist, p) * invDet;
			if(u < 0.0 || u > 1.0)
				return false;
				
			Vector3 uVec = Vector3.CrossProduct(dist, edge1);
			double v = Vector3.DotProduct(rD, uVec) * invDet;
			if(v < 0.0 || v > 1.0)
				return false;
				
			return true;
		}
		
		/// <summary>
		/// Intersection of Ray and BoundingBox
		/// </summary>
		// Original code by Andrew Woo
		// "Graphics Gems", Academic Press, 1990
		public static bool RayBoundingBox(Vector3 rO, Vector3 rD, BoundingBox box, out double[] point)
		{
			bool originInside = true;
			point = new double[3];
			double[] origin = rO.GetArray();
			double[] dir = rD.GetArray();
			double[] boxMax = box.max.GetArray();
			double[] boxMin = box.min.GetArray();
			double[] maxT = new double[3];
			double[] quadrant = new double[3];
			double[] plane = new double[3];

			for(int i = 0; i < 3; i++)
			{
				if(origin[i] < boxMin[i])
				{
					quadrant[i] = 1;
					plane[i] = boxMin[i];
					originInside = false;
				} else if (origin[i] > boxMax[i]) {
					quadrant[i] = 0;
					plane[i] = boxMax[i];
					originInside = false;					
				} else {
					quadrant[i] = 2;
				}
			}
			
			if(originInside)
			{
				point = origin;
				return true;
			}
				
			for(int i = 0; i < 3; i++)
			{
				if(quadrant[i] != 2 && dir[i] != 0)
					maxT[i] = (plane[i] - origin[i]) / dir[i];
				else
					maxT[i] = -1;
			}

			int whichPlane = 0;
			for(int i = 0; i < 3; i++)
			{
				if(maxT[whichPlane] < maxT[i])
					whichPlane = i;
			}

			if(maxT[whichPlane] < 0)
				return false;
				
			for(int i = 0; i < 3; i++)
			{
				if(whichPlane != i)
				{
					point[i] = origin[i] + maxT[whichPlane] * dir[i];
					if(point[i] < boxMin[i] || point[i] > boxMax[i])
						return false;
				} else {
					point[i] = plane[i];
				}
			}
			
			return true;
		}
	}
}
