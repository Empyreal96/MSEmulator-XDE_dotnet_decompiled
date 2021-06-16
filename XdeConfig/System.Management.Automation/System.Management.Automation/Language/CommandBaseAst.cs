using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200056B RID: 1387
	public abstract class CommandBaseAst : StatementAst
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x001300AE File Offset: 0x0012E2AE
		protected CommandBaseAst(IScriptExtent extent, IEnumerable<RedirectionAst> redirections) : base(extent)
		{
			if (redirections != null)
			{
				this.Redirections = new ReadOnlyCollection<RedirectionAst>(redirections.ToArray<RedirectionAst>());
				base.SetParents<RedirectionAst>(this.Redirections);
				return;
			}
			this.Redirections = CommandBaseAst.EmptyRedirections;
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x001300E3 File Offset: 0x0012E2E3
		// (set) Token: 0x06003985 RID: 14725 RVA: 0x001300EB File Offset: 0x0012E2EB
		public ReadOnlyCollection<RedirectionAst> Redirections { get; private set; }

		// Token: 0x04001D13 RID: 7443
		internal const int MaxRedirections = 7;

		// Token: 0x04001D14 RID: 7444
		private static readonly ReadOnlyCollection<RedirectionAst> EmptyRedirections = new ReadOnlyCollection<RedirectionAst>(new RedirectionAst[0]);
	}
}
