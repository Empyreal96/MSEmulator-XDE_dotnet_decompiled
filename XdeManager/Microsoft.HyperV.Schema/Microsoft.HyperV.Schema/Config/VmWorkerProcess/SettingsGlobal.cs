using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000EE RID: 238
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SettingsGlobal
	{
		// Token: 0x06000391 RID: 913 RVA: 0x0000C725 File Offset: 0x0000A925
		public static bool IsJsonDefault(SettingsGlobal val)
		{
			return SettingsGlobal._default.JsonEquals(val);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000C734 File Offset: 0x0000A934
		public bool JsonEquals(object obj)
		{
			SettingsGlobal graph = obj as SettingsGlobal;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SettingsGlobal), new DataContractJsonSerializerSettings
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

		// Token: 0x0400048B RID: 1163
		private static readonly SettingsGlobal _default = new SettingsGlobal();

		// Token: 0x0400048C RID: 1164
		[DataMember(Name = "logical_id")]
		public Guid VmId;
	}
}
