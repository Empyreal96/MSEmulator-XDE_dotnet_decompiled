using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000211 RID: 529
	public class PSListModifier<T> : PSListModifier
	{
		// Token: 0x060018CA RID: 6346 RVA: 0x00096F5E File Offset: 0x0009515E
		public PSListModifier()
		{
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00096F66 File Offset: 0x00095166
		public PSListModifier(Collection<object> removeItems, Collection<object> addItems) : base(removeItems, addItems)
		{
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00096F70 File Offset: 0x00095170
		public PSListModifier(object replacementItems) : base(replacementItems)
		{
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00096F79 File Offset: 0x00095179
		public PSListModifier(Hashtable hash) : base(hash)
		{
		}
	}
}
