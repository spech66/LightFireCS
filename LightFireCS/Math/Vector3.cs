//-----------------------------------------------------------------------------
//  Vector3.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - 3D Vector -
//-----------------------------------------------------------------------------

using System;

namespace LightFireCS.Math
{
	/// <summary>
	/// 3 Dimensional Vector
	/// </summary>
	public class Vector3
	{
		public double x, y, z;

		/// <summary>Creates a null-vector</summary>
		public Vector3()
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}
		
		/// <summary>Copy constructur creates a new Vector from an existing.</summary>
		/// <param name="vec">The Vector to copy</param>
		public Vector3(Vector3 vec)
		{
			x = vec.x;
			y = vec.y;
			z = vec.z;
		}

		public Vector3(double X, double Y, double Z)
		{
			x = X;
			y = Y;
			z = Z;
		}
		
		public Vector3(string xyz)
		{
			string[] values = xyz.Split(' ');
			x = Convert.ToByte(values[0]);
			y = Convert.ToByte(values[1]);
			z = Convert.ToByte(values[2]);
		}

		public static Vector3 operator+(Vector3 Vector, Vector3 Vector2)
		{
			return new Vector3(	Vector.x + Vector2.x,
								Vector.y + Vector2.y,
								Vector.z + Vector2.z);
		}

		public static Vector3 operator-(Vector3 Vector, Vector3 Vector2)
		{
			return new Vector3(	Vector.x - Vector2.x,
								Vector.y - Vector2.y,
								Vector.z - Vector2.z);
		}
		
		public static Vector3 operator-(Vector3 vec)
		{
			return new Vector3(-vec.x, -vec.y, -vec.z);
		}

		public static Vector3 operator *(Vector3 Vector, Vector3 Vector2)
		{
			return new Vector3(	Vector.x * Vector2.x,
								Vector.y * Vector2.y,
								Vector.z * Vector2.z);
		}
		
		public static Vector3 operator *(Vector3 Vector, double scal)
		{
			return new Vector3(	Vector.x * scal,
								Vector.y * scal,
								Vector.z * scal);
		}

		public static Vector3 operator/(Vector3 Vector, Vector3 Vector2)
		{
			return new Vector3(	Vector.x / Vector2.x,
								Vector.y / Vector2.y,
								Vector.z / Vector2.z);
		}
		
		public static Vector3 CalcNormal(Vector3 vert1, Vector3 vert2, Vector3 vert3)
	    {
	    	Vector3 vec1 = new Vector3();
	    	Vector3 vec2 = new Vector3();
	    	
	    	vec1 = vert1 - vert2;
	    	vec2 = vert1 - vert3;
	    	
	    	Vector3 vecOut = new Vector3();
	    	vecOut = Vector3.CrossProduct(vec1, vec2);
	    	vecOut.Normalize();
	    	return vecOut;
	    }		
		
		public static Vector3 CrossProduct(Vector3 u, Vector3 v)
		{
			return new Vector3( u.y * v.z - u.z * v.y,
								u.z * v.x - u.x * v.z,
								u.x * v.y - u.y * v.x);
		}
		
		public static double Angle(Vector3 u, Vector3 v)
		{
			double dotProd = DotProduct(u, v);
			double norm = u.Length() * v.Length();
			double angle = System.Math.Acos(dotProd / norm);
			return angle;
		}

		public static double DotProduct(Vector3 u, Vector3 v)
		{
			return u.x*v.x + u.y*v.y + u.z*v.z;
		}		
		
		public double Length()
		{
			return System.Math.Sqrt(x*x + y*y + z*z);
		}
		
		public void Normalize()
		{
			double l = Length();
			x = x/l;
			y = y/l;
			z = z/l;
		}
		
		public double[] GetArray()
		{
			double[] r = new double[3];
			r[0] = x;
			r[1] = y;
			r[2] = z;
			return r;
		}
	}
}
