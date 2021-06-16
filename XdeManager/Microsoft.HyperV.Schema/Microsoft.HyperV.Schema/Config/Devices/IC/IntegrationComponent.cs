using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x02000150 RID: 336
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IntegrationComponent
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x00011348 File Offset: 0x0000F548
		public static bool IsJsonDefault(IntegrationComponent val)
		{
			return IntegrationComponent._default.JsonEquals(val);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00011358 File Offset: 0x0000F558
		public bool JsonEquals(object obj)
		{
			IntegrationComponent graph = obj as IntegrationComponent;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IntegrationComponent), new DataContractJsonSerializerSettings
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

		// Token: 0x040006E9 RID: 1769
		private static readonly IntegrationComponent _default = new IntegrationComponent();

		// Token: 0x040006EA RID: 1770
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040006EB RID: 1771
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040006EC RID: 1772
		[DataMember]
		public bool Enabled = true;
	}
}
