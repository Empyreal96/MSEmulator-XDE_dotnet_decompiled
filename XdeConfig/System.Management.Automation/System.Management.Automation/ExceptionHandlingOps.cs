using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000631 RID: 1585
	internal static class ExceptionHandlingOps
	{
		// Token: 0x060044BC RID: 17596 RVA: 0x0016FF78 File Offset: 0x0016E178
		internal static int FindMatchingHandler(MutableTuple tuple, RuntimeException rte, Type[] types, ExecutionContext context)
		{
			Exception ex = rte;
			Exception innerException = rte.InnerException;
			int num = -1;
			if (innerException != null)
			{
				num = ExceptionHandlingOps.FindMatchingHandlerByType(innerException.GetType(), types);
				ex = innerException;
			}
			if (num == -1 || types[num].Equals(typeof(ExceptionHandlingOps.CatchAll)))
			{
				num = ExceptionHandlingOps.FindMatchingHandlerByType(rte.GetType(), types);
				ex = rte;
			}
			if (num == -1 || types[num].Equals(typeof(ExceptionHandlingOps.CatchAll)))
			{
				ActionPreferenceStopException ex2 = rte as ActionPreferenceStopException;
				if (ex2 != null)
				{
					ex = ex2.ErrorRecord.Exception;
					if (ex is RuntimeException)
					{
						return ExceptionHandlingOps.FindMatchingHandler(tuple, (RuntimeException)ex, types, context);
					}
					if (ex != null)
					{
						num = ExceptionHandlingOps.FindMatchingHandlerByType(ex.GetType(), types);
					}
				}
				else if (rte is CmdletInvocationException && innerException != null)
				{
					ex = innerException.InnerException;
					if (ex != null)
					{
						num = ExceptionHandlingOps.FindMatchingHandlerByType(ex.GetType(), types);
					}
				}
			}
			if (num != -1)
			{
				ErrorRecord value = new ErrorRecord(rte.ErrorRecord, ex);
				tuple.SetAutomaticVariable(AutomaticVariable.Underbar, value, context);
			}
			return num;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x00170060 File Offset: 0x0016E260
		private static int FindMatchingHandlerByType(Type exceptionType, Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (exceptionType.Equals(types[i]))
				{
					return i;
				}
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (exceptionType.IsSubclassOf(types[i]))
				{
					return i;
				}
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].Equals(typeof(ExceptionHandlingOps.CatchAll)))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x001700C8 File Offset: 0x0016E2C8
		internal static bool SuspendStoppingPipeline(ExecutionContext context)
		{
			LocalPipeline localPipeline = (LocalPipeline)context.CurrentRunspace.GetCurrentlyRunningPipeline();
			bool isStopping = localPipeline.Stopper.IsStopping;
			localPipeline.Stopper.IsStopping = false;
			return isStopping;
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x00170100 File Offset: 0x0016E300
		internal static void RestoreStoppingPipeline(ExecutionContext context, bool oldIsStopping)
		{
			LocalPipeline localPipeline = (LocalPipeline)context.CurrentRunspace.GetCurrentlyRunningPipeline();
			localPipeline.Stopper.IsStopping = oldIsStopping;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0017012C File Offset: 0x0016E32C
		internal static void CheckActionPreference(FunctionContext funcContext, Exception exception)
		{
			if (exception is TargetInvocationException)
			{
				exception = exception.InnerException;
			}
			CommandProcessorBase.CheckForSevereException(exception);
			RuntimeException ex = exception as RuntimeException;
			if (ex == null)
			{
				ex = ExceptionHandlingOps.ConvertToRuntimeException(exception, funcContext.CurrentPosition);
			}
			else
			{
				InterpreterError.UpdateExceptionErrorRecordPosition(ex, funcContext.CurrentPosition);
			}
			RuntimeException.LockStackTrace(ex);
			ExecutionContext executionContext = funcContext._executionContext;
			Pipe outputPipe = funcContext._outputPipe;
			IScriptExtent scriptPosition = ex.ErrorRecord.InvocationInfo.ScriptPosition;
			ExceptionHandlingOps.SetErrorVariables(scriptPosition, ex, executionContext, outputPipe);
			executionContext.QuestionMarkVariableValue = false;
			bool flag = funcContext._traps.Any<Tuple<Type[], Action<FunctionContext>[], Type[]>>() && funcContext._traps.Last<Tuple<Type[], Action<FunctionContext>[], Type[]>>().Item2 != null;
			if (!flag && !ExceptionHandlingOps.NeedToQueryForActionPreference(ex, executionContext))
			{
				throw ex;
			}
			ActionPreference actionPreference;
			if (flag)
			{
				actionPreference = ExceptionHandlingOps.ProcessTraps(funcContext, ex);
			}
			else
			{
				actionPreference = ExceptionHandlingOps.QueryForAction(ex, ex.Message, executionContext);
			}
			executionContext.QuestionMarkVariableValue = false;
			if (actionPreference == ActionPreference.SilentlyContinue || actionPreference == ActionPreference.Ignore)
			{
				return;
			}
			if (actionPreference == ActionPreference.Stop)
			{
				ex.SuppressPromptInInterpreter = true;
				throw ex;
			}
			if (!flag && ex.WasThrownFromThrowStatement)
			{
				throw ex;
			}
			bool flag2 = ExceptionHandlingOps.ReportErrorRecord(scriptPosition, ex, executionContext);
			executionContext.QuestionMarkVariableValue = false;
			if (!flag2)
			{
				throw ex;
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x00170244 File Offset: 0x0016E444
		private static ActionPreference ProcessTraps(FunctionContext funcContext, RuntimeException rte)
		{
			int num = -1;
			Exception ex = null;
			Exception innerException = rte.InnerException;
			Type[] item = funcContext._traps.Last<Tuple<Type[], Action<FunctionContext>[], Type[]>>().Item1;
			Action<FunctionContext>[] item2 = funcContext._traps.Last<Tuple<Type[], Action<FunctionContext>[], Type[]>>().Item2;
			if (innerException != null)
			{
				num = ExceptionHandlingOps.FindMatchingHandlerByType(innerException.GetType(), item);
				ex = innerException;
			}
			if (num == -1 || item[num].Equals(typeof(ExceptionHandlingOps.CatchAll)))
			{
				int num2 = ExceptionHandlingOps.FindMatchingHandlerByType(rte.GetType(), item);
				if (num2 != num)
				{
					num = num2;
					ex = rte;
				}
			}
			if (num != -1)
			{
				try
				{
					ErrorRecord errorRecord = rte.ErrorRecord;
					ExecutionContext executionContext = funcContext._executionContext;
					if (executionContext.CurrentCommandProcessor != null)
					{
						executionContext.CurrentCommandProcessor.ForgetScriptException();
					}
					try
					{
						MutableTuple mutableTuple = MutableTuple.MakeTuple(funcContext._traps.Last<Tuple<Type[], Action<FunctionContext>[], Type[]>>().Item3[num], Compiler.DottedLocalsNameIndexMap);
						mutableTuple.SetAutomaticVariable(AutomaticVariable.Underbar, new ErrorRecord(errorRecord, ex), executionContext);
						for (int i = 1; i < 9; i++)
						{
							mutableTuple.SetValue(i, funcContext._localsTuple.GetValue(i));
						}
						SessionStateScope sessionStateScope = executionContext.EngineSessionState.NewScope(false);
						executionContext.EngineSessionState.CurrentScope = sessionStateScope;
						sessionStateScope.LocalsTuple = mutableTuple;
						FunctionContext obj = new FunctionContext
						{
							_file = funcContext._file,
							_scriptBlock = funcContext._scriptBlock,
							_sequencePoints = funcContext._sequencePoints,
							_debuggerHidden = funcContext._debuggerHidden,
							_debuggerStepThrough = funcContext._debuggerStepThrough,
							_executionContext = funcContext._executionContext,
							_boundBreakpoints = funcContext._boundBreakpoints,
							_outputPipe = funcContext._outputPipe,
							_breakPoints = funcContext._breakPoints,
							_localsTuple = mutableTuple
						};
						item2[num](obj);
					}
					catch (TargetInvocationException ex2)
					{
						throw ex2.InnerException;
					}
					finally
					{
						executionContext.EngineSessionState.RemoveScope(executionContext.EngineSessionState.CurrentScope);
					}
					return ExceptionHandlingOps.QueryForAction(rte, ex.Message, executionContext);
				}
				catch (ContinueException)
				{
					return ActionPreference.SilentlyContinue;
				}
				catch (BreakException)
				{
					return ActionPreference.Stop;
				}
				return ActionPreference.Stop;
			}
			return ActionPreference.Stop;
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x001704AC File Offset: 0x0016E6AC
		internal static ActionPreference QueryForAction(RuntimeException rte, string message, ExecutionContext context)
		{
			bool flag;
			ActionPreference enumPreference = context.GetEnumPreference<ActionPreference>(SpecialVariables.ErrorActionPreferenceVarPath, ActionPreference.Continue, out flag);
			if (enumPreference != ActionPreference.Inquire || rte.SuppressPromptInInterpreter)
			{
				return enumPreference;
			}
			return ExceptionHandlingOps.InquireForActionPreference(message, context);
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x001704E0 File Offset: 0x0016E6E0
		internal static ActionPreference InquireForActionPreference(string message, ExecutionContext context)
		{
			InternalHostUserInterface internalHostUserInterface = (InternalHostUserInterface)context.EngineHostInterface.UI;
			Collection<ChoiceDescription> collection = new Collection<ChoiceDescription>();
			string continueLabel = ParserStrings.ContinueLabel;
			string continueHelpMessage = ParserStrings.ContinueHelpMessage;
			string silentlyContinueLabel = ParserStrings.SilentlyContinueLabel;
			string silentlyContinueHelpMessage = ParserStrings.SilentlyContinueHelpMessage;
			string breakLabel = ParserStrings.BreakLabel;
			string breakHelpMessage = ParserStrings.BreakHelpMessage;
			string suspendLabel = ParserStrings.SuspendLabel;
			string helpMessage = StringUtil.Format(ParserStrings.SuspendHelpMessage, new object[0]);
			collection.Add(new ChoiceDescription(continueLabel, continueHelpMessage));
			collection.Add(new ChoiceDescription(silentlyContinueLabel, silentlyContinueHelpMessage));
			collection.Add(new ChoiceDescription(breakLabel, breakHelpMessage));
			collection.Add(new ChoiceDescription(suspendLabel, helpMessage));
			string exceptionActionPromptCaption = ParserStrings.ExceptionActionPromptCaption;
			int num;
			while ((num = internalHostUserInterface.PromptForChoice(exceptionActionPromptCaption, message, collection, 0)) == 3)
			{
				context.EngineHostInterface.EnterNestedPrompt();
			}
			if (num == 0)
			{
				return ActionPreference.Continue;
			}
			if (num == 1)
			{
				return ActionPreference.SilentlyContinue;
			}
			return ActionPreference.Stop;
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x001705B4 File Offset: 0x0016E7B4
		internal static void SetErrorVariables(IScriptExtent extent, RuntimeException rte, ExecutionContext context, Pipe outputPipe)
		{
			string newValue = null;
			Exception ex = rte;
			int num = 0;
			while (ex != null && num++ < 10)
			{
				if (!string.IsNullOrEmpty(ex.StackTrace))
				{
					newValue = ex.StackTrace;
				}
				ex = ex.InnerException;
			}
			context.SetVariable(SpecialVariables.StackTraceVarPath, newValue);
			InterpreterError.UpdateExceptionErrorRecordPosition(rte, extent);
			ErrorRecord obj = rte.ErrorRecord.WrapException(rte);
			if (!(rte is PipelineStoppedException))
			{
				if (outputPipe != null)
				{
					outputPipe.AppendVariableList(VariableStreamKind.Error, obj);
				}
				context.AppendDollarError(obj);
			}
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0017062A File Offset: 0x0016E82A
		internal static bool NeedToQueryForActionPreference(RuntimeException rte, ExecutionContext context)
		{
			return !context.PropagateExceptionsToEnclosingStatementBlock && context.ShellFunctionErrorOutputPipe != null && !context.CurrentPipelineStopping && !rte.SuppressPromptInInterpreter && !(rte is PipelineStoppedException);
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0017065C File Offset: 0x0016E85C
		internal static bool ReportErrorRecord(IScriptExtent extent, RuntimeException rte, ExecutionContext context)
		{
			if (context.ShellFunctionErrorOutputPipe == null)
			{
				return false;
			}
			if (rte.ErrorRecord.InvocationInfo == null && extent != null && extent != PositionUtilities.EmptyExtent)
			{
				rte.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, extent, context));
			}
			PSObject psobject = PSObject.AsPSObject(new ErrorRecord(rte.ErrorRecord, rte));
			PSNoteProperty member = new PSNoteProperty("writeErrorStream", true);
			psobject.Properties.Add(member);
			if (context.InternalHost.UI.IsTranscribing)
			{
				context.InternalHost.UI.TranscribeError(context, rte.ErrorRecord.InvocationInfo, psobject);
			}
			context.ShellFunctionErrorOutputPipe.Add(psobject);
			return true;
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0017070C File Offset: 0x0016E90C
		internal static RuntimeException ConvertToException(object result, IScriptExtent extent)
		{
			result = PSObject.Base(result);
			RuntimeException ex = result as RuntimeException;
			if (ex != null)
			{
				InterpreterError.UpdateExceptionErrorRecordPosition(ex, extent);
				ex.WasThrownFromThrowStatement = true;
				return ex;
			}
			ErrorRecord errorRecord = result as ErrorRecord;
			if (errorRecord != null)
			{
				ex = new RuntimeException(errorRecord.ToString(), errorRecord.Exception, errorRecord)
				{
					WasThrownFromThrowStatement = true
				};
				InterpreterError.UpdateExceptionErrorRecordPosition(ex, extent);
				return ex;
			}
			Exception ex2 = result as Exception;
			if (ex2 != null)
			{
				errorRecord = new ErrorRecord(ex2, ex2.Message, ErrorCategory.OperationStopped, null);
				ex = new RuntimeException(ex2.Message, ex2, errorRecord)
				{
					WasThrownFromThrowStatement = true
				};
				InterpreterError.UpdateExceptionErrorRecordPosition(ex, extent);
				return ex;
			}
			string text = LanguagePrimitives.IsNull(result) ? "ScriptHalted" : ParserOps.ConvertTo<string>(result, PositionUtilities.EmptyExtent);
			ex2 = new RuntimeException(text, null);
			errorRecord = new ErrorRecord(ex2, text, ErrorCategory.OperationStopped, null);
			ex = new RuntimeException(text, ex2, errorRecord)
			{
				WasThrownFromThrowStatement = true
			};
			ex.SetTargetObject(result);
			InterpreterError.UpdateExceptionErrorRecordPosition(ex, extent);
			return ex;
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00170800 File Offset: 0x0016EA00
		internal static RuntimeException ConvertToRuntimeException(Exception exception, IScriptExtent extent)
		{
			RuntimeException ex = exception as RuntimeException;
			if (ex == null)
			{
				IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
				ErrorRecord errorRecord = (containsErrorRecord != null) ? containsErrorRecord.ErrorRecord : new ErrorRecord(exception, exception.GetType().FullName, ErrorCategory.OperationStopped, null);
				ex = new RuntimeException(exception.Message, exception, errorRecord);
			}
			InterpreterError.UpdateExceptionErrorRecordPosition(ex, extent);
			return ex;
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x00170854 File Offset: 0x0016EA54
		internal static void ConvertToArgumentConversionException(Exception exception, string parameterName, object argument, string method, Type toType)
		{
			throw new MethodException("MethodArgumentConversionInvalidCastArgument", exception, ExtendedTypeSystem.MethodArgumentConversionException, new object[]
			{
				parameterName,
				argument,
				method,
				toType,
				exception.Message
			});
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x00170894 File Offset: 0x0016EA94
		internal static void ConvertToMethodInvocationException(Exception exception, Type typeToThrow, string methodName, int numArgs, MemberInfo memberInfo = null)
		{
			if (exception is TargetInvocationException)
			{
				exception = exception.InnerException;
			}
			CommandProcessorBase.CheckForSevereException(exception);
			if ((exception is FlowControlException || exception is ScriptCallDepthException || exception is PipelineStoppedException) && (memberInfo == null || (memberInfo.DeclaringType != typeof(PowerShell) && memberInfo.DeclaringType != typeof(Pipeline))))
			{
				return;
			}
			if (typeToThrow == typeof(MethodException))
			{
				if (exception is MethodException)
				{
					return;
				}
				throw new MethodInvocationException(exception.GetType().Name, exception, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodName,
					numArgs,
					exception.Message
				});
			}
			else
			{
				if (methodName.StartsWith("set_", StringComparison.Ordinal) || methodName.StartsWith("get_", StringComparison.Ordinal))
				{
					methodName = methodName.Substring(4);
				}
				if (typeToThrow == typeof(GetValueInvocationException))
				{
					if (exception is GetValueException)
					{
						return;
					}
					throw new GetValueInvocationException("ExceptionWhenGetting", exception, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
					{
						methodName,
						exception.Message
					});
				}
				else
				{
					if (exception is SetValueException)
					{
						return;
					}
					throw new SetValueInvocationException("ExceptionWhenSetting", exception, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
					{
						methodName,
						exception.Message
					});
				}
			}
		}

		// Token: 0x02000632 RID: 1586
		internal class CatchAll
		{
		}
	}
}
