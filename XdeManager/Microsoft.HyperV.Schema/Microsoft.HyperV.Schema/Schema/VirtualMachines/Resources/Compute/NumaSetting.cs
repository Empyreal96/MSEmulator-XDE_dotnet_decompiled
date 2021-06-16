using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x0200004A RID: 74
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaSetting
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00005418 File Offset: 0x00003618
		public static bool IsJsonDefault(NumaSetting val)
		{
			return NumaSetting._default.JsonEquals(val);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005428 File Offset: 0x00003628
		public bool JsonEquals(object obj)
		{
			NumaSetting graph = obj as NumaSetting;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaSetting), new DataContractJsonSerializerSettings
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

		// Token: 0x0400017F RID: 383
		private static readonly NumaSetting _default = new NumaSetting();

		// Token: 0x04000180 RID: 384
		[DataMember]
		public uint VirtualNodeNumber;

		// Token: 0x04000181 RID: 385
		[DataMember]
		public uint PhysicalNodeNumber;

		// Token: 0x04000182 RID: 386
		[DataMember]
		public uint CountOfProcessors;

		// Token: 0x04000183 RID: 387
		[DataMember]
		public ulong CountOfMemoryBlocks;
	}
}
