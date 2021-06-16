using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005B RID: 91
	internal class Database
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x00009F78 File Offset: 0x00008178
		public Database(Stream stream)
		{
			long position = stream.Position;
			byte[] array = new byte[512];
			stream.Read(array, 0, array.Length);
			this._vmdb = new DatabaseHeader();
			this._vmdb.ReadFrom(array, 0);
			stream.Position = position + (long)((ulong)this._vmdb.HeaderSize);
			array = StreamUtilities.ReadExact(stream, (int)(this._vmdb.BlockSize * this._vmdb.NumVBlks));
			this._records = new Dictionary<ulong, DatabaseRecord>();
			int num = 0;
			while ((long)num < (long)((ulong)this._vmdb.NumVBlks))
			{
				DatabaseRecord databaseRecord = DatabaseRecord.ReadFrom(array, (int)((long)num * (long)((ulong)this._vmdb.BlockSize)));
				if (databaseRecord != null)
				{
					this._records.Add(databaseRecord.Id, databaseRecord);
				}
				num++;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000A042 File Offset: 0x00008242
		internal IEnumerable<DiskRecord> Disks
		{
			get
			{
				foreach (DatabaseRecord databaseRecord in this._records.Values)
				{
					if (databaseRecord.RecordType == RecordType.Disk)
					{
						yield return (DiskRecord)databaseRecord;
					}
				}
				Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator enumerator = default(Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000A052 File Offset: 0x00008252
		internal IEnumerable<VolumeRecord> Volumes
		{
			get
			{
				foreach (DatabaseRecord databaseRecord in this._records.Values)
				{
					if (databaseRecord.RecordType == RecordType.Volume)
					{
						yield return (VolumeRecord)databaseRecord;
					}
				}
				Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator enumerator = default(Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000A064 File Offset: 0x00008264
		internal DiskGroupRecord GetDiskGroup(Guid guid)
		{
			foreach (DatabaseRecord databaseRecord in this._records.Values)
			{
				if (databaseRecord.RecordType == RecordType.DiskGroup)
				{
					DiskGroupRecord diskGroupRecord = (DiskGroupRecord)databaseRecord;
					if (new Guid(diskGroupRecord.GroupGuidString) == guid || guid == Guid.Empty)
					{
						return diskGroupRecord;
					}
				}
			}
			return null;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000A0EC File Offset: 0x000082EC
		internal IEnumerable<ComponentRecord> GetVolumeComponents(ulong volumeId)
		{
			foreach (DatabaseRecord databaseRecord in this._records.Values)
			{
				if (databaseRecord.RecordType == RecordType.Component)
				{
					ComponentRecord componentRecord = (ComponentRecord)databaseRecord;
					if (componentRecord.VolumeId == volumeId)
					{
						yield return componentRecord;
					}
				}
			}
			Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator enumerator = default(Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000A103 File Offset: 0x00008303
		internal IEnumerable<ExtentRecord> GetComponentExtents(ulong componentId)
		{
			foreach (DatabaseRecord databaseRecord in this._records.Values)
			{
				if (databaseRecord.RecordType == RecordType.Extent)
				{
					ExtentRecord extentRecord = (ExtentRecord)databaseRecord;
					if (extentRecord.ComponentId == componentId)
					{
						yield return extentRecord;
					}
				}
			}
			Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator enumerator = default(Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000A11A File Offset: 0x0000831A
		internal DiskRecord GetDisk(ulong diskId)
		{
			return (DiskRecord)this._records[diskId];
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000A12D File Offset: 0x0000832D
		internal VolumeRecord GetVolume(ulong volumeId)
		{
			return (VolumeRecord)this._records[volumeId];
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000A140 File Offset: 0x00008340
		internal VolumeRecord GetVolume(Guid id)
		{
			return this.FindRecord<VolumeRecord>((VolumeRecord r) => r.VolumeGuid == id, RecordType.Volume);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000A16D File Offset: 0x0000836D
		internal IEnumerable<VolumeRecord> GetVolumes()
		{
			foreach (DatabaseRecord databaseRecord in this._records.Values)
			{
				if (databaseRecord.RecordType == RecordType.Volume)
				{
					yield return (VolumeRecord)databaseRecord;
				}
			}
			Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator enumerator = default(Dictionary<ulong, DatabaseRecord>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000A180 File Offset: 0x00008380
		internal T FindRecord<T>(Predicate<T> pred, RecordType typeId) where T : DatabaseRecord
		{
			foreach (DatabaseRecord databaseRecord in this._records.Values)
			{
				if (databaseRecord.RecordType == typeId)
				{
					T t = (T)((object)databaseRecord);
					if (pred(t))
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x04000102 RID: 258
		private readonly Dictionary<ulong, DatabaseRecord> _records;

		// Token: 0x04000103 RID: 259
		private readonly DatabaseHeader _vmdb;
	}
}
