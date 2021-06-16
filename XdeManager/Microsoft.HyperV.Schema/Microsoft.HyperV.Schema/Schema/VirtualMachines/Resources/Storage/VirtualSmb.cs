using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003C RID: 60
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualSmb
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00004998 File Offset: 0x00002B98
		public static bool IsJsonDefault(VirtualSmb val)
		{
			return VirtualSmb._default.JsonEquals(val);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000049A8 File Offset: 0x00002BA8
		public bool JsonEquals(object obj)
		{
			VirtualSmb graph = obj as VirtualSmb;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualSmb), new DataContractJsonSerializerSettings
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

		// Token: 0x04000136 RID: 310
		private static readonly VirtualSmb _default = new VirtualSmb();

		// Token: 0x04000137 RID: 311
		[DataMember(EmitDefaultValue = false)]
		public VirtualSmbShare[] Shares;

		// Token: 0x04000138 RID: 312
		[DataMember(EmitDefaultValue = false)]
		public long DirectFileMappingInMB;
	}
}
