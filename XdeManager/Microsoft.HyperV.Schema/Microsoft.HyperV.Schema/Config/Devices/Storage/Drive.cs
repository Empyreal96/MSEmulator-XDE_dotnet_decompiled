using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012A RID: 298
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Drive
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0000F536 File Offset: 0x0000D736
		public static bool IsJsonDefault(Drive val)
		{
			return Drive._default.JsonEquals(val);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000F544 File Offset: 0x0000D744
		public bool JsonEquals(object obj)
		{
			Drive graph = obj as Drive;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Drive), new DataContractJsonSerializerSettings
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0000F622 File Offset: 0x0000D822
		[DataMember(EmitDefaultValue = false, Name = "type")]
		private string _Type
		{
			get
			{
				AttachmentType type = this.Type;
				if (type == AttachmentType.None)
				{
					return null;
				}
				if (type != AttachmentType.None_Holder)
				{
					return this.Type.ToString();
				}
				return "NONE";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = AttachmentType.None;
				}
				this.Type = (AttachmentType)Enum.Parse(typeof(AttachmentType), value, true);
			}
		}

		// Token: 0x04000610 RID: 1552
		private static readonly Drive _default = new Drive();

		// Token: 0x04000611 RID: 1553
		[DataMember(EmitDefaultValue = false, Name = "pathname")]
		public string PathName;

		// Token: 0x04000612 RID: 1554
		[DataMember(EmitDefaultValue = false, Name = "relpathname")]
		public string RelPathName;

		// Token: 0x04000613 RID: 1555
		public AttachmentType Type;

		// Token: 0x04000614 RID: 1556
		[DataMember(EmitDefaultValue = false, Name = "pool_id")]
		public string PoolId;

		// Token: 0x04000615 RID: 1557
		[DataMember(EmitDefaultValue = false, Name = "iops_limit")]
		public long? IopsLimit;

		// Token: 0x04000616 RID: 1558
		[DataMember(EmitDefaultValue = false, Name = "iops_reservation")]
		public long? IopsReservation;

		// Token: 0x04000617 RID: 1559
		[DataMember(EmitDefaultValue = false, Name = "weight")]
		public long? Weight;

		// Token: 0x04000618 RID: 1560
		[DataMember(EmitDefaultValue = false, Name = "qos_policy_id")]
		public Guid QosPolicyId;

		// Token: 0x04000619 RID: 1561
		[DataMember(EmitDefaultValue = false, Name = "persistent_reservations_supported")]
		public bool PersistentReservationsSupported;

		// Token: 0x0400061A RID: 1562
		[DataMember(EmitDefaultValue = false, Name = "storage_subsystem_type")]
		public string StorageSubsystemType;

		// Token: 0x0400061B RID: 1563
		[DataMember(EmitDefaultValue = false, Name = "snapshot_id")]
		public Guid SnapshotId;

		// Token: 0x0400061C RID: 1564
		[DataMember(EmitDefaultValue = false, Name = "caching_mode")]
		public CachingMode CachingMode;

		// Token: 0x0400061D RID: 1565
		[DataMember(EmitDefaultValue = false, Name = "ignore_flushes")]
		public bool IgnoreFlushes;

		// Token: 0x0400061E RID: 1566
		[DataMember(EmitDefaultValue = false, Name = "disable_expansion_optimization")]
		public bool DisableExpansionOptimization;

		// Token: 0x0400061F RID: 1567
		[DataMember(EmitDefaultValue = false, Name = "ignore_relative_locator")]
		public bool IgnoreRelativeLocator;

		// Token: 0x04000620 RID: 1568
		[DataMember(EmitDefaultValue = false, Name = "no_write_hardening")]
		public bool NoWriteHardening;

		// Token: 0x04000621 RID: 1569
		[DataMember(EmitDefaultValue = false, Name = "write_hardening_method")]
		public WriteHardening WriteHardeningMethod;

		// Token: 0x04000622 RID: 1570
		[DataMember(EmitDefaultValue = false, Name = "capture_io_attribution_context")]
		public bool CaptureIoAttributionContext;

		// Token: 0x04000623 RID: 1571
		[DataMember(EmitDefaultValue = false, Name = "logfile")]
		public string LogFileName;

		// Token: 0x04000624 RID: 1572
		[DataMember(EmitDefaultValue = false, Name = "read_only")]
		public bool ReadOnly;
	}
}
