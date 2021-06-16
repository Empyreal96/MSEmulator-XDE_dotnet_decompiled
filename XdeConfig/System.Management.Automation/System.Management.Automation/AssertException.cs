using System;

namespace System.Management.Automation
{
	// Token: 0x02000866 RID: 2150
	internal class AssertException : SystemException
	{
		// Token: 0x0600529C RID: 21148 RVA: 0x001B8F2F File Offset: 0x001B712F
		internal AssertException(string message) : base(message)
		{
			this.stackTrace = Diagnostics.StackTrace(3);
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x0600529D RID: 21149 RVA: 0x001B8F44 File Offset: 0x001B7144
		public override string StackTrace
		{
			get
			{
				return this.stackTrace;
			}
		}

		// Token: 0x04002A80 RID: 10880
		private string stackTrace;
	}
}
