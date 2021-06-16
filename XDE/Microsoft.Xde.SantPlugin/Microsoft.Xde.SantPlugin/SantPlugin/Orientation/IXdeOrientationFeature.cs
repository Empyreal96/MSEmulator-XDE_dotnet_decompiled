using System;
using System.ComponentModel;
using System.ServiceModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x0200001D RID: 29
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeOrientationFeature : INotifyPropertyChanged, IXdeFeature, IXdePluginComponent, IDisposable
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600011E RID: 286
		// (set) Token: 0x0600011F RID: 287
		OrientationMode CurrentOrientationMode { [OperationContract] get; [OperationContract] set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000120 RID: 288
		// (set) Token: 0x06000121 RID: 289
		OrientationConfiguration CurrentOrientationConfig { [OperationContract] get; [OperationContract] set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000122 RID: 290
		bool IsConnected { [OperationContract] get; }

		// Token: 0x06000123 RID: 291
		[OperationContract]
		void RotateLeft();

		// Token: 0x06000124 RID: 292
		[OperationContract]
		void RotateRight();

		// Token: 0x06000125 RID: 293
		OrientationModeInformation GetOrientationModeInfo(OrientationMode mode);
	}
}
