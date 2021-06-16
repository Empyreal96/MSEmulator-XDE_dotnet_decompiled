using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000597 RID: 1431
	internal class AstSearcher : AstVisitor2
	{
		// Token: 0x06003B8A RID: 15242 RVA: 0x001375BC File Offset: 0x001357BC
		internal static IEnumerable<Ast> FindAll(Ast ast, Func<Ast, bool> predicate, bool searchNestedScriptBlocks)
		{
			AstSearcher astSearcher = new AstSearcher(predicate, false, searchNestedScriptBlocks);
			ast.InternalVisit(astSearcher);
			return astSearcher.Results;
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x001375E0 File Offset: 0x001357E0
		internal static Ast FindFirst(Ast ast, Func<Ast, bool> predicate, bool searchNestedScriptBlocks)
		{
			AstSearcher astSearcher = new AstSearcher(predicate, true, searchNestedScriptBlocks);
			ast.InternalVisit(astSearcher);
			return astSearcher.Results.FirstOrDefault<Ast>();
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0013760C File Offset: 0x0013580C
		internal static bool Contains(Ast ast, Func<Ast, bool> predicate, bool searchNestedScriptBlocks)
		{
			AstSearcher astSearcher = new AstSearcher(predicate, true, searchNestedScriptBlocks);
			ast.InternalVisit(astSearcher);
			return astSearcher.Results.FirstOrDefault<Ast>() != null;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0013767A File Offset: 0x0013587A
		internal static bool IsUsingDollarInput(Ast ast)
		{
			return AstSearcher.Contains(ast, delegate(Ast ast_)
			{
				VariableExpressionAst variableExpressionAst = ast_ as VariableExpressionAst;
				return variableExpressionAst != null && variableExpressionAst.VariablePath.IsVariable && variableExpressionAst.VariablePath.UnqualifiedPath.Equals("input", StringComparison.OrdinalIgnoreCase);
			}, false);
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001376A0 File Offset: 0x001358A0
		protected AstSearcher(Func<Ast, bool> callback, bool stopOnFirst, bool searchNestedScriptBlocks)
		{
			this._callback = callback;
			this._stopOnFirst = stopOnFirst;
			this._searchNestedScriptBlocks = searchNestedScriptBlocks;
			this.Results = new List<Ast>();
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001376C8 File Offset: 0x001358C8
		protected AstVisitAction Check(Ast ast)
		{
			if (this._callback(ast))
			{
				this.Results.Add(ast);
				if (this._stopOnFirst)
				{
					return AstVisitAction.StopVisit;
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x001376F0 File Offset: 0x001358F0
		protected AstVisitAction CheckScriptBlock(Ast ast)
		{
			AstVisitAction astVisitAction = this.Check(ast);
			if (astVisitAction == AstVisitAction.Continue && !this._searchNestedScriptBlocks)
			{
				astVisitAction = AstVisitAction.SkipChildren;
			}
			return astVisitAction;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x00137713 File Offset: 0x00135913
		public override AstVisitAction VisitErrorStatement(ErrorStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0013771C File Offset: 0x0013591C
		public override AstVisitAction VisitErrorExpression(ErrorExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00137725 File Offset: 0x00135925
		public override AstVisitAction VisitScriptBlock(ScriptBlockAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0013772E File Offset: 0x0013592E
		public override AstVisitAction VisitParamBlock(ParamBlockAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x00137737 File Offset: 0x00135937
		public override AstVisitAction VisitNamedBlock(NamedBlockAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x00137740 File Offset: 0x00135940
		public override AstVisitAction VisitTypeConstraint(TypeConstraintAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x00137749 File Offset: 0x00135949
		public override AstVisitAction VisitAttribute(AttributeAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x00137752 File Offset: 0x00135952
		public override AstVisitAction VisitParameter(ParameterAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x0013775B File Offset: 0x0013595B
		public override AstVisitAction VisitTypeExpression(TypeExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x00137764 File Offset: 0x00135964
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst ast)
		{
			return this.CheckScriptBlock(ast);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0013776D File Offset: 0x0013596D
		public override AstVisitAction VisitStatementBlock(StatementBlockAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x00137776 File Offset: 0x00135976
		public override AstVisitAction VisitIfStatement(IfStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0013777F File Offset: 0x0013597F
		public override AstVisitAction VisitTrap(TrapStatementAst ast)
		{
			return this.CheckScriptBlock(ast);
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x00137788 File Offset: 0x00135988
		public override AstVisitAction VisitSwitchStatement(SwitchStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x00137791 File Offset: 0x00135991
		public override AstVisitAction VisitDataStatement(DataStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0013779A File Offset: 0x0013599A
		public override AstVisitAction VisitForEachStatement(ForEachStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001377A3 File Offset: 0x001359A3
		public override AstVisitAction VisitDoWhileStatement(DoWhileStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001377AC File Offset: 0x001359AC
		public override AstVisitAction VisitForStatement(ForStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001377B5 File Offset: 0x001359B5
		public override AstVisitAction VisitWhileStatement(WhileStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x001377BE File Offset: 0x001359BE
		public override AstVisitAction VisitCatchClause(CatchClauseAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x001377C7 File Offset: 0x001359C7
		public override AstVisitAction VisitTryStatement(TryStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001377D0 File Offset: 0x001359D0
		public override AstVisitAction VisitBreakStatement(BreakStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001377D9 File Offset: 0x001359D9
		public override AstVisitAction VisitContinueStatement(ContinueStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x001377E2 File Offset: 0x001359E2
		public override AstVisitAction VisitReturnStatement(ReturnStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x001377EB File Offset: 0x001359EB
		public override AstVisitAction VisitExitStatement(ExitStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x001377F4 File Offset: 0x001359F4
		public override AstVisitAction VisitThrowStatement(ThrowStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x001377FD File Offset: 0x001359FD
		public override AstVisitAction VisitDoUntilStatement(DoUntilStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00137806 File Offset: 0x00135A06
		public override AstVisitAction VisitAssignmentStatement(AssignmentStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0013780F File Offset: 0x00135A0F
		public override AstVisitAction VisitPipeline(PipelineAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x00137818 File Offset: 0x00135A18
		public override AstVisitAction VisitCommand(CommandAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00137821 File Offset: 0x00135A21
		public override AstVisitAction VisitCommandExpression(CommandExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0013782A File Offset: 0x00135A2A
		public override AstVisitAction VisitCommandParameter(CommandParameterAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x00137833 File Offset: 0x00135A33
		public override AstVisitAction VisitMergingRedirection(MergingRedirectionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0013783C File Offset: 0x00135A3C
		public override AstVisitAction VisitFileRedirection(FileRedirectionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x00137845 File Offset: 0x00135A45
		public override AstVisitAction VisitBinaryExpression(BinaryExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0013784E File Offset: 0x00135A4E
		public override AstVisitAction VisitUnaryExpression(UnaryExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x00137857 File Offset: 0x00135A57
		public override AstVisitAction VisitConvertExpression(ConvertExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x00137860 File Offset: 0x00135A60
		public override AstVisitAction VisitConstantExpression(ConstantExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x00137869 File Offset: 0x00135A69
		public override AstVisitAction VisitStringConstantExpression(StringConstantExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x00137872 File Offset: 0x00135A72
		public override AstVisitAction VisitSubExpression(SubExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0013787B File Offset: 0x00135A7B
		public override AstVisitAction VisitUsingExpression(UsingExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00137884 File Offset: 0x00135A84
		public override AstVisitAction VisitVariableExpression(VariableExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0013788D File Offset: 0x00135A8D
		public override AstVisitAction VisitMemberExpression(MemberExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x00137896 File Offset: 0x00135A96
		public override AstVisitAction VisitInvokeMemberExpression(InvokeMemberExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0013789F File Offset: 0x00135A9F
		public override AstVisitAction VisitArrayExpression(ArrayExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x001378A8 File Offset: 0x00135AA8
		public override AstVisitAction VisitArrayLiteral(ArrayLiteralAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x001378B1 File Offset: 0x00135AB1
		public override AstVisitAction VisitHashtable(HashtableAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x001378BA File Offset: 0x00135ABA
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst ast)
		{
			return this.CheckScriptBlock(ast);
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001378C3 File Offset: 0x00135AC3
		public override AstVisitAction VisitParenExpression(ParenExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001378CC File Offset: 0x00135ACC
		public override AstVisitAction VisitExpandableStringExpression(ExpandableStringExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x001378D5 File Offset: 0x00135AD5
		public override AstVisitAction VisitIndexExpression(IndexExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001378DE File Offset: 0x00135ADE
		public override AstVisitAction VisitAttributedExpression(AttributedExpressionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x001378E7 File Offset: 0x00135AE7
		public override AstVisitAction VisitNamedAttributeArgument(NamedAttributeArgumentAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001378F0 File Offset: 0x00135AF0
		public override AstVisitAction VisitTypeDefinition(TypeDefinitionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x001378F9 File Offset: 0x00135AF9
		public override AstVisitAction VisitPropertyMember(PropertyMemberAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x00137902 File Offset: 0x00135B02
		public override AstVisitAction VisitFunctionMember(FunctionMemberAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x0013790B File Offset: 0x00135B0B
		public override AstVisitAction VisitUsingStatement(UsingStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00137914 File Offset: 0x00135B14
		public override AstVisitAction VisitBlockStatement(BlockStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x0013791D File Offset: 0x00135B1D
		public override AstVisitAction VisitConfigurationDefinition(ConfigurationDefinitionAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00137926 File Offset: 0x00135B26
		public override AstVisitAction VisitDynamicKeywordStatement(DynamicKeywordStatementAst ast)
		{
			return this.Check(ast);
		}

		// Token: 0x04001D98 RID: 7576
		private readonly Func<Ast, bool> _callback;

		// Token: 0x04001D99 RID: 7577
		private readonly bool _stopOnFirst;

		// Token: 0x04001D9A RID: 7578
		private readonly bool _searchNestedScriptBlocks;

		// Token: 0x04001D9B RID: 7579
		protected readonly List<Ast> Results;
	}
}
