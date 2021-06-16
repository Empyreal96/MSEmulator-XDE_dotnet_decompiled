using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003E RID: 62
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Plan9
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00004B4A File Offset: 0x00002D4A
		public static bool IsJsonDefault(Plan9 val)
		{
			return Plan9._default.JsonEquals(val);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004B58 File Offset: 0x00002D58
		public bool JsonEquals(object obj)
		{
			Plan9 graph = obj as Plan9;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Plan9), new DataContractJsonSerializerSettings
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

		// Token: 0x04000140 RID: 320
		private static readonly Plan9 _default = new Plan9();

		// Token: 0x04000141 RID: 321
		[DataMember(EmitDefaultValue = false)]
		public Plan9Share[] Shares;
	}
}
