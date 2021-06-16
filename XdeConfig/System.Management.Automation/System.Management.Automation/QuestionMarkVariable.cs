using System;

namespace System.Management.Automation
{
	// Token: 0x02000839 RID: 2105
	internal class QuestionMarkVariable : PSVariable
	{
		// Token: 0x0600510D RID: 20749 RVA: 0x001B025B File Offset: 0x001AE45B
		internal QuestionMarkVariable(ExecutionContext context) : base("?", true, ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope, RunspaceInit.DollarHookDescription)
		{
			this._context = context;
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x001B027C File Offset: 0x001AE47C
		// (set) Token: 0x0600510F RID: 20751 RVA: 0x001B0294 File Offset: 0x001AE494
		public override object Value
		{
			get
			{
				base.DebuggerCheckVariableRead();
				return this._context.QuestionMarkVariableValue;
			}
			set
			{
				base.Value = value;
			}
		}

		// Token: 0x04002971 RID: 10609
		private readonly ExecutionContext _context;
	}
}
