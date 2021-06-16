using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000003 RID: 3
	public sealed class ClientWebSocket2 : WebSocket
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x0000683F File Offset: 0x00004A3F
		static ClientWebSocket2()
		{
			WebSocket.RegisterPrefixes();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006846 File Offset: 0x00004A46
		public ClientWebSocket2()
		{
			this.state = 0;
			this.options = new ClientWebSocketOptions();
			this.cts = new CancellationTokenSource();
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000686B File Offset: 0x00004A6B
		public ClientWebSocketOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006874 File Offset: 0x00004A74
		public override WebSocketCloseStatus? CloseStatus
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.CloseStatus;
				}
				return null;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000689E File Offset: 0x00004A9E
		public override string CloseStatusDescription
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.CloseStatusDescription;
				}
				return null;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000068B5 File Offset: 0x00004AB5
		public override string SubProtocol
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.SubProtocol;
				}
				return null;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000068CC File Offset: 0x00004ACC
		public override WebSocketState State
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.State;
				}
				switch (this.state)
				{
				case 0:
					return WebSocketState.None;
				case 1:
					return WebSocketState.Connecting;
				case 3:
					return WebSocketState.Closed;
				}
				return WebSocketState.Closed;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006914 File Offset: 0x00004B14
		public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException();
			}
			int num = Interlocked.CompareExchange(ref this.state, 1, 0);
			if (num == 3)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
			this.options.SetToReadOnly();
			return this.ConnectAsyncCore(uri, cancellationToken);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006984 File Offset: 0x00004B84
		private async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
		{
			HttpWebResponse httpWebResponse = null;
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			try
			{
				HttpWebRequest httpWebRequest = this.CreateAndConfigureRequest(uri);
				httpWebRequest.ContentLength = 0L;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
				cancellationTokenRegistration = cancellationToken.Register(new Action<object>(this.AbortRequest), httpWebRequest, false);
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				string subProtocol = this.ValidateResponse(httpWebRequest, httpWebResponse);
				this.innerWebSocket = WebSocket.CreateClientWebSocket(httpWebResponse.GetResponseStream(), subProtocol, this.options.ReceiveBufferSize, this.options.SendBufferSize, this.options.KeepAliveInterval, false, this.options.GetOrCreateBuffer());
				if (Interlocked.CompareExchange(ref this.state, 2, 1) != 1)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
			}
			catch (WebException innerException)
			{
				this.ConnectExceptionCleanup(httpWebResponse);
				throw new WebSocketException("Connection failure", innerException);
			}
			catch (Exception)
			{
				this.ConnectExceptionCleanup(httpWebResponse);
				throw;
			}
			finally
			{
				cancellationTokenRegistration.Dispose();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000069D9 File Offset: 0x00004BD9
		private void ConnectExceptionCleanup(HttpWebResponse response)
		{
			this.Dispose();
			if (response != null)
			{
				response.Dispose();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000069EC File Offset: 0x00004BEC
		private HttpWebRequest CreateAndConfigureRequest(Uri uri)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
			if (httpWebRequest == null)
			{
				throw new InvalidOperationException();
			}
			foreach (object obj in this.options.RequestHeaders.Keys)
			{
				string name = (string)obj;
				httpWebRequest.Headers.Add(name, this.options.RequestHeaders[name]);
			}
			if (this.options.UseDefaultCredentials)
			{
				httpWebRequest.UseDefaultCredentials = true;
			}
			else if (this.options.Credentials != null)
			{
				httpWebRequest.Credentials = this.options.Credentials;
			}
			if (this.options.InternalClientCertificates != null)
			{
				httpWebRequest.ClientCertificates = this.options.InternalClientCertificates;
			}
			httpWebRequest.Proxy = this.options.Proxy;
			httpWebRequest.CookieContainer = this.options.Cookies;
			this.cts.Token.Register(new Action<object>(this.AbortRequest), httpWebRequest, false);
			return httpWebRequest;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006B14 File Offset: 0x00004D14
		private string ValidateResponse(HttpWebRequest request, HttpWebResponse response)
		{
			if (response.StatusCode != HttpStatusCode.SwitchingProtocols)
			{
				throw new WebSocketException();
			}
			if (!string.Equals(response.Headers["Upgrade"], "websocket", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException();
			}
			if (!string.Equals(response.Headers["Connection"], "Upgrade", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException();
			}
			string a = response.Headers["Sec-WebSocket-Accept"];
			string secWebSocketAcceptString = WebSocketHelpers.GetSecWebSocketAcceptString(request.Headers["Sec-WebSocket-Key"]);
			if (!string.Equals(a, secWebSocketAcceptString, StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException();
			}
			string text = response.Headers["Sec-WebSocket-Protocol"];
			if (!string.IsNullOrWhiteSpace(text) && this.options.RequestedSubProtocols.Count > 0)
			{
				bool flag = false;
				using (IEnumerator<string> enumerator = this.options.RequestedSubProtocols.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (string.Equals(enumerator.Current, text, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					throw new WebSocketException();
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006C38 File Offset: 0x00004E38
		public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006C50 File Offset: 0x00004E50
		public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.ReceiveAsync(buffer, cancellationToken);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006C65 File Offset: 0x00004E65
		public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006C7B File Offset: 0x00004E7B
		public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006C91 File Offset: 0x00004E91
		public override void Abort()
		{
			if (this.state == 3)
			{
				return;
			}
			if (this.innerWebSocket != null)
			{
				this.innerWebSocket.Abort();
			}
			this.Dispose();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006CB6 File Offset: 0x00004EB6
		private void AbortRequest(object obj)
		{
			((HttpWebRequest)obj).Abort();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006CC3 File Offset: 0x00004EC3
		public override void Dispose()
		{
			if (Interlocked.Exchange(ref this.state, 3) == 3)
			{
				return;
			}
			this.cts.Cancel(false);
			this.cts.Dispose();
			if (this.innerWebSocket != null)
			{
				this.innerWebSocket.Dispose();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006CFF File Offset: 0x00004EFF
		private void ThrowIfNotConnected()
		{
			if (this.state == 3)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.state != 2)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04000099 RID: 153
		private readonly ClientWebSocketOptions options;

		// Token: 0x0400009A RID: 154
		private WebSocket innerWebSocket;

		// Token: 0x0400009B RID: 155
		private readonly CancellationTokenSource cts;

		// Token: 0x0400009C RID: 156
		private int state;

		// Token: 0x0400009D RID: 157
		private const int created = 0;

		// Token: 0x0400009E RID: 158
		private const int connecting = 1;

		// Token: 0x0400009F RID: 159
		private const int connected = 2;

		// Token: 0x040000A0 RID: 160
		private const int disposed = 3;
	}
}
