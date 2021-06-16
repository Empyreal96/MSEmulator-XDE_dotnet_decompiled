using System;
using System.Globalization;
using System.Text;
using Microsoft.WindowsAPICodePack.Resources;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000005 RID: 5
	public static class CoreHelpers
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002096 File Offset: 0x00000296
		public static bool RunningOnXP
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020BC File Offset: 0x000002BC
		public static void ThrowIfNotXP()
		{
			if (!CoreHelpers.RunningOnXP)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOnXp);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020D0 File Offset: 0x000002D0
		public static bool RunningOnVista
		{
			get
			{
				return Environment.OSVersion.Version.Major >= 6;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020E7 File Offset: 0x000002E7
		public static void ThrowIfNotVista()
		{
			if (!CoreHelpers.RunningOnVista)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOnVista);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020FB File Offset: 0x000002FB
		public static bool RunningOnWin7
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002128 File Offset: 0x00000328
		public static void ThrowIfNotWin7()
		{
			if (!CoreHelpers.RunningOnWin7)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOn7);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000213C File Offset: 0x0000033C
		public static string GetStringResource(string resourceId)
		{
			if (string.IsNullOrEmpty(resourceId))
			{
				return string.Empty;
			}
			resourceId = resourceId.Replace("shell32,dll", "shell32.dll");
			string[] array = resourceId.Split(new char[]
			{
				','
			});
			IntPtr instanceHandle = CoreNativeMethods.LoadLibrary(Environment.ExpandEnvironmentVariables(array[0].Replace("@", string.Empty)));
			array[1] = array[1].Replace("-", string.Empty);
			int id = int.Parse(array[1], CultureInfo.InvariantCulture);
			StringBuilder stringBuilder = new StringBuilder(255);
			if (CoreNativeMethods.LoadString(instanceHandle, id, stringBuilder, 255) == 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}
	}
}
