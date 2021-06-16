using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000056 RID: 86
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestMemoryInfo
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00005A33 File Offset: 0x00003C33
		public static bool IsJsonDefault(GuestMemoryInfo val)
		{
			return GuestMemoryInfo._default.JsonEquals(val);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005A40 File Offset: 0x00003C40
		public bool JsonEquals(object obj)
		{
			GuestMemoryInfo graph = obj as GuestMemoryInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestMemoryInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x040001BC RID: 444
		private static readonly GuestMemoryInfo _default = new GuestMemoryInfo();

		// Token: 0x040001BD RID: 445
		[DataMember]
		public ulong TotalPhysicalBytes;

		// Token: 0x040001BE RID: 446
		[DataMember]
		public ulong TotalUsage;

		// Token: 0x040001BF RID: 447
		[DataMember]
		public ulong CommittedBytes;

		// Token: 0x040001C0 RID: 448
		[DataMember]
		public ulong SharedCommittedBytes;

		// Token: 0x040001C1 RID: 449
		[DataMember]
		public ulong CommitLimitBytes;

		// Token: 0x040001C2 RID: 450
		[DataMember]
		public ulong PeakCommitmentBytes;
	}
}
