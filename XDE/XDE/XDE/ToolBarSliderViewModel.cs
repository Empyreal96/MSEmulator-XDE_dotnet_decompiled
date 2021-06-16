using System;
using System.ComponentModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200001A RID: 26
	public class ToolBarSliderViewModel : ToolBarItemViewModel
	{
		// Token: 0x06000196 RID: 406 RVA: 0x00007CF9 File Offset: 0x00005EF9
		public ToolBarSliderViewModel(IXdeTrackbar trackbar) : base(trackbar)
		{
			this.trackbar = trackbar;
			this.trackbar.PropertyChanged += this.Trackbar_PropertyChanged;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007D20 File Offset: 0x00005F20
		private void Trackbar_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				this.OnPropertyChanged("Value");
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007D3F File Offset: 0x00005F3F
		public double Maximum
		{
			get
			{
				return (double)this.trackbar.MaxValue;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007D4D File Offset: 0x00005F4D
		public double TickFrequency
		{
			get
			{
				return (this.Maximum - this.Minimum) / 10.0;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00007D66 File Offset: 0x00005F66
		public double Minimum
		{
			get
			{
				return (double)this.trackbar.MinValue;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007D74 File Offset: 0x00005F74
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00007D82 File Offset: 0x00005F82
		public double Value
		{
			get
			{
				return (double)this.trackbar.Value;
			}
			set
			{
				this.trackbar.Value = (int)value;
			}
		}

		// Token: 0x040000A0 RID: 160
		private IXdeTrackbar trackbar;
	}
}
