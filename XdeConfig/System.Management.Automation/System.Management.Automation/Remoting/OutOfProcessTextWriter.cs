using System;
using System.IO;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002F8 RID: 760
	internal class OutOfProcessTextWriter
	{
		// Token: 0x060023F5 RID: 9205 RVA: 0x000C9DEC File Offset: 0x000C7FEC
		internal OutOfProcessTextWriter(TextWriter writerToWrap)
		{
			this.writer = writerToWrap;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x000C9E08 File Offset: 0x000C8008
		internal virtual void WriteLine(string data)
		{
			if (this.isStopped)
			{
				return;
			}
			lock (this.syncObject)
			{
				this.writer.WriteLine(data);
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x000C9E58 File Offset: 0x000C8058
		internal void StopWriting()
		{
			this.isStopped = true;
		}

		// Token: 0x040011B3 RID: 4531
		private TextWriter writer;

		// Token: 0x040011B4 RID: 4532
		private bool isStopped;

		// Token: 0x040011B5 RID: 4533
		private object syncObject = new object();
	}
}
