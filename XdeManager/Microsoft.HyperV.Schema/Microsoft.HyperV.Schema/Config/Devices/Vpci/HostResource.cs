using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x0200010C RID: 268
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostResource
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x0000E250 File Offset: 0x0000C450
		public static bool IsJsonDefault(HostResource val)
		{
			return HostResource._default.JsonEquals(val);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000E260 File Offset: 0x0000C460
		public bool JsonEquals(object obj)
		{
			HostResource graph = obj as HostResource;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostResource), new DataContractJsonSerializerSettings
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

		// Token: 0x0400054C RID: 1356
		private static readonly HostResource _default = new HostResource();

		// Token: 0x0400054D RID: 1357
		[DataMember(Name = "Instance")]
		public string DeviceInstancePath;

		// Token: 0x0400054E RID: 1358
		[DataMember(EmitDefaultValue = false)]
		public ushort VirtualFunction;
	}
}
