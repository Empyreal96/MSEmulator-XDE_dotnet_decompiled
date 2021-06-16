using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020005F8 RID: 1528
	internal class ScriptBlockToPowerShellConverter
	{
		// Token: 0x060041D1 RID: 16849 RVA: 0x0015C146 File Offset: 0x0015A346
		private ScriptBlockToPowerShellConverter()
		{
			this._powershell = PowerShell.Create();
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0015C15C File Offset: 0x0015A35C
		internal static PowerShell Convert(ScriptBlockAst body, ReadOnlyCollection<ParameterAst> functionParameters, bool isTrustedInput, ExecutionContext context, Dictionary<string, object> variables, bool filterNonUsingVariables, bool? createLocalScope, object[] args)
		{
			ExecutionContext.CheckStackDepth();
			if (args == null)
			{
				args = ScriptBlock.EmptyArray;
			}
			string text;
			string message;
			body.GetSimplePipeline(true, out text, out message);
			if (text != null)
			{
				throw new ScriptBlockToPowerShellNotSupportedException(text, null, message, new object[0]);
			}
			ScriptBlockToPowerShellChecker scriptBlockToPowerShellChecker = new ScriptBlockToPowerShellChecker
			{
				ScriptBeingConverted = body
			};
			if (functionParameters != null)
			{
				foreach (ParameterAst parameterAst in functionParameters)
				{
					parameterAst.InternalVisit(scriptBlockToPowerShellChecker);
				}
			}
			body.InternalVisit(scriptBlockToPowerShellChecker);
			if (context == null && (scriptBlockToPowerShellChecker.HasUsingExpr || scriptBlockToPowerShellChecker.UsesParameter) && variables == null)
			{
				throw new PSInvalidOperationException(AutomationExceptions.CantConvertScriptBlockWithNoContext);
			}
			PowerShell powershell;
			try
			{
				ScriptBlockToPowerShellConverter scriptBlockToPowerShellConverter = new ScriptBlockToPowerShellConverter
				{
					_context = context,
					_createLocalScope = createLocalScope
				};
				if (scriptBlockToPowerShellChecker.HasUsingExpr)
				{
					scriptBlockToPowerShellConverter._usingValueMap = ScriptBlockToPowerShellConverter.GetUsingValues(body, isTrustedInput, context, variables, filterNonUsingVariables).Item1;
				}
				if (scriptBlockToPowerShellChecker.UsesParameter)
				{
					SessionStateScope sessionStateScope = context.EngineSessionState.NewScope(false);
					context.EngineSessionState.CurrentScope = sessionStateScope;
					context.EngineSessionState.CurrentScope.ScopeOrigin = CommandOrigin.Internal;
					MutableTuple mutableTuple = MutableTuple.MakeTuple(Compiler.DottedLocalsTupleType, Compiler.DottedLocalsNameIndexMap);
					bool flag = false;
					RuntimeDefinedParameterDictionary runtimeDefinedParameterDictionary = (functionParameters != null) ? Compiler.GetParameterMetaData(functionParameters, true, ref flag) : ((IParameterMetadataProvider)body).GetParameterMetadata(true, ref flag);
					object[] value = ScriptBlock.BindArgumentsForScripblockInvoke((RuntimeDefinedParameter[])runtimeDefinedParameterDictionary.Data, args, context, false, null, mutableTuple);
					mutableTuple.SetAutomaticVariable(AutomaticVariable.Args, value, context);
					sessionStateScope.LocalsTuple = mutableTuple;
				}
				foreach (PipelineAst pipelineAst in body.EndBlock.Statements.OfType<PipelineAst>())
				{
					scriptBlockToPowerShellConverter._powershell.AddStatement();
					scriptBlockToPowerShellConverter.ConvertPipeline(pipelineAst, isTrustedInput);
				}
				powershell = scriptBlockToPowerShellConverter._powershell;
			}
			finally
			{
				if (scriptBlockToPowerShellChecker.UsesParameter)
				{
					context.EngineSessionState.RemoveScope(context.EngineSessionState.CurrentScope);
				}
			}
			return powershell;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x0015C3A0 File Offset: 0x0015A5A0
		internal static Dictionary<string, object> GetUsingValuesAsDictionary(ScriptBlock scriptBlock, bool isTrustedInput, ExecutionContext context, Dictionary<string, object> variables)
		{
			return ScriptBlockToPowerShellConverter.GetUsingValues(scriptBlock.Ast, isTrustedInput, context, variables, false).Item1;
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x0015C3B6 File Offset: 0x0015A5B6
		internal static object[] GetUsingValuesAsArray(ScriptBlock scriptBlock, bool isTrustedInput, ExecutionContext context, Dictionary<string, object> variables)
		{
			return ScriptBlockToPowerShellConverter.GetUsingValues(scriptBlock.Ast, isTrustedInput, context, variables, false).Item2;
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x0015C3CC File Offset: 0x0015A5CC
		private static Tuple<Dictionary<string, object>, object[]> GetUsingValues(Ast body, bool isTrustedInput, ExecutionContext context, Dictionary<string, object> variables, bool filterNonUsingVariables)
		{
			List<Ast> list = UsingExpressionAstSearcher.FindAllUsingExpressionExceptForWorkflow(body).ToList<Ast>();
			object[] array = new object[list.Count];
			Dictionary<string, object> dictionary = new Dictionary<string, object>(list.Count);
			HashSet<string> hashSet = (variables != null && filterNonUsingVariables) ? new HashSet<string>() : null;
			bool flag = false;
			ScriptBlockAst scriptBlockAst = null;
			UsingExpressionAst usingExpressionAst = null;
			Version strictModeVersion = null;
			try
			{
				if (context != null)
				{
					strictModeVersion = context.EngineSessionState.CurrentScope.StrictModeVersion;
					context.EngineSessionState.CurrentScope.StrictModeVersion = PSVersionInfo.PSVersion;
				}
				for (int i = 0; i < list.Count; i++)
				{
					usingExpressionAst = (UsingExpressionAst)list[i];
					object obj = null;
					if (!flag && ScriptBlockToPowerShellConverter.HasUsingExpressionsInDifferentScopes(usingExpressionAst, body, ref scriptBlockAst))
					{
						flag = true;
					}
					if (variables != null)
					{
						VariableExpressionAst variableExpressionAst = usingExpressionAst.SubExpression as VariableExpressionAst;
						if (variableExpressionAst == null)
						{
							throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), usingExpressionAst.Extent, "CantGetUsingExpressionValueWithSpecifiedVariableDictionary", AutomationExceptions.CantGetUsingExpressionValueWithSpecifiedVariableDictionary, new object[]
							{
								usingExpressionAst.Extent.Text
							});
						}
						string userPath = variableExpressionAst.VariablePath.UserPath;
						if (userPath != null && variables.TryGetValue(userPath, out obj) && hashSet != null)
						{
							hashSet.Add(userPath);
						}
					}
					else
					{
						obj = Compiler.GetExpressionValue(usingExpressionAst.SubExpression, isTrustedInput, context, null);
					}
					array[i] = obj;
					string usingExpressionKey = PsUtils.GetUsingExpressionKey(usingExpressionAst);
					if (!dictionary.ContainsKey(usingExpressionKey))
					{
						dictionary.Add(usingExpressionKey, obj);
					}
				}
			}
			catch (RuntimeException ex)
			{
				if (ex.ErrorRecord.FullyQualifiedErrorId.Equals("VariableIsUndefined", StringComparison.Ordinal))
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), usingExpressionAst.Extent, "UsingVariableIsUndefined", AutomationExceptions.UsingVariableIsUndefined, new object[]
					{
						ex.ErrorRecord.TargetObject
					});
				}
				if (ex.ErrorRecord.FullyQualifiedErrorId.Equals("CantGetUsingExpressionValueWithSpecifiedVariableDictionary", StringComparison.Ordinal))
				{
					throw;
				}
			}
			finally
			{
				if (context != null)
				{
					context.EngineSessionState.CurrentScope.StrictModeVersion = strictModeVersion;
				}
			}
			if (hashSet != null)
			{
				string[] array2 = variables.Keys.ToArray<string>();
				foreach (string text in array2)
				{
					if (!hashSet.Contains(text))
					{
						variables.Remove(text);
					}
				}
			}
			if (flag)
			{
				array = null;
			}
			return Tuple.Create<Dictionary<string, object>, object[]>(dictionary, array);
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x0015C644 File Offset: 0x0015A844
		private static bool HasUsingExpressionsInDifferentScopes(UsingExpressionAst usingExpr, Ast topLevelParent, ref ScriptBlockAst sbClosestToPreviousUsingExpr)
		{
			Ast ast = usingExpr;
			ScriptBlockAst scriptBlockAst;
			FunctionDefinitionAst functionDefinitionAst;
			for (;;)
			{
				ast = ast.Parent;
				scriptBlockAst = (ast as ScriptBlockAst);
				if (scriptBlockAst != null)
				{
					break;
				}
				functionDefinitionAst = (ast as FunctionDefinitionAst);
				if (functionDefinitionAst != null)
				{
					goto Block_4;
				}
				if (ast == topLevelParent)
				{
					return true;
				}
			}
			if (sbClosestToPreviousUsingExpr == null)
			{
				sbClosestToPreviousUsingExpr = scriptBlockAst;
				return false;
			}
			return scriptBlockAst != sbClosestToPreviousUsingExpr;
			Block_4:
			if (sbClosestToPreviousUsingExpr == null)
			{
				sbClosestToPreviousUsingExpr = functionDefinitionAst.Body;
				return false;
			}
			return functionDefinitionAst.Body != sbClosestToPreviousUsingExpr;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x0015C6A4 File Offset: 0x0015A8A4
		private void ConvertPipeline(PipelineAst pipelineAst, bool isTrustedInput)
		{
			foreach (CommandBaseAst commandBaseAst in pipelineAst.PipelineElements)
			{
				this.ConvertCommand((CommandAst)commandBaseAst, isTrustedInput);
			}
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0015C6F8 File Offset: 0x0015A8F8
		private void ConvertCommand(CommandAst commandAst, bool isTrustedInput)
		{
			string commandName = this.GetCommandName(commandAst.CommandElements[0], isTrustedInput);
			Command command = new Command(commandName, false, this._createLocalScope);
			if (commandAst.Redirections.Count > 0)
			{
				PipelineResultTypes toResult = PipelineResultTypes.Output;
				PipelineResultTypes myResult;
				switch (commandAst.Redirections[0].FromStream)
				{
				case RedirectionStream.All:
					myResult = PipelineResultTypes.All;
					goto IL_84;
				case RedirectionStream.Error:
					myResult = PipelineResultTypes.Error;
					goto IL_84;
				case RedirectionStream.Warning:
					myResult = PipelineResultTypes.Warning;
					goto IL_84;
				case RedirectionStream.Verbose:
					myResult = PipelineResultTypes.Verbose;
					goto IL_84;
				case RedirectionStream.Debug:
					myResult = PipelineResultTypes.Debug;
					goto IL_84;
				case RedirectionStream.Information:
					myResult = PipelineResultTypes.Information;
					goto IL_84;
				}
				myResult = PipelineResultTypes.Error;
				IL_84:
				command.MergeMyResults(myResult, toResult);
			}
			this._powershell.AddCommand(command);
			foreach (CommandElementAst commandElementAst in commandAst.CommandElements.Skip(1))
			{
				ExpressionAst expressionAst = commandElementAst as ExpressionAst;
				if (expressionAst != null)
				{
					UsingExpressionAst usingExpressionAst = commandElementAst as UsingExpressionAst;
					if (usingExpressionAst != null)
					{
						string usingExpressionKey = PsUtils.GetUsingExpressionKey(usingExpressionAst);
						object obj = this._usingValueMap[usingExpressionKey];
						VariableExpressionAst variableExpressionAst = usingExpressionAst.SubExpression as VariableExpressionAst;
						if (variableExpressionAst != null && variableExpressionAst.Splatted)
						{
							IDictionary dictionary = obj as IDictionary;
							if (dictionary != null)
							{
								this._powershell.AddParameters(dictionary);
							}
							else
							{
								IEnumerable enumerable = obj as IEnumerable;
								if (enumerable != null)
								{
									using (IEnumerator enumerator2 = enumerable.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											object value = enumerator2.Current;
											this._powershell.AddArgument(value);
										}
										continue;
									}
								}
								this._powershell.AddArgument(obj);
							}
						}
						else
						{
							this._powershell.AddArgument(obj);
						}
					}
					else
					{
						VariableExpressionAst variableExpressionAst = commandElementAst as VariableExpressionAst;
						if (variableExpressionAst != null && variableExpressionAst.Splatted)
						{
							this.GetSplattedVariable(variableExpressionAst);
						}
						else
						{
							ConstantExpressionAst constantExpressionAst = commandElementAst as ConstantExpressionAst;
							object obj2;
							if (constantExpressionAst != null && LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(constantExpressionAst.StaticType)))
							{
								string text = constantExpressionAst.Extent.Text;
								obj2 = constantExpressionAst.Value;
								if (!text.Equals(constantExpressionAst.Value.ToString(), StringComparison.Ordinal))
								{
									obj2 = ParserOps.WrappedNumber(obj2, text);
								}
							}
							else
							{
								if (!isTrustedInput)
								{
									try
									{
										obj2 = GetSafeValueVisitor.GetSafeValue(expressionAst, this._context, true);
										goto IL_277;
									}
									catch (Exception)
									{
										throw new ScriptBlockToPowerShellNotSupportedException("CantConvertWithDynamicExpression", null, AutomationExceptions.CantConvertWithDynamicExpression, new object[]
										{
											expressionAst.Extent.Text
										});
									}
								}
								obj2 = this.GetExpressionValue(expressionAst, isTrustedInput);
							}
							IL_277:
							this._powershell.AddArgument(obj2);
						}
					}
				}
				else
				{
					this.AddParameter((CommandParameterAst)commandElementAst, isTrustedInput);
				}
			}
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x0015CA00 File Offset: 0x0015AC00
		private string GetCommandName(CommandElementAst commandNameAst, bool isTrustedInput)
		{
			ExpressionAst expressionAst = commandNameAst as ExpressionAst;
			string text;
			if (expressionAst != null)
			{
				object expressionValue = this.GetExpressionValue(expressionAst, isTrustedInput);
				if (expressionValue == null)
				{
					ScriptBlockToPowerShellChecker.ThrowError(new ScriptBlockToPowerShellNotSupportedException("CantConvertWithScriptBlockInvocation", null, AutomationExceptions.CantConvertWithScriptBlockInvocation, new object[0]), expressionAst);
				}
				if (expressionValue is CommandInfo)
				{
					text = ((CommandInfo)expressionValue).Name;
				}
				else
				{
					text = (expressionValue as string);
				}
			}
			else
			{
				text = commandNameAst.Extent.Text;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ScriptBlockToPowerShellNotSupportedException("CantConvertWithScriptBlockInvocation", null, AutomationExceptions.CantConvertWithScriptBlockInvocation, new object[0]);
			}
			return text;
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x0015CA8C File Offset: 0x0015AC8C
		private void GetSplattedVariable(VariableExpressionAst variableAst)
		{
			if (this._context == null)
			{
				throw new PSInvalidOperationException(AutomationExceptions.CantConvertScriptBlockWithNoContext);
			}
			object variableValue = this._context.GetVariableValue(variableAst.VariablePath);
			foreach (CommandParameterInternal internalParameter in PipelineOps.Splat(variableValue, variableAst.Extent))
			{
				CommandParameter commandParameter = CommandParameter.FromCommandParameterInternal(internalParameter);
				this._powershell.AddParameter(commandParameter.Name, commandParameter.Value);
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0015CB1C File Offset: 0x0015AD1C
		private object GetExpressionValue(ExpressionAst exprAst, bool isTrustedInput)
		{
			if (this._context == null)
			{
				Runspace runspace = RunspaceFactory.CreateRunspace(InitialSessionState.Create());
				runspace.Open();
				this._context = runspace.ExecutionContext;
			}
			if (!isTrustedInput)
			{
				return GetSafeValueVisitor.GetSafeValue(exprAst, this._context, true);
			}
			return Compiler.GetExpressionValue(exprAst, isTrustedInput, this._context, this._usingValueMap);
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x0015CB74 File Offset: 0x0015AD74
		private void AddParameter(CommandParameterAst commandParameterAst, bool isTrustedInput)
		{
			string text;
			object value;
			if (commandParameterAst.Argument != null)
			{
				ExpressionAst argument = commandParameterAst.Argument;
				IScriptExtent errorPosition = commandParameterAst.ErrorPosition;
				text = ((errorPosition.EndLineNumber != argument.Extent.StartLineNumber || errorPosition.EndColumnNumber != argument.Extent.StartColumnNumber) ? ": " : ":");
				value = this.GetExpressionValue(commandParameterAst.Argument, isTrustedInput);
			}
			else
			{
				text = "";
				value = null;
			}
			this._powershell.AddParameter(string.Format(CultureInfo.InvariantCulture, "-{0}{1}", new object[]
			{
				commandParameterAst.ParameterName,
				text
			}), value);
		}

		// Token: 0x040020FD RID: 8445
		private readonly PowerShell _powershell;

		// Token: 0x040020FE RID: 8446
		private ExecutionContext _context;

		// Token: 0x040020FF RID: 8447
		private Dictionary<string, object> _usingValueMap;

		// Token: 0x04002100 RID: 8448
		private bool? _createLocalScope;
	}
}
