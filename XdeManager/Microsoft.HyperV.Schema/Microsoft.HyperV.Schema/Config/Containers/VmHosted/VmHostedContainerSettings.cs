using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;
using HCS.Schema.Containers.CredentialGuard;
using HCS.Schema.Registry;
using HCS.Schema.VirtualMachines.Resources;

namespace HCS.Config.Containers.VmHosted
{
	// Token: 0x02000185 RID: 389
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VmHostedContainerSettings
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x00013D7C File Offset: 0x00011F7C
		public static bool IsJsonDefault(VmHostedContainerSettings val)
		{
			return VmHostedContainerSettings._default.JsonEquals(val);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00013D8C File Offset: 0x00011F8C
		public bool JsonEquals(object obj)
		{
			VmHostedContainerSettings graph = obj as VmHostedContainerSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VmHostedContainerSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00013E34 File Offset: 0x00012034
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x00013E4E File Offset: 0x0001204E
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00013E7B File Offset: 0x0001207B
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x00013E92 File Offset: 0x00012092
		[DataMember(EmitDefaultValue = false, Name = "TimeZoneInformation")]
		private TimeZoneInformation _TimeZoneInformation
		{
			get
			{
				if (!TimeZoneInformation.IsJsonDefault(this.TimeZoneInformation))
				{
					return this.TimeZoneInformation;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.TimeZoneInformation = value;
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00013E9E File Offset: 0x0001209E
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x00013EB5 File Offset: 0x000120B5
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00013EC1 File Offset: 0x000120C1
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x00013ED8 File Offset: 0x000120D8
		[DataMember(EmitDefaultValue = false, Name = "HvSocketAddress")]
		private HvSocketAddress _HvSocketAddress
		{
			get
			{
				if (!HvSocketAddress.IsJsonDefault(this.HvSocketAddress))
				{
					return this.HvSocketAddress;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.HvSocketAddress = value;
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00013EE4 File Offset: 0x000120E4
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00013EFB File Offset: 0x000120FB
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

		// Token: 0x0400084D RID: 2125
		private static readonly VmHostedContainerSettings _default = new VmHostedContainerSettings();

		// Token: 0x0400084E RID: 2126
		public SystemType SystemType;

		// Token: 0x0400084F RID: 2127
		[DataMember]
		public string Name;

		// Token: 0x04000850 RID: 2128
		[DataMember(EmitDefaultValue = false)]
		public bool HvPartition;

		// Token: 0x04000851 RID: 2129
		[DataMember]
		public string HostName;

		// Token: 0x04000852 RID: 2130
		[DataMember(EmitDefaultValue = false)]
		public string DNSSearchList;

		// Token: 0x04000853 RID: 2131
		[DataMember]
		public byte[] CcgCookie;

		// Token: 0x04000854 RID: 2132
		[DataMember(EmitDefaultValue = false)]
		public ContainerCredentialGuardState CredentialGuard;

		// Token: 0x04000855 RID: 2133
		[DataMember]
		public uint SandboxVolumeLun;

		// Token: 0x04000856 RID: 2134
		[DataMember]
		public string SandboxDataPath;

		// Token: 0x04000857 RID: 2135
		[DataMember(EmitDefaultValue = false)]
		public ulong? SandboxSize;

		// Token: 0x04000858 RID: 2136
		[DataMember]
		public Layer[] Layers;

		// Token: 0x04000859 RID: 2137
		[DataMember]
		public bool IgnoreFlushesDuringBoot;

		// Token: 0x0400085A RID: 2138
		[DataMember(EmitDefaultValue = false)]
		public bool LayersUseVPMEM;

		// Token: 0x0400085B RID: 2139
		[DataMember(EmitDefaultValue = false)]
		public NetworkAdapter[] NetworkAdapters;

		// Token: 0x0400085C RID: 2140
		[DataMember(EmitDefaultValue = false)]
		public MappedDirectory[] MappedDirectories;

		// Token: 0x0400085D RID: 2141
		[DataMember(EmitDefaultValue = false)]
		public MappedPipe[] MappedPipes;

		// Token: 0x0400085E RID: 2142
		[DataMember(EmitDefaultValue = false)]
		public MappedVirtualDisk[] MappedVirtualDisks;

		// Token: 0x0400085F RID: 2143
		public TimeZoneInformation TimeZoneInformation = new TimeZoneInformation();

		// Token: 0x04000860 RID: 2144
		[DataMember(EmitDefaultValue = false)]
		public long? MemoryMaximumInMB;

		// Token: 0x04000861 RID: 2145
		[DataMember(EmitDefaultValue = false, Name = "AllowUnqualifiedDnsQuery")]
		public bool EnableServiceDiscovery;

		// Token: 0x04000862 RID: 2146
		[DataMember(EmitDefaultValue = false)]
		public string Namespace;

		// Token: 0x04000863 RID: 2147
		[DataMember(EmitDefaultValue = false)]
		public bool EnableDefender;

		// Token: 0x04000864 RID: 2148
		[DataMember(EmitDefaultValue = false)]
		public string DefenderDefinitionPath;

		// Token: 0x04000865 RID: 2149
		public VsockPortRange VsockStdioPortRange = new VsockPortRange();

		// Token: 0x04000866 RID: 2150
		public HvSocketAddress HvSocketAddress = new HvSocketAddress();

		// Token: 0x04000867 RID: 2151
		public RegistryChanges RegistryChanges = new RegistryChanges();
	}
}
