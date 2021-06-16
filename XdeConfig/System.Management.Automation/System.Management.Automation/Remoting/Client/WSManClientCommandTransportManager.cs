using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000394 RID: 916
	internal sealed class WSManClientCommandTransportManager : BaseClientCommandTransportManager
	{
		// Token: 0x06002C4E RID: 11342 RVA: 0x000F6B88 File Offset: 0x000F4D88
		static WSManClientCommandTransportManager()
		{
			WSManNativeApi.WSManShellCompletionFunction callback = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnCreateCmdCompleted);
			WSManClientCommandTransportManager.cmdCreateCallback = new WSManNativeApi.WSManShellAsyncCallback(callback);
			WSManNativeApi.WSManShellCompletionFunction callback2 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnCloseCmdCompleted);
			WSManClientCommandTransportManager.cmdCloseCallback = new WSManNativeApi.WSManShellAsyncCallback(callback2);
			WSManNativeApi.WSManShellCompletionFunction callback3 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnRemoteCmdDataReceived);
			WSManClientCommandTransportManager.cmdReceiveCallback = new WSManNativeApi.WSManShellAsyncCallback(callback3);
			WSManNativeApi.WSManShellCompletionFunction callback4 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnRemoteCmdSendCompleted);
			WSManClientCommandTransportManager.cmdSendCallback = new WSManNativeApi.WSManShellAsyncCallback(callback4);
			WSManNativeApi.WSManShellCompletionFunction callback5 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnRemoteCmdSignalCompleted);
			WSManClientCommandTransportManager.cmdSignalCallback = new WSManNativeApi.WSManShellAsyncCallback(callback5);
			WSManNativeApi.WSManShellCompletionFunction callback6 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnReconnectCmdCompleted);
			WSManClientCommandTransportManager.cmdReconnectCallback = new WSManNativeApi.WSManShellAsyncCallback(callback6);
			WSManNativeApi.WSManShellCompletionFunction callback7 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientCommandTransportManager.OnConnectCmdCompleted);
			WSManClientCommandTransportManager.cmdConnectCallback = new WSManNativeApi.WSManShellAsyncCallback(callback7);
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000F6C5C File Offset: 0x000F4E5C
		internal WSManClientCommandTransportManager(WSManConnectionInfo connectionInfo, IntPtr wsManShellOperationHandle, ClientRemotePowerShell shell, bool noInput, WSManClientSessionTransportManager sessnTM) : base(shell, sessnTM.CryptoHelper, sessnTM)
		{
			this.wsManShellOperationHandle = wsManShellOperationHandle;
			base.ReceivedDataCollection.MaximumReceivedDataSize = connectionInfo.MaximumReceivedDataSizePerCommand;
			base.ReceivedDataCollection.MaximumReceivedObjectSize = connectionInfo.MaximumReceivedObjectSize;
			this.cmdLine = shell.PowerShell.Commands.Commands.GetCommandStringForHistory();
			this.onDataAvailableToSendCallback = new PrioritySendDataCollection.OnDataAvailableCallback(this.OnDataAvailableCallback);
			this._sessnTm = sessnTM;
			sessnTM.RobustConnectionsInitiated += this.HandleRobustConnectionsIntiated;
			sessnTM.RobustConnectionsCompleted += this.HandleRobusConnectionsCompleted;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000F6CFD File Offset: 0x000F4EFD
		private void HandleRobustConnectionsIntiated(object sender, EventArgs e)
		{
			base.SuspendQueue(false);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x000F6D06 File Offset: 0x000F4F06
		private void HandleRobusConnectionsCompleted(object sender, EventArgs e)
		{
			base.ResumeQueue();
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x000F6D10 File Offset: 0x000F4F10
		internal override void ConnectAsync()
		{
			base.ReceivedDataCollection.PrepareForStreamConnect();
			this.serializedPipeline.Read();
			this.cmdContextId = WSManClientCommandTransportManager.GetNextCmdTMHandleId();
			WSManClientCommandTransportManager.AddCmdTransportManager(this.cmdContextId, this);
			this.connectCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdConnectCallback);
			this.reconnectCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdReconnectCallback);
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				WSManNativeApi.WSManConnectShellCommandEx(this.wsManShellOperationHandle, 0, base.PowershellInstanceId.ToString().ToUpperInvariant(), IntPtr.Zero, IntPtr.Zero, this.connectCmdCompleted, ref this.wsManCmdOperationHandle);
			}
			if (this.wsManCmdOperationHandle == IntPtr.Zero)
			{
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.RunShellCommandExFailed);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ConnectShellCommandEx);
				this.ProcessWSManTransportError(eventArgs);
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000F6E28 File Offset: 0x000F5028
		internal override void CreateAsync()
		{
			byte[] array = this.serializedPipeline.ReadOrRegisterCallback(null);
			if (array != null)
			{
				bool flag = true;
				if (WSManClientCommandTransportManager.commandCodeSendRedirect != null)
				{
					object[] array2 = new object[]
					{
						null,
						array
					};
					flag = (bool)WSManClientCommandTransportManager.commandCodeSendRedirect.DynamicInvoke(array2);
					array = (byte[])array2[0];
				}
				if (!flag)
				{
					return;
				}
				WSManNativeApi.WSManCommandArgSet wsmanCommandArgSet = new WSManNativeApi.WSManCommandArgSet(array);
				this.cmdContextId = WSManClientCommandTransportManager.GetNextCmdTMHandleId();
				WSManClientCommandTransportManager.AddCmdTransportManager(this.cmdContextId, this);
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateCommand, PSOpcode.Connect, PSTask.CreateRunspace, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString(),
					this.powershellInstanceId.ToString()
				});
				this.createCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdCreateCallback);
				this.createCmdCompletedGCHandle = GCHandle.Alloc(this.createCmdCompleted);
				this.reconnectCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdReconnectCallback);
				using (wsmanCommandArgSet)
				{
					lock (this.syncObject)
					{
						if (!this.isClosed)
						{
							WSManNativeApi.WSManRunShellCommandEx(this.wsManShellOperationHandle, 0, base.PowershellInstanceId.ToString().ToUpperInvariant(), (this.cmdLine == null || this.cmdLine.Length == 0) ? " " : ((this.cmdLine.Length <= 256) ? this.cmdLine : this.cmdLine.Substring(0, 255)), wsmanCommandArgSet, IntPtr.Zero, this.createCmdCompleted, ref this.wsManCmdOperationHandle);
							BaseClientTransportManager.tracer.WriteLine("Started cmd with command context : {0} Operation context: {1}", new object[]
							{
								this.cmdContextId,
								this.wsManCmdOperationHandle
							});
						}
					}
				}
			}
			if (this.wsManCmdOperationHandle == IntPtr.Zero)
			{
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.RunShellCommandExFailed);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.RunShellCommandEx);
				this.ProcessWSManTransportError(eventArgs);
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000F7080 File Offset: 0x000F5280
		internal override void ReconnectAsync()
		{
			base.ReceivedDataCollection.PrepareForStreamConnect();
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					WSManNativeApi.WSManReconnectShellCommandEx(this.wsManCmdOperationHandle, 0, this.reconnectCmdCompleted);
				}
			}
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x000F70E8 File Offset: 0x000F52E8
		internal override void SendStopSignal()
		{
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					if (!this.isCreateCallbackReceived)
					{
						this.isStopSignalPending = true;
					}
					else
					{
						this.isStopSignalPending = false;
						BaseClientTransportManager.tracer.WriteLine("Sending stop signal with command context: {0} Operation Context {1}", new object[]
						{
							this.cmdContextId,
							this.wsManCmdOperationHandle
						});
						PSEtwLog.LogAnalyticInformational(PSEventId.WSManSignal, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
						{
							base.RunspacePoolInstanceId.ToString(),
							this.powershellInstanceId.ToString(),
							"powershell/signal/crtl_c"
						});
						this.signalCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdSignalCallback);
						WSManNativeApi.WSManSignalShellEx(this.wsManShellOperationHandle, this.wsManCmdOperationHandle, 0, "powershell/signal/crtl_c", this.signalCmdCompleted, ref this.cmdSignalOperationHandle);
					}
				}
			}
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000F7220 File Offset: 0x000F5420
		internal override void CloseAsync()
		{
			BaseClientTransportManager.tracer.WriteLine("Closing command with command context: {0} Operation Context {1}", new object[]
			{
				this.cmdContextId,
				this.wsManCmdOperationHandle
			});
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				this.isClosed = true;
				if (IntPtr.Zero == this.wsManCmdOperationHandle)
				{
					flag = true;
				}
			}
			base.CloseAsync();
			if (!flag)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseCommand, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString(),
					this.powershellInstanceId.ToString()
				});
				this.closeCmdCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdCloseCallback);
				WSManNativeApi.WSManCloseCommand(this.wsManCmdOperationHandle, 0, this.closeCmdCompleted);
				return;
			}
			try
			{
				base.RaiseCloseCompleted();
			}
			finally
			{
				WSManClientCommandTransportManager.RemoveCmdTransportManager(this.cmdContextId);
			}
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x000F7360 File Offset: 0x000F5560
		internal void ProcessWSManTransportError(TransportErrorOccuredEventArgs eventArgs)
		{
			base.EnqueueAndStartProcessingThread(null, eventArgs, null);
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000F736C File Offset: 0x000F556C
		internal override void RaiseErrorHandler(TransportErrorOccuredEventArgs eventArgs)
		{
			string text;
			if (!string.IsNullOrEmpty(eventArgs.Exception.StackTrace))
			{
				text = eventArgs.Exception.StackTrace;
			}
			else if (eventArgs.Exception.InnerException != null && !string.IsNullOrEmpty(eventArgs.Exception.InnerException.StackTrace))
			{
				text = eventArgs.Exception.InnerException.StackTrace;
			}
			else
			{
				text = string.Empty;
			}
			PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Open, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString(),
				eventArgs.Exception.ErrorCode.ToString(CultureInfo.InvariantCulture),
				eventArgs.Exception.Message,
				text
			});
			PSEtwLog.LogAnalyticError(PSEventId.TransportError_Analytic, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString(),
				eventArgs.Exception.ErrorCode.ToString(CultureInfo.InvariantCulture),
				eventArgs.Exception.Message,
				text
			});
			base.RaiseErrorHandler(eventArgs);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000F74D4 File Offset: 0x000F56D4
		internal override void ProcessPrivateData(object privateData)
		{
			bool flag = (bool)privateData;
			if (flag)
			{
				base.RaiseSignalCompleted();
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000F74F4 File Offset: 0x000F56F4
		internal void ClearReceiveOrSendResources(int flags, bool shouldClearSend)
		{
			if (shouldClearSend)
			{
				if (this.sendToRemoteCompleted != null)
				{
					this.sendToRemoteCompleted.Dispose();
					this.sendToRemoteCompleted = null;
				}
				if (IntPtr.Zero != this.wsManSendOperationHandle)
				{
					WSManNativeApi.WSManCloseOperation(this.wsManSendOperationHandle, 0);
					this.wsManSendOperationHandle = IntPtr.Zero;
					return;
				}
			}
			else if (flags == 1)
			{
				if (IntPtr.Zero != this.wsManRecieveOperationHandle)
				{
					WSManNativeApi.WSManCloseOperation(this.wsManRecieveOperationHandle, 0);
					this.wsManRecieveOperationHandle = IntPtr.Zero;
				}
				if (this.receivedFromRemote != null)
				{
					this.receivedFromRemote.Dispose();
					this.receivedFromRemote = null;
				}
			}
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000F758F File Offset: 0x000F578F
		internal override void PrepareForDisconnect()
		{
			this.isDisconnectPending = true;
			if (this.isClosed || this.isDisconnectedOnInvoke || (this.isCreateCallbackReceived && this.serializedPipeline.Length == 0L && !this.isSendingInput))
			{
				base.RaiseReadyForDisconnect();
			}
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000F75CD File Offset: 0x000F57CD
		internal override void PrepareForConnect()
		{
			this.isDisconnectPending = false;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000F75D8 File Offset: 0x000F57D8
		private static void OnCreateCmdCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("OnCreateCmdCompleted callback received", new object[0]);
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("OnCreateCmdCompleted: Unable to find a transport manager for the command context {0}.", new object[]
				{
					num
				});
				return;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateCommandCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientCommandTransportManager.RunspacePoolInstanceId.ToString(),
				wsmanClientCommandTransportManager.powershellInstanceId.ToString()
			});
			if (wsmanClientCommandTransportManager.createCmdCompleted != null)
			{
				wsmanClientCommandTransportManager.createCmdCompletedGCHandle.Free();
				wsmanClientCommandTransportManager.createCmdCompleted.Dispose();
				wsmanClientCommandTransportManager.createCmdCompleted = null;
			}
			wsmanClientCommandTransportManager.wsManCmdOperationHandle = commandOperationHandle;
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine("OnCreateCmdCompleted callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.RunShellCommandEx, RemotingErrorIdStrings.RunShellCommandExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			lock (wsmanClientCommandTransportManager.syncObject)
			{
				wsmanClientCommandTransportManager.isCreateCallbackReceived = true;
				if (wsmanClientCommandTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					if (wsmanClientCommandTransportManager.isDisconnectPending)
					{
						wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
					}
				}
				else if (wsmanClientCommandTransportManager.isDisconnectPending)
				{
					wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
				}
				else
				{
					if (wsmanClientCommandTransportManager.serializedPipeline.Length == 0L)
					{
						wsmanClientCommandTransportManager.shouldStartReceivingData = true;
					}
					wsmanClientCommandTransportManager.SendOneItem();
					if (wsmanClientCommandTransportManager.isStopSignalPending)
					{
						wsmanClientCommandTransportManager.SendStopSignal();
					}
				}
			}
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000F77CC File Offset: 0x000F59CC
		private static void OnConnectCmdCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("OnConnectCmdCompleted callback received", new object[0]);
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("OnConnectCmdCompleted: Unable to find a transport manager for the command context {0}.", new object[]
				{
					num
				});
				return;
			}
			if (wsmanClientCommandTransportManager.connectCmdCompleted != null)
			{
				wsmanClientCommandTransportManager.connectCmdCompleted.Dispose();
				wsmanClientCommandTransportManager.connectCmdCompleted = null;
			}
			wsmanClientCommandTransportManager.wsManCmdOperationHandle = commandOperationHandle;
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine("OnConnectCmdCompleted callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.ReconnectShellCommandEx, RemotingErrorIdStrings.ReconnectShellCommandExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			lock (wsmanClientCommandTransportManager.syncObject)
			{
				if (wsmanClientCommandTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					if (wsmanClientCommandTransportManager.isDisconnectPending)
					{
						wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
					}
					return;
				}
				if (wsmanClientCommandTransportManager.isDisconnectPending)
				{
					wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
					return;
				}
				wsmanClientCommandTransportManager.isCreateCallbackReceived = true;
				if (wsmanClientCommandTransportManager.isStopSignalPending)
				{
					wsmanClientCommandTransportManager.SendStopSignal();
				}
			}
			wsmanClientCommandTransportManager.SendOneItem();
			wsmanClientCommandTransportManager.RaiseConnectCompleted();
			wsmanClientCommandTransportManager.StartReceivingData();
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000F795C File Offset: 0x000F5B5C
		private static void OnCloseCmdCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("OnCloseCmdCompleted callback received for operation context {0}", new object[]
			{
				commandOperationHandle
			});
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("OnCloseCmdCompleted: Unable to find a transport manager for the command context {0}.", new object[]
				{
					num
				});
				return;
			}
			BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Close completed callback received for command: {0}", new object[]
			{
				wsmanClientCommandTransportManager.cmdContextId
			}), new object[0]);
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseCommandCallbackReceived, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientCommandTransportManager.RunspacePoolInstanceId.ToString(),
				wsmanClientCommandTransportManager.powershellInstanceId.ToString()
			});
			if (wsmanClientCommandTransportManager.isDisconnectPending)
			{
				wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
			}
			wsmanClientCommandTransportManager.RaiseCloseCompleted();
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000F7A58 File Offset: 0x000F5C58
		private static void OnRemoteCmdSendCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("SendComplete callback received", new object[0]);
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("Unable to find a transport manager for the command context {0}.", new object[]
				{
					num
				});
				return;
			}
			wsmanClientCommandTransportManager.isSendingInput = false;
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputExCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientCommandTransportManager.RunspacePoolInstanceId.ToString(),
				wsmanClientCommandTransportManager.powershellInstanceId.ToString()
			});
			if (!shellOperationHandle.Equals(wsmanClientCommandTransportManager.wsManShellOperationHandle) || !commandOperationHandle.Equals(wsmanClientCommandTransportManager.wsManCmdOperationHandle))
			{
				BaseClientTransportManager.tracer.WriteLine("SendShellInputEx callback: ShellOperationHandles are not the same as the Send is initiated with", new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.CommandSendExFailed);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.CommandInputEx);
				wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			wsmanClientCommandTransportManager.ClearReceiveOrSendResources(flags, true);
			if (wsmanClientCommandTransportManager.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Command TM: Transport manager is closed. So returning", new object[0]);
				if (wsmanClientCommandTransportManager.isDisconnectPending)
				{
					wsmanClientCommandTransportManager.RaiseReadyForDisconnect();
				}
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0 && errorStruct.errorCode != 995)
				{
					BaseClientTransportManager.tracer.WriteLine("CmdSend callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.CommandInputEx, RemotingErrorIdStrings.CommandSendExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			wsmanClientCommandTransportManager.SendOneItem();
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000F7C38 File Offset: 0x000F5E38
		private static void OnRemoteCmdDataReceived(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Remote Command DataReceived callback.", new object[0]);
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("Unable to find a transport manager for the given command context {0}.", new object[]
				{
					num
				});
				return;
			}
			if (!shellOperationHandle.Equals(wsmanClientCommandTransportManager.wsManShellOperationHandle) || !commandOperationHandle.Equals(wsmanClientCommandTransportManager.wsManCmdOperationHandle))
			{
				BaseClientTransportManager.tracer.WriteLine("CmdReceive callback: ShellOperationHandles are not the same as the Receive is initiated with", new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.CommandReceiveExFailed);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveCommandOutputEx);
				wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			wsmanClientCommandTransportManager.ClearReceiveOrSendResources(flags, false);
			if (wsmanClientCommandTransportManager.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Command TM: Transport manager is closed. So returning", new object[0]);
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine("CmdReceive callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.ReceiveCommandOutputEx, RemotingErrorIdStrings.CommandReceiveExCallBackError, new object[]
					{
						errorStruct.errorDetail
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			if (flags == 8192)
			{
				wsmanClientCommandTransportManager.isDisconnectedOnInvoke = true;
				wsmanClientCommandTransportManager.RaiseDelayStreamProcessedEvent();
				return;
			}
			WSManNativeApi.WSManReceiveDataResult wsmanReceiveDataResult = WSManNativeApi.WSManReceiveDataResult.UnMarshal(data);
			if (wsmanReceiveDataResult.data != null)
			{
				BaseClientTransportManager.tracer.WriteLine("Cmd Received Data : {0}", new object[]
				{
					wsmanReceiveDataResult.data.Length
				});
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputExCallbackReceived, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					wsmanClientCommandTransportManager.RunspacePoolInstanceId.ToString(),
					wsmanClientCommandTransportManager.powershellInstanceId.ToString(),
					wsmanReceiveDataResult.data.Length.ToString(CultureInfo.InvariantCulture)
				});
				wsmanClientCommandTransportManager.ProcessRawData(wsmanReceiveDataResult.data, wsmanReceiveDataResult.stream);
			}
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000F7E6C File Offset: 0x000F606C
		private static void OnReconnectCmdCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("Unable to find a transport manager for the given command context {0}.", new object[]
				{
					num
				});
				return;
			}
			if (!shellOperationHandle.Equals(wsmanClientCommandTransportManager.wsManShellOperationHandle) || !commandOperationHandle.Equals(wsmanClientCommandTransportManager.wsManCmdOperationHandle))
			{
				BaseClientTransportManager.tracer.WriteLine("Cmd Signal callback: ShellOperationHandles are not the same as the signal is initiated with", new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.ReconnectShellCommandExCallBackError);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReconnectShellCommandEx);
				wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine("OnReconnectCmdCompleted callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.ReconnectShellCommandEx, RemotingErrorIdStrings.ReconnectShellCommandExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			wsmanClientCommandTransportManager.shouldStartReceivingData = true;
			wsmanClientCommandTransportManager.SendOneItem();
			wsmanClientCommandTransportManager.RaiseReconnectCompleted();
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000F7FAC File Offset: 0x000F61AC
		private static void OnRemoteCmdSignalCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Signal Completed callback received.", new object[0]);
			long num = 0L;
			WSManClientCommandTransportManager wsmanClientCommandTransportManager = null;
			if (!WSManClientCommandTransportManager.TryGetCmdTransportManager(operationContext, out wsmanClientCommandTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine("Unable to find a transport manager for the given command context {0}.", new object[]
				{
					num
				});
				return;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSignalCallbackReceived, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientCommandTransportManager.RunspacePoolInstanceId.ToString(),
				wsmanClientCommandTransportManager.powershellInstanceId.ToString()
			});
			if (!shellOperationHandle.Equals(wsmanClientCommandTransportManager.wsManShellOperationHandle) || !commandOperationHandle.Equals(wsmanClientCommandTransportManager.wsManCmdOperationHandle))
			{
				BaseClientTransportManager.tracer.WriteLine("Cmd Signal callback: ShellOperationHandles are not the same as the signal is initiated with", new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(RemotingErrorIdStrings.CommandSendExFailed);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.CommandInputEx);
				wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			if (IntPtr.Zero != wsmanClientCommandTransportManager.cmdSignalOperationHandle)
			{
				WSManNativeApi.WSManCloseOperation(wsmanClientCommandTransportManager.cmdSignalOperationHandle, 0);
				wsmanClientCommandTransportManager.cmdSignalOperationHandle = IntPtr.Zero;
			}
			if (wsmanClientCommandTransportManager.signalCmdCompleted != null)
			{
				wsmanClientCommandTransportManager.signalCmdCompleted.Dispose();
				wsmanClientCommandTransportManager.signalCmdCompleted = null;
			}
			if (wsmanClientCommandTransportManager.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Command TM: Transport manager is closed. So returning", new object[0]);
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine("Cmd Signal callback: WSMan reported an error: {0}", new object[]
					{
						errorStruct.errorDetail
					});
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientCommandTransportManager._sessnTm.WSManAPIData.WSManAPIHandle, null, errorStruct, TransportMethodEnum.CommandInputEx, RemotingErrorIdStrings.CommandSendExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientCommandTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			wsmanClientCommandTransportManager.EnqueueAndStartProcessingThread(null, null, true);
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000F81AC File Offset: 0x000F63AC
		private void SendOneItem()
		{
			if (this.isDisconnectPending)
			{
				base.RaiseReadyForDisconnect();
				return;
			}
			DataPriorityType priorityType = DataPriorityType.Default;
			byte[] array;
			if (this.serializedPipeline.Length > 0L)
			{
				array = this.serializedPipeline.ReadOrRegisterCallback(null);
				if (this.serializedPipeline.Length == 0L)
				{
					this.shouldStartReceivingData = true;
				}
			}
			else if (this.chunkToSend != null)
			{
				array = this.chunkToSend.Data;
				priorityType = this.chunkToSend.Type;
				this.chunkToSend = null;
			}
			else
			{
				array = this.dataToBeSent.ReadOrRegisterCallback(this.onDataAvailableToSendCallback, out priorityType);
			}
			if (array != null)
			{
				this.isSendingInput = true;
				this.SendData(array, priorityType);
			}
			if (this.shouldStartReceivingData)
			{
				this.StartReceivingData();
			}
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000F825D File Offset: 0x000F645D
		private void OnDataAvailableCallback(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Received data from dataToBeSent store.", new object[0]);
			this.chunkToSend = new WSManClientCommandTransportManager.SendDataChunk(data, priorityType);
			this.SendOneItem();
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000F8288 File Offset: 0x000F6488
		private void SendData(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Command sending data of size : {0}", new object[]
			{
				data.Length
			});
			byte[] array = data;
			bool flag = true;
			if (WSManClientCommandTransportManager.commandSendRedirect != null)
			{
				object[] array2 = new object[]
				{
					null,
					array
				};
				flag = (bool)WSManClientCommandTransportManager.commandSendRedirect.DynamicInvoke(array2);
				array = (byte[])array2[0];
			}
			if (!flag)
			{
				return;
			}
			using (WSManNativeApi.WSManData_ManToUn wsmanData_ManToUn = new WSManNativeApi.WSManData_ManToUn(array))
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputEx, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString(),
					this.powershellInstanceId.ToString(),
					wsmanData_ManToUn.BufferLength.ToString(CultureInfo.InvariantCulture)
				});
				lock (this.syncObject)
				{
					if (this.isClosed)
					{
						BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					}
					else
					{
						this.sendToRemoteCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdSendCallback);
						WSManNativeApi.WSManSendShellInputEx(this.wsManShellOperationHandle, this.wsManCmdOperationHandle, 0, (priorityType == DataPriorityType.Default) ? "stdin" : "pr", wsmanData_ManToUn, this.sendToRemoteCompleted, ref this.wsManSendOperationHandle);
					}
				}
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000F8418 File Offset: 0x000F6618
		internal override void StartReceivingData()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputEx, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				this.powershellInstanceId.ToString()
			});
			this.shouldStartReceivingData = false;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				}
				else if (this.receiveDataInitiated)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: ReceiveData has already been called.", new object[0]);
				}
				else
				{
					this.receiveDataInitiated = true;
					this.receivedFromRemote = new WSManNativeApi.WSManShellAsync(new IntPtr(this.cmdContextId), WSManClientCommandTransportManager.cmdReceiveCallback);
					WSManNativeApi.WSManReceiveShellOutputEx(this.wsManShellOperationHandle, this.wsManCmdOperationHandle, this.startInDisconnectedMode ? 16 : 0, this._sessnTm.WSManAPIData.OutputStreamSet, this.receivedFromRemote, ref this.wsManRecieveOperationHandle);
				}
			}
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000F854C File Offset: 0x000F674C
		internal override void Dispose(bool isDisposing)
		{
			BaseClientTransportManager.tracer.WriteLine("Disposing command with command context: {0} Operation Context: {1}", new object[]
			{
				this.cmdContextId,
				this.wsManCmdOperationHandle
			});
			base.Dispose(isDisposing);
			WSManClientCommandTransportManager.RemoveCmdTransportManager(this.cmdContextId);
			if (this._sessnTm != null)
			{
				this._sessnTm.RobustConnectionsInitiated -= this.HandleRobustConnectionsIntiated;
				this._sessnTm.RobustConnectionsCompleted -= this.HandleRobusConnectionsCompleted;
			}
			if (this.closeCmdCompleted != null)
			{
				this.closeCmdCompleted.Dispose();
				this.closeCmdCompleted = null;
			}
			if (this.reconnectCmdCompleted != null)
			{
				this.reconnectCmdCompleted.Dispose();
				this.reconnectCmdCompleted = null;
			}
			this.wsManCmdOperationHandle = IntPtr.Zero;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000F8613 File Offset: 0x000F6813
		private static long GetNextCmdTMHandleId()
		{
			return Interlocked.Increment(ref WSManClientCommandTransportManager.CmdTMSeed);
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000F8620 File Offset: 0x000F6820
		private static void AddCmdTransportManager(long cmdTMId, WSManClientCommandTransportManager cmdTransportManager)
		{
			lock (WSManClientCommandTransportManager.CmdTMHandles)
			{
				WSManClientCommandTransportManager.CmdTMHandles.Add(cmdTMId, cmdTransportManager);
			}
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000F8668 File Offset: 0x000F6868
		private static void RemoveCmdTransportManager(long cmdTMId)
		{
			lock (WSManClientCommandTransportManager.CmdTMHandles)
			{
				if (WSManClientCommandTransportManager.CmdTMHandles.ContainsKey(cmdTMId))
				{
					WSManClientCommandTransportManager.CmdTMHandles[cmdTMId] = null;
					WSManClientCommandTransportManager.CmdTMHandles.Remove(cmdTMId);
				}
			}
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000F86C8 File Offset: 0x000F68C8
		private static bool TryGetCmdTransportManager(IntPtr operationContext, out WSManClientCommandTransportManager cmdTransportManager, out long cmdTMId)
		{
			cmdTMId = operationContext.ToInt64();
			cmdTransportManager = null;
			bool result;
			lock (WSManClientCommandTransportManager.CmdTMHandles)
			{
				result = WSManClientCommandTransportManager.CmdTMHandles.TryGetValue(cmdTMId, out cmdTransportManager);
			}
			return result;
		}

		// Token: 0x0400165C RID: 5724
		internal const string StopSignal = "powershell/signal/crtl_c";

		// Token: 0x0400165D RID: 5725
		private IntPtr wsManShellOperationHandle;

		// Token: 0x0400165E RID: 5726
		private IntPtr wsManCmdOperationHandle;

		// Token: 0x0400165F RID: 5727
		private IntPtr cmdSignalOperationHandle;

		// Token: 0x04001660 RID: 5728
		private IntPtr wsManRecieveOperationHandle;

		// Token: 0x04001661 RID: 5729
		private IntPtr wsManSendOperationHandle;

		// Token: 0x04001662 RID: 5730
		private long cmdContextId;

		// Token: 0x04001663 RID: 5731
		private PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableToSendCallback;

		// Token: 0x04001664 RID: 5732
		private bool shouldStartReceivingData;

		// Token: 0x04001665 RID: 5733
		private bool isCreateCallbackReceived;

		// Token: 0x04001666 RID: 5734
		private bool isStopSignalPending;

		// Token: 0x04001667 RID: 5735
		private bool isDisconnectPending;

		// Token: 0x04001668 RID: 5736
		private bool isSendingInput;

		// Token: 0x04001669 RID: 5737
		private bool isDisconnectedOnInvoke;

		// Token: 0x0400166A RID: 5738
		private WSManNativeApi.WSManShellAsync createCmdCompleted;

		// Token: 0x0400166B RID: 5739
		private WSManNativeApi.WSManShellAsync receivedFromRemote;

		// Token: 0x0400166C RID: 5740
		private WSManNativeApi.WSManShellAsync sendToRemoteCompleted;

		// Token: 0x0400166D RID: 5741
		private WSManNativeApi.WSManShellAsync reconnectCmdCompleted;

		// Token: 0x0400166E RID: 5742
		private WSManNativeApi.WSManShellAsync connectCmdCompleted;

		// Token: 0x0400166F RID: 5743
		private GCHandle createCmdCompletedGCHandle;

		// Token: 0x04001670 RID: 5744
		private WSManNativeApi.WSManShellAsync closeCmdCompleted;

		// Token: 0x04001671 RID: 5745
		private WSManNativeApi.WSManShellAsync signalCmdCompleted;

		// Token: 0x04001672 RID: 5746
		private WSManClientCommandTransportManager.SendDataChunk chunkToSend;

		// Token: 0x04001673 RID: 5747
		private string cmdLine;

		// Token: 0x04001674 RID: 5748
		private readonly WSManClientSessionTransportManager _sessnTm;

		// Token: 0x04001675 RID: 5749
		private static WSManNativeApi.WSManShellAsyncCallback cmdCreateCallback;

		// Token: 0x04001676 RID: 5750
		private static WSManNativeApi.WSManShellAsyncCallback cmdCloseCallback;

		// Token: 0x04001677 RID: 5751
		private static WSManNativeApi.WSManShellAsyncCallback cmdReceiveCallback;

		// Token: 0x04001678 RID: 5752
		private static WSManNativeApi.WSManShellAsyncCallback cmdSendCallback;

		// Token: 0x04001679 RID: 5753
		private static WSManNativeApi.WSManShellAsyncCallback cmdSignalCallback;

		// Token: 0x0400167A RID: 5754
		private static WSManNativeApi.WSManShellAsyncCallback cmdReconnectCallback;

		// Token: 0x0400167B RID: 5755
		private static WSManNativeApi.WSManShellAsyncCallback cmdConnectCallback;

		// Token: 0x0400167C RID: 5756
		private static Delegate commandCodeSendRedirect = null;

		// Token: 0x0400167D RID: 5757
		private static Delegate commandSendRedirect = null;

		// Token: 0x0400167E RID: 5758
		private static Dictionary<long, WSManClientCommandTransportManager> CmdTMHandles = new Dictionary<long, WSManClientCommandTransportManager>();

		// Token: 0x0400167F RID: 5759
		private static long CmdTMSeed;

		// Token: 0x02000395 RID: 917
		private class SendDataChunk
		{
			// Token: 0x06002C6D RID: 11373 RVA: 0x000F871C File Offset: 0x000F691C
			public SendDataChunk(byte[] data, DataPriorityType type)
			{
				this.data = data;
				this.type = type;
			}

			// Token: 0x17000A80 RID: 2688
			// (get) Token: 0x06002C6E RID: 11374 RVA: 0x000F8732 File Offset: 0x000F6932
			public byte[] Data
			{
				get
				{
					return this.data;
				}
			}

			// Token: 0x17000A81 RID: 2689
			// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000F873A File Offset: 0x000F693A
			public DataPriorityType Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x04001680 RID: 5760
			private byte[] data;

			// Token: 0x04001681 RID: 5761
			private DataPriorityType type;
		}
	}
}
