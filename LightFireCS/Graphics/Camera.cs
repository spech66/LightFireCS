//-----------------------------------------------------------------------------
//  Camera.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Camera -
//-----------------------------------------------------------------------------
using System;
using LightFireCS.Math;
using Tao.OpenGl;

namespace LightFireCS.Graphics
{
	/// <summary>
	/// Summary description for Camera.
	/// </summary>
	public class Camera
	{
		private Vector3 position;
		private Vector3 rotation;
		private Vector3 target;
		private double	distance;
		private Frustum frustum;

		public Vector3 Position { get { return position; } }
		public Vector3 Rotation { get { return rotation; } }
		public Vector3 Target { get { return target; } }
		public double Distance { get { return distance; }
								set { distance = value; } }
		public Frustum ViewFrustum { get { return frustum; } }

		public Camera()
		{
			position = new Vector3();
			rotation = new Vector3();
			target = new Vector3();
			distance = 1;
		}

		public void SetPosition(double x, double y, double z)
		{
			position.x = x;
			position.y = y;
			position.z = z;
		}

		public void SetRotation(double x, double y, double z)
		{
			rotation.x = x;
			rotation.y = y;
			rotation.z = z;
		}

		public void SetTarget(double x, double y, double z)
		{
			target.x = x;
			target.y = y;
			target.z = z;
		}

		public void MoveCamera(double x, double y, double z)
		{
			position.x += x;
			position.y += y;
			position.z += z;
		}

		public void RotateCamera(double x, double y, double z)
		{
			rotation.x += x;
			rotation.y += y;
			rotation.z += z;
			if(rotation.x > 360.0f) rotation.x -= 360.0f;
			if(rotation.y > 360.0f) rotation.y -= 360.0f;
			if(rotation.z > 360.0f) rotation.z -= 360.0f;
		}

		public void UpdateCamera()
		{
			Vector3 cameraPosition = new Vector3(position);
			cameraPosition.x += distance * System.Math.Sin(rotation.y * System.Math.PI / 180.0);
			cameraPosition.y += distance * System.Math.Sin(rotation.z * System.Math.PI / 180.0);
			cameraPosition.z += distance * System.Math.Cos(rotation.y * System.Math.PI / 180.0);

			Glu.gluLookAt(cameraPosition.x, cameraPosition.y, cameraPosition.z,
				position.x, position.y, position.z, 0, 1, 0);
			
			UpdateFrustum();
		}
		
		public void UpdatePOCamera()
		{
			Glu.gluLookAt(position.x, position.y, position.z,
				target.x, target.y, target.z, 0, 1, 0);
				
			UpdateFrustum();
		}
						
		private void UpdateFrustum()
		{
			double[] dMatProj = new double[16];
			double[] dMatModel = new double[16];
			
			Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, dMatProj);
			Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, dMatModel);
			
			Matrix4 matProj = new Matrix4(dMatProj);
			Matrix4 matModel = new Matrix4(dMatModel);
			Matrix4 clip = matModel * matProj;

			clip = clip.Transpose();
			frustum = new Frustum(clip);
		}
	}
}
