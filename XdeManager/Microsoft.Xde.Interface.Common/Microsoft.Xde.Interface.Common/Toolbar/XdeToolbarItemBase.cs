using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common.Toolbar
{
	// Token: 0x0200001F RID: 31
	public class XdeToolbarItemBase : IXdeToolbarItem, IXdePluginComponent, INotifyPropertyChanged
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060000AD RID: 173 RVA: 0x0000212C File Offset: 0x0000032C
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x00002164 File Offset: 0x00000364
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002199 File Offset: 0x00000399
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000021A1 File Offset: 0x000003A1
		public string Name
		{
			get
			{
				return this.skuName;
			}
			protected set
			{
				if (this.skuName != value)
				{
					this.skuName = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000021C3 File Offset: 0x000003C3
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000021CB File Offset: 0x000003CB
		public string Tooltip
		{
			get
			{
				return this.tooltip;
			}
			set
			{
				if (this.tooltip != value)
				{
					this.tooltip = value;
					this.OnPropertyChanged("Tooltip");
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000021ED File Offset: 0x000003ED
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000021F5 File Offset: 0x000003F5
		public virtual bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					this.OnPropertyChanged("Visible");
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00002212 File Offset: 0x00000412
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x0000221A File Offset: 0x0000041A
		public virtual bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					this.OnPropertyChanged("Enabled");
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002237 File Offset: 0x00000437
		protected void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000013 RID: 19
		private string skuName;

		// Token: 0x04000014 RID: 20
		private string tooltip;

		// Token: 0x04000015 RID: 21
		private bool visible = true;

		// Token: 0x04000016 RID: 22
		private bool enabled = true;
	}
}
