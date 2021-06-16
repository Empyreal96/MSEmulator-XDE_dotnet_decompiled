using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xde.Common;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000009 RID: 9
	internal class OptionalPackageDevice : XdeDevice
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002D14 File Offset: 0x00000F14
		private OptionalPackageDevice(string packageId, string fileName)
		{
			this.packageId = packageId;
			this.optionalPackageDevice = AppXdeDevice.LoadFromFile(fileName);
			string text = Path.Combine(XdeInstallation.DevicesFolder, string.Format("OptPkg-{0}.json", this.optionalPackageDevice.ID));
			if (File.Exists(text))
			{
				try
				{
					this.overrideDevice = AppXdeDevice.LoadFromFile(text);
				}
				catch (Exception)
				{
				}
			}
			if (this.overrideDevice == null)
			{
				this.overrideDevice = AppXdeDevice.InitNewDevice(text);
				this.overrideDevice.Name = this.optionalPackageDevice.Name;
				this.overrideDevice.UseCheckpoint = this.optionalPackageDevice.UseCheckpoint;
				this.overrideDevice.ID = this.optionalPackageDevice.ID;
				this.overrideDevice.NoGpu = this.optionalPackageDevice.NoGpu;
				this.overrideDevice.ShowDisplayName = this.optionalPackageDevice.ShowDisplayName;
				this.overrideDevice.MemSize = this.optionalPackageDevice.MemSize;
				this.overrideDevice.Save();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002E30 File Offset: 0x00001030
		public static OptionalPackageDevice LoadFromFile(string packageId, string fileName)
		{
			return new OptionalPackageDevice(packageId, fileName);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002E39 File Offset: 0x00001039
		public static IEnumerable<OptionalPackageDevice> LoadDevices()
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				yield break;
			}
			foreach (Package package in XdeAppUtils.EmulatorPackage.Dependencies)
			{
				string path = Path.Combine(package.InstalledLocation.Path, "content");
				List<XdeDevice> list = new List<XdeDevice>();
				if (Directory.Exists(path))
				{
					foreach (string fileName in Directory.GetFiles(path, "device*.json"))
					{
						OptionalPackageDevice item = OptionalPackageDevice.LoadFromFile(package.Id.FamilyName, fileName);
						list.Add(item);
					}
				}
				ReadOnlyCollection<XdeDevice> readOnlyDevices = list.AsReadOnly();
				foreach (XdeDevice xdeDevice in list)
				{
					OptionalPackageDevice optionalPackageDevice = (OptionalPackageDevice)xdeDevice;
					optionalPackageDevice.SetRelatedDevices(readOnlyDevices);
					yield return optionalPackageDevice;
				}
				List<XdeDevice>.Enumerator enumerator2 = default(List<XdeDevice>.Enumerator);
				readOnlyDevices = null;
			}
			IEnumerator<Package> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002E42 File Offset: 0x00001042
		public override bool IsDirty
		{
			get
			{
				return this.overrideDevice.IsDirty;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002E4F File Offset: 0x0000104F
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002E5C File Offset: 0x0000105C
		public override Guid ID
		{
			get
			{
				return this.optionalPackageDevice.ID;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002E63 File Offset: 0x00001063
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002E70 File Offset: 0x00001070
		public override string Name
		{
			get
			{
				return this.overrideDevice.Name;
			}
			set
			{
				this.overrideDevice.Name = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002E7E File Offset: 0x0000107E
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002E8B File Offset: 0x0000108B
		public override string Vhd
		{
			get
			{
				return this.optionalPackageDevice.Vhd;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002E92 File Offset: 0x00001092
		public override string UapVersion
		{
			get
			{
				return this.optionalPackageDevice.UapVersion;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002E9F File Offset: 0x0000109F
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002EAC File Offset: 0x000010AC
		public override bool UseCheckpoint
		{
			get
			{
				return this.optionalPackageDevice.UseCheckpoint;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002EB3 File Offset: 0x000010B3
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002EC0 File Offset: 0x000010C0
		public override string Sku
		{
			get
			{
				return this.optionalPackageDevice.Sku;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002EC7 File Offset: 0x000010C7
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002ED4 File Offset: 0x000010D4
		public override string Skin
		{
			get
			{
				return this.optionalPackageDevice.Skin;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002EDB File Offset: 0x000010DB
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002EE8 File Offset: 0x000010E8
		public override int MemSize
		{
			get
			{
				return this.overrideDevice.MemSize;
			}
			set
			{
				this.overrideDevice.MemSize = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002EF6 File Offset: 0x000010F6
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002F03 File Offset: 0x00001103
		public override bool NoGpu
		{
			get
			{
				return this.overrideDevice.NoGpu;
			}
			set
			{
				this.overrideDevice.NoGpu = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002F11 File Offset: 0x00001111
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002F1E File Offset: 0x0000111E
		public override bool ShowDisplayName
		{
			get
			{
				return this.overrideDevice.ShowDisplayName;
			}
			set
			{
				this.overrideDevice.ShowDisplayName = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002F2C File Offset: 0x0000112C
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002F39 File Offset: 0x00001139
		public override string DisplayName
		{
			get
			{
				return this.overrideDevice.DisplayName;
			}
			set
			{
				this.overrideDevice.DisplayName = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002F47 File Offset: 0x00001147
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002F54 File Offset: 0x00001154
		public override bool UseWmi
		{
			get
			{
				return this.overrideDevice.UseWmi;
			}
			set
			{
				this.overrideDevice.UseWmi = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002F62 File Offset: 0x00001162
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002F6F File Offset: 0x0000116F
		public override bool DisableStateSep
		{
			get
			{
				return this.optionalPackageDevice.DisableStateSep;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002F76 File Offset: 0x00001176
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002F79 File Offset: 0x00001179
		public override bool UseDiffDisk
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002F80 File Offset: 0x00001180
		public override string MacAddress
		{
			get
			{
				return this.optionalPackageDevice.MacAddress;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002F8D File Offset: 0x0000118D
		public override bool CanKernelDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002F90 File Offset: 0x00001190
		public override string XdeLocation
		{
			get
			{
				return this.optionalPackageDevice.XdeLocation;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002F9D File Offset: 0x0000119D
		public override string FileName
		{
			get
			{
				return this.optionalPackageDevice.FileName;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00002FAA File Offset: 0x000011AA
		public override bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002FAD File Offset: 0x000011AD
		public override bool AppShutsDownForDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002FB0 File Offset: 0x000011B0
		protected override bool UsingOldEmulator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002FB3 File Offset: 0x000011B3
		public override IReadOnlyList<XdeDevice> RelatedDevices
		{
			get
			{
				return this.relatedDevices;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002FBB File Offset: 0x000011BB
		private void SetRelatedDevices(IReadOnlyList<XdeDevice> relatedDevices)
		{
			this.relatedDevices = relatedDevices;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002FC4 File Offset: 0x000011C4
		public override bool CanModifyProperty(string propName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(propName);
			if (num <= 2601956777U)
			{
				if (num != 266367750U)
				{
					if (num != 2254608527U)
					{
						if (num != 2601956777U)
						{
							return false;
						}
						if (!(propName == "ShowDisplayName"))
						{
							return false;
						}
					}
					else if (!(propName == "UseWmi"))
					{
						return false;
					}
				}
				else if (!(propName == "Name"))
				{
					return false;
				}
			}
			else if (num <= 3179060416U)
			{
				if (num != 2987437714U)
				{
					if (num != 3179060416U)
					{
						return false;
					}
					if (!(propName == "NoGpu"))
					{
						return false;
					}
				}
				else if (!(propName == "VisibleToVisualStudio"))
				{
					return false;
				}
			}
			else if (num != 4024579583U)
			{
				if (num != 4176258230U)
				{
					return false;
				}
				if (!(propName == "DisplayName"))
				{
					return false;
				}
			}
			else if (!(propName == "MemSize"))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003094 File Offset: 0x00001294
		public override async Task Delete()
		{
			Package package = this.GetPackage();
			PackageManager pm = new PackageManager();
			Package p = pm.FindPackageForUser(string.Empty, package.Id.FullName);
			if (p != null)
			{
				foreach (XdeDevice xdeDevice in this.RelatedDevices)
				{
					OptionalPackageDevice device = (OptionalPackageDevice)xdeDevice;
					AppXdeDevice appXdeDevice = device.overrideDevice;
					if (appXdeDevice != null)
					{
						await appXdeDevice.Delete();
					}
					device.DeleteVirtualMachine();
					device = null;
				}
				IEnumerator<XdeDevice> enumerator = null;
				await pm.RemovePackageAsync(p.Id.FullName);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000030D9 File Offset: 0x000012D9
		public override void Save()
		{
			this.overrideDevice.Save();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000030E6 File Offset: 0x000012E6
		private Package GetPackage()
		{
			return XdeAppUtils.EmulatorPackage.Dependencies.FirstOrDefault((Package p) => StringComparer.OrdinalIgnoreCase.Equals(p.Id.FamilyName, this.packageId));
		}

		// Token: 0x04000026 RID: 38
		private AppXdeDevice optionalPackageDevice;

		// Token: 0x04000027 RID: 39
		private AppXdeDevice overrideDevice;

		// Token: 0x04000028 RID: 40
		private string packageId;

		// Token: 0x04000029 RID: 41
		private IReadOnlyList<XdeDevice> relatedDevices;
	}
}
