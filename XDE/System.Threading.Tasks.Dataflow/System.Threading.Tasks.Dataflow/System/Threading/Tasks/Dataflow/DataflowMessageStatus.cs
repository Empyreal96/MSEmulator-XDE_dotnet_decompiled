using System;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Represents the status of a <see cref="T:System.Threading.Tasks.Dataflow.DataflowMessageHeader" /> when passed between dataflow blocks.</summary>
	// Token: 0x0200002D RID: 45
	public enum DataflowMessageStatus
	{
		/// <summary>Indicates that the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> accepted the message. Once a target has accepted a message, it is wholly owned by the target.</summary>
		// Token: 0x0400007B RID: 123
		Accepted,
		/// <summary>Indicates that the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> declined the message. The <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> still owns the message.</summary>
		// Token: 0x0400007C RID: 124
		Declined,
		/// <summary>Indicates that the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> postponed the message for potential consumption at a later time.  The <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> still owns the message.</summary>
		// Token: 0x0400007D RID: 125
		Postponed,
		/// <summary>Indicates that the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> tried to accept the message from the <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" />, but the message was no longer available.</summary>
		// Token: 0x0400007E RID: 126
		NotAvailable,
		/// <summary>Indicates that the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> declined the message. The <see cref="T:System.Threading.Tasks.Dataflow.ISourceBlock`1" /> still owns the message.  Additionally, the <see cref="T:System.Threading.Tasks.Dataflow.ITargetBlock`1" /> will decline all future messages sent by the source.</summary>
		// Token: 0x0400007F RID: 127
		DecliningPermanently
	}
}
