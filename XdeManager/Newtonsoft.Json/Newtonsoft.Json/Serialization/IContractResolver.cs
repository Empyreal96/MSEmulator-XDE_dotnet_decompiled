using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000077 RID: 119
	public interface IContractResolver
	{
		// Token: 0x0600066A RID: 1642
		JsonContract ResolveContract(Type type);
	}
}
