using System;
using System.Collections.Generic;
using System.Reflection;
using DiscUtils.CoreCompat;
using DiscUtils.Internal;

namespace DiscUtils
{
	// Token: 0x02000030 RID: 48
	public static class VirtualDiskManager
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00004D62 File Offset: 0x00002F62
		static VirtualDiskManager()
		{
			VirtualDiskManager.DiskTransports = new Dictionary<string, Type>();
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00004D82 File Offset: 0x00002F82
		internal static Dictionary<string, Type> DiskTransports { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00004D89 File Offset: 0x00002F89
		internal static Dictionary<string, VirtualDiskFactory> ExtensionMap { get; } = new Dictionary<string, VirtualDiskFactory>();

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00004D90 File Offset: 0x00002F90
		public static ICollection<string> SupportedDiskFormats
		{
			get
			{
				return VirtualDiskManager.ExtensionMap.Keys;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00004D9C File Offset: 0x00002F9C
		public static ICollection<string> SupportedDiskTypes
		{
			get
			{
				return VirtualDiskManager.TypeMap.Keys;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00004DA8 File Offset: 0x00002FA8
		internal static Dictionary<string, VirtualDiskFactory> TypeMap { get; } = new Dictionary<string, VirtualDiskFactory>();

		// Token: 0x060001EF RID: 495 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public static void RegisterVirtualDiskTypes(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				VirtualDiskFactoryAttribute virtualDiskFactoryAttribute = (VirtualDiskFactoryAttribute)ReflectionHelper.GetCustomAttribute(type, typeof(VirtualDiskFactoryAttribute), false);
				if (virtualDiskFactoryAttribute != null)
				{
					VirtualDiskFactory value = (VirtualDiskFactory)Activator.CreateInstance(type);
					VirtualDiskManager.TypeMap.Add(virtualDiskFactoryAttribute.Type, value);
					foreach (string text in virtualDiskFactoryAttribute.FileExtensions)
					{
						VirtualDiskManager.ExtensionMap.Add(text.ToUpperInvariant(), value);
					}
				}
				VirtualDiskTransportAttribute virtualDiskTransportAttribute = ReflectionHelper.GetCustomAttribute(type, typeof(VirtualDiskTransportAttribute), false) as VirtualDiskTransportAttribute;
				if (virtualDiskTransportAttribute != null)
				{
					VirtualDiskManager.DiskTransports.Add(virtualDiskTransportAttribute.Scheme.ToUpperInvariant(), type);
				}
			}
		}
	}
}
