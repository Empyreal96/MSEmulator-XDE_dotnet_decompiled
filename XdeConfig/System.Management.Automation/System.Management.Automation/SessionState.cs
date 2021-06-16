using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000039 RID: 57
	public sealed class SessionState
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000AC04 File Offset: 0x00008E04
		internal SessionState(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000AC24 File Offset: 0x00008E24
		internal SessionState(ExecutionContext context, bool createAsChild, bool linkToGlobal)
		{
			if (context == null)
			{
				throw new InvalidOperationException("ExecutionContext");
			}
			if (createAsChild)
			{
				this.sessionState = new SessionStateInternal(context.EngineSessionState, linkToGlobal, context);
			}
			else
			{
				this.sessionState = new SessionStateInternal(context);
			}
			this.sessionState.PublicSessionState = this;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000AC78 File Offset: 0x00008E78
		public SessionState()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				throw new InvalidOperationException("ExecutionContext");
			}
			this.sessionState = new SessionStateInternal(executionContextFromTLS);
			this.sessionState.PublicSessionState = this;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000ACB7 File Offset: 0x00008EB7
		public DriveManagementIntrinsics Drive
		{
			get
			{
				if (this.drive == null)
				{
					this.drive = new DriveManagementIntrinsics(this.sessionState);
				}
				return this.drive;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000ACD8 File Offset: 0x00008ED8
		public CmdletProviderManagementIntrinsics Provider
		{
			get
			{
				if (this.provider == null)
				{
					this.provider = new CmdletProviderManagementIntrinsics(this.sessionState);
				}
				return this.provider;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000ACF9 File Offset: 0x00008EF9
		public PathIntrinsics Path
		{
			get
			{
				if (this.path == null)
				{
					this.path = new PathIntrinsics(this.sessionState);
				}
				return this.path;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000AD1A File Offset: 0x00008F1A
		public PSVariableIntrinsics PSVariable
		{
			get
			{
				if (this.variable == null)
				{
					this.variable = new PSVariableIntrinsics(this.sessionState);
				}
				return this.variable;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000AD3B File Offset: 0x00008F3B
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000AD48 File Offset: 0x00008F48
		public PSLanguageMode LanguageMode
		{
			get
			{
				return this.sessionState.LanguageMode;
			}
			set
			{
				this.sessionState.LanguageMode = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000AD56 File Offset: 0x00008F56
		public bool UseFullLanguageModeInDebugger
		{
			get
			{
				return this.sessionState.UseFullLanguageModeInDebugger;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000AD63 File Offset: 0x00008F63
		public List<string> Scripts
		{
			get
			{
				return this.sessionState.Scripts;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000AD70 File Offset: 0x00008F70
		public List<string> Applications
		{
			get
			{
				return this.sessionState.Applications;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000AD7D File Offset: 0x00008F7D
		public PSModuleInfo Module
		{
			get
			{
				return this.sessionState.Module;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000AD8A File Offset: 0x00008F8A
		public ProviderIntrinsics InvokeProvider
		{
			get
			{
				return this.sessionState.InvokeProvider;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000AD97 File Offset: 0x00008F97
		public CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				return this.sessionState.ExecutionContext.EngineIntrinsics.InvokeCommand;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		public static void ThrowIfNotVisible(CommandOrigin origin, object valueToCheck)
		{
			if (SessionState.IsVisible(origin, valueToCheck))
			{
				return;
			}
			PSVariable psvariable = valueToCheck as PSVariable;
			SessionStateException ex;
			if (psvariable != null)
			{
				ex = new SessionStateException(psvariable.Name, SessionStateCategory.Variable, "VariableIsPrivate", SessionStateStrings.VariableIsPrivate, ErrorCategory.PermissionDenied, new object[0]);
				throw ex;
			}
			CommandInfo commandInfo = valueToCheck as CommandInfo;
			if (commandInfo != null)
			{
				string text = null;
				if (commandInfo != null)
				{
					text = commandInfo.Name;
				}
				if (text != null)
				{
					ex = new SessionStateException(text, SessionStateCategory.Command, "NamedCommandIsPrivate", SessionStateStrings.NamedCommandIsPrivate, ErrorCategory.PermissionDenied, new object[0]);
				}
				else
				{
					ex = new SessionStateException("", SessionStateCategory.Command, "CommandIsPrivate", SessionStateStrings.CommandIsPrivate, ErrorCategory.PermissionDenied, new object[0]);
				}
				throw ex;
			}
			ex = new SessionStateException(null, SessionStateCategory.Resource, "ResourceIsPrivate", SessionStateStrings.ResourceIsPrivate, ErrorCategory.PermissionDenied, new object[0]);
			throw ex;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000AE68 File Offset: 0x00009068
		public static bool IsVisible(CommandOrigin origin, object valueToCheck)
		{
			if (origin == CommandOrigin.Internal)
			{
				return true;
			}
			IHasSessionStateEntryVisibility hasSessionStateEntryVisibility = valueToCheck as IHasSessionStateEntryVisibility;
			return hasSessionStateEntryVisibility == null || hasSessionStateEntryVisibility.Visibility == SessionStateEntryVisibility.Public;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000AE90 File Offset: 0x00009090
		public static bool IsVisible(CommandOrigin origin, PSVariable variable)
		{
			if (origin == CommandOrigin.Internal)
			{
				return true;
			}
			if (variable == null)
			{
				throw PSTraceSource.NewArgumentNullException("variable");
			}
			return variable.Visibility == SessionStateEntryVisibility.Public;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000AEAF File Offset: 0x000090AF
		public static bool IsVisible(CommandOrigin origin, CommandInfo commandInfo)
		{
			if (origin == CommandOrigin.Internal)
			{
				return true;
			}
			if (commandInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandInfo");
			}
			return commandInfo.Visibility == SessionStateEntryVisibility.Public;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000AECE File Offset: 0x000090CE
		internal SessionStateInternal Internal
		{
			get
			{
				return this.sessionState;
			}
		}

		// Token: 0x040000ED RID: 237
		private SessionStateInternal sessionState;

		// Token: 0x040000EE RID: 238
		private DriveManagementIntrinsics drive;

		// Token: 0x040000EF RID: 239
		private CmdletProviderManagementIntrinsics provider;

		// Token: 0x040000F0 RID: 240
		private PathIntrinsics path;

		// Token: 0x040000F1 RID: 241
		private PSVariableIntrinsics variable;
	}
}
