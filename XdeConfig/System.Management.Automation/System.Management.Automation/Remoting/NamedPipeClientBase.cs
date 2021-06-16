using System;
using System.IO;
using System.IO.Pipes;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002EE RID: 750
	internal class NamedPipeClientBase : IDisposable
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000C8449 File Offset: 0x000C6649
		public StreamReader TextReader
		{
			get
			{
				return this._streamReader;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000C8451 File Offset: 0x000C6651
		public StreamWriter TextWriter
		{
			get
			{
				return this._streamWriter;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000C8459 File Offset: 0x000C6659
		public string PipeName
		{
			get
			{
				return this._pipeName;
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000C8474 File Offset: 0x000C6674
		public void Dispose()
		{
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
			if (this._clientPipeStream != null)
			{
				try
				{
					this._clientPipeStream.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000C8500 File Offset: 0x000C6700
		public void Connect(int timeout)
		{
			this._clientPipeStream = this.DoConnect(timeout);
			this._streamReader = new StreamReader(this._clientPipeStream);
			this._streamWriter = new StreamWriter(this._clientPipeStream);
			this._streamWriter.AutoFlush = true;
			this._tracer.WriteMessage("NamedPipeClientBase", "Connect", Guid.Empty, "Connection started on pipe: {0}", new string[]
			{
				this._pipeName
			});
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000C8578 File Offset: 0x000C6778
		public void Close()
		{
			if (this._clientPipeStream != null)
			{
				this._clientPipeStream.Dispose();
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000C858D File Offset: 0x000C678D
		public virtual void AbortConnect()
		{
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000C858F File Offset: 0x000C678F
		protected virtual NamedPipeClientStream DoConnect(int timeout)
		{
			return null;
		}

		// Token: 0x04001184 RID: 4484
		private NamedPipeClientStream _clientPipeStream;

		// Token: 0x04001185 RID: 4485
		private StreamReader _streamReader;

		// Token: 0x04001186 RID: 4486
		private StreamWriter _streamWriter;

		// Token: 0x04001187 RID: 4487
		private PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04001188 RID: 4488
		protected string _pipeName;
	}
}
