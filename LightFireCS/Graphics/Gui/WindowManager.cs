//-----------------------------------------------------------------------------
//  WindowManager.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Window Manager behaves as a desktop for all windows -
//-----------------------------------------------------------------------------
using System;
using System.Collections;

namespace LightFireCS.Graphics.Gui
{
	public class WindowManager
	{
		private static WindowManager instance;
		private ArrayList windowList = new ArrayList();
		private long windowID;
		private bool windowActive;
		
		private WindowManager()
		{
			windowID = 0;
			
			LightFireCS.Input.IDevice IDevice = LightFireCS.Input.IDevice.Get();
			IDevice.MouseLeftDown += new EventHandler(OnLeftDown);
			IDevice.MouseMove += new EventHandler(OnMouseMove);
			IDevice.KeyDown += new KeyEventHandler(OnKeyDown);
			windowActive = false;
		}
		
		public static WindowManager Get()
		{
			if(null == instance)
				instance = new WindowManager();

			return instance;
		}
		
		public void RegisterWindow(Window window)
		{
			windowID++;
			window.Id = windowID;
			windowList.Add(window);
		}
		
		public void UnregisterWindow(Window window)
		{
			foreach(Window w in windowList)
			{
				if(w == window)
				{
					windowList.Remove(w);
					return;
				}
			}
		}
		
		public void UnregisterAll()
		{
			windowList.RemoveRange(0, windowList.Count);
		}
		
		public void MoveToTop(Window window)
		{			
			windowList.Reverse();
			foreach(Window w in windowList)
			{
				if(w == window)
				{
					Window tempw = w;
					windowList.Remove(w);
					windowList.Insert(0, tempw);
					break;
				}
			}
			windowList.Reverse();
		}
		
		public void Render()
		{
			foreach(Window w in windowList)
			{
				w.Render();
			}
		}
		
		public void OnLeftDown(object o, EventArgs e)
		{
			windowActive = false;
			int mouseX, mouseY;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mouseX, out mouseY);
			windowList.Reverse(); //TODO: Use inverse iteration!
			Window modalWindow = null;
			foreach(Window w in windowList)
			{
				if(w.Modal && null == modalWindow)
					modalWindow = w;

				if(mouseX >= w.Size.X1 && mouseX <= w.Size.X2 &&
					mouseY >= w.Size.Y1 && mouseY <= w.Size.Y2 &&
					w.Visible && (null == modalWindow || modalWindow == w))
				{
						windowActive = true;
						Window tempw = w;
						windowList.Remove(w);
						windowList.Insert(0, tempw);
						tempw.OnLeftDown(e);
						break;
				}
			}
			windowList.Reverse();
		}
		
		public void OnMouseMove(object o, EventArgs e)
		{
			int mouseX, mouseY;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mouseX, out mouseY);
			//windowList.Reverse(); //TODO: Use inverse iteration!
			Window modalWindow = null;
			Window hoverWindow = null;
			foreach(Window w in windowList)
			{
				if(w.Modal && null == modalWindow)
					modalWindow = w;

				if(mouseX >= w.Size.X1 && mouseX <= w.Size.X2 &&
					mouseY >= w.Size.Y1 && mouseY <= w.Size.Y2 &&
					w.Visible && (null == modalWindow || modalWindow == w))
				{
					windowActive = true;
					hoverWindow = w;
					//break;
				} else {
					w.OnMouseLeave(e);
				}
			}
			//windowList.Reverse();
			if(null != hoverWindow)
			{
				hoverWindow.OnMouseMove(e);
				hoverWindow.OnMouseEnter(e);
			}
		}
		
		public void OnKeyDown(object o, KeyEventArgs e)
		{
			if(!windowActive)
				return;
			
			if(windowList.Count > 0)
			{
				((Window)(windowList[windowList.Count-1])).OnKeyDown(e);
			}
		}
	}
}
