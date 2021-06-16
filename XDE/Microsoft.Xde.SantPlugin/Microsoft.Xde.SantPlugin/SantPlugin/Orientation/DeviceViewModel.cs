using System;
using System.ComponentModel;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000019 RID: 25
	public class DeviceViewModel : INotifyPropertyChanged
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00004C19 File Offset: 0x00002E19
		public DeviceViewModel()
		{
			this.HingeAngle = 0.0;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000E3 RID: 227 RVA: 0x00004C30 File Offset: 0x00002E30
		// (remove) Token: 0x060000E4 RID: 228 RVA: 0x00004C68 File Offset: 0x00002E68
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004C9D File Offset: 0x00002E9D
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004CA5 File Offset: 0x00002EA5
		public AngleChangeConstraint AngleChangeConstraint
		{
			get
			{
				return this.moveConstraint;
			}
			set
			{
				this.moveConstraint = value;
				this.FirePropertyChanged("AngleChangeConstraint");
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004CB9 File Offset: 0x00002EB9
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public double HingeAngle
		{
			get
			{
				return this.Screen2YRotation - this.Screen1YRotation;
			}
			set
			{
				if (this.HingeAngle == value)
				{
					return;
				}
				double hingeAngle = this.HingeAngle;
				double num = value - hingeAngle;
				if (this.AngleChangeConstraint == AngleChangeConstraint.Right)
				{
					this.Screen2YRotation += num;
					return;
				}
				if (this.AngleChangeConstraint == AngleChangeConstraint.Left)
				{
					this.Screen1YRotation -= num;
					return;
				}
				double num2 = num / 2.0;
				this.Screen1YRotation -= num2;
				this.Screen2YRotation += num2;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004D41 File Offset: 0x00002F41
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00004D49 File Offset: 0x00002F49
		public double LeftX
		{
			get
			{
				return this.leftX;
			}
			set
			{
				this.leftX = value;
				this.FirePropertyChanged("LeftX");
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004D5D File Offset: 0x00002F5D
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004D65 File Offset: 0x00002F65
		public double LeftZ
		{
			get
			{
				return this.leftZ;
			}
			set
			{
				this.leftZ = value;
				this.FirePropertyChanged("LeftZ");
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004D79 File Offset: 0x00002F79
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00004D81 File Offset: 0x00002F81
		public double RightX
		{
			get
			{
				return this.rightX;
			}
			set
			{
				this.rightX = value;
				this.FirePropertyChanged("RightX");
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004D95 File Offset: 0x00002F95
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004D9D File Offset: 0x00002F9D
		public double RightZ
		{
			get
			{
				return this.rightZ;
			}
			set
			{
				this.rightZ = value;
				this.FirePropertyChanged("RightZ");
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004DB1 File Offset: 0x00002FB1
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004DB9 File Offset: 0x00002FB9
		public double Screen1YRotation
		{
			get
			{
				return this.screen1Yrotation;
			}
			set
			{
				if (this.screen1Yrotation != value)
				{
					this.screen1Yrotation = value;
					this.UpdateHingeRotation();
					this.FirePropertyChanged("Screen1YRotation");
					this.FirePropertyChanged("HingeAngle");
					this.FirePropertyChanged("OrientationConfiguration");
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004DF2 File Offset: 0x00002FF2
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004DFA File Offset: 0x00002FFA
		public double HingeYRotation
		{
			get
			{
				return this.hingeYRotation;
			}
			set
			{
				this.hingeYRotation = value;
				this.FirePropertyChanged("HingeYRotation");
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004E0E File Offset: 0x0000300E
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004E16 File Offset: 0x00003016
		public double Screen2YRotation
		{
			get
			{
				return this.screen2Yrotation;
			}
			set
			{
				if (this.screen2Yrotation != value)
				{
					this.screen2Yrotation = value;
					this.UpdateHingeRotation();
					this.FirePropertyChanged("Screen2YRotation");
					this.FirePropertyChanged("HingeAngle");
					this.FirePropertyChanged("OrientationConfiguration");
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004E4F File Offset: 0x0000304F
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004E57 File Offset: 0x00003057
		public PanelId PanelIdOverride { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004E60 File Offset: 0x00003060
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004EE6 File Offset: 0x000030E6
		public OrientationConfiguration OrientationConfiguration
		{
			get
			{
				PanelId panelId;
				if (this.PanelIdOverride != PanelId.None)
				{
					panelId = this.PanelIdOverride;
				}
				else
				{
					switch (this.AngleChangeConstraint)
					{
					case AngleChangeConstraint.Left:
						panelId = PanelId.Left;
						break;
					case AngleChangeConstraint.Right:
						panelId = PanelId.Right;
						break;
					default:
						panelId = PanelId.Both;
						break;
					}
				}
				return new OrientationConfiguration
				{
					LeftAngle = this.Screen1YRotation,
					RightAngle = this.Screen2YRotation,
					YRotation = this.YRotation,
					ZRotation = this.ZRotation,
					PanelId = panelId
				};
			}
			set
			{
				this.Screen1YRotation = value.LeftAngle;
				this.Screen2YRotation = value.RightAngle;
				this.YRotation = value.YRotation;
				this.ZRotation = value.ZRotation;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004F20 File Offset: 0x00003120
		public double YRotation
		{
			get
			{
				return this.yrotation;
			}
			set
			{
				if (this.yrotation != value)
				{
					this.yrotation = value;
					this.FirePropertyChanged("YRotation");
					this.FirePropertyChanged("OrientationConfiguration");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004F48 File Offset: 0x00003148
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004F50 File Offset: 0x00003150
		public double ZRotation
		{
			get
			{
				return this.zrotation;
			}
			set
			{
				if (this.zrotation != value)
				{
					this.zrotation = value;
					this.FirePropertyChanged("ZRotation");
					this.FirePropertyChanged("OrientationConfiguration");
				}
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004F78 File Offset: 0x00003178
		private static void Rotate(double x, double y, double angle, out double x1, out double y1)
		{
			double num = angle * 0.017453;
			x1 = Math.Cos(num) * x - Math.Sin(num) * y;
			y1 = Math.Sin(num) * x + Math.Cos(num) * y;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004FB8 File Offset: 0x000031B8
		private void UpdateHingeRotation()
		{
			this.HingeYRotation = -this.HingeAngle / 2.0 + this.Screen2YRotation;
			double num;
			double num2;
			DeviceViewModel.Rotate(0.0, -0.225, -this.HingeYRotation, out num, out num2);
			this.RightX = num;
			this.RightZ = num2;
			DeviceViewModel.Rotate(0.0, 0.225, -this.HingeYRotation, out num, out num2);
			this.LeftX = num;
			this.LeftZ = num2;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005043 File Offset: 0x00003243
		private void FirePropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x0400006C RID: 108
		private double yrotation;

		// Token: 0x0400006D RID: 109
		private double zrotation;

		// Token: 0x0400006E RID: 110
		private double screen1Yrotation;

		// Token: 0x0400006F RID: 111
		private double screen2Yrotation;

		// Token: 0x04000070 RID: 112
		private double hingeYRotation;

		// Token: 0x04000071 RID: 113
		private double leftX;

		// Token: 0x04000072 RID: 114
		private double leftZ;

		// Token: 0x04000073 RID: 115
		private double rightX;

		// Token: 0x04000074 RID: 116
		private double rightZ;

		// Token: 0x04000075 RID: 117
		private const double HingeDistBetweenHoles = 0.45;

		// Token: 0x04000076 RID: 118
		private AngleChangeConstraint moveConstraint;
	}
}
