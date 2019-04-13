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
namespace Sfondo
{
	partial class MainForm
	{
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione Windows Form

		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.pict = new System.Windows.Forms.PictureBox();
			this.Clock = new System.Windows.Forms.Timer(this.components);
			this.SubClock = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pict)).BeginInit();
			this.SuspendLayout();
			// 
			// pict
			// 
			this.pict.BackColor = System.Drawing.Color.DarkRed;
			this.pict.Location = new System.Drawing.Point(0, 0);
			this.pict.Name = "pict";
			this.pict.Size = new System.Drawing.Size(204, 109);
			this.pict.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pict.TabIndex = 0;
			this.pict.TabStop = false;
			this.pict.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pict_MouseClick);
			this.pict.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pict_MouseDoubleClick);
			// 
			// Clock
			// 
			this.Clock.Enabled = true;
			this.Clock.Interval = 1000;
			this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
			// 
			// SubClock
			// 
			this.SubClock.Interval = 5;
			this.SubClock.Tick += new System.EventHandler(this.SubClock_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(290, 163);
			this.Controls.Add(this.pict);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Activated += new System.EventHandler(this.MainForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			((System.ComponentModel.ISupportInitialize)(this.pict)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pict;
		private System.Windows.Forms.Timer Clock;
		private System.Windows.Forms.Timer SubClock;
	}
}

