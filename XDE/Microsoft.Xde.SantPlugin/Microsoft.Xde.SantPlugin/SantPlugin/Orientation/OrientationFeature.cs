using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xde.Common;
using Microsoft.Xde.Communication;
using Microsoft.Xde.Interface;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000020 RID: 32
	[Export(typeof(IXdeFeature))]
	[ExportMetadata("Name", "SantPlugin.OrientationFeature")]
	[Export(typeof(IXdeOrientationFeature))]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class OrientationFeature : IXdeOrientationFeature, INotifyPropertyChanged, IXdeFeature, IXdePluginComponent, IDisposable
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000598C File Offset: 0x00003B8C
		public OrientationFeature()
		{
			this.AddUnknownMode();
			this.AddClosedMode();
			this.AddBrochureModes();
			this.AddFlipLeftModes();
			this.AddFlipRightModes();
			this.AddFlatModes();
			this.AddPaletteModes();
			this.AddPeekModes();
			this.CurrentOrientationMode = OrientationMode.FlatLandscape;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000131 RID: 305 RVA: 0x000059EC File Offset: 0x00003BEC
		// (remove) Token: 0x06000132 RID: 306 RVA: 0x00005A24 File Offset: 0x00003C24
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005A59 File Offset: 0x00003C59
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00005A61 File Offset: 0x00003C61
		[Import]
		public IXdeConnectionAddressInfo AddressInfo { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005A6A File Offset: 0x00003C6A
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00005A72 File Offset: 0x00003C72
		[Import]
		public IXdeMinUiFactory UiFactory { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005A7B File Offset: 0x00003C7B
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00005A83 File Offset: 0x00003C83
		[Import]
		public IXdeControllerState ControllerState { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005A8C File Offset: 0x00003C8C
		public string Name
		{
			get
			{
				return "SantPlugin.OrientationFeature";
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00005A94 File Offset: 0x00003C94
		public IXdeConnectionController Connection
		{
			get
			{
				if (this.orientationPipe == null)
				{
					this.orientationPipe = XdeOrientationPipe.XdeOrientationPipeFactory(this.AddressInfo);
					this.orientationPipe.ConnectionSucceeded += this.OrientationPipe_ConnectedEvent;
					this.orientationPipe.ConnectionFailed += this.OrientationPipe_ConnectionErrorEncountered;
				}
				return this.orientationPipe;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005AEE File Offset: 0x00003CEE
		public bool IsConnected
		{
			get
			{
				return this.orientationPipe != null && this.orientationPipe.IsConnected;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00005B05 File Offset: 0x00003D05
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00005B10 File Offset: 0x00003D10
		public OrientationMode CurrentOrientationMode
		{
			get
			{
				return this.orientationMode;
			}
			set
			{
				if (this.orientationMode != value)
				{
					OrientationModeInformation orientationModeInfo = this.GetOrientationModeInfo(value);
					this.CurrentOrientationConfig = orientationModeInfo.Config;
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005B3A File Offset: 0x00003D3A
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00005B44 File Offset: 0x00003D44
		public OrientationConfiguration CurrentOrientationConfig
		{
			get
			{
				return this.currentOrientationConfig;
			}
			set
			{
				if (!this.currentOrientationConfig.Equals(value))
				{
					this.currentOrientationConfig = value;
					OrientationModeInformation orientationModeInformation = (from i in this.orientationInfos
					where i.Config.Equals(value)
					select i).FirstOrDefault<OrientationModeInformation>();
					if (orientationModeInformation == null)
					{
						this.orientationMode = OrientationMode.Unknown;
					}
					else
					{
						this.orientationMode = orientationModeInformation.Mode;
						if (this.ControllerState != null && this.ControllerState.Skin != null)
						{
							this.ControllerState.Skin.MonitorMode = orientationModeInformation.MonitorMode;
							this.ControllerState.Skin.Orientation = orientationModeInformation.DisplayOrientation;
						}
					}
					this.orientationMode = ((orientationModeInformation != null) ? orientationModeInformation.Mode : OrientationMode.Unknown);
					this.SendOrientationConfig(value);
					this.OnPropertyChanged("CurrentOrientationConfig");
					this.OnPropertyChanged("CurrentOrientationMode");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00005C2A File Offset: 0x00003E2A
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00005C32 File Offset: 0x00003E32
		[Import]
		public IXdeAutomationServices AutomationServices
		{
			get
			{
				return this.automationServices;
			}
			set
			{
				this.automationServices = value;
				this.automationServices.RegisterAutomationFeature<IXdeOrientationFeature>(this);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005C48 File Offset: 0x00003E48
		public OrientationModeInformation GetOrientationModeInfo(OrientationMode mode)
		{
			return this.orientationInfos.Find((OrientationModeInformation i) => i.Mode == mode);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005C7C File Offset: 0x00003E7C
		public void RotateLeft()
		{
			OrientationModeInformation orientationModeInfo = this.GetOrientationModeInfo(this.orientationMode);
			this.CurrentOrientationMode = orientationModeInfo.LeftMode;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public void RotateRight()
		{
			OrientationModeInformation orientationModeInfo = this.GetOrientationModeInfo(this.orientationMode);
			this.CurrentOrientationMode = orientationModeInfo.RightMode;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005CCA File Offset: 0x00003ECA
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			this.DisposePipe();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005CE8 File Offset: 0x00003EE8
		private void SendOrientationConfig(OrientationConfiguration orientationConfig)
		{
			if (this.orientationPipe != null && this.orientationPipe.IsConnected)
			{
				OrientationReading2 reading = orientationConfig.GetReading();
				try
				{
					object obj = this.orientationPipeLock;
					lock (obj)
					{
						this.orientationPipe.SetReading(reading);
					}
				}
				catch (Exception ex)
				{
					this.UiFactory.ShowErrorMessageForException(Resources.OrientationFeature_OrientationSendFailedFormat, ex, "OrientationSent");
					if ((!(ex is XdePipeException) || !this.ControllerState.IsShuttingDown) && this.orientationPipe.IsConnected)
					{
						this.orientationPipe.DisconnectFromGuest();
					}
				}
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005DA4 File Offset: 0x00003FA4
		private void AddMode(OrientationMode mode, string displayName, string bitmapName, DisplayOrientation displayOrientation, MonitorMode monitorMode, OrientationMode[] rotateModes)
		{
			OrientationModeInformation item = new OrientationModeInformation(mode, displayName, bitmapName, displayOrientation, monitorMode, rotateModes, OrientationFeature.OrientationConfigs[(int)mode]);
			this.orientationInfos.Add(item);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005DD8 File Offset: 0x00003FD8
		private void AddUnknownMode()
		{
			OrientationMode[] rotateModes = new OrientationMode[1];
			this.AddMode(OrientationMode.Unknown, "No stock mode selected", "Unknown.png", DisplayOrientation.Portrait, MonitorMode.Both, rotateModes);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005E00 File Offset: 0x00004000
		private void AddClosedMode()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.Closed
			};
			this.AddMode(OrientationMode.Closed, "Closed", "Closed.png", DisplayOrientation.Portrait, MonitorMode.Both, rotateModes);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005E2C File Offset: 0x0000402C
		private void AddPeekModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.Peek
			};
			this.AddMode(OrientationMode.Peek, "Peek", "peek.png", DisplayOrientation.Portrait, MonitorMode.Both, rotateModes);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005E5C File Offset: 0x0000405C
		private void AddPaletteModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.Read,
				OrientationMode.PaletteRight,
				OrientationMode.PaletteLeft
			};
			this.AddMode(OrientationMode.Read, "Book", "read.png", DisplayOrientation.Portrait, MonitorMode.Both, rotateModes);
			this.AddMode(OrientationMode.PaletteLeft, "Palette", "paletteleft.png", DisplayOrientation.LandscapeRight, MonitorMode.Both, rotateModes);
			this.AddMode(OrientationMode.PaletteRight, "Palette right", "paletteright.png", DisplayOrientation.LandscapeLeft, MonitorMode.Both, rotateModes);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005EB8 File Offset: 0x000040B8
		private void AddFlatModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.FlatLandscape,
				OrientationMode.FlatPortraitLeft,
				OrientationMode.FlatPortraitRight
			};
			this.AddMode(OrientationMode.FlatLandscape, "Dual portrait", "FlatLandscape.png", DisplayOrientation.Portrait, MonitorMode.Both, rotateModes);
			this.AddMode(OrientationMode.FlatPortraitLeft, "Flat rotated left", "FlatPortraitLeft.png", DisplayOrientation.LandscapeLeft, MonitorMode.Both, rotateModes);
			this.AddMode(OrientationMode.FlatPortraitRight, "Dual landscape", "FlatPortraitRight.png", DisplayOrientation.LandscapeRight, MonitorMode.Both, rotateModes);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005F18 File Offset: 0x00004118
		private void AddFlipRightModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.FlipRightPortrait,
				OrientationMode.FlipRightLandscapeLeft,
				OrientationMode.FlipRightLandscapeRight
			};
			this.AddMode(OrientationMode.FlipRightPortrait, "Flip portrait", "FlipRightPortrait.png", DisplayOrientation.Portrait, MonitorMode.RightOnly, rotateModes);
			this.AddMode(OrientationMode.FlipRightLandscapeLeft, "Flip right, rotated right", "FlipRightLandscape.png", DisplayOrientation.LandscapeLeft, MonitorMode.RightOnly, rotateModes);
			this.AddMode(OrientationMode.FlipRightLandscapeRight, "Flip landscape", "FlipRightLandscape.png", DisplayOrientation.LandscapeRight, MonitorMode.RightOnly, rotateModes);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005F78 File Offset: 0x00004178
		private void AddFlipLeftModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.FlipLeftPortrait,
				OrientationMode.FlipLeftLandscapeLeft,
				OrientationMode.FlipLeftLandscapeRight
			};
			this.AddMode(OrientationMode.FlipLeftPortrait, "Flip left", "FlipLeftPortrait.png", DisplayOrientation.Portrait, MonitorMode.LeftOnly, rotateModes);
			this.AddMode(OrientationMode.FlipLeftLandscapeLeft, "Flip left, rotated left", "FlipLeftLandscape.png", DisplayOrientation.LandscapeLeft, MonitorMode.LeftOnly, rotateModes);
			this.AddMode(OrientationMode.FlipLeftLandscapeRight, "Flip left, rotated right", "FlipLeftLandscape.png", DisplayOrientation.LandscapeRight, MonitorMode.LeftOnly, rotateModes);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005FD8 File Offset: 0x000041D8
		private void AddBrochureModes()
		{
			OrientationMode[] rotateModes = new OrientationMode[]
			{
				OrientationMode.BrochureRight,
				OrientationMode.RampRight,
				OrientationMode.TentRight
			};
			this.AddMode(OrientationMode.BrochureRight, "Brochure", "BrochureRight.png", DisplayOrientation.Portrait, MonitorMode.RightOnly, rotateModes);
			this.AddMode(OrientationMode.RampRight, "Ramp right", "RampRight.png", DisplayOrientation.LandscapeLeft, MonitorMode.RightOnly, rotateModes);
			this.AddMode(OrientationMode.TentRight, "Tent right", "TentRight.png", DisplayOrientation.LandscapeRight, MonitorMode.Both, rotateModes);
			OrientationMode[] rotateModes2 = new OrientationMode[]
			{
				OrientationMode.BrochureLeft,
				OrientationMode.RampLeft,
				OrientationMode.TentLeft
			};
			this.AddMode(OrientationMode.BrochureLeft, "Brochure left", "BrochureLeft.png", DisplayOrientation.Portrait, MonitorMode.LeftOnly, rotateModes2);
			this.AddMode(OrientationMode.RampLeft, "Ramp left", "RampLeft.png", DisplayOrientation.LandscapeRight, MonitorMode.LeftOnly, rotateModes2);
			this.AddMode(OrientationMode.TentLeft, "Tent left", "TentLeft.png", DisplayOrientation.LandscapeLeft, MonitorMode.Both, rotateModes2);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006084 File Offset: 0x00004284
		private void OrientationPipe_ConnectionErrorEncountered(object sender, ExEventArgs e)
		{
			Logger.Instance.LogException("OrientationConnected", e.ExceptionData, this.disposed);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000060A1 File Offset: 0x000042A1
		private void OrientationPipe_ConnectedEvent(object sender, EventArgs e)
		{
			this.OnPropertyChanged("IsConnected");
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000060AE File Offset: 0x000042AE
		private void DisposePipe()
		{
			if (this.orientationPipe != null)
			{
				this.orientationPipe.Dispose();
				this.orientationPipe = null;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000060CA File Offset: 0x000042CA
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x040000AF RID: 175
		public const string SkuName = "SantPlugin.OrientationFeature";

		// Token: 0x040000B0 RID: 176
		public const string OrientationNotificationId = "Xde.StockOneCore.Orientation";

		// Token: 0x040000B1 RID: 177
		public const string AngleNotificationId = "Xde.StockOneCore.Angle";

		// Token: 0x040000B2 RID: 178
		private static readonly OrientationConfiguration[] OrientationConfigs = new OrientationConfiguration[]
		{
			new OrientationConfiguration
			{
				LeftAngle = 1000.0
			},
			new OrientationConfiguration
			{
				LeftAngle = 0.0,
				RightAngle = 0.0,
				ZRotation = 0.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 0.0,
				ZRotation = 0.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 180.0,
				ZRotation = 0.0,
				YRotation = 0.0,
				PanelId = PanelId.Right
			},
			new OrientationConfiguration
			{
				LeftAngle = -90.0,
				RightAngle = 20.0,
				ZRotation = 90.0,
				YRotation = 0.0,
				PanelId = PanelId.Left
			},
			new OrientationConfiguration
			{
				LeftAngle = -45.0,
				RightAngle = 45.0,
				ZRotation = 0.0,
				YRotation = -90.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -315.0,
				RightAngle = 0.0,
				ZRotation = -90.0,
				YRotation = -22.5
			},
			new OrientationConfiguration
			{
				LeftAngle = -270.0,
				RightAngle = 12.5,
				ZRotation = 90.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -315.0,
				RightAngle = 0.0,
				ZRotation = 0.0,
				YRotation = 0.0,
				PanelId = PanelId.Left
			},
			new OrientationConfiguration
			{
				LeftAngle = -20.0,
				RightAngle = 20.0,
				ZRotation = 0.0,
				YRotation = -90.0
			},
			new OrientationConfiguration
			{
				LeftAngle = 0.0,
				RightAngle = 360.0,
				ZRotation = 0.0,
				YRotation = 0.0,
				PanelId = PanelId.Left
			},
			new OrientationConfiguration
			{
				LeftAngle = -192.5,
				RightAngle = 90.0,
				ZRotation = -90.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 0.0,
				ZRotation = 90.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 0.0,
				ZRotation = -90.0,
				YRotation = 0.0
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 180.0,
				ZRotation = 90.0,
				YRotation = 0.0,
				PanelId = PanelId.Right
			},
			new OrientationConfiguration
			{
				LeftAngle = -180.0,
				RightAngle = 180.0,
				ZRotation = -90.0,
				YRotation = 0.0,
				PanelId = PanelId.Right
			},
			new OrientationConfiguration
			{
				LeftAngle = 0.0,
				RightAngle = 360.0,
				ZRotation = 90.0,
				YRotation = 0.0,
				PanelId = PanelId.Left
			},
			new OrientationConfiguration
			{
				LeftAngle = 0.0,
				RightAngle = 360.0,
				ZRotation = -90.0,
				YRotation = 0.0,
				PanelId = PanelId.Left
			},
			new OrientationConfiguration
			{
				LeftAngle = -90.0,
				RightAngle = 20.0,
				ZRotation = -90.0,
				YRotation = -112.5
			},
			new OrientationConfiguration
			{
				LeftAngle = -315.0,
				RightAngle = 0.0,
				ZRotation = 90.0,
				YRotation = 157.5
			},
			new OrientationConfiguration
			{
				LeftAngle = -315.0,
				RightAngle = 0.0,
				ZRotation = 0.0,
				YRotation = 135.0,
				PanelId = PanelId.Right
			}
		};

		// Token: 0x040000B3 RID: 179
		private OrientationMode orientationMode;

		// Token: 0x040000B4 RID: 180
		private object orientationPipeLock = new object();

		// Token: 0x040000B5 RID: 181
		private IXdeOrientationPipe orientationPipe;

		// Token: 0x040000B6 RID: 182
		private bool disposed;

		// Token: 0x040000B7 RID: 183
		private List<OrientationModeInformation> orientationInfos = new List<OrientationModeInformation>();

		// Token: 0x040000B8 RID: 184
		private OrientationConfiguration currentOrientationConfig;

		// Token: 0x040000B9 RID: 185
		private IXdeAutomationServices automationServices;
	}
}
