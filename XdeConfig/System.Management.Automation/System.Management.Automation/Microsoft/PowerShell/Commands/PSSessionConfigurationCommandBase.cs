using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security.AccessControl;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000317 RID: 791
	public class PSSessionConfigurationCommandBase : PSCmdlet
	{
		// Token: 0x060025AF RID: 9647 RVA: 0x000D2596 File Offset: 0x000D0796
		internal static string GetLocalSddl()
		{
			if (!(Environment.OSVersion.Version >= new Version(6, 2)))
			{
				return "O:NSG:BAD:P(D;;GA;;;NU)(A;;GA;;;BA)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";
			}
			return "O:NSG:BAD:P(D;;GA;;;NU)(A;;GA;;;BA)(A;;GA;;;RM)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000D25BB File Offset: 0x000D07BB
		internal static string GetRemoteSddl()
		{
			if (!(Environment.OSVersion.Version >= new Version(6, 2)))
			{
				return "O:NSG:BAD:P(A;;GA;;;BA)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";
			}
			return "O:NSG:BAD:P(A;;GA;;;BA)(A;;GA;;;RM)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000D25E0 File Offset: 0x000D07E0
		internal static void CheckPSVersion(Version version)
		{
			if (version != null && (version.Major < 2 || version.Major > PSVersionInfo.PSVersion.Major || version.Minor != 0))
			{
				throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.PSVersionParameterOutOfRange, new object[]
				{
					version,
					"PSVersion"
				}));
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000D263D File Offset: 0x000D083D
		// (set) Token: 0x060025B3 RID: 9651 RVA: 0x000D2645 File Offset: 0x000D0845
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "AssemblyNameParameterSet")]
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "NameParameterSet")]
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "SessionConfigurationFile")]
		public string Name
		{
			get
			{
				return this.shellName;
			}
			set
			{
				this.shellName = value;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000D264E File Offset: 0x000D084E
		// (set) Token: 0x060025B5 RID: 9653 RVA: 0x000D2656 File Offset: 0x000D0856
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "AssemblyNameParameterSet")]
		public string AssemblyName
		{
			get
			{
				return this.assemblyName;
			}
			set
			{
				this.assemblyName = value;
				this.isAssemblyNameSpecified = true;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000D2666 File Offset: 0x000D0866
		// (set) Token: 0x060025B7 RID: 9655 RVA: 0x000D266E File Offset: 0x000D086E
		[Parameter(ParameterSetName = "AssemblyNameParameterSet")]
		[Parameter(ParameterSetName = "NameParameterSet")]
		public string ApplicationBase
		{
			get
			{
				return this.applicationBase;
			}
			set
			{
				this.applicationBase = value;
				this.isApplicationBaseSpecified = true;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000D267E File Offset: 0x000D087E
		// (set) Token: 0x060025B9 RID: 9657 RVA: 0x000D2686 File Offset: 0x000D0886
		[Parameter(Position = 2, Mandatory = true, ParameterSetName = "AssemblyNameParameterSet")]
		public string ConfigurationTypeName
		{
			get
			{
				return this.configurationTypeName;
			}
			set
			{
				this.configurationTypeName = value;
				this.isConfigurationTypeNameSpecified = true;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000D2696 File Offset: 0x000D0896
		// (set) Token: 0x060025BB RID: 9659 RVA: 0x000D269E File Offset: 0x000D089E
		[Credential]
		[Parameter]
		public PSCredential RunAsCredential
		{
			get
			{
				return this.runAsCredential;
			}
			set
			{
				this.runAsCredential = value;
				this.isRunAsCredentialSpecified = true;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000D26AE File Offset: 0x000D08AE
		// (set) Token: 0x060025BD RID: 9661 RVA: 0x000D26CA File Offset: 0x000D08CA
		[Parameter]
		public ApartmentState ThreadApartmentState
		{
			get
			{
				if (this.threadAptState != null)
				{
					return this.threadAptState.Value;
				}
				return ApartmentState.Unknown;
			}
			set
			{
				this.threadAptState = new ApartmentState?(value);
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000D26D8 File Offset: 0x000D08D8
		// (set) Token: 0x060025BF RID: 9663 RVA: 0x000D26F4 File Offset: 0x000D08F4
		[Parameter]
		public PSThreadOptions ThreadOptions
		{
			get
			{
				if (this.threadOptions != null)
				{
					return this.threadOptions.Value;
				}
				return PSThreadOptions.UseCurrentThread;
			}
			set
			{
				this.threadOptions = new PSThreadOptions?(value);
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000D2702 File Offset: 0x000D0902
		// (set) Token: 0x060025C1 RID: 9665 RVA: 0x000D270A File Offset: 0x000D090A
		[Parameter]
		public PSSessionConfigurationAccessMode AccessMode
		{
			get
			{
				return this.accessMode;
			}
			set
			{
				this.accessMode = value;
				this.accessModeSpecified = true;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x000D271A File Offset: 0x000D091A
		// (set) Token: 0x060025C3 RID: 9667 RVA: 0x000D2727 File Offset: 0x000D0927
		[Parameter]
		public SwitchParameter UseSharedProcess
		{
			get
			{
				return this.useSharedProcess;
			}
			set
			{
				this.useSharedProcess = value;
				this.isUseSharedProcessSpecified = true;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000D273C File Offset: 0x000D093C
		// (set) Token: 0x060025C5 RID: 9669 RVA: 0x000D2744 File Offset: 0x000D0944
		[Parameter]
		public string StartupScript
		{
			get
			{
				return this.configurationScript;
			}
			set
			{
				this.configurationScript = value;
				this.isConfigurationScriptSpecified = true;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000D2754 File Offset: 0x000D0954
		// (set) Token: 0x060025C7 RID: 9671 RVA: 0x000D275C File Offset: 0x000D095C
		[AllowNull]
		[Parameter]
		public double? MaximumReceivedDataSizePerCommandMB
		{
			get
			{
				return this.maxCommandSizeMB;
			}
			set
			{
				if (value != null && value.Value < 0.0)
				{
					throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.CSCDoubleParameterOutOfRange, new object[]
					{
						value.Value,
						"MaximumReceivedDataSizePerCommandMB"
					}));
				}
				this.maxCommandSizeMB = value;
				this.isMaxCommandSizeMBSpecified = true;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060025C8 RID: 9672 RVA: 0x000D27C1 File Offset: 0x000D09C1
		// (set) Token: 0x060025C9 RID: 9673 RVA: 0x000D27CC File Offset: 0x000D09CC
		[AllowNull]
		[Parameter]
		public double? MaximumReceivedObjectSizeMB
		{
			get
			{
				return this.maxObjectSizeMB;
			}
			set
			{
				if (value != null && value.Value < 0.0)
				{
					throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.CSCDoubleParameterOutOfRange, new object[]
					{
						value.Value,
						"MaximumReceivedObjectSizeMB"
					}));
				}
				this.maxObjectSizeMB = value;
				this.isMaxObjectSizeMBSpecified = true;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x000D2831 File Offset: 0x000D0A31
		// (set) Token: 0x060025CB RID: 9675 RVA: 0x000D283C File Offset: 0x000D0A3C
		[Parameter]
		public string SecurityDescriptorSddl
		{
			get
			{
				return this.sddl;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && new CommonSecurityDescriptor(false, false, value) == null)
				{
					throw new NotSupportedException();
				}
				this.sddl = value;
				this.isSddlSpecified = true;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x000D2871 File Offset: 0x000D0A71
		// (set) Token: 0x060025CD RID: 9677 RVA: 0x000D287E File Offset: 0x000D0A7E
		[Parameter]
		public SwitchParameter ShowSecurityDescriptorUI
		{
			get
			{
				return this.showUI;
			}
			set
			{
				this.showUI = value;
				this.showUISpecified = true;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060025CE RID: 9678 RVA: 0x000D2893 File Offset: 0x000D0A93
		// (set) Token: 0x060025CF RID: 9679 RVA: 0x000D28A0 File Offset: 0x000D0AA0
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return this.force;
			}
			set
			{
				this.force = value;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x000D28AE File Offset: 0x000D0AAE
		// (set) Token: 0x060025D1 RID: 9681 RVA: 0x000D28BB File Offset: 0x000D0ABB
		[Parameter]
		public SwitchParameter NoServiceRestart
		{
			get
			{
				return this.noRestart;
			}
			set
			{
				this.noRestart = value;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060025D2 RID: 9682 RVA: 0x000D28C9 File Offset: 0x000D0AC9
		// (set) Token: 0x060025D3 RID: 9683 RVA: 0x000D28D1 File Offset: 0x000D0AD1
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "AssemblyNameParameterSet")]
		[Alias(new string[]
		{
			"PowerShellVersion"
		})]
		public Version PSVersion
		{
			get
			{
				return this.psVersion;
			}
			set
			{
				PSSessionConfigurationCommandBase.CheckPSVersion(value);
				PSSessionConfigurationCommandUtilities.CheckIfPowerShellVersionIsInstalled(value);
				this.psVersion = value;
				this.isPSVersionSpecified = true;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x000D28ED File Offset: 0x000D0AED
		// (set) Token: 0x060025D5 RID: 9685 RVA: 0x000D28F5 File Offset: 0x000D0AF5
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "AssemblyNameParameterSet")]
		public PSSessionTypeOption SessionTypeOption
		{
			get
			{
				return this.sessionTypeOption;
			}
			set
			{
				this.sessionTypeOption = value;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000D28FE File Offset: 0x000D0AFE
		// (set) Token: 0x060025D7 RID: 9687 RVA: 0x000D2906 File Offset: 0x000D0B06
		[Parameter]
		public PSTransportOption TransportOption
		{
			get
			{
				return this.transportOption;
			}
			set
			{
				this.transportOption = value;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x000D290F File Offset: 0x000D0B0F
		// (set) Token: 0x060025D9 RID: 9689 RVA: 0x000D2918 File Offset: 0x000D0B18
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "AssemblyNameParameterSet")]
		public object[] ModulesToImport
		{
			get
			{
				return this.modulesToImport;
			}
			set
			{
				List<object> list = new List<object>();
				if (value != null)
				{
					for (int i = 0; i < value.Length; i++)
					{
						object obj = value[i];
						Hashtable hashtable = obj as Hashtable;
						if (hashtable != null)
						{
							ModuleSpecification moduleSpecification = new ModuleSpecification(hashtable);
							if (moduleSpecification != null)
							{
								list.Add(moduleSpecification);
							}
						}
						else
						{
							string text = obj.ToString();
							if (!string.IsNullOrEmpty(text.Trim()))
							{
								if ((text.Contains("\\") || text.Contains(":")) && !Directory.Exists(text) && !File.Exists(text))
								{
									throw new ArgumentException(StringUtil.Format(RemotingErrorIdStrings.InvalidRegisterPSSessionConfigurationModulePath, text));
								}
								list.Add(text);
							}
						}
					}
				}
				this.modulesToImport = list.ToArray();
				this.modulePathSpecified = true;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x000D29E1 File Offset: 0x000D0BE1
		// (set) Token: 0x060025DB RID: 9691 RVA: 0x000D29E9 File Offset: 0x000D0BE9
		[Parameter(Mandatory = true, ParameterSetName = "SessionConfigurationFile")]
		[ValidateNotNullOrEmpty]
		public string Path
		{
			get
			{
				return this.configPath;
			}
			set
			{
				this.configPath = value;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x000D29F2 File Offset: 0x000D0BF2
		// (set) Token: 0x060025DD RID: 9693 RVA: 0x000D29FA File Offset: 0x000D0BFA
		protected bool RunAsVirtualAccount
		{
			get
			{
				return this.runAsVirtualAccount;
			}
			set
			{
				this.runAsVirtualAccount = value;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x000D2A03 File Offset: 0x000D0C03
		// (set) Token: 0x060025DF RID: 9695 RVA: 0x000D2A0B File Offset: 0x000D0C0B
		protected bool RunAsVirtualAccountSpecified
		{
			get
			{
				return this.runAsVirtualAccountSpecified;
			}
			set
			{
				this.runAsVirtualAccountSpecified = value;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000D2A14 File Offset: 0x000D0C14
		// (set) Token: 0x060025E1 RID: 9697 RVA: 0x000D2A1C File Offset: 0x000D0C1C
		protected string RunAsVirtualAccountGroups { get; set; }

		// Token: 0x060025E2 RID: 9698 RVA: 0x000D2A25 File Offset: 0x000D0C25
		internal PSSessionConfigurationCommandBase()
		{
		}

		// Token: 0x04001270 RID: 4720
		internal const string NameParameterSetName = "NameParameterSet";

		// Token: 0x04001271 RID: 4721
		internal const string AssemblyNameParameterSetName = "AssemblyNameParameterSet";

		// Token: 0x04001272 RID: 4722
		internal const string SessionConfigurationFileParameterSetName = "SessionConfigurationFile";

		// Token: 0x04001273 RID: 4723
		private const string localSDDL = "O:NSG:BAD:P(D;;GA;;;NU)(A;;GA;;;BA)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";

		// Token: 0x04001274 RID: 4724
		private const string localSDDL_Win8 = "O:NSG:BAD:P(D;;GA;;;NU)(A;;GA;;;BA)(A;;GA;;;RM)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";

		// Token: 0x04001275 RID: 4725
		private const string remoteSDDL = "O:NSG:BAD:P(A;;GA;;;BA)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";

		// Token: 0x04001276 RID: 4726
		private const string remoteSDDL_Win8 = "O:NSG:BAD:P(A;;GA;;;BA)(A;;GA;;;RM)(A;;GA;;;IU)S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)";

		// Token: 0x04001277 RID: 4727
		internal const string RemoteManagementUsersSID = "S-1-5-32-580";

		// Token: 0x04001278 RID: 4728
		internal const string InteractiveUsersSID = "S-1-5-4";

		// Token: 0x04001279 RID: 4729
		internal Version MaxPSVersion;

		// Token: 0x0400127A RID: 4730
		internal string shellName;

		// Token: 0x0400127B RID: 4731
		internal string assemblyName;

		// Token: 0x0400127C RID: 4732
		internal bool isAssemblyNameSpecified;

		// Token: 0x0400127D RID: 4733
		internal string applicationBase;

		// Token: 0x0400127E RID: 4734
		internal bool isApplicationBaseSpecified;

		// Token: 0x0400127F RID: 4735
		internal string configurationTypeName;

		// Token: 0x04001280 RID: 4736
		internal bool isConfigurationTypeNameSpecified;

		// Token: 0x04001281 RID: 4737
		internal PSCredential runAsCredential;

		// Token: 0x04001282 RID: 4738
		internal bool isRunAsCredentialSpecified;

		// Token: 0x04001283 RID: 4739
		internal ApartmentState? threadAptState;

		// Token: 0x04001284 RID: 4740
		internal PSThreadOptions? threadOptions;

		// Token: 0x04001285 RID: 4741
		private PSSessionConfigurationAccessMode accessMode = PSSessionConfigurationAccessMode.Remote;

		// Token: 0x04001286 RID: 4742
		internal bool accessModeSpecified;

		// Token: 0x04001287 RID: 4743
		private bool useSharedProcess;

		// Token: 0x04001288 RID: 4744
		internal bool isUseSharedProcessSpecified;

		// Token: 0x04001289 RID: 4745
		internal string configurationScript;

		// Token: 0x0400128A RID: 4746
		internal bool isConfigurationScriptSpecified;

		// Token: 0x0400128B RID: 4747
		internal double? maxCommandSizeMB;

		// Token: 0x0400128C RID: 4748
		internal bool isMaxCommandSizeMBSpecified;

		// Token: 0x0400128D RID: 4749
		internal double? maxObjectSizeMB;

		// Token: 0x0400128E RID: 4750
		internal bool isMaxObjectSizeMBSpecified;

		// Token: 0x0400128F RID: 4751
		internal string sddl;

		// Token: 0x04001290 RID: 4752
		internal bool isSddlSpecified;

		// Token: 0x04001291 RID: 4753
		private bool showUI;

		// Token: 0x04001292 RID: 4754
		internal bool showUISpecified;

		// Token: 0x04001293 RID: 4755
		internal bool force;

		// Token: 0x04001294 RID: 4756
		internal bool noRestart;

		// Token: 0x04001295 RID: 4757
		internal Version psVersion;

		// Token: 0x04001296 RID: 4758
		internal bool isPSVersionSpecified;

		// Token: 0x04001297 RID: 4759
		internal PSSessionTypeOption sessionTypeOption;

		// Token: 0x04001298 RID: 4760
		internal PSTransportOption transportOption;

		// Token: 0x04001299 RID: 4761
		internal object[] modulesToImport;

		// Token: 0x0400129A RID: 4762
		internal bool modulePathSpecified;

		// Token: 0x0400129B RID: 4763
		private string configPath;

		// Token: 0x0400129C RID: 4764
		private bool runAsVirtualAccount;

		// Token: 0x0400129D RID: 4765
		private bool runAsVirtualAccountSpecified;
	}
}
