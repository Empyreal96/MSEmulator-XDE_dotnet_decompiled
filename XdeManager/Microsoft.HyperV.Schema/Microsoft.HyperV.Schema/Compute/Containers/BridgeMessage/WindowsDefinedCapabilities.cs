using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AD RID: 429
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class WindowsDefinedCapabilities
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x00015DE7 File Offset: 0x00013FE7
		public static bool IsJsonDefault(WindowsDefinedCapabilities val)
		{
			return WindowsDefinedCapabilities._default.JsonEquals(val);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00015DF4 File Offset: 0x00013FF4
		public bool JsonEquals(object obj)
		{
			WindowsDefinedCapabilities graph = obj as WindowsDefinedCapabilities;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(WindowsDefinedCapabilities), new DataContractJsonSerializerSettings
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

		// Token: 0x0400098A RID: 2442
		private static readonly WindowsDefinedCapabilities _default = new WindowsDefinedCapabilities();

		// Token: 0x0400098B RID: 2443
		[DataMember(EmitDefaultValue = false)]
		public bool NamespaceAddRequestSupported;

		// Token: 0x0400098C RID: 2444
		[DataMember(EmitDefaultValue = false)]
		public bool SignalProcessSupported;
	}
}
