using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200001A RID: 26
	public partial class DeviceView : UserControl
	{
		// Token: 0x06000102 RID: 258 RVA: 0x0000505C File Offset: 0x0000325C
		public DeviceView()
		{
			this.InitializeComponent();
			this.model = new DeviceViewViewModel(this.device);
			base.DataContext = this.model;
			this.InitStoryboard();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000050CF File Offset: 0x000032CF
		public void Init(IXdeOrientationFeature orientationFeature, IXdeDisplayOutput displayOutput)
		{
			this.orientationFeature = orientationFeature;
			this.model.Init(orientationFeature, displayOutput);
			this.model.PropertyChanged += this.Model_PropertyChanged;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000050FC File Offset: 0x000032FC
		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedPostureItem")
			{
				PostureItemViewModel selectedPostureItem = this.model.SelectedPostureItem;
				if (selectedPostureItem != null && !this.device.OrientationConfiguration.Equals(selectedPostureItem.Info.Config))
				{
					OrientationModeInformation info = selectedPostureItem.Info;
					this.leftAngleAnim.To = new double?(info.Config.LeftAngle);
					this.rightAngleAnim.To = new double?(info.Config.RightAngle);
					this.yrotAnim.To = new double?(info.Config.YRotation);
					this.zrotAnim.To = new double?(info.Config.ZRotation);
					this.storyboardTargePostureItem = selectedPostureItem;
					this.storyboard.Begin();
					this.device.PanelIdOverride = info.Config.PanelId;
				}
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000051F0 File Offset: 0x000033F0
		private void Storyboard_Completed(object sender, EventArgs e)
		{
			PostureItemViewModel postureItemViewModel = this.storyboardTargePostureItem;
			if (postureItemViewModel != null)
			{
				this.orientationFeature.CurrentOrientationMode = postureItemViewModel.Info.Mode;
				this.storyboardTargePostureItem = null;
			}
			this.device.PanelIdOverride = PanelId.None;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005230 File Offset: 0x00003430
		private void InitStoryboard()
		{
			this.storyboard.FillBehavior = FillBehavior.Stop;
			this.storyboard.Completed += this.Storyboard_Completed;
			Duration duration = TimeSpan.FromSeconds(1.0);
			this.leftAngleAnim.Duration = duration;
			this.rightAngleAnim.Duration = duration;
			this.yrotAnim.Duration = duration;
			this.zrotAnim.Duration = duration;
			Storyboard.SetTarget(this.leftAngleAnim, this.device);
			Storyboard.SetTargetProperty(this.leftAngleAnim, new PropertyPath(Device.LeftAngleProperty));
			Storyboard.SetTarget(this.rightAngleAnim, this.device);
			Storyboard.SetTargetProperty(this.rightAngleAnim, new PropertyPath(Device.RightAngleProperty));
			Storyboard.SetTarget(this.yrotAnim, this.device);
			Storyboard.SetTargetProperty(this.yrotAnim, new PropertyPath(Device.YRotationProperty));
			Storyboard.SetTarget(this.zrotAnim, this.device);
			Storyboard.SetTargetProperty(this.zrotAnim, new PropertyPath(Device.ZRotationProperty));
			this.storyboard.Children.Add(this.leftAngleAnim);
			this.storyboard.Children.Add(this.rightAngleAnim);
			this.storyboard.Children.Add(this.yrotAnim);
			this.storyboard.Children.Add(this.zrotAnim);
		}

		// Token: 0x04000079 RID: 121
		private DeviceViewViewModel model;

		// Token: 0x0400007A RID: 122
		private IXdeOrientationFeature orientationFeature;

		// Token: 0x0400007B RID: 123
		private PostureItemViewModel storyboardTargePostureItem;

		// Token: 0x0400007C RID: 124
		private Storyboard storyboard = new Storyboard();

		// Token: 0x0400007D RID: 125
		private DoubleAnimation leftAngleAnim = new DoubleAnimation();

		// Token: 0x0400007E RID: 126
		private DoubleAnimation rightAngleAnim = new DoubleAnimation();

		// Token: 0x0400007F RID: 127
		private DoubleAnimation yrotAnim = new DoubleAnimation();

		// Token: 0x04000080 RID: 128
		private DoubleAnimation zrotAnim = new DoubleAnimation();
	}
}
