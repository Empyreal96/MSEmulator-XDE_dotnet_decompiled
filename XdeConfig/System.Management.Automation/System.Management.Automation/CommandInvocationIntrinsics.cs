using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000008 RID: 8
	public class CommandInvocationIntrinsics
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000243D File Offset: 0x0000063D
		internal CommandInvocationIntrinsics(ExecutionContext context, PSCmdlet cmdlet)
		{
			this._context = context;
			if (cmdlet != null)
			{
				this._cmdlet = cmdlet;
				this.commandRuntime = (cmdlet.CommandRuntime as MshCommandRuntime);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002467 File Offset: 0x00000667
		internal CommandInvocationIntrinsics(ExecutionContext context) : this(context, null)
		{
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002471 File Offset: 0x00000671
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002483 File Offset: 0x00000683
		public bool HasErrors
		{
			get
			{
				return this.commandRuntime.PipelineProcessor.ExecutionFailed;
			}
			set
			{
				this.commandRuntime.PipelineProcessor.ExecutionFailed = value;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002496 File Offset: 0x00000696
		public string ExpandString(string source)
		{
			if (this._cmdlet != null)
			{
				this._cmdlet.ThrowIfStopping();
			}
			return this._context.Engine.Expand(source);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024BC File Offset: 0x000006BC
		public CommandInfo GetCommand(string commandName, CommandTypes type)
		{
			return this.GetCommand(commandName, type, null);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024C8 File Offset: 0x000006C8
		public CommandInfo GetCommand(string commandName, CommandTypes type, object[] arguments)
		{
			CommandInfo commandInfo = null;
			try
			{
				CommandOrigin commandOrigin = CommandOrigin.Runspace;
				if (this._cmdlet != null)
				{
					commandOrigin = this._cmdlet.CommandOrigin;
				}
				commandInfo = CommandDiscovery.LookupCommandInfo(commandName, type, SearchResolutionOptions.None, commandOrigin, this._context);
				if (commandInfo != null && arguments != null && arguments.Length > 0 && commandInfo.ImplementsDynamicParameters)
				{
					commandInfo = commandInfo.CreateGetCommandCopy(arguments);
				}
			}
			catch (CommandNotFoundException)
			{
			}
			return commandInfo;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002530 File Offset: 0x00000730
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002538 File Offset: 0x00000738
		public EventHandler<CommandLookupEventArgs> CommandNotFoundAction { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002541 File Offset: 0x00000741
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002549 File Offset: 0x00000749
		public EventHandler<CommandLookupEventArgs> PreCommandLookupAction { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002552 File Offset: 0x00000752
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000255A File Offset: 0x0000075A
		public EventHandler<CommandLookupEventArgs> PostCommandLookupAction { get; set; }

		// Token: 0x06000037 RID: 55 RVA: 0x00002563 File Offset: 0x00000763
		public CmdletInfo GetCmdlet(string commandName)
		{
			return CommandInvocationIntrinsics.GetCmdlet(commandName, this._context);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002574 File Offset: 0x00000774
		internal static CmdletInfo GetCmdlet(string commandName, ExecutionContext context)
		{
			CmdletInfo result = null;
			CommandSearcher commandSearcher = new CommandSearcher(commandName, SearchResolutionOptions.None, CommandTypes.Cmdlet, context);
			for (;;)
			{
				try
				{
					IL_0C:
					if (!commandSearcher.MoveNext())
					{
						break;
					}
				}
				catch (ArgumentException)
				{
					goto IL_0C;
				}
				catch (PathTooLongException)
				{
					goto IL_0C;
				}
				catch (FileLoadException)
				{
					goto IL_0C;
				}
				catch (MetadataException)
				{
					goto IL_0C;
				}
				catch (FormatException)
				{
					goto IL_0C;
				}
				result = (((IEnumerator)commandSearcher).Current as CmdletInfo);
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000025F8 File Offset: 0x000007F8
		public CmdletInfo GetCmdletByTypeName(string cmdletTypeName)
		{
			if (string.IsNullOrEmpty(cmdletTypeName))
			{
				throw PSTraceSource.NewArgumentNullException("cmdletTypeName");
			}
			Exception ex = null;
			Type type = TypeResolver.ResolveType(cmdletTypeName, out ex);
			if (ex != null)
			{
				throw ex;
			}
			if (type == null)
			{
				return null;
			}
			CmdletAttribute cmdletAttribute = null;
			foreach (object obj in type.GetTypeInfo().GetCustomAttributes(true))
			{
				cmdletAttribute = (obj as CmdletAttribute);
				if (cmdletAttribute != null)
				{
					break;
				}
			}
			if (cmdletAttribute == null)
			{
				throw PSTraceSource.NewNotSupportedException();
			}
			string nounName = cmdletAttribute.NounName;
			string verbName = cmdletAttribute.VerbName;
			string name = verbName + "-" + nounName;
			return new CmdletInfo(name, type, null, null, this._context);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000269F File Offset: 0x0000089F
		public List<CmdletInfo> GetCmdlets()
		{
			return this.GetCmdlets("*");
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000026AC File Offset: 0x000008AC
		public List<CmdletInfo> GetCmdlets(string pattern)
		{
			if (pattern == null)
			{
				throw PSTraceSource.NewArgumentNullException("pattern");
			}
			List<CmdletInfo> list = new List<CmdletInfo>();
			CommandSearcher commandSearcher = new CommandSearcher(pattern, SearchResolutionOptions.CommandNameIsPattern, CommandTypes.Cmdlet, this._context);
			for (;;)
			{
				try
				{
					IL_25:
					if (!commandSearcher.MoveNext())
					{
						break;
					}
				}
				catch (ArgumentException)
				{
					goto IL_25;
				}
				catch (PathTooLongException)
				{
					goto IL_25;
				}
				catch (FileLoadException)
				{
					goto IL_25;
				}
				catch (MetadataException)
				{
					goto IL_25;
				}
				catch (FormatException)
				{
					goto IL_25;
				}
				CmdletInfo cmdletInfo = ((IEnumerator)commandSearcher).Current as CmdletInfo;
				if (cmdletInfo != null)
				{
					list.Add(cmdletInfo);
				}
			}
			return list;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002754 File Offset: 0x00000954
		public List<string> GetCommandName(string name, bool nameIsPattern, bool returnFullName)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			List<string> list = new List<string>();
			foreach (CommandInfo commandInfo in this.GetCommands(name, CommandTypes.All, nameIsPattern))
			{
				if (commandInfo.CommandType == CommandTypes.Application)
				{
					string extension = Path.GetExtension(commandInfo.Name);
					if (string.IsNullOrEmpty(extension))
					{
						continue;
					}
					using (IEnumerator<string> enumerator2 = CommandDiscovery.PathExtensions.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							string text = enumerator2.Current;
							if (text.Equals(extension, StringComparison.OrdinalIgnoreCase))
							{
								if (returnFullName)
								{
									list.Add(commandInfo.Definition);
								}
								else
								{
									list.Add(commandInfo.Name);
								}
							}
						}
						continue;
					}
				}
				if (commandInfo.CommandType == CommandTypes.ExternalScript)
				{
					if (returnFullName)
					{
						list.Add(commandInfo.Definition);
					}
					else
					{
						list.Add(commandInfo.Name);
					}
				}
				else
				{
					list.Add(commandInfo.Name);
				}
			}
			return list;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002878 File Offset: 0x00000A78
		public IEnumerable<CommandInfo> GetCommands(string name, CommandTypes commandTypes, bool nameIsPattern)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			SearchResolutionOptions options = nameIsPattern ? (SearchResolutionOptions.ResolveAliasPatterns | SearchResolutionOptions.ResolveFunctionPatterns | SearchResolutionOptions.CommandNameIsPattern) : SearchResolutionOptions.None;
			return this.GetCommands(name, commandTypes, options, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A7C File Offset: 0x00000C7C
		internal IEnumerable<CommandInfo> GetCommands(string name, CommandTypes commandTypes, SearchResolutionOptions options, CommandOrigin? commandOrigin = null)
		{
			CommandSearcher searcher = new CommandSearcher(name, options, commandTypes, this._context);
			if (commandOrigin != null)
			{
				searcher.CommandOrigin = commandOrigin.Value;
			}
			for (;;)
			{
				try
				{
					IL_70:
					if (!searcher.MoveNext())
					{
						yield break;
					}
				}
				catch (ArgumentException)
				{
					goto IL_70;
				}
				catch (PathTooLongException)
				{
					goto IL_70;
				}
				catch (FileLoadException)
				{
					goto IL_70;
				}
				catch (MetadataException)
				{
					goto IL_70;
				}
				catch (FormatException)
				{
					goto IL_70;
				}
				CommandInfo commandInfo = ((IEnumerator)searcher).Current as CommandInfo;
				if (commandInfo != null)
				{
					yield return commandInfo;
				}
			}
			yield break;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002AB6 File Offset: 0x00000CB6
		public Collection<PSObject> InvokeScript(string script)
		{
			return this.InvokeScript(script, true, PipelineResultTypes.None, null, new object[0]);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public Collection<PSObject> InvokeScript(string script, params object[] args)
		{
			return this.InvokeScript(script, true, PipelineResultTypes.None, args, new object[0]);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002ADC File Offset: 0x00000CDC
		public Collection<PSObject> InvokeScript(SessionState sessionState, ScriptBlock scriptBlock, params object[] args)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			SessionStateInternal engineSessionState = this._context.EngineSessionState;
			Collection<PSObject> result;
			try
			{
				this._context.EngineSessionState = sessionState.Internal;
				result = this.InvokeScript(scriptBlock, false, PipelineResultTypes.None, null, args);
			}
			finally
			{
				this._context.EngineSessionState = engineSessionState;
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B50 File Offset: 0x00000D50
		public Collection<PSObject> InvokeScript(bool useLocalScope, ScriptBlock scriptBlock, IList input, params object[] args)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			Runspace defaultRunspace = Runspace.DefaultRunspace;
			Runspace.DefaultRunspace = this._context.CurrentRunspace;
			Collection<PSObject> result;
			try
			{
				result = this.InvokeScript(scriptBlock, useLocalScope, PipelineResultTypes.None, input, args);
			}
			finally
			{
				Runspace.DefaultRunspace = defaultRunspace;
			}
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public Collection<PSObject> InvokeScript(string script, bool useNewScope, PipelineResultTypes writeToPipeline, IList input, params object[] args)
		{
			if (script == null)
			{
				throw new ArgumentNullException("script");
			}
			ScriptBlock sb = ScriptBlock.Create(this._context, script);
			return this.InvokeScript(sb, useNewScope, writeToPipeline, input, args);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002BE0 File Offset: 0x00000DE0
		private Collection<PSObject> InvokeScript(ScriptBlock sb, bool useNewScope, PipelineResultTypes writeToPipeline, IList input, params object[] args)
		{
			if (this._cmdlet != null)
			{
				this._cmdlet.ThrowIfStopping();
			}
			Cmdlet cmdlet = null;
			ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior = ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe;
			if ((writeToPipeline & PipelineResultTypes.Output) == PipelineResultTypes.Output)
			{
				cmdlet = this._cmdlet;
				writeToPipeline &= ~PipelineResultTypes.Output;
			}
			if ((writeToPipeline & PipelineResultTypes.Error) == PipelineResultTypes.Error)
			{
				errorHandlingBehavior = ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe;
				writeToPipeline &= ~PipelineResultTypes.Error;
			}
			if (writeToPipeline != PipelineResultTypes.None)
			{
				throw PSTraceSource.NewNotImplementedException();
			}
			object obj;
			if (cmdlet != null)
			{
				sb.InvokeUsingCmdlet(cmdlet, useNewScope, errorHandlingBehavior, AutomationNull.Value, input, AutomationNull.Value, args);
				obj = AutomationNull.Value;
			}
			else
			{
				obj = sb.DoInvokeReturnAsIs(useNewScope, errorHandlingBehavior, AutomationNull.Value, input, AutomationNull.Value, args);
			}
			if (obj == AutomationNull.Value)
			{
				return new Collection<PSObject>();
			}
			Collection<PSObject> collection = obj as Collection<PSObject>;
			if (collection != null)
			{
				return collection;
			}
			collection = new Collection<PSObject>();
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(obj);
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					collection.Add(LanguagePrimitives.AsPSObjectOrNull(obj2));
				}
			}
			else
			{
				collection.Add(LanguagePrimitives.AsPSObjectOrNull(obj));
			}
			return collection;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public ScriptBlock NewScriptBlock(string scriptText)
		{
			if (this.commandRuntime != null)
			{
				this.commandRuntime.ThrowIfStopping();
			}
			return ScriptBlock.Create(this._context, scriptText);
		}

		// Token: 0x04000012 RID: 18
		private ExecutionContext _context;

		// Token: 0x04000013 RID: 19
		private PSCmdlet _cmdlet;

		// Token: 0x04000014 RID: 20
		private MshCommandRuntime commandRuntime;
	}
}
