using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x020005F6 RID: 1526
	internal class ScriptBlockToPowerShellChecker : AstVisitor
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060041BD RID: 16829 RVA: 0x0015BE8E File Offset: 0x0015A08E
		// (set) Token: 0x060041BE RID: 16830 RVA: 0x0015BE96 File Offset: 0x0015A096
		internal ScriptBlockAst ScriptBeingConverted { get; set; }

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x0015BE9F File Offset: 0x0015A09F
		// (set) Token: 0x060041C0 RID: 16832 RVA: 0x0015BEA7 File Offset: 0x0015A0A7
		internal bool UsesParameter { get; private set; }

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x060041C1 RID: 16833 RVA: 0x0015BEB0 File Offset: 0x0015A0B0
		// (set) Token: 0x060041C2 RID: 16834 RVA: 0x0015BEB8 File Offset: 0x0015A0B8
		internal bool HasUsingExpr { get; private set; }

		// Token: 0x060041C3 RID: 16835 RVA: 0x0015BEC1 File Offset: 0x0015A0C1
		public override AstVisitAction VisitParameter(ParameterAst parameterAst)
		{
			if (parameterAst.Name.VariablePath.IsAnyLocal())
			{
				this._validVariables.Add(parameterAst.Name.VariablePath.UnqualifiedPath);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x0015BEF4 File Offset: 0x0015A0F4
		public override AstVisitAction VisitPipeline(PipelineAst pipelineAst)
		{
			if (pipelineAst.PipelineElements[0] is CommandExpressionAst && (pipelineAst.GetPureExpression() == null || pipelineAst.Parent.Parent == this.ScriptBeingConverted))
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertPipelineStartsWithExpression", null, AutomationExceptions.CantConvertPipelineStartsWithExpression, new object[0]), pipelineAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x0015BF4C File Offset: 0x0015A14C
		public override AstVisitAction VisitCommand(CommandAst commandAst)
		{
			if (commandAst.InvocationOperator == TokenKind.Dot)
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithDotSourcing", null, AutomationExceptions.CantConvertWithDotSourcing, new object[0]), commandAst);
			}
			if (commandAst.Parent.Parent.Parent != this.ScriptBeingConverted)
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithCommandInvocations", null, AutomationExceptions.CantConvertWithCommandInvocations, new object[0]), commandAst);
			}
			if (commandAst.CommandElements[0] is ScriptBlockExpressionAst)
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithScriptBlockInvocation", null, AutomationExceptions.CantConvertWithScriptBlockInvocation, new object[0]), commandAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x0015BFE3 File Offset: 0x0015A1E3
		public override AstVisitAction VisitMergingRedirection(MergingRedirectionAst redirectionAst)
		{
			if (redirectionAst.ToStream != RedirectionStream.Output)
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CanConvertOneOutputErrorRedir", null, AutomationExceptions.CanConvertOneOutputErrorRedir, new object[0]), redirectionAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x0015C00B File Offset: 0x0015A20B
		public override AstVisitAction VisitFileRedirection(FileRedirectionAst redirectionAst)
		{
			ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CanConvertOneOutputErrorRedir", null, AutomationExceptions.CanConvertOneOutputErrorRedir, new object[0]), redirectionAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x0015C02C File Offset: 0x0015A22C
		public override AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			bool usesParameter = this.UsesParameter;
			bool flag = variableExpressionAst.IsSafeVariableReference(this._validVariables, ref usesParameter);
			if (usesParameter != this.UsesParameter)
			{
				this.UsesParameter = usesParameter;
			}
			if (!flag)
			{
				ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithUndeclaredVariables", null, AutomationExceptions.CantConvertWithUndeclaredVariables, new object[]
				{
					variableExpressionAst.VariablePath
				}), variableExpressionAst);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x0015C08A File Offset: 0x0015A28A
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithScriptBlocks", null, AutomationExceptions.CantConvertWithScriptBlocks, new object[0]), scriptBlockExpressionAst);
			return AstVisitAction.SkipChildren;
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x0015C0A9 File Offset: 0x0015A2A9
		public override AstVisitAction VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			this.HasUsingExpr = true;
			return AstVisitAction.SkipChildren;
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x0015C0B3 File Offset: 0x0015A2B3
		internal static void ThrowError(ScriptBlockToPowerShellNotSupportedException ex, Ast ast)
		{
			InterpreterError.UpdateExceptionErrorRecordPosition(ex, ast.Extent);
			throw ex;
		}

		// Token: 0x040020F8 RID: 8440
		private readonly HashSet<string> _validVariables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	}
}
