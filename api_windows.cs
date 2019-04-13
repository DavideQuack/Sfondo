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
using System.Drawing;
using System.Runtime.InteropServices;

namespace Sfondo
{
	public class api_windows
	{
		// http://www.pinvoke.net/

		[DllImport("user32")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		
		public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
		public const UInt32 SWP_NOSIZE = 0x0001;
		public const UInt32 SWP_NOMOVE = 0x0002;
		public const UInt32 SWP_NOACTIVATE = 0x0010;

		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		public static extern bool GetClientRect(HandleRef hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner
		}

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(HandleRef hWnd, ref Point lpPoint);

	}
}
