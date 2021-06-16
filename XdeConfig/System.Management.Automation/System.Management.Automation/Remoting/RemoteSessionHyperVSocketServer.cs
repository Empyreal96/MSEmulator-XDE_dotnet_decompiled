using System;
using System.IO;
using System.Management.Automation.Tracing;
using System.Net.Sockets;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002F3 RID: 755
	internal sealed class RemoteSessionHyperVSocketServer : IDisposable
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x000C8A50 File Offset: 0x000C6C50
		public Socket HyperVSocket
		{
			get
			{
				return this._socket;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000C8A58 File Offset: 0x000C6C58
		public NetworkStream Stream
		{
			get
			{
				return this._networkStream;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000C8A60 File Offset: 0x000C6C60
		public StreamReader TextReader
		{
			get
			{
				return this._streamReader;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000C8A68 File Offset: 0x000C6C68
		public StreamWriter TextWriter
		{
			get
			{
				return this._streamWriter;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000C8A70 File Offset: 0x000C6C70
		public bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000C8A78 File Offset: 0x000C6C78
		public RemoteSessionHyperVSocketServer(bool LoopbackMode)
		{
			this._syncObject = new object();
			Exception ex = null;
			try
			{
				Guid serviceId = new Guid("a5201c21-2770-4c11-a68e-f182edb29220");
				HyperVSocketEndPoint hyperVSocketEndPoint = new HyperVSocketEndPoint((AddressFamily)34, HyperVSocketEndPoint.HyperVSocketFlag.VM, Guid.Empty, serviceId);
				Socket socket = new Socket(hyperVSocketEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Icmp);
				socket.Bind(hyperVSocketEndPoint);
				socket.Listen(1);
				this._socket = socket.Accept();
				this._networkStream = new NetworkStream(this._socket, true);
				this._streamReader = new StreamReader(this._networkStream);
				this._streamWriter = new StreamWriter(this._networkStream);
				this._streamWriter.AutoFlush = true;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ex = ex2;
			}
			if (ex != null)
			{
				string text = (!string.IsNullOrEmpty(ex.Message)) ? ex.Message : string.Empty;
				this._tracer.WriteMessage("RemoteSessionHyperVSocketServer", "RemoteSessionHyperVSocketServer", Guid.Empty, "Unexpected error in constructor: {0}", new string[]
				{
					text
				});
				throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RemoteSessionHyperVSocketServerConstructorFailure, new object[0]), ex, PSRemotingErrorId.RemoteSessionHyperVSocketServerConstructorFailure.ToString(), ErrorCategory.InvalidOperation, null);
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000C8BBC File Offset: 0x000C6DBC
		public void Dispose()
		{
			lock (this._syncObject)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			if (this._streamReader != null)
			{
				try
				{
					this._streamReader.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				this._streamReader = null;
			}
			if (this._streamWriter != null)
			{
				try
				{
					this._streamWriter.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				this._streamWriter = null;
			}
			if (this._networkStream != null)
			{
				try
				{
					this._networkStream.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			if (this._socket != null)
			{
				try
				{
					this._socket.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x04001193 RID: 4499
		private Socket _socket;

		// Token: 0x04001194 RID: 4500
		private NetworkStream _networkStream;

		// Token: 0x04001195 RID: 4501
		private StreamReader _streamReader;

		// Token: 0x04001196 RID: 4502
		private StreamWriter _streamWriter;

		// Token: 0x04001197 RID: 4503
		private readonly object _syncObject;

		// Token: 0x04001198 RID: 4504
		private bool _disposed;

		// Token: 0x04001199 RID: 4505
		private PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();
	}
}
