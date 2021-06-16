using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000099 RID: 153
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Storage
	{
		// Token: 0x06000251 RID: 593 RVA: 0x00008C58 File Offset: 0x00006E58
		public static bool IsJsonDefault(Storage val)
		{
			return Storage._default.JsonEquals(val);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008C68 File Offset: 0x00006E68
		public bool JsonEquals(object obj)
		{
			Storage graph = obj as Storage;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Storage), new DataContractJsonSerializerSettings
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

		// Token: 0x0400032A RID: 810
		private static readonly Storage _default = new Storage();

		// Token: 0x0400032B RID: 811
		[DataMember]
		public Layer[] Layers;

		// Token: 0x0400032C RID: 812
		[DataMember]
		public string Path;

		// Token: 0x0400032D RID: 813
		[DataMember(EmitDefaultValue = false)]
		public StorageQoS QoS;
	}
}
