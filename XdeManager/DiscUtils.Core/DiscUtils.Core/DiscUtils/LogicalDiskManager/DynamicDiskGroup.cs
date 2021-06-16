using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000061 RID: 97
	internal class DynamicDiskGroup : IDiagnosticTraceable
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0000AC0C File Offset: 0x00008E0C
		internal DynamicDiskGroup(VirtualDisk disk)
		{
			this._disks = new Dictionary<Guid, DynamicDisk>();
			DynamicDisk dynamicDisk = new DynamicDisk(disk);
			this._database = dynamicDisk.Database;
			this._disks.Add(dynamicDisk.Id, dynamicDisk);
			this._record = dynamicDisk.Database.GetDiskGroup(dynamicDisk.GroupId);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000AC68 File Offset: 0x00008E68
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "DISK GROUP (" + this._record.Name + ")");
			writer.WriteLine(linePrefix + "  Name: " + this._record.Name);
			writer.WriteLine(linePrefix + "  Flags: 0x" + (this._record.Flags & 65520U).ToString("X4", CultureInfo.InvariantCulture));
			writer.WriteLine(linePrefix + "  Database Id: " + this._record.Id);
			writer.WriteLine(linePrefix + "  Guid: " + this._record.GroupGuidString);
			writer.WriteLine();
			writer.WriteLine(linePrefix + "  DISKS");
			foreach (DiskRecord diskRecord in this._database.Disks)
			{
				writer.WriteLine(linePrefix + "    DISK (" + diskRecord.Name + ")");
				writer.WriteLine(linePrefix + "      Name: " + diskRecord.Name);
				writer.WriteLine(linePrefix + "      Flags: 0x" + (diskRecord.Flags & 65520U).ToString("X4", CultureInfo.InvariantCulture));
				writer.WriteLine(linePrefix + "      Database Id: " + diskRecord.Id);
				writer.WriteLine(linePrefix + "      Guid: " + diskRecord.DiskGuidString);
				DynamicDisk dynamicDisk;
				if (this._disks.TryGetValue(new Guid(diskRecord.DiskGuidString), out dynamicDisk))
				{
					writer.WriteLine(linePrefix + "      PRIVATE HEADER");
					dynamicDisk.Dump(writer, linePrefix + "        ");
				}
			}
			writer.WriteLine(linePrefix + "  VOLUMES");
			foreach (VolumeRecord volumeRecord in this._database.Volumes)
			{
				writer.WriteLine(linePrefix + "    VOLUME (" + volumeRecord.Name + ")");
				writer.WriteLine(linePrefix + "      Name: " + volumeRecord.Name);
				writer.WriteLine(string.Concat(new string[]
				{
					linePrefix,
					"      BIOS Type: ",
					volumeRecord.BiosType.ToString("X2", CultureInfo.InvariantCulture),
					" [",
					BiosPartitionTypes.ToString(volumeRecord.BiosType),
					"]"
				}));
				writer.WriteLine(linePrefix + "      Flags: 0x" + (volumeRecord.Flags & 65520U).ToString("X4", CultureInfo.InvariantCulture));
				writer.WriteLine(linePrefix + "      Database Id: " + volumeRecord.Id);
				writer.WriteLine(linePrefix + "      Guid: " + volumeRecord.VolumeGuid);
				writer.WriteLine(linePrefix + "      State: " + volumeRecord.ActiveString);
				writer.WriteLine(linePrefix + "      Drive Hint: " + volumeRecord.MountHint);
				writer.WriteLine(linePrefix + "      Num Components: " + volumeRecord.ComponentCount);
				writer.WriteLine(linePrefix + "      Link Id: " + volumeRecord.PartitionComponentLink);
				writer.WriteLine(linePrefix + "      COMPONENTS");
				foreach (ComponentRecord componentRecord in this._database.GetVolumeComponents(volumeRecord.Id))
				{
					writer.WriteLine(linePrefix + "        COMPONENT (" + componentRecord.Name + ")");
					writer.WriteLine(linePrefix + "          Name: " + componentRecord.Name);
					writer.WriteLine(linePrefix + "          Flags: 0x" + (componentRecord.Flags & 65520U).ToString("X4", CultureInfo.InvariantCulture));
					writer.WriteLine(linePrefix + "          Database Id: " + componentRecord.Id);
					writer.WriteLine(linePrefix + "          State: " + componentRecord.StatusString);
					writer.WriteLine(linePrefix + "          Mode: " + componentRecord.MergeType);
					writer.WriteLine(linePrefix + "          Num Extents: " + componentRecord.NumExtents);
					writer.WriteLine(linePrefix + "          Link Id: " + componentRecord.LinkId);
					writer.WriteLine(string.Concat(new object[]
					{
						linePrefix,
						"          Stripe Size: ",
						componentRecord.StripeSizeSectors,
						" (Sectors)"
					}));
					writer.WriteLine(linePrefix + "          Stripe Stride: " + componentRecord.StripeStride);
					writer.WriteLine(linePrefix + "          EXTENTS");
					foreach (ExtentRecord extentRecord in this._database.GetComponentExtents(componentRecord.Id))
					{
						writer.WriteLine(linePrefix + "            EXTENT (" + extentRecord.Name + ")");
						writer.WriteLine(linePrefix + "              Name: " + extentRecord.Name);
						writer.WriteLine(linePrefix + "              Flags: 0x" + (extentRecord.Flags & 65520U).ToString("X4", CultureInfo.InvariantCulture));
						writer.WriteLine(linePrefix + "              Database Id: " + extentRecord.Id);
						writer.WriteLine(string.Concat(new object[]
						{
							linePrefix,
							"              Disk Offset: ",
							extentRecord.DiskOffsetLba,
							" (Sectors)"
						}));
						writer.WriteLine(string.Concat(new object[]
						{
							linePrefix,
							"              Volume Offset: ",
							extentRecord.OffsetInVolumeLba,
							" (Sectors)"
						}));
						writer.WriteLine(string.Concat(new object[]
						{
							linePrefix,
							"              Size: ",
							extentRecord.SizeLba,
							" (Sectors)"
						}));
						writer.WriteLine(linePrefix + "              Component Id: " + extentRecord.ComponentId);
						writer.WriteLine(linePrefix + "              Disk Id: " + extentRecord.DiskId);
						writer.WriteLine(linePrefix + "              Link Id: " + extentRecord.PartitionComponentLink);
						writer.WriteLine(linePrefix + "              Interleave Order: " + extentRecord.InterleaveOrder);
					}
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public void Add(VirtualDisk disk)
		{
			DynamicDisk dynamicDisk = new DynamicDisk(disk);
			this._disks.Add(dynamicDisk.Id, dynamicDisk);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000B3F0 File Offset: 0x000095F0
		internal DynamicVolume[] GetVolumes()
		{
			List<DynamicVolume> list = new List<DynamicVolume>();
			foreach (VolumeRecord volumeRecord in this._database.GetVolumes())
			{
				list.Add(new DynamicVolume(this, volumeRecord.VolumeGuid));
			}
			return list.ToArray();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000B45C File Offset: 0x0000965C
		internal VolumeRecord GetVolume(Guid volume)
		{
			return this._database.GetVolume(volume);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000B46A File Offset: 0x0000966A
		internal LogicalVolumeStatus GetVolumeStatus(ulong volumeId)
		{
			return this.GetVolumeStatus(this._database.GetVolume(volumeId));
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000B47E File Offset: 0x0000967E
		internal SparseStream OpenVolume(ulong volumeId)
		{
			return this.OpenVolume(this._database.GetVolume(volumeId));
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000B492 File Offset: 0x00009692
		private static int CompareExtentOffsets(ExtentRecord x, ExtentRecord y)
		{
			if (x.OffsetInVolumeLba > y.OffsetInVolumeLba)
			{
				return 1;
			}
			if (x.OffsetInVolumeLba < y.OffsetInVolumeLba)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000B4B5 File Offset: 0x000096B5
		private static int CompareExtentInterleaveOrder(ExtentRecord x, ExtentRecord y)
		{
			if (x.InterleaveOrder > y.InterleaveOrder)
			{
				return 1;
			}
			if (x.InterleaveOrder < y.InterleaveOrder)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000B4D8 File Offset: 0x000096D8
		private static LogicalVolumeStatus WorstOf(LogicalVolumeStatus x, LogicalVolumeStatus y)
		{
			return (LogicalVolumeStatus)Math.Max((int)x, (int)y);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000B4E4 File Offset: 0x000096E4
		private LogicalVolumeStatus GetVolumeStatus(VolumeRecord volume)
		{
			int num = 0;
			ulong num2 = 0UL;
			LogicalVolumeStatus logicalVolumeStatus = LogicalVolumeStatus.Healthy;
			foreach (ComponentRecord cmpnt in this._database.GetVolumeComponents(volume.Id))
			{
				LogicalVolumeStatus componentStatus = this.GetComponentStatus(cmpnt);
				logicalVolumeStatus = DynamicDiskGroup.WorstOf(logicalVolumeStatus, componentStatus);
				if (componentStatus == LogicalVolumeStatus.Failed)
				{
					num++;
				}
				else
				{
					num2 += 1UL;
				}
			}
			if (num2 < 1UL)
			{
				return LogicalVolumeStatus.Failed;
			}
			if (num2 == volume.ComponentCount)
			{
				return logicalVolumeStatus;
			}
			return LogicalVolumeStatus.FailedRedundancy;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000B574 File Offset: 0x00009774
		private LogicalVolumeStatus GetComponentStatus(ComponentRecord cmpnt)
		{
			LogicalVolumeStatus result = LogicalVolumeStatus.Healthy;
			foreach (ExtentRecord extentRecord in this._database.GetComponentExtents(cmpnt.Id))
			{
				DiskRecord disk = this._database.GetDisk(extentRecord.DiskId);
				if (!this._disks.ContainsKey(new Guid(disk.DiskGuidString)))
				{
					result = LogicalVolumeStatus.Failed;
					break;
				}
			}
			return result;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000B5F8 File Offset: 0x000097F8
		private SparseStream OpenExtent(ExtentRecord extent)
		{
			DiskRecord disk = this._database.GetDisk(extent.DiskId);
			DynamicDisk dynamicDisk = this._disks[new Guid(disk.DiskGuidString)];
			return new SubStream(dynamicDisk.Content, Ownership.None, (dynamicDisk.DataOffset + extent.DiskOffsetLba) * 512L, extent.SizeLba * 512L);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000B65C File Offset: 0x0000985C
		private SparseStream OpenComponent(ComponentRecord component)
		{
			if (component.MergeType == ExtentMergeType.Concatenated)
			{
				List<ExtentRecord> list = new List<ExtentRecord>(this._database.GetComponentExtents(component.Id));
				list.Sort(new Comparison<ExtentRecord>(DynamicDiskGroup.CompareExtentOffsets));
				long num = 0L;
				foreach (ExtentRecord extentRecord in list)
				{
					if (extentRecord.OffsetInVolumeLba != num)
					{
						throw new IOException("Volume extents are non-contiguous");
					}
					num += extentRecord.SizeLba;
				}
				List<SparseStream> list2 = new List<SparseStream>();
				foreach (ExtentRecord extent in list)
				{
					list2.Add(this.OpenExtent(extent));
				}
				return new ConcatStream(Ownership.Dispose, list2.ToArray());
			}
			if (component.MergeType == ExtentMergeType.Interleaved)
			{
				List<ExtentRecord> list3 = new List<ExtentRecord>(this._database.GetComponentExtents(component.Id));
				list3.Sort(new Comparison<ExtentRecord>(DynamicDiskGroup.CompareExtentInterleaveOrder));
				List<SparseStream> list4 = new List<SparseStream>();
				foreach (ExtentRecord extent2 in list3)
				{
					list4.Add(this.OpenExtent(extent2));
				}
				return new StripedStream(component.StripeSizeSectors * 512L, Ownership.Dispose, list4.ToArray());
			}
			throw new NotImplementedException("Unknown component mode: " + component.MergeType);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000B808 File Offset: 0x00009A08
		private SparseStream OpenVolume(VolumeRecord volume)
		{
			List<SparseStream> list = new List<SparseStream>();
			foreach (ComponentRecord componentRecord in this._database.GetVolumeComponents(volume.Id))
			{
				if (this.GetComponentStatus(componentRecord) == LogicalVolumeStatus.Healthy)
				{
					list.Add(this.OpenComponent(componentRecord));
				}
			}
			if (list.Count < 1)
			{
				throw new IOException("Volume with no associated or healthy components");
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			return new MirrorStream(Ownership.Dispose, list.ToArray());
		}

		// Token: 0x0400012A RID: 298
		private readonly Database _database;

		// Token: 0x0400012B RID: 299
		private readonly Dictionary<Guid, DynamicDisk> _disks;

		// Token: 0x0400012C RID: 300
		private readonly DiskGroupRecord _record;
	}
}
