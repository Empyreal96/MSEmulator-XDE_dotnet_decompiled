using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200003D RID: 61
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600043E RID: 1086
		object UnderlyingCollection { get; }
	}
}
