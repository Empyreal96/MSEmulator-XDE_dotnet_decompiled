using System;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000009 RID: 9
	[VirtualDiskFactory("VHDX", ".vhdx,.avhdx")]
	internal sealed class DiskFactory : VirtualDiskFactory
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000034AB File Offset: 0x000016AB
		public override string[] Variants
		{
			get
			{
				return new string[]
				{
					"fixed",
					"dynamic"
				};
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000034C3 File Offset: 0x000016C3
		public override VirtualDiskTypeInfo GetDiskTypeInformation(string variant)
		{
			return DiskFactory.MakeDiskTypeInfo(variant);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000034CC File Offset: 0x000016CC
		public override DiskImageBuilder GetImageBuilder(string variant)
		{
			DiskBuilder diskBuilder = new DiskBuilder();
			if (variant != null)
			{
				if (!(variant == "fixed"))
				{
					if (!(variant == "dynamic"))
					{
						goto IL_37;
					}
					diskBuilder.DiskType = DiskType.Dynamic;
				}
				else
				{
					diskBuilder.DiskType = DiskType.Fixed;
				}
				return diskBuilder;
			}
			IL_37:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown VHD disk variant '{0}'", new object[]
			{
				variant
			}), "variant");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003538 File Offset: 0x00001738
		public override VirtualDisk CreateDisk(FileLocator locator, string variant, string path, VirtualDiskParameters diskParameters)
		{
			if (variant != null)
			{
				if (variant == "fixed")
				{
					return Disk.InitializeFixed(locator, path, diskParameters.Capacity, diskParameters.Geometry);
				}
				if (variant == "dynamic")
				{
					return Disk.InitializeDynamic(locator, path, diskParameters.Capacity, 33554432L);
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown VHD disk variant '{0}'", new object[]
			{
				variant
			}), "variant");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000035B2 File Offset: 0x000017B2
		public override VirtualDisk OpenDisk(string path, FileAccess access)
		{
			return new Disk(path, access);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000035BB File Offset: 0x000017BB
		public override VirtualDisk OpenDisk(FileLocator locator, string path, FileAccess access)
		{
			return new Disk(locator, path, access);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000035C5 File Offset: 0x000017C5
		public override VirtualDiskLayer OpenDiskLayer(FileLocator locator, string path, FileAccess access)
		{
			return new DiskImageFile(locator, path, access);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000035D0 File Offset: 0x000017D0
		internal static VirtualDiskTypeInfo MakeDiskTypeInfo(string variant)
		{
			VirtualDiskTypeInfo virtualDiskTypeInfo = new VirtualDiskTypeInfo();
			virtualDiskTypeInfo.Name = "VHDX";
			virtualDiskTypeInfo.Variant = variant;
			virtualDiskTypeInfo.CanBeHardDisk = true;
			virtualDiskTypeInfo.DeterministicGeometry = true;
			virtualDiskTypeInfo.PreservesBiosGeometry = false;
			virtualDiskTypeInfo.CalcGeometry = ((long c) => Geometry.FromCapacity(c));
			return virtualDiskTypeInfo;
		}
	}
}
