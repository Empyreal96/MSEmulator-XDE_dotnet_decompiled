using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.VmWorkerProcess;

namespace HCS.Config.VirtualMachine
{
	// Token: 0x02000108 RID: 264
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Configuration
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
		public static bool IsJsonDefault(Configuration val)
		{
			return Configuration._default.JsonEquals(val);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public bool JsonEquals(object obj)
		{
			Configuration graph = obj as Configuration;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Configuration), new DataContractJsonSerializerSettings
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000E068 File Offset: 0x0000C268
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x0000E070 File Offset: 0x0000C270
		[DataMember(IsRequired = true, Name = "InitContext")]
		private InitContext _InitContext
		{
			get
			{
				return this.InitContext;
			}
			set
			{
				if (value != null)
				{
					this.InitContext = value;
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000E07C File Offset: 0x0000C27C
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0000E084 File Offset: 0x0000C284
		[DataMember(IsRequired = true, Name = "properties")]
		private Properties _Properties
		{
			get
			{
				return this.Properties;
			}
			set
			{
				if (value != null)
				{
					this.Properties = value;
				}
			}
		}

		// Token: 0x0400053F RID: 1343
		private static readonly Configuration _default = new Configuration();

		// Token: 0x04000540 RID: 1344
		public InitContext InitContext = new InitContext();

		// Token: 0x04000541 RID: 1345
		public Properties Properties = new Properties();
	}
}
