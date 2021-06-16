using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200003B RID: 59
	internal sealed class NameInfo : ConcurrentSetItem<KeyValuePair<string, EventTags>, NameInfo>
	{
		// Token: 0x060001ED RID: 493 RVA: 0x0000DA10 File Offset: 0x0000BC10
		internal static void ReserveEventIDsBelow(int eventId)
		{
			int num;
			int num2;
			do
			{
				num = NameInfo.lastIdentity;
				num2 = checked((NameInfo.lastIdentity & -16777216) + eventId);
				num2 = Math.Max(num2, num);
			}
			while (Interlocked.CompareExchange(ref NameInfo.lastIdentity, num2, num) != num);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000DA48 File Offset: 0x0000BC48
		public NameInfo(string name, EventTags tags, int typeMetadataSize)
		{
			this.name = name;
			this.tags = (tags & (EventTags)268435455);
			this.identity = Interlocked.Increment(ref NameInfo.lastIdentity);
			int prefixSize = 0;
			Statics.EncodeTags((int)this.tags, ref prefixSize, null);
			this.nameMetadata = Statics.MetadataForString(name, prefixSize, 0, typeMetadataSize);
			prefixSize = 2;
			Statics.EncodeTags((int)this.tags, ref prefixSize, this.nameMetadata);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000DAB3 File Offset: 0x0000BCB3
		public override int Compare(NameInfo other)
		{
			return this.Compare(other.name, other.tags);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
		public override int Compare(KeyValuePair<string, EventTags> key)
		{
			return this.Compare(key.Key, key.Value & (EventTags)268435455);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000DAE4 File Offset: 0x0000BCE4
		private int Compare(string otherName, EventTags otherTags)
		{
			int num = StringComparer.Ordinal.Compare(this.name, otherName);
			if (num == 0 && this.tags != otherTags)
			{
				num = ((this.tags < otherTags) ? -1 : 1);
			}
			return num;
		}

		// Token: 0x04000127 RID: 295
		private static int lastIdentity = 184549376;

		// Token: 0x04000128 RID: 296
		internal readonly string name;

		// Token: 0x04000129 RID: 297
		internal readonly EventTags tags;

		// Token: 0x0400012A RID: 298
		internal readonly int identity;

		// Token: 0x0400012B RID: 299
		internal readonly byte[] nameMetadata;
	}
}
