using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F3 RID: 243
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemorySettings
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0000CB20 File Offset: 0x0000AD20
		public static bool IsJsonDefault(MemorySettings val)
		{
			return MemorySettings._default.JsonEquals(val);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000CB30 File Offset: 0x0000AD30
		public bool JsonEquals(object obj)
		{
			MemorySettings graph = obj as MemorySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemorySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		[DataMember(Name = "bank")]
		private Memory _Memory
		{
			get
			{
				return this.Memory;
			}
			set
			{
				if (value != null)
				{
					this.Memory = value;
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		// (set) Token: 0x060003AA RID: 938 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		[DataMember(Name = "vnuma")]
		private NumaMemory _Numa
		{
			get
			{
				return this.Numa;
			}
			set
			{
				if (value != null)
				{
					this.Numa = value;
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000CC00 File Offset: 0x0000AE00
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000CC08 File Offset: 0x0000AE08
		[DataMember(Name = "sgx")]
		private SgxMemory _Sgx
		{
			get
			{
				return this.Sgx;
			}
			set
			{
				if (value != null)
				{
					this.Sgx = value;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000CC14 File Offset: 0x0000AE14
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000CC1C File Offset: 0x0000AE1C
		[DataMember(Name = "enlightenments")]
		private MemoryEnlightenments _Enlightenments
		{
			get
			{
				return this.Enlightenments;
			}
			set
			{
				if (value != null)
				{
					this.Enlightenments = value;
				}
			}
		}

		// Token: 0x040004AC RID: 1196
		private static readonly MemorySettings _default = new MemorySettings();

		// Token: 0x040004AD RID: 1197
		[DataMember(EmitDefaultValue = false, Name = "element_name")]
		public string ElementName;

		// Token: 0x040004AE RID: 1198
		public Memory Memory = new Memory();

		// Token: 0x040004AF RID: 1199
		public NumaMemory Numa = new NumaMemory();

		// Token: 0x040004B0 RID: 1200
		public SgxMemory Sgx = new SgxMemory();

		// Token: 0x040004B1 RID: 1201
		public MemoryEnlightenments Enlightenments = new MemoryEnlightenments();

		// Token: 0x040004B2 RID: 1202
		[DataMember(EmitDefaultValue = false, Name = "numa_affinity")]
		public List<byte> NumaAffinity;
	}
}
