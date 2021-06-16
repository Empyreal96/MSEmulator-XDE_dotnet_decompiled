using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200003D RID: 61
	internal abstract class ScriptCommandProcessorBase : CommandProcessorBase
	{
		// Token: 0x060002FE RID: 766 RVA: 0x0000AEDE File Offset: 0x000090DE
		protected ScriptCommandProcessorBase(ScriptBlock scriptBlock, ExecutionContext context, bool useLocalScope, CommandOrigin origin, SessionStateInternal sessionState)
		{
			this._dontUseScopeCommandOrigin = false;
			base.CommandInfo = new ScriptInfo(string.Empty, scriptBlock, context);
			this._fromScriptFile = false;
			this.CommonInitialization(scriptBlock, context, useLocalScope, origin, sessionState);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000AF14 File Offset: 0x00009114
		protected ScriptCommandProcessorBase(IScriptCommandInfo commandInfo, ExecutionContext context, bool useLocalScope, SessionStateInternal sessionState) : base((CommandInfo)commandInfo)
		{
			this._fromScriptFile = (base.CommandInfo is ExternalScriptInfo || base.CommandInfo is ScriptInfo);
			this._dontUseScopeCommandOrigin = true;
			this.CommonInitialization(commandInfo.ScriptBlock, context, useLocalScope, CommandOrigin.Internal, sessionState);
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000AF6C File Offset: 0x0000916C
		internal ScriptParameterBinderController ScriptParameterBinderController
		{
			get
			{
				if (this._scriptParameterBinderController == null)
				{
					this._scriptParameterBinderController = new ScriptParameterBinderController(((IScriptCommandInfo)base.CommandInfo).ScriptBlock, base.Command.MyInvocation, base.Context, base.Command, base.CommandScope);
					this._scriptParameterBinderController.CommandLineParameters.UpdateInvocationInfo(base.Command.MyInvocation);
					base.Command.MyInvocation.UnboundArguments = this._scriptParameterBinderController.DollarArgs;
				}
				return this._scriptParameterBinderController;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000AFF8 File Offset: 0x000091F8
		protected void CommonInitialization(ScriptBlock scriptBlock, ExecutionContext context, bool useLocalScope, CommandOrigin origin, SessionStateInternal sessionState)
		{
			base.CommandSessionState = sessionState;
			this._context = context;
			this._rethrowExitException = base.Context.ScriptCommandProcessorShouldRethrowExit;
			this._context.ScriptCommandProcessorShouldRethrowExit = false;
			ScriptCommand scriptCommand = new ScriptCommand
			{
				CommandInfo = base.CommandInfo
			};
			base.Command = scriptCommand;
			base.Command.CommandOriginInternal = origin;
			base.Command.commandRuntime = (this.commandRuntime = new MshCommandRuntime(base.Context, base.CommandInfo, scriptCommand));
			base.CommandScope = (useLocalScope ? base.CommandSessionState.NewScope(base.FromScriptFile) : base.CommandSessionState.CurrentScope);
			base.UseLocalScope = useLocalScope;
			this._scriptBlock = scriptBlock;
			if (!base.UseLocalScope && !this._rethrowExitException)
			{
				CommandProcessorBase.ValidateCompatibleLanguageMode(this._scriptBlock, context.LanguageMode, base.Command.MyInvocation);
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000B0E0 File Offset: 0x000092E0
		internal override bool IsHelpRequested(out string helpTarget, out HelpCategory helpCategory)
		{
			if (this.arguments != null && base.CommandInfo != null && !string.IsNullOrEmpty(base.CommandInfo.Name) && this._scriptBlock != null)
			{
				foreach (CommandParameterInternal commandParameterInternal in this.arguments)
				{
					if (commandParameterInternal.IsDashQuestion())
					{
						Dictionary<Ast, Token[]> scriptBlockTokenCache = new Dictionary<Ast, Token[]>();
						string text;
						HelpInfo helpInfo = this._scriptBlock.GetHelpInfo(base.Context, base.CommandInfo, false, scriptBlockTokenCache, out text, out text);
						if (helpInfo != null)
						{
							helpTarget = helpInfo.Name;
							helpCategory = helpInfo.HelpCategory;
							return true;
						}
						break;
					}
				}
			}
			return base.IsHelpRequested(out helpTarget, out helpCategory);
		}

		// Token: 0x040000FA RID: 250
		protected bool _dontUseScopeCommandOrigin;

		// Token: 0x040000FB RID: 251
		protected bool _rethrowExitException;

		// Token: 0x040000FC RID: 252
		protected bool _exitWasCalled;

		// Token: 0x040000FD RID: 253
		protected ScriptBlock _scriptBlock;

		// Token: 0x040000FE RID: 254
		private ScriptParameterBinderController _scriptParameterBinderController;
	}
}
