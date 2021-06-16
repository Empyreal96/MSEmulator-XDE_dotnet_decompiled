using System;
using System.ComponentModel;
using System.ServiceModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.H2LPlugin.Orientation
{
	// Token: 0x0200000D RID: 13
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeH2LOrientationFeature : INotifyPropertyChanged, IXdeFeature, IXdePluginComponent, IDisposable
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000048 RID: 72
		// (set) Token: 0x06000049 RID: 73
		OrientationMode CurrentOrientationMode { [OperationContract] get; [OperationContract] set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004A RID: 74
		// (set) Token: 0x0600004B RID: 75
		OrientationConfiguration CurrentOrientationConfig { [OperationContract] get; [OperationContract] set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004C RID: 76
		bool IsConnected { [OperationContract] get; }

		// Token: 0x0600004D RID: 77
		[OperationContract]
		void RotateLeft();

		// Token: 0x0600004E RID: 78
		[OperationContract]
		void RotateRight();

		// Token: 0x0600004F RID: 79
		OrientationModeInformation GetOrientationModeInfo(OrientationMode mode);
	}
}
