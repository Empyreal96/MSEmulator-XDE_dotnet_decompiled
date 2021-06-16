using System;
using System.ComponentModel;
using System.ServiceModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000047 RID: 71
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeAutomationSDCardPipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000188 RID: 392
		// (remove) Token: 0x06000189 RID: 393
		event EventHandler<InsertEjectCompletedEventArgs> InsertCompleted;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600018A RID: 394
		// (remove) Token: 0x0600018B RID: 395
		event EventHandler<InsertEjectCompletedEventArgs> EjectCompleted;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600018C RID: 396
		// (remove) Token: 0x0600018D RID: 397
		event EventHandler<UpdateSyncProgressEventArgs> ProgressBarUpdated;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600018E RID: 398
		bool IsSDCardInserted { [OperationContract] get; }

		// Token: 0x0600018F RID: 399
		[OperationContract(IsOneWay = false)]
		bool InsertSDCard(string folderPath);

		// Token: 0x06000190 RID: 400
		[OperationContract(IsOneWay = false)]
		bool EjectSDCard(string folderPath, bool shouldSyncOnEject);

		// Token: 0x06000191 RID: 401
		[OperationContract]
		void CancelSync();
	}
}
