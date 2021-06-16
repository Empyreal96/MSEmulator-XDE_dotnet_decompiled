using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x02000013 RID: 19
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestConnection
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002E2C File Offset: 0x0000102C
		public static bool IsJsonDefault(GuestConnection val)
		{
			return GuestConnection._default.JsonEquals(val);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E3C File Offset: 0x0000103C
		public bool JsonEquals(object obj)
		{
			GuestConnection graph = obj as GuestConnection;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestConnection), new DataContractJsonSerializerSettings
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

		// Token: 0x04000063 RID: 99
		private static readonly GuestConnection _default = new GuestConnection();

		// Token: 0x04000064 RID: 100
		[DataMember(EmitDefaultValue = false)]
		public bool UseVsock;

		// Token: 0x04000065 RID: 101
		[DataMember(EmitDefaultValue = false)]
		public bool UseConnectedSuspend;
	}
}
