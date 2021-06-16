using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200013D RID: 317
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMControllerInfo
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x0001062C File Offset: 0x0000E82C
		public static bool IsJsonDefault(VPMEMControllerInfo val)
		{
			return VPMEMControllerInfo._default.JsonEquals(val);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001063C File Offset: 0x0000E83C
		public bool JsonEquals(object obj)
		{
			VPMEMControllerInfo graph = obj as VPMEMControllerInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMControllerInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x0400067B RID: 1659
		private static readonly VPMEMControllerInfo _default = new VPMEMControllerInfo();

		// Token: 0x0400067C RID: 1660
		[DataMember]
		public VPMEMDeviceInfo[] Devices;
	}
}
