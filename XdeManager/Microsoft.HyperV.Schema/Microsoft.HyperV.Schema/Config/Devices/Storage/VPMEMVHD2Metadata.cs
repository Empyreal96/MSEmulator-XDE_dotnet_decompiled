using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000138 RID: 312
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMVHD2Metadata
	{
		// Token: 0x060004ED RID: 1261 RVA: 0x000101D0 File Offset: 0x0000E3D0
		public static bool IsJsonDefault(VPMEMVHD2Metadata val)
		{
			return VPMEMVHD2Metadata._default.JsonEquals(val);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public bool JsonEquals(object obj)
		{
			VPMEMVHD2Metadata graph = obj as VPMEMVHD2Metadata;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMVHD2Metadata), new DataContractJsonSerializerSettings
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

		// Token: 0x0400065C RID: 1628
		private static readonly VPMEMVHD2Metadata _default = new VPMEMVHD2Metadata();

		// Token: 0x0400065D RID: 1629
		[DataMember]
		public ushort VHD2Version;

		// Token: 0x0400065E RID: 1630
		[DataMember]
		public List<VPMEMVHD2MetadataItem> MetadataList;
	}
}
