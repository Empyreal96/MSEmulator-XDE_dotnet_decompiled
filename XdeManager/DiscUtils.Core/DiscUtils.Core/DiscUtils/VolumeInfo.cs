using System;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x02000033 RID: 51
	public abstract class VolumeInfo : MarshalByRefObject
	{
		// Token: 0x06000209 RID: 521 RVA: 0x00004F5B File Offset: 0x0000315B
		internal VolumeInfo()
		{
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600020A RID: 522
		public abstract byte BiosType { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600020B RID: 523
		public abstract long Length { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600020C RID: 524
		public abstract string Identity { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600020D RID: 525
		public abstract Geometry PhysicalGeometry { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600020E RID: 526
		public abstract Geometry BiosGeometry { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600020F RID: 527
		public abstract long PhysicalStartSector { get; }

		// Token: 0x06000210 RID: 528
		public abstract SparseStream Open();
	}
}
