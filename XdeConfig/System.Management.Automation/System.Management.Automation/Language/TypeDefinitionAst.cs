using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200054A RID: 1354
	public class TypeDefinitionAst : StatementAst
	{
		// Token: 0x06003851 RID: 14417 RVA: 0x0012D0E0 File Offset: 0x0012B2E0
		public TypeDefinitionAst(IScriptExtent extent, string name, IEnumerable<AttributeAst> attributes, IEnumerable<MemberAst> members, TypeAttributes typeAttributes, IEnumerable<TypeConstraintAst> baseTypes) : base(extent)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (attributes != null && attributes.Any<AttributeAst>())
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = TypeDefinitionAst.EmptyAttributeList;
			}
			if (members != null && members.Any<MemberAst>())
			{
				this.Members = new ReadOnlyCollection<MemberAst>(members.ToArray<MemberAst>());
				base.SetParents<MemberAst>(this.Members);
			}
			else
			{
				this.Members = TypeDefinitionAst.EmptyMembersCollection;
			}
			if (baseTypes != null && baseTypes.Any<TypeConstraintAst>())
			{
				this.BaseTypes = new ReadOnlyCollection<TypeConstraintAst>(baseTypes.ToArray<TypeConstraintAst>());
				base.SetParents<TypeConstraintAst>(this.BaseTypes);
			}
			else
			{
				this.BaseTypes = TypeDefinitionAst.EmptyBaseTypesCollection;
			}
			this.Name = name;
			this.TypeAttributes = typeAttributes;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x0012D1BB File Offset: 0x0012B3BB
		// (set) Token: 0x06003853 RID: 14419 RVA: 0x0012D1C3 File Offset: 0x0012B3C3
		public string Name { get; private set; }

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x0012D1CC File Offset: 0x0012B3CC
		// (set) Token: 0x06003855 RID: 14421 RVA: 0x0012D1D4 File Offset: 0x0012B3D4
		public ReadOnlyCollection<AttributeAst> Attributes { get; private set; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x0012D1DD File Offset: 0x0012B3DD
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x0012D1E5 File Offset: 0x0012B3E5
		public ReadOnlyCollection<TypeConstraintAst> BaseTypes { get; private set; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06003858 RID: 14424 RVA: 0x0012D1EE File Offset: 0x0012B3EE
		// (set) Token: 0x06003859 RID: 14425 RVA: 0x0012D1F6 File Offset: 0x0012B3F6
		public ReadOnlyCollection<MemberAst> Members { get; private set; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x0012D1FF File Offset: 0x0012B3FF
		// (set) Token: 0x0600385B RID: 14427 RVA: 0x0012D207 File Offset: 0x0012B407
		public TypeAttributes TypeAttributes { get; private set; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600385C RID: 14428 RVA: 0x0012D210 File Offset: 0x0012B410
		public bool IsEnum
		{
			get
			{
				return (this.TypeAttributes & TypeAttributes.Enum) == TypeAttributes.Enum;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600385D RID: 14429 RVA: 0x0012D21D File Offset: 0x0012B41D
		public bool IsClass
		{
			get
			{
				return (this.TypeAttributes & TypeAttributes.Class) == TypeAttributes.Class;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x0012D22A File Offset: 0x0012B42A
		public bool IsInterface
		{
			get
			{
				return (this.TypeAttributes & TypeAttributes.Interface) == TypeAttributes.Interface;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x0600385F RID: 14431 RVA: 0x0012D237 File Offset: 0x0012B437
		// (set) Token: 0x06003860 RID: 14432 RVA: 0x0012D23F File Offset: 0x0012B43F
		internal Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0012D248 File Offset: 0x0012B448
		public override Ast Copy()
		{
			return new TypeDefinitionAst(base.Extent, this.Name, Ast.CopyElements<AttributeAst>(this.Attributes), Ast.CopyElements<MemberAst>(this.Members), this.TypeAttributes, Ast.CopyElements<TypeConstraintAst>(this.BaseTypes));
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x0012D284 File Offset: 0x0012B484
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitTypeDefinition(this);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x0012D2A4 File Offset: 0x0012B4A4
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitTypeDefinition(this);
				if (astVisitAction == AstVisitAction.SkipChildren)
				{
					return visitor.CheckForPostAction(this, AstVisitAction.Continue);
				}
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
				for (int j = 0; j < this.BaseTypes.Count; j++)
				{
					TypeConstraintAst typeConstraintAst = this.BaseTypes[j];
					astVisitAction = typeConstraintAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int k = 0; k < this.Members.Count; k++)
				{
					MemberAst memberAst = this.Members[k];
					astVisitAction = memberAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0012D37B File Offset: 0x0012B57B
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x04001CA7 RID: 7335
		private static readonly ReadOnlyCollection<AttributeAst> EmptyAttributeList = new ReadOnlyCollection<AttributeAst>(new AttributeAst[0]);

		// Token: 0x04001CA8 RID: 7336
		private static readonly ReadOnlyCollection<MemberAst> EmptyMembersCollection = new ReadOnlyCollection<MemberAst>(new MemberAst[0]);

		// Token: 0x04001CA9 RID: 7337
		private static readonly ReadOnlyCollection<TypeConstraintAst> EmptyBaseTypesCollection = new ReadOnlyCollection<TypeConstraintAst>(new TypeConstraintAst[0]);

		// Token: 0x04001CAA RID: 7338
		private Type _type;
	}
}
