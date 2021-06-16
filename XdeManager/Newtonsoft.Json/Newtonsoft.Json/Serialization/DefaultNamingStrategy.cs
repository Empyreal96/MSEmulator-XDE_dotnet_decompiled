using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006E RID: 110
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0001BA9F File Offset: 0x00019C9F
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
