using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000018 RID: 24
	public partial class Device : UserControl
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x000044DF File Offset: 0x000026DF
		public Device()
		{
			base.DataContext = this.model;
			this.model.PropertyChanged += this.Model_PropertyChanged;
			this.InitializeComponent();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000C4 RID: 196 RVA: 0x0000451C File Offset: 0x0000271C
		// (remove) Token: 0x060000C5 RID: 197 RVA: 0x00004554 File Offset: 0x00002754
		public event EventHandler OrientationConfigurationChanged;

		// Token: 0x060000C6 RID: 198 RVA: 0x0000458C File Offset: 0x0000278C
		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Screen1YRotation")
			{
				this.SafeSetValueViaModelChange<double>(Device.LeftAngleProperty, this.model.Screen1YRotation);
				return;
			}
			if (e.PropertyName == "Screen2YRotation")
			{
				this.SafeSetValueViaModelChange<double>(Device.RightAngleProperty, this.model.Screen2YRotation);
				return;
			}
			if (e.PropertyName == "YRotation")
			{
				this.SafeSetValueViaModelChange<double>(Device.YRotationProperty, this.model.YRotation);
				return;
			}
			if (e.PropertyName == "ZRotation")
			{
				this.SafeSetValueViaModelChange<double>(Device.ZRotationProperty, this.model.ZRotation);
				return;
			}
			if (!(e.PropertyName == "OrientationConfiguration"))
			{
				if (e.PropertyName == "HingeAngle")
				{
					this.SafeSetValueViaModelChange<double>(Device.HingeAngleProperty, this.model.HingeAngle);
				}
				return;
			}
			EventHandler orientationConfigurationChanged = this.OrientationConfigurationChanged;
			if (orientationConfigurationChanged == null)
			{
				return;
			}
			orientationConfigurationChanged(this, EventArgs.Empty);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004690 File Offset: 0x00002890
		private void SafeSetValueViaModelChange<T>(DependencyProperty property, T value) where T : struct
		{
			T t = (T)((object)base.GetValue(property));
			if (!t.Equals(value))
			{
				this.modelAlreadySet = true;
				base.SetValue(property, value);
				this.modelAlreadySet = false;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000046DA File Offset: 0x000028DA
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000046EC File Offset: 0x000028EC
		public double HingeAngle
		{
			get
			{
				return (double)base.GetValue(Device.HingeAngleProperty);
			}
			set
			{
				base.SetValue(Device.HingeAngleProperty, value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000046FF File Offset: 0x000028FF
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004711 File Offset: 0x00002911
		public double YRotation
		{
			get
			{
				return (double)base.GetValue(Device.YRotationProperty);
			}
			set
			{
				base.SetValue(Device.YRotationProperty, value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004724 File Offset: 0x00002924
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004736 File Offset: 0x00002936
		public double ZRotation
		{
			get
			{
				return (double)base.GetValue(Device.ZRotationProperty);
			}
			set
			{
				base.SetValue(Device.ZRotationProperty, value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004749 File Offset: 0x00002949
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000475B File Offset: 0x0000295B
		public double LeftAngle
		{
			get
			{
				return (double)base.GetValue(Device.LeftAngleProperty);
			}
			set
			{
				base.SetValue(Device.LeftAngleProperty, value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000476E File Offset: 0x0000296E
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004780 File Offset: 0x00002980
		public double RightAngle
		{
			get
			{
				return (double)base.GetValue(Device.RightAngleProperty);
			}
			set
			{
				base.SetValue(Device.RightAngleProperty, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004793 File Offset: 0x00002993
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000047A5 File Offset: 0x000029A5
		public AngleChangeConstraint AngleChangeConstraint
		{
			get
			{
				return (AngleChangeConstraint)base.GetValue(Device.AngleChangeConstraintProperty);
			}
			set
			{
				base.SetValue(Device.AngleChangeConstraintProperty, value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000047B8 File Offset: 0x000029B8
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x000047C5 File Offset: 0x000029C5
		public OrientationConfiguration OrientationConfiguration
		{
			get
			{
				return this.model.OrientationConfiguration;
			}
			set
			{
				this.model.OrientationConfiguration = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000047D3 File Offset: 0x000029D3
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000047E0 File Offset: 0x000029E0
		public PanelId PanelIdOverride
		{
			get
			{
				return this.model.PanelIdOverride;
			}
			set
			{
				this.model.PanelIdOverride = value;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000047F0 File Offset: 0x000029F0
		public void SetScreenImageSource(int screenIndex, ImageSource source)
		{
			if (screenIndex < 0 || screenIndex > 1)
			{
				throw new ArgumentOutOfRangeException("screenIndex");
			}
			string resourceKey = string.Format("Screen{0}Mat", screenIndex + 1);
			foreach (Material material in ((MaterialGroup)base.FindResource(resourceKey)).Children)
			{
				DiffuseMaterial diffuseMaterial = material as DiffuseMaterial;
				if (diffuseMaterial != null)
				{
					((ImageBrush)diffuseMaterial.Brush).ImageSource = source;
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004888 File Offset: 0x00002A88
		private static void OnHingeAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			double hingeAngle = (double)e.NewValue;
			device.model.HingeAngle = hingeAngle;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000048C0 File Offset: 0x00002AC0
		private static void OnZRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			device.model.ZRotation = (double)e.NewValue;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000048F4 File Offset: 0x00002AF4
		private static void OnYRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			device.model.YRotation = (double)e.NewValue;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004928 File Offset: 0x00002B28
		private static void OnLeftAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			double screen1YRotation = (double)e.NewValue;
			device.model.Screen1YRotation = screen1YRotation;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004960 File Offset: 0x00002B60
		private static void OnRightAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			double num = (double)e.NewValue;
			device.model.Screen2YRotation = (double)e.NewValue;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000049A4 File Offset: 0x00002BA4
		private static void OnAngleChangeConstraintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Device device = (Device)d;
			if (device.modelAlreadySet)
			{
				return;
			}
			device.model.AngleChangeConstraint = (AngleChangeConstraint)e.NewValue;
		}

		// Token: 0x0400005D RID: 93
		public static readonly DependencyProperty HingeAngleProperty = DependencyProperty.Register("HingeAngle", typeof(double), typeof(Device), new FrameworkPropertyMetadata(180.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnHingeAngleChanged)));

		// Token: 0x0400005E RID: 94
		public static readonly DependencyProperty YRotationProperty = DependencyProperty.Register("YRotation", typeof(double), typeof(Device), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnYRotationChanged)));

		// Token: 0x0400005F RID: 95
		public static readonly DependencyProperty ZRotationProperty = DependencyProperty.Register("ZRotation", typeof(double), typeof(Device), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnZRotationChanged)));

		// Token: 0x04000060 RID: 96
		public static readonly DependencyProperty AngleChangeConstraintProperty = DependencyProperty.Register("AngleChangeConstraint", typeof(AngleChangeConstraint), typeof(Device), new FrameworkPropertyMetadata(AngleChangeConstraint.Left, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnAngleChangeConstraintChanged)));

		// Token: 0x04000061 RID: 97
		public static readonly DependencyProperty LeftAngleProperty = DependencyProperty.Register("LeftAngle", typeof(double), typeof(Device), new FrameworkPropertyMetadata(-180.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnLeftAngleChanged)));

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty RightAngleProperty = DependencyProperty.Register("RightAngle", typeof(double), typeof(Device), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Device.OnRightAngleChanged)));

		// Token: 0x04000063 RID: 99
		private DeviceViewModel model = new DeviceViewModel();

		// Token: 0x04000064 RID: 100
		private bool modelAlreadySet;
	}
}
