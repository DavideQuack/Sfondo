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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfondo
{
	public class immagini
	{
		/**
		 * 
The image orientation viewed in terms of rows and columns.  
Tag  = 274 (112.H) 
Type  = SHORT 
Count  = 1 
Default = 1
  1 = La riga 0 si trova nella parte superiore dell'immagine, e la colonna 0 è il lato visivo sinistro.
  2 = La riga 0 si trova nella parte superiore visiva dell'immagine e la colonna 0 è il lato visivo destro.
  3 = La riga 0 si trova nella parte inferiore dell'immagine, e la colonna 0 è il lato visivo destro.
  4 = La riga 0 si trova nella parte inferiore dell'immagine, e la colonna 0 è il lato visivo sinistro.
  5 = La riga 0 è il lato visivo sinistro dell'immagine e la colonna 0 è la parte superiore visiva.
  6 = La riga 0 è il lato destro visivo dell'immagine, e la colonna 0 è la parte superiore visiva.
  7 = La riga 0 è il lato destro visivo dell'immagine, e la colonna 0 è il fondo visivo.
  8 = La riga 0 è il lato visivo sinistro dell'immagine e la colonna 0 è il fondo visivo.		 
		 */
		public Image LoadImageAndRotate(string filepath)
		{
			Image img = Image.FromFile(filepath);
			//ret.RotateFlip(RotateFlipType.Rotate90FlipNone);
			int orientationId = 0x0112;
			PropertyItem pItvalem = null;
			if (img.PropertyIdList.Contains(orientationId))
			{
				pItvalem = img.GetPropertyItem(orientationId);
				switch (pItvalem.Value[0])
				{
					case 1:
						// No rotation required.
						//return img;
						break;
					case 2:
						img.RotateFlip(RotateFlipType.RotateNoneFlipX);
						break;
					case 3:
						img.RotateFlip(RotateFlipType.Rotate180FlipNone);
						break;
					case 4:
						img.RotateFlip(RotateFlipType.Rotate180FlipX);
						break;
					case 5:
						img.RotateFlip(RotateFlipType.Rotate90FlipX);
						break;
					case 6:
						img.RotateFlip(RotateFlipType.Rotate90FlipNone);
						break;
					case 7:
						img.RotateFlip(RotateFlipType.Rotate270FlipX);
						break;
					case 8:
						img.RotateFlip(RotateFlipType.Rotate270FlipNone);
						break;
				}
			}
			return img;
		}

		//	public Bitmap SetAlpha(Image bmpIn, int alpha)
		//	{
		//		Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
		//		float a = alpha / 255f;
		//		Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

		//		float[][] matrixItems = {
		//			new float[] {1, 0, 0, 0, 0},
		//			new float[] {0, 1, 0, 0, 0},
		//			new float[] {0, 0, 1, 0, 0},
		//			new float[] {0, 0, 0, a, 0},
		//			new float[] {0, 0, 0, 0, 1}};

		//		ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

		//		ImageAttributes imageAtt = new ImageAttributes();
		//		imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

		//		using (Graphics g = Graphics.FromImage(bmpOut))
		//			g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

		//		return bmpOut;
		//	}

		public Bitmap ChangeOpacity(Image imgA, Image imgB, float opacityvalue)
		{
			Bitmap bmp = new Bitmap(imgB.Width, imgB.Height); // Determining Width and Height of Source Image
			Graphics graphics = Graphics.FromImage(bmp);
			ColorMatrix colormatrix = new ColorMatrix();
			colormatrix.Matrix33 = opacityvalue;
			ImageAttributes imgAttribute = new ImageAttributes();
			imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			//graphics.DrawImage(imgA, new Point(0, 0));
			graphics.DrawImage(imgB, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, imgB.Width, imgB.Height, GraphicsUnit.Pixel, imgAttribute);
			graphics.Dispose();   // Releasing all resource used by graphics 
			return bmp;
		}
		//float opacityvalue = float.Parse(txtopacityvalue.Text) / 100;
		//pictureBox1.Image = ImageUtils.ImageTransparency.ChangeOpacity(Image.FromFile("filename"),opacityvalue);  //calling ChangeOpacity Function 
	}
}

