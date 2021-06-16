using System;

namespace System.Management.Automation
{
	// Token: 0x02000415 RID: 1045
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CmdletAttribute : CmdletCommonMetadataAttribute
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x00100298 File Offset: 0x000FE498
		public string NounName
		{
			get
			{
				return this.nounName;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x001002A0 File Offset: 0x000FE4A0
		public string VerbName
		{
			get
			{
				return this.verbName;
			}
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x001002A8 File Offset: 0x000FE4A8
		public CmdletAttribute(string verbName, string nounName)
		{
			if (string.IsNullOrEmpty(nounName))
			{
				throw PSTraceSource.NewArgumentException("nounName");
			}
			if (string.IsNullOrEmpty(verbName))
			{
				throw PSTraceSource.NewArgumentException("verbName");
			}
			this.nounName = nounName;
			this.verbName = verbName;
		}

		// Token: 0x0400188E RID: 6286
		private string nounName;

		// Token: 0x0400188F RID: 6287
		private string verbName;
	}
}
