using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200000B RID: 11
	public sealed class DiskImageFileInfo
	{
		// Token: 0x06000087 RID: 135 RVA: 0x000044E8 File Offset: 0x000026E8
		internal DiskImageFileInfo(FileHeader fileHeader, VhdxHeader vhdxHeader1, VhdxHeader vhdxHeader2, RegionTable regions, Metadata metadata, LogSequence activeLogSequence)
		{
			this._fileHeader = fileHeader;
			this._vhdxHeader1 = vhdxHeader1;
			this._vhdxHeader2 = vhdxHeader2;
			this._regions = regions;
			this._metadata = metadata;
			this._activeLogSequence = activeLogSequence;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004520 File Offset: 0x00002720
		public HeaderInfo ActiveHeader
		{
			get
			{
				if (this._vhdxHeader1 == null)
				{
					if (this._vhdxHeader2 == null)
					{
						return null;
					}
					return new HeaderInfo(this._vhdxHeader2);
				}
				else
				{
					if (this._vhdxHeader2 == null)
					{
						return new HeaderInfo(this._vhdxHeader1);
					}
					return new HeaderInfo((this._vhdxHeader1.SequenceNumber > this._vhdxHeader2.SequenceNumber) ? this._vhdxHeader1 : this._vhdxHeader2);
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000458A File Offset: 0x0000278A
		public IEnumerable<LogEntryInfo> ActiveLogSequence
		{
			get
			{
				if (this._activeLogSequence != null)
				{
					foreach (LogEntry entry in this._activeLogSequence)
					{
						yield return new LogEntryInfo(entry);
					}
					List<LogEntry>.Enumerator enumerator = default(List<LogEntry>.Enumerator);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000459A File Offset: 0x0000279A
		public long BlockSize
		{
			get
			{
				return (long)((ulong)this._metadata.FileParameters.BlockSize);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000045AD File Offset: 0x000027AD
		public string Creator
		{
			get
			{
				return this._fileHeader.Creator;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000045BA File Offset: 0x000027BA
		public long DiskSize
		{
			get
			{
				return (long)this._metadata.DiskSize;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000045C7 File Offset: 0x000027C7
		public HeaderInfo FirstHeader
		{
			get
			{
				return new HeaderInfo(this._vhdxHeader1);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000045D4 File Offset: 0x000027D4
		public bool HasParent
		{
			get
			{
				return (this._metadata.FileParameters.Flags & FileParametersFlags.HasParent) > FileParametersFlags.None;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000045EB File Offset: 0x000027EB
		public bool LeaveBlocksAllocated
		{
			get
			{
				return (this._metadata.FileParameters.Flags & FileParametersFlags.LeaveBlocksAllocated) > FileParametersFlags.None;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004602 File Offset: 0x00002802
		public long LogicalSectorSize
		{
			get
			{
				return (long)((ulong)this._metadata.LogicalSectorSize);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004610 File Offset: 0x00002810
		public MetadataTableInfo MetadataTable
		{
			get
			{
				return new MetadataTableInfo(this._metadata.Table);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004622 File Offset: 0x00002822
		public IDictionary<string, string> ParentLocatorEntries
		{
			get
			{
				if (this._metadata.ParentLocator == null)
				{
					return new Dictionary<string, string>();
				}
				return this._metadata.ParentLocator.Entries;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004647 File Offset: 0x00002847
		public Guid ParentLocatorType
		{
			get
			{
				if (this._metadata.ParentLocator == null)
				{
					return Guid.Empty;
				}
				return this._metadata.ParentLocator.LocatorType;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000466C File Offset: 0x0000286C
		public long PhysicalSectorSize
		{
			get
			{
				return (long)((ulong)this._metadata.PhysicalSectorSize);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000467A File Offset: 0x0000287A
		public RegionTableInfo RegionTable
		{
			get
			{
				return new RegionTableInfo(this._regions);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004687 File Offset: 0x00002887
		public HeaderInfo SecondHeader
		{
			get
			{
				return new HeaderInfo(this._vhdxHeader2);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004694 File Offset: 0x00002894
		public string Signature
		{
			get
			{
				byte[] array = new byte[8];
				EndianUtilities.WriteBytesLittleEndian(this._fileHeader.Signature, array, 0);
				return EndianUtilities.BytesToString(array, 0, 8);
			}
		}

		// Token: 0x0400002A RID: 42
		private readonly LogSequence _activeLogSequence;

		// Token: 0x0400002B RID: 43
		private readonly FileHeader _fileHeader;

		// Token: 0x0400002C RID: 44
		private readonly Metadata _metadata;

		// Token: 0x0400002D RID: 45
		private readonly RegionTable _regions;

		// Token: 0x0400002E RID: 46
		private readonly VhdxHeader _vhdxHeader1;

		// Token: 0x0400002F RID: 47
		private readonly VhdxHeader _vhdxHeader2;
	}
}
