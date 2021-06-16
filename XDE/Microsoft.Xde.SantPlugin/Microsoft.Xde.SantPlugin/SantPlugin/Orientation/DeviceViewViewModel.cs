using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200001B RID: 27
	public class DeviceViewViewModel : INotifyPropertyChanged
	{
		// Token: 0x06000109 RID: 265 RVA: 0x0000546A File Offset: 0x0000366A
		public DeviceViewViewModel(Device device)
		{
			this.device = device;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600010A RID: 266 RVA: 0x0000549C File Offset: 0x0000369C
		// (remove) Token: 0x0600010B RID: 267 RVA: 0x000054D4 File Offset: 0x000036D4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600010C RID: 268 RVA: 0x0000550C File Offset: 0x0000370C
		public void Init(IXdeOrientationFeature orientationFeature, IXdeDisplayOutput displayOutput)
		{
			this.orientationFeature = orientationFeature;
			this.orientationFeature.PropertyChanged += this.OrientationFeature_PropertyChanged;
			this.device.OrientationConfigurationChanged += this.Device_OrientationConfigurationChanged;
			if (displayOutput.DisplayCount > 1)
			{
				this.device.SetScreenImageSource(0, displayOutput.GetDisplayOutput(0));
				this.device.SetScreenImageSource(1, displayOutput.GetDisplayOutput(1));
			}
			this.AddPostureItems();
			this.UpdateSelectedItem();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005589 File Offset: 0x00003789
		private void Device_OrientationConfigurationChanged(object sender, EventArgs e)
		{
			this.orientationFeature.CurrentOrientationConfig = this.device.OrientationConfiguration;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000055A4 File Offset: 0x000037A4
		private void OrientationFeature_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentOrientationConfig")
			{
				this.device.OrientationConfiguration = this.orientationFeature.CurrentOrientationConfig;
				this.FirePropertyChanged("OrientationConfiguration");
				return;
			}
			if (e.PropertyName == "CurrentOrientationMode")
			{
				this.UpdateSelectedItem();
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005600 File Offset: 0x00003800
		private void UpdateSelectedItem()
		{
			PostureItemViewModel postureItemViewModel = null;
			OrientationMode currentOrientationMode = this.orientationFeature.CurrentOrientationMode;
			if (currentOrientationMode != OrientationMode.Unknown)
			{
				this.modeToItemMap.TryGetValue(currentOrientationMode, out postureItemViewModel);
			}
			this.SelectedPostureItem = postureItemViewModel;
			this.device.OrientationConfiguration = this.orientationFeature.CurrentOrientationConfig;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000564C File Offset: 0x0000384C
		private void AddPostureItems()
		{
			foreach (OrientationMode orientationMode in new OrientationMode[]
			{
				OrientationMode.FlatLandscape,
				OrientationMode.FlatPortraitRight,
				OrientationMode.FlipRightPortrait,
				OrientationMode.FlipRightLandscapeRight,
				OrientationMode.PaletteLeft,
				OrientationMode.BrochureRight,
				OrientationMode.Read
			})
			{
				PostureItemViewModel postureItemViewModel = new PostureItemViewModel(this.orientationFeature.GetOrientationModeInfo(orientationMode));
				this.infos.Add(postureItemViewModel);
				this.modeToItemMap[orientationMode] = postureItemViewModel;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000056A8 File Offset: 0x000038A8
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000056B0 File Offset: 0x000038B0
		public PostureItemViewModel SelectedPostureItem
		{
			get
			{
				return this.selectedPostureItem;
			}
			set
			{
				if (this.selectedPostureItem != value)
				{
					this.selectedPostureItem = value;
					this.FirePropertyChanged("SelectedPostureItem");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000056CD File Offset: 0x000038CD
		public IList<PostureItemViewModel> PostureItems
		{
			get
			{
				return this.infos;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000056D5 File Offset: 0x000038D5
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000056E5 File Offset: 0x000038E5
		public bool IsLeftChecked
		{
			get
			{
				return this.device.AngleChangeConstraint == AngleChangeConstraint.Left;
			}
			set
			{
				this.device.AngleChangeConstraint = AngleChangeConstraint.Left;
				this.FireConstraintChanged();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000056F9 File Offset: 0x000038F9
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00005709 File Offset: 0x00003909
		public bool IsRightChecked
		{
			get
			{
				return this.device.AngleChangeConstraint == AngleChangeConstraint.Right;
			}
			set
			{
				this.device.AngleChangeConstraint = AngleChangeConstraint.Right;
				this.FireConstraintChanged();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000571D File Offset: 0x0000391D
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000572D File Offset: 0x0000392D
		public bool IsBothChecked
		{
			get
			{
				return this.device.AngleChangeConstraint == AngleChangeConstraint.Both;
			}
			set
			{
				this.device.AngleChangeConstraint = AngleChangeConstraint.Both;
				this.FireConstraintChanged();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005741 File Offset: 0x00003941
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000574E File Offset: 0x0000394E
		public OrientationConfiguration OrientationConfiguration
		{
			get
			{
				return this.orientationFeature.CurrentOrientationConfig;
			}
			set
			{
				this.orientationFeature.CurrentOrientationConfig = value;
				this.FirePropertyChanged("OrientationConfiguration");
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005767 File Offset: 0x00003967
		private void FireConstraintChanged()
		{
			this.FirePropertyChanged("IsLeftChecked");
			this.FirePropertyChanged("IsRightChecked");
			this.FirePropertyChanged("IsBothChecked");
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000578A File Offset: 0x0000398A
		private void FirePropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x0400008A RID: 138
		private Device device;

		// Token: 0x0400008B RID: 139
		private List<PostureItemViewModel> infos = new List<PostureItemViewModel>();

		// Token: 0x0400008C RID: 140
		private Dictionary<OrientationMode, PostureItemViewModel> modeToItemMap = new Dictionary<OrientationMode, PostureItemViewModel>();

		// Token: 0x0400008D RID: 141
		private IXdeOrientationFeature orientationFeature;

		// Token: 0x0400008E RID: 142
		private PostureItemViewModel selectedPostureItem;

		// Token: 0x0400008F RID: 143
		private WriteableBitmap[] bmpScreen = new WriteableBitmap[2];
	}
}
