using System;
using System.Diagnostics;

namespace System.Management.Automation
{
	// Token: 0x020008B0 RID: 2224
	internal class MonadTraceSource : TraceSource
	{
		// Token: 0x060054CE RID: 21710 RVA: 0x001BF607 File Offset: 0x001BD807
		internal MonadTraceSource(string name) : base(name)
		{
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x001BF610 File Offset: 0x001BD810
		protected override string[] GetSupportedAttributes()
		{
			return new string[]
			{
				"Options"
			};
		}
	}
}
