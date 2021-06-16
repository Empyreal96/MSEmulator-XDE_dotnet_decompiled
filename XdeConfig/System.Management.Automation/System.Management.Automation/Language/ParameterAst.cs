using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000547 RID: 1351
	public class ParameterAst : Ast
	{
		// Token: 0x06003837 RID: 14391 RVA: 0x0012C99C File Offset: 0x0012AB9C
		public ParameterAst(IScriptExtent extent, VariableExpressionAst name, IEnumerable<AttributeBaseAst> attributes, ExpressionAst defaultValue) : base(extent)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeBaseAst>(attributes.ToArray<AttributeBaseAst>());
				base.SetParents<AttributeBaseAst>(this.Attributes);
			}
			else
			{
				this.Attributes = ParameterAst.EmptyAttributeList;
			}
			this.Name = name;
			base.SetParent(name);
			if (defaultValue != null)
			{
				this.DefaultValue = defaultValue;
				base.SetParent(defaultValue);
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x0012CA0D File Offset: 0x0012AC0D
		// (set) Token: 0x06003839 RID: 14393 RVA: 0x0012CA15 File Offset: 0x0012AC15
		public ReadOnlyCollection<AttributeBaseAst> Attributes { get; private set; }

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600383A RID: 14394 RVA: 0x0012CA1E File Offset: 0x0012AC1E
		// (set) Token: 0x0600383B RID: 14395 RVA: 0x0012CA26 File Offset: 0x0012AC26
		public VariableExpressionAst Name { get; private set; }

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x0012CA2F File Offset: 0x0012AC2F
		// (set) Token: 0x0600383D RID: 14397 RVA: 0x0012CA37 File Offset: 0x0012AC37
		public ExpressionAst DefaultValue { get; private set; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x0012CA40 File Offset: 0x0012AC40
		public Type StaticType
		{
			get
			{
				Type type = null;
				TypeConstraintAst typeConstraintAst = this.Attributes.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
				if (typeConstraintAst != null)
				{
					type = typeConstraintAst.TypeName.GetReflectionType();
				}
				return type ?? typeof(object);
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x0012CA80 File Offset: 0x0012AC80
		public override Ast Copy()
		{
			VariableExpressionAst name = Ast.CopyElement<VariableExpressionAst>(this.Name);
			AttributeBaseAst[] attributes = Ast.CopyElements<AttributeBaseAst>(this.Attributes);
			ExpressionAst defaultValue = Ast.CopyElement<ExpressionAst>(this.DefaultValue);
			return new ParameterAst(base.Extent, name, attributes, defaultValue);
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x0012CD04 File Offset: 0x0012AF04
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			TypeConstraintAst typeConstraint = this.Attributes.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
			if (typeConstraint != null)
			{
				yield return new PSTypeName(typeConstraint.TypeName);
			}
			foreach (AttributeAst attributeAst in this.Attributes.OfType<AttributeAst>())
			{
				PSTypeNameAttribute attribute = null;
				try
				{
					attribute = (attributeAst.GetAttribute() as PSTypeNameAttribute);
				}
				catch (RuntimeException)
				{
				}
				if (attribute != null)
				{
					yield return new PSTypeName(attribute.PSTypeName);
				}
			}
			yield break;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0012CD24 File Offset: 0x0012AF24
		internal string GetTooltip()
		{
			TypeConstraintAst typeConstraintAst = this.Attributes.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
			string str = (typeConstraintAst != null) ? typeConstraintAst.TypeName.FullName : "object";
			return str + " " + this.Name.VariablePath.UserPath;
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x0012CD74 File Offset: 0x0012AF74
		internal string GetParamTextWithDollarUsingHandling(IEnumerator<VariableExpressionAst> orderedUsingVar)
		{
			int startOffset = base.Extent.StartOffset;
			int num = 0;
			int num2 = base.Extent.EndOffset - base.Extent.StartOffset;
			string text = this.ToString();
			if (orderedUsingVar.Current == null && !orderedUsingVar.MoveNext())
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				VariableExpressionAst variableExpressionAst = orderedUsingVar.Current;
				int num3 = variableExpressionAst.Extent.StartOffset - startOffset;
				int num4 = variableExpressionAst.Extent.EndOffset - startOffset;
				if (num3 >= num)
				{
					if (num3 >= num2)
					{
						break;
					}
					string userPath = variableExpressionAst.VariablePath.UserPath;
					string str = variableExpressionAst.Splatted ? "@" : "$";
					string value = str + "__using_" + userPath;
					stringBuilder.Append(text.Substring(num, num3 - num));
					stringBuilder.Append(value);
					num = num4;
				}
			}
			while (orderedUsingVar.MoveNext());
			if (num == 0)
			{
				return text;
			}
			stringBuilder.Append(text.Substring(num, num2 - num));
			return stringBuilder.ToString();
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x0012CE77 File Offset: 0x0012B077
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitParameter(this);
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x0012CE80 File Offset: 0x0012B080
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitParameter(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.Attributes.Count; i++)
				{
					AttributeBaseAst attributeBaseAst = this.Attributes[i];
					astVisitAction = attributeBaseAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Name.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.DefaultValue != null)
			{
				astVisitAction = this.DefaultValue.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001C9B RID: 7323
		private static readonly ReadOnlyCollection<AttributeBaseAst> EmptyAttributeList = new ReadOnlyCollection<AttributeBaseAst>(new AttributeBaseAst[0]);
	}
}
