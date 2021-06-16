using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000EA RID: 234
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Security
	{
		// Token: 0x06000373 RID: 883 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		public static bool IsJsonDefault(Security val)
		{
			return Security._default.JsonEquals(val);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		public bool JsonEquals(object obj)
		{
			Security graph = obj as Security;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Security), new DataContractJsonSerializerSettings
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000C39C File Offset: 0x0000A59C
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000C3B3 File Offset: 0x0000A5B3
		[DataMember(EmitDefaultValue = false, Name = "settings")]
		private SecuritySettings _Settings
		{
			get
			{
				if (!SecuritySettings.IsJsonDefault(this.Settings))
				{
					return this.Settings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Settings = value;
				}
			}
		}

		// Token: 0x0400047B RID: 1147
		private static readonly Security _default = new Security();

		// Token: 0x0400047C RID: 1148
		public SecuritySettings Settings = new SecuritySettings();
	}
}
