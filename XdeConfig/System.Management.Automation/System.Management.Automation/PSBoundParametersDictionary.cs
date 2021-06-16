using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000077 RID: 119
	internal sealed class PSBoundParametersDictionary : Dictionary<string, object>
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x0001F02F File Offset: 0x0001D22F
		internal PSBoundParametersDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{
			this.BoundPositionally = new List<string>();
			this.ImplicitUsingParameters = PSBoundParametersDictionary.EmptyUsingParameters;
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0001F052 File Offset: 0x0001D252
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0001F05A File Offset: 0x0001D25A
		public List<string> BoundPositionally { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001F063 File Offset: 0x0001D263
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0001F06B File Offset: 0x0001D26B
		internal IDictionary ImplicitUsingParameters { get; set; }

		// Token: 0x04000290 RID: 656
		private static readonly IDictionary EmptyUsingParameters = new ReadOnlyDictionary<object, object>(new Dictionary<object, object>());
	}
}
