using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000191 RID: 401
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNamespace
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x00014714 File Offset: 0x00012914
		public static bool IsJsonDefault(HNSNamespace val)
		{
			return HNSNamespace._default.JsonEquals(val);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00014724 File Offset: 0x00012924
		public bool JsonEquals(object obj)
		{
			HNSNamespace graph = obj as HNSNamespace;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNamespace), new DataContractJsonSerializerSettings
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

		// Token: 0x040008A8 RID: 2216
		private static readonly HNSNamespace _default = new HNSNamespace();

		// Token: 0x040008A9 RID: 2217
		[DataMember]
		public Guid ID;

		// Token: 0x040008AA RID: 2218
		[DataMember]
		public bool IsDefault;

		// Token: 0x040008AB RID: 2219
		[DataMember(EmitDefaultValue = false)]
		public HNSNamespaceResource[] ResourceList;
	}
}
