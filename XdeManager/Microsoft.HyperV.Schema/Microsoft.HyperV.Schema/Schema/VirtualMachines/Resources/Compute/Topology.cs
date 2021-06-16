using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x0200004C RID: 76
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Topology
	{
		// Token: 0x0600012B RID: 299 RVA: 0x000055B0 File Offset: 0x000037B0
		public static bool IsJsonDefault(Topology val)
		{
			return Topology._default.JsonEquals(val);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000055C0 File Offset: 0x000037C0
		public bool JsonEquals(object obj)
		{
			Topology graph = obj as Topology;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Topology), new DataContractJsonSerializerSettings
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00005668 File Offset: 0x00003868
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00005670 File Offset: 0x00003870
		[DataMember(Name = "Memory")]
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000567C File Offset: 0x0000387C
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00005684 File Offset: 0x00003884
		[DataMember(Name = "Processor")]
		private Processor _Processor
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

		// Token: 0x04000188 RID: 392
		private static readonly Topology _default = new Topology();

		// Token: 0x04000189 RID: 393
		public Memory Memory = new Memory();

		// Token: 0x0400018A RID: 394
		public Processor Processor = new Processor();

		// Token: 0x0400018B RID: 395
		[DataMember(EmitDefaultValue = false)]
		public Numa Numa;
	}
}
