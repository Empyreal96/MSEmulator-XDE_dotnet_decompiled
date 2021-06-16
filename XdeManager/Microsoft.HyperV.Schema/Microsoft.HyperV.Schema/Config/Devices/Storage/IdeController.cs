using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012D RID: 301
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IdeController
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x0000F817 File Offset: 0x0000DA17
		public static bool IsJsonDefault(IdeController val)
		{
			return IdeController._default.JsonEquals(val);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000F824 File Offset: 0x0000DA24
		public bool JsonEquals(object obj)
		{
			IdeController graph = obj as IdeController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IdeController), new DataContractJsonSerializerSettings
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

		// Token: 0x04000630 RID: 1584
		private static readonly IdeController _default = new IdeController();

		// Token: 0x04000631 RID: 1585
		[DataMember(Name = "drive")]
		public Drive[] Drives;
	}
}
