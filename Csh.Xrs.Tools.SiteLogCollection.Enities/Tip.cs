using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public class Tip : ITip
    {
        private static readonly Tip instance = new Tip(new Point(960, 540), 50, FontFamily.GenericSansSerif);
        private IntPtr transparentWndHandle;
        public Point Location { get; set; }
        public FontFamily FontFamily { get; set; }


        public int FontSize { get; set; }

        static Tip()
        {

        }
        private Tip(Point locatoin, int fontSize, FontFamily fontFamily)
        {
            Location = locatoin;
            FontSize = fontSize;
            FontFamily = fontFamily;
            //transparentWndHandle = 
        }
        public static Tip Instance
        {
            get
            {
                return instance;
            }
        }
        public void Show(string text, int displayTime)
        {
            var Msg = new Win32.MSG();
            var window = new TransparentWindow(text, Location.X, Location.Y, FontSize, FontFamily.Name);
            var hwnd = window.CreateTransparentWindow();
            DateTime dt = DateTime.Now;
            while ((Win32.GetMessage(out Msg, IntPtr.Zero, 0, 0)) > 0)
            {
                Win32.TranslateMessage(ref Msg);
                Win32.DispatchMessage(ref Msg);
                var ts = DateTime.Now - dt;
                if (ts.TotalSeconds > 5)
                {
                    Win32.DestroyWindow(hwnd);
                }
            }
        }
    }


    internal class TransparentWindow
    {
        private static string resultMessage = "Hello";
        static string AppName = "DrawHello Program";
        static string ClassName = "DrawHelloClass";
        static IntPtr hWnd;
        public const int LWA_ALPHA = 0x2;
        public const int LWA_COLORKEY = 0x1;
        private static int pX;
        private static int pY;
        private static int fontSize = 40;
        private static string fontFamily = "Sans-serif";


        public TransparentWindow(string message, int positionX, int positionY, int fontSize, string fontFamily)
        {
            resultMessage = message;
            pX = positionX;
            pY = positionY;

            if (fontSize > 0)
            {
                TransparentWindow.fontSize = fontSize;
            }

            if (!string.IsNullOrEmpty(fontFamily) && !string.IsNullOrWhiteSpace(fontFamily))
            {
                TransparentWindow.fontFamily = fontFamily;
            }
        }

        private static int RegisterClass()
        {
            Win32.WNDCLASSEX wcex = new Win32.WNDCLASSEX();
            wcex.style = Win32.ClassStyles.DoubleClicks;
            wcex.cbSize = (uint)Marshal.SizeOf(wcex);
            wcex.lpfnWndProc = WndProc;
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hIcon = Win32.LoadIcon(IntPtr.Zero, (IntPtr)Win32.IDI_APPLICATION);
            wcex.hCursor = Win32.LoadCursor(IntPtr.Zero, (int)Win32.IDC_ARROW);
            wcex.hIconSm = IntPtr.Zero;
            wcex.hbrBackground = (IntPtr)(Win32.COLOR_WINDOW + 1);
            wcex.lpszMenuName = null;
            wcex.lpszClassName = ClassName;
            if (Win32.RegisterClassEx(ref wcex) == 0)
            {
                Win32.MessageBox(IntPtr.Zero, "RegisterClassEx failed", AppName,
                    (int)(Win32.MB_OK | Win32.MB_ICONEXCLAMATION | Win32.MB_SETFOREGROUND));
                return (0);
            }
            return (1);
        }

        private static IntPtr Create()
        {
            hWnd = Win32.CreateWindowEx(Win32.WS_EX_TOPMOST | Win32.WS_EX_LAYERED, ClassName, AppName, Win32.WS_POPUP | Win32.WS_VISIBLE,
                0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            Win32.ShowWindow(hWnd, 1);
            Win32.UpdateWindow(hWnd);
            Win32.SetLayeredWindowAttributes(hWnd, ColorTranslator.ToWin32(Color.White), 255, LWA_COLORKEY);
            return hWnd;
        }


        private static IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                case Win32.WM_PAINT:
                    {
                        IntPtr hDC;
                        Win32.PAINTSTRUCT ps = new Win32.PAINTSTRUCT();
                        hDC = Win32.BeginPaint(hWnd, out ps);
                        Win32.SetTextColor(hDC, ColorTranslator.ToWin32(Color.Red));
                        var hfont = Win32.CreateFont(-fontSize, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, fontFamily);
                        Win32.SelectObject(hDC, hfont);
                        Win32.TextOut(hDC, pX, pY, resultMessage, resultMessage.Length);
                        Win32.EndPaint(hWnd, ref ps);
                        return IntPtr.Zero;
                    }
                case Win32.WM_DESTROY:
                    Win32.PostQuitMessage(0);
                    return IntPtr.Zero;
                //case Win32.WM_CREATE:
                //    return IntPtr.Zero;
                //case Win32.WM_RBUTTONDBLCLK:
                //    {
                //        //var f = new Form1();
                //        //f.ShowDialog();
                //        //var f1 = new Form1();
                //        //f1.ShowDialog();
                //        //return IntPtr.Zero;
                //    }
                //    break;
                default:
                    return (Win32.DefWindowProc(hWnd, message, wParam, lParam));
            }
        }

        public IntPtr CreateTransparentWindow()
        {
            if (RegisterClass() == 0)
                return IntPtr.Zero;
            return Create();
        }
    }


    internal class Win32
    {
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_CREATE = 0x0001;
        public const uint WM_PAINT = 0x000F;
        public const uint WM_RBUTTONDBLCLK = 0X0203;

        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_CAPTION = 0x00C00000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        public const uint WS_POPUP = 0x80000000;

        public const uint IDI_APPLICATION = 32512;

        public const uint IDC_ARROW = 32512;

        public const uint COLOR_WINDOW = 5;

        public const uint MB_OK = 0x00000000;
        public const uint MB_ICONEXCLAMATION = 0x00000030;
        public const uint MB_SETFOREGROUND = 0x00010000;

        public const uint WS_EX_LAYERED = 0X00080000;

        public const uint WS_EX_TOPMOST = 0x00000008;
        //public const uint CW_USEDEFAULT = 0x80000000;

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /*
		public enum GetWindowLongConst
		{
			GWL_WNDPROC = (-4),
			GWL_HINSTANCE = (-6),
			GWL_HWNDPARENT = (-8),
			GWL_STYLE = (-16),
			GWL_EXSTYLE = (-20),
			GWL_USERDATA = (-21),
			GWL_ID = (-12)
		}
		 * */

        [Flags]
        public enum ClassStyles : uint
        {
            ByteAlignClient = 0x1000,
            ByteAlignWindow = 0x2000,
            ClassDC = 0x40,
            DoubleClicks = 0x8,
            DropShadow = 0x20000,
            GlobalClass = 0x4000,
            HorizontalRedraw = 0x2,
            NoClose = 0x200,
            OwnDC = 0x20,
            ParentDC = 0x80,
            SaveBits = 0x800,
            VerticalRedraw = 0x1
        }

        public enum MessageBoxResult : uint
        {
            Ok = 1,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No,
            Close,
            Help,
            TryAgain,
            Continue,
            Timeout = 32000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;

            public SIZE(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        /*
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct CREATESTRUCT
		{
			public IntPtr lpCreateParams;
			public IntPtr hInstance;
			public IntPtr hMenu;
			public IntPtr hwndParent;
			public int cy;
			public int cx;
			public int y;
			public int x;
			public int style;
			public string lpszName;
			public string lpszClass;
			public int dwExStyle;
		}

		[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public byte tmFirstChar;    // for compatibility it must be byte instead of char
			public byte tmLastChar;    // for compatibility it must be byte instead of char
			public byte tmDefaultChar;    // for compatibility it must be byte instead of char
			public byte tmBreakChar;    // for compatibility it must be byte instead of char
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}
		*/

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            public uint cbSize;
            public ClassStyles style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;

            public byte BlendFlags;

            public byte SourceConstantAlpha;

            public byte AlphaFormat;

        }
        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref MSG lpmsg);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        //[DllImport("user32.dll", EntryPoint = "GetWindowLong")]		// 32 bits only
        //public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern MessageBoxResult MessageBox(IntPtr hWnd, String text, String caption, int options);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
           uint dwExStyle,
           string lpClassName,
           string lpWindowName,
           uint dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short RegisterClassEx(ref WNDCLASSEX lpwcx);

        //[DllImport("gdi.dll")]
        //public static extern bool GetTextExtentPoint(IntPtr hdc, string lpString, int cbString, out SIZE lpSize);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart,
           string lpString, int cbString);

        //[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        //public static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);
        [DllImportAttribute("user32.dll")]
        public static extern bool UpdateLayeredWindow(IntPtr handle, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, uint dwFlags);

        [DllImport("gdi32.dll")]
        public static extern uint SetTextColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement,
            int nOrientation, int fnWeight, uint fdwItalic, uint fdwUnderline, uint
                fdwStrikeOut, uint fdwCharSet, uint fdwOutputPrecision, uint
                fdwClipPrecision, uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern System.IntPtr SelectObject(System.IntPtr hObject, System.IntPtr hFont);

    }
}
