using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema
{
	// Token: 0x02000003 RID: 3
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestOs
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000211C File Offset: 0x0000031C
		public static bool IsJsonDefault(GuestOs val)
		{
			return GuestOs._default.JsonEquals(val);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000212C File Offset: 0x0000032C
		public bool JsonEquals(object obj)
		{
			GuestOs graph = obj as GuestOs;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestOs), new DataContractJsonSerializerSettings
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

		// Token: 0x04000004 RID: 4
		private static readonly GuestOs _default = new GuestOs();

		// Token: 0x04000005 RID: 5
		[DataMember(EmitDefaultValue = false)]
		public string HostName;
	}
}
