using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B6 RID: 182
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CpuGroupConfig
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000A18C File Offset: 0x0000838C
		public static bool IsJsonDefault(CpuGroupConfig val)
		{
			return CpuGroupConfig._default.JsonEquals(val);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A19C File Offset: 0x0000839C
		public bool JsonEquals(object obj)
		{
			CpuGroupConfig graph = obj as CpuGroupConfig;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CpuGroupConfig), new DataContractJsonSerializerSettings
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000A244 File Offset: 0x00008444
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000A24C File Offset: 0x0000844C
		[DataMember(Name = "Affinity")]
		private CpuGroupAffinity _Affinity
		{
			get
			{
				return this.Affinity;
			}
			set
			{
				if (value != null)
				{
					this.Affinity = value;
				}
			}
		}

		// Token: 0x0400039F RID: 927
		private static readonly CpuGroupConfig _default = new CpuGroupConfig();

		// Token: 0x040003A0 RID: 928
		[DataMember]
		public Guid GroupId;

		// Token: 0x040003A1 RID: 929
		public CpuGroupAffinity Affinity = new CpuGroupAffinity();

		// Token: 0x040003A2 RID: 930
		[DataMember(EmitDefaultValue = false)]
		public CpuGroupProperty[] GroupProperties;
	}
}
