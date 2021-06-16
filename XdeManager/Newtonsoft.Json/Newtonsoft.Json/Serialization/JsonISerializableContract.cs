using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000087 RID: 135
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001D34D File Offset: 0x0001B54D
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001D355 File Offset: 0x0001B555
		public ObjectConstructor<object> ISerializableCreator { get; set; }

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001D35E File Offset: 0x0001B55E
		public JsonISerializableContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
