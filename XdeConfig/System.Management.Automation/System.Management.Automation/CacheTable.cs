using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000101 RID: 257
	internal class CacheTable
	{
		// Token: 0x06000E46 RID: 3654 RVA: 0x0004DCD8 File Offset: 0x0004BED8
		internal CacheTable()
		{
			this.memberCollection = new Collection<object>();
			this.indexes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0004DCFB File Offset: 0x0004BEFB
		internal void Add(string name, object member)
		{
			this.indexes[name] = this.memberCollection.Count;
			this.memberCollection.Add(member);
		}

		// Token: 0x170003C9 RID: 969
		internal object this[string name]
		{
			get
			{
				int index;
				if (!this.indexes.TryGetValue(name, out index))
				{
					return null;
				}
				return this.memberCollection[index];
			}
		}

		// Token: 0x0400064A RID: 1610
		internal Collection<object> memberCollection;

		// Token: 0x0400064B RID: 1611
		private Dictionary<string, int> indexes;
	}
}
