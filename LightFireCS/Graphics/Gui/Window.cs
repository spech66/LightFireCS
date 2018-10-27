//-----------------------------------------------------------------------------
//  Window.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Window`s contain all controls -
//-----------------------------------------------------------------------------
using System;
using System.Xml.XPath;
using System.Collections;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public class Window
	{
		private bool modal = false;
		private bool visible = true;
		private bool disabled = false;
		private bool hovered = false;

		protected long childId = 0;
		protected Box background;
		protected Box backgroundHover;
		protected Box backgroundDisabled;
		protected string title;
		protected ArrayList childs = new ArrayList();
		protected Font font;
		protected Color fontColor;
		
		public long Id { get; set; }
		public Rect Size { get { return background.Position; } }
		public Font WindowFont { get { return font; } }
		public Color FontColor { get { return fontColor; } }
		public bool Visible { get { return visible; } }
		public bool Modal { get { return modal; } }
		public bool Disabled { get { return disabled; } set { disabled = value; } }
		public bool Hovered { get { return hovered; } set { hovered = value; } }

		//public event EventHandler Activated;
		//public event EventHandler Closing;
		//public event EventHandler Moved;
		//public event EventHandler Resized;
		//public event EventHandler KeyUp;
		//public event EventHandler KeyDown;
		//public event EventHandler LeftDoubleClick;
		public event EventHandler LeftDown;
		//public event EventHandler LeftUp;
		//public event EventHandler RightDoubleClick;
		//public event EventHandler RightDown;
		//public event EventHandler RightUp;
		public event EventHandler MouseEnter;
		public event EventHandler MouseLeave;
		public event EventHandler MouseMove;

		public string GetAttribute(XPathNodeIterator xni, string attribute)
		{
			try
			{
				return xni.Current.GetAttribute(attribute, "");
			} catch {
				return "";
			}
		}

		public Window(Rect rect, string title, StyleFactory factory, string style)
		{
			Id = 0;

			background = new Box(rect, "");
			backgroundHover = new Box(rect, "");
			backgroundDisabled = new Box(rect, "");
			this.title = title;

			background.SetTexture(factory.GetValue(style, "background", "img"));
			background.Color1 = new Color(factory.GetValue(style, "background", "color"));
			backgroundHover.SetTexture(factory.GetValue(style, "backgroundHover", "img"));
			backgroundHover.Color1 = new Color(factory.GetValue(style, "backgroundHover", "color"));
			backgroundDisabled.SetTexture(factory.GetValue(style, "backgroundDisabled", "img"));
			backgroundDisabled.Color1 = new Color(factory.GetValue(style, "backgroundDisabled", "color"));
						
			string fontfile = factory.GetValue(style, "font", "file");
			string fontsize = factory.GetValue(style, "font", "size");
			if("" != fontfile)
			{
				font = new Font();
				if(0 != font.LoadFont(fontfile, Convert.ToInt32(fontsize)))
					font = null;
				fontColor = new Color(factory.GetValue(style, "font", "color"));
			} else {
				font = null;
			}
		}

		public Window(Rect rect, string title, StyleFactory factory): this(rect, title, factory, "window") {}
						
		public void RegisterChild(Window child)
		{
			childId++;
			child.Id = Id * 1000 + childId;
			childs.Add(child);
		}
		
		public void UnregisterChild(Window child)
		{
			foreach(Window w in childs)
			{
				if(w == child)
				{
					childs.Remove(w);
					return;
				}
			}
		}
		
		public void UnregisterAll()
		{
			childs.RemoveRange(0, childs.Count);
		}
		
		public void MakeActive(Window child)
		{			
			childs.Reverse();
			foreach(Window w in childs)
			{
				if(w == child)
				{
					Window tempw = w;
					childs.Remove(w);
					childs.Insert(0, tempw);
					break;
				}
			}
			childs.Reverse();
		}
		
		public void Show()
		{
			visible = true;
			modal = false;
			WindowManager.Get().MoveToTop(this);
		}
		
		public void ShowModal()
		{
			visible = true;
			modal = true;
			WindowManager.Get().MoveToTop(this);
		}
		
		public void Hide()
		{
			visible = false;
		}
		
		public virtual void OnMouseMove(EventArgs e)
		{
			if(null != MouseMove)
				MouseMove(this, EventArgs.Empty);
		}
		
		public virtual void OnLeftDown(EventArgs e)
		{
			if(disabled)
				return;
			
			int mouseX, mouseY;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mouseX, out mouseY);
			
			foreach(Window w in childs)
			{
				if(mouseX >= w.Size.X1 && mouseX <= w.Size.X2 &&
					mouseY >= w.Size.Y1 && mouseY <= w.Size.Y2)
				{
					MakeActive(w);
					w.OnLeftDown(e);
					break;
				}
			}
			
			if(null != LeftDown)
				LeftDown(this, EventArgs.Empty);
		}
		
		public virtual void OnKeyDown(KeyEventArgs e)
		{
			if(disabled)
				return;

			if(childs.Count > 0)
			{
				//((Window)(childs[0])).OnKeyDown(e);
				((Window)(childs[childs.Count-1])).OnKeyDown(e);
			}
		}
		
		public virtual void OnMouseEnter(EventArgs e)
		{
			int mouseX, mouseY;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mouseX, out mouseY);
			
			foreach(Window w in childs)
			{
				if(mouseX >= w.Size.X1 && mouseX <= w.Size.X2 &&
					mouseY >= w.Size.Y1 && mouseY <= w.Size.Y2)
				{
					w.OnMouseEnter(e);
				} else {
					w.OnMouseLeave(e);
				}
			}
			
			if(Hovered)
				return;
			hovered = true;
			
			if(null != MouseEnter)
				MouseEnter(this, EventArgs.Empty);
		}
		
		public virtual void OnMouseLeave(EventArgs e)
		{
			if(!Hovered)
				return;

			hovered = false;
			
			if(null != MouseLeave)
				MouseLeave(this, EventArgs.Empty);

			int mouseX, mouseY;
			LightFireCS.Input.IDevice.Get().GetMousePos(out mouseX, out mouseY);

			foreach (Window w in childs)
			{
				if (mouseX < w.Size.X1 || mouseX > w.Size.X2 ||
					mouseY < w.Size.Y1 || mouseY > w.Size.Y2)
				{
					w.OnMouseLeave(e);
				}
			}		
		}
		
		public virtual void Render()
		{
			if(!visible)
				return;
			
			if(disabled && !hovered)
			{
				backgroundDisabled.Render();
			} else if(hovered) {
				backgroundHover.Render(); 
			} else {
				background.Render();
			}
			
			if(title != "" && font != null)
				font.Render(title, fontColor, background.Position, FontVAlign.Center, FontHAlign.Top);
			
			foreach(Window w in childs)
			{
				w.Render();
			}
		}
	}
}