using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000136 RID: 310
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMArsErrorRange
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x00010038 File Offset: 0x0000E238
		public static bool IsJsonDefault(VPMEMArsErrorRange val)
		{
			return VPMEMArsErrorRange._default.JsonEquals(val);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00010048 File Offset: 0x0000E248
		public bool JsonEquals(object obj)
		{
			VPMEMArsErrorRange graph = obj as VPMEMArsErrorRange;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMArsErrorRange), new DataContractJsonSerializerSettings
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

		// Token: 0x04000652 RID: 1618
		private static readonly VPMEMArsErrorRange _default = new VPMEMArsErrorRange();

		// Token: 0x04000653 RID: 1619
		[DataMember]
		public ulong VirtualOffset;

		// Token: 0x04000654 RID: 1620
		[DataMember]
		public ulong Length;

		// Token: 0x04000655 RID: 1621
		[DataMember]
		public bool Injected;
	}
}
