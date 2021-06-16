using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001E RID: 30
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RdpConnectionOptions
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003524 File Offset: 0x00001724
		public static bool IsJsonDefault(RdpConnectionOptions val)
		{
			return RdpConnectionOptions._default.JsonEquals(val);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003534 File Offset: 0x00001734
		public bool JsonEquals(object obj)
		{
			RdpConnectionOptions graph = obj as RdpConnectionOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RdpConnectionOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x04000099 RID: 153
		private static readonly RdpConnectionOptions _default = new RdpConnectionOptions();

		// Token: 0x0400009A RID: 154
		[DataMember(EmitDefaultValue = false)]
		public string[] AccessSids;

		// Token: 0x0400009B RID: 155
		[DataMember(EmitDefaultValue = false)]
		public string NamedPipe;
	}
}
