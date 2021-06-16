using System;
using System.IO;
using System.Management.Automation.Tracing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002F4 RID: 756
	internal sealed class RemoteSessionHyperVSocketClient : IDisposable
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000C8CA8 File Offset: 0x000C6EA8
		public HyperVSocketEndPoint EndPoint
		{
			get
			{
				return this._endPoint;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000C8CB0 File Offset: 0x000C6EB0
		public Socket HyperVSocket
		{
			get
			{
				return this._socket;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x000C8CB8 File Offset: 0x000C6EB8
		public NetworkStream Stream
		{
			get
			{
				return this._networkStream;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x000C8CC0 File Offset: 0x000C6EC0
		public StreamReader TextReader
		{
			get
			{
				return this._streamReader;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x000C8CC8 File Offset: 0x000C6EC8
		public StreamWriter TextWriter
		{
			get
			{
				return this._streamWriter;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x000C8CD0 File Offset: 0x000C6ED0
		public bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000C8CD8 File Offset: 0x000C6ED8
		internal RemoteSessionHyperVSocketClient(Guid vmId, bool isFirstConnection, bool isContainer = false)
		{
			this._syncObject = new object();
			Guid serviceId;
			if (isFirstConnection)
			{
				serviceId = new Guid("999e53d4-3d5c-4c3e-8779-bed06ec056e1");
			}
			else
			{
				serviceId = new Guid("a5201c21-2770-4c11-a68e-f182edb29220");
			}
			this._endPoint = new HyperVSocketEndPoint((AddressFamily)34, isContainer ? HyperVSocketEndPoint.HyperVSocketFlag.HyperVContainer : HyperVSocketEndPoint.HyperVSocketFlag.VM, vmId, serviceId);
			this._socket = new Socket(this._endPoint.AddressFamily, SocketType.Stream, ProtocolType.Icmp);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000C8D4C File Offset: 0x000C6F4C
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

		// Token: 0x060023DE RID: 9182 RVA: 0x000C8E38 File Offset: 0x000C7038
		public bool Connect(NetworkCredential networkCredential, bool isFirstConnection)
		{
			this._socket.Connect(this._endPoint);
			bool result;
			if (this._socket.Connected)
			{
				this._tracer.WriteMessage("RemoteSessionHyperVSocketClient", "Connect", Guid.Empty, "Client connected.", new string[0]);
				this._networkStream = new NetworkStream(this._socket, true);
				if (isFirstConnection)
				{
					if (string.IsNullOrEmpty(networkCredential.Domain))
					{
						networkCredential.Domain = "localhost";
					}
					if (string.IsNullOrEmpty(networkCredential.UserName))
					{
						throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidUsername, new object[0]), null, PSRemotingErrorId.InvalidUsername.ToString(), ErrorCategory.InvalidOperation, null);
					}
					bool flag = false;
					if (string.IsNullOrEmpty(networkCredential.Password))
					{
						flag = true;
					}
					byte[] bytes = Encoding.Unicode.GetBytes(networkCredential.Domain);
					byte[] bytes2 = Encoding.Unicode.GetBytes(networkCredential.UserName);
					byte[] bytes3 = Encoding.Unicode.GetBytes(networkCredential.Password);
					byte[] array = new byte[4];
					this._socket.Send(bytes);
					this._socket.Receive(array);
					this._socket.Send(bytes2);
					this._socket.Receive(array);
					string @string;
					if (flag)
					{
						this._socket.Send(Encoding.ASCII.GetBytes("EMPTYPW"));
						this._socket.Receive(array);
						this._socket.Send(array);
						@string = Encoding.ASCII.GetString(array);
					}
					else
					{
						this._socket.Send(Encoding.ASCII.GetBytes("NONEMPTYPW"));
						this._socket.Receive(array);
						this._socket.Send(bytes3);
						this._socket.Receive(array);
						this._socket.Send(array);
						@string = Encoding.ASCII.GetString(array);
					}
					if (string.Compare(@string, "FAIL", StringComparison.Ordinal) == 0)
					{
						throw new PSInvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidCredential, new object[0]), null, PSRemotingErrorId.InvalidCredential.ToString(), ErrorCategory.InvalidOperation, null);
					}
				}
				this._streamReader = new StreamReader(this._networkStream);
				this._streamWriter = new StreamWriter(this._networkStream);
				this._streamWriter.AutoFlush = true;
				result = true;
			}
			else
			{
				this._tracer.WriteMessage("RemoteSessionHyperVSocketClient", "Connect", Guid.Empty, "Client unable to connect.", new string[0]);
				result = false;
			}
			return result;
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000C90B1 File Offset: 0x000C72B1
		public void Close()
		{
			this._networkStream.Dispose();
			this._socket.Dispose();
		}

		// Token: 0x0400119A RID: 4506
		private HyperVSocketEndPoint _endPoint;

		// Token: 0x0400119B RID: 4507
		private Socket _socket;

		// Token: 0x0400119C RID: 4508
		private NetworkStream _networkStream;

		// Token: 0x0400119D RID: 4509
		private StreamReader _streamReader;

		// Token: 0x0400119E RID: 4510
		private StreamWriter _streamWriter;

		// Token: 0x0400119F RID: 4511
		private readonly object _syncObject;

		// Token: 0x040011A0 RID: 4512
		private bool _disposed;

		// Token: 0x040011A1 RID: 4513
		private PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x040011A2 RID: 4514
		private static ManualResetEvent connectDone = new ManualResetEvent(false);
	}
}
