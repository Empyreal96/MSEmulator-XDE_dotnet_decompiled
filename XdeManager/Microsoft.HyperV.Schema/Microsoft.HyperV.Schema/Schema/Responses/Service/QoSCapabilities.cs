using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x02000068 RID: 104
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class QoSCapabilities
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public static bool IsJsonDefault(QoSCapabilities val)
		{
			return QoSCapabilities._default.JsonEquals(val);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public bool JsonEquals(object obj)
		{
			QoSCapabilities graph = obj as QoSCapabilities;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(QoSCapabilities), new DataContractJsonSerializerSettings
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

		// Token: 0x04000238 RID: 568
		private static readonly QoSCapabilities _default = new QoSCapabilities();

		// Token: 0x04000239 RID: 569
		[DataMember(EmitDefaultValue = false)]
		public bool ProcessorQoSSupported;
	}
}
