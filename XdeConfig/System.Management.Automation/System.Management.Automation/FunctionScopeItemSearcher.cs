using System;

namespace System.Management.Automation
{
	// Token: 0x02000823 RID: 2083
	internal class FunctionScopeItemSearcher : ScopedItemSearcher<FunctionInfo>
	{
		// Token: 0x06004FEE RID: 20462 RVA: 0x001A7A89 File Offset: 0x001A5C89
		public FunctionScopeItemSearcher(SessionStateInternal sessionState, VariablePath lookupPath, CommandOrigin origin) : base(sessionState, lookupPath)
		{
			this._origin = origin;
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001A7AA8 File Offset: 0x001A5CA8
		protected override bool GetScopeItem(SessionStateScope scope, VariablePath path, out FunctionInfo script)
		{
			bool result = true;
			this.name = (path.IsFunction ? path.UnqualifiedPath : path.QualifiedName);
			script = scope.GetFunction(this.name);
			if (script != null)
			{
				FilterInfo filterInfo = script as FilterInfo;
				bool flag;
				if (filterInfo != null)
				{
					flag = ((filterInfo.Options & ScopedItemOptions.Private) != ScopedItemOptions.None);
				}
				else
				{
					flag = ((script.Options & ScopedItemOptions.Private) != ScopedItemOptions.None);
				}
				if (flag && scope != this.sessionState.CurrentScope)
				{
					result = false;
				}
				else
				{
					SessionState.ThrowIfNotVisible(this._origin, script);
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x001A7B38 File Offset: 0x001A5D38
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040028E5 RID: 10469
		private readonly CommandOrigin _origin;

		// Token: 0x040028E6 RID: 10470
		private string name = string.Empty;
	}
}
