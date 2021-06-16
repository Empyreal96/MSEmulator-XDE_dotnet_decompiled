using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000171 RID: 369
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VsockPortRange
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x000128F4 File Offset: 0x00010AF4
		public static bool IsJsonDefault(VsockPortRange val)
		{
			return VsockPortRange._default.JsonEquals(val);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00012904 File Offset: 0x00010B04
		public bool JsonEquals(object obj)
		{
			VsockPortRange graph = obj as VsockPortRange;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VsockPortRange), new DataContractJsonSerializerSettings
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

		// Token: 0x040007C0 RID: 1984
		private static readonly VsockPortRange _default = new VsockPortRange();

		// Token: 0x040007C1 RID: 1985
		[DataMember]
		public uint Min;

		// Token: 0x040007C2 RID: 1986
		[DataMember]
		public uint Max;
	}
}
