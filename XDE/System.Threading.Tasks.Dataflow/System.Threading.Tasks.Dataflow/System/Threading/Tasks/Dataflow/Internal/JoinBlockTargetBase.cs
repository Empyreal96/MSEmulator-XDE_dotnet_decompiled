using System;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000068 RID: 104
	internal abstract class JoinBlockTargetBase
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000378 RID: 888
		internal abstract bool IsDecliningPermanently { get; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000379 RID: 889
		internal abstract bool HasAtLeastOneMessageAvailable { get; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600037A RID: 890
		internal abstract bool HasAtLeastOnePostponedMessage { get; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600037B RID: 891
		internal abstract int NumberOfMessagesAvailableOrPostponed { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600037C RID: 892
		internal abstract bool HasTheHighestNumberOfMessagesAvailable { get; }

		// Token: 0x0600037D RID: 893
		internal abstract bool ReserveOneMessage();

		// Token: 0x0600037E RID: 894
		internal abstract bool ConsumeReservedMessage();

		// Token: 0x0600037F RID: 895
		internal abstract bool ConsumeOnePostponedMessage();

		// Token: 0x06000380 RID: 896
		internal abstract void ReleaseReservedMessage();

		// Token: 0x06000381 RID: 897
		internal abstract void ClearReservation();

		// Token: 0x06000382 RID: 898 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		public void Complete()
		{
			this.CompleteCore(null, false, false);
		}

		// Token: 0x06000383 RID: 899
		internal abstract void CompleteCore(Exception exception, bool dropPendingMessages, bool releaseReservedMessages);

		// Token: 0x06000384 RID: 900
		internal abstract void CompleteOncePossible();
	}
}
