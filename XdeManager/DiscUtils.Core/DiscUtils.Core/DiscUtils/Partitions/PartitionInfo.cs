using System;
using System.Globalization;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000055 RID: 85
	public abstract class PartitionInfo
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000394 RID: 916
		public abstract byte BiosType { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000395 RID: 917
		public abstract long FirstSector { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000396 RID: 918
		public abstract Guid GuidType { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000397 RID: 919
		public abstract long LastSector { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00009C75 File Offset: 0x00007E75
		public virtual long SectorCount
		{
			get
			{
				return 1L + this.LastSector - this.FirstSector;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000399 RID: 921
		public abstract string TypeAsString { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600039A RID: 922
		internal abstract PhysicalVolumeType VolumeType { get; }

		// Token: 0x0600039B RID: 923
		public abstract SparseStream Open();

		// Token: 0x0600039C RID: 924 RVA: 0x00009C87 File Offset: 0x00007E87
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X} - 0x{1:X} ({2})", new object[]
			{
				this.FirstSector,
				this.LastSector,
				this.TypeAsString
			});
		}
	}
}
