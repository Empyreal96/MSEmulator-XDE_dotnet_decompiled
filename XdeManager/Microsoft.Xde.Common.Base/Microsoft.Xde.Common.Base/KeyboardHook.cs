using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000009 RID: 9
	public sealed class KeyboardHook : IDisposable
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public KeyboardHook()
		{
			this.callback = new KeyboardHook.HookHandlerDelegate(this.HookCallback);
			using (Process currentProcess = Process.GetCurrentProcess())
			{
				using (ProcessModule mainModule = currentProcess.MainModule)
				{
					this.hookID = KeyboardHook.NativeMethods.SetWindowsHookEx(13, this.callback, KeyboardHook.NativeMethods.GetModuleHandle(mainModule.ModuleName), 0U);
				}
			}
			this.Enabled = true;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C40 File Offset: 0x00000E40
		~KeyboardHook()
		{
			this.Dispose(false);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003D RID: 61 RVA: 0x00002C70 File Offset: 0x00000E70
		// (remove) Token: 0x0600003E RID: 62 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public event EventHandler<KeyboardHookEventArgs> KeyIntercepted;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002CDD File Offset: 0x00000EDD
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002CE5 File Offset: 0x00000EE5
		public KeyboardHookFlags Flags { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002CEE File Offset: 0x00000EEE
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002CF6 File Offset: 0x00000EF6
		public bool Enabled { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00002CFF File Offset: 0x00000EFF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D10 File Offset: 0x00000F10
		private IntPtr HookCallback(int nCode, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam)
		{
			if (this.Enabled)
			{
				bool flag = false;
				if (this.Flags != KeyboardHookFlags.None && nCode >= 0 && (wParam == (IntPtr)257 || wParam == (IntPtr)261 || wParam == (IntPtr)256 || wParam == (IntPtr)260))
				{
					if (this.Flags.HasFlag(KeyboardHookFlags.EatWindowsKey))
					{
						int flags = lParam.flags;
						if (flags != 0)
						{
							if (flags == 1)
							{
								if (lParam.vkCode == 91 || lParam.vkCode == 92)
								{
									flag = true;
								}
							}
						}
						else if (lParam.vkCode == 27)
						{
							flag = true;
						}
					}
					if (this.Flags.HasFlag(KeyboardHookFlags.EatAltTab) && lParam.flags == 32 && lParam.vkCode == 9)
					{
						flag = true;
					}
				}
				this.OnKeyIntercepted(new KeyboardHookEventArgs(lParam.vkCode, lParam.scanCode, lParam.Down, lParam.Extended));
				if (flag)
				{
					return (IntPtr)1;
				}
			}
			return KeyboardHook.NativeMethods.CallNextHookEx(this.hookID, nCode, wParam, ref lParam);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E39 File Offset: 0x00001039
		private void OnKeyIntercepted(KeyboardHookEventArgs e)
		{
			EventHandler<KeyboardHookEventArgs> keyIntercepted = this.KeyIntercepted;
			if (keyIntercepted == null)
			{
				return;
			}
			keyIntercepted(this, e);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E4D File Offset: 0x0000104D
		private void Dispose(bool disposing)
		{
			if (this.hookID != IntPtr.Zero)
			{
				KeyboardHook.NativeMethods.UnhookWindowsHookEx(this.hookID);
				this.hookID = IntPtr.Zero;
			}
		}

		// Token: 0x0400001F RID: 31
		private const int WH_KEYBOARD_LL = 13;

		// Token: 0x04000020 RID: 32
		private const int WM_KEYDOWN = 256;

		// Token: 0x04000021 RID: 33
		private const int WM_KEYUP = 257;

		// Token: 0x04000022 RID: 34
		private const int WM_SYSKEYDOWN = 260;

		// Token: 0x04000023 RID: 35
		private const int WM_SYSKEYUP = 261;

		// Token: 0x04000024 RID: 36
		private const int KF_EXTENDED = 256;

		// Token: 0x04000025 RID: 37
		private const int KF_UP = 32768;

		// Token: 0x04000026 RID: 38
		private const int LLKHF_EXTENDED = 1;

		// Token: 0x04000027 RID: 39
		private const int LLKHF_UP = 128;

		// Token: 0x04000028 RID: 40
		private IntPtr hookID;

		// Token: 0x04000029 RID: 41
		private KeyboardHook.HookHandlerDelegate callback;

		// Token: 0x02000037 RID: 55
		// (Invoke) Token: 0x060001D3 RID: 467
		private delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam);

		// Token: 0x02000038 RID: 56
		private struct KBDLLHOOKSTRUCT
		{
			// Token: 0x1700008A RID: 138
			// (get) Token: 0x060001D6 RID: 470 RVA: 0x00005054 File Offset: 0x00003254
			public bool Down
			{
				get
				{
					return !this.Up;
				}
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000505F File Offset: 0x0000325F
			public bool Up
			{
				get
				{
					return (this.flags & 128) == 128;
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x060001D8 RID: 472 RVA: 0x00005074 File Offset: 0x00003274
			public bool Extended
			{
				get
				{
					return (this.flags & 1) == 1;
				}
			}

			// Token: 0x0400013C RID: 316
			public int vkCode;

			// Token: 0x0400013D RID: 317
			public int scanCode;

			// Token: 0x0400013E RID: 318
			public int flags;

			// Token: 0x0400013F RID: 319
			public int time;

			// Token: 0x04000140 RID: 320
			public int dwExtraInfo;
		}

		// Token: 0x02000039 RID: 57
		private class NativeMethods
		{
			// Token: 0x060001D9 RID: 473
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr GetModuleHandle(string lpModuleName);

			// Token: 0x060001DA RID: 474
			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHook.HookHandlerDelegate lpfn, IntPtr hMod, uint dwThreadId);

			// Token: 0x060001DB RID: 475
			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool UnhookWindowsHookEx(IntPtr hhk);

			// Token: 0x060001DC RID: 476
			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KeyboardHook.KBDLLHOOKSTRUCT lParam);

			// Token: 0x060001DD RID: 477
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern short GetKeyState(int keyCode);
		}
	}
}
