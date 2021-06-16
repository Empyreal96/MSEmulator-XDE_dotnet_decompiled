using System;
using System.IO;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000525 RID: 1317
	internal class TextWriterLineOutput : LineOutput
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x001296F9 File Offset: 0x001278F9
		internal override int ColumnNumber
		{
			get
			{
				base.CheckStopProcessing();
				return this.columns;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x00129707 File Offset: 0x00127907
		internal override int RowNumber
		{
			get
			{
				base.CheckStopProcessing();
				return -1;
			}
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x00129710 File Offset: 0x00127910
		internal override void WriteLine(string s)
		{
			base.CheckStopProcessing();
			if (this.suppressNewline)
			{
				this.writer.Write(s);
				return;
			}
			this.writer.WriteLine(s);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x00129739 File Offset: 0x00127939
		internal TextWriterLineOutput(TextWriter writer, int columns)
		{
			this.writer = writer;
			this.columns = columns;
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x0012974F File Offset: 0x0012794F
		internal TextWriterLineOutput(TextWriter writer, int columns, bool suppressNewline) : this(writer, columns)
		{
			this.suppressNewline = suppressNewline;
		}

		// Token: 0x04001C3A RID: 7226
		private int columns;

		// Token: 0x04001C3B RID: 7227
		private TextWriter writer;

		// Token: 0x04001C3C RID: 7228
		private bool suppressNewline;
	}
}
