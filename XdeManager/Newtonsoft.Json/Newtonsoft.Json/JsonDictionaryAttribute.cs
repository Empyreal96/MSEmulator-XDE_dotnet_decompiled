using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000017 RID: 23
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonDictionaryAttribute : JsonContainerAttribute
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00002F78 File Offset: 0x00001178
		public JsonDictionaryAttribute()
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002F80 File Offset: 0x00001180
		public JsonDictionaryAttribute(string id) : base(id)
		{
		}
	}
}
