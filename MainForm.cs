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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using static Sfondo.api_windows;
using System.Threading;

namespace Sfondo
{
	public partial class MainForm : Form
	{
		//string[] m_imglist;
		List<string> m_imglist = new List<string>();
		int[] m_rebase;
		int m_index = 0;
		bool m_work = false;
		bool m_subwork = false;

		transizioni m_trnz = new transizioni();
		Thread m_workThead;
		delegate void WorkThreadHandler();
		WorkThreadHandler ThreadStopDelegate;

		public MainForm(string[] args)
		{
			GestoreConfigurazione gcfg = new GestoreConfigurazione();
			Configuration cfg = gcfg.LoadConfiguration();

			//bool requestFix = false;
			//foreach (string param in args)
			//{
			//	int pos = param.IndexOf(':');
			//	if (pos > 0)
			//	{
			//		string lparam = param.Substring(0, pos);
			//		switch (lparam)
			//		{
			//			case "bounds":
			//				if (param.Length > pos + 4)
			//				{
			//					string[] rparam = param.Substring(pos + 1).Split(',');
			//					if (rparam.Length == 4)
			//					{
			//						try
			//						{
			//							Rectangle rect = new Rectangle();
			//							rect.X = int.Parse(rparam[0]);
			//							rect.Y = int.Parse(rparam[1]);
			//							rect.Width = int.Parse(rparam[2]);
			//							rect.Height = int.Parse(rparam[3]);

			//							this.StartPosition = FormStartPosition.Manual;
			//							this.Location = new Point(rect.X, rect.Y);
			//							this.ClientSize = new Size(rect.Width, rect.Height);
			//						}
			//						catch
			//						{
			//						}
			//					}
			//				}
			//				break;
			//			case "fix":
			//				requestFix = true;
			//				break;
			//		}
			//	}
			//}

			this.ClientSize = new Size( (int)(this.ClientSize.Height * 1.777), this.ClientSize.Height);

			foreach (DirFS d in cfg.WatchDirFS)
			{
				m_imglist.AddRange(Directory.GetFiles(d.directory).Where(x => Path.GetExtension(x).ToLower() == ".jpg") );
			}
			m_trnz.SetStep(cfg.StepForTransition);

			//ordine casuale
			List<int> ord = new List<int>();
			List<int> caos = new List<int>();
			for (int i = 0; i < m_imglist.Count; ++i)
				ord.Add(i);
			Random rnd = new Random();
			while (ord.Count > 0)
			{
				int pos = rnd.Next(ord.Count);
				caos.Add(ord[pos]);
				ord.RemoveAt(pos);
			}
			m_rebase = caos.ToArray();
			if(m_rebase.Length == 0)
				MessageBox.Show("Non ci sono immagini nei path indicati");
			else
				m_trnz.SetEndImage(m_imglist[rnd.Next(m_imglist.Count)]);

			this.WindowState = FormWindowState.Normal;

			InitializeComponent();

			pict.Location = new Point(0, 0);

			Clock.Interval = cfg.SecondNextImage + 1000;
			SubClock.Interval = cfg.MillisecondNextTransition;

			MainForm_SizeChanged(null, null);

			ThreadStopDelegate = new WorkThreadHandler(StopWorkThread);

			//if (requestFix)
			//{
			//	MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
			//	pict_MouseDoubleClick(pict, e);
			//}
		}

		private void pict_MouseClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
				StartTransition();
			if(e.Button == MouseButtons.Right)
			{
				string s = string.Format("bounds:{0},{1},{2},{3}", this.Location.X, this.Location.Y, this.ClientSize.Width, this.ClientSize.Height);
				MessageBox.Show(this, s);
			}
		}

		private void pict_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			bool showIco = true;
			if (e.Button == MouseButtons.Left)
			{
				if (this.FormBorderStyle != FormBorderStyle.Sizable)
				{
					//Size? memory = null;
					//if (this.WindowState == FormWindowState.Normal)
					//	memory = new Size(m_origFormsize.Width, m_origFormsize.Height);
					this.FormBorderStyle = FormBorderStyle.Sizable;
					//if(this.WindowState == FormWindowState.Normal)
					//	SetWindowPos(Handle, HWND_BOTTOM, m_rect_form.Left, m_rect_form.Top, 0, 0, SWP_NOSIZE );
					//if(memory != null)
					//	this.ClientSize = memory.Value;
				}
				else
				{
					//GetWindowRect(new HandleRef(this, this.Handle), out m_rect_form);
					//GetWindowRect(new HandleRef(pict, pict.Handle), out m_rect_pict);

					this.FormBorderStyle = FormBorderStyle.None;
					showIco = false;
					//if (this.WindowState == FormWindowState.Normal)
					//	SetWindowPos(Handle, HWND_BOTTOM, m_rect_pict.Left, m_rect_pict.Top, 0, 0, SWP_NOSIZE | SWP_NOACTIVATE);
					//if (this.WindowState == FormWindowState.Maximized)
					//{
					//	this.WindowState = FormWindowState.Normal;
					//	this.WindowState = FormWindowState.Maximized;
					//}
				}
				this.ShowIcon = showIco;
				this.ShowInTaskbar = showIco;
			}
		}

		private void MainForm_Activated(object sender, EventArgs e)
		{
			if (this.FormBorderStyle != FormBorderStyle.Sizable)
			{
				this.FormBorderStyle = FormBorderStyle.None;
				SetWindowPos(Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
			}
		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (m_work || m_subwork)
				return;
			if (m_workThead != null)
				return;

			SubClock.Enabled = false;
			Clock.Enabled = false;
			m_work = true;
			m_subwork = true;

			if (m_trnz.SetClientAreaForm(this.ClientSize))
			{
				m_trnz.DrawEndImage();
				pict.Image = m_trnz.GetResizeEndImage();
				pict.Refresh();
			}

			m_subwork = false;
			m_work = false;
			SubClock.Enabled = false;
			Clock.Enabled = true;
		}

		private void Clock_Tick(object sender, EventArgs e)
		{
			if (m_imglist.Count == 0)
				return;

			if (m_work)
				return;
			if (m_workThead != null)
				return;

			m_work = true;

			StartTransition();
		}

		private void WorkThread()
		{
			m_trnz.SetEndImage( m_imglist[m_rebase[m_index]] );
			m_index = (m_index + 1) % m_imglist.Count;
			m_trnz.GenerateTransictionImage();
			this.Invoke(ThreadStopDelegate);
		}

		private void StopWorkThread()
		{
			this.Cursor = Cursors.Default;
			SubClock.Enabled = true;
			m_work = false;
			lock (this)
			{
				m_workThead = null;
			}
		}

		private void StartTransition()
		{
			lock (this)
			{
				if (m_workThead != null && m_workThead.IsAlive)
					return;
			}

			if (this.FormBorderStyle == FormBorderStyle.Sizable)
				this.Cursor = Cursors.WaitCursor;
			Clock.Enabled = false;
			m_workThead = new Thread(new ThreadStart(WorkThread));

			m_workThead.Start();
		}

		private void StopTransition()
		{
			SubClock.Enabled = false;
			Clock.Enabled = true;

			m_trnz.SetClientAreaForm(this.ClientSize);
			m_trnz.GetResizeEndImage();
			m_trnz.DrawEndImage();
			pict.Image = m_trnz.GetResizeEndImage();
			pict.Refresh();

			if (this.FormBorderStyle != FormBorderStyle.Sizable)
				this.Size = pict.Size;
		}

		private void SubClock_Tick(object sender, EventArgs e)
		{
			if (m_subwork)
				return;
			m_subwork = true;

			Image img = m_trnz.GetNextStep();
			if (img == null)
				StopTransition();
			else
			{
				pict.Image = img;
				pict.Refresh();
			}

			m_subwork = false;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_subwork = true;
			m_work = true;
			SubClock.Enabled = false;
			Clock.Enabled = false;
			lock (this)
			{
				if (m_workThead != null)
					if (m_workThead.IsAlive)
						m_workThead.Abort();
			}
		}
	}
}
