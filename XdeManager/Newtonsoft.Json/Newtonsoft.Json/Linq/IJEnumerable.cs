using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B0 RID: 176
	public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x170001B6 RID: 438
		IJEnumerable<JToken> this[object key]
		{
			get;
		}
	}
}
