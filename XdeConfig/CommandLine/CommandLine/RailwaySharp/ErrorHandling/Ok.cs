using System;
using System.Collections.Generic;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x0200001A RID: 26
	internal sealed class Ok<TSuccess, TMessage> : Result<TSuccess, TMessage>
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003E0F File Offset: 0x0000200F
		public Ok(TSuccess success, IEnumerable<TMessage> messages) : base(ResultType.Ok)
		{
			this.value = Tuple.Create<TSuccess, IEnumerable<TMessage>>(success, messages);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003E25 File Offset: 0x00002025
		public TSuccess Success
		{
			get
			{
				return this.value.Item1;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003E32 File Offset: 0x00002032
		public IEnumerable<TMessage> Messages
		{
			get
			{
				return this.value.Item2;
			}
		}

		// Token: 0x04000035 RID: 53
		private readonly Tuple<TSuccess, IEnumerable<TMessage>> value;
	}
}
