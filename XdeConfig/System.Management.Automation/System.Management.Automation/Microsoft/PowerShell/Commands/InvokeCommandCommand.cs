using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000326 RID: 806
	[Cmdlet("Invoke", "Command", DefaultParameterSetName = "InProcess", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135225", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class InvokeCommandCommand : PSExecutionCmdlet, IDisposable
	{
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x000D840E File Offset: 0x000D660E
		// (set) Token: 0x060026A7 RID: 9895 RVA: 0x000D8416 File Offset: 0x000D6616
		[Parameter(Position = 0, ParameterSetName = "Session")]
		[Parameter(Position = 0, ParameterSetName = "FilePathRunspace")]
		[ValidateNotNullOrEmpty]
		public override PSSession[] Session
		{
			get
			{
				return base.Session;
			}
			set
			{
				base.Session = value;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000D841F File Offset: 0x000D661F
		// (set) Token: 0x060026A9 RID: 9897 RVA: 0x000D8427 File Offset: 0x000D6627
		[Alias(new string[]
		{
			"Cn"
		})]
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ParameterSetName = "ComputerName")]
		[Parameter(Position = 0, ParameterSetName = "FilePathComputerName")]
		public override string[] ComputerName
		{
			get
			{
				return base.ComputerName;
			}
			set
			{
				base.ComputerName = value;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000D8430 File Offset: 0x000D6630
		// (set) Token: 0x060026AB RID: 9899 RVA: 0x000D8438 File Offset: 0x000D6638
		[Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "VMName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathUri")]
		[Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "VMId")]
		[Credential]
		[Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "FilePathVMId")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "FilePathVMName")]
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

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x000D8441 File Offset: 0x000D6641
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x000D8449 File Offset: 0x000D6649
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[ValidateRange(1, 65535)]
		public override int Port
		{
			get
			{
				return base.Port;
			}
			set
			{
				base.Port = value;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x000D8452 File Offset: 0x000D6652
		// (set) Token: 0x060026AF RID: 9903 RVA: 0x000D845A File Offset: 0x000D665A
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		public override SwitchParameter UseSSL
		{
			get
			{
				return base.UseSSL;
			}
			set
			{
				base.UseSSL = value;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000D8463 File Offset: 0x000D6663
		// (set) Token: 0x060026B1 RID: 9905 RVA: 0x000D846B File Offset: 0x000D666B
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathUri")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
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

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000D8474 File Offset: 0x000D6674
		// (set) Token: 0x060026B3 RID: 9907 RVA: 0x000D847C File Offset: 0x000D667C
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		public override string ApplicationName
		{
			get
			{
				return base.ApplicationName;
			}
			set
			{
				base.ApplicationName = value;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000D848E File Offset: 0x000D668E
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x000D8485 File Offset: 0x000D6685
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathRunspace")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "VMId")]
		[Parameter(ParameterSetName = "VMName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathVMId")]
		[Parameter(ParameterSetName = "FilePathVMName")]
		[Parameter(ParameterSetName = "FilePathContainerName")]
		public override int ThrottleLimit
		{
			get
			{
				return base.ThrottleLimit;
			}
			set
			{
				base.ThrottleLimit = value;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000D8496 File Offset: 0x000D6696
		// (set) Token: 0x060026B7 RID: 9911 RVA: 0x000D849E File Offset: 0x000D669E
		[Parameter(Position = 0, ParameterSetName = "Uri")]
		[Parameter(Position = 0, ParameterSetName = "FilePathUri")]
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
		public override Uri[] ConnectionUri
		{
			get
			{
				return base.ConnectionUri;
			}
			set
			{
				base.ConnectionUri = value;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x000D84A7 File Offset: 0x000D66A7
		// (set) Token: 0x060026B9 RID: 9913 RVA: 0x000D84B4 File Offset: 0x000D66B4
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "FilePathContainerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathRunspace")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "VMId")]
		[Parameter(ParameterSetName = "VMName")]
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathVMId")]
		[Parameter(ParameterSetName = "FilePathVMName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		public SwitchParameter AsJob
		{
			get
			{
				return this.asjob;
			}
			set
			{
				this.asjob = value;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x000D84C2 File Offset: 0x000D66C2
		// (set) Token: 0x060026BB RID: 9915 RVA: 0x000D84CF File Offset: 0x000D66CF
		[Alias(new string[]
		{
			"Disconnected"
		})]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		public SwitchParameter InDisconnectedSession
		{
			get
			{
				return base.InvokeAndDisconnect;
			}
			set
			{
				base.InvokeAndDisconnect = value;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x000D84DD File Offset: 0x000D66DD
		// (set) Token: 0x060026BD RID: 9917 RVA: 0x000D84E5 File Offset: 0x000D66E5
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		public string[] SessionName
		{
			get
			{
				return base.DisconnectedSessionName;
			}
			set
			{
				base.DisconnectedSessionName = value;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000D84EE File Offset: 0x000D66EE
		// (set) Token: 0x060026BF RID: 9919 RVA: 0x000D84FB File Offset: 0x000D66FB
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "VMName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		[Alias(new string[]
		{
			"HCN"
		})]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathRunspace")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "VMId")]
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathVMId")]
		[Parameter(ParameterSetName = "FilePathVMName")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "FilePathContainerName")]
		public SwitchParameter HideComputerName
		{
			get
			{
				return this.hideComputerName;
			}
			set
			{
				this.hideComputerName = value;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000D8509 File Offset: 0x000D6709
		// (set) Token: 0x060026C1 RID: 9921 RVA: 0x000D8511 File Offset: 0x000D6711
		[Parameter(ParameterSetName = "FilePathContainerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathRunspace")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		public string JobName
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
					this.asjob = true;
				}
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000D8529 File Offset: 0x000D6729
		// (set) Token: 0x060026C3 RID: 9923 RVA: 0x000D8531 File Offset: 0x000D6731
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "Session")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "VMName")]
		[Alias(new string[]
		{
			"Command"
		})]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "Uri")]
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "InProcess")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "VMId")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "ComputerName")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "ContainerId")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "ContainerName")]
		[ValidateNotNull]
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

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x000D853A File Offset: 0x000D673A
		// (set) Token: 0x060026C5 RID: 9925 RVA: 0x000D8542 File Offset: 0x000D6742
		[Parameter(ParameterSetName = "InProcess")]
		public SwitchParameter NoNewScope { get; set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x000D854B File Offset: 0x000D674B
		// (set) Token: 0x060026C7 RID: 9927 RVA: 0x000D8553 File Offset: 0x000D6753
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathUri")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathComputerName")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathRunspace")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathVMId")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathVMName")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathContainerId")]
		[Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePathContainerName")]
		[ValidateNotNull]
		[Alias(new string[]
		{
			"PSPath"
		})]
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

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x000D855C File Offset: 0x000D675C
		// (set) Token: 0x060026C9 RID: 9929 RVA: 0x000D8564 File Offset: 0x000D6764
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathUri")]
		public override SwitchParameter AllowRedirection
		{
			get
			{
				return base.AllowRedirection;
			}
			set
			{
				base.AllowRedirection = value;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x000D856D File Offset: 0x000D676D
		// (set) Token: 0x060026CB RID: 9931 RVA: 0x000D8575 File Offset: 0x000D6775
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "FilePathUri")]
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

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000D857E File Offset: 0x000D677E
		// (set) Token: 0x060026CD RID: 9933 RVA: 0x000D8586 File Offset: 0x000D6786
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "Uri")]
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

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x000D858F File Offset: 0x000D678F
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x000D8597 File Offset: 0x000D6797
		[Parameter(ParameterSetName = "FilePathComputerName")]
		[Parameter(ParameterSetName = "FilePathUri")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Uri")]
		public override SwitchParameter EnableNetworkAccess
		{
			get
			{
				return base.EnableNetworkAccess;
			}
			set
			{
				base.EnableNetworkAccess = value;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000D85A0 File Offset: 0x000D67A0
		// (set) Token: 0x060026D1 RID: 9937 RVA: 0x000D85A8 File Offset: 0x000D67A8
		[Parameter(ParameterSetName = "ContainerId")]
		[Parameter(ParameterSetName = "ContainerName")]
		[Parameter(ParameterSetName = "FilePathContainerId")]
		[Parameter(ParameterSetName = "FilePathContainerName")]
		public override SwitchParameter RunAsAdministrator
		{
			get
			{
				return base.RunAsAdministrator;
			}
			set
			{
				base.RunAsAdministrator = value;
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000D85B4 File Offset: 0x000D67B4
		protected override void BeginProcessing()
		{
			if (base.InvokeAndDisconnect && this.asjob)
			{
				throw new InvalidOperationException(RemotingErrorIdStrings.AsJobAndDisconnectedError);
			}
			if (base.InvokeAndDisconnect && (this.ComputerName == null || this.ComputerName.Length == 0) && (this.ConnectionUri == null || this.ConnectionUri.Length == 0))
			{
				throw new InvalidOperationException(RemotingErrorIdStrings.InvokeDisconnectedWithoutComputerName);
			}
			if (base.MyInvocation.BoundParameters.ContainsKey("SessionName") && !base.InvokeAndDisconnect)
			{
				throw new InvalidOperationException(RemotingErrorIdStrings.SessionNameWithoutInvokeDisconnected);
			}
			if (!this.asjob && (base.ParameterSetName.Equals("Session") || base.ParameterSetName.Equals("FilePathRunspace")))
			{
				long localPipelineId = ((LocalRunspace)base.Context.CurrentRunspace).GetCurrentlyRunningPipeline().InstanceId;
				List<PSSession> list = new List<PSSession>();
				foreach (PSSession pssession in this.Session)
				{
					if (pssession.Runspace.RunspaceStateInfo.State != RunspaceState.Opened)
					{
						string message = StringUtil.Format(RemotingErrorIdStrings.ICMInvalidSessionState, new object[]
						{
							pssession.Name,
							pssession.InstanceId,
							pssession.ComputerName,
							pssession.Runspace.RunspaceStateInfo.State
						});
						base.WriteError(new ErrorRecord(new InvalidRunspaceStateException(message), "InvokeCommandCommandInvalidSessionState", ErrorCategory.InvalidOperation, pssession));
					}
					else if (pssession.Runspace.RunspaceAvailability != RunspaceAvailability.Available)
					{
						RemoteRunspace remoteRunspace = pssession.Runspace as RemoteRunspace;
						if (remoteRunspace != null && remoteRunspace.RunspaceAvailability == RunspaceAvailability.Busy && remoteRunspace.IsAnotherInvokeCommandExecuting(this, localPipelineId))
						{
							list.Add(pssession);
						}
						else
						{
							string message2 = StringUtil.Format(RemotingErrorIdStrings.ICMInvalidSessionAvailability, new object[]
							{
								pssession.Name,
								pssession.InstanceId,
								pssession.ComputerName,
								pssession.Runspace.RunspaceAvailability
							});
							base.WriteError(new ErrorRecord(new InvalidRunspaceStateException(message2), "InvokeCommandCommandInvalidSessionAvailability", ErrorCategory.InvalidOperation, pssession));
						}
					}
					else
					{
						list.Add(pssession);
					}
				}
				if (list.Count == 0)
				{
					throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ICMNoValidRunspaces, new object[0]));
				}
				if (list.Count < this.Session.Length)
				{
					this.Session = list.ToArray();
				}
			}
			if (base.ParameterSetName.Equals("InProcess"))
			{
				if (this.FilePath != null)
				{
					this.ScriptBlock = base.GetScriptBlockFromFile(this.FilePath, false);
				}
				if (base.MyInvocation.ExpectingInput && !this.ScriptBlock.IsUsingDollarInput())
				{
					try
					{
						this.steppablePipeline = this.ScriptBlock.GetSteppablePipeline(CommandOrigin.Internal, this.ArgumentList);
						this.steppablePipeline.Begin(this);
					}
					catch (InvalidOperationException)
					{
					}
				}
				return;
			}
			base.BeginProcessing();
			foreach (IThrottleOperation throttleOperation in base.Operations)
			{
				this.inputWriters.Add(((ExecutionCmdletHelper)throttleOperation).Pipeline.Input);
			}
			if (base.ParameterSetName.Equals("Session"))
			{
				long localPipelineId2 = ((LocalRunspace)base.Context.CurrentRunspace).GetCurrentlyRunningPipeline().InstanceId;
				foreach (PSSession pssession2 in this.Session)
				{
					RemoteRunspace remoteRunspace2 = (RemoteRunspace)pssession2.Runspace;
					if (remoteRunspace2.IsAnotherInvokeCommandExecuting(this, localPipelineId2))
					{
						if (base.MyInvocation != null && base.MyInvocation.PipelinePosition == 1 && !base.MyInvocation.ExpectingInput)
						{
							PSPrimitiveDictionary psprimitiveDictionary = pssession2.ApplicationPrivateData["PSVersionTable"] as PSPrimitiveDictionary;
							if (psprimitiveDictionary != null)
							{
								Version v = psprimitiveDictionary["PSRemotingProtocolVersion"] as Version;
								if (v != null && v >= RemotingConstants.ProtocolVersionWin8RTM)
								{
									this.needToCollect = false;
									this.needToStartSteppablePipelineOnServer = true;
									break;
								}
							}
						}
						this.needToCollect = true;
						this.needToStartSteppablePipelineOnServer = false;
						break;
					}
				}
			}
			if (this.needToStartSteppablePipelineOnServer)
			{
				using (List<IThrottleOperation>.Enumerator enumerator2 = base.Operations.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IThrottleOperation throttleOperation2 = enumerator2.Current;
						ExecutionCmdletHelperRunspace executionCmdletHelperRunspace = throttleOperation2 as ExecutionCmdletHelperRunspace;
						if (executionCmdletHelperRunspace == null)
						{
							break;
						}
						executionCmdletHelperRunspace.ShouldUseSteppablePipelineOnServer = true;
					}
					goto IL_479;
				}
			}
			this.clearInvokeCommandOnRunspace = true;
			IL_479:
			this.DetermineThrowStatementBehavior();
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000D8A68 File Offset: 0x000D6C68
		protected override void ProcessRecord()
		{
			if (!this.pipelineinvoked && !this.needToCollect)
			{
				this.pipelineinvoked = true;
				if (this.InputObject == AutomationNull.Value)
				{
					base.CloseAllInputStreams();
					this.inputStreamClosed = true;
				}
				if (!base.ParameterSetName.Equals("InProcess"))
				{
					string parameterSetName;
					PSRemotingJob psremotingJob;
					PSRemotingJob psremotingJob2;
					string[] array;
					int i;
					PSRemotingJob psremotingJob3;
					if (!this.asjob)
					{
						this.CreateAndRunSyncJob();
					}
					else
						switch (parameterSetName = base.ParameterSetName)
						{
						case "ComputerName":
						case "FilePathComputerName":
						case "VMId":
						case "VMName":
						case "ContainerId":
						case "ContainerName":
						case "FilePathVMId":
						case "FilePathVMName":
						case "FilePathContainerId":
						case "FilePathContainerName":
							if (base.ResolvedComputerNames.Length != 0 && base.Operations.Count > 0)
							{
								psremotingJob = new PSRemotingJob(base.ResolvedComputerNames, base.Operations, this.ScriptBlock.ToString(), this.ThrottleLimit, this.name);
								psremotingJob.PSJobTypeName = InvokeCommandCommand.RemoteJobType;
								psremotingJob.HideComputerName = this.hideComputerName;
								base.JobRepository.Add(psremotingJob);
								base.WriteObject(psremotingJob);
							}
							break;
						case "Session":
						case "FilePathRunspace":
							psremotingJob2 = new PSRemotingJob(this.Session, base.Operations, this.ScriptBlock.ToString(), this.ThrottleLimit, this.name);
							psremotingJob2.PSJobTypeName = InvokeCommandCommand.RemoteJobType;
							psremotingJob2.HideComputerName = this.hideComputerName;
							base.JobRepository.Add(psremotingJob2);
							base.WriteObject(psremotingJob2);
							break;
						case "Uri":
						case "FilePathUri":
							if (base.Operations.Count > 0)
							{
								array = new string[this.ConnectionUri.Length];
								for (i = 0; i < array.Length; i++)
								{
									array[i] = this.ConnectionUri[i].ToString();
								}
								psremotingJob3 = new PSRemotingJob(array, base.Operations, this.ScriptBlock.ToString(), this.ThrottleLimit, this.name);
								psremotingJob3.PSJobTypeName = InvokeCommandCommand.RemoteJobType;
								psremotingJob3.HideComputerName = this.hideComputerName;
								base.JobRepository.Add(psremotingJob3);
								base.WriteObject(psremotingJob3);
							}
							break;
						}
				}
			}
			if (this.InputObject != AutomationNull.Value && !this.inputStreamClosed)
			{
				if ((base.ParameterSetName.Equals("InProcess") && this.steppablePipeline == null) || this.needToCollect)
				{
					this.input.Add(this.InputObject);
					return;
				}
				if (base.ParameterSetName.Equals("InProcess") && this.steppablePipeline != null)
				{
					this.steppablePipeline.Process(this.InputObject);
					return;
				}
				this.WriteInput(this.InputObject);
				if (!this.asjob)
				{
					this.WriteJobResults(true);
				}
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x000D8DF4 File Offset: 0x000D6FF4
		protected override void EndProcessing()
		{
			if (!this.needToCollect)
			{
				base.CloseAllInputStreams();
			}
			if (!this.asjob)
			{
				if (base.ParameterSetName.Equals("InProcess"))
				{
					if (this.steppablePipeline != null)
					{
						this.steppablePipeline.End();
						return;
					}
					this.ScriptBlock.InvokeUsingCmdlet(this, !this.NoNewScope, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, this.input, AutomationNull.Value, this.ArgumentList);
					return;
				}
				else if (this.job != null)
				{
					if (base.InvokeAndDisconnect)
					{
						this.WaitForDisconnectAndDisposeJob();
						return;
					}
					this.WriteJobResults(false);
					if (!this.asjob)
					{
						this.job.Dispose();
						return;
					}
				}
				else if (this.needToCollect && base.ParameterSetName.Equals("Session"))
				{
					this.CreateAndRunSyncJob();
					foreach (object inputValue in this.input)
					{
						this.WriteInput(inputValue);
					}
					base.CloseAllInputStreams();
					if (base.InvokeAndDisconnect)
					{
						this.WaitForDisconnectAndDisposeJob();
						return;
					}
					this.WriteJobResults(false);
					if (!this.asjob)
					{
						this.job.Dispose();
					}
				}
			}
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x000D8F38 File Offset: 0x000D7138
		protected override void StopProcessing()
		{
			if (!base.ParameterSetName.Equals("InProcess") && !this.asjob)
			{
				bool flag = false;
				lock (this.jobSyncObject)
				{
					if (this.job != null)
					{
						flag = true;
					}
					else
					{
						this.nojob = true;
					}
				}
				if (flag)
				{
					this.job.StopJob();
				}
				this.needToCollect = false;
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000D8FB8 File Offset: 0x000D71B8
		private void HandleThrottleComplete(object sender, EventArgs eventArgs)
		{
			this.operationsComplete.Set();
			this.throttleManager.ThrottleComplete -= this.HandleThrottleComplete;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000D8FE0 File Offset: 0x000D71E0
		private void ClearInvokeCommandOnRunspaces()
		{
			if (base.ParameterSetName.Equals("Session"))
			{
				foreach (PSSession pssession in this.Session)
				{
					RemoteRunspace remoteRunspace = (RemoteRunspace)pssession.Runspace;
					remoteRunspace.ClearInvokeCommand();
				}
			}
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000D902C File Offset: 0x000D722C
		private void CreateAndRunSyncJob()
		{
			lock (this.jobSyncObject)
			{
				if (!this.nojob)
				{
					this.throttleManager.ThrottleLimit = this.ThrottleLimit;
					this.throttleManager.ThrottleComplete += this.HandleThrottleComplete;
					this.operationsComplete.Reset();
					this.disconnectComplete = new ManualResetEvent(false);
					this.job = new PSInvokeExpressionSyncJob(base.Operations, this.throttleManager);
					this.job.HideComputerName = this.hideComputerName;
					this.job.StateChanged += this.HandleJobStateChanged;
					this.AddConnectionRetryHandler(this.job);
					this.job.StartOperations(base.Operations);
				}
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x000D9110 File Offset: 0x000D7310
		private void HandleJobStateChanged(object sender, JobStateEventArgs e)
		{
			JobState state = e.JobStateInfo.State;
			if (state == JobState.Disconnected || state == JobState.Completed || state == JobState.Stopped || state == JobState.Failed)
			{
				this.job.StateChanged -= this.HandleJobStateChanged;
				this.RemoveConnectionRetryHandler(sender as PSInvokeExpressionSyncJob);
				lock (this.jobSyncObject)
				{
					if (this.disconnectComplete != null)
					{
						this.disconnectComplete.Set();
					}
				}
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000D91A0 File Offset: 0x000D73A0
		private void AddConnectionRetryHandler(PSInvokeExpressionSyncJob job)
		{
			if (job == null)
			{
				return;
			}
			Collection<PowerShell> powerShells = job.GetPowerShells();
			foreach (PowerShell powerShell in powerShells)
			{
				if (powerShell.RemotePowerShell != null)
				{
					powerShell.RemotePowerShell.RCConnectionNotification += this.RCConnectionNotificationHandler;
				}
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000D920C File Offset: 0x000D740C
		private void RemoveConnectionRetryHandler(PSInvokeExpressionSyncJob job)
		{
			this.StopProgressBar(0L);
			if (job == null)
			{
				return;
			}
			Collection<PowerShell> powerShells = job.GetPowerShells();
			foreach (PowerShell powerShell in powerShells)
			{
				if (powerShell.RemotePowerShell != null)
				{
					powerShell.RemotePowerShell.RCConnectionNotification -= this.RCConnectionNotificationHandler;
				}
			}
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000D9280 File Offset: 0x000D7480
		private void RCConnectionNotificationHandler(object sender, PSConnectionRetryStatusEventArgs e)
		{
			switch (e.Notification)
			{
			case PSConnectionRetryStatus.NetworkFailureDetected:
				this.StartProgressBar((long)sender.GetHashCode(), e.ComputerName, e.MaxRetryConnectionTime / 1000);
				return;
			case PSConnectionRetryStatus.ConnectionRetryAttempt:
			case PSConnectionRetryStatus.AutoDisconnectSucceeded:
				break;
			case PSConnectionRetryStatus.ConnectionRetrySucceeded:
			case PSConnectionRetryStatus.AutoDisconnectStarting:
			case PSConnectionRetryStatus.InternalErrorAbort:
				this.StopProgressBar((long)sender.GetHashCode());
				break;
			default:
				return;
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000D92E4 File Offset: 0x000D74E4
		private void WaitForDisconnectAndDisposeJob()
		{
			if (this.disconnectComplete != null)
			{
				this.disconnectComplete.WaitOne();
				List<PSSession> disconnectedSessions = this.GetDisconnectedSessions(this.job);
				foreach (PSSession pssession in disconnectedSessions)
				{
					base.RunspaceRepository.AddOrReplace(pssession);
					base.WriteObject(pssession);
				}
				if (this.job.Error.Count > 0)
				{
					this.WriteStreamObjectsFromCollection(this.job.ReadAll());
				}
				this.job.Dispose();
			}
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000D9394 File Offset: 0x000D7594
		private List<PSSession> GetDisconnectedSessions(PSInvokeExpressionSyncJob job)
		{
			List<PSSession> list = new List<PSSession>();
			Collection<PowerShell> powerShells = job.GetPowerShells();
			foreach (PowerShell powerShell in powerShells)
			{
				string cmdStr = (powerShell.Commands != null && powerShell.Commands.Commands.Count > 0) ? powerShell.Commands.Commands[0].CommandText : string.Empty;
				ConnectCommandInfo connectCommandInfo = new ConnectCommandInfo(powerShell.InstanceId, cmdStr);
				RunspacePool runspacePool = null;
				if (powerShell.RunspacePool != null)
				{
					runspacePool = powerShell.RunspacePool;
				}
				else
				{
					object runspaceConnection = powerShell.GetRunspaceConnection();
					RunspacePool runspacePool2 = runspaceConnection as RunspacePool;
					if (runspacePool2 != null)
					{
						runspacePool = runspacePool2;
					}
					else
					{
						RemoteRunspace remoteRunspace = runspaceConnection as RemoteRunspace;
						if (remoteRunspace != null)
						{
							runspacePool = remoteRunspace.RunspacePool;
						}
					}
				}
				if (runspacePool != null)
				{
					if (runspacePool.RunspacePoolStateInfo.State != RunspacePoolState.Disconnected)
					{
						if (!base.InvokeAndDisconnect || runspacePool.RunspacePoolStateInfo.State != RunspacePoolState.Opened)
						{
							continue;
						}
						runspacePool.Disconnect();
					}
					string value = runspacePool.RemoteRunspacePoolInternal.Name;
					if (string.IsNullOrEmpty(value))
					{
						int num;
						value = PSSession.GenerateRunspaceName(out num);
					}
					RemoteRunspace remoteRunspace2 = new RemoteRunspace(new RunspacePool(true, runspacePool.RemoteRunspacePoolInternal.InstanceId, value, new ConnectCommandInfo[]
					{
						connectCommandInfo
					}, runspacePool.RemoteRunspacePoolInternal.ConnectionInfo, base.Host, base.Context.TypeTable)
					{
						RemoteRunspacePoolInternal = 
						{
							IsRemoteDebugStop = runspacePool.RemoteRunspacePoolInternal.IsRemoteDebugStop
						}
					});
					list.Add(new PSSession(remoteRunspace2));
				}
			}
			return list;
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000D9554 File Offset: 0x000D7754
		private void WriteInput(object inputValue)
		{
			if (this.inputWriters.Count == 0)
			{
				if (!this.asjob)
				{
					this.WriteJobResults(false);
				}
				this.EndProcessing();
				throw new StopUpstreamCommandsException(this);
			}
			List<PipelineWriter> list = new List<PipelineWriter>();
			foreach (PipelineWriter pipelineWriter in this.inputWriters)
			{
				try
				{
					pipelineWriter.Write(inputValue);
				}
				catch (PipelineClosedException)
				{
					list.Add(pipelineWriter);
				}
			}
			foreach (PipelineWriter item in list)
			{
				this.inputWriters.Remove(item);
			}
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000D9634 File Offset: 0x000D7834
		private void WriteJobResults(bool nonblocking)
		{
			if (this.job != null)
			{
				PipelineStoppedException ex = null;
				this.job.PropagateThrows = this.propagateErrors;
				do
				{
					if (!nonblocking)
					{
						if (this.disconnectComplete != null)
						{
							WaitHandle.WaitAny(new WaitHandle[]
							{
								this.disconnectComplete,
								this.job.Results.WaitHandle
							});
						}
						else
						{
							this.job.Results.WaitHandle.WaitOne();
						}
					}
					try
					{
						this.WriteStreamObjectsFromCollection(this.job.ReadAll());
					}
					catch (PipelineStoppedException ex2)
					{
						ex = ex2;
					}
					if (nonblocking)
					{
						break;
					}
				}
				while (!this.job.IsTerminalState());
				try
				{
					this.WriteStreamObjectsFromCollection(this.job.ReadAll());
				}
				catch (PipelineStoppedException ex3)
				{
					ex = ex3;
				}
				if (ex != null)
				{
					this.HandlePipelinesStopped();
					throw ex;
				}
				if (this.job.JobStateInfo.State == JobState.Disconnected)
				{
					if (base.ParameterSetName == "Session" || base.ParameterSetName == "FilePathRunspace")
					{
						PSRemotingJob psremotingJob = this.job.CreateDisconnectedRemotingJob();
						if (psremotingJob != null)
						{
							psremotingJob.PSJobTypeName = InvokeCommandCommand.RemoteJobType;
							this.asjob = true;
							List<Job> list = new List<Job>();
							foreach (Job job in psremotingJob.ChildJobs)
							{
								PSRemotingChildJob psremotingChildJob = job as PSRemotingChildJob;
								if (psremotingChildJob != null)
								{
									PSSession pssession = this.GetPSSession(psremotingChildJob.Runspace.InstanceId);
									if (pssession != null)
									{
										RemoteDebugger remoteDebugger = pssession.Runspace.Debugger as RemoteDebugger;
										if (remoteDebugger != null && remoteDebugger.IsRemoteDebug)
										{
											psremotingChildJob.RemoveJobAggregation();
											list.Add(psremotingChildJob);
											base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectDebug, new object[]
											{
												pssession.Name,
												pssession.InstanceId,
												pssession.ComputerName
											}));
										}
										else
										{
											this.WriteNetworkFailedError(pssession);
											base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectSession, new object[]
											{
												pssession.Name,
												pssession.InstanceId,
												pssession.ComputerName
											}));
										}
									}
								}
							}
							foreach (Job item in list)
							{
								psremotingJob.ChildJobs.Remove(item);
							}
							if (psremotingJob.ChildJobs.Count > 0)
							{
								base.JobRepository.Add(psremotingJob);
								base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectedJob, psremotingJob.Name));
							}
						}
					}
					else if (base.ParameterSetName == "ComputerName" || base.ParameterSetName == "FilePathComputerName")
					{
						List<PSSession> disconnectedSessions = this.GetDisconnectedSessions(this.job);
						foreach (PSSession pssession2 in disconnectedSessions)
						{
							base.RunspaceRepository.AddOrReplace(pssession2);
							RemoteRunspace remoteRunspace = pssession2.Runspace as RemoteRunspace;
							if (remoteRunspace != null && remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.IsRemoteDebugStop)
							{
								base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectDebug, new object[]
								{
									pssession2.Name,
									pssession2.InstanceId,
									pssession2.ComputerName
								}));
							}
							else
							{
								this.WriteNetworkFailedError(pssession2);
								base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectSession, new object[]
								{
									pssession2.Name,
									pssession2.InstanceId,
									pssession2.ComputerName
								}));
							}
							base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.RCDisconnectSessionCreated, pssession2.Name, pssession2.InstanceId));
						}
					}
					this.HandleThrottleComplete(null, null);
				}
			}
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x000D9ABC File Offset: 0x000D7CBC
		private void WriteNetworkFailedError(PSSession session)
		{
			RuntimeException exception = new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.RCAutoDisconnectingError, session.ComputerName));
			base.WriteError(new ErrorRecord(exception, "PowerShellNetworkFailedStartDisconnect", ErrorCategory.OperationTimeout, session));
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000D9AF4 File Offset: 0x000D7CF4
		private PSSession GetPSSession(Guid runspaceId)
		{
			foreach (PSSession pssession in this.Session)
			{
				if (pssession.Runspace.InstanceId == runspaceId)
				{
					return pssession;
				}
			}
			return null;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000D9B34 File Offset: 0x000D7D34
		private void HandlePipelinesStopped()
		{
			bool flag = false;
			Collection<PowerShell> powerShells = this.job.GetPowerShells();
			foreach (PowerShell powerShell in powerShells)
			{
				if (powerShell.RemotePowerShell != null && powerShell.RemotePowerShell.ConnectionRetryStatus != PSConnectionRetryStatus.None && powerShell.RemotePowerShell.ConnectionRetryStatus != PSConnectionRetryStatus.ConnectionRetrySucceeded && powerShell.RemotePowerShell.ConnectionRetryStatus != PSConnectionRetryStatus.AutoDisconnectSucceeded)
				{
					flag = true;
					break;
				}
			}
			if (flag && base.Host != null)
			{
				base.Host.UI.WriteWarningLine(RemotingErrorIdStrings.StopCommandOnRetry);
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000D9BD8 File Offset: 0x000D7DD8
		private void StartProgressBar(long sourceId, string computerName, int totalSeconds)
		{
			InvokeCommandCommand.RCProgress.StartProgress(sourceId, computerName, totalSeconds, base.Host);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000D9BED File Offset: 0x000D7DED
		private void StopProgressBar(long sourceId)
		{
			InvokeCommandCommand.RCProgress.StopProgress(sourceId);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000D9BFC File Offset: 0x000D7DFC
		private void WriteStreamObjectsFromCollection(IEnumerable<PSStreamObject> results)
		{
			foreach (PSStreamObject psstreamObject in results)
			{
				if (psstreamObject != null)
				{
					psstreamObject.WriteStreamObject(this, false);
				}
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000D9C48 File Offset: 0x000D7E48
		private void DetermineThrowStatementBehavior()
		{
			if (base.ParameterSetName.Equals("InProcess"))
			{
				return;
			}
			if (!this.asjob)
			{
				if (base.ParameterSetName.Equals("ComputerName") || base.ParameterSetName.Equals("FilePathComputerName"))
				{
					if (this.ComputerName.Length == 1)
					{
						this.propagateErrors = true;
						return;
					}
				}
				else if (base.ParameterSetName.Equals("Session") || base.ParameterSetName.Equals("FilePathRunspace"))
				{
					if (this.Session.Length == 1)
					{
						this.propagateErrors = true;
						return;
					}
				}
				else if ((base.ParameterSetName.Equals("Uri") || base.ParameterSetName.Equals("FilePathUri")) && this.ConnectionUri.Length == 1)
				{
					this.propagateErrors = true;
				}
			}
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000D9D17 File Offset: 0x000D7F17
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000D9D28 File Offset: 0x000D7F28
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopProcessing();
				this.operationsComplete.WaitOne();
				this.operationsComplete.Dispose();
				if (!this.asjob)
				{
					if (this.job != null)
					{
						this.job.Dispose();
					}
					this.throttleManager.ThrottleComplete -= this.HandleThrottleComplete;
					this.throttleManager.Dispose();
					this.throttleManager = null;
				}
				if (this.clearInvokeCommandOnRunspace)
				{
					this.ClearInvokeCommandOnRunspaces();
				}
				this.input.Dispose();
				lock (this.jobSyncObject)
				{
					if (this.disconnectComplete != null)
					{
						this.disconnectComplete.Dispose();
						this.disconnectComplete = null;
					}
				}
			}
		}

		// Token: 0x04001320 RID: 4896
		private const string InProcParameterSet = "InProcess";

		// Token: 0x04001321 RID: 4897
		private bool asjob;

		// Token: 0x04001322 RID: 4898
		private bool hideComputerName;

		// Token: 0x04001323 RID: 4899
		private string name = string.Empty;

		// Token: 0x04001324 RID: 4900
		private ThrottleManager throttleManager = new ThrottleManager();

		// Token: 0x04001325 RID: 4901
		private ManualResetEvent operationsComplete = new ManualResetEvent(true);

		// Token: 0x04001326 RID: 4902
		private ManualResetEvent disconnectComplete;

		// Token: 0x04001327 RID: 4903
		private PSInvokeExpressionSyncJob job;

		// Token: 0x04001328 RID: 4904
		private SteppablePipeline steppablePipeline;

		// Token: 0x04001329 RID: 4905
		private bool pipelineinvoked;

		// Token: 0x0400132A RID: 4906
		private bool inputStreamClosed;

		// Token: 0x0400132B RID: 4907
		private PSDataCollection<object> input = new PSDataCollection<object>();

		// Token: 0x0400132C RID: 4908
		private bool needToCollect;

		// Token: 0x0400132D RID: 4909
		private bool needToStartSteppablePipelineOnServer;

		// Token: 0x0400132E RID: 4910
		private bool clearInvokeCommandOnRunspace;

		// Token: 0x0400132F RID: 4911
		private List<PipelineWriter> inputWriters = new List<PipelineWriter>();

		// Token: 0x04001330 RID: 4912
		private object jobSyncObject = new object();

		// Token: 0x04001331 RID: 4913
		private bool nojob;

		// Token: 0x04001332 RID: 4914
		private Guid instanceId = Guid.NewGuid();

		// Token: 0x04001333 RID: 4915
		private bool propagateErrors;

		// Token: 0x04001334 RID: 4916
		private static RobustConnectionProgress RCProgress = new RobustConnectionProgress();

		// Token: 0x04001335 RID: 4917
		internal static readonly string RemoteJobType = "RemoteJob";
	}
}
