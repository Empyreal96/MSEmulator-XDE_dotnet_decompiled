using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DiscUtils.CoreCompat;
using DiscUtils.Raw;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000056 RID: 86
	public abstract class PartitionTable
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00009CCB File Offset: 0x00007ECB
		public int Count
		{
			get
			{
				return this.Partitions.Count;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600039F RID: 927
		public abstract Guid DiskGuid { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00009CD8 File Offset: 0x00007ED8
		private static List<PartitionTableFactory> Factories
		{
			get
			{
				if (PartitionTable._factories == null)
				{
					List<PartitionTableFactory> list = new List<PartitionTableFactory>();
					foreach (Type type in ReflectionHelper.GetAssembly(typeof(VolumeManager)).GetTypes())
					{
						foreach (Attribute attribute in ReflectionHelper.GetCustomAttributes(type, typeof(PartitionTableFactoryAttribute), false))
						{
							PartitionTableFactoryAttribute partitionTableFactoryAttribute = (PartitionTableFactoryAttribute)attribute;
							list.Add((PartitionTableFactory)Activator.CreateInstance(type));
						}
					}
					PartitionTable._factories = list;
				}
				return PartitionTable._factories;
			}
		}

		// Token: 0x1700010A RID: 266
		public PartitionInfo this[int index]
		{
			get
			{
				return this.Partitions[index];
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003A2 RID: 930
		public abstract ReadOnlyCollection<PartitionInfo> Partitions { get; }

		// Token: 0x060003A3 RID: 931 RVA: 0x00009D98 File Offset: 0x00007F98
		public static bool IsPartitioned(Stream content)
		{
			using (List<PartitionTableFactory>.Enumerator enumerator = PartitionTable.Factories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DetectIsPartitioned(content))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009DF4 File Offset: 0x00007FF4
		public static bool IsPartitioned(VirtualDisk disk)
		{
			return PartitionTable.IsPartitioned(disk.Content);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00009E04 File Offset: 0x00008004
		public static IList<PartitionTable> GetPartitionTables(VirtualDisk disk)
		{
			List<PartitionTable> list = new List<PartitionTable>();
			foreach (PartitionTableFactory partitionTableFactory in PartitionTable.Factories)
			{
				PartitionTable partitionTable = partitionTableFactory.DetectPartitionTable(disk);
				if (partitionTable != null)
				{
					list.Add(partitionTable);
				}
			}
			return list;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009E68 File Offset: 0x00008068
		public static IList<PartitionTable> GetPartitionTables(Stream contentStream)
		{
			return PartitionTable.GetPartitionTables(new Disk(contentStream, Ownership.None));
		}

		// Token: 0x060003A7 RID: 935
		public abstract int Create(WellKnownPartitionType type, bool active);

		// Token: 0x060003A8 RID: 936
		public abstract int Create(long size, WellKnownPartitionType type, bool active);

		// Token: 0x060003A9 RID: 937
		public abstract int CreateAligned(WellKnownPartitionType type, bool active, int alignment);

		// Token: 0x060003AA RID: 938
		public abstract int CreateAligned(long size, WellKnownPartitionType type, bool active, int alignment);

		// Token: 0x060003AB RID: 939
		public abstract void Delete(int index);

		// Token: 0x040000F0 RID: 240
		private static List<PartitionTableFactory> _factories;
	}
}
