using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000551 RID: 1361
	public class FunctionMemberAst : MemberAst, IParameterMetadataProvider
	{
		// Token: 0x0600388D RID: 14477 RVA: 0x0012D890 File Offset: 0x0012BA90
		public FunctionMemberAst(IScriptExtent extent, FunctionDefinitionAst functionDefinitionAst, TypeConstraintAst returnType, IEnumerable<AttributeAst> attributes, MethodAttributes methodAttributes) : base(extent)
		{
			if (functionDefinitionAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("functionDefinitionAst");
			}
			if ((methodAttributes & (MethodAttributes.Public | MethodAttributes.Private)) == (MethodAttributes.Public | MethodAttributes.Private))
			{
				throw PSTraceSource.NewArgumentException("methodAttributes");
			}
			if (returnType != null)
			{
				this.ReturnType = returnType;
				base.SetParent(returnType);
			}
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = FunctionMemberAst.EmptyAttributeList;
			}
			this._functionDefinitionAst = functionDefinitionAst;
			base.SetParent(functionDefinitionAst);
			this.MethodAttributes = methodAttributes;
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600388E RID: 14478 RVA: 0x0012D91A File Offset: 0x0012BB1A
		public override string Name
		{
			get
			{
				return this._functionDefinitionAst.Name;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600388F RID: 14479 RVA: 0x0012D927 File Offset: 0x0012BB27
		// (set) Token: 0x06003890 RID: 14480 RVA: 0x0012D92F File Offset: 0x0012BB2F
		public ReadOnlyCollection<AttributeAst> Attributes { get; private set; }

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06003891 RID: 14481 RVA: 0x0012D938 File Offset: 0x0012BB38
		// (set) Token: 0x06003892 RID: 14482 RVA: 0x0012D940 File Offset: 0x0012BB40
		public TypeConstraintAst ReturnType { get; private set; }

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003893 RID: 14483 RVA: 0x0012D949 File Offset: 0x0012BB49
		public ReadOnlyCollection<ParameterAst> Parameters
		{
			get
			{
				return this._functionDefinitionAst.Parameters ?? FunctionMemberAst.EmptyParameterList;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003894 RID: 14484 RVA: 0x0012D95F File Offset: 0x0012BB5F
		public ScriptBlockAst Body
		{
			get
			{
				return this._functionDefinitionAst.Body;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003895 RID: 14485 RVA: 0x0012D96C File Offset: 0x0012BB6C
		// (set) Token: 0x06003896 RID: 14486 RVA: 0x0012D974 File Offset: 0x0012BB74
		public MethodAttributes MethodAttributes { get; private set; }

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003897 RID: 14487 RVA: 0x0012D97D File Offset: 0x0012BB7D
		public bool IsPublic
		{
			get
			{
				return (this.MethodAttributes & MethodAttributes.Public) != MethodAttributes.None;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06003898 RID: 14488 RVA: 0x0012D98D File Offset: 0x0012BB8D
		public bool IsPrivate
		{
			get
			{
				return (this.MethodAttributes & MethodAttributes.Private) != MethodAttributes.None;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003899 RID: 14489 RVA: 0x0012D99D File Offset: 0x0012BB9D
		public bool IsHidden
		{
			get
			{
				return (this.MethodAttributes & MethodAttributes.Hidden) != MethodAttributes.None;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600389A RID: 14490 RVA: 0x0012D9AE File Offset: 0x0012BBAE
		public bool IsStatic
		{
			get
			{
				return (this.MethodAttributes & MethodAttributes.Static) != MethodAttributes.None;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600389B RID: 14491 RVA: 0x0012D9BF File Offset: 0x0012BBBF
		public bool IsConstructor
		{
			get
			{
				return this.Name.Equals(((TypeDefinitionAst)base.Parent).Name, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600389C RID: 14492 RVA: 0x0012D9DD File Offset: 0x0012BBDD
		internal IScriptExtent NameExtent
		{
			get
			{
				return this._functionDefinitionAst.NameExtent;
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0012D9EC File Offset: 0x0012BBEC
		public override Ast Copy()
		{
			FunctionDefinitionAst functionDefinitionAst = Ast.CopyElement<FunctionDefinitionAst>(this._functionDefinitionAst);
			TypeConstraintAst returnType = Ast.CopyElement<TypeConstraintAst>(this.ReturnType);
			AttributeAst[] attributes = Ast.CopyElements<AttributeAst>(this.Attributes);
			return new FunctionMemberAst(base.Extent, functionDefinitionAst, returnType, attributes, this.MethodAttributes);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0012DA34 File Offset: 0x0012BC34
		internal override string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsStatic)
			{
				stringBuilder.Append("static ");
			}
			stringBuilder.Append(this.IsReturnTypeVoid() ? "void" : this.ReturnType.TypeName.FullName);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.Name);
			stringBuilder.Append('(');
			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(this.Parameters[i].GetTooltip());
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0012DAF0 File Offset: 0x0012BCF0
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitFunctionMember(this);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0012DB10 File Offset: 0x0012BD10
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitFunctionMember(this);
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
				if (astVisitAction == AstVisitAction.Continue && this.ReturnType != null)
				{
					astVisitAction = this.ReturnType.InternalVisit(visitor);
				}
				if (astVisitAction == AstVisitAction.Continue)
				{
					astVisitAction = this._functionDefinitionAst.InternalVisit(visitor);
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0012DB9F File Offset: 0x0012BD9F
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060038A2 RID: 14498 RVA: 0x0012DBA6 File Offset: 0x0012BDA6
		ReadOnlyCollection<ParameterAst> IParameterMetadataProvider.Parameters
		{
			get
			{
				return ((IParameterMetadataProvider)this._functionDefinitionAst).Parameters;
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0012DBB3 File Offset: 0x0012BDB3
		RuntimeDefinedParameterDictionary IParameterMetadataProvider.GetParameterMetadata(bool automaticPositions, ref bool usesCmdletBinding)
		{
			return ((IParameterMetadataProvider)this._functionDefinitionAst).GetParameterMetadata(automaticPositions, ref usesCmdletBinding);
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x0012DBC2 File Offset: 0x0012BDC2
		IEnumerable<Attribute> IParameterMetadataProvider.GetScriptBlockAttributes()
		{
			return ((IParameterMetadataProvider)this._functionDefinitionAst).GetScriptBlockAttributes();
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0012DBCF File Offset: 0x0012BDCF
		bool IParameterMetadataProvider.UsesCmdletBinding()
		{
			return ((IParameterMetadataProvider)this._functionDefinitionAst).UsesCmdletBinding();
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x0012DBDC File Offset: 0x0012BDDC
		PowerShell IParameterMetadataProvider.GetPowerShell(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x0012DBE3 File Offset: 0x0012BDE3
		string IParameterMetadataProvider.GetWithInputHandlingForInvokeCommand()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x0012DBEA File Offset: 0x0012BDEA
		Tuple<string, string> IParameterMetadataProvider.GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x0012DBF4 File Offset: 0x0012BDF4
		internal bool IsReturnTypeVoid()
		{
			if (this.ReturnType == null)
			{
				return true;
			}
			TypeName typeName = this.ReturnType.TypeName as TypeName;
			return typeName != null && typeName.IsType(typeof(void));
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x0012DC31 File Offset: 0x0012BE31
		internal Type GetReturnType()
		{
			if (this.ReturnType != null)
			{
				return this.ReturnType.TypeName.GetReflectionType();
			}
			return typeof(void);
		}

		// Token: 0x04001CCD RID: 7373
		private static readonly ReadOnlyCollection<AttributeAst> EmptyAttributeList = new ReadOnlyCollection<AttributeAst>(new AttributeAst[0]);

		// Token: 0x04001CCE RID: 7374
		private static readonly ReadOnlyCollection<ParameterAst> EmptyParameterList = new ReadOnlyCollection<ParameterAst>(new ParameterAst[0]);

		// Token: 0x04001CCF RID: 7375
		private readonly FunctionDefinitionAst _functionDefinitionAst;
	}
}
