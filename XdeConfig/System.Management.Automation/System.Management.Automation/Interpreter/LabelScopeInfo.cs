using System;
using System.Linq.Expressions;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D8 RID: 1752
	internal sealed class LabelScopeInfo
	{
		// Token: 0x060048AA RID: 18602 RVA: 0x0017F2BF File Offset: 0x0017D4BF
		internal LabelScopeInfo(LabelScopeInfo parent, LabelScopeKind kind)
		{
			this.Parent = parent;
			this.Kind = kind;
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x060048AB RID: 18603 RVA: 0x0017F2D8 File Offset: 0x0017D4D8
		internal bool CanJumpInto
		{
			get
			{
				switch (this.Kind)
				{
				case LabelScopeKind.Statement:
				case LabelScopeKind.Block:
				case LabelScopeKind.Switch:
				case LabelScopeKind.Lambda:
					return true;
				default:
					return false;
				}
			}
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0017F307 File Offset: 0x0017D507
		internal bool ContainsTarget(LabelTarget target)
		{
			return this.Labels != null && this.Labels.ContainsKey(target);
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0017F31F File Offset: 0x0017D51F
		internal bool TryGetLabelInfo(LabelTarget target, out LabelInfo info)
		{
			if (this.Labels == null)
			{
				info = null;
				return false;
			}
			return this.Labels.TryGetValue(target, out info);
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0017F33B File Offset: 0x0017D53B
		internal void AddLabelInfo(LabelTarget target, LabelInfo info)
		{
			if (this.Labels == null)
			{
				this.Labels = new HybridReferenceDictionary<LabelTarget, LabelInfo>();
			}
			this.Labels[target] = info;
		}

		// Token: 0x04002382 RID: 9090
		private HybridReferenceDictionary<LabelTarget, LabelInfo> Labels;

		// Token: 0x04002383 RID: 9091
		internal readonly LabelScopeKind Kind;

		// Token: 0x04002384 RID: 9092
		internal readonly LabelScopeInfo Parent;
	}
}
