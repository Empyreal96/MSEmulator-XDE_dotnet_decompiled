using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.Containers.VmHosted;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B2 RID: 434
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerExecuteProcess
	{
		// Token: 0x06000705 RID: 1797 RVA: 0x0001629A File Offset: 0x0001449A
		public static bool IsJsonDefault(ContainerExecuteProcess val)
		{
			return ContainerExecuteProcess._default.JsonEquals(val);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000162A8 File Offset: 0x000144A8
		public bool JsonEquals(object obj)
		{
			ContainerExecuteProcess graph = obj as ContainerExecuteProcess;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerExecuteProcess), new DataContractJsonSerializerSettings
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00016350 File Offset: 0x00014550
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x00016358 File Offset: 0x00014558
		[DataMember(Name = "Settings")]
		private ExecuteProcessSettings _Settings
		{
			get
			{
				return this.Settings;
			}
			set
			{
				if (value != null)
				{
					this.Settings = value;
				}
			}
		}

		// Token: 0x040009A1 RID: 2465
		private static readonly ContainerExecuteProcess _default = new ContainerExecuteProcess();

		// Token: 0x040009A2 RID: 2466
		[DataMember]
		public string ContainerId;

		// Token: 0x040009A3 RID: 2467
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009A4 RID: 2468
		public ExecuteProcessSettings Settings = new ExecuteProcessSettings();
	}
}
