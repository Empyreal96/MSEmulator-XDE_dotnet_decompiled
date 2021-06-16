using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000325 RID: 805
	public abstract class PSExecutionCmdlet : PSRemotingBaseCmdlet
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x000D6FFB File Offset: 0x000D51FB
		// (set) Token: 0x06002679 RID: 9849 RVA: 0x000D7003 File Offset: 0x000D5203
		[Parameter(ValueFromPipeline = true)]
		public virtual PSObject InputObject
		{
			get
			{
				return this.inputObject;
			}
			set
			{
				this.inputObject = value;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x000D700C File Offset: 0x000D520C
		// (set) Token: 0x0600267B RID: 9851 RVA: 0x000D7014 File Offset: 0x000D5214
		public virtual ScriptBlock ScriptBlock
		{
			get
			{
				return this.scriptBlock;
			}
			set
			{
				this.scriptBlock = value;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x000D701D File Offset: 0x000D521D
		// (set) Token: 0x0600267D RID: 9853 RVA: 0x000D7025 File Offset: 0x000D5225
		[ValidateNotNull]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathUri")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathRunspace")]
		public virtual string FilePath
		{
			get
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x000D702E File Offset: 0x000D522E
		// (set) Token: 0x0600267F RID: 9855 RVA: 0x000D7036 File Offset: 0x000D5236
		protected bool IsLiteralPath { get; set; }

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x000D703F File Offset: 0x000D523F
		// (set) Token: 0x06002681 RID: 9857 RVA: 0x000D7047 File Offset: 0x000D5247
		[Alias(new string[]
		{
			"Args"
		})]
		[Parameter]
		public virtual object[] ArgumentList
		{
			get
			{
				return this.args;
			}
			set
			{
				this.args = value;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x000D7050 File Offset: 0x000D5250
		// (set) Token: 0x06002683 RID: 9859 RVA: 0x000D7058 File Offset: 0x000D5258
		protected bool InvokeAndDisconnect
		{
			get
			{
				return this.invokeAndDisconnect;
			}
			set
			{
				this.invokeAndDisconnect = value;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x000D7061 File Offset: 0x000D5261
		// (set) Token: 0x06002685 RID: 9861 RVA: 0x000D7069 File Offset: 0x000D5269
		protected string[] DisconnectedSessionName
		{
			get
			{
				return this.sessionName;
			}
			set
			{
				this.sessionName = value;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x000D7072 File Offset: 0x000D5272
		// (set) Token: 0x06002687 RID: 9863 RVA: 0x000D707A File Offset: 0x000D527A
		public virtual SwitchParameter EnableNetworkAccess
		{
			get
			{
				return this.enableNetworkAccess;
			}
			set
			{
				this.enableNetworkAccess = value;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x000D7083 File Offset: 0x000D5283
		// (set) Token: 0x06002689 RID: 9865 RVA: 0x000D708B File Offset: 0x000D528B
		[Alias(new string[]
		{
			"VMGuid"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMId")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathVMId")]
		[ValidateNotNullOrEmpty]
		public virtual Guid[] VMId
		{
			get
			{
				return this.vmId;
			}
			set
			{
				this.vmId = value;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x000D7094 File Offset: 0x000D5294
		// (set) Token: 0x0600268B RID: 9867 RVA: 0x000D709C File Offset: 0x000D529C
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathVMName")]
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMName")]
		public virtual string[] VMName
		{
			get
			{
				return this.vmName;
			}
			set
			{
				this.vmName = value;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x000D70A5 File Offset: 0x000D52A5
		// (set) Token: 0x0600268D RID: 9869 RVA: 0x000D70AD File Offset: 0x000D52AD
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathContainerId")]
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ContainerId")]
		public virtual string[] ContainerId
		{
			get
			{
				return this.containerId;
			}
			set
			{
				this.containerId = value;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x000D70B6 File Offset: 0x000D52B6
		// (set) Token: 0x0600268F RID: 9871 RVA: 0x000D70BE File Offset: 0x000D52BE
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ContainerName")]
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathContainerName")]
		public virtual string[] ContainerName
		{
			get
			{
				return this.containerName;
			}
			set
			{
				this.containerName = value;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x000D70C7 File Offset: 0x000D52C7
		// (set) Token: 0x06002691 RID: 9873 RVA: 0x000D70CF File Offset: 0x000D52CF
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathContainerName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		[Parameter(ParameterSetName = "ContainerId")]
		public virtual SwitchParameter RunAsAdministrator
		{
			get
			{
				return this.runAsAdministrator;
			}
			set
			{
				this.runAsAdministrator = value;
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000D70D8 File Offset: 0x000D52D8
		protected virtual void CreateHelpersForSpecifiedComputerNames()
		{
			base.ValidateComputerName(base.ResolvedComputerNames);
			RemoteRunspace remoteRunspace = null;
			string scheme = this.UseSSL.IsPresent ? "https" : "http";
			int i = 0;
			while (i < base.ResolvedComputerNames.Length)
			{
				try
				{
					WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
					wsmanConnectionInfo.Scheme = scheme;
					wsmanConnectionInfo.ComputerName = base.ResolvedComputerNames[i];
					wsmanConnectionInfo.Port = this.Port;
					wsmanConnectionInfo.AppName = this.ApplicationName;
					wsmanConnectionInfo.ShellUri = this.ConfigurationName;
					if (this.CertificateThumbprint != null)
					{
						wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
					}
					else
					{
						wsmanConnectionInfo.Credential = this.Credential;
					}
					wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
					base.UpdateConnectionInfo(wsmanConnectionInfo);
					wsmanConnectionInfo.EnableNetworkAccess = this.EnableNetworkAccess;
					int id = PSSession.GenerateRunspaceId();
					string name = (this.DisconnectedSessionName != null && this.DisconnectedSessionName.Length > i) ? this.DisconnectedSessionName[i] : PSSession.ComposeRunspaceName(id);
					remoteRunspace = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), wsmanConnectionInfo, base.Host, this.SessionOption.ApplicationArguments, name, id);
					remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.OnRunspacePSEventReceived;
				}
				catch (UriFormatException exception)
				{
					ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, base.ResolvedComputerNames[i]);
					base.WriteError(errorRecord);
					goto IL_16D;
				}
				goto IL_147;
				IL_16D:
				i++;
				continue;
				IL_147:
				Pipeline pipeline = this.CreatePipeline(remoteRunspace);
				IThrottleOperation item = new ExecutionCmdletHelperComputerName(remoteRunspace, pipeline, this.invokeAndDisconnect);
				this.Operations.Add(item);
				goto IL_16D;
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000D7274 File Offset: 0x000D5474
		protected void CreateHelpersForSpecifiedRunspaces()
		{
			int num = this.Session.Length;
			RemoteRunspace[] array = new RemoteRunspace[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (RemoteRunspace)this.Session[i].Runspace;
			}
			Pipeline[] array2 = new Pipeline[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = this.CreatePipeline(array[j]);
				IThrottleOperation item = new ExecutionCmdletHelperRunspace(array2[j]);
				this.Operations.Add(item);
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000D72F0 File Offset: 0x000D54F0
		protected void CreateHelpersForSpecifiedUris()
		{
			RemoteRunspace remoteRunspace = null;
			int i = 0;
			while (i < this.ConnectionUri.Length)
			{
				try
				{
					WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
					wsmanConnectionInfo.ConnectionUri = this.ConnectionUri[i];
					wsmanConnectionInfo.ShellUri = this.ConfigurationName;
					if (this.CertificateThumbprint != null)
					{
						wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
					}
					else
					{
						wsmanConnectionInfo.Credential = this.Credential;
					}
					wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
					base.UpdateConnectionInfo(wsmanConnectionInfo);
					wsmanConnectionInfo.EnableNetworkAccess = this.EnableNetworkAccess;
					remoteRunspace = (RemoteRunspace)RunspaceFactory.CreateRunspace(wsmanConnectionInfo, base.Host, Utils.GetTypeTableFromExecutionContextTLS(), this.SessionOption.ApplicationArguments);
					remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.OnRunspacePSEventReceived;
				}
				catch (UriFormatException e)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e, this.ConnectionUri[i]);
					goto IL_10F;
				}
				catch (InvalidOperationException e2)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e2, this.ConnectionUri[i]);
					goto IL_10F;
				}
				catch (ArgumentException e3)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e3, this.ConnectionUri[i]);
					goto IL_10F;
				}
				goto IL_E9;
				IL_10F:
				i++;
				continue;
				IL_E9:
				Pipeline pipeline = this.CreatePipeline(remoteRunspace);
				IThrottleOperation item = new ExecutionCmdletHelperComputerName(remoteRunspace, pipeline, this.invokeAndDisconnect);
				this.Operations.Add(item);
				goto IL_10F;
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000D7448 File Offset: 0x000D5648
		protected virtual void CreateHelpersForSpecifiedVMSession()
		{
			if (base.ParameterSetName == "VMId" || base.ParameterSetName == "FilePathVMId")
			{
				int num = this.VMId.Length;
				this.VMName = new string[num];
				for (int i = 0; i < num; i++)
				{
					string script = "Get-VM -Id $args[0]";
					Collection<PSObject> collection;
					try
					{
						collection = base.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null, new object[]
						{
							this.VMId[i]
						});
					}
					catch (CommandNotFoundException)
					{
						base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HyperVModuleNotAvailable), PSRemotingErrorId.HyperVModuleNotAvailable.ToString(), ErrorCategory.NotInstalled, null));
						return;
					}
					if (collection.Count != 1)
					{
						this.VMName[i] = string.Empty;
					}
					else
					{
						this.VMName[i] = (string)collection[0].Properties["VMName"].Value;
					}
				}
			}
			else
			{
				int num = this.VMName.Length;
				this.VMId = new Guid[num];
				for (int i = 0; i < num; i++)
				{
					string script = "Get-VM -Name $args";
					Collection<PSObject> collection;
					try
					{
						collection = base.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null, new object[]
						{
							this.VMName[i]
						});
					}
					catch (CommandNotFoundException)
					{
						base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HyperVModuleNotAvailable), PSRemotingErrorId.HyperVModuleNotAvailable.ToString(), ErrorCategory.NotInstalled, null));
						return;
					}
					if (collection.Count != 1)
					{
						this.VMId[i] = Guid.Empty;
					}
					else
					{
						this.VMId[i] = (Guid)collection[0].Properties["VMId"].Value;
						this.VMName[i] = (string)collection[0].Properties["VMName"].Value;
					}
				}
			}
			base.ResolvedComputerNames = this.VMName;
			for (int i = 0; i < base.ResolvedComputerNames.Length; i++)
			{
				if (this.VMId[i] == Guid.Empty && (base.ParameterSetName == "VMName" || base.ParameterSetName == "FilePathVMName"))
				{
					base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.InvalidVMNameNotSingle, new object[]
					{
						this.VMName[i]
					})), PSRemotingErrorId.InvalidVMNameNotSingle.ToString(), ErrorCategory.InvalidArgument, null));
				}
				else if (this.VMName[i] == string.Empty && (base.ParameterSetName == "VMId" || base.ParameterSetName == "FilePathVMId"))
				{
					base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.InvalidVMIdNotSingle, new object[]
					{
						this.VMId[i].ToString(null)
					})), PSRemotingErrorId.InvalidVMIdNotSingle.ToString(), ErrorCategory.InvalidArgument, null));
				}
				else
				{
					RemoteRunspace remoteRunspace = null;
					try
					{
						VMConnectionInfo connectionInfo = new VMConnectionInfo(this.Credential, this.VMId[i], this.VMName[i]);
						remoteRunspace = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), connectionInfo, base.Host, null, null, -1);
						remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.OnRunspacePSEventReceived;
					}
					catch (InvalidOperationException exception)
					{
						ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceForVMFailed", ErrorCategory.InvalidOperation, null);
						base.WriteError(errorRecord);
					}
					catch (ArgumentException exception2)
					{
						ErrorRecord errorRecord2 = new ErrorRecord(exception2, "CreateRemoteRunspaceForVMFailed", ErrorCategory.InvalidArgument, null);
						base.WriteError(errorRecord2);
					}
					Pipeline pipeline = this.CreatePipeline(remoteRunspace);
					IThrottleOperation item = new ExecutionCmdletHelperComputerName(remoteRunspace, pipeline, false);
					this.Operations.Add(item);
				}
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000D7874 File Offset: 0x000D5A74
		protected virtual void CreateHelpersForSpecifiedContainerSession()
		{
			bool flag = false;
			List<string> list = new List<string>();
			string[] array;
			if (base.ParameterSetName == "ContainerId" || base.ParameterSetName == "FilePathContainerId")
			{
				array = this.ContainerId;
				flag = true;
			}
			else
			{
				array = this.ContainerName;
			}
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				RemoteRunspace remoteRunspace = null;
				try
				{
					ContainerConnectionInfo containerConnectionInfo;
					if (flag)
					{
						containerConnectionInfo = ContainerConnectionInfo.CreateContainerConnectionInfoById(text, this.RunAsAdministrator.IsPresent);
					}
					else
					{
						containerConnectionInfo = ContainerConnectionInfo.CreateContainerConnectionInfoByName(text, this.RunAsAdministrator.IsPresent);
					}
					list.Add(containerConnectionInfo.ComputerName);
					containerConnectionInfo.CreateContainerProcess();
					remoteRunspace = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), containerConnectionInfo, base.Host, null, null, -1);
					remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.OnRunspacePSEventReceived;
				}
				catch (InvalidOperationException exception)
				{
					ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidOperation, null);
					base.WriteError(errorRecord);
					goto IL_149;
				}
				catch (ArgumentException exception2)
				{
					ErrorRecord errorRecord2 = new ErrorRecord(exception2, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidArgument, null);
					base.WriteError(errorRecord2);
					goto IL_149;
				}
				catch (Exception exception3)
				{
					ErrorRecord errorRecord3 = new ErrorRecord(exception3, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidOperation, null);
					base.WriteError(errorRecord3);
					goto IL_149;
				}
				goto IL_126;
				IL_149:
				i++;
				continue;
				IL_126:
				Pipeline pipeline = this.CreatePipeline(remoteRunspace);
				IThrottleOperation item = new ExecutionCmdletHelperComputerName(remoteRunspace, pipeline, false);
				this.Operations.Add(item);
				goto IL_149;
			}
			base.ResolvedComputerNames = list.ToArray();
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000D7A10 File Offset: 0x000D5C10
		internal Pipeline CreatePipeline(RemoteRunspace remoteRunspace)
		{
			string remoteServerPsVersion = this.GetRemoteServerPsVersion(remoteRunspace);
			PowerShell powerShell = (remoteServerPsVersion == "PSv2") ? this.GetPowerShellForPSv2() : this.GetPowerShellForPSv3OrLater(remoteServerPsVersion);
			Pipeline pipeline = remoteRunspace.CreatePipeline(powerShell.Commands.Commands[0].CommandText, true);
			pipeline.Commands.Clear();
			foreach (Command item in powerShell.Commands.Commands)
			{
				pipeline.Commands.Add(item);
			}
			pipeline.RedirectShellErrorOutputPipe = true;
			return pipeline;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000D7AC4 File Offset: 0x000D5CC4
		private string GetRemoteServerPsVersion(RemoteRunspace remoteRunspace)
		{
			if (remoteRunspace.ConnectionInfo is NewProcessConnectionInfo)
			{
				return "PSv5OrLater";
			}
			PSPrimitiveDictionary applicationPrivateData = remoteRunspace.GetApplicationPrivateData();
			if (applicationPrivateData == null)
			{
				return "PSv2";
			}
			if (remoteRunspace.CanDisconnect)
			{
				Version version = null;
				PSPrimitiveDictionary.TryPathGet<Version>(applicationPrivateData, out version, new string[]
				{
					"PSVersionTable",
					"PSVersion"
				});
				if (version != null)
				{
					if (version.Major < 5)
					{
						return "PSv3Orv4";
					}
					return "PSv5OrLater";
				}
			}
			return "PSv2";
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000D7B42 File Offset: 0x000D5D42
		internal void OnRunspacePSEventReceived(object sender, PSEventArgs e)
		{
			if (base.Events != null)
			{
				base.Events.AddForwardedEvent(e);
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x0600269A RID: 9882 RVA: 0x000D7B58 File Offset: 0x000D5D58
		internal List<IThrottleOperation> Operations
		{
			get
			{
				return this.operations;
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000D7B60 File Offset: 0x000D5D60
		protected void CloseAllInputStreams()
		{
			foreach (IThrottleOperation throttleOperation in this.Operations)
			{
				ExecutionCmdletHelper executionCmdletHelper = (ExecutionCmdletHelper)throttleOperation;
				executionCmdletHelper.Pipeline.Input.Close();
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000D7BC4 File Offset: 0x000D5DC4
		private void WriteErrorCreateRemoteRunspaceFailed(Exception e, Uri uri)
		{
			ErrorRecord errorRecord = new ErrorRecord(e, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, uri);
			base.WriteError(errorRecord);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000D7BE8 File Offset: 0x000D5DE8
		protected ScriptBlock GetScriptBlockFromFile(string filePath, bool isLiteralPath)
		{
			if (!isLiteralPath && WildcardPattern.ContainsWildcardCharacters(filePath))
			{
				throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.WildCardErrorFilePathParameter, new object[0]), "filePath");
			}
			if (!filePath.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.FilePathShouldPS1Extension, new object[0]), "filePath");
			}
			PathResolver pathResolver = new PathResolver();
			string path = pathResolver.ResolveProviderAndPath(filePath, isLiteralPath, this, false, RemotingErrorIdStrings.FilePathNotFromFileSystemProvider);
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(filePath, path, base.Context);
			if (!filePath.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
			{
				base.Context.AuthorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Internal, base.Context.EngineHostInterface);
			}
			return externalScriptInfo.ScriptBlock;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000D7C9C File Offset: 0x000D5E9C
		protected override void BeginProcessing()
		{
			if (base.ParameterSetName == "VMId" || base.ParameterSetName == "VMName" || base.ParameterSetName == "ContainerId" || base.ParameterSetName == "ContainerName" || base.ParameterSetName == "FilePathVMId" || base.ParameterSetName == "FilePathVMName" || base.ParameterSetName == "FilePathContainerId" || base.ParameterSetName == "FilePathContainerName")
			{
				base.SkipWinRMCheck = true;
			}
			base.BeginProcessing();
			if (this.filePath != null)
			{
				this.scriptBlock = this.GetScriptBlockFromFile(this.filePath, this.IsLiteralPath);
			}
			try
			{
				bool isTrustedInput = base.Context.LanguageMode == PSLanguageMode.FullLanguage;
				this._powershell = this.scriptBlock.GetPowerShell(isTrustedInput, this.args);
			}
			catch (ScriptBlockToPowerShellNotSupportedException)
			{
			}
			string parameterSetName;
			switch (parameterSetName = base.ParameterSetName)
			{
			case "FilePathComputerName":
			case "LiteralFilePathComputerName":
			case "ComputerName":
			{
				string[] resolvedComputerNames = null;
				base.ResolveComputerNames(this.ComputerName, out resolvedComputerNames);
				base.ResolvedComputerNames = resolvedComputerNames;
				this.CreateHelpersForSpecifiedComputerNames();
				return;
			}
			case "FilePathRunspace":
			case "Session":
				base.ValidateRemoteRunspacesSpecified();
				this.CreateHelpersForSpecifiedRunspaces();
				return;
			case "FilePathUri":
			case "Uri":
				this.CreateHelpersForSpecifiedUris();
				return;
			case "VMId":
			case "VMName":
			case "FilePathVMId":
			case "FilePathVMName":
				this.CreateHelpersForSpecifiedVMSession();
				return;
			case "ContainerId":
			case "ContainerName":
			case "FilePathContainerId":
			case "FilePathContainerName":
				this.CreateHelpersForSpecifiedContainerSession();
				break;

				return;
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000D7F1C File Offset: 0x000D611C
		private PowerShell GetPowerShellForPSv2()
		{
			if (this._powershell != null)
			{
				return this._powershell;
			}
			List<string> list;
			List<object> list2;
			string convertedScript = this.GetConvertedScript(out list, out list2);
			this._powershell = PowerShell.Create().AddScript(convertedScript);
			if (this.args != null)
			{
				foreach (object value in this.args)
				{
					this._powershell.AddArgument(value);
				}
			}
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					this._powershell.AddParameter(list[j], list2[j]);
				}
			}
			return this._powershell;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000D7FC4 File Offset: 0x000D61C4
		private PowerShell GetPowerShellForPSv3OrLater(string serverPsVersion)
		{
			if (this._powershell != null)
			{
				return this._powershell;
			}
			bool isTrustedInput = base.Context.SessionState.LanguageMode != PSLanguageMode.NoLanguage;
			object[] array = null;
			IDictionary dictionary = null;
			if (serverPsVersion == "PSv3Orv4")
			{
				array = ScriptBlockToPowerShellConverter.GetUsingValuesAsArray(this.scriptBlock, isTrustedInput, base.Context, null);
				if (array == null)
				{
					return this.GetPowerShellForPSv2();
				}
			}
			else
			{
				dictionary = ScriptBlockToPowerShellConverter.GetUsingValuesAsDictionary(this.scriptBlock, isTrustedInput, base.Context, null);
			}
			string script = base.MyInvocation.ExpectingInput ? this.scriptBlock.GetWithInputHandlingForInvokeCommand() : this.scriptBlock.ToString();
			this._powershell = PowerShell.Create().AddScript(script);
			if (this.args != null)
			{
				foreach (object value in this.args)
				{
					this._powershell.AddArgument(value);
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				this._powershell.AddParameter("--%", dictionary);
			}
			else if (array != null && array.Length > 0)
			{
				this._powershell.AddParameter("--%", array);
			}
			return this._powershell;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000D80EC File Offset: 0x000D62EC
		private string GetConvertedScript(out List<string> newParameterNames, out List<object> newParameterValues)
		{
			newParameterNames = null;
			newParameterValues = null;
			List<VariableExpressionAst> usingVariables = this.GetUsingVariables(this.scriptBlock);
			string result;
			if (usingVariables == null || usingVariables.Count == 0)
			{
				result = (base.MyInvocation.ExpectingInput ? this.scriptBlock.GetWithInputHandlingForInvokeCommand() : this.scriptBlock.ToString());
			}
			else
			{
				newParameterNames = new List<string>();
				List<string> list = new List<string>();
				List<VariableExpressionAst> list2 = new List<VariableExpressionAst>();
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (VariableExpressionAst variableExpressionAst in usingVariables)
				{
					string userPath = variableExpressionAst.VariablePath.UserPath;
					string text = "__using_" + userPath;
					string item = "$" + text;
					if (!hashSet.Contains(item))
					{
						newParameterNames.Add(text);
						list.Add(item);
						list2.Add(variableExpressionAst);
						hashSet.Add(item);
					}
				}
				newParameterValues = this.GetUsingVariableValues(list2);
				string item2 = string.Join(", ", list);
				result = (base.MyInvocation.ExpectingInput ? this.scriptBlock.GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple.Create<List<VariableExpressionAst>, string>(usingVariables, item2)) : this.scriptBlock.ToStringWithDollarUsingHandling(Tuple.Create<List<VariableExpressionAst>, string>(usingVariables, item2)));
			}
			return result;
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000D8244 File Offset: 0x000D6444
		private List<object> GetUsingVariableValues(List<VariableExpressionAst> paramUsingVars)
		{
			List<object> list = new List<object>(paramUsingVars.Count);
			VariableExpressionAst variableExpressionAst = null;
			Version strictModeVersion = base.Context.EngineSessionState.CurrentScope.StrictModeVersion;
			try
			{
				base.Context.EngineSessionState.CurrentScope.StrictModeVersion = PSVersionInfo.PSVersion;
				bool isTrustedInput = base.Context.SessionState.LanguageMode != PSLanguageMode.NoLanguage;
				foreach (VariableExpressionAst variableExpressionAst2 in paramUsingVars)
				{
					variableExpressionAst = variableExpressionAst2;
					object expressionValue = Compiler.GetExpressionValue(variableExpressionAst2, isTrustedInput, base.Context, null);
					list.Add(expressionValue);
				}
			}
			catch (RuntimeException ex)
			{
				if (ex.ErrorRecord.FullyQualifiedErrorId.Equals("VariableIsUndefined", StringComparison.Ordinal))
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), variableExpressionAst.Extent, "UsingVariableIsUndefined", AutomationExceptions.UsingVariableIsUndefined, new object[]
					{
						ex.ErrorRecord.TargetObject
					});
				}
			}
			finally
			{
				base.Context.EngineSessionState.CurrentScope.StrictModeVersion = strictModeVersion;
			}
			return list;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000D839C File Offset: 0x000D659C
		private List<VariableExpressionAst> GetUsingVariables(ScriptBlock localScriptBlock)
		{
			if (localScriptBlock == null)
			{
				throw new ArgumentNullException("localScriptBlock", "Caller needs to make sure the parameter value is not null");
			}
			IEnumerable<Ast> source = UsingExpressionAstSearcher.FindAllUsingExpressionExceptForWorkflow(localScriptBlock.Ast);
			return (from usingExpr in source
			select UsingExpressionAst.ExtractUsingVariable((UsingExpressionAst)usingExpr)).ToList<VariableExpressionAst>();
		}

		// Token: 0x04001301 RID: 4865
		protected const string VMIdParameterSet = "VMId";

		// Token: 0x04001302 RID: 4866
		protected const string VMNameParameterSet = "VMName";

		// Token: 0x04001303 RID: 4867
		protected const string ContainerIdParameterSet = "ContainerId";

		// Token: 0x04001304 RID: 4868
		protected const string ContainerNameParameterSet = "ContainerName";

		// Token: 0x04001305 RID: 4869
		protected const string FilePathVMIdParameterSet = "FilePathVMId";

		// Token: 0x04001306 RID: 4870
		protected const string FilePathVMNameParameterSet = "FilePathVMName";

		// Token: 0x04001307 RID: 4871
		protected const string FilePathContainerIdParameterSet = "FilePathContainerId";

		// Token: 0x04001308 RID: 4872
		protected const string FilePathContainerNameParameterSet = "FilePathContainerName";

		// Token: 0x04001309 RID: 4873
		protected const string FilePathComputerNameParameterSet = "FilePathComputerName";

		// Token: 0x0400130A RID: 4874
		protected const string LiteralFilePathComputerNameParameterSet = "LiteralFilePathComputerName";

		// Token: 0x0400130B RID: 4875
		protected const string FilePathSessionParameterSet = "FilePathRunspace";

		// Token: 0x0400130C RID: 4876
		protected const string FilePathUriParameterSet = "FilePathUri";

		// Token: 0x0400130D RID: 4877
		private const string PSv5OrLater = "PSv5OrLater";

		// Token: 0x0400130E RID: 4878
		private const string PSv3Orv4 = "PSv3Orv4";

		// Token: 0x0400130F RID: 4879
		private const string PSv2 = "PSv2";

		// Token: 0x04001310 RID: 4880
		private PSObject inputObject = AutomationNull.Value;

		// Token: 0x04001311 RID: 4881
		private ScriptBlock scriptBlock;

		// Token: 0x04001312 RID: 4882
		private string filePath;

		// Token: 0x04001313 RID: 4883
		private object[] args;

		// Token: 0x04001314 RID: 4884
		private bool invokeAndDisconnect;

		// Token: 0x04001315 RID: 4885
		private string[] sessionName;

		// Token: 0x04001316 RID: 4886
		private SwitchParameter enableNetworkAccess;

		// Token: 0x04001317 RID: 4887
		private Guid[] vmId;

		// Token: 0x04001318 RID: 4888
		private string[] vmName;

		// Token: 0x04001319 RID: 4889
		private string[] containerId;

		// Token: 0x0400131A RID: 4890
		private string[] containerName;

		// Token: 0x0400131B RID: 4891
		private SwitchParameter runAsAdministrator;

		// Token: 0x0400131C RID: 4892
		private List<IThrottleOperation> operations = new List<IThrottleOperation>();

		// Token: 0x0400131D RID: 4893
		private PowerShell _powershell;
	}
}
