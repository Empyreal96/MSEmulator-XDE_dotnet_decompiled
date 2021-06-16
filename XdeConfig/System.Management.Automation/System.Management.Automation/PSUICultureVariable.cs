using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x0200083B RID: 2107
	internal class PSUICultureVariable : PSVariable
	{
		// Token: 0x06005112 RID: 20754 RVA: 0x001B02C9 File Offset: 0x001AE4C9
		internal PSUICultureVariable() : base("PSUICulture", true, ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope, RunspaceInit.DollarPSUICultureDescription)
		{
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06005113 RID: 20755 RVA: 0x001B02E3 File Offset: 0x001AE4E3
		public override object Value
		{
			get
			{
				base.DebuggerCheckVariableRead();
				return CultureInfo.CurrentUICulture.Name;
			}
		}
	}
}
