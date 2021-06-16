using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000009 RID: 9
	internal class WebSocket<T>
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00007270 File Offset: 0x00005470
		public WebSocket(IDevicePortalConnection connection, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> serverCertificateValidationHandler, bool sendStreams = false)
		{
			this.sendStreams = sendStreams;
			this.deviceConnection = connection;
			this.IsListeningForMessages = false;
			this.serverCertificateValidationHandler = serverCertificateValidationHandler;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007294 File Offset: 0x00005494
		private async Task ConnectInternalAsync(Uri endpoint)
		{
			this.websocket = new ClientWebSocket();
			this.websocket.Options.UseDefaultCredentials = true;
			this.websocket.Options.Credentials = this.deviceConnection.Credentials;
			string text = this.deviceConnection.Connection.AbsoluteUri;
			if (text.EndsWith("/"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			this.websocket.Options.SetRequestHeader("Origin", text);
			ServicePointManager.ServerCertificateValidationCallback = ((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors) => this.serverCertificateValidationHandler(sender, cert, chain, policyErrors));
			await this.websocket.ConnectAsync(endpoint, CancellationToken.None);
			this.IsConnected = true;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000072E4 File Offset: 0x000054E4
		private async Task CloseInternalAsync()
		{
			await Task.Run(delegate()
			{
				this.websocket.Dispose();
				this.websocket = null;
				this.IsConnected = false;
			});
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000732C File Offset: 0x0000552C
		private async Task StopListeningForMessagesInternalAsync()
		{
			await this.websocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
			if (this.IsListeningForMessages)
			{
				await this.receivingMessagesTask;
				this.receivingMessagesTask = null;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007374 File Offset: 0x00005574
		private async Task StartListeningForMessagesInternalAsync()
		{
			await Task.Run(delegate()
			{
				this.StartListeningForMessagesInternal();
			});
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000073B9 File Offset: 0x000055B9
		private void StartListeningForMessagesInternal()
		{
			this.IsListeningForMessages = true;
			this.receivingMessagesTask = Task.Run(async delegate()
			{
				try
				{
					ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[WebSocket<T>.MaxChunkSizeInBytes]);
					while (this.websocket.State == WebSocketState.Open)
					{
						using (MemoryStream ms = new MemoryStream())
						{
							WebSocketReceiveResult webSocketReceiveResult;
							for (;;)
							{
								webSocketReceiveResult = await this.websocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
								if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
								{
									break;
								}
								if ((long)webSocketReceiveResult.Count > (long)((ulong)WebSocket<T>.MaxChunkSizeInBytes))
								{
									goto Block_6;
								}
								ms.Write(buffer.Array, buffer.Offset, webSocketReceiveResult.Count);
								if (webSocketReceiveResult.EndOfMessage)
								{
									goto Block_7;
								}
							}
							await this.websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
							return;
							Block_6:
							throw new InvalidOperationException("Buffer not large enough");
							Block_7:
							ms.Seek(0L, SeekOrigin.Begin);
							if (webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
							{
								Stream stream = new MemoryStream();
								await ms.CopyToAsync(stream);
								stream.Position = 0L;
								this.ConvertStreamToMessage(stream);
								stream = null;
							}
						}
						MemoryStream ms = null;
					}
					buffer = default(ArraySegment<byte>);
				}
				catch (WebSocketException ex)
				{
					Exception innerException = ex.InnerException;
					SocketException ex2 = ((innerException != null) ? innerException.InnerException : null) as SocketException;
					if (ex2 == null || ex2.NativeErrorCode != 10054)
					{
						throw;
					}
				}
				finally
				{
					this.IsListeningForMessages = false;
				}
			});
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000073DC File Offset: 0x000055DC
		private async Task SendMessageInternalAsync(string message)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
			await this.websocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600012E RID: 302 RVA: 0x0000742C File Offset: 0x0000562C
		// (remove) Token: 0x0600012F RID: 303 RVA: 0x00007464 File Offset: 0x00005664
		public event WebSocketMessageReceivedEventInternalHandler<T> WebSocketMessageReceived;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000130 RID: 304 RVA: 0x0000749C File Offset: 0x0000569C
		// (remove) Token: 0x06000131 RID: 305 RVA: 0x000074D4 File Offset: 0x000056D4
		public event WebSocketStreamReceivedEventInternalHandler<T> WebSocketStreamReceived;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00007509 File Offset: 0x00005709
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00007511 File Offset: 0x00005711
		public bool IsConnected { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000751A File Offset: 0x0000571A
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00007522 File Offset: 0x00005722
		public bool IsListeningForMessages { get; private set; }

		// Token: 0x06000136 RID: 310 RVA: 0x0000752C File Offset: 0x0000572C
		internal async Task ConnectAsync(string apiPath, string payload = null)
		{
			if (!this.IsConnected)
			{
				Uri endpoint = Utilities.BuildEndpoint(this.deviceConnection.WebSocketConnection, apiPath, payload);
				await this.ConnectInternalAsync(endpoint);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007584 File Offset: 0x00005784
		internal async Task CloseAsync()
		{
			if (this.IsConnected)
			{
				if (this.IsListeningForMessages)
				{
					await this.StopListeningForMessagesInternalAsync();
				}
				await this.CloseInternalAsync();
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000075CC File Offset: 0x000057CC
		internal async Task ReceiveMessagesAsync()
		{
			if (this.IsConnected && !this.IsListeningForMessages)
			{
				await this.StartListeningForMessagesInternalAsync();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007614 File Offset: 0x00005814
		internal async Task SendMessageAsync(string message)
		{
			if (this.IsConnected)
			{
				await this.SendMessageInternalAsync(message);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007664 File Offset: 0x00005864
		private void ConvertStreamToMessage(Stream stream)
		{
			if (stream != null && stream.Length != 0L)
			{
				if (this.sendStreams)
				{
					WebSocketStreamReceivedEventInternalHandler<T> webSocketStreamReceived = this.WebSocketStreamReceived;
					if (webSocketStreamReceived == null)
					{
						return;
					}
					webSocketStreamReceived(this, new WebSocketMessageReceivedEventArgs<Stream>(stream));
					return;
				}
				else
				{
					try
					{
						DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
						{
							UseSimpleDictionaryFormat = true
						};
						T message = (T)((object)new DataContractJsonSerializer(typeof(T), settings).ReadObject(stream));
						WebSocketMessageReceivedEventInternalHandler<T> webSocketMessageReceived = this.WebSocketMessageReceived;
						if (webSocketMessageReceived != null)
						{
							webSocketMessageReceived(this, new WebSocketMessageReceivedEventArgs<T>(message));
						}
					}
					finally
					{
						if (stream != null)
						{
							((IDisposable)stream).Dispose();
						}
					}
				}
			}
		}

		// Token: 0x040000F6 RID: 246
		private const int WSAECONNRESET = 10054;

		// Token: 0x040000F7 RID: 247
		private static readonly uint MaxChunkSizeInBytes = 1024U;

		// Token: 0x040000F8 RID: 248
		private ClientWebSocket websocket;

		// Token: 0x040000F9 RID: 249
		private Task receivingMessagesTask;

		// Token: 0x040000FA RID: 250
		private Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> serverCertificateValidationHandler;

		// Token: 0x040000FB RID: 251
		private IDevicePortalConnection deviceConnection;

		// Token: 0x040000FC RID: 252
		private bool sendStreams;
	}
}
