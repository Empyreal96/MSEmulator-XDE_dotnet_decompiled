using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000763 RID: 1891
	public class AliasProviderDynamicParameters
	{
		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x0018BE0A File Offset: 0x0018A00A
		// (set) Token: 0x06004B9F RID: 19359 RVA: 0x0018BE12 File Offset: 0x0018A012
		[Parameter]
		public ScopedItemOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.optionsSet = true;
				this.options = value;
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x0018BE22 File Offset: 0x0018A022
		internal bool OptionsSet
		{
			get
			{
				return this.optionsSet;
			}
		}

		// Token: 0x0400247E RID: 9342
		private ScopedItemOptions options;

		// Token: 0x0400247F RID: 9343
		private bool optionsSet;
	}
}
