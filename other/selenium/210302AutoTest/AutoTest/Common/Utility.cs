using Microsoft.CSharp;
using AutoTest.Data;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace AutoTest.Core
{
    public static class Utility
    {
        #region Inner

        private static class GDI32
        {

            public const int SRCCOPY = 0x00CC0020;

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
        private static class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }


        #endregion

        public static bool IsEmpty(this string val)
        {
            return val == null || val.Trim().Length == 0;
        }
        public static bool IsEmpty(this IEnumerable val)
        {
            return val == null || !val.GetEnumerator().MoveNext();
        }

        public static string GetEnvironment(string envName, EnvironmentVariableTarget target)
        {
            string st = Environment.GetEnvironmentVariable(envName, target);
            return getEnvironmentReference(st, target);
        }
        private static string getEnvironmentReference(string envValue, EnvironmentVariableTarget target)
        {
            //Dim nPos, nStart, nEnd, strResult, strTemp
            string result = "";
            int start = envValue.IndexOf('%');
            if (start < 0) return envValue;
            int end = envValue.IndexOf('%', start + 1);
            if (end < start) return envValue;

            string temp = envValue.Substring(start + 1, end - 1);
            temp = GetEnvironment(temp, target);

            result = envValue.Substring(0, start) + temp + (end < envValue.Length - 1 ? envValue.Substring(end + 1) : "");
            return result;
        }

        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }
        public static Image CaptureWindow(IntPtr handle)
        {
            IntPtr hSource = IntPtr.Zero;
            IntPtr hTarget = IntPtr.Zero; 
            IntPtr hBitmap = IntPtr.Zero;
            Image img = null;
            try
            {
                User32.RECT rect = new User32.RECT();
                User32.GetWindowRect(handle, ref rect);
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                handle = User32.GetDesktopWindow();
                hSource = User32.GetWindowDC(handle);
                hTarget = GDI32.CreateCompatibleDC(hSource);

                hBitmap = GDI32.CreateCompatibleBitmap(hSource, width, height);
                GDI32.SelectObject(hTarget, hBitmap);
                GDI32.BitBlt(hTarget, 0, 0, width, height, hSource, rect.left, rect.top, GDI32.SRCCOPY);
                img = Image.FromHbitmap(hBitmap);
            }
            finally
            {
                User32.ReleaseDC(handle, hSource);

                GDI32.DeleteDC(hTarget);
                GDI32.DeleteObject(hBitmap);
            }

            return img;
        }
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }
    }
}