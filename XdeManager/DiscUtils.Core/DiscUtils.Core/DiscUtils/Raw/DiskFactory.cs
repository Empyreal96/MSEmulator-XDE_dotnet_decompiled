using System;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Raw
{
	// Token: 0x02000047 RID: 71
	[VirtualDiskFactory("RAW", ".img,.ima,.vfd,.flp,.bif")]
	internal sealed class DiskFactory : VirtualDiskFactory
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00006640 File Offset: 0x00004840
		public override string[] Variants
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00006648 File Offset: 0x00004848
		public override VirtualDiskTypeInfo GetDiskTypeInformation(string variant)
		{
			return DiskFactory.MakeDiskTypeInfo();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000664F File Offset: 0x0000484F
		public override DiskImageBuilder GetImageBuilder(string variant)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00006656 File Offset: 0x00004856
		public override VirtualDisk CreateDisk(FileLocator locator, string variant, string path, VirtualDiskParameters diskParameters)
		{
			return Disk.Initialize(locator.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None), Ownership.Dispose, diskParameters.Capacity, diskParameters.Geometry);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00006676 File Offset: 0x00004876
		public override VirtualDisk OpenDisk(string path, FileAccess access)
		{
			return new Disk(path, access);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00006680 File Offset: 0x00004880
		public override VirtualDisk OpenDisk(FileLocator locator, string path, FileAccess access)
		{
			FileShare share = (access == FileAccess.Read) ? FileShare.Read : FileShare.None;
			return new Disk(locator.Open(path, FileMode.Open, access, share), Ownership.Dispose);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000066A6 File Offset: 0x000048A6
		public override VirtualDiskLayer OpenDiskLayer(FileLocator locator, string path, FileAccess access)
		{
			return null;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000066AC File Offset: 0x000048AC
		internal static VirtualDiskTypeInfo MakeDiskTypeInfo()
		{
			VirtualDiskTypeInfo virtualDiskTypeInfo = new VirtualDiskTypeInfo();
			virtualDiskTypeInfo.Name = "RAW";
			virtualDiskTypeInfo.Variant = string.Empty;
			virtualDiskTypeInfo.CanBeHardDisk = true;
			virtualDiskTypeInfo.DeterministicGeometry = true;
			virtualDiskTypeInfo.PreservesBiosGeometry = false;
			virtualDiskTypeInfo.CalcGeometry = ((long c) => Geometry.FromCapacity(c));
			return virtualDiskTypeInfo;
		}
	}
}
