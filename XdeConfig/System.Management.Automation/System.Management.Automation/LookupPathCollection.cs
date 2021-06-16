using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000056 RID: 86
	internal class LookupPathCollection : Collection<string>
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x000166FF File Offset: 0x000148FF
		internal LookupPathCollection()
		{
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00016708 File Offset: 0x00014908
		internal LookupPathCollection(IEnumerable<string> collection)
		{
			foreach (string item in collection)
			{
				this.Add(item);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00016758 File Offset: 0x00014958
		public new int Add(string item)
		{
			int result = -1;
			if (!this.Contains(item))
			{
				base.Add(item);
				result = base.IndexOf(item);
			}
			return result;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00016780 File Offset: 0x00014980
		internal void AddRange(ICollection<string> collection)
		{
			foreach (string item in collection)
			{
				this.Add(item);
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000167CC File Offset: 0x000149CC
		public new bool Contains(string item)
		{
			bool result = false;
			foreach (string b in this)
			{
				if (string.Equals(item, b, StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00016820 File Offset: 0x00014A20
		internal Collection<int> IndexOfRelativePath()
		{
			Collection<int> collection = new Collection<int>();
			for (int i = 0; i < base.Count; i++)
			{
				string text = base[i];
				if (!string.IsNullOrEmpty(text) && text.StartsWith(".", StringComparison.CurrentCulture))
				{
					collection.Add(i);
				}
			}
			return collection;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001686C File Offset: 0x00014A6C
		public new int IndexOf(string item)
		{
			if (string.IsNullOrEmpty(item))
			{
				throw PSTraceSource.NewArgumentException("item");
			}
			int result = -1;
			for (int i = 0; i < base.Count; i++)
			{
				if (string.Equals(base[i], item, StringComparison.OrdinalIgnoreCase))
				{
					result = i;
					break;
				}
			}
			return result;
		}
	}
}
