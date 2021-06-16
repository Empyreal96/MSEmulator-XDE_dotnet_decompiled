using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000629 RID: 1577
	internal static class PipelineOps
	{
		// Token: 0x06004488 RID: 17544 RVA: 0x0016E154 File Offset: 0x0016C354
		private static CommandProcessorBase AddCommand(PipelineProcessor pipe, CommandParameterInternal[] commandElements, CommandBaseAst commandBaseAst, CommandRedirection[] redirections, ExecutionContext context)
		{
			CommandAst commandAst = commandBaseAst as CommandAst;
			TokenKind tokenKind = (commandAst != null) ? commandAst.InvocationOperator : TokenKind.Unknown;
			bool flag = tokenKind == TokenKind.Dot;
			SessionStateInternal sessionStateInternal = null;
			int num = 0;
			PSModuleInfo psmoduleInfo = PSObject.Base(commandElements[0].ArgumentValue) as PSModuleInfo;
			if (psmoduleInfo != null)
			{
				if (psmoduleInfo.ModuleType == ModuleType.Binary && psmoduleInfo.SessionState == null)
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "CantInvokeInBinaryModule", ParserStrings.CantInvokeInBinaryModule, new object[]
					{
						psmoduleInfo.Name
					});
				}
				if (psmoduleInfo.SessionState == null)
				{
					throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "CantInvokeInNonImportedModule", ParserStrings.CantInvokeInNonImportedModule, new object[]
					{
						psmoduleInfo.Name
					});
				}
				sessionStateInternal = psmoduleInfo.SessionState.Internal;
				num++;
			}
			CommandParameterInternal commandParameterInternal = commandElements[num];
			object obj;
			IScriptExtent scriptExtent;
			if (commandParameterInternal.ParameterNameSpecified)
			{
				obj = commandParameterInternal.ParameterText;
				scriptExtent = commandParameterInternal.ParameterExtent;
				if (commandParameterInternal.ArgumentSpecified)
				{
				}
			}
			else
			{
				obj = PSObject.Base(commandParameterInternal.ArgumentValue);
				scriptExtent = commandParameterInternal.ArgumentExtent;
			}
			string text = flag ? "." : ((tokenKind == TokenKind.Ampersand) ? "&" : null);
			ScriptBlock scriptBlock = obj as ScriptBlock;
			CommandProcessorBase commandProcessorBase;
			if (scriptBlock != null)
			{
				commandProcessorBase = CommandDiscovery.CreateCommandProcessorForScript(scriptBlock, context, !flag, sessionStateInternal);
			}
			else
			{
				CommandInfo commandInfo = obj as CommandInfo;
				if (commandInfo != null)
				{
					commandProcessorBase = context.CommandDiscovery.LookupCommandProcessor(commandInfo, context.EngineSessionState.CurrentScope.ScopeOrigin, new bool?(!flag), sessionStateInternal);
				}
				else
				{
					string text2 = (obj as string) ?? PSObject.ToStringParser(context, obj);
					text = (text ?? text2);
					if (string.IsNullOrEmpty(text2))
					{
						throw InterpreterError.NewInterpreterException(obj, typeof(RuntimeException), scriptExtent, "BadExpression", ParserStrings.BadExpression, new object[]
						{
							flag ? "." : "&"
						});
					}
					try
					{
						if (sessionStateInternal != null)
						{
							SessionStateInternal engineSessionState = context.EngineSessionState;
							try
							{
								context.EngineSessionState = sessionStateInternal;
								commandProcessorBase = context.CreateCommand(text2, flag);
								goto IL_222;
							}
							finally
							{
								context.EngineSessionState = engineSessionState;
							}
						}
						commandProcessorBase = context.CreateCommand(text2, flag);
						IL_222:;
					}
					catch (RuntimeException ex)
					{
						if (ex.ErrorRecord.InvocationInfo == null)
						{
							InvocationInfo invocationInfo = new InvocationInfo(null, scriptExtent, context)
							{
								InvocationName = text
							};
							ex.ErrorRecord.SetInvocationInfo(invocationInfo);
						}
						throw;
					}
				}
			}
			InternalCommand command = commandProcessorBase.Command;
			commandProcessorBase.UseLocalScope = (!flag && (command is ScriptCommand || command is PSScriptCmdlet));
			bool flag2 = commandProcessorBase is NativeCommandProcessor;
			for (int i = num + 1; i < commandElements.Length; i++)
			{
				CommandParameterInternal commandParameterInternal2 = commandElements[i];
				if (!commandParameterInternal2.ParameterNameSpecified || !commandParameterInternal2.ParameterName.Equals("-", StringComparison.OrdinalIgnoreCase) || flag2)
				{
					if (commandParameterInternal2.ArgumentSplatted)
					{
						using (IEnumerator<CommandParameterInternal> enumerator = PipelineOps.Splat(commandParameterInternal2.ArgumentValue, commandParameterInternal2.ArgumentExtent).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CommandParameterInternal parameter = enumerator.Current;
								commandProcessorBase.AddParameter(parameter);
							}
							goto IL_31E;
						}
					}
					commandProcessorBase.AddParameter(commandParameterInternal2);
				}
				IL_31E:;
			}
			string helpTarget;
			HelpCategory helpCategory;
			if (commandProcessorBase.IsHelpRequested(out helpTarget, out helpCategory))
			{
				commandProcessorBase = CommandProcessorBase.CreateGetHelpCommandProcessor(context, helpTarget, helpCategory);
			}
			commandProcessorBase.Command.InvocationExtent = commandBaseAst.Extent;
			commandProcessorBase.Command.MyInvocation.ScriptPosition = commandBaseAst.Extent;
			commandProcessorBase.Command.MyInvocation.InvocationName = text;
			pipe.Add(commandProcessorBase);
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			if (redirections != null)
			{
				foreach (CommandRedirection commandRedirection in redirections)
				{
					commandRedirection.Bind(pipe, commandProcessorBase, context);
					switch (commandRedirection.FromStream)
					{
					case RedirectionStream.All:
						flag3 = true;
						flag4 = true;
						flag5 = true;
						flag6 = true;
						flag7 = true;
						break;
					case RedirectionStream.Error:
						flag3 = true;
						break;
					case RedirectionStream.Warning:
						flag4 = true;
						break;
					case RedirectionStream.Verbose:
						flag5 = true;
						break;
					case RedirectionStream.Debug:
						flag6 = true;
						break;
					case RedirectionStream.Information:
						flag7 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				if (context.ShellFunctionErrorOutputPipe != null)
				{
					commandProcessorBase.CommandRuntime.ErrorOutputPipe = context.ShellFunctionErrorOutputPipe;
				}
				else
				{
					commandProcessorBase.CommandRuntime.ErrorOutputPipe.ExternalWriter = context.ExternalErrorOutput;
				}
			}
			if (!flag4 && context.ExpressionWarningOutputPipe != null)
			{
				commandProcessorBase.CommandRuntime.WarningOutputPipe = context.ExpressionWarningOutputPipe;
				flag4 = true;
			}
			if (!flag5 && context.ExpressionVerboseOutputPipe != null)
			{
				commandProcessorBase.CommandRuntime.VerboseOutputPipe = context.ExpressionVerboseOutputPipe;
				flag5 = true;
			}
			if (!flag6 && context.ExpressionDebugOutputPipe != null)
			{
				commandProcessorBase.CommandRuntime.DebugOutputPipe = context.ExpressionDebugOutputPipe;
				flag6 = true;
			}
			if (!flag7 && context.ExpressionInformationOutputPipe != null)
			{
				commandProcessorBase.CommandRuntime.InformationOutputPipe = context.ExpressionInformationOutputPipe;
				flag7 = true;
			}
			if (context.CurrentCommandProcessor != null && context.CurrentCommandProcessor.CommandRuntime != null)
			{
				if (!flag4 && context.CurrentCommandProcessor.CommandRuntime.WarningOutputPipe != null)
				{
					commandProcessorBase.CommandRuntime.WarningOutputPipe = context.CurrentCommandProcessor.CommandRuntime.WarningOutputPipe;
				}
				if (!flag5 && context.CurrentCommandProcessor.CommandRuntime.VerboseOutputPipe != null)
				{
					commandProcessorBase.CommandRuntime.VerboseOutputPipe = context.CurrentCommandProcessor.CommandRuntime.VerboseOutputPipe;
				}
				if (!flag6 && context.CurrentCommandProcessor.CommandRuntime.DebugOutputPipe != null)
				{
					commandProcessorBase.CommandRuntime.DebugOutputPipe = context.CurrentCommandProcessor.CommandRuntime.DebugOutputPipe;
				}
				if (!flag7 && context.CurrentCommandProcessor.CommandRuntime.InformationOutputPipe != null)
				{
					commandProcessorBase.CommandRuntime.InformationOutputPipe = context.CurrentCommandProcessor.CommandRuntime.InformationOutputPipe;
				}
			}
			return commandProcessorBase;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0016EAB8 File Offset: 0x0016CCB8
		internal static IEnumerable<CommandParameterInternal> Splat(object splattedValue, IScriptExtent splatExtent)
		{
			splattedValue = PSObject.Base(splattedValue);
			IDictionary splattedTable = splattedValue as IDictionary;
			if (splattedTable != null)
			{
				foreach (object obj2 in splattedTable)
				{
					DictionaryEntry de = (DictionaryEntry)obj2;
					DictionaryEntry dictionaryEntry = de;
					string parameterName = dictionaryEntry.Key.ToString();
					DictionaryEntry dictionaryEntry2 = de;
					object parameterValue = dictionaryEntry2.Value;
					string parameterText = PipelineOps.GetParameterText(parameterName);
					yield return CommandParameterInternal.CreateParameterWithArgument(splatExtent, parameterName, parameterText, splatExtent, parameterValue, false, false);
				}
			}
			else
			{
				IEnumerable enumerableValue = splattedValue as IEnumerable;
				if (enumerableValue != null)
				{
					foreach (object obj in enumerableValue)
					{
						yield return PipelineOps.SplatEnumerableElement(obj, splatExtent);
					}
				}
				else
				{
					yield return PipelineOps.SplatEnumerableElement(splattedValue, splatExtent);
				}
			}
			yield break;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0016EADC File Offset: 0x0016CCDC
		private static CommandParameterInternal SplatEnumerableElement(object splattedArgument, IScriptExtent splatExtent)
		{
			PSObject psobject = splattedArgument as PSObject;
			if (psobject != null)
			{
				PSPropertyInfo pspropertyInfo = psobject.Properties["<CommandParameterName>"];
				object baseObject = psobject.BaseObject;
				if (pspropertyInfo != null && pspropertyInfo.Value is string && baseObject is string)
				{
					return CommandParameterInternal.CreateParameter(splatExtent, (string)pspropertyInfo.Value, (string)baseObject);
				}
			}
			return CommandParameterInternal.CreateArgument(splatExtent, splattedArgument, false, false);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0016EB44 File Offset: 0x0016CD44
		private static string GetParameterText(string parameterName)
		{
			int num = parameterName.Length;
			while (num > 0 && char.IsWhiteSpace(parameterName[num - 1]))
			{
				num--;
			}
			if (num == 0 || parameterName[num - 1] == ':')
			{
				return "-" + parameterName;
			}
			string result;
			if (num == parameterName.Length)
			{
				result = "-" + parameterName + ":";
			}
			else
			{
				string str = parameterName.Substring(num);
				result = "-" + parameterName.Substring(0, num) + ":" + str;
			}
			return result;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0016EBCC File Offset: 0x0016CDCC
		internal static void InvokePipeline(object input, bool ignoreInput, CommandParameterInternal[][] pipeElements, CommandBaseAst[] pipeElementAsts, CommandRedirection[][] commandRedirections, FunctionContext funcContext)
		{
			PipelineProcessor pipelineProcessor = new PipelineProcessor();
			ExecutionContext executionContext = funcContext._executionContext;
			Pipe outputPipe = funcContext._outputPipe;
			try
			{
				if (executionContext.Events != null)
				{
					executionContext.Events.ProcessPendingActions();
				}
				if (input == AutomationNull.Value && !ignoreInput)
				{
					PipelineOps.AddNoopCommandProcessor(pipelineProcessor, executionContext);
				}
				CommandProcessorBase commandProcessorBase = null;
				CommandRedirection[] array = null;
				for (int i = 0; i < pipeElements.Length; i++)
				{
					array = ((commandRedirections != null) ? commandRedirections[i] : null);
					commandProcessorBase = PipelineOps.AddCommand(pipelineProcessor, pipeElements[i], pipeElementAsts[i], array, executionContext);
				}
				if (commandProcessorBase != null && !commandProcessorBase.CommandRuntime.OutputPipe.IsRedirected)
				{
					pipelineProcessor.LinkPipelineSuccessOutput(outputPipe ?? new Pipe(new List<object>()));
					if (array != null)
					{
						foreach (CommandRedirection commandRedirection in array)
						{
							if (commandRedirection is MergingRedirection)
							{
								commandRedirection.Bind(pipelineProcessor, commandProcessorBase, executionContext);
							}
						}
					}
				}
				executionContext.PushPipelineProcessor(pipelineProcessor);
				try
				{
					pipelineProcessor.SynchronousExecuteEnumerate(input, null, true);
				}
				finally
				{
					executionContext.PopPipelineProcessor(false);
				}
			}
			finally
			{
				executionContext.QuestionMarkVariableValue = !pipelineProcessor.ExecutionFailed;
				pipelineProcessor.Dispose();
			}
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0016ECFC File Offset: 0x0016CEFC
		private static void AddNoopCommandProcessor(PipelineProcessor pipelineProcessor, ExecutionContext context)
		{
			CmdletInfo commandInfo = new CmdletInfo("Out-Null", typeof(OutNullCommand));
			CommandProcessorBase commandProcessor = context.CommandDiscovery.LookupCommandProcessor(commandInfo, context.EngineSessionState.CurrentScope.ScopeOrigin, new bool?(false), null);
			pipelineProcessor.Add(commandProcessor);
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0016ED4C File Offset: 0x0016CF4C
		internal static object CheckAutomationNullInCommandArgument(object obj)
		{
			if (obj == AutomationNull.Value)
			{
				return null;
			}
			object[] array = obj as object[];
			if (array == null)
			{
				return obj;
			}
			return PipelineOps.CheckAutomationNullInCommandArgumentArray(array);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0016ED78 File Offset: 0x0016CF78
		internal static object[] CheckAutomationNullInCommandArgumentArray(object[] objArray)
		{
			if (objArray != null)
			{
				for (int i = 0; i < objArray.Length; i++)
				{
					if (objArray[i] == AutomationNull.Value)
					{
						objArray[i] = null;
					}
				}
			}
			return objArray;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0016EDA8 File Offset: 0x0016CFA8
		internal static SteppablePipeline GetSteppablePipeline(PipelineAst pipelineAst, CommandOrigin commandOrigin, ScriptBlock scriptBlock, object[] args)
		{
			PipelineProcessor pipelineProcessor = new PipelineProcessor();
			List<Tuple<CommandAst, List<CommandParameterInternal>, List<CommandRedirection>>> list = new List<Tuple<CommandAst, List<CommandParameterInternal>, List<CommandRedirection>>>();
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				string text = scriptBlock.ToString();
				text = ErrorCategoryInfo.Ellipsize(CultureInfo.CurrentUICulture, text);
				PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(ParserStrings.GetSteppablePipelineFromWrongThread, new object[]
				{
					text
				});
				ex.SetErrorId("GetSteppablePipelineFromWrongThread");
				throw ex;
			}
			bool flag = args != null && args.Length > 0 && scriptBlock.RuntimeDefinedParameters.Data != RuntimeDefinedParameterDictionary.EmptyParameterArray;
			try
			{
				if (flag)
				{
					SessionStateScope sessionStateScope = executionContextFromTLS.EngineSessionState.NewScope(false);
					executionContextFromTLS.EngineSessionState.CurrentScope = sessionStateScope;
					executionContextFromTLS.EngineSessionState.CurrentScope.ScopeOrigin = CommandOrigin.Internal;
					MutableTuple mutableTuple = MutableTuple.MakeTuple(Compiler.DottedLocalsTupleType, Compiler.DottedLocalsNameIndexMap);
					object[] value = ScriptBlock.BindArgumentsForScripblockInvoke((RuntimeDefinedParameter[])scriptBlock.RuntimeDefinedParameters.Data, args, executionContextFromTLS, false, null, mutableTuple);
					mutableTuple.SetAutomaticVariable(AutomaticVariable.Args, value, executionContextFromTLS);
					sessionStateScope.LocalsTuple = mutableTuple;
				}
				bool flag2 = scriptBlock.LanguageMode == PSLanguageMode.FullLanguage;
				foreach (CommandAst commandAst in pipelineAst.PipelineElements.Cast<CommandAst>())
				{
					List<CommandParameterInternal> list2 = new List<CommandParameterInternal>();
					foreach (CommandElementAst commandElementAst in commandAst.CommandElements)
					{
						CommandParameterAst commandParameterAst = commandElementAst as CommandParameterAst;
						if (commandParameterAst != null)
						{
							list2.Add(PipelineOps.GetCommandParameter(commandParameterAst, flag2, executionContextFromTLS));
						}
						else
						{
							ExpressionAst expressionAst = (ExpressionAst)commandElementAst;
							object expressionValue = Compiler.GetExpressionValue(expressionAst, flag2, executionContextFromTLS, null);
							bool splatted = expressionAst is VariableExpressionAst && ((VariableExpressionAst)expressionAst).Splatted;
							list2.Add(CommandParameterInternal.CreateArgument(expressionAst.Extent, expressionValue, splatted, false));
						}
					}
					List<CommandRedirection> list3 = new List<CommandRedirection>();
					foreach (RedirectionAst redirectionAst in commandAst.Redirections)
					{
						list3.Add(PipelineOps.GetCommandRedirection(redirectionAst, flag2, executionContextFromTLS));
					}
					list.Add(Tuple.Create<CommandAst, List<CommandParameterInternal>, List<CommandRedirection>>(commandAst, list2, list3));
				}
			}
			finally
			{
				if (flag)
				{
					executionContextFromTLS.EngineSessionState.RemoveScope(executionContextFromTLS.EngineSessionState.CurrentScope);
				}
			}
			foreach (Tuple<CommandAst, List<CommandParameterInternal>, List<CommandRedirection>> tuple in list)
			{
				CommandProcessorBase commandProcessorBase = PipelineOps.AddCommand(pipelineProcessor, tuple.Item2.ToArray(), tuple.Item1, tuple.Item3.ToArray(), executionContextFromTLS);
				commandProcessorBase.Command.CommandOriginInternal = commandOrigin;
				commandProcessorBase.CommandScope.ScopeOrigin = commandOrigin;
				commandProcessorBase.Command.MyInvocation.CommandOrigin = commandOrigin;
				CallStackFrame[] array = executionContextFromTLS.Debugger.GetCallStack().ToArray<CallStackFrame>();
				if (array.Length > 0 && Regex.IsMatch(array[0].Position.Text, "GetSteppablePipeline", RegexOptions.IgnoreCase))
				{
					InvocationInfo myInvocation = commandProcessorBase.Command.MyInvocation;
					myInvocation.InvocationName = array[0].InvocationInfo.InvocationName;
					if (array.Length > 1)
					{
						IScriptExtent position = array[1].Position;
						if (position != null && position != PositionUtilities.EmptyExtent)
						{
							myInvocation.DisplayScriptPosition = position;
						}
					}
				}
				if (executionContextFromTLS.CurrentCommandProcessor != null && executionContextFromTLS.CurrentCommandProcessor.CommandRuntime != null)
				{
					commandProcessorBase.CommandRuntime.SetMergeFromRuntime(executionContextFromTLS.CurrentCommandProcessor.CommandRuntime);
				}
			}
			return new SteppablePipeline(executionContextFromTLS, pipelineProcessor);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0016F1BC File Offset: 0x0016D3BC
		private static CommandParameterInternal GetCommandParameter(CommandParameterAst commandParameterAst, bool isTrusted, ExecutionContext context)
		{
			ExpressionAst argument = commandParameterAst.Argument;
			IScriptExtent errorPosition = commandParameterAst.ErrorPosition;
			if (argument == null)
			{
				return CommandParameterInternal.CreateParameter(errorPosition, commandParameterAst.ParameterName, errorPosition.Text);
			}
			object expressionValue = Compiler.GetExpressionValue(argument, isTrusted, context, null);
			bool spaceAfterParameter = errorPosition.EndLineNumber != argument.Extent.StartLineNumber || errorPosition.EndColumnNumber != argument.Extent.StartColumnNumber;
			return CommandParameterInternal.CreateParameterWithArgument(errorPosition, commandParameterAst.ParameterName, errorPosition.Text, argument.Extent, expressionValue, spaceAfterParameter, false);
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0016F240 File Offset: 0x0016D440
		private static CommandRedirection GetCommandRedirection(RedirectionAst redirectionAst, bool isTrusted, ExecutionContext context)
		{
			FileRedirectionAst fileRedirectionAst = redirectionAst as FileRedirectionAst;
			if (fileRedirectionAst != null)
			{
				object expressionValue = Compiler.GetExpressionValue(fileRedirectionAst.Location, isTrusted, context, null);
				return new FileRedirection(fileRedirectionAst.FromStream, fileRedirectionAst.Append, expressionValue.ToString());
			}
			MergingRedirectionAst mergingRedirectionAst = (MergingRedirectionAst)redirectionAst;
			return new MergingRedirection(mergingRedirectionAst.FromStream, mergingRedirectionAst.ToStream);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0016F298 File Offset: 0x0016D498
		internal static object PipelineResult(List<object> resultList)
		{
			int count = resultList.Count;
			if (count == 0)
			{
				return AutomationNull.Value;
			}
			object result = (count == 1) ? resultList[0] : resultList.ToArray();
			resultList.Clear();
			return result;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0016F2D0 File Offset: 0x0016D4D0
		internal static void FlushPipe(Pipe oldPipe, List<object> resultList)
		{
			for (int i = 0; i < resultList.Count; i++)
			{
				oldPipe.Add(resultList[i]);
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0016F2FB File Offset: 0x0016D4FB
		internal static void ClearPipe(List<object> resultList)
		{
			resultList.Clear();
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0016F304 File Offset: 0x0016D504
		internal static ExitException GetExitException(object exitCodeObj)
		{
			int num = 0;
			try
			{
				if (!LanguagePrimitives.IsNull(exitCodeObj))
				{
					num = ParserOps.ConvertTo<int>(exitCodeObj, PositionUtilities.EmptyExtent);
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return new ExitException(num);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0016F350 File Offset: 0x0016D550
		internal static void CheckForInterrupts(ExecutionContext context)
		{
			if (context.Events != null)
			{
				context.Events.ProcessPendingActions();
			}
			if (context.CurrentPipelineStopping)
			{
				throw new PipelineStoppedException();
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0016F373 File Offset: 0x0016D573
		internal static void Nop()
		{
		}
	}
}
