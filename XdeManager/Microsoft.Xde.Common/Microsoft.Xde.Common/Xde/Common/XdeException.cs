using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200007A RID: 122
	public class XdeException : Exception
	{
		// Token: 0x060002DC RID: 732 RVA: 0x00007B13 File Offset: 0x00005D13
		public XdeException(string message, XdeError err) : base(message)
		{
			this.err = err;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00007B23 File Offset: 0x00005D23
		public XdeError Error
		{
			get
			{
				return this.err;
			}
		}

		// Token: 0x040001BA RID: 442
		private XdeError err;
	}
}
