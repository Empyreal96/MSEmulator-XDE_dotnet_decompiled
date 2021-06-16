using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005A RID: 90
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Statistics
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00005D60 File Offset: 0x00003F60
		public static bool IsJsonDefault(Statistics val)
		{
			return Statistics._default.JsonEquals(val);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005D70 File Offset: 0x00003F70
		public bool JsonEquals(object obj)
		{
			Statistics graph = obj as Statistics;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Statistics), new DataContractJsonSerializerSettings
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00005E18 File Offset: 0x00004018
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00005E3D File Offset: 0x0000403D
		[DataMember(Name = "Timestamp")]
		private string _Timestamp
		{
			get
			{
				return this.Timestamp.ToUniversalTime().ToString("o");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Timestamp = default(DateTime);
				}
				this.Timestamp = DateTime.Parse(value);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00005E60 File Offset: 0x00004060
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005E9D File Offset: 0x0000409D
		[DataMember(EmitDefaultValue = false, Name = "ContainerStartTime")]
		private string _ContainerStartTime
		{
			get
			{
				if (this.ContainerStartTime == default(DateTime))
				{
					return null;
				}
				return this.ContainerStartTime.ToUniversalTime().ToString("o");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ContainerStartTime = default(DateTime);
				}
				this.ContainerStartTime = DateTime.Parse(value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005EBF File Offset: 0x000040BF
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005EC7 File Offset: 0x000040C7
		[DataMember(Name = "Processor")]
		private ProcessorStats _Processor
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005ED3 File Offset: 0x000040D3
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005EDB File Offset: 0x000040DB
		[DataMember(Name = "Memory")]
		private MemoryStats _Memory
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005EE7 File Offset: 0x000040E7
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00005EEF File Offset: 0x000040EF
		[DataMember(Name = "Storage")]
		private StorageStats _Storage
		{
			get
			{
				return this.Storage;
			}
			set
			{
				if (value != null)
				{
					this.Storage = value;
				}
			}
		}

		// Token: 0x040001D0 RID: 464
		private static readonly Statistics _default = new Statistics();

		// Token: 0x040001D1 RID: 465
		public DateTime Timestamp;

		// Token: 0x040001D2 RID: 466
		public DateTime ContainerStartTime;

		// Token: 0x040001D3 RID: 467
		[DataMember]
		public ulong Uptime100ns;

		// Token: 0x040001D4 RID: 468
		public ProcessorStats Processor = new ProcessorStats();

		// Token: 0x040001D5 RID: 469
		public MemoryStats Memory = new MemoryStats();

		// Token: 0x040001D6 RID: 470
		public StorageStats Storage = new StorageStats();
	}
}
