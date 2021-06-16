using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000097 RID: 151
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Networking
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public static bool IsJsonDefault(Networking val)
		{
			return Networking._default.JsonEquals(val);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public bool JsonEquals(object obj)
		{
			Networking graph = obj as Networking;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Networking), new DataContractJsonSerializerSettings
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

		// Token: 0x04000320 RID: 800
		private static readonly Networking _default = new Networking();

		// Token: 0x04000321 RID: 801
		[DataMember(EmitDefaultValue = false)]
		public bool AllowUnqualifiedDnsQuery;

		// Token: 0x04000322 RID: 802
		[DataMember(EmitDefaultValue = false)]
		public string DnsSearchList;

		// Token: 0x04000323 RID: 803
		[DataMember(EmitDefaultValue = false)]
		public string NetworkSharedContainerName;

		// Token: 0x04000324 RID: 804
		[DataMember(EmitDefaultValue = false)]
		public string Namespace;

		// Token: 0x04000325 RID: 805
		[DataMember(EmitDefaultValue = false)]
		public Guid[] NetworkAdapters;
	}
}
