using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000591 RID: 1425
	public class IndexExpressionAst : ExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003B15 RID: 15125 RVA: 0x00136BCA File Offset: 0x00134DCA
		public IndexExpressionAst(IScriptExtent extent, ExpressionAst target, ExpressionAst index) : base(extent)
		{
			if (target == null || index == null)
			{
				throw PSTraceSource.NewArgumentNullException((target == null) ? "target" : "index");
			}
			this.Target = target;
			base.SetParent(target);
			this.Index = index;
			base.SetParent(index);
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x00136C0A File Offset: 0x00134E0A
		// (set) Token: 0x06003B17 RID: 15127 RVA: 0x00136C12 File Offset: 0x00134E12
		public ExpressionAst Target { get; private set; }

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00136C1B File Offset: 0x00134E1B
		// (set) Token: 0x06003B19 RID: 15129 RVA: 0x00136C23 File Offset: 0x00134E23
		public ExpressionAst Index { get; private set; }

		// Token: 0x06003B1A RID: 15130 RVA: 0x00136C2C File Offset: 0x00134E2C
		public override Ast Copy()
		{
			ExpressionAst target = Ast.CopyElement<ExpressionAst>(this.Target);
			ExpressionAst index = Ast.CopyElement<ExpressionAst>(this.Index);
			return new IndexExpressionAst(base.Extent, target, index);
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001370F8 File Offset: 0x001352F8
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			IEnumerable<PSTypeName> targetTypes = this.Target.GetInferredType(context);
			foreach (PSTypeName psType in targetTypes)
			{
				Type type = psType.Type;
				if (type != null)
				{
					if (type.IsArray)
					{
						yield return new PSTypeName(type.GetElementType());
						continue;
					}
					foreach (Type i in type.GetInterfaces())
					{
						if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<, >))
						{
							Type valueType = i.GetGenericArguments()[1];
							if (!valueType.GetTypeInfo().ContainsGenericParameters)
							{
								yield return new PSTypeName(valueType);
							}
						}
					}
					DefaultMemberAttribute defaultMember = type.GetCustomAttributes(true).FirstOrDefault<DefaultMemberAttribute>();
					if (defaultMember != null)
					{
						IEnumerable<MethodInfo> indexers = from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
						where m.Name.Equals("get_" + defaultMember.MemberName)
						select m;
						foreach (MethodInfo indexer in indexers)
						{
							yield return new PSTypeName(indexer.ReturnType);
						}
					}
				}
				yield return psType;
			}
			yield break;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0013711C File Offset: 0x0013531C
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitIndexExpression(this);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x00137128 File Offset: 0x00135328
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitIndexExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Target.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Index.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x00137174 File Offset: 0x00135374
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return new IndexAssignableValue
			{
				IndexExpressionAst = this
			};
		}
	}
}
