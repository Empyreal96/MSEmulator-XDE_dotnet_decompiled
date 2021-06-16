using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Xde.Common;
using Microsoft.Xde.Common.Toolbar;

namespace Microsoft.Xde.SantPlugin.Toolbar
{
	// Token: 0x02000012 RID: 18
	[Export(typeof(IXdeButton))]
	[ExportMetadata("Name", "SantPlugin.LandscapeRotateFlyout")]
	public class LandscapeRotateFlyoutButton : XdeButtonBase
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00003E70 File Offset: 0x00002070
		public LandscapeRotateFlyoutButton()
		{
			base.Name = "SantPlugin.LandscapeRotateFlyout";
			base.Tooltip = Resources.Orientation_Toolbar_LRF_Tooltip;
			this.Toggled = true;
			this.Arrowed = true;
			this.AddButton(Resources.Orientation_Toolbar_NRL_ToolTip, Resources.Orientation_Toolbar_NRL_Icon, false);
			this.AddButton(Resources.Orientation_Toolbar_ERL_ToolTip, Resources.Orientation_Toolbar_ERL_Icon, true);
			this.UpdateTogglesAndCurrentImage();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003EDA File Offset: 0x000020DA
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003EE2 File Offset: 0x000020E2
		[Import]
		public IXdeView View
		{
			get
			{
				return this.view;
			}
			set
			{
				this.view = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003EEC File Offset: 0x000020EC
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00002EBF File Offset: 0x000010BF
		public override bool Enabled
		{
			get
			{
				bool flag = this.guestDisplay != null && this.guestDisplay.IsConnected && this.guestDisplay.SupportsRotateLandscapeUpperHalf;
				bool flag2 = this.controllerState != null && this.controllerState.Skin != null && this.controllerState.Skin.Orientation > DisplayOrientation.Portrait;
				return flag && flag2;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003F4A File Offset: 0x0000214A
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003F52 File Offset: 0x00002152
		[Import]
		public IXdeGuestDisplay GuestDisplay
		{
			get
			{
				return this.guestDisplay;
			}
			set
			{
				if (this.guestDisplay != value)
				{
					this.guestDisplay = value;
					if (this.guestDisplay != null)
					{
						this.guestDisplay.PropertyChanged += this.GuestDisplay_PropertyChanged;
					}
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003F83 File Offset: 0x00002183
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003F8B File Offset: 0x0000218B
		[Import]
		public IXdeControllerState ControllerState
		{
			get
			{
				return this.controllerState;
			}
			set
			{
				if (this.controllerState != value)
				{
					this.controllerState = value;
					if (this.controllerState != null)
					{
						this.OnSkinChanged();
						this.controllerState.SkinChanged += this.ControllerState_SkinChanged;
					}
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003FC2 File Offset: 0x000021C2
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003FD9 File Offset: 0x000021D9
		public bool CurrentRotate
		{
			get
			{
				return this.guestDisplay != null && this.guestDisplay.RotateLandscapeUpperHalf;
			}
			set
			{
				if (this.CurrentRotate != value && this.guestDisplay != null)
				{
					this.guestDisplay.RotateLandscapeUpperHalf = value;
				}
				this.UpdateTogglesAndCurrentImage();
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003FFE File Offset: 0x000021FE
		private void OnSkinChanged()
		{
			if (this.controllerState.Skin != null)
			{
				this.controllerState.Skin.PropertyChanged += this.Skin_PropertyChanged;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004029 File Offset: 0x00002229
		private void ControllerState_SkinChanged(object sender, EventArgs e)
		{
			this.OnSkinChanged();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004031 File Offset: 0x00002231
		private void Skin_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Orientation")
			{
				base.OnPropertyChanged("Enabled");
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004050 File Offset: 0x00002250
		private void GuestDisplay_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected")
			{
				base.OnPropertyChanged("Enabled");
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000406F File Offset: 0x0000226F
		public override void OnClicked(object sender, EventArgs e)
		{
			this.view.Toolbar.ShowToolbarFlyout(base.Name, this.buttons, ToolbarFlags.HideOnNextButtonPressed | ToolbarFlags.HideIfAlreadyShowing);
			base.OnClicked(sender, e);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004096 File Offset: 0x00002296
		private void AddButton(string tooltip, Bitmap icon, bool rotateLandscape)
		{
			this.buttons.Add(new LandscapeRotateFlyoutButton.RotationButton(this, tooltip, icon, rotateLandscape));
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000040AC File Offset: 0x000022AC
		private void UpdateTogglesAndCurrentImage()
		{
			foreach (LandscapeRotateFlyoutButton.RotationButton rotationButton in this.buttons)
			{
				rotationButton.Toggled = (rotationButton.Rotate == this.CurrentRotate);
				if (rotationButton.Toggled)
				{
					base.Image = rotationButton.Image;
				}
			}
		}

		// Token: 0x0400004A RID: 74
		public const string SkuName = "SantPlugin.LandscapeRotateFlyout";

		// Token: 0x0400004B RID: 75
		private List<LandscapeRotateFlyoutButton.RotationButton> buttons = new List<LandscapeRotateFlyoutButton.RotationButton>();

		// Token: 0x0400004C RID: 76
		private IXdeView view;

		// Token: 0x0400004D RID: 77
		private IXdeControllerState controllerState;

		// Token: 0x0400004E RID: 78
		private IXdeGuestDisplay guestDisplay;

		// Token: 0x02000031 RID: 49
		private class RotationButton : XdeButtonBase
		{
			// Token: 0x06000193 RID: 403 RVA: 0x0000724C File Offset: 0x0000544C
			public RotationButton(LandscapeRotateFlyoutButton owner, string toolTip, Bitmap image, bool rotate)
			{
				this.owner = owner;
				this.rotate = rotate;
				base.Tooltip = toolTip;
				base.Image = image;
				base.Name = this.owner.Name + "." + rotate.ToString();
				this.Toggled = (this.rotate == this.owner.CurrentRotate);
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000194 RID: 404 RVA: 0x000072B7 File Offset: 0x000054B7
			public bool Rotate
			{
				get
				{
					return this.rotate;
				}
			}

			// Token: 0x06000195 RID: 405 RVA: 0x000072BF File Offset: 0x000054BF
			public override void OnClicked(object sender, EventArgs e)
			{
				this.owner.CurrentRotate = this.rotate;
				base.OnClicked(sender, e);
			}

			// Token: 0x04000103 RID: 259
			private bool rotate;

			// Token: 0x04000104 RID: 260
			private LandscapeRotateFlyoutButton owner;
		}
	}
}
