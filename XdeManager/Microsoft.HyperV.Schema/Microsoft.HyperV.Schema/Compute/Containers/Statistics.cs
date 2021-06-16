using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Responses.System;

namespace HCS.Compute.Containers
{
	// Token: 0x020001A7 RID: 423
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Statistics
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0001595C File Offset: 0x00013B5C
		public static bool IsJsonDefault(Statistics val)
		{
			return Statistics._default.JsonEquals(val);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001596C File Offset: 0x00013B6C
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00015A14 File Offset: 0x00013C14
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00015A39 File Offset: 0x00013C39
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00015A5C File Offset: 0x00013C5C
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00015A99 File Offset: 0x00013C99
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00015ABB File Offset: 0x00013CBB
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00015AC3 File Offset: 0x00013CC3
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00015ACF File Offset: 0x00013CCF
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00015AD7 File Offset: 0x00013CD7
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00015AE3 File Offset: 0x00013CE3
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00015AEB File Offset: 0x00013CEB
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

		// Token: 0x04000964 RID: 2404
		private static readonly Statistics _default = new Statistics();

		// Token: 0x04000965 RID: 2405
		public DateTime Timestamp;

		// Token: 0x04000966 RID: 2406
		public DateTime ContainerStartTime;

		// Token: 0x04000967 RID: 2407
		[DataMember]
		public ulong Uptime100ns;

		// Token: 0x04000968 RID: 2408
		public ProcessorStats Processor = new ProcessorStats();

		// Token: 0x04000969 RID: 2409
		public MemoryStats Memory = new MemoryStats();

		// Token: 0x0400096A RID: 2410
		public StorageStats Storage = new StorageStats();

		// Token: 0x0400096B RID: 2411
		[DataMember]
		public object[] Network;
	}
}
