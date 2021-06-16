using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Language
{
	// Token: 0x02000553 RID: 1363
	internal class CompilerGeneratedMemberFunctionAst : MemberAst, IParameterMetadataProvider
	{
		// Token: 0x060038AC RID: 14508 RVA: 0x0012DC78 File Offset: 0x0012BE78
		internal CompilerGeneratedMemberFunctionAst(IScriptExtent extent, TypeDefinitionAst definingType, SpecialMemberFunctionType type) : base(extent)
		{
			StatementAst statementAst = null;
			if (type == SpecialMemberFunctionType.DefaultConstructor)
			{
				BaseCtorInvokeMemberExpressionAst expression = new BaseCtorInvokeMemberExpressionAst(extent, extent, new ExpressionAst[0]);
				statementAst = new CommandExpressionAst(extent, expression, null);
			}
			this.Body = new ScriptBlockAst(extent, null, new StatementBlockAst(extent, (statementAst == null) ? null : new StatementAst[]
			{
				statementAst
			}, null), false);
			base.SetParent(this.Body);
			definingType.SetParent(this);
			this.DefiningType = definingType;
			this.Type = type;
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x0012DCF1 File Offset: 0x0012BEF1
		public override string Name
		{
			get
			{
				return this.DefiningType.Name;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x0012DCFE File Offset: 0x0012BEFE
		// (set) Token: 0x060038AF RID: 14511 RVA: 0x0012DD06 File Offset: 0x0012BF06
		internal TypeDefinitionAst DefiningType { get; private set; }

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x0012DD0F File Offset: 0x0012BF0F
		// (set) Token: 0x060038B1 RID: 14513 RVA: 0x0012DD17 File Offset: 0x0012BF17
		internal SpecialMemberFunctionType Type { get; private set; }

		// Token: 0x060038B2 RID: 14514 RVA: 0x0012DD20 File Offset: 0x0012BF20
		internal override string GetTooltip()
		{
			return this.DefiningType.Name + " new()";
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x0012DD37 File Offset: 0x0012BF37
		public override Ast Copy()
		{
			return new CompilerGeneratedMemberFunctionAst(base.Extent, (TypeDefinitionAst)this.DefiningType.Copy(), this.Type);
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x0012DD5A File Offset: 0x0012BF5A
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return null;
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0012DD5D File Offset: 0x0012BF5D
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0012DD60 File Offset: 0x0012BF60
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0012DD68 File Offset: 0x0012BF68
		public RuntimeDefinedParameterDictionary GetParameterMetadata(bool automaticPositions, ref bool usesCmdletBinding)
		{
			return new RuntimeDefinedParameterDictionary
			{
				Data = RuntimeDefinedParameterDictionary.EmptyParameterArray
			};
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0012DD87 File Offset: 0x0012BF87
		public IEnumerable<Attribute> GetScriptBlockAttributes()
		{
			return ((IParameterMetadataProvider)this.Body).GetScriptBlockAttributes();
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0012DD94 File Offset: 0x0012BF94
		public bool UsesCmdletBinding()
		{
			return false;
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x0012DD97 File Offset: 0x0012BF97
		public ReadOnlyCollection<ParameterAst> Parameters
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x0012DD9A File Offset: 0x0012BF9A
		// (set) Token: 0x060038BC RID: 14524 RVA: 0x0012DDA2 File Offset: 0x0012BFA2
		public ScriptBlockAst Body { get; private set; }

		// Token: 0x060038BD RID: 14525 RVA: 0x0012DDAB File Offset: 0x0012BFAB
		public PowerShell GetPowerShell(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args)
		{
			return null;
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0012DDAE File Offset: 0x0012BFAE
		public string GetWithInputHandlingForInvokeCommand()
		{
			return null;
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0012DDB1 File Offset: 0x0012BFB1
		public Tuple<string, string> GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			return null;
		}
	}
}
