using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000038 RID: 56
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Scsi
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00004623 File Offset: 0x00002823
		public static bool IsJsonDefault(Scsi val)
		{
			return Scsi._default.JsonEquals(val);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004630 File Offset: 0x00002830
		public bool JsonEquals(object obj)
		{
			Scsi graph = obj as Scsi;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Scsi), new DataContractJsonSerializerSettings
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

		// Token: 0x04000112 RID: 274
		private static readonly Scsi _default = new Scsi();

		// Token: 0x04000113 RID: 275
		[DataMember]
		public Dictionary<uint, Attachment> Attachments;
	}
}
