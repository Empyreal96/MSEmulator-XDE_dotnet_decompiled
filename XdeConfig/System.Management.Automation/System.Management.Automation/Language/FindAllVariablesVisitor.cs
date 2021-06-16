using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x020005EC RID: 1516
	internal class FindAllVariablesVisitor : AstVisitor
	{
		// Token: 0x060040E2 RID: 16610 RVA: 0x001586FC File Offset: 0x001568FC
		static FindAllVariablesVisitor()
		{
			foreach (string item in FindAllVariablesVisitor.pessimizingCmdlets)
			{
				FindAllVariablesVisitor.hashOfPessimizingCmdlets.Add(item);
			}
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x001587B8 File Offset: 0x001569B8
		internal static Dictionary<string, VariableAnalysisDetails> Visit(TrapStatementAst trap)
		{
			FindAllVariablesVisitor findAllVariablesVisitor = new FindAllVariablesVisitor(true, false);
			trap.Body.InternalVisit(findAllVariablesVisitor);
			return findAllVariablesVisitor._variables;
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x001587E0 File Offset: 0x001569E0
		internal static Dictionary<string, VariableAnalysisDetails> Visit(ExpressionAst exprAst)
		{
			FindAllVariablesVisitor findAllVariablesVisitor = new FindAllVariablesVisitor(true, false);
			exprAst.InternalVisit(findAllVariablesVisitor);
			return findAllVariablesVisitor._variables;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00158818 File Offset: 0x00156A18
		internal static Dictionary<string, VariableAnalysisDetails> Visit(IParameterMetadataProvider ast, bool disableOptimizations, bool scriptCmdlet, out int localsAllocated, out bool forceNoOptimizing)
		{
			FindAllVariablesVisitor findAllVariablesVisitor = new FindAllVariablesVisitor(disableOptimizations, scriptCmdlet);
			ast.Body.InternalVisit(findAllVariablesVisitor);
			forceNoOptimizing = findAllVariablesVisitor._disableOptimizations;
			if (ast.Parameters != null)
			{
				findAllVariablesVisitor.VisitParameters(ast.Parameters);
			}
			localsAllocated = (from details in findAllVariablesVisitor._variables
			where details.Value.LocalTupleIndex != -1
			select details).Count<KeyValuePair<string, VariableAnalysisDetails>>();
			return findAllVariablesVisitor._variables;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x0015888C File Offset: 0x00156A8C
		private FindAllVariablesVisitor(bool disableOptimizations, bool scriptCmdlet)
		{
			this._disableOptimizations = disableOptimizations;
			string[] automaticVariables = SpecialVariables.AutomaticVariables;
			for (int i = 0; i < automaticVariables.Length; i++)
			{
				this.NoteVariable(automaticVariables[i], i, SpecialVariables.AutomaticVariableTypes[i], true, false);
			}
			if (scriptCmdlet)
			{
				string[] preferenceVariables = SpecialVariables.PreferenceVariables;
				for (int i = 0; i < preferenceVariables.Length; i++)
				{
					this.NoteVariable(preferenceVariables[i], i + 9, SpecialVariables.PreferenceVariableTypes[i], false, true);
				}
			}
			this.NoteVariable("?", -1, typeof(bool), true, false);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x00158924 File Offset: 0x00156B24
		private void VisitParameters(ReadOnlyCollection<ParameterAst> parameters)
		{
			foreach (ParameterAst parameterAst in parameters)
			{
				VariableExpressionAst name = parameterAst.Name;
				VariablePath variablePath = name.VariablePath;
				if (variablePath.IsAnyLocal())
				{
					string unaliasedVariableName = VariableAnalysis.GetUnaliasedVariableName(variablePath);
					VariableAnalysisDetails variableAnalysisDetails;
					if (this._variables.TryGetValue(unaliasedVariableName, out variableAnalysisDetails))
					{
						variableAnalysisDetails.Type = parameterAst.StaticType;
						object obj;
						if (!Compiler.TryGetDefaultParameterValue(variableAnalysisDetails.Type, out obj))
						{
							variableAnalysisDetails.LocalTupleIndex = -2;
						}
					}
					else
					{
						this.NoteVariable(unaliasedVariableName, -1, parameterAst.StaticType, false, false);
					}
				}
			}
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x001589D0 File Offset: 0x00156BD0
		public override AstVisitAction VisitDataStatement(DataStatementAst dataStatementAst)
		{
			if (dataStatementAst.Variable != null)
			{
				this.NoteVariable(dataStatementAst.Variable, -1, null, false, false);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x001589EB File Offset: 0x00156BEB
		public override AstVisitAction VisitSwitchStatement(SwitchStatementAst switchStatementAst)
		{
			this.NoteVariable("switch", -1, typeof(IEnumerator), false, false);
			return AstVisitAction.Continue;
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x00158A06 File Offset: 0x00156C06
		public override AstVisitAction VisitForEachStatement(ForEachStatementAst forEachStatementAst)
		{
			this.NoteVariable("foreach", -1, typeof(IEnumerator), false, false);
			return AstVisitAction.Continue;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00158A24 File Offset: 0x00156C24
		public override AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
		{
			VariablePath variablePath = variableExpressionAst.VariablePath;
			if (variablePath.IsAnyLocal())
			{
				if (variablePath.IsPrivate)
				{
					this._disableOptimizations = true;
				}
				this.NoteVariable(VariableAnalysis.GetUnaliasedVariableName(variablePath), -1, null, false, false);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00158A60 File Offset: 0x00156C60
		public override AstVisitAction VisitUsingExpression(UsingExpressionAst usingExpressionAst)
		{
			if (usingExpressionAst.RuntimeUsingIndex == -1)
			{
				usingExpressionAst.RuntimeUsingIndex = this._runtimeUsingIndex;
			}
			this._runtimeUsingIndex++;
			return AstVisitAction.Continue;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00158A88 File Offset: 0x00156C88
		public override AstVisitAction VisitCommand(CommandAst commandAst)
		{
			StringConstantExpressionAst stringConstantExpressionAst = commandAst.CommandElements[0] as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null && FindAllVariablesVisitor.hashOfPessimizingCmdlets.Contains(stringConstantExpressionAst.Value))
			{
				this._disableOptimizations = true;
			}
			if (commandAst.InvocationOperator == TokenKind.Dot)
			{
				this._disableOptimizations = true;
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00158AD5 File Offset: 0x00156CD5
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			return AstVisitAction.SkipChildren;
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00158AD8 File Offset: 0x00156CD8
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			return AstVisitAction.SkipChildren;
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00158ADB File Offset: 0x00156CDB
		public override AstVisitAction VisitTrap(TrapStatementAst trapStatementAst)
		{
			return AstVisitAction.SkipChildren;
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00158AE0 File Offset: 0x00156CE0
		private void NoteVariable(string variableName, int index, Type type, bool automatic = false, bool preferenceVariable = false)
		{
			if (!this._variables.ContainsKey(variableName))
			{
				VariableAnalysisDetails value = new VariableAnalysisDetails
				{
					BitIndex = this._variables.Count,
					LocalTupleIndex = index,
					Name = variableName,
					Type = type,
					Automatic = automatic,
					PreferenceVariable = preferenceVariable,
					Assigned = false
				};
				this._variables.Add(variableName, value);
			}
		}

		// Token: 0x04002097 RID: 8343
		private static readonly HashSet<string> hashOfPessimizingCmdlets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002098 RID: 8344
		private static readonly string[] pessimizingCmdlets = new string[]
		{
			"New-Variable",
			"Remove-Variable",
			"Set-Variable",
			"Set-PSBreakpoint",
			"Microsoft.PowerShell.Utility\\New-Variable",
			"Microsoft.PowerShell.Utility\\Remove-Variable",
			"Microsoft.PowerShell.Utility\\Set-Variable",
			"Microsoft.PowerShell.Utility\\Set-PSBreakpoint",
			"nv",
			"rv",
			"sbp",
			"sv",
			"set"
		};

		// Token: 0x04002099 RID: 8345
		private bool _disableOptimizations;

		// Token: 0x0400209A RID: 8346
		private readonly Dictionary<string, VariableAnalysisDetails> _variables = new Dictionary<string, VariableAnalysisDetails>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400209B RID: 8347
		private int _runtimeUsingIndex;
	}
}
