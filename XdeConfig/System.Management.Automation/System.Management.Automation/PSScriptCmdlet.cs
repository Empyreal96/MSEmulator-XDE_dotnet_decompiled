using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x020005F5 RID: 1525
	internal sealed class PSScriptCmdlet : PSCmdlet, IDynamicParameters, IDisposable
	{
		// Token: 0x060041AD RID: 16813 RVA: 0x0015B5DC File Offset: 0x001597DC
		public PSScriptCmdlet(ScriptBlock scriptBlock, bool useNewScope, bool fromScriptFile, ExecutionContext context)
		{
			this._scriptBlock = scriptBlock;
			this._useLocalScope = useNewScope;
			this._fromScriptFile = fromScriptFile;
			this._runOptimized = this._scriptBlock.Compile(context._debuggingMode <= 0 && useNewScope);
			this._localsTuple = this._scriptBlock.MakeLocalsTuple(this._runOptimized);
			this._localsTuple.SetAutomaticVariable(AutomaticVariable.PSCmdlet, this, context);
			this._scriptBlock.SetPSScriptRootAndPSCommandPath(this._localsTuple, context);
			this._functionContext = new FunctionContext
			{
				_localsTuple = this._localsTuple,
				_scriptBlock = this._scriptBlock,
				_file = this._scriptBlock.File,
				_sequencePoints = this._scriptBlock.SequencePoints,
				_debuggerHidden = this._scriptBlock.DebuggerHidden,
				_debuggerStepThrough = this._scriptBlock.DebuggerStepThrough,
				_executionContext = context
			};
			this._rethrowExitException = context.ScriptCommandProcessorShouldRethrowExit;
			context.ScriptCommandProcessorShouldRethrowExit = false;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0015B6F0 File Offset: 0x001598F0
		protected override void BeginProcessing()
		{
			this._commandRuntime = (MshCommandRuntime)this.commandRuntime;
			this._functionContext._outputPipe = this._commandRuntime.OutputPipe;
			this.SetPreferenceVariables();
			if (this._scriptBlock.HasBeginBlock)
			{
				this.RunClause(this._runOptimized ? this._scriptBlock.BeginBlock : this._scriptBlock.UnoptimizedBeginBlock, AutomationNull.Value, this._input);
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0015B768 File Offset: 0x00159968
		internal override void DoProcessRecord()
		{
			if (this._exitWasCalled)
			{
				return;
			}
			this._input.Add(base.CurrentPipelineObject);
			if (this._scriptBlock.HasProcessBlock)
			{
				PSObject dollarUnderbar = (base.CurrentPipelineObject == AutomationNull.Value) ? null : base.CurrentPipelineObject;
				this.RunClause(this._runOptimized ? this._scriptBlock.ProcessBlock : this._scriptBlock.UnoptimizedProcessBlock, dollarUnderbar, this._input);
				this._input.Clear();
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0015B7EC File Offset: 0x001599EC
		internal override void DoEndProcessing()
		{
			if (this._exitWasCalled)
			{
				return;
			}
			if (this._scriptBlock.HasEndBlock)
			{
				this.RunClause(this._runOptimized ? this._scriptBlock.EndBlock : this._scriptBlock.UnoptimizedEndBlock, AutomationNull.Value, this._input.ToArray());
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0015B845 File Offset: 0x00159A45
		private void EnterScope()
		{
			this._commandRuntime.SetVariableListsInPipe();
			if (!this._useLocalScope)
			{
				base.Context.SessionState.Internal.CurrentScope.DottedScopes.Push(this._localsTuple);
			}
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0015B87F File Offset: 0x00159A7F
		private void ExitScope()
		{
			this._commandRuntime.RemoveVariableListsInPipe();
			if (!this._useLocalScope)
			{
				base.Context.SessionState.Internal.CurrentScope.DottedScopes.Pop();
			}
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0015B8B4 File Offset: 0x00159AB4
		private void RunClause(Action<FunctionContext> clause, object dollarUnderbar, object inputToProcess)
		{
			Pipe shellFunctionErrorOutputPipe = base.Context.ShellFunctionErrorOutputPipe;
			PSLanguageMode? pslanguageMode = null;
			PSLanguageMode? pslanguageMode2 = null;
			if (this._scriptBlock.LanguageMode != null && this._scriptBlock.LanguageMode != base.Context.LanguageMode)
			{
				pslanguageMode = new PSLanguageMode?(base.Context.LanguageMode);
				pslanguageMode2 = this._scriptBlock.LanguageMode;
			}
			try
			{
				try
				{
					this.EnterScope();
					if (this._commandRuntime.ErrorMergeTo == MshCommandRuntime.MergeDataStream.Output)
					{
						base.Context.RedirectErrorPipe(this._commandRuntime.OutputPipe);
					}
					else if (this._commandRuntime.ErrorOutputPipe.IsRedirected)
					{
						base.Context.RedirectErrorPipe(this._commandRuntime.ErrorOutputPipe);
					}
					if (dollarUnderbar != AutomationNull.Value)
					{
						this._localsTuple.SetAutomaticVariable(AutomaticVariable.Underbar, dollarUnderbar, base.Context);
					}
					if (inputToProcess != AutomationNull.Value)
					{
						this._localsTuple.SetAutomaticVariable(AutomaticVariable.Input, inputToProcess, base.Context);
					}
					if (pslanguageMode2 != null)
					{
						base.Context.LanguageMode = pslanguageMode2.Value;
					}
					clause(this._functionContext);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
				finally
				{
					base.Context.RestoreErrorPipe(shellFunctionErrorOutputPipe);
					if (pslanguageMode != null)
					{
						base.Context.LanguageMode = pslanguageMode.Value;
					}
					this.ExitScope();
				}
			}
			catch (ExitException ex2)
			{
				if (!this._fromScriptFile || this._rethrowExitException)
				{
					throw;
				}
				this._exitWasCalled = true;
				int num = (int)ex2.Argument;
				base.Context.SetVariable(SpecialVariables.LastExitCodeVarPath, num);
				if (num != 0)
				{
					this._commandRuntime.PipelineProcessor.ExecutionFailed = true;
				}
			}
			catch (TerminateException)
			{
				throw;
			}
			catch (RuntimeException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw;
			}
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0015BAE8 File Offset: 0x00159CE8
		public object GetDynamicParameters()
		{
			this._commandRuntime = (MshCommandRuntime)this.commandRuntime;
			if (!this._scriptBlock.HasDynamicParameters)
			{
				return null;
			}
			List<object> list = new List<object>();
			this._functionContext._outputPipe = new Pipe(list);
			this.RunClause(this._runOptimized ? this._scriptBlock.DynamicParamBlock : this._scriptBlock.UnoptimizedDynamicParamBlock, AutomationNull.Value, AutomationNull.Value);
			if (list.Count > 1)
			{
				throw PSTraceSource.NewInvalidOperationException(AutomationExceptions.DynamicParametersWrongType, new object[]
				{
					PSObject.ToStringParser(base.Context, list)
				});
			}
			if (list.Count != 0)
			{
				return PSObject.Base(list[0]);
			}
			return null;
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0015BBA0 File Offset: 0x00159DA0
		public void PrepareForBinding(SessionStateScope scope, CommandLineParameters commandLineParameters)
		{
			if (this._useLocalScope && scope.LocalsTuple == null)
			{
				scope.LocalsTuple = this._localsTuple;
			}
			this._localsTuple.SetAutomaticVariable(AutomaticVariable.PSBoundParameters, commandLineParameters.GetValueToBindToPSBoundParameters(), base.Context);
			this._localsTuple.SetAutomaticVariable(AutomaticVariable.MyInvocation, base.MyInvocation, base.Context);
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0015BBFC File Offset: 0x00159DFC
		private void SetPreferenceVariables()
		{
			if (this._commandRuntime.IsDebugFlagSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Debug, this._commandRuntime.Debug ? ActionPreference.Inquire : ActionPreference.SilentlyContinue);
			}
			if (this._commandRuntime.IsVerboseFlagSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Verbose, this._commandRuntime.Verbose ? ActionPreference.Continue : ActionPreference.SilentlyContinue);
			}
			if (this._commandRuntime.IsErrorActionSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Error, this._commandRuntime.ErrorAction);
			}
			if (this._commandRuntime.IsWarningActionSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Warning, this._commandRuntime.WarningPreference);
			}
			if (this._commandRuntime.IsInformationActionSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Information, this._commandRuntime.InformationPreference);
			}
			if (this._commandRuntime.IsWhatIfFlagSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.WhatIf, this._commandRuntime.WhatIf);
			}
			if (this._commandRuntime.IsConfirmFlagSet)
			{
				this._localsTuple.SetPreferenceVariable(PreferenceVariable.Confirm, this._commandRuntime.Confirm ? ConfirmImpact.Low : ConfirmImpact.None);
			}
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060041B7 RID: 16823 RVA: 0x0015BD48 File Offset: 0x00159F48
		// (remove) Token: 0x060041B8 RID: 16824 RVA: 0x0015BD80 File Offset: 0x00159F80
		internal event EventHandler StoppingEvent;

		// Token: 0x060041B9 RID: 16825 RVA: 0x0015BDB5 File Offset: 0x00159FB5
		protected override void StopProcessing()
		{
			this.StoppingEvent.SafeInvoke(this, EventArgs.Empty);
			base.StopProcessing();
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060041BA RID: 16826 RVA: 0x0015BDD0 File Offset: 0x00159FD0
		// (remove) Token: 0x060041BB RID: 16827 RVA: 0x0015BE08 File Offset: 0x0015A008
		internal event EventHandler DisposingEvent;

		// Token: 0x060041BC RID: 16828 RVA: 0x0015BE40 File Offset: 0x0015A040
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.DisposingEvent.SafeInvoke(this, EventArgs.Empty);
			this.commandRuntime = null;
			this.currentObjectInPipeline = null;
			this._input.Clear();
			base.InternalDispose(true);
			this._disposed = true;
		}

		// Token: 0x040020EB RID: 8427
		private readonly ArrayList _input = new ArrayList();

		// Token: 0x040020EC RID: 8428
		private readonly ScriptBlock _scriptBlock;

		// Token: 0x040020ED RID: 8429
		private readonly bool _fromScriptFile;

		// Token: 0x040020EE RID: 8430
		private readonly bool _useLocalScope;

		// Token: 0x040020EF RID: 8431
		private readonly bool _runOptimized;

		// Token: 0x040020F0 RID: 8432
		private bool _rethrowExitException;

		// Token: 0x040020F1 RID: 8433
		private MshCommandRuntime _commandRuntime;

		// Token: 0x040020F2 RID: 8434
		private readonly MutableTuple _localsTuple;

		// Token: 0x040020F3 RID: 8435
		private bool _exitWasCalled;

		// Token: 0x040020F4 RID: 8436
		private readonly FunctionContext _functionContext;

		// Token: 0x040020F6 RID: 8438
		private bool _disposed;
	}
}
