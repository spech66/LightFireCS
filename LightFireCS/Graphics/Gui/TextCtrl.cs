//-----------------------------------------------------------------------------
//  TextCtrl.cs
//  Copyright (C) 2004 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Text control -
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Tao.OpenGl;

namespace LightFireCS.Graphics.Gui
{
	public class TextCtrl : Window
	{
		private string content;
        private ArrayList renderList = new ArrayList();
	
		public TextCtrl(Window parent, Rect rect, string title, StyleFactory factory, string style):
					base(rect, title, factory, style)
		{
			parent.RegisterChild(((Window)this));
			
			if(null == font)
			{
				font = parent.WindowFont;
				fontColor = parent.FontColor;
			}

			rect.X1 += parent.Size.X1;
			rect.Y1 += parent.Size.Y1;
			rect.X2 += parent.Size.X1;
			rect.Y2 += parent.Size.Y1;
			
			content = "";
		}
		
		public TextCtrl(Window parent, Rect rect, string title, StyleFactory factory):
					this(parent, rect, title, factory, "textctrl") {}
		
		public override void OnKeyDown(KeyEventArgs e)
		{
			if(!Visible || Disabled)
				return;
			AppendText(e.input);
            Update();
		}

        public void Update()
        {
            if("" == content)
				return;

			renderList.Clear();
			Rect textArea = new Rect(background.Position);

			string[] text = content.Split('\n');
			int maxWidth = Convert.ToInt32(background.Position.X2 - background.Position.X1);
			foreach(string t in text)
			{
				int width = font.SizeText(t);
				if(width < maxWidth)
				{
					renderList.Add(t);
				} else {
					string[] words = t.Split(' ');
					ArrayList wordList = new ArrayList();
					for(int i = 0; i < words.Length; i++)
					{
						if(font.SizeText(words[i] + " ") >= maxWidth)
						{
							string rest = words[i];
							int blockSize = 1;
							while (rest.Length > blockSize)
							{
								wordList.Add(rest.Substring(0, blockSize));
								rest = rest.Substring(blockSize);
							}
							wordList.Add(rest + " ");
						} else {
							wordList.Add(words[i] + " ");
						}
					}

					for(int i = 0; i < wordList.Count; i++)
					{
						string renderText = "";
						while(i < wordList.Count && font.SizeText(renderText + wordList[i]) < maxWidth)
						{
							renderText += wordList[i];
							i++;
						}
						i--;
						renderList.Add(renderText);
					}
				}
			}
        }

		public override void Render()
		{
			if(!Visible)
				return;
				
			if(Disabled && !Hovered)
			{
				backgroundDisabled.Render();
			} else if(Hovered) {
				backgroundHover.Render(); 
			} else {
				background.Render();
			}

			Rect textArea = new Rect(background.Position);
			for(int i = 0; i < renderList.Count; i++)
			{
				if(textArea.Y1 + font.Size.Y1 >= background.Position.Y2)
					break;
				font.Render((string)renderList[i], fontColor, textArea);
				textArea.Y1 += font.Size.Y1;
			}
			
			foreach(Window w in childs)
			{
				w.Render();
			}
		}
		
		public void AppendText(string text)
		{
			if(text == "\b")
			{
				if(content.Length > 0)
					content = content.Substring(0, content.Length-1);
			} else {
				content += text;
			}

            Update();
		}
	}
}