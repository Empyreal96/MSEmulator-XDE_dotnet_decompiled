using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Xde.Common;
using Microsoft.Xde.Common.Toolbar;
using Microsoft.Xde.SantPlugin.Orientation;

namespace Microsoft.Xde.SantPlugin.Toolbar
{
	// Token: 0x02000013 RID: 19
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "SantPlugin.OrientationFlyout")]
	public class OrientationFlyoutButton : XdeButtonBase
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00004120 File Offset: 0x00002320
		public OrientationFlyoutButton()
		{
			base.Name = "SantPlugin.OrientationFlyout";
			base.Tooltip = Resources.Orientation_Toolbar_OF_Tooltip;
			this.Toggled = true;
			this.Arrowed = true;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000416D File Offset: 0x0000236D
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004175 File Offset: 0x00002375
		[Import]
		public IXdeView View { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000417E File Offset: 0x0000237E
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004186 File Offset: 0x00002386
		[Import]
		public IXdeControllerState ControllerState
		{
			get
			{
				return this.controllerState;
			}
			set
			{
				this.controllerState = value;
				this.controllerState.SkinChanged += delegate(object s, EventArgs e)
				{
					base.OnPropertyChanged("Visible");
				};
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000041A6 File Offset: 0x000023A6
		// (set) Token: 0x060000AF RID: 175 RVA: 0x000041AE File Offset: 0x000023AE
		[Import(typeof(IXdeOrientationFeature))]
		public IXdeOrientationFeature OrientationFeature
		{
			get
			{
				return this.orientationFeature;
			}
			set
			{
				this.orientationFeature = value;
				if (this.orientationFeature != null)
				{
					this.orientationFeature.PropertyChanged += this.OrientationFeature_PropertyChanged;
				}
				this.AddButtons();
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000041DC File Offset: 0x000023DC
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00004205 File Offset: 0x00002405
		public override bool Visible
		{
			get
			{
				return this.ControllerState.Skin != null && this.ControllerState.Skin.Display.DisplayCount == 2;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000420C File Offset: 0x0000240C
		private void AddButtons()
		{
			foreach (OrientationMode mode in new OrientationMode[]
			{
				OrientationMode.FlatLandscape,
				OrientationMode.FlatPortraitRight,
				OrientationMode.FlipRightPortrait,
				OrientationMode.FlipRightLandscapeRight
			})
			{
				OrientationModeInformation orientationModeInfo = this.orientationFeature.GetOrientationModeInfo(mode);
				this.AddButton(orientationModeInfo.DisplayName, ImageUtils.CreateBitmapFromBitmapSource(orientationModeInfo.Source), mode);
			}
			this.UpdateTogglesAndCurrentImage();
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004268 File Offset: 0x00002468
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00002EBF File Offset: 0x000010BF
		public override bool Enabled
		{
			get
			{
				return this.orientationFeature != null && this.orientationFeature.IsConnected;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000427F File Offset: 0x0000247F
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000042A3 File Offset: 0x000024A3
		public OrientationMode CurrentOrientationMode
		{
			get
			{
				if (this.orientationFeature != null && this.orientationFeature.IsConnected)
				{
					return this.orientationFeature.CurrentOrientationMode;
				}
				return OrientationMode.FlatLandscape;
			}
			set
			{
				if (this.orientationFeature == null)
				{
					return;
				}
				this.orientationFeature.CurrentOrientationMode = value;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000042BA File Offset: 0x000024BA
		public override void OnClicked(object sender, EventArgs e)
		{
			this.View.Toolbar.ShowToolbarFlyout(base.Name, this.buttons, ToolbarFlags.HideOnNextButtonPressed | ToolbarFlags.HideIfAlreadyShowing);
			base.OnClicked(sender, e);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000042E1 File Offset: 0x000024E1
		private void OrientationFeature_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected")
			{
				base.OnPropertyChanged("Enabled");
				return;
			}
			if (e.PropertyName == "CurrentOrientationMode")
			{
				this.UpdateTogglesAndCurrentImage();
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004319 File Offset: 0x00002519
		private void AddButton(string tooltip, Bitmap icon, OrientationMode mode)
		{
			this.buttons.Add(new OrientationFlyoutButton.OrientationButton(this, tooltip, icon, mode));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004330 File Offset: 0x00002530
		private void UpdateTogglesAndCurrentImage()
		{
			Bitmap bitmap = null;
			foreach (OrientationFlyoutButton.OrientationButton orientationButton in this.buttons)
			{
				orientationButton.Toggled = (orientationButton.OrientationValue == this.CurrentOrientationMode);
				if (orientationButton.Toggled)
				{
					bitmap = orientationButton.Image;
					this.Toggled = true;
				}
			}
			if (bitmap == null)
			{
				this.modeToIconMap.TryGetValue(this.CurrentOrientationMode, out bitmap);
				if (bitmap == null)
				{
					bitmap = ImageUtils.CreateBitmapFromBitmapSource(this.orientationFeature.GetOrientationModeInfo(this.CurrentOrientationMode).Source);
					this.modeToIconMap[this.CurrentOrientationMode] = bitmap;
				}
				this.Toggled = false;
			}
			base.Image = bitmap;
		}

		// Token: 0x0400004F RID: 79
		public const string SkuName = "SantPlugin.OrientationFlyout";

		// Token: 0x04000050 RID: 80
		private List<OrientationFlyoutButton.OrientationButton> buttons = new List<OrientationFlyoutButton.OrientationButton>();

		// Token: 0x04000051 RID: 81
		private IXdeOrientationFeature orientationFeature;

		// Token: 0x04000052 RID: 82
		private Dictionary<OrientationMode, Bitmap> modeToIconMap = new Dictionary<OrientationMode, Bitmap>();

		// Token: 0x04000053 RID: 83
		private IXdeControllerState controllerState;

		// Token: 0x02000032 RID: 50
		private class OrientationButton : XdeButtonBase
		{
			// Token: 0x06000196 RID: 406 RVA: 0x000072DC File Offset: 0x000054DC
			public OrientationButton(OrientationFlyoutButton owner, string toolTip, Bitmap image, OrientationMode orientationValue)
			{
				this.owner = owner;
				this.orientationValue = orientationValue;
				base.Tooltip = toolTip;
				base.Image = image;
				base.Name = this.owner.Name + "." + orientationValue.ToString();
				this.Toggled = (this.orientationValue == this.owner.CurrentOrientationMode);
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000197 RID: 407 RVA: 0x0000734D File Offset: 0x0000554D
			public OrientationMode OrientationValue
			{
				get
				{
					return this.orientationValue;
				}
			}

			// Token: 0x06000198 RID: 408 RVA: 0x00007355 File Offset: 0x00005555
			public override void OnClicked(object sender, EventArgs e)
			{
				this.owner.CurrentOrientationMode = this.orientationValue;
				base.OnClicked(sender, e);
			}

			// Token: 0x04000105 RID: 261
			private OrientationMode orientationValue;

			// Token: 0x04000106 RID: 262
			private OrientationFlyoutButton owner;
		}
	}
}
