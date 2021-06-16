using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Requests.Guest;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B7 RID: 439
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerModifySettings
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x000166B0 File Offset: 0x000148B0
		public static bool IsJsonDefault(ContainerModifySettings val)
		{
			return ContainerModifySettings._default.JsonEquals(val);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000166C0 File Offset: 0x000148C0
		public bool JsonEquals(object obj)
		{
			ContainerModifySettings graph = obj as ContainerModifySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerModifySettings), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x040009B9 RID: 2489
		private static readonly ContainerModifySettings _default = new ContainerModifySettings();

		// Token: 0x040009BA RID: 2490
		[DataMember]
		public string ContainerId;

		// Token: 0x040009BB RID: 2491
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009BC RID: 2492
		[DataMember]
		public object Request;

		// Token: 0x040009BD RID: 2493
		[DataMember]
		public GuestModifySettingRequest v2Request;
	}
}
