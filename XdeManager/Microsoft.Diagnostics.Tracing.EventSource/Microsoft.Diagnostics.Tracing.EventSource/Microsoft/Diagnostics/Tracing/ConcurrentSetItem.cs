using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200001A RID: 26
	internal abstract class ConcurrentSetItem<KeyType, ItemType> where ItemType : ConcurrentSetItem<KeyType, ItemType>
	{
		// Token: 0x06000104 RID: 260
		public abstract int Compare(ItemType other);

		// Token: 0x06000105 RID: 261
		public abstract int Compare(KeyType key);
	}
}
