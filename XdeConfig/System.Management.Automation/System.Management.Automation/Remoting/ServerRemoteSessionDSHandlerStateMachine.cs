using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002FF RID: 767
	internal class ServerRemoteSessionDSHandlerStateMachine
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x000CB2B4 File Offset: 0x000C94B4
		internal ServerRemoteSessionDSHandlerStateMachine(ServerRemoteSession session)
		{
			if (session == null)
			{
				throw PSTraceSource.NewArgumentNullException("session");
			}
			this._session = session;
			this._syncObject = new object();
			this._stateMachineHandle = new EventHandler<RemoteSessionStateMachineEventArgs>[20, 32];
			for (int i = 0; i < this._stateMachineHandle.GetLength(0); i++)
			{
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle;
				IntPtr intPtr;
				(stateMachineHandle = this._stateMachineHandle)[(int)(intPtr = (IntPtr)i), 17] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle[(int)intPtr, 17], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoFatalError));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle2;
				IntPtr intPtr2;
				(stateMachineHandle2 = this._stateMachineHandle)[(int)(intPtr2 = (IntPtr)i), 9] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle2[(int)intPtr2, 9], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoClose));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle3;
				IntPtr intPtr3;
				(stateMachineHandle3 = this._stateMachineHandle)[(int)(intPtr3 = (IntPtr)i), 11] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle3[(int)intPtr3, 11], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoCloseFailed));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle4;
				IntPtr intPtr4;
				(stateMachineHandle4 = this._stateMachineHandle)[(int)(intPtr4 = (IntPtr)i), 10] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle4[(int)intPtr4, 10], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoCloseCompleted));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle5;
				IntPtr intPtr5;
				(stateMachineHandle5 = this._stateMachineHandle)[(int)(intPtr5 = (IntPtr)i), 14] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle5[(int)intPtr5, 14], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationTimeout));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle6;
				IntPtr intPtr6;
				(stateMachineHandle6 = this._stateMachineHandle)[(int)(intPtr6 = (IntPtr)i), 15] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle6[(int)intPtr6, 15], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoSendFailed));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle7;
				IntPtr intPtr7;
				(stateMachineHandle7 = this._stateMachineHandle)[(int)(intPtr7 = (IntPtr)i), 16] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle7[(int)intPtr7, 16], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoReceiveFailed));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle8;
				IntPtr intPtr8;
				(stateMachineHandle8 = this._stateMachineHandle)[(int)(intPtr8 = (IntPtr)i), 2] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle8[(int)intPtr8, 2], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoConnect));
			}
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle9;
			(stateMachineHandle9 = this._stateMachineHandle)[1, 1] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle9[1, 1], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoCreateSession));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle10;
			(stateMachineHandle10 = this._stateMachineHandle)[8, 6] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle10[8, 6], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationReceived));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle11;
			(stateMachineHandle11 = this._stateMachineHandle)[7, 3] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle11[7, 3], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationSending));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle12;
			(stateMachineHandle12 = this._stateMachineHandle)[4, 5] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle12[4, 5], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationCompleted));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle13;
			(stateMachineHandle13 = this._stateMachineHandle)[6, 7] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle13[6, 7], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoEstablished));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle14;
			(stateMachineHandle14 = this._stateMachineHandle)[6, 8] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle14[6, 8], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationPending));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle15;
			(stateMachineHandle15 = this._stateMachineHandle)[11, 18] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle15[11, 18], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoMessageReceived));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle16;
			(stateMachineHandle16 = this._stateMachineHandle)[7, 13] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle16[7, 13], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationFailed));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle17;
			(stateMachineHandle17 = this._stateMachineHandle)[2, 12] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle17[2, 12], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoConnectFailed));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle18;
			(stateMachineHandle18 = this._stateMachineHandle)[11, 21] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle18[11, 21], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle19;
			(stateMachineHandle19 = this._stateMachineHandle)[11, 23] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle19[11, 23], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle20;
			(stateMachineHandle20 = this._stateMachineHandle)[11, 22] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle20[11, 22], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle21;
			(stateMachineHandle21 = this._stateMachineHandle)[14, 21] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle21[14, 21], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle22;
			(stateMachineHandle22 = this._stateMachineHandle)[14, 19] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle22[14, 19], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle23;
			(stateMachineHandle23 = this._stateMachineHandle)[14, 22] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle23[14, 22], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle24;
			(stateMachineHandle24 = this._stateMachineHandle)[13, 20] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle24[13, 20], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle25;
			(stateMachineHandle25 = this._stateMachineHandle)[13, 19] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle25[13, 19], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle26;
			(stateMachineHandle26 = this._stateMachineHandle)[15, 21] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle26[15, 21], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle27;
			(stateMachineHandle27 = this._stateMachineHandle)[15, 23] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle27[15, 23], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle28;
			(stateMachineHandle28 = this._stateMachineHandle)[15, 22] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle28[15, 22], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoKeyExchange));
			for (int j = 0; j < this._stateMachineHandle.GetLength(0); j++)
			{
				for (int k = 0; k < this._stateMachineHandle.GetLength(1); k++)
				{
					if (this._stateMachineHandle[j, k] == null)
					{
						EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle29;
						IntPtr intPtr9;
						IntPtr intPtr10;
						(stateMachineHandle29 = this._stateMachineHandle)[(int)(intPtr9 = (IntPtr)j), (int)(intPtr10 = (IntPtr)k)] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle29[(int)intPtr9, (int)intPtr10], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoClose));
					}
				}
			}
			this.SetState(RemoteSessionState.Idle, null);
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x000CB91E File Offset: 0x000C9B1E
		internal RemoteSessionState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000CB926 File Offset: 0x000C9B26
		internal bool CanByPassRaiseEvent(RemoteSessionStateMachineEventArgs arg)
		{
			return arg.StateEvent == RemoteSessionEvent.MessageReceived && (this._state == RemoteSessionState.Established || this._state == RemoteSessionState.EstablishedAndKeySent || this._state == RemoteSessionState.EstablishedAndKeyReceived || this._state == RemoteSessionState.EstablishedAndKeyExchanged);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000CB960 File Offset: 0x000C9B60
		internal void RaiseEvent(RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			lock (this._syncObject)
			{
				ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("Event received : {0}", new object[]
				{
					fsmEventArg.StateEvent
				});
				this.processPendingEventsQueue.Enqueue(fsmEventArg);
				if (this.eventsInProcess)
				{
					return;
				}
				this.eventsInProcess = true;
			}
			this.ProcessEvents();
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000CB9E4 File Offset: 0x000C9BE4
		private void ProcessEvents()
		{
			RemoteSessionStateMachineEventArgs fsmEventArg = null;
			do
			{
				lock (this._syncObject)
				{
					if (this.processPendingEventsQueue.Count == 0)
					{
						this.eventsInProcess = false;
						break;
					}
					fsmEventArg = this.processPendingEventsQueue.Dequeue();
				}
				this.RaiseEventPrivate(fsmEventArg);
			}
			while (this.eventsInProcess);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000CBA54 File Offset: 0x000C9C54
		private void RaiseEventPrivate(RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			if (fsmEventArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("fsmEventArg");
			}
			EventHandler<RemoteSessionStateMachineEventArgs> eventHandler = this._stateMachineHandle[(int)this._state, (int)fsmEventArg.StateEvent];
			if (eventHandler != null)
			{
				ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("Before calling state machine event handler: state = {0}, event = {1}", new object[]
				{
					this._state,
					fsmEventArg.StateEvent
				});
				eventHandler(this, fsmEventArg);
				ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("After calling state machine event handler: state = {0}, event = {1}", new object[]
				{
					this._state,
					fsmEventArg.StateEvent
				});
			}
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000CBAF8 File Offset: 0x000C9CF8
		private void DoCreateSession(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				this.DoNegotiationPending(sender, fsmEventArg);
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000CBB44 File Offset: 0x000C9D44
		private void DoNegotiationPending(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				this.SetState(RemoteSessionState.NegotiationPending, null);
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000CBB90 File Offset: 0x000C9D90
		private void DoNegotiationReceived(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (fsmEventArg.StateEvent != RemoteSessionEvent.NegotiationReceived)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				if (fsmEventArg.RemoteSessionCapability == null)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				this.SetState(RemoteSessionState.NegotiationReceived, null);
			}
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000CBC04 File Offset: 0x000C9E04
		private void DoNegotiationSending(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			if (fsmEventArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("fsmEventArg");
			}
			this.SetState(RemoteSessionState.NegotiationSending, null);
			this._session.SessionDataStructureHandler.SendNegotiationAsync();
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000CBC2C File Offset: 0x000C9E2C
		private void DoNegotiationCompleted(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				this.SetState(RemoteSessionState.NegotiationSent, null);
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000CBC78 File Offset: 0x000C9E78
		private void DoEstablished(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (fsmEventArg.StateEvent != RemoteSessionEvent.NegotiationCompleted)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				if (this._state != RemoteSessionState.NegotiationSent)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				this.SetState(RemoteSessionState.Established, null);
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000CBCE8 File Offset: 0x000C9EE8
		internal void DoMessageReceived(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (fsmEventArg.RemoteData == null)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				RemotingTargetInterface targetInterface = fsmEventArg.RemoteData.TargetInterface;
				RemotingDataType dataType = fsmEventArg.RemoteData.DataType;
				switch (targetInterface)
				{
				case RemotingTargetInterface.Session:
				{
					RemotingDataType remotingDataType = dataType;
					if (remotingDataType == RemotingDataType.CreateRunspacePool)
					{
						RemoteDataEventArgs arg = new RemoteDataEventArgs(fsmEventArg.RemoteData);
						this._session.SessionDataStructureHandler.RaiseDataReceivedEvent(arg);
					}
					break;
				}
				case RemotingTargetInterface.RunspacePool:
				{
					Guid runspacePoolId = fsmEventArg.RemoteData.RunspacePoolId;
					ServerRunspacePoolDriver runspacePoolDriver = this._session.GetRunspacePoolDriver(runspacePoolId);
					if (runspacePoolDriver != null)
					{
						runspacePoolDriver.DataStructureHandler.ProcessReceivedData(fsmEventArg.RemoteData);
					}
					else
					{
						ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("Server received data for Runspace (id: {0}), \r\n                                but the Runspace cannot be found", new object[]
						{
							runspacePoolId
						});
						PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.RunspaceCannotBeFound, new object[]
						{
							runspacePoolId
						});
						RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.FatalError, reason);
						this.RaiseEvent(fsmEventArg2);
					}
					break;
				}
				case RemotingTargetInterface.PowerShell:
				{
					Guid runspacePoolId = fsmEventArg.RemoteData.RunspacePoolId;
					ServerRunspacePoolDriver runspacePoolDriver = this._session.GetRunspacePoolDriver(runspacePoolId);
					runspacePoolDriver.DataStructureHandler.DispatchMessageToPowerShell(fsmEventArg.RemoteData);
					break;
				}
				default:
				{
					ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("Server received data unknown targetInterface: {0}", new object[]
					{
						targetInterface
					});
					PSRemotingDataStructureException reason2 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ReceivedUnsupportedRemotingTargetInterfaceType, new object[]
					{
						targetInterface
					});
					RemoteSessionStateMachineEventArgs fsmEventArg3 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.FatalError, reason2);
					this.RaiseEvent(fsmEventArg3);
					break;
				}
				}
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000CBEC8 File Offset: 0x000CA0C8
		private void DoConnectFailed(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			IDisposable disposable = ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers();
			try
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (fsmEventArg.StateEvent != RemoteSessionEvent.ConnectFailed)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				throw PSTraceSource.NewInvalidOperationException();
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
					goto IL_3D;
				}
				goto IL_3D;
				IL_3D:;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000CBF24 File Offset: 0x000CA124
		private void DoFatalError(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (fsmEventArg.StateEvent != RemoteSessionEvent.FatalError)
				{
					throw PSTraceSource.NewArgumentException("fsmEventArg");
				}
				this.DoClose(this, fsmEventArg);
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000CBF84 File Offset: 0x000CA184
		private void DoConnect(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			if (this._state != RemoteSessionState.Closed && this._state != RemoteSessionState.ClosingConnection)
			{
				this._session.HandlePostConnect();
			}
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000CBFA8 File Offset: 0x000CA1A8
		private void DoClose(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				switch (this._state)
				{
				case RemoteSessionState.Connecting:
				case RemoteSessionState.Connected:
				case RemoteSessionState.NegotiationSending:
				case RemoteSessionState.NegotiationSent:
				case RemoteSessionState.NegotiationReceived:
				case RemoteSessionState.Established:
				case RemoteSessionState.EstablishedAndKeySent:
				case RemoteSessionState.EstablishedAndKeyReceived:
				case RemoteSessionState.EstablishedAndKeyExchanged:
					this.SetState(RemoteSessionState.ClosingConnection, fsmEventArg.Reason);
					this._session.SessionDataStructureHandler.CloseConnectionAsync(fsmEventArg.Reason);
					goto IL_B0;
				case RemoteSessionState.ClosingConnection:
				case RemoteSessionState.Closed:
					goto IL_B0;
				}
				Exception reasion = new PSRemotingTransportException(fsmEventArg.Reason, RemotingErrorIdStrings.ForceClosed, new object[0]);
				this.SetState(RemoteSessionState.Closed, reasion);
				IL_B0:
				this.CleanAll();
			}
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000CC088 File Offset: 0x000CA288
		private void DoCloseFailed(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				this.SetState(RemoteSessionState.Closed, fsmEventArg.Reason);
				this.CleanAll();
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000CC0E0 File Offset: 0x000CA2E0
		private void DoCloseCompleted(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				this.SetState(RemoteSessionState.Closed, fsmEventArg.Reason);
				this._session.Close(fsmEventArg);
				this.CleanAll();
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000CC144 File Offset: 0x000CA344
		private void DoNegotiationFailed(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
				this.RaiseEventPrivate(fsmEventArg2);
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000CC198 File Offset: 0x000CA398
		private void DoNegotiationTimeout(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				if (this._state != RemoteSessionState.Established)
				{
					RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
					this.RaiseEventPrivate(fsmEventArg2);
				}
			}
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000CC1F8 File Offset: 0x000CA3F8
		private void DoSendFailed(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
				this.RaiseEventPrivate(fsmEventArg2);
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000CC24C File Offset: 0x000CA44C
		private void DoReceiveFailed(object sender, RemoteSessionStateMachineEventArgs fsmEventArg)
		{
			using (ServerRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (fsmEventArg == null)
				{
					throw PSTraceSource.NewArgumentNullException("fsmEventArg");
				}
				RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
				this.RaiseEventPrivate(fsmEventArg2);
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000CC2A0 File Offset: 0x000CA4A0
		private void DoKeyExchange(object sender, RemoteSessionStateMachineEventArgs eventArgs)
		{
			switch (eventArgs.StateEvent)
			{
			case RemoteSessionEvent.KeySent:
				if (this._state == RemoteSessionState.EstablishedAndKeyReceived)
				{
					this.SetState(RemoteSessionState.EstablishedAndKeyExchanged, eventArgs.Reason);
					return;
				}
				break;
			case RemoteSessionEvent.KeySendFailed:
				this.DoClose(this, eventArgs);
				break;
			case RemoteSessionEvent.KeyReceived:
				if (this._state == RemoteSessionState.EstablishedAndKeyRequested)
				{
					Timer timer = Interlocked.Exchange<Timer>(ref this._keyExchangeTimer, null);
					if (timer != null)
					{
						timer.Dispose();
					}
				}
				this.SetState(RemoteSessionState.EstablishedAndKeyReceived, eventArgs.Reason);
				this._session.SendEncryptedSessionKey();
				return;
			case RemoteSessionEvent.KeyReceiveFailed:
				if (this._state == RemoteSessionState.Established || this._state == RemoteSessionState.EstablishedAndKeyExchanged)
				{
					return;
				}
				this.DoClose(this, eventArgs);
				return;
			case RemoteSessionEvent.KeyRequested:
				if (this._state == RemoteSessionState.Established || this._state == RemoteSessionState.EstablishedAndKeyExchanged)
				{
					this.SetState(RemoteSessionState.EstablishedAndKeyRequested, eventArgs.Reason);
					this._keyExchangeTimer = new Timer(new TimerCallback(this.HandleKeyExchangeTimeout), null, 240000, -1);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000CC38C File Offset: 0x000CA58C
		private void HandleKeyExchangeTimeout(object sender)
		{
			Timer timer = Interlocked.Exchange<Timer>(ref this._keyExchangeTimer, null);
			if (timer != null)
			{
				timer.Dispose();
			}
			PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerKeyExchangeFailed);
			this.RaiseEvent(new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceiveFailed, reason));
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000CC3C8 File Offset: 0x000CA5C8
		private void CleanAll()
		{
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000CC3CC File Offset: 0x000CA5CC
		private void SetState(RemoteSessionState newState, Exception reasion)
		{
			RemoteSessionState state = this._state;
			if (newState != state)
			{
				this._state = newState;
				ServerRemoteSessionDSHandlerStateMachine._trace.WriteLine("state machine state transition: from state {0} to state {1}", new object[]
				{
					state,
					this._state
				});
			}
		}

		// Token: 0x040011D0 RID: 4560
		[TraceSource("ServerRemoteSessionDSHandlerStateMachine", "ServerRemoteSessionDSHandlerStateMachine")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("ServerRemoteSessionDSHandlerStateMachine", "ServerRemoteSessionDSHandlerStateMachine");

		// Token: 0x040011D1 RID: 4561
		private ServerRemoteSession _session;

		// Token: 0x040011D2 RID: 4562
		private object _syncObject;

		// Token: 0x040011D3 RID: 4563
		private Queue<RemoteSessionStateMachineEventArgs> processPendingEventsQueue = new Queue<RemoteSessionStateMachineEventArgs>();

		// Token: 0x040011D4 RID: 4564
		private bool eventsInProcess;

		// Token: 0x040011D5 RID: 4565
		private EventHandler<RemoteSessionStateMachineEventArgs>[,] _stateMachineHandle;

		// Token: 0x040011D6 RID: 4566
		private RemoteSessionState _state;

		// Token: 0x040011D7 RID: 4567
		private Timer _keyExchangeTimer;
	}
}
