using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000060 RID: 96
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Properties
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00006370 File Offset: 0x00004570
		public static bool IsJsonDefault(Properties val)
		{
			return Properties._default.JsonEquals(val);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006380 File Offset: 0x00004580
		public bool JsonEquals(object obj)
		{
			Properties graph = obj as Properties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Properties), new DataContractJsonSerializerSettings
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006428 File Offset: 0x00004628
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00006442 File Offset: 0x00004642
		[DataMember(Name = "SystemType")]
		private string _SystemType
		{
			get
			{
				SystemType systemType = this.SystemType;
				return this.SystemType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SystemType = SystemType.Container;
				}
				this.SystemType = (SystemType)Enum.Parse(typeof(SystemType), value, true);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006470 File Offset: 0x00004670
		// (set) Token: 0x06000180 RID: 384 RVA: 0x0000649A File Offset: 0x0000469A
		[DataMember(EmitDefaultValue = false, Name = "RuntimeOsType")]
		private string _RuntimeOsType
		{
			get
			{
				if (this.RuntimeOsType == OsType.Unknown)
				{
					return null;
				}
				return this.RuntimeOsType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RuntimeOsType = OsType.Unknown;
				}
				this.RuntimeOsType = (OsType)Enum.Parse(typeof(OsType), value, true);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000064C8 File Offset: 0x000046C8
		// (set) Token: 0x06000182 RID: 386 RVA: 0x000064F2 File Offset: 0x000046F2
		[DataMember(EmitDefaultValue = false, Name = "State")]
		private string _State
		{
			get
			{
				if (this.State == State.Created)
				{
					return null;
				}
				return this.State.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.State = State.Created;
				}
				this.State = (State)Enum.Parse(typeof(State), value, true);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006520 File Offset: 0x00004720
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000654A File Offset: 0x0000474A
		[DataMember(EmitDefaultValue = false, Name = "ExitType")]
		private string _ExitType
		{
			get
			{
				if (this.ExitType == NotificationType.None)
				{
					return null;
				}
				return this.ExitType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ExitType = NotificationType.None;
				}
				this.ExitType = (NotificationType)Enum.Parse(typeof(NotificationType), value, true);
			}
		}

		// Token: 0x040001EE RID: 494
		private static readonly Properties _default = new Properties();

		// Token: 0x040001EF RID: 495
		[DataMember]
		public string Id;

		// Token: 0x040001F0 RID: 496
		public SystemType SystemType;

		// Token: 0x040001F1 RID: 497
		public OsType RuntimeOsType;

		// Token: 0x040001F2 RID: 498
		[DataMember(EmitDefaultValue = false)]
		public string Name;

		// Token: 0x040001F3 RID: 499
		[DataMember(EmitDefaultValue = false)]
		public string Owner;

		// Token: 0x040001F4 RID: 500
		[DataMember(EmitDefaultValue = false)]
		public string ObRoot;

		// Token: 0x040001F5 RID: 501
		[DataMember(EmitDefaultValue = false)]
		public bool IsDummy;

		// Token: 0x040001F6 RID: 502
		[DataMember(EmitDefaultValue = false)]
		public Guid RuntimeId;

		// Token: 0x040001F7 RID: 503
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeImagePath;

		// Token: 0x040001F8 RID: 504
		[DataMember(EmitDefaultValue = false)]
		public bool IsRuntimeTemplate;

		// Token: 0x040001F9 RID: 505
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeTemplateId;

		// Token: 0x040001FA RID: 506
		public State State;

		// Token: 0x040001FB RID: 507
		[DataMember(EmitDefaultValue = false)]
		public bool Stopped;

		// Token: 0x040001FC RID: 508
		public NotificationType ExitType;

		// Token: 0x040001FD RID: 509
		[DataMember(EmitDefaultValue = false)]
		public MemoryInformationForVm Memory;

		// Token: 0x040001FE RID: 510
		[DataMember(EmitDefaultValue = false)]
		public GuestMemoryInfo GuestMemoryInfo;

		// Token: 0x040001FF RID: 511
		[DataMember(EmitDefaultValue = false)]
		public Guid CpuGroupId;

		// Token: 0x04000200 RID: 512
		[DataMember(EmitDefaultValue = false)]
		public Statistics Statistics;

		// Token: 0x04000201 RID: 513
		[DataMember(EmitDefaultValue = false)]
		public ProcessDetails[] ProcessList;

		// Token: 0x04000202 RID: 514
		[DataMember(EmitDefaultValue = false)]
		public bool TerminateOnLastHandleClosed;

		// Token: 0x04000203 RID: 515
		[DataMember(EmitDefaultValue = false)]
		public Guid? SystemGUID;

		// Token: 0x04000204 RID: 516
		[DataMember(EmitDefaultValue = false)]
		public string HostingSystemId;

		// Token: 0x04000205 RID: 517
		[DataMember(EmitDefaultValue = false)]
		public SharedMemoryRegionInfo[] SharedMemoryRegionInfo;

		// Token: 0x04000206 RID: 518
		[DataMember(EmitDefaultValue = false)]
		public GuestConnectionInfo GuestConnectionInfo;

		// Token: 0x04000207 RID: 519
		[DataMember(EmitDefaultValue = false)]
		public SiloProperties Silo;

		// Token: 0x04000208 RID: 520
		[DataMember(EmitDefaultValue = false)]
		public uint CosIndex;

		// Token: 0x04000209 RID: 521
		[DataMember(EmitDefaultValue = false)]
		public uint Rmid;

		// Token: 0x0400020A RID: 522
		[DataMember(EmitDefaultValue = false)]
		public CacheQueryStatsResponse CacheStats;
	}
}
