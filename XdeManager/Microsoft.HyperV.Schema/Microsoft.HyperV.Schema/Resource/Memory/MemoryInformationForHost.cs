using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CA RID: 202
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryInformationForHost
	{
		// Token: 0x0600030B RID: 779 RVA: 0x0000AF78 File Offset: 0x00009178
		public static bool IsJsonDefault(MemoryInformationForHost val)
		{
			return MemoryInformationForHost._default.JsonEquals(val);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000AF88 File Offset: 0x00009188
		public bool JsonEquals(object obj)
		{
			MemoryInformationForHost graph = obj as MemoryInformationForHost;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryInformationForHost), new DataContractJsonSerializerSettings
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000B030 File Offset: 0x00009230
		// (set) Token: 0x0600030E RID: 782 RVA: 0x0000B038 File Offset: 0x00009238
		[DataMember(Name = "HostMemoryInformation")]
		private HostMemory _HostMemoryInformation
		{
			get
			{
				return this.HostMemoryInformation;
			}
			set
			{
				if (value != null)
				{
					this.HostMemoryInformation = value;
				}
			}
		}

		// Token: 0x040003ED RID: 1005
		private static readonly MemoryInformationForHost _default = new MemoryInformationForHost();

		// Token: 0x040003EE RID: 1006
		[DataMember]
		public byte NumaNodeCount;

		// Token: 0x040003EF RID: 1007
		public HostMemory HostMemoryInformation = new HostMemory();

		// Token: 0x040003F0 RID: 1008
		[DataMember(EmitDefaultValue = false)]
		public NumaNodeMemory[] NumaNodes;

		// Token: 0x040003F1 RID: 1009
		[DataMember(EmitDefaultValue = false)]
		public MemoryBalancerInfo[] Balancers;
	}
}
