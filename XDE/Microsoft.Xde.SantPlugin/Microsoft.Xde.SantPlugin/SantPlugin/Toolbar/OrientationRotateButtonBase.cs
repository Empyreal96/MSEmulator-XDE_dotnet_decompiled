using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Xde.Common.Toolbar;
using Microsoft.Xde.SantPlugin.Orientation;

namespace Microsoft.Xde.SantPlugin.Toolbar
{
	// Token: 0x02000014 RID: 20
	public class OrientationRotateButtonBase : XdeButtonBase
	{
		// Token: 0x060000BC RID: 188 RVA: 0x0000440D File Offset: 0x0000260D
		protected OrientationRotateButtonBase(string name, string toolTip, Bitmap image, bool rotateRight)
		{
			base.Name = name;
			base.Tooltip = toolTip;
			base.Image = image;
			this.rotateRight = rotateRight;
			this.Enabled = false;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004439 File Offset: 0x00002639
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00004441 File Offset: 0x00002641
		[Import]
		public IXdeOrientationFeature OrientationFeature
		{
			get
			{
				return this.feature;
			}
			set
			{
				this.feature = value;
				this.feature.PropertyChanged += this.Feature_PropertyChanged;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004461 File Offset: 0x00002661
		public override void OnClicked(object sender, EventArgs e)
		{
			base.OnClicked(sender, e);
			if (this.rotateRight)
			{
				this.feature.RotateRight();
				return;
			}
			this.feature.RotateLeft();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000448A File Offset: 0x0000268A
		private void Feature_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected")
			{
				this.Enabled = this.feature.IsConnected;
			}
		}

		// Token: 0x04000055 RID: 85
		private bool rotateRight;

		// Token: 0x04000056 RID: 86
		private IXdeOrientationFeature feature;
	}
}
