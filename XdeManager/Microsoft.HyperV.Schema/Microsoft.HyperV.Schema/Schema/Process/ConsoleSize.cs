using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Process
{
	// Token: 0x02000081 RID: 129
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ConsoleSize
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00007C2C File Offset: 0x00005E2C
		public static bool IsJsonDefault(ConsoleSize val)
		{
			return ConsoleSize._default.JsonEquals(val);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007C3C File Offset: 0x00005E3C
		public bool JsonEquals(object obj)
		{
			ConsoleSize graph = obj as ConsoleSize;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ConsoleSize), new DataContractJsonSerializerSettings
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

		// Token: 0x040002D0 RID: 720
		private static readonly ConsoleSize _default = new ConsoleSize();

		// Token: 0x040002D1 RID: 721
		[DataMember]
		public ushort Height;

		// Token: 0x040002D2 RID: 722
		[DataMember]
		public ushort Width;
	}
}
