using System;
using System.Collections.Generic;

namespace DiscUtils.Registry
{
	// Token: 0x02000007 RID: 7
	internal abstract class ListCell : Cell
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002D95 File Offset: 0x00000F95
		public ListCell(int index) : base(index)
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31
		internal abstract int Count { get; }

		// Token: 0x06000020 RID: 32
		internal abstract int FindKey(string name, out int cellIndex);

		// Token: 0x06000021 RID: 33
		internal abstract void EnumerateKeys(List<string> names);

		// Token: 0x06000022 RID: 34
		internal abstract IEnumerable<KeyNodeCell> EnumerateKeys();

		// Token: 0x06000023 RID: 35
		internal abstract int LinkSubKey(string name, int cellIndex);

		// Token: 0x06000024 RID: 36
		internal abstract int UnlinkSubKey(string name);
	}
}
