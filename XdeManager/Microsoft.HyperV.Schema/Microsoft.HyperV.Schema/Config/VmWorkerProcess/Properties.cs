using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E4 RID: 228
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Properties
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public static bool IsJsonDefault(Properties val)
		{
			return Properties._default.JsonEquals(val);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000BE2C File Offset: 0x0000A02C
		public bool JsonEquals(object obj)
		{
			Properties graph = obj as Properties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Properties), new DataContractJsonSerializerSettings
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

		// Token: 0x0400045B RID: 1115
		private static readonly Properties _default = new Properties();

		// Token: 0x0400045C RID: 1116
		[DataMember(Name = "global_id")]
		public Guid GlobalId;

		// Token: 0x0400045D RID: 1117
		[DataMember(Name = "type_id")]
		public string TypeId;

		// Token: 0x0400045E RID: 1118
		[DataMember(Name = "type")]
		public SystemType VirtualSystemType;

		// Token: 0x0400045F RID: 1119
		[DataMember(Name = "subtype")]
		public SubType VirtualSystemSubType;

		// Token: 0x04000460 RID: 1120
		[DataMember(Name = "version")]
		public uint Version;

		// Token: 0x04000461 RID: 1121
		[DataMember(Name = "name")]
		public string Name;

		// Token: 0x04000462 RID: 1122
		[DataMember(EmitDefaultValue = false)]
		public bool HcsManaged;
	}
}
