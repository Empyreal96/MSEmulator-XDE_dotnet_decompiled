using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200026F RID: 623
	[Serializable]
	public class InvalidJobStateException : SystemException
	{
		// Token: 0x06001D68 RID: 7528 RVA: 0x000A9FC9 File Offset: 0x000A81C9
		public InvalidJobStateException() : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidJobStateGeneral, new object[0]))
		{
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x000A9FE1 File Offset: 0x000A81E1
		public InvalidJobStateException(string message) : base(message)
		{
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000A9FEA File Offset: 0x000A81EA
		public InvalidJobStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x000A9FF4 File Offset: 0x000A81F4
		public InvalidJobStateException(JobState currentState, string actionMessage) : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidJobStateSpecific, new object[]
		{
			currentState,
			actionMessage
		}))
		{
			this.currState = currentState;
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x000AA02D File Offset: 0x000A822D
		internal InvalidJobStateException(JobState currentState) : base(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidJobStateGeneral, new object[0]))
		{
			this.currState = currentState;
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000AA04C File Offset: 0x000A824C
		protected InvalidJobStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000AA056 File Offset: 0x000A8256
		public JobState CurrentState
		{
			get
			{
				return this.currState;
			}
		}

		// Token: 0x04000D0F RID: 3343
		[NonSerialized]
		private JobState currState;
	}
}
