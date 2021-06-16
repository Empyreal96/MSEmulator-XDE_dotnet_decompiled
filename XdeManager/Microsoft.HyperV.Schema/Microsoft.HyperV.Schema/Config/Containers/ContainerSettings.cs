using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;
using HCS.Schema.DeviceAssignment;
using HCS.Schema.HvSocket;
using HCS.Schema.Registry;

namespace HCS.Config.Containers
{
	// Token: 0x02000172 RID: 370
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerSettings
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x000129C0 File Offset: 0x00010BC0
		public static bool IsJsonDefault(ContainerSettings val)
		{
			return ContainerSettings._default.JsonEquals(val);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000129D0 File Offset: 0x00010BD0
		public bool JsonEquals(object obj)
		{
			ContainerSettings graph = obj as ContainerSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00012A78 File Offset: 0x00010C78
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x00012A92 File Offset: 0x00010C92
		[DataMember(IsRequired = true, Name = "SystemType")]
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00012AC0 File Offset: 0x00010CC0
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x00012AEA File Offset: 0x00010CEA
		[DataMember(EmitDefaultValue = false, Name = "ContainerType")]
		private string _ContainerType
		{
			get
			{
				if (this.ContainerType == ContainerType.Windows)
				{
					return null;
				}
				return this.ContainerType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ContainerType = ContainerType.Windows;
				}
				this.ContainerType = (ContainerType)Enum.Parse(typeof(ContainerType), value, true);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00012B17 File Offset: 0x00010D17
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00012B2E File Offset: 0x00010D2E
		[DataMember(EmitDefaultValue = false, Name = "HvRuntime")]
		private UtilityVmSettings _HvRuntime
		{
			get
			{
				if (!UtilityVmSettings.IsJsonDefault(this.HvRuntime))
				{
					return this.HvRuntime;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.HvRuntime = value;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00012B3A File Offset: 0x00010D3A
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x00012B51 File Offset: 0x00010D51
		[DataMember(EmitDefaultValue = false, Name = "RegistryChanges")]
		private RegistryChanges _RegistryChanges
		{
			get
			{
				if (!RegistryChanges.IsJsonDefault(this.RegistryChanges))
				{
					return this.RegistryChanges;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.RegistryChanges = value;
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00012B5D File Offset: 0x00010D5D
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x00012B74 File Offset: 0x00010D74
		[DataMember(EmitDefaultValue = false, Name = "VsockStdioPortRange")]
		private VsockPortRange _VsockStdioPortRange
		{
			get
			{
				if (!VsockPortRange.IsJsonDefault(this.VsockStdioPortRange))
				{
					return this.VsockStdioPortRange;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.VsockStdioPortRange = value;
				}
			}
		}

		// Token: 0x040007C3 RID: 1987
		private static readonly ContainerSettings _default = new ContainerSettings();

		// Token: 0x040007C4 RID: 1988
		public SystemType SystemType;

		// Token: 0x040007C5 RID: 1989
		[DataMember]
		public string Name;

		// Token: 0x040007C6 RID: 1990
		[DataMember(EmitDefaultValue = false)]
		public bool HvPartition;

		// Token: 0x040007C7 RID: 1991
		[DataMember(EmitDefaultValue = false)]
		public string Owner;

		// Token: 0x040007C8 RID: 1992
		[DataMember(EmitDefaultValue = false)]
		public bool IsDummy;

		// Token: 0x040007C9 RID: 1993
		[DataMember(EmitDefaultValue = false)]
		public bool TerminateOnLastHandleClosed;

		// Token: 0x040007CA RID: 1994
		public ContainerType ContainerType;

		// Token: 0x040007CB RID: 1995
		public UtilityVmSettings HvRuntime = new UtilityVmSettings();

		// Token: 0x040007CC RID: 1996
		[DataMember(EmitDefaultValue = false)]
		public string HostName;

		// Token: 0x040007CD RID: 1997
		[DataMember(EmitDefaultValue = false)]
		public string DNSSearchList;

		// Token: 0x040007CE RID: 1998
		[DataMember(EmitDefaultValue = false)]
		public string Credentials;

		// Token: 0x040007CF RID: 1999
		public RegistryChanges RegistryChanges = new RegistryChanges();

		// Token: 0x040007D0 RID: 2000
		[DataMember(EmitDefaultValue = false)]
		public long? MemoryMaximumInMB;

		// Token: 0x040007D1 RID: 2001
		[DataMember(EmitDefaultValue = false)]
		public uint? ProcessorCount;

		// Token: 0x040007D2 RID: 2002
		[DataMember(EmitDefaultValue = false)]
		public long? ProcessorMaximum;

		// Token: 0x040007D3 RID: 2003
		[DataMember(EmitDefaultValue = false)]
		public long? ProcessorWeight;

		// Token: 0x040007D4 RID: 2004
		[DataMember(EmitDefaultValue = false)]
		public MemoryPartition MemoryPartition;

		// Token: 0x040007D5 RID: 2005
		[DataMember(EmitDefaultValue = false)]
		public long? DirectFileMappingMB;

		// Token: 0x040007D6 RID: 2006
		[DataMember(EmitDefaultValue = false)]
		public long? SharedMemoryMB;

		// Token: 0x040007D7 RID: 2007
		[DataMember(EmitDefaultValue = false)]
		public bool IgnoreFlushesDuringBoot;

		// Token: 0x040007D8 RID: 2008
		[DataMember(EmitDefaultValue = false)]
		public string SandboxPath;

		// Token: 0x040007D9 RID: 2009
		[DataMember(EmitDefaultValue = false)]
		public string VolumePath;

		// Token: 0x040007DA RID: 2010
		[DataMember(EmitDefaultValue = false)]
		public string LayerFolderPath;

		// Token: 0x040007DB RID: 2011
		[DataMember(EmitDefaultValue = false)]
		public Layer[] Layers;

		// Token: 0x040007DC RID: 2012
		[DataMember(EmitDefaultValue = false)]
		public MappedDirectory[] MappedDirectories;

		// Token: 0x040007DD RID: 2013
		[DataMember(EmitDefaultValue = false)]
		public MappedPipe[] MappedPipes;

		// Token: 0x040007DE RID: 2014
		[DataMember(EmitDefaultValue = false)]
		public MappedVirtualDisk[] MappedVirtualDisks;

		// Token: 0x040007DF RID: 2015
		[DataMember(EmitDefaultValue = false)]
		public ulong? StorageIOPSMaximum;

		// Token: 0x040007E0 RID: 2016
		[DataMember(EmitDefaultValue = false)]
		public ulong? StorageBandwidthMaximum;

		// Token: 0x040007E1 RID: 2017
		[DataMember(EmitDefaultValue = false)]
		public ulong? StorageSandboxSize;

		// Token: 0x040007E2 RID: 2018
		[DataMember(EmitDefaultValue = false)]
		public DeviceBase[] Devices;

		// Token: 0x040007E3 RID: 2019
		[DataMember(EmitDefaultValue = false)]
		public NetworkEndpoint[] NetworkEndpoints;

		// Token: 0x040007E4 RID: 2020
		[DataMember(EmitDefaultValue = false)]
		public Guid[] EndpointList;

		// Token: 0x040007E5 RID: 2021
		[DataMember(EmitDefaultValue = false)]
		public string NetworkSharedContainerName;

		// Token: 0x040007E6 RID: 2022
		[DataMember(EmitDefaultValue = false)]
		public string Namespace;

		// Token: 0x040007E7 RID: 2023
		[DataMember(EmitDefaultValue = false, Name = "AllowUnqualifiedDnsQuery")]
		public bool EnableServiceDiscovery;

		// Token: 0x040007E8 RID: 2024
		public VsockPortRange VsockStdioPortRange = new VsockPortRange();

		// Token: 0x040007E9 RID: 2025
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePowerShellDirect;

		// Token: 0x040007EA RID: 2026
		[DataMember(EmitDefaultValue = false)]
		public bool EnableUtcRelay;

		// Token: 0x040007EB RID: 2027
		[DataMember(EmitDefaultValue = false)]
		public bool EnableAuditing;

		// Token: 0x040007EC RID: 2028
		[DataMember(EmitDefaultValue = false)]
		public HvSocketSystemConfig HvSocketConfig;

		// Token: 0x040007ED RID: 2029
		[DataMember(EmitDefaultValue = false)]
		public Device[] AssignedDevices;
	}
}
