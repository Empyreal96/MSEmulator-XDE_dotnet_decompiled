using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200032B RID: 811
	[OutputType(new Type[]
	{
		typeof(PSSession)
	})]
	[Cmdlet("Disconnect", "PSSession", SupportsShouldProcess = true, DefaultParameterSetName = "Session", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210605", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class DisconnectPSSessionCommand : PSRunspaceCmdlet, IDisposable
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x000DB6EF File Offset: 0x000D98EF
		// (set) Token: 0x06002725 RID: 10021 RVA: 0x000DB6F7 File Offset: 0x000D98F7
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Session")]
		public PSSession[] Session
		{
			get
			{
				return this.remotePSSessionInfo;
			}
			set
			{
				this.remotePSSessionInfo = value;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x000DB700 File Offset: 0x000D9900
		// (set) Token: 0x06002727 RID: 10023 RVA: 0x000DB720 File Offset: 0x000D9920
		[ValidateRange(0, 2147483647)]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "Id")]
		[Parameter(ParameterSetName = "InstanceId")]
		public int IdleTimeoutSec
		{
			get
			{
				return this.PSSessionOption.IdleTimeout.Seconds;
			}
			set
			{
				this.PSSessionOption.IdleTimeout = TimeSpan.FromSeconds((double)value);
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x000DB734 File Offset: 0x000D9934
		// (set) Token: 0x06002729 RID: 10025 RVA: 0x000DB741 File Offset: 0x000D9941
		[Parameter(ParameterSetName = "InstanceId")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "Id")]
		public OutputBufferingMode OutputBufferingMode
		{
			get
			{
				return this.PSSessionOption.OutputBufferingMode;
			}
			set
			{
				this.PSSessionOption.OutputBufferingMode = value;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000DB74F File Offset: 0x000D994F
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x000DB757 File Offset: 0x000D9957
		[Parameter(ParameterSetName = "InstanceId")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "Id")]
		public int ThrottleLimit
		{
			get
			{
				return this.throttleLimit;
			}
			set
			{
				this.throttleLimit = value;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000DB760 File Offset: 0x000D9960
		// (set) Token: 0x0600272D RID: 10029 RVA: 0x000DB768 File Offset: 0x000D9968
		public override string[] ComputerName { get; set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000DB771 File Offset: 0x000D9971
		private PSSessionOption PSSessionOption
		{
			get
			{
				if (this.sessionOption == null)
				{
					this.sessionOption = new PSSessionOption();
				}
				return this.sessionOption;
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000DB78C File Offset: 0x000D998C
		protected override void BeginProcessing()
		{
			base.BeginProcessing();
			this.throttleManager.ThrottleLimit = this.ThrottleLimit;
			this.throttleManager.ThrottleComplete += this.HandleThrottleDisconnectComplete;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000DB7BC File Offset: 0x000D99BC
		protected override void ProcessRecord()
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			try
			{
				Dictionary<Guid, PSSession> dictionary;
				if (base.ParameterSetName == "Session")
				{
					if (this.remotePSSessionInfo == null || this.remotePSSessionInfo.Length == 0)
					{
						return;
					}
					dictionary = new Dictionary<Guid, PSSession>();
					foreach (PSSession pssession in this.remotePSSessionInfo)
					{
						dictionary.Add(pssession.InstanceId, pssession);
					}
				}
				else
				{
					dictionary = base.GetMatchingRunspaces(false, true);
				}
				string localhostWithNetworkAccessEnabled = this.GetLocalhostWithNetworkAccessEnabled(dictionary);
				if (!string.IsNullOrEmpty(localhostWithNetworkAccessEnabled))
				{
					base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.EnableNetworkAccessWarning, localhostWithNetworkAccessEnabled));
				}
				foreach (PSSession pssession2 in dictionary.Values)
				{
					if (base.ShouldProcess(pssession2.Name, "Disconnect"))
					{
						if (pssession2.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
						{
							if (this.sessionOption != null)
							{
								pssession2.Runspace.ConnectionInfo.SetSessionOptions(this.sessionOption);
							}
							if (this.ValidateIdleTimeout(pssession2))
							{
								DisconnectPSSessionCommand.DisconnectRunspaceOperation item = new DisconnectPSSessionCommand.DisconnectRunspaceOperation(pssession2, this.stream);
								list.Add(item);
							}
						}
						else if (pssession2.Runspace.RunspaceStateInfo.State != RunspaceState.Disconnected)
						{
							string message = StringUtil.Format(RemotingErrorIdStrings.RunspaceCannotBeDisconnected, pssession2.Name);
							Exception exception = new RuntimeException(message);
							ErrorRecord errorRecord = new ErrorRecord(exception, "CannotDisconnectSessionWhenNotOpened", ErrorCategory.InvalidOperation, pssession2);
							base.WriteError(errorRecord);
						}
						else
						{
							base.WriteObject(pssession2);
						}
					}
				}
			}
			catch (PSRemotingDataStructureException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (PSRemotingTransportException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (RemoteException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (InvalidRunspaceStateException)
			{
				this.operationsComplete.Set();
				throw;
			}
			if (list.Count > 0)
			{
				this.operationsComplete.Reset();
				this.throttleManager.SubmitOperations(list);
				Collection<object> collection = this.stream.ObjectReader.NonBlockingRead();
				foreach (object obj in collection)
				{
					base.WriteStreamObject((Action<Cmdlet>)obj);
				}
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000DBA8C File Offset: 0x000D9C8C
		protected override void EndProcessing()
		{
			this.throttleManager.EndSubmitOperations();
			this.operationsComplete.WaitOne();
			while (!this.stream.ObjectReader.EndOfPipeline)
			{
				object obj = this.stream.ObjectReader.Read();
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000DBAE1 File Offset: 0x000D9CE1
		protected override void StopProcessing()
		{
			this.stream.ObjectWriter.Close();
			this.throttleManager.StopAllOperations();
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000DBAFE File Offset: 0x000D9CFE
		private void HandleThrottleDisconnectComplete(object sender, EventArgs eventArgs)
		{
			this.stream.ObjectWriter.Close();
			this.operationsComplete.Set();
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000DBB1C File Offset: 0x000D9D1C
		private bool ValidateIdleTimeout(PSSession session)
		{
			int idleTimeout = session.Runspace.ConnectionInfo.IdleTimeout;
			int maxIdleTimeout = session.Runspace.ConnectionInfo.MaxIdleTimeout;
			int num = 60000;
			if (idleTimeout != -1 && (idleTimeout > maxIdleTimeout || idleTimeout < num))
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.CannotDisconnectSessionWithInvalidIdleTimeout, new object[]
				{
					session.Name,
					idleTimeout / 1000,
					maxIdleTimeout / 1000,
					num / 1000
				});
				ErrorRecord errorRecord = new ErrorRecord(new RuntimeException(message), "CannotDisconnectSessionWithInvalidIdleTimeout", ErrorCategory.InvalidArgument, session);
				base.WriteError(errorRecord);
				return false;
			}
			return true;
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000DBBCC File Offset: 0x000D9DCC
		private string GetLocalhostWithNetworkAccessEnabled(Dictionary<Guid, PSSession> psSessions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PSSession pssession in psSessions.Values)
			{
				WSManConnectionInfo wsmanConnectionInfo = pssession.Runspace.ConnectionInfo as WSManConnectionInfo;
				if (wsmanConnectionInfo.IsLocalhostAndNetworkAccess)
				{
					stringBuilder.Append(pssession.Name + ", ");
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000DBC70 File Offset: 0x000D9E70
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000DBC80 File Offset: 0x000D9E80
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.throttleManager.Dispose();
				this.operationsComplete.WaitOne();
				this.operationsComplete.Dispose();
				this.throttleManager.ThrottleComplete -= this.HandleThrottleDisconnectComplete;
				this.stream.Dispose();
			}
		}

		// Token: 0x04001357 RID: 4951
		private PSSession[] remotePSSessionInfo;

		// Token: 0x04001358 RID: 4952
		private int throttleLimit;

		// Token: 0x04001359 RID: 4953
		private PSSessionOption sessionOption;

		// Token: 0x0400135A RID: 4954
		private ThrottleManager throttleManager = new ThrottleManager();

		// Token: 0x0400135B RID: 4955
		private ManualResetEvent operationsComplete = new ManualResetEvent(true);

		// Token: 0x0400135C RID: 4956
		private ObjectStream stream = new ObjectStream();

		// Token: 0x0200032C RID: 812
		private class DisconnectRunspaceOperation : IThrottleOperation
		{
			// Token: 0x06002739 RID: 10041 RVA: 0x000DBCFE File Offset: 0x000D9EFE
			internal DisconnectRunspaceOperation(PSSession session, ObjectStream stream)
			{
				this.remoteSession = session;
				this.writeStream = stream;
				this.remoteSession.Runspace.StateChanged += this.StateCallBackHandler;
			}

			// Token: 0x0600273A RID: 10042 RVA: 0x000DBD30 File Offset: 0x000D9F30
			internal override void StartOperation()
			{
				bool flag = true;
				try
				{
					this.remoteSession.Runspace.DisconnectAsync();
				}
				catch (InvalidRunspacePoolStateException e)
				{
					flag = false;
					this.WriteDisconnectFailed(e);
				}
				catch (PSInvalidOperationException e2)
				{
					flag = false;
					this.WriteDisconnectFailed(e2);
				}
				if (!flag)
				{
					this.remoteSession.Runspace.StateChanged -= this.StateCallBackHandler;
					this.SendStartComplete();
				}
			}

			// Token: 0x0600273B RID: 10043 RVA: 0x000DBDAC File Offset: 0x000D9FAC
			internal override void StopOperation()
			{
				this.remoteSession.Runspace.StateChanged -= this.StateCallBackHandler;
				this.SendStopComplete();
			}

			// Token: 0x14000085 RID: 133
			// (add) Token: 0x0600273C RID: 10044 RVA: 0x000DBDD0 File Offset: 0x000D9FD0
			// (remove) Token: 0x0600273D RID: 10045 RVA: 0x000DBE08 File Offset: 0x000DA008
			internal override event EventHandler<OperationStateEventArgs> OperationComplete;

			// Token: 0x0600273E RID: 10046 RVA: 0x000DBE40 File Offset: 0x000DA040
			private void StateCallBackHandler(object sender, RunspaceStateEventArgs eArgs)
			{
				if (eArgs.RunspaceStateInfo.State == RunspaceState.Disconnecting)
				{
					return;
				}
				if (eArgs.RunspaceStateInfo.State == RunspaceState.Disconnected)
				{
					this.WriteDisconnectedPSSession();
				}
				else
				{
					this.WriteDisconnectFailed(null);
				}
				this.remoteSession.Runspace.StateChanged -= this.StateCallBackHandler;
				this.SendStartComplete();
			}

			// Token: 0x0600273F RID: 10047 RVA: 0x000DBE9C File Offset: 0x000DA09C
			private void SendStartComplete()
			{
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StartComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x06002740 RID: 10048 RVA: 0x000DBEC4 File Offset: 0x000DA0C4
			private void SendStopComplete()
			{
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StopComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x06002741 RID: 10049 RVA: 0x000DBEFC File Offset: 0x000DA0FC
			private void WriteDisconnectedPSSession()
			{
				if (this.writeStream.ObjectWriter.IsOpen)
				{
					Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteObject(this.remoteSession);
					};
					this.writeStream.ObjectWriter.Write(obj);
				}
			}

			// Token: 0x06002742 RID: 10050 RVA: 0x000DBF58 File Offset: 0x000DA158
			private void WriteDisconnectFailed(Exception e = null)
			{
				if (this.writeStream.ObjectWriter.IsOpen)
				{
					string message;
					if (e != null && !string.IsNullOrWhiteSpace(e.Message))
					{
						message = StringUtil.Format(RemotingErrorIdStrings.RunspaceDisconnectFailedWithReason, this.remoteSession.InstanceId, e.Message);
					}
					else
					{
						message = StringUtil.Format(RemotingErrorIdStrings.RunspaceDisconnectFailed, this.remoteSession.InstanceId);
					}
					Exception exception = new RuntimeException(message, e);
					ErrorRecord errorRecord = new ErrorRecord(exception, "PSSessionDisconnectFailed", ErrorCategory.InvalidOperation, this.remoteSession);
					Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteError(errorRecord);
					};
					this.writeStream.ObjectWriter.Write(obj);
				}
			}

			// Token: 0x0400135E RID: 4958
			private PSSession remoteSession;

			// Token: 0x0400135F RID: 4959
			private ObjectStream writeStream;
		}
	}
}
