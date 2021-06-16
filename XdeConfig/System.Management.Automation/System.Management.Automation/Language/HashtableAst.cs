using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058C RID: 1420
	public class HashtableAst : ExpressionAst
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x001364E7 File Offset: 0x001346E7
		public HashtableAst(IScriptExtent extent, IEnumerable<Tuple<ExpressionAst, StatementAst>> keyValuePairs) : base(extent)
		{
			if (keyValuePairs != null)
			{
				this.KeyValuePairs = new ReadOnlyCollection<Tuple<ExpressionAst, StatementAst>>(keyValuePairs.ToArray<Tuple<ExpressionAst, StatementAst>>());
				base.SetParents<ExpressionAst, StatementAst>(this.KeyValuePairs);
				return;
			}
			this.KeyValuePairs = HashtableAst.EmptyKeyValuePairs;
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x0013651C File Offset: 0x0013471C
		// (set) Token: 0x06003AEA RID: 15082 RVA: 0x00136524 File Offset: 0x00134724
		public ReadOnlyCollection<Tuple<ExpressionAst, StatementAst>> KeyValuePairs { get; private set; }

		// Token: 0x06003AEB RID: 15083 RVA: 0x00136530 File Offset: 0x00134730
		public override Ast Copy()
		{
			List<Tuple<ExpressionAst, StatementAst>> list = null;
			if (this.KeyValuePairs.Count > 0)
			{
				list = new List<Tuple<ExpressionAst, StatementAst>>(this.KeyValuePairs.Count);
				for (int i = 0; i < this.KeyValuePairs.Count; i++)
				{
					Tuple<ExpressionAst, StatementAst> tuple = this.KeyValuePairs[i];
					ExpressionAst item = Ast.CopyElement<ExpressionAst>(tuple.Item1);
					StatementAst item2 = Ast.CopyElement<StatementAst>(tuple.Item2);
					list.Add(Tuple.Create<ExpressionAst, StatementAst>(item, item2));
				}
			}
			return new HashtableAst(base.Extent, list);
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x001365B4 File Offset: 0x001347B4
		public override Type StaticType
		{
			get
			{
				return typeof(Hashtable);
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00136690 File Offset: 0x00134890
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(typeof(Hashtable));
			yield break;
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x001366AD File Offset: 0x001348AD
		// (set) Token: 0x06003AEF RID: 15087 RVA: 0x001366B5 File Offset: 0x001348B5
		internal bool IsSchemaElement { get; set; }

		// Token: 0x06003AF0 RID: 15088 RVA: 0x001366BE File Offset: 0x001348BE
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitHashtable(this);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001366C8 File Offset: 0x001348C8
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitHashtable(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.KeyValuePairs.Count; i++)
				{
					Tuple<ExpressionAst, StatementAst> tuple = this.KeyValuePairs[i];
					astVisitAction = tuple.Item1.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
					astVisitAction = tuple.Item2.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001D7E RID: 7550
		private static readonly ReadOnlyCollection<Tuple<ExpressionAst, StatementAst>> EmptyKeyValuePairs = new ReadOnlyCollection<Tuple<ExpressionAst, StatementAst>>(new Tuple<ExpressionAst, StatementAst>[0]);
	}
}
