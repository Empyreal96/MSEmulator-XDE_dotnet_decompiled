using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Chipset
{
	// Token: 0x02000156 RID: 342
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RealTimeClock
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00011852 File Offset: 0x0000FA52
		public static bool IsJsonDefault(RealTimeClock val)
		{
			return RealTimeClock._default.JsonEquals(val);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00011860 File Offset: 0x0000FA60
		public bool JsonEquals(object obj)
		{
			RealTimeClock graph = obj as RealTimeClock;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RealTimeClock), new DataContractJsonSerializerSettings
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

		// Token: 0x04000719 RID: 1817
		private static readonly RealTimeClock _default = new RealTimeClock();

		// Token: 0x0400071A RID: 1818
		[DataMember(EmitDefaultValue = false)]
		public byte[] RuntimeState;

		// Token: 0x0400071B RID: 1819
		[DataMember]
		public long? CmosUtcSkew;
	}
}
