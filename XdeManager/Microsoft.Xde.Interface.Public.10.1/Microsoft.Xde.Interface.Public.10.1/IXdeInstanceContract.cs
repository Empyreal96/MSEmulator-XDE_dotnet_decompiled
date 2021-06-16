using System;
using System.ServiceModel;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000007 RID: 7
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IXdeServiceCallback))]
	public interface IXdeInstanceContract
	{
		// Token: 0x0600000E RID: 14
		[OperationContract]
		void GetEndPoint(out string hostIP, out string deviceIP);

		// Token: 0x0600000F RID: 15
		[OperationContract]
		void BringToFront();

		// Token: 0x06000010 RID: 16
		[OperationContract]
		void Close();

		// Token: 0x06000011 RID: 17
		[OperationContract]
		VirtualMachineEnabledState GetVmState();

		// Token: 0x06000012 RID: 18
		[OperationContract]
		void RegisterForEvents(EndPointType endPointType);

		// Token: 0x06000013 RID: 19
		[OperationContract]
		void UnRegisterForEvents(EndPointType endPointType);

		// Token: 0x06000014 RID: 20
		[OperationContract]
		bool IsToolsPipeReady();
	}
}
