using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000055 RID: 85
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryInformationForVm
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00005948 File Offset: 0x00003B48
		public static bool IsJsonDefault(MemoryInformationForVm val)
		{
			return MemoryInformationForVm._default.JsonEquals(val);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005958 File Offset: 0x00003B58
		public bool JsonEquals(object obj)
		{
			MemoryInformationForVm graph = obj as MemoryInformationForVm;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryInformationForVm), new DataContractJsonSerializerSettings
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005A00 File Offset: 0x00003C00
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00005A08 File Offset: 0x00003C08
		[DataMember(Name = "VirtualMachineMemory")]
		private VmMemory _VirtualMachineMemory
		{
			get
			{
				return this.VirtualMachineMemory;
			}
			set
			{
				if (value != null)
				{
					this.VirtualMachineMemory = value;
				}
			}
		}

		// Token: 0x040001B8 RID: 440
		private static readonly MemoryInformationForVm _default = new MemoryInformationForVm();

		// Token: 0x040001B9 RID: 441
		[DataMember]
		public byte VirtualNodeCount;

		// Token: 0x040001BA RID: 442
		public VmMemory VirtualMachineMemory = new VmMemory();

		// Token: 0x040001BB RID: 443
		[DataMember(EmitDefaultValue = false)]
		public VirtualNodeInfo[] VirtualNodes;
	}
}
