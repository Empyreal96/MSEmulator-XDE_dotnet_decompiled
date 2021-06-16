using System;
using System.ComponentModel;
using System.ServiceModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000053 RID: 83
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeAutomationNotificationSimulationPipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x060001A4 RID: 420
		[OperationContract]
		int SendNotificationSimulationPayload(string uri, int type, string payload, string tagId, string groupId);

		// Token: 0x060001A5 RID: 421
		[OperationContract]
		void SetNotificationSimulationEnabled(bool enabled);

		// Token: 0x060001A6 RID: 422
		[OperationContract]
		string[] GetNotificationUriListForApp(string appId);

		// Token: 0x060001A7 RID: 423
		[OperationContract]
		string[] GetNotificationSimulationAppList();
	}
}
