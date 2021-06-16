using System;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002B RID: 43
	public static class XdeAppUtils
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x000046CC File Offset: 0x000028CC
		static XdeAppUtils()
		{
			object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\SQMClient", "MSFTInternal", -1);
			int num = -1;
			if (value is int)
			{
				num = (int)value;
			}
			if (num == -1)
			{
				num = (Directory.Exists("\\\\winbuilds\\release") ? 1 : 0);
			}
			XdeAppUtils.IsInternalVersion = (num == 1);
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000471E File Offset: 0x0000291E
		public static string AppDataFolder
		{
			get
			{
				if (!XdeAppUtils.IsPackagedEmulatorInstalled)
				{
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\XdeAppUnpackaged");
				}
				return XdeAppUtils.GetUnvirtualizedPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\XdeApp"));
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000474F File Offset: 0x0000294F
		public static bool IsInternalVersion { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00004756 File Offset: 0x00002956
		public static Package EmulatorPackage
		{
			get
			{
				return new PackageManager().FindPackagesForUser(string.Empty, "Microsoft.MicrosoftEmulator_8wekyb3d8bbwe").FirstOrDefault<Package>();
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00004774 File Offset: 0x00002974
		public static bool IsRunningAsPackagedEmulator
		{
			get
			{
				bool result;
				try
				{
					Package package = Package.Current;
					result = StringComparer.OrdinalIgnoreCase.Equals(package.Id.FamilyName, "Microsoft.MicrosoftEmulator_8wekyb3d8bbwe");
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000047BC File Offset: 0x000029BC
		public static bool IsPackagedEmulatorInstalled
		{
			get
			{
				return XdeAppUtils.EmulatorPackage != null;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000047C8 File Offset: 0x000029C8
		public static void GetKernelDebuggerSettingsForVmName(string name, out string key, out int port)
		{
			byte[] array = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(name));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 4; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(".");
				}
				ulong number = BitConverter.ToUInt64(array, i * 8);
				stringBuilder.Append(StringUtilities.ConvertToBase36(number));
			}
			int num = 0;
			foreach (byte b in array)
			{
				num += (int)b;
			}
			key = stringBuilder.ToString();
			port = 50000 + num % 40;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00004860 File Offset: 0x00002A60
		public static string GetUnvirtualizedPath(string path)
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				return path;
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
			string text2 = Path.Combine(text, "packages", "Microsoft.MicrosoftEmulator_8wekyb3d8bbwe") + "\\";
			if (path.StartsWith(text, StringComparison.OrdinalIgnoreCase) && !path.StartsWith(text2, StringComparison.OrdinalIgnoreCase) && Directory.Exists(text2))
			{
				string path2 = path.Substring(text.Length);
				return Path.Combine(text2, "LocalCache\\Local", path2);
			}
			return path;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000048DC File Offset: 0x00002ADC
		public static void CleanCoreconDatabase()
		{
			string[] directories = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\VisualStudio"), "corecon", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				Directory.Delete(directories[i], true);
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00004920 File Offset: 0x00002B20
		public static void EnsureLegacyAssemblyInGac()
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				return;
			}
			string text = Path.Combine(Path.Combine(XdeAppUtils.EmulatorPackage.InstalledLocation.Path, "XDE\\ref"), "microsoft.xde.interface.public.dll");
			if (File.Exists(text))
			{
				new Publish().GacInstall(text);
			}
		}

		// Token: 0x04000107 RID: 263
		public const string MicrosoftEmulatorFamilyName = "Microsoft.MicrosoftEmulator_8wekyb3d8bbwe";
	}
}
