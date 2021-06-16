using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema
{
	// Token: 0x02000002 RID: 2
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Version
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool IsJsonDefault(Version val)
		{
			return Version._default.JsonEquals(val);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public bool JsonEquals(object obj)
		{
			Version graph = obj as Version;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Version), new DataContractJsonSerializerSettings
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

		// Token: 0x04000001 RID: 1
		private static readonly Version _default = new Version();

		// Token: 0x04000002 RID: 2
		[DataMember]
		public uint Major;

		// Token: 0x04000003 RID: 3
		[DataMember]
		public uint Minor;
	}
}
