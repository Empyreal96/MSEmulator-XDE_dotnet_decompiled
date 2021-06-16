using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.VmHosted
{
	// Token: 0x02000188 RID: 392
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ExecuteProcessSettings
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x000140DC File Offset: 0x000122DC
		public static bool IsJsonDefault(ExecuteProcessSettings val)
		{
			return ExecuteProcessSettings._default.JsonEquals(val);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000140EC File Offset: 0x000122EC
		public bool JsonEquals(object obj)
		{
			ExecuteProcessSettings graph = obj as ExecuteProcessSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ExecuteProcessSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00014194 File Offset: 0x00012394
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x000141AB File Offset: 0x000123AB
		[DataMember(EmitDefaultValue = false, Name = "StdioRelaySettings")]
		private ExecuteProcessStdioRelaySettings _StdioRelaySettings
		{
			get
			{
				if (!ExecuteProcessStdioRelaySettings.IsJsonDefault(this.StdioRelaySettings))
				{
					return this.StdioRelaySettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.StdioRelaySettings = value;
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x000141B7 File Offset: 0x000123B7
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x000141CE File Offset: 0x000123CE
		[DataMember(EmitDefaultValue = false, Name = "VsockStdioRelaySettings")]
		private ExecuteProcessVsockStdioRelaySettings _VsockStdioRelaySettings
		{
			get
			{
				if (!ExecuteProcessVsockStdioRelaySettings.IsJsonDefault(this.VsockStdioRelaySettings))
				{
					return this.VsockStdioRelaySettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.VsockStdioRelaySettings = value;
				}
			}
		}

		// Token: 0x04000870 RID: 2160
		private static readonly ExecuteProcessSettings _default = new ExecuteProcessSettings();

		// Token: 0x04000871 RID: 2161
		[DataMember]
		public string ProcessParameters;

		// Token: 0x04000872 RID: 2162
		public ExecuteProcessStdioRelaySettings StdioRelaySettings = new ExecuteProcessStdioRelaySettings();

		// Token: 0x04000873 RID: 2163
		public ExecuteProcessVsockStdioRelaySettings VsockStdioRelaySettings = new ExecuteProcessVsockStdioRelaySettings();
	}
}
