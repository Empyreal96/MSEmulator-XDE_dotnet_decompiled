using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Vpci
{
	// Token: 0x020000A5 RID: 165
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DeviceInstance
	{
		// Token: 0x06000285 RID: 645 RVA: 0x000095C3 File Offset: 0x000077C3
		public static bool IsJsonDefault(DeviceInstance val)
		{
			return DeviceInstance._default.JsonEquals(val);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000095D0 File Offset: 0x000077D0
		public bool JsonEquals(object obj)
		{
			DeviceInstance graph = obj as DeviceInstance;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DeviceInstance), new DataContractJsonSerializerSettings
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

		// Token: 0x04000355 RID: 853
		private static readonly DeviceInstance _default = new DeviceInstance();

		// Token: 0x04000356 RID: 854
		[DataMember]
		public string Id;

		// Token: 0x04000357 RID: 855
		[DataMember]
		public long Luid;
	}
}
