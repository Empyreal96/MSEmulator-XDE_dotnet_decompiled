using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x0200018B RID: 395
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNetworkSwitchExtension
	{
		// Token: 0x06000653 RID: 1619 RVA: 0x00014204 File Offset: 0x00012404
		public static bool IsJsonDefault(HNSNetworkSwitchExtension val)
		{
			return HNSNetworkSwitchExtension._default.JsonEquals(val);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00014214 File Offset: 0x00012414
		public bool JsonEquals(object obj)
		{
			HNSNetworkSwitchExtension graph = obj as HNSNetworkSwitchExtension;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNetworkSwitchExtension), new DataContractJsonSerializerSettings
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

		// Token: 0x0400087F RID: 2175
		private static readonly HNSNetworkSwitchExtension _default = new HNSNetworkSwitchExtension();

		// Token: 0x04000880 RID: 2176
		[DataMember(EmitDefaultValue = false)]
		public Guid Id;

		// Token: 0x04000881 RID: 2177
		[DataMember(EmitDefaultValue = false)]
		public bool IsEnabled;
	}
}
