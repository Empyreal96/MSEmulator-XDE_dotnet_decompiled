using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000296 RID: 662
	internal class ClientRemoteSessionDSHandlerStateMachine
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x000B79C4 File Offset: 0x000B5BC4
		private void ProcessEvents()
		{
			RemoteSessionStateMachineEventArgs arg = null;
			do
			{
				lock (this.syncObject)
				{
					if (this.processPendingEventsQueue.Count == 0)
					{
						this.eventsInProcess = false;
						break;
					}
					arg = this.processPendingEventsQueue.Dequeue();
				}
				try
				{
					this.RaiseEventPrivate(arg);
				}
				catch (Exception ex)
				{
					this.HandleFatalError(ex);
				}
				try
				{
					this.RaiseStateMachineEvents();
				}
				catch (Exception ex2)
				{
					this.HandleFatalError(ex2);
				}
			}
			while (this.eventsInProcess);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000B7A6C File Offset: 0x000B5C6C
		private void HandleFatalError(Exception ex)
		{
			PSRemotingDataStructureException reason = new PSRemotingDataStructureException(ex, RemotingErrorIdStrings.FatalErrorCausingClose, new object[0]);
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close, reason);
			this.RaiseEvent(arg, true);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000B7A9C File Offset: 0x000B5C9C
		private void RaiseStateMachineEvents()
		{
			while (this._clientRemoteSessionStateChangeQueue.Count > 0)
			{
				RemoteSessionStateEventArgs eventArgs = this._clientRemoteSessionStateChangeQueue.Dequeue();
				this.StateChanged.SafeInvoke(this, eventArgs);
			}
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000B7AD4 File Offset: 0x000B5CD4
		private void SetStateHandler(object sender, RemoteSessionStateMachineEventArgs eventArgs)
		{
			RemoteSessionEvent stateEvent = eventArgs.StateEvent;
			switch (stateEvent)
			{
			case RemoteSessionEvent.NegotiationSendCompleted:
				this.SetState(RemoteSessionState.NegotiationSent, null);
				return;
			case RemoteSessionEvent.NegotiationReceived:
				if (eventArgs.RemoteSessionCapability == null)
				{
					throw PSTraceSource.NewArgumentException("eventArgs");
				}
				this.SetState(RemoteSessionState.NegotiationReceived, null);
				return;
			case RemoteSessionEvent.NegotiationCompleted:
				this.SetState(RemoteSessionState.Established, null);
				return;
			case RemoteSessionEvent.NegotiationPending:
			case RemoteSessionEvent.Close:
				break;
			case RemoteSessionEvent.CloseCompleted:
				this.SetState(RemoteSessionState.Closed, eventArgs.Reason);
				return;
			case RemoteSessionEvent.CloseFailed:
				this.SetState(RemoteSessionState.Closed, eventArgs.Reason);
				return;
			case RemoteSessionEvent.ConnectFailed:
				this.SetState(RemoteSessionState.ClosingConnection, eventArgs.Reason);
				return;
			default:
				switch (stateEvent)
				{
				case RemoteSessionEvent.KeySent:
					if (this._state == RemoteSessionState.Established || this._state == RemoteSessionState.EstablishedAndKeyRequested)
					{
						this.SetState(RemoteSessionState.EstablishedAndKeySent, eventArgs.Reason);
						this._keyExchangeTimer = new Timer(new TimerCallback(this.HandleKeyExchangeTimeout), null, 180000, -1);
						return;
					}
					break;
				case RemoteSessionEvent.KeySendFailed:
				case RemoteSessionEvent.KeyReceiveFailed:
				case RemoteSessionEvent.KeyRequestFailed:
				case RemoteSessionEvent.DisconnectStart:
				case RemoteSessionEvent.ReconnectStart:
					break;
				case RemoteSessionEvent.KeyReceived:
					if (this._state == RemoteSessionState.EstablishedAndKeySent)
					{
						Timer timer = Interlocked.Exchange<Timer>(ref this._keyExchangeTimer, null);
						if (timer != null)
						{
							timer.Dispose();
						}
						this.keyExchanged = true;
						this.SetState(RemoteSessionState.Established, eventArgs.Reason);
						if (this.pendingDisconnect)
						{
							this.pendingDisconnect = false;
							this.DoDisconnect(sender, eventArgs);
							return;
						}
					}
					break;
				case RemoteSessionEvent.KeyRequested:
					if (this._state == RemoteSessionState.Established)
					{
						this.SetState(RemoteSessionState.EstablishedAndKeyRequested, eventArgs.Reason);
						return;
					}
					break;
				case RemoteSessionEvent.DisconnectCompleted:
					if (this._state == RemoteSessionState.Disconnecting || this._state == RemoteSessionState.RCDisconnecting)
					{
						this.SetState(RemoteSessionState.Disconnected, eventArgs.Reason);
						return;
					}
					break;
				case RemoteSessionEvent.DisconnectFailed:
					if (this._state == RemoteSessionState.Disconnecting)
					{
						this.SetState(RemoteSessionState.Disconnected, eventArgs.Reason);
						return;
					}
					break;
				case RemoteSessionEvent.ReconnectCompleted:
					if (this._state == RemoteSessionState.Reconnecting)
					{
						this.SetState(RemoteSessionState.Established, eventArgs.Reason);
					}
					break;
				default:
					return;
				}
				break;
			}
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000B7CAC File Offset: 0x000B5EAC
		private void HandleKeyExchangeTimeout(object sender)
		{
			Timer timer = Interlocked.Exchange<Timer>(ref this._keyExchangeTimer, null);
			if (timer != null)
			{
				timer.Dispose();
			}
			PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientKeyExchangeFailed);
			this.RaiseEvent(new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceiveFailed, reason), false);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000B7CE9 File Offset: 0x000B5EE9
		private void SetStateToClosedHandler(object sender, RemoteSessionStateMachineEventArgs eventArgs)
		{
			if (eventArgs.StateEvent == RemoteSessionEvent.NegotiationTimeout && this.State == RemoteSessionState.Established)
			{
				return;
			}
			this.RaiseEvent(new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close, eventArgs.Reason), false);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000B7D14 File Offset: 0x000B5F14
		internal ClientRemoteSessionDSHandlerStateMachine()
		{
			this._clientRemoteSessionStateChangeQueue = new Queue<RemoteSessionStateEventArgs>();
			this._stateMachineHandle = new EventHandler<RemoteSessionStateMachineEventArgs>[20, 32];
			for (int i = 0; i < this._stateMachineHandle.GetLength(0); i++)
			{
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle;
				IntPtr intPtr;
				(stateMachineHandle = this._stateMachineHandle)[(int)(intPtr = (IntPtr)i), 17] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle[(int)intPtr, 17], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoFatal));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle2;
				IntPtr intPtr2;
				(stateMachineHandle2 = this._stateMachineHandle)[(int)(intPtr2 = (IntPtr)i), 9] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle2[(int)intPtr2, 9], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoClose));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle3;
				IntPtr intPtr3;
				(stateMachineHandle3 = this._stateMachineHandle)[(int)(intPtr3 = (IntPtr)i), 11] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle3[(int)intPtr3, 11], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle4;
				IntPtr intPtr4;
				(stateMachineHandle4 = this._stateMachineHandle)[(int)(intPtr4 = (IntPtr)i), 10] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle4[(int)intPtr4, 10], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle5;
				IntPtr intPtr5;
				(stateMachineHandle5 = this._stateMachineHandle)[(int)(intPtr5 = (IntPtr)i), 14] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle5[(int)intPtr5, 14], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle6;
				IntPtr intPtr6;
				(stateMachineHandle6 = this._stateMachineHandle)[(int)(intPtr6 = (IntPtr)i), 15] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle6[(int)intPtr6, 15], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle7;
				IntPtr intPtr7;
				(stateMachineHandle7 = this._stateMachineHandle)[(int)(intPtr7 = (IntPtr)i), 16] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle7[(int)intPtr7, 16], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle8;
				IntPtr intPtr8;
				(stateMachineHandle8 = this._stateMachineHandle)[(int)(intPtr8 = (IntPtr)i), 1] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle8[(int)intPtr8, 1], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoCreateSession));
				EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle9;
				IntPtr intPtr9;
				(stateMachineHandle9 = this._stateMachineHandle)[(int)(intPtr9 = (IntPtr)i), 2] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle9[(int)intPtr9, 2], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoConnectSession));
			}
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle10;
			(stateMachineHandle10 = this._stateMachineHandle)[1, 3] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle10[1, 3], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationSending));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle11;
			(stateMachineHandle11 = this._stateMachineHandle)[1, 4] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle11[1, 4], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoNegotiationSending));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle12;
			(stateMachineHandle12 = this._stateMachineHandle)[4, 5] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle12[4, 5], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle13;
			(stateMachineHandle13 = this._stateMachineHandle)[5, 5] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle13[5, 5], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle14;
			(stateMachineHandle14 = this._stateMachineHandle)[6, 6] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle14[6, 6], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle15;
			(stateMachineHandle15 = this._stateMachineHandle)[7, 7] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle15[7, 7], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle16;
			(stateMachineHandle16 = this._stateMachineHandle)[7, 13] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle16[7, 13], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle17;
			(stateMachineHandle17 = this._stateMachineHandle)[2, 12] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle17[2, 12], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle18;
			(stateMachineHandle18 = this._stateMachineHandle)[9, 10] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle18[9, 10], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle19;
			(stateMachineHandle19 = this._stateMachineHandle)[11, 25] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle19[11, 25], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoDisconnect));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle20;
			(stateMachineHandle20 = this._stateMachineHandle)[16, 26] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle20[16, 26], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle21;
			(stateMachineHandle21 = this._stateMachineHandle)[16, 27] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle21[16, 27], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle22;
			(stateMachineHandle22 = this._stateMachineHandle)[17, 28] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle22[17, 28], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoReconnect));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle23;
			(stateMachineHandle23 = this._stateMachineHandle)[18, 29] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle23[18, 29], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle24;
			(stateMachineHandle24 = this._stateMachineHandle)[18, 30] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle24[18, 30], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle25;
			(stateMachineHandle25 = this._stateMachineHandle)[16, 31] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle25[16, 31], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoRCDisconnectStarted));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle26;
			(stateMachineHandle26 = this._stateMachineHandle)[17, 31] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle26[17, 31], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoRCDisconnectStarted));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle27;
			(stateMachineHandle27 = this._stateMachineHandle)[11, 31] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle27[11, 31], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoRCDisconnectStarted));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle28;
			(stateMachineHandle28 = this._stateMachineHandle)[19, 26] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle28[19, 26], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle29;
			(stateMachineHandle29 = this._stateMachineHandle)[12, 25] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle29[12, 25], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoDisconnectDuringKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle30;
			(stateMachineHandle30 = this._stateMachineHandle)[14, 25] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle30[14, 25], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoDisconnectDuringKeyExchange));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle31;
			(stateMachineHandle31 = this._stateMachineHandle)[11, 23] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle31[11, 23], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle32;
			(stateMachineHandle32 = this._stateMachineHandle)[11, 19] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle32[11, 19], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle33;
			(stateMachineHandle33 = this._stateMachineHandle)[11, 20] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle33[11, 20], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle34;
			(stateMachineHandle34 = this._stateMachineHandle)[12, 21] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle34[12, 21], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle35;
			(stateMachineHandle35 = this._stateMachineHandle)[14, 19] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle35[14, 19], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle36;
			(stateMachineHandle36 = this._stateMachineHandle)[12, 22] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle36[12, 22], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
			EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle37;
			(stateMachineHandle37 = this._stateMachineHandle)[14, 20] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle37[14, 20], new EventHandler<RemoteSessionStateMachineEventArgs>(this.SetStateToClosedHandler));
			for (int j = 0; j < this._stateMachineHandle.GetLength(0); j++)
			{
				for (int k = 0; k < this._stateMachineHandle.GetLength(1); k++)
				{
					if (this._stateMachineHandle[j, k] == null)
					{
						EventHandler<RemoteSessionStateMachineEventArgs>[,] stateMachineHandle38;
						IntPtr intPtr10;
						IntPtr intPtr11;
						(stateMachineHandle38 = this._stateMachineHandle)[(int)(intPtr10 = (IntPtr)j), (int)(intPtr11 = (IntPtr)k)] = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(stateMachineHandle38[(int)intPtr10, (int)intPtr11], new EventHandler<RemoteSessionStateMachineEventArgs>(this.DoClose));
					}
				}
			}
			this.id = Guid.NewGuid();
			this.SetState(RemoteSessionState.Idle, null);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000B854C File Offset: 0x000B674C
		internal bool CanByPassRaiseEvent(RemoteSessionStateMachineEventArgs arg)
		{
			return arg.StateEvent == RemoteSessionEvent.MessageReceived && (this._state == RemoteSessionState.Established || this._state == RemoteSessionState.EstablishedAndKeyReceived || this._state == RemoteSessionState.EstablishedAndKeySent || this._state == RemoteSessionState.Disconnecting || this._state == RemoteSessionState.Disconnected);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000B8598 File Offset: 0x000B6798
		internal void RaiseEvent(RemoteSessionStateMachineEventArgs arg, bool clearQueuedEvents = false)
		{
			lock (this.syncObject)
			{
				ClientRemoteSessionDSHandlerStateMachine._trace.WriteLine("Event recieved : {0} for {1}", new object[]
				{
					arg.StateEvent,
					this.id
				});
				if (clearQueuedEvents)
				{
					this.processPendingEventsQueue.Clear();
				}
				this.processPendingEventsQueue.Enqueue(arg);
				if (this.eventsInProcess)
				{
					return;
				}
				this.eventsInProcess = true;
			}
			this.ProcessEvents();
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000B8638 File Offset: 0x000B6838
		private void RaiseEventPrivate(RemoteSessionStateMachineEventArgs arg)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException("arg");
			}
			EventHandler<RemoteSessionStateMachineEventArgs> eventHandler = this._stateMachineHandle[(int)this.State, (int)arg.StateEvent];
			if (eventHandler != null)
			{
				ClientRemoteSessionDSHandlerStateMachine._trace.WriteLine("Before calling state machine event handler: state = {0}, event = {1}, id = {2}", new object[]
				{
					this.State,
					arg.StateEvent,
					this.id
				});
				eventHandler(this, arg);
				ClientRemoteSessionDSHandlerStateMachine._trace.WriteLine("After calling state machine event handler: state = {0}, event = {1}, id = {2}", new object[]
				{
					this.State,
					arg.StateEvent,
					this.id
				});
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x000B86FB File Offset: 0x000B68FB
		internal RemoteSessionState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06001FBE RID: 8126 RVA: 0x000B8704 File Offset: 0x000B6904
		// (remove) Token: 0x06001FBF RID: 8127 RVA: 0x000B873C File Offset: 0x000B693C
		internal event EventHandler<RemoteSessionStateEventArgs> StateChanged;

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000B8774 File Offset: 0x000B6974
		private void DoCreateSession(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			using (ClientRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (this.State == RemoteSessionState.Idle)
				{
					RemoteSessionStateMachineEventArgs arg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSending);
					this.RaiseEvent(arg2, false);
				}
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000B87C0 File Offset: 0x000B69C0
		private void DoConnectSession(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			using (ClientRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
				if (this.State == RemoteSessionState.Idle)
				{
					RemoteSessionStateMachineEventArgs arg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSendingOnConnect);
					this.RaiseEvent(arg2, false);
				}
			}
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000B880C File Offset: 0x000B6A0C
		private void DoNegotiationSending(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			if (arg.StateEvent == RemoteSessionEvent.NegotiationSending)
			{
				this.SetState(RemoteSessionState.NegotiationSending, null);
				return;
			}
			if (arg.StateEvent == RemoteSessionEvent.NegotiationSendingOnConnect)
			{
				this.SetState(RemoteSessionState.NegotiationSendingOnConnect, null);
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000B8831 File Offset: 0x000B6A31
		private void DoDisconnectDuringKeyExchange(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			this.pendingDisconnect = true;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000B883A File Offset: 0x000B6A3A
		private void DoDisconnect(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			this.SetState(RemoteSessionState.Disconnecting, null);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000B8845 File Offset: 0x000B6A45
		private void DoReconnect(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			this.SetState(RemoteSessionState.Reconnecting, null);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000B8850 File Offset: 0x000B6A50
		private void DoRCDisconnectStarted(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			if (this.State != RemoteSessionState.Disconnecting && this.State != RemoteSessionState.Disconnected)
			{
				this.SetState(RemoteSessionState.RCDisconnecting, null);
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000B8870 File Offset: 0x000B6A70
		private void DoClose(object sender, RemoteSessionStateMachineEventArgs arg)
		{
			using (ClientRemoteSessionDSHandlerStateMachine._trace.TraceEventHandlers())
			{
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
				case RemoteSessionState.Disconnecting:
				case RemoteSessionState.Disconnected:
				case RemoteSessionState.Reconnecting:
				case RemoteSessionState.RCDisconnecting:
					this.SetState(RemoteSessionState.ClosingConnection, arg.Reason);
					goto IL_9C;
				case RemoteSessionState.ClosingConnection:
				case RemoteSessionState.Closed:
					goto IL_9C;
				}
				PSRemotingTransportException reason = new PSRemotingTransportException(arg.Reason, RemotingErrorIdStrings.ForceClosed, new object[0]);
				this.SetState(RemoteSessionState.Closed, reason);
				IL_9C:
				this.CleanAll();
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000B893C File Offset: 0x000B6B3C
		private void DoFatal(object sender, RemoteSessionStateMachineEventArgs eventArgs)
		{
			PSRemotingDataStructureException reason = new PSRemotingDataStructureException(eventArgs.Reason, RemotingErrorIdStrings.FatalErrorCausingClose, new object[0]);
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close, reason);
			this.RaiseEvent(arg, false);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000B8971 File Offset: 0x000B6B71
		private void CleanAll()
		{
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000B8974 File Offset: 0x000B6B74
		private void SetState(RemoteSessionState newState, Exception reason)
		{
			RemoteSessionState state = this._state;
			if (newState != state)
			{
				this._state = newState;
				ClientRemoteSessionDSHandlerStateMachine._trace.WriteLine("state machine state transition: from state {0} to state {1}", new object[]
				{
					state,
					this._state
				});
				RemoteSessionStateInfo remoteSessionStateInfo = new RemoteSessionStateInfo(this._state, reason);
				RemoteSessionStateEventArgs item = new RemoteSessionStateEventArgs(remoteSessionStateInfo);
				this._clientRemoteSessionStateChangeQueue.Enqueue(item);
			}
		}

		// Token: 0x04000E03 RID: 3587
		[TraceSource("CRSessionFSM", "CRSessionFSM")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("CRSessionFSM", "CRSessionFSM");

		// Token: 0x04000E04 RID: 3588
		private EventHandler<RemoteSessionStateMachineEventArgs>[,] _stateMachineHandle;

		// Token: 0x04000E05 RID: 3589
		private Queue<RemoteSessionStateEventArgs> _clientRemoteSessionStateChangeQueue;

		// Token: 0x04000E06 RID: 3590
		private RemoteSessionState _state;

		// Token: 0x04000E07 RID: 3591
		private Queue<RemoteSessionStateMachineEventArgs> processPendingEventsQueue = new Queue<RemoteSessionStateMachineEventArgs>();

		// Token: 0x04000E08 RID: 3592
		private object syncObject = new object();

		// Token: 0x04000E09 RID: 3593
		private bool eventsInProcess;

		// Token: 0x04000E0A RID: 3594
		private Timer _keyExchangeTimer;

		// Token: 0x04000E0B RID: 3595
		private bool keyExchanged;

		// Token: 0x04000E0C RID: 3596
		private bool pendingDisconnect;

		// Token: 0x04000E0D RID: 3597
		private Guid id;
	}
}
