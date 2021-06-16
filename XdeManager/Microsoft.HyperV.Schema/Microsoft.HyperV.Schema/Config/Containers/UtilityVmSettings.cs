using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.VirtualMachines.Resources.Gpu;

namespace HCS.Config.Containers
{
	// Token: 0x0200016D RID: 365
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class UtilityVmSettings
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x00012488 File Offset: 0x00010688
		public static bool IsJsonDefault(UtilityVmSettings val)
		{
			return UtilityVmSettings._default.JsonEquals(val);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00012498 File Offset: 0x00010698
		public bool JsonEquals(object obj)
		{
			UtilityVmSettings graph = obj as UtilityVmSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(UtilityVmSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00012540 File Offset: 0x00010740
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00012557 File Offset: 0x00010757
		[DataMember(EmitDefaultValue = false, Name = "NetworkSettings")]
		private UtilityVmNetworkSettings _NetworkSettings
		{
			get
			{
				if (!UtilityVmNetworkSettings.IsJsonDefault(this.NetworkSettings))
				{
					return this.NetworkSettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.NetworkSettings = value;
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00012563 File Offset: 0x00010763
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x0001257A File Offset: 0x0001077A
		[DataMember(EmitDefaultValue = false, Name = "GpuSettings")]
		private GpuConfiguration _GpuSettings
		{
			get
			{
				if (!GpuConfiguration.IsJsonDefault(this.GpuSettings))
				{
					return this.GpuSettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.GpuSettings = value;
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00012588 File Offset: 0x00010788
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x000125B2 File Offset: 0x000107B2
		[DataMember(EmitDefaultValue = false, Name = "BootSource")]
		private string _BootSource
		{
			get
			{
				if (this.BootSource == UtilityVmBootSource.Vmbfs)
				{
					return null;
				}
				return this.BootSource.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.BootSource = UtilityVmBootSource.Vmbfs;
				}
				this.BootSource = (UtilityVmBootSource)Enum.Parse(typeof(UtilityVmBootSource), value, true);
			}
		}

		// Token: 0x04000793 RID: 1939
		private static readonly UtilityVmSettings _default = new UtilityVmSettings();

		// Token: 0x04000794 RID: 1940
		[DataMember(EmitDefaultValue = false)]
		public string ImagePath;

		// Token: 0x04000795 RID: 1941
		[DataMember(EmitDefaultValue = false)]
		public string LinuxInitrdFile;

		// Token: 0x04000796 RID: 1942
		[DataMember(EmitDefaultValue = false)]
		public string LinuxKernelFile;

		// Token: 0x04000797 RID: 1943
		[DataMember(EmitDefaultValue = false)]
		public string LinuxBootParameters;

		// Token: 0x04000798 RID: 1944
		[DataMember(EmitDefaultValue = false)]
		public string LinuxRootDiskPath;

		// Token: 0x04000799 RID: 1945
		[DataMember(EmitDefaultValue = false)]
		public Guid RuntimeId;

		// Token: 0x0400079A RID: 1946
		[DataMember(EmitDefaultValue = false)]
		public bool SkipTemplate;

		// Token: 0x0400079B RID: 1947
		[DataMember(EmitDefaultValue = false)]
		public bool EnableConsole;

		// Token: 0x0400079C RID: 1948
		public UtilityVmNetworkSettings NetworkSettings = new UtilityVmNetworkSettings();

		// Token: 0x0400079D RID: 1949
		[DataMember(EmitDefaultValue = false)]
		public string Com1PipeName;

		// Token: 0x0400079E RID: 1950
		[DataMember(EmitDefaultValue = false)]
		public string Com2PipeName;

		// Token: 0x0400079F RID: 1951
		[DataMember(EmitDefaultValue = false)]
		public bool EnableUefiDebugger;

		// Token: 0x040007A0 RID: 1952
		[DataMember(EmitDefaultValue = false)]
		public bool NoDirectMapOsShare;

		// Token: 0x040007A1 RID: 1953
		[DataMember(EmitDefaultValue = false)]
		public bool NoDirectMapContainerLayerShares;

		// Token: 0x040007A2 RID: 1954
		[DataMember(EmitDefaultValue = false)]
		public bool NoOplocksMappedDirectories;

		// Token: 0x040007A3 RID: 1955
		[DataMember(EmitDefaultValue = false)]
		public bool EnableRdp;

		// Token: 0x040007A4 RID: 1956
		[DataMember(EmitDefaultValue = false)]
		public string[] RdpAccessSids;

		// Token: 0x040007A5 RID: 1957
		public GpuConfiguration GpuSettings = new GpuConfiguration();

		// Token: 0x040007A6 RID: 1958
		[DataMember(EmitDefaultValue = false)]
		public bool SynchronizeQPC;

		// Token: 0x040007A7 RID: 1959
		[DataMember(EmitDefaultValue = false)]
		public bool BootFromLayers;

		// Token: 0x040007A8 RID: 1960
		[DataMember(EmitDefaultValue = false)]
		public bool EnableWindowsDefender;

		// Token: 0x040007A9 RID: 1961
		[DataMember(EmitDefaultValue = false)]
		public bool? EnableMemoryHotHint;

		// Token: 0x040007AA RID: 1962
		[DataMember(EmitDefaultValue = false)]
		public bool? EnableMemoryColdHint;

		// Token: 0x040007AB RID: 1963
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePrivateMemoryCompressionStore;

		// Token: 0x040007AC RID: 1964
		[DataMember(EmitDefaultValue = false)]
		public bool EnableDeferredCommit;

		// Token: 0x040007AD RID: 1965
		[DataMember(EmitDefaultValue = false)]
		public bool EnableEpf;

		// Token: 0x040007AE RID: 1966
		[DataMember(EmitDefaultValue = false)]
		public bool EnableSchedulerAssist;

		// Token: 0x040007AF RID: 1967
		public UtilityVmBootSource BootSource;

		// Token: 0x040007B0 RID: 1968
		[DataMember(EmitDefaultValue = false)]
		public bool EnableBattery;

		// Token: 0x040007B1 RID: 1969
		[DataMember(EmitDefaultValue = false)]
		public bool EnableLicensing;

		// Token: 0x040007B2 RID: 1970
		[DataMember(EmitDefaultValue = false)]
		public bool WritableBootSource;

		// Token: 0x040007B3 RID: 1971
		[DataMember(EmitDefaultValue = false)]
		public bool NoDynamicMemoryVirtualDevice;

		// Token: 0x040007B4 RID: 1972
		[DataMember(EmitDefaultValue = false)]
		public string BugcheckSavedStateFileName;
	}
}
