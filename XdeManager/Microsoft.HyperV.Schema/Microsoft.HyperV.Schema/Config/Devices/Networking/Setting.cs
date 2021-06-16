using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000143 RID: 323
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Setting
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00010A6C File Offset: 0x0000EC6C
		public static bool IsJsonDefault(Setting val)
		{
			return Setting._default.JsonEquals(val);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00010A7C File Offset: 0x0000EC7C
		public bool JsonEquals(object obj)
		{
			Setting graph = obj as Setting;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Setting), new DataContractJsonSerializerSettings
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

		// Token: 0x04000694 RID: 1684
		private static readonly Setting _default = new Setting();

		// Token: 0x04000695 RID: 1685
		[DataMember]
		public ulong Version;

		// Token: 0x04000696 RID: 1686
		[DataMember]
		public byte[] Data;
	}
}
