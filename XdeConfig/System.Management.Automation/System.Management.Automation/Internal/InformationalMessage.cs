using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x020002A8 RID: 680
	internal class InformationalMessage
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x000BEA41 File Offset: 0x000BCC41
		internal object Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x000BEA49 File Offset: 0x000BCC49
		internal RemotingDataType DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000BEA51 File Offset: 0x000BCC51
		internal InformationalMessage(object message, RemotingDataType dataType)
		{
			this.dataType = dataType;
			this.message = message;
		}

		// Token: 0x04000E9A RID: 3738
		private object message;

		// Token: 0x04000E9B RID: 3739
		private RemotingDataType dataType;
	}
}
