using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.VmHosted
{
	// Token: 0x02000187 RID: 391
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ExecuteProcessVsockStdioRelaySettings
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x00014010 File Offset: 0x00012210
		public static bool IsJsonDefault(ExecuteProcessVsockStdioRelaySettings val)
		{
			return ExecuteProcessVsockStdioRelaySettings._default.JsonEquals(val);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00014020 File Offset: 0x00012220
		public bool JsonEquals(object obj)
		{
			ExecuteProcessVsockStdioRelaySettings graph = obj as ExecuteProcessVsockStdioRelaySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ExecuteProcessVsockStdioRelaySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x0400086C RID: 2156
		private static readonly ExecuteProcessVsockStdioRelaySettings _default = new ExecuteProcessVsockStdioRelaySettings();

		// Token: 0x0400086D RID: 2157
		[DataMember(EmitDefaultValue = false)]
		public uint StdIn;

		// Token: 0x0400086E RID: 2158
		[DataMember(EmitDefaultValue = false)]
		public uint StdOut;

		// Token: 0x0400086F RID: 2159
		[DataMember(EmitDefaultValue = false)]
		public uint StdErr;
	}
}
