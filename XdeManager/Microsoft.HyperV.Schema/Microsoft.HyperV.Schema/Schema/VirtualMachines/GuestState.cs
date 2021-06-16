using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x02000011 RID: 17
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestState
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002C94 File Offset: 0x00000E94
		public static bool IsJsonDefault(GuestState val)
		{
			return GuestState._default.JsonEquals(val);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public bool JsonEquals(object obj)
		{
			GuestState graph = obj as GuestState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestState), new DataContractJsonSerializerSettings
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

		// Token: 0x0400005B RID: 91
		private static readonly GuestState _default = new GuestState();

		// Token: 0x0400005C RID: 92
		[DataMember(EmitDefaultValue = false)]
		public string GuestStateFilePath;

		// Token: 0x0400005D RID: 93
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeStateFilePath;

		// Token: 0x0400005E RID: 94
		[DataMember(EmitDefaultValue = false)]
		public bool ForceTransientState;
	}
}
