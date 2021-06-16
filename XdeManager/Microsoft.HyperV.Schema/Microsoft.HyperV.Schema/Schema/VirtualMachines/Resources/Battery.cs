using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000026 RID: 38
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Battery
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00003C04 File Offset: 0x00001E04
		public static bool IsJsonDefault(Battery val)
		{
			return Battery._default.JsonEquals(val);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003C14 File Offset: 0x00001E14
		public bool JsonEquals(object obj)
		{
			Battery graph = obj as Battery;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Battery), new DataContractJsonSerializerSettings
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

		// Token: 0x040000AD RID: 173
		private static readonly Battery _default = new Battery();
	}
}
