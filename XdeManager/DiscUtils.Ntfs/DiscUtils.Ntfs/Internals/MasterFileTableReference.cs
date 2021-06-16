using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000064 RID: 100
	public struct MasterFileTableReference
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x000157E9 File Offset: 0x000139E9
		internal MasterFileTableReference(FileRecordReference recordRef)
		{
			this._ref = recordRef;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x000157F2 File Offset: 0x000139F2
		public long RecordIndex
		{
			get
			{
				return this._ref.MftIndex;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x000157FF File Offset: 0x000139FF
		public int RecordSequenceNumber
		{
			get
			{
				return (int)this._ref.SequenceNumber;
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001580C File Offset: 0x00013A0C
		public static bool operator ==(MasterFileTableReference a, MasterFileTableReference b)
		{
			return a._ref == b._ref;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001581F File Offset: 0x00013A1F
		public static bool operator !=(MasterFileTableReference a, MasterFileTableReference b)
		{
			return a._ref != b._ref;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015832 File Offset: 0x00013A32
		public override bool Equals(object obj)
		{
			return obj != null && obj is MasterFileTableReference && this._ref == ((MasterFileTableReference)obj)._ref;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015857 File Offset: 0x00013A57
		public override int GetHashCode()
		{
			return this._ref.GetHashCode();
		}

		// Token: 0x040001D4 RID: 468
		private FileRecordReference _ref;
	}
}
