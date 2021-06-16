using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Resource.Gpup;

namespace HCS.Config.Devices.Gpup
{
	// Token: 0x02000155 RID: 341
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GpuPartition
	{
		// Token: 0x0600055D RID: 1373 RVA: 0x0001175B File Offset: 0x0000F95B
		public static bool IsJsonDefault(GpuPartition val)
		{
			return GpuPartition._default.JsonEquals(val);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00011768 File Offset: 0x0000F968
		public bool JsonEquals(object obj)
		{
			GpuPartition graph = obj as GpuPartition;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GpuPartition), new DataContractJsonSerializerSettings
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

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00011810 File Offset: 0x0000FA10
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x00011827 File Offset: 0x0000FA27
		[DataMember(EmitDefaultValue = false, Name = "Preallocation")]
		private Allocation _Preallocation
		{
			get
			{
				if (!Allocation.IsJsonDefault(this.Preallocation))
				{
					return this.Preallocation;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Preallocation = value;
				}
			}
		}

		// Token: 0x04000707 RID: 1799
		private static readonly GpuPartition _default = new GpuPartition();

		// Token: 0x04000708 RID: 1800
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000709 RID: 1801
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400070A RID: 1802
		[DataMember(Name = "InstanceGuid")]
		public Guid ChannelInstanceGuid;

		// Token: 0x0400070B RID: 1803
		[DataMember(EmitDefaultValue = false)]
		public string PoolId;

		// Token: 0x0400070C RID: 1804
		[DataMember(EmitDefaultValue = false)]
		public ulong? MinPartitionVRAM;

		// Token: 0x0400070D RID: 1805
		[DataMember(EmitDefaultValue = false)]
		public ulong? MaxPartitionVRAM;

		// Token: 0x0400070E RID: 1806
		[DataMember(EmitDefaultValue = false)]
		public ulong? OptimalPartitionVRAM;

		// Token: 0x0400070F RID: 1807
		[DataMember(EmitDefaultValue = false)]
		public ulong? MinPartitionEncode;

		// Token: 0x04000710 RID: 1808
		[DataMember(EmitDefaultValue = false)]
		public ulong? MaxPartitionEncode;

		// Token: 0x04000711 RID: 1809
		[DataMember(EmitDefaultValue = false)]
		public ulong? OptimalPartitionEncode;

		// Token: 0x04000712 RID: 1810
		[DataMember(EmitDefaultValue = false)]
		public ulong? MinPartitionDecode;

		// Token: 0x04000713 RID: 1811
		[DataMember(EmitDefaultValue = false)]
		public ulong? MaxPartitionDecode;

		// Token: 0x04000714 RID: 1812
		[DataMember(EmitDefaultValue = false)]
		public ulong? OptimalPartitionDecode;

		// Token: 0x04000715 RID: 1813
		[DataMember(EmitDefaultValue = false)]
		public ulong? MinPartitionCompute;

		// Token: 0x04000716 RID: 1814
		[DataMember(EmitDefaultValue = false)]
		public ulong? MaxPartitionCompute;

		// Token: 0x04000717 RID: 1815
		[DataMember(EmitDefaultValue = false)]
		public ulong? OptimalPartitionCompute;

		// Token: 0x04000718 RID: 1816
		public Allocation Preallocation = new Allocation();
	}
}
