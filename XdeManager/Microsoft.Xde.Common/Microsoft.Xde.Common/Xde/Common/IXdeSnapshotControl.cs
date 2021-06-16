using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000054 RID: 84
	public interface IXdeSnapshotControl : INotifyPropertyChanged
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001A8 RID: 424
		bool CanDoOperation { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001A9 RID: 425
		// (set) Token: 0x060001AA RID: 426
		string DefaultSnapshot { get; set; }

		// Token: 0x060001AB RID: 427
		void TakeNewSnapshot(string name);

		// Token: 0x060001AC RID: 428
		void DeleteSnapshot(string uniqueId);

		// Token: 0x060001AD RID: 429
		void ResumeSnapshot(string uniqueId);
	}
}
