using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000046 RID: 70
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600049D RID: 1181
		object UnderlyingDictionary { get; }
	}
}
