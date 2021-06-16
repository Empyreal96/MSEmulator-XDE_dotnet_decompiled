using System;
using System.ComponentModel;
using System.Drawing;

namespace Microsoft.Xde.Common.Toolbar
{
	// Token: 0x0200001E RID: 30
	public class XdeButtonBase : XdeToolbarItemBase, IXdeButton, IXdeToolbarItem, IXdePluginComponent, INotifyPropertyChanged
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000020B2 File Offset: 0x000002B2
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000020BA File Offset: 0x000002BA
		public Bitmap Image
		{
			get
			{
				return this.image;
			}
			protected set
			{
				if (this.image != value)
				{
					this.image = value;
					base.OnPropertyChanged("Image");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000020D7 File Offset: 0x000002D7
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000020DF File Offset: 0x000002DF
		public virtual bool Toggled
		{
			get
			{
				return this.toggled;
			}
			set
			{
				if (this.toggled != value)
				{
					this.toggled = value;
					base.OnPropertyChanged("Toggled");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000020FC File Offset: 0x000002FC
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00002104 File Offset: 0x00000304
		public virtual bool Arrowed
		{
			get
			{
				return this.arrowed;
			}
			set
			{
				if (this.arrowed != value)
				{
					this.arrowed = value;
					base.OnPropertyChanged("Arrowed");
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002121 File Offset: 0x00000321
		public virtual void OnClicked(object sender, EventArgs e)
		{
		}

		// Token: 0x04000010 RID: 16
		private Bitmap image;

		// Token: 0x04000011 RID: 17
		private bool toggled;

		// Token: 0x04000012 RID: 18
		private bool arrowed;
	}
}
