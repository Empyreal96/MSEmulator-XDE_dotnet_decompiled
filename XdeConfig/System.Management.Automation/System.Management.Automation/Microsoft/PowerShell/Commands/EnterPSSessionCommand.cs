using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000349 RID: 841
	[Cmdlet("Enter", "PSSession", DefaultParameterSetName = "ComputerName", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135210", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class EnterPSSessionCommand : PSRemotingBaseCmdlet
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000E614A File Offset: 0x000E434A
		// (set) Token: 0x0600292C RID: 10540 RVA: 0x000E6148 File Offset: 0x000E4348
		public new int ThrottleLimit
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000E614D File Offset: 0x000E434D
		// (set) Token: 0x0600292F RID: 10543 RVA: 0x000E6155 File Offset: 0x000E4355
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Alias(new string[]
		{
			"Cn"
		})]
		public new string ComputerName
		{
			get
			{
				return this.computerName;
			}
			set
			{
				this.computerName = value;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x000E615E File Offset: 0x000E435E
		// (set) Token: 0x06002931 RID: 10545 RVA: 0x000E6166 File Offset: 0x000E4366
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Session")]
		public new PSSession Session
		{
			get
			{
				return this.remoteRunspaceInfo;
			}
			set
			{
				this.remoteRunspaceInfo = value;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x000E616F File Offset: 0x000E436F
		// (set) Token: 0x06002933 RID: 10547 RVA: 0x000E6177 File Offset: 0x000E4377
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
		[Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		[ValidateNotNullOrEmpty]
		public new Uri ConnectionUri
		{
			get
			{
				return this.connectionUri;
			}
			set
			{
				this.connectionUri = value;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06002934 RID: 10548 RVA: 0x000E6180 File Offset: 0x000E4380
		// (set) Token: 0x06002935 RID: 10549 RVA: 0x000E6188 File Offset: 0x000E4388
		[ValidateNotNull]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "InstanceId")]
		public Guid InstanceId
		{
			get
			{
				return this.remoteRunspaceId;
			}
			set
			{
				this.remoteRunspaceId = value;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x000E6191 File Offset: 0x000E4391
		// (set) Token: 0x06002937 RID: 10551 RVA: 0x000E6199 File Offset: 0x000E4399
		[ValidateNotNull]
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "Id")]
		public int Id
		{
			get
			{
				return this.sessionId;
			}
			set
			{
				this.sessionId = value;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x000E61A2 File Offset: 0x000E43A2
		// (set) Token: 0x06002939 RID: 10553 RVA: 0x000E61AA File Offset: 0x000E43AA
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x000E61B3 File Offset: 0x000E43B3
		// (set) Token: 0x0600293B RID: 10555 RVA: 0x000E61BB File Offset: 0x000E43BB
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Uri")]
		public SwitchParameter EnableNetworkAccess
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

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600293C RID: 10556 RVA: 0x000E61C4 File Offset: 0x000E43C4
		// (set) Token: 0x0600293D RID: 10557 RVA: 0x000E61CC File Offset: 0x000E43CC
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"VMGuid"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMId")]
		public Guid VMId
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

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x000E61D5 File Offset: 0x000E43D5
		// (set) Token: 0x0600293F RID: 10559 RVA: 0x000E61DD File Offset: 0x000E43DD
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMName")]
		[ValidateNotNullOrEmpty]
		public string VMName
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

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06002940 RID: 10560 RVA: 0x000E61E6 File Offset: 0x000E43E6
		// (set) Token: 0x06002941 RID: 10561 RVA: 0x000E61EE File Offset: 0x000E43EE
		[Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMName")]
		[Credential]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		[Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "VMId")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
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

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06002942 RID: 10562 RVA: 0x000E61F7 File Offset: 0x000E43F7
		// (set) Token: 0x06002943 RID: 10563 RVA: 0x000E61FF File Offset: 0x000E43FF
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ContainerId")]
		public string ContainerId
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

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x000E6208 File Offset: 0x000E4408
		// (set) Token: 0x06002945 RID: 10565 RVA: 0x000E6210 File Offset: 0x000E4410
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ContainerName")]
		[ValidateNotNullOrEmpty]
		public string ContainerName
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

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x000E6219 File Offset: 0x000E4419
		// (set) Token: 0x06002947 RID: 10567 RVA: 0x000E6221 File Offset: 0x000E4421
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "ContainerName")]
		public SwitchParameter RunAsAdministrator
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

		// Token: 0x06002948 RID: 10568 RVA: 0x000E622C File Offset: 0x000E442C
		protected override void ProcessRecord()
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = base.Host as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.HostDoesNotSupportPushRunspace)), PSRemotingErrorId.HostDoesNotSupportPushRunspace.ToString(), ErrorCategory.InvalidArgument, null));
				return;
			}
			if (!this.IsParameterSetForVMSession() && !this.IsParameterSetForContainerSession() && base.Context != null && base.Context.EngineHostInterface != null && base.Context.EngineHostInterface.ExternalHost != null && base.Context.EngineHostInterface.ExternalHost is ServerRemoteHost)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.RemoteHostDoesNotSupportPushRunspace)), PSRemotingErrorId.RemoteHostDoesNotSupportPushRunspace.ToString(), ErrorCategory.InvalidArgument, null));
				return;
			}
			InternalHost internalHost = base.Host as InternalHost;
			if (!this.IsParameterSetForVMSession() && !this.IsParameterSetForContainerSession() && internalHost != null && internalHost.HostInNestedPrompt())
			{
				base.ThrowTerminatingError(new ErrorRecord(new InvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.HostInNestedPrompt, new object[0])), "HostInNestedPrompt", ErrorCategory.InvalidOperation, internalHost));
			}
			RemoteRunspace remoteRunspace = null;
			string parameterSetName;
			switch (parameterSetName = base.ParameterSetName)
			{
			case "ComputerName":
				remoteRunspace = this.CreateRunspaceWhenComputerNameParameterSpecified();
				break;
			case "Uri":
				remoteRunspace = this.CreateRunspaceWhenUriParameterSpecified();
				break;
			case "Session":
				remoteRunspace = (RemoteRunspace)this.remoteRunspaceInfo.Runspace;
				break;
			case "InstanceId":
				remoteRunspace = this.GetRunspaceMatchingRunspaceId(this.InstanceId);
				break;
			case "Id":
				remoteRunspace = this.GetRunspaceMatchingSessionId(this.Id);
				break;
			case "Name":
				remoteRunspace = this.GetRunspaceMatchingName(this.Name);
				break;
			case "VMId":
			case "VMName":
				remoteRunspace = this.GetRunspaceForVMSession();
				break;
			case "ContainerId":
			case "ContainerName":
				remoteRunspace = this.GetRunspaceForContainerSession();
				break;
			}
			if (remoteRunspace == null)
			{
				return;
			}
			bool flag = false;
			if (remoteRunspace.RunspaceStateInfo.State == RunspaceState.Disconnected)
			{
				if (!remoteRunspace.CanConnect)
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.SessionNotAvailableForConnection, new object[0]);
					base.WriteError(new ErrorRecord(new RuntimeException(message), "EnterPSSessionCannotConnectDisconnectedSession", ErrorCategory.InvalidOperation, remoteRunspace));
					return;
				}
				Exception ex = null;
				try
				{
					remoteRunspace.Connect();
					flag = true;
				}
				catch (PSRemotingTransportException ex2)
				{
					ex = ex2;
				}
				catch (PSInvalidOperationException ex3)
				{
					ex = ex3;
				}
				catch (InvalidRunspacePoolStateException ex4)
				{
					ex = ex4;
				}
				if (ex != null)
				{
					string message2 = StringUtil.Format(RemotingErrorIdStrings.SessionConnectFailed, new object[0]);
					base.WriteError(new ErrorRecord(new RuntimeException(message2, ex), "EnterPSSessionConnectSessionFailed", ErrorCategory.InvalidOperation, remoteRunspace));
					return;
				}
			}
			if (remoteRunspace.RunspaceStateInfo.State == RunspaceState.Opened)
			{
				Debugger debugger = null;
				try
				{
					if (hostSupportsInteractiveSession.Runspace != null)
					{
						debugger = hostSupportsInteractiveSession.Runspace.Debugger;
					}
				}
				catch (PSNotImplementedException)
				{
				}
				bool flag2 = debugger != null && (debugger.DebugMode & DebugModes.RemoteScript) == DebugModes.RemoteScript;
				if (remoteRunspace.RunspaceAvailability != RunspaceAvailability.Available)
				{
					if (!flag2)
					{
						if (flag)
						{
							string message3 = StringUtil.Format(RemotingErrorIdStrings.EnterPSSessionDisconnected, remoteRunspace.PSSessionName);
							base.WriteError(new ErrorRecord(new RuntimeException(message3), "EnterPSSessionConnectSessionNotAvailable", ErrorCategory.InvalidOperation, this.remoteRunspaceInfo));
							remoteRunspace.DisconnectAsync();
							return;
						}
						base.WriteWarning(base.GetMessage(RunspaceStrings.RunspaceNotReady));
					}
					else
					{
						Job job = this.FindJobForRunspace(remoteRunspace.InstanceId);
						string text;
						if (job != null)
						{
							text = StringUtil.Format(RunspaceStrings.RunningCmdWithJob, (!string.IsNullOrEmpty(job.Name)) ? job.Name : string.Empty);
						}
						else if (remoteRunspace.RunspaceAvailability == RunspaceAvailability.RemoteDebug)
						{
							text = StringUtil.Format(RunspaceStrings.RunningCmdDebugStop, new object[0]);
						}
						else
						{
							text = StringUtil.Format(RunspaceStrings.RunningCmdWithoutJob, new object[0]);
						}
						base.WriteWarning(text);
					}
				}
				if (this.remoteRunspaceInfo != null)
				{
					base.RunspaceRepository.AddOrReplace(this.remoteRunspaceInfo);
				}
				this.SetRunspacePrompt(remoteRunspace);
				try
				{
					hostSupportsInteractiveSession.PushRunspace(remoteRunspace);
				}
				catch (Exception)
				{
					if (remoteRunspace != null && remoteRunspace.ShouldCloseOnPop)
					{
						remoteRunspace.Close();
					}
					throw;
				}
				return;
			}
			if (base.ParameterSetName == "Session")
			{
				string text2 = (this.remoteRunspaceInfo != null) ? this.remoteRunspaceInfo.Name : string.Empty;
				base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.EnterPSSessionBrokenSession, new object[]
				{
					text2,
					remoteRunspace.ConnectionInfo.ComputerName,
					remoteRunspace.InstanceId
				})), PSRemotingErrorId.PushedRunspaceMustBeOpen.ToString(), ErrorCategory.InvalidArgument, null));
				return;
			}
			base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.PushedRunspaceMustBeOpen)), PSRemotingErrorId.PushedRunspaceMustBeOpen.ToString(), ErrorCategory.InvalidArgument, null));
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000E6774 File Offset: 0x000E4974
		protected override void EndProcessing()
		{
			if (this.stream != null)
			{
				for (;;)
				{
					this.stream.ObjectReader.WaitHandle.WaitOne();
					if (this.stream.ObjectReader.EndOfPipeline)
					{
						break;
					}
					object obj = this.stream.ObjectReader.Read();
					base.WriteStreamObject((Action<Cmdlet>)obj);
				}
			}
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000E67D0 File Offset: 0x000E49D0
		protected override void StopProcessing()
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = base.Host as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.HostDoesNotSupportPushRunspace)), PSRemotingErrorId.HostDoesNotSupportPushRunspace.ToString(), ErrorCategory.InvalidArgument, null));
				return;
			}
			hostSupportsInteractiveSession.PopRunspace();
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000E6820 File Offset: 0x000E4A20
		private RemoteRunspace CreateTemporaryRemoteRunspace(PSHost host, WSManConnectionInfo connectionInfo)
		{
			int id;
			string text = PSSession.GenerateRunspaceName(out id);
			RemoteRunspace remoteRunspace = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), connectionInfo, host, this.SessionOption.ApplicationArguments, text, id);
			remoteRunspace.URIRedirectionReported += this.HandleURIDirectionReported;
			this.stream = new ObjectStream();
			try
			{
				remoteRunspace.Open();
				remoteRunspace.ShouldCloseOnPop = true;
			}
			finally
			{
				remoteRunspace.URIRedirectionReported -= this.HandleURIDirectionReported;
				this.stream.ObjectWriter.Close();
				if (remoteRunspace.RunspaceStateInfo.State != RunspaceState.Opened)
				{
					remoteRunspace.Dispose();
					remoteRunspace = null;
				}
			}
			return remoteRunspace;
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000E68C8 File Offset: 0x000E4AC8
		private void WriteErrorCreateRemoteRunspaceFailed(Exception exception, object argument)
		{
			PSRemotingTransportException ex = exception as PSRemotingTransportException;
			string errorDetails_Message = null;
			if (ex != null && ex.ErrorCode == -2144108135)
			{
				string text = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.URIRedirectionReported, new object[]
				{
					ex.Message,
					"MaximumConnectionRedirectionCount",
					"PSSessionOption",
					"AllowRedirection"
				});
				errorDetails_Message = text;
			}
			ErrorRecord errorRecord = new ErrorRecord(exception, argument, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, null, null, null, null, null, errorDetails_Message, null);
			base.WriteError(errorRecord);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000E6948 File Offset: 0x000E4B48
		private void WriteInvalidArgumentError(PSRemotingErrorId errorId, string resourceString, object errorArgument)
		{
			string message = base.GetMessage(resourceString, new object[]
			{
				errorArgument
			});
			base.WriteError(new ErrorRecord(new ArgumentException(message), errorId.ToString(), ErrorCategory.InvalidArgument, errorArgument));
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000E69A0 File Offset: 0x000E4BA0
		private void HandleURIDirectionReported(object sender, RemoteDataEventArgs<Uri> eventArgs)
		{
			string message = StringUtil.Format(RemotingErrorIdStrings.URIRedirectWarningToHost, eventArgs.Data.OriginalString);
			Action<Cmdlet> value = delegate(Cmdlet cmdlet)
			{
				cmdlet.WriteWarning(message);
			};
			this.stream.Write(value);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000E69E8 File Offset: 0x000E4BE8
		private RemoteRunspace CreateRunspaceWhenComputerNameParameterSpecified()
		{
			RemoteRunspace result = null;
			string argument = base.ResolveComputerName(this.computerName);
			try
			{
				WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
				string scheme = this.UseSSL.IsPresent ? "https" : "http";
				wsmanConnectionInfo.ComputerName = argument;
				wsmanConnectionInfo.Port = this.Port;
				wsmanConnectionInfo.AppName = this.ApplicationName;
				wsmanConnectionInfo.ShellUri = this.ConfigurationName;
				wsmanConnectionInfo.Scheme = scheme;
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
				result = this.CreateTemporaryRemoteRunspace(base.Host, wsmanConnectionInfo);
			}
			catch (InvalidOperationException exception)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception, argument);
			}
			catch (ArgumentException exception2)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception2, argument);
			}
			catch (PSRemotingTransportException exception3)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception3, argument);
			}
			return result;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000E6B04 File Offset: 0x000E4D04
		private RemoteRunspace CreateRunspaceWhenUriParameterSpecified()
		{
			RemoteRunspace result = null;
			try
			{
				WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
				wsmanConnectionInfo.ConnectionUri = this.ConnectionUri;
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
				result = this.CreateTemporaryRemoteRunspace(base.Host, wsmanConnectionInfo);
			}
			catch (UriFormatException exception)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception, this.ConnectionUri);
			}
			catch (InvalidOperationException exception2)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception2, this.ConnectionUri);
			}
			catch (ArgumentException exception3)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception3, this.ConnectionUri);
			}
			catch (PSRemotingTransportException exception4)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception4, this.ConnectionUri);
			}
			catch (NotSupportedException exception5)
			{
				this.WriteErrorCreateRemoteRunspaceFailed(exception5, this.ConnectionUri);
			}
			return result;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000E6C20 File Offset: 0x000E4E20
		private RemoteRunspace GetRunspaceMatchingCondition(Predicate<PSSession> condition, PSRemotingErrorId tooFew, PSRemotingErrorId tooMany, string tooFewResourceString, string tooManyResourceString, object errorArgument)
		{
			List<PSSession> list = base.RunspaceRepository.Runspaces.FindAll(condition);
			RemoteRunspace result = null;
			if (list.Count == 0)
			{
				this.WriteInvalidArgumentError(tooFew, tooFewResourceString, errorArgument);
			}
			else if (list.Count > 1)
			{
				this.WriteInvalidArgumentError(tooMany, tooManyResourceString, errorArgument);
			}
			else
			{
				result = (RemoteRunspace)list[0].Runspace;
			}
			return result;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000E6C9C File Offset: 0x000E4E9C
		private RemoteRunspace GetRunspaceMatchingRunspaceId(Guid remoteRunspaceId)
		{
			Predicate<PSSession> condition = (PSSession info) => info.InstanceId == remoteRunspaceId;
			PSRemotingErrorId tooFew = PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedRunspaceId;
			PSRemotingErrorId tooMany = PSRemotingErrorId.RemoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId;
			string remoteRunspaceNotAvailableForSpecifiedRunspaceId = RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedRunspaceId;
			string remoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId = RemotingErrorIdStrings.RemoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId;
			return this.GetRunspaceMatchingCondition(condition, tooFew, tooMany, remoteRunspaceNotAvailableForSpecifiedRunspaceId, remoteRunspaceHasMultipleMatchesForSpecifiedRunspaceId, remoteRunspaceId);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000E6D10 File Offset: 0x000E4F10
		private RemoteRunspace GetRunspaceMatchingSessionId(int sessionId)
		{
			Predicate<PSSession> condition = (PSSession info) => info.Id == sessionId;
			PSRemotingErrorId tooFew = PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedSessionId;
			PSRemotingErrorId tooMany = PSRemotingErrorId.RemoteRunspaceHasMultipleMatchesForSpecifiedSessionId;
			string remoteRunspaceNotAvailableForSpecifiedSessionId = RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedSessionId;
			string remoteRunspaceHasMultipleMatchesForSpecifiedSessionId = RemotingErrorIdStrings.RemoteRunspaceHasMultipleMatchesForSpecifiedSessionId;
			return this.GetRunspaceMatchingCondition(condition, tooFew, tooMany, remoteRunspaceNotAvailableForSpecifiedSessionId, remoteRunspaceHasMultipleMatchesForSpecifiedSessionId, sessionId);
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000E6D88 File Offset: 0x000E4F88
		private RemoteRunspace GetRunspaceMatchingName(string name)
		{
			Predicate<PSSession> condition = (PSSession info) => info.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
			PSRemotingErrorId tooFew = PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedName;
			PSRemotingErrorId tooMany = PSRemotingErrorId.RemoteRunspaceHasMultipleMatchesForSpecifiedName;
			string remoteRunspaceNotAvailableForSpecifiedName = RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedName;
			string remoteRunspaceHasMultipleMatchesForSpecifiedName = RemotingErrorIdStrings.RemoteRunspaceHasMultipleMatchesForSpecifiedName;
			return this.GetRunspaceMatchingCondition(condition, tooFew, tooMany, remoteRunspaceNotAvailableForSpecifiedName, remoteRunspaceHasMultipleMatchesForSpecifiedName, name);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000E6DE0 File Offset: 0x000E4FE0
		private Job FindJobForRunspace(Guid id)
		{
			foreach (Job job in base.JobRepository.Jobs)
			{
				foreach (Job job2 in job.ChildJobs)
				{
					PSRemotingChildJob psremotingChildJob = job2 as PSRemotingChildJob;
					if (psremotingChildJob != null && psremotingChildJob.Runspace != null && psremotingChildJob.JobStateInfo.State == JobState.Running && psremotingChildJob.Runspace.InstanceId.Equals(id))
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000E6EAC File Offset: 0x000E50AC
		private bool IsParameterSetForVMSession()
		{
			return base.ParameterSetName == "VMId" || base.ParameterSetName == "VMName";
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000E6ED5 File Offset: 0x000E50D5
		private bool IsParameterSetForContainerSession()
		{
			return base.ParameterSetName == "ContainerId" || base.ParameterSetName == "ContainerName";
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000E6EFC File Offset: 0x000E50FC
		private RemoteRunspace GetRunspaceForVMSession()
		{
			RemoteRunspace result = null;
			if (base.ParameterSetName == "VMId")
			{
				string script = "Get-VM -Id $args[0]";
				Collection<PSObject> collection;
				try
				{
					collection = base.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null, new object[]
					{
						this.VMId
					});
				}
				catch (CommandNotFoundException)
				{
					base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HyperVModuleNotAvailable), PSRemotingErrorId.HyperVModuleNotAvailable.ToString(), ErrorCategory.NotInstalled, null));
					return null;
				}
				if (collection.Count != 1)
				{
					base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.InvalidVMId), PSRemotingErrorId.InvalidVMId.ToString(), ErrorCategory.InvalidArgument, null));
					return null;
				}
				this.VMName = (string)collection[0].Properties["VMName"].Value;
			}
			else
			{
				string script = "Get-VM -Name $args";
				Collection<PSObject> collection;
				try
				{
					collection = base.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null, new object[]
					{
						this.VMName
					});
				}
				catch (CommandNotFoundException)
				{
					base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HyperVModuleNotAvailable), PSRemotingErrorId.HyperVModuleNotAvailable.ToString(), ErrorCategory.NotInstalled, null));
					return null;
				}
				if (collection.Count == 0)
				{
					base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.InvalidVMNameNoVM), PSRemotingErrorId.InvalidVMNameNoVM.ToString(), ErrorCategory.InvalidArgument, null));
					return null;
				}
				if (collection.Count > 1)
				{
					base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.InvalidVMNameMultipleVM), PSRemotingErrorId.InvalidVMNameMultipleVM.ToString(), ErrorCategory.InvalidArgument, null));
					return null;
				}
				this.VMId = (Guid)collection[0].Properties["VMId"].Value;
				this.VMName = (string)collection[0].Properties["VMName"].Value;
			}
			try
			{
				VMConnectionInfo connectionInfo = new VMConnectionInfo(this.Credential, this.VMId, this.VMName);
				result = this.CreateTemporaryRemoteRunspaceForPowerShellDirect(base.Host, connectionInfo);
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
			return result;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000E7180 File Offset: 0x000E5380
		private RemoteRunspace CreateTemporaryRemoteRunspaceForPowerShellDirect(PSHost host, RunspaceConnectionInfo connectionInfo)
		{
			TypeTable typeTable = TypeTable.LoadDefaultTypeFiles();
			RemoteRunspace remoteRunspace = RunspaceFactory.CreateRunspace(connectionInfo, host, typeTable) as RemoteRunspace;
			remoteRunspace.Name = "PowerShellDirectAttach";
			try
			{
				remoteRunspace.Open();
				remoteRunspace.ShouldCloseOnPop = true;
			}
			finally
			{
				if (remoteRunspace.RunspaceStateInfo.State != RunspaceState.Opened)
				{
					remoteRunspace.Dispose();
					remoteRunspace = null;
				}
			}
			return remoteRunspace;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000E71E4 File Offset: 0x000E53E4
		private void SetRunspacePrompt(RemoteRunspace remoteRunspace)
		{
			if (this.IsParameterSetForVMSession() || this.IsParameterSetForContainerSession())
			{
				string script = StringUtil.Format(RemotingErrorIdStrings.EnterVMSessionPrompt, new object[]
				{
					"function global:prompt { \"",
					this.IsParameterSetForVMSession() ? this.VMName : this.ContainerName,
					"PS $($executionContext.SessionState.Path.CurrentLocation)> \" }"
				});
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.Runspace = remoteRunspace;
					try
					{
						powerShell.AddScript(script).Invoke();
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000E728C File Offset: 0x000E548C
		private RemoteRunspace GetRunspaceForContainerSession()
		{
			RemoteRunspace result = null;
			try
			{
				ContainerConnectionInfo containerConnectionInfo;
				if (!string.IsNullOrEmpty(this.ContainerId))
				{
					containerConnectionInfo = ContainerConnectionInfo.CreateContainerConnectionInfoById(this.ContainerId, this.RunAsAdministrator.IsPresent);
				}
				else
				{
					containerConnectionInfo = ContainerConnectionInfo.CreateContainerConnectionInfoByName(this.ContainerName, this.RunAsAdministrator.IsPresent);
				}
				this.ContainerName = containerConnectionInfo.ComputerName;
				containerConnectionInfo.CreateContainerProcess();
				result = this.CreateTemporaryRemoteRunspaceForPowerShellDirect(base.Host, containerConnectionInfo);
			}
			catch (InvalidOperationException exception)
			{
				ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidOperation, null);
				base.WriteError(errorRecord);
			}
			catch (ArgumentException exception2)
			{
				ErrorRecord errorRecord2 = new ErrorRecord(exception2, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidArgument, null);
				base.WriteError(errorRecord2);
			}
			catch (Exception exception3)
			{
				ErrorRecord errorRecord3 = new ErrorRecord(exception3, "CreateRemoteRunspaceForContainerFailed", ErrorCategory.InvalidOperation, null);
				base.WriteError(errorRecord3);
			}
			return result;
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000E73C4 File Offset: 0x000E55C4
		internal static RemotePipeline ConnectRunningPipeline(RemoteRunspace remoteRunspace)
		{
			RemotePipeline remotePipeline = null;
			if (remoteRunspace.RemoteCommand != null)
			{
				remotePipeline = new RemotePipeline(remoteRunspace);
			}
			else
			{
				remotePipeline = (remoteRunspace.GetCurrentlyRunningPipeline() as RemotePipeline);
			}
			if (remotePipeline != null && remotePipeline.PipelineStateInfo.State == PipelineState.Disconnected)
			{
				using (ManualResetEvent connected = new ManualResetEvent(false))
				{
					remotePipeline.StateChanged += delegate(object sender, PipelineStateEventArgs args)
					{
						if (args.PipelineStateInfo.State != PipelineState.Disconnected)
						{
							try
							{
								connected.Set();
							}
							catch (ObjectDisposedException)
							{
							}
						}
					};
					remotePipeline.ConnectAsync();
					connected.WaitOne();
				}
			}
			return remotePipeline;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000E7468 File Offset: 0x000E5668
		internal static void ContinueCommand(RemoteRunspace remoteRunspace, Pipeline cmd, PSHost host, bool inDebugMode, System.Management.Automation.ExecutionContext context)
		{
			RemotePipeline remotePipeline = cmd as RemotePipeline;
			if (remotePipeline != null)
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					PSInvocationSettings settings = new PSInvocationSettings
					{
						Host = host
					};
					PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
					CommandInfo commandInfo = new CmdletInfo("Out-Default", typeof(OutDefaultCommand), null, null, context);
					Command command = new Command(commandInfo);
					powerShell.AddCommand(command);
					IAsyncResult asyncResult = powerShell.BeginInvoke<PSObject>(psdataCollection, settings, null, null);
					RemoteDebugger remoteDebugger = remoteRunspace.Debugger as RemoteDebugger;
					if (remoteDebugger != null)
					{
						remoteDebugger.SendBreakpointUpdatedEvents();
						if (!inDebugMode)
						{
							remoteDebugger.CheckStateAndRaiseStopEvent();
						}
					}
					while (!remotePipeline.Output.EndOfPipeline)
					{
						remotePipeline.Output.WaitHandle.WaitOne();
						while (remotePipeline.Output.Count > 0)
						{
							psdataCollection.Add(remotePipeline.Output.Read());
						}
					}
					psdataCollection.Complete();
					powerShell.EndInvoke(asyncResult);
				}
			}
		}

		// Token: 0x04001455 RID: 5205
		private const string InstanceIdParameterSet = "InstanceId";

		// Token: 0x04001456 RID: 5206
		private const string IdParameterSet = "Id";

		// Token: 0x04001457 RID: 5207
		private const string NameParameterSet = "Name";

		// Token: 0x04001458 RID: 5208
		private const string VMIdParameterSet = "VMId";

		// Token: 0x04001459 RID: 5209
		private const string VMNameParameterSet = "VMName";

		// Token: 0x0400145A RID: 5210
		private const string ContainerIdParameterSet = "ContainerId";

		// Token: 0x0400145B RID: 5211
		private const string ContainerNameParameterSet = "ContainerName";

		// Token: 0x0400145C RID: 5212
		private ObjectStream stream;

		// Token: 0x0400145D RID: 5213
		private string computerName;

		// Token: 0x0400145E RID: 5214
		private PSSession remoteRunspaceInfo;

		// Token: 0x0400145F RID: 5215
		private Uri connectionUri;

		// Token: 0x04001460 RID: 5216
		private Guid remoteRunspaceId;

		// Token: 0x04001461 RID: 5217
		private int sessionId;

		// Token: 0x04001462 RID: 5218
		private string name;

		// Token: 0x04001463 RID: 5219
		private SwitchParameter enableNetworkAccess;

		// Token: 0x04001464 RID: 5220
		private Guid vmId;

		// Token: 0x04001465 RID: 5221
		private string vmName;

		// Token: 0x04001466 RID: 5222
		private string containerId;

		// Token: 0x04001467 RID: 5223
		private string containerName;

		// Token: 0x04001468 RID: 5224
		private SwitchParameter runAsAdministrator;
	}
}
