using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x02000366 RID: 870
	internal abstract class AbstractServerTransportManager : BaseTransportManager
	{
		// Token: 0x06002B06 RID: 11014 RVA: 0x000EC615 File Offset: 0x000EA815
		protected AbstractServerTransportManager(int fragmentSize, PSRemotingCryptoHelper cryptoHelper) : base(cryptoHelper)
		{
			base.Fragmentor.FragmentSize = fragmentSize;
			this.onDataAvailable = new SerializedDataStream.OnDataAvailableCallback(this.OnDataAvailable);
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000EC648 File Offset: 0x000EA848
		internal void SendDataToClient<T>(RemoteDataObject<T> data, bool flush, bool reportPending = false)
		{
			lock (this.syncObject)
			{
				RemoteDataObject remoteDataObject = RemoteDataObject.CreateFrom(data.Destination, data.DataType, data.RunspacePoolId, data.PowerShellId, data.Data);
				if (this.isSerializing)
				{
					if (this.dataToBeSentQueue == null)
					{
						this.dataToBeSentQueue = new Queue<Tuple<RemoteDataObject, bool, bool>>();
					}
					this.dataToBeSentQueue.Enqueue(new Tuple<RemoteDataObject, bool, bool>(remoteDataObject, flush, reportPending));
				}
				else
				{
					this.isSerializing = true;
					try
					{
						do
						{
							using (SerializedDataStream serializedDataStream = new SerializedDataStream(base.Fragmentor.FragmentSize, this.onDataAvailable))
							{
								this.shouldFlushData = flush;
								this.reportAsPending = reportPending;
								this.runpacePoolInstanceId = remoteDataObject.RunspacePoolId;
								this.powerShellInstanceId = remoteDataObject.PowerShellId;
								this.dataType = remoteDataObject.DataType;
								this.targetInterface = remoteDataObject.TargetInterface;
								base.Fragmentor.Fragment<object>(remoteDataObject, serializedDataStream);
							}
							if (this.dataToBeSentQueue != null && this.dataToBeSentQueue.Count > 0)
							{
								Tuple<RemoteDataObject, bool, bool> tuple = this.dataToBeSentQueue.Dequeue();
								remoteDataObject = tuple.Item1;
								flush = tuple.Item2;
								reportPending = tuple.Item3;
							}
							else
							{
								remoteDataObject = null;
							}
						}
						while (remoteDataObject != null);
					}
					finally
					{
						this.isSerializing = false;
					}
				}
			}
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
		private void OnDataAvailable(byte[] dataToSend, bool isEndFragment)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerSendData, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				this.runpacePoolInstanceId.ToString(),
				this.powerShellInstanceId.ToString(),
				dataToSend.Length.ToString(CultureInfo.InvariantCulture),
				(uint)this.dataType,
				(uint)this.targetInterface
			});
			this.SendDataToClient(dataToSend, isEndFragment & this.shouldFlushData, this.reportAsPending, isEndFragment);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000EC87E File Offset: 0x000EAA7E
		internal void SendDataToClient(RemoteDataObject psObjectData, bool flush, bool reportAsPending = false)
		{
			this.SendDataToClient<object>(psObjectData, flush, reportAsPending);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000EC8BC File Offset: 0x000EAABC
		internal void ReportError(int errorCode, string methodName)
		{
			string generalError = RemotingErrorIdStrings.GeneralError;
			string message = string.Format(CultureInfo.InvariantCulture, generalError, new object[]
			{
				errorCode,
				methodName
			});
			PSRemotingTransportException e = new PSRemotingTransportException(message);
			e.ErrorCode = errorCode;
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.Unknown);
				this.RaiseErrorHandler(eventArgs);
			});
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000EC927 File Offset: 0x000EAB27
		internal void RaiseClosingEvent()
		{
			this.Closing.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06002B0C RID: 11020 RVA: 0x000EC93C File Offset: 0x000EAB3C
		// (remove) Token: 0x06002B0D RID: 11021 RVA: 0x000EC974 File Offset: 0x000EAB74
		internal event EventHandler Closing;

		// Token: 0x06002B0E RID: 11022
		protected abstract void SendDataToClient(byte[] data, bool flush, bool reportAsPending, bool reportAsDataBoundary);

		// Token: 0x06002B0F RID: 11023
		internal abstract void ReportExecutionStatusAsRunning();

		// Token: 0x06002B10 RID: 11024
		internal abstract void Close(Exception reasonForClose);

		// Token: 0x06002B11 RID: 11025 RVA: 0x000EC9A9 File Offset: 0x000EABA9
		internal virtual void Prepare()
		{
			base.ReceivedDataCollection.AllowTwoThreadsToProcessRawData();
		}

		// Token: 0x0400155E RID: 5470
		private object syncObject = new object();

		// Token: 0x0400155F RID: 5471
		private SerializedDataStream.OnDataAvailableCallback onDataAvailable;

		// Token: 0x04001560 RID: 5472
		private bool shouldFlushData;

		// Token: 0x04001561 RID: 5473
		private bool reportAsPending;

		// Token: 0x04001562 RID: 5474
		private Guid runpacePoolInstanceId;

		// Token: 0x04001563 RID: 5475
		private Guid powerShellInstanceId;

		// Token: 0x04001564 RID: 5476
		private RemotingDataType dataType;

		// Token: 0x04001565 RID: 5477
		private RemotingTargetInterface targetInterface;

		// Token: 0x04001566 RID: 5478
		private Queue<Tuple<RemoteDataObject, bool, bool>> dataToBeSentQueue;

		// Token: 0x04001567 RID: 5479
		private bool isSerializing;
	}
}
