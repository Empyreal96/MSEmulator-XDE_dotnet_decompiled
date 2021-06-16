using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x0200010D RID: 269
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostResources
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0000E31C File Offset: 0x0000C51C
		public static bool IsJsonDefault(HostResources val)
		{
			return HostResources._default.JsonEquals(val);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000E32C File Offset: 0x0000C52C
		public bool JsonEquals(object obj)
		{
			HostResources graph = obj as HostResources;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostResources), new DataContractJsonSerializerSettings
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

		// Token: 0x0400054F RID: 1359
		private static readonly HostResources _default = new HostResources();

		// Token: 0x04000550 RID: 1360
		[DataMember(Name = "count")]
		public int Count;

		// Token: 0x04000551 RID: 1361
		[DataMember(EmitDefaultValue = false, Name = "HostResource")]
		public HostResource[] Resources;
	}
}
