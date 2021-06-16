using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Bios
{
	// Token: 0x0200015E RID: 350
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class BaseBoard
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00011AE0 File Offset: 0x0000FCE0
		public static bool IsJsonDefault(BaseBoard val)
		{
			return BaseBoard._default.JsonEquals(val);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00011AF0 File Offset: 0x0000FCF0
		public bool JsonEquals(object obj)
		{
			BaseBoard graph = obj as BaseBoard;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(BaseBoard), new DataContractJsonSerializerSettings
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

		// Token: 0x0400073A RID: 1850
		private static readonly BaseBoard _default = new BaseBoard();

		// Token: 0x0400073B RID: 1851
		[DataMember(Name = "serial_number")]
		public string SerialNumber;
	}
}
