using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200002C RID: 44
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SharedMemoryConfiguration
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00004134 File Offset: 0x00002334
		public static bool IsJsonDefault(SharedMemoryConfiguration val)
		{
			return SharedMemoryConfiguration._default.JsonEquals(val);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004144 File Offset: 0x00002344
		public bool JsonEquals(object obj)
		{
			SharedMemoryConfiguration graph = obj as SharedMemoryConfiguration;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SharedMemoryConfiguration), new DataContractJsonSerializerSettings
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

		// Token: 0x040000CA RID: 202
		private static readonly SharedMemoryConfiguration _default = new SharedMemoryConfiguration();

		// Token: 0x040000CB RID: 203
		[DataMember(EmitDefaultValue = false)]
		public SharedMemoryRegion[] Regions;
	}
}
