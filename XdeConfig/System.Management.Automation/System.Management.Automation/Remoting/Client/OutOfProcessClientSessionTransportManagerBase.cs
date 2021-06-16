using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x0200037D RID: 893
	internal abstract class OutOfProcessClientSessionTransportManagerBase : BaseClientSessionTransportManager
	{
		// Token: 0x06002B91 RID: 11153 RVA: 0x000F0C94 File Offset: 0x000EEE94
		internal OutOfProcessClientSessionTransportManagerBase(Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(runspaceId, cryptoHelper)
		{
			this.onDataAvailableToSendCallback = new PrioritySendDataCollection.OnDataAvailableCallback(this.OnDataAvailableCallback);
			this.cmdTransportManagers = new Dictionary<Guid, OutOfProcessClientCommandTransportManager>();
			this.dataProcessingCallbacks = default(OutOfProcessUtils.DataProcessingDelegates);
			this.dataProcessingCallbacks.DataPacketReceived = (OutOfProcessUtils.DataPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.DataPacketReceived, new OutOfProcessUtils.DataPacketReceived(this.OnDataPacketReceived));
			this.dataProcessingCallbacks.DataAckPacketReceived = (OutOfProcessUtils.DataAckPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.DataAckPacketReceived, new OutOfProcessUtils.DataAckPacketReceived(this.OnDataAckPacketReceived));
			this.dataProcessingCallbacks.CommandCreationPacketReceived = (OutOfProcessUtils.CommandCreationPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.CommandCreationPacketReceived, new OutOfProcessUtils.CommandCreationPacketReceived(this.OnCommandCreationPacketReceived));
			this.dataProcessingCallbacks.CommandCreationAckReceived = (OutOfProcessUtils.CommandCreationAckReceived)Delegate.Combine(this.dataProcessingCallbacks.CommandCreationAckReceived, new OutOfProcessUtils.CommandCreationAckReceived(this.OnCommandCreationAckReceived));
			this.dataProcessingCallbacks.SignalPacketReceived = (OutOfProcessUtils.SignalPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.SignalPacketReceived, new OutOfProcessUtils.SignalPacketReceived(this.OnSignalPacketReceived));
			this.dataProcessingCallbacks.SignalAckPacketReceived = (OutOfProcessUtils.SignalAckPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.SignalAckPacketReceived, new OutOfProcessUtils.SignalAckPacketReceived(this.OnSiganlAckPacketReceived));
			this.dataProcessingCallbacks.ClosePacketReceived = (OutOfProcessUtils.ClosePacketReceived)Delegate.Combine(this.dataProcessingCallbacks.ClosePacketReceived, new OutOfProcessUtils.ClosePacketReceived(this.OnClosePacketReceived));
			this.dataProcessingCallbacks.CloseAckPacketReceived = (OutOfProcessUtils.CloseAckPacketReceived)Delegate.Combine(this.dataProcessingCallbacks.CloseAckPacketReceived, new OutOfProcessUtils.CloseAckPacketReceived(this.OnCloseAckReceived));
			this.dataToBeSent.Fragmentor = base.Fragmentor;
			base.ReceivedDataCollection.MaximumReceivedDataSize = null;
			base.ReceivedDataCollection.MaximumReceivedObjectSize = new int?(10485760);
			this.closeTimeOutTimer = new Timer(new TimerCallback(this.OnCloseTimeOutTimerElapsed), null, -1, -1);
			this._tracer = PowerShellTraceSourceFactory.GetTraceSource();
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000F0E69 File Offset: 0x000EF069
		internal override void ConnectAsync()
		{
			throw new NotImplementedException(RemotingErrorIdStrings.IPCTransportConnectError);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000F0E78 File Offset: 0x000EF078
		internal override void CloseAsync()
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				this.isClosed = true;
				if (this.stdInWriter == null)
				{
					flag = true;
				}
			}
			base.CloseAsync();
			if (!flag)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseShell, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString()
				});
				this._tracer.WriteMessage(string.Concat(new object[]
				{
					"OutOfProcessClientSessionTransportManager.CloseAsync, when sending close session packet, progress command count should be zero, current cmd count: ",
					this.cmdTransportManagers.Count,
					", RunSpacePool Id : ",
					base.RunspacePoolInstanceId
				}));
				try
				{
					this.stdInWriter.WriteLine(OutOfProcessUtils.CreateClosePacket(Guid.Empty));
					this.closeTimeOutTimer.Change(60000, -1);
				}
				catch (IOException)
				{
					flag = true;
				}
				if (flag)
				{
					base.RaiseCloseCompleted();
				}
				return;
			}
			base.RaiseCloseCompleted();
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000F0FA8 File Offset: 0x000EF1A8
		internal override BaseClientCommandTransportManager CreateClientCommandTransportManager(RunspaceConnectionInfo connectionInfo, ClientRemotePowerShell cmd, bool noInput)
		{
			OutOfProcessClientCommandTransportManager outOfProcessClientCommandTransportManager = new OutOfProcessClientCommandTransportManager(cmd, noInput, this, this.stdInWriter);
			this.AddCommandTransportManager(cmd.InstanceId, outOfProcessClientCommandTransportManager);
			return outOfProcessClientCommandTransportManager;
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000F0FD2 File Offset: 0x000EF1D2
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				this.cmdTransportManagers.Clear();
				this.closeTimeOutTimer.Dispose();
			}
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000F0FF4 File Offset: 0x000EF1F4
		private void AddCommandTransportManager(Guid key, OutOfProcessClientCommandTransportManager cmdTM)
		{
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					this._tracer.WriteMessage("OutOfProcessClientSessionTransportManager.AddCommandTransportManager, Adding command transport on closed session, RunSpacePool Id : " + base.RunspacePoolInstanceId);
				}
				else
				{
					this.cmdTransportManagers.Add(key, cmdTM);
				}
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000F1068 File Offset: 0x000EF268
		internal override void RemoveCommandTransportManager(Guid key)
		{
			lock (this.syncObject)
			{
				if (this.cmdTransportManagers.ContainsKey(key))
				{
					this.cmdTransportManagers.Remove(key);
				}
				else
				{
					this._tracer.WriteMessage("key does not exist to remove from cmdTransportManagers");
				}
			}
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000F10D0 File Offset: 0x000EF2D0
		private OutOfProcessClientCommandTransportManager GetCommandTransportManager(Guid key)
		{
			OutOfProcessClientCommandTransportManager result;
			lock (this.syncObject)
			{
				OutOfProcessClientCommandTransportManager outOfProcessClientCommandTransportManager = null;
				this.cmdTransportManagers.TryGetValue(key, out outOfProcessClientCommandTransportManager);
				result = outOfProcessClientCommandTransportManager;
			}
			return result;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000F1120 File Offset: 0x000EF320
		private void OnCloseSessionCompleted()
		{
			this.closeTimeOutTimer.Change(-1, -1);
			base.RaiseCloseCompleted();
			this.CleanupConnection();
		}

		// Token: 0x06002B9A RID: 11162
		protected abstract void CleanupConnection();

		// Token: 0x06002B9B RID: 11163 RVA: 0x000F113C File Offset: 0x000EF33C
		protected void HandleOutputDataReceived(string data)
		{
			try
			{
				OutOfProcessUtils.ProcessData(data, this.dataProcessingCallbacks);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCErrorProcessingServerData, RemotingErrorIdStrings.IPCErrorProcessingServerData, new object[]
				{
					ex.Message
				});
				this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx));
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000F11A0 File Offset: 0x000EF3A0
		protected void HandleErrorDataReceived(string data)
		{
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
			}
			PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCServerProcessReportedError, RemotingErrorIdStrings.IPCServerProcessReportedError, new object[]
			{
				data
			});
			this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e, TransportMethodEnum.Unknown));
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000F1210 File Offset: 0x000EF410
		protected void OnExited(object sender, EventArgs e)
		{
			TransportMethodEnum m = TransportMethodEnum.Unknown;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					m = TransportMethodEnum.CloseShellOperationEx;
				}
				this.stdInWriter.StopWriting();
			}
			PSRemotingTransportException e2 = new PSRemotingTransportException(PSRemotingErrorId.IPCServerProcessExited, RemotingErrorIdStrings.IPCServerProcessExited, new object[0]);
			this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e2, m));
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000F1288 File Offset: 0x000EF488
		protected void SendOneItem()
		{
			DataPriorityType priorityType;
			byte[] array = this.dataToBeSent.ReadOrRegisterCallback(this.onDataAvailableToSendCallback, out priorityType);
			if (array != null)
			{
				this.SendData(array, priorityType);
			}
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000F12B4 File Offset: 0x000EF4B4
		private void OnDataAvailableCallback(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Received data to be sent from the callback.", new object[0]);
			this.SendData(data, priorityType);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000F12D4 File Offset: 0x000EF4D4
		private void SendData(byte[] data, DataPriorityType priorityType)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputEx, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				Guid.Empty.ToString(),
				data.Length.ToString(CultureInfo.InvariantCulture)
			});
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					this.stdInWriter.WriteLine(OutOfProcessUtils.CreateDataPacket(data, priorityType, Guid.Empty));
				}
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000F1390 File Offset: 0x000EF590
		private void OnRemoteSessionSendCompleted()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputExCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				Guid.Empty.ToString()
			});
			this.SendOneItem();
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000F13F0 File Offset: 0x000EF5F0
		private void OnDataPacketReceived(byte[] rawData, string stream, Guid psGuid)
		{
			string stream2 = "stdout";
			if (stream.Equals(DataPriorityType.PromptResponse.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				stream2 = "pr";
			}
			if (psGuid == Guid.Empty)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputExCallbackReceived, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString(),
					Guid.Empty.ToString(),
					rawData.Length.ToString(CultureInfo.InvariantCulture)
				});
				base.ProcessRawData(rawData, stream2);
				return;
			}
			OutOfProcessClientCommandTransportManager commandTransportManager = this.GetCommandTransportManager(psGuid);
			if (commandTransportManager != null)
			{
				commandTransportManager.OnRemoteCmdDataReceived(rawData, stream2);
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000F14A8 File Offset: 0x000EF6A8
		private void OnDataAckPacketReceived(Guid psGuid)
		{
			if (psGuid == Guid.Empty)
			{
				this.OnRemoteSessionSendCompleted();
				return;
			}
			OutOfProcessClientCommandTransportManager commandTransportManager = this.GetCommandTransportManager(psGuid);
			if (commandTransportManager != null)
			{
				commandTransportManager.OnRemoteCmdSendCompleted();
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000F14DC File Offset: 0x000EF6DC
		private void OnCommandCreationPacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"Command"
			});
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000F1508 File Offset: 0x000EF708
		private void OnCommandCreationAckReceived(Guid psGuid)
		{
			OutOfProcessClientCommandTransportManager commandTransportManager = this.GetCommandTransportManager(psGuid);
			if (commandTransportManager == null)
			{
				throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownCommandGuid, RemotingErrorIdStrings.IPCUnknownCommandGuid, new object[]
				{
					psGuid.ToString(),
					"CommandAck"
				});
			}
			commandTransportManager.OnCreateCmdCompleted();
			this._tracer.WriteMessage(string.Concat(new object[]
			{
				"OutOfProcessClientSessionTransportManager.OnCommandCreationAckReceived, in progress command count after cmd creation ACK : ",
				this.cmdTransportManagers.Count,
				", psGuid : ",
				psGuid.ToString()
			}));
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000F15A4 File Offset: 0x000EF7A4
		private void OnSignalPacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"Signal"
			});
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000F15D0 File Offset: 0x000EF7D0
		private void OnSiganlAckPacketReceived(Guid psGuid)
		{
			if (psGuid == Guid.Empty)
			{
				throw new PSRemotingTransportException(PSRemotingErrorId.IPCNoSignalForSession, RemotingErrorIdStrings.IPCNoSignalForSession, new object[]
				{
					"SignalAck"
				});
			}
			OutOfProcessClientCommandTransportManager commandTransportManager = this.GetCommandTransportManager(psGuid);
			if (commandTransportManager != null)
			{
				commandTransportManager.OnRemoteCmdSignalCompleted();
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000F161C File Offset: 0x000EF81C
		private void OnClosePacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"Close"
			});
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000F1648 File Offset: 0x000EF848
		private void OnCloseAckReceived(Guid psGuid)
		{
			int count;
			lock (this.syncObject)
			{
				count = this.cmdTransportManagers.Count;
			}
			if (psGuid == Guid.Empty)
			{
				this._tracer.WriteMessage(string.Concat(new object[]
				{
					"OutOfProcessClientSessionTransportManager.OnCloseAckReceived, progress command count after CLOSE ACK should be zero = ",
					count,
					" psGuid : ",
					psGuid.ToString()
				}));
				this.OnCloseSessionCompleted();
				return;
			}
			this._tracer.WriteMessage(string.Concat(new object[]
			{
				"OutOfProcessClientSessionTransportManager.OnCloseAckReceived, in progress command count should be greater than zero: ",
				count,
				", RunSpacePool Id : ",
				base.RunspacePoolInstanceId,
				", psGuid : ",
				psGuid.ToString()
			}));
			OutOfProcessClientCommandTransportManager commandTransportManager = this.GetCommandTransportManager(psGuid);
			if (commandTransportManager != null)
			{
				commandTransportManager.OnCloseCmdCompleted();
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000F175C File Offset: 0x000EF95C
		internal void OnCloseTimeOutTimerElapsed(object source)
		{
			PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCCloseTimedOut, RemotingErrorIdStrings.IPCCloseTimedOut, new object[0]);
			this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e, TransportMethodEnum.CloseShellOperationEx));
		}

		// Token: 0x040015E5 RID: 5605
		private PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableToSendCallback;

		// Token: 0x040015E6 RID: 5606
		private OutOfProcessUtils.DataProcessingDelegates dataProcessingCallbacks;

		// Token: 0x040015E7 RID: 5607
		private Dictionary<Guid, OutOfProcessClientCommandTransportManager> cmdTransportManagers;

		// Token: 0x040015E8 RID: 5608
		private Timer closeTimeOutTimer;

		// Token: 0x040015E9 RID: 5609
		protected OutOfProcessTextWriter stdInWriter;

		// Token: 0x040015EA RID: 5610
		protected PowerShellTraceSource _tracer;
	}
}
