using System;
using System.Collections;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x0200003E RID: 62
	internal sealed class DlrScriptCommandProcessor : ScriptCommandProcessorBase
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000B1AC File Offset: 0x000093AC
		internal DlrScriptCommandProcessor(ScriptBlock scriptBlock, ExecutionContext context, bool useNewScope, CommandOrigin origin, SessionStateInternal sessionState) : base(scriptBlock, context, useNewScope, origin, sessionState)
		{
			this.Init();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000B1CC File Offset: 0x000093CC
		internal DlrScriptCommandProcessor(FunctionInfo functionInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState) : base(functionInfo, context, useNewScope, sessionState)
		{
			this.Init();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000B1EA File Offset: 0x000093EA
		internal DlrScriptCommandProcessor(ScriptInfo scriptInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState) : base(scriptInfo, context, useNewScope, sessionState)
		{
			this.Init();
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000B208 File Offset: 0x00009408
		internal DlrScriptCommandProcessor(ExternalScriptInfo scriptInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState) : base(scriptInfo, context, useNewScope, sessionState)
		{
			this.Init();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000B228 File Offset: 0x00009428
		private void Init()
		{
			this._scriptBlock = this._scriptBlock;
			this._obsoleteAttribute = this._scriptBlock.ObsoleteAttribute;
			this._runOptimizedCode = this._scriptBlock.Compile(this._context._debuggingMode <= 0 && base.UseLocalScope);
			this._localsTuple = this._scriptBlock.MakeLocalsTuple(this._runOptimizedCode);
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000B291 File Offset: 0x00009491
		internal override ObsoleteAttribute ObsoleteAttribute
		{
			get
			{
				return this._obsoleteAttribute;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000B29C File Offset: 0x0000949C
		internal override void Prepare(IDictionary psDefaultParameterValues)
		{
			if (base.UseLocalScope)
			{
				base.CommandScope.LocalsTuple = this._localsTuple;
			}
			this._localsTuple.SetAutomaticVariable(AutomaticVariable.MyInvocation, base.Command.MyInvocation, this._context);
			this._scriptBlock.SetPSScriptRootAndPSCommandPath(this._localsTuple, this._context);
			this._functionContext = new FunctionContext
			{
				_executionContext = this._context,
				_outputPipe = this.commandRuntime.OutputPipe,
				_localsTuple = this._localsTuple,
				_scriptBlock = this._scriptBlock,
				_file = this._scriptBlock.File,
				_debuggerHidden = this._scriptBlock.DebuggerHidden,
				_debuggerStepThrough = this._scriptBlock.DebuggerStepThrough,
				_sequencePoints = this._scriptBlock.SequencePoints
			};
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000B37C File Offset: 0x0000957C
		internal override void DoBegin()
		{
			if (!this.RanBeginAlready)
			{
				this.RanBeginAlready = true;
				ScriptBlock.LogScriptBlockStart(this._scriptBlock, base.Context.CurrentRunspace.InstanceId);
				base.SetCurrentScopeToExecutionScope();
				CommandProcessorBase currentCommandProcessor = base.Context.CurrentCommandProcessor;
				try
				{
					base.Context.CurrentCommandProcessor = this;
					if (this._scriptBlock.HasBeginBlock)
					{
						this.RunClause(this._runOptimizedCode ? this._scriptBlock.BeginBlock : this._scriptBlock.UnoptimizedBeginBlock, AutomationNull.Value, this._input);
					}
				}
				finally
				{
					base.Context.CurrentCommandProcessor = currentCommandProcessor;
					base.RestorePreviousScope();
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B438 File Offset: 0x00009638
		internal override void ProcessRecord()
		{
			if (this._exitWasCalled)
			{
				return;
			}
			if (!this.RanBeginAlready)
			{
				this.RanBeginAlready = true;
				if (this._scriptBlock.HasBeginBlock)
				{
					this.RunClause(this._runOptimizedCode ? this._scriptBlock.BeginBlock : this._scriptBlock.UnoptimizedBeginBlock, AutomationNull.Value, this._input);
				}
			}
			if (!this._scriptBlock.HasProcessBlock)
			{
				if (base.IsPipelineInputExpected() && base.CommandRuntime.InputPipe.ExternalReader == null)
				{
					while (this.Read())
					{
						this._input.Add(base.Command.CurrentPipelineObject);
					}
				}
				return;
			}
			if (!base.IsPipelineInputExpected())
			{
				this.RunClause(this._runOptimizedCode ? this._scriptBlock.ProcessBlock : this._scriptBlock.UnoptimizedProcessBlock, null, this._input);
				return;
			}
			this.DoProcessRecordWithInput();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B520 File Offset: 0x00009720
		internal override void Complete()
		{
			try
			{
				if (!this._exitWasCalled)
				{
					if (this._scriptBlock.HasProcessBlock && base.IsPipelineInputExpected())
					{
						this.DoProcessRecordWithInput();
					}
					if (this._scriptBlock.HasEndBlock)
					{
						Action<FunctionContext> clause = this._runOptimizedCode ? this._scriptBlock.EndBlock : this._scriptBlock.UnoptimizedEndBlock;
						if (base.CommandRuntime.InputPipe.ExternalReader == null)
						{
							if (base.IsPipelineInputExpected())
							{
								while (this.Read())
								{
									this._input.Add(base.Command.CurrentPipelineObject);
								}
							}
							this.RunClause(clause, AutomationNull.Value, this._input);
						}
						else
						{
							this.RunClause(clause, AutomationNull.Value, base.CommandRuntime.InputPipe.ExternalReader.GetReadEnumerator());
						}
					}
				}
			}
			finally
			{
				ScriptBlock.LogScriptBlockEnd(this._scriptBlock, base.Context.CurrentRunspace.InstanceId);
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000B624 File Offset: 0x00009824
		private void DoProcessRecordWithInput()
		{
			Action<FunctionContext> clause = this._runOptimizedCode ? this._scriptBlock.ProcessBlock : this._scriptBlock.UnoptimizedProcessBlock;
			while (this.Read())
			{
				this._input.Add(base.Command.CurrentPipelineObject);
				base.Command.MyInvocation.PipelineIterationInfo[base.Command.MyInvocation.PipelinePosition]++;
				this.RunClause(clause, base.Command.CurrentPipelineObject, this._input);
				this._input.Clear();
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000B6C8 File Offset: 0x000098C8
		private void RunClause(Action<FunctionContext> clause, object dollarUnderbar, object inputToProcess)
		{
			ExecutionContext.CheckStackDepth();
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
				CommandOrigin scopeOrigin = base.Context.EngineSessionState.CurrentScope.ScopeOrigin;
				try
				{
					base.Context.EngineSessionState.CurrentScope.ScopeOrigin = (this._dontUseScopeCommandOrigin ? CommandOrigin.Internal : base.Command.CommandOrigin);
					if (pslanguageMode2 != null)
					{
						base.Context.LanguageMode = pslanguageMode2.Value;
					}
					this.EnterScope();
					if (this.commandRuntime.ErrorMergeTo == MshCommandRuntime.MergeDataStream.Output)
					{
						base.Context.RedirectErrorPipe(this.commandRuntime.OutputPipe);
					}
					else if (this.commandRuntime.ErrorOutputPipe.IsRedirected)
					{
						base.Context.RedirectErrorPipe(this.commandRuntime.ErrorOutputPipe);
					}
					if (dollarUnderbar != AutomationNull.Value)
					{
						this._localsTuple.SetAutomaticVariable(AutomaticVariable.Underbar, dollarUnderbar, this._context);
					}
					if (inputToProcess != AutomationNull.Value)
					{
						if (inputToProcess == null)
						{
							inputToProcess = MshCommandRuntime.StaticEmptyArray.GetEnumerator();
						}
						else
						{
							IList list = inputToProcess as IList;
							inputToProcess = ((list != null) ? list.GetEnumerator() : LanguagePrimitives.GetEnumerator(inputToProcess));
						}
						this._localsTuple.SetAutomaticVariable(AutomaticVariable.Input, inputToProcess, this._context);
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
					base.Context.EngineSessionState.CurrentScope.ScopeOrigin = scopeOrigin;
				}
			}
			catch (ExitException ex2)
			{
				if (!base.FromScriptFile || this._rethrowExitException)
				{
					throw;
				}
				this._exitWasCalled = true;
				int num = (int)ex2.Argument;
				base.Command.Context.SetVariable(SpecialVariables.LastExitCodeVarPath, num);
				if (num != 0)
				{
					this.commandRuntime.PipelineProcessor.ExecutionFailed = true;
				}
			}
			catch (FlowControlException)
			{
				throw;
			}
			catch (RuntimeException e)
			{
				base.ManageScriptException(e);
				throw;
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
				throw base.ManageInvocationException(e2);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000B9E0 File Offset: 0x00009BE0
		private void EnterScope()
		{
			if (!this._argsBound)
			{
				this._argsBound = true;
				using (this.commandRuntime.AllowThisCommandToWrite(false))
				{
					base.ScriptParameterBinderController.BindCommandLineParameters(this.arguments);
				}
				this._localsTuple.SetAutomaticVariable(AutomaticVariable.PSBoundParameters, base.ScriptParameterBinderController.CommandLineParameters.GetValueToBindToPSBoundParameters(), this._context);
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000BA58 File Offset: 0x00009C58
		protected override void OnSetCurrentScope()
		{
			if (!base.UseLocalScope)
			{
				base.CommandSessionState.CurrentScope.DottedScopes.Push(this._localsTuple);
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000BA7D File Offset: 0x00009C7D
		protected override void OnRestorePreviousScope()
		{
			if (!base.UseLocalScope)
			{
				base.CommandSessionState.CurrentScope.DottedScopes.Pop();
			}
		}

		// Token: 0x040000FF RID: 255
		private new ScriptBlock _scriptBlock;

		// Token: 0x04000100 RID: 256
		private readonly ArrayList _input = new ArrayList();

		// Token: 0x04000101 RID: 257
		private MutableTuple _localsTuple;

		// Token: 0x04000102 RID: 258
		private bool _runOptimizedCode;

		// Token: 0x04000103 RID: 259
		private bool _argsBound;

		// Token: 0x04000104 RID: 260
		private FunctionContext _functionContext;

		// Token: 0x04000105 RID: 261
		private ObsoleteAttribute _obsoleteAttribute;
	}
}
