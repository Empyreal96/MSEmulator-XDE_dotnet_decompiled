using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000A RID: 10
	public class PackageInfo
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002E7C File Offset: 0x0000107C
		public static PackageInfo Current
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				uint capacity = (uint)stringBuilder.Capacity;
				if (PackageInfo.GetCurrentPackageFamilyName(ref capacity, stringBuilder) == 0)
				{
					uint num = (uint)Marshal.SizeOf(typeof(PackageInfo.PACKAGE_ID));
					GCHandle gchandle = GCHandle.Alloc(num, GCHandleType.Pinned);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						if (PackageInfo.GetCurrentPackageId(ref num, intPtr) == 0)
						{
							PackageInfo.PACKAGE_ID package_ID = Marshal.PtrToStructure<PackageInfo.PACKAGE_ID>(intPtr);
							Version version = new Version((int)package_ID.version.Major, (int)package_ID.version.Minor, (int)package_ID.version.Build, (int)package_ID.version.Revision);
							return new PackageInfo
							{
								FamilyName = stringBuilder.ToString(),
								Version = version
							};
						}
					}
					finally
					{
						gchandle.Free();
					}
				}
				return null;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002F54 File Offset: 0x00001154
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002F5C File Offset: 0x0000115C
		public string FamilyName { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002F65 File Offset: 0x00001165
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002F6D File Offset: 0x0000116D
		public Version Version { get; private set; }

		// Token: 0x0600004C RID: 76
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetCurrentPackageFamilyName(ref uint length, StringBuilder packageFamilyName);

		// Token: 0x0600004D RID: 77
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetCurrentPackageId(ref uint size, IntPtr buffer);

		// Token: 0x0200003A RID: 58
		private struct PACKAGE_VERSION
		{
			// Token: 0x04000141 RID: 321
			public ushort Revision;

			// Token: 0x04000142 RID: 322
			public ushort Build;

			// Token: 0x04000143 RID: 323
			public ushort Minor;

			// Token: 0x04000144 RID: 324
			public ushort Major;
		}

		// Token: 0x0200003B RID: 59
		private struct PACKAGE_ID
		{
			// Token: 0x04000145 RID: 325
			public uint reserved;

			// Token: 0x04000146 RID: 326
			public uint processorArchitecture;

			// Token: 0x04000147 RID: 327
			public PackageInfo.PACKAGE_VERSION version;

			// Token: 0x04000148 RID: 328
			public IntPtr name;

			// Token: 0x04000149 RID: 329
			public IntPtr publisher;

			// Token: 0x0400014A RID: 330
			public IntPtr resourceId;

			// Token: 0x0400014B RID: 331
			public IntPtr publisherId;

			// Token: 0x0400014C RID: 332
			[FixedBuffer(typeof(char), 1024)]
			public PackageInfo.PACKAGE_ID.<fixedBuffer>e__FixedBuffer fixedBuffer;

			// Token: 0x02000077 RID: 119
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 2048)]
			public struct <fixedBuffer>e__FixedBuffer
			{
				// Token: 0x04000248 RID: 584
				public char FixedElementField;
			}
		}
	}
}
