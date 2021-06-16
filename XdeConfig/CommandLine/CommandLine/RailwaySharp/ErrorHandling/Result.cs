using System;
using System.Linq;

namespace RailwaySharp.ErrorHandling
{
	// Token: 0x02000019 RID: 25
	internal abstract class Result<TSuccess, TMessage>
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00003D4C File Offset: 0x00001F4C
		protected Result(ResultType tag)
		{
			this.tag = tag;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003D5B File Offset: 0x00001F5B
		public ResultType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003D64 File Offset: 0x00001F64
		public override string ToString()
		{
			if (this.Tag == ResultType.Ok)
			{
				Ok<TSuccess, TMessage> ok = (Ok<TSuccess, TMessage>)this;
				return string.Format("OK: {0} - {1}", ok.Success, string.Join(Environment.NewLine, from v in ok.Messages
				select v.ToString()));
			}
			Bad<TSuccess, TMessage> bad = (Bad<TSuccess, TMessage>)this;
			return string.Format("Error: {0}", string.Join(Environment.NewLine, from v in bad.Messages
			select v.ToString()));
		}

		// Token: 0x04000034 RID: 52
		private readonly ResultType tag;
	}
}
