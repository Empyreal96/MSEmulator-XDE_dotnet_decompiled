using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200002B RID: 43
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SharedMemoryRegion
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x0000406A File Offset: 0x0000226A
		public static bool IsJsonDefault(SharedMemoryRegion val)
		{
			return SharedMemoryRegion._default.JsonEquals(val);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004078 File Offset: 0x00002278
		public bool JsonEquals(object obj)
		{
			SharedMemoryRegion graph = obj as SharedMemoryRegion;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SharedMemoryRegion), new DataContractJsonSerializerSettings
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

		// Token: 0x040000C4 RID: 196
		private static readonly SharedMemoryRegion _default = new SharedMemoryRegion();

		// Token: 0x040000C5 RID: 197
		[DataMember]
		public string SectionName;

		// Token: 0x040000C6 RID: 198
		[DataMember(EmitDefaultValue = false)]
		public ulong StartOffset;

		// Token: 0x040000C7 RID: 199
		[DataMember]
		public ulong Length;

		// Token: 0x040000C8 RID: 200
		[DataMember(EmitDefaultValue = false)]
		public bool AllowGuestWrite;

		// Token: 0x040000C9 RID: 201
		[DataMember(EmitDefaultValue = false)]
		public bool HiddenFromGuest;
	}
}
