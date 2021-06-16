using System;

namespace System.Management.Automation
{
	// Token: 0x02000822 RID: 2082
	internal class AliasScopeItemSearcher : ScopedItemSearcher<AliasInfo>
	{
		// Token: 0x06004FEC RID: 20460 RVA: 0x001A7A41 File Offset: 0x001A5C41
		public AliasScopeItemSearcher(SessionStateInternal sessionState, VariablePath lookupPath) : base(sessionState, lookupPath)
		{
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001A7A4C File Offset: 0x001A5C4C
		protected override bool GetScopeItem(SessionStateScope scope, VariablePath name, out AliasInfo alias)
		{
			bool result = true;
			alias = scope.GetAlias(name.QualifiedName);
			if (alias == null || ((alias.Options & ScopedItemOptions.Private) != ScopedItemOptions.None && scope != this.sessionState.CurrentScope))
			{
				result = false;
			}
			return result;
		}
	}
}
