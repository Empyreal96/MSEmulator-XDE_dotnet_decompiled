using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200078C RID: 1932
	public class FunctionProviderDynamicParameters
	{
		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x001954EE File Offset: 0x001936EE
		// (set) Token: 0x06004CAA RID: 19626 RVA: 0x001954F6 File Offset: 0x001936F6
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

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06004CAB RID: 19627 RVA: 0x00195506 File Offset: 0x00193706
		internal bool OptionsSet
		{
			get
			{
				return this.optionsSet;
			}
		}

		// Token: 0x04002538 RID: 9528
		private ScopedItemOptions options;

		// Token: 0x04002539 RID: 9529
		private bool optionsSet;
	}
}
