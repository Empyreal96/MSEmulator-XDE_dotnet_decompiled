using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000144 RID: 324
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Feature
	{
		// Token: 0x0600051F RID: 1311 RVA: 0x00010B38 File Offset: 0x0000ED38
		public static bool IsJsonDefault(Feature val)
		{
			return Feature._default.JsonEquals(val);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00010B48 File Offset: 0x0000ED48
		public bool JsonEquals(object obj)
		{
			Feature graph = obj as Feature;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Feature), new DataContractJsonSerializerSettings
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

		// Token: 0x04000697 RID: 1687
		private static readonly Feature _default = new Feature();

		// Token: 0x04000698 RID: 1688
		[DataMember]
		public string DisplayName;

		// Token: 0x04000699 RID: 1689
		[DataMember]
		public ulong Flags;

		// Token: 0x0400069A RID: 1690
		[DataMember]
		public List<Guid> Settings;

		// Token: 0x0400069B RID: 1691
		[DataMember(Name = "Setting_")]
		public Dictionary<Guid, Setting> SettingsMap;
	}
}
