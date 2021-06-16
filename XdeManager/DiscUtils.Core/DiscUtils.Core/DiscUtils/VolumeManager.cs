using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using DiscUtils.CoreCompat;
using DiscUtils.Internal;
using DiscUtils.Partitions;
using DiscUtils.Raw;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000034 RID: 52
	public sealed class VolumeManager : MarshalByRefObject
	{
		// Token: 0x06000211 RID: 529 RVA: 0x00004F63 File Offset: 0x00003163
		public VolumeManager()
		{
			this._disks = new List<VirtualDisk>();
			this._physicalVolumes = new Dictionary<string, PhysicalVolumeInfo>();
			this._logicalVolumes = new Dictionary<string, LogicalVolumeInfo>();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00004F8C File Offset: 0x0000318C
		public VolumeManager(VirtualDisk initialDisk) : this()
		{
			this.AddDisk(initialDisk);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00004F9C File Offset: 0x0000319C
		public VolumeManager(Stream initialDiskContent) : this()
		{
			this.AddDisk(initialDiskContent);
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00004FAC File Offset: 0x000031AC
		private static List<LogicalVolumeFactory> LogicalVolumeFactories
		{
			get
			{
				if (VolumeManager.s_logicalVolumeFactories == null)
				{
					List<LogicalVolumeFactory> list = new List<LogicalVolumeFactory>();
					list.AddRange(VolumeManager.GetLogicalVolumeFactories(VolumeManager._coreAssembly));
					VolumeManager.s_logicalVolumeFactories = list;
				}
				return VolumeManager.s_logicalVolumeFactories;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00004FD4 File Offset: 0x000031D4
		private static IEnumerable<LogicalVolumeFactory> GetLogicalVolumeFactories(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				foreach (Attribute attribute in ReflectionHelper.GetCustomAttributes(type, typeof(LogicalVolumeFactoryAttribute), false))
				{
					LogicalVolumeFactoryAttribute logicalVolumeFactoryAttribute = (LogicalVolumeFactoryAttribute)attribute;
					yield return (LogicalVolumeFactory)Activator.CreateInstance(type);
				}
				IEnumerator<Attribute> enumerator = null;
				type = null;
			}
			Type[] array = null;
			yield break;
			yield break;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00004FE4 File Offset: 0x000031E4
		public static void RegisterLogicalVolumeFactory(Assembly assembly)
		{
			if (assembly == VolumeManager._coreAssembly)
			{
				return;
			}
			VolumeManager.LogicalVolumeFactories.AddRange(VolumeManager.GetLogicalVolumeFactories(assembly));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00005004 File Offset: 0x00003204
		public static PhysicalVolumeInfo[] GetPhysicalVolumes(Stream diskContent)
		{
			return VolumeManager.GetPhysicalVolumes(new Disk(diskContent, Ownership.None));
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00005012 File Offset: 0x00003212
		public static PhysicalVolumeInfo[] GetPhysicalVolumes(VirtualDisk disk)
		{
			return new VolumeManager(disk).GetPhysicalVolumes();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00005020 File Offset: 0x00003220
		public string AddDisk(VirtualDisk disk)
		{
			this._needScan = true;
			int count = this._disks.Count;
			this._disks.Add(disk);
			return this.GetDiskId(count);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00005053 File Offset: 0x00003253
		public string AddDisk(Stream content)
		{
			return this.AddDisk(new Disk(content, Ownership.None));
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00005062 File Offset: 0x00003262
		public PhysicalVolumeInfo[] GetPhysicalVolumes()
		{
			if (this._needScan)
			{
				this.Scan();
			}
			return new List<PhysicalVolumeInfo>(this._physicalVolumes.Values).ToArray();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00005087 File Offset: 0x00003287
		public LogicalVolumeInfo[] GetLogicalVolumes()
		{
			if (this._needScan)
			{
				this.Scan();
			}
			return new List<LogicalVolumeInfo>(this._logicalVolumes.Values).ToArray();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000050AC File Offset: 0x000032AC
		public VolumeInfo GetVolume(string identity)
		{
			if (this._needScan)
			{
				this.Scan();
			}
			PhysicalVolumeInfo result;
			if (this._physicalVolumes.TryGetValue(identity, out result))
			{
				return result;
			}
			LogicalVolumeInfo result2;
			if (this._logicalVolumes.TryGetValue(identity, out result2))
			{
				return result2;
			}
			return null;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000050EC File Offset: 0x000032EC
		private static void MapPhysicalVolumes(IEnumerable<PhysicalVolumeInfo> physicalVols, Dictionary<string, LogicalVolumeInfo> result)
		{
			foreach (PhysicalVolumeInfo physicalVolumeInfo in physicalVols)
			{
				LogicalVolumeInfo logicalVolumeInfo = new LogicalVolumeInfo(physicalVolumeInfo.PartitionIdentity, physicalVolumeInfo, new SparseStreamOpenDelegate(physicalVolumeInfo.Open), physicalVolumeInfo.Length, physicalVolumeInfo.BiosType, LogicalVolumeStatus.Healthy);
				result.Add(logicalVolumeInfo.Identity, logicalVolumeInfo);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00005164 File Offset: 0x00003364
		private void Scan()
		{
			Dictionary<string, PhysicalVolumeInfo> dictionary = this.ScanForPhysicalVolumes();
			Dictionary<string, LogicalVolumeInfo> logicalVolumes = this.ScanForLogicalVolumes(dictionary.Values);
			this._physicalVolumes = dictionary;
			this._logicalVolumes = logicalVolumes;
			this._needScan = false;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000519C File Offset: 0x0000339C
		private Dictionary<string, LogicalVolumeInfo> ScanForLogicalVolumes(IEnumerable<PhysicalVolumeInfo> physicalVols)
		{
			List<PhysicalVolumeInfo> list = new List<PhysicalVolumeInfo>();
			Dictionary<string, LogicalVolumeInfo> result = new Dictionary<string, LogicalVolumeInfo>();
			foreach (PhysicalVolumeInfo physicalVolumeInfo in physicalVols)
			{
				bool flag = false;
				using (List<LogicalVolumeFactory>.Enumerator enumerator2 = VolumeManager.LogicalVolumeFactories.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.HandlesPhysicalVolume(physicalVolumeInfo))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					list.Add(physicalVolumeInfo);
				}
			}
			VolumeManager.MapPhysicalVolumes(list, result);
			foreach (LogicalVolumeFactory logicalVolumeFactory in VolumeManager.LogicalVolumeFactories)
			{
				logicalVolumeFactory.MapDisks(this._disks, result);
			}
			return result;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000528C File Offset: 0x0000348C
		private Dictionary<string, PhysicalVolumeInfo> ScanForPhysicalVolumes()
		{
			Dictionary<string, PhysicalVolumeInfo> dictionary = new Dictionary<string, PhysicalVolumeInfo>();
			int i = 0;
			while (i < this._disks.Count)
			{
				VirtualDisk virtualDisk = this._disks[i];
				string diskId = this.GetDiskId(i);
				if (PartitionTable.IsPartitioned(virtualDisk.Content))
				{
					using (IEnumerator<PartitionTable> enumerator = PartitionTable.GetPartitionTables(virtualDisk).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PartitionTable partitionTable = enumerator.Current;
							foreach (PartitionInfo partitionInfo in partitionTable.Partitions)
							{
								PhysicalVolumeInfo physicalVolumeInfo = new PhysicalVolumeInfo(diskId, virtualDisk, partitionInfo);
								dictionary.Add(physicalVolumeInfo.Identity, physicalVolumeInfo);
							}
						}
						goto IL_BC;
					}
					goto IL_A4;
				}
				goto IL_A4;
				IL_BC:
				i++;
				continue;
				IL_A4:
				PhysicalVolumeInfo physicalVolumeInfo2 = new PhysicalVolumeInfo(diskId, virtualDisk);
				dictionary.Add(physicalVolumeInfo2.Identity, physicalVolumeInfo2);
				goto IL_BC;
			}
			return dictionary;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00005388 File Offset: 0x00003588
		private string GetDiskId(int ordinal)
		{
			VirtualDisk virtualDisk = this._disks[ordinal];
			if (virtualDisk.IsPartitioned)
			{
				Guid diskGuid = virtualDisk.Partitions.DiskGuid;
				if (diskGuid != Guid.Empty)
				{
					return "DG" + diskGuid.ToString("B");
				}
			}
			int signature = virtualDisk.Signature;
			if (signature != 0)
			{
				return "DS" + signature.ToString("X8", CultureInfo.InvariantCulture);
			}
			return "DO" + ordinal;
		}

		// Token: 0x04000088 RID: 136
		private static List<LogicalVolumeFactory> s_logicalVolumeFactories;

		// Token: 0x04000089 RID: 137
		private readonly List<VirtualDisk> _disks;

		// Token: 0x0400008A RID: 138
		private bool _needScan;

		// Token: 0x0400008B RID: 139
		private Dictionary<string, PhysicalVolumeInfo> _physicalVolumes;

		// Token: 0x0400008C RID: 140
		private Dictionary<string, LogicalVolumeInfo> _logicalVolumes;

		// Token: 0x0400008D RID: 141
		private static readonly Assembly _coreAssembly = ReflectionHelper.GetAssembly(typeof(VolumeManager));
	}
}
