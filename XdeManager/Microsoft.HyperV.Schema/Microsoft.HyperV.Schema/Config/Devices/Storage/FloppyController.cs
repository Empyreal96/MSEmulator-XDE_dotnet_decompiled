using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012F RID: 303
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class FloppyController
	{
		// Token: 0x060004C1 RID: 1217 RVA: 0x0000F9AC File Offset: 0x0000DBAC
		public static bool IsJsonDefault(FloppyController val)
		{
			return FloppyController._default.JsonEquals(val);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		public bool JsonEquals(object obj)
		{
			FloppyController graph = obj as FloppyController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FloppyController), new DataContractJsonSerializerSettings
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

		// Token: 0x04000636 RID: 1590
		private static readonly FloppyController _default = new FloppyController();

		// Token: 0x04000637 RID: 1591
		[DataMember(Name = "drive")]
		public Drive[] Drives;
	}
}
