using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000054 RID: 84
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VmMemory
	{
		// Token: 0x0600013D RID: 317 RVA: 0x0000587C File Offset: 0x00003A7C
		public static bool IsJsonDefault(VmMemory val)
		{
			return VmMemory._default.JsonEquals(val);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000588C File Offset: 0x00003A8C
		public bool JsonEquals(object obj)
		{
			VmMemory graph = obj as VmMemory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VmMemory), new DataContractJsonSerializerSettings
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

		// Token: 0x040001B0 RID: 432
		private static readonly VmMemory _default = new VmMemory();

		// Token: 0x040001B1 RID: 433
		[DataMember]
		public int AvailableMemory;

		// Token: 0x040001B2 RID: 434
		[DataMember]
		public int AvailableMemoryBuffer;

		// Token: 0x040001B3 RID: 435
		[DataMember]
		public ulong ReservedMemory;

		// Token: 0x040001B4 RID: 436
		[DataMember]
		public ulong AssignedMemory;

		// Token: 0x040001B5 RID: 437
		[DataMember]
		public bool SlpActive;

		// Token: 0x040001B6 RID: 438
		[DataMember]
		public bool BalancingEnabled;

		// Token: 0x040001B7 RID: 439
		[DataMember]
		public bool DmOperationInProgress;
	}
}
