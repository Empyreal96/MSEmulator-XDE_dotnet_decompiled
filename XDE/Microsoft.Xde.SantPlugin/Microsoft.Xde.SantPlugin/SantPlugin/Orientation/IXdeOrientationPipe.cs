using System;
using System.ComponentModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200001E RID: 30
	public interface IXdeOrientationPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000126 RID: 294
		bool GuestSupportsIndividualReadings { get; }

		// Token: 0x06000127 RID: 295
		void SetReading(OrientationReading2 reading);

		// Token: 0x06000128 RID: 296
		void UpdateAngle(AngleReading angleReading);

		// Token: 0x06000129 RID: 297
		void UpdateAccelerometer(AccelerometerReading accelReading);

		// Token: 0x0600012A RID: 298
		void UpdateOcclusion(OcclusionReading occlusionReading);

		// Token: 0x0600012B RID: 299
		void UpdateFold(FoldReading foldReading);
	}
}
