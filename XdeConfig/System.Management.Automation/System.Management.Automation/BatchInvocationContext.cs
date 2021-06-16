using System;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000235 RID: 565
	internal class BatchInvocationContext
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x0009B847 File Offset: 0x00099A47
		internal BatchInvocationContext(PSCommand command, PSDataCollection<PSObject> output)
		{
			this.command = command;
			this.output = output;
			this.completionEvent = new AutoResetEvent(false);
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x0009B869 File Offset: 0x00099A69
		internal PSDataCollection<PSObject> Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x0009B871 File Offset: 0x00099A71
		internal PSCommand Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0009B879 File Offset: 0x00099A79
		internal void Wait()
		{
			this.completionEvent.WaitOne();
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0009B887 File Offset: 0x00099A87
		internal void Signal()
		{
			this.completionEvent.Set();
		}

		// Token: 0x04000ADB RID: 2779
		private PSCommand command;

		// Token: 0x04000ADC RID: 2780
		private PSDataCollection<PSObject> output;

		// Token: 0x04000ADD RID: 2781
		private AutoResetEvent completionEvent;
	}
}
