using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CB RID: 203
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VmMemoryModificationRequest
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000B063 File Offset: 0x00009263
		public static bool IsJsonDefault(VmMemoryModificationRequest val)
		{
			return VmMemoryModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000B070 File Offset: 0x00009270
		public bool JsonEquals(object obj)
		{
			VmMemoryModificationRequest graph = obj as VmMemoryModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VmMemoryModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000B118 File Offset: 0x00009318
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000B132 File Offset: 0x00009332
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				VmMemoryOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = VmMemoryOperation.ResizeMemory;
				}
				this.Operation = (VmMemoryOperation)Enum.Parse(typeof(VmMemoryOperation), value, true);
			}
		}

		// Token: 0x040003F2 RID: 1010
		private static readonly VmMemoryModificationRequest _default = new VmMemoryModificationRequest();

		// Token: 0x040003F3 RID: 1011
		public VmMemoryOperation Operation;

		// Token: 0x040003F4 RID: 1012
		[DataMember]
		public ulong RequestedPageCount;
	}
}
