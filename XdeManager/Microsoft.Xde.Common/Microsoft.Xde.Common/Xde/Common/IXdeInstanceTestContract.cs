using System;
using System.ServiceModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200004D RID: 77
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeInstanceTestContract
	{
		// Token: 0x0600019D RID: 413
		[OperationContract]
		void Close();
	}
}
