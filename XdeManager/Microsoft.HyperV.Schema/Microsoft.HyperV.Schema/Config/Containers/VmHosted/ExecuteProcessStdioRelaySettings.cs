using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.VmHosted
{
	// Token: 0x02000186 RID: 390
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ExecuteProcessStdioRelaySettings
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x00013F47 File Offset: 0x00012147
		public static bool IsJsonDefault(ExecuteProcessStdioRelaySettings val)
		{
			return ExecuteProcessStdioRelaySettings._default.JsonEquals(val);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00013F54 File Offset: 0x00012154
		public bool JsonEquals(object obj)
		{
			ExecuteProcessStdioRelaySettings graph = obj as ExecuteProcessStdioRelaySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ExecuteProcessStdioRelaySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x04000868 RID: 2152
		private static readonly ExecuteProcessStdioRelaySettings _default = new ExecuteProcessStdioRelaySettings();

		// Token: 0x04000869 RID: 2153
		[DataMember(EmitDefaultValue = false)]
		public Guid StdIn;

		// Token: 0x0400086A RID: 2154
		[DataMember(EmitDefaultValue = false)]
		public Guid StdOut;

		// Token: 0x0400086B RID: 2155
		[DataMember(EmitDefaultValue = false)]
		public Guid StdErr;
	}
}
