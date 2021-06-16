using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200033A RID: 826
	[OutputType(new Type[]
	{
		typeof(PSRemotingJob)
	})]
	[Cmdlet("Start", "Job", DefaultParameterSetName = "ComputerName", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113405")]
	public class StartJobCommand : PSExecutionCmdlet, IDisposable
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000DF7D4 File Offset: 0x000DD9D4
		// (set) Token: 0x060027F8 RID: 10232 RVA: 0x000DF7DC File Offset: 0x000DD9DC
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "DefinitionName")]
		[ValidateNotNullOrEmpty]
		public string DefinitionName
		{
			get
			{
				return this._definitionName;
			}
			set
			{
				this._definitionName = value;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000DF7E5 File Offset: 0x000DD9E5
		// (set) Token: 0x060027FA RID: 10234 RVA: 0x000DF7ED File Offset: 0x000DD9ED
		[Parameter(Position = 1, ParameterSetName = "DefinitionName")]
		[ValidateNotNullOrEmpty]
		public string DefinitionPath
		{
			get
			{
				return this._definitionPath;
			}
			set
			{
				this._definitionPath = value;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x000DF7F6 File Offset: 0x000DD9F6
		// (set) Token: 0x060027FC RID: 10236 RVA: 0x000DF7FE File Offset: 0x000DD9FE
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 2, ParameterSetName = "DefinitionName")]
		public string Type
		{
			get
			{
				return this._definitionType;
			}
			set
			{
				this._definitionType = value;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x000DF807 File Offset: 0x000DDA07
		// (set) Token: 0x060027FE RID: 10238 RVA: 0x000DF80F File Offset: 0x000DDA0F
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathComputerName")]
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.name = value;
				}
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x000DF820 File Offset: 0x000DDA20
		// (set) Token: 0x06002800 RID: 10240 RVA: 0x000DF828 File Offset: 0x000DDA28
		[Alias(new string[]
		{
			"Command"
		})]
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "ComputerName")]
		public override ScriptBlock ScriptBlock
		{
			get
			{
				return base.ScriptBlock;
			}
			set
			{
				base.ScriptBlock = value;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x000DF831 File Offset: 0x000DDA31
		public override PSSession[] Session
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002802 RID: 10242 RVA: 0x000DF834 File Offset: 0x000DDA34
		public override string[] ComputerName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000DF837 File Offset: 0x000DDA37
		public override SwitchParameter EnableNetworkAccess
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x000DF83F File Offset: 0x000DDA3F
		// (set) Token: 0x06002805 RID: 10245 RVA: 0x000DF847 File Offset: 0x000DDA47
		[Credential]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "LiteralFilePathComputerName")]
		public override PSCredential Credential
		{
			get
			{
				return base.Credential;
			}
			set
			{
				base.Credential = value;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x000DF850 File Offset: 0x000DDA50
		public override int Port
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x000DF853 File Offset: 0x000DDA53
		public override SwitchParameter UseSSL
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x000DF85B File Offset: 0x000DDA5B
		// (set) Token: 0x06002809 RID: 10249 RVA: 0x000DF863 File Offset: 0x000DDA63
		public override string ConfigurationName
		{
			get
			{
				return base.ConfigurationName;
			}
			set
			{
				base.ConfigurationName = value;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000DF86C File Offset: 0x000DDA6C
		public override int ThrottleLimit
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x000DF86F File Offset: 0x000DDA6F
		public override string ApplicationName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x000DF872 File Offset: 0x000DDA72
		public override Uri[] ConnectionUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x000DF875 File Offset: 0x000DDA75
		// (set) Token: 0x0600280E RID: 10254 RVA: 0x000DF87D File Offset: 0x000DDA7D
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "FilePathComputerName")]
		public override string FilePath
		{
			get
			{
				return base.FilePath;
			}
			set
			{
				base.FilePath = value;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x000DF886 File Offset: 0x000DDA86
		// (set) Token: 0x06002810 RID: 10256 RVA: 0x000DF88E File Offset: 0x000DDA8E
		[Alias(new string[]
		{
			"PSPath"
		})]
		[Parameter(Mandatory = true, ParameterSetName = "LiteralFilePathComputerName")]
		public string LiteralPath
		{
			get
			{
				return base.FilePath;
			}
			set
			{
				base.FilePath = value;
				base.IsLiteralPath = true;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000DF89E File Offset: 0x000DDA9E
		// (set) Token: 0x06002812 RID: 10258 RVA: 0x000DF8A6 File Offset: 0x000DDAA6
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		public override AuthenticationMechanism Authentication
		{
			get
			{
				return base.Authentication;
			}
			set
			{
				base.Authentication = value;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x000DF8AF File Offset: 0x000DDAAF
		// (set) Token: 0x06002814 RID: 10260 RVA: 0x000DF8B7 File Offset: 0x000DDAB7
		public override string CertificateThumbprint
		{
			get
			{
				return base.CertificateThumbprint;
			}
			set
			{
				base.CertificateThumbprint = value;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000DF8C0 File Offset: 0x000DDAC0
		public override SwitchParameter AllowRedirection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x000DF8C8 File Offset: 0x000DDAC8
		public override Guid[] VMId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x000DF8CB File Offset: 0x000DDACB
		public override string[] VMName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x000DF8CE File Offset: 0x000DDACE
		public override string[] ContainerId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x000DF8D1 File Offset: 0x000DDAD1
		public override string[] ContainerName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x000DF8D4 File Offset: 0x000DDAD4
		public override SwitchParameter RunAsAdministrator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600281B RID: 10267 RVA: 0x000DF8DC File Offset: 0x000DDADC
		// (set) Token: 0x0600281C RID: 10268 RVA: 0x000DF8E4 File Offset: 0x000DDAE4
		public override PSSessionOption SessionOption
		{
			get
			{
				return base.SessionOption;
			}
			set
			{
				base.SessionOption = value;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600281D RID: 10269 RVA: 0x000DF8ED File Offset: 0x000DDAED
		// (set) Token: 0x0600281E RID: 10270 RVA: 0x000DF8F5 File Offset: 0x000DDAF5
		[Parameter(Position = 1, ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(Position = 1, ParameterSetName = "ComputerName")]
		[Parameter(Position = 1, ParameterSetName = "FilePathComputerName")]
		public virtual ScriptBlock InitializationScript
		{
			get
			{
				return this.initScript;
			}
			set
			{
				this.initScript = value;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600281F RID: 10271 RVA: 0x000DF8FE File Offset: 0x000DDAFE
		// (set) Token: 0x06002820 RID: 10272 RVA: 0x000DF90B File Offset: 0x000DDB0B
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		public virtual SwitchParameter RunAs32
		{
			get
			{
				return this.shouldRunAs32;
			}
			set
			{
				this.shouldRunAs32 = value;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06002821 RID: 10273 RVA: 0x000DF919 File Offset: 0x000DDB19
		// (set) Token: 0x06002822 RID: 10274 RVA: 0x000DF921 File Offset: 0x000DDB21
		[Parameter(ParameterSetName = "LiteralFilePathComputerName")]
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual Version PSVersion
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
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000DF936 File Offset: 0x000DDB36
		// (set) Token: 0x06002824 RID: 10276 RVA: 0x000DF93E File Offset: 0x000DDB3E
		[Parameter(ValueFromPipeline = true, ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(ValueFromPipeline = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(ValueFromPipeline = true, ParameterSetName = "ComputerName")]
		public override PSObject InputObject
		{
			get
			{
				return base.InputObject;
			}
			set
			{
				base.InputObject = value;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x000DF947 File Offset: 0x000DDB47
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x000DF94F File Offset: 0x000DDB4F
		[Parameter(ParameterSetName = "LiteralFilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Alias(new string[]
		{
			"Args"
		})]
		[Parameter(ParameterSetName = "ComputerName")]
		public override object[] ArgumentList
		{
			get
			{
				return base.ArgumentList;
			}
			set
			{
				base.ArgumentList = value;
			}
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000DF958 File Offset: 0x000DDB58
		protected override void BeginProcessing()
		{
			CommandDiscovery.AutoloadModulesWithJobSourceAdapters(base.Context, base.CommandOrigin);
			if (base.ParameterSetName == "DefinitionName")
			{
				return;
			}
			base.SkipWinRMCheck = true;
			base.BeginProcessing();
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000DF98C File Offset: 0x000DDB8C
		protected override void CreateHelpersForSpecifiedComputerNames()
		{
			if (base.Context.LanguageMode == PSLanguageMode.ConstrainedLanguage && SystemPolicy.GetSystemLockdownPolicy() != SystemEnforcementMode.Enforce && (this.ScriptBlock != null || this.InitializationScript != null))
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSNotSupportedException(RemotingErrorIdStrings.CannotStartJobInconsistentLanguageMode), "CannotStartJobInconsistentLanguageMode", ErrorCategory.PermissionDenied, base.Context.LanguageMode));
			}
			RemoteRunspace remoteRunspace = (RemoteRunspace)RunspaceFactory.CreateRunspace(new NewProcessConnectionInfo(this.Credential)
			{
				RunAs32 = this.shouldRunAs32,
				InitializationScript = this.initScript,
				AuthenticationMechanism = this.Authentication,
				PSVersion = this.PSVersion
			}, base.Host, Utils.GetTypeTableFromExecutionContextTLS());
			remoteRunspace.Events.ReceivedEvents.PSEventReceived += base.OnRunspacePSEventReceived;
			Pipeline pipeline = base.CreatePipeline(remoteRunspace);
			IThrottleOperation item = new ExecutionCmdletHelperComputerName(remoteRunspace, pipeline, false);
			base.Operations.Add(item);
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000DFA78 File Offset: 0x000DDC78
		protected override void ProcessRecord()
		{
			if (!(base.ParameterSetName == "DefinitionName"))
			{
				if (this.firstProcessRecord)
				{
					this.firstProcessRecord = false;
					PSRemotingJob psremotingJob = new PSRemotingJob(base.ResolvedComputerNames, base.Operations, this.ScriptBlock.ToString(), this.ThrottleLimit, this.name);
					psremotingJob.PSJobTypeName = StartJobCommand.StartJobType;
					base.JobRepository.Add(psremotingJob);
					base.WriteObject(psremotingJob);
				}
				if (this.InputObject != AutomationNull.Value)
				{
					foreach (IThrottleOperation throttleOperation in base.Operations)
					{
						ExecutionCmdletHelper executionCmdletHelper = (ExecutionCmdletHelper)throttleOperation;
						executionCmdletHelper.Pipeline.Input.Write(this.InputObject);
					}
				}
				return;
			}
			string definitionPath = null;
			if (!string.IsNullOrEmpty(this._definitionPath))
			{
				ProviderInfo providerInfo = null;
				Collection<string> resolvedProviderPathFromPSPath = base.Context.SessionState.Path.GetResolvedProviderPathFromPSPath(this._definitionPath, out providerInfo);
				if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem))
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.StartJobDefinitionPathInvalidNotFSProvider, new object[]
					{
						this._definitionName,
						this._definitionPath,
						providerInfo.FullName
					});
					base.WriteError(new ErrorRecord(new RuntimeException(message), "StartJobFromDefinitionNamePathInvalidNotFileSystemProvider", ErrorCategory.InvalidArgument, null));
					return;
				}
				if (resolvedProviderPathFromPSPath.Count != 1)
				{
					string message2 = StringUtil.Format(RemotingErrorIdStrings.StartJobDefinitionPathInvalidNotSingle, this._definitionName, this._definitionPath);
					base.WriteError(new ErrorRecord(new RuntimeException(message2), "StartJobFromDefinitionNamePathInvalidNotSingle", ErrorCategory.InvalidArgument, null));
					return;
				}
				definitionPath = resolvedProviderPathFromPSPath[0];
			}
			List<Job2> jobToStart = base.JobManager.GetJobToStart(this._definitionName, definitionPath, this._definitionType, this, false);
			if (jobToStart.Count == 0)
			{
				string message3 = (this._definitionType != null) ? StringUtil.Format(RemotingErrorIdStrings.StartJobDefinitionNotFound2, this._definitionType, this._definitionName) : StringUtil.Format(RemotingErrorIdStrings.StartJobDefinitionNotFound1, this._definitionName);
				base.WriteError(new ErrorRecord(new RuntimeException(message3), "StartJobFromDefinitionNameNotFound", ErrorCategory.ObjectNotFound, null));
				return;
			}
			if (jobToStart.Count > 1)
			{
				string message4 = StringUtil.Format(RemotingErrorIdStrings.StartJobManyDefNameMatches, this._definitionName);
				base.WriteError(new ErrorRecord(new RuntimeException(message4), "StartJobFromDefinitionNameMoreThanOneMatch", ErrorCategory.InvalidResult, null));
				return;
			}
			Job2 job = jobToStart[0];
			job.StartJob();
			base.WriteObject(job);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000DFD00 File Offset: 0x000DDF00
		protected override void EndProcessing()
		{
			base.CloseAllInputStreams();
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000DFD08 File Offset: 0x000DDF08
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000DFD17 File Offset: 0x000DDF17
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				base.CloseAllInputStreams();
			}
		}

		// Token: 0x040013C9 RID: 5065
		private const string DefinitionNameParameterSet = "DefinitionName";

		// Token: 0x040013CA RID: 5066
		private static readonly string StartJobType = "BackgroundJob";

		// Token: 0x040013CB RID: 5067
		private string _definitionName;

		// Token: 0x040013CC RID: 5068
		private string _definitionPath;

		// Token: 0x040013CD RID: 5069
		private string _definitionType;

		// Token: 0x040013CE RID: 5070
		private string name;

		// Token: 0x040013CF RID: 5071
		private ScriptBlock initScript;

		// Token: 0x040013D0 RID: 5072
		private bool shouldRunAs32;

		// Token: 0x040013D1 RID: 5073
		private Version psVersion;

		// Token: 0x040013D2 RID: 5074
		private bool firstProcessRecord = true;
	}
}
