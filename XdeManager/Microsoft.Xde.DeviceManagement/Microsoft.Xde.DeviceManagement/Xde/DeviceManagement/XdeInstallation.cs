using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xde.Common;
using Windows.ApplicationModel;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000011 RID: 17
	public class XdeInstallation
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000499D File Offset: 0x00002B9D
		public static string DevicesFolder
		{
			get
			{
				return Path.Combine(XdeAppUtils.AppDataFolder, "devices");
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000049B7 File Offset: 0x00002BB7
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000049AE File Offset: 0x00002BAE
		public string ResolvedXdePath
		{
			get
			{
				if (this.resolvedXdePath != null)
				{
					return this.resolvedXdePath;
				}
				return this.XdePath;
			}
			private set
			{
				this.resolvedXdePath = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000049CE File Offset: 0x00002BCE
		// (set) Token: 0x0600014E RID: 334 RVA: 0x000049D6 File Offset: 0x00002BD6
		public string XdePath { get; private set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000049DF File Offset: 0x00002BDF
		// (set) Token: 0x06000150 RID: 336 RVA: 0x000049E7 File Offset: 0x00002BE7
		public string Name { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000049F0 File Offset: 0x00002BF0
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000049F8 File Offset: 0x00002BF8
		public IReadOnlyCollection<XdeSku> Skus { get; private set; }

		// Token: 0x06000153 RID: 339 RVA: 0x00004A04 File Offset: 0x00002C04
		public static XdeInstallation LoadFromPath(string path)
		{
			if (path == "[UseAppInstallXde]")
			{
				return XdeInstallation.GetAppInstallXdeInstallation();
			}
			if (!File.Exists(path))
			{
				return null;
			}
			XdeInstallation xdeInstallation = new XdeInstallation();
			xdeInstallation.XdePath = path;
			xdeInstallation.Name = path;
			List<string> list = new List<string>();
			list.Add(Path.Combine(Path.GetDirectoryName(path), "SKUs"));
			list.AddRange(XdeInstallation.FindOptionalPackageSkuPaths());
			List<XdeSku> list2 = new List<XdeSku>();
			foreach (string path2 in list)
			{
				string[] directories = Directory.GetDirectories(path2);
				for (int i = 0; i < directories.Length; i++)
				{
					XdeSku xdeSku = XdeSku.LoadFromPath(directories[i]);
					if (xdeSku != null)
					{
						list2.Add(xdeSku);
					}
				}
			}
			xdeInstallation.Skus = list2;
			return xdeInstallation;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public static void EnsureAppInstallHasDevice()
		{
			XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
			if (appInstallXdeInstallation == null)
			{
				return;
			}
			HashSet<string> namesHash = new HashSet<string>();
			if (AppXdeDevice.LoadDevices().FirstOrDefault<AppXdeDevice>() != null)
			{
				return;
			}
			XdeInstallation.GetDeviceNamesHash();
			foreach (XdeSku sku in appInstallXdeInstallation.Skus)
			{
				appInstallXdeInstallation.CreateDeviceForSkuWithHash(sku, namesHash);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004B58 File Offset: 0x00002D58
		public static XdeInstallation GetAppInstallXdeInstallation()
		{
			string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\xde\\xde.exe");
			if (File.Exists(text))
			{
				XdeInstallation xdeInstallation = XdeInstallation.LoadFromPath(text);
				if (xdeInstallation != null)
				{
					xdeInstallation.XdePath = "[UseAppInstallXde]";
					xdeInstallation.ResolvedXdePath = Path.GetFullPath(new Uri(text).LocalPath);
					xdeInstallation.Name = "Microsoft Emulator";
					return xdeInstallation;
				}
			}
			return null;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004BC0 File Offset: 0x00002DC0
		public static IEnumerable<XdeInstallation> GetXdeInstallations()
		{
			HashSet<string> foundInstallations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
			if (appInstallXdeInstallation != null)
			{
				foundInstallations.Add(appInstallXdeInstallation.XdePath);
				yield return appInstallXdeInstallation;
			}
			foreach (VisualStudioXdeDevice visualStudioXdeDevice in VisualStudioXdeDevice.LoadDevices())
			{
				if (!foundInstallations.Contains(visualStudioXdeDevice.XdeLocation))
				{
					foundInstallations.Add(visualStudioXdeDevice.XdeLocation);
					XdeInstallation xdeInstallation = XdeInstallation.LoadFromPath(visualStudioXdeDevice.XdeLocation);
					if (xdeInstallation != null)
					{
						yield return xdeInstallation;
					}
				}
			}
			IEnumerator<VisualStudioXdeDevice> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004BCC File Offset: 0x00002DCC
		public XdeDevice CreateDeviceForSku(XdeSku sku)
		{
			HashSet<string> deviceNamesHash = XdeInstallation.GetDeviceNamesHash();
			return this.CreateDeviceForSkuWithHash(sku, deviceNamesHash);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004BE7 File Offset: 0x00002DE7
		private static IEnumerable<string> FindOptionalPackageSkuPaths()
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				yield break;
			}
			foreach (Package package in XdeAppUtils.EmulatorPackage.Dependencies)
			{
				string text = Path.Combine(package.InstalledLocation.Path, "skus");
				if (Directory.Exists(text))
				{
					yield return text;
				}
			}
			IEnumerator<Package> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004BF0 File Offset: 0x00002DF0
		private static HashSet<string> GetDeviceNamesHash()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (XdeDevice xdeDevice in XdeDeviceFactory.GetDevices())
			{
				hashSet.Add(xdeDevice.Name);
			}
			return hashSet;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004C4C File Offset: 0x00002E4C
		private XdeDevice CreateDeviceForSkuWithHash(XdeSku sku, HashSet<string> namesHash)
		{
			SkuData skuData = sku.GetSkuData();
			string defaultDeviceName = skuData.Options.DefaultDeviceName;
			string text = defaultDeviceName;
			int num = 1;
			while (namesHash.Contains(text))
			{
				text = string.Format("{0} ({1})", defaultDeviceName, num);
				num++;
			}
			XdeDevice xdeDevice = XdeDeviceFactory.InitNewDevice();
			namesHash.Add(text);
			xdeDevice.Name = text;
			xdeDevice.MemSize = skuData.Options.DefaultMemSize;
			xdeDevice.Skin = skuData.Options.DefaultSkin;
			xdeDevice.UseCheckpoint = false;
			xdeDevice.Sku = sku.Name;
			xdeDevice.ShowDisplayName = true;
			xdeDevice.Save();
			return xdeDevice;
		}

		// Token: 0x0400004C RID: 76
		private const string AppInstallXdeName = "Microsoft Emulator";

		// Token: 0x0400004D RID: 77
		private string resolvedXdePath;
	}
}
