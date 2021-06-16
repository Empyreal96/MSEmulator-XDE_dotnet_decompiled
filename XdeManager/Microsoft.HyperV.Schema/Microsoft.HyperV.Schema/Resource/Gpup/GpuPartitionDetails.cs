using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Gpup
{
	// Token: 0x020000D0 RID: 208
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GpuPartitionDetails
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0000B528 File Offset: 0x00009728
		public static bool IsJsonDefault(GpuPartitionDetails val)
		{
			return GpuPartitionDetails._default.JsonEquals(val);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000B538 File Offset: 0x00009738
		public bool JsonEquals(object obj)
		{
			GpuPartitionDetails graph = obj as GpuPartitionDetails;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GpuPartitionDetails), new DataContractJsonSerializerSettings
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

		// Token: 0x04000402 RID: 1026
		private static readonly GpuPartitionDetails _default = new GpuPartitionDetails();

		// Token: 0x04000403 RID: 1027
		[DataMember(IsRequired = true)]
		public ushort VirtualFunctionIndex;

		// Token: 0x04000404 RID: 1028
		[DataMember]
		public long Luid;

		// Token: 0x04000405 RID: 1029
		[DataMember]
		public Guid UserModeVirtualDeviceProvider;

		// Token: 0x04000406 RID: 1030
		[DataMember]
		public ulong MinVRAM;

		// Token: 0x04000407 RID: 1031
		[DataMember]
		public ulong MaxVRAM;

		// Token: 0x04000408 RID: 1032
		[DataMember]
		public ulong CurrentVRAM;

		// Token: 0x04000409 RID: 1033
		[DataMember]
		public ulong MinEncode;

		// Token: 0x0400040A RID: 1034
		[DataMember]
		public ulong MaxEncode;

		// Token: 0x0400040B RID: 1035
		[DataMember]
		public ulong CurrentEncode;

		// Token: 0x0400040C RID: 1036
		[DataMember]
		public ulong MinDecode;

		// Token: 0x0400040D RID: 1037
		[DataMember]
		public ulong MaxDecode;

		// Token: 0x0400040E RID: 1038
		[DataMember]
		public ulong CurrentDecode;

		// Token: 0x0400040F RID: 1039
		[DataMember]
		public ulong MinCompute;

		// Token: 0x04000410 RID: 1040
		[DataMember]
		public ulong MaxCompute;

		// Token: 0x04000411 RID: 1041
		[DataMember]
		public ulong CurrentCompute;
	}
}
