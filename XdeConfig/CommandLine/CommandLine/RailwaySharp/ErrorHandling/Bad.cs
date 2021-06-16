using System;
using System.Collections.Generic;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x0200001B RID: 27
	internal sealed class Bad<TSuccess, TMessage> : Result<TSuccess, TMessage>
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00003E3F File Offset: 0x0000203F
		public Bad(IEnumerable<TMessage> messages) : base(ResultType.Bad)
		{
			this.messages = messages;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003E4F File Offset: 0x0000204F
		public IEnumerable<TMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x04000036 RID: 54
		private readonly IEnumerable<TMessage> messages;
	}
}
