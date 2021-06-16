using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000EF RID: 239
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Memory
	{
		// Token: 0x06000395 RID: 917 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
		public static bool IsJsonDefault(Memory val)
		{
			return Memory._default.JsonEquals(val);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000C800 File Offset: 0x0000AA00
		public bool JsonEquals(object obj)
		{
			Memory graph = obj as Memory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Memory), new DataContractJsonSerializerSettings
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

		// Token: 0x0400048D RID: 1165
		private static readonly Memory _default = new Memory();

		// Token: 0x0400048E RID: 1166
		[DataMember(Name = "size")]
		public ulong SizeInMB;

		// Token: 0x0400048F RID: 1167
		[DataMember(Name = "backing_type")]
		public ulong BackingType;

		// Token: 0x04000490 RID: 1168
		[DataMember(EmitDefaultValue = false, Name = "pool_id")]
		public string ResourcePoolId;

		// Token: 0x04000491 RID: 1169
		[DataMember(EmitDefaultValue = false, Name = "priority")]
		public uint? Priority;

		// Token: 0x04000492 RID: 1170
		[DataMember(EmitDefaultValue = false, Name = "dynamic_memory_enabled")]
		public bool? DMEnabled;

		// Token: 0x04000493 RID: 1171
		[DataMember(Name = "limit")]
		public ulong LimitInMB;

		// Token: 0x04000494 RID: 1172
		[DataMember(Name = "reservation")]
		public ulong ReservationInMB;

		// Token: 0x04000495 RID: 1173
		[DataMember(EmitDefaultValue = false, Name = "target_memory_buffer")]
		public uint? TargetBuffer;

		// Token: 0x04000496 RID: 1174
		[DataMember(Name = "consolidation_enabled")]
		public bool? ConsolidationEnabled;

		// Token: 0x04000497 RID: 1175
		[DataMember(EmitDefaultValue = false, Name = "working_set_low")]
		public ulong WorkingSetLowInMB;

		// Token: 0x04000498 RID: 1176
		[DataMember(EmitDefaultValue = false, Name = "working_set_high")]
		public ulong WorkingSetHighInMB;

		// Token: 0x04000499 RID: 1177
		[DataMember(EmitDefaultValue = false, Name = "private_compression_store_enabled")]
		public bool PrivateCompressionStoreEnabled;

		// Token: 0x0400049A RID: 1178
		[DataMember(EmitDefaultValue = false, Name = "deferred_commit")]
		public bool DeferredCommitEnabled;

		// Token: 0x0400049B RID: 1179
		[DataMember(EmitDefaultValue = false, Name = "fault_cluster_size_shift")]
		public ulong FaultClusterSizeShift;

		// Token: 0x0400049C RID: 1180
		[DataMember(EmitDefaultValue = false, Name = "backing_size_in_pages")]
		public ulong BackingSizeInPages;

		// Token: 0x0400049D RID: 1181
		[DataMember(EmitDefaultValue = false, Name = "backing_size_required")]
		public bool BackingSizeRequired;

		// Token: 0x0400049E RID: 1182
		[DataMember(EmitDefaultValue = false, Name = "mapping_size_in_pages")]
		public ulong MappingSizeInPages;

		// Token: 0x0400049F RID: 1183
		[DataMember(EmitDefaultValue = false, Name = "pin_backing_pages")]
		public bool PinBackingPages;

		// Token: 0x040004A0 RID: 1184
		[DataMember(EmitDefaultValue = false, Name = "image_base_addresses_exposed")]
		public bool ImageBaseAddressesExposed;
	}
}
