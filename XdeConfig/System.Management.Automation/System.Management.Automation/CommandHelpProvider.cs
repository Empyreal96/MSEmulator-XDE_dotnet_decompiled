using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Help;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security;
using System.Xml;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020001B8 RID: 440
	internal class CommandHelpProvider : HelpProviderWithCache
	{
		// Token: 0x06001476 RID: 5238 RVA: 0x0007DC80 File Offset: 0x0007BE80
		internal CommandHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			this._context = helpSystem.ExecutionContext;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0007DCA0 File Offset: 0x0007BEA0
		static CommandHelpProvider()
		{
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Diagnostics", "Microsoft.PowerShell.Commands.Diagnostics.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Core", "System.Management.Automation.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Utility", "Microsoft.PowerShell.Commands.Utility.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Host", "Microsoft.PowerShell.ConsoleHost.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Management", "Microsoft.PowerShell.Commands.Management.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.PowerShell.Security", "Microsoft.PowerShell.Security.dll-Help.xml");
			CommandHelpProvider.engineModuleHelpFileCache.Add("Microsoft.WSMan.Management", "Microsoft.Wsman.Management.dll-Help.xml");
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0007DD57 File Offset: 0x0007BF57
		internal override string Name
		{
			get
			{
				return "Command Help Provider";
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x0007DD5E File Offset: 0x0007BF5E
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Alias | HelpCategory.Cmdlet;
			}
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0007DD64 File Offset: 0x0007BF64
		private void GetModulePaths(CommandInfo commandInfo, out string moduleName, out string moduleDir, out string nestedModulePath)
		{
			CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
			IScriptCommandInfo scriptCommandInfo = commandInfo as IScriptCommandInfo;
			string key = null;
			bool flag = false;
			moduleName = null;
			moduleDir = null;
			nestedModulePath = null;
			if (commandInfo.Module != null)
			{
				moduleName = commandInfo.Module.Name;
				moduleDir = commandInfo.Module.ModuleBase;
				if (!string.IsNullOrEmpty(commandInfo.Prefix))
				{
					flag = true;
					key = ModuleCmdletBase.RemovePrefixFromCommandName(commandInfo.Name, commandInfo.Prefix);
				}
				if (commandInfo.Module.NestedModules != null)
				{
					foreach (PSModuleInfo psmoduleInfo in commandInfo.Module.NestedModules)
					{
						if (cmdletInfo != null && (psmoduleInfo.ExportedCmdlets.ContainsKey(commandInfo.Name) || (flag && psmoduleInfo.ExportedCmdlets.ContainsKey(key))))
						{
							nestedModulePath = psmoduleInfo.Path;
							break;
						}
						if (scriptCommandInfo != null && (psmoduleInfo.ExportedFunctions.ContainsKey(commandInfo.Name) || psmoduleInfo.ExportedWorkflows.ContainsKey(commandInfo.Name) || (flag && psmoduleInfo.ExportedFunctions.ContainsKey(key)) || (flag && psmoduleInfo.ExportedWorkflows.ContainsKey(key))))
						{
							nestedModulePath = psmoduleInfo.Path;
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0007DEC0 File Offset: 0x0007C0C0
		private string GetHelpName(CommandInfo commandInfo)
		{
			CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
			if (cmdletInfo != null)
			{
				return cmdletInfo.FullName;
			}
			return commandInfo.Name;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0007DEE4 File Offset: 0x0007C0E4
		private HelpInfo GetHelpInfoFromHelpFile(CommandInfo commandInfo, string helpFileToFind, Collection<string> searchPaths, bool reportErrors, out string helpFile)
		{
			CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
			IScriptCommandInfo scriptCommandInfo = commandInfo as IScriptCommandInfo;
			HelpInfo result = null;
			helpFile = MUIFileSearcher.LocateFile(helpFileToFind, searchPaths);
			if (!string.IsNullOrEmpty(helpFile))
			{
				if (!this._helpFiles.Contains(helpFile))
				{
					if (cmdletInfo != null)
					{
						this.LoadHelpFile(helpFile, cmdletInfo.ModuleName, cmdletInfo.Name, reportErrors);
					}
					else if (scriptCommandInfo != null)
					{
						this.LoadHelpFile(helpFile, helpFile, commandInfo.Name, reportErrors);
					}
				}
				if (cmdletInfo != null)
				{
					result = this.GetFromCommandCacheOrCmdletInfo(cmdletInfo);
				}
				else if (scriptCommandInfo != null)
				{
					result = this.GetFromCommandCache(helpFile, commandInfo);
				}
			}
			return result;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0007DF74 File Offset: 0x0007C174
		private HelpInfo GetHelpInfo(CommandInfo commandInfo, bool reportErrors, bool searchOnlyContent)
		{
			HelpInfo helpInfo = null;
			string text = null;
			string text2 = null;
			string text3 = null;
			CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
			IScriptCommandInfo scriptCommandInfo = commandInfo as IScriptCommandInfo;
			FunctionInfo functionInfo = commandInfo as FunctionInfo;
			bool flag = cmdletInfo != null;
			bool flag2 = scriptCommandInfo != null;
			bool flag3 = functionInfo != null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			if (!flag && !flag2)
			{
				return null;
			}
			if (flag)
			{
				helpInfo = this.GetFromCommandCache(cmdletInfo.ModuleName, cmdletInfo.Name, cmdletInfo.HelpCategory);
				if (helpInfo == null)
				{
					text = this.FindHelpFile(cmdletInfo);
					if (!string.IsNullOrEmpty(text) && !this._helpFiles.Contains(text))
					{
						this.LoadHelpFile(text, cmdletInfo.ModuleName, cmdletInfo.Name, reportErrors);
					}
					helpInfo = this.GetFromCommandCacheOrCmdletInfo(cmdletInfo);
				}
			}
			else if (flag3)
			{
				text = functionInfo.HelpFile;
				if (!string.IsNullOrEmpty(text))
				{
					if (!this._helpFiles.Contains(text))
					{
						this.LoadHelpFile(text, text, commandInfo.Name, reportErrors);
					}
					helpInfo = this.GetFromCommandCache(text, commandInfo);
				}
			}
			if (helpInfo == null && flag2)
			{
				ScriptBlock scriptBlock = null;
				try
				{
					scriptBlock = scriptCommandInfo.ScriptBlock;
				}
				catch (RuntimeException)
				{
					return null;
				}
				if (scriptBlock == null)
				{
					goto IL_1A0;
				}
				text = null;
				helpInfo = scriptBlock.GetHelpInfo(this._context, commandInfo, searchOnlyContent, base.HelpSystem.ScriptBlockTokenCache, out text, out text3);
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						new Uri(text3);
						text2 = text3;
					}
					catch (UriFormatException)
					{
					}
				}
				if (helpInfo != null)
				{
					Uri uriForOnlineHelp = helpInfo.GetUriForOnlineHelp();
					if (uriForOnlineHelp != null)
					{
						text2 = uriForOnlineHelp.ToString();
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (!this._helpFiles.Contains(text))
					{
						this.LoadHelpFile(text, text, commandInfo.Name, reportErrors);
					}
					helpInfo = (this.GetFromCommandCache(text, commandInfo) ?? helpInfo);
				}
			}
			IL_1A0:
			if (helpInfo == null)
			{
				this.GetModulePaths(commandInfo, out text4, out text5, out text6);
				Collection<string> collection = new Collection<string>();
				if (!string.IsNullOrEmpty(text5))
				{
					collection.Add(text5);
				}
				if (!string.IsNullOrEmpty(text4) && !string.IsNullOrEmpty(text5))
				{
					string helpFileToFind = text4 + "-Help.xml";
					helpInfo = this.GetHelpInfoFromHelpFile(commandInfo, helpFileToFind, collection, reportErrors, out text);
				}
				if (helpInfo == null && !string.IsNullOrEmpty(text6))
				{
					collection.Add(Path.GetDirectoryName(text6));
					string helpFileToFind2 = Path.GetFileName(text6) + "-Help.xml";
					helpInfo = this.GetHelpInfoFromHelpFile(commandInfo, helpFileToFind2, collection, reportErrors, out text);
				}
			}
			if (helpInfo != null && !string.IsNullOrEmpty(text))
			{
				if (flag)
				{
					cmdletInfo.HelpFile = text;
				}
				else if (flag3)
				{
					functionInfo.HelpFile = text;
				}
			}
			if (helpInfo == null)
			{
				if (commandInfo.CommandType == CommandTypes.ExternalScript || commandInfo.CommandType == CommandTypes.Script)
				{
					helpInfo = SyntaxHelpInfo.GetHelpInfo(commandInfo.Name, commandInfo.Syntax, commandInfo.HelpCategory);
				}
				else
				{
					PSObject psobjectFromCmdletInfo = DefaultCommandHelpObjectBuilder.GetPSObjectFromCmdletInfo(commandInfo);
					psobjectFromCmdletInfo.TypeNames.Clear();
					psobjectFromCmdletInfo.TypeNames.Add(DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp);
					psobjectFromCmdletInfo.TypeNames.Add("CmdletHelpInfo");
					psobjectFromCmdletInfo.TypeNames.Add("HelpInfo");
					helpInfo = new MamlCommandHelpInfo(psobjectFromCmdletInfo, commandInfo.HelpCategory);
				}
			}
			if (helpInfo != null)
			{
				if (flag2 && helpInfo.GetUriForOnlineHelp() == null)
				{
					if (!string.IsNullOrEmpty(commandInfo.CommandMetadata.HelpUri))
					{
						DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(helpInfo.FullHelp, commandInfo.CommandMetadata.HelpUri);
					}
					else if (!string.IsNullOrEmpty(text2))
					{
						DefaultCommandHelpObjectBuilder.AddRelatedLinksProperties(helpInfo.FullHelp, text2);
					}
				}
				if (flag && helpInfo.FullHelp.Properties["PSSnapIn"] == null)
				{
					helpInfo.FullHelp.Properties.Add(new PSNoteProperty("PSSnapIn", cmdletInfo.PSSnapIn));
				}
				if (helpInfo.FullHelp.Properties["ModuleName"] == null)
				{
					helpInfo.FullHelp.Properties.Add(new PSNoteProperty("ModuleName", commandInfo.ModuleName));
				}
			}
			return helpInfo;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0007E5AC File Offset: 0x0007C7AC
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			int countHelpInfosFound = 0;
			string target = helpRequest.Target;
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			CommandSearcher searcher = this.GetCommandSearcherForExactMatch(target, this._context);
			while (searcher.MoveNext())
			{
				CommandInfo current = ((IEnumerator<CommandInfo>)searcher).Current;
				if (SessionState.IsVisible(helpRequest.CommandOrigin, current))
				{
					HelpInfo helpInfo = this.GetHelpInfo(current, true, false);
					string helpName = this.GetHelpName(current);
					if (helpInfo != null && !string.IsNullOrEmpty(helpName))
					{
						if (helpInfo.ForwardHelpCategory == helpRequest.HelpCategory && helpInfo.ForwardTarget.Equals(helpRequest.Target, StringComparison.OrdinalIgnoreCase))
						{
							throw new PSInvalidOperationException(HelpErrors.CircularDependencyInHelpForwarding);
						}
						if (!hashtable.ContainsKey(helpName) && CommandHelpProvider.Match(helpInfo, helpRequest, current))
						{
							countHelpInfosFound++;
							hashtable.Add(helpName, null);
							yield return helpInfo;
							if (countHelpInfosFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
							{
								break;
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0007E5D0 File Offset: 0x0007C7D0
		private static string GetCmdletAssemblyPath(CmdletInfo cmdletInfo)
		{
			if (cmdletInfo == null)
			{
				return null;
			}
			if (cmdletInfo.ImplementingType == null)
			{
				return null;
			}
			return Path.GetDirectoryName(ClrFacade.GetAssemblyLocation(cmdletInfo.ImplementingType.GetTypeInfo().Assembly));
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0007E604 File Offset: 0x0007C804
		private string FindHelpFile(CmdletInfo cmdletInfo)
		{
			if (cmdletInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdletInfo");
			}
			string helpFile = cmdletInfo.HelpFile;
			if (!string.IsNullOrEmpty(helpFile))
			{
				string text = helpFile;
				PSSnapInInfo pssnapIn = cmdletInfo.PSSnapIn;
				Collection<string> collection = new Collection<string>();
				if (!File.Exists(text))
				{
					text = Path.GetFileName(text);
					if (pssnapIn != null)
					{
						text = Path.Combine(pssnapIn.ApplicationBase, helpFile);
					}
					else if (cmdletInfo.Module != null && !string.IsNullOrEmpty(cmdletInfo.Module.Path))
					{
						text = Path.Combine(cmdletInfo.Module.ModuleBase, helpFile);
					}
					else
					{
						collection.Add(base.GetDefaultShellSearchPath());
						collection.Add(CommandHelpProvider.GetCmdletAssemblyPath(cmdletInfo));
					}
				}
				else
				{
					text = Path.GetFullPath(text);
				}
				string text2 = MUIFileSearcher.LocateFile(text, collection);
				if (string.IsNullOrEmpty(text2))
				{
					CommandHelpProvider.tracer.WriteLine("Unable to load file {0}", new object[]
					{
						text
					});
				}
				return text2;
			}
			if (cmdletInfo.Module != null && InitialSessionState.IsEngineModule(cmdletInfo.Module.Name))
			{
				return Path.Combine(cmdletInfo.Module.ModuleBase, CultureInfo.CurrentCulture.Name, CommandHelpProvider.engineModuleHelpFileCache[cmdletInfo.Module.Name]);
			}
			return helpFile;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0007E72C File Offset: 0x0007C92C
		private void LoadHelpFile(string helpFile, string helpFileIdentifier, string commandName, bool reportErrors)
		{
			Exception ex = null;
			try
			{
				this.LoadHelpFile(helpFile, helpFileIdentifier);
			}
			catch (IOException ex2)
			{
				ex = ex2;
			}
			catch (SecurityException ex3)
			{
				ex = ex3;
			}
			catch (XmlException ex4)
			{
				ex = ex4;
			}
			catch (NotSupportedException ex5)
			{
				ex = ex5;
			}
			catch (UnauthorizedAccessException ex6)
			{
				ex = ex6;
			}
			catch (InvalidOperationException ex7)
			{
				ex = ex7;
			}
			if (reportErrors && ex != null)
			{
				base.ReportHelpFileError(ex, commandName, helpFile);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0007E7C8 File Offset: 0x0007C9C8
		private void LoadHelpFile(string helpFile, string helpFileIdentifier)
		{
			XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(new FileInfo(helpFile), false, null);
			this._helpFiles[helpFile] = 0;
			XmlNode xmlNode = null;
			if (xmlDocument.HasChildNodes)
			{
				for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlDocument.ChildNodes[i];
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.LocalName, "helpItems", StringComparison.OrdinalIgnoreCase) == 0)
					{
						xmlNode = xmlNode2;
						break;
					}
				}
			}
			if (xmlNode == null)
			{
				CommandHelpProvider.tracer.WriteLine("Unable to find 'helpItems' element in file {0}", new object[]
				{
					helpFile
				});
				return;
			}
			bool flag = CommandHelpProvider.IsMamlHelp(helpFile, xmlNode);
			using (base.HelpSystem.Trace(helpFile))
			{
				if (xmlNode.HasChildNodes)
				{
					for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
					{
						XmlNode xmlNode3 = xmlNode.ChildNodes[j];
						if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.LocalName, "command", StringComparison.OrdinalIgnoreCase) == 0)
						{
							MamlCommandHelpInfo mamlCommandHelpInfo = null;
							if (flag)
							{
								mamlCommandHelpInfo = MamlCommandHelpInfo.Load(xmlNode3, HelpCategory.Cmdlet);
							}
							if (mamlCommandHelpInfo != null)
							{
								base.HelpSystem.TraceErrors(mamlCommandHelpInfo.Errors);
								this.AddToCommandCache(helpFileIdentifier, mamlCommandHelpInfo.Name, mamlCommandHelpInfo);
							}
						}
						if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "UserDefinedData", StringComparison.OrdinalIgnoreCase) == 0)
						{
							UserDefinedHelpData userDefinedHelpData = UserDefinedHelpData.Load(xmlNode3);
							this.ProcessUserDefineddHelpData(helpFileIdentifier, userDefinedHelpData);
						}
					}
				}
			}
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0007E964 File Offset: 0x0007CB64
		private void ProcessUserDefineddHelpData(string mshSnapInId, UserDefinedHelpData userDefinedHelpData)
		{
			if (userDefinedHelpData == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(userDefinedHelpData.Name))
			{
				return;
			}
			HelpInfo fromCommandCache = this.GetFromCommandCache(mshSnapInId, userDefinedHelpData.Name, HelpCategory.Cmdlet);
			if (fromCommandCache == null)
			{
				return;
			}
			MamlCommandHelpInfo mamlCommandHelpInfo = fromCommandCache as MamlCommandHelpInfo;
			if (mamlCommandHelpInfo == null)
			{
				return;
			}
			mamlCommandHelpInfo.AddUserDefinedData(userDefinedHelpData);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0007E9A8 File Offset: 0x0007CBA8
		private HelpInfo GetFromCommandCache(string helpFileIdentifier, string commandName, HelpCategory helpCategory)
		{
			string text = commandName;
			if (!string.IsNullOrEmpty(helpFileIdentifier))
			{
				text = helpFileIdentifier + "\\" + text;
			}
			HelpInfo helpInfo = base.GetCache(text);
			if (helpInfo != null && helpInfo.HelpCategory != helpCategory)
			{
				MamlCommandHelpInfo mamlCommandHelpInfo = (MamlCommandHelpInfo)helpInfo;
				helpInfo = mamlCommandHelpInfo.Copy(helpCategory);
			}
			return helpInfo;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0007E9F0 File Offset: 0x0007CBF0
		private HelpInfo GetFromCommandCache(string helpFileIdentifier, CommandInfo commandInfo)
		{
			HelpInfo fromCommandCache = this.GetFromCommandCache(helpFileIdentifier, commandInfo.Name, commandInfo.HelpCategory);
			if (fromCommandCache == null && commandInfo.Module != null && !string.IsNullOrEmpty(commandInfo.Prefix))
			{
				MamlCommandHelpInfo fromCommandCacheByRemovingPrefix = this.GetFromCommandCacheByRemovingPrefix(helpFileIdentifier, commandInfo);
				if (fromCommandCacheByRemovingPrefix != null)
				{
					this.AddToCommandCache(helpFileIdentifier, commandInfo.Name, fromCommandCacheByRemovingPrefix);
					return fromCommandCacheByRemovingPrefix;
				}
			}
			return fromCommandCache;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0007EA48 File Offset: 0x0007CC48
		private HelpInfo GetFromCommandCacheOrCmdletInfo(CmdletInfo cmdletInfo)
		{
			HelpInfo fromCommandCache = this.GetFromCommandCache(cmdletInfo.ModuleName, cmdletInfo.Name, cmdletInfo.HelpCategory);
			if (fromCommandCache == null && cmdletInfo.Module != null && !string.IsNullOrEmpty(cmdletInfo.Prefix))
			{
				MamlCommandHelpInfo fromCommandCacheByRemovingPrefix = this.GetFromCommandCacheByRemovingPrefix(cmdletInfo.ModuleName, cmdletInfo);
				if (fromCommandCacheByRemovingPrefix != null)
				{
					if (fromCommandCacheByRemovingPrefix.FullHelp.Properties["Details"] != null && fromCommandCacheByRemovingPrefix.FullHelp.Properties["Details"].Value != null)
					{
						PSObject psobject = PSObject.AsPSObject(fromCommandCacheByRemovingPrefix.FullHelp.Properties["Details"].Value);
						if (psobject.Properties["Noun"] != null)
						{
							psobject.Properties.Remove("Noun");
						}
						psobject.Properties.Add(new PSNoteProperty("Noun", cmdletInfo.Noun));
					}
					this.AddToCommandCache(cmdletInfo.ModuleName, cmdletInfo.Name, fromCommandCacheByRemovingPrefix);
					return fromCommandCacheByRemovingPrefix;
				}
			}
			return fromCommandCache;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0007EB4C File Offset: 0x0007CD4C
		private MamlCommandHelpInfo GetFromCommandCacheByRemovingPrefix(string helpIdentifier, CommandInfo cmdInfo)
		{
			MamlCommandHelpInfo mamlCommandHelpInfo = null;
			MamlCommandHelpInfo mamlCommandHelpInfo2 = this.GetFromCommandCache(helpIdentifier, ModuleCmdletBase.RemovePrefixFromCommandName(cmdInfo.Name, cmdInfo.Prefix), cmdInfo.HelpCategory) as MamlCommandHelpInfo;
			if (mamlCommandHelpInfo2 != null)
			{
				mamlCommandHelpInfo = mamlCommandHelpInfo2.Copy();
				if (mamlCommandHelpInfo.FullHelp.Properties["Name"] != null)
				{
					mamlCommandHelpInfo.FullHelp.Properties.Remove("Name");
				}
				mamlCommandHelpInfo.FullHelp.Properties.Add(new PSNoteProperty("Name", cmdInfo.Name));
				if (mamlCommandHelpInfo.FullHelp.Properties["Details"] != null && mamlCommandHelpInfo.FullHelp.Properties["Details"].Value != null)
				{
					PSObject psobject = PSObject.AsPSObject(mamlCommandHelpInfo.FullHelp.Properties["Details"].Value).Copy();
					if (psobject.Properties["Name"] != null)
					{
						psobject.Properties.Remove("Name");
					}
					psobject.Properties.Add(new PSNoteProperty("Name", cmdInfo.Name));
					mamlCommandHelpInfo.FullHelp.Properties["Details"].Value = psobject;
				}
			}
			return mamlCommandHelpInfo;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0007EC8C File Offset: 0x0007CE8C
		private void AddToCommandCache(string mshSnapInId, string cmdletName, MamlCommandHelpInfo helpInfo)
		{
			string text = cmdletName;
			helpInfo.FullHelp.TypeNames.Insert(0, string.Format(CultureInfo.InvariantCulture, "MamlCommandHelpInfo#{0}#{1}", new object[]
			{
				mshSnapInId,
				cmdletName
			}));
			if (!string.IsNullOrEmpty(mshSnapInId))
			{
				text = mshSnapInId + "\\" + text;
				helpInfo.FullHelp.TypeNames.Insert(1, string.Format(CultureInfo.InvariantCulture, "MamlCommandHelpInfo#{0}", new object[]
				{
					mshSnapInId
				}));
			}
			base.AddCache(text, helpInfo);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0007ED14 File Offset: 0x0007CF14
		internal static bool IsMamlHelp(string helpFile, XmlNode helpItemsNode)
		{
			if (helpFile.EndsWith(".maml", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			if (helpItemsNode.Attributes == null)
			{
				return false;
			}
			foreach (object obj in helpItemsNode.Attributes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name.Equals("schema", StringComparison.OrdinalIgnoreCase) && xmlNode.Value.Equals("maml", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0007F448 File Offset: 0x0007D648
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			string target = helpRequest.Target;
			Collection<string> patternList = new Collection<string>();
			WildcardPattern wildCardPattern = null;
			bool decoratedSearch = !WildcardPattern.ContainsWildcardCharacters(helpRequest.Target);
			if (!searchOnlyContent)
			{
				if (decoratedSearch)
				{
					if (target.IndexOf('-') >= 0)
					{
						patternList.Add(target + "*");
					}
					else
					{
						patternList.Add("*" + target + "*");
					}
				}
				else
				{
					patternList.Add(target);
				}
			}
			else
			{
				patternList.Add("*");
				string pattern2 = helpRequest.Target;
				if (decoratedSearch)
				{
					pattern2 = "*" + helpRequest.Target + "*";
				}
				wildCardPattern = new WildcardPattern(pattern2, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			}
			int countOfHelpInfoObjectsFound = 0;
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			Hashtable hiddenCommands = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (string pattern in patternList)
			{
				CommandSearcher searcher = this.GetCommandSearcherForSearch(pattern, this._context);
				while (searcher.MoveNext())
				{
					if (this._context.CurrentPipelineStopping)
					{
						yield break;
					}
					CommandInfo current = ((IEnumerator<CommandInfo>)searcher).Current;
					HelpInfo helpInfo = this.GetHelpInfo(current, !decoratedSearch, searchOnlyContent);
					string helpName = this.GetHelpName(current);
					if (helpInfo != null && !string.IsNullOrEmpty(helpName))
					{
						if (!SessionState.IsVisible(helpRequest.CommandOrigin, current))
						{
							if (!hiddenCommands.ContainsKey(helpName))
							{
								hiddenCommands.Add(helpName, null);
							}
						}
						else if (!hashtable.ContainsKey(helpName) && CommandHelpProvider.Match(helpInfo, helpRequest, current) && (!searchOnlyContent || helpInfo.MatchPatternInContent(wildCardPattern)))
						{
							hashtable.Add(helpName, null);
							countOfHelpInfoObjectsFound++;
							yield return helpInfo;
							if (countOfHelpInfoObjectsFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
							{
								yield break;
							}
						}
					}
				}
				if (this.HelpCategory == (HelpCategory.Alias | HelpCategory.Cmdlet))
				{
					foreach (CommandInfo current2 in ModuleUtils.GetMatchingCommands(pattern, this._context, helpRequest.CommandOrigin, false, false))
					{
						if (this._context.CurrentPipelineStopping)
						{
							yield break;
						}
						if (SessionState.IsVisible(helpRequest.CommandOrigin, current2))
						{
							HelpInfo helpInfo2 = this.GetHelpInfo(current2, !decoratedSearch, searchOnlyContent);
							string helpName2 = this.GetHelpName(current2);
							if (helpInfo2 != null && !string.IsNullOrEmpty(helpName2) && !hashtable.ContainsKey(helpName2) && !hiddenCommands.ContainsKey(helpName2) && CommandHelpProvider.Match(helpInfo2, helpRequest, current2) && (!searchOnlyContent || helpInfo2.MatchPatternInContent(wildCardPattern)))
							{
								hashtable.Add(helpName2, null);
								countOfHelpInfoObjectsFound++;
								yield return helpInfo2;
								if (countOfHelpInfoObjectsFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
								{
									yield break;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0007F474 File Offset: 0x0007D674
		private static bool Match(HelpInfo helpInfo, HelpRequest helpRequest, CommandInfo commandInfo)
		{
			return helpRequest == null || ((helpRequest.HelpCategory & commandInfo.HelpCategory) != HelpCategory.None && helpInfo is BaseCommandHelpInfo && CommandHelpProvider.Match(helpInfo.Component, helpRequest.Component) && CommandHelpProvider.Match(helpInfo.Role, helpRequest.Role) && CommandHelpProvider.Match(helpInfo.Functionality, helpRequest.Functionality));
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0007F4E4 File Offset: 0x0007D6E4
		private static bool Match(string target, string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				return true;
			}
			if (string.IsNullOrEmpty(target))
			{
				target = "";
			}
			WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			return wildcardPattern.IsMatch(target);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0007F51C File Offset: 0x0007D71C
		private static bool Match(string target, ICollection<string> patterns)
		{
			if (patterns == null || patterns.Count == 0)
			{
				return true;
			}
			foreach (string pattern in patterns)
			{
				if (CommandHelpProvider.Match(target, pattern))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0007F830 File Offset: 0x0007DA30
		internal override IEnumerable<HelpInfo> ProcessForwardedHelp(HelpInfo helpInfo, HelpRequest helpRequest)
		{
			HelpCategory categoriesHandled = HelpCategory.Alias | HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow;
			if ((helpInfo.HelpCategory & categoriesHandled) != HelpCategory.None)
			{
				HelpRequest commandHelpRequest = helpRequest.Clone();
				commandHelpRequest.Target = helpInfo.ForwardTarget;
				commandHelpRequest.CommandOrigin = CommandOrigin.Internal;
				if (helpInfo.ForwardHelpCategory != HelpCategory.None && helpInfo.HelpCategory != HelpCategory.Alias)
				{
					commandHelpRequest.HelpCategory = helpInfo.ForwardHelpCategory;
				}
				else
				{
					try
					{
						CommandInfo commandInfo = this._context.CommandDiscovery.LookupCommandInfo(commandHelpRequest.Target);
						commandHelpRequest.HelpCategory = commandInfo.HelpCategory;
					}
					catch (CommandNotFoundException)
					{
					}
				}
				foreach (HelpInfo helpInfoToReturn in this.ExactMatchHelp(commandHelpRequest))
				{
					yield return helpInfoToReturn;
				}
			}
			else
			{
				yield return helpInfo;
			}
			yield break;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0007F85B File Offset: 0x0007DA5B
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0007F870 File Offset: 0x0007DA70
		internal virtual CommandSearcher GetCommandSearcherForExactMatch(string commandName, ExecutionContext context)
		{
			return new CommandSearcher(commandName, SearchResolutionOptions.None, CommandTypes.Cmdlet, context);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0007F888 File Offset: 0x0007DA88
		internal virtual CommandSearcher GetCommandSearcherForSearch(string pattern, ExecutionContext context)
		{
			return new CommandSearcher(pattern, SearchResolutionOptions.CommandNameIsPattern, CommandTypes.Cmdlet, context);
		}

		// Token: 0x040008D7 RID: 2263
		private static Dictionary<string, string> engineModuleHelpFileCache = new Dictionary<string, string>();

		// Token: 0x040008D8 RID: 2264
		private readonly ExecutionContext _context;

		// Token: 0x040008D9 RID: 2265
		private readonly Hashtable _helpFiles = new Hashtable();

		// Token: 0x040008DA RID: 2266
		[TraceSource("CommandHelpProvider", "CommandHelpProvider")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("CommandHelpProvider", "CommandHelpProvider");
	}
}
