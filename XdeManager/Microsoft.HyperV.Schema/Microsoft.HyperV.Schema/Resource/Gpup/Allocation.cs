using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Gpup
{
	// Token: 0x020000D1 RID: 209
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Allocation
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000B5F4 File Offset: 0x000097F4
		public static bool IsJsonDefault(Allocation val)
		{
			return Allocation._default.JsonEquals(val);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000B604 File Offset: 0x00009804
		public bool JsonEquals(object obj)
		{
			Allocation graph = obj as Allocation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Allocation), new DataContractJsonSerializerSettings
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000B6AC File Offset: 0x000098AC
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000B6B4 File Offset: 0x000098B4
		[DataMember(Name = "PartitionDetails")]
		private GpuPartitionDetails _PartitionDetails
		{
			get
			{
				return this.PartitionDetails;
			}
			set
			{
				if (value != null)
				{
					this.PartitionDetails = value;
				}
			}
		}

		// Token: 0x04000412 RID: 1042
		private static readonly Allocation _default = new Allocation();

		// Token: 0x04000413 RID: 1043
		[DataMember]
		public string InstanceId;

		// Token: 0x04000414 RID: 1044
		[DataMember]
		public long Luid;

		// Token: 0x04000415 RID: 1045
		public GpuPartitionDetails PartitionDetails = new GpuPartitionDetails();
	}
}
