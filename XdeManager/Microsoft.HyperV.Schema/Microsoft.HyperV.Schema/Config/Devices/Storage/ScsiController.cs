using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012B RID: 299
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ScsiController
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x0000F663 File Offset: 0x0000D863
		public static bool IsJsonDefault(ScsiController val)
		{
			return ScsiController._default.JsonEquals(val);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000F670 File Offset: 0x0000D870
		public bool JsonEquals(object obj)
		{
			ScsiController graph = obj as ScsiController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ScsiController), new DataContractJsonSerializerSettings
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

		// Token: 0x04000625 RID: 1573
		private static readonly ScsiController _default = new ScsiController();

		// Token: 0x04000626 RID: 1574
		[DataMember(Name = "drive")]
		public Dictionary<byte, Drive> Drives;
	}
}
