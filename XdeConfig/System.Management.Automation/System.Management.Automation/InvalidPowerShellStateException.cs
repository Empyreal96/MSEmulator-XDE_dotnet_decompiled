using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200022F RID: 559
	[Serializable]
	public class InvalidPowerShellStateException : SystemException
	{
		// Token: 0x06001A1B RID: 6683 RVA: 0x0009B6A4 File Offset: 0x000998A4
		public InvalidPowerShellStateException() : base(StringUtil.Format(PowerShellStrings.InvalidPowerShellStateGeneral, new object[0]))
		{
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0009B6BC File Offset: 0x000998BC
		public InvalidPowerShellStateException(string message) : base(message)
		{
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0009B6C5 File Offset: 0x000998C5
		public InvalidPowerShellStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0009B6CF File Offset: 0x000998CF
		internal InvalidPowerShellStateException(PSInvocationState currentState) : base(StringUtil.Format(PowerShellStrings.InvalidPowerShellStateGeneral, new object[0]))
		{
			this.currState = currentState;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0009B6EE File Offset: 0x000998EE
		protected InvalidPowerShellStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0009B6F8 File Offset: 0x000998F8
		public PSInvocationState CurrentState
		{
			get
			{
				return this.currState;
			}
		}

		// Token: 0x04000AC3 RID: 2755
		[NonSerialized]
		private PSInvocationState currState;
	}
}
