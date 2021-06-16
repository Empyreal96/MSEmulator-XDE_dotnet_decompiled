using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000021 RID: 33
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Keyboard
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003788 File Offset: 0x00001988
		public static bool IsJsonDefault(Keyboard val)
		{
			return Keyboard._default.JsonEquals(val);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003798 File Offset: 0x00001998
		public bool JsonEquals(object obj)
		{
			Keyboard graph = obj as Keyboard;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Keyboard), new DataContractJsonSerializerSettings
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

		// Token: 0x040000A1 RID: 161
		private static readonly Keyboard _default = new Keyboard();
	}
}
