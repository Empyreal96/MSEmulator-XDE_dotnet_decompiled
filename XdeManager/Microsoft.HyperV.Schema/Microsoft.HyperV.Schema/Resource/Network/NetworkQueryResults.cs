using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Network
{
	// Token: 0x020000C1 RID: 193
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkQueryResults
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public static bool IsJsonDefault(NetworkQueryResults val)
		{
			return NetworkQueryResults._default.JsonEquals(val);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		public bool JsonEquals(object obj)
		{
			NetworkQueryResults graph = obj as NetworkQueryResults;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkQueryResults), new DataContractJsonSerializerSettings
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

		// Token: 0x040003C3 RID: 963
		private static readonly NetworkQueryResults _default = new NetworkQueryResults();

		// Token: 0x040003C4 RID: 964
		[DataMember]
		public Guid HNSEndpoint;

		// Token: 0x040003C5 RID: 965
		[DataMember]
		public bool Precreated;
	}
}
