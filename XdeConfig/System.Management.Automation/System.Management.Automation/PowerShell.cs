using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Security.Principal;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000238 RID: 568
	public sealed class PowerShell : IDisposable
	{
		// Token: 0x06001A43 RID: 6723 RVA: 0x0009B8C0 File Offset: 0x00099AC0
		private PowerShell(PSCommand command, Collection<PSCommand> extraCommands, object rsConnection)
		{
			this.extraCommands = ((extraCommands == null) ? new Collection<PSCommand>() : extraCommands);
			this.runningExtraCommands = false;
			this.psCommand = command;
			this.psCommand.Owner = this;
			RemoteRunspace remoteRunspace = rsConnection as RemoteRunspace;
			this.rsConnection = ((remoteRunspace != null) ? remoteRunspace.RunspacePool : rsConnection);
			this.instanceId = Guid.NewGuid();
			this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.NotStarted, null);
			this.outputBuffer = null;
			this.outputBufferOwner = true;
			this.errorBuffer = new PSDataCollection<ErrorRecord>();
			this.errorBufferOwner = true;
			this.informationalBuffers = new PSInformationalBuffers(this.instanceId);
			this.dataStreams = new PSDataStreams(this);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0009B98C File Offset: 0x00099B8C
		internal PowerShell(ConnectCommandInfo connectCmdInfo, object rsConnection) : this(new PSCommand(), null, rsConnection)
		{
			this.extraCommands = new Collection<PSCommand>();
			this.runningExtraCommands = false;
			this.AddCommand(connectCmdInfo.Command);
			this.connectCmdInfo = connectCmdInfo;
			this.instanceId = this.connectCmdInfo.CommandId;
			this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Disconnected, null);
			if (rsConnection is RemoteRunspace)
			{
				this.runspace = (rsConnection as Runspace);
				this.runspacePool = ((RemoteRunspace)rsConnection).RunspacePool;
			}
			else if (rsConnection is RunspacePool)
			{
				this.runspacePool = (RunspacePool)rsConnection;
			}
			this.remotePowerShell = new ClientRemotePowerShell(this, this.runspacePool.RemoteRunspacePoolInternal);
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0009BA3C File Offset: 0x00099C3C
		internal PowerShell(ObjectStreamBase inputstream, ObjectStreamBase outputstream, ObjectStreamBase errorstream, RunspacePool runspacePool)
		{
			this.extraCommands = new Collection<PSCommand>();
			this.runningExtraCommands = false;
			this.rsConnection = runspacePool;
			this.instanceId = Guid.NewGuid();
			this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.NotStarted, null);
			this.informationalBuffers = new PSInformationalBuffers(this.instanceId);
			this.dataStreams = new PSDataStreams(this);
			PSDataCollectionStream<PSObject> psdataCollectionStream = (PSDataCollectionStream<PSObject>)outputstream;
			this.outputBuffer = psdataCollectionStream.ObjectStore;
			PSDataCollectionStream<ErrorRecord> psdataCollectionStream2 = (PSDataCollectionStream<ErrorRecord>)errorstream;
			this.errorBuffer = psdataCollectionStream2.ObjectStore;
			if (runspacePool != null && runspacePool.RemoteRunspacePoolInternal != null)
			{
				this.remotePowerShell = new ClientRemotePowerShell(this, runspacePool.RemoteRunspacePoolInternal);
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0009BB04 File Offset: 0x00099D04
		internal PowerShell(ConnectCommandInfo connectCmdInfo, ObjectStreamBase inputstream, ObjectStreamBase outputstream, ObjectStreamBase errorstream, RunspacePool runspacePool) : this(inputstream, outputstream, errorstream, runspacePool)
		{
			this.extraCommands = new Collection<PSCommand>();
			this.runningExtraCommands = false;
			this.psCommand = new PSCommand();
			this.psCommand.Owner = this;
			this.runspacePool = runspacePool;
			this.AddCommand(connectCmdInfo.Command);
			this.connectCmdInfo = connectCmdInfo;
			this.instanceId = this.connectCmdInfo.CommandId;
			this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Disconnected, null);
			this.remotePowerShell = new ClientRemotePowerShell(this, runspacePool.RemoteRunspacePoolInternal);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0009BB94 File Offset: 0x00099D94
		internal void InitForRemotePipeline(CommandCollection command, ObjectStreamBase inputstream, ObjectStreamBase outputstream, ObjectStreamBase errorstream, PSInvocationSettings settings, bool redirectShellErrorOutputPipe)
		{
			this.psCommand = new PSCommand(command[0]);
			this.psCommand.Owner = this;
			for (int i = 1; i < command.Count; i++)
			{
				this.AddCommand(command[i]);
			}
			this.redirectShellErrorOutputPipe = redirectShellErrorOutputPipe;
			if (this.remotePowerShell == null)
			{
				this.remotePowerShell = new ClientRemotePowerShell(this, ((RunspacePool)this.rsConnection).RemoteRunspacePoolInternal);
			}
			this.DetermineIsBatching();
			if (this.isBatching)
			{
				this.SetupAsyncBatchExecution();
			}
			this.remotePowerShell.Initialize(inputstream, outputstream, errorstream, this.informationalBuffers, settings);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0009BC38 File Offset: 0x00099E38
		internal void InitForRemotePipelineConnect(ObjectStreamBase inputstream, ObjectStreamBase outputstream, ObjectStreamBase errorstream, PSInvocationSettings settings, bool redirectShellErrorOutputPipe)
		{
			this.CheckRunspacePoolAndConnect();
			if (this.invocationStateInfo.State != PSInvocationState.Disconnected)
			{
				throw new InvalidPowerShellStateException(this.invocationStateInfo.State);
			}
			this.redirectShellErrorOutputPipe = redirectShellErrorOutputPipe;
			if (this.remotePowerShell == null)
			{
				this.remotePowerShell = new ClientRemotePowerShell(this, ((RunspacePool)this.rsConnection).RemoteRunspacePoolInternal);
			}
			if (!this.remotePowerShell.Initialized)
			{
				this.remotePowerShell.Initialize(inputstream, outputstream, errorstream, this.informationalBuffers, settings);
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0009BCB9 File Offset: 0x00099EB9
		public static PowerShell Create()
		{
			return new PowerShell(new PSCommand(), null, null);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0009BCC8 File Offset: 0x00099EC8
		public static PowerShell Create(RunspaceMode runspace)
		{
			PowerShell powerShell = null;
			switch (runspace)
			{
			case RunspaceMode.CurrentRunspace:
				if (Runspace.DefaultRunspace == null)
				{
					throw new InvalidOperationException(PowerShellStrings.NoDefaultRunspaceForPSCreate);
				}
				powerShell = new PowerShell(new PSCommand(), null, Runspace.DefaultRunspace);
				powerShell.isChild = true;
				powerShell.isNested = true;
				powerShell.IsRunspaceOwner = false;
				powerShell.runspace = Runspace.DefaultRunspace;
				break;
			case RunspaceMode.NewRunspace:
				powerShell = new PowerShell(new PSCommand(), null, null);
				break;
			}
			return powerShell;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0009BD3C File Offset: 0x00099F3C
		public static PowerShell Create(InitialSessionState initialSessionState)
		{
			PowerShell powerShell = PowerShell.Create();
			powerShell.Runspace = RunspaceFactory.CreateRunspace(initialSessionState);
			powerShell.Runspace.Open();
			return powerShell;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0009BD68 File Offset: 0x00099F68
		public PowerShell CreateNestedPowerShell()
		{
			if (this.worker != null && this.worker.CurrentlyRunningPipeline != null)
			{
				return new PowerShell(new PSCommand(), null, this.worker.CurrentlyRunningPipeline.Runspace)
				{
					isNested = true
				};
			}
			throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.InvalidStateCreateNested, new object[0]);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x0009BDC0 File Offset: 0x00099FC0
		private static PowerShell Create(bool isNested, PSCommand psCommand, Collection<PSCommand> extraCommands)
		{
			return new PowerShell(psCommand, extraCommands, null)
			{
				isNested = isNested
			};
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0009BDE0 File Offset: 0x00099FE0
		public PowerShell AddCommand(string cmdlet)
		{
			lock (this.syncObject)
			{
				this.AssertChangesAreAccepted();
				this.psCommand.AddCommand(cmdlet);
			}
			return this;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0009BE30 File Offset: 0x0009A030
		public PowerShell AddCommand(string cmdlet, bool useLocalScope)
		{
			lock (this.syncObject)
			{
				this.AssertChangesAreAccepted();
				this.psCommand.AddCommand(cmdlet, useLocalScope);
			}
			return this;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0009BE84 File Offset: 0x0009A084
		public PowerShell AddScript(string script)
		{
			lock (this.syncObject)
			{
				this.AssertChangesAreAccepted();
				this.psCommand.AddScript(script);
			}
			return this;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0009BED4 File Offset: 0x0009A0D4
		public PowerShell AddScript(string script, bool useLocalScope)
		{
			lock (this.syncObject)
			{
				this.AssertChangesAreAccepted();
				this.psCommand.AddScript(script, useLocalScope);
			}
			return this;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0009BF28 File Offset: 0x0009A128
		internal PowerShell AddCommand(Command command)
		{
			lock (this.syncObject)
			{
				this.AssertChangesAreAccepted();
				this.psCommand.AddCommand(command);
			}
			return this;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0009BF78 File Offset: 0x0009A178
		public PowerShell AddCommand(CommandInfo commandInfo)
		{
			if (commandInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandInfo");
			}
			Command command = new Command(commandInfo);
			this.psCommand.AddCommand(command);
			return this;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0009BFA8 File Offset: 0x0009A1A8
		public PowerShell AddParameter(string parameterName, object value)
		{
			lock (this.syncObject)
			{
				if (this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.ParameterRequiresCommand, new object[0]);
				}
				this.AssertChangesAreAccepted();
				this.psCommand.AddParameter(parameterName, value);
			}
			return this;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0009C01C File Offset: 0x0009A21C
		public PowerShell AddParameter(string parameterName)
		{
			lock (this.syncObject)
			{
				if (this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.ParameterRequiresCommand, new object[0]);
				}
				this.AssertChangesAreAccepted();
				this.psCommand.AddParameter(parameterName);
			}
			return this;
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0009C090 File Offset: 0x0009A290
		public PowerShell AddParameters(IList parameters)
		{
			lock (this.syncObject)
			{
				if (parameters == null)
				{
					throw PSTraceSource.NewArgumentNullException("parameters");
				}
				if (this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.ParameterRequiresCommand, new object[0]);
				}
				this.AssertChangesAreAccepted();
				foreach (object value in parameters)
				{
					this.psCommand.AddParameter(null, value);
				}
			}
			return this;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0009C150 File Offset: 0x0009A350
		public PowerShell AddParameters(IDictionary parameters)
		{
			lock (this.syncObject)
			{
				if (parameters == null)
				{
					throw PSTraceSource.NewArgumentNullException("parameters");
				}
				if (this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.ParameterRequiresCommand, new object[0]);
				}
				this.AssertChangesAreAccepted();
				foreach (object obj2 in parameters)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					string text = dictionaryEntry.Key as string;
					if (text == null)
					{
						throw PSTraceSource.NewArgumentException("parameters", PowerShellStrings.KeyMustBeString, new object[0]);
					}
					this.psCommand.AddParameter(text, dictionaryEntry.Value);
				}
			}
			return this;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0009C244 File Offset: 0x0009A444
		public PowerShell AddArgument(object value)
		{
			lock (this.syncObject)
			{
				if (this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.ParameterRequiresCommand, new object[0]);
				}
				this.AssertChangesAreAccepted();
				this.psCommand.AddArgument(value);
			}
			return this;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0009C2B8 File Offset: 0x0009A4B8
		public PowerShell AddStatement()
		{
			PowerShell result;
			lock (this.syncObject)
			{
				if (this.psCommand.Commands.Count == 0)
				{
					result = this;
				}
				else
				{
					this.AssertChangesAreAccepted();
					this.psCommand.Commands[this.psCommand.Commands.Count - 1].IsEndOfStatement = true;
					result = this;
				}
			}
			return result;
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0009C33C File Offset: 0x0009A53C
		// (set) Token: 0x06001A5B RID: 6747 RVA: 0x0009C344 File Offset: 0x0009A544
		public PSCommand Commands
		{
			get
			{
				return this.psCommand;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Command");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.psCommand = value.Clone();
					this.psCommand.Owner = this;
				}
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0009C3AC File Offset: 0x0009A5AC
		public PSDataStreams Streams
		{
			get
			{
				return this.dataStreams;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0009C3B4 File Offset: 0x0009A5B4
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x0009C3BC File Offset: 0x0009A5BC
		internal PSDataCollection<ErrorRecord> ErrorBuffer
		{
			get
			{
				return this.errorBuffer;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Error");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.errorBuffer = value;
					this.errorBufferOwner = false;
				}
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x0009C418 File Offset: 0x0009A618
		// (set) Token: 0x06001A60 RID: 6752 RVA: 0x0009C428 File Offset: 0x0009A628
		internal PSDataCollection<ProgressRecord> ProgressBuffer
		{
			get
			{
				return this.informationalBuffers.Progress;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Progress");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationalBuffers.Progress = value;
				}
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0009C484 File Offset: 0x0009A684
		// (set) Token: 0x06001A62 RID: 6754 RVA: 0x0009C494 File Offset: 0x0009A694
		internal PSDataCollection<VerboseRecord> VerboseBuffer
		{
			get
			{
				return this.informationalBuffers.Verbose;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Verbose");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationalBuffers.Verbose = value;
				}
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0009C4F0 File Offset: 0x0009A6F0
		// (set) Token: 0x06001A64 RID: 6756 RVA: 0x0009C500 File Offset: 0x0009A700
		internal PSDataCollection<DebugRecord> DebugBuffer
		{
			get
			{
				return this.informationalBuffers.Debug;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Debug");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationalBuffers.Debug = value;
				}
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001A65 RID: 6757 RVA: 0x0009C55C File Offset: 0x0009A75C
		// (set) Token: 0x06001A66 RID: 6758 RVA: 0x0009C56C File Offset: 0x0009A76C
		internal PSDataCollection<WarningRecord> WarningBuffer
		{
			get
			{
				return this.informationalBuffers.Warning;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Warning");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationalBuffers.Warning = value;
				}
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x0009C5C8 File Offset: 0x0009A7C8
		// (set) Token: 0x06001A68 RID: 6760 RVA: 0x0009C5D8 File Offset: 0x0009A7D8
		internal PSDataCollection<InformationRecord> InformationBuffer
		{
			get
			{
				return this.informationalBuffers.Information;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Information");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationalBuffers.Information = value;
				}
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0009C634 File Offset: 0x0009A834
		internal PSInformationalBuffers InformationalBuffers
		{
			get
			{
				return this.informationalBuffers;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0009C63C File Offset: 0x0009A83C
		// (set) Token: 0x06001A6B RID: 6763 RVA: 0x0009C644 File Offset: 0x0009A844
		internal bool RedirectShellErrorOutputPipe
		{
			get
			{
				return this.redirectShellErrorOutputPipe;
			}
			set
			{
				this.redirectShellErrorOutputPipe = value;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x0009C64D File Offset: 0x0009A84D
		public Guid InstanceId
		{
			get
			{
				return this.instanceId;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x0009C655 File Offset: 0x0009A855
		public PSInvocationStateInfo InvocationStateInfo
		{
			get
			{
				return this.invocationStateInfo;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0009C65D File Offset: 0x0009A85D
		public bool IsNested
		{
			get
			{
				return this.isNested;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x0009C665 File Offset: 0x0009A865
		internal bool IsChild
		{
			get
			{
				return this.isChild;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0009C66D File Offset: 0x0009A86D
		public bool HadErrors
		{
			get
			{
				return this._hadErrors;
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0009C675 File Offset: 0x0009A875
		internal void SetHadErrors(bool status)
		{
			this._hadErrors = status;
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0009C67E File Offset: 0x0009A87E
		// (set) Token: 0x06001A73 RID: 6771 RVA: 0x0009C686 File Offset: 0x0009A886
		internal AsyncResult EndInvokeAsyncResult { get; private set; }

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001A74 RID: 6772 RVA: 0x0009C690 File Offset: 0x0009A890
		// (remove) Token: 0x06001A75 RID: 6773 RVA: 0x0009C6C8 File Offset: 0x0009A8C8
		public event EventHandler<PSInvocationStateChangedEventArgs> InvocationStateChanged;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001A76 RID: 6774 RVA: 0x0009C700 File Offset: 0x0009A900
		// (remove) Token: 0x06001A77 RID: 6775 RVA: 0x0009C738 File Offset: 0x0009A938
		internal event EventHandler<PSEventArgs<Runspace>> RunspaceAssigned;

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x0009C770 File Offset: 0x0009A970
		// (set) Token: 0x06001A79 RID: 6777 RVA: 0x0009C7F0 File Offset: 0x0009A9F0
		public Runspace Runspace
		{
			get
			{
				if (this.runspace == null && this.runspacePool == null)
				{
					lock (this.syncObject)
					{
						if (this.runspace == null && this.runspacePool == null)
						{
							this.AssertChangesAreAccepted();
							this.SetRunspace(RunspaceFactory.CreateRunspace(), true);
							this.Runspace.Open();
						}
					}
				}
				return this.runspace;
			}
			set
			{
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					if (this.runspace != null && this.runspaceOwner)
					{
						this.runspace.Dispose();
						this.runspace = null;
						this.runspaceOwner = false;
					}
					this.SetRunspace(value, false);
				}
			}
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0009C864 File Offset: 0x0009AA64
		private void SetRunspace(Runspace runspace, bool owner)
		{
			RemoteRunspace remoteRunspace = runspace as RemoteRunspace;
			if (remoteRunspace == null)
			{
				this.rsConnection = runspace;
			}
			else
			{
				this.rsConnection = remoteRunspace.RunspacePool;
				if (this.remotePowerShell != null)
				{
					this.remotePowerShell.Clear();
					this.remotePowerShell.Dispose();
				}
				this.remotePowerShell = new ClientRemotePowerShell(this, remoteRunspace.RunspacePool.RemoteRunspacePoolInternal);
			}
			this.runspace = runspace;
			this.runspaceOwner = owner;
			this.runspacePool = null;
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0009C8DA File Offset: 0x0009AADA
		// (set) Token: 0x06001A7C RID: 6780 RVA: 0x0009C8E4 File Offset: 0x0009AAE4
		public RunspacePool RunspacePool
		{
			get
			{
				return this.runspacePool;
			}
			set
			{
				if (value != null)
				{
					lock (this.syncObject)
					{
						this.AssertChangesAreAccepted();
						if (this.runspace != null && this.runspaceOwner)
						{
							this.runspace.Dispose();
							this.runspace = null;
							this.runspaceOwner = false;
						}
						this.rsConnection = value;
						this.runspacePool = value;
						if (this.runspacePool.IsRemote)
						{
							if (this.remotePowerShell != null)
							{
								this.remotePowerShell.Clear();
								this.remotePowerShell.Dispose();
							}
							this.remotePowerShell = new ClientRemotePowerShell(this, this.runspacePool.RemoteRunspacePoolInternal);
						}
						this.runspace = null;
					}
				}
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0009C9AC File Offset: 0x0009ABAC
		internal object GetRunspaceConnection()
		{
			return this.rsConnection;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x0009C9B4 File Offset: 0x0009ABB4
		public Collection<PSObject> Connect()
		{
			this.commandInvokedSynchronously = true;
			IAsyncResult asyncResult = this.ConnectAsync();
			PowerShellAsyncResult powerShellAsyncResult = asyncResult as PowerShellAsyncResult;
			this.EndInvokeAsyncResult = powerShellAsyncResult;
			powerShellAsyncResult.EndInvoke();
			this.EndInvokeAsyncResult = null;
			Collection<PSObject> result;
			if (powerShellAsyncResult.Output != null)
			{
				result = powerShellAsyncResult.Output.ReadAll();
			}
			else
			{
				result = new Collection<PSObject>();
			}
			return result;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x0009CA09 File Offset: 0x0009AC09
		public IAsyncResult ConnectAsync()
		{
			return this.ConnectAsync(null, null, null);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0009CA14 File Offset: 0x0009AC14
		public IAsyncResult ConnectAsync(PSDataCollection<PSObject> output, AsyncCallback invocationCallback, object state)
		{
			if (this.invocationStateInfo.State != PSInvocationState.Disconnected)
			{
				throw new InvalidPowerShellStateException(this.invocationStateInfo.State);
			}
			this.CheckRunspacePoolAndConnect();
			if (this.connectCmdInfo != null)
			{
				PSDataCollection<PSObject> psdataCollection = this.outputBuffer;
				if (!this.remotePowerShell.Initialized)
				{
					ObjectStreamBase objectStreamBase = new ObjectStream();
					objectStreamBase.Close();
					if (output != null)
					{
						this.outputBuffer = output;
						this.outputBufferOwner = false;
					}
					else if (this.outputBuffer == null)
					{
						this.outputBuffer = new PSDataCollection<PSObject>();
						this.outputBufferOwner = true;
					}
					psdataCollection = this.outputBuffer;
					ObjectStreamBase outputstream = new PSDataCollectionStream<PSObject>(this.instanceId, psdataCollection);
					this.remotePowerShell.Initialize(objectStreamBase, outputstream, new PSDataCollectionStream<ErrorRecord>(this.instanceId, this.errorBuffer), this.informationalBuffers, null);
				}
				this.invokeAsyncResult = new PowerShellAsyncResult(this.instanceId, invocationCallback, state, psdataCollection, true);
			}
			else if (output != null || invocationCallback != null || this.invokeAsyncResult.IsCompleted)
			{
				PSDataCollection<PSObject> output2;
				if (output != null)
				{
					output2 = output;
					this.outputBuffer = output;
					this.outputBufferOwner = false;
				}
				else if (this.invokeAsyncResult.Output == null || !this.invokeAsyncResult.Output.IsOpen)
				{
					this.outputBuffer = new PSDataCollection<PSObject>();
					this.outputBufferOwner = true;
					output2 = this.outputBuffer;
				}
				else
				{
					output2 = this.invokeAsyncResult.Output;
					this.outputBuffer = output2;
					this.outputBufferOwner = false;
				}
				this.invokeAsyncResult = new PowerShellAsyncResult(this.instanceId, invocationCallback ?? this.invokeAsyncResult.Callback, (invocationCallback != null) ? state : this.invokeAsyncResult.AsyncState, output2, true);
			}
			try
			{
				this.remotePowerShell.ConnectAsync(this.connectCmdInfo);
			}
			catch (Exception ex)
			{
				this.invokeAsyncResult = null;
				this.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Failed, ex));
				InvalidRunspacePoolStateException ex2 = ex as InvalidRunspacePoolStateException;
				if (ex2 != null && this.runspace != null)
				{
					throw ex2.ToInvalidRunspaceStateException();
				}
				throw;
			}
			return this.invokeAsyncResult;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0009CC04 File Offset: 0x0009AE04
		private void CheckRunspacePoolAndConnect()
		{
			RemoteRunspacePoolInternal remoteRunspacePoolInternal = null;
			if (this.rsConnection is RemoteRunspace)
			{
				remoteRunspacePoolInternal = (this.rsConnection as RemoteRunspace).RunspacePool.RemoteRunspacePoolInternal;
			}
			else if (this.rsConnection is RunspacePool)
			{
				remoteRunspacePoolInternal = (this.rsConnection as RunspacePool).RemoteRunspacePoolInternal;
			}
			if (remoteRunspacePoolInternal == null)
			{
				throw new InvalidOperationException(PowerShellStrings.CannotConnect);
			}
			if (remoteRunspacePoolInternal.RunspacePoolStateInfo.State == RunspacePoolState.Disconnected)
			{
				remoteRunspacePoolInternal.Connect();
			}
			if (remoteRunspacePoolInternal.RunspacePoolStateInfo.State != RunspacePoolState.Opened)
			{
				throw new InvalidRunspacePoolStateException(RunspacePoolStrings.InvalidRunspacePoolState, remoteRunspacePoolInternal.RunspacePoolStateInfo.State, RunspacePoolState.Opened);
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0009CCA0 File Offset: 0x0009AEA0
		internal void InvokeWithDebugger(IEnumerable<object> input, IList<PSObject> output, PSInvocationSettings settings, bool invokeMustRun)
		{
			Debugger debugger = this.runspace.Debugger;
			bool flag = true;
			if (debugger != null && this.Commands.Commands.Count > 0)
			{
				Command command = this.Commands.Commands[0];
				DebuggerCommand debuggerCommand = debugger.InternalProcessCommand(command.CommandText, output);
				if (debuggerCommand.ResumeAction != null || debuggerCommand.ExecutedByDebugger)
				{
					output.Add(new PSObject(debuggerCommand));
					this.Commands.Commands.Clear();
					flag = false;
				}
				else if (!debuggerCommand.Command.Equals(command.CommandText, StringComparison.OrdinalIgnoreCase))
				{
					this.Commands.Commands[0] = new Command(debuggerCommand.Command, false, new bool?(true), true);
					DebuggerCommand obj = new DebuggerCommand(debuggerCommand.Command, null, false, true);
					output.Add(new PSObject(obj));
					flag = false;
				}
			}
			if (flag && this.Commands.Commands.Count > 0)
			{
				flag = DebuggerUtils.ShouldAddCommandToHistory(this.Commands.Commands[0].CommandText);
			}
			if (this.Commands.Commands.Count == 0 && invokeMustRun)
			{
				this.Commands.Commands.AddScript("");
			}
			if (this.Commands.Commands.Count > 0)
			{
				if (flag)
				{
					if (settings == null)
					{
						settings = new PSInvocationSettings();
					}
					settings.AddToHistory = true;
				}
				this.Invoke<PSObject>(input, output, settings);
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0009CE1D File Offset: 0x0009B01D
		public Collection<PSObject> Invoke()
		{
			return this.Invoke(null, null);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0009CE27 File Offset: 0x0009B027
		public Collection<PSObject> Invoke(IEnumerable input)
		{
			return this.Invoke(input, null);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0009CE34 File Offset: 0x0009B034
		public Collection<PSObject> Invoke(IEnumerable input, PSInvocationSettings settings)
		{
			Collection<PSObject> collection = new Collection<PSObject>();
			PSDataCollection<PSObject> output = new PSDataCollection<PSObject>(collection);
			this.CoreInvoke<PSObject>(input, output, settings);
			return collection;
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x0009CE58 File Offset: 0x0009B058
		public Collection<T> Invoke<T>()
		{
			Collection<T> collection = new Collection<T>();
			this.Invoke<T>(null, collection, null);
			return collection;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0009CE78 File Offset: 0x0009B078
		public Collection<T> Invoke<T>(IEnumerable input)
		{
			Collection<T> collection = new Collection<T>();
			this.Invoke<T>(input, collection, null);
			return collection;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0009CE98 File Offset: 0x0009B098
		public Collection<T> Invoke<T>(IEnumerable input, PSInvocationSettings settings)
		{
			Collection<T> collection = new Collection<T>();
			this.Invoke<T>(input, collection, settings);
			return collection;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0009CEB5 File Offset: 0x0009B0B5
		public void Invoke<T>(IEnumerable input, IList<T> output)
		{
			this.Invoke<T>(input, output, null);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0009CEC0 File Offset: 0x0009B0C0
		public void Invoke<T>(IEnumerable input, IList<T> output, PSInvocationSettings settings)
		{
			if (output == null)
			{
				throw PSTraceSource.NewArgumentNullException("output");
			}
			PSDataCollection<T> output2 = new PSDataCollection<T>(output);
			this.CoreInvoke<T>(input, output2, settings);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0009CEEB File Offset: 0x0009B0EB
		public void Invoke<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings)
		{
			if (output == null)
			{
				throw PSTraceSource.NewArgumentNullException("output");
			}
			this.CoreInvoke<TInput, TOutput>(input, output, settings);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0009CF04 File Offset: 0x0009B104
		public IAsyncResult BeginInvoke()
		{
			return this.BeginInvoke<object>(null, null, null, null);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0009CF10 File Offset: 0x0009B110
		public IAsyncResult BeginInvoke<T>(PSDataCollection<T> input)
		{
			return this.BeginInvoke<T>(input, null, null, null);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0009CF1C File Offset: 0x0009B11C
		public IAsyncResult BeginInvoke<T>(PSDataCollection<T> input, PSInvocationSettings settings, AsyncCallback callback, object state)
		{
			this.DetermineIsBatching();
			if (this.outputBuffer != null)
			{
				if (this.isBatching || this.extraCommands.Count != 0)
				{
					return this.BeginBatchInvoke<T, PSObject>(input, this.outputBuffer, settings, callback, state);
				}
				return this.CoreInvokeAsync<T, PSObject>(input, this.outputBuffer, settings, callback, state, null);
			}
			else
			{
				this.outputBuffer = new PSDataCollection<PSObject>();
				this.outputBufferOwner = true;
				if (this.isBatching || this.extraCommands.Count != 0)
				{
					return this.BeginBatchInvoke<T, PSObject>(input, this.outputBuffer, settings, callback, state);
				}
				return this.CoreInvokeAsync<T, PSObject>(input, this.outputBuffer, settings, callback, state, this.outputBuffer);
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0009CFC1 File Offset: 0x0009B1C1
		public IAsyncResult BeginInvoke<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output)
		{
			return this.BeginInvoke<TInput, TOutput>(input, output, null, null, null);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0009CFD0 File Offset: 0x0009B1D0
		public IAsyncResult BeginInvoke<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings, AsyncCallback callback, object state)
		{
			if (output == null)
			{
				throw PSTraceSource.NewArgumentNullException("output");
			}
			this.DetermineIsBatching();
			if (this.isBatching || this.extraCommands.Count != 0)
			{
				return this.BeginBatchInvoke<TInput, TOutput>(input, output, settings, callback, state);
			}
			return this.CoreInvokeAsync<TInput, TOutput>(input, output, settings, callback, state, null);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0009D024 File Offset: 0x0009B224
		private IAsyncResult BeginBatchInvoke<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings, AsyncCallback callback, object state)
		{
			PSDataCollection<PSObject> psdataCollection = output as PSDataCollection<PSObject>;
			if (psdataCollection == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			if (this.isBatching)
			{
				this.SetupAsyncBatchExecution();
			}
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			if (runspacePool != null && runspacePool.IsRemote && this.ServerSupportsBatchInvocation())
			{
				try
				{
					return this.CoreInvokeAsync<TInput, TOutput>(input, output, settings, callback, state, psdataCollection);
				}
				finally
				{
					if (this.isBatching)
					{
						this.EndAsyncBatchExecution();
					}
				}
			}
			this.runningExtraCommands = true;
			this.batchInvocationSettings = settings;
			this.batchAsyncResult = new PowerShellAsyncResult(this.instanceId, callback, state, psdataCollection, true);
			this.CoreInvokeAsync<TInput, TOutput>(input, output, settings, new AsyncCallback(this.BatchInvocationCallback), state, psdataCollection);
			return this.batchAsyncResult;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x0009D0E4 File Offset: 0x0009B2E4
		private void BatchInvocationWorkItem(object state)
		{
			BatchInvocationContext batchInvocationContext = state as BatchInvocationContext;
			PSCommand pscommand = this.psCommand;
			try
			{
				this.psCommand = batchInvocationContext.Command;
				if (this.psCommand == this.extraCommands[this.extraCommands.Count - 1])
				{
					this.runningExtraCommands = false;
				}
				try
				{
					IAsyncResult asyncResult = this.CoreInvokeAsync<object, PSObject>(null, batchInvocationContext.Output, this.batchInvocationSettings, null, this.batchAsyncResult.AsyncState, batchInvocationContext.Output);
					this.EndInvoke(asyncResult);
				}
				catch (ActionPreferenceStopException asCompleted)
				{
					this.stopBatchExecution = true;
					this.batchAsyncResult.SetAsCompleted(asCompleted);
					return;
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					this.SetHadErrors(true);
					if (this.batchInvocationSettings != null && this.batchInvocationSettings.ErrorActionPreference == ActionPreference.Stop)
					{
						this.stopBatchExecution = true;
						this.AppendExceptionToErrorStream(ex);
						this.batchAsyncResult.SetAsCompleted(null);
						return;
					}
					if (this.batchInvocationSettings == null)
					{
						switch ((ActionPreference)this.Runspace.SessionStateProxy.GetVariable("ErrorActionPreference"))
						{
						case ActionPreference.SilentlyContinue:
						case ActionPreference.Continue:
							this.AppendExceptionToErrorStream(ex);
							break;
						case ActionPreference.Stop:
							this.batchAsyncResult.SetAsCompleted(ex);
							return;
						}
					}
					else if (this.batchInvocationSettings.ErrorActionPreference != ActionPreference.Ignore)
					{
						this.AppendExceptionToErrorStream(ex);
					}
				}
				if (this.psCommand == this.extraCommands[this.extraCommands.Count - 1])
				{
					this.batchAsyncResult.SetAsCompleted(null);
				}
			}
			finally
			{
				this.psCommand = pscommand;
				batchInvocationContext.Signal();
			}
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x0009D2F0 File Offset: 0x0009B4F0
		private void BatchInvocationCallback(IAsyncResult result)
		{
			PSDataCollection<PSObject> psdataCollection = null;
			try
			{
				psdataCollection = this.EndInvoke(result);
				if (psdataCollection == null)
				{
					psdataCollection = this.batchAsyncResult.Output;
				}
				this.DoRemainingBatchCommands(psdataCollection);
			}
			catch (PipelineStoppedException asCompleted)
			{
				this.batchAsyncResult.SetAsCompleted(asCompleted);
			}
			catch (ActionPreferenceStopException asCompleted2)
			{
				this.batchAsyncResult.SetAsCompleted(asCompleted2);
			}
			catch (Exception ex)
			{
				this.runningExtraCommands = false;
				CommandProcessorBase.CheckForSevereException(ex);
				this.SetHadErrors(true);
				ActionPreference actionPreference;
				if (this.batchInvocationSettings != null)
				{
					actionPreference = ((this.batchInvocationSettings.ErrorActionPreference != null) ? this.batchInvocationSettings.ErrorActionPreference.Value : ActionPreference.Continue);
				}
				else
				{
					actionPreference = ((this.Runspace != null) ? ((ActionPreference)this.Runspace.SessionStateProxy.GetVariable("ErrorActionPreference")) : ActionPreference.Continue);
				}
				switch (actionPreference)
				{
				case ActionPreference.SilentlyContinue:
				case ActionPreference.Continue:
					this.AppendExceptionToErrorStream(ex);
					break;
				case ActionPreference.Stop:
					this.batchAsyncResult.SetAsCompleted(ex);
					return;
				}
				if (psdataCollection == null)
				{
					psdataCollection = this.batchAsyncResult.Output;
				}
				this.DoRemainingBatchCommands(psdataCollection);
			}
			finally
			{
				if (this.isBatching)
				{
					this.EndAsyncBatchExecution();
				}
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0009D480 File Offset: 0x0009B680
		private void DoRemainingBatchCommands(PSDataCollection<PSObject> objs)
		{
			if (this.extraCommands.Count > 1)
			{
				for (int i = 1; i < this.extraCommands.Count; i++)
				{
					if (this.stopBatchExecution)
					{
						return;
					}
					BatchInvocationContext batchInvocationContext = new BatchInvocationContext(this.extraCommands[i], objs);
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.BatchInvocationWorkItem), batchInvocationContext);
					batchInvocationContext.Wait();
				}
			}
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0009D4E8 File Offset: 0x0009B6E8
		private void DetermineIsBatching()
		{
			foreach (Command command in this.psCommand.Commands)
			{
				if (command.IsEndOfStatement)
				{
					this.isBatching = true;
					return;
				}
			}
			this.isBatching = false;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0009D54C File Offset: 0x0009B74C
		private void SetupAsyncBatchExecution()
		{
			this.backupPSCommand = this.psCommand.Clone();
			this.extraCommands.Clear();
			PSCommand pscommand = new PSCommand();
			pscommand.Owner = this;
			foreach (Command command in this.psCommand.Commands)
			{
				if (command.IsEndOfStatement)
				{
					pscommand.Commands.Add(command);
					this.extraCommands.Add(pscommand);
					pscommand = new PSCommand();
					pscommand.Owner = this;
				}
				else
				{
					pscommand.Commands.Add(command);
				}
			}
			if (pscommand.Commands.Count != 0)
			{
				this.extraCommands.Add(pscommand);
			}
			this.psCommand = this.extraCommands[0];
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0009D628 File Offset: 0x0009B828
		private void EndAsyncBatchExecution()
		{
			this.psCommand = this.backupPSCommand;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0009D638 File Offset: 0x0009B838
		private void AppendExceptionToErrorStream(Exception e)
		{
			IContainsErrorRecord containsErrorRecord = e as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				this.Streams.Error.Add(containsErrorRecord.ErrorRecord);
				return;
			}
			this.Streams.Error.Add(new ErrorRecord(e, "InvalidOperation", ErrorCategory.InvalidOperation, null));
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0009D68C File Offset: 0x0009B88C
		public PSDataCollection<PSObject> EndInvoke(IAsyncResult asyncResult)
		{
			PSDataCollection<PSObject> output;
			try
			{
				this.commandInvokedSynchronously = true;
				if (asyncResult == null)
				{
					throw PSTraceSource.NewArgumentNullException("asyncResult");
				}
				PowerShellAsyncResult powerShellAsyncResult = asyncResult as PowerShellAsyncResult;
				if (powerShellAsyncResult == null || powerShellAsyncResult.OwnerId != this.instanceId || !powerShellAsyncResult.IsAssociatedWithAsyncInvoke)
				{
					throw PSTraceSource.NewArgumentException("asyncResult", PowerShellStrings.AsyncResultNotOwned, new object[]
					{
						"IAsyncResult",
						"BeginInvoke"
					});
				}
				this.EndInvokeAsyncResult = powerShellAsyncResult;
				powerShellAsyncResult.EndInvoke();
				this.EndInvokeAsyncResult = null;
				if (this.outputBufferOwner)
				{
					this.outputBufferOwner = false;
					this.outputBuffer = null;
				}
				output = powerShellAsyncResult.Output;
			}
			catch (InvalidRunspacePoolStateException ex)
			{
				this.SetHadErrors(true);
				if (this.runspace != null)
				{
					throw ex.ToInvalidRunspaceStateException();
				}
				throw;
			}
			return output;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0009D758 File Offset: 0x0009B958
		public void Stop()
		{
			try
			{
				IAsyncResult asyncResult = this.CoreStop(true, null, null);
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0009D790 File Offset: 0x0009B990
		public IAsyncResult BeginStop(AsyncCallback callback, object state)
		{
			return this.CoreStop(false, callback, state);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0009D79C File Offset: 0x0009B99C
		public void EndStop(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw PSTraceSource.NewArgumentNullException("asyncResult");
			}
			PowerShellAsyncResult powerShellAsyncResult = asyncResult as PowerShellAsyncResult;
			if (powerShellAsyncResult == null || powerShellAsyncResult.OwnerId != this.instanceId || powerShellAsyncResult.IsAssociatedWithAsyncInvoke)
			{
				throw PSTraceSource.NewArgumentException("asyncResult", PowerShellStrings.AsyncResultNotOwned, new object[]
				{
					"IAsyncResult",
					"BeginStop"
				});
			}
			powerShellAsyncResult.EndInvoke();
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x0009D80C File Offset: 0x0009BA0C
		private void PipelineStateChanged(object source, PipelineStateEventArgs stateEventArgs)
		{
			PSInvocationStateInfo stateChanged = new PSInvocationStateInfo(stateEventArgs.PipelineStateInfo);
			this.SetStateChanged(stateChanged);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x0009D82C File Offset: 0x0009BA2C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001A9F RID: 6815 RVA: 0x0009D83B File Offset: 0x0009BA3B
		// (set) Token: 0x06001AA0 RID: 6816 RVA: 0x0009D843 File Offset: 0x0009BA43
		public bool IsRunspaceOwner
		{
			get
			{
				return this.runspaceOwner;
			}
			internal set
			{
				this.runspaceOwner = value;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x0009D84C File Offset: 0x0009BA4C
		// (set) Token: 0x06001AA2 RID: 6818 RVA: 0x0009D854 File Offset: 0x0009BA54
		internal bool ErrorBufferOwner
		{
			get
			{
				return this.errorBufferOwner;
			}
			set
			{
				this.errorBufferOwner = value;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x0009D85D File Offset: 0x0009BA5D
		// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x0009D865 File Offset: 0x0009BA65
		internal bool OutputBufferOwner
		{
			get
			{
				return this.outputBufferOwner;
			}
			set
			{
				this.outputBufferOwner = value;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x0009D86E File Offset: 0x0009BA6E
		internal PSDataCollection<PSObject> OutputBuffer
		{
			get
			{
				return this.outputBuffer;
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x0009D876 File Offset: 0x0009BA76
		internal void GenerateNewInstanceId()
		{
			this.instanceId = Guid.NewGuid();
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0009D884 File Offset: 0x0009BA84
		internal SteppablePipeline GetSteppablePipeline()
		{
			ExecutionContext contextFromTLS = this.GetContextFromTLS();
			return this.GetSteppablePipeline(contextFromTLS, CommandOrigin.Internal);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0009D8A4 File Offset: 0x0009BAA4
		internal ExecutionContext GetContextFromTLS()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				string text = (this.Commands.Commands.Count > 0) ? this.Commands.Commands[0].CommandText : null;
				PSInvalidOperationException ex;
				if (text != null)
				{
					text = ErrorCategoryInfo.Ellipsize(CultureInfo.CurrentUICulture, text);
					ex = PSTraceSource.NewInvalidOperationException(PowerShellStrings.CommandInvokedFromWrongThreadWithCommand, new object[]
					{
						text
					});
				}
				else
				{
					ex = PSTraceSource.NewInvalidOperationException(PowerShellStrings.CommandInvokedFromWrongThreadWithoutCommand, new object[0]);
				}
				ex.SetErrorId("CommandInvokedFromWrongThread");
				throw ex;
			}
			return executionContextFromTLS;
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x0009D930 File Offset: 0x0009BB30
		private SteppablePipeline GetSteppablePipeline(ExecutionContext context, CommandOrigin commandOrigin)
		{
			if (this.Commands.Commands.Count == 0)
			{
				return null;
			}
			PipelineProcessor pipelineProcessor = new PipelineProcessor();
			bool flag = false;
			try
			{
				foreach (Command command in this.Commands.Commands)
				{
					CommandProcessorBase commandProcessorBase = command.CreateCommandProcessor(Runspace.DefaultRunspace.ExecutionContext, ((LocalRunspace)Runspace.DefaultRunspace).CommandFactory, false, this.isNested ? CommandOrigin.Internal : CommandOrigin.Runspace);
					commandProcessorBase.RedirectShellErrorOutputPipe = this.redirectShellErrorOutputPipe;
					pipelineProcessor.Add(commandProcessorBase);
				}
			}
			catch (RuntimeException)
			{
				flag = true;
				throw;
			}
			catch (Exception ex)
			{
				flag = true;
				CommandProcessorBase.CheckForSevereException(ex);
				throw new RuntimeException(PipelineStrings.CannotCreatePipeline, ex);
			}
			finally
			{
				if (flag)
				{
					pipelineProcessor.Dispose();
				}
			}
			return new SteppablePipeline(context, pipelineProcessor);
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x0009DA34 File Offset: 0x0009BC34
		// (set) Token: 0x06001AAB RID: 6827 RVA: 0x0009DA3C File Offset: 0x0009BC3C
		internal bool IsGetCommandMetadataSpecialPipeline
		{
			get
			{
				return this.isGetCommandMetadataSpecialPipeline;
			}
			set
			{
				this.isGetCommandMetadataSpecialPipeline = value;
			}
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0009DA45 File Offset: 0x0009BC45
		private bool IsCommandRunning()
		{
			return this.InvocationStateInfo.State == PSInvocationState.Running;
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x0009DA58 File Offset: 0x0009BC58
		private bool IsDisconnected()
		{
			return this.InvocationStateInfo.State == PSInvocationState.Disconnected;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x0009DA68 File Offset: 0x0009BC68
		private void AssertExecutionNotStarted()
		{
			this.AssertNotDisposed();
			if (this.IsCommandRunning())
			{
				string message = StringUtil.Format(PowerShellStrings.ExecutionAlreadyStarted, new object[0]);
				throw new InvalidOperationException(message);
			}
			if (this.IsDisconnected())
			{
				string message2 = StringUtil.Format(PowerShellStrings.ExecutionDisconnected, new object[0]);
				throw new InvalidOperationException(message2);
			}
			if (this.invocationStateInfo.State == PSInvocationState.Stopping)
			{
				string message3 = StringUtil.Format(PowerShellStrings.ExecutionStopping, new object[0]);
				throw new InvalidOperationException(message3);
			}
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0009DAE4 File Offset: 0x0009BCE4
		internal void AssertChangesAreAccepted()
		{
			lock (this.syncObject)
			{
				this.AssertNotDisposed();
				if (this.IsCommandRunning() || this.IsDisconnected())
				{
					throw new InvalidPowerShellStateException(this.InvocationStateInfo.State);
				}
			}
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x0009DB48 File Offset: 0x0009BD48
		private void AssertNotDisposed()
		{
			if (this.isDisposed)
			{
				throw PSTraceSource.NewObjectDisposedException("PowerShell");
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x0009DB60 File Offset: 0x0009BD60
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this.syncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
				}
				if (this.invocationStateInfo.State == PSInvocationState.Running || this.invocationStateInfo.State == PSInvocationState.Stopping)
				{
					this.Stop();
				}
				lock (this.syncObject)
				{
					this.isDisposed = true;
				}
				if (this.outputBuffer != null && this.outputBufferOwner)
				{
					this.outputBuffer.Dispose();
				}
				if (this.errorBuffer != null && this.errorBufferOwner)
				{
					this.errorBuffer.Dispose();
				}
				if (this.runspaceOwner)
				{
					this.runspace.Dispose();
				}
				if (this.remotePowerShell != null)
				{
					this.remotePowerShell.Dispose();
				}
				this.invokeAsyncResult = null;
				this.stopAsyncResult = null;
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0009DC6C File Offset: 0x0009BE6C
		private void InternalClearSuppressExceptions()
		{
			lock (this.syncObject)
			{
				if (this.worker != null)
				{
					this.worker.InternalClearSuppressExceptions();
				}
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x0009DCBC File Offset: 0x0009BEBC
		private void RaiseStateChangeEvent(PSInvocationStateInfo stateInfo)
		{
			RemoteRunspace remoteRunspace = this.runspace as RemoteRunspace;
			if (remoteRunspace != null && !this.IsNested)
			{
				this.runspace.UpdateRunspaceAvailability(this.invocationStateInfo.State, true, this.instanceId);
			}
			if (stateInfo.State == PSInvocationState.Running)
			{
				this.AddToRemoteRunspaceRunningList();
			}
			else if (stateInfo.State == PSInvocationState.Completed || stateInfo.State == PSInvocationState.Stopped || stateInfo.State == PSInvocationState.Failed)
			{
				this.RemoveFromRemoteRunspaceRunningList();
			}
			this.InvocationStateChanged.SafeInvoke(this, new PSInvocationStateChangedEventArgs(stateInfo));
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0009DD44 File Offset: 0x0009BF44
		internal void SetStateChanged(PSInvocationStateInfo stateInfo)
		{
			PSInvocationStateInfo psinvocationStateInfo = stateInfo;
			if (this.worker != null && this.worker.CurrentlyRunningPipeline != null)
			{
				this.SetHadErrors(this.worker.CurrentlyRunningPipeline.HadErrors);
			}
			PSInvocationState state;
			PowerShellAsyncResult powerShellAsyncResult;
			PowerShellAsyncResult powerShellAsyncResult2;
			lock (this.syncObject)
			{
				state = this.invocationStateInfo.State;
				switch (this.invocationStateInfo.State)
				{
				case PSInvocationState.Running:
					if (stateInfo.State == PSInvocationState.Running)
					{
						return;
					}
					break;
				case PSInvocationState.Stopping:
					if (stateInfo.State == PSInvocationState.Running || stateInfo.State == PSInvocationState.Stopping)
					{
						return;
					}
					if (stateInfo.State == PSInvocationState.Completed || stateInfo.State == PSInvocationState.Failed)
					{
						psinvocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Stopped, stateInfo.Reason);
					}
					break;
				case PSInvocationState.Stopped:
				case PSInvocationState.Completed:
				case PSInvocationState.Failed:
					return;
				}
				powerShellAsyncResult = this.invokeAsyncResult;
				powerShellAsyncResult2 = this.stopAsyncResult;
				this.invocationStateInfo = psinvocationStateInfo;
			}
			bool flag2 = false;
			switch (this.invocationStateInfo.State)
			{
			case PSInvocationState.Running:
				this.CloseInputBufferOnReconnection(state);
				this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
				return;
			case PSInvocationState.Stopping:
				this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
				return;
			case PSInvocationState.Stopped:
			case PSInvocationState.Completed:
			case PSInvocationState.Failed:
				this.InternalClearSuppressExceptions();
				if (this.remotePowerShell != null)
				{
					this.ResumeIncomingData();
				}
				try
				{
					try
					{
						if (this.runningExtraCommands)
						{
							if (powerShellAsyncResult != null)
							{
								powerShellAsyncResult.SetAsCompleted(this.invocationStateInfo.Reason);
							}
							this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
						}
						else
						{
							this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
							if (powerShellAsyncResult != null)
							{
								powerShellAsyncResult.SetAsCompleted(this.invocationStateInfo.Reason);
							}
						}
						if (powerShellAsyncResult2 != null)
						{
							powerShellAsyncResult2.SetAsCompleted(null);
						}
					}
					catch (Exception)
					{
						flag2 = true;
						this.SetHadErrors(true);
						throw;
					}
					return;
				}
				finally
				{
					if (flag2 && powerShellAsyncResult2 != null)
					{
						powerShellAsyncResult2.Release();
					}
				}
				break;
			case PSInvocationState.Disconnected:
				break;
			default:
				return;
			}
			try
			{
				if (this.remotePowerShell != null)
				{
					this.ResumeIncomingData();
				}
				if (this.commandInvokedSynchronously && powerShellAsyncResult != null)
				{
					powerShellAsyncResult.SetAsCompleted(new RuntimeException(PowerShellStrings.DiscOnSyncCommand));
				}
				if (powerShellAsyncResult2 != null)
				{
					powerShellAsyncResult2.SetAsCompleted(null);
				}
				if (state != PSInvocationState.Disconnected)
				{
					this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
				}
			}
			catch (Exception)
			{
				flag2 = true;
				this.SetHadErrors(true);
				throw;
			}
			finally
			{
				if (flag2 && powerShellAsyncResult2 != null)
				{
					powerShellAsyncResult2.Release();
				}
			}
			this.connectCmdInfo = null;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0009DFD4 File Offset: 0x0009C1D4
		private void CloseInputBufferOnReconnection(PSInvocationState previousState)
		{
			if (previousState == PSInvocationState.Disconnected && this.commandInvokedSynchronously && this.remotePowerShell.InputStream != null && this.remotePowerShell.InputStream.IsOpen && this.remotePowerShell.InputStream.Count > 0)
			{
				this.remotePowerShell.InputStream.Close();
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x0009E030 File Offset: 0x0009C230
		internal void ClearRemotePowerShell()
		{
			lock (this.syncObject)
			{
				if (this.remotePowerShell != null)
				{
					this.remotePowerShell.Clear();
				}
			}
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0009E080 File Offset: 0x0009C280
		internal void SetIsNested(bool isNested)
		{
			this.AssertChangesAreAccepted();
			this.isNested = isNested;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0009E090 File Offset: 0x0009C290
		private void CoreInvoke<TOutput>(IEnumerable input, PSDataCollection<TOutput> output, PSInvocationSettings settings)
		{
			PSDataCollection<object> psdataCollection = null;
			if (input != null)
			{
				psdataCollection = new PSDataCollection<object>();
				foreach (object item in input)
				{
					psdataCollection.Add(item);
				}
				psdataCollection.Complete();
			}
			this.CoreInvoke<object, TOutput>(psdataCollection, output, settings);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0009E0FC File Offset: 0x0009C2FC
		private void CoreInvokeHelper<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings)
		{
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			this.Prepare<TInput, TOutput>(input, output, settings, true);
			try
			{
				if (!this.isNested)
				{
					Runspace runspace;
					if (runspacePool != null)
					{
						this.VerifyThreadSettings(settings, runspacePool.ApartmentState, runspacePool.ThreadOptions, false);
						this.worker.GetRunspaceAsyncResult = runspacePool.BeginGetRunspace(null, null);
						this.worker.GetRunspaceAsyncResult.AsyncWaitHandle.WaitOne();
						runspace = runspacePool.EndGetRunspace(this.worker.GetRunspaceAsyncResult);
					}
					else
					{
						runspace = (this.rsConnection as Runspace);
						if (runspace != null)
						{
							this.VerifyThreadSettings(settings, runspace.ApartmentState, runspace.ThreadOptions, false);
							if (runspace.RunspaceStateInfo.State != RunspaceState.Opened)
							{
								string message = StringUtil.Format(PowerShellStrings.InvalidRunspaceState, RunspaceState.Opened, runspace.RunspaceStateInfo.State);
								InvalidRunspaceStateException ex = new InvalidRunspaceStateException(message, runspace.RunspaceStateInfo.State, RunspaceState.Opened);
								throw ex;
							}
						}
					}
					this.worker.CreateRunspaceIfNeededAndDoWork(runspace, true);
				}
				else
				{
					Runspace runspace = this.rsConnection as Runspace;
					this.worker.ConstructPipelineAndDoWork(runspace, true);
				}
			}
			catch (Exception ex2)
			{
				this.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Failed, ex2));
				InvalidRunspacePoolStateException ex3 = ex2 as InvalidRunspacePoolStateException;
				if (ex3 != null && this.runspace != null)
				{
					throw ex3.ToInvalidRunspaceStateException();
				}
				throw;
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0009E254 File Offset: 0x0009C454
		private void CoreInvokeRemoteHelper<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings)
		{
			IAsyncResult asyncResult = this.CoreInvokeAsync<TInput, TOutput>(input, output, settings, null, null, null);
			this.commandInvokedSynchronously = true;
			PowerShellAsyncResult powerShellAsyncResult = asyncResult as PowerShellAsyncResult;
			this.EndInvokeAsyncResult = powerShellAsyncResult;
			powerShellAsyncResult.EndInvoke();
			this.EndInvokeAsyncResult = null;
			if (PSInvocationState.Failed == this.invocationStateInfo.State && this.invocationStateInfo.Reason != null)
			{
				throw this.invocationStateInfo.Reason;
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0009E2B8 File Offset: 0x0009C4B8
		private void CoreInvoke<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings)
		{
			bool flag = false;
			this.DetermineIsBatching();
			if (this.isBatching)
			{
				this.SetupAsyncBatchExecution();
			}
			this.SetHadErrors(false);
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			if (runspacePool != null && runspacePool.IsRemote)
			{
				if (this.ServerSupportsBatchInvocation())
				{
					try
					{
						this.CoreInvokeRemoteHelper<TInput, TOutput>(input, output, settings);
					}
					finally
					{
						if (this.isBatching)
						{
							this.EndAsyncBatchExecution();
						}
					}
					return;
				}
				flag = true;
			}
			if (this.isBatching)
			{
				try
				{
					foreach (PSCommand pscommand in this.extraCommands)
					{
						if (this.psCommand != this.extraCommands[this.extraCommands.Count - 1])
						{
							this.runningExtraCommands = true;
						}
						else
						{
							this.runningExtraCommands = false;
						}
						try
						{
							this.psCommand = pscommand;
							if (flag)
							{
								this.CoreInvokeRemoteHelper<TInput, TOutput>(input, output, settings);
							}
							else
							{
								this.CoreInvokeHelper<TInput, TOutput>(input, output, settings);
							}
						}
						catch (ActionPreferenceStopException)
						{
							throw;
						}
						catch (Exception ex)
						{
							CommandProcessorBase.CheckForSevereException(ex);
							this.SetHadErrors(true);
							if (settings != null && settings.ErrorActionPreference == ActionPreference.Stop)
							{
								throw;
							}
							if (settings == null || !(settings.ErrorActionPreference == ActionPreference.Ignore))
							{
								IContainsErrorRecord containsErrorRecord = ex as IContainsErrorRecord;
								if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
								{
									this.Streams.Error.Add(containsErrorRecord.ErrorRecord);
								}
								else
								{
									this.Streams.Error.Add(new ErrorRecord(ex, "InvalidOperation", ErrorCategory.InvalidOperation, null));
								}
							}
						}
					}
					return;
				}
				finally
				{
					this.runningExtraCommands = false;
					if (this.isBatching)
					{
						this.EndAsyncBatchExecution();
					}
				}
			}
			this.runningExtraCommands = false;
			if (flag)
			{
				this.CoreInvokeRemoteHelper<TInput, TOutput>(input, output, settings);
				return;
			}
			this.CoreInvokeHelper<TInput, TOutput>(input, output, settings);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0009E508 File Offset: 0x0009C708
		private IAsyncResult CoreInvokeAsync<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings, AsyncCallback callback, object state, PSDataCollection<PSObject> asyncResultOutput)
		{
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			this.Prepare<TInput, TOutput>(input, output, settings, runspacePool == null || !runspacePool.IsRemote);
			this.invokeAsyncResult = new PowerShellAsyncResult(this.instanceId, callback, state, asyncResultOutput, true);
			try
			{
				if (this.isNested && (runspacePool == null || !runspacePool.IsRemote))
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.NestedPowerShellInvokeAsync, new object[0]);
				}
				if (runspacePool != null)
				{
					this.VerifyThreadSettings(settings, runspacePool.ApartmentState, runspacePool.ThreadOptions, runspacePool.IsRemote);
					runspacePool.AssertPoolIsOpen();
					if (runspacePool.IsRemote)
					{
						this.worker = null;
						lock (this.syncObject)
						{
							this.AssertExecutionNotStarted();
							this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Running, null);
							ObjectStreamBase objectStreamBase = null;
							if (input != null)
							{
								objectStreamBase = new PSDataCollectionStream<TInput>(this.instanceId, input);
							}
							if (!this.remotePowerShell.Initialized)
							{
								if (objectStreamBase == null)
								{
									objectStreamBase = new ObjectStream();
									objectStreamBase.Close();
								}
								this.remotePowerShell.Initialize(objectStreamBase, new PSDataCollectionStream<TOutput>(this.instanceId, output), new PSDataCollectionStream<ErrorRecord>(this.instanceId, this.errorBuffer), this.informationalBuffers, settings);
							}
							else
							{
								if (objectStreamBase != null)
								{
									this.remotePowerShell.InputStream = objectStreamBase;
								}
								if (output != null)
								{
									this.remotePowerShell.OutputStream = new PSDataCollectionStream<TOutput>(this.instanceId, output);
								}
							}
							runspacePool.RemoteRunspacePoolInternal.CreatePowerShellOnServerAndInvoke(this.remotePowerShell);
						}
						this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
					}
					else
					{
						this.worker.GetRunspaceAsyncResult = runspacePool.BeginGetRunspace(new AsyncCallback(this.worker.RunspaceAvailableCallback), null);
					}
				}
				else
				{
					LocalRunspace localRunspace = this.rsConnection as LocalRunspace;
					if (localRunspace != null)
					{
						this.VerifyThreadSettings(settings, localRunspace.ApartmentState, localRunspace.ThreadOptions, false);
						if (localRunspace.RunspaceStateInfo.State != RunspaceState.Opened)
						{
							string message = StringUtil.Format(PowerShellStrings.InvalidRunspaceState, RunspaceState.Opened, localRunspace.RunspaceStateInfo.State);
							InvalidRunspaceStateException ex = new InvalidRunspaceStateException(message, localRunspace.RunspaceStateInfo.State, RunspaceState.Opened);
							throw ex;
						}
						this.worker.CreateRunspaceIfNeededAndDoWork(localRunspace, false);
					}
					else
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.worker.CreateRunspaceIfNeededAndDoWork), this.rsConnection);
					}
				}
			}
			catch (Exception ex2)
			{
				this.invokeAsyncResult = null;
				this.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Failed, ex2));
				InvalidRunspacePoolStateException ex3 = ex2 as InvalidRunspacePoolStateException;
				if (ex3 != null && this.runspace != null)
				{
					throw ex3.ToInvalidRunspaceStateException();
				}
				throw;
			}
			return this.invokeAsyncResult;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0009E7C8 File Offset: 0x0009C9C8
		private void VerifyThreadSettings(PSInvocationSettings settings, ApartmentState runspaceApartmentState, PSThreadOptions runspaceThreadOptions, bool isRemote)
		{
			ApartmentState apartmentState;
			if (settings != null && settings.ApartmentState != ApartmentState.Unknown)
			{
				apartmentState = settings.ApartmentState;
			}
			else
			{
				apartmentState = runspaceApartmentState;
			}
			if (runspaceThreadOptions == PSThreadOptions.ReuseThread)
			{
				if (apartmentState != runspaceApartmentState)
				{
					throw new InvalidOperationException(PowerShellStrings.ApartmentStateMismatch);
				}
			}
			else if (runspaceThreadOptions == PSThreadOptions.UseCurrentThread && !isRemote && apartmentState != ApartmentState.Unknown && apartmentState != Thread.CurrentThread.GetApartmentState())
			{
				throw new InvalidOperationException(PowerShellStrings.ApartmentStateMismatchCurrentThread);
			}
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x0009E824 File Offset: 0x0009CA24
		private void Prepare<TInput, TOutput>(PSDataCollection<TInput> input, PSDataCollection<TOutput> output, PSInvocationSettings settings, bool shouldCreateWorker)
		{
			lock (this.syncObject)
			{
				if (this.psCommand == null || this.psCommand.Commands == null || this.psCommand.Commands.Count == 0)
				{
					throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.NoCommandToInvoke, new object[0]);
				}
				this.AssertExecutionNotStarted();
				if (shouldCreateWorker)
				{
					this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Running, null);
					if (settings != null && settings.FlowImpersonationPolicy)
					{
						settings.WindowsIdentityToImpersonate = WindowsIdentity.GetCurrent(false);
					}
					ObjectStreamBase objectStreamBase;
					if (input != null)
					{
						objectStreamBase = new PSDataCollectionStream<TInput>(this.instanceId, input);
					}
					else
					{
						objectStreamBase = new ObjectStream();
						objectStreamBase.Close();
					}
					ObjectStreamBase outputStream = new PSDataCollectionStream<TOutput>(this.instanceId, output);
					this.worker = new PowerShell.Worker(objectStreamBase, outputStream, settings, this);
				}
			}
			if (shouldCreateWorker)
			{
				this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
			}
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0009E914 File Offset: 0x0009CB14
		private IAsyncResult CoreStop(bool isSyncCall, AsyncCallback callback, object state)
		{
			bool flag = false;
			bool flag2 = false;
			Queue<PSInvocationStateInfo> queue = new Queue<PSInvocationStateInfo>();
			lock (this.syncObject)
			{
				switch (this.invocationStateInfo.State)
				{
				case PSInvocationState.NotStarted:
					this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Stopping, null);
					queue.Enqueue(new PSInvocationStateInfo(PSInvocationState.Stopped, null));
					break;
				case PSInvocationState.Running:
					this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Stopping, null);
					flag = true;
					break;
				case PSInvocationState.Stopping:
					if (this.stopAsyncResult == null)
					{
						this.stopAsyncResult = new PowerShellAsyncResult(this.instanceId, callback, state, null, false);
						this.stopAsyncResult.SetAsCompleted(null);
					}
					return this.stopAsyncResult;
				case PSInvocationState.Stopped:
				case PSInvocationState.Completed:
				case PSInvocationState.Failed:
					this.stopAsyncResult = new PowerShellAsyncResult(this.instanceId, callback, state, null, false);
					this.stopAsyncResult.SetAsCompleted(null);
					return this.stopAsyncResult;
				case PSInvocationState.Disconnected:
					this.invocationStateInfo = new PSInvocationStateInfo(PSInvocationState.Failed, null);
					flag2 = true;
					break;
				}
				this.stopAsyncResult = new PowerShellAsyncResult(this.instanceId, callback, state, null, false);
			}
			if (flag2)
			{
				if (this.invokeAsyncResult != null)
				{
					this.invokeAsyncResult.SetAsCompleted(null);
				}
				this.stopAsyncResult.SetAsCompleted(null);
				this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
				return this.stopAsyncResult;
			}
			this.ReleaseDebugger();
			this.RaiseStateChangeEvent(this.invocationStateInfo.Clone());
			bool flag4 = false;
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			if (runspacePool != null && runspacePool.IsRemote)
			{
				if (this.remotePowerShell != null && this.remotePowerShell.Initialized)
				{
					this.remotePowerShell.StopAsync();
					if (isSyncCall)
					{
						this.stopAsyncResult.AsyncWaitHandle.WaitOne();
					}
				}
				else
				{
					flag4 = true;
				}
			}
			else if (flag)
			{
				this.worker.Stop(isSyncCall);
			}
			else
			{
				flag4 = true;
			}
			if (flag4)
			{
				if (isSyncCall)
				{
					this.StopHelper(queue);
				}
				else
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.StopThreadProc), queue);
				}
			}
			return this.stopAsyncResult;
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0009EB30 File Offset: 0x0009CD30
		private void ReleaseDebugger()
		{
			LocalRunspace localRunspace = this.runspace as LocalRunspace;
			if (localRunspace != null)
			{
				localRunspace.ReleaseDebugger();
			}
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x0009EB54 File Offset: 0x0009CD54
		private void StopHelper(object state)
		{
			Queue<PSInvocationStateInfo> queue = state as Queue<PSInvocationStateInfo>;
			while (queue.Count > 0)
			{
				PSInvocationStateInfo stateChanged = queue.Dequeue();
				this.SetStateChanged(stateChanged);
			}
			this.InternalClearSuppressExceptions();
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0009EB88 File Offset: 0x0009CD88
		private void StopThreadProc(object state)
		{
			try
			{
				this.StopHelper(state);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0009EBB8 File Offset: 0x0009CDB8
		internal ClientRemotePowerShell RemotePowerShell
		{
			get
			{
				return this.remotePowerShell;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0009EBC0 File Offset: 0x0009CDC0
		// (set) Token: 0x06001AC5 RID: 6853 RVA: 0x0009EBC8 File Offset: 0x0009CDC8
		public string HistoryString
		{
			get
			{
				return this.historyString;
			}
			set
			{
				this.historyString = value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x0009EBD1 File Offset: 0x0009CDD1
		internal Collection<PSCommand> ExtraCommands
		{
			get
			{
				return this.extraCommands;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0009EBD9 File Offset: 0x0009CDD9
		internal bool RunningExtraCommands
		{
			get
			{
				return this.runningExtraCommands;
			}
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0009EBE4 File Offset: 0x0009CDE4
		private bool ServerSupportsBatchInvocation()
		{
			if (this.runspace != null)
			{
				return this.runspace.RunspaceStateInfo.State != RunspaceState.BeforeOpen && this.runspace.GetRemoteProtocolVersion() >= RemotingConstants.ProtocolVersionWin8RTM;
			}
			RemoteRunspacePoolInternal remoteRunspacePoolInternal = null;
			if (this.rsConnection is RemoteRunspace)
			{
				remoteRunspacePoolInternal = (this.rsConnection as RemoteRunspace).RunspacePool.RemoteRunspacePoolInternal;
			}
			else if (this.rsConnection is RunspacePool)
			{
				remoteRunspacePoolInternal = (this.rsConnection as RunspacePool).RemoteRunspacePoolInternal;
			}
			return remoteRunspacePoolInternal != null && remoteRunspacePoolInternal.PSRemotingProtocolVersion >= RemotingConstants.ProtocolVersionWin8RTM;
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0009EC80 File Offset: 0x0009CE80
		private void AddToRemoteRunspaceRunningList()
		{
			if (this.runspace != null)
			{
				this.runspace.PushRunningPowerShell(this);
				return;
			}
			RemoteRunspacePoolInternal remoteRunspacePoolInternal = this.GetRemoteRunspacePoolInternal();
			if (remoteRunspacePoolInternal != null)
			{
				remoteRunspacePoolInternal.PushRunningPowerShell(this);
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0009ECB4 File Offset: 0x0009CEB4
		private void RemoveFromRemoteRunspaceRunningList()
		{
			if (this.runspace != null)
			{
				this.runspace.PopRunningPowerShell();
				return;
			}
			RemoteRunspacePoolInternal remoteRunspacePoolInternal = this.GetRemoteRunspacePoolInternal();
			if (remoteRunspacePoolInternal != null)
			{
				remoteRunspacePoolInternal.PopRunningPowerShell();
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0009ECE8 File Offset: 0x0009CEE8
		private RemoteRunspacePoolInternal GetRemoteRunspacePoolInternal()
		{
			RunspacePool runspacePool = this.rsConnection as RunspacePool;
			if (runspacePool == null)
			{
				return null;
			}
			return runspacePool.RemoteRunspacePoolInternal;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0009ED0C File Offset: 0x0009CF0C
		internal static PowerShell FromPSObjectForRemoting(PSObject powerShellAsPSObject)
		{
			if (powerShellAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("powerShellAsPSObject");
			}
			Collection<PSCommand> collection = null;
			ReadOnlyPSMemberInfoCollection<PSPropertyInfo> readOnlyPSMemberInfoCollection = powerShellAsPSObject.Properties.Match("ExtraCmds");
			if (readOnlyPSMemberInfoCollection.Count > 0)
			{
				collection = new Collection<PSCommand>();
				foreach (PSObject psObject in RemotingDecoder.EnumerateListProperty<PSObject>(powerShellAsPSObject, "ExtraCmds"))
				{
					PSCommand pscommand = null;
					foreach (PSObject commandAsPSObject in RemotingDecoder.EnumerateListProperty<PSObject>(psObject, "Cmds"))
					{
						Command command = Command.FromPSObjectForRemoting(commandAsPSObject);
						if (pscommand == null)
						{
							pscommand = new PSCommand(command);
						}
						else
						{
							pscommand.AddCommand(command);
						}
					}
					collection.Add(pscommand);
				}
			}
			PSCommand pscommand2 = null;
			foreach (PSObject commandAsPSObject2 in RemotingDecoder.EnumerateListProperty<PSObject>(powerShellAsPSObject, "Cmds"))
			{
				Command command2 = Command.FromPSObjectForRemoting(commandAsPSObject2);
				if (pscommand2 == null)
				{
					pscommand2 = new PSCommand(command2);
				}
				else
				{
					pscommand2.AddCommand(command2);
				}
			}
			bool propertyValue = RemotingDecoder.GetPropertyValue<bool>(powerShellAsPSObject, "IsNested");
			PowerShell powerShell = PowerShell.Create(propertyValue, pscommand2, collection);
			powerShell.HistoryString = RemotingDecoder.GetPropertyValue<string>(powerShellAsPSObject, "History");
			powerShell.RedirectShellErrorOutputPipe = RemotingDecoder.GetPropertyValue<bool>(powerShellAsPSObject, "RedirectShellErrorOutputPipe");
			return powerShell;
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0009EE9C File Offset: 0x0009D09C
		internal PSObject ToPSObjectForRemoting()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			Version psremotingProtocolVersion = RemotingEncoder.GetPSRemotingProtocolVersion(this.rsConnection as RunspacePool);
			if (this.ServerSupportsBatchInvocation() && this.extraCommands.Count > 0)
			{
				List<PSObject> list = new List<PSObject>(this.extraCommands.Count);
				foreach (PSCommand pscommand in this.extraCommands)
				{
					PSObject psobject2 = RemotingEncoder.CreateEmptyPSObject();
					psobject2.Properties.Add(new PSNoteProperty("Cmds", this.CommandsAsListOfPSObjects(pscommand.Commands, psremotingProtocolVersion)));
					list.Add(psobject2);
				}
				psobject.Properties.Add(new PSNoteProperty("ExtraCmds", list));
			}
			List<PSObject> value = this.CommandsAsListOfPSObjects(this.Commands.Commands, psremotingProtocolVersion);
			psobject.Properties.Add(new PSNoteProperty("Cmds", value));
			psobject.Properties.Add(new PSNoteProperty("IsNested", this.IsNested));
			psobject.Properties.Add(new PSNoteProperty("History", this.historyString));
			psobject.Properties.Add(new PSNoteProperty("RedirectShellErrorOutputPipe", this.RedirectShellErrorOutputPipe));
			return psobject;
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0009EFFC File Offset: 0x0009D1FC
		private List<PSObject> CommandsAsListOfPSObjects(CommandCollection commands, Version psRPVersion)
		{
			List<PSObject> list = new List<PSObject>(commands.Count);
			foreach (Command command in commands)
			{
				list.Add(command.ToPSObjectForRemoting(psRPVersion));
			}
			return list;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0009F058 File Offset: 0x0009D258
		internal void SuspendIncomingData()
		{
			if (this.remotePowerShell == null)
			{
				throw new PSNotSupportedException();
			}
			if (this.remotePowerShell.DataStructureHandler != null)
			{
				this.remotePowerShell.DataStructureHandler.TransportManager.SuspendQueue(true);
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0009F08B File Offset: 0x0009D28B
		internal void ResumeIncomingData()
		{
			if (this.remotePowerShell == null)
			{
				throw new PSNotSupportedException();
			}
			if (this.remotePowerShell.DataStructureHandler != null)
			{
				this.remotePowerShell.DataStructureHandler.TransportManager.ResumeQueue();
			}
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0009F0C0 File Offset: 0x0009D2C0
		internal void WaitForServicingComplete()
		{
			if (this.remotePowerShell == null)
			{
				throw new PSNotSupportedException();
			}
			if (this.remotePowerShell.DataStructureHandler != null)
			{
				int num = 0;
				while (++num < 2 && this.remotePowerShell.DataStructureHandler.TransportManager.IsServicing)
				{
					Thread.Sleep(50);
				}
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0009F114 File Offset: 0x0009D314
		public PSJobProxy AsJobProxy()
		{
			if (this.Commands.Commands.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.GetJobForCommandRequiresACommand, new object[0]);
			}
			if (this.Commands.Commands.Count > 1)
			{
				throw PSTraceSource.NewInvalidOperationException(PowerShellStrings.GetJobForCommandNotSupported, new object[0]);
			}
			bool flag = false;
			foreach (CommandParameter commandParameter in this.Commands.Commands[0].Parameters)
			{
				if (string.Compare(commandParameter.Name, "AsJob", StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.AddParameter("AsJob");
			}
			PSJobProxy psjobProxy = new PSJobProxy(this.Commands.Commands[0].CommandText);
			psjobProxy.InitializeJobProxy(this.Commands, this.Runspace, this.RunspacePool);
			return psjobProxy;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x0009F20C File Offset: 0x0009D40C
		internal CimInstance AsPSPowerShellPipeline()
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance("PS_PowerShellPipeline");
			CimProperty newItem = InternalMISerializer.CreateCimProperty("InstanceId", this.InstanceId.ToString(), CimType.String);
			cimInstance.CimInstanceProperties.Add(newItem);
			CimProperty newItem2 = InternalMISerializer.CreateCimProperty("IsNested", this.IsNested, CimType.Boolean);
			cimInstance.CimInstanceProperties.Add(newItem2);
			bool flag = false;
			bool flag2 = false;
			uint num = 0U;
			if (this.worker != null)
			{
				this.worker.GetSettings(out flag, out flag2, out num);
			}
			CimProperty newItem3 = InternalMISerializer.CreateCimProperty("AddToHistory", flag, CimType.Boolean);
			cimInstance.CimInstanceProperties.Add(newItem3);
			CimProperty newItem4 = InternalMISerializer.CreateCimProperty("NoInput", flag2, CimType.Boolean);
			cimInstance.CimInstanceProperties.Add(newItem4);
			CimProperty newItem5 = InternalMISerializer.CreateCimProperty("ApartmentState", num, CimType.UInt32);
			cimInstance.CimInstanceProperties.Add(newItem5);
			if (this.Commands.Commands.Count > 0)
			{
				List<CimInstance> list = new List<CimInstance>();
				foreach (Command command in this.Commands.Commands)
				{
					list.Add(command.ToCimInstance());
				}
				CimProperty newItem6 = InternalMISerializer.CreateCimProperty("Commands", list.ToArray(), CimType.ReferenceArray);
				cimInstance.CimInstanceProperties.Add(newItem6);
			}
			return cimInstance;
		}

		// Token: 0x04000AE6 RID: 2790
		private bool isGetCommandMetadataSpecialPipeline;

		// Token: 0x04000AE7 RID: 2791
		private PSCommand psCommand;

		// Token: 0x04000AE8 RID: 2792
		private Collection<PSCommand> extraCommands;

		// Token: 0x04000AE9 RID: 2793
		private bool runningExtraCommands;

		// Token: 0x04000AEA RID: 2794
		private PowerShell.Worker worker;

		// Token: 0x04000AEB RID: 2795
		private PSInvocationStateInfo invocationStateInfo;

		// Token: 0x04000AEC RID: 2796
		private PowerShellAsyncResult invokeAsyncResult;

		// Token: 0x04000AED RID: 2797
		private PowerShellAsyncResult stopAsyncResult;

		// Token: 0x04000AEE RID: 2798
		private PowerShellAsyncResult batchAsyncResult;

		// Token: 0x04000AEF RID: 2799
		private PSInvocationSettings batchInvocationSettings;

		// Token: 0x04000AF0 RID: 2800
		private PSCommand backupPSCommand;

		// Token: 0x04000AF1 RID: 2801
		private bool isNested;

		// Token: 0x04000AF2 RID: 2802
		private bool isChild;

		// Token: 0x04000AF3 RID: 2803
		private object rsConnection;

		// Token: 0x04000AF4 RID: 2804
		private PSDataCollection<PSObject> outputBuffer;

		// Token: 0x04000AF5 RID: 2805
		private bool outputBufferOwner = true;

		// Token: 0x04000AF6 RID: 2806
		private PSDataCollection<ErrorRecord> errorBuffer;

		// Token: 0x04000AF7 RID: 2807
		private bool errorBufferOwner = true;

		// Token: 0x04000AF8 RID: 2808
		private PSInformationalBuffers informationalBuffers;

		// Token: 0x04000AF9 RID: 2809
		private PSDataStreams dataStreams;

		// Token: 0x04000AFA RID: 2810
		private bool isDisposed;

		// Token: 0x04000AFB RID: 2811
		private Guid instanceId;

		// Token: 0x04000AFC RID: 2812
		private object syncObject = new object();

		// Token: 0x04000AFD RID: 2813
		private ClientRemotePowerShell remotePowerShell;

		// Token: 0x04000AFE RID: 2814
		private string historyString;

		// Token: 0x04000AFF RID: 2815
		private ConnectCommandInfo connectCmdInfo;

		// Token: 0x04000B00 RID: 2816
		private bool commandInvokedSynchronously;

		// Token: 0x04000B01 RID: 2817
		private bool isBatching;

		// Token: 0x04000B02 RID: 2818
		private bool stopBatchExecution;

		// Token: 0x04000B03 RID: 2819
		private bool redirectShellErrorOutputPipe = true;

		// Token: 0x04000B04 RID: 2820
		private bool _hadErrors;

		// Token: 0x04000B07 RID: 2823
		private Runspace runspace;

		// Token: 0x04000B08 RID: 2824
		private bool runspaceOwner;

		// Token: 0x04000B09 RID: 2825
		private RunspacePool runspacePool;

		// Token: 0x02000239 RID: 569
		private sealed class Worker
		{
			// Token: 0x06001AD4 RID: 6868 RVA: 0x0009F388 File Offset: 0x0009D588
			internal Worker(ObjectStreamBase inputStream, ObjectStreamBase outputStream, PSInvocationSettings settings, PowerShell shell)
			{
				this.inputStream = inputStream;
				this.outputStream = outputStream;
				this.errorStream = new PSDataCollectionStream<ErrorRecord>(shell.instanceId, shell.errorBuffer);
				this.settings = settings;
				this.shell = shell;
			}

			// Token: 0x1700069D RID: 1693
			// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x0009F3DC File Offset: 0x0009D5DC
			// (set) Token: 0x06001AD6 RID: 6870 RVA: 0x0009F3E4 File Offset: 0x0009D5E4
			internal IAsyncResult GetRunspaceAsyncResult
			{
				get
				{
					return this.getRunspaceAsyncResult;
				}
				set
				{
					this.getRunspaceAsyncResult = value;
				}
			}

			// Token: 0x1700069E RID: 1694
			// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x0009F3ED File Offset: 0x0009D5ED
			internal Pipeline CurrentlyRunningPipeline
			{
				get
				{
					return this.currentlyRunningPipeline;
				}
			}

			// Token: 0x06001AD8 RID: 6872 RVA: 0x0009F3F8 File Offset: 0x0009D5F8
			internal void CreateRunspaceIfNeededAndDoWork(object state)
			{
				Runspace rsToUse = state as Runspace;
				this.CreateRunspaceIfNeededAndDoWork(rsToUse, false);
			}

			// Token: 0x06001AD9 RID: 6873 RVA: 0x0009F414 File Offset: 0x0009D614
			internal void CreateRunspaceIfNeededAndDoWork(Runspace rsToUse, bool isSync)
			{
				try
				{
					if (!(rsToUse is LocalRunspace))
					{
						lock (this.shell.syncObject)
						{
							if (this.shell.runspace != null)
							{
								rsToUse = this.shell.runspace;
							}
							else
							{
								Runspace runspace;
								if (this.settings != null && this.settings.Host != null)
								{
									runspace = RunspaceFactory.CreateRunspace(this.settings.Host);
								}
								else
								{
									runspace = RunspaceFactory.CreateRunspace();
								}
								this.shell.SetRunspace(runspace, true);
								rsToUse = (LocalRunspace)runspace;
								rsToUse.Open();
							}
						}
					}
					this.ConstructPipelineAndDoWork(rsToUse, isSync);
				}
				catch (Exception ex)
				{
					lock (this.syncObject)
					{
						if (this.isNotActive)
						{
							return;
						}
						this.isNotActive = true;
					}
					this.shell.PipelineStateChanged(this, new PipelineStateEventArgs(new PipelineStateInfo(PipelineState.Failed, ex)));
					if (isSync)
					{
						throw;
					}
					CommandProcessorBase.CheckForSevereException(ex);
				}
			}

			// Token: 0x06001ADA RID: 6874 RVA: 0x0009F548 File Offset: 0x0009D748
			internal void RunspaceAvailableCallback(IAsyncResult asyncResult)
			{
				try
				{
					RunspacePool runspacePool = this.shell.rsConnection as RunspacePool;
					Runspace runspace = runspacePool.EndGetRunspace(asyncResult);
					if (!this.ConstructPipelineAndDoWork(runspace, false))
					{
						runspacePool.ReleaseRunspace(runspace);
					}
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					lock (this.syncObject)
					{
						if (this.isNotActive)
						{
							return;
						}
						this.isNotActive = true;
					}
					this.shell.PipelineStateChanged(this, new PipelineStateEventArgs(new PipelineStateInfo(PipelineState.Failed, ex)));
				}
			}

			// Token: 0x06001ADB RID: 6875 RVA: 0x0009F5F4 File Offset: 0x0009D7F4
			internal bool ConstructPipelineAndDoWork(Runspace rs, bool performSyncInvoke)
			{
				this.shell.RunspaceAssigned.SafeInvoke(this, new PSEventArgs<Runspace>(rs));
				LocalRunspace localRunspace = rs as LocalRunspace;
				lock (this.syncObject)
				{
					if (this.isNotActive)
					{
						return false;
					}
					if (localRunspace == null)
					{
						throw PSTraceSource.NewNotImplementedException();
					}
					LocalPipeline localPipeline = new LocalPipeline(localRunspace, this.shell.Commands.Commands, this.settings != null && this.settings.AddToHistory, this.shell.IsNested, this.inputStream, this.outputStream, this.errorStream, this.shell.informationalBuffers);
					localPipeline.IsChild = this.shell.IsChild;
					if (!string.IsNullOrEmpty(this.shell.HistoryString))
					{
						localPipeline.SetHistoryString(this.shell.HistoryString);
					}
					localPipeline.RedirectShellErrorOutputPipe = this.shell.RedirectShellErrorOutputPipe;
					this.currentlyRunningPipeline = localPipeline;
					this.currentlyRunningPipeline.StateChanged += this.shell.PipelineStateChanged;
				}
				this.currentlyRunningPipeline.InvocationSettings = this.settings;
				if (performSyncInvoke)
				{
					this.currentlyRunningPipeline.Invoke();
				}
				else
				{
					this.currentlyRunningPipeline.InvokeAsync();
				}
				return true;
			}

			// Token: 0x06001ADC RID: 6876 RVA: 0x0009F75C File Offset: 0x0009D95C
			internal void Stop(bool isSyncCall)
			{
				lock (this.syncObject)
				{
					if (this.isNotActive)
					{
						return;
					}
					this.isNotActive = true;
					if (this.currentlyRunningPipeline != null)
					{
						if (isSyncCall)
						{
							this.currentlyRunningPipeline.Stop();
						}
						else
						{
							this.currentlyRunningPipeline.StopAsync();
						}
						return;
					}
					if (this.getRunspaceAsyncResult != null)
					{
						RunspacePool runspacePool = this.shell.rsConnection as RunspacePool;
						runspacePool.CancelGetRunspace(this.getRunspaceAsyncResult);
					}
				}
				Queue<PSInvocationStateInfo> queue = new Queue<PSInvocationStateInfo>();
				queue.Enqueue(new PSInvocationStateInfo(PSInvocationState.Stopped, null));
				if (isSyncCall)
				{
					this.shell.StopHelper(queue);
					return;
				}
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.shell.StopThreadProc), queue);
			}

			// Token: 0x06001ADD RID: 6877 RVA: 0x0009F830 File Offset: 0x0009DA30
			internal void InternalClearSuppressExceptions()
			{
				try
				{
					if (this.settings != null && this.settings.WindowsIdentityToImpersonate != null)
					{
						this.settings.WindowsIdentityToImpersonate.Dispose();
						this.settings.WindowsIdentityToImpersonate = null;
					}
					this.inputStream.Close();
					this.outputStream.Close();
					this.errorStream.Close();
					if (this.currentlyRunningPipeline == null)
					{
						return;
					}
					this.currentlyRunningPipeline.StateChanged -= this.shell.PipelineStateChanged;
					if (this.getRunspaceAsyncResult == null && this.shell.rsConnection == null)
					{
						this.currentlyRunningPipeline.Runspace.Close();
					}
					else
					{
						RunspacePool runspacePool = this.shell.rsConnection as RunspacePool;
						if (runspacePool != null)
						{
							runspacePool.ReleaseRunspace(this.currentlyRunningPipeline.Runspace);
						}
					}
					this.currentlyRunningPipeline.Dispose();
				}
				catch (ArgumentException)
				{
				}
				catch (InvalidOperationException)
				{
				}
				catch (InvalidRunspaceStateException)
				{
				}
				catch (InvalidRunspacePoolStateException)
				{
				}
				this.currentlyRunningPipeline = null;
			}

			// Token: 0x06001ADE RID: 6878 RVA: 0x0009F958 File Offset: 0x0009DB58
			internal void GetSettings(out bool addToHistory, out bool noInput, out uint apartmentState)
			{
				addToHistory = this.settings.AddToHistory;
				noInput = false;
				apartmentState = (uint)this.settings.ApartmentState;
			}

			// Token: 0x04000B0B RID: 2827
			private ObjectStreamBase inputStream;

			// Token: 0x04000B0C RID: 2828
			private ObjectStreamBase outputStream;

			// Token: 0x04000B0D RID: 2829
			private ObjectStreamBase errorStream;

			// Token: 0x04000B0E RID: 2830
			private PSInvocationSettings settings;

			// Token: 0x04000B0F RID: 2831
			private IAsyncResult getRunspaceAsyncResult;

			// Token: 0x04000B10 RID: 2832
			private Pipeline currentlyRunningPipeline;

			// Token: 0x04000B11 RID: 2833
			private bool isNotActive;

			// Token: 0x04000B12 RID: 2834
			private PowerShell shell;

			// Token: 0x04000B13 RID: 2835
			private object syncObject = new object();
		}
	}
}
