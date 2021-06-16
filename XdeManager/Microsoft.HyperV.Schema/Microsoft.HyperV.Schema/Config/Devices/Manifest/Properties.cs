using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Manifest
{
	// Token: 0x02000149 RID: 329
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Properties
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x00010F34 File Offset: 0x0000F134
		public static bool IsJsonDefault(Properties val)
		{
			return Properties._default.JsonEquals(val);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00010F44 File Offset: 0x0000F144
		public bool JsonEquals(object obj)
		{
			Properties graph = obj as Properties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Properties), new DataContractJsonSerializerSettings
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

		// Token: 0x040006C9 RID: 1737
		private static readonly Properties _default = new Properties();

		// Token: 0x040006CA RID: 1738
		[DataMember(Name = "version")]
		public uint Version;

		// Token: 0x040006CB RID: 1739
		[DataMember(Name = "size")]
		public int Size;

		// Token: 0x040006CC RID: 1740
		[DataMember(Name = "vdev")]
		public Dictionary<int, DeviceEntry> Devices;
	}
}
