using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x0200083A RID: 2106
	internal class PSCultureVariable : PSVariable
	{
		// Token: 0x06005110 RID: 20752 RVA: 0x001B029D File Offset: 0x001AE49D
		internal PSCultureVariable() : base("PSCulture", true, ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope, RunspaceInit.DollarPSCultureDescription)
		{
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06005111 RID: 20753 RVA: 0x001B02B7 File Offset: 0x001AE4B7
		public override object Value
		{
			get
			{
				base.DebuggerCheckVariableRead();
				return CultureInfo.CurrentCulture.Name;
			}
		}
	}
}
