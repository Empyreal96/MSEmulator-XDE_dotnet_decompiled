using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000091 RID: 145
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060007ED RID: 2029 RVA: 0x00023665 File Offset: 0x00021865
		public JsonStringContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
