using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000350 RID: 848
	[Cmdlet("New", "PSSessionConfigurationFile", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=217036")]
	public class NewPSSessionConfigurationFileCommand : PSCmdlet
	{
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000E8620 File Offset: 0x000E6820
		// (set) Token: 0x060029F2 RID: 10738 RVA: 0x000E8628 File Offset: 0x000E6828
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true)]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000E8631 File Offset: 0x000E6831
		// (set) Token: 0x060029F4 RID: 10740 RVA: 0x000E8639 File Offset: 0x000E6839
		[Parameter]
		[ValidateNotNull]
		public Version SchemaVersion
		{
			get
			{
				return this.schemaVersion;
			}
			set
			{
				this.schemaVersion = value;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000E8642 File Offset: 0x000E6842
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x000E864A File Offset: 0x000E684A
		[Parameter]
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				this.guid = value;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000E8653 File Offset: 0x000E6853
		// (set) Token: 0x060029F8 RID: 10744 RVA: 0x000E865B File Offset: 0x000E685B
		[Parameter]
		public string Author
		{
			get
			{
				return this.author;
			}
			set
			{
				this.author = value;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x000E8664 File Offset: 0x000E6864
		// (set) Token: 0x060029FA RID: 10746 RVA: 0x000E866C File Offset: 0x000E686C
		[Parameter]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x000E8675 File Offset: 0x000E6875
		// (set) Token: 0x060029FC RID: 10748 RVA: 0x000E867D File Offset: 0x000E687D
		[Parameter]
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
			set
			{
				this.companyName = value;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x000E8686 File Offset: 0x000E6886
		// (set) Token: 0x060029FE RID: 10750 RVA: 0x000E868E File Offset: 0x000E688E
		[Parameter]
		public string Copyright
		{
			get
			{
				return this.copyright;
			}
			set
			{
				this.copyright = value;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x000E8697 File Offset: 0x000E6897
		// (set) Token: 0x06002A00 RID: 10752 RVA: 0x000E869F File Offset: 0x000E689F
		[Parameter]
		public SessionType SessionType
		{
			get
			{
				return this.sessionType;
			}
			set
			{
				this.sessionType = value;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x000E86A8 File Offset: 0x000E68A8
		// (set) Token: 0x06002A02 RID: 10754 RVA: 0x000E86B0 File Offset: 0x000E68B0
		[Parameter]
		public string TranscriptDirectory
		{
			get
			{
				return this.transcriptDirectory;
			}
			set
			{
				this.transcriptDirectory = value;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x000E86B9 File Offset: 0x000E68B9
		// (set) Token: 0x06002A04 RID: 10756 RVA: 0x000E86C1 File Offset: 0x000E68C1
		[Parameter]
		public SwitchParameter RunAsVirtualAccount { get; set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x000E86CA File Offset: 0x000E68CA
		// (set) Token: 0x06002A06 RID: 10758 RVA: 0x000E86D2 File Offset: 0x000E68D2
		[Parameter]
		public string[] RunAsVirtualAccountGroups { get; set; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x000E86DB File Offset: 0x000E68DB
		// (set) Token: 0x06002A08 RID: 10760 RVA: 0x000E86E3 File Offset: 0x000E68E3
		[Parameter]
		public string[] ScriptsToProcess
		{
			get
			{
				return this.scriptsToProcess;
			}
			set
			{
				this.scriptsToProcess = value;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06002A09 RID: 10761 RVA: 0x000E86EC File Offset: 0x000E68EC
		// (set) Token: 0x06002A0A RID: 10762 RVA: 0x000E86F4 File Offset: 0x000E68F4
		[Parameter]
		public IDictionary RoleDefinitions
		{
			get
			{
				return this.roleDefinitions;
			}
			set
			{
				this.roleDefinitions = value;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x000E86FD File Offset: 0x000E68FD
		// (set) Token: 0x06002A0C RID: 10764 RVA: 0x000E8705 File Offset: 0x000E6905
		[Parameter]
		public PSLanguageMode LanguageMode
		{
			get
			{
				return this.languageMode;
			}
			set
			{
				this.languageMode = value;
				this.isLanguageModeSpecified = true;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06002A0D RID: 10765 RVA: 0x000E8715 File Offset: 0x000E6915
		// (set) Token: 0x06002A0E RID: 10766 RVA: 0x000E871D File Offset: 0x000E691D
		[Parameter]
		public ExecutionPolicy ExecutionPolicy
		{
			get
			{
				return this.executionPolicy;
			}
			set
			{
				this.executionPolicy = value;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x000E8726 File Offset: 0x000E6926
		// (set) Token: 0x06002A10 RID: 10768 RVA: 0x000E872E File Offset: 0x000E692E
		[Parameter]
		public Version PowerShellVersion
		{
			get
			{
				return this.powerShellVersion;
			}
			set
			{
				this.powerShellVersion = value;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000E8737 File Offset: 0x000E6937
		// (set) Token: 0x06002A12 RID: 10770 RVA: 0x000E873F File Offset: 0x000E693F
		[Parameter]
		public object[] ModulesToImport
		{
			get
			{
				return this.modulesToImport;
			}
			set
			{
				this.modulesToImport = value;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06002A13 RID: 10771 RVA: 0x000E8748 File Offset: 0x000E6948
		// (set) Token: 0x06002A14 RID: 10772 RVA: 0x000E8750 File Offset: 0x000E6950
		[Parameter]
		public string[] VisibleAliases
		{
			get
			{
				return this.visibleAliases;
			}
			set
			{
				this.visibleAliases = value;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x000E8759 File Offset: 0x000E6959
		// (set) Token: 0x06002A16 RID: 10774 RVA: 0x000E8761 File Offset: 0x000E6961
		[Parameter]
		public object[] VisibleCmdlets
		{
			get
			{
				return this.visibleCmdlets;
			}
			set
			{
				this.visibleCmdlets = value;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06002A17 RID: 10775 RVA: 0x000E876A File Offset: 0x000E696A
		// (set) Token: 0x06002A18 RID: 10776 RVA: 0x000E8772 File Offset: 0x000E6972
		[Parameter]
		public object[] VisibleFunctions
		{
			get
			{
				return this.visibleFunctions;
			}
			set
			{
				this.visibleFunctions = value;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002A19 RID: 10777 RVA: 0x000E877B File Offset: 0x000E697B
		// (set) Token: 0x06002A1A RID: 10778 RVA: 0x000E8783 File Offset: 0x000E6983
		[Parameter]
		public string[] VisibleExternalCommands
		{
			get
			{
				return this.visibleExternalCommands;
			}
			set
			{
				this.visibleExternalCommands = value;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x000E878C File Offset: 0x000E698C
		// (set) Token: 0x06002A1C RID: 10780 RVA: 0x000E8794 File Offset: 0x000E6994
		[Parameter]
		public string[] VisibleProviders
		{
			get
			{
				return this.visibleProviders;
			}
			set
			{
				this.visibleProviders = value;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000E879D File Offset: 0x000E699D
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x000E87A5 File Offset: 0x000E69A5
		[Parameter]
		public IDictionary[] AliasDefinitions
		{
			get
			{
				return this.aliasDefinitions;
			}
			set
			{
				this.aliasDefinitions = value;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000E87AE File Offset: 0x000E69AE
		// (set) Token: 0x06002A20 RID: 10784 RVA: 0x000E87B6 File Offset: 0x000E69B6
		[Parameter]
		public IDictionary[] FunctionDefinitions
		{
			get
			{
				return this.functionDefinitions;
			}
			set
			{
				this.functionDefinitions = value;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x000E87BF File Offset: 0x000E69BF
		// (set) Token: 0x06002A22 RID: 10786 RVA: 0x000E87C7 File Offset: 0x000E69C7
		[Parameter]
		public object VariableDefinitions
		{
			get
			{
				return this.variableDefinitions;
			}
			set
			{
				this.variableDefinitions = value;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000E87D0 File Offset: 0x000E69D0
		// (set) Token: 0x06002A24 RID: 10788 RVA: 0x000E87D8 File Offset: 0x000E69D8
		[Parameter]
		public IDictionary EnvironmentVariables
		{
			get
			{
				return this.environmentVariables;
			}
			set
			{
				this.environmentVariables = value;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000E87E1 File Offset: 0x000E69E1
		// (set) Token: 0x06002A26 RID: 10790 RVA: 0x000E87E9 File Offset: 0x000E69E9
		[Parameter]
		public string[] TypesToProcess
		{
			get
			{
				return this.typesToProcess;
			}
			set
			{
				this.typesToProcess = value;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x000E87F2 File Offset: 0x000E69F2
		// (set) Token: 0x06002A28 RID: 10792 RVA: 0x000E87FA File Offset: 0x000E69FA
		[Parameter]
		public string[] FormatsToProcess
		{
			get
			{
				return this.formatsToProcess;
			}
			set
			{
				this.formatsToProcess = value;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002A29 RID: 10793 RVA: 0x000E8803 File Offset: 0x000E6A03
		// (set) Token: 0x06002A2A RID: 10794 RVA: 0x000E880B File Offset: 0x000E6A0B
		[Parameter]
		public string[] AssembliesToLoad
		{
			get
			{
				return this.assembliesToLoad;
			}
			set
			{
				this.assembliesToLoad = value;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002A2B RID: 10795 RVA: 0x000E8814 File Offset: 0x000E6A14
		// (set) Token: 0x06002A2C RID: 10796 RVA: 0x000E881C File Offset: 0x000E6A1C
		[Parameter]
		public SwitchParameter Full { get; set; }

		// Token: 0x06002A2D RID: 10797 RVA: 0x000E8828 File Offset: 0x000E6A28
		protected override void ProcessRecord()
		{
			ProviderInfo providerInfo = null;
			PSDriveInfo psdriveInfo;
			string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(this.path, out providerInfo, out psdriveInfo);
			if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || !unresolvedProviderPathFromPSPath.EndsWith(".pssc", StringComparison.OrdinalIgnoreCase))
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFilePath, this.path);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "InvalidPSSessionConfigurationFilePath", ErrorCategory.InvalidArgument, this.path);
				base.ThrowTerminatingError(errorRecord);
			}
			FileStream fileStream;
			StreamWriter streamWriter;
			FileInfo fileInfo;
			PathUtils.MasterStreamOpen(this, unresolvedProviderPathFromPSPath, "unicode", false, false, false, false, out fileStream, out streamWriter, out fileInfo, false);
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("@{");
				stringBuilder.Append(streamWriter.NewLine);
				stringBuilder.Append(streamWriter.NewLine);
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.SchemaVersion, RemotingErrorIdStrings.DISCSchemaVersionComment, SessionConfigurationUtils.QuoteName(this.schemaVersion), streamWriter, false));
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Guid, RemotingErrorIdStrings.DISCGUIDComment, SessionConfigurationUtils.QuoteName(this.guid), streamWriter, false));
				if (string.IsNullOrEmpty(this.author))
				{
					this.author = Environment.UserName;
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Author, RemotingErrorIdStrings.DISCAuthorComment, SessionConfigurationUtils.QuoteName(this.author), streamWriter, false));
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Description, RemotingErrorIdStrings.DISCDescriptionComment, SessionConfigurationUtils.QuoteName(this.description), streamWriter, string.IsNullOrEmpty(this.description)));
				if (this.ShouldGenerateConfigurationSnippet("CompanyName"))
				{
					if (string.IsNullOrEmpty(this.companyName))
					{
						this.companyName = Modules.DefaultCompanyName;
					}
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.CompanyName, RemotingErrorIdStrings.DISCCompanyNameComment, SessionConfigurationUtils.QuoteName(this.companyName), streamWriter, false));
				}
				if (this.ShouldGenerateConfigurationSnippet("Copyright"))
				{
					if (string.IsNullOrEmpty(this.copyright))
					{
						this.copyright = StringUtil.Format(Modules.DefaultCopyrightMessage, DateTime.Now.Year, this.author);
					}
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Copyright, RemotingErrorIdStrings.DISCCopyrightComment, SessionConfigurationUtils.QuoteName(this.copyright), streamWriter, false));
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.SessionType, RemotingErrorIdStrings.DISCInitialSessionStateComment, SessionConfigurationUtils.QuoteName(this.sessionType), streamWriter, false));
				string value = string.IsNullOrEmpty(this.transcriptDirectory) ? "'C:\\Transcripts\\'" : SessionConfigurationUtils.QuoteName(this.transcriptDirectory);
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.TranscriptDirectory, RemotingErrorIdStrings.DISCTranscriptDirectoryComment, value, streamWriter, string.IsNullOrEmpty(this.transcriptDirectory)));
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.RunAsVirtualAccount, RemotingErrorIdStrings.DISCRunAsVirtualAccountComment, SessionConfigurationUtils.WriteBoolean(true), streamWriter, this.RunAsVirtualAccount == false));
				bool flag = this.RunAsVirtualAccountGroups != null && this.RunAsVirtualAccountGroups.Length > 0;
				value = (flag ? SessionConfigurationUtils.CombineStringArray(this.RunAsVirtualAccountGroups) : "'Remote Desktop Users', 'Remote Management Users'");
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.RunAsVirtualAccountGroups, RemotingErrorIdStrings.DISCRunAsVirtualAccountGroupsComment, value, streamWriter, !flag));
				value = ((this.scriptsToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.scriptsToProcess) : "'C:\\ConfigData\\InitScript1.ps1', 'C:\\ConfigData\\InitScript2.ps1'");
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ScriptsToProcess, RemotingErrorIdStrings.DISCScriptsToProcessComment, value, streamWriter, this.scriptsToProcess.Length == 0));
				if (this.roleDefinitions == null)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.RoleDefinitions, RemotingErrorIdStrings.DISCRoleDefinitionsComment, "@{ 'CONTOSO\\SqlAdmins' = @{ RoleCapabilities = 'SqlAdministration' }; 'CONTOSO\\ServerMonitors' = @{ VisibleCmdlets = 'Get-Process' } } ", streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.RoleDefinitions, RemotingErrorIdStrings.DISCRoleDefinitionsComment, SessionConfigurationUtils.CombineHashtable(this.roleDefinitions, streamWriter, new int?(0)), streamWriter, false));
				}
				if (this.ShouldGenerateConfigurationSnippet("LanguageMode"))
				{
					if (!this.isLanguageModeSpecified && this.sessionType == SessionType.Default)
					{
						this.languageMode = PSLanguageMode.FullLanguage;
					}
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.LanguageMode, RemotingErrorIdStrings.DISCLanguageModeComment, SessionConfigurationUtils.QuoteName(this.languageMode), streamWriter, false));
				}
				if (this.ShouldGenerateConfigurationSnippet("ExecutionPolicy"))
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ExecutionPolicy, RemotingErrorIdStrings.DISCExecutionPolicyComment, SessionConfigurationUtils.QuoteName(this.executionPolicy), streamWriter, false));
				}
				bool isExample = false;
				if (this.ShouldGenerateConfigurationSnippet("PowerShellVersion"))
				{
					if (this.powerShellVersion == null)
					{
						isExample = true;
						this.powerShellVersion = PSVersionInfo.PSVersion;
					}
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.PowerShellVersion, RemotingErrorIdStrings.DISCPowerShellVersionComment, SessionConfigurationUtils.QuoteName(this.powerShellVersion), streamWriter, isExample));
				}
				if (this.modulesToImport == null)
				{
					if (this.Full)
					{
						string value2 = "'MyCustomModule', @{ ModuleName = 'MyCustomModule'; ModuleVersion = '1.0.0.0'; GUID = '4d30d5f0-cb16-4898-812d-f20a6c596bdf' }";
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ModulesToImport, RemotingErrorIdStrings.DISCModulesToImportComment, value2, streamWriter, true));
					}
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ModulesToImport, RemotingErrorIdStrings.DISCModulesToImportComment, SessionConfigurationUtils.CombineHashTableOrStringArray(this.modulesToImport, streamWriter, this), streamWriter, false));
				}
				if (this.ShouldGenerateConfigurationSnippet("VisibleAliases"))
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleAliases, RemotingErrorIdStrings.DISCVisibleAliasesComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleAliases, streamWriter, this), streamWriter, this.visibleAliases.Length == 0));
				}
				if (this.visibleCmdlets == null || this.visibleCmdlets.Length == 0)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleCmdlets, RemotingErrorIdStrings.DISCVisibleCmdletsComment, "'Invoke-Cmdlet1', @{ Name = 'Invoke-Cmdlet2'; Parameters = @{ Name = 'Parameter1'; ValidateSet = 'Item1', 'Item2' }, @{ Name = 'Parameter2'; ValidatePattern = 'L*' } }", streamWriter, true));
					}
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleCmdlets, RemotingErrorIdStrings.DISCVisibleCmdletsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleCmdlets, streamWriter, this), streamWriter, false));
				}
				if (this.visibleFunctions == null || this.visibleFunctions.Length == 0)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleFunctions, RemotingErrorIdStrings.DISCVisibleFunctionsComment, "'Invoke-Function1', @{ Name = 'Invoke-Function2'; Parameters = @{ Name = 'Parameter1'; ValidateSet = 'Item1', 'Item2' }, @{ Name = 'Parameter2'; ValidatePattern = 'L*' } }", streamWriter, true));
					}
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleFunctions, RemotingErrorIdStrings.DISCVisibleFunctionsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleFunctions, streamWriter, this), streamWriter, this.visibleFunctions.Length == 0));
				}
				if (this.ShouldGenerateConfigurationSnippet("VisibleExternalCommands"))
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleExternalCommands, RemotingErrorIdStrings.DISCVisibleExternalCommandsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleExternalCommands, streamWriter, this), streamWriter, this.visibleExternalCommands.Length == 0));
				}
				if (this.ShouldGenerateConfigurationSnippet("VisibleProviders"))
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleProviders, RemotingErrorIdStrings.DISCVisibleProvidersComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleProviders, streamWriter, this), streamWriter, this.visibleProviders.Length == 0));
				}
				if (this.aliasDefinitions == null || this.aliasDefinitions.Length == 0)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AliasDefinitions, RemotingErrorIdStrings.DISCAliasDefinitionsComment, "@{ Name = 'Alias1'; Value = 'Invoke-Alias1'}, @{ Name = 'Alias2'; Value = 'Invoke-Alias2'}", streamWriter, true));
					}
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AliasDefinitions, RemotingErrorIdStrings.DISCAliasDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(this.aliasDefinitions, streamWriter, new int?(0)), streamWriter, false));
				}
				if (this.functionDefinitions == null)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FunctionDefinitions, RemotingErrorIdStrings.DISCFunctionDefinitionsComment, "@{ Name = 'MyFunction'; ScriptBlock = { param($MyInput) $MyInput } }", streamWriter, true));
					}
				}
				else
				{
					Hashtable[] array = DISCPowerShellConfiguration.TryGetHashtableArray(this.functionDefinitions);
					if (array != null)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FunctionDefinitions, RemotingErrorIdStrings.DISCFunctionDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(array, streamWriter, new int?(0)), streamWriter, false));
						foreach (Hashtable hashtable in array)
						{
							if (!hashtable.ContainsKey(ConfigFileConstants.FunctionNameToken))
							{
								PSArgumentException ex = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
								{
									ConfigFileConstants.FunctionDefinitions,
									ConfigFileConstants.FunctionNameToken,
									this.path
								}));
								base.ThrowTerminatingError(ex.ErrorRecord);
							}
							if (!hashtable.ContainsKey(ConfigFileConstants.FunctionValueToken))
							{
								PSArgumentException ex2 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
								{
									ConfigFileConstants.FunctionDefinitions,
									ConfigFileConstants.FunctionValueToken,
									this.path
								}));
								base.ThrowTerminatingError(ex2.ErrorRecord);
							}
							if (!(hashtable[ConfigFileConstants.FunctionValueToken] is ScriptBlock))
							{
								PSArgumentException ex3 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCKeyMustBeScriptBlock, new object[]
								{
									ConfigFileConstants.FunctionValueToken,
									ConfigFileConstants.FunctionDefinitions,
									this.path
								}));
								base.ThrowTerminatingError(ex3.ErrorRecord);
							}
							foreach (object obj in hashtable.Keys)
							{
								string text = (string)obj;
								if (!string.Equals(text, ConfigFileConstants.FunctionNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionValueToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionOptionsToken, StringComparison.OrdinalIgnoreCase))
								{
									PSArgumentException ex4 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
									{
										text,
										ConfigFileConstants.FunctionDefinitions,
										this.path
									}));
									base.ThrowTerminatingError(ex4.ErrorRecord);
								}
							}
						}
					}
					else
					{
						PSArgumentException ex5 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, ConfigFileConstants.FunctionDefinitions, unresolvedProviderPathFromPSPath));
						base.ThrowTerminatingError(ex5.ErrorRecord);
					}
				}
				if (this.variableDefinitions == null)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, "@{ Name = 'Variable1'; Value = { 'Dynamic' + 'InitialValue' } }, @{ Name = 'Variable2'; Value = 'StaticInitialValue' }", streamWriter, true));
					}
				}
				else
				{
					string text2 = this.variableDefinitions as string;
					if (text2 != null)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, text2, streamWriter, false));
					}
					else
					{
						Hashtable[] array3 = DISCPowerShellConfiguration.TryGetHashtableArray(this.variableDefinitions);
						if (array3 != null)
						{
							stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(array3, streamWriter, new int?(0)), streamWriter, false));
							foreach (Hashtable hashtable2 in array3)
							{
								if (!hashtable2.ContainsKey(ConfigFileConstants.VariableNameToken))
								{
									PSArgumentException ex6 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
									{
										ConfigFileConstants.VariableDefinitions,
										ConfigFileConstants.VariableNameToken,
										this.path
									}));
									base.ThrowTerminatingError(ex6.ErrorRecord);
								}
								if (!hashtable2.ContainsKey(ConfigFileConstants.VariableValueToken))
								{
									PSArgumentException ex7 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
									{
										ConfigFileConstants.VariableDefinitions,
										ConfigFileConstants.VariableValueToken,
										this.path
									}));
									base.ThrowTerminatingError(ex7.ErrorRecord);
								}
								foreach (object obj2 in hashtable2.Keys)
								{
									string text3 = (string)obj2;
									if (!string.Equals(text3, ConfigFileConstants.VariableNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text3, ConfigFileConstants.VariableValueToken, StringComparison.OrdinalIgnoreCase))
									{
										PSArgumentException ex8 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
										{
											text3,
											ConfigFileConstants.VariableDefinitions,
											this.path
										}));
										base.ThrowTerminatingError(ex8.ErrorRecord);
									}
								}
							}
						}
						else
						{
							PSArgumentException ex9 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, ConfigFileConstants.VariableDefinitions, unresolvedProviderPathFromPSPath));
							base.ThrowTerminatingError(ex9.ErrorRecord);
						}
					}
				}
				if (this.environmentVariables == null)
				{
					if (this.Full)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.EnvironmentVariables, RemotingErrorIdStrings.DISCEnvironmentVariablesComment, "@{ Variable1 = 'Value1'; Variable2 = 'Value2' }", streamWriter, true));
					}
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.EnvironmentVariables, RemotingErrorIdStrings.DISCEnvironmentVariablesComment, SessionConfigurationUtils.CombineHashtable(this.environmentVariables, streamWriter, new int?(0)), streamWriter, false));
				}
				if (this.ShouldGenerateConfigurationSnippet("TypesToProcess"))
				{
					value = ((this.typesToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.typesToProcess) : "'C:\\ConfigData\\MyTypes.ps1xml', 'C:\\ConfigData\\OtherTypes.ps1xml'");
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.TypesToProcess, RemotingErrorIdStrings.DISCTypesToProcessComment, value, streamWriter, this.typesToProcess.Length == 0));
				}
				if (this.ShouldGenerateConfigurationSnippet("FormatsToProcess"))
				{
					value = ((this.formatsToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.formatsToProcess) : "'C:\\ConfigData\\MyFormats.ps1xml', 'C:\\ConfigData\\OtherFormats.ps1xml'");
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FormatsToProcess, RemotingErrorIdStrings.DISCFormatsToProcessComment, value, streamWriter, this.formatsToProcess.Length == 0));
				}
				if (this.ShouldGenerateConfigurationSnippet("AssembliesToLoad"))
				{
					isExample = false;
					if (this.assembliesToLoad == null || this.assembliesToLoad.Length == 0)
					{
						isExample = true;
						this.assembliesToLoad = new string[]
						{
							"System.Web",
							"System.OtherAssembly, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
						};
					}
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AssembliesToLoad, RemotingErrorIdStrings.DISCAssembliesToLoadComment, SessionConfigurationUtils.CombineStringArray(this.assembliesToLoad), streamWriter, isExample));
				}
				stringBuilder.Append("}");
				streamWriter.Write(stringBuilder.ToString());
			}
			finally
			{
				streamWriter.Dispose();
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000E95B4 File Offset: 0x000E77B4
		private bool ShouldGenerateConfigurationSnippet(string parameterName)
		{
			return this.Full || base.MyInvocation.BoundParameters.ContainsKey(parameterName);
		}

		// Token: 0x040014BF RID: 5311
		private string path;

		// Token: 0x040014C0 RID: 5312
		private Version schemaVersion = new Version("2.0.0.0");

		// Token: 0x040014C1 RID: 5313
		private Guid guid = Guid.NewGuid();

		// Token: 0x040014C2 RID: 5314
		private string author;

		// Token: 0x040014C3 RID: 5315
		private string description;

		// Token: 0x040014C4 RID: 5316
		private string companyName;

		// Token: 0x040014C5 RID: 5317
		private string copyright;

		// Token: 0x040014C6 RID: 5318
		private SessionType sessionType = SessionType.Default;

		// Token: 0x040014C7 RID: 5319
		private string transcriptDirectory;

		// Token: 0x040014C8 RID: 5320
		private string[] scriptsToProcess = new string[0];

		// Token: 0x040014C9 RID: 5321
		private IDictionary roleDefinitions;

		// Token: 0x040014CA RID: 5322
		private PSLanguageMode languageMode = PSLanguageMode.NoLanguage;

		// Token: 0x040014CB RID: 5323
		private bool isLanguageModeSpecified;

		// Token: 0x040014CC RID: 5324
		private ExecutionPolicy executionPolicy = ExecutionPolicy.Restricted;

		// Token: 0x040014CD RID: 5325
		private Version powerShellVersion;

		// Token: 0x040014CE RID: 5326
		private object[] modulesToImport;

		// Token: 0x040014CF RID: 5327
		private string[] visibleAliases = new string[0];

		// Token: 0x040014D0 RID: 5328
		private object[] visibleCmdlets;

		// Token: 0x040014D1 RID: 5329
		private object[] visibleFunctions;

		// Token: 0x040014D2 RID: 5330
		private string[] visibleExternalCommands = new string[0];

		// Token: 0x040014D3 RID: 5331
		private string[] visibleProviders = new string[0];

		// Token: 0x040014D4 RID: 5332
		private IDictionary[] aliasDefinitions;

		// Token: 0x040014D5 RID: 5333
		private IDictionary[] functionDefinitions;

		// Token: 0x040014D6 RID: 5334
		private object variableDefinitions;

		// Token: 0x040014D7 RID: 5335
		private IDictionary environmentVariables;

		// Token: 0x040014D8 RID: 5336
		private string[] typesToProcess = new string[0];

		// Token: 0x040014D9 RID: 5337
		private string[] formatsToProcess = new string[0];

		// Token: 0x040014DA RID: 5338
		private string[] assembliesToLoad;
	}
}
