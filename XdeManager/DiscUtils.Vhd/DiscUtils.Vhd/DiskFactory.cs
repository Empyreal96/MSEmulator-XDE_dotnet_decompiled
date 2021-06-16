using System;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils.Vhd
{
	// Token: 0x02000005 RID: 5
	[VirtualDiskFactory("VHD", ".vhd,.avhd")]
	internal sealed class DiskFactory : VirtualDiskFactory
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000298D File Offset: 0x00000B8D
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

		// Token: 0x06000029 RID: 41 RVA: 0x000029A5 File Offset: 0x00000BA5
		public override VirtualDiskTypeInfo GetDiskTypeInformation(string variant)
		{
			return DiskFactory.MakeDiskTypeInfo(variant);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029B0 File Offset: 0x00000BB0
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
					diskBuilder.DiskType = FileType.Dynamic;
				}
				else
				{
					diskBuilder.DiskType = FileType.Fixed;
				}
				return diskBuilder;
			}
			IL_37:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown VHD disk variant '{0}'", new object[]
			{
				variant
			}), "variant");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A1C File Offset: 0x00000C1C
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
					return Disk.InitializeDynamic(locator, path, diskParameters.Capacity, diskParameters.Geometry, 2097152L);
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown VHD disk variant '{0}'", new object[]
			{
				variant
			}), "variant");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A9D File Offset: 0x00000C9D
		public override VirtualDisk OpenDisk(string path, FileAccess access)
		{
			return new Disk(path, access);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public override VirtualDisk OpenDisk(FileLocator locator, string path, FileAccess access)
		{
			return new Disk(locator, path, access);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public override VirtualDiskLayer OpenDiskLayer(FileLocator locator, string path, FileAccess access)
		{
			return new DiskImageFile(locator, path, access);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002ABC File Offset: 0x00000CBC
		internal static VirtualDiskTypeInfo MakeDiskTypeInfo(string variant)
		{
			VirtualDiskTypeInfo virtualDiskTypeInfo = new VirtualDiskTypeInfo();
			virtualDiskTypeInfo.Name = "VHD";
			virtualDiskTypeInfo.Variant = variant;
			virtualDiskTypeInfo.CanBeHardDisk = true;
			virtualDiskTypeInfo.DeterministicGeometry = true;
			virtualDiskTypeInfo.PreservesBiosGeometry = false;
			virtualDiskTypeInfo.CalcGeometry = ((long c) => Geometry.FromCapacity(c));
			return virtualDiskTypeInfo;
		}
	}
}
