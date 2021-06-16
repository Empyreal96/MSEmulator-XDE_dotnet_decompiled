using System;

namespace System.Management.Automation
{
	// Token: 0x02000821 RID: 2081
	internal class VariableScopeItemSearcher : ScopedItemSearcher<PSVariable>
	{
		// Token: 0x06004FEA RID: 20458 RVA: 0x001A79EE File Offset: 0x001A5BEE
		public VariableScopeItemSearcher(SessionStateInternal sessionState, VariablePath lookupPath, CommandOrigin origin) : base(sessionState, lookupPath)
		{
			this._origin = origin;
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x001A7A00 File Offset: 0x001A5C00
		protected override bool GetScopeItem(SessionStateScope scope, VariablePath name, out PSVariable variable)
		{
			bool result = true;
			variable = scope.GetVariable(name.QualifiedName, this._origin);
			if (variable == null || (variable.IsPrivate && scope != this.sessionState.CurrentScope))
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040028E4 RID: 10468
		private readonly CommandOrigin _origin;
	}
}
