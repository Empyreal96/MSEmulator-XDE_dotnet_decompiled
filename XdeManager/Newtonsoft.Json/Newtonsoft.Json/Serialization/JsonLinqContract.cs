using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000088 RID: 136
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x0001D36E File Offset: 0x0001B56E
		public JsonLinqContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
