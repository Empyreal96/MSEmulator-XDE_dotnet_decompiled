using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D6 RID: 1750
	internal sealed class LabelInfo
	{
		// Token: 0x0600489C RID: 18588 RVA: 0x0017EF22 File Offset: 0x0017D122
		internal LabelInfo(LabelTarget node)
		{
			this._node = node;
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x0017EF3C File Offset: 0x0017D13C
		internal BranchLabel GetLabel(LightCompiler compiler)
		{
			this.EnsureLabel(compiler);
			return this._label;
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x0017EF4B File Offset: 0x0017D14B
		internal void Reference(LabelScopeInfo block)
		{
			this._references.Add(block);
			if (this.HasDefinitions)
			{
				this.ValidateJump(block);
			}
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0017EF68 File Offset: 0x0017D168
		internal void Define(LabelScopeInfo block)
		{
			for (LabelScopeInfo labelScopeInfo = block; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
			{
				if (labelScopeInfo.ContainsTarget(this._node))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Label target already defined: {0}", new object[]
					{
						this._node.Name
					}));
				}
			}
			this.AddDefinition(block);
			block.AddLabelInfo(this._node, this);
			if (this.HasDefinitions && !this.HasMultipleDefinitions)
			{
				using (List<LabelScopeInfo>.Enumerator enumerator = this._references.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LabelScopeInfo reference = enumerator.Current;
						this.ValidateJump(reference);
					}
					return;
				}
			}
			if (this._acrossBlockJump)
			{
				throw new InvalidOperationException("Ambiguous jump");
			}
			this._label = null;
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x0017F048 File Offset: 0x0017D248
		private void ValidateJump(LabelScopeInfo reference)
		{
			for (LabelScopeInfo labelScopeInfo = reference; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
			{
				if (this.DefinedIn(labelScopeInfo))
				{
					return;
				}
				if (labelScopeInfo.Kind == LabelScopeKind.Filter)
				{
					break;
				}
			}
			this._acrossBlockJump = true;
			if (this.HasMultipleDefinitions)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Ambiguous jump {0}", new object[]
				{
					this._node.Name
				}));
			}
			LabelScopeInfo labelScopeInfo2 = this.FirstDefinition();
			LabelScopeInfo labelScopeInfo3 = LabelInfo.CommonNode<LabelScopeInfo>(labelScopeInfo2, reference, (LabelScopeInfo b) => b.Parent);
			for (LabelScopeInfo labelScopeInfo4 = reference; labelScopeInfo4 != labelScopeInfo3; labelScopeInfo4 = labelScopeInfo4.Parent)
			{
				if (labelScopeInfo4.Kind == LabelScopeKind.Filter)
				{
					throw new InvalidOperationException("Control cannot leave filter test");
				}
			}
			LabelScopeInfo labelScopeInfo5 = labelScopeInfo2;
			while (labelScopeInfo5 != labelScopeInfo3)
			{
				if (!labelScopeInfo5.CanJumpInto)
				{
					if (labelScopeInfo5.Kind == LabelScopeKind.Expression)
					{
						throw new InvalidOperationException("Control cannot enter an expression");
					}
					throw new InvalidOperationException("Control cannot enter try");
				}
				else
				{
					labelScopeInfo5 = labelScopeInfo5.Parent;
				}
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x0017F13E File Offset: 0x0017D33E
		internal void ValidateFinish()
		{
			if (this._references.Count > 0 && !this.HasDefinitions)
			{
				throw new InvalidOperationException("label target undefined");
			}
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0017F161 File Offset: 0x0017D361
		private void EnsureLabel(LightCompiler compiler)
		{
			if (this._label == null)
			{
				this._label = compiler.Instructions.MakeLabel();
			}
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0017F17C File Offset: 0x0017D37C
		private bool DefinedIn(LabelScopeInfo scope)
		{
			if (this._definitions == scope)
			{
				return true;
			}
			HashSet<LabelScopeInfo> hashSet = this._definitions as HashSet<LabelScopeInfo>;
			return hashSet != null && hashSet.Contains(scope);
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x060048A4 RID: 18596 RVA: 0x0017F1AC File Offset: 0x0017D3AC
		private bool HasDefinitions
		{
			get
			{
				return this._definitions != null;
			}
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x0017F1BC File Offset: 0x0017D3BC
		private LabelScopeInfo FirstDefinition()
		{
			LabelScopeInfo labelScopeInfo = this._definitions as LabelScopeInfo;
			if (labelScopeInfo != null)
			{
				return labelScopeInfo;
			}
			return ((HashSet<LabelScopeInfo>)this._definitions).First<LabelScopeInfo>();
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x0017F1EC File Offset: 0x0017D3EC
		private void AddDefinition(LabelScopeInfo scope)
		{
			if (this._definitions == null)
			{
				this._definitions = scope;
				return;
			}
			HashSet<LabelScopeInfo> hashSet = this._definitions as HashSet<LabelScopeInfo>;
			if (hashSet == null)
			{
				hashSet = (this._definitions = new HashSet<LabelScopeInfo>
				{
					(LabelScopeInfo)this._definitions
				});
			}
			hashSet.Add(scope);
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x060048A7 RID: 18599 RVA: 0x0017F241 File Offset: 0x0017D441
		private bool HasMultipleDefinitions
		{
			get
			{
				return this._definitions is HashSet<LabelScopeInfo>;
			}
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0017F254 File Offset: 0x0017D454
		internal static T CommonNode<T>(T first, T second, Func<T, T> parent) where T : class
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (@default.Equals(first, second))
			{
				return first;
			}
			HashSet<T> hashSet = new HashSet<T>(@default);
			for (T t = first; t != null; t = parent(t))
			{
				hashSet.Add(t);
			}
			for (T t2 = second; t2 != null; t2 = parent(t2))
			{
				if (hashSet.Contains(t2))
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x04002372 RID: 9074
		private readonly LabelTarget _node;

		// Token: 0x04002373 RID: 9075
		private BranchLabel _label;

		// Token: 0x04002374 RID: 9076
		private object _definitions;

		// Token: 0x04002375 RID: 9077
		private readonly List<LabelScopeInfo> _references = new List<LabelScopeInfo>();

		// Token: 0x04002376 RID: 9078
		private bool _acrossBlockJump;
	}
}
