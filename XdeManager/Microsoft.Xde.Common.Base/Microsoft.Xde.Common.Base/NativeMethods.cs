using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000C RID: 12
	public static class NativeMethods
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00003026 File Offset: 0x00001226
		public static bool IsPenInput(IntPtr data)
		{
			return (data.ToInt32() & -256) == -11446528;
		}

		// Token: 0x06000055 RID: 85
		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000056 RID: 86
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int CreateVirtualDisk(ref NativeMethods.VIRTUAL_STORAGE_TYPE VirtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string Path, NativeMethods.VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask, IntPtr SecurityDescriptor, NativeMethods.CREATE_VIRTUAL_DISK_FLAG Flags, uint ProviderSpecificFlags, ref NativeMethods.CREATE_VIRTUAL_DISK_PARAMETERS Parameters, IntPtr Overlapped, ref IntPtr Handle);

		// Token: 0x06000057 RID: 87
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int OpenVirtualDisk(ref NativeMethods.VIRTUAL_STORAGE_TYPE virtualStorageType, [MarshalAs(UnmanagedType.LPWStr)] string path, NativeMethods.VIRTUAL_DISK_ACCESS_MASK virtualDiskAccessMask, NativeMethods.OPEN_VIRTUAL_DISK_FLAG flags, ref NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS parameters, out IntPtr handle);

		// Token: 0x06000058 RID: 88
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int AttachVirtualDisk(IntPtr virtualDiskHandle, IntPtr securityDescriptor, NativeMethods.ATTACH_VIRTUAL_DISK_FLAG flags, uint providerSpecificFlags, ref NativeMethods.ATTACH_VIRTUAL_DISK_PARAMETERS parameters, IntPtr overlapped);

		// Token: 0x06000059 RID: 89
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int GetVirtualDiskInformation(IntPtr virtualDiskHandle, ref uint virtualDiskInfoSize, ref NativeMethods.GetVirtualDiskInfoIdentifier info, ref uint sizeUsed);

		// Token: 0x0600005A RID: 90
		[DllImport("Virtdisk.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int GetVirtualDiskInformation(IntPtr virtualDiskHandle, ref uint virtualDiskInfoSize, ref NativeMethods.GetVirtualDiskInfoParentLocation info, ref uint sizeUsed);

		// Token: 0x0600005B RID: 91
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr FindWindowExW(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		// Token: 0x0600005C RID: 92
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr FindWindowW(string className, string windowName);

		// Token: 0x0600005D RID: 93
		[DllImport("User32.dll")]
		public static extern IntPtr SetForegroundWindow(IntPtr hwnd);

		// Token: 0x0600005E RID: 94
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		// Token: 0x0600005F RID: 95
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetWindowTextW(IntPtr handle, StringBuilder text, int textSize);

		// Token: 0x06000060 RID: 96
		[DllImport("user32.dll")]
		public static extern int GetWindowRect(IntPtr hWnd, out NativeMethods.RECT ret);

		// Token: 0x06000061 RID: 97
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetClassNameW(IntPtr handle, StringBuilder text, int textSize);

		// Token: 0x06000062 RID: 98
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InvalidateRect(IntPtr hwnd, IntPtr rect, int erase);

		// Token: 0x06000063 RID: 99
		[DllImport("user32")]
		public static extern IntPtr SendMessageW(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x06000064 RID: 100
		[DllImport("Gdi32.dll")]
		public static extern IntPtr CreateRoundRectRgn(int leftRect, int topRect, int rightRect, int nottomRect, int widthEllipse, int heightEllipse);

		// Token: 0x06000065 RID: 101
		[DllImport("Gdi32.dll")]
		public static extern NativeMethods.RegionRet GetRgnBox(IntPtr rgn, out NativeMethods.RECT rect);

		// Token: 0x06000066 RID: 102
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref NativeMethods.Point pptDst, ref NativeMethods.Size psize, IntPtr hdcSrc, ref NativeMethods.Point pprSrc, int key, ref NativeMethods.BLENDFUNCTION pblend, int flags);

		// Token: 0x06000067 RID: 103
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		// Token: 0x06000068 RID: 104
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetDC(IntPtr hwnd);

		// Token: 0x06000069 RID: 105
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

		// Token: 0x0600006A RID: 106
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteDC(IntPtr hdc);

		// Token: 0x0600006B RID: 107
		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr handle);

		// Token: 0x0600006C RID: 108
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr handle);

		// Token: 0x0600006D RID: 109
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

		// Token: 0x0600006E RID: 110
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process([In] IntPtr process, out bool wow64Process);

		// Token: 0x0600006F RID: 111
		[DllImport("user32", SetLastError = true)]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newValue);

		// Token: 0x06000070 RID: 112
		[DllImport("user32")]
		public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, NativeMethods.Win32WndProc newProc);

		// Token: 0x06000071 RID: 113
		[DllImport("user32")]
		public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000072 RID: 114
		[DllImport("user32")]
		public static extern IntPtr SetCapture(IntPtr hWnd);

		// Token: 0x06000073 RID: 115
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseCapture();

		// Token: 0x06000074 RID: 116
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterTouchWindow(IntPtr hwnd, uint ulFlags);

		// Token: 0x06000075 RID: 117
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseTouchInputHandle(IntPtr hTouchInput);

		// Token: 0x06000076 RID: 118
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr SetCursor(IntPtr cursor);

		// Token: 0x06000077 RID: 119
		[DllImport("user32")]
		public static extern IntPtr GetMessageExtraInfo();

		// Token: 0x06000078 RID: 120
		[DllImport("user32")]
		public static extern int GetSystemMetrics(int index);

		// Token: 0x06000079 RID: 121
		[DllImport("XdeUIUtils")]
		public static extern bool XdeUIUtilsInitTouchWindow(IntPtr hwnd, IntPtr socket);

		// Token: 0x0600007A RID: 122
		[DllImport("XdeUIUtils")]
		public static extern void XdeUIUtilsSetWindowScale(float scale);

		// Token: 0x0600007B RID: 123
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool ShowWindow(IntPtr hwnd, int cmdShow);

		// Token: 0x0600007C RID: 124
		[DllImport("netapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int NetLocalGroupAddMembers(string servername, string groupname, int level, string[] names, int entries);

		// Token: 0x0600007D RID: 125
		[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int NetLocalGroupGetMembers([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string localGroupName, int level, out IntPtr bufPtr, uint prefMaxLen, out int entriesRead, out int totalEntries, ref int resumeHandle);

		// Token: 0x0600007E RID: 126
		[DllImport("NetApi32.dll", CharSet = CharSet.Unicode)]
		public static extern int NetApiBufferFree(IntPtr ptr);

		// Token: 0x0600007F RID: 127
		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int vkey);

		// Token: 0x06000080 RID: 128
		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		// Token: 0x06000081 RID: 129
		[DllImport("LocBootPresets", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int GetGlobalizationRegSettingsToApplyForXDE(string muiLanguageName, string strOutputDirectoryPath, StringBuilder regFullFilePath, ref uint fullFilePathSize);

		// Token: 0x06000082 RID: 130
		[DllImport("user32.dll")]
		public static extern bool GetPointerInfo(uint pointerId, ref NativeMethods.POINTER_INFO pointerInfo);

		// Token: 0x06000083 RID: 131
		[DllImport("user32.dll")]
		public static extern bool GetPointerFrameTouchInfo(uint pointerId, ref int pointerCountref, [In] [Out] NativeMethods.POINTER_TOUCH_INFO[] touchInfo);

		// Token: 0x06000084 RID: 132
		[DllImport("user32.dll")]
		public static extern bool GetPointerFramePenInfo(uint pointerId, ref int pointerCountref, [In] [Out] NativeMethods.POINTER_PEN_INFO[] penInfo);

		// Token: 0x06000085 RID: 133
		[DllImport("user32.dll")]
		public static extern bool SkipPointerFrameMessages(uint pointerId);

		// Token: 0x06000086 RID: 134 RVA: 0x0000303C File Offset: 0x0000123C
		public static bool IsMessageFromTouch()
		{
			return NativeMethods.IsPenInput(NativeMethods.GetMessageExtraInfo());
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003048 File Offset: 0x00001248
		public static List<IntPtr> GetChildWindows(IntPtr parent)
		{
			List<IntPtr> list = new List<IntPtr>();
			GCHandle value = GCHandle.Alloc(list);
			try
			{
				NativeMethods.EnumWindowProc callback = new NativeMethods.EnumWindowProc(NativeMethods.EnumWindow);
				NativeMethods.EnumChildWindows(parent, callback, GCHandle.ToIntPtr(value));
			}
			finally
			{
				if (value.IsAllocated)
				{
					value.Free();
				}
			}
			return list;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000030A4 File Offset: 0x000012A4
		public static IntPtr FindDescendentWindow(IntPtr parent, string windowText, string windowClass)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			IntPtr result = IntPtr.Zero;
			foreach (IntPtr intPtr in NativeMethods.GetChildWindows(parent))
			{
				if (!string.IsNullOrEmpty(windowText))
				{
					NativeMethods.GetWindowTextW(intPtr, stringBuilder, stringBuilder.Capacity);
					if (stringBuilder.ToString() == windowText)
					{
						return intPtr;
					}
				}
				if (!string.IsNullOrEmpty(windowClass))
				{
					NativeMethods.GetClassNameW(intPtr, stringBuilder, stringBuilder.Capacity);
					if (stringBuilder.ToString() == windowClass)
					{
						return intPtr;
					}
				}
				IntPtr intPtr2 = NativeMethods.FindDescendentWindow(intPtr, windowText, windowClass);
				if (intPtr2 != IntPtr.Zero)
				{
					result = intPtr2;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003174 File Offset: 0x00001374
		public static IntPtr FindTopLevelWindowWithMatchingText(string windowTextStartsWith)
		{
			IntPtr found = IntPtr.Zero;
			NativeMethods.EnumWindows(delegate(IntPtr hwnd, IntPtr lparam)
			{
				int windowTextLength = NativeMethods.GetWindowTextLength(hwnd);
				if (windowTextLength++ > 0 && NativeMethods.IsWindowVisible(hwnd))
				{
					StringBuilder stringBuilder = new StringBuilder(windowTextLength);
					if (NativeMethods.GetWindowTextW(hwnd, stringBuilder, windowTextLength) > 0 && stringBuilder.ToString().StartsWith(windowTextStartsWith, StringComparison.OrdinalIgnoreCase))
					{
						found = hwnd;
						return false;
					}
				}
				return true;
			}, IntPtr.Zero);
			return found;
		}

		// Token: 0x0600008A RID: 138
		[DllImport("ntdll.dll", SetLastError = true)]
		public static extern int NtQueryInformationProcess(IntPtr ProcessHandle, int ProcessInformationClass, ref NativeMethods.PROCESS_BASIC_INFORMATION ProcessInformation, int ProcessInformationLength, out int ReturnLength);

		// Token: 0x0600008B RID: 139 RVA: 0x000031AC File Offset: 0x000013AC
		public static Process FindParentProcess()
		{
			NativeMethods.PROCESS_BASIC_INFORMATION structure = default(NativeMethods.PROCESS_BASIC_INFORMATION);
			int num;
			if (NativeMethods.NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 0, ref structure, Marshal.SizeOf<NativeMethods.PROCESS_BASIC_INFORMATION>(structure), out num) == 0)
			{
				return Process.GetProcessById(structure.Reserved3.ToInt32());
			}
			return null;
		}

		// Token: 0x0600008C RID: 140
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ScreenToClient(IntPtr hwnd, ref System.Drawing.Point point);

		// Token: 0x0600008D RID: 141
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ScreenToClient(IntPtr hwnd, ref NativeMethods.Point point);

		// Token: 0x0600008E RID: 142
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ClientToScreen(IntPtr hwnd, ref System.Drawing.Point point);

		// Token: 0x0600008F RID: 143
		[DllImport("user32.dll", CharSet = CharSet.Auto, PreserveSig = false, SetLastError = true)]
		public static extern void PostMessage(IntPtr windowHandle, int message, IntPtr wparam, IntPtr lparam);

		// Token: 0x06000090 RID: 144
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000091 RID: 145
		[DllImport("Kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool QueryPerformanceCounter(ref long count);

		// Token: 0x06000092 RID: 146
		[DllImport("Kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool QueryPerformanceFrequency(ref long frequency);

		// Token: 0x06000093 RID: 147
		[DllImport("sensapi.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsNetworkAlive(out int flags);

		// Token: 0x06000094 RID: 148
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetTouchInputInfo(IntPtr hTouchInput, uint cInputs, [In] [Out] NativeMethods.TOUCHINPUT[] output, int cbSize);

		// Token: 0x06000095 RID: 149 RVA: 0x000031F0 File Offset: 0x000013F0
		private static bool EnumWindow(IntPtr handle, IntPtr pointer)
		{
			((List<IntPtr>)GCHandle.FromIntPtr(pointer).Target).Add(handle);
			return true;
		}

		// Token: 0x06000096 RID: 150
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumChildWindows(IntPtr window, NativeMethods.EnumWindowProc callback, IntPtr i);

		// Token: 0x06000097 RID: 151
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumWindows(NativeMethods.EnumWindowProc enumProc, IntPtr lParam);

		// Token: 0x06000098 RID: 152
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		// Token: 0x06000099 RID: 153
		[DllImport("gdi32.dll")]
		public static extern int D3DKMTCloseAdapter(ref NativeMethods.D3DKMT_CLOSEADAPTER arg1);

		// Token: 0x0600009A RID: 154
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
		public static extern int D3DKMTOpenAdapterFromDeviceName(ref NativeMethods.D3DKMT_OPENADAPTERFROMDEVICENAME arg1);

		// Token: 0x0600009B RID: 155
		[DllImport("dxgi.dll")]
		public static extern int CreateDXGIFactory2(uint flags, ref Guid riid, out NativeMethods.IDXGIFactory6 factory);

		// Token: 0x0400002F RID: 47
		public const int WS_EX_TRANSPARENT = 32;

		// Token: 0x04000030 RID: 48
		public const int WS_EX_LAYERED = 524288;

		// Token: 0x04000031 RID: 49
		public const int HTCLIENT = 1;

		// Token: 0x04000032 RID: 50
		public const int HTCAPTION = 2;

		// Token: 0x04000033 RID: 51
		public const int WM_NCHITTEST = 132;

		// Token: 0x04000034 RID: 52
		public const int ULW_ALPHA = 2;

		// Token: 0x04000035 RID: 53
		public const byte AC_SRC_OVER = 0;

		// Token: 0x04000036 RID: 54
		public const byte AC_SRC_ALPHA = 1;

		// Token: 0x04000037 RID: 55
		public const int GWL_WNDPROC = -4;

		// Token: 0x04000038 RID: 56
		public const int WM_USER = 1024;

		// Token: 0x04000039 RID: 57
		public const int WHEEL_DELTA = 120;

		// Token: 0x0400003A RID: 58
		public const int WM_SETFOCUS = 7;

		// Token: 0x0400003B RID: 59
		public const int WM_KILLFOCUS = 8;

		// Token: 0x0400003C RID: 60
		public const int WM_TOUCH = 576;

		// Token: 0x0400003D RID: 61
		public const int WM_POINTERUPDATE = 581;

		// Token: 0x0400003E RID: 62
		public const int WM_POINTERDOWN = 582;

		// Token: 0x0400003F RID: 63
		public const int WM_POINTERUP = 583;

		// Token: 0x04000040 RID: 64
		public const int WM_POINTERENTER = 585;

		// Token: 0x04000041 RID: 65
		public const int WM_POINTERLEAVE = 586;

		// Token: 0x04000042 RID: 66
		public const int WM_GESTURE = 281;

		// Token: 0x04000043 RID: 67
		public const int WM_GESTURENOTIFY = 282;

		// Token: 0x04000044 RID: 68
		public const int WM_SIZE = 5;

		// Token: 0x04000045 RID: 69
		public const int WM_SETCURSOR = 32;

		// Token: 0x04000046 RID: 70
		public const int WM_MOUSEMOVE = 512;

		// Token: 0x04000047 RID: 71
		public const int WM_LBUTTONDOWN = 513;

		// Token: 0x04000048 RID: 72
		public const int WM_LBUTTONUP = 514;

		// Token: 0x04000049 RID: 73
		public const int WM_LBUTTONDBLCLK = 515;

		// Token: 0x0400004A RID: 74
		public const int WM_RBUTTONDOWN = 516;

		// Token: 0x0400004B RID: 75
		public const int WM_RBUTTONUP = 517;

		// Token: 0x0400004C RID: 76
		public const int WM_RBUTTONDBLCLK = 518;

		// Token: 0x0400004D RID: 77
		public const int WM_MOUSEWHEEL = 522;

		// Token: 0x0400004E RID: 78
		public const int WM_KEYDOWN = 256;

		// Token: 0x0400004F RID: 79
		public const int WM_KEYUP = 257;

		// Token: 0x04000050 RID: 80
		public const int WM_CHAR = 258;

		// Token: 0x04000051 RID: 81
		public const int WM_SYSKEYDOWN = 260;

		// Token: 0x04000052 RID: 82
		public const int WM_SYSKEYUP = 261;

		// Token: 0x04000053 RID: 83
		public const uint TWF_FINETOUCH = 1U;

		// Token: 0x04000054 RID: 84
		public const int SM_DIGITIZER = 94;

		// Token: 0x04000055 RID: 85
		public const int NID_INTEGRATED_TOUCH = 1;

		// Token: 0x04000056 RID: 86
		public const int NID_EXTERNAL_TOUCH = 2;

		// Token: 0x04000057 RID: 87
		public const int NID_INTEGRATED_PEN = 4;

		// Token: 0x04000058 RID: 88
		public const int NID_EXTERNAL_PEN = 8;

		// Token: 0x04000059 RID: 89
		public const int NID_MULTI_INPUT = 64;

		// Token: 0x0400005A RID: 90
		public const int NID_READY = 128;

		// Token: 0x0400005B RID: 91
		public const uint MI_WP_SIGNATURE = 4283520768U;

		// Token: 0x0400005C RID: 92
		public const uint SIGNATURE_MASK = 4294967040U;

		// Token: 0x0400005D RID: 93
		public const int SW_SHOWNORMAL = 1;

		// Token: 0x0400005E RID: 94
		public const int ERROR_MEMBER_IN_ALIAS = 1378;

		// Token: 0x0400005F RID: 95
		public const int MK_CONTROL = 8;

		// Token: 0x04000060 RID: 96
		public static readonly IntPtr HwndTop = IntPtr.Zero;

		// Token: 0x04000061 RID: 97
		public const int SWP_NOSIZE = 1;

		// Token: 0x04000062 RID: 98
		public const int SWP_NOMOVE = 2;

		// Token: 0x04000063 RID: 99
		public const int SWP_NOZORDER = 4;

		// Token: 0x04000064 RID: 100
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x060001E0 RID: 480
		public delegate IntPtr Win32WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x060001E4 RID: 484
		private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

		// Token: 0x0200003E RID: 62
		[Flags]
		public enum TouchFlags : uint
		{
			// Token: 0x0400014E RID: 334
			TouchEventMove = 1U,
			// Token: 0x0400014F RID: 335
			TouchEventDown = 2U,
			// Token: 0x04000150 RID: 336
			TouchEventUp = 4U,
			// Token: 0x04000151 RID: 337
			TouchEventInRange = 8U
		}

		// Token: 0x0200003F RID: 63
		[Flags]
		public enum SetWindowPosFlags
		{
			// Token: 0x04000153 RID: 339
			SWP_NOSIZE = 1,
			// Token: 0x04000154 RID: 340
			SWP_NOMOVE = 2
		}

		// Token: 0x02000040 RID: 64
		public enum RegionRet
		{
			// Token: 0x04000156 RID: 342
			ERROR,
			// Token: 0x04000157 RID: 343
			NULLREGION,
			// Token: 0x04000158 RID: 344
			SIMPLEREGION,
			// Token: 0x04000159 RID: 345
			COMPLEXREGION
		}

		// Token: 0x02000041 RID: 65
		public enum VHD_STORAGE_TYPE_DEVICE : uint
		{
			// Token: 0x0400015B RID: 347
			VIRTUAL_STORAGE_TYPE_DEVICE_UNKNOWN,
			// Token: 0x0400015C RID: 348
			VIRTUAL_STORAGE_TYPE_DEVICE_ISO,
			// Token: 0x0400015D RID: 349
			VIRTUAL_STORAGE_TYPE_DEVICE_VHD,
			// Token: 0x0400015E RID: 350
			VIRTUAL_STORAGE_TYPE_DEVICE_VHDX
		}

		// Token: 0x02000042 RID: 66
		[Flags]
		public enum CREATE_VIRTUAL_DISK_FLAG
		{
			// Token: 0x04000160 RID: 352
			CREATE_VIRTUAL_DISK_FLAG_NONE = 0,
			// Token: 0x04000161 RID: 353
			CREATE_VIRTUAL_DISK_FLAG_FULL_PHYSICAL_ALLOCATION = 1
		}

		// Token: 0x02000043 RID: 67
		[Flags]
		public enum OPEN_VIRTUAL_DISK_FLAG
		{
			// Token: 0x04000163 RID: 355
			OPEN_VIRTUAL_DISK_FLAG_NONE = 0,
			// Token: 0x04000164 RID: 356
			OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = 1,
			// Token: 0x04000165 RID: 357
			OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = 2,
			// Token: 0x04000166 RID: 358
			OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = 4
		}

		// Token: 0x02000044 RID: 68
		[StructLayout(LayoutKind.Explicit)]
		public struct OPEN_VIRTUAL_DISK_PARAMETERS
		{
			// Token: 0x04000167 RID: 359
			[FieldOffset(0)]
			public NativeMethods.OPEN_VIRTUAL_DISK_VERSION Version;

			// Token: 0x04000168 RID: 360
			[FieldOffset(4)]
			public NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS.OpenVersion1 Version1;

			// Token: 0x04000169 RID: 361
			[FieldOffset(4)]
			public NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS.OpenVersion2 Version2;

			// Token: 0x02000078 RID: 120
			public struct OpenVersion1
			{
				// Token: 0x04000249 RID: 585
				public uint RWDepth;
			}

			// Token: 0x02000079 RID: 121
			public struct OpenVersion2
			{
				// Token: 0x0400024A RID: 586
				public int GetInfoOnly;

				// Token: 0x0400024B RID: 587
				public int ReadOnly;

				// Token: 0x0400024C RID: 588
				public Guid ResiliencyGuid;

				// Token: 0x0400024D RID: 589
				public Guid SnapshotId;
			}
		}

		// Token: 0x02000045 RID: 69
		public enum OPEN_VIRTUAL_DISK_VERSION
		{
			// Token: 0x0400016B RID: 363
			OPEN_VIRTUAL_DISK_VERSION_UNSPECIFIED,
			// Token: 0x0400016C RID: 364
			OPEN_VIRTUAL_DISK_VERSION_1,
			// Token: 0x0400016D RID: 365
			OPEN_VIRTUAL_DISK_VERSION_2
		}

		// Token: 0x02000046 RID: 70
		public enum ATTACH_VIRTUAL_DISK_VERSION
		{
			// Token: 0x0400016F RID: 367
			ATTACH_VIRTUAL_DISK_VERSION_UNSPECIFIED,
			// Token: 0x04000170 RID: 368
			ATTACH_VIRTUAL_DISK_VERSION_1
		}

		// Token: 0x02000047 RID: 71
		public struct ATTACH_VIRTUAL_DISK_PARAMETERS
		{
			// Token: 0x04000171 RID: 369
			public NativeMethods.ATTACH_VIRTUAL_DISK_VERSION Version;

			// Token: 0x04000172 RID: 370
			public uint Reserved;
		}

		// Token: 0x02000048 RID: 72
		public enum GET_VIRTUAL_DISK_INFO_VERSION
		{
			// Token: 0x04000174 RID: 372
			GET_VIRTUAL_DISK_INFO_UNSPECIFIED,
			// Token: 0x04000175 RID: 373
			GET_VIRTUAL_DISK_INFO_SIZE,
			// Token: 0x04000176 RID: 374
			GET_VIRTUAL_DISK_INFO_IDENTIFIER,
			// Token: 0x04000177 RID: 375
			GET_VIRTUAL_DISK_INFO_PARENT_LOCATION,
			// Token: 0x04000178 RID: 376
			GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER,
			// Token: 0x04000179 RID: 377
			GET_VIRTUAL_DISK_INFO_PARENT_TIMESTAMP,
			// Token: 0x0400017A RID: 378
			GET_VIRTUAL_DISK_INFO_VIRTUAL_STORAGE_TYPE,
			// Token: 0x0400017B RID: 379
			GET_VIRTUAL_DISK_INFO_PROVIDER_SUBTYPE,
			// Token: 0x0400017C RID: 380
			GET_VIRTUAL_DISK_INFO_IS_4K_ALIGNED,
			// Token: 0x0400017D RID: 381
			GET_VIRTUAL_DISK_INFO_PHYSICAL_DISK,
			// Token: 0x0400017E RID: 382
			GET_VIRTUAL_DISK_INFO_VHD_PHYSICAL_SECTOR_SIZE,
			// Token: 0x0400017F RID: 383
			GET_VIRTUAL_DISK_INFO_SMALLEST_SAFE_VIRTUAL_SIZE,
			// Token: 0x04000180 RID: 384
			GET_VIRTUAL_DISK_INFO_FRAGMENTATION,
			// Token: 0x04000181 RID: 385
			GET_VIRTUAL_DISK_INFO_IS_LOADED,
			// Token: 0x04000182 RID: 386
			GET_VIRTUAL_DISK_INFO_VIRTUAL_DISK_ID,
			// Token: 0x04000183 RID: 387
			GET_VIRTUAL_DISK_INFO_CHANGE_TRACKING_STATE
		}

		// Token: 0x02000049 RID: 73
		public enum VIRTUAL_DISK_ACCESS_MASK
		{
			// Token: 0x04000185 RID: 389
			VIRTUAL_DISK_ACCESS_NONE,
			// Token: 0x04000186 RID: 390
			VIRTUAL_DISK_ACCESS_ATTACH_RO = 65536,
			// Token: 0x04000187 RID: 391
			VIRTUAL_DISK_ACCESS_ATTACH_RW = 131072,
			// Token: 0x04000188 RID: 392
			VIRTUAL_DISK_ACCESS_DETACH = 262144,
			// Token: 0x04000189 RID: 393
			VIRTUAL_DISK_ACCESS_GET_INFO = 524288,
			// Token: 0x0400018A RID: 394
			VIRTUAL_DISK_ACCESS_CREATE = 1048576,
			// Token: 0x0400018B RID: 395
			VIRTUAL_DISK_ACCESS_METAOPS = 2097152,
			// Token: 0x0400018C RID: 396
			VIRTUAL_DISK_ACCESS_READ = 851968,
			// Token: 0x0400018D RID: 397
			VIRTUAL_DISK_ACCESS_ALL = 4128768,
			// Token: 0x0400018E RID: 398
			VIRTUAL_DISK_ACCESS_WRITABLE = 3276800
		}

		// Token: 0x0200004A RID: 74
		public enum CREATE_VIRTUAL_DISK_VERSION
		{
			// Token: 0x04000190 RID: 400
			CREATE_VIRTUAL_DISK_VERSION_UNSPECIFIED,
			// Token: 0x04000191 RID: 401
			CREATE_VIRTUAL_DISK_VERSION_1
		}

		// Token: 0x0200004B RID: 75
		public enum ATTACH_VIRTUAL_DISK_FLAG
		{
			// Token: 0x04000193 RID: 403
			ATTACH_VIRTUAL_DISK_FLAG_NONE,
			// Token: 0x04000194 RID: 404
			ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY,
			// Token: 0x04000195 RID: 405
			ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER,
			// Token: 0x04000196 RID: 406
			ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = 4,
			// Token: 0x04000197 RID: 407
			ATTACH_VIRTUAL_DISK_FLAG_NO_LOCAL_HOST = 8,
			// Token: 0x04000198 RID: 408
			ATTACH_VIRTUAL_DISK_FLAG_NO_SECURITY_DESCRIPTOR = 16
		}

		// Token: 0x0200004C RID: 76
		public struct CREATE_VIRTUAL_DISK_PARAMETERS_V1
		{
			// Token: 0x04000199 RID: 409
			public Guid UniqueId;

			// Token: 0x0400019A RID: 410
			public ulong MaximumSize;

			// Token: 0x0400019B RID: 411
			public uint BlockSizeInBytes;

			// Token: 0x0400019C RID: 412
			public uint SectorSizeInBytes;

			// Token: 0x0400019D RID: 413
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ParentPath;

			// Token: 0x0400019E RID: 414
			[MarshalAs(UnmanagedType.LPWStr)]
			public string SourcePath;
		}

		// Token: 0x0200004D RID: 77
		public struct CREATE_VIRTUAL_DISK_PARAMETERS
		{
			// Token: 0x0400019F RID: 415
			public NativeMethods.CREATE_VIRTUAL_DISK_VERSION Version;

			// Token: 0x040001A0 RID: 416
			public NativeMethods.CREATE_VIRTUAL_DISK_PARAMETERS_V1 Version1Data;
		}

		// Token: 0x0200004E RID: 78
		public struct VIRTUAL_STORAGE_TYPE
		{
			// Token: 0x040001A1 RID: 417
			public NativeMethods.VHD_STORAGE_TYPE_DEVICE DeviceId;

			// Token: 0x040001A2 RID: 418
			public Guid VendorId;
		}

		// Token: 0x0200004F RID: 79
		public struct Point
		{
			// Token: 0x060001E7 RID: 487 RVA: 0x00005081 File Offset: 0x00003281
			public Point(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}

			// Token: 0x040001A3 RID: 419
			public int X;

			// Token: 0x040001A4 RID: 420
			public int Y;
		}

		// Token: 0x02000050 RID: 80
		public struct RECT
		{
			// Token: 0x040001A5 RID: 421
			public int left;

			// Token: 0x040001A6 RID: 422
			public int top;

			// Token: 0x040001A7 RID: 423
			public int right;

			// Token: 0x040001A8 RID: 424
			public int bottom;
		}

		// Token: 0x02000051 RID: 81
		public struct DXGI_OUTDUPL_MOVE_RECT
		{
			// Token: 0x040001A9 RID: 425
			private NativeMethods.Point SourcePoint;

			// Token: 0x040001AA RID: 426
			private NativeMethods.RECT DestinationRect;
		}

		// Token: 0x02000052 RID: 82
		public struct Size
		{
			// Token: 0x060001E8 RID: 488 RVA: 0x00005091 File Offset: 0x00003291
			public Size(int cx, int cy)
			{
				this.Width = cx;
				this.Height = cy;
			}

			// Token: 0x040001AB RID: 427
			public int Width;

			// Token: 0x040001AC RID: 428
			public int Height;
		}

		// Token: 0x02000053 RID: 83
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ARGB
		{
			// Token: 0x040001AD RID: 429
			public byte Blue;

			// Token: 0x040001AE RID: 430
			public byte Green;

			// Token: 0x040001AF RID: 431
			public byte Red;

			// Token: 0x040001B0 RID: 432
			public byte Alpha;
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BLENDFUNCTION
		{
			// Token: 0x040001B1 RID: 433
			public byte BlendOp;

			// Token: 0x040001B2 RID: 434
			public byte BlendFlags;

			// Token: 0x040001B3 RID: 435
			public byte SourceConstantAlpha;

			// Token: 0x040001B4 RID: 436
			public byte AlphaFormat;
		}

		// Token: 0x02000055 RID: 85
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct LOCALGROUP_MEMBERS_INFO_3
		{
			// Token: 0x040001B5 RID: 437
			[MarshalAs(UnmanagedType.LPWStr)]
			public string domainAndName;
		}

		// Token: 0x02000056 RID: 86
		public struct TOUCHINPUT
		{
			// Token: 0x040001B6 RID: 438
			public int X;

			// Token: 0x040001B7 RID: 439
			public int Y;

			// Token: 0x040001B8 RID: 440
			public IntPtr Source;

			// Token: 0x040001B9 RID: 441
			public uint Id;

			// Token: 0x040001BA RID: 442
			public NativeMethods.TouchFlags Flags;

			// Token: 0x040001BB RID: 443
			public uint Mask;

			// Token: 0x040001BC RID: 444
			public uint Time;

			// Token: 0x040001BD RID: 445
			public UIntPtr ExtraInfo;

			// Token: 0x040001BE RID: 446
			public uint ContactX;

			// Token: 0x040001BF RID: 447
			public uint ContactY;
		}

		// Token: 0x02000057 RID: 87
		public struct PROCESS_BASIC_INFORMATION
		{
			// Token: 0x040001C0 RID: 448
			public IntPtr Reserved1;

			// Token: 0x040001C1 RID: 449
			public IntPtr PebBaseAddress;

			// Token: 0x040001C2 RID: 450
			public IntPtr Reserved2_A;

			// Token: 0x040001C3 RID: 451
			public IntPtr Reserved2_B;

			// Token: 0x040001C4 RID: 452
			public IntPtr UniqueProcessId;

			// Token: 0x040001C5 RID: 453
			public IntPtr Reserved3;
		}

		// Token: 0x02000058 RID: 88
		public static class VIRTUAL_STORAGE_TYPE_VENDOR
		{
			// Token: 0x040001C6 RID: 454
			public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");

			// Token: 0x040001C7 RID: 455
			public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_UNKNOWN = Guid.Empty;
		}

		// Token: 0x02000059 RID: 89
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct GetVirtualDiskInfoIdentifier
		{
			// Token: 0x040001C8 RID: 456
			[FieldOffset(0)]
			public NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION Version;

			// Token: 0x040001C9 RID: 457
			[FieldOffset(8)]
			public Guid Identifier;
		}

		// Token: 0x0200005A RID: 90
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct GetVirtualDiskInfoParentLocation
		{
			// Token: 0x040001CA RID: 458
			[FieldOffset(0)]
			public NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION Version;

			// Token: 0x040001CB RID: 459
			[FieldOffset(8)]
			public NativeMethods.ParentLocation ParentLocation;
		}

		// Token: 0x0200005B RID: 91
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct ParentLocation
		{
			// Token: 0x040001CC RID: 460
			public int ParentResolved;

			// Token: 0x040001CD RID: 461
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string ParentPath;
		}

		// Token: 0x0200005C RID: 92
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct GetVirtualDiskInfoUnion
		{
			// Token: 0x040001CE RID: 462
			[FieldOffset(0)]
			public Guid Identifier;

			// Token: 0x040001CF RID: 463
			[FieldOffset(0)]
			public Guid ParentIdentifier;

			// Token: 0x040001D0 RID: 464
			[FieldOffset(0)]
			public NativeMethods.ParentLocation ParentLocation;

			// Token: 0x040001D1 RID: 465
			[FieldOffset(0)]
			public uint Reserved;
		}

		// Token: 0x0200005D RID: 93
		[Flags]
		public enum POINTER_INPUT_TYPE
		{
			// Token: 0x040001D3 RID: 467
			None = 0,
			// Token: 0x040001D4 RID: 468
			Pointer = 1,
			// Token: 0x040001D5 RID: 469
			Touch = 2,
			// Token: 0x040001D6 RID: 470
			Pen = 3,
			// Token: 0x040001D7 RID: 471
			Mouse = 4,
			// Token: 0x040001D8 RID: 472
			TouchPad = 5
		}

		// Token: 0x0200005E RID: 94
		[Flags]
		public enum POINTER_FLAGS
		{
			// Token: 0x040001DA RID: 474
			None = 0,
			// Token: 0x040001DB RID: 475
			New = 1,
			// Token: 0x040001DC RID: 476
			InRange = 2,
			// Token: 0x040001DD RID: 477
			InContact = 4,
			// Token: 0x040001DE RID: 478
			FirstButton = 16,
			// Token: 0x040001DF RID: 479
			SecondButton = 32,
			// Token: 0x040001E0 RID: 480
			ThirdButton = 64,
			// Token: 0x040001E1 RID: 481
			FourthButton = 128,
			// Token: 0x040001E2 RID: 482
			FifthButton = 256,
			// Token: 0x040001E3 RID: 483
			Primary = 8192,
			// Token: 0x040001E4 RID: 484
			Confidence = 16384,
			// Token: 0x040001E5 RID: 485
			Canceled = 32768,
			// Token: 0x040001E6 RID: 486
			Down = 65536,
			// Token: 0x040001E7 RID: 487
			Update = 131072,
			// Token: 0x040001E8 RID: 488
			Up = 262144,
			// Token: 0x040001E9 RID: 489
			Wheel = 524288,
			// Token: 0x040001EA RID: 490
			CaptureChanged = 2097152,
			// Token: 0x040001EB RID: 491
			HasTransform = 4194304
		}

		// Token: 0x0200005F RID: 95
		public enum POINTER_BUTTON_CHANGE_TYPE
		{
			// Token: 0x040001ED RID: 493
			POINTER_CHANGE_NONE,
			// Token: 0x040001EE RID: 494
			POINTER_CHANGE_FIRSTBUTTON_DOWN,
			// Token: 0x040001EF RID: 495
			POINTER_CHANGE_FIRSTBUTTON_UP,
			// Token: 0x040001F0 RID: 496
			POINTER_CHANGE_SECONDBUTTON_DOWN,
			// Token: 0x040001F1 RID: 497
			POINTER_CHANGE_SECONDBUTTON_UP,
			// Token: 0x040001F2 RID: 498
			POINTER_CHANGE_THIRDBUTTON_DOWN,
			// Token: 0x040001F3 RID: 499
			POINTER_CHANGE_THIRDBUTTON_UP,
			// Token: 0x040001F4 RID: 500
			POINTER_CHANGE_FOURTHBUTTON_DOWN,
			// Token: 0x040001F5 RID: 501
			POINTER_CHANGE_FOURTHBUTTON_UP,
			// Token: 0x040001F6 RID: 502
			POINTER_CHANGE_FIFTHBUTTON_DOWN,
			// Token: 0x040001F7 RID: 503
			POINTER_CHANGE_FIFTHBUTTON_UP
		}

		// Token: 0x02000060 RID: 96
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct POINTER_INFO
		{
			// Token: 0x040001F8 RID: 504
			public NativeMethods.POINTER_INPUT_TYPE pointerType;

			// Token: 0x040001F9 RID: 505
			public uint pointerId;

			// Token: 0x040001FA RID: 506
			public uint frameId;

			// Token: 0x040001FB RID: 507
			public NativeMethods.POINTER_FLAGS pointerFlags;

			// Token: 0x040001FC RID: 508
			public IntPtr sourceDevice;

			// Token: 0x040001FD RID: 509
			public IntPtr hwndTarget;

			// Token: 0x040001FE RID: 510
			public NativeMethods.Point ptPixelLocation;

			// Token: 0x040001FF RID: 511
			public NativeMethods.Point ptHimetricLocation;

			// Token: 0x04000200 RID: 512
			public NativeMethods.Point ptPixelLocationRaw;

			// Token: 0x04000201 RID: 513
			public NativeMethods.Point ptHimetricLocationRaw;

			// Token: 0x04000202 RID: 514
			public uint dwTime;

			// Token: 0x04000203 RID: 515
			public uint historyCount;

			// Token: 0x04000204 RID: 516
			public int InputData;

			// Token: 0x04000205 RID: 517
			public uint dwKeyStates;

			// Token: 0x04000206 RID: 518
			public ulong PerformanceCount;

			// Token: 0x04000207 RID: 519
			public NativeMethods.POINTER_BUTTON_CHANGE_TYPE ButtonChangeType;
		}

		// Token: 0x02000061 RID: 97
		public enum TOUCH_FLAGS
		{
			// Token: 0x04000209 RID: 521
			None
		}

		// Token: 0x02000062 RID: 98
		public enum TOUCH_MASK
		{
			// Token: 0x0400020B RID: 523
			None,
			// Token: 0x0400020C RID: 524
			ContactArea,
			// Token: 0x0400020D RID: 525
			Orienation,
			// Token: 0x0400020E RID: 526
			Pressure = 4
		}

		// Token: 0x02000063 RID: 99
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct POINTER_TOUCH_INFO
		{
			// Token: 0x0400020F RID: 527
			public NativeMethods.POINTER_INFO pointerInfo;

			// Token: 0x04000210 RID: 528
			public NativeMethods.TOUCH_FLAGS touchFlags;

			// Token: 0x04000211 RID: 529
			public NativeMethods.TOUCH_MASK touchMask;

			// Token: 0x04000212 RID: 530
			public NativeMethods.RECT rcContact;

			// Token: 0x04000213 RID: 531
			public NativeMethods.RECT rcContactRaw;

			// Token: 0x04000214 RID: 532
			public int orientation;

			// Token: 0x04000215 RID: 533
			public int pressure;
		}

		// Token: 0x02000064 RID: 100
		public enum PEN_FLAGS
		{
			// Token: 0x04000217 RID: 535
			None,
			// Token: 0x04000218 RID: 536
			Barrel,
			// Token: 0x04000219 RID: 537
			Inverted,
			// Token: 0x0400021A RID: 538
			Eraser = 4
		}

		// Token: 0x02000065 RID: 101
		public enum PEN_MASK
		{
			// Token: 0x0400021C RID: 540
			None,
			// Token: 0x0400021D RID: 541
			Pressure,
			// Token: 0x0400021E RID: 542
			Rotation,
			// Token: 0x0400021F RID: 543
			TiltX = 4,
			// Token: 0x04000220 RID: 544
			TiltY = 8
		}

		// Token: 0x02000066 RID: 102
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct POINTER_PEN_INFO
		{
			// Token: 0x04000221 RID: 545
			public NativeMethods.POINTER_INFO pointerInfo;

			// Token: 0x04000222 RID: 546
			public NativeMethods.PEN_FLAGS penFlags;

			// Token: 0x04000223 RID: 547
			public NativeMethods.PEN_MASK penMask;

			// Token: 0x04000224 RID: 548
			public uint pressure;

			// Token: 0x04000225 RID: 549
			public uint rotation;

			// Token: 0x04000226 RID: 550
			public int tiltX;

			// Token: 0x04000227 RID: 551
			public int tiltY;
		}

		// Token: 0x02000067 RID: 103
		[StructLayout(LayoutKind.Explicit, Pack = 8)]
		public struct TOUCH_PEN_UNION
		{
			// Token: 0x04000228 RID: 552
			[FieldOffset(0)]
			public NativeMethods.POINTER_TOUCH_INFO TouchInfo;

			// Token: 0x04000229 RID: 553
			[FieldOffset(0)]
			public NativeMethods.POINTER_PEN_INFO PenInfo;
		}

		// Token: 0x02000068 RID: 104
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct POINTER_TYPE_INFO
		{
			// Token: 0x0400022A RID: 554
			public NativeMethods.POINTER_INPUT_TYPE type;

			// Token: 0x0400022B RID: 555
			public NativeMethods.TOUCH_PEN_UNION TouchPen;
		}

		// Token: 0x02000069 RID: 105
		public enum DXGI_GPU_PREFERENCE
		{
			// Token: 0x0400022D RID: 557
			DXGI_GPU_PREFERENCE_UNSPECIFIED,
			// Token: 0x0400022E RID: 558
			DXGI_GPU_PREFERENCE_MINIMUM_POWER,
			// Token: 0x0400022F RID: 559
			DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE
		}

		// Token: 0x0200006A RID: 106
		public struct LUID
		{
			// Token: 0x060001EA RID: 490 RVA: 0x000050BC File Offset: 0x000032BC
			public ulong ToULong()
			{
				return (ulong)this.HighPart << 32 | (ulong)this.LowPart;
			}

			// Token: 0x04000230 RID: 560
			public uint LowPart;

			// Token: 0x04000231 RID: 561
			public uint HighPart;
		}

		// Token: 0x0200006B RID: 107
		public struct DXGI_ADAPTER_DESC
		{
			// Token: 0x04000232 RID: 562
			[FixedBuffer(typeof(char), 128)]
			public NativeMethods.DXGI_ADAPTER_DESC.<Description>e__FixedBuffer Description;

			// Token: 0x04000233 RID: 563
			public uint VendorId;

			// Token: 0x04000234 RID: 564
			public uint DeviceId;

			// Token: 0x04000235 RID: 565
			public uint SubSysId;

			// Token: 0x04000236 RID: 566
			public uint Revision;

			// Token: 0x04000237 RID: 567
			public UIntPtr DedicatedVideoMemory;

			// Token: 0x04000238 RID: 568
			public UIntPtr DedicatedSystemMemory;

			// Token: 0x04000239 RID: 569
			public UIntPtr SharedSystemMemory;

			// Token: 0x0400023A RID: 570
			public NativeMethods.LUID AdapterLuid;

			// Token: 0x0200007A RID: 122
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 256)]
			public struct <Description>e__FixedBuffer
			{
				// Token: 0x0400024E RID: 590
				public char FixedElementField;
			}
		}

		// Token: 0x0200006C RID: 108
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct D3DKMT_OPENADAPTERFROMDEVICENAME
		{
			// Token: 0x0400023B RID: 571
			public string pDeviceName;

			// Token: 0x0400023C RID: 572
			public uint hAdapter;

			// Token: 0x0400023D RID: 573
			public NativeMethods.LUID AdapterLuid;
		}

		// Token: 0x0200006D RID: 109
		public struct D3DKMT_CLOSEADAPTER
		{
			// Token: 0x0400023E RID: 574
			public uint hAdapter;
		}

		// Token: 0x0200006E RID: 110
		[Guid("2411e7e1-12ac-4ccf-bd14-9798e8534dc0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIAdapter
		{
			// Token: 0x060001EB RID: 491
			void SetPrivateData();

			// Token: 0x060001EC RID: 492
			void SetPrivateDataInterface();

			// Token: 0x060001ED RID: 493
			void GetPrivateData();

			// Token: 0x060001EE RID: 494
			void GetParent();

			// Token: 0x060001EF RID: 495
			void EnumOutputs();

			// Token: 0x060001F0 RID: 496
			void GetDesc(out NativeMethods.DXGI_ADAPTER_DESC desc);

			// Token: 0x060001F1 RID: 497
			void CheckInterfaceSupport();
		}

		// Token: 0x0200006F RID: 111
		[Guid("c1b6694f-ff09-44a9-b03c-77900a0a1d17")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIFactory6
		{
			// Token: 0x060001F2 RID: 498
			void SetPrivateData();

			// Token: 0x060001F3 RID: 499
			void SetPrivateDataInterface();

			// Token: 0x060001F4 RID: 500
			void GetPrivateData();

			// Token: 0x060001F5 RID: 501
			void GetParent();

			// Token: 0x060001F6 RID: 502
			void EnumAdapters();

			// Token: 0x060001F7 RID: 503
			void MakeWindowAssociation();

			// Token: 0x060001F8 RID: 504
			void GetWindowAssociation();

			// Token: 0x060001F9 RID: 505
			void CreateSwapChain();

			// Token: 0x060001FA RID: 506
			void CreateSoftwareAdapter();

			// Token: 0x060001FB RID: 507
			void EnumAdapters1();

			// Token: 0x060001FC RID: 508
			int IsCurrent();

			// Token: 0x060001FD RID: 509
			int IsWindowedStereoEnabled();

			// Token: 0x060001FE RID: 510
			void CreateSwapChainForHwnd();

			// Token: 0x060001FF RID: 511
			void CreateSwapChainForCoreWindow();

			// Token: 0x06000200 RID: 512
			void GetSharedResourceAdapterLuid();

			// Token: 0x06000201 RID: 513
			void RegisterStereoStatusWindow();

			// Token: 0x06000202 RID: 514
			void RegisterStereoStatusEvent();

			// Token: 0x06000203 RID: 515
			void UnregisterStereoStatus();

			// Token: 0x06000204 RID: 516
			void RegisterOcclusionStatusWindow();

			// Token: 0x06000205 RID: 517
			void RegisterOcclusionStatusEvent();

			// Token: 0x06000206 RID: 518
			void UnregisterOcclusionStatus();

			// Token: 0x06000207 RID: 519
			void CreateSwapChainForComposition();

			// Token: 0x06000208 RID: 520
			uint GetCreationFlags();

			// Token: 0x06000209 RID: 521
			void EnumAdapterByLuid();

			// Token: 0x0600020A RID: 522
			void EnumWarpAdapter();

			// Token: 0x0600020B RID: 523
			void CheckFeatureSupport();

			// Token: 0x0600020C RID: 524
			void EnumAdapterByGpuPreference(uint adapter, NativeMethods.DXGI_GPU_PREFERENCE GpuPreference, ref Guid riid, out NativeMethods.IDXGIAdapter adapgter);
		}
	}
}
