using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Microsoft.Xde.Common;
using Microsoft.Xde.Communication;

namespace Microsoft.Xde.Base.Connection
{
	// Token: 0x0200000E RID: 14
	[Export(typeof(IXdeConnectionManager))]
	public class XdeConnectionManager : IXdeConnectionManager, IDisposable
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060000F1 RID: 241 RVA: 0x00005554 File Offset: 0x00003754
		// (remove) Token: 0x060000F2 RID: 242 RVA: 0x0000558C File Offset: 0x0000378C
		public event EventHandler ConnectionsSucceeded;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060000F3 RID: 243 RVA: 0x000055C4 File Offset: 0x000037C4
		// (remove) Token: 0x060000F4 RID: 244 RVA: 0x000055FC File Offset: 0x000037FC
		public event EventHandler<MessageEventArgs> ConnectionsFailed;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060000F5 RID: 245 RVA: 0x00005634 File Offset: 0x00003834
		// (remove) Token: 0x060000F6 RID: 246 RVA: 0x0000566C File Offset: 0x0000386C
		public event EventHandler ShellReady;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060000F7 RID: 247 RVA: 0x000056A4 File Offset: 0x000038A4
		// (remove) Token: 0x060000F8 RID: 248 RVA: 0x000056DC File Offset: 0x000038DC
		public event EventHandler ConnectionsReleased;

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00005711 File Offset: 0x00003911
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00005719 File Offset: 0x00003919
		public bool IsShellReady { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00005722 File Offset: 0x00003922
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000572A File Offset: 0x0000392A
		[Import]
		public IXdeSku Sku { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005733 File Offset: 0x00003933
		// (set) Token: 0x060000FE RID: 254 RVA: 0x0000573B File Offset: 0x0000393B
		[Import]
		public IXdeConnectionAddressInfo AddressInfo { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005744 File Offset: 0x00003944
		// (set) Token: 0x06000100 RID: 256 RVA: 0x0000574C File Offset: 0x0000394C
		public IXdeShellReadyPipe ShellReadyPipe { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005755 File Offset: 0x00003955
		public bool AreConnectionsUp
		{
			get
			{
				return this.allConnected;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000575D File Offset: 0x0000395D
		public void DoShellReady()
		{
			this.InitShellReadyPipe();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005768 File Offset: 0x00003968
		public void DoConnections()
		{
			this.resultSent = false;
			this.allConnected = false;
			this.connectionControllers.Clear();
			this.connectionControllers.AddRange(this.Sku.ConnectionControllers);
			foreach (IXdeConnectionController xdeConnectionController in this.connectionControllers)
			{
				xdeConnectionController.ConnectionSucceeded += this.ConnectionController_Connected;
				xdeConnectionController.ConnectionFailed += this.NetworkController_ConnectionError;
				xdeConnectionController.InitiateConnection();
			}
			if (this.connectionControllers.Count == 0)
			{
				object obj = this.resultSync;
				lock (obj)
				{
					this.FireSuccessIfNeeded();
				}
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005848 File Offset: 0x00003A48
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this.disposed)
			{
				return;
			}
			EventHandler connectionsReleased = this.ConnectionsReleased;
			if (connectionsReleased != null)
			{
				connectionsReleased(this, EventArgs.Empty);
			}
			this.disposed = true;
			IXdeShellReadyPipe shellReadyPipe = this.ShellReadyPipe;
			if (shellReadyPipe == null)
			{
				return;
			}
			shellReadyPipe.Dispose();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005888 File Offset: 0x00003A88
		private void InitShellReadyPipe()
		{
			if (this.ShellReadyPipe == null)
			{
				this.ShellReadyPipe = XdeShellReadyPipe.XdeShellReadyPipeFactory(this.AddressInfo);
			}
			this.ShellReadyPipe.ConnectionFailed += this.ShellReadyPipe_ConnectionErrorEncountered;
			this.ShellReadyPipe.ConnectionSucceeded += this.ShellReadyPipe_ConnectedEvent;
			this.ShellReadyPipe.InitiateConnection();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000058E7 File Offset: 0x00003AE7
		private void ShellReadyPipe_ConnectedEvent(object sender, EventArgs e)
		{
			this.ShellReadyPipe.ShellReadyEvent += this.ShellReadyPipe_ShellReadyEvent;
			this.ShellReadyPipe.StartListening();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000590B File Offset: 0x00003B0B
		private void ShellReadyPipe_ShellReadyEvent(object sender, EventArgs e)
		{
			this.IsShellReady = true;
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				this.ShellReadyPipe.StopListening();
				this.ShellReadyPipe.ShellReadyEvent -= this.ShellReadyPipe_ShellReadyEvent;
				this.ShellReadyPipe.Dispose();
				this.ShellReadyPipe = null;
				this.FireShellReady();
			});
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005926 File Offset: 0x00003B26
		private void FireShellReady()
		{
			if (this.ShellReady != null)
			{
				this.ShellReady(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005941 File Offset: 0x00003B41
		private void ShellReadyPipe_ConnectionErrorEncountered(object sender, ExEventArgs e)
		{
			this.OnConnectionsFailed(e.ExceptionData.Message);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005954 File Offset: 0x00003B54
		private void OnConnectionsFailed(string message)
		{
			if (this.ConnectionsFailed != null)
			{
				this.ConnectionsFailed(this, new MessageEventArgs(message));
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005970 File Offset: 0x00003B70
		private void NetworkController_ConnectionError(object sender, ExEventArgs e)
		{
			IXdeConnectionController xdeConnectionController = (IXdeConnectionController)sender;
			object obj = this.resultSync;
			lock (obj)
			{
				if (!this.resultSent)
				{
					this.resultSent = true;
					this.FireConnectionsFailed(e.ExceptionData.Message);
				}
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000059D4 File Offset: 0x00003BD4
		private void FireSuccessIfNeeded()
		{
			if (this.connectResult.Values.Count((bool v) => v) == this.connectResult.Count)
			{
				this.resultSent = true;
				this.allConnected = true;
				this.FireConnectionsSucceeded();
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005A34 File Offset: 0x00003C34
		private void ConnectionController_Connected(object sender, EventArgs e)
		{
			IXdeConnectionController key = (IXdeConnectionController)sender;
			object obj = this.resultSync;
			lock (obj)
			{
				if (!this.resultSent)
				{
					this.connectResult[key] = true;
					this.FireSuccessIfNeeded();
				}
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005A94 File Offset: 0x00003C94
		private void FireConnectionsSucceeded()
		{
			EventHandler connectionsSucceeded = this.ConnectionsSucceeded;
			if (connectionsSucceeded == null)
			{
				return;
			}
			connectionsSucceeded(this, EventArgs.Empty);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005AAC File Offset: 0x00003CAC
		private void FireConnectionsFailed(string message)
		{
			EventHandler<MessageEventArgs> connectionsFailed = this.ConnectionsFailed;
			if (connectionsFailed == null)
			{
				return;
			}
			connectionsFailed(this, new MessageEventArgs(message));
		}

		// Token: 0x04000058 RID: 88
		private List<IXdeConnectionController> connectionControllers = new List<IXdeConnectionController>();

		// Token: 0x04000059 RID: 89
		private Dictionary<IXdeConnectionController, bool> connectResult = new Dictionary<IXdeConnectionController, bool>();

		// Token: 0x0400005A RID: 90
		private object resultSync = new object();

		// Token: 0x0400005B RID: 91
		private bool resultSent;

		// Token: 0x0400005C RID: 92
		private bool allConnected;

		// Token: 0x0400005D RID: 93
		private bool disposed;
	}
}
