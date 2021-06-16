using System;

namespace System.Management.Automation
{
	// Token: 0x020008AF RID: 2223
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class TraceSourceAttribute : Attribute
	{
		// Token: 0x060054CA RID: 21706 RVA: 0x001BF5D8 File Offset: 0x001BD7D8
		internal TraceSourceAttribute(string category, string description)
		{
			this.category = category;
			this.description = description;
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x060054CB RID: 21707 RVA: 0x001BF5EE File Offset: 0x001BD7EE
		internal string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x060054CC RID: 21708 RVA: 0x001BF5F6 File Offset: 0x001BD7F6
		// (set) Token: 0x060054CD RID: 21709 RVA: 0x001BF5FE File Offset: 0x001BD7FE
		internal string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x04002B98 RID: 11160
		private string category;

		// Token: 0x04002B99 RID: 11161
		private string description;
	}
}
