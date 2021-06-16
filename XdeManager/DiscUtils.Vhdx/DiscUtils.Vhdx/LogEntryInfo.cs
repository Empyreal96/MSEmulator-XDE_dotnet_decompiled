using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000014 RID: 20
	public sealed class LogEntryInfo
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00004DF2 File Offset: 0x00002FF2
		internal LogEntryInfo(LogEntry entry)
		{
			this._entry = entry;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004E01 File Offset: 0x00003001
		public long FlushedFileOffset
		{
			get
			{
				return (long)this._entry.FlushedFileOffset;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004E0E File Offset: 0x0000300E
		public bool IsEmpty
		{
			get
			{
				return this._entry.IsEmpty;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004E1B File Offset: 0x0000301B
		public long LastFileOffset
		{
			get
			{
				return (long)this._entry.LastFileOffset;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004E28 File Offset: 0x00003028
		public IEnumerable<Range<long, long>> ModifiedExtents
		{
			get
			{
				foreach (Range<ulong, ulong> range in this._entry.ModifiedExtents)
				{
					yield return new Range<long, long>((long)range.Offset, (long)range.Count);
				}
				IEnumerator<Range<ulong, ulong>> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004E38 File Offset: 0x00003038
		public long SequenceNumber
		{
			get
			{
				return (long)this._entry.SequenceNumber;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004E45 File Offset: 0x00003045
		public int Tail
		{
			get
			{
				return (int)this._entry.Tail;
			}
		}

		// Token: 0x04000054 RID: 84
		private readonly LogEntry _entry;
	}
}
