using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200053B RID: 1339
	public class ErrorStatementAst : PipelineBaseAst
	{
		// Token: 0x0600379E RID: 14238 RVA: 0x0012A996 File Offset: 0x00128B96
		internal ErrorStatementAst(IScriptExtent extent, IEnumerable<Ast> nestedAsts = null) : base(extent)
		{
			if (nestedAsts != null && nestedAsts.Any<Ast>())
			{
				this.NestedAst = new ReadOnlyCollection<Ast>(nestedAsts.ToArray<Ast>());
				base.SetParents<Ast>(this.NestedAst);
			}
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0012A9C8 File Offset: 0x00128BC8
		internal ErrorStatementAst(IScriptExtent extent, Token kind, IEnumerable<Ast> nestedAsts = null) : base(extent)
		{
			if (kind == null)
			{
				throw PSTraceSource.NewArgumentNullException("kind");
			}
			this.Kind = kind;
			if (nestedAsts != null && nestedAsts.Any<Ast>())
			{
				this.NestedAst = new ReadOnlyCollection<Ast>(nestedAsts.ToArray<Ast>());
				base.SetParents<Ast>(this.NestedAst);
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x0012AA1C File Offset: 0x00128C1C
		internal ErrorStatementAst(IScriptExtent extent, Token kind, IEnumerable<KeyValuePair<string, Tuple<Token, Ast>>> flags, IEnumerable<Ast> conditions, IEnumerable<Ast> bodies) : base(extent)
		{
			if (kind == null)
			{
				throw PSTraceSource.NewArgumentNullException("kind");
			}
			this.Kind = kind;
			if (flags != null && flags.Any<KeyValuePair<string, Tuple<Token, Ast>>>())
			{
				this.Flags = new Dictionary<string, Tuple<Token, Ast>>(StringComparer.OrdinalIgnoreCase);
				foreach (KeyValuePair<string, Tuple<Token, Ast>> keyValuePair in flags)
				{
					if (!this.Flags.ContainsKey(keyValuePair.Key))
					{
						this.Flags.Add(keyValuePair.Key, keyValuePair.Value);
						if (keyValuePair.Value.Item2 != null)
						{
							base.SetParent(keyValuePair.Value.Item2);
						}
					}
				}
			}
			if (conditions != null && conditions.Any<Ast>())
			{
				this.Conditions = new ReadOnlyCollection<Ast>(conditions.ToArray<Ast>());
				base.SetParents<Ast>(this.Conditions);
			}
			if (bodies != null && bodies.Any<Ast>())
			{
				this.Bodies = new ReadOnlyCollection<Ast>(bodies.ToArray<Ast>());
				base.SetParents<Ast>(this.Bodies);
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x0012AB40 File Offset: 0x00128D40
		// (set) Token: 0x060037A2 RID: 14242 RVA: 0x0012AB48 File Offset: 0x00128D48
		public Token Kind { get; private set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x0012AB51 File Offset: 0x00128D51
		// (set) Token: 0x060037A4 RID: 14244 RVA: 0x0012AB59 File Offset: 0x00128D59
		public Dictionary<string, Tuple<Token, Ast>> Flags { get; private set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0012AB62 File Offset: 0x00128D62
		// (set) Token: 0x060037A6 RID: 14246 RVA: 0x0012AB6A File Offset: 0x00128D6A
		public ReadOnlyCollection<Ast> Conditions { get; private set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0012AB73 File Offset: 0x00128D73
		// (set) Token: 0x060037A8 RID: 14248 RVA: 0x0012AB7B File Offset: 0x00128D7B
		public ReadOnlyCollection<Ast> Bodies { get; private set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x060037A9 RID: 14249 RVA: 0x0012AB84 File Offset: 0x00128D84
		// (set) Token: 0x060037AA RID: 14250 RVA: 0x0012AB8C File Offset: 0x00128D8C
		public ReadOnlyCollection<Ast> NestedAst { get; private set; }

		// Token: 0x060037AB RID: 14251 RVA: 0x0012AB98 File Offset: 0x00128D98
		public override Ast Copy()
		{
			if (this.Kind == null)
			{
				Ast[] nestedAsts = Ast.CopyElements<Ast>(this.NestedAst);
				return new ErrorStatementAst(base.Extent, nestedAsts);
			}
			if (this.Flags != null || this.Conditions != null || this.Bodies != null)
			{
				Ast[] conditions = Ast.CopyElements<Ast>(this.Conditions);
				Ast[] bodies = Ast.CopyElements<Ast>(this.Bodies);
				Dictionary<string, Tuple<Token, Ast>> dictionary = null;
				if (this.Flags != null)
				{
					dictionary = new Dictionary<string, Tuple<Token, Ast>>(StringComparer.OrdinalIgnoreCase);
					foreach (KeyValuePair<string, Tuple<Token, Ast>> keyValuePair in this.Flags)
					{
						Ast item = Ast.CopyElement<Ast>(keyValuePair.Value.Item2);
						dictionary.Add(keyValuePair.Key, new Tuple<Token, Ast>(keyValuePair.Value.Item1, item));
					}
				}
				return new ErrorStatementAst(base.Extent, this.Kind, dictionary, conditions, bodies);
			}
			Ast[] nestedAsts2 = Ast.CopyElements<Ast>(this.NestedAst);
			return new ErrorStatementAst(base.Extent, this.Kind, nestedAsts2);
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0012ACD0 File Offset: 0x00128ED0
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Conditions.Concat(this.Bodies).Concat(this.NestedAst).SelectMany((Ast nestedAst) => nestedAst.GetInferredType(context));
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x0012AD17 File Offset: 0x00128F17
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitErrorStatement(this);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x0012AD20 File Offset: 0x00128F20
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitErrorStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.NestedAst != null)
			{
				for (int i = 0; i < this.NestedAst.Count; i++)
				{
					Ast ast = this.NestedAst[i];
					astVisitAction = ast.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.Flags != null)
			{
				foreach (Tuple<Token, Ast> tuple in this.Flags.Values)
				{
					if (tuple.Item2 != null)
					{
						astVisitAction = tuple.Item2.InternalVisit(visitor);
						if (astVisitAction != AstVisitAction.Continue)
						{
							break;
						}
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.Conditions != null)
			{
				for (int j = 0; j < this.Conditions.Count; j++)
				{
					Ast ast2 = this.Conditions[j];
					astVisitAction = ast2.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.Bodies != null)
			{
				for (int k = 0; k < this.Bodies.Count; k++)
				{
					Ast ast3 = this.Bodies[k];
					astVisitAction = ast3.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
