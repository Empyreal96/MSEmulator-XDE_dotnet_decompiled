using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000521 RID: 1313
	internal abstract class LineOutput
	{
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600370E RID: 14094 RVA: 0x00129580 File Offset: 0x00127780
		internal virtual bool RequiresBuffering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x00129583 File Offset: 0x00127783
		internal virtual void ExecuteBufferPlayBack(LineOutput.DoPlayBackCall playback)
		{
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003710 RID: 14096
		internal abstract int ColumnNumber { get; }

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06003711 RID: 14097
		internal abstract int RowNumber { get; }

		// Token: 0x06003712 RID: 14098
		internal abstract void WriteLine(string s);

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x00129585 File Offset: 0x00127785
		// (set) Token: 0x06003714 RID: 14100 RVA: 0x0012958D File Offset: 0x0012778D
		internal WriteStreamType WriteStream { get; set; }

		// Token: 0x06003715 RID: 14101 RVA: 0x00129596 File Offset: 0x00127796
		internal void StopProcessing()
		{
			this._isStopping = true;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x0012959F File Offset: 0x0012779F
		internal void CheckStopProcessing()
		{
			if (!this._isStopping)
			{
				return;
			}
			throw new PipelineStoppedException();
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x001295AF File Offset: 0x001277AF
		internal virtual DisplayCells DisplayCells
		{
			get
			{
				this.CheckStopProcessing();
				return LineOutput._displayCellsDefault;
			}
		}

		// Token: 0x04001C33 RID: 7219
		private bool _isStopping;

		// Token: 0x04001C34 RID: 7220
		protected static DisplayCells _displayCellsDefault = new DisplayCells();

		// Token: 0x02000522 RID: 1314
		// (Invoke) Token: 0x0600371B RID: 14107
		internal delegate void DoPlayBackCall();
	}
}
