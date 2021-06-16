using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000137 RID: 311
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMVHD2MetadataItem
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x00010104 File Offset: 0x0000E304
		public static bool IsJsonDefault(VPMEMVHD2MetadataItem val)
		{
			return VPMEMVHD2MetadataItem._default.JsonEquals(val);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00010114 File Offset: 0x0000E314
		public bool JsonEquals(object obj)
		{
			VPMEMVHD2MetadataItem graph = obj as VPMEMVHD2MetadataItem;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMVHD2MetadataItem), new DataContractJsonSerializerSettings
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

		// Token: 0x04000656 RID: 1622
		private static readonly VPMEMVHD2MetadataItem _default = new VPMEMVHD2MetadataItem();

		// Token: 0x04000657 RID: 1623
		[DataMember]
		public Guid ItemId;

		// Token: 0x04000658 RID: 1624
		[DataMember]
		public byte[] Buffer;

		// Token: 0x04000659 RID: 1625
		[DataMember]
		public bool IsUser;

		// Token: 0x0400065A RID: 1626
		[DataMember]
		public bool IsRequired;

		// Token: 0x0400065B RID: 1627
		[DataMember]
		public bool IsVirtualDisk;
	}
}
