using System;
using System.Collections.ObjectModel;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A1 RID: 2465
	internal sealed class MethodParametersCollection : KeyedCollection<string, MethodParameter>
	{
		// Token: 0x06005AD4 RID: 23252 RVA: 0x001E8AAE File Offset: 0x001E6CAE
		public MethodParametersCollection() : base(StringComparer.Ordinal, 5)
		{
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x001E8ABC File Offset: 0x001E6CBC
		protected override string GetKeyForItem(MethodParameter item)
		{
			return item.Name;
		}
	}
}
