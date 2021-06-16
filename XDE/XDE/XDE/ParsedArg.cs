using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000023 RID: 35
	public class ParsedArg
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000088A9 File Offset: 0x00006AA9
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x000088B1 File Offset: 0x00006AB1
		public string Name { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000088BA File Offset: 0x00006ABA
		public ReadOnlyCollection<string> Values
		{
			get
			{
				return this.values.AsReadOnly();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000088C7 File Offset: 0x00006AC7
		public void AddValue(string value)
		{
			this.values.Add(value);
		}

		// Token: 0x040000BF RID: 191
		private List<string> values = new List<string>();
	}
}
