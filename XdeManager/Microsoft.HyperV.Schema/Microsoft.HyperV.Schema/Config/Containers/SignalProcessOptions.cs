using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000183 RID: 387
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SignalProcessOptions
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00013BE4 File Offset: 0x00011DE4
		public static bool IsJsonDefault(SignalProcessOptions val)
		{
			return SignalProcessOptions._default.JsonEquals(val);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00013BF4 File Offset: 0x00011DF4
		public bool JsonEquals(object obj)
		{
			SignalProcessOptions graph = obj as SignalProcessOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SignalProcessOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x04000839 RID: 2105
		private static readonly SignalProcessOptions _default = new SignalProcessOptions();

		// Token: 0x0400083A RID: 2106
		[DataMember]
		public int? Signal;
	}
}
