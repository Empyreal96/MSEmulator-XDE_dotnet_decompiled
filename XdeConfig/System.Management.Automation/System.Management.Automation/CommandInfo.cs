using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200002C RID: 44
	public abstract class CommandInfo : IHasSessionStateEntryVisibility
	{
		// Token: 0x060001EC RID: 492 RVA: 0x00008698 File Offset: 0x00006898
		internal CommandInfo(string name, CommandTypes type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
			this._type = type;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000086E5 File Offset: 0x000068E5
		internal CommandInfo(string name, CommandTypes type, ExecutionContext context) : this(name, type)
		{
			this.Context = context;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000086F8 File Offset: 0x000068F8
		internal CommandInfo(CommandInfo other)
		{
			this._module = other._module;
			this._visibility = other._visibility;
			this._arguments = other._arguments;
			this.Context = other.Context;
			this._name = other._name;
			this._type = other._type;
			this._copiedCommand = other;
			this.DefiningLanguageMode = other.DefiningLanguageMode;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008784 File Offset: 0x00006984
		internal CommandInfo(string name, CommandInfo other) : this(other)
		{
			this._name = name;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008794 File Offset: 0x00006994
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000879C File Offset: 0x0000699C
		public CommandTypes CommandType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000087A4 File Offset: 0x000069A4
		public virtual string Source
		{
			get
			{
				return this.ModuleName;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000087AC File Offset: 0x000069AC
		public virtual Version Version
		{
			get
			{
				if (this._version == null && this._module != null)
				{
					if (this._module.Version.Equals(new Version(0, 0)))
					{
						if (this._module.Path.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
						{
							this._module.SetVersion(ModuleIntrinsics.GetManifestModuleVersion(this._module.Path));
						}
						else if (this._module.Path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
						{
							this._module.SetVersion(ClrFacade.GetAssemblyName(this._module.Path).Version);
						}
					}
					this._version = this._module.Version;
				}
				return this._version;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00008872 File Offset: 0x00006A72
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000887C File Offset: 0x00006A7C
		internal ExecutionContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
				if (value != null && this.DefiningLanguageMode == null)
				{
					this.DefiningLanguageMode = new PSLanguageMode?(value.LanguageMode);
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000088B4 File Offset: 0x00006AB4
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x000088BC File Offset: 0x00006ABC
		internal PSLanguageMode? DefiningLanguageMode { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000088C5 File Offset: 0x00006AC5
		internal virtual HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.None;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000088C8 File Offset: 0x00006AC8
		// (set) Token: 0x060001FA RID: 506 RVA: 0x000088D0 File Offset: 0x00006AD0
		internal CommandInfo CopiedCommand
		{
			get
			{
				return this._copiedCommand;
			}
			set
			{
				this._copiedCommand = value;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000088D9 File Offset: 0x00006AD9
		internal void SetCommandType(CommandTypes newType)
		{
			this._type = newType;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001FC RID: 508
		public abstract string Definition { get; }

		// Token: 0x060001FD RID: 509 RVA: 0x000088E2 File Offset: 0x00006AE2
		internal void Rename(string newName)
		{
			if (string.IsNullOrEmpty(newName))
			{
				throw new ArgumentNullException("newName");
			}
			this._name = newName;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000088FE File Offset: 0x00006AFE
		public override string ToString()
		{
			return ModuleCmdletBase.AddPrefixToCommandName(this._name, this._prefix);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00008911 File Offset: 0x00006B11
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000892D File Offset: 0x00006B2D
		public virtual SessionStateEntryVisibility Visibility
		{
			get
			{
				if (this._copiedCommand != null)
				{
					return this._copiedCommand.Visibility;
				}
				return this._visibility;
			}
			set
			{
				if (this._copiedCommand == null)
				{
					this._visibility = value;
				}
				else
				{
					this._copiedCommand.Visibility = value;
				}
				if (value == SessionStateEntryVisibility.Private && this._module != null)
				{
					this._module.ModuleHasPrivateMembers = true;
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00008964 File Offset: 0x00006B64
		internal virtual CommandMetadata CommandMetadata
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000896B File Offset: 0x00006B6B
		internal virtual string Syntax
		{
			get
			{
				return this.Definition;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008974 File Offset: 0x00006B74
		public string ModuleName
		{
			get
			{
				string text = null;
				if (this._module != null && !string.IsNullOrEmpty(this._module.Name))
				{
					text = this._module.Name;
				}
				else
				{
					CmdletInfo cmdletInfo = this as CmdletInfo;
					if (cmdletInfo != null && cmdletInfo.PSSnapIn != null)
					{
						text = cmdletInfo.PSSnapInName;
					}
				}
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000089CE File Offset: 0x00006BCE
		public PSModuleInfo Module
		{
			get
			{
				return this._module;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000089D8 File Offset: 0x00006BD8
		public RemotingCapability RemotingCapability
		{
			get
			{
				RemotingCapability result;
				try
				{
					result = this.ExternalCommandMetadata.RemotingCapability;
				}
				catch (PSNotSupportedException)
				{
					result = RemotingCapability.PowerShell;
				}
				return result;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008A0C File Offset: 0x00006C0C
		internal void SetModule(PSModuleInfo module)
		{
			this._module = module;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008A15 File Offset: 0x00006C15
		internal virtual bool ImplementsDynamicParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008A18 File Offset: 0x00006C18
		private MergedCommandParameterMetadata GetMergedCommandParameterMetdata()
		{
			if (this._context == null)
			{
				return null;
			}
			CommandProcessor commandProcessor;
			if (this.Context.CurrentCommandProcessor != null && this.Context.CurrentCommandProcessor.CommandInfo == this)
			{
				commandProcessor = (CommandProcessor)this.Context.CurrentCommandProcessor;
			}
			else
			{
				IScriptCommandInfo scriptCommandInfo = this as IScriptCommandInfo;
				commandProcessor = ((scriptCommandInfo != null) ? new CommandProcessor(scriptCommandInfo, this._context, true, false, scriptCommandInfo.ScriptBlock.SessionStateInternal ?? this.Context.EngineSessionState) : new CommandProcessor((CmdletInfo)this, this._context)
				{
					UseLocalScope = true
				});
				ParameterBinderController.AddArgumentsToCommandProcessor(commandProcessor, this.Arguments);
				CommandProcessorBase currentCommandProcessor = this.Context.CurrentCommandProcessor;
				try
				{
					this.Context.CurrentCommandProcessor = commandProcessor;
					commandProcessor.SetCurrentScopeToExecutionScope();
					commandProcessor.CmdletParameterBinderController.BindCommandLineParametersNoValidation(commandProcessor.arguments);
				}
				catch (ParameterBindingException)
				{
					if (commandProcessor.arguments.Count > 0)
					{
						throw;
					}
				}
				finally
				{
					this.Context.CurrentCommandProcessor = currentCommandProcessor;
					commandProcessor.RestorePreviousScope();
				}
			}
			return commandProcessor.CmdletParameterBinderController.BindableParameters;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008B40 File Offset: 0x00006D40
		public virtual Dictionary<string, ParameterMetadata> Parameters
		{
			get
			{
				Dictionary<string, ParameterMetadata> dictionary = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
				if (this.ImplementsDynamicParameters && this.Context != null)
				{
					MergedCommandParameterMetadata mergedCommandParameterMetdata = this.GetMergedCommandParameterMetdata();
					foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair in mergedCommandParameterMetdata.BindableParameters)
					{
						dictionary.Add(keyValuePair.Key, new ParameterMetadata(keyValuePair.Value.Parameter));
					}
					return dictionary;
				}
				return this.ExternalCommandMetadata.Parameters;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008BD4 File Offset: 0x00006DD4
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00008BF1 File Offset: 0x00006DF1
		internal CommandMetadata ExternalCommandMetadata
		{
			get
			{
				if (this._externalCommandMetadata == null)
				{
					this._externalCommandMetadata = new CommandMetadata(this, true);
				}
				return this._externalCommandMetadata;
			}
			set
			{
				this._externalCommandMetadata = value;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008BFC File Offset: 0x00006DFC
		public ParameterMetadata ResolveParameter(string name)
		{
			MergedCommandParameterMetadata mergedCommandParameterMetdata = this.GetMergedCommandParameterMetdata();
			MergedCompiledCommandParameter matchingParameter = mergedCommandParameterMetdata.GetMatchingParameter(name, true, true, null);
			return this.Parameters[matchingParameter.Parameter.Name];
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008C34 File Offset: 0x00006E34
		public ReadOnlyCollection<CommandParameterSetInfo> ParameterSets
		{
			get
			{
				if (this._parameterSets == null)
				{
					Collection<CommandParameterSetInfo> list = this.GenerateCommandParameterSetInfo();
					this._parameterSets = new ReadOnlyCollection<CommandParameterSetInfo>(list);
				}
				return this._parameterSets;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600020E RID: 526
		public abstract ReadOnlyCollection<PSTypeName> OutputType { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008C62 File Offset: 0x00006E62
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00008C6A File Offset: 0x00006E6A
		internal bool IsImported
		{
			get
			{
				return this._isImported;
			}
			set
			{
				this._isImported = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008C73 File Offset: 0x00006E73
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00008C7B File Offset: 0x00006E7B
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008C84 File Offset: 0x00006E84
		internal virtual CommandInfo CreateGetCommandCopy(object[] argumentList)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008C8C File Offset: 0x00006E8C
		internal Collection<CommandParameterSetInfo> GenerateCommandParameterSetInfo()
		{
			Collection<CommandParameterSetInfo> result;
			if (this.IsGetCommandCopy && this.ImplementsDynamicParameters)
			{
				result = CommandInfo.GetParameterMetadata(this.CommandMetadata, this.GetMergedCommandParameterMetdata());
			}
			else
			{
				result = CommandInfo.GetCacheableMetadata(this.CommandMetadata);
			}
			return result;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008CCA File Offset: 0x00006ECA
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00008CD2 File Offset: 0x00006ED2
		internal bool IsGetCommandCopy { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00008CDB File Offset: 0x00006EDB
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00008CE3 File Offset: 0x00006EE3
		internal object[] Arguments
		{
			get
			{
				return this._arguments;
			}
			set
			{
				this._arguments = value;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008CEC File Offset: 0x00006EEC
		internal static Collection<CommandParameterSetInfo> GetCacheableMetadata(CommandMetadata metadata)
		{
			return CommandInfo.GetParameterMetadata(metadata, metadata.StaticCommandParameterMetadata);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00008CFC File Offset: 0x00006EFC
		internal static Collection<CommandParameterSetInfo> GetParameterMetadata(CommandMetadata metadata, MergedCommandParameterMetadata parameterMetadata)
		{
			Collection<CommandParameterSetInfo> collection = new Collection<CommandParameterSetInfo>();
			if (parameterMetadata != null)
			{
				if (parameterMetadata.ParameterSetCount == 0)
				{
					collection.Add(new CommandParameterSetInfo("__AllParameterSets", false, uint.MaxValue, parameterMetadata));
				}
				else
				{
					int parameterSetCount = parameterMetadata.ParameterSetCount;
					for (int i = 0; i < parameterSetCount; i++)
					{
						uint num = 1U << i;
						string parameterSetName = parameterMetadata.GetParameterSetName(num);
						bool isDefaultParameterSet = (num & metadata.DefaultParameterSetFlag) != 0U;
						collection.Add(new CommandParameterSetInfo(parameterSetName, isDefaultParameterSet, num, parameterMetadata));
					}
				}
			}
			return collection;
		}

		// Token: 0x040000B8 RID: 184
		internal const int HasWorkflowKeyWord = 8;

		// Token: 0x040000B9 RID: 185
		internal const int IsCimCommand = 16;

		// Token: 0x040000BA RID: 186
		internal const int IsFile = 32;

		// Token: 0x040000BB RID: 187
		private string _name = string.Empty;

		// Token: 0x040000BC RID: 188
		private CommandTypes _type = CommandTypes.Application;

		// Token: 0x040000BD RID: 189
		private Version _version;

		// Token: 0x040000BE RID: 190
		private ExecutionContext _context;

		// Token: 0x040000BF RID: 191
		private CommandInfo _copiedCommand;

		// Token: 0x040000C0 RID: 192
		private SessionStateEntryVisibility _visibility;

		// Token: 0x040000C1 RID: 193
		private PSModuleInfo _module;

		// Token: 0x040000C2 RID: 194
		private CommandMetadata _externalCommandMetadata;

		// Token: 0x040000C3 RID: 195
		internal ReadOnlyCollection<CommandParameterSetInfo> _parameterSets;

		// Token: 0x040000C4 RID: 196
		private bool _isImported;

		// Token: 0x040000C5 RID: 197
		private string _prefix = "";

		// Token: 0x040000C6 RID: 198
		private object[] _arguments;
	}
}
