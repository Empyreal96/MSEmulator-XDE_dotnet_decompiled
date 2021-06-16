using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000385 RID: 901
	internal class OutOfProcessClientCommandTransportManager : BaseClientCommandTransportManager
	{
		// Token: 0x06002BC6 RID: 11206 RVA: 0x000F2288 File Offset: 0x000F0488
		internal OutOfProcessClientCommandTransportManager(ClientRemotePowerShell cmd, bool noInput, OutOfProcessClientSessionTransportManagerBase sessnTM, OutOfProcessTextWriter stdInWriter) : base(cmd, sessnTM.CryptoHelper, sessnTM)
		{
			this.stdInWriter = stdInWriter;
			this.onDataAvailableToSendCallback = new PrioritySendDataCollection.OnDataAvailableCallback(this.OnDataAvailableCallback);
			this.signalTimeOutTimer = new Timer(new TimerCallback(this.OnSignalTimeOutTimerElapsed), null, -1, -1);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000F22D7 File Offset: 0x000F04D7
		internal override void ConnectAsync()
		{
			throw new NotImplementedException(RemotingErrorIdStrings.IPCTransportConnectError);
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000F22E4 File Offset: 0x000F04E4
		internal override void CreateAsync()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateCommand, PSOpcode.Connect, PSTask.CreateRunspace, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			this.stdInWriter.WriteLine(OutOfProcessUtils.CreateCommandPacket(this.powershellInstanceId));
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000F2350 File Offset: 0x000F0550
		internal override void CloseAsync()
		{
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				this.isClosed = true;
			}
			base.CloseAsync();
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseCommand, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			if (this.stdInWriter != null)
			{
				try
				{
					this.stdInWriter.WriteLine(OutOfProcessUtils.CreateClosePacket(this.powershellInstanceId));
				}
				catch (IOException innerException)
				{
					this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(new PSRemotingTransportException(RemotingErrorIdStrings.NamedPipeTransportProcessEnded, innerException), TransportMethodEnum.CloseShellOperationEx));
				}
			}
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000F2434 File Offset: 0x000F0634
		internal override void SendStopSignal()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSignal, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString(),
				"stopsignal"
			});
			base.CloseAsync();
			this.stdInWriter.WriteLine(OutOfProcessUtils.CreateSignalPacket(this.powershellInstanceId));
			this.signalTimeOutTimer.Change(60000, -1);
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000F24C0 File Offset: 0x000F06C0
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				this.StopSignalTimerAndDecrementOperations();
				this.signalTimeOutTimer.Dispose();
			}
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000F24E0 File Offset: 0x000F06E0
		internal void OnCreateCmdCompleted()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateCommandCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				}
				else
				{
					this.SendOneItem();
				}
			}
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000F2588 File Offset: 0x000F0788
		internal void OnRemoteCmdSendCompleted()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputExCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Command TM: Transport manager is closed. So returning", new object[0]);
					return;
				}
			}
			this.SendOneItem();
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000F2630 File Offset: 0x000F0830
		internal void OnRemoteCmdDataReceived(byte[] rawData, string stream)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputExCallbackReceived, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString(),
				rawData.Length.ToString(CultureInfo.InvariantCulture)
			});
			if (this.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Command TM: Transport manager is closed. So returning", new object[0]);
				return;
			}
			this.ProcessRawData(rawData, stream);
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000F26C0 File Offset: 0x000F08C0
		internal void OnRemoteCmdSignalCompleted()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSignalCallbackReceived, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			this.StopSignalTimerAndDecrementOperations();
			if (this.isClosed)
			{
				return;
			}
			base.EnqueueAndStartProcessingThread(null, null, true);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000F2734 File Offset: 0x000F0934
		internal void OnSignalTimeOutTimerElapsed(object source)
		{
			if (this.isClosed)
			{
				return;
			}
			PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.IPCSignalTimedOut);
			this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx));
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000F2764 File Offset: 0x000F0964
		private void StopSignalTimerAndDecrementOperations()
		{
			lock (this.syncObject)
			{
				this.signalTimeOutTimer.Change(-1, -1);
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000F27AC File Offset: 0x000F09AC
		internal override void ProcessPrivateData(object privateData)
		{
			bool flag = (bool)privateData;
			if (flag)
			{
				base.RaiseSignalCompleted();
			}
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000F27CC File Offset: 0x000F09CC
		internal void OnCloseCmdCompleted()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseCommandCallbackReceived, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			base.RaiseCloseCompleted();
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000F2828 File Offset: 0x000F0A28
		private void SendOneItem()
		{
			DataPriorityType priorityType = DataPriorityType.Default;
			byte[] array;
			if (this.serializedPipeline.Length > 0L)
			{
				array = this.serializedPipeline.ReadOrRegisterCallback(null);
			}
			else
			{
				array = this.dataToBeSent.ReadOrRegisterCallback(this.onDataAvailableToSendCallback, out priorityType);
			}
			if (array != null)
			{
				this.SendData(array, priorityType);
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000F2878 File Offset: 0x000F0A78
		private void SendData(byte[] data, DataPriorityType priorityType)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputEx, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString(),
				data.Length.ToString(CultureInfo.InvariantCulture)
			});
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					this.stdInWriter.WriteLine(OutOfProcessUtils.CreateDataPacket(data, priorityType, this.powershellInstanceId));
				}
			}
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000F2934 File Offset: 0x000F0B34
		private void OnDataAvailableCallback(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Received data from dataToBeSent store.", new object[0]);
			this.SendData(data, priorityType);
		}

		// Token: 0x040015FD RID: 5629
		private OutOfProcessTextWriter stdInWriter;

		// Token: 0x040015FE RID: 5630
		private PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableToSendCallback;

		// Token: 0x040015FF RID: 5631
		private Timer signalTimeOutTimer;
	}
}
