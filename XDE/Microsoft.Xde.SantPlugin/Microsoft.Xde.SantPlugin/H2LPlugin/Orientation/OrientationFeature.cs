using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xde.Common;
using Microsoft.Xde.Communication;
using Microsoft.Xde.Interface;
using Microsoft.Xde.SantPlugin;
using Microsoft.Xde.SantPlugin.Orientation;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.H2LPlugin.Orientation
{
	// Token: 0x0200000F RID: 15
	[Export(typeof(IXdeFeature))]
	[ExportMetadata("Name", "H2LPlugin.OrientationFeature")]
	[Export(typeof(IXdeH2LOrientationFeature))]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class OrientationFeature : IXdeH2LOrientationFeature, INotifyPropertyChanged, IXdeFeature, IXdePluginComponent, IDisposable
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00003320 File Offset: 0x00001520
		public OrientationFeature()
		{
			this.AddUnknownMode();
			this.AddClosedMode();
			this.AddFlatModes();
			this.AddMediaModes();
			this.AddRampModes();
			this.AddTabletModes();
			this.AddClamshellModes();
			this.CurrentOrientationMode = OrientationMode.Flat;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000055 RID: 85 RVA: 0x0000337C File Offset: 0x0000157C
		// (remove) Token: 0x06000056 RID: 86 RVA: 0x000033B4 File Offset: 0x000015B4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000033E9 File Offset: 0x000015E9
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000033F1 File Offset: 0x000015F1
		[Import]
		public IXdeConnectionAddressInfo AddressInfo { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000033FA File Offset: 0x000015FA
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003402 File Offset: 0x00001602
		[Import]
		public IXdeMinUiFactory UiFactory { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000340B File Offset: 0x0000160B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003413 File Offset: 0x00001613
		[Import]
		public IXdeControllerState ControllerState { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000341C File Offset: 0x0000161C
		public string Name
		{
			get
			{
				return "H2LPlugin.OrientationFeature";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003424 File Offset: 0x00001624
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000347E File Offset: 0x0000167E
		public bool IsConnected
		{
			get
			{
				return this.orientationPipe != null && this.orientationPipe.IsConnected;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003495 File Offset: 0x00001695
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000034A0 File Offset: 0x000016A0
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000034CA File Offset: 0x000016CA
		// (set) Token: 0x06000063 RID: 99 RVA: 0x000034D4 File Offset: 0x000016D4
		public OrientationConfiguration CurrentOrientationConfig
		{
			get
			{
				return this.currentOrientationConfig;
			}
			set
			{
				if (!this.currentOrientationConfig.Equals(value) || value.ZRotation != this.currentOrientationConfig.ZRotation)
				{
					if (this.currentOrientationConfig.ZRotation != value.ZRotation && this.ControllerState != null && this.ControllerState.Skin != null)
					{
						DisplayOrientation orientation = this.ControllerState.Skin.Orientation;
						double zrotation = value.ZRotation;
						if (!0.0.Equals(zrotation))
						{
							if (!90.0.Equals(zrotation))
							{
								if (-90.0.Equals(zrotation))
								{
									orientation = DisplayOrientation.LandscapeRight;
								}
							}
							else
							{
								orientation = DisplayOrientation.LandscapeLeft;
							}
						}
						else
						{
							orientation = DisplayOrientation.Portrait;
						}
						this.ControllerState.Skin.Orientation = orientation;
					}
					this.currentOrientationConfig = value;
					OrientationModeInformation orientationModeInformation = (from i in this.orientationInfos
					where i.Config.Equals(value)
					select i).FirstOrDefault<OrientationModeInformation>();
					if (orientationModeInformation != null && this.ControllerState != null && this.ControllerState.Skin != null)
					{
						this.ControllerState.Skin.MonitorMode = orientationModeInformation.MonitorMode;
					}
					this.orientationMode = ((orientationModeInformation != null) ? orientationModeInformation.Mode : OrientationMode.Unknown);
					this.SendOrientationConfig(value);
					this.OnPropertyChanged("CurrentOrientationConfig");
					this.OnPropertyChanged("CurrentOrientationMode");
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003653 File Offset: 0x00001853
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000365B File Offset: 0x0000185B
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
				this.automationServices.RegisterAutomationFeature<IXdeH2LOrientationFeature>(this);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003670 File Offset: 0x00001870
		public OrientationModeInformation GetOrientationModeInfo(OrientationMode mode)
		{
			return this.orientationInfos.Find((OrientationModeInformation i) => i.Mode == mode);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000036A4 File Offset: 0x000018A4
		public void RotateLeft()
		{
			if (this.ControllerState == null || this.ControllerState.Skin == null)
			{
				return;
			}
			DisplayOrientation displayOrientation = this.ControllerState.Skin.Orientation;
			displayOrientation++;
			if (displayOrientation > DisplayOrientation.LandscapeRight)
			{
				displayOrientation = DisplayOrientation.Portrait;
			}
			this.UpdateConfig(displayOrientation);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000036EC File Offset: 0x000018EC
		public void RotateRight()
		{
			if (this.ControllerState == null || this.ControllerState.Skin == null)
			{
				return;
			}
			DisplayOrientation displayOrientation = this.ControllerState.Skin.Orientation;
			displayOrientation--;
			if (displayOrientation < DisplayOrientation.Portrait)
			{
				displayOrientation = DisplayOrientation.LandscapeRight;
			}
			this.UpdateConfig(displayOrientation);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003731 File Offset: 0x00001931
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

		// Token: 0x0600006A RID: 106 RVA: 0x00003750 File Offset: 0x00001950
		private void UpdateConfig(DisplayOrientation orientation)
		{
			OrientationConfiguration config = this.GetOrientationModeInfo(this.orientationMode).Config;
			switch (orientation)
			{
			case DisplayOrientation.LandscapeLeft:
				config.ZRotation = 90.0;
				goto IL_5A;
			case DisplayOrientation.LandscapeRight:
				config.ZRotation = -90.0;
				goto IL_5A;
			}
			config.ZRotation = 0.0;
			IL_5A:
			this.CurrentOrientationConfig = config;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000037C0 File Offset: 0x000019C0
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
						if (this.orientationPipe.GuestSupportsIndividualReadings)
						{
							this.orientationPipe.UpdateAccelerometer(new AccelerometerReading
							{
								PanelGroup = 2U,
								Vector = reading.C3
							});
							this.orientationPipe.UpdateAccelerometer(new AccelerometerReading
							{
								PanelGroup = 0U,
								Vector = reading.R2
							});
							this.orientationPipe.UpdateAngle(new AngleReading
							{
								PanelGroup = 1U,
								Angle = (float)orientationConfig.Angle1
							});
							this.orientationPipe.UpdateAngle(new AngleReading
							{
								PanelGroup = 2U,
								Angle = (float)orientationConfig.Angle2
							});
							OcclusionReading occlusionReading = new OcclusionReading
							{
								DeviceType = OcclusionDeviceType.Chassis
							};
							if (orientationConfig.OcclusionPercent != 0)
							{
								occlusionReading.Width = 100;
								occlusionReading.Height = orientationConfig.OcclusionPercent;
							}
							this.orientationPipe.UpdateOcclusion(occlusionReading);
						}
						else
						{
							this.orientationPipe.SetReading(reading);
						}
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

		// Token: 0x0600006C RID: 108 RVA: 0x00003994 File Offset: 0x00001B94
		private void AddMode(OrientationMode mode, string displayName, string bitmapName, DisplayOrientation displayOrientation, MonitorMode monitorMode)
		{
			OrientationModeInformation item = new OrientationModeInformation(mode, displayName, bitmapName, displayOrientation, monitorMode, OrientationFeature.OrientationConfigs[(int)mode]);
			this.orientationInfos.Add(item);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000039C5 File Offset: 0x00001BC5
		private void AddUnknownMode()
		{
			this.AddMode(OrientationMode.Unknown, "No stock mode selected", "Unknown.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000039DA File Offset: 0x00001BDA
		private void AddClosedMode()
		{
			this.AddMode(OrientationMode.Closed, "Closed", "Closed.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000039EF File Offset: 0x00001BEF
		private void AddClamshellModes()
		{
			this.AddMode(OrientationMode.Clamshell, "Clamshell", "clamshell.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003A04 File Offset: 0x00001C04
		private void AddTabletModes()
		{
			this.AddMode(OrientationMode.Tablet, "Tablet", "tablet.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A19 File Offset: 0x00001C19
		private void AddRampModes()
		{
			this.AddMode(OrientationMode.Ramp, "Ramp", "ramp.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003A2E File Offset: 0x00001C2E
		private void AddFlatModes()
		{
			this.AddMode(OrientationMode.Flat, "Flat", "Flat.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003A43 File Offset: 0x00001C43
		private void AddMediaModes()
		{
			this.AddMode(OrientationMode.Media, "Media", "Media.png", DisplayOrientation.Portrait, MonitorMode.Both);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003A58 File Offset: 0x00001C58
		private void OrientationPipe_ConnectionErrorEncountered(object sender, ExEventArgs e)
		{
			Logger.Instance.LogException("OrientationConnected", e.ExceptionData, this.disposed);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003A75 File Offset: 0x00001C75
		private void OrientationPipe_ConnectedEvent(object sender, EventArgs e)
		{
			this.OnPropertyChanged("IsConnected");
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A82 File Offset: 0x00001C82
		private void DisposePipe()
		{
			if (this.orientationPipe != null)
			{
				this.orientationPipe.Dispose();
				this.orientationPipe = null;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A9E File Offset: 0x00001C9E
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04000034 RID: 52
		public const string SkuName = "H2LPlugin.OrientationFeature";

		// Token: 0x04000035 RID: 53
		private static readonly OrientationConfiguration[] OrientationConfigs = new OrientationConfiguration[]
		{
			new OrientationConfiguration
			{
				Angle1 = 1000.0
			},
			new OrientationConfiguration
			{
				Angle1 = 0.0,
				Angle2 = 0.0,
				OcclusionPercent = 100
			},
			new OrientationConfiguration
			{
				Angle1 = 180.0,
				Angle2 = 0.0
			},
			new OrientationConfiguration
			{
				Angle1 = 120.0,
				Angle2 = 0.0
			},
			new OrientationConfiguration
			{
				Angle1 = 0.0,
				Angle2 = 180.0,
				OcclusionPercent = 100
			},
			new OrientationConfiguration
			{
				Angle1 = 120.0,
				Angle2 = 120.0
			},
			new OrientationConfiguration
			{
				Angle1 = 45.0,
				Angle2 = 75.0,
				OcclusionPercent = 66
			}
		};

		// Token: 0x04000036 RID: 54
		private OrientationMode orientationMode;

		// Token: 0x04000037 RID: 55
		private object orientationPipeLock = new object();

		// Token: 0x04000038 RID: 56
		private IXdeOrientationPipe orientationPipe;

		// Token: 0x04000039 RID: 57
		private bool disposed;

		// Token: 0x0400003A RID: 58
		private List<OrientationModeInformation> orientationInfos = new List<OrientationModeInformation>();

		// Token: 0x0400003B RID: 59
		private OrientationConfiguration currentOrientationConfig;

		// Token: 0x0400003C RID: 60
		private IXdeAutomationServices automationServices;
	}
}
