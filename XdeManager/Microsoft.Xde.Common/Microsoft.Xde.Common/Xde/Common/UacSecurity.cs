using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000067 RID: 103
	public class UacSecurity
	{
		// Token: 0x0600025D RID: 605 RVA: 0x00005371 File Offset: 0x00003571
		public static bool IsAdmin()
		{
			return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00005388 File Offset: 0x00003588
		public static bool IsHyperVAdmin()
		{
			string localGroupName = "Hyper-V Administrators";
			string name = WindowsIdentity.GetCurrent().Name;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int level = 3;
			IntPtr zero = IntPtr.Zero;
			if (NativeMethods.NetLocalGroupGetMembers(null, localGroupName, level, out zero, 4294967295U, out num, out num2, ref num3) != 0)
			{
				return false;
			}
			try
			{
				IntPtr intPtr = zero;
				for (int i = 0; i < num2; i++)
				{
					if (((NativeMethods.LOCALGROUP_MEMBERS_INFO_3)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.LOCALGROUP_MEMBERS_INFO_3))).domainAndName.Equals(name))
					{
						return true;
					}
					intPtr += Marshal.SizeOf(typeof(NativeMethods.LOCALGROUP_MEMBERS_INFO_3));
				}
			}
			finally
			{
				NativeMethods.NetApiBufferFree(zero);
			}
			return false;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00005444 File Offset: 0x00003644
		public static void AddShieldToButton(Button button)
		{
			button.FlatStyle = FlatStyle.System;
			NativeMethods.SendMessageW(button.Handle, 5644U, IntPtr.Zero, new IntPtr(-1));
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000546C File Offset: 0x0000366C
		public static void RestartElevated(string extraArgs, bool wait = false)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = true;
			processStartInfo.WorkingDirectory = Environment.CurrentDirectory;
			processStartInfo.FileName = Application.ExecutablePath;
			string text = Environment.CommandLine;
			int num = text.IndexOfAny(new char[]
			{
				'/',
				'-'
			});
			if (num != -1)
			{
				text = text.Substring(num);
			}
			else
			{
				text = string.Empty;
			}
			if (!string.IsNullOrEmpty(extraArgs))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " ";
				}
				text += extraArgs;
			}
			processStartInfo.Arguments = text;
			processStartInfo.Verb = "runas";
			try
			{
				Process process = Process.Start(processStartInfo);
				if (wait)
				{
					process.WaitForExit();
				}
			}
			catch (Win32Exception e)
			{
				Logger.Instance.LogException("RestartElevated", e);
			}
		}

		// Token: 0x0400016E RID: 366
		private const int BCM_FIRST = 5632;

		// Token: 0x0400016F RID: 367
		private const int BCM_SETSHIELD = 5644;
	}
}
