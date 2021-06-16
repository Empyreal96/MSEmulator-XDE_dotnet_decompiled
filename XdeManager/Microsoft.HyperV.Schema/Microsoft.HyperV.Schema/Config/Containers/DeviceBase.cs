using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200016B RID: 363
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DeviceBase
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x000122F3 File Offset: 0x000104F3
		public static bool IsJsonDefault(DeviceBase val)
		{
			return DeviceBase._default.JsonEquals(val);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00012300 File Offset: 0x00010500
		public bool JsonEquals(object obj)
		{
			DeviceBase graph = obj as DeviceBase;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DeviceBase), new DataContractJsonSerializerSettings
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

		// Token: 0x0400078E RID: 1934
		private static readonly DeviceBase _default = new DeviceBase();

		// Token: 0x0400078F RID: 1935
		[DataMember]
		public string DeviceType;
	}
}
