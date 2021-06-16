using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000362 RID: 866
	internal abstract class BaseClientTransportManager : BaseTransportManager, IDisposable
	{
		// Token: 0x06002AC7 RID: 10951 RVA: 0x000EB8E4 File Offset: 0x000E9AE4
		protected BaseClientTransportManager(Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(cryptoHelper)
		{
			this.runspacePoolInstanceId = runspaceId;
			this.dataToBeSent = new PrioritySendDataCollection();
			this.onDataAvailableCallback = new ReceiveDataCollection.OnDataAvailableCallback(this.OnDataAvailableHandler);
			this.callbackNotificationQueue = new Queue<BaseClientTransportManager.CallbackNotificationInformation>();
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06002AC8 RID: 10952 RVA: 0x000EB934 File Offset: 0x000E9B34
		// (remove) Token: 0x06002AC9 RID: 10953 RVA: 0x000EB96C File Offset: 0x000E9B6C
		internal event EventHandler<CreateCompleteEventArgs> CreateCompleted;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06002ACA RID: 10954 RVA: 0x000EB9A4 File Offset: 0x000E9BA4
		// (remove) Token: 0x06002ACB RID: 10955 RVA: 0x000EB9DC File Offset: 0x000E9BDC
		internal event EventHandler<EventArgs> CloseCompleted;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06002ACC RID: 10956 RVA: 0x000EBA14 File Offset: 0x000E9C14
		// (remove) Token: 0x06002ACD RID: 10957 RVA: 0x000EBA4C File Offset: 0x000E9C4C
		internal event EventHandler<EventArgs> ConnectCompleted;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06002ACE RID: 10958 RVA: 0x000EBA84 File Offset: 0x000E9C84
		// (remove) Token: 0x06002ACF RID: 10959 RVA: 0x000EBABC File Offset: 0x000E9CBC
		internal event EventHandler<EventArgs> DisconnectCompleted;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06002AD0 RID: 10960 RVA: 0x000EBAF4 File Offset: 0x000E9CF4
		// (remove) Token: 0x06002AD1 RID: 10961 RVA: 0x000EBB2C File Offset: 0x000E9D2C
		internal event EventHandler<EventArgs> ReconnectCompleted;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06002AD2 RID: 10962 RVA: 0x000EBB64 File Offset: 0x000E9D64
		// (remove) Token: 0x06002AD3 RID: 10963 RVA: 0x000EBB9C File Offset: 0x000E9D9C
		internal event EventHandler<EventArgs> ReadyForDisconnect;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06002AD4 RID: 10964 RVA: 0x000EBBD4 File Offset: 0x000E9DD4
		// (remove) Token: 0x06002AD5 RID: 10965 RVA: 0x000EBC0C File Offset: 0x000E9E0C
		internal event EventHandler<ConnectionStatusEventArgs> RobustConnectionNotification;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06002AD6 RID: 10966 RVA: 0x000EBC44 File Offset: 0x000E9E44
		// (remove) Token: 0x06002AD7 RID: 10967 RVA: 0x000EBC7C File Offset: 0x000E9E7C
		internal event EventHandler<EventArgs> DelayStreamRequestProcessed;

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000EBCB1 File Offset: 0x000E9EB1
		internal PrioritySendDataCollection DataToBeSentCollection
		{
			get
			{
				return this.dataToBeSent;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000EBCB9 File Offset: 0x000E9EB9
		internal Guid RunspacePoolInstanceId
		{
			get
			{
				return this.runspacePoolInstanceId;
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000EBCC1 File Offset: 0x000E9EC1
		internal void RaiseCreateCompleted(CreateCompleteEventArgs eventArgs)
		{
			this.CreateCompleted.SafeInvoke(this, eventArgs);
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000EBCD0 File Offset: 0x000E9ED0
		internal void RaiseConnectCompleted()
		{
			this.ConnectCompleted.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000EBCE3 File Offset: 0x000E9EE3
		internal void RaiseDisconnectCompleted()
		{
			this.DisconnectCompleted.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000EBCF6 File Offset: 0x000E9EF6
		internal void RaiseReconnectCompleted()
		{
			this.ReconnectCompleted.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000EBD09 File Offset: 0x000E9F09
		internal void RaiseCloseCompleted()
		{
			this.CloseCompleted.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000EBD1C File Offset: 0x000E9F1C
		internal void RaiseReadyForDisconnect()
		{
			this.ReadyForDisconnect.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000EBD30 File Offset: 0x000E9F30
		internal void QueueRobustConnectionNotification(int flags)
		{
			ConnectionStatusEventArgs privateData = null;
			if (flags <= 512)
			{
				if (flags != 64)
				{
					if (flags != 256)
					{
						if (flags == 512)
						{
							privateData = new ConnectionStatusEventArgs(ConnectionStatus.ConnectionRetryAttempt);
						}
					}
					else
					{
						privateData = new ConnectionStatusEventArgs(ConnectionStatus.NetworkFailureDetected);
					}
				}
				else
				{
					privateData = new ConnectionStatusEventArgs(ConnectionStatus.AutoDisconnectSucceeded);
				}
			}
			else if (flags != 1024)
			{
				if (flags != 2048)
				{
					if (flags == 4096)
					{
						privateData = new ConnectionStatusEventArgs(ConnectionStatus.InternalErrorAbort);
					}
				}
				else
				{
					privateData = new ConnectionStatusEventArgs(ConnectionStatus.AutoDisconnectStarting);
				}
			}
			else
			{
				privateData = new ConnectionStatusEventArgs(ConnectionStatus.ConnectionRetrySucceeded);
			}
			this.EnqueueAndStartProcessingThread(null, null, privateData);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000EBDB7 File Offset: 0x000E9FB7
		internal void RaiseRobustConnectionNotification(ConnectionStatusEventArgs args)
		{
			this.RobustConnectionNotification.SafeInvoke(this, args);
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000EBDC6 File Offset: 0x000E9FC6
		internal void RaiseDelayStreamProcessedEvent()
		{
			this.DelayStreamRequestProcessed.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000EBDDC File Offset: 0x000E9FDC
		internal override void ProcessRawData(byte[] data, string stream)
		{
			if (this.isClosed)
			{
				return;
			}
			try
			{
				base.ProcessRawData(data, stream, this.onDataAvailableCallback);
			}
			catch (PSRemotingTransportException ex)
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Exception processing data. {0}", new object[]
				{
					ex.Message
				}), new object[0]);
				TransportErrorOccuredEventArgs transportErrorArgs = new TransportErrorOccuredEventArgs(ex, TransportMethodEnum.ReceiveShellOutputEx);
				this.EnqueueAndStartProcessingThread(null, transportErrorArgs, null);
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Exception processing data. {0}", new object[]
				{
					ex2.Message
				}), new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(ex2.Message);
				TransportErrorOccuredEventArgs transportErrorArgs2 = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx);
				this.EnqueueAndStartProcessingThread(null, transportErrorArgs2, null);
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000EBEC4 File Offset: 0x000EA0C4
		private void OnDataAvailableHandler(RemoteDataObject<PSObject> remoteObject)
		{
			this.EnqueueAndStartProcessingThread(remoteObject, null, null);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000EBED0 File Offset: 0x000EA0D0
		internal void EnqueueAndStartProcessingThread(RemoteDataObject<PSObject> remoteObject, TransportErrorOccuredEventArgs transportErrorArgs, object privateData)
		{
			if (this.isClosed)
			{
				return;
			}
			lock (this.callbackNotificationQueue)
			{
				if (remoteObject != null || transportErrorArgs != null || privateData != null)
				{
					BaseClientTransportManager.CallbackNotificationInformation callbackNotificationInformation = new BaseClientTransportManager.CallbackNotificationInformation();
					callbackNotificationInformation.remoteObject = remoteObject;
					callbackNotificationInformation.transportError = transportErrorArgs;
					callbackNotificationInformation.privateData = privateData;
					if (remoteObject != null && (remoteObject.DataType == RemotingDataType.PublicKey || remoteObject.DataType == RemotingDataType.EncryptedSessionKey || remoteObject.DataType == RemotingDataType.PublicKeyRequest))
					{
						base.CryptoHelper.Session.BaseSessionDataStructureHandler.RaiseKeyExchangeMessageReceived(remoteObject);
					}
					else
					{
						this.callbackNotificationQueue.Enqueue(callbackNotificationInformation);
					}
				}
				if (this.suspendQueueServicing && this.isDebuggerSuspend)
				{
					this.suspendQueueServicing = !this.CheckForInteractiveHostCall(remoteObject);
				}
				if (!this.isServicingCallbacks && !this.suspendQueueServicing)
				{
					if (this.callbackNotificationQueue.Count > 0)
					{
						this.isServicingCallbacks = true;
						WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
						windowsIdentity = ((windowsIdentity.ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? windowsIdentity : null);
						Utils.QueueWorkItemWithImpersonation(windowsIdentity, new WaitCallback(this.ServicePendingCallbacks), null);
					}
				}
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000EBFF4 File Offset: 0x000EA1F4
		private bool CheckForInteractiveHostCall(RemoteDataObject<PSObject> remoteObject)
		{
			bool result = false;
			if (remoteObject != null && remoteObject.DataType == RemotingDataType.RemoteHostCallUsingPowerShellHost)
			{
				RemoteHostMethodId remoteHostMethodId = (RemoteHostMethodId)0;
				try
				{
					remoteHostMethodId = RemotingDecoder.GetPropertyValue<RemoteHostMethodId>(remoteObject.Data, "mi");
				}
				catch (PSArgumentNullException)
				{
				}
				catch (PSRemotingDataStructureException)
				{
				}
				RemoteHostMethodId remoteHostMethodId2 = remoteHostMethodId;
				if (remoteHostMethodId2 <= RemoteHostMethodId.PromptForChoice)
				{
					switch (remoteHostMethodId2)
					{
					case RemoteHostMethodId.ReadLine:
					case RemoteHostMethodId.ReadLineAsSecureString:
						break;
					default:
						switch (remoteHostMethodId2)
						{
						case RemoteHostMethodId.Prompt:
						case RemoteHostMethodId.PromptForCredential1:
						case RemoteHostMethodId.PromptForCredential2:
						case RemoteHostMethodId.PromptForChoice:
							break;
						default:
							return result;
						}
						break;
					}
				}
				else if (remoteHostMethodId2 != RemoteHostMethodId.ReadKey && remoteHostMethodId2 != RemoteHostMethodId.PromptForChoiceMultipleSelection)
				{
					return result;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000EC08C File Offset: 0x000EA28C
		internal void ServicePendingCallbacks(object objectToProcess)
		{
			BaseClientTransportManager.tracer.WriteLine("ServicePendingCallbacks thread is starting", new object[0]);
			PSEtwLog.ReplaceActivityIdForCurrentThread(this.runspacePoolInstanceId, PSEventId.OperationalTransferEventRunspacePool, PSEventId.AnalyticTransferEventRunspacePool, PSKeyword.Transport, PSTask.None);
			try
			{
				while (!this.isClosed)
				{
					BaseClientTransportManager.CallbackNotificationInformation callbackNotificationInformation = null;
					lock (this.callbackNotificationQueue)
					{
						if (this.callbackNotificationQueue.Count <= 0 || this.suspendQueueServicing)
						{
							break;
						}
						callbackNotificationInformation = this.callbackNotificationQueue.Dequeue();
					}
					if (callbackNotificationInformation == null)
					{
						continue;
					}
					if (callbackNotificationInformation.transportError != null)
					{
						this.RaiseErrorHandler(callbackNotificationInformation.transportError);
					}
					else
					{
						if (callbackNotificationInformation.privateData != null)
						{
							this.ProcessPrivateData(callbackNotificationInformation.privateData);
							continue;
						}
						base.OnDataAvailableCallback(callbackNotificationInformation.remoteObject);
						continue;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Exception processing data. {0}", new object[]
				{
					ex.Message
				}), new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(ex.Message, ex);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx);
				this.RaiseErrorHandler(eventArgs);
			}
			finally
			{
				lock (this.callbackNotificationQueue)
				{
					BaseClientTransportManager.tracer.WriteLine("ServicePendingCallbacks thread is exiting", new object[0]);
					this.isServicingCallbacks = false;
					this.EnqueueAndStartProcessingThread(null, null, null);
				}
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x000EC234 File Offset: 0x000EA434
		internal bool IsServicing
		{
			get
			{
				bool result;
				lock (this.callbackNotificationQueue)
				{
					result = this.isServicingCallbacks;
				}
				return result;
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000EC278 File Offset: 0x000EA478
		internal void SuspendQueue(bool debuggerSuspend = false)
		{
			lock (this.callbackNotificationQueue)
			{
				this.isDebuggerSuspend = debuggerSuspend;
				this.suspendQueueServicing = true;
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000EC2C0 File Offset: 0x000EA4C0
		internal void ResumeQueue()
		{
			lock (this.callbackNotificationQueue)
			{
				this.isDebuggerSuspend = false;
				if (this.suspendQueueServicing)
				{
					this.suspendQueueServicing = false;
					this.EnqueueAndStartProcessingThread(null, null, null);
				}
			}
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000EC31C File Offset: 0x000EA51C
		internal virtual void ProcessPrivateData(object privateData)
		{
		}

		// Token: 0x06002AEC RID: 10988
		internal abstract void CreateAsync();

		// Token: 0x06002AED RID: 10989
		internal abstract void ConnectAsync();

		// Token: 0x06002AEE RID: 10990 RVA: 0x000EC31E File Offset: 0x000EA51E
		internal virtual void CloseAsync()
		{
			this.dataToBeSent.Clear();
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000EC32B File Offset: 0x000EA52B
		internal virtual void StartReceivingData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x000EC332 File Offset: 0x000EA532
		internal virtual void PrepareForDisconnect()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000EC339 File Offset: 0x000EA539
		internal virtual void PrepareForConnect()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000EC34C File Offset: 0x000EA54C
		~BaseClientTransportManager()
		{
			if (this.isClosed)
			{
				this.Dispose(false);
			}
			else
			{
				this.CloseCompleted += delegate(object source, EventArgs args)
				{
					this.Dispose(false);
				};
				try
				{
					this.CloseAsync();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000EC3B4 File Offset: 0x000EA5B4
		internal override void Dispose(bool isDisposing)
		{
			this.CreateCompleted = null;
			this.CloseCompleted = null;
			this.ConnectCompleted = null;
			this.DisconnectCompleted = null;
			this.ReconnectCompleted = null;
			base.Dispose(isDisposing);
		}

		// Token: 0x04001543 RID: 5443
		[TraceSource("ClientTransport", "Traces ClientTransportManager")]
		protected static PSTraceSource tracer = PSTraceSource.GetTracer("ClientTransport", "Traces ClientTransportManager");

		// Token: 0x04001544 RID: 5444
		protected bool isClosed;

		// Token: 0x04001545 RID: 5445
		protected object syncObject = new object();

		// Token: 0x04001546 RID: 5446
		protected PrioritySendDataCollection dataToBeSent;

		// Token: 0x04001547 RID: 5447
		private Queue<BaseClientTransportManager.CallbackNotificationInformation> callbackNotificationQueue;

		// Token: 0x04001548 RID: 5448
		private ReceiveDataCollection.OnDataAvailableCallback onDataAvailableCallback;

		// Token: 0x04001549 RID: 5449
		private bool isServicingCallbacks;

		// Token: 0x0400154A RID: 5450
		private bool suspendQueueServicing;

		// Token: 0x0400154B RID: 5451
		private bool isDebuggerSuspend;

		// Token: 0x0400154C RID: 5452
		private Guid runspacePoolInstanceId;

		// Token: 0x0400154D RID: 5453
		protected bool receiveDataInitiated;

		// Token: 0x02000363 RID: 867
		internal class CallbackNotificationInformation
		{
			// Token: 0x04001556 RID: 5462
			internal RemoteDataObject<PSObject> remoteObject;

			// Token: 0x04001557 RID: 5463
			internal TransportErrorOccuredEventArgs transportError;

			// Token: 0x04001558 RID: 5464
			internal object privateData;
		}
	}
}
