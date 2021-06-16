using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000382 RID: 898
	internal abstract class NamedPipeClientSessionTransportManagerBase : OutOfProcessClientSessionTransportManagerBase
	{
		// Token: 0x06002BBB RID: 11195 RVA: 0x000F1F40 File Offset: 0x000F0140
		internal NamedPipeClientSessionTransportManagerBase(RunspaceConnectionInfo connectionInfo, Guid runspaceId, PSRemotingCryptoHelper cryptoHelper, string threadName) : base(runspaceId, cryptoHelper)
		{
			if (connectionInfo == null)
			{
				throw new PSArgumentNullException("connectionInfo");
			}
			this._connectionInfo = connectionInfo;
			this._threadName = threadName;
			base.Fragmentor.FragmentSize = RemoteSessionNamedPipeServer.NamedPipeBufferSizeForRemoting;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000F1F8D File Offset: 0x000F018D
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing && this._clientPipe != null)
			{
				this._clientPipe.Dispose();
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000F1FAC File Offset: 0x000F01AC
		protected override void CleanupConnection()
		{
			this._clientPipe.Close();
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000F1FBC File Offset: 0x000F01BC
		protected void StartReaderThread(StreamReader reader)
		{
			new Thread(new ParameterizedThreadStart(this.ProcessReaderThread))
			{
				Name = this._threadName,
				IsBackground = true
			}.Start(reader);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000F1FF8 File Offset: 0x000F01F8
		private void ProcessReaderThread(object state)
		{
			try
			{
				StreamReader streamReader = state as StreamReader;
				base.SendOneItem();
				for (;;)
				{
					string text = streamReader.ReadLine();
					if (text == null)
					{
						break;
					}
					if (text.StartsWith(NamedPipeErrorTextWriter.ErrorPrepend, StringComparison.OrdinalIgnoreCase))
					{
						string data = text.Substring(NamedPipeErrorTextWriter.ErrorPrepend.Length);
						base.HandleErrorDataReceived(data);
					}
					else
					{
						base.HandleOutputDataReceived(text);
					}
				}
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCServerProcessReportedError, RemotingErrorIdStrings.IPCServerProcessReportedError, new object[]
				{
					RemotingErrorIdStrings.NamedPipeTransportProcessEnded
				});
				this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx));
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				ArgumentOutOfRangeException ex2 = ex as ArgumentOutOfRangeException;
				string text2 = (ex.Message != null) ? ex.Message : string.Empty;
				this._tracer.WriteMessage("NamedPipeClientSessionTransportManager", "StartReaderThread", Guid.Empty, "Transport manager reader thread ended with error: {0}", new string[]
				{
					text2
				});
			}
		}

		// Token: 0x040015F6 RID: 5622
		private RunspaceConnectionInfo _connectionInfo;

		// Token: 0x040015F7 RID: 5623
		protected NamedPipeClientBase _clientPipe = new NamedPipeClientBase();

		// Token: 0x040015F8 RID: 5624
		private string _threadName;
	}
}
