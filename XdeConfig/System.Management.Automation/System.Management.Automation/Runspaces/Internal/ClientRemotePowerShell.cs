using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x020002A2 RID: 674
	internal class ClientRemotePowerShell : IDisposable
	{
		// Token: 0x06002061 RID: 8289 RVA: 0x000BBD1B File Offset: 0x000B9F1B
		internal ClientRemotePowerShell(PowerShell shell, RemoteRunspacePoolInternal runspacePool)
		{
			this.shell = shell;
			this.clientRunspacePoolId = runspacePool.InstanceId;
			this.runspacePool = runspacePool;
			this.computerName = runspacePool.ConnectionInfo.ComputerName;
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x000BBD59 File Offset: 0x000B9F59
		internal Guid InstanceId
		{
			get
			{
				return this.PowerShell.InstanceId;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x000BBD66 File Offset: 0x000B9F66
		internal PowerShell PowerShell
		{
			get
			{
				return this.shell;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000BBD6E File Offset: 0x000B9F6E
		internal void SetStateInfo(PSInvocationStateInfo stateInfo)
		{
			this.shell.SetStateChanged(stateInfo);
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x000BBD7C File Offset: 0x000B9F7C
		internal bool NoInput
		{
			get
			{
				return this.noInput;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x000BBD84 File Offset: 0x000B9F84
		// (set) Token: 0x06002067 RID: 8295 RVA: 0x000BBD8C File Offset: 0x000B9F8C
		internal ObjectStreamBase InputStream
		{
			get
			{
				return this.inputstream;
			}
			set
			{
				this.inputstream = value;
				if (this.inputstream != null && (this.inputstream.IsOpen || this.inputstream.Count > 0))
				{
					this.noInput = false;
					return;
				}
				this.noInput = true;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000BBDC7 File Offset: 0x000B9FC7
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x000BBDCF File Offset: 0x000B9FCF
		internal ObjectStreamBase OutputStream
		{
			get
			{
				return this.outputstream;
			}
			set
			{
				this.outputstream = value;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000BBDD8 File Offset: 0x000B9FD8
		internal ClientPowerShellDataStructureHandler DataStructureHandler
		{
			get
			{
				return this.dataStructureHandler;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x000BBDE0 File Offset: 0x000B9FE0
		internal PSInvocationSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000BBDE8 File Offset: 0x000B9FE8
		internal void UnblockCollections()
		{
			this.shell.ClearRemotePowerShell();
			this.outputstream.Close();
			this.errorstream.Close();
			if (this.inputstream != null)
			{
				this.inputstream.Close();
			}
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x000BBE20 File Offset: 0x000BA020
		internal void StopAsync()
		{
			PSConnectionRetryStatus psconnectionRetryStatus = this.connectionRetryStatus;
			if ((psconnectionRetryStatus == PSConnectionRetryStatus.NetworkFailureDetected || psconnectionRetryStatus == PSConnectionRetryStatus.ConnectionRetryAttempt) && this.runspacePool.RunspacePoolStateInfo.State == RunspacePoolState.Opened)
			{
				this.runspacePool.BeginDisconnect(null, null);
				return;
			}
			this.stopCalled = true;
			this.dataStructureHandler.SendStopPowerShellMessage();
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000BBE70 File Offset: 0x000BA070
		internal void SendInput()
		{
			this.dataStructureHandler.SendInput(this.inputstream);
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x0600206F RID: 8303 RVA: 0x000BBE84 File Offset: 0x000BA084
		// (remove) Token: 0x06002070 RID: 8304 RVA: 0x000BBEBC File Offset: 0x000BA0BC
		internal event EventHandler<RemoteDataEventArgs<RemoteHostCall>> HostCallReceived;

		// Token: 0x06002071 RID: 8305 RVA: 0x000BBEF4 File Offset: 0x000BA0F4
		internal void Initialize(ObjectStreamBase inputstream, ObjectStreamBase outputstream, ObjectStreamBase errorstream, PSInformationalBuffers informationalBuffers, PSInvocationSettings settings)
		{
			this.initialized = true;
			this.informationalBuffers = informationalBuffers;
			this.InputStream = inputstream;
			this.errorstream = errorstream;
			this.outputstream = outputstream;
			this.settings = settings;
			if (settings == null || settings.Host == null)
			{
				this.hostToUse = this.runspacePool.Host;
			}
			else
			{
				this.hostToUse = settings.Host;
			}
			this.dataStructureHandler = this.runspacePool.DataStructureHandler.CreatePowerShellDataStructureHandler(this);
			this.dataStructureHandler.InvocationStateInfoReceived += this.HandleInvocationStateInfoReceived;
			this.dataStructureHandler.OutputReceived += this.HandleOutputReceived;
			this.dataStructureHandler.ErrorReceived += this.HandleErrorReceived;
			this.dataStructureHandler.InformationalMessageReceived += this.HandleInformationalMessageReceived;
			this.dataStructureHandler.HostCallReceived += this.HandleHostCallReceived;
			this.dataStructureHandler.ClosedNotificationFromRunspacePool += this.HandleCloseNotificationFromRunspacePool;
			this.dataStructureHandler.BrokenNotificationFromRunspacePool += this.HandleBrokenNotificationFromRunspacePool;
			this.dataStructureHandler.ConnectCompleted += this.HandleConnectCompleted;
			this.dataStructureHandler.ReconnectCompleted += this.HandleConnectCompleted;
			this.dataStructureHandler.RobustConnectionNotification += this.HandleRobustConnectionNotification;
			this.dataStructureHandler.CloseCompleted += this.HandleCloseCompleted;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000BC06E File Offset: 0x000BA26E
		internal void Clear()
		{
			this.initialized = false;
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x000BC077 File Offset: 0x000BA277
		internal bool Initialized
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000BC080 File Offset: 0x000BA280
		internal static void ExitHandler(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			RemoteHostCall data = eventArgs.Data;
			if (data.IsSetShouldExitOrPopRunspace)
			{
				return;
			}
			ClientRemotePowerShell clientRemotePowerShell = (ClientRemotePowerShell)sender;
			clientRemotePowerShell.ExecuteHostCall(data);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000BC0AB File Offset: 0x000BA2AB
		internal void ConnectAsync(ConnectCommandInfo connectCmdInfo)
		{
			if (connectCmdInfo == null)
			{
				this.dataStructureHandler.ReconnectAsync();
				return;
			}
			this.shell.RunspacePool.RemoteRunspacePoolInternal.AddRemotePowerShellDSHandler(this.InstanceId, this.dataStructureHandler);
			this.dataStructureHandler.ConnectAsync();
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06002076 RID: 8310 RVA: 0x000BC0E8 File Offset: 0x000BA2E8
		// (remove) Token: 0x06002077 RID: 8311 RVA: 0x000BC120 File Offset: 0x000BA320
		internal event EventHandler<PSConnectionRetryStatusEventArgs> RCConnectionNotification;

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000BC155 File Offset: 0x000BA355
		internal PSConnectionRetryStatus ConnectionRetryStatus
		{
			get
			{
				return this.connectionRetryStatus;
			}
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000BC160 File Offset: 0x000BA360
		private void HandleErrorReceived(object sender, RemoteDataEventArgs<ErrorRecord> eventArgs)
		{
			using (ClientRemotePowerShell.tracer.TraceEventHandlers())
			{
				this.shell.SetHadErrors(true);
				this.errorstream.Write(eventArgs.Data);
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000BC1B4 File Offset: 0x000BA3B4
		private void HandleOutputReceived(object sender, RemoteDataEventArgs<object> eventArgs)
		{
			using (ClientRemotePowerShell.tracer.TraceEventHandlers())
			{
				object data = eventArgs.Data;
				try
				{
					this.outputstream.Write(data);
				}
				catch (PSInvalidCastException reason)
				{
					this.shell.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Failed, reason));
				}
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000BC220 File Offset: 0x000BA420
		private void HandleInvocationStateInfoReceived(object sender, RemoteDataEventArgs<PSInvocationStateInfo> eventArgs)
		{
			using (ClientRemotePowerShell.tracer.TraceEventHandlers())
			{
				PSInvocationStateInfo data = eventArgs.Data;
				if (data.State == PSInvocationState.Disconnected)
				{
					this.SetStateInfo(data);
				}
				else if (data.State == PSInvocationState.Stopped || data.State == PSInvocationState.Failed || data.State == PSInvocationState.Completed)
				{
					this.UnblockCollections();
					if (this.stopCalled)
					{
						this.stopCalled = false;
						this.stateInfoQueue.Enqueue(new PSInvocationStateInfo(PSInvocationState.Stopped, data.Reason));
						this.CheckAndCloseRunspaceAfterStop(data.Reason);
					}
					else
					{
						this.stateInfoQueue.Enqueue(data);
					}
					this.dataStructureHandler.CloseConnectionAsync(null);
				}
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000BC2DC File Offset: 0x000BA4DC
		private void CheckAndCloseRunspaceAfterStop(Exception ex)
		{
			PSRemotingTransportException ex2 = ex as PSRemotingTransportException;
			if (ex2 != null && (ex2.ErrorCode == -2144108526 || ex2.ErrorCode == -2144108250))
			{
				object runspaceConnection = this.shell.GetRunspaceConnection();
				if (runspaceConnection is Runspace)
				{
					Runspace runspace = (Runspace)runspaceConnection;
					if (runspace.RunspaceStateInfo.State != RunspaceState.Opened)
					{
						return;
					}
					try
					{
						runspace.Close();
						return;
					}
					catch (PSRemotingTransportException)
					{
						return;
					}
				}
				if (runspaceConnection is RunspacePool)
				{
					RunspacePool runspacePool = (RunspacePool)runspaceConnection;
					if (runspacePool.RunspacePoolStateInfo.State == RunspacePoolState.Opened)
					{
						try
						{
							runspacePool.Close();
						}
						catch (PSRemotingTransportException)
						{
						}
					}
				}
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000BC388 File Offset: 0x000BA588
		private void HandleInformationalMessageReceived(object sender, RemoteDataEventArgs<InformationalMessage> eventArgs)
		{
			using (ClientRemotePowerShell.tracer.TraceEventHandlers())
			{
				InformationalMessage data = eventArgs.Data;
				RemotingDataType dataType = data.DataType;
				switch (dataType)
				{
				case RemotingDataType.PowerShellDebug:
					this.informationalBuffers.AddDebug((DebugRecord)data.Message);
					break;
				case RemotingDataType.PowerShellVerbose:
					this.informationalBuffers.AddVerbose((VerboseRecord)data.Message);
					break;
				case RemotingDataType.PowerShellWarning:
					this.informationalBuffers.AddWarning((WarningRecord)data.Message);
					break;
				default:
					switch (dataType)
					{
					case RemotingDataType.PowerShellProgress:
					{
						ProgressRecord item = (ProgressRecord)LanguagePrimitives.ConvertTo(data.Message, typeof(ProgressRecord), CultureInfo.InvariantCulture);
						this.informationalBuffers.AddProgress(item);
						break;
					}
					case RemotingDataType.PowerShellInformationStream:
						this.informationalBuffers.AddInformation((InformationRecord)data.Message);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000BC488 File Offset: 0x000BA688
		private void HandleHostCallReceived(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			using (ClientRemotePowerShell.tracer.TraceEventHandlers())
			{
				Collection<RemoteHostCall> collection = eventArgs.Data.PerformSecurityChecksOnHostMessage(this.computerName);
				if (this.HostCallReceived != null)
				{
					if (collection.Count > 0)
					{
						foreach (RemoteHostCall data in collection)
						{
							RemoteDataEventArgs<RemoteHostCall> eventArgs2 = new RemoteDataEventArgs<RemoteHostCall>(data);
							this.HostCallReceived.SafeInvoke(this, eventArgs2);
						}
					}
					this.HostCallReceived.SafeInvoke(this, eventArgs);
				}
				else
				{
					if (collection.Count > 0)
					{
						foreach (RemoteHostCall hostcall in collection)
						{
							this.ExecuteHostCall(hostcall);
						}
					}
					this.ExecuteHostCall(eventArgs.Data);
				}
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000BC58C File Offset: 0x000BA78C
		private void HandleConnectCompleted(object sender, RemoteDataEventArgs<Exception> e)
		{
			this.SetStateInfo(new PSInvocationStateInfo(PSInvocationState.Running, null));
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000BC59C File Offset: 0x000BA79C
		private void HandleCloseCompleted(object sender, EventArgs args)
		{
			this.UnblockCollections();
			this.dataStructureHandler.RaiseRemoveAssociationEvent();
			if (this.stateInfoQueue.Count == 0)
			{
				if (!this.IsFinished(this.shell.InvocationStateInfo.State))
				{
					RemoteSessionStateEventArgs remoteSessionStateEventArgs = args as RemoteSessionStateEventArgs;
					Exception reason = (remoteSessionStateEventArgs != null) ? remoteSessionStateEventArgs.SessionStateInfo.Reason : null;
					PSInvocationState state = (this.shell.InvocationStateInfo.State == PSInvocationState.Disconnected) ? PSInvocationState.Failed : PSInvocationState.Stopped;
					this.SetStateInfo(new PSInvocationStateInfo(state, reason));
					return;
				}
			}
			else
			{
				while (this.stateInfoQueue.Count > 0)
				{
					PSInvocationStateInfo stateInfo = this.stateInfoQueue.Dequeue();
					this.SetStateInfo(stateInfo);
				}
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000BC63F File Offset: 0x000BA83F
		private bool IsFinished(PSInvocationState state)
		{
			return state == PSInvocationState.Completed || state == PSInvocationState.Failed || state == PSInvocationState.Stopped;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000BC650 File Offset: 0x000BA850
		private void ExecuteHostCall(RemoteHostCall hostcall)
		{
			if (hostcall.IsVoidMethod)
			{
				if (hostcall.IsSetShouldExitOrPopRunspace)
				{
					this.shell.ClearRemotePowerShell();
				}
				hostcall.ExecuteVoidMethod(this.hostToUse);
				return;
			}
			RemoteHostResponse hostResponse = hostcall.ExecuteNonVoidMethod(this.hostToUse);
			this.dataStructureHandler.SendHostResponseToServer(hostResponse);
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000BC69E File Offset: 0x000BA89E
		private void HandleCloseNotificationFromRunspacePool(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			this.UnblockCollections();
			this.dataStructureHandler.RaiseRemoveAssociationEvent();
			this.SetStateInfo(new PSInvocationStateInfo(PSInvocationState.Stopped, eventArgs.Data));
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000BC6C4 File Offset: 0x000BA8C4
		private void HandleBrokenNotificationFromRunspacePool(object sender, RemoteDataEventArgs<Exception> eventArgs)
		{
			this.UnblockCollections();
			this.dataStructureHandler.RaiseRemoveAssociationEvent();
			if (this.stopCalled)
			{
				this.stopCalled = false;
				this.SetStateInfo(new PSInvocationStateInfo(PSInvocationState.Stopped, eventArgs.Data));
				return;
			}
			this.SetStateInfo(new PSInvocationStateInfo(PSInvocationState.Failed, eventArgs.Data));
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000BC718 File Offset: 0x000BA918
		private void HandleRobustConnectionNotification(object sender, ConnectionStatusEventArgs e)
		{
			PSConnectionRetryStatusEventArgs psconnectionRetryStatusEventArgs = null;
			WarningRecord warningRecord = null;
			ErrorRecord errorRecord = null;
			int maxRetryConnectionTime = this.runspacePool.MaxRetryConnectionTime;
			int num = maxRetryConnectionTime / 60000;
			switch (e.Notification)
			{
			case ConnectionStatus.NetworkFailureDetected:
				warningRecord = new WarningRecord("PowerShellNetworkFailureDetected", StringUtil.Format(RemotingErrorIdStrings.RCNetworkFailureDetected, this.computerName, num));
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.NetworkFailureDetected, this.computerName, maxRetryConnectionTime, warningRecord);
				break;
			case ConnectionStatus.ConnectionRetryAttempt:
				warningRecord = new WarningRecord("PowerShellConnectionRetryAttempt", StringUtil.Format(RemotingErrorIdStrings.RCConnectionRetryAttempt, this.computerName));
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.ConnectionRetryAttempt, this.computerName, maxRetryConnectionTime, warningRecord);
				break;
			case ConnectionStatus.ConnectionRetrySucceeded:
				warningRecord = new WarningRecord("PowerShellConnectionRetrySucceeded", StringUtil.Format(RemotingErrorIdStrings.RCReconnectSucceeded, this.computerName));
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.ConnectionRetrySucceeded, this.computerName, num, warningRecord);
				break;
			case ConnectionStatus.AutoDisconnectStarting:
				warningRecord = new WarningRecord("PowerShellNetworkFailedStartDisconnect", StringUtil.Format(RemotingErrorIdStrings.RCAutoDisconnectingWarning, this.computerName));
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.AutoDisconnectStarting, this.computerName, num, warningRecord);
				break;
			case ConnectionStatus.AutoDisconnectSucceeded:
				warningRecord = new WarningRecord("PowerShellAutoDisconnectSucceeded", StringUtil.Format(RemotingErrorIdStrings.RCAutoDisconnected, this.computerName));
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.AutoDisconnectSucceeded, this.computerName, num, warningRecord);
				break;
			case ConnectionStatus.InternalErrorAbort:
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.RCInternalError, this.computerName);
				RuntimeException exception = new RuntimeException(message);
				errorRecord = new ErrorRecord(exception, "PowerShellNetworkOrDisconnectFailed", ErrorCategory.InvalidOperation, this);
				psconnectionRetryStatusEventArgs = new PSConnectionRetryStatusEventArgs(PSConnectionRetryStatus.InternalErrorAbort, this.computerName, num, errorRecord);
				break;
			}
			}
			if (psconnectionRetryStatusEventArgs == null)
			{
				return;
			}
			this.connectionRetryStatus = psconnectionRetryStatusEventArgs.Notification;
			if (warningRecord != null)
			{
				RemotingWarningRecord message2 = new RemotingWarningRecord(warningRecord, new OriginInfo(this.computerName, this.InstanceId));
				this.HandleInformationalMessageReceived(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(message2, RemotingDataType.PowerShellWarning)));
				RemoteHostCall data = new RemoteHostCall(-100L, RemoteHostMethodId.WriteWarningLine, new object[]
				{
					warningRecord.Message
				});
				try
				{
					this.HandleHostCallReceived(this, new RemoteDataEventArgs<RemoteHostCall>(data));
				}
				catch (PSNotImplementedException)
				{
				}
			}
			if (errorRecord != null)
			{
				RemotingErrorRecord data2 = new RemotingErrorRecord(errorRecord, new OriginInfo(this.computerName, this.InstanceId));
				this.HandleErrorReceived(this, new RemoteDataEventArgs<ErrorRecord>(data2));
			}
			this.RCConnectionNotification.SafeInvoke(this, psconnectionRetryStatusEventArgs);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000BC958 File Offset: 0x000BAB58
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000BC967 File Offset: 0x000BAB67
		protected void Dispose(bool disposing)
		{
		}

		// Token: 0x04000E3F RID: 3647
		protected const string WRITE_DEBUG_LINE = "WriteDebugLine";

		// Token: 0x04000E40 RID: 3648
		protected const string WRITE_VERBOSE_LINE = "WriteVerboseLine";

		// Token: 0x04000E41 RID: 3649
		protected const string WRITE_WARNING_LINE = "WriteWarningLine";

		// Token: 0x04000E42 RID: 3650
		protected const string WRITE_PROGRESS = "WriteProgress";

		// Token: 0x04000E43 RID: 3651
		[TraceSource("CRPS", "ClientRemotePowerShell")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("CRPS", "ClientRemotePowerShellBase");

		// Token: 0x04000E46 RID: 3654
		protected ObjectStreamBase inputstream;

		// Token: 0x04000E47 RID: 3655
		protected ObjectStreamBase errorstream;

		// Token: 0x04000E48 RID: 3656
		protected PSInformationalBuffers informationalBuffers;

		// Token: 0x04000E49 RID: 3657
		protected PowerShell shell;

		// Token: 0x04000E4A RID: 3658
		protected Guid clientRunspacePoolId;

		// Token: 0x04000E4B RID: 3659
		protected bool noInput;

		// Token: 0x04000E4C RID: 3660
		protected PSInvocationSettings settings;

		// Token: 0x04000E4D RID: 3661
		protected ObjectStreamBase outputstream;

		// Token: 0x04000E4E RID: 3662
		protected string computerName;

		// Token: 0x04000E4F RID: 3663
		protected ClientPowerShellDataStructureHandler dataStructureHandler;

		// Token: 0x04000E50 RID: 3664
		protected bool stopCalled;

		// Token: 0x04000E51 RID: 3665
		protected PSHost hostToUse;

		// Token: 0x04000E52 RID: 3666
		protected RemoteRunspacePoolInternal runspacePool;

		// Token: 0x04000E53 RID: 3667
		protected bool initialized;

		// Token: 0x04000E54 RID: 3668
		private Queue<PSInvocationStateInfo> stateInfoQueue = new Queue<PSInvocationStateInfo>();

		// Token: 0x04000E55 RID: 3669
		private PSConnectionRetryStatus connectionRetryStatus;
	}
}
