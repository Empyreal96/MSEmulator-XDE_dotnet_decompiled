using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Sqm;

namespace System.Management.Automation
{
	// Token: 0x02000014 RID: 20
	internal class CommandProcessor : CommandProcessorBase
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00004F82 File Offset: 0x00003182
		internal CommandProcessor(CmdletInfo cmdletInfo, ExecutionContext context) : base(cmdletInfo)
		{
			this._context = context;
			this.Init(cmdletInfo);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004FA0 File Offset: 0x000031A0
		internal CommandProcessor(IScriptCommandInfo scriptCommandInfo, ExecutionContext context, bool useLocalScope, bool fromScriptFile, SessionStateInternal sessionState) : base(scriptCommandInfo as CommandInfo)
		{
			this._context = context;
			this._useLocalScope = useLocalScope;
			this._fromScriptFile = fromScriptFile;
			base.CommandSessionState = sessionState;
			this.Init(scriptCommandInfo);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004FDC File Offset: 0x000031DC
		internal ParameterBinderController NewParameterBinderController(InternalCommand command)
		{
			Cmdlet cmdlet = command as Cmdlet;
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentException("command");
			}
			IScriptCommandInfo scriptCommandInfo = base.CommandInfo as IScriptCommandInfo;
			ParameterBinderBase parameterBinder;
			if (scriptCommandInfo != null)
			{
				parameterBinder = new ScriptParameterBinder(scriptCommandInfo.ScriptBlock, cmdlet.MyInvocation, this._context, cmdlet, base.CommandScope);
			}
			else
			{
				parameterBinder = new ReflectionParameterBinder(cmdlet, cmdlet);
			}
			this._cmdletParameterBinderController = new CmdletParameterBinderController(cmdlet, base.CommandInfo.CommandMetadata, parameterBinder);
			return this._cmdletParameterBinderController;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005054 File Offset: 0x00003254
		internal CmdletParameterBinderController CmdletParameterBinderController
		{
			get
			{
				if (this._cmdletParameterBinderController == null)
				{
					this.NewParameterBinderController(base.Command);
				}
				return this._cmdletParameterBinderController;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005071 File Offset: 0x00003271
		internal override ObsoleteAttribute ObsoleteAttribute
		{
			get
			{
				return this._obsoleteAttribute;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000507C File Offset: 0x0000327C
		internal void BindCommandLineParameters()
		{
			using (this.commandRuntime.AllowThisCommandToWrite(false))
			{
				this.CmdletParameterBinderController.CommandLineParameters.UpdateInvocationInfo(base.Command.MyInvocation);
				base.Command.MyInvocation.UnboundArguments = new List<object>();
				this.CmdletParameterBinderController.BindCommandLineParameters(this.arguments);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000050F4 File Offset: 0x000032F4
		internal override void Prepare(IDictionary psDefaultParameterValues)
		{
			this.CmdletParameterBinderController.DefaultParameterValues = psDefaultParameterValues;
			CmdletInfo cmdletInfo = base.CommandInfo as CmdletInfo;
			if (cmdletInfo != null)
			{
				PSSQMAPI.IncrementData(cmdletInfo);
			}
			this.BindCommandLineParameters();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005128 File Offset: 0x00003328
		internal override void DoBegin()
		{
			if (!this.RanBeginAlready && this.CmdletParameterBinderController.ObsoleteParameterWarningList != null)
			{
				using (base.CommandRuntime.AllowThisCommandToWrite(false))
				{
					foreach (WarningRecord record in this.CmdletParameterBinderController.ObsoleteParameterWarningList)
					{
						base.CommandRuntime.WriteWarning(record, false);
					}
				}
				this.CmdletParameterBinderController.ObsoleteParameterWarningList.Clear();
			}
			base.DoBegin();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000051D8 File Offset: 0x000033D8
		internal override void ProcessRecord()
		{
			if (this.RanBeginAlready)
			{
				goto IL_1C6;
			}
			this.RanBeginAlready = true;
			try
			{
				using (this.commandRuntime.AllowThisCommandToWrite(true))
				{
					if (base.Context._debuggingMode > 0 && !(base.Command is PSScriptCmdlet))
					{
						base.Context.Debugger.CheckCommand(base.Command.MyInvocation);
					}
					base.Command.DoBeginProcessing();
				}
				goto IL_1C6;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw base.ManageInvocationException(e);
			}
			IL_84:
			Pipe shellFunctionErrorOutputPipe = this._context.ShellFunctionErrorOutputPipe;
			Exception ex = null;
			try
			{
				if (base.RedirectShellErrorOutputPipe || this._context.ShellFunctionErrorOutputPipe != null)
				{
					this._context.ShellFunctionErrorOutputPipe = this.commandRuntime.ErrorOutputPipe;
				}
				using (this.commandRuntime.AllowThisCommandToWrite(true))
				{
					if (this.CmdletParameterBinderController.ObsoleteParameterWarningList != null && this.CmdletParameterBinderController.ObsoleteParameterWarningList.Count > 0)
					{
						foreach (WarningRecord record in this.CmdletParameterBinderController.ObsoleteParameterWarningList)
						{
							base.CommandRuntime.WriteWarning(record, false);
						}
						this.CmdletParameterBinderController.ObsoleteParameterWarningList.Clear();
					}
					base.Command.MyInvocation.PipelineIterationInfo[base.Command.MyInvocation.PipelinePosition]++;
					base.Command.DoProcessRecord();
				}
			}
			catch (RuntimeException ex2)
			{
				if (ex2.WasThrownFromThrowStatement)
				{
					throw;
				}
				ex = ex2;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			finally
			{
				this._context.ShellFunctionErrorOutputPipe = shellFunctionErrorOutputPipe;
			}
			if (ex != null)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw base.ManageInvocationException(ex);
			}
			IL_1C6:
			if (!this.Read())
			{
				return;
			}
			goto IL_84;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000547C File Offset: 0x0000367C
		internal sealed override bool Read()
		{
			if (this._bailInNextCall)
			{
				return false;
			}
			base.Command.ThrowIfStopping();
			if (this._firstCallToRead)
			{
				this._firstCallToRead = false;
				if (!base.IsPipelineInputExpected())
				{
					this._bailInNextCall = true;
					return true;
				}
			}
			bool flag = false;
			while (!flag)
			{
				object obj = this.commandRuntime.InputPipe.Retrieve();
				if (obj == AutomationNull.Value)
				{
					base.Command.CurrentPipelineObject = null;
					return false;
				}
				if (base.Command.MyInvocation.PipelinePosition == 1)
				{
					base.Command.MyInvocation.PipelineIterationInfo[0]++;
				}
				try
				{
					if (!this.ProcessInputPipelineObject(obj))
					{
						this.WriteInputObjectError(obj, ParameterBinderStrings.InputObjectNotBound, "InputObjectNotBound", new object[0]);
						continue;
					}
				}
				catch (ParameterBindingException ex)
				{
					ex.ErrorRecord.SetTargetObject(obj);
					ErrorRecord errorRecord = new ErrorRecord(ex.ErrorRecord, ex);
					this.commandRuntime._WriteErrorSkipAllowCheck(errorRecord, null);
					continue;
				}
				Collection<MergedCompiledCommandParameter> missingMandatoryParameters;
				using (ParameterBinderBase.bindingTracer.TraceScope("MANDATORY PARAMETER CHECK on cmdlet [{0}]", new object[]
				{
					base.CommandInfo.Name
				}))
				{
					flag = this.CmdletParameterBinderController.HandleUnboundMandatoryParameters(out missingMandatoryParameters);
				}
				if (!flag)
				{
					string text = CmdletParameterBinderController.BuildMissingParamsString(missingMandatoryParameters);
					this.WriteInputObjectError(obj, ParameterBinderStrings.InputObjectMissingMandatory, "InputObjectMissingMandatory", new object[]
					{
						text
					});
				}
			}
			return true;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005614 File Offset: 0x00003814
		private void WriteInputObjectError(object inputObject, string resourceString, string errorId, params object[] args)
		{
			Type typeSpecified = (inputObject == null) ? null : inputObject.GetType();
			ParameterBindingException exception = new ParameterBindingException(ErrorCategory.InvalidArgument, base.Command.MyInvocation, null, null, null, typeSpecified, resourceString, errorId, args);
			ErrorRecord errorRecord = new ErrorRecord(exception, errorId, ErrorCategory.InvalidArgument, inputObject);
			errorRecord.SetInvocationInfo(base.Command.MyInvocation);
			this.commandRuntime._WriteErrorSkipAllowCheck(errorRecord, null);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005678 File Offset: 0x00003878
		private bool ProcessInputPipelineObject(object inputObject)
		{
			PSObject psobject = null;
			if (inputObject != null)
			{
				psobject = PSObject.AsPSObject(inputObject);
			}
			base.Command.CurrentPipelineObject = psobject;
			return this.CmdletParameterBinderController.BindPipelineParameters(psobject);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000056E5 File Offset: 0x000038E5
		private static Cmdlet ConstructInstance(Type type)
		{
			return CommandProcessor.ConstructInstanceCache.GetOrAdd(type, (Type t) => Expression.Lambda<Func<Cmdlet>>(typeof(Cmdlet).IsAssignableFrom(t) ? Expression.New(t) : Expression.Constant(null, typeof(Cmdlet)), new ParameterExpression[0]).Compile())();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005714 File Offset: 0x00003914
		private void Init(CmdletInfo cmdletInformation)
		{
			Cmdlet cmdlet = null;
			Exception ex = null;
			string text = null;
			string text2 = null;
			try
			{
				cmdlet = CommandProcessor.ConstructInstance(cmdletInformation.ImplementingType);
				if (cmdlet == null)
				{
					ex = new InvalidCastException();
					text = "CmdletDoesNotDeriveFromCmdletType";
					text2 = DiscoveryExceptions.CmdletDoesNotDeriveFromCmdletType;
				}
			}
			catch (MemberAccessException ex2)
			{
				ex = ex2;
			}
			catch (TypeLoadException ex3)
			{
				ex = ex3;
			}
			catch (Exception ex4)
			{
				CommandProcessorBase.CheckForSevereException(ex4);
				CmdletInvocationException ex5 = new CmdletInvocationException(ex4, null);
				MshLog.LogCommandHealthEvent(this._context, ex5, Severity.Warning);
				throw ex5;
			}
			if (ex != null)
			{
				MshLog.LogCommandHealthEvent(this._context, ex, Severity.Warning);
				CommandNotFoundException ex6 = new CommandNotFoundException(cmdletInformation.Name, ex, text ?? "CmdletNotFoundException", text2 ?? DiscoveryExceptions.CmdletNotFoundException, new object[]
				{
					ex.Message
				});
				throw ex6;
			}
			base.Command = cmdlet;
			base.CommandScope = base.Context.EngineSessionState.CurrentScope;
			this.InitCommon();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005814 File Offset: 0x00003A14
		private void Init(IScriptCommandInfo scriptCommandInfo)
		{
			InternalCommand command = new PSScriptCmdlet(scriptCommandInfo.ScriptBlock, this._useLocalScope, base.FromScriptFile, this._context);
			base.Command = command;
			base.CommandScope = (this._useLocalScope ? base.CommandSessionState.NewScope(this._fromScriptFile) : base.CommandSessionState.CurrentScope);
			this.InitCommon();
			if (!base.UseLocalScope)
			{
				CommandProcessorBase.ValidateCompatibleLanguageMode(scriptCommandInfo.ScriptBlock, this._context.LanguageMode, base.Command.MyInvocation);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000058A4 File Offset: 0x00003AA4
		private void InitCommon()
		{
			base.Command.CommandInfo = base.CommandInfo;
			this._obsoleteAttribute = base.CommandInfo.CommandMetadata.Obsolete;
			base.Command.Context = this._context;
			try
			{
				this.commandRuntime = new MshCommandRuntime(this._context, base.CommandInfo, base.Command);
				base.Command.commandRuntime = this.commandRuntime;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				MshLog.LogCommandHealthEvent(this._context, ex, Severity.Warning);
				throw;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005940 File Offset: 0x00003B40
		internal override bool IsHelpRequested(out string helpTarget, out HelpCategory helpCategory)
		{
			if (this.arguments != null)
			{
				foreach (CommandParameterInternal commandParameterInternal in this.arguments)
				{
					if (commandParameterInternal.IsDashQuestion())
					{
						helpCategory = HelpCategory.All;
						if (base.Command != null && base.Command.MyInvocation != null && !string.IsNullOrEmpty(base.Command.MyInvocation.InvocationName))
						{
							helpTarget = base.Command.MyInvocation.InvocationName;
							if (string.Equals(base.Command.MyInvocation.InvocationName, base.CommandInfo.Name, StringComparison.OrdinalIgnoreCase))
							{
								helpCategory = base.CommandInfo.HelpCategory;
							}
						}
						else
						{
							helpTarget = base.CommandInfo.Name;
							helpCategory = base.CommandInfo.HelpCategory;
						}
						return true;
					}
				}
			}
			return base.IsHelpRequested(out helpTarget, out helpCategory);
		}

		// Token: 0x04000056 RID: 86
		private CmdletParameterBinderController _cmdletParameterBinderController;

		// Token: 0x04000057 RID: 87
		private ObsoleteAttribute _obsoleteAttribute;

		// Token: 0x04000058 RID: 88
		private bool _firstCallToRead = true;

		// Token: 0x04000059 RID: 89
		private bool _bailInNextCall;

		// Token: 0x0400005A RID: 90
		private static readonly ConcurrentDictionary<Type, Func<Cmdlet>> ConstructInstanceCache = new ConcurrentDictionary<Type, Func<Cmdlet>>();
	}
}
