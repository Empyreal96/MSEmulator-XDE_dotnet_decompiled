using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Xde.Common;
using Microsoft.Xde.Common.Toolbar;
using Microsoft.Xde.H2LPlugin.Orientation;
using Microsoft.Xde.SantPlugin;

namespace Microsoft.Xde.H2LPlugin.Toolbar
{
	// Token: 0x02000008 RID: 8
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "H2LPlugin.OrientationFlyout")]
	public class OrientationFlyoutButton : XdeButtonBase
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public OrientationFlyoutButton()
		{
			base.Name = "H2LPlugin.OrientationFlyout";
			base.Tooltip = Resources.Orientation_Toolbar_OF_Tooltip;
			this.Toggled = true;
			this.Arrowed = true;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002E05 File Offset: 0x00001005
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002E0D File Offset: 0x0000100D
		[Import]
		public IXdeView View { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002E16 File Offset: 0x00001016
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002E1E File Offset: 0x0000101E
		[Import(typeof(IXdeH2LOrientationFeature))]
		public IXdeH2LOrientationFeature OrientationFeature
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

		// Token: 0x06000036 RID: 54 RVA: 0x00002E4C File Offset: 0x0000104C
		private void AddButtons()
		{
			foreach (OrientationMode mode in new OrientationMode[]
			{
				OrientationMode.Flat,
				OrientationMode.Clamshell,
				OrientationMode.Tablet,
				OrientationMode.Ramp,
				OrientationMode.Media
			})
			{
				OrientationModeInformation orientationModeInfo = this.orientationFeature.GetOrientationModeInfo(mode);
				this.AddButton(orientationModeInfo.DisplayName, ImageUtils.CreateBitmapFromBitmapSource(orientationModeInfo.Source), mode);
			}
			this.UpdateTogglesAndCurrentImage();
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EA8 File Offset: 0x000010A8
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002EBF File Offset: 0x000010BF
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002EC6 File Offset: 0x000010C6
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002ECE File Offset: 0x000010CE
		[Import]
		public IXdeControllerState ControllerState { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002ED7 File Offset: 0x000010D7
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002EFB File Offset: 0x000010FB
		public OrientationMode CurrentOrientationMode
		{
			get
			{
				if (this.orientationFeature != null && this.orientationFeature.IsConnected)
				{
					return this.orientationFeature.CurrentOrientationMode;
				}
				return OrientationMode.Flat;
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

		// Token: 0x0600003D RID: 61 RVA: 0x00002F12 File Offset: 0x00001112
		public override void OnClicked(object sender, EventArgs e)
		{
			this.View.Toolbar.ShowToolbarFlyout(base.Name, this.buttons, ToolbarFlags.HideOnNextButtonPressed | ToolbarFlags.HideIfAlreadyShowing);
			base.OnClicked(sender, e);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002F39 File Offset: 0x00001139
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

		// Token: 0x0600003F RID: 63 RVA: 0x00002F71 File Offset: 0x00001171
		private void AddButton(string tooltip, Bitmap icon, OrientationMode mode)
		{
			this.buttons.Add(new OrientationFlyoutButton.OrientationButton(this, tooltip, icon, mode));
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F88 File Offset: 0x00001188
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

		// Token: 0x04000019 RID: 25
		public const string SkuName = "H2LPlugin.OrientationFlyout";

		// Token: 0x0400001A RID: 26
		private List<OrientationFlyoutButton.OrientationButton> buttons = new List<OrientationFlyoutButton.OrientationButton>();

		// Token: 0x0400001B RID: 27
		private IXdeH2LOrientationFeature orientationFeature;

		// Token: 0x0400001C RID: 28
		private Dictionary<OrientationMode, Bitmap> modeToIconMap = new Dictionary<OrientationMode, Bitmap>();

		// Token: 0x0200002E RID: 46
		private class OrientationButton : XdeButtonBase
		{
			// Token: 0x0600018C RID: 396 RVA: 0x00007184 File Offset: 0x00005384
			public OrientationButton(OrientationFlyoutButton owner, string toolTip, Bitmap image, OrientationMode orientationValue)
			{
				this.owner = owner;
				this.orientationValue = orientationValue;
				base.Tooltip = toolTip;
				base.Image = image;
				base.Name = this.owner.Name + "." + orientationValue.ToString();
				this.Toggled = (this.orientationValue == this.owner.CurrentOrientationMode);
			}

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x0600018D RID: 397 RVA: 0x000071F5 File Offset: 0x000053F5
			public OrientationMode OrientationValue
			{
				get
				{
					return this.orientationValue;
				}
			}

			// Token: 0x0600018E RID: 398 RVA: 0x000071FD File Offset: 0x000053FD
			public override void OnClicked(object sender, EventArgs e)
			{
				this.owner.CurrentOrientationMode = this.orientationValue;
				base.OnClicked(sender, e);
			}

			// Token: 0x040000FF RID: 255
			private OrientationMode orientationValue;

			// Token: 0x04000100 RID: 256
			private OrientationFlyoutButton owner;
		}
	}
}
