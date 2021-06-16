using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000523 RID: 1315
	internal class WriteLineHelper
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x001295D0 File Offset: 0x001277D0
		internal WriteLineHelper(bool lineWrap, WriteLineHelper.WriteCallback wlc, WriteLineHelper.WriteCallback wc, DisplayCells displayCells)
		{
			if (wlc == null)
			{
				throw PSTraceSource.NewArgumentNullException("wlc");
			}
			if (displayCells == null)
			{
				throw PSTraceSource.NewArgumentNullException("displayCells");
			}
			this._displayCells = displayCells;
			this.writeLineCall = wlc;
			this.writeCall = ((wc != null) ? wc : wlc);
			this.lineWrap = lineWrap;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x00129623 File Offset: 0x00127823
		internal void WriteLine(string s, int cols)
		{
			this.WriteLineInternal(s, cols);
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x00129630 File Offset: 0x00127830
		private void WriteLineInternal(string val, int cols)
		{
			if (string.IsNullOrEmpty(val))
			{
				this.writeLineCall(val);
				return;
			}
			if (!this.lineWrap)
			{
				this.writeCall(val);
				return;
			}
			string[] array = StringManipulationHelper.SplitLines(val);
			for (int i = 0; i < array.Length; i++)
			{
				int num = this._displayCells.Length(array[i]);
				if (num < cols)
				{
					this.writeLineCall(array[i]);
				}
				else if (num == cols)
				{
					this.writeCall(array[i]);
				}
				else
				{
					string text = array[i];
					do
					{
						int headSplitLength = this._displayCells.GetHeadSplitLength(text, cols);
						this.WriteLineInternal(text.Substring(0, headSplitLength), cols);
						text = text.Substring(headSplitLength);
					}
					while (this._displayCells.Length(text) > cols);
					this.WriteLineInternal(text, cols);
				}
			}
		}

		// Token: 0x04001C36 RID: 7222
		private WriteLineHelper.WriteCallback writeCall;

		// Token: 0x04001C37 RID: 7223
		private WriteLineHelper.WriteCallback writeLineCall;

		// Token: 0x04001C38 RID: 7224
		private bool lineWrap;

		// Token: 0x04001C39 RID: 7225
		private DisplayCells _displayCells;

		// Token: 0x02000524 RID: 1316
		// (Invoke) Token: 0x06003722 RID: 14114
		internal delegate void WriteCallback(string s);
	}
}
