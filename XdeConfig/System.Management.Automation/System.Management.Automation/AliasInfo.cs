using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200004B RID: 75
	public class AliasInfo : CommandInfo
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		internal AliasInfo(string name, string definition, ExecutionContext context) : base(name, CommandTypes.Alias)
		{
			this._definition = definition;
			base.Context = context;
			if (context != null)
			{
				base.SetModule(context.SessionState.Internal.Module);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000E120 File Offset: 0x0000C320
		internal AliasInfo(string name, string definition, ExecutionContext context, ScopedItemOptions options) : base(name, CommandTypes.Alias)
		{
			this._definition = definition;
			base.Context = context;
			this.options = options;
			if (context != null)
			{
				base.SetModule(context.SessionState.Internal.Module);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000E17A File Offset: 0x0000C37A
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Alias;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000E180 File Offset: 0x0000C380
		public CommandInfo ReferencedCommand
		{
			get
			{
				CommandInfo result = null;
				if (this._definition != null && base.Context != null)
				{
					CommandSearcher commandSearcher = new CommandSearcher(this._definition, SearchResolutionOptions.None, CommandTypes.All, base.Context);
					if (commandSearcher.MoveNext())
					{
						IEnumerator<CommandInfo> enumerator = commandSearcher;
						result = enumerator.Current;
					}
				}
				return result;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000E1CC File Offset: 0x0000C3CC
		public CommandInfo ResolvedCommand
		{
			get
			{
				CommandInfo commandInfo = null;
				if (this._definition != null)
				{
					List<string> list = new List<string>();
					list.Add(base.Name);
					string definition = this._definition;
					commandInfo = this.ReferencedCommand;
					while (commandInfo != null && commandInfo.CommandType == CommandTypes.Alias)
					{
						commandInfo = ((AliasInfo)commandInfo).ReferencedCommand;
						if (commandInfo is AliasInfo)
						{
							if (SessionStateUtilities.CollectionContainsValue(list, commandInfo.Name, StringComparer.OrdinalIgnoreCase))
							{
								commandInfo = null;
								break;
							}
							list.Add(commandInfo.Name);
							definition = commandInfo.Definition;
						}
					}
					if (commandInfo == null)
					{
						this.unresolvedCommandName = definition;
					}
				}
				return commandInfo;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000E25A File Offset: 0x0000C45A
		public override string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000E264 File Offset: 0x0000C464
		internal void SetDefinition(string definition, bool force)
		{
			if ((this.options & ScopedItemOptions.Constant) != ScopedItemOptions.None || (!force && (this.options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None))
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Alias, "AliasNotWritable", SessionStateStrings.AliasNotWritable);
				throw ex;
			}
			this._definition = definition;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
		public ScopedItemOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.SetOptions(value, false);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		internal void SetOptions(ScopedItemOptions newOptions, bool force)
		{
			if ((this.options & ScopedItemOptions.Constant) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Alias, "AliasIsConstant", SessionStateStrings.AliasIsConstant);
				throw ex;
			}
			if (!force && (this.options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex2 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Alias, "AliasIsReadOnly", SessionStateStrings.AliasIsReadOnly);
				throw ex2;
			}
			if ((newOptions & ScopedItemOptions.Constant) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex3 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Alias, "AliasCannotBeMadeConstant", SessionStateStrings.AliasCannotBeMadeConstant);
				throw ex3;
			}
			if ((newOptions & ScopedItemOptions.AllScope) == ScopedItemOptions.None && (this.options & ScopedItemOptions.AllScope) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex4 = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Alias, "AliasAllScopeOptionCannotBeRemoved", SessionStateStrings.AliasAllScopeOptionCannotBeRemoved);
				throw ex4;
			}
			this.options = newOptions;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000E35F File Offset: 0x0000C55F
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000E367 File Offset: 0x0000C567
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000E370 File Offset: 0x0000C570
		internal string UnresolvedCommandName
		{
			get
			{
				return this.unresolvedCommandName;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000E378 File Offset: 0x0000C578
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				CommandInfo resolvedCommand = this.ResolvedCommand;
				if (resolvedCommand != null)
				{
					return resolvedCommand.OutputType;
				}
				return null;
			}
		}

		// Token: 0x0400016D RID: 365
		private string _definition = string.Empty;

		// Token: 0x0400016E RID: 366
		private ScopedItemOptions options;

		// Token: 0x0400016F RID: 367
		private string description = string.Empty;

		// Token: 0x04000170 RID: 368
		private string unresolvedCommandName;
	}
}
