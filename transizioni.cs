/**************************************************************
 		Davide Infantino - 2019
    This file is part of Sfondo.exe

    Sfondo.exe is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Nome-Programma is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Nome-Programma.If not, see<http://www.gnu.org/licenses/>.
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfondo
{
	public class transizioni
	{
		immagini m_img_util = new immagini();

		Image m_img_start = null;
		Image m_img_end = null;
		//Image m_img_transition = null;
		//Image m_img_screen = null;
		Image[] m_img_screen = null;

		int m_total_step = 0;
		int m_actual_step = 0;

		Size m_formSize;

		public void SetStep(int passi)
		{
			m_actual_step = m_total_step = passi;

			m_img_screen = new Image[passi+1];
		}

		public bool SetClientAreaForm(Size formSize)
		{
			m_formSize = formSize;

			if (m_img_end == null)
				return false;

			double ratio_form = m_formSize.Width * 1.0 / m_formSize.Height;
			double ratio_img_end = m_img_end.Width * 1.0 / m_img_end.Height;

			int screenWidth;
			int screenHeight;

			if (ratio_img_end > ratio_form)
			{
				screenWidth = m_formSize.Width;
				screenHeight = (int)(screenWidth / ratio_img_end);
			}
			else
			{
				screenHeight = m_formSize.Height;
				screenWidth = (int)(screenHeight * ratio_img_end);
			}

			if (m_img_screen[m_total_step] != null)
			{
				if (m_img_screen[m_total_step].Width != screenWidth || m_img_screen[m_total_step].Height != screenHeight)
				{
					m_img_screen[m_total_step].Dispose();
					m_img_screen[m_total_step] = null;
				}
			}

			if (m_img_screen[m_total_step] == null)
			{
				m_img_screen[m_total_step] = new Bitmap(screenWidth, screenHeight);
				return true;
			}
			return false;
		}

		public bool SetEndImage(string path_new_image)
		{
			if (m_img_end != null)
			{
				if (m_img_start != null)
				{
					m_img_start.Dispose();
					m_img_start = null;
				}
				m_img_start = m_img_end;
				m_img_end = null;
			}
			m_img_end = m_img_util.LoadImageAndRotate(path_new_image);
			return m_img_end != null;
		}

		public void DrawEndImage()
		{
			using (var graphics = Graphics.FromImage(m_img_screen[m_total_step]))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Rectangle destRect = new Rectangle(0, 0, m_img_screen[m_total_step].Width, m_img_screen[m_total_step].Height);
				Rectangle sorcRect = new Rectangle(0, 0, m_img_end.Width, m_img_end.Height);

				graphics.DrawImage(m_img_end, destRect, sorcRect, GraphicsUnit.Pixel);
				//System.Diagnostics.Trace.WriteLine(string.Format("{0}: {1}", DateTime.Now.Millisecond, destRect.ToString() ));
			}
		}

		public Image GetResizeEndImage()
		{
			return m_img_screen[m_total_step];
		}

		public void GenerateTransictionImage()
		{
			if (m_img_end == null)
				return;
			if (m_img_start == null)
			{
				m_img_start = new Bitmap(m_img_end.Width, m_img_end.Height);
				using (SolidBrush myBrush = new SolidBrush(Color.DarkGreen))
				{
					using (Graphics graph = Graphics.FromImage(m_img_start))
					{
						graph.FillRectangle(myBrush, 0, 0, m_img_start.Width, m_img_start.Height);
					}
				}
			}

			Image transct = new Bitmap(m_img_end.Width, m_img_end.Height);

			Rectangle rectStart = new Rectangle(0, 0, m_img_start.Width, m_img_start.Height);
			Rectangle rectTranct = new Rectangle(0, 0, transct.Width, transct.Height);

			//double ratio_form = m_formSize.Width * 1.0 / m_formSize.Height;
			//double ratio_transz = m_img_end.Width * 1.0 / m_img_end.Height;

			Graphics graphics = Graphics.FromImage(transct);

			for (int step = 0; step <= m_total_step; ++step)
			{
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.InterpolationMode = InterpolationMode.Bilinear;
				graphics.SmoothingMode = SmoothingMode.HighSpeed;
				graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

				using (SolidBrush myBrush = new SolidBrush(Color.GreenYellow))
				{
					using (Graphics graph = Graphics.FromImage(transct))
					{
						graph.FillRectangle(myBrush, 0, 0, transct.Width, transct.Height);
					}
				}
				graphics.DrawImage(m_img_start, rectTranct, rectStart, GraphicsUnit.Pixel);

				int passoHeightEnd = (int)((1.0 * m_img_end.Height) / m_total_step * step);
				int passoHeightTranct = (int)((1.0 * transct.Height) / m_total_step * step);

				RectangleF rectPartEnd = new RectangleF(0, m_img_end.Height - passoHeightEnd, m_img_end.Width, m_img_end.Height);
				//RectangleF rectPartTranct = new RectangleF(0, 0, transct.Width, passoHeightTranct);
				RectangleF rectPartTranct = new RectangleF(0, 0, transct.Width, transct.Height);

				graphics.DrawImage(m_img_end, rectPartTranct, rectPartEnd, GraphicsUnit.Pixel);

				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				ResizeStepImage(step);

				Graphics graphicsScreen = Graphics.FromImage(m_img_screen[step]);

				RectangleF rectStep = new RectangleF(0, 0, m_img_screen[step].Width, m_img_screen[step].Height);
				rectPartTranct = new RectangleF(0, 0, transct.Width, transct.Height);

				graphicsScreen.DrawImage(transct, rectStep, rectTranct, GraphicsUnit.Pixel);
				graphicsScreen.Dispose();
			}

			graphics.Dispose();
			transct.Dispose();
			transct = null;

			m_actual_step = 0;
		}

		private void ResizeStepImage(int step)
		{
			if (m_img_end == null)
				return;

			double ratio_form = m_formSize.Width * 1.0 / m_formSize.Height;
			double ratio_end = m_img_end.Width * 1.0 / m_img_end.Height;
			double ratio_start = m_img_start.Width * 1.0 / m_img_start.Height;
			Size screenEnd = new Size();
			Size screenStart = new Size();
			Size screenStep = new Size();

			if (ratio_end > ratio_form)
			{
				screenEnd.Width = m_formSize.Width;
				screenEnd.Height = (int)(screenEnd.Width / ratio_end);
			}
			else
			{
				screenEnd.Height = m_formSize.Height;
				screenEnd.Width = (int)(screenEnd.Height * ratio_end);
			}

			if (ratio_start > ratio_form)
			{
				screenStart.Width = m_formSize.Width;
				screenStart.Height = (int)(screenStart.Width / ratio_start);
			}
			else
			{
				screenStart.Height = m_formSize.Height;
				screenStart.Width = (int)(screenStart.Height * ratio_start);
			}

			screenStep.Height = (int)((1.0 * (screenEnd.Height - screenStart.Height)) * step / m_total_step) + screenStart.Height;
			screenStep.Width = (int)((1.0 * (screenEnd.Width - screenStart.Width)) * step / m_total_step) + screenStart.Width;

			if (m_img_screen[step] == null || m_img_screen[step].Width != screenStep.Width || m_img_screen[step].Height != screenStep.Height)
			{
				if (m_img_screen[step] != null)
					m_img_screen[step].Dispose();
				m_img_screen[step] = new Bitmap(screenStep.Width, screenStep.Height);
			}
		}

		public Image GetNextStep()
		{
			Image ret = null;
			if(m_actual_step < m_total_step)
			{
				ret = m_img_screen[m_actual_step];
				++m_actual_step;
			}
			return ret;
		}

	}
}
