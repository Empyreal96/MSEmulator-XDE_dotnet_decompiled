using System;
using System.ServiceModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200003B RID: 59
	public interface IXdeAutomationSDCardPipeCallback
	{
		// Token: 0x0600016C RID: 364
		[OperationContract(IsOneWay = true)]
		void OnInsertCompleted(InsertEjectCompletedEventArgs args);

		// Token: 0x0600016D RID: 365
		[OperationContract(IsOneWay = true)]
		void OnEjectCompleted(InsertEjectCompletedEventArgs args);

		// Token: 0x0600016E RID: 366
		[OperationContract(IsOneWay = true)]
		void OnProgressBarUpdated(UpdateSyncProgressEventArgs args);

		// Token: 0x0600016F RID: 367
		[OperationContract(IsOneWay = true)]
		void OnPropertyChanged(string propertyName);
	}
}
