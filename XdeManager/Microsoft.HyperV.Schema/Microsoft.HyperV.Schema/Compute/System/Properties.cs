using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Compute.Containers;
using HCS.Config.Containers;
using HCS.Resource.Memory;
using HCS.Schema.Responses.System;

namespace HCS.Compute.System
{
	// Token: 0x0200019F RID: 415
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Properties
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x000150B8 File Offset: 0x000132B8
		public static bool IsJsonDefault(Properties val)
		{
			return Properties._default.JsonEquals(val);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000150C8 File Offset: 0x000132C8
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00015170 File Offset: 0x00013370
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0001518A File Offset: 0x0001338A
		[DataMember(Name = "SystemType")]
		private string _SystemType
		{
			get
			{
				HCS.Config.Containers.SystemType systemType = this.SystemType;
				return this.SystemType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SystemType = HCS.Config.Containers.SystemType.Container;
				}
				this.SystemType = (HCS.Config.Containers.SystemType)Enum.Parse(typeof(HCS.Config.Containers.SystemType), value, true);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000151B8 File Offset: 0x000133B8
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000151E2 File Offset: 0x000133E2
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00015210 File Offset: 0x00013410
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x0001523A File Offset: 0x0001343A
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00015268 File Offset: 0x00013468
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x00015292 File Offset: 0x00013492
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

		// Token: 0x0400091D RID: 2333
		private static readonly Properties _default = new Properties();

		// Token: 0x0400091E RID: 2334
		[DataMember]
		public string Id;

		// Token: 0x0400091F RID: 2335
		public HCS.Config.Containers.SystemType SystemType;

		// Token: 0x04000920 RID: 2336
		public OsType RuntimeOsType;

		// Token: 0x04000921 RID: 2337
		[DataMember(EmitDefaultValue = false)]
		public string Name;

		// Token: 0x04000922 RID: 2338
		[DataMember(EmitDefaultValue = false)]
		public string Owner;

		// Token: 0x04000923 RID: 2339
		[DataMember(EmitDefaultValue = false)]
		public string ObRoot;

		// Token: 0x04000924 RID: 2340
		[DataMember(EmitDefaultValue = false)]
		public bool IsDummy;

		// Token: 0x04000925 RID: 2341
		[DataMember(EmitDefaultValue = false)]
		public Guid RuntimeId;

		// Token: 0x04000926 RID: 2342
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeImagePath;

		// Token: 0x04000927 RID: 2343
		[DataMember(EmitDefaultValue = false)]
		public bool IsRuntimeTemplate;

		// Token: 0x04000928 RID: 2344
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeTemplateId;

		// Token: 0x04000929 RID: 2345
		public State State;

		// Token: 0x0400092A RID: 2346
		[DataMember(EmitDefaultValue = false)]
		public bool Stopped;

		// Token: 0x0400092B RID: 2347
		public NotificationType ExitType;

		// Token: 0x0400092C RID: 2348
		[DataMember(EmitDefaultValue = false)]
		public MemoryInformationForVm Memory;

		// Token: 0x0400092D RID: 2349
		[DataMember(EmitDefaultValue = false)]
		public GuestMemoryInfo ContainerReportedMemory;

		// Token: 0x0400092E RID: 2350
		[DataMember(EmitDefaultValue = false)]
		public Guid CpuGroupId;

		// Token: 0x0400092F RID: 2351
		[DataMember(EmitDefaultValue = false)]
		public HCS.Compute.Containers.Statistics Statistics;

		// Token: 0x04000930 RID: 2352
		[DataMember(EmitDefaultValue = false)]
		public ProcessDetails[] ProcessList;

		// Token: 0x04000931 RID: 2353
		[DataMember(EmitDefaultValue = false)]
		public bool TerminateOnLastHandleClosed;

		// Token: 0x04000932 RID: 2354
		[DataMember(EmitDefaultValue = false)]
		public Guid? SystemGUID;

		// Token: 0x04000933 RID: 2355
		[DataMember(EmitDefaultValue = false)]
		public string HostingSystemId;

		// Token: 0x04000934 RID: 2356
		[DataMember(EmitDefaultValue = false)]
		public HCS.Resource.Memory.SharedMemoryRegionInfo[] SharedMemoryRegionInfo;

		// Token: 0x04000935 RID: 2357
		[DataMember(EmitDefaultValue = false)]
		public GuestConnectionInfo GuestConnectionInfo;

		// Token: 0x04000936 RID: 2358
		[DataMember(EmitDefaultValue = false)]
		public SiloProperties Silo;

		// Token: 0x04000937 RID: 2359
		[DataMember(EmitDefaultValue = false)]
		public uint CosIndex;

		// Token: 0x04000938 RID: 2360
		[DataMember(EmitDefaultValue = false)]
		public uint Rmid;

		// Token: 0x04000939 RID: 2361
		[DataMember(EmitDefaultValue = false)]
		public CacheQueryStatsResponse CacheStats;

		// Token: 0x0400093A RID: 2362
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<byte, MappedVirtualDiskController> MappedVirtualDiskControllers;
	}
}
