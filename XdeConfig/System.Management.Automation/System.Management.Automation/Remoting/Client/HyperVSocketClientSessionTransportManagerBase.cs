using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Server;
using System.Threading;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x0200037F RID: 895
	internal abstract class HyperVSocketClientSessionTransportManagerBase : OutOfProcessClientSessionTransportManagerBase
	{
		// Token: 0x06002BB2 RID: 11186 RVA: 0x000F1B94 File Offset: 0x000EFD94
		internal HyperVSocketClientSessionTransportManagerBase(Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(runspaceId, cryptoHelper)
		{
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000F1B9E File Offset: 0x000EFD9E
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing && this._client != null)
			{
				this._client.Dispose();
			}
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000F1BBD File Offset: 0x000EFDBD
		protected override void CleanupConnection()
		{
			this._client.Close();
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000F1BCC File Offset: 0x000EFDCC
		protected void StartReaderThread(StreamReader reader)
		{
			new Thread(new ParameterizedThreadStart(this.ProcessReaderThread))
			{
				Name = "HyperVSocketTransport Reader Thread",
				IsBackground = true
			}.Start(reader);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000F1C04 File Offset: 0x000EFE04
		protected void ProcessReaderThread(object state)
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
					if (text.StartsWith(HyperVSocketErrorTextWriter.ErrorPrepend, StringComparison.OrdinalIgnoreCase))
					{
						string data = text.Substring(HyperVSocketErrorTextWriter.ErrorPrepend.Length);
						base.HandleErrorDataReceived(data);
					}
					else
					{
						base.HandleOutputDataReceived(text);
					}
				}
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCServerProcessReportedError, RemotingErrorIdStrings.IPCServerProcessReportedError, new object[]
				{
					RemotingErrorIdStrings.HyperVSocketTransportProcessEnded
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
				this._tracer.WriteMessage("HyperVSocketClientSessionTransportManager", "StartReaderThread", Guid.Empty, "Transport manager reader thread ended with error: {0}", new string[]
				{
					text2
				});
				PSRemotingTransportException e2 = new PSRemotingTransportException(PSRemotingErrorId.IPCServerProcessReportedError, RemotingErrorIdStrings.IPCServerProcessReportedError, new object[]
				{
					RemotingErrorIdStrings.HyperVSocketTransportProcessEnded
				});
				this.RaiseErrorHandler(new TransportErrorOccuredEventArgs(e2, TransportMethodEnum.ReceiveShellOutputEx));
			}
		}

		// Token: 0x040015EF RID: 5615
		private const string _threadName = "HyperVSocketTransport Reader Thread";

		// Token: 0x040015F0 RID: 5616
		protected RemoteSessionHyperVSocketClient _client;
	}
}
