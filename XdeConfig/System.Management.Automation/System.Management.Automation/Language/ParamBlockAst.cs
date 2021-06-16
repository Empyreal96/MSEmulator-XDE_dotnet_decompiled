using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000541 RID: 1345
	public class ParamBlockAst : Ast
	{
		// Token: 0x060037FA RID: 14330 RVA: 0x0012C180 File Offset: 0x0012A380
		public ParamBlockAst(IScriptExtent extent, IEnumerable<AttributeAst> attributes, IEnumerable<ParameterAst> parameters) : base(extent)
		{
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = ParamBlockAst.EmptyAttributeList;
			}
			if (parameters != null)
			{
				this.Parameters = new ReadOnlyCollection<ParameterAst>(parameters.ToArray<ParameterAst>());
				base.SetParents<ParameterAst>(this.Parameters);
				return;
			}
			this.Parameters = ParamBlockAst.EmptyParameterList;
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x0012C1ED File Offset: 0x0012A3ED
		// (set) Token: 0x060037FC RID: 14332 RVA: 0x0012C1F5 File Offset: 0x0012A3F5
		public ReadOnlyCollection<AttributeAst> Attributes { get; private set; }

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x0012C1FE File Offset: 0x0012A3FE
		// (set) Token: 0x060037FE RID: 14334 RVA: 0x0012C206 File Offset: 0x0012A406
		public ReadOnlyCollection<ParameterAst> Parameters { get; private set; }

		// Token: 0x060037FF RID: 14335 RVA: 0x0012C210 File Offset: 0x0012A410
		public override Ast Copy()
		{
			AttributeAst[] attributes = Ast.CopyElements<AttributeAst>(this.Attributes);
			ParameterAst[] parameters = Ast.CopyElements<ParameterAst>(this.Parameters);
			return new ParamBlockAst(base.Extent, attributes, parameters);
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0012C242 File Offset: 0x0012A442
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x0012C249 File Offset: 0x0012A449
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitParamBlock(this);
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x0012C254 File Offset: 0x0012A454
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitParamBlock(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.Attributes.Count; i++)
				{
					AttributeAst attributeAst = this.Attributes[i];
					astVisitAction = attributeAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int j = 0; j < this.Parameters.Count; j++)
				{
					ParameterAst parameterAst = this.Parameters[j];
					astVisitAction = parameterAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x0012C314 File Offset: 0x0012A514
		internal static bool UsesCmdletBinding(IEnumerable<ParameterAst> parameters)
		{
			bool flag = false;
			foreach (ParameterAst parameterAst in parameters)
			{
				flag = parameterAst.Attributes.Any((AttributeBaseAst attribute) => attribute.TypeName.GetReflectionAttributeType() != null && attribute.TypeName.GetReflectionAttributeType() == typeof(ParameterAttribute));
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x04001C88 RID: 7304
		private static readonly ReadOnlyCollection<AttributeAst> EmptyAttributeList = new ReadOnlyCollection<AttributeAst>(new AttributeAst[0]);

		// Token: 0x04001C89 RID: 7305
		private static readonly ReadOnlyCollection<ParameterAst> EmptyParameterList = new ReadOnlyCollection<ParameterAst>(new ParameterAst[0]);
	}
}
