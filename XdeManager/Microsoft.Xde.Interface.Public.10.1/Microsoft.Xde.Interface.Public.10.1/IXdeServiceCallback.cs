using System;
using System.ServiceModel;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000008 RID: 8
	public interface IXdeServiceCallback
	{
		// Token: 0x06000015 RID: 21
		[OperationContract(IsOneWay = true)]
		void OnDeviceToolsPipeReady();

		// Token: 0x06000016 RID: 22
		[OperationContract(IsOneWay = true)]
		void OnXdeReboot();

		// Token: 0x06000017 RID: 23
		[OperationContract(IsOneWay = true)]
		void OnXdeExit();
	}
}
