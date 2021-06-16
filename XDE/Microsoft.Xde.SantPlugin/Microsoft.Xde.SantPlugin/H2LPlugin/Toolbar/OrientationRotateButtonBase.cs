using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Xde.Common.Toolbar;
using Microsoft.Xde.H2LPlugin.Orientation;

namespace Microsoft.Xde.H2LPlugin.Toolbar
{
	// Token: 0x02000009 RID: 9
	public class OrientationRotateButtonBase : XdeButtonBase
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003058 File Offset: 0x00001258
		protected OrientationRotateButtonBase(string name, string toolTip, Bitmap image, bool rotateRight)
		{
			base.Name = name;
			base.Tooltip = toolTip;
			base.Image = image;
			this.rotateRight = rotateRight;
			this.Enabled = false;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003084 File Offset: 0x00001284
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000308C File Offset: 0x0000128C
		[Import]
		public IXdeH2LOrientationFeature OrientationFeature
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

		// Token: 0x06000044 RID: 68 RVA: 0x000030AC File Offset: 0x000012AC
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

		// Token: 0x06000045 RID: 69 RVA: 0x000030D5 File Offset: 0x000012D5
		private void Feature_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected")
			{
				this.Enabled = this.feature.IsConnected;
			}
		}

		// Token: 0x0400001F RID: 31
		private bool rotateRight;

		// Token: 0x04000020 RID: 32
		private IXdeH2LOrientationFeature feature;
	}
}
