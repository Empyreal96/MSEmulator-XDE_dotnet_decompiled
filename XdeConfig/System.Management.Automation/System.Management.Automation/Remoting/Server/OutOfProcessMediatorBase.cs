using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Security.Principal;
using System.Threading;
using Microsoft.PowerShell;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002F5 RID: 757
	internal abstract class OutOfProcessMediatorBase
	{
		// Token: 0x060023E1 RID: 9185 RVA: 0x000C90D8 File Offset: 0x000C72D8
		protected OutOfProcessMediatorBase(bool exitProcessOnError)
		{
			this._exitProcessOnError = exitProcessOnError;
			this.callbacks = default(OutOfProcessUtils.DataProcessingDelegates);
			this.callbacks.DataPacketReceived = (OutOfProcessUtils.DataPacketReceived)Delegate.Combine(this.callbacks.DataPacketReceived, new OutOfProcessUtils.DataPacketReceived(this.OnDataPacketReceived));
			this.callbacks.DataAckPacketReceived = (OutOfProcessUtils.DataAckPacketReceived)Delegate.Combine(this.callbacks.DataAckPacketReceived, new OutOfProcessUtils.DataAckPacketReceived(this.OnDataAckPacketReceived));
			this.callbacks.CommandCreationPacketReceived = (OutOfProcessUtils.CommandCreationPacketReceived)Delegate.Combine(this.callbacks.CommandCreationPacketReceived, new OutOfProcessUtils.CommandCreationPacketReceived(this.OnCommandCreationPacketReceived));
			this.callbacks.CommandCreationAckReceived = (OutOfProcessUtils.CommandCreationAckReceived)Delegate.Combine(this.callbacks.CommandCreationAckReceived, new OutOfProcessUtils.CommandCreationAckReceived(this.OnCommandCreationAckReceived));
			this.callbacks.ClosePacketReceived = (OutOfProcessUtils.ClosePacketReceived)Delegate.Combine(this.callbacks.ClosePacketReceived, new OutOfProcessUtils.ClosePacketReceived(this.OnClosePacketReceived));
			this.callbacks.CloseAckPacketReceived = (OutOfProcessUtils.CloseAckPacketReceived)Delegate.Combine(this.callbacks.CloseAckPacketReceived, new OutOfProcessUtils.CloseAckPacketReceived(this.OnCloseAckPacketReceived));
			this.callbacks.SignalPacketReceived = (OutOfProcessUtils.SignalPacketReceived)Delegate.Combine(this.callbacks.SignalPacketReceived, new OutOfProcessUtils.SignalPacketReceived(this.OnSignalPacketReceived));
			this.callbacks.SignalAckPacketReceived = (OutOfProcessUtils.SignalAckPacketReceived)Delegate.Combine(this.callbacks.SignalAckPacketReceived, new OutOfProcessUtils.SignalAckPacketReceived(this.OnSignalAckPacketReceived));
			this.allcmdsClosedEvent = new ManualResetEvent(true);
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000C9258 File Offset: 0x000C7458
		protected void ProcessingThreadStart(object state)
		{
			try
			{
				Thread.CurrentThread.CurrentUICulture = NativeCultureResolver.UICulture;
				Thread.CurrentThread.CurrentCulture = NativeCultureResolver.Culture;
				string data = state as string;
				OutOfProcessUtils.ProcessData(data, this.callbacks);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Open, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
				{
					Guid.Empty.ToString(),
					Guid.Empty.ToString(),
					4000,
					ex.Message,
					ex.StackTrace
				});
				PSEtwLog.LogAnalyticError(PSEventId.TransportError_Analytic, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					Guid.Empty.ToString(),
					Guid.Empty.ToString(),
					4000,
					ex.Message,
					ex.StackTrace
				});
				if (this._exitProcessOnError)
				{
					this.originalStdErr.WriteLine(ex.Message + ex.StackTrace);
					Environment.Exit(4000);
				}
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000C93CC File Offset: 0x000C75CC
		protected void OnDataPacketReceived(byte[] rawData, string stream, Guid psGuid)
		{
			string stream2 = "stdin";
			if (stream.Equals(DataPriorityType.PromptResponse.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				stream2 = "pr";
			}
			if (Guid.Empty == psGuid)
			{
				lock (this._syncObject)
				{
					this.sessionTM.ProcessRawData(rawData, stream2);
					return;
				}
			}
			AbstractServerTransportManager abstractServerTransportManager = null;
			lock (this._syncObject)
			{
				abstractServerTransportManager = this.sessionTM.GetCommandTransportManager(psGuid);
			}
			if (abstractServerTransportManager != null)
			{
				abstractServerTransportManager.ProcessRawData(rawData, stream2);
				return;
			}
			this.originalStdOut.WriteLine(OutOfProcessUtils.CreateDataAckPacket(psGuid));
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000C949C File Offset: 0x000C769C
		protected void OnDataAckPacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"DataAck"
			});
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000C94C8 File Offset: 0x000C76C8
		protected void OnCommandCreationPacketReceived(Guid psGuid)
		{
			lock (this._syncObject)
			{
				this.sessionTM.CreateCommandTransportManager(psGuid);
				if (this._inProgressCommandsCount == 0)
				{
					this.allcmdsClosedEvent.Reset();
				}
				this._inProgressCommandsCount++;
				this.tracer.WriteMessage(string.Concat(new object[]
				{
					"OutOfProcessMediator.OnCommandCreationPacketReceived, in progress command count : ",
					this._inProgressCommandsCount,
					" psGuid : ",
					psGuid.ToString()
				}));
			}
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x000C9578 File Offset: 0x000C7778
		protected void OnCommandCreationAckReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"CommandAck"
			});
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000C95A4 File Offset: 0x000C77A4
		protected void OnSignalPacketReceived(Guid psGuid)
		{
			if (psGuid == Guid.Empty)
			{
				throw new PSRemotingTransportException(PSRemotingErrorId.IPCNoSignalForSession, RemotingErrorIdStrings.IPCNoSignalForSession, new object[]
				{
					"Signal"
				});
			}
			AbstractServerTransportManager abstractServerTransportManager = null;
			try
			{
				lock (this._syncObject)
				{
					abstractServerTransportManager = this.sessionTM.GetCommandTransportManager(psGuid);
				}
				if (abstractServerTransportManager != null)
				{
					abstractServerTransportManager.Close(null);
				}
			}
			finally
			{
				this.originalStdOut.WriteLine(OutOfProcessUtils.CreateSignalAckPacket(psGuid));
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000C9644 File Offset: 0x000C7844
		protected void OnSignalAckPacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"SignalAck"
			});
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000C9670 File Offset: 0x000C7870
		protected void OnClosePacketReceived(Guid psGuid)
		{
			PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource();
			if (psGuid == Guid.Empty)
			{
				traceSource.WriteMessage("BEGIN calling close on session transport manager");
				bool flag = false;
				lock (this._syncObject)
				{
					if (this._inProgressCommandsCount > 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.allcmdsClosedEvent.WaitOne();
				}
				lock (this._syncObject)
				{
					traceSource.WriteMessage(string.Concat(new object[]
					{
						"OnClosePacketReceived, in progress commands count should be zero : ",
						this._inProgressCommandsCount,
						", psGuid : ",
						psGuid.ToString()
					}));
					if (this.sessionTM != null)
					{
						this.sessionTM.Close(null);
					}
					traceSource.WriteMessage("END calling close on session transport manager");
					this.sessionTM = null;
					goto IL_1D5;
				}
			}
			traceSource.WriteMessage("Closing command with GUID " + psGuid.ToString());
			AbstractServerTransportManager abstractServerTransportManager = null;
			lock (this._syncObject)
			{
				abstractServerTransportManager = this.sessionTM.GetCommandTransportManager(psGuid);
			}
			if (abstractServerTransportManager != null)
			{
				abstractServerTransportManager.Close(null);
			}
			lock (this._syncObject)
			{
				traceSource.WriteMessage(string.Concat(new object[]
				{
					"OnClosePacketReceived, in progress commands count should be greater than zero : ",
					this._inProgressCommandsCount,
					", psGuid : ",
					psGuid.ToString()
				}));
				this._inProgressCommandsCount--;
				if (this._inProgressCommandsCount == 0)
				{
					this.allcmdsClosedEvent.Set();
				}
			}
			IL_1D5:
			this.originalStdOut.WriteLine(OutOfProcessUtils.CreateCloseAckPacket(psGuid));
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000C9898 File Offset: 0x000C7A98
		protected void OnCloseAckPacketReceived(Guid psGuid)
		{
			throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
			{
				"CloseAck"
			});
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000C98C4 File Offset: 0x000C7AC4
		protected OutOfProcessServerSessionTransportManager CreateSessionTransportManager()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			PSPrincipal userPrincipal = new PSPrincipal(new PSIdentity("", true, current.Name, null), current);
			PSSenderInfo senderInfo = new PSSenderInfo(userPrincipal, "http://localhost");
			OutOfProcessServerSessionTransportManager outOfProcessServerSessionTransportManager = new OutOfProcessServerSessionTransportManager(this.originalStdOut, this.originalStdErr);
			ServerRemoteSession.CreateServerRemoteSession(senderInfo, this._initialCommand, outOfProcessServerSessionTransportManager);
			return outOfProcessServerSessionTransportManager;
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000C9920 File Offset: 0x000C7B20
		protected void Start(string initialCommand)
		{
			this._initialCommand = initialCommand;
			this.sessionTM = this.CreateSessionTransportManager();
			try
			{
				for (;;)
				{
					string text = this.originalStdIn.ReadLine();
					lock (this._syncObject)
					{
						if (this.sessionTM == null)
						{
							this.sessionTM = this.CreateSessionTransportManager();
						}
					}
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					Utils.QueueWorkItemWithImpersonation(this._windowsIdentityToImpersonate, new WaitCallback(this.ProcessingThreadStart), text);
				}
				lock (this._syncObject)
				{
					this.sessionTM.Close(null);
					this.sessionTM = null;
				}
				throw new PSRemotingTransportException(PSRemotingErrorId.IPCUnknownElementReceived, RemotingErrorIdStrings.IPCUnknownElementReceived, new object[]
				{
					string.Empty
				});
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Open, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
				{
					Guid.Empty.ToString(),
					Guid.Empty.ToString(),
					4000,
					ex.Message,
					ex.StackTrace
				});
				PSEtwLog.LogAnalyticError(PSEventId.TransportError_Analytic, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					Guid.Empty.ToString(),
					Guid.Empty.ToString(),
					4000,
					ex.Message,
					ex.StackTrace
				});
				if (this._exitProcessOnError)
				{
					this.originalStdErr.WriteLine(ex.Message);
					Environment.Exit(4000);
				}
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000C9B58 File Offset: 0x000C7D58
		internal static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			PSEtwLog.LogOperationalError(PSEventId.AppDomainUnhandledException, PSOpcode.Close, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
			{
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace
			});
			PSEtwLog.LogAnalyticError(PSEventId.AppDomainUnhandledException_Analytic, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace
			});
		}

		// Token: 0x040011A3 RID: 4515
		protected TextReader originalStdIn;

		// Token: 0x040011A4 RID: 4516
		protected OutOfProcessTextWriter originalStdOut;

		// Token: 0x040011A5 RID: 4517
		protected OutOfProcessTextWriter originalStdErr;

		// Token: 0x040011A6 RID: 4518
		protected OutOfProcessServerSessionTransportManager sessionTM;

		// Token: 0x040011A7 RID: 4519
		protected OutOfProcessUtils.DataProcessingDelegates callbacks;

		// Token: 0x040011A8 RID: 4520
		protected static object SyncObject = new object();

		// Token: 0x040011A9 RID: 4521
		protected object _syncObject = new object();

		// Token: 0x040011AA RID: 4522
		protected string _initialCommand;

		// Token: 0x040011AB RID: 4523
		protected ManualResetEvent allcmdsClosedEvent;

		// Token: 0x040011AC RID: 4524
		protected WindowsIdentity _windowsIdentityToImpersonate;

		// Token: 0x040011AD RID: 4525
		protected int _inProgressCommandsCount;

		// Token: 0x040011AE RID: 4526
		protected PowerShellTraceSource tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x040011AF RID: 4527
		protected bool _exitProcessOnError;
	}
}
