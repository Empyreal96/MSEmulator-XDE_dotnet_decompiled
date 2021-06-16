using System;

namespace System.Management.Automation
{
	// Token: 0x02000842 RID: 2114
	internal class LocalVariable : PSVariable
	{
		// Token: 0x0600516E RID: 20846 RVA: 0x001B2280 File Offset: 0x001B0480
		public LocalVariable(string name, MutableTuple tuple, int tupleSlot) : base(name, false)
		{
			this._tuple = tuple;
			this._tupleSlot = tupleSlot;
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x001B2298 File Offset: 0x001B0498
		// (set) Token: 0x06005170 RID: 20848 RVA: 0x001B22A0 File Offset: 0x001B04A0
		public override ScopedItemOptions Options
		{
			get
			{
				return base.Options;
			}
			set
			{
				if (value != base.Options)
				{
					SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Variable, "VariableOptionsNotSettable", SessionStateStrings.VariableOptionsNotSettable);
					throw ex;
				}
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x001B22CF File Offset: 0x001B04CF
		// (set) Token: 0x06005172 RID: 20850 RVA: 0x001B22E8 File Offset: 0x001B04E8
		public override object Value
		{
			get
			{
				base.DebuggerCheckVariableRead();
				return this._tuple.GetValue(this._tupleSlot);
			}
			set
			{
				this._tuple.SetValue(this._tupleSlot, value);
				base.DebuggerCheckVariableWrite();
			}
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x001B2302 File Offset: 0x001B0502
		internal override void SetValueRaw(object newValue, bool preserveValueTypeSemantics)
		{
			if (preserveValueTypeSemantics)
			{
				newValue = PSVariable.CopyMutableValues(newValue);
			}
			this.Value = newValue;
		}

		// Token: 0x040029C0 RID: 10688
		private readonly MutableTuple _tuple;

		// Token: 0x040029C1 RID: 10689
		private readonly int _tupleSlot;
	}
}
