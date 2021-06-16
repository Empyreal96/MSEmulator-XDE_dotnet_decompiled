using System;
using System.ComponentModel;
using System.ServiceModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000013 RID: 19
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeAutomationPipe : INotifyPropertyChanged
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007A RID: 122
		bool IsConnected { [OperationContract(IsOneWay = false)] get; }
	}
}
