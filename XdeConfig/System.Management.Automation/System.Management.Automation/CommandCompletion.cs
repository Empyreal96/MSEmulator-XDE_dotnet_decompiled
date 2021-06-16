using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x02000974 RID: 2420
	public class CommandCompletion
	{
		// Token: 0x060058D7 RID: 22743 RVA: 0x001D1D5E File Offset: 0x001CFF5E
		public CommandCompletion(Collection<CompletionResult> matches, int currentMatchIndex, int replacementIndex, int replacementLength)
		{
			this.CompletionMatches = matches;
			this.CurrentMatchIndex = currentMatchIndex;
			this.ReplacementIndex = replacementIndex;
			this.ReplacementLength = replacementLength;
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060058D8 RID: 22744 RVA: 0x001D1D83 File Offset: 0x001CFF83
		// (set) Token: 0x060058D9 RID: 22745 RVA: 0x001D1D8B File Offset: 0x001CFF8B
		public int CurrentMatchIndex { get; set; }

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060058DA RID: 22746 RVA: 0x001D1D94 File Offset: 0x001CFF94
		// (set) Token: 0x060058DB RID: 22747 RVA: 0x001D1D9C File Offset: 0x001CFF9C
		public int ReplacementIndex { get; set; }

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060058DC RID: 22748 RVA: 0x001D1DA5 File Offset: 0x001CFFA5
		// (set) Token: 0x060058DD RID: 22749 RVA: 0x001D1DAD File Offset: 0x001CFFAD
		public int ReplacementLength { get; set; }

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060058DE RID: 22750 RVA: 0x001D1DB6 File Offset: 0x001CFFB6
		// (set) Token: 0x060058DF RID: 22751 RVA: 0x001D1DBE File Offset: 0x001CFFBE
		public Collection<CompletionResult> CompletionMatches { get; set; }

		// Token: 0x060058E0 RID: 22752 RVA: 0x001D1DC8 File Offset: 0x001CFFC8
		public static Tuple<Ast, Token[], IScriptPosition> MapStringInputToParsedInput(string input, int cursorIndex)
		{
			if (cursorIndex > input.Length)
			{
				throw PSTraceSource.NewArgumentException("cursorIndex");
			}
			Token[] item;
			ParseError[] array;
			ScriptBlockAst scriptBlockAst = Parser.ParseInput(input, out item, out array);
			IScriptPosition item2 = ((InternalScriptPosition)scriptBlockAst.Extent.StartScriptPosition).CloneWithNewOffset(cursorIndex);
			return Tuple.Create<Ast, Token[], IScriptPosition>(scriptBlockAst, item, item2);
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x001D1E14 File Offset: 0x001D0014
		public static CommandCompletion CompleteInput(string input, int cursorIndex, Hashtable options)
		{
			if (input == null)
			{
				return CommandCompletion.EmptyCommandCompletion;
			}
			Tuple<Ast, Token[], IScriptPosition> tuple = CommandCompletion.MapStringInputToParsedInput(input, cursorIndex);
			return CommandCompletion.CompleteInputImpl(tuple.Item1, tuple.Item2, tuple.Item3, options);
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x001D1E4A File Offset: 0x001D004A
		public static CommandCompletion CompleteInput(Ast ast, Token[] tokens, IScriptPosition positionOfCursor, Hashtable options)
		{
			if (ast == null)
			{
				throw PSTraceSource.NewArgumentNullException("ast");
			}
			if (tokens == null)
			{
				throw PSTraceSource.NewArgumentNullException("tokens");
			}
			if (positionOfCursor == null)
			{
				throw PSTraceSource.NewArgumentNullException("positionOfCursor");
			}
			return CommandCompletion.CompleteInputImpl(ast, tokens, positionOfCursor, options);
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x001D1E80 File Offset: 0x001D0080
		public static CommandCompletion CompleteInput(string input, int cursorIndex, Hashtable options, PowerShell powershell)
		{
			if (input == null)
			{
				return CommandCompletion.EmptyCommandCompletion;
			}
			if (cursorIndex > input.Length)
			{
				throw PSTraceSource.NewArgumentException("cursorIndex");
			}
			if (powershell == null)
			{
				throw PSTraceSource.NewArgumentNullException("powershell");
			}
			Debugger debugger = (powershell.Runspace != null) ? powershell.Runspace.Debugger : null;
			if (debugger != null && debugger.InBreakpoint)
			{
				return CommandCompletion.CompleteInputInDebugger(input, cursorIndex, options, debugger);
			}
			RemoteRunspace remoteRunspace = powershell.Runspace as RemoteRunspace;
			if (remoteRunspace != null)
			{
				if (powershell.IsNested || remoteRunspace.RunspaceAvailability != RunspaceAvailability.Available)
				{
					return CommandCompletion.EmptyCommandCompletion;
				}
				if (!powershell.IsChild)
				{
					CommandCompletion.CheckScriptCallOnRemoteRunspace(remoteRunspace);
					if (remoteRunspace.GetCapabilities().Equals(RunspaceCapability.Default))
					{
						powershell.Commands.Clear();
						int replacementIndex;
						int replacementLength;
						List<CompletionResult> list = CommandCompletion.InvokeLegacyTabExpansion(powershell, input, cursorIndex, true, out replacementIndex, out replacementLength);
						return new CommandCompletion(new Collection<CompletionResult>(list ?? CommandCompletion.EmptyCompletionResult), -1, replacementIndex, replacementLength);
					}
				}
			}
			return CommandCompletion.CallScriptWithStringParameterSet(input, cursorIndex, options, powershell);
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x001D1F6C File Offset: 0x001D016C
		public static CommandCompletion CompleteInput(Ast ast, Token[] tokens, IScriptPosition cursorPosition, Hashtable options, PowerShell powershell)
		{
			if (ast == null)
			{
				throw PSTraceSource.NewArgumentNullException("ast");
			}
			if (tokens == null)
			{
				throw PSTraceSource.NewArgumentNullException("tokens");
			}
			if (cursorPosition == null)
			{
				throw PSTraceSource.NewArgumentNullException("cursorPosition");
			}
			if (powershell == null)
			{
				throw PSTraceSource.NewArgumentNullException("powershell");
			}
			Debugger debugger = (powershell.Runspace != null) ? powershell.Runspace.Debugger : null;
			if (debugger != null && debugger.InBreakpoint)
			{
				return CommandCompletion.CompleteInputInDebugger(ast, tokens, cursorPosition, options, debugger);
			}
			RemoteRunspace remoteRunspace = powershell.Runspace as RemoteRunspace;
			if (remoteRunspace != null)
			{
				if (powershell.IsNested || remoteRunspace.RunspaceAvailability != RunspaceAvailability.Available)
				{
					return CommandCompletion.EmptyCommandCompletion;
				}
				if (!powershell.IsChild)
				{
					CommandCompletion.CheckScriptCallOnRemoteRunspace(remoteRunspace);
					if (remoteRunspace.GetCapabilities().Equals(RunspaceCapability.Default))
					{
						powershell.Commands.Clear();
						Tuple<string, int, int> inputAndCursorFromAst = CommandCompletion.GetInputAndCursorFromAst(cursorPosition);
						int num;
						int replacementLength;
						List<CompletionResult> list = CommandCompletion.InvokeLegacyTabExpansion(powershell, inputAndCursorFromAst.Item1, inputAndCursorFromAst.Item2, true, out num, out replacementLength);
						return new CommandCompletion(new Collection<CompletionResult>(list ?? CommandCompletion.EmptyCompletionResult), -1, num + inputAndCursorFromAst.Item3, replacementLength);
					}
					string text = ast.Extent.Text;
					int offset = ((InternalScriptPosition)cursorPosition).Offset;
					return CommandCompletion.CallScriptWithStringParameterSet(text, offset, options, powershell);
				}
			}
			return CommandCompletion.CallScriptWithAstParameterSet(ast, tokens, cursorPosition, options, powershell);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x001D20BC File Offset: 0x001D02BC
		public CompletionResult GetNextResult(bool forward)
		{
			CompletionResult result = null;
			int count = this.CompletionMatches.Count;
			if (count > 0)
			{
				this.CurrentMatchIndex += (forward ? 1 : -1);
				if (this.CurrentMatchIndex >= count)
				{
					this.CurrentMatchIndex = 0;
				}
				else if (this.CurrentMatchIndex < 0)
				{
					this.CurrentMatchIndex = count - 1;
				}
				result = this.CompletionMatches[this.CurrentMatchIndex];
			}
			return result;
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x001D2128 File Offset: 0x001D0328
		internal static CommandCompletion CompleteInputInDebugger(string input, int cursorIndex, Hashtable options, Debugger debugger)
		{
			if (input == null)
			{
				return CommandCompletion.EmptyCommandCompletion;
			}
			if (cursorIndex > input.Length)
			{
				throw PSTraceSource.NewArgumentException("cursorIndex");
			}
			if (debugger == null)
			{
				throw PSTraceSource.NewArgumentNullException("debugger");
			}
			return CommandCompletion.ProcessCompleteInputCommand(new Command("TabExpansion2")
			{
				Parameters = 
				{
					{
						"InputScript",
						input
					},
					{
						"CursorColumn",
						cursorIndex
					},
					{
						"Options",
						options
					}
				}
			}, debugger);
		}

		// Token: 0x060058E7 RID: 22759 RVA: 0x001D21AC File Offset: 0x001D03AC
		internal static CommandCompletion CompleteInputInDebugger(Ast ast, Token[] tokens, IScriptPosition cursorPosition, Hashtable options, Debugger debugger)
		{
			if (ast == null)
			{
				throw PSTraceSource.NewArgumentNullException("ast");
			}
			if (tokens == null)
			{
				throw PSTraceSource.NewArgumentNullException("tokens");
			}
			if (cursorPosition == null)
			{
				throw PSTraceSource.NewArgumentNullException("cursorPosition");
			}
			if (debugger == null)
			{
				throw PSTraceSource.NewArgumentNullException("debugger");
			}
			if (debugger is RemoteDebugger || debugger.IsPushed)
			{
				string text = ast.Extent.Text;
				int offset = ((InternalScriptPosition)cursorPosition).Offset;
				return CommandCompletion.CompleteInputInDebugger(text, offset, options, debugger);
			}
			return CommandCompletion.ProcessCompleteInputCommand(new Command("TabExpansion2")
			{
				Parameters = 
				{
					{
						"Ast",
						ast
					},
					{
						"Tokens",
						tokens
					},
					{
						"PositionOfCursor",
						cursorPosition
					},
					{
						"Options",
						options
					}
				}
			}, debugger);
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x001D2280 File Offset: 0x001D0480
		private static CommandCompletion ProcessCompleteInputCommand(Command cmd, Debugger debugger)
		{
			PSCommand command = new PSCommand(cmd);
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			debugger.ProcessCommand(command, psdataCollection);
			if (psdataCollection.Count == 1)
			{
				CommandCompletion commandCompletion = psdataCollection[0].BaseObject as CommandCompletion;
				if (commandCompletion != null)
				{
					return commandCompletion;
				}
			}
			return CommandCompletion.EmptyCommandCompletion;
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x001D22C8 File Offset: 0x001D04C8
		private static void CheckScriptCallOnRemoteRunspace(RemoteRunspace remoteRunspace)
		{
			RemoteRunspacePoolInternal remoteRunspacePoolInternal = remoteRunspace.RunspacePool.RemoteRunspacePoolInternal;
			if (remoteRunspacePoolInternal != null)
			{
				BaseClientSessionTransportManager transportManager = remoteRunspacePoolInternal.DataStructureHandler.TransportManager;
				if (transportManager != null && transportManager.TypeTable == null)
				{
					throw PSTraceSource.NewInvalidOperationException(TabCompletionStrings.CannotDeserializeTabCompletionResult, new object[0]);
				}
			}
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x001D230C File Offset: 0x001D050C
		private static CommandCompletion CallScriptWithStringParameterSet(string input, int cursorIndex, Hashtable options, PowerShell powershell)
		{
			try
			{
				powershell.Commands.Clear();
				powershell.AddCommand("TabExpansion2").AddArgument(input).AddArgument(cursorIndex).AddArgument(options);
				Collection<PSObject> collection = powershell.Invoke();
				if (collection == null)
				{
					return CommandCompletion.EmptyCommandCompletion;
				}
				if (collection.Count == 1)
				{
					object obj = PSObject.Base(collection[0]);
					CommandCompletion commandCompletion = obj as CommandCompletion;
					if (commandCompletion != null)
					{
						return commandCompletion;
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				powershell.Commands.Clear();
			}
			return CommandCompletion.EmptyCommandCompletion;
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x001D23BC File Offset: 0x001D05BC
		private static CommandCompletion CallScriptWithAstParameterSet(Ast ast, Token[] tokens, IScriptPosition cursorPosition, Hashtable options, PowerShell powershell)
		{
			try
			{
				powershell.Commands.Clear();
				powershell.AddCommand("TabExpansion2").AddArgument(ast).AddArgument(tokens).AddArgument(cursorPosition).AddArgument(options);
				Collection<PSObject> collection = powershell.Invoke();
				if (collection == null)
				{
					return CommandCompletion.EmptyCommandCompletion;
				}
				if (collection.Count == 1)
				{
					object obj = PSObject.Base(collection[0]);
					CommandCompletion commandCompletion = obj as CommandCompletion;
					if (commandCompletion != null)
					{
						return commandCompletion;
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				powershell.Commands.Clear();
			}
			return CommandCompletion.EmptyCommandCompletion;
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x001D2474 File Offset: 0x001D0674
		private static CommandCompletion CompleteInputImpl(Ast ast, Token[] tokens, IScriptPosition positionOfCursor, Hashtable options)
		{
			CommandCompletion result;
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				bool flag = executionContextFromTLS.TakeResponsibilityForModuleAnalysisAppDomain();
				try
				{
					int num = -1;
					int replacementLength = -1;
					List<CompletionResult> list = null;
					if (CommandCompletion.NeedToInvokeLegacyTabExpansion(powerShell))
					{
						Tuple<string, int, int> inputAndCursorFromAst = CommandCompletion.GetInputAndCursorFromAst(positionOfCursor);
						list = CommandCompletion.InvokeLegacyTabExpansion(powerShell, inputAndCursorFromAst.Item1, inputAndCursorFromAst.Item2, false, out num, out replacementLength);
						num += inputAndCursorFromAst.Item3;
					}
					if (list == null || list.Count == 0)
					{
						CompletionAnalysis completionAnalysis = new CompletionAnalysis(ast, tokens, positionOfCursor, options);
						list = completionAnalysis.GetResults(powerShell, out num, out replacementLength);
					}
					result = new CommandCompletion(new Collection<CompletionResult>(list ?? CommandCompletion.EmptyCompletionResult), -1, num, replacementLength);
				}
				finally
				{
					if (flag)
					{
						executionContextFromTLS.ReleaseResponsibilityForModuleAnalysisAppDomain();
					}
				}
			}
			return result;
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x001D2548 File Offset: 0x001D0748
		private static Tuple<string, int, int> GetInputAndCursorFromAst(IScriptPosition cursorPosition)
		{
			string line = cursorPosition.Line;
			int num = cursorPosition.ColumnNumber - 1;
			int item = cursorPosition.Offset - num;
			return Tuple.Create<string, int, int>(line.Substring(0, num), num, item);
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x001D2580 File Offset: 0x001D0780
		private static bool NeedToInvokeLegacyTabExpansion(PowerShell powershell)
		{
			ExecutionContext contextFromTLS = powershell.GetContextFromTLS();
			FunctionInfo function = contextFromTLS.EngineSessionState.GetFunction("TabExpansion");
			return function != null || contextFromTLS.EngineSessionState.GetAlias("TabExpansion") != null;
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x001D25C4 File Offset: 0x001D07C4
		private static List<CompletionResult> InvokeLegacyTabExpansion(PowerShell powershell, string input, int cursorIndex, bool remoteToWin7, out int replacementIndex, out int replacementLength)
		{
			List<CompletionResult> list = null;
			string text = (cursorIndex != input.Length) ? input.Substring(0, cursorIndex) : input;
			char c;
			string text2 = CommandCompletion.LastWordFinder.FindLastWord(text, out replacementIndex, out c);
			replacementLength = text.Length - replacementIndex;
			CompletionExecutionHelper completionExecutionHelper = new CompletionExecutionHelper(powershell);
			powershell.AddCommand("TabExpansion").AddArgument(text).AddArgument(text2);
			Exception ex;
			Collection<PSObject> collection = completionExecutionHelper.ExecuteCurrentPowerShell(out ex, null);
			if (collection != null)
			{
				list = new List<CompletionResult>();
				foreach (PSObject psobject in collection)
				{
					CompletionResult completionResult = PSObject.Base(psobject) as CompletionResult;
					if (completionResult == null)
					{
						string text3 = psobject.ToString();
						if (c != '\0' && text3.Length > 2 && text3[0] != c)
						{
							text3 = c + text3 + c;
						}
						completionResult = new CompletionResult(text3);
					}
					list.Add(completionResult);
				}
			}
			if (remoteToWin7 && (list == null || list.Count == 0))
			{
				string quote = (c == '\0') ? string.Empty : c.ToString();
				list = CommandCompletion.PSv2CompletionCompleter.PSv2GenerateMatchSetOfFiles(completionExecutionHelper, text2, replacementIndex == 0, quote);
				List<CompletionResult> list2 = CommandCompletion.PSv2CompletionCompleter.PSv2GenerateMatchSetOfCmdlets(completionExecutionHelper, text2, quote, replacementIndex == 0);
				if (list2 != null && list2.Count > 0)
				{
					list.AddRange(list2);
				}
			}
			return list;
		}

		// Token: 0x04002FC8 RID: 12232
		internal static readonly IList<CompletionResult> EmptyCompletionResult = new CompletionResult[0];

		// Token: 0x04002FC9 RID: 12233
		private static readonly CommandCompletion EmptyCommandCompletion = new CommandCompletion(new Collection<CompletionResult>(CommandCompletion.EmptyCompletionResult), -1, -1, -1);

		// Token: 0x02000975 RID: 2421
		private static class PSv2CompletionCompleter
		{
			// Token: 0x060058F1 RID: 22769 RVA: 0x001D274C File Offset: 0x001D094C
			private static bool PSv2IsCommandLikeCmdlet(string lastWord, out bool isSnapinSpecified)
			{
				isSnapinSpecified = false;
				string[] array = lastWord.Split(new char[]
				{
					'\\'
				});
				if (array.Length == 1)
				{
					return CommandCompletion.PSv2CompletionCompleter.CmdletTabRegex.IsMatch(lastWord);
				}
				if (array.Length == 2)
				{
					isSnapinSpecified = PSSnapInInfo.IsPSSnapinIdValid(array[0]);
					if (isSnapinSpecified)
					{
						return CommandCompletion.PSv2CompletionCompleter.CmdletTabRegex.IsMatch(array[1]);
					}
				}
				return false;
			}

			// Token: 0x060058F2 RID: 22770 RVA: 0x001D27A8 File Offset: 0x001D09A8
			internal static List<CompletionResult> PSv2GenerateMatchSetOfCmdlets(CompletionExecutionHelper helper, string lastWord, string quote, bool completingAtStartOfLine)
			{
				List<CompletionResult> list = new List<CompletionResult>();
				bool flag;
				if (!CommandCompletion.PSv2CompletionCompleter.PSv2IsCommandLikeCmdlet(lastWord, out flag))
				{
					return list;
				}
				helper.CurrentPowerShell.AddCommand("Get-Command").AddParameter("Name", lastWord + "*").AddCommand("Sort-Object").AddParameter("Property", "Name");
				Exception ex;
				Collection<PSObject> collection = helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null && collection.Count > 0)
				{
					CommandCompletion.PSv2CompletionCompleter.CommandAndName[] array = new CommandCompletion.PSv2CompletionCompleter.CommandAndName[collection.Count];
					for (int i = 0; i < collection.Count; i++)
					{
						PSObject psobject = collection[i];
						string fullName = CmdletInfo.GetFullName(psobject);
						array[i] = new CommandCompletion.PSv2CompletionCompleter.CommandAndName(psobject, PSSnapinQualifiedName.GetInstance(fullName));
					}
					if (flag)
					{
						foreach (CommandCompletion.PSv2CompletionCompleter.CommandAndName commandAndName in array)
						{
							CommandCompletion.PSv2CompletionCompleter.AddCommandResult(commandAndName, true, completingAtStartOfLine, quote, list);
						}
					}
					else
					{
						CommandCompletion.PSv2CompletionCompleter.PrependSnapInNameForSameCmdletNames(array, completingAtStartOfLine, quote, list);
					}
				}
				return list;
			}

			// Token: 0x060058F3 RID: 22771 RVA: 0x001D28B8 File Offset: 0x001D0AB8
			private static void AddCommandResult(CommandCompletion.PSv2CompletionCompleter.CommandAndName commandAndName, bool useFullName, bool completingAtStartOfLine, string quote, List<CompletionResult> results)
			{
				string completionText = useFullName ? commandAndName.CommandName.FullName : commandAndName.CommandName.ShortName;
				string completionText2 = CommandCompletion.PSv2CompletionCompleter.AddQuoteIfNecessary(completionText, quote, completingAtStartOfLine);
				CommandTypes? commandTypes = CommandCompletion.PSv2CompletionCompleter.SafeGetProperty<CommandTypes?>(commandAndName.Command, "CommandType");
				if (commandTypes == null)
				{
					return;
				}
				string text = CommandCompletion.PSv2CompletionCompleter.SafeGetProperty<string>(commandAndName.Command, "Name");
				string toolTip;
				if (commandTypes.Value == CommandTypes.Cmdlet || commandTypes.Value == CommandTypes.Application)
				{
					toolTip = CommandCompletion.PSv2CompletionCompleter.SafeGetProperty<string>(commandAndName.Command, "Definition");
				}
				else
				{
					toolTip = text;
				}
				results.Add(new CompletionResult(completionText2, text, CompletionResultType.Command, toolTip));
			}

			// Token: 0x060058F4 RID: 22772 RVA: 0x001D2958 File Offset: 0x001D0B58
			private static void PrependSnapInNameForSameCmdletNames(CommandCompletion.PSv2CompletionCompleter.CommandAndName[] cmdlets, bool completingAtStartOfLine, string quote, List<CompletionResult> results)
			{
				int num = 0;
				bool useFullName = false;
				CommandCompletion.PSv2CompletionCompleter.CommandAndName commandAndName;
				for (;;)
				{
					commandAndName = cmdlets[num];
					int num2 = num + 1;
					if (num2 >= cmdlets.Length)
					{
						break;
					}
					CommandCompletion.PSv2CompletionCompleter.CommandAndName commandAndName2 = cmdlets[num2];
					if (string.Compare(commandAndName.CommandName.ShortName, commandAndName2.CommandName.ShortName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						CommandCompletion.PSv2CompletionCompleter.AddCommandResult(commandAndName, true, completingAtStartOfLine, quote, results);
						useFullName = true;
					}
					else
					{
						CommandCompletion.PSv2CompletionCompleter.AddCommandResult(commandAndName, useFullName, completingAtStartOfLine, quote, results);
						useFullName = false;
					}
					num++;
				}
				CommandCompletion.PSv2CompletionCompleter.AddCommandResult(commandAndName, useFullName, completingAtStartOfLine, quote, results);
			}

			// Token: 0x060058F5 RID: 22773 RVA: 0x001D29D8 File Offset: 0x001D0BD8
			internal static List<CompletionResult> PSv2GenerateMatchSetOfFiles(CompletionExecutionHelper helper, string lastWord, bool completingAtStartOfLine, string quote)
			{
				List<CompletionResult> list = new List<CompletionResult>();
				lastWord = (lastWord ?? string.Empty);
				bool flag = string.IsNullOrEmpty(lastWord);
				bool flag2 = !flag && lastWord.EndsWith("*", StringComparison.Ordinal);
				bool flag3 = WildcardPattern.ContainsWildcardCharacters(lastWord);
				string path = lastWord + "*";
				bool shouldFullyQualifyPaths = CommandCompletion.PSv2CompletionCompleter.PSv2ShouldFullyQualifyPathsPath(helper, lastWord);
				bool flag4 = lastWord.StartsWith("\\\\", StringComparison.Ordinal) || lastWord.StartsWith("//", StringComparison.Ordinal);
				List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> s = null;
				List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> s2 = null;
				if (flag3 && !flag)
				{
					s = CommandCompletion.PSv2CompletionCompleter.PSv2FindMatches(helper, lastWord, shouldFullyQualifyPaths);
				}
				if (!flag2)
				{
					s2 = CommandCompletion.PSv2CompletionCompleter.PSv2FindMatches(helper, path, shouldFullyQualifyPaths);
				}
				IEnumerable<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> enumerable = CommandCompletion.PSv2CompletionCompleter.CombineMatchSets(s, s2);
				if (enumerable != null)
				{
					foreach (CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath pathItemAndConvertedPath in enumerable)
					{
						string text = WildcardPattern.Escape(pathItemAndConvertedPath.Path);
						string text2 = WildcardPattern.Escape(pathItemAndConvertedPath.ConvertedPath);
						string completionText = flag4 ? text2 : text;
						completionText = CommandCompletion.PSv2CompletionCompleter.AddQuoteIfNecessary(completionText, quote, completingAtStartOfLine);
						bool? flag5 = CommandCompletion.PSv2CompletionCompleter.SafeGetProperty<bool?>(pathItemAndConvertedPath.Item, "PSIsContainer");
						string text3 = CommandCompletion.PSv2CompletionCompleter.SafeGetProperty<string>(pathItemAndConvertedPath.Item, "PSChildName");
						string text4 = CompletionExecutionHelper.SafeToString(pathItemAndConvertedPath.ConvertedPath);
						if (flag5 != null && text3 != null && text4 != null)
						{
							CompletionResultType resultType = flag5.Value ? CompletionResultType.ProviderContainer : CompletionResultType.ProviderItem;
							list.Add(new CompletionResult(completionText, text3, resultType, text4));
						}
					}
				}
				return list;
			}

			// Token: 0x060058F6 RID: 22774 RVA: 0x001D2B64 File Offset: 0x001D0D64
			private static string AddQuoteIfNecessary(string completionText, string quote, bool completingAtStartOfLine)
			{
				if (completionText.IndexOfAny(CommandCompletion.PSv2CompletionCompleter.CharsRequiringQuotedString) != -1)
				{
					bool flag = quote.Length == 0 && completingAtStartOfLine;
					string text = (quote.Length == 0) ? "'" : quote;
					completionText = ((text == "'") ? completionText.Replace("'", "''") : completionText);
					completionText = text + completionText + text;
					completionText = (flag ? ("& " + completionText) : completionText);
				}
				else
				{
					completionText = quote + completionText + quote;
				}
				return completionText;
			}

			// Token: 0x060058F7 RID: 22775 RVA: 0x001D2BEC File Offset: 0x001D0DEC
			private static IEnumerable<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> CombineMatchSets(List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> s1, List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> s2)
			{
				if (s1 == null || s1.Count < 1)
				{
					return s2;
				}
				if (s2 == null || s2.Count < 1)
				{
					return s1;
				}
				List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> list = new List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath>();
				list.AddRange(s1);
				int i = 0;
				int num = 0;
				while (i < s2.Count)
				{
					if (num < s1.Count && string.Compare(s2[i].Path, s1[num].Path, StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						num++;
					}
					else
					{
						list.Add(s2[i]);
					}
					i++;
				}
				return list;
			}

			// Token: 0x060058F8 RID: 22776 RVA: 0x001D2C74 File Offset: 0x001D0E74
			private static T SafeGetProperty<T>(PSObject psObject, string propertyName)
			{
				if (psObject == null)
				{
					return default(T);
				}
				PSPropertyInfo pspropertyInfo = psObject.Properties[propertyName];
				if (pspropertyInfo == null)
				{
					return default(T);
				}
				object value = pspropertyInfo.Value;
				if (value == null)
				{
					return default(T);
				}
				T result;
				if (LanguagePrimitives.TryConvertTo<T>(value, out result))
				{
					return result;
				}
				return default(T);
			}

			// Token: 0x060058F9 RID: 22777 RVA: 0x001D2CD4 File Offset: 0x001D0ED4
			private static bool PSv2ShouldFullyQualifyPathsPath(CompletionExecutionHelper helper, string lastWord)
			{
				if (lastWord.StartsWith("~", StringComparison.OrdinalIgnoreCase) || lastWord.StartsWith("\\", StringComparison.OrdinalIgnoreCase) || lastWord.StartsWith("/", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				helper.CurrentPowerShell.AddCommand("Split-Path").AddParameter("Path", lastWord).AddParameter("IsAbsolute", true);
				return helper.ExecuteCommandAndGetResultAsBool();
			}

			// Token: 0x060058FA RID: 22778 RVA: 0x001D2D58 File Offset: 0x001D0F58
			private static List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> PSv2FindMatches(CompletionExecutionHelper helper, string path, bool shouldFullyQualifyPaths)
			{
				List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath> list = new List<CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath>();
				PowerShell currentPowerShell = helper.CurrentPowerShell;
				if (!shouldFullyQualifyPaths)
				{
					currentPowerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "& {{ trap {{ continue }} ; resolve-path {0} -Relative -WarningAction SilentlyContinue | %{{,($_,(get-item $_ -WarningAction SilentlyContinue),(convert-path $_ -WarningAction SilentlyContinue))}} }}", new object[]
					{
						path
					}));
				}
				else
				{
					currentPowerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "& {{ trap {{ continue }} ; resolve-path {0} -WarningAction SilentlyContinue | %{{,($_,(get-item $_ -WarningAction SilentlyContinue),(convert-path $_ -WarningAction SilentlyContinue))}} }}", new object[]
					{
						path
					}));
				}
				Exception ex;
				Collection<PSObject> collection = helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection == null || collection.Count == 0)
				{
					return null;
				}
				foreach (PSObject psobject in collection)
				{
					IList list2 = psobject.BaseObject as IList;
					if (list2 != null && list2.Count == 3)
					{
						object obj = list2[0];
						PSObject psobject2 = list2[1] as PSObject;
						object obj2 = list2[1];
						if (obj != null && psobject2 != null && obj2 != null)
						{
							list.Add(new CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath(CompletionExecutionHelper.SafeToString(obj), psobject2, CompletionExecutionHelper.SafeToString(obj2)));
						}
					}
				}
				if (list.Count == 0)
				{
					return null;
				}
				list.Sort((CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath x, CommandCompletion.PSv2CompletionCompleter.PathItemAndConvertedPath y) => string.Compare(x.Path, y.Path, StringComparison.CurrentCultureIgnoreCase));
				return list;
			}

			// Token: 0x04002FCE RID: 12238
			private static readonly Regex CmdletTabRegex = new Regex("^[\\w\\*\\?]+-[\\w\\*\\?]*");

			// Token: 0x04002FCF RID: 12239
			private static readonly char[] CharsRequiringQuotedString = "`&@'#{}()$,;|<> \t".ToCharArray();

			// Token: 0x02000976 RID: 2422
			private struct CommandAndName
			{
				// Token: 0x060058FD RID: 22781 RVA: 0x001D2EC8 File Offset: 0x001D10C8
				internal CommandAndName(PSObject command, PSSnapinQualifiedName commandName)
				{
					this.Command = command;
					this.CommandName = commandName;
				}

				// Token: 0x04002FD1 RID: 12241
				internal readonly PSObject Command;

				// Token: 0x04002FD2 RID: 12242
				internal readonly PSSnapinQualifiedName CommandName;
			}

			// Token: 0x02000977 RID: 2423
			private struct PathItemAndConvertedPath
			{
				// Token: 0x060058FE RID: 22782 RVA: 0x001D2ED8 File Offset: 0x001D10D8
				internal PathItemAndConvertedPath(string path, PSObject item, string convertedPath)
				{
					this.Path = path;
					this.Item = item;
					this.ConvertedPath = convertedPath;
				}

				// Token: 0x04002FD3 RID: 12243
				internal readonly string Path;

				// Token: 0x04002FD4 RID: 12244
				internal readonly PSObject Item;

				// Token: 0x04002FD5 RID: 12245
				internal readonly string ConvertedPath;
			}
		}

		// Token: 0x02000978 RID: 2424
		private class LastWordFinder
		{
			// Token: 0x060058FF RID: 22783 RVA: 0x001D2EEF File Offset: 0x001D10EF
			internal static string FindLastWord(string sentence, out int replacementIndexOut, out char closingQuote)
			{
				return new CommandCompletion.LastWordFinder(sentence).FindLastWord(out replacementIndexOut, out closingQuote);
			}

			// Token: 0x06005900 RID: 22784 RVA: 0x001D2EFE File Offset: 0x001D10FE
			private LastWordFinder(string sentence)
			{
				this.replacementIndex = 0;
				this.sentence = sentence;
			}

			// Token: 0x06005901 RID: 22785 RVA: 0x001D2F14 File Offset: 0x001D1114
			private string FindLastWord(out int replacementIndexOut, out char closingQuote)
			{
				bool flag = false;
				bool flag2 = false;
				this.ReplacementIndex = 0;
				this.sentenceIndex = 0;
				while (this.sentenceIndex < this.sentence.Length)
				{
					char c = this.sentence[this.sentenceIndex];
					if (c == '\'')
					{
						this.HandleQuote(ref flag, ref flag2, c);
					}
					else if (c == '"')
					{
						this.HandleQuote(ref flag2, ref flag, c);
					}
					else if (c == '`')
					{
						this.Consume(c);
						if (++this.sentenceIndex < this.sentence.Length)
						{
							this.Consume(this.sentence[this.sentenceIndex]);
						}
					}
					else if (CommandCompletion.LastWordFinder.IsWhitespace(c))
					{
						if (this.sequenceDueToEnd)
						{
							this.sequenceDueToEnd = false;
							if (flag)
							{
								flag = false;
							}
							if (flag2)
							{
								flag2 = false;
							}
							this.ReplacementIndex = this.sentenceIndex + 1;
						}
						else if (flag || flag2)
						{
							this.Consume(c);
						}
						else
						{
							this.ReplacementIndex = this.sentenceIndex + 1;
						}
					}
					else
					{
						this.Consume(c);
					}
					this.sentenceIndex++;
				}
				string result = new string(this.wordBuffer, 0, this.wordBufferIndex);
				closingQuote = (flag ? '\'' : (flag2 ? '"' : '\0'));
				replacementIndexOut = this.ReplacementIndex;
				return result;
			}

			// Token: 0x06005902 RID: 22786 RVA: 0x001D3064 File Offset: 0x001D1264
			private void HandleQuote(ref bool inQuote, ref bool inOppositeQuote, char c)
			{
				if (inOppositeQuote)
				{
					this.Consume(c);
					return;
				}
				if (inQuote)
				{
					if (this.sequenceDueToEnd)
					{
						this.ReplacementIndex = this.sentenceIndex + 1;
					}
					this.sequenceDueToEnd = !this.sequenceDueToEnd;
					return;
				}
				inQuote = true;
				this.ReplacementIndex = this.sentenceIndex;
			}

			// Token: 0x06005903 RID: 22787 RVA: 0x001D30B8 File Offset: 0x001D12B8
			private void Consume(char c)
			{
				this.wordBuffer[this.wordBufferIndex++] = c;
			}

			// Token: 0x170011E6 RID: 4582
			// (get) Token: 0x06005904 RID: 22788 RVA: 0x001D30DE File Offset: 0x001D12DE
			// (set) Token: 0x06005905 RID: 22789 RVA: 0x001D30E6 File Offset: 0x001D12E6
			private int ReplacementIndex
			{
				get
				{
					return this.replacementIndex;
				}
				set
				{
					this.wordBuffer = new char[this.sentence.Length];
					this.wordBufferIndex = 0;
					this.replacementIndex = value;
				}
			}

			// Token: 0x06005906 RID: 22790 RVA: 0x001D310C File Offset: 0x001D130C
			private static bool IsWhitespace(char c)
			{
				return c == ' ' || c == '\t';
			}

			// Token: 0x04002FD6 RID: 12246
			private readonly string sentence;

			// Token: 0x04002FD7 RID: 12247
			private char[] wordBuffer;

			// Token: 0x04002FD8 RID: 12248
			private int wordBufferIndex;

			// Token: 0x04002FD9 RID: 12249
			private int replacementIndex;

			// Token: 0x04002FDA RID: 12250
			private int sentenceIndex;

			// Token: 0x04002FDB RID: 12251
			private bool sequenceDueToEnd;
		}
	}
}
