using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005B RID: 91
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessDetails
	{
		// Token: 0x06000165 RID: 357 RVA: 0x00005F30 File Offset: 0x00004130
		public static bool IsJsonDefault(ProcessDetails val)
		{
			return ProcessDetails._default.JsonEquals(val);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005F40 File Offset: 0x00004140
		public bool JsonEquals(object obj)
		{
			ProcessDetails graph = obj as ProcessDetails;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessDetails), new DataContractJsonSerializerSettings
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00005FE8 File Offset: 0x000041E8
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000600D File Offset: 0x0000420D
		[DataMember(Name = "CreateTimestamp")]
		private string _CreateTimestamp
		{
			get
			{
				return this.CreateTimestamp.ToUniversalTime().ToString("o");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.CreateTimestamp = default(DateTime);
				}
				this.CreateTimestamp = DateTime.Parse(value);
			}
		}

		// Token: 0x040001D7 RID: 471
		private static readonly ProcessDetails _default = new ProcessDetails();

		// Token: 0x040001D8 RID: 472
		[DataMember]
		public uint ProcessId;

		// Token: 0x040001D9 RID: 473
		[DataMember]
		public string ImageName;

		// Token: 0x040001DA RID: 474
		public DateTime CreateTimestamp;

		// Token: 0x040001DB RID: 475
		[DataMember]
		public ulong UserTime100ns;

		// Token: 0x040001DC RID: 476
		[DataMember]
		public ulong KernelTime100ns;

		// Token: 0x040001DD RID: 477
		[DataMember]
		public ulong MemoryCommitBytes;

		// Token: 0x040001DE RID: 478
		[DataMember]
		public ulong MemoryWorkingSetPrivateBytes;

		// Token: 0x040001DF RID: 479
		[DataMember]
		public ulong MemoryWorkingSetSharedBytes;
	}
}
