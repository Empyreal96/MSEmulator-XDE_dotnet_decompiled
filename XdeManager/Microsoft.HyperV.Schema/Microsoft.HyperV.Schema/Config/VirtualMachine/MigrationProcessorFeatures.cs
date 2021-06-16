using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VirtualMachine
{
	// Token: 0x02000105 RID: 261
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MigrationProcessorFeatures
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		public static bool IsJsonDefault(MigrationProcessorFeatures val)
		{
			return MigrationProcessorFeatures._default.JsonEquals(val);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000DD00 File Offset: 0x0000BF00
		public bool JsonEquals(object obj)
		{
			MigrationProcessorFeatures graph = obj as MigrationProcessorFeatures;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MigrationProcessorFeatures), new DataContractJsonSerializerSettings
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

		// Token: 0x04000526 RID: 1318
		private static readonly MigrationProcessorFeatures _default = new MigrationProcessorFeatures();

		// Token: 0x04000527 RID: 1319
		[DataMember]
		public ulong Features;

		// Token: 0x04000528 RID: 1320
		[DataMember]
		public ulong XsaveFeatures;
	}
}
