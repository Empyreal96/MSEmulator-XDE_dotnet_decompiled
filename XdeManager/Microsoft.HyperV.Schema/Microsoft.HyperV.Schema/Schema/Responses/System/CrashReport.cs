using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000064 RID: 100
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CrashReport
	{
		// Token: 0x06000195 RID: 405 RVA: 0x00006843 File Offset: 0x00004A43
		public static bool IsJsonDefault(CrashReport val)
		{
			return CrashReport._default.JsonEquals(val);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006850 File Offset: 0x00004A50
		public bool JsonEquals(object obj)
		{
			CrashReport graph = obj as CrashReport;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CrashReport), new DataContractJsonSerializerSettings
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

		// Token: 0x0400021F RID: 543
		private static readonly CrashReport _default = new CrashReport();

		// Token: 0x04000220 RID: 544
		[DataMember(IsRequired = true)]
		public string SystemId;

		// Token: 0x04000221 RID: 545
		[DataMember(EmitDefaultValue = false)]
		public Guid ActivityId;

		// Token: 0x04000222 RID: 546
		[DataMember(EmitDefaultValue = false)]
		public WindowsCrashReport WindowsCrashInfo;
	}
}
