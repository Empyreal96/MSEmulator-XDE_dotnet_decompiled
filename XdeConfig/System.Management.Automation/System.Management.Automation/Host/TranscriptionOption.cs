using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Host
{
	// Token: 0x02000227 RID: 551
	internal class TranscriptionOption : IDisposable
	{
		// Token: 0x060019F1 RID: 6641 RVA: 0x0009AEF6 File Offset: 0x000990F6
		internal TranscriptionOption()
		{
			this.OutputToLog = new List<string>();
			this.OutputBeingLogged = new List<string>();
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0009AF14 File Offset: 0x00099114
		// (set) Token: 0x060019F3 RID: 6643 RVA: 0x0009AF1C File Offset: 0x0009911C
		internal string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
				this.Encoding = Encoding.UTF8;
				FileSystemCmdletProviderEncoding encoding = Utils.GetEncoding(value);
				if (encoding != FileSystemCmdletProviderEncoding.Default)
				{
					this.Encoding = Utils.GetEncodingFromEnum(encoding);
				}
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0009AF53 File Offset: 0x00099153
		// (set) Token: 0x060019F5 RID: 6645 RVA: 0x0009AF5B File Offset: 0x0009915B
		internal List<string> OutputToLog { get; private set; }

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0009AF64 File Offset: 0x00099164
		// (set) Token: 0x060019F7 RID: 6647 RVA: 0x0009AF6C File Offset: 0x0009916C
		internal List<string> OutputBeingLogged { get; private set; }

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0009AF75 File Offset: 0x00099175
		// (set) Token: 0x060019F9 RID: 6649 RVA: 0x0009AF7D File Offset: 0x0009917D
		internal bool IncludeInvocationHeader { get; set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0009AF86 File Offset: 0x00099186
		// (set) Token: 0x060019FB RID: 6651 RVA: 0x0009AF8E File Offset: 0x0009918E
		internal Encoding Encoding { get; private set; }

		// Token: 0x060019FC RID: 6652 RVA: 0x0009AF98 File Offset: 0x00099198
		internal void FlushContentToDisk()
		{
			lock (this.OutputBeingLogged)
			{
				if (!this.disposed)
				{
					if (this.contentWriter == null)
					{
						try
						{
							this.contentWriter = new StreamWriter(new FileStream(this.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), this.Encoding);
							this.contentWriter.BaseStream.Seek(0L, SeekOrigin.End);
						}
						catch (IOException)
						{
							this.contentWriter = new StreamWriter(new FileStream(this.Path, FileMode.Append, FileAccess.Write, FileShare.Read), this.Encoding);
						}
						this.contentWriter.AutoFlush = true;
					}
					foreach (string value in this.OutputBeingLogged)
					{
						this.contentWriter.WriteLine(value);
					}
				}
				this.OutputBeingLogged.Clear();
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0009B0A8 File Offset: 0x000992A8
		public void Dispose()
		{
			int num = 0;
			while (num < 1000 && (this.OutputToLog.Count > 0 || this.OutputBeingLogged.Count > 0))
			{
				Thread.Sleep(100);
				num += 100;
			}
			if (this.contentWriter != null)
			{
				this.contentWriter.Flush();
				this.contentWriter.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x04000AAD RID: 2733
		private string path;

		// Token: 0x04000AAE RID: 2734
		private StreamWriter contentWriter;

		// Token: 0x04000AAF RID: 2735
		private bool disposed;
	}
}
