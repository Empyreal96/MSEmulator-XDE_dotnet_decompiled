using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xde.Common;
using Newtonsoft.Json;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000003 RID: 3
	[JsonObject(MemberSerialization.OptIn)]
	internal class AppXdeDevice : XdeDevice
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000234C File Offset: 0x0000054C
		public override bool IsDirty
		{
			get
			{
				if (!File.Exists(this.fileName))
				{
					return true;
				}
				bool result;
				using (StringWriter stringWriter = new StringWriter())
				{
					new JsonSerializer().Serialize(stringWriter, this);
					result = (stringWriter.ToString() != File.ReadAllText(this.fileName));
				}
				return result;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000023B0 File Offset: 0x000005B0
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000023B8 File Offset: 0x000005B8
		[JsonProperty]
		public override string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					base.OnPropertyChanged("Name");
					base.OnPropertyChanged("MACAddress");
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000023E5 File Offset: 0x000005E5
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000023ED File Offset: 0x000005ED
		[JsonProperty]
		public override string Vhd { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000023F6 File Offset: 0x000005F6
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000023FE File Offset: 0x000005FE
		[JsonProperty]
		public override bool UseCheckpoint { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002407 File Offset: 0x00000607
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000240F File Offset: 0x0000060F
		[JsonProperty]
		public override string Sku { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002418 File Offset: 0x00000618
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002420 File Offset: 0x00000620
		[JsonProperty]
		public override string Skin { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002429 File Offset: 0x00000629
		public override string XdeLocation
		{
			get
			{
				return "[UseAppInstallXde]";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002438 File Offset: 0x00000638
		[JsonProperty]
		public override int MemSize { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002441 File Offset: 0x00000641
		public override string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002449 File Offset: 0x00000649
		public override bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000244C File Offset: 0x0000064C
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002454 File Offset: 0x00000654
		[JsonProperty]
		public override bool NoGpu { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000245D File Offset: 0x0000065D
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002465 File Offset: 0x00000665
		[JsonProperty]
		public override bool ShowDisplayName { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000246E File Offset: 0x0000066E
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002476 File Offset: 0x00000676
		[JsonProperty]
		public override string DisplayName { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000247F File Offset: 0x0000067F
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002487 File Offset: 0x00000687
		[JsonProperty]
		public override bool UseWmi { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002490 File Offset: 0x00000690
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002498 File Offset: 0x00000698
		[JsonProperty]
		public override bool DisableStateSep { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024A1 File Offset: 0x000006A1
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000024A9 File Offset: 0x000006A9
		[JsonProperty]
		public override Guid ID { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000024B2 File Offset: 0x000006B2
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000024BA File Offset: 0x000006BA
		[JsonProperty]
		public override bool UseDiffDisk { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000024C3 File Offset: 0x000006C3
		public override string MacAddress
		{
			get
			{
				return XdeMacAddressSettings.GetMacAddressForVmName(base.VmName);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024D0 File Offset: 0x000006D0
		public override bool CanKernelDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000024D3 File Offset: 0x000006D3
		protected override bool UsingOldEmulator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024D8 File Offset: 0x000006D8
		public override string UapVersion
		{
			get
			{
				DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.Vhd);
				if (downloadedVhdInfo != null)
				{
					return downloadedVhdInfo.UapVersion;
				}
				return "10.0.18362.0";
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002500 File Offset: 0x00000700
		public static IEnumerable<AppXdeDevice> LoadDevices()
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled || !Directory.Exists(XdeInstallation.DevicesFolder))
			{
				yield break;
			}
			foreach (string path in Directory.GetFiles(XdeInstallation.DevicesFolder, "*.json"))
			{
				Guid guid;
				if (Guid.TryParse(Path.GetFileNameWithoutExtension(path), out guid))
				{
					yield return AppXdeDevice.LoadFromFile(path);
				}
			}
			string[] array = null;
			yield break;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000250C File Offset: 0x0000070C
		public static AppXdeDevice LoadFromFile(string path)
		{
			AppXdeDevice result;
			using (StreamReader streamReader = File.OpenText(path))
			{
				AppXdeDevice appXdeDevice = (AppXdeDevice)new JsonSerializer().Deserialize(streamReader, typeof(AppXdeDevice));
				appXdeDevice.fileName = path;
				if (appXdeDevice.Vhd != null && !Path.IsPathRooted(appXdeDevice.Vhd))
				{
					appXdeDevice.Vhd = Path.Combine(Path.GetDirectoryName(path), appXdeDevice.Vhd);
				}
				result = appXdeDevice;
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002590 File Offset: 0x00000790
		public static AppXdeDevice InitNewDevice(string fileName)
		{
			AppXdeDevice appXdeDevice = new AppXdeDevice();
			appXdeDevice.ID = Guid.NewGuid();
			appXdeDevice.UseDiffDisk = true;
			appXdeDevice.ShowDisplayName = true;
			if (fileName == null)
			{
				if (!XdeAppUtils.IsPackagedEmulatorInstalled)
				{
					throw new InvalidOperationException("The packaged emulator is not installed.");
				}
				fileName = Path.Combine(XdeInstallation.DevicesFolder, string.Format("{0}.json", appXdeDevice.ID));
			}
			appXdeDevice.fileName = fileName;
			return appXdeDevice;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000025FA File Offset: 0x000007FA
		public override Task Delete()
		{
			base.DeleteVirtualMachine();
			if (File.Exists(this.fileName))
			{
				File.Delete(this.fileName);
			}
			return Task.FromResult<int>(0);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002620 File Offset: 0x00000820
		public override void Save()
		{
			string directoryName = Path.GetDirectoryName(this.fileName);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = File.CreateText(this.fileName))
			{
				new JsonSerializer().Serialize(streamWriter, this);
			}
		}

		// Token: 0x04000003 RID: 3
		public const string UseAppInstallXde = "[UseAppInstallXde]";

		// Token: 0x04000004 RID: 4
		private string name;

		// Token: 0x04000005 RID: 5
		private string fileName;
	}
}
