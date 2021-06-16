using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000020 RID: 32
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Mouse
	{
		// Token: 0x06000083 RID: 131 RVA: 0x000036BC File Offset: 0x000018BC
		public static bool IsJsonDefault(Mouse val)
		{
			return Mouse._default.JsonEquals(val);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000036CC File Offset: 0x000018CC
		public bool JsonEquals(object obj)
		{
			Mouse graph = obj as Mouse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Mouse), new DataContractJsonSerializerSettings
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

		// Token: 0x040000A0 RID: 160
		private static readonly Mouse _default = new Mouse();
	}
}
