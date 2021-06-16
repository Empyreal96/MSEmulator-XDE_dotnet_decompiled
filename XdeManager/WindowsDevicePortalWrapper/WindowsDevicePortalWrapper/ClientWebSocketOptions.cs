using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000004 RID: 4
	public sealed class ClientWebSocketOptions
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00006D2C File Offset: 0x00004F2C
		internal ClientWebSocketOptions()
		{
			this.requestedSubProtocols = new List<string>();
			this.requestHeaders = new WebHeaderCollection();
			this.Proxy = WebRequest.DefaultWebProxy;
			this.receiveBufferSize = 16384;
			this.sendBufferSize = 16384;
			this.keepAliveInterval = WebSocket.DefaultKeepAliveInterval;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006D81 File Offset: 0x00004F81
		public void SetRequestHeader(string headerName, string headerValue)
		{
			this.ThrowIfReadOnly();
			this.requestHeaders.Set(headerName, headerValue);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00006D96 File Offset: 0x00004F96
		internal WebHeaderCollection RequestHeaders
		{
			get
			{
				return this.requestHeaders;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00006D9E File Offset: 0x00004F9E
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00006DA6 File Offset: 0x00004FA6
		public bool UseDefaultCredentials
		{
			get
			{
				return this.useDefaultCredentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.useDefaultCredentials = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006DB5 File Offset: 0x00004FB5
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00006DBD File Offset: 0x00004FBD
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.credentials = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006DCC File Offset: 0x00004FCC
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00006DD4 File Offset: 0x00004FD4
		public IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.proxy = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006DE3 File Offset: 0x00004FE3
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00006DFE File Offset: 0x00004FFE
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.clientCertificates = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006E1B File Offset: 0x0000501B
		internal X509CertificateCollection InternalClientCertificates
		{
			get
			{
				return this.clientCertificates;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00006E23 File Offset: 0x00005023
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00006E2B File Offset: 0x0000502B
		public CookieContainer Cookies
		{
			get
			{
				return this.cookies;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.cookies = value;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006E3A File Offset: 0x0000503A
		public void SetBuffer(int receiveBufferSize, int sendBufferSize)
		{
			this.ThrowIfReadOnly();
			this.buffer = null;
			this.receiveBufferSize = receiveBufferSize;
			this.sendBufferSize = sendBufferSize;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006E5C File Offset: 0x0000505C
		public void SetBuffer(int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
		{
			this.ThrowIfReadOnly();
			this.receiveBufferSize = receiveBufferSize;
			this.sendBufferSize = sendBufferSize;
			if (AppDomain.CurrentDomain.IsFullyTrusted)
			{
				this.buffer = new ArraySegment<byte>?(buffer);
				return;
			}
			this.buffer = null;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00006E97 File Offset: 0x00005097
		internal int ReceiveBufferSize
		{
			get
			{
				return this.receiveBufferSize;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00006E9F File Offset: 0x0000509F
		internal int SendBufferSize
		{
			get
			{
				return this.sendBufferSize;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006EA7 File Offset: 0x000050A7
		internal ArraySegment<byte> GetOrCreateBuffer()
		{
			if (this.buffer == null)
			{
				this.buffer = new ArraySegment<byte>?(WebSocket.CreateClientBuffer(this.receiveBufferSize, this.sendBufferSize));
			}
			return this.buffer.Value;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006EE0 File Offset: 0x000050E0
		public void AddSubProtocol(string subProtocol)
		{
			this.ThrowIfReadOnly();
			using (IEnumerator<string> enumerator = this.requestedSubProtocols.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Equals(enumerator.Current, subProtocol, StringComparison.OrdinalIgnoreCase))
					{
						throw new ArgumentException();
					}
				}
			}
			this.requestedSubProtocols.Add(subProtocol);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006F48 File Offset: 0x00005148
		internal IList<string> RequestedSubProtocols
		{
			get
			{
				return this.requestedSubProtocols;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006F50 File Offset: 0x00005150
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00006F58 File Offset: 0x00005158
		public TimeSpan KeepAliveInterval
		{
			get
			{
				return this.keepAliveInterval;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value < Timeout.InfiniteTimeSpan)
				{
					throw new ArgumentOutOfRangeException("value", value, "too small");
				}
				this.keepAliveInterval = value;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006F8A File Offset: 0x0000518A
		internal void SetToReadOnly()
		{
			this.isReadOnly = true;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006F93 File Offset: 0x00005193
		private void ThrowIfReadOnly()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040000A1 RID: 161
		private bool isReadOnly;

		// Token: 0x040000A2 RID: 162
		private readonly IList<string> requestedSubProtocols;

		// Token: 0x040000A3 RID: 163
		private readonly WebHeaderCollection requestHeaders;

		// Token: 0x040000A4 RID: 164
		private TimeSpan keepAliveInterval;

		// Token: 0x040000A5 RID: 165
		private int receiveBufferSize;

		// Token: 0x040000A6 RID: 166
		private int sendBufferSize;

		// Token: 0x040000A7 RID: 167
		private ArraySegment<byte>? buffer;

		// Token: 0x040000A8 RID: 168
		private bool useDefaultCredentials;

		// Token: 0x040000A9 RID: 169
		private ICredentials credentials;

		// Token: 0x040000AA RID: 170
		private IWebProxy proxy;

		// Token: 0x040000AB RID: 171
		private X509CertificateCollection clientCertificates;

		// Token: 0x040000AC RID: 172
		private CookieContainer cookies;
	}
}
