using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Xde.Common;
using Microsoft.Xde.Communication;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200002C RID: 44
	public class XdeOrientationPipe : XdePipe, IXdeOrientationPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00006C54 File Offset: 0x00004E54
		protected XdeOrientationPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeOrientationPipe.OrientationGuid)
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006CA9 File Offset: 0x00004EA9
		public static IXdeOrientationPipe XdeOrientationPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeOrientationPipe(addressInfo);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public bool GuestSupportsIndividualReadings
		{
			get
			{
				if (this.guestSupportsIndividualReadings != null)
				{
					return this.guestSupportsIndividualReadings.Value;
				}
				XdeOrientationPipe.CommandHeader commandHeader = new XdeOrientationPipe.CommandHeader
				{
					CommandId = XdeOrientationPipe.CommandId.SupportsIndividualSensors,
					Size = 0
				};
				base.SendStructToGuest(commandHeader);
				int num = base.ReceiveIntFromGuest();
				this.guestSupportsIndividualReadings = new bool?(num != -2147467259);
				return this.guestSupportsIndividualReadings.Value;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006D2C File Offset: 0x00004F2C
		public void UpdateAngle(AngleReading angleReading)
		{
			Logger.Instance.Log("UpdateAngle", Logger.Level.Local, new
			{
				angleReading.PanelGroup,
				angleReading.Angle
			});
			float num;
			if (this.angleReadings.TryGetValue(angleReading.PanelGroup, out num) && num == angleReading.Angle)
			{
				Logger.Instance.Log("SkippingSendAngleNotChanged", Logger.Level.Local);
				return;
			}
			if (this.SendCommandAndStruct(XdeOrientationPipe.CommandId.SetAngleValue, angleReading))
			{
				this.angleReadings[angleReading.PanelGroup] = angleReading.Angle;
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006DB0 File Offset: 0x00004FB0
		public void UpdateAccelerometer(AccelerometerReading accelReading)
		{
			Logger.Instance.Log("UpdateAccelerometer", Logger.Level.Local, new
			{
				accelReading.PanelGroup,
				accelReading.Vector.X,
				accelReading.Vector.Y,
				accelReading.Vector.Z
			});
			Vector3F vector3F;
			if (this.accelReadings.TryGetValue(accelReading.PanelGroup, out vector3F) && vector3F.Equals(accelReading.Vector))
			{
				Logger.Instance.Log("SkippingSendAccelerometerNotChanged", Logger.Level.Local);
				return;
			}
			if (this.SendCommandAndStruct(XdeOrientationPipe.CommandId.SetAccelValue, accelReading))
			{
				this.accelReadings[accelReading.PanelGroup] = accelReading.Vector;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006E60 File Offset: 0x00005060
		public void UpdateOcclusion(OcclusionReading occlusionReading)
		{
			Logger.Instance.Log("UpdateOcclusion", Logger.Level.Local, new
			{
				occlusionReading.DeviceType,
				occlusionReading.DockingState,
				occlusionReading.X,
				occlusionReading.Y,
				occlusionReading.Width,
				occlusionReading.Height
			});
			OcclusionReading occlusionReading2;
			if (this.occlusionReadings.TryGetValue(occlusionReading.DeviceType, out occlusionReading2) && occlusionReading2.Equals(occlusionReading))
			{
				Logger.Instance.Log("SkippingSendOcclusionNotChanged", Logger.Level.Local);
				return;
			}
			if (this.SendCommandAndStruct(XdeOrientationPipe.CommandId.SetOcclusionValue, occlusionReading))
			{
				this.occlusionReadings[occlusionReading.DeviceType] = occlusionReading;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006F04 File Offset: 0x00005104
		public void UpdateFold(FoldReading foldReading)
		{
			Logger.Instance.Log("UpdateFold", Logger.Level.Local, new
			{
				foldReading.ContributingPanelId,
				foldReading.Angle
			});
			if (foldReading.Equals(this.foldReading))
			{
				Logger.Instance.Log("SkippingSendFoldNotChanged", Logger.Level.Local);
				return;
			}
			if (this.SendCommandAndStruct(XdeOrientationPipe.CommandId.SetFoldValue, foldReading))
			{
				this.foldReading = foldReading;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006F74 File Offset: 0x00005174
		public void SetReading(OrientationReading2 reading)
		{
			Logger.Instance.Log("SetReading", Logger.Level.Local, new
			{
				X1 = reading.C3.X,
				Y1 = reading.C3.Y,
				Z1 = reading.C3.Z,
				X2 = reading.R2.X,
				Y2 = reading.R2.Y,
				Z2 = reading.R2.Z,
				Angle = reading.Angle,
				PanelId = reading.PanelId
			});
			if (reading.Equals(this.lastReading))
			{
				Logger.Instance.Log("SkippingSetReadingNotChanged", Logger.Level.Local);
				return;
			}
			if (this.guestSupportsPanelId == null)
			{
				XdeOrientationPipe.CommandHeader commandHeader = new XdeOrientationPipe.CommandHeader
				{
					CommandId = XdeOrientationPipe.CommandId.CheckPanelIdSupport,
					Size = 0
				};
				base.SendStructToGuest(commandHeader);
				int num = base.ReceiveIntFromGuest();
				this.guestSupportsPanelId = new bool?(num != -2147467259);
			}
			bool flag;
			if (this.guestSupportsPanelId.Value)
			{
				flag = this.SendCommandAndStruct(XdeOrientationPipe.CommandId.ReadingWithPanelId, reading);
			}
			else
			{
				OrientationReading orientationReading = new OrientationReading
				{
					Angle = reading.Angle,
					C3 = reading.C3,
					R2 = reading.R2
				};
				flag = this.SendCommandAndStruct(XdeOrientationPipe.CommandId.Reading, orientationReading);
			}
			if (flag)
			{
				this.lastReading = reading;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000070D0 File Offset: 0x000052D0
		private bool SendCommandAndStruct(XdeOrientationPipe.CommandId id, object item)
		{
			XdeOrientationPipe.CommandHeader commandHeader = new XdeOrientationPipe.CommandHeader
			{
				CommandId = id,
				Size = Marshal.SizeOf(item)
			};
			base.SendStructToGuest(commandHeader);
			base.SendStructToGuest(item);
			int num = base.ReceiveIntFromGuest();
			if (num == 0)
			{
				return true;
			}
			Logger.Instance.Log("SendCommandAndStructError", Logger.Level.Local, new
			{
				errorCode = num,
				item = item.ToString()
			});
			if (num == -2147024304)
			{
				return false;
			}
			if (num == -2147023728 || num == -2147024894)
			{
				return false;
			}
			throw new Exception(StringUtilities.CurrentCultureFormat(Resources.OrientationFeature_OrientationSetSampleValueFailed, new object[]
			{
				num
			}));
		}

		// Token: 0x040000EA RID: 234
		private const int E_FAIL = -2147467259;

		// Token: 0x040000EB RID: 235
		private const int ERROR_DATA_NOT_ACCEPTED = -2147024304;

		// Token: 0x040000EC RID: 236
		private const int E_NOTFOUND = -2147023728;

		// Token: 0x040000ED RID: 237
		private const int E_FILENOTFOUND = -2147024894;

		// Token: 0x040000EE RID: 238
		private bool? guestSupportsPanelId;

		// Token: 0x040000EF RID: 239
		private bool? guestSupportsIndividualReadings;

		// Token: 0x040000F0 RID: 240
		private static readonly Guid OrientationGuid = new Guid("{5494BCD8-DB62-4C1B-9BC1-F3C1E511BA0F}");

		// Token: 0x040000F1 RID: 241
		private OrientationReading2 lastReading;

		// Token: 0x040000F2 RID: 242
		private Dictionary<uint, float> angleReadings = new Dictionary<uint, float>();

		// Token: 0x040000F3 RID: 243
		private Dictionary<uint, Vector3F> accelReadings = new Dictionary<uint, Vector3F>();

		// Token: 0x040000F4 RID: 244
		private Dictionary<OcclusionDeviceType, OcclusionReading> occlusionReadings = new Dictionary<OcclusionDeviceType, OcclusionReading>();

		// Token: 0x040000F5 RID: 245
		private FoldReading foldReading = new FoldReading
		{
			Angle = 1000f
		};

		// Token: 0x02000035 RID: 53
		private struct CommandHeader
		{
			// Token: 0x04000109 RID: 265
			public XdeOrientationPipe.CommandId CommandId;

			// Token: 0x0400010A RID: 266
			public int Size;
		}

		// Token: 0x02000036 RID: 54
		private enum CommandId
		{
			// Token: 0x0400010C RID: 268
			Reading = 10,
			// Token: 0x0400010D RID: 269
			CheckPanelIdSupport,
			// Token: 0x0400010E RID: 270
			ReadingWithPanelId,
			// Token: 0x0400010F RID: 271
			SupportsIndividualSensors,
			// Token: 0x04000110 RID: 272
			SetAngleValue,
			// Token: 0x04000111 RID: 273
			SetAccelValue,
			// Token: 0x04000112 RID: 274
			SetFoldValue,
			// Token: 0x04000113 RID: 275
			SetOcclusionValue
		}
	}
}
