using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FC RID: 252
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Settings
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x0000D304 File Offset: 0x0000B504
		public static bool IsJsonDefault(Settings val)
		{
			return Settings._default.JsonEquals(val);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D314 File Offset: 0x0000B514
		public bool JsonEquals(object obj)
		{
			Settings graph = obj as Settings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Settings), new DataContractJsonSerializerSettings
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		[DataMember(Name = "global")]
		private SettingsGlobal _Global
		{
			get
			{
				return this.Global;
			}
			set
			{
				if (value != null)
				{
					this.Global = value;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000D3E7 File Offset: 0x0000B5E7
		[DataMember(EmitDefaultValue = false, Name = "hcl")]
		private HclSettings _Hcl
		{
			get
			{
				if (!HclSettings.IsJsonDefault(this.Hcl))
				{
					return this.Hcl;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Hcl = value;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000D3F3 File Offset: 0x0000B5F3
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0000D40A File Offset: 0x0000B60A
		[DataMember(EmitDefaultValue = false, Name = "isolation")]
		private IsolationSettings _Isolation
		{
			get
			{
				if (!IsolationSettings.IsJsonDefault(this.Isolation))
				{
					return this.Isolation;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Isolation = value;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000D416 File Offset: 0x0000B616
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000D41E File Offset: 0x0000B61E
		[DataMember(Name = "memory")]
		private MemorySettings _Memory
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000D42A File Offset: 0x0000B62A
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000D432 File Offset: 0x0000B632
		[DataMember(Name = "processors")]
		private ProcessorSettings _Processor
		{
			get
			{
				return this.Processor;
			}
			set
			{
				if (value != null)
				{
					this.Processor = value;
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000D43E File Offset: 0x0000B63E
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0000D446 File Offset: 0x0000B646
		[DataMember(Name = "vnuma")]
		private NumaSettings _Numa
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

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000D452 File Offset: 0x0000B652
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x0000D45A File Offset: 0x0000B65A
		[DataMember(Name = "topology")]
		private TopologySettings _Topology
		{
			get
			{
				return this.Topology;
			}
			set
			{
				if (value != null)
				{
					this.Topology = value;
				}
			}
		}

		// Token: 0x040004E5 RID: 1253
		private static readonly Settings _default = new Settings();

		// Token: 0x040004E6 RID: 1254
		public SettingsGlobal Global = new SettingsGlobal();

		// Token: 0x040004E7 RID: 1255
		public HclSettings Hcl = new HclSettings();

		// Token: 0x040004E8 RID: 1256
		public IsolationSettings Isolation = new IsolationSettings();

		// Token: 0x040004E9 RID: 1257
		public MemorySettings Memory = new MemorySettings();

		// Token: 0x040004EA RID: 1258
		public ProcessorSettings Processor = new ProcessorSettings();

		// Token: 0x040004EB RID: 1259
		public NumaSettings Numa = new NumaSettings();

		// Token: 0x040004EC RID: 1260
		public TopologySettings Topology = new TopologySettings();
	}
}
