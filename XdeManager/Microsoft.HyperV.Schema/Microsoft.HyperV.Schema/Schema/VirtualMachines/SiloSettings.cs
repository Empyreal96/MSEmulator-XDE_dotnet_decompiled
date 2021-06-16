using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x0200000E RID: 14
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SiloSettings
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000029DC File Offset: 0x00000BDC
		public static bool IsJsonDefault(SiloSettings val)
		{
			return SiloSettings._default.JsonEquals(val);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000029EC File Offset: 0x00000BEC
		public bool JsonEquals(object obj)
		{
			SiloSettings graph = obj as SiloSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SiloSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x04000053 RID: 83
		private static readonly SiloSettings _default = new SiloSettings();

		// Token: 0x04000054 RID: 84
		[DataMember(EmitDefaultValue = false)]
		public string SiloBaseOsPath;

		// Token: 0x04000055 RID: 85
		[DataMember(EmitDefaultValue = false)]
		public bool NotifySiloJobCreated;

		// Token: 0x04000056 RID: 86
		[DataMember(EmitDefaultValue = false)]
		public Layer[] FileSystemLayers;
	}
}
