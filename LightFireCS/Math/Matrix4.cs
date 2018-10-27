//-----------------------------------------------------------------------------
//  Matrix4.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - 4D Matrix -
//-----------------------------------------------------------------------------

using System;

namespace LightFireCS.Math
{
	/// <summary>
	/// 4 Dimensional Matrix
	/// </summary>
	public class Matrix4
	{
		public double[,] m = new double[4,4];
		
		public double this[int i, int j] { get { return m[i-1, j-1]; } set { m[i-1, j-1] = value; } }

		public Matrix4()
		{
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					m[i, j] = 0;
				}
			}
		}
		
		// For OpenGL glMatrixLoad!
		public Matrix4(double[] mat)
		{
			if(mat.Length != 16)
				throw(new Exception("Matrix dimension must be 4x4"));

			m[0, 0] = mat[0];	m[0, 1] = mat[1];	m[0, 2] = mat[2];	m[0, 3] = mat[3];
			m[1, 0] = mat[4];	m[1, 1] = mat[5];	m[1, 2] = mat[6];	m[1, 3] = mat[7];
			m[2, 0] = mat[8];	m[2, 1] = mat[9];	m[2, 2] = mat[10];	m[2, 3] = mat[11];
			m[3, 0] = mat[12];	m[3, 1] = mat[13];	m[3, 2] = mat[14];	m[3, 3] = mat[15];
		}
		
		public void Identity()
		{
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					if(i == j)
						m[i, j] = 1;
					else
						m[i, j] = 0;
				}
			}
		}
		
		public static Matrix4 operator *(Matrix4 mat1, Matrix4 mat2)
		{
			Matrix4 temp = new Matrix4();
			
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					temp.m[i, j] = 0.0;
					for(int k = 0; k < 4; k++)
					{
						temp.m[i, j] += mat1.m[i, k] * mat2.m[k, j];
					}
				}
			}

			return temp;
		}
		
		// Function taken from Intel's developer pages
		// Thanks for the fast algorithm
		public Matrix4 Invert()
		{
			double[] dst = new double[16];
			double[] tmp = new double[12]; /* temp array for pairs */
			double[] src = new double[16]; /* array of transpose source matrix */
			double det; /* determinant */
			/* transpose matrix */
			for(int i = 0; i < 4; i++)
			{
				src[i] = m[i, 0];
				src[i + 4] = m[i, 1];
				src[i + 8] = m[i, 2];
				src[i + 12] = m[i, 3];
			}
			/* calculate pairs for first 8 elements (cofactors) */
			tmp[0] = src[10] * src[15];
			tmp[1] = src[11] * src[14];
			tmp[2] = src[9] * src[15];
			tmp[3] = src[11] * src[13];
			tmp[4] = src[9] * src[14];
			tmp[5] = src[10] * src[13];
			tmp[6] = src[8] * src[15];
			tmp[7] = src[11] * src[12];
			tmp[8] = src[8] * src[14];
			tmp[9] = src[10] * src[12];
			tmp[10] = src[8] * src[13];
			tmp[11] = src[9] * src[12];
			/* calculate first 8 elements (cofactors) */
			dst[0] = tmp[0] * src[5] + tmp[3] * src[6] + tmp[4] * src[7];
			dst[0] -= tmp[1] * src[5] + tmp[2] * src[6] + tmp[5] * src[7];
			dst[1] = tmp[1] * src[4] + tmp[6] * src[6] + tmp[9] * src[7];
			dst[1] -= tmp[0] * src[4] + tmp[7] * src[6] + tmp[8] * src[7];
			dst[2] = tmp[2] * src[4] + tmp[7] * src[5] + tmp[10] * src[7];
			dst[2] -= tmp[3] * src[4] + tmp[6] * src[5] + tmp[11] * src[7];
			dst[3] = tmp[5] * src[4] + tmp[8] * src[5] + tmp[11] * src[6];
			dst[3] -= tmp[4] * src[4] + tmp[9] * src[5] + tmp[10] * src[6];
			dst[4] = tmp[1] * src[1] + tmp[2] * src[2] + tmp[5] * src[3];
			dst[4] -= tmp[0] * src[1] + tmp[3] * src[2] + tmp[4] * src[3];
			dst[5] = tmp[0] * src[0] + tmp[7] * src[2] + tmp[8] * src[3];
			dst[5] -= tmp[1] * src[0] + tmp[6] * src[2] + tmp[9] * src[3];
			dst[6] = tmp[3] * src[0] + tmp[6] * src[1] + tmp[11] * src[3];
			dst[6] -= tmp[2] * src[0] + tmp[7] * src[1] + tmp[10] * src[3];
			dst[7] = tmp[4] * src[0] + tmp[9] * src[1] + tmp[10] * src[2];
			dst[7] -= tmp[5] * src[0] + tmp[8] * src[1] + tmp[11] * src[2];
			/* calculate pairs for second 8 elements (cofactors) */
			tmp[0] = src[2] * src[7];
			tmp[1] = src[3] * src[6];
			tmp[2] = src[1] * src[7];
			tmp[3] = src[3] * src[5];
			tmp[4] = src[1] * src[6];
			tmp[5] = src[2] * src[5];

			tmp[6] = src[0] * src[7];
			tmp[7] = src[3] * src[4];
			tmp[8] = src[0] * src[6];
			tmp[9] = src[2] * src[4];
			tmp[10] = src[0] * src[5];
			tmp[11] = src[1] * src[4];
 
			/* calculate second 8 elements (cofactors) */
			dst[8] = tmp[0] * src[13] + tmp[3] * src[14] + tmp[4] * src[15];
			dst[8] -= tmp[1] * src[13] + tmp[2] * src[14] + tmp[5] * src[15];
			dst[9] = tmp[1] * src[12] + tmp[6] * src[14] + tmp[9] * src[15];
			dst[9] -= tmp[0] * src[12] + tmp[7] * src[14] + tmp[8] * src[15];
			dst[10] = tmp[2] * src[12] + tmp[7] * src[13] + tmp[10] * src[15];
			dst[10] -= tmp[3] * src[12] + tmp[6] * src[13] + tmp[11] * src[15];
			dst[11] = tmp[5] * src[12] + tmp[8] * src[13] + tmp[11] * src[14];
			dst[11] -= tmp[4] * src[12] + tmp[9] * src[13] + tmp[10] * src[14];
			dst[12] = tmp[2] * src[10] + tmp[5] * src[11] + tmp[1] * src[9];
			dst[12] -= tmp[4] * src[11] + tmp[0] * src[9] + tmp[3] * src[10];
			dst[13] = tmp[8] * src[11] + tmp[0] * src[8] + tmp[7] * src[10];
			dst[13] -= tmp[6] * src[10] + tmp[9] * src[11] + tmp[1] * src[8];
			dst[14] = tmp[6] * src[9] + tmp[11] * src[11] + tmp[3] * src[8];
			dst[14] -= tmp[10] * src[11] + tmp[2] * src[8] + tmp[7] * src[9];
			dst[15] = tmp[10] * src[10] + tmp[4] * src[8] + tmp[9] * src[9];
			dst[15] -= tmp[8] * src[9] + tmp[11] * src[10] + tmp[5] * src[8];
 
			/* calculate determinant */
			det = src[0] * dst[0] + src[1] * dst[1] + src[2] * dst[2] + src[3] * dst[3];
 
			/* calculate matrix inverse */
			det = 1 / det;
			for ( int j = 0; j < 16; j ++ )
				dst[j] *= det;
 
			return new Matrix4(dst);
		}
		
		public Matrix4 Transpose()
		{
			Matrix4 t = new Matrix4();
			
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					t.m[i, j] = m[j, i];
			
			return t;		
		}
		
		public void Print()
		{
			Console.WriteLine("{0:f} {1:f} {2:f} {3:f}", m[0, 0], m[0, 1], m[0, 2], m[0, 3]);
			Console.WriteLine("{0:f} {1:f} {2:f} {3:f}", m[1, 0], m[1, 1], m[1, 2], m[1, 3]);
			Console.WriteLine("{0:f} {1:f} {2:f} {3:f}", m[2, 0], m[2, 1], m[2, 2], m[2, 3]);
			Console.WriteLine("{0:f} {1:f} {2:f} {3:f}", m[3, 0], m[3, 1], m[3, 2], m[3, 3]);
		}
	}
}