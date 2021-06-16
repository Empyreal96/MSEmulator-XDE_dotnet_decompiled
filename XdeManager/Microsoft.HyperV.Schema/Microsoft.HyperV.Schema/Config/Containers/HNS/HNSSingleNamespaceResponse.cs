using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000198 RID: 408
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSSingleNamespaceResponse
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x00014D5C File Offset: 0x00012F5C
		public static bool IsJsonDefault(HNSSingleNamespaceResponse val)
		{
			return HNSSingleNamespaceResponse._default.JsonEquals(val);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00014D6C File Offset: 0x00012F6C
		public bool JsonEquals(object obj)
		{
			HNSSingleNamespaceResponse graph = obj as HNSSingleNamespaceResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSSingleNamespaceResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00014E14 File Offset: 0x00013014
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x00014E2B File Offset: 0x0001302B
		[DataMember(EmitDefaultValue = false, Name = "Output")]
		private HNSNamespace _Output
		{
			get
			{
				if (!HNSNamespace.IsJsonDefault(this.Output))
				{
					return this.Output;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Output = value;
				}
			}
		}

		// Token: 0x040008DA RID: 2266
		private static readonly HNSSingleNamespaceResponse _default = new HNSSingleNamespaceResponse();

		// Token: 0x040008DB RID: 2267
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008DC RID: 2268
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008DD RID: 2269
		public HNSNamespace Output = new HNSNamespace();
	}
}
