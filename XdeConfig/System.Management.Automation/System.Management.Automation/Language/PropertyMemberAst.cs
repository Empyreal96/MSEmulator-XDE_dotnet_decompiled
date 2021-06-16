using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200054F RID: 1359
	public class PropertyMemberAst : MemberAst
	{
		// Token: 0x06003879 RID: 14457 RVA: 0x0012D5DC File Offset: 0x0012B7DC
		public PropertyMemberAst(IScriptExtent extent, string name, TypeConstraintAst propertyType, IEnumerable<AttributeAst> attributes, PropertyAttributes propertyAttributes, ExpressionAst initialValue) : base(extent)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if ((propertyAttributes & (PropertyAttributes.Public | PropertyAttributes.Private)) == (PropertyAttributes.Public | PropertyAttributes.Private))
			{
				throw PSTraceSource.NewArgumentException("propertyAttributes");
			}
			this._name = name;
			if (propertyType != null)
			{
				this.PropertyType = propertyType;
				base.SetParent(this.PropertyType);
			}
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = PropertyMemberAst.EmptyAttributeList;
			}
			this.PropertyAttributes = propertyAttributes;
			this.InitialValue = initialValue;
			if (this.InitialValue != null)
			{
				base.SetParent(this.InitialValue);
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x0012D685 File Offset: 0x0012B885
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x0012D68D File Offset: 0x0012B88D
		// (set) Token: 0x0600387C RID: 14460 RVA: 0x0012D695 File Offset: 0x0012B895
		public TypeConstraintAst PropertyType { get; private set; }

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x0012D69E File Offset: 0x0012B89E
		// (set) Token: 0x0600387E RID: 14462 RVA: 0x0012D6A6 File Offset: 0x0012B8A6
		public ReadOnlyCollection<AttributeAst> Attributes { get; private set; }

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x0600387F RID: 14463 RVA: 0x0012D6AF File Offset: 0x0012B8AF
		// (set) Token: 0x06003880 RID: 14464 RVA: 0x0012D6B7 File Offset: 0x0012B8B7
		public PropertyAttributes PropertyAttributes { get; private set; }

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x0012D6C0 File Offset: 0x0012B8C0
		// (set) Token: 0x06003882 RID: 14466 RVA: 0x0012D6C8 File Offset: 0x0012B8C8
		public ExpressionAst InitialValue { get; private set; }

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06003883 RID: 14467 RVA: 0x0012D6D1 File Offset: 0x0012B8D1
		public bool IsPublic
		{
			get
			{
				return (this.PropertyAttributes & PropertyAttributes.Public) != PropertyAttributes.None;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06003884 RID: 14468 RVA: 0x0012D6E1 File Offset: 0x0012B8E1
		public bool IsPrivate
		{
			get
			{
				return (this.PropertyAttributes & PropertyAttributes.Private) != PropertyAttributes.None;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06003885 RID: 14469 RVA: 0x0012D6F1 File Offset: 0x0012B8F1
		public bool IsHidden
		{
			get
			{
				return (this.PropertyAttributes & PropertyAttributes.Hidden) != PropertyAttributes.None;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06003886 RID: 14470 RVA: 0x0012D702 File Offset: 0x0012B902
		public bool IsStatic
		{
			get
			{
				return (this.PropertyAttributes & PropertyAttributes.Static) != PropertyAttributes.None;
			}
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0012D714 File Offset: 0x0012B914
		public override Ast Copy()
		{
			TypeConstraintAst propertyType = Ast.CopyElement<TypeConstraintAst>(this.PropertyType);
			AttributeAst[] attributes = Ast.CopyElements<AttributeAst>(this.Attributes);
			ExpressionAst initialValue = Ast.CopyElement<ExpressionAst>(this.InitialValue);
			return new PropertyMemberAst(base.Extent, this.Name, propertyType, attributes, this.PropertyAttributes, initialValue);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0012D760 File Offset: 0x0012B960
		internal override string GetTooltip()
		{
			string text = (this.PropertyType != null) ? this.PropertyType.TypeName.FullName : "object";
			if (!this.IsStatic)
			{
				return text + " " + this.Name;
			}
			return "static " + text + " " + this.Name;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x0012D7C0 File Offset: 0x0012B9C0
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitPropertyMember(this);
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0012D7E0 File Offset: 0x0012B9E0
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitPropertyMember(this);
				if (astVisitAction == AstVisitAction.SkipChildren)
				{
					return visitor.CheckForPostAction(this, AstVisitAction.Continue);
				}
				if (astVisitAction == AstVisitAction.Continue && this.PropertyType != null)
				{
					astVisitAction = this.PropertyType.InternalVisit(visitor);
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
				if (astVisitAction == AstVisitAction.Continue && this.InitialValue != null)
				{
					astVisitAction = this.InitialValue.InternalVisit(visitor);
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0012D877 File Offset: 0x0012BA77
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001CC1 RID: 7361
		private static readonly ReadOnlyCollection<AttributeAst> EmptyAttributeList = new ReadOnlyCollection<AttributeAst>(new AttributeAst[0]);

		// Token: 0x04001CC2 RID: 7362
		private readonly string _name;
	}
}
