using System;
using System.Collections.Generic;
using System.Globalization;
using DiscUtils.CoreCompat;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200000D RID: 13
	public abstract class DiskImageBuilder
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002AA2 File Offset: 0x00000CA2
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00002AAA File Offset: 0x00000CAA
		public Geometry BiosGeometry { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002AB3 File Offset: 0x00000CB3
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00002ABB File Offset: 0x00000CBB
		public SparseStream Content { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00002AC4 File Offset: 0x00000CC4
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00002ACC File Offset: 0x00000CCC
		public virtual GenericDiskAdapterType GenericAdapterType { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00002AD5 File Offset: 0x00000CD5
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00002ADD File Offset: 0x00000CDD
		public Geometry Geometry { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00002AE6 File Offset: 0x00000CE6
		public virtual bool PreservesBiosGeometry
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002AE9 File Offset: 0x00000CE9
		private static Dictionary<string, VirtualDiskFactory> TypeMap
		{
			get
			{
				if (DiskImageBuilder._typeMap == null)
				{
					DiskImageBuilder.InitializeMaps();
				}
				return DiskImageBuilder._typeMap;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002AFC File Offset: 0x00000CFC
		public static DiskImageBuilder GetBuilder(string type, string variant)
		{
			VirtualDiskFactory virtualDiskFactory;
			if (!DiskImageBuilder.TypeMap.TryGetValue(type, out virtualDiskFactory))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown disk type '{0}'", new object[]
				{
					type
				}), "type");
			}
			return virtualDiskFactory.GetImageBuilder(variant);
		}

		// Token: 0x060000A1 RID: 161
		public abstract DiskImageFileSpecification[] Build(string baseName);

		// Token: 0x060000A2 RID: 162 RVA: 0x00002B44 File Offset: 0x00000D44
		private static void InitializeMaps()
		{
			Dictionary<string, VirtualDiskFactory> dictionary = new Dictionary<string, VirtualDiskFactory>();
			foreach (Type type in ReflectionHelper.GetAssembly(typeof(VirtualDisk)).GetTypes())
			{
				VirtualDiskFactoryAttribute virtualDiskFactoryAttribute = (VirtualDiskFactoryAttribute)ReflectionHelper.GetCustomAttribute(type, typeof(VirtualDiskFactoryAttribute), false);
				if (virtualDiskFactoryAttribute != null)
				{
					VirtualDiskFactory value = (VirtualDiskFactory)Activator.CreateInstance(type);
					dictionary.Add(virtualDiskFactoryAttribute.Type, value);
				}
			}
			DiskImageBuilder._typeMap = dictionary;
		}

		// Token: 0x04000017 RID: 23
		private static Dictionary<string, VirtualDiskFactory> _typeMap;
	}
}
