using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Cim;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200097B RID: 2427
	public static class CompletionCompleters
	{
		// Token: 0x0600594A RID: 22858 RVA: 0x001D5C78 File Offset: 0x001D3E78
		static CompletionCompleters()
		{
			AppDomain.CurrentDomain.AssemblyLoad += CompletionCompleters.UpdateTypeCacheOnAssemblyLoad;
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x001D5E0C File Offset: 0x001D400C
		public static IEnumerable<CompletionResult> CompleteCommand(string commandName)
		{
			return CompletionCompleters.CompleteCommand(commandName, null, CommandTypes.All);
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x001D5E1C File Offset: 0x001D401C
		public static IEnumerable<CompletionResult> CompleteCommand(string commandName, string moduleName, CommandTypes commandTypes = CommandTypes.All)
		{
			if (Runspace.DefaultRunspace == null)
			{
				return CommandCompletion.EmptyCompletionResult;
			}
			CompletionExecutionHelper helper = new CompletionExecutionHelper(PowerShell.Create(RunspaceMode.CurrentRunspace));
			return CompletionCompleters.CompleteCommand(new CompletionContext
			{
				WordToComplete = commandName,
				Helper = helper
			}, moduleName, commandTypes);
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x001D5E60 File Offset: 0x001D4060
		internal static List<CompletionResult> CompleteCommand(CompletionContext context)
		{
			return CompletionCompleters.CompleteCommand(context, null, CommandTypes.All);
		}

		// Token: 0x0600594E RID: 22862 RVA: 0x001D5EA0 File Offset: 0x001D40A0
		private static List<CompletionResult> CompleteCommand(CompletionContext context, string moduleName, CommandTypes types = CommandTypes.All)
		{
			bool addAmpersandIfNecessary = CompletionCompleters.IsAmpersandNeeded(context, false);
			string text = context.WordToComplete;
			string quote = CompletionCompleters.HandleDoubleAndSingleQuote(ref text);
			text += "*";
			List<CompletionResult> list = null;
			if (text.IndexOfAny(new char[]
			{
				'/',
				'\\',
				':'
			}) == -1)
			{
				Ast ast = null;
				if (context.RelatedAsts != null && context.RelatedAsts.Count > 0)
				{
					ast = context.RelatedAsts.Last<Ast>();
				}
				PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-Command", typeof(GetCommandCommand)).AddParameter("All").AddParameter("Name", text);
				if (moduleName != null)
				{
					currentPowerShell.AddParameter("Module", moduleName);
				}
				if (!types.Equals(CommandTypes.All))
				{
					currentPowerShell.AddParameter("CommandType", types);
				}
				Exception ex;
				Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (ast != null)
				{
					collection = CompletionCompleters.CompleteWorkflowCommand(text, ast, collection);
				}
				if (collection != null && collection.Count > 1)
				{
					IOrderedEnumerable<PSObject> commandInfoPsObjs = collection.OrderBy((PSObject a) => a, new CompletionCompleters.CommandNameComparer());
					list = CompletionCompleters.MakeCommandsUnique(commandInfoPsObjs, false, addAmpersandIfNecessary, quote, context);
				}
				else
				{
					list = CompletionCompleters.MakeCommandsUnique(collection, false, addAmpersandIfNecessary, quote, context);
				}
				if (ast == null)
				{
					return list;
				}
				CompletionCompleters.FindFunctionsVisitor findFunctionsVisitor = new CompletionCompleters.FindFunctionsVisitor();
				while (ast.Parent != null)
				{
					ast = ast.Parent;
				}
				ast.Visit(findFunctionsVisitor);
				WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
				using (List<FunctionDefinitionAst>.Enumerator enumerator = findFunctionsVisitor.FunctionDefinitions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FunctionDefinitionAst defn = enumerator.Current;
						if (wildcardPattern.IsMatch(defn.Name))
						{
							if (!(from cr in list
							where cr.CompletionText.Equals(defn.Name, StringComparison.OrdinalIgnoreCase)
							select cr).Any<CompletionResult>())
							{
								list.Insert(0, CompletionCompleters.GetCommandNameCompletionResult(defn.Name, defn, addAmpersandIfNecessary, quote));
							}
						}
					}
					return list;
				}
			}
			int num = text.IndexOf(':');
			int num2 = text.IndexOf('\\');
			if (num2 > 0 && (num2 < num || num == -1))
			{
				moduleName = text.Substring(0, num2);
				text = text.Substring(num2 + 1);
				PowerShell currentPowerShell2 = context.Helper.CurrentPowerShell;
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell2, "Get-Command", typeof(GetCommandCommand)).AddParameter("All").AddParameter("Name", text).AddParameter("Module", moduleName);
				if (!types.Equals(CommandTypes.All))
				{
					currentPowerShell2.AddParameter("CommandType", types);
				}
				Exception ex2;
				Collection<PSObject> collection2 = context.Helper.ExecuteCurrentPowerShell(out ex2, null);
				if (collection2 != null && collection2.Count > 1)
				{
					IOrderedEnumerable<PSObject> commandInfoPsObjs2 = collection2.OrderBy((PSObject a) => a, new CompletionCompleters.CommandNameComparer());
					list = CompletionCompleters.MakeCommandsUnique(commandInfoPsObjs2, true, addAmpersandIfNecessary, quote, context);
				}
				else
				{
					list = CompletionCompleters.MakeCommandsUnique(collection2, true, addAmpersandIfNecessary, quote, context);
				}
			}
			return list;
		}

		// Token: 0x0600594F RID: 22863 RVA: 0x001D61F0 File Offset: 0x001D43F0
		internal static CompletionResult GetCommandNameCompletionResult(string name, object command, bool addAmpersandIfNecessary, string quote)
		{
			string text = name;
			string listItemText = name;
			CommandInfo commandInfo = command as CommandInfo;
			if (commandInfo != null)
			{
				try
				{
					listItemText = commandInfo.Name;
					text = commandInfo.Syntax;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			text = (string.IsNullOrEmpty(text) ? name : text);
			bool flag;
			if (CompletionCompleters.CompletionRequiresQuotes(name, false))
			{
				flag = (quote == string.Empty && addAmpersandIfNecessary);
				string text2 = (quote == string.Empty) ? "'" : quote;
				if (text2 == "'")
				{
					name = name.Replace("'", "''");
				}
				else
				{
					name = name.Replace("`", "``");
					name = name.Replace("$", "`$");
				}
				name = text2 + name + text2;
			}
			else
			{
				flag = (quote == string.Empty && addAmpersandIfNecessary && Tokenizer.IsKeyword(name) && !CompletionCompleters.KeywordsToExcludeFromAddingAmpersand.Contains(name));
				name = quote + name + quote;
			}
			if (flag && name != "foreach")
			{
				name = "& " + name;
			}
			return new CompletionResult(name, listItemText, CompletionResultType.Command, text);
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x001D6324 File Offset: 0x001D4524
		internal static List<CompletionResult> MakeCommandsUnique(IEnumerable<PSObject> commandInfoPsObjs, bool includeModulePrefix, bool addAmpersandIfNecessary, string quote, CompletionContext context)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			if (commandInfoPsObjs == null || !commandInfoPsObjs.Any<PSObject>())
			{
				return list;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			foreach (PSObject obj in commandInfoPsObjs)
			{
				object obj2 = PSObject.Base(obj);
				CommandInfo commandInfo = obj2 as CommandInfo;
				string text;
				if (commandInfo != null)
				{
					if (commandInfo.Visibility == SessionStateEntryVisibility.Private)
					{
						continue;
					}
					text = commandInfo.Name;
					if (includeModulePrefix && !string.IsNullOrEmpty(commandInfo.ModuleName) && (string.IsNullOrEmpty(commandInfo.Prefix) || !ModuleCmdletBase.IsPrefixedCommand(commandInfo)))
					{
						text = commandInfo.ModuleName + "\\" + commandInfo.Name;
					}
				}
				else
				{
					text = (obj2 as string);
					if (text == null)
					{
						continue;
					}
				}
				object obj3;
				if (!dictionary.TryGetValue(text, out obj3))
				{
					dictionary.Add(text, obj2);
				}
				else
				{
					List<object> list2 = obj3 as List<object>;
					if (list2 != null)
					{
						list2.Add(obj2);
					}
					else
					{
						list2 = new List<object>
						{
							obj3,
							obj2
						};
						dictionary[text] = list2;
					}
				}
			}
			List<CompletionResult> list3 = null;
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				List<object> list4 = keyValuePair.Value as List<object>;
				if (list4 != null)
				{
					if (list3 == null)
					{
						list3 = new List<CompletionResult>();
					}
					string text2 = keyValuePair.Key;
					if (!includeModulePrefix)
					{
						CommandInfo commandInfo2 = list4[0] as CommandInfo;
						if (commandInfo2 != null && !string.IsNullOrEmpty(commandInfo2.Prefix) && !ModuleCmdletBase.IsPrefixedCommand(commandInfo2))
						{
							text2 = commandInfo2.ModuleName + "\\" + text2;
						}
					}
					list.Add(CompletionCompleters.GetCommandNameCompletionResult(text2, list4[0], addAmpersandIfNecessary, quote));
					for (int i = 1; i < list4.Count; i++)
					{
						CommandInfo commandInfo3 = list4[i] as CommandInfo;
						if (commandInfo3 != null)
						{
							if (commandInfo3.CommandType == CommandTypes.Application)
							{
								list3.Add(CompletionCompleters.GetCommandNameCompletionResult(commandInfo3.Definition, commandInfo3, addAmpersandIfNecessary, quote));
							}
							else if (!string.IsNullOrEmpty(commandInfo3.ModuleName))
							{
								string name = commandInfo3.ModuleName + "\\" + commandInfo3.Name;
								list3.Add(CompletionCompleters.GetCommandNameCompletionResult(name, commandInfo3, addAmpersandIfNecessary, quote));
							}
						}
					}
				}
				else
				{
					string text3 = keyValuePair.Key;
					if (!includeModulePrefix)
					{
						CommandInfo commandInfo4 = keyValuePair.Value as CommandInfo;
						if (commandInfo4 != null && !string.IsNullOrEmpty(commandInfo4.Prefix) && !ModuleCmdletBase.IsPrefixedCommand(commandInfo4))
						{
							text3 = commandInfo4.ModuleName + "\\" + text3;
						}
					}
					list.Add(CompletionCompleters.GetCommandNameCompletionResult(text3, keyValuePair.Value, addAmpersandIfNecessary, quote));
				}
			}
			if (list3 != null && list3.Count > 0)
			{
				list.AddRange(list3);
			}
			return list;
		}

		// Token: 0x06005951 RID: 22865 RVA: 0x001D6640 File Offset: 0x001D4840
		private static Collection<PSObject> CompleteWorkflowCommand(string command, Ast lastAst, Collection<PSObject> commandInfos)
		{
			if (!lastAst.IsInWorkflow())
			{
				return commandInfos;
			}
			commandInfos = (commandInfos ?? new Collection<PSObject>());
			WildcardPattern wildcardPattern = new WildcardPattern(command, WildcardOptions.IgnoreCase);
			foreach (string text in CompletionCompleters.PseudoWorkflowCommands)
			{
				if (wildcardPattern.IsMatch(text))
				{
					commandInfos.Add(PSObject.AsPSObject(text));
				}
			}
			return commandInfos;
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x001D66C0 File Offset: 0x001D48C0
		internal static List<CompletionResult> CompleteModuleName(CompletionContext context, bool loadedModulesOnly)
		{
			string text = context.WordToComplete ?? string.Empty;
			List<CompletionResult> list = new List<CompletionResult>();
			string text2 = CompletionCompleters.HandleDoubleAndSingleQuote(ref text);
			if (!text.EndsWith("*", StringComparison.Ordinal))
			{
				text += "*";
			}
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-Module", typeof(GetModuleCommand)).AddParameter("Name", text);
			if (!loadedModulesOnly)
			{
				currentPowerShell.AddParameter("ListAvailable", true);
			}
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection != null)
			{
				foreach (object arg in collection)
				{
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Sitef == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Sitef = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Sitef.Target;
					CallSite <>p__Sitef = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Sitef;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site10 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site10 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj = target(<>p__Sitef, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site10.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site10, arg));
					object arg2 = obj;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site11 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site11 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object, object> target2 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site11.Target;
					CallSite <>p__Site = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site11;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site12 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site12 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, string, object> target3 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site12.Target;
					CallSite <>p__Site2 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site12;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site13 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site13 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object, object> target4 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site13.Target;
					CallSite <>p__Site3 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site13;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site14 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site14 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, string, object> target5 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site14.Target;
					CallSite <>p__Site4 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site14;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site15 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site15 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, string, object, object> target6 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site15.Target;
					CallSite <>p__Site5 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site15;
					string arg3 = "Description: ";
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site16 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site16 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target7 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site16.Target;
					CallSite <>p__Site6 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site16;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site17 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site17 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Description", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object arg4 = target5(<>p__Site4, target6(<>p__Site5, arg3, target7(<>p__Site6, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site17.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site17, arg))), "\r\nModuleType: ");
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site18 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site18 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target8 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site18.Target;
					CallSite <>p__Site7 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site18;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site19 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site19 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "ModuleType", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object arg5 = target3(<>p__Site2, target4(<>p__Site3, arg4, target8(<>p__Site7, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site19.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site19, arg))), "\r\nPath: ");
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1a == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1a = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target9 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1a.Target;
					CallSite <>p__Site1a = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1a;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1b == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1b = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Path", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object arg6 = target2(<>p__Site, arg5, target9(<>p__Site1a, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1b.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1b, arg)));
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1c == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1c = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target10 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1c.Target;
					CallSite <>p__Site1c = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1c;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1d == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1d = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					if (target10(<>p__Site1c, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1d.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1d, typeof(CompletionCompleters), obj, false)))
					{
						string text3 = (text2 == string.Empty) ? "'" : text2;
						if (text3 == "'")
						{
							if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1e == null)
							{
								CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1e = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							obj = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1e.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1e, obj, "'", "''");
						}
						if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1f == null)
						{
							CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1f = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target11 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1f.Target;
						CallSite <>p__Site1f = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site1f;
						if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site20 == null)
						{
							CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site20 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target11(<>p__Site1f, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site20.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site20, text3, obj), text3);
					}
					else
					{
						if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site21 == null)
						{
							CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site21 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target12 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site21.Target;
						CallSite <>p__Site8 = CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site21;
						if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site22 == null)
						{
							CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site22 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target12(<>p__Site8, CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site22.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site22, text2, obj), text2);
					}
					List<CompletionResult> list2 = list;
					if (CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site23 == null)
					{
						CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site23 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					list2.Add(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site23.Target(CompletionCompleters.<CompleteModuleName>o__SiteContainere.<>p__Site23, typeof(CompletionResult), obj, arg2, CompletionResultType.ParameterValue, arg6));
				}
			}
			return list;
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x001D6F64 File Offset: 0x001D5164
		internal static List<CompletionResult> CompleteCommandParameter(CompletionContext context)
		{
			bool withColon = false;
			List<CompletionResult> list = new List<CompletionResult>();
			CommandParameterAst commandParameterAst = null;
			DynamicKeywordStatementAst dynamicKeywordStatementAst = null;
			for (int i = context.RelatedAsts.Count - 1; i >= 0; i--)
			{
				if (dynamicKeywordStatementAst == null)
				{
					dynamicKeywordStatementAst = (context.RelatedAsts[i] as DynamicKeywordStatementAst);
				}
				commandParameterAst = (context.RelatedAsts[i] as CommandParameterAst);
				if (commandParameterAst != null)
				{
					break;
				}
			}
			if (commandParameterAst != null)
			{
				dynamicKeywordStatementAst = (commandParameterAst.Parent as DynamicKeywordStatementAst);
			}
			if (dynamicKeywordStatementAst != null && string.Equals(dynamicKeywordStatementAst.Keyword.Keyword, "Import-DscResource", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(context.WordToComplete) && context.WordToComplete.StartsWith("-", StringComparison.OrdinalIgnoreCase))
			{
				Ast ast2 = context.RelatedAsts.Last<Ast>();
				string pattern = context.WordToComplete.Substring(1) + "*";
				WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				IEnumerable<string> source = from ast in dynamicKeywordStatementAst.CommandElements
				where ast is CommandParameterAst
				select (ast as CommandParameterAst).ParameterName;
				foreach (string text in CompletionCompleters.ParameterNamesOfImportDSCResource)
				{
					if (wildcardPattern.IsMatch(text) && !source.Contains(text, StringComparer.OrdinalIgnoreCase))
					{
						string toolTip = "[String] " + text;
						list.Add(new CompletionResult("-" + text, text, CompletionResultType.ParameterName, toolTip));
					}
				}
				if (list.Count > 0)
				{
					context.ReplacementLength = context.WordToComplete.Length;
					context.ReplacementIndex = ast2.Extent.StartOffset;
				}
				return list;
			}
			CommandAst command;
			string parameterName;
			if (commandParameterAst != null)
			{
				command = (CommandAst)commandParameterAst.Parent;
				parameterName = commandParameterAst.ParameterName;
				withColon = context.WordToComplete.EndsWith(":", StringComparison.Ordinal);
			}
			else
			{
				StringConstantExpressionAst stringConstantExpressionAst = context.RelatedAsts[context.RelatedAsts.Count - 1] as StringConstantExpressionAst;
				if (stringConstantExpressionAst == null)
				{
					return list;
				}
				if (!stringConstantExpressionAst.Value.Trim().Equals("-", StringComparison.OrdinalIgnoreCase))
				{
					return list;
				}
				command = (CommandAst)stringConstantExpressionAst.Parent;
				parameterName = string.Empty;
			}
			PseudoBindingInfo pseudoBindingInfo = new PseudoParameterBinder().DoPseudoParameterBinding(command, null, commandParameterAst, PseudoParameterBinder.BindingType.ParameterCompletion);
			if (pseudoBindingInfo == null)
			{
				return list;
			}
			switch (pseudoBindingInfo.InfoType)
			{
			case PseudoBindingInfoType.PseudoBindingFail:
				list = CompletionCompleters.GetParameterCompletionResults(parameterName, uint.MaxValue, pseudoBindingInfo.UnboundParameters, withColon);
				break;
			case PseudoBindingInfoType.PseudoBindingSucceed:
				list = CompletionCompleters.GetParameterCompletionResults(parameterName, pseudoBindingInfo, commandParameterAst, withColon);
				break;
			}
			if (list.Count == 0)
			{
				list = (pseudoBindingInfo.CommandName.Equals("Set-Location", StringComparison.OrdinalIgnoreCase) ? new List<CompletionResult>(CompletionCompleters.CompleteFilename(context, true, null)) : new List<CompletionResult>(CompletionCompleters.CompleteFilename(context)));
			}
			return list;
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x001D72C4 File Offset: 0x001D54C4
		private static List<CompletionResult> GetParameterCompletionResults(string parameterName, PseudoBindingInfo bindingInfo, CommandParameterAst parameterAst, bool withColon)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			if (parameterName == string.Empty)
			{
				list = CompletionCompleters.GetParameterCompletionResults(parameterName, bindingInfo.ValidParameterSetsFlags, bindingInfo.UnboundParameters, withColon);
				return list;
			}
			if (bindingInfo.ParametersNotFound.Count > 0)
			{
				if (bindingInfo.ParametersNotFound.Any((CommandParameterAst pAst) => parameterAst.GetHashCode() == pAst.GetHashCode()))
				{
					return list;
				}
			}
			if (bindingInfo.AmbiguousParameters.Count > 0)
			{
				if (bindingInfo.AmbiguousParameters.Any((CommandParameterAst pAst) => parameterAst.GetHashCode() == pAst.GetHashCode()))
				{
					list = CompletionCompleters.GetParameterCompletionResults(parameterName, bindingInfo.ValidParameterSetsFlags, bindingInfo.UnboundParameters, withColon);
				}
				return list;
			}
			if (bindingInfo.DuplicateParameters.Count > 0)
			{
				if (bindingInfo.DuplicateParameters.Any((AstParameterArgumentPair pAst) => parameterAst.GetHashCode() == pAst.Parameter.GetHashCode()))
				{
					list = CompletionCompleters.GetParameterCompletionResults(parameterName, bindingInfo.ValidParameterSetsFlags, bindingInfo.BoundParameters.Values, withColon);
				}
				return list;
			}
			string text = null;
			foreach (KeyValuePair<string, AstParameterArgumentPair> keyValuePair in bindingInfo.BoundArguments)
			{
				switch (keyValuePair.Value.ParameterArgumentType)
				{
				case AstParameterArgumentType.AstPair:
				{
					AstPair astPair = (AstPair)keyValuePair.Value;
					if (astPair.ParameterSpecified && astPair.Parameter.GetHashCode() == parameterAst.GetHashCode())
					{
						text = keyValuePair.Key;
					}
					else if (astPair.ArgumentIsCommandParameterAst && astPair.Argument.GetHashCode() == parameterAst.GetHashCode())
					{
						return list;
					}
					break;
				}
				case AstParameterArgumentType.Switch:
				{
					SwitchPair switchPair = (SwitchPair)keyValuePair.Value;
					if (switchPair.ParameterSpecified && switchPair.Parameter.GetHashCode() == parameterAst.GetHashCode())
					{
						text = keyValuePair.Key;
					}
					break;
				}
				case AstParameterArgumentType.Fake:
				{
					FakePair fakePair = (FakePair)keyValuePair.Value;
					if (fakePair.ParameterSpecified && fakePair.Parameter.GetHashCode() == parameterAst.GetHashCode())
					{
						text = keyValuePair.Key;
					}
					break;
				}
				}
				if (text != null)
				{
					break;
				}
			}
			MergedCompiledCommandParameter mergedCompiledCommandParameter = bindingInfo.BoundParameters[text];
			WildcardPattern pattern = new WildcardPattern(parameterName + "*", WildcardOptions.IgnoreCase);
			string parameterType = "[" + ToStringCodeMethods.Type(mergedCompiledCommandParameter.Parameter.Type, true) + "] ";
			string colonSuffix = withColon ? ":" : string.Empty;
			if (pattern.IsMatch(text))
			{
				string completionText = "-" + text + colonSuffix;
				string toolTip = parameterType + text;
				list.Add(new CompletionResult(completionText, text, CompletionResultType.ParameterName, toolTip));
			}
			list.AddRange(from alias in mergedCompiledCommandParameter.Parameter.Aliases
			where pattern.IsMatch(alias)
			select new CompletionResult("-" + alias + colonSuffix, alias, CompletionResultType.ParameterName, parameterType + alias));
			return list;
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x001D7664 File Offset: 0x001D5864
		private static List<CompletionResult> GetParameterCompletionResults(string parameterName, uint validParameterSetFlags, IEnumerable<MergedCompiledCommandParameter> parameters, bool withColon)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			List<CompletionResult> list2 = new List<CompletionResult>();
			WildcardPattern pattern = new WildcardPattern(parameterName + "*", WildcardOptions.IgnoreCase);
			string colonSuffix = withColon ? ":" : string.Empty;
			bool flag = true;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in parameters)
			{
				bool flag2 = (mergedCompiledCommandParameter.Parameter.ParameterSetFlags & validParameterSetFlags) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets;
				if (flag2)
				{
					string name = mergedCompiledCommandParameter.Parameter.Name;
					string type = "[" + ToStringCodeMethods.Type(mergedCompiledCommandParameter.Parameter.Type, true) + "] ";
					List<CompletionResult> list3 = Cmdlet.CommonParameters.Contains(name, StringComparer.OrdinalIgnoreCase) ? list2 : list;
					if (pattern.IsMatch(name))
					{
						bool flag3 = true;
						Collection<Attribute> compiledAttributes = mergedCompiledCommandParameter.Parameter.CompiledAttributes;
						if (compiledAttributes != null && compiledAttributes.Count > 0)
						{
							foreach (Attribute attribute in compiledAttributes)
							{
								ParameterAttribute parameterAttribute = attribute as ParameterAttribute;
								if (parameterAttribute != null && parameterAttribute.DontShow)
								{
									flag3 = false;
									flag = false;
									break;
								}
							}
						}
						if (flag3)
						{
							string completionText = "-" + name + colonSuffix;
							string toolTip = type + name;
							list3.Add(new CompletionResult(completionText, name, CompletionResultType.ParameterName, toolTip));
						}
					}
					if (parameterName != string.Empty)
					{
						list3.AddRange(from alias in mergedCompiledCommandParameter.Parameter.Aliases
						where pattern.IsMatch(alias)
						select new CompletionResult("-" + alias + colonSuffix, alias, CompletionResultType.ParameterName, type + alias));
					}
				}
			}
			if (flag)
			{
				list.AddRange(list2);
			}
			return list;
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x001D78EC File Offset: 0x001D5AEC
		public static List<CompletionResult> CompleteOperator(string wordToComplete)
		{
			if (wordToComplete.StartsWith("-", StringComparison.Ordinal))
			{
				wordToComplete = wordToComplete.Substring(1);
			}
			return (from op in Tokenizer._operatorText
			where op.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
			orderby op
			select new CompletionResult("-" + op, op, CompletionResultType.ParameterName, CompletionCompleters.GetOperatorDescription(op))).ToList<CompletionResult>();
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x001D798A File Offset: 0x001D5B8A
		private static string GetOperatorDescription(string op)
		{
			return ResourceManagerCache.GetResourceString(typeof(CompletionCompleters).GetTypeInfo().Assembly, "TabCompletionStrings", op + "OperatorDescription");
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x001D79B8 File Offset: 0x001D5BB8
		internal static List<CompletionResult> CompleteCommandArgument(CompletionContext context)
		{
			CommandAst commandAst = null;
			List<CompletionResult> list = new List<CompletionResult>();
			ExpressionAst expressionAst = null;
			MemberExpressionAst memberExpressionAst = null;
			Ast ast = context.RelatedAsts.Last<Ast>();
			expressionAst = (ast as ExpressionAst);
			if (expressionAst != null)
			{
				if (expressionAst.Parent is CommandAst)
				{
					commandAst = (CommandAst)expressionAst.Parent;
					if (expressionAst is ErrorExpressionAst && expressionAst.Extent.Text.EndsWith(",", StringComparison.Ordinal))
					{
						context.WordToComplete = string.Empty;
					}
					else if (commandAst.CommandElements.Count == 1 || context.WordToComplete == string.Empty)
					{
						expressionAst = null;
					}
					else if (commandAst.CommandElements.Count > 2)
					{
						int count = commandAst.CommandElements.Count;
						int num = 1;
						while (num < count && commandAst.CommandElements[num] != expressionAst)
						{
							num++;
						}
						CommandElementAst commandElementAst = null;
						if (num > 1)
						{
							commandElementAst = commandAst.CommandElements[num - 1];
							memberExpressionAst = (commandElementAst as MemberExpressionAst);
						}
						StringConstantExpressionAst stringConstantExpressionAst = expressionAst as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null && commandElementAst != null && stringConstantExpressionAst.StringConstantType == StringConstantType.BareWord && commandElementAst.Extent.EndLineNumber == stringConstantExpressionAst.Extent.StartLineNumber && commandElementAst.Extent.EndColumnNumber == stringConstantExpressionAst.Extent.StartColumnNumber && stringConstantExpressionAst.Value.IndexOfAny(new char[]
						{
							'/',
							'\\'
						}) == 0)
						{
							StringConstantExpressionAst stringConstantExpressionAst2 = commandElementAst as StringConstantExpressionAst;
							ExpandableStringExpressionAst expandableStringExpressionAst = commandElementAst as ExpandableStringExpressionAst;
							ArrayLiteralAst arrayLiteralAst = commandElementAst as ArrayLiteralAst;
							CommandParameterAst commandParameterAst = commandElementAst as CommandParameterAst;
							if (stringConstantExpressionAst2 != null || expandableStringExpressionAst != null)
							{
								string wordToComplete = CompletionCompleters.ConcatenateStringPathArguments(commandElementAst, stringConstantExpressionAst.Value, context);
								expressionAst = ((stringConstantExpressionAst2 != null) ? stringConstantExpressionAst2 : expandableStringExpressionAst);
								context.ReplacementIndex = ((InternalScriptPosition)commandElementAst.Extent.StartScriptPosition).Offset;
								context.ReplacementLength += ((InternalScriptPosition)commandElementAst.Extent.EndScriptPosition).Offset - context.ReplacementIndex;
								context.WordToComplete = wordToComplete;
							}
							else if (arrayLiteralAst != null)
							{
								ExpressionAst expressionAst2 = arrayLiteralAst.Elements.LastOrDefault<ExpressionAst>();
								string text = CompletionCompleters.ConcatenateStringPathArguments(expressionAst2, stringConstantExpressionAst.Value, context);
								if (text != null)
								{
									expressionAst = arrayLiteralAst;
									context.ReplacementIndex = ((InternalScriptPosition)expressionAst2.Extent.StartScriptPosition).Offset;
									context.ReplacementLength += ((InternalScriptPosition)expressionAst2.Extent.EndScriptPosition).Offset - context.ReplacementIndex;
									context.WordToComplete = text;
								}
							}
							else if (commandParameterAst != null)
							{
								string text2 = CompletionCompleters.ConcatenateStringPathArguments(commandParameterAst.Argument, stringConstantExpressionAst.Value, context);
								if (text2 != null)
								{
									expressionAst = commandParameterAst.Argument;
									context.ReplacementIndex = ((InternalScriptPosition)commandParameterAst.Argument.Extent.StartScriptPosition).Offset;
									context.ReplacementLength += ((InternalScriptPosition)commandParameterAst.Argument.Extent.EndScriptPosition).Offset - context.ReplacementIndex;
									context.WordToComplete = text2;
								}
								else
								{
									ArrayLiteralAst arrayLiteralAst2 = commandParameterAst.Argument as ArrayLiteralAst;
									if (arrayLiteralAst2 != null)
									{
										ExpressionAst expressionAst3 = arrayLiteralAst2.Elements.LastOrDefault<ExpressionAst>();
										text2 = CompletionCompleters.ConcatenateStringPathArguments(expressionAst3, stringConstantExpressionAst.Value, context);
										if (text2 != null)
										{
											expressionAst = arrayLiteralAst2;
											context.ReplacementIndex = ((InternalScriptPosition)expressionAst3.Extent.StartScriptPosition).Offset;
											context.ReplacementLength += ((InternalScriptPosition)expressionAst3.Extent.EndScriptPosition).Offset - context.ReplacementIndex;
											context.WordToComplete = text2;
										}
									}
								}
							}
						}
					}
				}
				else if (expressionAst.Parent is ArrayLiteralAst && expressionAst.Parent.Parent is CommandAst)
				{
					commandAst = (CommandAst)expressionAst.Parent.Parent;
					if (commandAst.CommandElements.Count == 1 || context.WordToComplete == string.Empty)
					{
						expressionAst = null;
					}
					else
					{
						expressionAst = (ExpressionAst)expressionAst.Parent;
					}
				}
				else if (expressionAst.Parent is ArrayLiteralAst && expressionAst.Parent.Parent is CommandParameterAst)
				{
					commandAst = (CommandAst)expressionAst.Parent.Parent.Parent;
					if (context.WordToComplete == string.Empty)
					{
						expressionAst = null;
					}
					else
					{
						expressionAst = (ExpressionAst)expressionAst.Parent;
					}
				}
				else if (expressionAst.Parent is CommandParameterAst && expressionAst.Parent.Parent is CommandAst)
				{
					commandAst = (CommandAst)expressionAst.Parent.Parent;
					if (expressionAst is ErrorExpressionAst && expressionAst.Extent.Text.EndsWith(",", StringComparison.Ordinal))
					{
						context.WordToComplete = string.Empty;
					}
					else if (context.WordToComplete == string.Empty)
					{
						expressionAst = null;
					}
				}
			}
			else
			{
				CommandParameterAst commandParameterAst2 = ast as CommandParameterAst;
				if (commandParameterAst2 != null)
				{
					commandAst = (commandParameterAst2.Parent as CommandAst);
				}
				else
				{
					commandAst = (ast as CommandAst);
				}
			}
			if (commandAst == null)
			{
				return list;
			}
			PseudoBindingInfo pseudoBindingInfo = new PseudoParameterBinder().DoPseudoParameterBinding(commandAst, null, null, PseudoParameterBinder.BindingType.ArgumentCompletion);
			if (pseudoBindingInfo != null)
			{
				bool flag = false;
				if (pseudoBindingInfo.AllParsedArguments != null && pseudoBindingInfo.AllParsedArguments.Count > 0)
				{
					bool flag2 = false;
					if (expressionAst != null)
					{
						flag2 = true;
						StringConstantExpressionAst stringConstantExpressionAst3 = expressionAst as StringConstantExpressionAst;
						if (stringConstantExpressionAst3 != null && stringConstantExpressionAst3.Value.Trim().Equals("-", StringComparison.OrdinalIgnoreCase))
						{
							flag2 = false;
						}
					}
					CompletionCompleters.ArgumentLocation argumentLocation;
					if (flag2)
					{
						argumentLocation = CompletionCompleters.FindTargetArgumentLocation(pseudoBindingInfo.AllParsedArguments, expressionAst);
					}
					else
					{
						argumentLocation = CompletionCompleters.FindTargetArgumentLocation(pseudoBindingInfo.AllParsedArguments, context.TokenAtCursor ?? context.TokenBeforeCursor);
					}
					if (argumentLocation != null)
					{
						context.PseudoBindingInfo = pseudoBindingInfo;
						switch (pseudoBindingInfo.InfoType)
						{
						case PseudoBindingInfoType.PseudoBindingFail:
							list = CompletionCompleters.GetArgumentCompletionResultsWithFailedPseudoBinding(context, argumentLocation, commandAst);
							break;
						case PseudoBindingInfoType.PseudoBindingSucceed:
							list = CompletionCompleters.GetArgumentCompletionResultsWithSuccessfulPseudoBinding(context, argumentLocation, commandAst);
							break;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					int num2 = 0;
					CommandElementAst commandElementAst2 = null;
					if (expressionAst != null)
					{
						using (IEnumerator<CommandElementAst> enumerator = commandAst.CommandElements.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CommandElementAst commandElementAst3 = enumerator.Current;
								if (commandElementAst3.GetHashCode() == expressionAst.GetHashCode())
								{
									break;
								}
								commandElementAst2 = commandElementAst3;
								num2++;
							}
							goto IL_6AE;
						}
					}
					Token token = context.TokenAtCursor ?? context.TokenBeforeCursor;
					foreach (CommandElementAst commandElementAst4 in commandAst.CommandElements)
					{
						if (commandElementAst4.Extent.StartOffset > token.Extent.EndOffset)
						{
							break;
						}
						commandElementAst2 = commandElementAst4;
						num2++;
					}
					IL_6AE:
					if (num2 == 1)
					{
						CompletionCompleters.CompletePositionalArgument(pseudoBindingInfo.CommandName, commandAst, context, list, pseudoBindingInfo.UnboundParameters, pseudoBindingInfo.DefaultParameterSetFlag, uint.MaxValue, 0, null);
					}
					else if (commandElementAst2 is CommandParameterAst && ((CommandParameterAst)commandElementAst2).Argument == null)
					{
						string parameterName = ((CommandParameterAst)commandElementAst2).ParameterName;
						WildcardPattern wildcardPattern = new WildcardPattern(parameterName + "*", WildcardOptions.IgnoreCase);
						foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in pseudoBindingInfo.UnboundParameters)
						{
							if (wildcardPattern.IsMatch(mergedCompiledCommandParameter.Parameter.Name))
							{
								CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, list, mergedCompiledCommandParameter, null);
								break;
							}
							bool flag3 = false;
							foreach (string input in mergedCompiledCommandParameter.Parameter.Aliases)
							{
								if (wildcardPattern.IsMatch(input))
								{
									flag3 = true;
									CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, list, mergedCompiledCommandParameter, null);
									break;
								}
							}
							if (flag3)
							{
								break;
							}
						}
					}
				}
			}
			bool flag4 = false;
			if (list.Count > 0 && list[list.Count - 1].Equals(CompletionResult.Null))
			{
				list.RemoveAt(list.Count - 1);
				flag4 = true;
				if (list.Count > 0)
				{
					return list;
				}
			}
			if (expressionAst == null && !flag4 && commandAst.CommandElements.Count == 1 && commandAst.InvocationOperator != TokenKind.Unknown && context.WordToComplete != string.Empty)
			{
				bool flag5 = false;
				bool flag6 = CompletionCompleters.TurnOnLiteralPathOption(context);
				if (context.WordToComplete.IndexOf('-') != -1)
				{
					flag5 = true;
				}
				try
				{
					List<CompletionResult> list2 = new List<CompletionResult>(CompletionCompleters.CompleteFilename(context));
					if (flag5)
					{
						List<CompletionResult> list3 = CompletionCompleters.CompleteCommand(context);
						if (list3 != null && list3.Count > 0)
						{
							list2.AddRange(list3);
						}
					}
					return list2;
				}
				finally
				{
					if (flag6)
					{
						context.Options.Remove("LiteralPaths");
					}
				}
			}
			if (expressionAst is StringConstantExpressionAst)
			{
				StringConstantExpressionAst stringConstantExpressionAst4 = (StringConstantExpressionAst)expressionAst;
				Match match = Regex.Match(stringConstantExpressionAst4.Value, "^(\\[[\\w\\d\\.]+\\]::[\\w\\d\\*]*)$");
				if (match.Success)
				{
					string value = match.Groups[1].Value;
					Tuple<Ast, Token[], IScriptPosition> tuple = CommandCompletion.MapStringInputToParsedInput(value, value.Length);
					CompletionAnalysis completionAnalysis = new CompletionAnalysis(tuple.Item1, tuple.Item2, tuple.Item3, context.Options);
					int length;
					int num3;
					List<CompletionResult> results = completionAnalysis.GetResults(context.Helper.CurrentPowerShell, out length, out num3);
					if (results != null && results.Count > 0)
					{
						string str = TokenKind.LParen.Text() + value.Substring(0, length);
						foreach (CompletionResult completionResult in results)
						{
							string text3 = str + completionResult.CompletionText;
							if (completionResult.ResultType.Equals(CompletionResultType.Property))
							{
								text3 += TokenKind.RParen.Text();
							}
							list.Add(new CompletionResult(text3, completionResult.ListItemText, completionResult.ResultType, completionResult.ToolTip));
						}
						return list;
					}
				}
				if (stringConstantExpressionAst4.Value.IndexOf('*') != -1 && memberExpressionAst != null && memberExpressionAst.Extent.EndLineNumber == stringConstantExpressionAst4.Extent.StartLineNumber && memberExpressionAst.Extent.EndColumnNumber == stringConstantExpressionAst4.Extent.StartColumnNumber)
				{
					string text4 = stringConstantExpressionAst4.Value.EndsWith("*", StringComparison.Ordinal) ? stringConstantExpressionAst4.Value : (stringConstantExpressionAst4.Value + "*");
					ExpressionAst expression = memberExpressionAst.Expression;
					if (CompletionCompleters.IsSplattedVariable(expression))
					{
						return list;
					}
					StringConstantExpressionAst stringConstantExpressionAst5 = memberExpressionAst.Member as StringConstantExpressionAst;
					if (stringConstantExpressionAst5 != null)
					{
						text4 = stringConstantExpressionAst5.Value + text4;
					}
					CompletionCompleters.CompleteMemberHelper(false, text4, expression, context, list);
					if (list.Count > 0)
					{
						context.ReplacementIndex = ((InternalScriptPosition)memberExpressionAst.Expression.Extent.EndScriptPosition).Offset + 1;
						if (stringConstantExpressionAst5 != null)
						{
							context.ReplacementLength += stringConstantExpressionAst5.Value.Length;
						}
						return list;
					}
				}
				string text5 = stringConstantExpressionAst4.Value;
				if (commandAst.InvocationOperator != TokenKind.Unknown && text5.IndexOfAny(new char[]
				{
					'\\',
					'/'
				}) == 0 && commandAst.CommandElements.Count == 2 && commandAst.CommandElements[0] is StringConstantExpressionAst && commandAst.CommandElements[0].Extent.EndLineNumber == expressionAst.Extent.StartLineNumber && commandAst.CommandElements[0].Extent.EndColumnNumber == expressionAst.Extent.StartColumnNumber)
				{
					if (pseudoBindingInfo != null)
					{
						return list;
					}
					StringConstantExpressionAst stringConstantExpressionAst6 = (StringConstantExpressionAst)commandAst.CommandElements[0];
					text5 = stringConstantExpressionAst6.Value + text5;
					context.ReplacementIndex = ((InternalScriptPosition)stringConstantExpressionAst6.Extent.StartScriptPosition).Offset;
					context.ReplacementLength += ((InternalScriptPosition)stringConstantExpressionAst6.Extent.EndScriptPosition).Offset - context.ReplacementIndex;
					context.WordToComplete = text5;
					bool flag7 = CompletionCompleters.TurnOnLiteralPathOption(context);
					try
					{
						return new List<CompletionResult>(CompletionCompleters.CompleteFilename(context));
					}
					finally
					{
						if (flag7)
						{
							context.Options.Remove("LiteralPaths");
						}
					}
				}
			}
			if (!flag4)
			{
				string commandName = commandAst.GetCommandName();
				ScriptBlock customArgumentCompleter = CompletionCompleters.GetCustomArgumentCompleter("NativeArgumentCompleters", new string[]
				{
					commandName,
					Path.GetFileName(commandName),
					Path.GetFileNameWithoutExtension(commandName)
				}, context);
				if (customArgumentCompleter != null && CompletionCompleters.InvokeScriptArgumentCompleter(customArgumentCompleter, new object[]
				{
					context.WordToComplete,
					commandAst,
					context.CursorPosition.Offset
				}, list))
				{
					return list;
				}
				bool flag8 = false;
				if (pseudoBindingInfo == null)
				{
					flag8 = CompletionCompleters.TurnOnLiteralPathOption(context);
				}
				try
				{
					list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(context));
				}
				finally
				{
					if (flag8)
					{
						context.Options.Remove("LiteralPaths");
					}
				}
				if (context.WordToComplete != string.Empty && context.WordToComplete.IndexOf('-') != -1)
				{
					List<CompletionResult> list4 = CompletionCompleters.CompleteCommand(new CompletionContext
					{
						WordToComplete = context.WordToComplete,
						Helper = context.Helper
					});
					if (list4 != null)
					{
						list.AddRange(list4);
					}
				}
			}
			return list;
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x001D8794 File Offset: 0x001D6994
		internal static string ConcatenateStringPathArguments(CommandElementAst stringAst, string partialPath, CompletionContext completionContext)
		{
			StringConstantExpressionAst stringConstantExpressionAst = stringAst as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				string text = string.Empty;
				switch (stringConstantExpressionAst.StringConstantType)
				{
				case StringConstantType.SingleQuoted:
					text = "'";
					break;
				case StringConstantType.DoubleQuoted:
					text = "\"";
					break;
				}
				return text + stringConstantExpressionAst.Value + partialPath + text;
			}
			ExpandableStringExpressionAst expandableStringExpressionAst = stringAst as ExpandableStringExpressionAst;
			string result = null;
			if (expandableStringExpressionAst != null && CompletionCompleters.IsPathSafelyExpandable(expandableStringExpressionAst, partialPath, completionContext.ExecutionContext, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x001D880C File Offset: 0x001D6A0C
		private static List<CompletionResult> GetArgumentCompletionResultsWithFailedPseudoBinding(CompletionContext context, CompletionCompleters.ArgumentLocation argLocation, CommandAst commandAst)
		{
			List<CompletionResult> result = new List<CompletionResult>();
			PseudoBindingInfo pseudoBindingInfo = context.PseudoBindingInfo;
			if (argLocation.IsPositional)
			{
				CompletionCompleters.CompletePositionalArgument(pseudoBindingInfo.CommandName, commandAst, context, result, pseudoBindingInfo.UnboundParameters, pseudoBindingInfo.DefaultParameterSetFlag, uint.MaxValue, argLocation.Position, null);
			}
			else
			{
				string parameterName = argLocation.Argument.ParameterName;
				WildcardPattern wildcardPattern = new WildcardPattern(parameterName + "*", WildcardOptions.IgnoreCase);
				foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in pseudoBindingInfo.UnboundParameters)
				{
					if (wildcardPattern.IsMatch(mergedCompiledCommandParameter.Parameter.Name))
					{
						CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, result, mergedCompiledCommandParameter, null);
						break;
					}
					bool flag = false;
					foreach (string input in mergedCompiledCommandParameter.Parameter.Aliases)
					{
						if (wildcardPattern.IsMatch(input))
						{
							flag = true;
							CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, result, mergedCompiledCommandParameter, null);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x001D8990 File Offset: 0x001D6B90
		private static List<CompletionResult> GetArgumentCompletionResultsWithSuccessfulPseudoBinding(CompletionContext context, CompletionCompleters.ArgumentLocation argLocation, CommandAst commandAst)
		{
			PseudoBindingInfo pseudoBindingInfo = context.PseudoBindingInfo;
			List<CompletionResult> result = new List<CompletionResult>();
			if (argLocation.IsPositional && argLocation.Argument == null)
			{
				AstPair lastPositionalArg;
				AstParameterArgumentPair astParameterArgumentPair = CompletionCompleters.FindTargetPositionalArgument(pseudoBindingInfo.AllParsedArguments, argLocation.Position, out lastPositionalArg);
				if (astParameterArgumentPair == null)
				{
					if (lastPositionalArg != null)
					{
						bool flag = false;
						Collection<string> collection = new Collection<string>();
						foreach (KeyValuePair<string, AstParameterArgumentPair> keyValuePair in pseudoBindingInfo.BoundArguments)
						{
							if (!keyValuePair.Value.ParameterSpecified)
							{
								AstPair astPair = (AstPair)keyValuePair.Value;
								if (astPair.Argument.GetHashCode() == lastPositionalArg.Argument.GetHashCode())
								{
									flag = true;
									break;
								}
							}
							else if (keyValuePair.Value.ParameterArgumentType.Equals(AstParameterArgumentType.AstArray))
							{
								AstArrayPair astArrayPair = (AstArrayPair)keyValuePair.Value;
								if (astArrayPair.Argument.Any((ExpressionAst exp) => exp.GetHashCode() == lastPositionalArg.Argument.GetHashCode()))
								{
									collection.Add(keyValuePair.Key);
								}
							}
						}
						if (collection.Count > 0)
						{
							foreach (string key in collection)
							{
								MergedCompiledCommandParameter parameter = pseudoBindingInfo.BoundParameters[key];
								CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, result, parameter, pseudoBindingInfo.BoundArguments);
							}
							return result;
						}
						if (!flag)
						{
							return result;
						}
					}
					CompletionCompleters.CompletePositionalArgument(pseudoBindingInfo.CommandName, commandAst, context, result, pseudoBindingInfo.UnboundParameters, pseudoBindingInfo.DefaultParameterSetFlag, pseudoBindingInfo.ValidParameterSetsFlags, argLocation.Position, pseudoBindingInfo.BoundArguments);
					return result;
				}
				argLocation.Argument = astParameterArgumentPair;
			}
			if (argLocation.Argument != null)
			{
				Collection<string> collection2 = new Collection<string>();
				foreach (KeyValuePair<string, AstParameterArgumentPair> keyValuePair2 in pseudoBindingInfo.BoundArguments)
				{
					if (!keyValuePair2.Value.ParameterArgumentType.Equals(AstParameterArgumentType.PipeObject))
					{
						if (keyValuePair2.Value.ParameterArgumentType.Equals(AstParameterArgumentType.AstArray) && !argLocation.Argument.ParameterSpecified)
						{
							AstArrayPair astArrayPair2 = (AstArrayPair)keyValuePair2.Value;
							AstPair target = (AstPair)argLocation.Argument;
							if (astArrayPair2.Argument.Any((ExpressionAst exp) => exp.GetHashCode() == target.Argument.GetHashCode()))
							{
								collection2.Add(keyValuePair2.Key);
							}
						}
						else if (keyValuePair2.Value.GetHashCode() == argLocation.Argument.GetHashCode())
						{
							collection2.Add(keyValuePair2.Key);
						}
					}
				}
				if (collection2.Count > 0)
				{
					foreach (string key2 in collection2)
					{
						MergedCompiledCommandParameter parameter2 = pseudoBindingInfo.BoundParameters[key2];
						CompletionCompleters.ProcessParameter(pseudoBindingInfo.CommandName, commandAst, context, result, parameter2, pseudoBindingInfo.BoundArguments);
					}
				}
			}
			return result;
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x001D8D08 File Offset: 0x001D6F08
		private static void CompletePositionalArgument(string commandName, CommandAst commandAst, CompletionContext context, List<CompletionResult> result, IEnumerable<MergedCompiledCommandParameter> parameters, uint defaultParameterSetFlag, uint validParameterSetFlags, int position, Dictionary<string, AstParameterArgumentPair> boundArguments = null)
		{
			bool flag = false;
			bool flag2 = defaultParameterSetFlag != 0U && (defaultParameterSetFlag & validParameterSetFlags) != 0U;
			MergedCompiledCommandParameter mergedCompiledCommandParameter = null;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter2 in parameters)
			{
				bool flag3 = (mergedCompiledCommandParameter2.Parameter.ParameterSetFlags & validParameterSetFlags) != 0U || mergedCompiledCommandParameter2.Parameter.IsInAllSets;
				if (flag3)
				{
					IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter2.Parameter.GetMatchingParameterSetData(validParameterSetFlags);
					foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
					{
						if (!parameterSetSpecificMetadata.ValueFromRemainingArguments)
						{
							int position2 = parameterSetSpecificMetadata.Position;
							if (position2 != -2147483648 && position2 == position)
							{
								if (!flag2)
								{
									flag = true;
									CompletionCompleters.ProcessParameter(commandName, commandAst, context, result, mergedCompiledCommandParameter2, boundArguments);
									break;
								}
								if (parameterSetSpecificMetadata.ParameterSetFlag == defaultParameterSetFlag)
								{
									CompletionCompleters.ProcessParameter(commandName, commandAst, context, result, mergedCompiledCommandParameter2, boundArguments);
									flag = result.Any<CompletionResult>();
									break;
								}
								if (mergedCompiledCommandParameter == null)
								{
									mergedCompiledCommandParameter = mergedCompiledCommandParameter2;
								}
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag && mergedCompiledCommandParameter != null)
			{
				flag = true;
				CompletionCompleters.ProcessParameter(commandName, commandAst, context, result, mergedCompiledCommandParameter, boundArguments);
			}
			if (!flag)
			{
				foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter3 in parameters)
				{
					bool flag4 = (mergedCompiledCommandParameter3.Parameter.ParameterSetFlags & validParameterSetFlags) != 0U || mergedCompiledCommandParameter3.Parameter.IsInAllSets;
					if (flag4)
					{
						IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData2 = mergedCompiledCommandParameter3.Parameter.GetMatchingParameterSetData(validParameterSetFlags);
						foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata2 in matchingParameterSetData2)
						{
							if (parameterSetSpecificMetadata2.ValueFromRemainingArguments)
							{
								CompletionCompleters.ProcessParameter(commandName, commandAst, context, result, mergedCompiledCommandParameter3, boundArguments);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x001D8F68 File Offset: 0x001D7168
		private static void ProcessParameter(string commandName, CommandAst commandAst, CompletionContext context, List<CompletionResult> result, MergedCompiledCommandParameter parameter, Dictionary<string, AstParameterArgumentPair> boundArguments = null)
		{
			CompletionResult completionResult = null;
			Type type = CompletionCompleters.GetEffectiveParameterType(parameter.Parameter.Type);
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			if (type.GetTypeInfo().IsEnum)
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				string text = LanguagePrimitives.EnumSingleTypeConverter.EnumValues(type);
				string listSeparator = CultureInfo.CurrentUICulture.TextInfo.ListSeparator;
				string[] array = text.Split(new string[]
				{
					listSeparator
				}, StringSplitOptions.RemoveEmptyEntries);
				string wordToComplete = context.WordToComplete;
				string quote = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
				WildcardPattern wildcardPattern = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);
				List<string> list = new List<string>();
				foreach (string text2 in array)
				{
					if (wordToComplete.Equals(text2, StringComparison.OrdinalIgnoreCase))
					{
						string completionText = (quote == string.Empty) ? text2 : (quote + text2 + quote);
						completionResult = new CompletionResult(completionText, text2, CompletionResultType.ParameterValue, text2);
					}
					else if (wildcardPattern.IsMatch(text2))
					{
						list.Add(text2);
					}
				}
				if (completionResult != null)
				{
					result.Add(completionResult);
				}
				list.Sort();
				result.AddRange(from entry in list
				let completionText = (quote == string.Empty) ? entry : (quote + entry + quote)
				select new CompletionResult(completionText, entry, CompletionResultType.ParameterValue, entry));
				result.Add(CompletionResult.Null);
				return;
			}
			if (type.Equals(typeof(SwitchParameter)))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				if (context.WordToComplete == string.Empty || context.WordToComplete.Equals("$", StringComparison.Ordinal))
				{
					result.Add(new CompletionResult("$true", "$true", CompletionResultType.ParameterValue, "$true"));
					result.Add(new CompletionResult("$false", "$false", CompletionResultType.ParameterValue, "$false"));
				}
				result.Add(CompletionResult.Null);
				return;
			}
			foreach (ValidateArgumentsAttribute validateArgumentsAttribute in parameter.Parameter.ValidationAttributes)
			{
				if (validateArgumentsAttribute is ValidateSetAttribute)
				{
					CompletionCompleters.RemoveLastNullCompletionResult(result);
					ValidateSetAttribute validateSetAttribute = (ValidateSetAttribute)validateArgumentsAttribute;
					string wordToComplete2 = context.WordToComplete;
					string text3 = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete2);
					WildcardPattern wildcardPattern2 = new WildcardPattern(wordToComplete2 + "*", WildcardOptions.IgnoreCase);
					List<string> list2 = new List<string>();
					foreach (string text4 in validateSetAttribute.ValidValues)
					{
						if (wordToComplete2.Equals(text4, StringComparison.OrdinalIgnoreCase))
						{
							string completionText2 = (text3 == string.Empty) ? text4 : (text3 + text4 + text3);
							completionResult = new CompletionResult(completionText2, text4, CompletionResultType.ParameterValue, text4);
						}
						else if (wildcardPattern2.IsMatch(text4))
						{
							list2.Add(text4);
						}
					}
					if (completionResult != null)
					{
						result.Add(completionResult);
					}
					list2.Sort();
					foreach (string text5 in list2)
					{
						string str = text5;
						string completionText3 = text5;
						if (text3 == string.Empty)
						{
							if (CompletionCompleters.CompletionRequiresQuotes(text5, false))
							{
								str = CodeGeneration.EscapeSingleQuotedStringContent(text5);
								completionText3 = "'" + str + "'";
							}
						}
						else
						{
							if (text3.Equals("'", StringComparison.OrdinalIgnoreCase))
							{
								str = CodeGeneration.EscapeSingleQuotedStringContent(text5);
							}
							completionText3 = text3 + str + text3;
						}
						result.Add(new CompletionResult(completionText3, text5, CompletionResultType.ParameterValue, text5));
					}
					result.Add(CompletionResult.Null);
					return;
				}
			}
			CompletionCompleters.NativeCommandArgumentCompletion(commandName, parameter.Parameter, result, commandAst, context, boundArguments);
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x001D9838 File Offset: 0x001D7A38
		private static IEnumerable<PSTypeName> NativeCommandArgumentCompletion_InferTypesOfArugment(Dictionary<string, AstParameterArgumentPair> boundArguments, CommandAst commandAst, CompletionContext context, string parameterName)
		{
			AstParameterArgumentPair astParameterArgumentPair;
			if (boundArguments != null && boundArguments.TryGetValue(parameterName, out astParameterArgumentPair))
			{
				Ast argumentAst = null;
				AstParameterArgumentType parameterArgumentType = astParameterArgumentPair.ParameterArgumentType;
				if (parameterArgumentType != AstParameterArgumentType.AstPair)
				{
					if (parameterArgumentType == AstParameterArgumentType.PipeObject)
					{
						PipelineAst pipelineAst = commandAst.Parent as PipelineAst;
						if (pipelineAst != null)
						{
							int num = 0;
							while (num < pipelineAst.PipelineElements.Count && pipelineAst.PipelineElements[num] != commandAst)
							{
								num++;
							}
							if (num != 0)
							{
								argumentAst = pipelineAst.PipelineElements[num - 1];
							}
						}
					}
				}
				else
				{
					AstPair astPair = (AstPair)astParameterArgumentPair;
					argumentAst = astPair.Argument;
				}
				if (argumentAst != null)
				{
					ExpressionAst argumentExpressionAst = argumentAst as ExpressionAst;
					if (argumentExpressionAst == null)
					{
						CommandExpressionAst commandExpressionAst = argumentAst as CommandExpressionAst;
						if (commandExpressionAst != null)
						{
							argumentExpressionAst = commandExpressionAst.Expression;
						}
					}
					object argumentValue;
					if (argumentExpressionAst != null && SafeExprEvaluator.TrySafeEval(argumentExpressionAst, context.ExecutionContext, out argumentValue) && argumentValue != null)
					{
						IEnumerable enumerable = LanguagePrimitives.GetEnumerable(argumentValue);
						if (enumerable == null)
						{
							enumerable = new object[]
							{
								argumentValue
							};
						}
						foreach (object element in enumerable)
						{
							if (element != null)
							{
								PSObject pso = PSObject.AsPSObject(element);
								if (pso.TypeNames.Count > 0 && !pso.TypeNames[0].Equals(pso.BaseObject.GetType().FullName, StringComparison.OrdinalIgnoreCase))
								{
									yield return new PSTypeName(pso.TypeNames[0]);
								}
								if (!(pso.BaseObject is PSCustomObject))
								{
									yield return new PSTypeName(pso.BaseObject.GetType());
								}
							}
						}
					}
					else
					{
						foreach (PSTypeName typeName in argumentAst.GetInferredType(context))
						{
							yield return typeName;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x001D986C File Offset: 0x001D7A6C
		internal static IList<string> NativeCommandArgumentCompletion_ExtractSecondaryArgument(Dictionary<string, AstParameterArgumentPair> boundArguments, string parameterName)
		{
			List<string> list = new List<string>();
			if (boundArguments == null)
			{
				return list;
			}
			if (!boundArguments.ContainsKey(parameterName))
			{
				return list;
			}
			AstParameterArgumentPair astParameterArgumentPair = boundArguments[parameterName];
			AstParameterArgumentType parameterArgumentType = astParameterArgumentPair.ParameterArgumentType;
			if (parameterArgumentType != AstParameterArgumentType.AstPair)
			{
				if (parameterArgumentType != AstParameterArgumentType.AstArray)
				{
					return list;
				}
			}
			else
			{
				AstPair astPair = (AstPair)astParameterArgumentPair;
				if (astPair.Argument is StringConstantExpressionAst)
				{
					StringConstantExpressionAst stringConstantExpressionAst = (StringConstantExpressionAst)astPair.Argument;
					list.Add(stringConstantExpressionAst.Value);
					return list;
				}
				if (!(astPair.Argument is ArrayLiteralAst))
				{
					return list;
				}
				ArrayLiteralAst arrayLiteralAst = (ArrayLiteralAst)astPair.Argument;
				using (IEnumerator<ExpressionAst> enumerator = arrayLiteralAst.Elements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ExpressionAst expressionAst = enumerator.Current;
						StringConstantExpressionAst stringConstantExpressionAst2 = expressionAst as StringConstantExpressionAst;
						if (stringConstantExpressionAst2 == null)
						{
							list.Clear();
							break;
						}
						list.Add(stringConstantExpressionAst2.Value);
					}
					return list;
				}
			}
			AstArrayPair astArrayPair = (AstArrayPair)astParameterArgumentPair;
			ExpressionAst[] argument = astArrayPair.Argument;
			foreach (ExpressionAst expressionAst2 in argument)
			{
				StringConstantExpressionAst stringConstantExpressionAst3 = expressionAst2 as StringConstantExpressionAst;
				if (stringConstantExpressionAst3 == null)
				{
					list.Clear();
					break;
				}
				list.Add(stringConstantExpressionAst3.Value);
			}
			return list;
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x001D99B8 File Offset: 0x001D7BB8
		private static void NativeCommandArgumentCompletion(string commandName, CompiledCommandParameter parameter, List<CompletionResult> result, CommandAst commandAst, CompletionContext context, Dictionary<string, AstParameterArgumentPair> boundArguments = null)
		{
			if (string.IsNullOrEmpty(commandName))
			{
				return;
			}
			string name = parameter.Name;
			ScriptBlock customArgumentCompleter = CompletionCompleters.GetCustomArgumentCompleter("CustomArgumentCompleters", new string[]
			{
				commandName + ":" + name,
				name
			}, context);
			if (customArgumentCompleter != null && CompletionCompleters.InvokeScriptArgumentCompleter(customArgumentCompleter, commandName, name, context.WordToComplete, commandAst, context, result))
			{
				return;
			}
			ArgumentCompleterAttribute argumentCompleterAttribute = parameter.CompiledAttributes.OfType<ArgumentCompleterAttribute>().FirstOrDefault<ArgumentCompleterAttribute>();
			if (argumentCompleterAttribute != null)
			{
				try
				{
					if (argumentCompleterAttribute.Type != null)
					{
						IArgumentCompleter argumentCompleter = Activator.CreateInstance(argumentCompleterAttribute.Type) as IArgumentCompleter;
						if (argumentCompleter != null)
						{
							IEnumerable<CompletionResult> enumerable = argumentCompleter.CompleteArgument(commandName, name, context.WordToComplete, commandAst, CompletionCompleters.GetBoundArgumentsAsHashtable(context));
							if (enumerable != null)
							{
								result.AddRange(enumerable);
								result.Add(CompletionResult.Null);
								return;
							}
						}
					}
					else if (CompletionCompleters.InvokeScriptArgumentCompleter(argumentCompleterAttribute.ScriptBlock, commandName, name, context.WordToComplete, commandAst, context, result))
					{
						return;
					}
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			switch (commandName)
			{
			case "Get-Command":
				if (name.Equals("Module", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionGetCommand(context.WordToComplete, null, name, result, context);
					return;
				}
				if (name.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					IList<string> list = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(boundArguments, "Module");
					if (list.Count > 0)
					{
						using (IEnumerator<string> enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string moduleName = enumerator.Current;
								CompletionCompleters.NativeCompletionGetCommand(context.WordToComplete, moduleName, name, result, context);
							}
							return;
						}
					}
					CompletionCompleters.NativeCompletionGetCommand(context.WordToComplete, null, name, result, context);
					return;
				}
				if (name.Equals("ParameterType", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionTypeName(context, result);
					return;
				}
				return;
			case "Show-Command":
				CompletionCompleters.NativeCompletionGetHelpCommand(context.WordToComplete, name, false, result, context);
				return;
			case "help":
			case "Get-Help":
				CompletionCompleters.NativeCompletionGetHelpCommand(context.WordToComplete, name, true, result, context);
				return;
			case "Invoke-Expression":
			{
				if (!name.Equals("Command", StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				List<CompletionResult> list2 = CompletionCompleters.CompleteCommand(new CompletionContext
				{
					WordToComplete = context.WordToComplete,
					Helper = context.Helper
				});
				if (list2 != null)
				{
					result.AddRange(list2);
					return;
				}
				return;
			}
			case "Clear-EventLog":
			case "Get-EventLog":
			case "Limit-EventLog":
			case "Remove-EventLog":
			case "Write-EventLog":
				CompletionCompleters.NativeCompletionEventLogCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-Job":
			case "Receive-Job":
			case "Remove-Job":
			case "Stop-Job":
			case "Wait-Job":
			case "Suspend-Job":
			case "Resume-Job":
				CompletionCompleters.NativeCompletionJobCommands(context.WordToComplete, name, result, context);
				return;
			case "Disable-ScheduledJob":
			case "Enable-ScheduledJob":
			case "Get-ScheduledJob":
			case "Unregister-ScheduledJob":
				CompletionCompleters.NativeCompletionScheduledJobCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-Module":
			{
				bool loadedModulesOnly = boundArguments == null || !boundArguments.ContainsKey("ListAvailable");
				CompletionCompleters.NativeCompletionModuleCommands(context.WordToComplete, name, loadedModulesOnly, false, result, context);
				return;
			}
			case "Remove-Module":
				CompletionCompleters.NativeCompletionModuleCommands(context.WordToComplete, name, true, false, result, context);
				return;
			case "Import-Module":
				CompletionCompleters.NativeCompletionModuleCommands(context.WordToComplete, name, false, true, result, context);
				return;
			case "Debug-Process":
			case "Get-Process":
			case "Stop-Process":
			case "Wait-Process":
			case "Enter-PSHostProcess":
				CompletionCompleters.NativeCompletionProcessCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-PSDrive":
			case "Remove-PSDrive":
				if (name.Equals("PSProvider", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionProviderCommands(context.WordToComplete, name, result, context);
					return;
				}
				if (name.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					IList<string> list3 = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(boundArguments, "PSProvider");
					if (list3.Count > 0)
					{
						using (IEnumerator<string> enumerator2 = list3.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string psProvider = enumerator2.Current;
								CompletionCompleters.NativeCompletionDriveCommands(context.WordToComplete, psProvider, name, result, context);
							}
							return;
						}
					}
					CompletionCompleters.NativeCompletionDriveCommands(context.WordToComplete, null, name, result, context);
					return;
				}
				return;
			case "New-PSDrive":
				CompletionCompleters.NativeCompletionProviderCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-PSProvider":
				CompletionCompleters.NativeCompletionProviderCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-Service":
			case "Start-Service":
			case "Restart-Service":
			case "Resume-Service":
			case "Set-Service":
			case "Stop-Service":
			case "Suspend-Service":
				CompletionCompleters.NativeCompletionServiceCommands(context.WordToComplete, name, result, context);
				return;
			case "Clear-Variable":
			case "Get-Variable":
			case "Remove-Variable":
			case "Set-Variable":
				CompletionCompleters.NativeCompletionVariableCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-Alias":
				CompletionCompleters.NativeCompletionAliasCommands(context.WordToComplete, name, result, context);
				return;
			case "Get-TraceSource":
			case "Set-TraceSource":
			case "Trace-Command":
				CompletionCompleters.NativeCompletionTraceSourceCommands(context.WordToComplete, name, result, context);
				return;
			case "Push-Location":
			case "Set-Location":
				CompletionCompleters.NativeCompletionSetLocationCommand(context.WordToComplete, name, result, context);
				return;
			case "Move-Item":
			case "Copy-Item":
				CompletionCompleters.NativeCompletionCopyMoveItemCommand(context.WordToComplete, name, result, context);
				return;
			case "New-Item":
				CompletionCompleters.NativeCompletionNewItemCommand(context.WordToComplete, name, result, context);
				return;
			case "ForEach-Object":
				if (name.Equals("MemberName", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionMemberName(context.WordToComplete, result, commandAst, context);
					return;
				}
				return;
			case "Group-Object":
			case "Measure-Object":
			case "Select-Object":
			case "Sort-Object":
			case "Where-Object":
			case "Format-Custom":
			case "Format-List":
			case "Format-Table":
			case "Format-Wide":
				if (name.Equals("Property", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionMemberName(context.WordToComplete, result, commandAst, context);
					return;
				}
				return;
			case "New-Object":
				if (name.Equals("TypeName", StringComparison.OrdinalIgnoreCase))
				{
					CompletionCompleters.NativeCompletionTypeName(context, result);
					return;
				}
				return;
			case "Get-CimClass":
			case "Get-CimInstance":
			case "Get-CimAssociatedInstance":
			case "Invoke-CimMethod":
			case "New-CimInstance":
			case "Register-CimIndicationEvent":
				CompletionCompleters.NativeCompletionCimCommands(name, boundArguments, result, commandAst, context);
				return;
			}
			CompletionCompleters.NativeCompletionPathArgument(context.WordToComplete, name, result, context);
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x001DA31C File Offset: 0x001D851C
		private static Hashtable GetBoundArgumentsAsHashtable(CompletionContext context)
		{
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			if (context.PseudoBindingInfo != null)
			{
				Dictionary<string, AstParameterArgumentPair> boundArguments = context.PseudoBindingInfo.BoundArguments;
				if (boundArguments != null)
				{
					foreach (KeyValuePair<string, AstParameterArgumentPair> keyValuePair in boundArguments)
					{
						AstPair astPair = keyValuePair.Value as AstPair;
						if (astPair != null)
						{
							CommandParameterAst commandParameterAst = astPair.Argument as CommandParameterAst;
							ExpressionAst expressionAst = (commandParameterAst != null) ? commandParameterAst.Argument : (astPair.Argument as ExpressionAst);
							object value;
							if (expressionAst != null && SafeExprEvaluator.TrySafeEval(expressionAst, context.ExecutionContext, out value))
							{
								hashtable[keyValuePair.Key] = value;
							}
						}
						else
						{
							SwitchPair switchPair = keyValuePair.Value as SwitchPair;
							if (switchPair != null)
							{
								hashtable[keyValuePair.Key] = switchPair.Argument;
							}
						}
					}
				}
			}
			return hashtable;
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x001DA41C File Offset: 0x001D861C
		private static ScriptBlock GetCustomArgumentCompleter(string optionKey, IEnumerable<string> keys, CompletionContext context)
		{
			Hashtable options = context.Options;
			if (options != null)
			{
				Hashtable hashtable = options[optionKey] as Hashtable;
				if (hashtable != null)
				{
					foreach (string key in keys)
					{
						if (hashtable.ContainsKey(key))
						{
							ScriptBlock scriptBlock = hashtable[key] as ScriptBlock;
							if (scriptBlock != null)
							{
								return scriptBlock;
							}
						}
					}
				}
			}
			Dictionary<string, ScriptBlock> dictionary = optionKey.Equals("NativeArgumentCompleters", StringComparison.OrdinalIgnoreCase) ? context.NativeArgumentCompleters : context.CustomArgumentCompleters;
			if (dictionary != null)
			{
				foreach (string key2 in keys)
				{
					ScriptBlock scriptBlock;
					if (dictionary.TryGetValue(key2, out scriptBlock))
					{
						return scriptBlock;
					}
				}
			}
			return null;
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x001DA508 File Offset: 0x001D8708
		private static bool InvokeScriptArgumentCompleter(ScriptBlock scriptBlock, string commandName, string parameterName, string wordToComplete, CommandAst commandAst, CompletionContext context, List<CompletionResult> resultList)
		{
			bool flag = CompletionCompleters.InvokeScriptArgumentCompleter(scriptBlock, new object[]
			{
				commandName,
				parameterName,
				wordToComplete,
				commandAst,
				CompletionCompleters.GetBoundArgumentsAsHashtable(context)
			}, resultList);
			if (flag)
			{
				resultList.Add(CompletionResult.Null);
			}
			return flag;
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x001DA554 File Offset: 0x001D8754
		private static bool InvokeScriptArgumentCompleter(ScriptBlock scriptBlock, object[] argumentsToCompleter, List<CompletionResult> result)
		{
			Collection<PSObject> collection = null;
			try
			{
				collection = scriptBlock.Invoke(argumentsToCompleter);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			if (collection == null || !collection.Any<PSObject>())
			{
				return false;
			}
			foreach (PSObject psobject in collection)
			{
				CompletionResult completionResult = psobject.BaseObject as CompletionResult;
				if (completionResult != null)
				{
					result.Add(completionResult);
				}
				else
				{
					string completionText = psobject.ToString();
					result.Add(new CompletionResult(completionText));
				}
			}
			return true;
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x001DA5F8 File Offset: 0x001D87F8
		private static void RemoveLastNullCompletionResult(List<CompletionResult> result)
		{
			if (result.Count > 0 && result[result.Count - 1].Equals(CompletionResult.Null))
			{
				result.RemoveAt(result.Count - 1);
			}
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x001DA62C File Offset: 0x001D882C
		private static bool NativeCompletionCimCommands_ParseTypeName(PSTypeName typename, out string cimNamespace, out string className)
		{
			cimNamespace = null;
			className = null;
			if (typename == null)
			{
				return false;
			}
			if (typename.Type != null)
			{
				return false;
			}
			Match match = Regex.Match(typename.Name, "(?<NetTypeName>.*)#(?<CimNamespace>.*)[/\\\\](?<CimClassName>.*)");
			if (!match.Success)
			{
				return false;
			}
			if (!match.Groups["NetTypeName"].Value.Equals(typeof(CimInstance).FullName, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			cimNamespace = match.Groups["CimNamespace"].Value;
			className = match.Groups["CimClassName"].Value;
			return true;
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x001DA6CC File Offset: 0x001D88CC
		private static void NativeCompletionCimCommands(string parameter, Dictionary<string, AstParameterArgumentPair> boundArguments, List<CompletionResult> result, CommandAst commandAst, CompletionContext context)
		{
			if (boundArguments != null)
			{
				AstParameterArgumentPair astParameterArgumentPair = null;
				if (boundArguments.ContainsKey("ComputerName"))
				{
					astParameterArgumentPair = boundArguments["ComputerName"];
				}
				else if (boundArguments.ContainsKey("CimSession"))
				{
					astParameterArgumentPair = boundArguments["CimSession"];
				}
				if (astParameterArgumentPair != null)
				{
					switch (astParameterArgumentPair.ParameterArgumentType)
					{
					case AstParameterArgumentType.Fake:
					case AstParameterArgumentType.PipeObject:
						break;
					default:
						return;
					}
				}
			}
			if (parameter.Equals("Namespace", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.NativeCompletionCimNamespace(result, context);
				result.Add(CompletionResult.Null);
				return;
			}
			string text = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(boundArguments, "Namespace").FirstOrDefault<string>();
			if (parameter.Equals("ClassName", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.NativeCompletionCimClassName(text, result, context);
				result.Add(CompletionResult.Null);
				return;
			}
			bool flag = false;
			IEnumerable<PSTypeName> enumerable = null;
			string text2 = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(boundArguments, "ClassName").FirstOrDefault<string>();
			if (text2 != null)
			{
				flag = false;
				enumerable = new List<PSTypeName>
				{
					new PSTypeName(string.Concat(new string[]
					{
						typeof(CimInstance).FullName,
						"#",
						text ?? "root/cimv2",
						"/",
						text2
					}))
				};
			}
			else if (boundArguments != null && boundArguments.ContainsKey("InputObject"))
			{
				flag = true;
				enumerable = CompletionCompleters.NativeCommandArgumentCompletion_InferTypesOfArugment(boundArguments, commandAst, context, "InputObject");
			}
			if (enumerable != null)
			{
				foreach (PSTypeName typename in enumerable)
				{
					if (CompletionCompleters.NativeCompletionCimCommands_ParseTypeName(typename, out text, out text2))
					{
						if (parameter.Equals("ResultClassName", StringComparison.OrdinalIgnoreCase))
						{
							CompletionCompleters.NativeCompletionCimAssociationResultClassName(text, text2, result, context);
						}
						else if (parameter.Equals("MethodName", StringComparison.OrdinalIgnoreCase))
						{
							CompletionCompleters.NativeCompletionCimMethodName(text, text2, !flag, result, context);
						}
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x001DA8C4 File Offset: 0x001D8AC4
		private static IEnumerable<string> NativeCompletionCimAssociationResultClassName_GetResultClassNames(string cimNamespaceOfSource, string cimClassNameOfSource)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in cimClassNameOfSource)
			{
				if (char.IsLetterOrDigit(c) || c == '_')
				{
					stringBuilder.Append(c);
				}
			}
			List<string> list = new List<string>();
			using (CimSession cimSession = CimSession.Create(null))
			{
				for (CimClass cimClass = cimSession.GetClass(cimNamespaceOfSource ?? "root/cimv2", cimClassNameOfSource); cimClass != null; cimClass = cimClass.CimSuperClass)
				{
					string queryExpression = string.Format(CultureInfo.InvariantCulture, "associators of {{{0}}} WHERE SchemaOnly", new object[]
					{
						cimClass.CimSystemProperties.ClassName
					});
					list.AddRange(from associationInstance in cimSession.QueryInstances(cimNamespaceOfSource ?? "root/cimv2", "WQL", queryExpression)
					select associationInstance.CimSystemProperties.ClassName);
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			return list.Distinct(StringComparer.OrdinalIgnoreCase).ToList<string>();
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x001DAA38 File Offset: 0x001D8C38
		private static void NativeCompletionCimAssociationResultClassName(string pseudoboundNamespace, string pseudoboundClassName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrWhiteSpace(pseudoboundClassName))
			{
				return;
			}
			IEnumerable<string> orAdd = CompletionCompleters.cimNamespaceAndClassNameToAssociationResultClassNames.GetOrAdd((pseudoboundNamespace ?? "root/cimv2") + ":" + pseudoboundClassName, (string _) => CompletionCompleters.NativeCompletionCimAssociationResultClassName_GetResultClassNames(pseudoboundNamespace, pseudoboundClassName));
			WildcardPattern @object = new WildcardPattern(context.WordToComplete + "*", WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			result.AddRange(from x in orAdd.Where(new Func<string, bool>(@object.IsMatch))
			select new CompletionResult(x, x, CompletionResultType.Type, string.Format(CultureInfo.InvariantCulture, "{0} -> {1}", new object[]
			{
				pseudoboundClassName,
				x
			})));
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x001DAB0C File Offset: 0x001D8D0C
		private static void NativeCompletionCimMethodName(string pseudoboundNamespace, string pseudoboundClassName, bool staticMethod, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrWhiteSpace(pseudoboundClassName))
			{
				return;
			}
			CimClass @class;
			using (CimSession cimSession = CimSession.Create(null))
			{
				@class = cimSession.GetClass(pseudoboundNamespace ?? "root/cimv2", pseudoboundClassName);
			}
			WildcardPattern wildcardPattern = new WildcardPattern(context.WordToComplete + "*", WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			List<CompletionResult> list = new List<CompletionResult>();
			foreach (CimMethodDeclaration cimMethodDeclaration in @class.CimClassMethods)
			{
				string name = cimMethodDeclaration.Name;
				if (wildcardPattern.IsMatch(name))
				{
					bool flag = cimMethodDeclaration.Qualifiers.Any((CimQualifier q) => q.Name.Equals("Static", StringComparison.OrdinalIgnoreCase));
					if ((!flag || staticMethod) && (flag || !staticMethod))
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(name);
						stringBuilder.Append("(");
						bool flag2 = false;
						foreach (CimMethodParameterDeclaration cimMethodParameterDeclaration in cimMethodDeclaration.Parameters)
						{
							bool flag3 = cimMethodParameterDeclaration.Qualifiers.Any((CimQualifier q) => q.Name.Equals("Out", StringComparison.OrdinalIgnoreCase));
							if (!flag2)
							{
								flag2 = true;
							}
							else
							{
								stringBuilder.Append(", ");
							}
							if (flag3)
							{
								stringBuilder.Append("[out] ");
							}
							stringBuilder.Append(CimInstanceAdapter.CimTypeToTypeNameDisplayString(cimMethodParameterDeclaration.CimType));
							stringBuilder.Append(" ");
							stringBuilder.Append(cimMethodParameterDeclaration.Name);
						}
						stringBuilder.Append(")");
						list.Add(new CompletionResult(name, name, CompletionResultType.Method, stringBuilder.ToString()));
					}
				}
			}
			result.AddRange(list.OrderBy((CompletionResult x) => x.ListItemText, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x001DAD6C File Offset: 0x001D8F6C
		private static IEnumerable<string> NativeCompletionCimClassName_GetClassNames(string targetNamespace)
		{
			List<string> list = new List<string>();
			using (CimSession cimSession = CimSession.Create(null))
			{
				using (CimOperationOptions cimOperationOptions = new CimOperationOptions
				{
					ClassNamesOnly = true
				})
				{
					foreach (CimClass cimClass in cimSession.EnumerateClasses(targetNamespace, null, cimOperationOptions))
					{
						using (cimClass)
						{
							string className = cimClass.CimSystemProperties.ClassName;
							list.Add(className);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x001DAE60 File Offset: 0x001D9060
		private static void NativeCompletionCimClassName(string pseudoBoundNamespace, List<CompletionResult> result, CompletionContext context)
		{
			string targetNamespace = pseudoBoundNamespace ?? "root/cimv2";
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			IEnumerable<string> orAdd = CompletionCompleters.cimNamespaceToClassNames.GetOrAdd(targetNamespace, new Func<string, IEnumerable<string>>(CompletionCompleters.NativeCompletionCimClassName_GetClassNames));
			WildcardPattern wildcardPattern = new WildcardPattern(context.WordToComplete + "*", WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			foreach (string text in orAdd)
			{
				if (context.Helper.CancelTabCompletion)
				{
					break;
				}
				if (wildcardPattern.IsMatch(text))
				{
					if (text.Length > 0 && text[0] == '_')
					{
						list2.Add(text);
					}
					else
					{
						list.Add(text);
					}
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			list2.Sort(StringComparer.OrdinalIgnoreCase);
			result.AddRange(from className in list.Concat(list2)
			select new CompletionResult(className, className, CompletionResultType.Type, targetNamespace + ":" + className));
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x001DAF7C File Offset: 0x001D917C
		private static void NativeCompletionCimNamespace(List<CompletionResult> result, CompletionContext context)
		{
			string text = "root";
			string str = "";
			if (!string.IsNullOrEmpty(context.WordToComplete))
			{
				int num = context.WordToComplete.LastIndexOfAny(new char[]
				{
					'\\',
					'/'
				});
				if (num != -1)
				{
					text = context.WordToComplete.Substring(0, num);
					str = context.WordToComplete.Substring(num + 1);
				}
			}
			List<CompletionResult> list = new List<CompletionResult>();
			WildcardPattern wildcardPattern = new WildcardPattern(str + "*", WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			using (CimSession cimSession = CimSession.Create(null))
			{
				foreach (CimInstance cimInstance in cimSession.EnumerateInstances(text, "__Namespace"))
				{
					using (cimInstance)
					{
						if (context.Helper.CancelTabCompletion)
						{
							break;
						}
						CimProperty cimProperty = cimInstance.CimInstanceProperties["Name"];
						if (cimProperty != null)
						{
							string text2 = cimProperty.Value as string;
							if (text2 != null)
							{
								if (wildcardPattern.IsMatch(text2))
								{
									list.Add(new CompletionResult(text + "/" + text2, text2, CompletionResultType.Namespace, text + "/" + text2));
								}
							}
						}
					}
				}
			}
			result.AddRange(list.OrderBy((CompletionResult x) => x.ListItemText, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001DB12C File Offset: 0x001D932C
		private static void NativeCompletionGetCommand(string commandName, string moduleName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (!string.IsNullOrEmpty(paramName) && paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				List<CompletionResult> list = CompletionCompleters.CompleteCommand(new CompletionContext
				{
					WordToComplete = commandName,
					Helper = context.Helper
				}, moduleName, CommandTypes.All);
				if (list != null)
				{
					result.AddRange(list);
				}
				if (moduleName == null)
				{
					HashSet<string> extension = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
					{
						".ps1"
					};
					List<CompletionResult> list2 = new List<CompletionResult>(CompletionCompleters.CompleteFilename(new CompletionContext
					{
						WordToComplete = commandName,
						Helper = context.Helper
					}, false, extension));
					if (list2.Count > 0)
					{
						result.AddRange(list2);
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (!string.IsNullOrEmpty(paramName) && paramName.Equals("Module", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				List<CompletionResult> list3 = CompletionCompleters.CompleteModuleName(new CompletionContext
				{
					WordToComplete = commandName,
					Helper = context.Helper
				}, true);
				if (list3 != null)
				{
					foreach (CompletionResult completionResult in list3)
					{
						if (!hashSet.Contains(completionResult.ToolTip))
						{
							hashSet.Add(completionResult.ToolTip);
							result.Add(completionResult);
						}
					}
				}
				list3 = CompletionCompleters.CompleteModuleName(new CompletionContext
				{
					WordToComplete = commandName,
					Helper = context.Helper
				}, false);
				if (list3 != null)
				{
					foreach (CompletionResult completionResult2 in list3)
					{
						if (!hashSet.Contains(completionResult2.ToolTip))
						{
							hashSet.Add(completionResult2.ToolTip);
							result.Add(completionResult2);
						}
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x001DB348 File Offset: 0x001D9548
		private static void NativeCompletionGetHelpCommand(string commandName, string paramName, bool isHelpRelated, List<CompletionResult> result, CompletionContext context)
		{
			if (!string.IsNullOrEmpty(paramName) && paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				List<CompletionResult> list = CompletionCompleters.CompleteCommand(new CompletionContext
				{
					WordToComplete = commandName,
					Helper = context.Helper
				}, null, CommandTypes.Alias | CommandTypes.Function | CommandTypes.Cmdlet | CommandTypes.ExternalScript | CommandTypes.Workflow | CommandTypes.Configuration);
				if (list != null)
				{
					result.AddRange(list);
				}
				HashSet<string> extension = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					".ps1"
				};
				List<CompletionResult> list2 = new List<CompletionResult>(CompletionCompleters.CompleteFilename(new CompletionContext
				{
					WordToComplete = commandName,
					Helper = context.Helper
				}, false, extension));
				if (list2.Count > 0)
				{
					result.AddRange(list2);
				}
				if (isHelpRelated)
				{
					List<CompletionResult> list3 = CompletionCompleters.CompleteHelpTopics(new CompletionContext
					{
						WordToComplete = commandName,
						Helper = context.Helper
					});
					if (list3 != null)
					{
						result.AddRange(list3);
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x001DB444 File Offset: 0x001D9644
		private static void NativeCompletionEventLogCommands(string logName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (!string.IsNullOrEmpty(paramName) && paramName.Equals("LogName", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				logName = (logName ?? string.Empty);
				string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref logName);
				if (!logName.EndsWith("*", StringComparison.Ordinal))
				{
					logName += "*";
				}
				WildcardPattern arg = new WildcardPattern(logName, WildcardOptions.IgnoreCase);
				PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-EventLog", null).AddParameter("LogName", "*");
				Exception ex;
				Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null)
				{
					foreach (object arg2 in collection)
					{
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7c == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7c = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, object> target = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7c.Target;
						CallSite <>p__Site7c = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7c;
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7d == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7d = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Log", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj = target(<>p__Site7c, CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7d.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7d, arg2));
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7e == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7e = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target2 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7e.Target;
						CallSite <>p__Site7e = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7e;
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7f == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7f = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						if (target2(<>p__Site7e, CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7f.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site7f, typeof(CompletionCompleters), obj, false)))
						{
							string text2 = (text == string.Empty) ? "'" : text;
							if (text2 == "'")
							{
								if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site80 == null)
								{
									CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site80 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								obj = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site80.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site80, obj, "'", "''");
							}
							if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site81 == null)
							{
								CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site81 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site81.Target;
							CallSite <>p__Site = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site81;
							if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site82 == null)
							{
								CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site82 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target3(<>p__Site, CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site82.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site82, text2, obj), text2);
						}
						else
						{
							if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site83 == null)
							{
								CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site83 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target4 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site83.Target;
							CallSite <>p__Site2 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site83;
							if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site84 == null)
							{
								CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site84 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target4(<>p__Site2, CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site84.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site84, text, obj), text);
						}
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site85 == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site85 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target5 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site85.Target;
						CallSite <>p__Site3 = CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site85;
						if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site86 == null)
						{
							CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site86 = CallSite<Func<CallSite, WildcardPattern, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IsMatch", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						if (target5(<>p__Site3, CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site86.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site86, arg, obj2)))
						{
							if (CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site87 == null)
							{
								CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site87 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							result.Add(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site87.Target(CompletionCompleters.<NativeCompletionEventLogCommands>o__SiteContainer7b.<>p__Site87, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
						}
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x001DB9F0 File Offset: 0x001D9BF0
		private static void NativeCompletionJobCommands(string wordToComplete, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			wordToComplete = (wordToComplete ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!wordToComplete.EndsWith("*", StringComparison.Ordinal))
			{
				wordToComplete += "*";
			}
			WildcardPattern arg = new WildcardPattern(wordToComplete, WildcardOptions.IgnoreCase);
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-Job", typeof(GetJobCommand)).AddParameter("Name", wordToComplete);
			}
			else
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-Job", typeof(GetJobCommand)).AddParameter("IncludeChildJob", true);
			}
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			if (paramName.Equals("Id", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				foreach (object arg2 in collection)
				{
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site89 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site89 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site89.Target;
					CallSite <>p__Site = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site89;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8a == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8a = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Id", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj = target(<>p__Site, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8a.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8a, arg2));
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8b == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8b = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target2 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8b.Target;
					CallSite <>p__Site8b = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8b;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8c == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8c = CallSite<Func<CallSite, WildcardPattern, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IsMatch", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (target2(<>p__Site8b, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8c.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8c, arg, obj)))
					{
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8d == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8d = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8d.Target;
						CallSite <>p__Site8d = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8d;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8e == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8e = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target3(<>p__Site8d, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8e.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8e, text, obj), text);
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8f == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8f = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8f.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site8f, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("InstanceId", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				foreach (object arg3 in collection)
				{
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site90 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site90 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target4 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site90.Target;
					CallSite <>p__Site2 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site90;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site91 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site91 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "InstanceId", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj3 = target4(<>p__Site2, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site91.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site91, arg3));
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site92 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site92 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target5 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site92.Target;
					CallSite <>p__Site3 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site92;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site93 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site93 = CallSite<Func<CallSite, WildcardPattern, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IsMatch", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (target5(<>p__Site3, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site93.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site93, arg, obj3)))
					{
						object obj4 = obj3;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site94 == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site94 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target6 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site94.Target;
						CallSite <>p__Site4 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site94;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site95 == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site95 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj3 = target6(<>p__Site4, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site95.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site95, text, obj3), text);
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site96 == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site96 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site96.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site96, typeof(CompletionResult), obj3, obj4, CompletionResultType.ParameterValue, obj4));
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				foreach (object arg4 in collection)
				{
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site97 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site97 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj5 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site97.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site97, arg4);
					object obj6 = obj5;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site98 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site98 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target7 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site98.Target;
					CallSite <>p__Site5 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site98;
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site99 == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site99 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					if (target7(<>p__Site5, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site99.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site99, typeof(CompletionCompleters), obj5, false)))
					{
						string text2 = (text == string.Empty) ? "'" : text;
						if (text2 == "'")
						{
							if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9a == null)
							{
								CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9a = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							obj5 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9a.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9a, obj5, "'", "''");
						}
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9b == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9b = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target8 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9b.Target;
						CallSite <>p__Site9b = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9b;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9c == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9c = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj5 = target8(<>p__Site9b, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9c.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9c, text2, obj5), text2);
					}
					else
					{
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9d == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9d = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target9 = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9d.Target;
						CallSite <>p__Site9d = CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9d;
						if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9e == null)
						{
							CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9e = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj5 = target9(<>p__Site9d, CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9e.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9e, text, obj5), text);
					}
					if (CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9f == null)
					{
						CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9f = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					result.Add(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9f.Target(CompletionCompleters.<NativeCompletionJobCommands>o__SiteContainer88.<>p__Site9f, typeof(CompletionResult), obj5, obj6, CompletionResultType.ParameterValue, obj6));
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x001DC4D8 File Offset: 0x001DA6D8
		private static void NativeCompletionScheduledJobCommands(string wordToComplete, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			wordToComplete = (wordToComplete ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!wordToComplete.EndsWith("*", StringComparison.Ordinal))
			{
				wordToComplete += "*";
			}
			WildcardPattern arg = new WildcardPattern(wordToComplete, WildcardOptions.IgnoreCase);
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "PSScheduledJob\\Get-ScheduledJob", null).AddParameter("Name", wordToComplete);
			}
			else
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "PSScheduledJob\\Get-ScheduledJob", null);
			}
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			if (paramName.Equals("Id", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				foreach (object arg2 in collection)
				{
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea1 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea1.Target;
					CallSite <>p__Sitea = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea1;
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea2 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Id", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj = target(<>p__Sitea, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea2.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea2, arg2));
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea3 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea3 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target2 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea3.Target;
					CallSite <>p__Sitea2 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea3;
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea4 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea4 = CallSite<Func<CallSite, WildcardPattern, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IsMatch", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (target2(<>p__Sitea2, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea4.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea4, arg, obj)))
					{
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea5 == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea5.Target;
						CallSite <>p__Sitea3 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea5;
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea6 == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea6 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target3(<>p__Sitea3, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea6.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea6, text, obj), text);
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea7 == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea7 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea7.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea7, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				foreach (object arg3 in collection)
				{
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea8 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj3 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea8.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea8, arg3);
					object obj4 = obj3;
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea9 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea9 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target4 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea9.Target;
					CallSite <>p__Sitea4 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitea9;
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaa == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaa = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					if (target4(<>p__Sitea4, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaa.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaa, typeof(CompletionCompleters), obj3, false)))
					{
						string text2 = (text == string.Empty) ? "'" : text;
						if (text2 == "'")
						{
							if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteab == null)
							{
								CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteab = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							obj3 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteab.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteab, obj3, "'", "''");
						}
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteac == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteac = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target5 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteac.Target;
						CallSite <>p__Siteac = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteac;
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitead == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitead = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj3 = target5(<>p__Siteac, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitead.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Sitead, text2, obj3), text2);
					}
					else
					{
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteae == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteae = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target6 = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteae.Target;
						CallSite <>p__Siteae = CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteae;
						if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaf == null)
						{
							CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaf = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj3 = target6(<>p__Siteae, CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaf.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteaf, text, obj3), text);
					}
					if (CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteb0 == null)
					{
						CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteb0 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					result.Add(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteb0.Target(CompletionCompleters.<NativeCompletionScheduledJobCommands>o__SiteContainera0.<>p__Siteb0, typeof(CompletionResult), obj3, obj4, CompletionResultType.ParameterValue, obj4));
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x001DCCA8 File Offset: 0x001DAEA8
		private static void NativeCompletionModuleCommands(string assemblyOrModuleName, string paramName, bool loadedModulesOnly, bool isImportModule, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				if (isImportModule)
				{
					HashSet<string> extension = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
					{
						".ps1",
						".psm1",
						".psd1",
						".dll",
						".cdxml",
						".xaml"
					};
					List<CompletionResult> list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(new CompletionContext
					{
						WordToComplete = assemblyOrModuleName,
						Helper = context.Helper
					}, false, extension));
					if (list.Count > 0)
					{
						result.AddRange(list);
					}
					if (assemblyOrModuleName.IndexOfAny(new char[]
					{
						'\\',
						'/',
						':'
					}) != -1)
					{
						return;
					}
				}
				List<CompletionResult> list2 = CompletionCompleters.CompleteModuleName(new CompletionContext
				{
					WordToComplete = assemblyOrModuleName,
					Helper = context.Helper
				}, loadedModulesOnly);
				if (list2 != null && list2.Count > 0)
				{
					result.AddRange(list2);
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("Assembly", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				HashSet<string> extension2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					".dll"
				};
				List<CompletionResult> list3 = new List<CompletionResult>(CompletionCompleters.CompleteFilename(new CompletionContext
				{
					WordToComplete = assemblyOrModuleName,
					Helper = context.Helper
				}, false, extension2));
				if (list3.Count > 0)
				{
					result.AddRange(list3);
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x001DCE50 File Offset: 0x001DB050
		private static void NativeCompletionProcessCommands(string wordToComplete, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			wordToComplete = (wordToComplete ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!wordToComplete.EndsWith("*", StringComparison.Ordinal))
			{
				wordToComplete += "*";
			}
			if (paramName.Equals("Id", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Process", null);
			}
			else
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Process", null).AddParameter("Name", wordToComplete);
			}
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			if (paramName.Equals("Id", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				WildcardPattern arg = new WildcardPattern(wordToComplete, WildcardOptions.IgnoreCase);
				foreach (object arg2 in collection)
				{
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb7 == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb7 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object> target = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb7.Target;
					CallSite <>p__Siteb = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb7;
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb8 == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Id", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj = target(<>p__Siteb, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb8.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb8, arg2));
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb9 == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb9 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target2 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb9.Target;
					CallSite <>p__Siteb2 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteb9;
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteba == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteba = CallSite<Func<CallSite, WildcardPattern, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IsMatch", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (target2(<>p__Siteb2, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteba.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Siteba, arg, obj)))
					{
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebb == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebb = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebb.Target;
						CallSite <>p__Sitebb = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebb;
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebc == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebc = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target3(<>p__Sitebb, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebc.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebc, text, obj), text);
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebd == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebd = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebd.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebd, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				HashSet<string> arg3 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (object arg4 in collection)
				{
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebe == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebe = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj3 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebe.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebe, arg4);
					object obj4 = obj3;
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebf == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebf = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target4 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebf.Target;
					CallSite <>p__Sitebf = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitebf;
					if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec0 == null)
					{
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec0 = CallSite<Func<CallSite, HashSet<string>, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Contains", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (!target4(<>p__Sitebf, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec0.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec0, arg3, obj3)))
					{
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec1 == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec1 = CallSite<Action<CallSite, HashSet<string>, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec1.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec1, arg3, obj3);
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec2 == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec2 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target5 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec2.Target;
						CallSite <>p__Sitec = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec2;
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec3 == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec3 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						if (target5(<>p__Sitec, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec3.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec3, typeof(CompletionCompleters), obj3, false)))
						{
							string text2 = (text == string.Empty) ? "'" : text;
							if (text2 == "'")
							{
								if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec4 == null)
								{
									CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec4 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								obj3 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec4.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec4, obj3, "'", "''");
							}
							if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec5 == null)
							{
								CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target6 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec5.Target;
							CallSite <>p__Sitec2 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec5;
							if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec6 == null)
							{
								CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec6 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj3 = target6(<>p__Sitec2, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec6.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec6, text2, obj3), text2);
						}
						else
						{
							if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec7 == null)
							{
								CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec7 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target7 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec7.Target;
							CallSite <>p__Sitec3 = CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec7;
							if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec8 == null)
							{
								CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec8 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj3 = target7(<>p__Sitec3, CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec8.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec8, text, obj3), text);
						}
						if (CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec9 == null)
						{
							CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec9 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec9.Target(CompletionCompleters.<NativeCompletionProcessCommands>o__SiteContainerb6.<>p__Sitec9, typeof(CompletionResult), obj3, obj4, CompletionResultType.ParameterValue, obj4));
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x001DD730 File Offset: 0x001DB930
		private static void NativeCompletionProviderCommands(string providerName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || !paramName.Equals("PSProvider", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			providerName = (providerName ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref providerName);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!providerName.EndsWith("*", StringComparison.Ordinal))
			{
				providerName += "*";
			}
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-PSProvider", null).AddParameter("PSProvider", providerName);
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			foreach (object arg in collection)
			{
				if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecb == null)
				{
					CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecb = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecb.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecb, arg);
				object obj2 = obj;
				if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecc == null)
				{
					CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecc = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecc.Target;
				CallSite <>p__Sitecc = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecc;
				if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecd == null)
				{
					CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecd = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				if (target(<>p__Sitecc, CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecd.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecd, typeof(CompletionCompleters), obj, false)))
				{
					string text2 = (text == string.Empty) ? "'" : text;
					if (text2 == "'")
					{
						if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitece == null)
						{
							CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitece = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						obj = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitece.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitece, obj, "'", "''");
					}
					if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecf == null)
					{
						CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecf = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target2 = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecf.Target;
					CallSite <>p__Sitecf = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sitecf;
					if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited0 == null)
					{
						CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited0 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target2(<>p__Sitecf, CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited0.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited0, text2, obj), text2);
				}
				else
				{
					if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited1 == null)
					{
						CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited1 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited1.Target;
					CallSite <>p__Sited = CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited1;
					if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited2 == null)
					{
						CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited2 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target3(<>p__Sited, CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited2.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited2, text, obj), text);
				}
				if (CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited3 == null)
				{
					CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited3 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				result.Add(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited3.Target(CompletionCompleters.<NativeCompletionProviderCommands>o__SiteContainerca.<>p__Sited3, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x001DDBC8 File Offset: 0x001DBDC8
		private static void NativeCompletionDriveCommands(string wordToComplete, string psProvider, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || !paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			wordToComplete = (wordToComplete ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!wordToComplete.EndsWith("*", StringComparison.Ordinal))
			{
				wordToComplete += "*";
			}
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-PSDrive", null).AddParameter("Name", wordToComplete);
			if (psProvider != null)
			{
				currentPowerShell.AddParameter("PSProvider", psProvider);
			}
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection != null)
			{
				foreach (object arg in collection)
				{
					if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited5 == null)
					{
						CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited5 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object obj = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited5.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited5, arg);
					object obj2 = obj;
					if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited6 == null)
					{
						CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited6 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited6.Target;
					CallSite <>p__Sited = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited6;
					if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited7 == null)
					{
						CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited7 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					if (target(<>p__Sited, CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited7.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited7, typeof(CompletionCompleters), obj, false)))
					{
						string text2 = (text == string.Empty) ? "'" : text;
						if (text2 == "'")
						{
							if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited8 == null)
							{
								CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited8 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							obj = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited8.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited8, obj, "'", "''");
						}
						if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited9 == null)
						{
							CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited9 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target2 = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited9.Target;
						CallSite <>p__Sited2 = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sited9;
						if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Siteda == null)
						{
							CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Siteda = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target2(<>p__Sited2, CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Siteda.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Siteda, text2, obj), text2);
					}
					else
					{
						if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedb == null)
						{
							CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedb = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedb.Target;
						CallSite <>p__Sitedb = CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedb;
						if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedc == null)
						{
							CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedc = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						obj = target3(<>p__Sitedb, CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedc.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedc, text, obj), text);
					}
					if (CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedd == null)
					{
						CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedd = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					result.Add(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedd.Target(CompletionCompleters.<NativeCompletionDriveCommands>o__SiteContainerd4.<>p__Sitedd, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
				}
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x001DE074 File Offset: 0x001DC274
		private static void NativeCompletionServiceCommands(string wordToComplete, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			wordToComplete = (wordToComplete ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref wordToComplete);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!wordToComplete.EndsWith("*", StringComparison.Ordinal))
			{
				wordToComplete += "*";
			}
			if (paramName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Service", null).AddParameter("DisplayName", wordToComplete);
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", "DisplayName");
				Exception ex;
				Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null)
				{
					foreach (object arg in collection)
					{
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitedf == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitedf = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "DisplayName", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitedf.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitedf, arg);
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee0 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee0 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee0.Target;
						CallSite <>p__Sitee = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee0;
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee1 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee1 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						if (target(<>p__Sitee, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee1.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee1, typeof(CompletionCompleters), obj, false)))
						{
							string text2 = (text == string.Empty) ? "'" : text;
							if (text2 == "'")
							{
								if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee2 == null)
								{
									CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee2 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								obj = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee2.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee2, obj, "'", "''");
							}
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee3 == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee3 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target2 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee3.Target;
							CallSite <>p__Sitee2 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee3;
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee4 == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee4 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target2(<>p__Sitee2, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee4.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee4, text2, obj), text2);
						}
						else
						{
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee5 == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee5.Target;
							CallSite <>p__Sitee3 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee5;
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee6 == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee6 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target3(<>p__Sitee3, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee6.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee6, text, obj), text);
						}
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee7 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee7 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee7.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee7, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
					}
				}
				result.Add(CompletionResult.Null);
				return;
			}
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Service", null).AddParameter("Name", wordToComplete);
				Exception ex;
				Collection<PSObject> collection2 = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection2 != null)
				{
					foreach (object arg2 in collection2)
					{
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee8 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj3 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee8.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee8, arg2);
						object obj4 = obj3;
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee9 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee9 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target4 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee9.Target;
						CallSite <>p__Sitee4 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitee9;
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteea == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteea = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						if (target4(<>p__Sitee4, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteea.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteea, typeof(CompletionCompleters), obj3, false)))
						{
							string text3 = (text == string.Empty) ? "'" : text;
							if (text3 == "'")
							{
								if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteeb == null)
								{
									CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteeb = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								obj3 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteeb.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteeb, obj3, "'", "''");
							}
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteec == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteec = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target5 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteec.Target;
							CallSite <>p__Siteec = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteec;
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteed == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteed = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj3 = target5(<>p__Siteec, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteed.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteed, text3, obj3), text3);
						}
						else
						{
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteee == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteee = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target6 = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteee.Target;
							CallSite <>p__Siteee = CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteee;
							if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteef == null)
							{
								CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteef = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj3 = target6(<>p__Siteee, CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteef.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Siteef, text, obj3), text);
						}
						if (CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitef0 == null)
						{
							CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitef0 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitef0.Target(CompletionCompleters.<NativeCompletionServiceCommands>o__SiteContainerde.<>p__Sitef0, typeof(CompletionResult), obj3, obj4, CompletionResultType.ParameterValue, obj4));
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x001DE974 File Offset: 0x001DCB74
		private static void NativeCompletionVariableCommands(string variableName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || !paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			variableName = (variableName ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref variableName);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!variableName.EndsWith("*", StringComparison.Ordinal))
			{
				variableName += "*";
			}
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Get-Variable", null).AddParameter("Name", variableName);
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			foreach (object arg in collection)
			{
				string text2 = text;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef2 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef2.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef2, arg);
				object obj2 = obj;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef3 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef3 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef3.Target;
				CallSite <>p__Sitef = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef3;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef4 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef4 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, int, object> target2 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef4.Target;
				CallSite <>p__Sitef2 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef4;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef5 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef5 = CallSite<Func<CallSite, object, char[], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "IndexOfAny", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				if (target(<>p__Sitef, target2(<>p__Sitef2, CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef5.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef5, obj, new char[]
				{
					'?',
					'*'
				}), -1)))
				{
					text2 = "'";
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef6 == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef6 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					obj = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef6.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef6, obj, "?", "`?");
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef7 == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef7 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					obj = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef7.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef7, obj, "*", "`*");
				}
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef8 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef8 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target3 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef8.Target;
				CallSite <>p__Sitef3 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef8;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef9 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef9 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target4 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef9.Target;
				CallSite <>p__Sitef4 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitef9;
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefa == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefa = CallSite<Func<CallSite, object, string, StringComparison, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Equals", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				object obj3 = target4(<>p__Sitef4, CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefa.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefa, obj, "$", StringComparison.Ordinal));
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefb == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefb = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg3;
				if (!CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefb.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefb, obj3))
				{
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefc == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefc = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, object, object> target5 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefc.Target;
					CallSite <>p__Sitefc = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefc;
					object arg2 = obj3;
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefd == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefd = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					arg3 = target5(<>p__Sitefc, arg2, CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefd.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefd, typeof(CompletionCompleters), obj, false));
				}
				else
				{
					arg3 = obj3;
				}
				if (target3(<>p__Sitef3, arg3))
				{
					string text3 = (text2 == string.Empty) ? "'" : text2;
					if (text3 == "'")
					{
						if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefe == null)
						{
							CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefe = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						obj = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefe.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Sitefe, obj, "'", "''");
					}
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Siteff == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Siteff = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target6 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Siteff.Target;
					CallSite <>p__Siteff = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Siteff;
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site100 == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site100 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target6(<>p__Siteff, CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site100.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site100, text3, obj), text3);
				}
				else
				{
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site101 == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site101 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target7 = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site101.Target;
					CallSite <>p__Site = CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site101;
					if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site102 == null)
					{
						CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site102 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target7(<>p__Site, CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site102.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site102, text2, obj), text2);
				}
				if (CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site103 == null)
				{
					CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site103 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				result.Add(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site103.Target(CompletionCompleters.<NativeCompletionVariableCommands>o__SiteContainerf1.<>p__Site103, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x06005979 RID: 22905 RVA: 0x001DF188 File Offset: 0x001DD388
		private static void NativeCompletionAliasCommands(string commandName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || (!paramName.Equals("Definition", StringComparison.OrdinalIgnoreCase) && !paramName.Equals("Name", StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			if (paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				commandName = (commandName ?? string.Empty);
				string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref commandName);
				PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
				if (!commandName.EndsWith("*", StringComparison.Ordinal))
				{
					commandName += "*";
				}
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Get-Alias", null).AddParameter("Name", commandName);
				Exception ex;
				Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection == null)
				{
					goto IL_502;
				}
				using (IEnumerator<PSObject> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object arg = enumerator.Current;
						if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site107 == null)
						{
							CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site107 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						object obj = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site107.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site107, arg);
						object obj2 = obj;
						if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site108 == null)
						{
							CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site108 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site108.Target;
						CallSite <>p__Site = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site108;
						if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site109 == null)
						{
							CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site109 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						if (target(<>p__Site, CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site109.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site109, typeof(CompletionCompleters), obj, false)))
						{
							string text2 = (text == string.Empty) ? "'" : text;
							if (text2 == "'")
							{
								if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10a == null)
								{
									CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10a = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
									}));
								}
								obj = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10a.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10a, obj, "'", "''");
							}
							if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10b == null)
							{
								CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10b = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target2 = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10b.Target;
							CallSite <>p__Site10b = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10b;
							if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10c == null)
							{
								CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10c = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target2(<>p__Site10b, CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10c.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10c, text2, obj), text2);
						}
						else
						{
							if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10d == null)
							{
								CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10d = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10d.Target;
							CallSite <>p__Site10d = CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10d;
							if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10e == null)
							{
								CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10e = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							obj = target3(<>p__Site10d, CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10e.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10e, text, obj), text);
						}
						if (CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10f == null)
						{
							CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10f = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						result.Add(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10f.Target(CompletionCompleters.<NativeCompletionAliasCommands>o__SiteContainer106.<>p__Site10f, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
					}
					goto IL_502;
				}
			}
			List<CompletionResult> list = CompletionCompleters.CompleteCommand(new CompletionContext
			{
				WordToComplete = commandName,
				Helper = context.Helper
			}, null, CommandTypes.Function | CommandTypes.Cmdlet | CommandTypes.ExternalScript | CommandTypes.Workflow | CommandTypes.Configuration);
			if (list != null && list.Count > 0)
			{
				result.AddRange(list);
			}
			List<CompletionResult> list2 = new List<CompletionResult>(CompletionCompleters.CompleteFilename(new CompletionContext
			{
				WordToComplete = commandName,
				Helper = context.Helper
			}));
			if (list2.Count > 0)
			{
				result.AddRange(list2);
			}
			IL_502:
			result.Add(CompletionResult.Null);
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x001DF6C0 File Offset: 0x001DD8C0
		private static void NativeCompletionTraceSourceCommands(string traceSourceName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || !paramName.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			traceSourceName = (traceSourceName ?? string.Empty);
			string text = CompletionCompleters.HandleDoubleAndSingleQuote(ref traceSourceName);
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			if (!traceSourceName.EndsWith("*", StringComparison.Ordinal))
			{
				traceSourceName += "*";
			}
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Get-TraceSource", null).AddParameter("Name", traceSourceName);
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection == null)
			{
				return;
			}
			foreach (object arg in collection)
			{
				if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site111 == null)
				{
					CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site111 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site111.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site111, arg);
				object obj2 = obj;
				if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site112 == null)
				{
					CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site112 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> target = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site112.Target;
				CallSite <>p__Site = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site112;
				if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site113 == null)
				{
					CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site113 = CallSite<Func<CallSite, Type, object, bool, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CompletionRequiresQuotes", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				if (target(<>p__Site, CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site113.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site113, typeof(CompletionCompleters), obj, false)))
				{
					string text2 = (text == string.Empty) ? "'" : text;
					if (text2 == "'")
					{
						if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site114 == null)
						{
							CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site114 = CallSite<Func<CallSite, object, string, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Replace", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						obj = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site114.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site114, obj, "'", "''");
					}
					if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site115 == null)
					{
						CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site115 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target2 = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site115.Target;
					CallSite <>p__Site2 = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site115;
					if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site116 == null)
					{
						CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site116 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target2(<>p__Site2, CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site116.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site116, text2, obj), text2);
				}
				else
				{
					if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site117 == null)
					{
						CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site117 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					Func<CallSite, object, string, object> target3 = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site117.Target;
					CallSite <>p__Site3 = CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site117;
					if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site118 == null)
					{
						CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site118 = CallSite<Func<CallSite, string, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					obj = target3(<>p__Site3, CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site118.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site118, text, obj), text);
				}
				if (CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site119 == null)
				{
					CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site119 = CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CompletionCompleters), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				result.Add(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site119.Target(CompletionCompleters.<NativeCompletionTraceSourceCommands>o__SiteContainer110.<>p__Site119, typeof(CompletionResult), obj, obj2, CompletionResultType.ParameterValue, obj2));
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x001DFB58 File Offset: 0x001DDD58
		private static void NativeCompletionSetLocationCommand(string dirName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || (!paramName.Equals("Path", StringComparison.OrdinalIgnoreCase) && !paramName.Equals("LiteralPath", StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			context.WordToComplete = (dirName ?? string.Empty);
			bool flag = false;
			if (paramName.Equals("LiteralPath", StringComparison.OrdinalIgnoreCase))
			{
				flag = CompletionCompleters.TurnOnLiteralPathOption(context);
			}
			try
			{
				IEnumerable<CompletionResult> enumerable = CompletionCompleters.CompleteFilename(context, true, null);
				if (enumerable != null)
				{
					result.AddRange(enumerable);
				}
			}
			finally
			{
				if (flag)
				{
					context.Options.Remove("LiteralPaths");
				}
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x001DFBFC File Offset: 0x001DDDFC
		private static void NativeCompletionNewItemCommand(string itemTypeToComplete, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			ExecutionContext contextFromTLS = currentPowerShell.GetContextFromTLS();
			Hashtable boundArgumentsAsHashtable = CompletionCompleters.GetBoundArgumentsAsHashtable(context);
			string path = (boundArgumentsAsHashtable["Path"] as string) ?? contextFromTLS.SessionState.Path.CurrentLocation.Path;
			ProviderInfo providerInfo;
			contextFromTLS.LocationGlobber.GetProviderPath(path, out providerInfo);
			bool flag = providerInfo != null && providerInfo.Name.Equals("FileSystem", StringComparison.OrdinalIgnoreCase);
			if (flag && paramName.Equals("ItemType", StringComparison.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrEmpty(itemTypeToComplete))
				{
					WildcardPattern wildcardPattern = new WildcardPattern(itemTypeToComplete + "*", WildcardOptions.IgnoreCase);
					if (wildcardPattern.IsMatch("file"))
					{
						result.Add(new CompletionResult("File"));
					}
					else if (wildcardPattern.IsMatch("directory"))
					{
						result.Add(new CompletionResult("Directory"));
					}
					else if (wildcardPattern.IsMatch("symboliclink"))
					{
						result.Add(new CompletionResult("SymbolicLink"));
					}
					else if (wildcardPattern.IsMatch("junction"))
					{
						result.Add(new CompletionResult("Junction"));
					}
					else if (wildcardPattern.IsMatch("hardlink"))
					{
						result.Add(new CompletionResult("HardLink"));
					}
				}
				else
				{
					result.Add(new CompletionResult("File"));
					result.Add(new CompletionResult("Directory"));
					result.Add(new CompletionResult("SymbolicLink"));
					result.Add(new CompletionResult("Junction"));
					result.Add(new CompletionResult("HardLink"));
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x001DFDBC File Offset: 0x001DDFBC
		private static void NativeCompletionCopyMoveItemCommand(string pathName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				return;
			}
			if (paramName.Equals("LiteralPath", StringComparison.OrdinalIgnoreCase) || paramName.Equals("Path", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.NativeCompletionPathArgument(pathName, paramName, result, context);
				return;
			}
			if (paramName.Equals("Destination", StringComparison.OrdinalIgnoreCase))
			{
				CompletionCompleters.RemoveLastNullCompletionResult(result);
				context.WordToComplete = (pathName ?? string.Empty);
				bool flag = CompletionCompleters.TurnOnLiteralPathOption(context);
				try
				{
					IEnumerable<CompletionResult> enumerable = CompletionCompleters.CompleteFilename(context);
					if (enumerable != null)
					{
						result.AddRange(enumerable);
					}
				}
				finally
				{
					if (flag)
					{
						context.Options.Remove("LiteralPaths");
					}
				}
				result.Add(CompletionResult.Null);
			}
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x001DFE68 File Offset: 0x001DE068
		private static void NativeCompletionPathArgument(string pathName, string paramName, List<CompletionResult> result, CompletionContext context)
		{
			if (string.IsNullOrEmpty(paramName) || (!paramName.Equals("LiteralPath", StringComparison.OrdinalIgnoreCase) && !paramName.Equals("Path", StringComparison.OrdinalIgnoreCase) && !paramName.Equals("FilePath", StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			CompletionCompleters.RemoveLastNullCompletionResult(result);
			context.WordToComplete = (pathName ?? string.Empty);
			bool flag = false;
			if (paramName.Equals("LiteralPath", StringComparison.OrdinalIgnoreCase))
			{
				flag = CompletionCompleters.TurnOnLiteralPathOption(context);
			}
			try
			{
				IEnumerable<CompletionResult> enumerable = CompletionCompleters.CompleteFilename(context);
				if (enumerable != null)
				{
					result.AddRange(enumerable);
				}
			}
			finally
			{
				if (flag)
				{
					context.Options.Remove("LiteralPaths");
				}
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x001DFF18 File Offset: 0x001DE118
		private static void NativeCompletionMemberName(string wordToComplete, List<CompletionResult> result, CommandAst commandAst, CompletionContext context)
		{
			PipelineAst pipelineAst = commandAst.Parent as PipelineAst;
			if (pipelineAst == null)
			{
				return;
			}
			int num = 0;
			while (num < pipelineAst.PipelineElements.Count && pipelineAst.PipelineElements[num] != commandAst)
			{
				num++;
			}
			IEnumerable<PSTypeName> inferredType;
			if (num == 0)
			{
				AstParameterArgumentPair astParameterArgumentPair;
				if (!context.PseudoBindingInfo.BoundArguments.TryGetValue("InputObject", out astParameterArgumentPair) || !astParameterArgumentPair.ArgumentSpecified)
				{
					return;
				}
				AstPair astPair = astParameterArgumentPair as AstPair;
				if (astPair == null || astPair.Argument == null)
				{
					return;
				}
				inferredType = astPair.Argument.GetInferredType(context);
			}
			else
			{
				inferredType = pipelineAst.PipelineElements[num - 1].GetInferredType(context);
			}
			CompletionCompleters.CompleteMemberByInferredType(context, inferredType, result, wordToComplete + "*", new Func<object, bool>(CompletionCompleters.IsPropertyMember), false);
			result.Add(CompletionResult.Null);
		}

		// Token: 0x06005980 RID: 22912 RVA: 0x001DFFE8 File Offset: 0x001DE1E8
		private static void NativeCompletionTypeName(CompletionContext context, List<CompletionResult> result)
		{
			string text = context.WordToComplete;
			bool flag = text.Length > 0 && (text[0].IsSingleQuote() || text[0].IsDoubleQuote());
			string text2 = "";
			string text3 = "";
			if (flag)
			{
				text3 = (text2 = text.Substring(0, 1));
				bool flag2 = text.Length > 1 && text[text.Length - 1] == text[0];
				text = text.Substring(1, text.Length - (flag2 ? 2 : 1));
			}
			if (text.IndexOf('[') != -1)
			{
				InternalScriptPosition internalScriptPosition = (InternalScriptPosition)context.CursorPosition;
				internalScriptPosition = internalScriptPosition.CloneWithNewOffset(internalScriptPosition.Offset - context.TokenAtCursor.Extent.StartOffset - (flag ? 1 : 0));
				ITypeName typeName = Parser.ScanType(text, true);
				TypeName typeName2 = CompletionAnalysis.FindTypeNameToComplete(typeName, internalScriptPosition);
				if (typeName2 == null)
				{
					return;
				}
				int num = 0;
				int num2 = 0;
				foreach (char c in text)
				{
					if (c == '[')
					{
						num++;
					}
					else if (c == ']')
					{
						num2++;
					}
				}
				text = typeName2.FullName;
				string text5 = typeName.Extent.Text;
				if (!flag)
				{
					text3 = (text2 = "'");
				}
				if (num2 < num)
				{
					text3 = text3.Insert(0, new string(']', num - num2));
				}
				if (flag && num2 == num)
				{
					context.ReplacementIndex = typeName2.Extent.StartOffset + context.TokenAtCursor.Extent.StartOffset + 1;
					context.ReplacementLength = text.Length;
					text3 = (text2 = "");
				}
				else
				{
					text2 += text5.Substring(0, typeName2.Extent.StartOffset);
					text3 = text3.Insert(0, text5.Substring(typeName2.Extent.EndOffset));
				}
			}
			context.WordToComplete = text;
			List<CompletionResult> list = CompletionCompleters.CompleteType(context, text2, text3);
			if (list != null)
			{
				result.AddRange(list);
			}
			result.Add(CompletionResult.Null);
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x001E01FC File Offset: 0x001DE3FC
		private static AstPair FindTargetPositionalArgument(Collection<AstParameterArgumentPair> parsedArguments, int position, out AstPair lastPositionalArgument)
		{
			int num = 0;
			lastPositionalArgument = null;
			foreach (AstParameterArgumentPair astParameterArgumentPair in parsedArguments)
			{
				if (!astParameterArgumentPair.ParameterSpecified && num == position)
				{
					return (AstPair)astParameterArgumentPair;
				}
				if (!astParameterArgumentPair.ParameterSpecified)
				{
					num++;
					lastPositionalArgument = (AstPair)astParameterArgumentPair;
				}
			}
			return null;
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x001E0270 File Offset: 0x001DE470
		private static CompletionCompleters.ArgumentLocation FindTargetArgumentLocation(Collection<AstParameterArgumentPair> parsedArguments, Token token)
		{
			int num = 0;
			AstParameterArgumentPair prev = null;
			foreach (AstParameterArgumentPair astParameterArgumentPair in parsedArguments)
			{
				switch (astParameterArgumentPair.ParameterArgumentType)
				{
				case AstParameterArgumentType.AstPair:
				{
					AstPair astPair = (AstPair)astParameterArgumentPair;
					if (astPair.ParameterSpecified)
					{
						if (astPair.Parameter.Extent.StartOffset > token.Extent.StartOffset)
						{
							return CompletionCompleters.GenerateArgumentLocation(prev, num);
						}
						if (!astPair.ParameterContainsArgument && astPair.Argument.Extent.StartOffset > token.Extent.StartOffset)
						{
							return new CompletionCompleters.ArgumentLocation
							{
								Argument = astPair,
								IsPositional = false,
								Position = -1
							};
						}
					}
					else
					{
						if (astPair.Argument.Extent.StartOffset > token.Extent.StartOffset)
						{
							return CompletionCompleters.GenerateArgumentLocation(prev, num);
						}
						num++;
					}
					prev = astPair;
					break;
				}
				case AstParameterArgumentType.Switch:
				case AstParameterArgumentType.Fake:
					if (astParameterArgumentPair.Parameter.Extent.StartOffset > token.Extent.StartOffset)
					{
						return CompletionCompleters.GenerateArgumentLocation(prev, num);
					}
					prev = astParameterArgumentPair;
					break;
				}
			}
			return CompletionCompleters.GenerateArgumentLocation(prev, num);
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x001E03DC File Offset: 0x001DE5DC
		private static CompletionCompleters.ArgumentLocation GenerateArgumentLocation(AstParameterArgumentPair prev, int position)
		{
			if (prev == null)
			{
				return new CompletionCompleters.ArgumentLocation
				{
					Argument = null,
					IsPositional = true,
					Position = 0
				};
			}
			switch (prev.ParameterArgumentType)
			{
			case AstParameterArgumentType.AstPair:
			case AstParameterArgumentType.Switch:
				if (!prev.ParameterSpecified)
				{
					return new CompletionCompleters.ArgumentLocation
					{
						Argument = null,
						IsPositional = true,
						Position = position
					};
				}
				if (!prev.Parameter.Extent.Text.EndsWith(":", StringComparison.Ordinal))
				{
					return new CompletionCompleters.ArgumentLocation
					{
						Argument = null,
						IsPositional = true,
						Position = position
					};
				}
				return new CompletionCompleters.ArgumentLocation
				{
					Argument = prev,
					IsPositional = false,
					Position = -1
				};
			case AstParameterArgumentType.Fake:
				return new CompletionCompleters.ArgumentLocation
				{
					Argument = prev,
					IsPositional = false,
					Position = -1
				};
			default:
				return null;
			}
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x001E04C8 File Offset: 0x001DE6C8
		private static CompletionCompleters.ArgumentLocation FindTargetArgumentLocation(Collection<AstParameterArgumentPair> parsedArguments, ExpressionAst expAst)
		{
			int num = 0;
			foreach (AstParameterArgumentPair astParameterArgumentPair in parsedArguments)
			{
				switch (astParameterArgumentPair.ParameterArgumentType)
				{
				case AstParameterArgumentType.AstPair:
				{
					AstPair astPair = (AstPair)astParameterArgumentPair;
					if (!astPair.ArgumentIsCommandParameterAst)
					{
						if (astPair.ParameterContainsArgument && astPair.Argument == expAst)
						{
							return new CompletionCompleters.ArgumentLocation
							{
								IsPositional = false,
								Position = -1,
								Argument = astPair
							};
						}
						if (astPair.Argument.GetHashCode() == expAst.GetHashCode())
						{
							return astPair.ParameterSpecified ? new CompletionCompleters.ArgumentLocation
							{
								IsPositional = false,
								Position = -1,
								Argument = astPair
							} : new CompletionCompleters.ArgumentLocation
							{
								IsPositional = true,
								Position = num,
								Argument = astPair
							};
						}
						if (!astPair.ParameterSpecified)
						{
							num++;
						}
					}
					break;
				}
				}
			}
			return null;
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x001E05F8 File Offset: 0x001DE7F8
		public static IEnumerable<CompletionResult> CompleteFilename(string fileName)
		{
			if (Runspace.DefaultRunspace == null)
			{
				return CommandCompletion.EmptyCompletionResult;
			}
			CompletionExecutionHelper helper = new CompletionExecutionHelper(PowerShell.Create(RunspaceMode.CurrentRunspace));
			return CompletionCompleters.CompleteFilename(new CompletionContext
			{
				WordToComplete = fileName,
				Helper = helper
			});
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x001E063A File Offset: 0x001DE83A
		internal static IEnumerable<CompletionResult> CompleteFilename(CompletionContext context)
		{
			return CompletionCompleters.CompleteFilename(context, false, null);
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x001E0648 File Offset: 0x001DE848
		internal static IEnumerable<CompletionResult> CompleteFilename(CompletionContext context, bool containerOnly, HashSet<string> extension)
		{
			string text = context.WordToComplete;
			string text2 = CompletionCompleters.HandleDoubleAndSingleQuote(ref text);
			List<CompletionResult> list = new List<CompletionResult>();
			Match match = Regex.Match(text, "^\\\\\\\\([^\\\\]+)\\\\([^\\\\]*)$");
			if (match.Success)
			{
				string value = match.Groups[1].Value;
				WildcardPattern wildcardPattern = new WildcardPattern(match.Groups[2].Value + "*", WildcardOptions.IgnoreCase);
				bool option = context.GetOption("IgnoreHiddenShares", false);
				List<string> fileShares = CompletionCompleters.GetFileShares(value, option);
				using (List<string>.Enumerator enumerator = fileShares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text3 = enumerator.Current;
						if (wildcardPattern.IsMatch(text3))
						{
							string text4 = "\\\\" + value + "\\" + text3;
							if (text2 != string.Empty)
							{
								text4 = text2 + text4 + text2;
							}
							list.Add(new CompletionResult(text4, text4, CompletionResultType.ProviderContainer, text4));
						}
					}
					return list;
				}
			}
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			ExecutionContext contextFromTLS = currentPowerShell.GetContextFromTLS();
			string text5;
			bool flag = string.IsNullOrWhiteSpace(text) || (text.IndexOfAny(new char[]
			{
				'\\',
				'/'
			}) != 0 && !Regex.Match(text, "^~[\\\\/]+.*").Success && !contextFromTLS.LocationGlobber.IsAbsolutePath(text, out text5));
			bool option2 = context.GetOption("RelativePaths", flag);
			bool option3 = context.GetOption("LiteralPaths", false);
			if (option3 && LocationGlobber.StringContainsGlobCharacters(text))
			{
				text = WildcardPattern.Escape(text, new char[]
				{
					'*',
					'?'
				});
			}
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Resolve-Path", null).AddParameter("Path", text + "*");
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection != null)
			{
				bool flag2 = false;
				bool flag3 = CompletionCompleters.ProviderSpecified(text);
				if (collection.Count > 0)
				{
					object arg = collection[0];
					if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site125 == null)
					{
						CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site125 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Provider", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					ProviderInfo providerInfo = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site125.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site125, arg) as ProviderInfo;
					flag2 = (providerInfo != null && providerInfo.Name.Equals("FileSystem", StringComparison.OrdinalIgnoreCase));
				}
				else
				{
					try
					{
						ProviderInfo provider;
						if (flag)
						{
							provider = contextFromTLS.EngineSessionState.CurrentDrive.Provider;
						}
						else
						{
							contextFromTLS.LocationGlobber.GetProviderPath(text, out provider);
						}
						flag2 = (provider != null && provider.Name.Equals("FileSystem", StringComparison.OrdinalIgnoreCase));
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
				if (flag2)
				{
					bool flag4 = false;
					if (collection.Count > 0 && !LocationGlobber.StringContainsGlobCharacters(text))
					{
						string text6 = null;
						string path = flag3 ? text.Substring(text.IndexOf(':') + 2) : text;
						try
						{
							text6 = Path.GetFileName(path);
						}
						catch (Exception e2)
						{
							CommandProcessorBase.CheckForSevereException(e2);
						}
						HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						string text7 = null;
						foreach (object arg2 in collection)
						{
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site126 == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site126 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CompletionCompleters)));
							}
							Func<CallSite, object, string> target = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site126.Target;
							CallSite <>p__Site = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site126;
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site127 == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site127 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "ProviderPath", typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							text7 = target(<>p__Site, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site127.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site127, arg2));
							if (string.IsNullOrEmpty(text7))
							{
								text6 = null;
								break;
							}
							if (!hashSet.Contains(text7))
							{
								hashSet.Add(text7);
							}
						}
						if (text6 != null)
						{
							text6 += "*";
							string directoryName = Path.GetDirectoryName(text7);
							if (!string.IsNullOrEmpty(directoryName))
							{
								string[] array = null;
								try
								{
									array = Directory.GetFileSystemEntries(directoryName, text6);
								}
								catch (Exception e3)
								{
									CommandProcessorBase.CheckForSevereException(e3);
								}
								if (array != null)
								{
									flag4 = true;
									if (array.Length > hashSet.Count)
									{
										foreach (string text8 in array)
										{
											if (!hashSet.Contains(text8))
											{
												FileInfo fileInfo = new FileInfo(text8);
												if ((fileInfo.Attributes & FileAttributes.Hidden) != (FileAttributes)0)
												{
													PSObject item = PSObject.AsPSObject(text8);
													collection.Add(item);
												}
											}
										}
									}
								}
							}
						}
					}
					if (!flag4)
					{
						CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-ChildItem", null).AddParameter("Path", text + "*").AddParameter("Hidden", true);
						Collection<PSObject> collection2 = context.Helper.ExecuteCurrentPowerShell(out ex, null);
						if (collection2 != null && collection2.Count > 0)
						{
							foreach (PSObject item2 in collection2)
							{
								collection.Add(item2);
							}
						}
					}
				}
				IOrderedEnumerable<PSObject> orderedEnumerable = collection.OrderBy((PSObject a) => a, new CompletionCompleters.ItemPathComparer());
				foreach (PSObject psobject in orderedEnumerable)
				{
					object obj = PSObject.Base(psobject);
					string text9 = null;
					string text10 = null;
					PathInfo pathInfo = obj as PathInfo;
					if (pathInfo != null)
					{
						text9 = pathInfo.Path;
						text10 = pathInfo.ProviderPath;
					}
					else if (obj is FileSystemInfo)
					{
						object arg3 = psobject;
						if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site128 == null)
						{
							CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site128 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CompletionCompleters)));
						}
						Func<CallSite, object, string> target2 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site128.Target;
						CallSite <>p__Site2 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site128;
						if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site129 == null)
						{
							CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site129 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "FullName", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						text10 = target2(<>p__Site2, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site129.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site129, arg3));
						if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12a == null)
						{
							CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12a = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CompletionCompleters)));
						}
						Func<CallSite, object, string> target3 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12a.Target;
						CallSite <>p__Site12a = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12a;
						object arg4;
						if (!flag3)
						{
							arg4 = text10;
						}
						else
						{
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12b == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12b = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "PSPath", typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							arg4 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12b.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12b, arg3);
						}
						text9 = target3(<>p__Site12a, arg4);
					}
					else
					{
						string text11 = obj as string;
						if (text11 != null)
						{
							text10 = text11;
							text9 = (flag3 ? ("FileSystem::" + text11) : text10);
						}
					}
					if (text9 != null && (!flag2 || text10 != null))
					{
						string text12;
						if (option2)
						{
							try
							{
								SessionStateInternal engineSessionState = contextFromTLS.EngineSessionState;
								text12 = engineSessionState.NormalizeRelativePath(text9, engineSessionState.CurrentLocation.ProviderPath);
								if (!text12.StartsWith("..\\", StringComparison.Ordinal))
								{
									text12 = ".\\" + text12;
								}
								goto IL_77D;
							}
							catch (Exception e4)
							{
								CommandProcessorBase.CheckForSevereException(e4);
								continue;
							}
							goto IL_779;
						}
						goto IL_779;
						IL_77D:
						if (CompletionCompleters.ProviderSpecified(text12) && !flag3)
						{
							int num = text12.IndexOf(':');
							text12 = text12.Substring(num + 2);
						}
						if (CompletionCompleters.CompletionRequiresQuotes(text12, !option3))
						{
							string text13 = (text2 == string.Empty) ? "'" : text2;
							if (text13 == "'")
							{
								text12 = text12.Replace("'", "''");
							}
							else
							{
								text12 = text12.Replace("`", "``");
								text12 = text12.Replace("$", "`$");
							}
							if (!option3)
							{
								if (text13 == "'")
								{
									text12 = text12.Replace("[", "`[");
									text12 = text12.Replace("]", "`]");
								}
								else
								{
									text12 = text12.Replace("[", "``[");
									text12 = text12.Replace("]", "``]");
								}
							}
							text12 = text13 + text12 + text13;
						}
						else if (text2 != string.Empty)
						{
							text12 = text2 + text12 + text2;
						}
						if (flag2)
						{
							bool flag5 = Directory.Exists(text10);
							if ((!containerOnly || flag5) && (containerOnly || flag5 || CompletionCompleters.CheckFileExtension(text10, extension)))
							{
								string toolTip = text10;
								string fileName = Path.GetFileName(text10);
								list.Add(new CompletionResult(text12, fileName, flag5 ? CompletionResultType.ProviderContainer : CompletionResultType.ProviderItem, toolTip));
								continue;
							}
							continue;
						}
						else
						{
							CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Item", null).AddParameter("LiteralPath", text9);
							Collection<PSObject> collection3 = context.Helper.ExecuteCurrentPowerShell(out ex, null);
							if (collection3 == null || collection3.Count != 1)
							{
								list.Add(new CompletionResult(text12));
								continue;
							}
							object arg5 = collection3[0];
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12c == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12c = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ConvertTo", new Type[]
								{
									typeof(bool)
								}, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, Type, object, object> target4 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12c.Target;
							CallSite <>p__Site12c = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12c;
							Type typeFromHandle = typeof(LanguagePrimitives);
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12d == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12d = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "PSIsContainer", typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							object arg6 = target4(<>p__Site12c, typeFromHandle, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12d.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12d, arg5));
							if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12e == null)
							{
								CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12e = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, object, bool> target5 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12e.Target;
							CallSite <>p__Site12e = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12e;
							object arg7;
							if (containerOnly)
							{
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12f == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12f = CallSite<Func<CallSite, bool, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								Func<CallSite, bool, object, object> target6 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12f.Target;
								CallSite <>p__Site12f = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site12f;
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site130 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site130 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								arg7 = target6(<>p__Site12f, containerOnly, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site130.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site130, arg6));
							}
							else
							{
								arg7 = containerOnly;
							}
							if (!target5(<>p__Site12e, arg7))
							{
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site131 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site131 = CallSite<Action<CallSite, PowerShell, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddParameter", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								Action<CallSite, PowerShell, string, object> target7 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site131.Target;
								CallSite <>p__Site3 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site131;
								PowerShell arg8 = CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Convert-Path", null);
								string arg9 = "LiteralPath";
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site132 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site132 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "PSPath", typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								target7(<>p__Site3, arg8, arg9, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site132.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site132, arg5));
								Collection<PSObject> collection4 = context.Helper.ExecuteCurrentPowerShell(out ex, null);
								string text14 = null;
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site133 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site133 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CompletionCompleters)));
								}
								Func<CallSite, object, string> target8 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site133.Target;
								CallSite <>p__Site4 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site133;
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site134 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site134 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "PSChildName", typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								string text15 = target8(<>p__Site4, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site134.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site134, arg5));
								if (collection4 != null && collection4.Count == 1)
								{
									text14 = (PSObject.Base(collection4[0]) as string);
								}
								if (string.IsNullOrEmpty(text15))
								{
									if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site135 == null)
									{
										CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site135 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CompletionCompleters)));
									}
									Func<CallSite, object, string> target9 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site135.Target;
									CallSite <>p__Site5 = CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site135;
									if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site136 == null)
									{
										CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site136 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									text15 = target9(<>p__Site5, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site136.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site136, arg5));
								}
								List<CompletionResult> list2 = list;
								string completionText = text12;
								string listItemText = text15;
								if (CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site137 == null)
								{
									CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site137 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CompletionCompleters), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								list2.Add(new CompletionResult(completionText, listItemText, CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site137.Target(CompletionCompleters.<CompleteFilename>o__SiteContainer124.<>p__Site137, arg6) ? CompletionResultType.ProviderContainer : CompletionResultType.ProviderItem, text14 ?? text9));
								continue;
							}
							continue;
						}
						IL_779:
						text12 = text9;
						goto IL_77D;
					}
				}
			}
			return list;
		}

		// Token: 0x06005988 RID: 22920
		[DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int NetShareEnum(string serverName, int level, out IntPtr bufptr, int prefMaxLen, out uint entriesRead, out uint totalEntries, ref uint resumeHandle);

		// Token: 0x06005989 RID: 22921 RVA: 0x001E14F4 File Offset: 0x001DF6F4
		internal static List<string> GetFileShares(string machine, bool ignoreHidden)
		{
			uint num = 0U;
			IntPtr value;
			uint num3;
			uint num4;
			int num2 = CompletionCompleters.NetShareEnum(machine, 1, out value, -1, out num3, out num4, ref num);
			List<string> list = new List<string>();
			if (num2 == 0 || num2 == 234)
			{
				int num5 = 0;
				while ((long)num5 < (long)((ulong)num3))
				{
					IntPtr ptr = (IntPtr)((long)value + (long)(ClrFacade.SizeOf<CompletionCompleters.SHARE_INFO_1>() * num5));
					CompletionCompleters.SHARE_INFO_1 share_INFO_ = ClrFacade.PtrToStructure<CompletionCompleters.SHARE_INFO_1>(ptr);
					if ((share_INFO_.type & 255) == 0 && (!ignoreHidden || !share_INFO_.netname.EndsWith("$", StringComparison.Ordinal)))
					{
						list.Add(share_INFO_.netname);
					}
					num5++;
				}
			}
			return list;
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x001E1594 File Offset: 0x001DF794
		private static bool CheckFileExtension(string path, HashSet<string> extension)
		{
			if (extension == null || extension.Count == 0)
			{
				return true;
			}
			string extension2 = Path.GetExtension(path);
			return extension2 == null || extension.Contains(extension2);
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x001E15C4 File Offset: 0x001DF7C4
		public static IEnumerable<CompletionResult> CompleteVariable(string variableName)
		{
			if (Runspace.DefaultRunspace == null)
			{
				return CommandCompletion.EmptyCompletionResult;
			}
			CompletionExecutionHelper helper = new CompletionExecutionHelper(PowerShell.Create(RunspaceMode.CurrentRunspace));
			return CompletionCompleters.CompleteVariable(new CompletionContext
			{
				WordToComplete = variableName,
				Helper = helper
			});
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x001E1608 File Offset: 0x001DF808
		internal static List<CompletionResult> CompleteVariable(CompletionContext context)
		{
			HashSet<string> hashedResults = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<CompletionResult> list = new List<CompletionResult>();
			string wordToComplete = context.WordToComplete;
			int num = wordToComplete.IndexOf(':');
			string text = "$";
			Ast ast = context.RelatedAsts.Last<Ast>();
			VariableExpressionAst variableExpressionAst = ast as VariableExpressionAst;
			if (variableExpressionAst != null && variableExpressionAst.Splatted)
			{
				text = "@";
			}
			WildcardPattern wildcardPattern = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);
			if (ast != null)
			{
				Ast parent = ast.Parent;
				CompletionCompleters.FindVariablesVisitor findVariablesVisitor = new CompletionCompleters.FindVariablesVisitor
				{
					CompletionVariableAst = ast
				};
				while (parent != null)
				{
					if (parent is IParameterMetadataProvider)
					{
						findVariablesVisitor.Top = parent;
						parent.Visit(findVariablesVisitor);
					}
					parent = parent.Parent;
				}
				foreach (Tuple<string, Ast> tuple in findVariablesVisitor.VariableSources)
				{
					Ast ast2 = null;
					string text2 = null;
					VariableExpressionAst variableExpressionAst2 = tuple.Item2 as VariableExpressionAst;
					if (variableExpressionAst2 != null)
					{
						text2 = tuple.Item1;
						ast2 = tuple.Item2.Parent;
					}
					else
					{
						CommandAst commandAst = tuple.Item2 as CommandAst;
						if (commandAst != null)
						{
							text2 = tuple.Item1;
							ast2 = tuple.Item2;
						}
					}
					string.IsNullOrEmpty(text2);
					if (wildcardPattern.IsMatch(text2))
					{
						string completionText = (text2.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + text2) : (text + "{" + text2 + "}");
						string tooltip = text2;
						Ast ast3 = ast2;
						while (ast3 != null)
						{
							ParameterAst parameterAst = ast3 as ParameterAst;
							if (parameterAst != null)
							{
								TypeConstraintAst typeConstraintAst = parameterAst.Attributes.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
								if (typeConstraintAst != null)
								{
									tooltip = StringUtil.Format("{0}${1}", typeConstraintAst.Extent.Text, text2);
									break;
								}
								break;
							}
							else
							{
								AssignmentStatementAst assignmentStatementAst = ast3.Parent as AssignmentStatementAst;
								if (assignmentStatementAst != null)
								{
									if (assignmentStatementAst.Left == ast3)
									{
										tooltip = ast3.Extent.Text;
										break;
									}
									break;
								}
								else
								{
									CommandAst commandAst2 = ast3 as CommandAst;
									if (commandAst2 != null)
									{
										PSTypeName pstypeName = ast3.GetInferredType(context).FirstOrDefault<PSTypeName>();
										if (pstypeName != null)
										{
											tooltip = StringUtil.Format("[{0}]${1}", pstypeName.Name, text2);
											break;
										}
										break;
									}
									else
									{
										ast3 = ast3.Parent;
									}
								}
							}
						}
						CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText, text2, tooltip);
					}
				}
			}
			string text3;
			string text4;
			if (num == -1)
			{
				text3 = "variable:" + wordToComplete + "*";
				text4 = "";
			}
			else
			{
				text4 = wordToComplete.Substring(0, num + 1);
				if (CompletionCompleters.VariableScopes.Contains(text4, StringComparer.OrdinalIgnoreCase))
				{
					text3 = "variable:" + wordToComplete.Substring(num + 1) + "*";
				}
				else
				{
					text3 = wordToComplete + "*";
				}
			}
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Item", null).AddParameter("Path", text3);
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", "Name");
			Exception ex;
			Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			if (collection != null)
			{
				foreach (object obj in collection)
				{
					if (CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13d == null)
					{
						CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13d = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					string text5 = CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13d.Target(CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13d, obj) as string;
					if (!string.IsNullOrEmpty(text5))
					{
						string tooltip2 = text5;
						if (CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13e == null)
						{
							CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13e = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Base", null, typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						PSVariable psvariable = CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13e.Target(CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13e, typeof(PSObject), obj) as PSVariable;
						if (psvariable != null)
						{
							object value = psvariable.Value;
							if (value != null)
							{
								tooltip2 = StringUtil.Format("[{0}]${1}", ToStringCodeMethods.Type(value.GetType(), true), text5);
							}
						}
						string completionText2 = (text5.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + text4 + text5) : string.Concat(new string[]
						{
							text,
							"{",
							text4,
							text5,
							"}"
						});
						CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText2, text5, tooltip2);
					}
				}
			}
			if (num == -1 && "env".StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
			{
				currentPowerShell = context.Helper.CurrentPowerShell;
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-Item", null).AddParameter("Path", "env:*");
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", "Key");
				collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null)
				{
					foreach (object arg in collection)
					{
						if (CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13f == null)
						{
							CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13f = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CompletionCompleters), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						string text6 = CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13f.Target(CompletionCompleters.<CompleteVariable>o__SiteContainer13c.<>p__Site13f, arg) as string;
						if (!string.IsNullOrEmpty(text6))
						{
							text6 = "env:" + text6;
							string completionText3 = (text6.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + text6) : (text + "{" + text6 + "}");
							CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText3, text6, "[string]" + text6);
						}
					}
				}
			}
			foreach (string text7 in CompletionCompleters._specialVariablesCache.Value)
			{
				if (wildcardPattern.IsMatch(text7))
				{
					string completionText4 = (text7.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + text7) : (text + "{" + text7 + "}");
					CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText4, text7, text7);
				}
			}
			if (num == -1)
			{
				text3 = wordToComplete + "*";
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Management\\Get-PSDrive", null).AddParameter("Name", text3);
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", "Name");
				collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null)
				{
					foreach (PSObject obj2 in collection)
					{
						PSDriveInfo psdriveInfo = PSObject.Base(obj2) as PSDriveInfo;
						if (psdriveInfo != null)
						{
							string name = psdriveInfo.Name;
							if (name != null && !string.IsNullOrWhiteSpace(name) && name.Length > 1)
							{
								string completionText5 = (name.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + name + ":") : (text + "{" + name + ":}");
								string tooltip3 = string.IsNullOrEmpty(psdriveInfo.Description) ? name : psdriveInfo.Description;
								CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText5, name, tooltip3);
							}
						}
					}
				}
				WildcardPattern wildcardPattern2 = new WildcardPattern(text3, WildcardOptions.IgnoreCase);
				foreach (string text8 in CompletionCompleters.VariableScopes)
				{
					if (wildcardPattern2.IsMatch(text8))
					{
						string completionText6 = (text8.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) == -1) ? (text + text8) : (text + "{" + text8 + "}");
						CompletionCompleters.AddUniqueVariable(hashedResults, list, completionText6, text8, text8);
					}
				}
			}
			return list;
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x001E1EB0 File Offset: 0x001E00B0
		private static void AddUniqueVariable(HashSet<string> hashedResults, List<CompletionResult> results, string completionText, string listItemText, string tooltip)
		{
			if (!hashedResults.Contains(completionText))
			{
				hashedResults.Add(completionText);
				results.Add(new CompletionResult(completionText, listItemText, CompletionResultType.Variable, tooltip));
			}
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x001E1ED4 File Offset: 0x001E00D4
		private static SortedSet<string> BuildSpecialVariablesCache()
		{
			SortedSet<string> sortedSet = new SortedSet<string>();
			foreach (FieldInfo fieldInfo in typeof(SpecialVariables).GetFields(BindingFlags.Static | BindingFlags.NonPublic))
			{
				if (fieldInfo.FieldType.Equals(typeof(string)))
				{
					sortedSet.Add((string)fieldInfo.GetValue(null));
				}
			}
			return sortedSet;
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x001E1F38 File Offset: 0x001E0138
		internal static List<CompletionResult> CompleteComment(CompletionContext context)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			Match match = Regex.Match(context.WordToComplete, "^#([\\w\\-]*)$");
			if (!match.Success)
			{
				return list;
			}
			string text = match.Groups[1].Value;
			PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
			int num;
			Exception ex;
			Collection<PSObject> collection;
			if (Regex.IsMatch(text, "^[0-9]+$") && LanguagePrimitives.TryConvertTo<int>(text, out num))
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-History", typeof(GetHistoryCommand)).AddParameter("Id", num);
				collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
				if (collection != null && collection.Count == 1)
				{
					HistoryInfo historyInfo = PSObject.Base(collection[0]) as HistoryInfo;
					if (historyInfo != null)
					{
						string commandLine = historyInfo.CommandLine;
						if (!string.IsNullOrEmpty(commandLine))
						{
							list.Add(new CompletionResult(commandLine, commandLine, CompletionResultType.History, commandLine));
						}
					}
				}
				return list;
			}
			text = "*" + text + "*";
			CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Get-History", typeof(GetHistoryCommand));
			collection = context.Helper.ExecuteCurrentPowerShell(out ex, null);
			WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
			if (collection != null)
			{
				for (int i = collection.Count - 1; i >= 0; i--)
				{
					PSObject obj = collection[i];
					HistoryInfo historyInfo2 = PSObject.Base(obj) as HistoryInfo;
					if (historyInfo2 != null)
					{
						string commandLine2 = historyInfo2.CommandLine;
						if (!string.IsNullOrEmpty(commandLine2) && wildcardPattern.IsMatch(commandLine2))
						{
							list.Add(new CompletionResult(commandLine2, commandLine2, CompletionResultType.History, commandLine2));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x001E20D0 File Offset: 0x001E02D0
		internal static List<CompletionResult> CompleteMember(CompletionContext context, bool @static)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			Ast ast = context.RelatedAsts.Last<Ast>();
			MemberExpressionAst memberExpressionAst = ast as MemberExpressionAst;
			Ast ast2 = null;
			ExpressionAst expressionAst = null;
			if (memberExpressionAst != null)
			{
				if (context.TokenAtCursor.Extent.StartOffset >= memberExpressionAst.Member.Extent.StartOffset)
				{
					ast2 = memberExpressionAst.Member;
				}
				expressionAst = memberExpressionAst.Expression;
			}
			else
			{
				ast2 = ast;
			}
			StringConstantExpressionAst stringConstantExpressionAst = ast2 as StringConstantExpressionAst;
			string text = "*";
			if (stringConstantExpressionAst != null)
			{
				if (!stringConstantExpressionAst.Value.Equals(".", StringComparison.OrdinalIgnoreCase) && !stringConstantExpressionAst.Value.Equals("::", StringComparison.OrdinalIgnoreCase))
				{
					text = stringConstantExpressionAst.Value + "*";
				}
			}
			else if (!(ast is ErrorExpressionAst) && expressionAst == null)
			{
				return list;
			}
			CommandAst commandAst = ast.Parent as CommandAst;
			if (commandAst != null)
			{
				int num = commandAst.CommandElements.Count - 1;
				while (num >= 0 && commandAst.CommandElements[num] != ast)
				{
					num--;
				}
				CommandElementAst commandElementAst = commandAst.CommandElements[num - 1];
				IScriptExtent extent = commandElementAst.Extent;
				IScriptExtent extent2 = ast.Extent;
				if (extent.EndLineNumber == extent2.StartLineNumber && extent.EndColumnNumber == extent2.StartColumnNumber)
				{
					expressionAst = (commandElementAst as ExpressionAst);
				}
			}
			else if (ast.Parent is MemberExpressionAst)
			{
				if (expressionAst == null)
				{
					MemberExpressionAst memberExpressionAst2 = (MemberExpressionAst)ast.Parent;
					expressionAst = memberExpressionAst2.Expression;
				}
			}
			else if (ast.Parent is BinaryExpressionAst && context.TokenAtCursor.Kind.Equals(TokenKind.Multiply))
			{
				MemberExpressionAst memberExpressionAst3 = ((BinaryExpressionAst)ast.Parent).Left as MemberExpressionAst;
				if (memberExpressionAst3 != null)
				{
					expressionAst = memberExpressionAst3.Expression;
					if (memberExpressionAst3.Member is StringConstantExpressionAst)
					{
						text = ((StringConstantExpressionAst)memberExpressionAst3.Member).Value + "*";
					}
				}
			}
			if (expressionAst == null)
			{
				return list;
			}
			if (CompletionCompleters.IsSplattedVariable(expressionAst))
			{
				return list;
			}
			CompletionCompleters.CompleteMemberHelper(@static, text, expressionAst, context, list);
			if (list.Count == 0)
			{
				PSTypeName[] array = null;
				if (@static)
				{
					TypeExpressionAst typeExpressionAst = expressionAst as TypeExpressionAst;
					if (typeExpressionAst != null)
					{
						array = new PSTypeName[]
						{
							new PSTypeName(typeExpressionAst.TypeName)
						};
					}
				}
				else
				{
					array = expressionAst.GetInferredType(context).ToArray<PSTypeName>();
				}
				if (array != null && array.Length > 0)
				{
					CompletionCompleters.CompleteMemberByInferredType(context, array, list, text, null, @static);
				}
				else
				{
					VariableExpressionAst variableExpressionAst = expressionAst as VariableExpressionAst;
					MemberExpressionAst memberExpressionAst4 = expressionAst as MemberExpressionAst;
					bool flag = false;
					if (variableExpressionAst != null)
					{
						VariablePath variablePath = variableExpressionAst.VariablePath;
						if (variablePath.IsVariable && CompletionCompleters.DscCollectionVariables.Contains(variablePath.UserPath) && CompletionCompleters.IsInDscContext(variableExpressionAst))
						{
							flag = true;
						}
					}
					else if (memberExpressionAst4 != null)
					{
						StringConstantExpressionAst stringConstantExpressionAst2 = memberExpressionAst4.Member as StringConstantExpressionAst;
						if (CompletionCompleters.IsConfigurationDataVariable(memberExpressionAst4.Expression) && stringConstantExpressionAst2 != null && string.Equals("AllNodes", stringConstantExpressionAst2.Value, StringComparison.OrdinalIgnoreCase) && CompletionCompleters.IsInDscContext(memberExpressionAst4))
						{
							flag = true;
						}
					}
					if (flag)
					{
						CompletionCompleters.CompleteExtensionMethods(text, list);
					}
				}
				if (list.Count == 0 && CompletionCompleters.IsConfigurationDataVariable(expressionAst) && CompletionCompleters.IsInDscContext(expressionAst))
				{
					WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
					if (wildcardPattern.IsMatch("AllNodes"))
					{
						list.Add(new CompletionResult("AllNodes", "AllNodes", CompletionResultType.Property, "AllNodes"));
					}
				}
			}
			return list;
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x001E2440 File Offset: 0x001E0640
		private static void CompleteExtensionMethods(string memberName, List<CompletionResult> results)
		{
			WildcardPattern pattern = new WildcardPattern(memberName, WildcardOptions.IgnoreCase);
			CompletionCompleters.CompleteExtensionMethods(pattern, results);
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x001E249C File Offset: 0x001E069C
		private static void CompleteExtensionMethods(WildcardPattern pattern, List<CompletionResult> results)
		{
			results.AddRange(from member in CompletionCompleters.ExtensionMethods
			where pattern.IsMatch(member.Item1)
			select new CompletionResult(member.Item1 + "(", member.Item1, CompletionResultType.Method, member.Item2));
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x001E24F4 File Offset: 0x001E06F4
		private static bool IsConfigurationDataVariable(ExpressionAst targetExpr)
		{
			VariableExpressionAst variableExpressionAst = targetExpr as VariableExpressionAst;
			if (variableExpressionAst != null)
			{
				VariablePath variablePath = variableExpressionAst.VariablePath;
				if (variablePath.IsVariable && variablePath.UserPath.Equals("ConfigurationData", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005994 RID: 22932 RVA: 0x001E2530 File Offset: 0x001E0730
		private static bool IsInDscContext(ExpressionAst expression)
		{
			return Ast.GetAncestorAst<ConfigurationDefinitionAst>(expression) != null;
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x001E254C File Offset: 0x001E074C
		private static void CompleteMemberByInferredType(CompletionContext context, IEnumerable<PSTypeName> inferredTypes, List<CompletionResult> results, string memberName, Func<object, bool> filter, bool isStatic)
		{
			bool flag = false;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			WildcardPattern wildcardPattern = new WildcardPattern(memberName, WildcardOptions.IgnoreCase);
			foreach (PSTypeName pstypeName in inferredTypes)
			{
				if (!hashSet.Contains(pstypeName.Name))
				{
					hashSet.Add(pstypeName.Name);
					IEnumerable<object> membersByInferredType = CompletionCompleters.GetMembersByInferredType(pstypeName, context, isStatic, filter);
					foreach (object member in membersByInferredType)
					{
						CompletionCompleters.AddInferredMember(member, wildcardPattern, results);
					}
					if (!flag && pstypeName.Type != null && CompletionCompleters.IsStaticTypeEnumerable(pstypeName.Type))
					{
						flag = true;
						CompletionCompleters.CompleteExtensionMethods(wildcardPattern, results);
					}
				}
			}
			if (results.Count > 0)
			{
				CompletionCompleters.AddCommandWithPreferenceSetting(context.Helper.CurrentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", new string[]
				{
					"ResultType",
					"ListItemText"
				}).AddParameter("Unique");
				Exception ex;
				Collection<PSObject> source = context.Helper.ExecuteCurrentPowerShell(out ex, results);
				results.Clear();
				results.AddRange(from psobj in source
				select PSObject.Base(psobj) as CompletionResult);
			}
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x001E27DC File Offset: 0x001E09DC
		private static void AddInferredMember(object member, WildcardPattern memberNamePattern, List<CompletionResult> results)
		{
			string memberName = null;
			bool flag = false;
			Func<string> func = null;
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				memberName = propertyInfo.Name;
				func = (() => string.Concat(new string[]
				{
					ToStringCodeMethods.Type(propertyInfo.PropertyType, false),
					" ",
					memberName,
					" { ",
					(propertyInfo.GetGetMethod() != null) ? "get; " : "",
					(propertyInfo.GetSetMethod() != null) ? "set; " : "",
					"}"
				}));
			}
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				memberName = fieldInfo.Name;
				func = (() => ToStringCodeMethods.Type(fieldInfo.FieldType, false) + " " + memberName);
			}
			DotNetAdapter.MethodCacheEntry methodCacheEntry = member as DotNetAdapter.MethodCacheEntry;
			if (methodCacheEntry != null)
			{
				memberName = methodCacheEntry[0].method.Name;
				flag = true;
				func = (() => string.Join("\n", from m in methodCacheEntry.methodInformationStructures
				select m.methodDefinition));
			}
			PSMemberInfo psmemberInfo = member as PSMemberInfo;
			if (psmemberInfo != null)
			{
				memberName = psmemberInfo.Name;
				flag = (member is PSMethodInfo);
				func = new Func<string>(psmemberInfo.ToString);
			}
			CimPropertyDeclaration cimProperty = member as CimPropertyDeclaration;
			if (cimProperty != null)
			{
				memberName = cimProperty.Name;
				flag = false;
				func = (() => CompletionCompleters.GetCimPropertyToString(cimProperty));
			}
			MemberAst memberAst = member as MemberAst;
			if (memberAst != null)
			{
				memberName = ((memberAst is CompilerGeneratedMemberFunctionAst) ? "new" : memberAst.Name);
				flag = (memberAst is FunctionMemberAst || memberAst is CompilerGeneratedMemberFunctionAst);
				func = new Func<string>(memberAst.GetTooltip);
			}
			if (memberName == null || !memberNamePattern.IsMatch(memberName))
			{
				return;
			}
			CompletionResultType resultType = flag ? CompletionResultType.Method : CompletionResultType.Property;
			string completionText = flag ? (memberName + "(") : memberName;
			results.Add(new CompletionResult(completionText, memberName, resultType, func()));
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x001E29F4 File Offset: 0x001E0BF4
		private static string GetCimPropertyToString(CimPropertyDeclaration cimProperty)
		{
			CimType cimType = cimProperty.CimType;
			switch (cimType)
			{
			case CimType.DateTime:
			case CimType.Reference:
			case CimType.Instance:
				break;
			case CimType.String:
				goto IL_58;
			default:
				switch (cimType)
				{
				case CimType.DateTimeArray:
				case CimType.ReferenceArray:
				case CimType.InstanceArray:
					break;
				case CimType.StringArray:
					goto IL_58;
				default:
					goto IL_58;
				}
				break;
			}
			string text = "CimInstance#" + cimProperty.CimType.ToString();
			goto IL_6A;
			IL_58:
			text = ToStringCodeMethods.Type(CimConverter.GetDotNetType(cimProperty.CimType), false);
			IL_6A:
			bool flag = CimFlags.ReadOnly == (cimProperty.Flags & CimFlags.ReadOnly);
			return string.Concat(new string[]
			{
				text,
				" ",
				cimProperty.Name,
				" { get; ",
				flag ? "}" : "set; }"
			});
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x001E2AC0 File Offset: 0x001E0CC0
		private static bool IsWriteablePropertyMember(object member)
		{
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.CanWrite;
			}
			PSPropertyInfo pspropertyInfo = member as PSPropertyInfo;
			return pspropertyInfo != null && pspropertyInfo.IsSettable;
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x001E2AF6 File Offset: 0x001E0CF6
		private static bool IsPropertyMember(object member)
		{
			return member is PropertyInfo || member is FieldInfo || member is PSPropertyInfo || member is CimPropertyDeclaration || member is PropertyMemberAst;
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x001E2B24 File Offset: 0x001E0D24
		private static bool IsMemberHidden(object member)
		{
			PSMemberInfo psmemberInfo = member as PSMemberInfo;
			if (psmemberInfo != null)
			{
				return psmemberInfo.IsHidden;
			}
			MemberInfo memberInfo = member as MemberInfo;
			if (memberInfo != null)
			{
				return memberInfo.GetCustomAttributes(typeof(HiddenAttribute), false).Any<object>();
			}
			PropertyMemberAst propertyMemberAst = member as PropertyMemberAst;
			if (propertyMemberAst != null)
			{
				return propertyMemberAst.IsHidden;
			}
			FunctionMemberAst functionMemberAst = member as FunctionMemberAst;
			return functionMemberAst != null && functionMemberAst.IsHidden;
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x001E2B8C File Offset: 0x001E0D8C
		private static bool IsConstructor(object member)
		{
			PSMethod psmethod = member as PSMethod;
			if (psmethod != null)
			{
				DotNetAdapter.MethodCacheEntry methodCacheEntry = psmethod.adapterData as DotNetAdapter.MethodCacheEntry;
				if (methodCacheEntry != null)
				{
					return methodCacheEntry.methodInformationStructures[0].method.IsConstructor;
				}
			}
			return false;
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x001E2C60 File Offset: 0x001E0E60
		internal static IEnumerable<object> GetMembersByInferredType(PSTypeName typename, CompletionContext context, bool @static, Func<object, bool> filter)
		{
			List<object> list = new List<object>();
			Func<object, bool> func = filter;
			if (typename.Type != null)
			{
				if (context.CurrentTypeDefinitionAst == null || context.CurrentTypeDefinitionAst.Type != typename.Type)
				{
					if (func == null)
					{
						func = ((object o) => !CompletionCompleters.IsMemberHidden(o));
					}
					else
					{
						func = ((object o) => !CompletionCompleters.IsMemberHidden(o) && filter(o));
					}
				}
				IEnumerable<Type> collection;
				if (typename.Type.IsArray)
				{
					collection = new Type[]
					{
						typename.Type.GetElementType()
					};
				}
				else
				{
					collection = from t in typename.Type.GetInterfaces()
					where t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
					select t;
				}
				using (IEnumerator<Type> enumerator = collection.Prepend(typename.Type).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Type type = enumerator.Current;
						if (!@static)
						{
							ConsolidatedString internedTypeNameHierarchy = DotNetAdapter.GetInternedTypeNameHierarchy(type);
							list.AddRange(context.ExecutionContext.TypeTable.GetMembers<PSMemberInfo>(internedTypeNameHierarchy));
						}
						IEnumerable<object> enumerable = @static ? ((IEnumerable<object>)PSObject.dotNetStaticAdapter.BaseGetMembers<PSMemberInfo>(type)) : PSObject.dotNetInstanceAdapter.GetPropertiesAndMethods(type, false);
						list.AddRange((func != null) ? enumerable.Where(func) : enumerable);
					}
					return list;
				}
			}
			if (typename.TypeDefinitionAst != null)
			{
				if (context.CurrentTypeDefinitionAst != typename.TypeDefinitionAst)
				{
					if (func == null)
					{
						func = ((object o) => !CompletionCompleters.IsMemberHidden(o));
					}
					else
					{
						func = ((object o) => !CompletionCompleters.IsMemberHidden(o) && filter(o));
					}
				}
				bool flag = false;
				foreach (MemberAst memberAst in typename.TypeDefinitionAst.Members)
				{
					bool flag2 = false;
					PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
					if (propertyMemberAst != null)
					{
						if (propertyMemberAst.IsStatic == @static)
						{
							flag2 = true;
						}
					}
					else
					{
						FunctionMemberAst functionMemberAst = (FunctionMemberAst)memberAst;
						if (functionMemberAst.IsStatic == @static)
						{
							flag2 = true;
						}
						flag |= functionMemberAst.IsConstructor;
					}
					if (func != null && flag2)
					{
						flag2 = func(memberAst);
					}
					if (flag2)
					{
						list.Add(memberAst);
					}
				}
				foreach (TypeConstraintAst typeConstraintAst in typename.TypeDefinitionAst.BaseTypes)
				{
					TypeName typeName = typeConstraintAst.TypeName as TypeName;
					if (typeName != null)
					{
						TypeDefinitionAst typeDefinitionAst = typeName._typeDefinitionAst;
						list.AddRange(CompletionCompleters.GetMembersByInferredType(new PSTypeName(typeDefinitionAst), context, @static, func));
					}
				}
				if (@static)
				{
					if (filter == null)
					{
						func = ((object o) => !CompletionCompleters.IsConstructor(o));
					}
					else
					{
						func = ((object o) => !CompletionCompleters.IsConstructor(o) && filter(o));
					}
					if (!flag)
					{
						list.Add(new CompilerGeneratedMemberFunctionAst(PositionUtilities.EmptyExtent, typename.TypeDefinitionAst, SpecialMemberFunctionType.DefaultConstructor));
					}
				}
				else
				{
					func = filter;
				}
				list.AddRange(CompletionCompleters.GetMembersByInferredType(new PSTypeName(typeof(object)), context, @static, func));
			}
			else
			{
				if (!@static)
				{
					ConsolidatedString types = new ConsolidatedString(new string[]
					{
						typename.Name
					});
					list.AddRange(context.ExecutionContext.TypeTable.GetMembers<PSMemberInfo>(types));
				}
				string value;
				string value2;
				if (CompletionCompleters.NativeCompletionCimCommands_ParseTypeName(typename, out value, out value2))
				{
					CompletionCompleters.AddCommandWithPreferenceSetting(context.Helper.CurrentPowerShell, "CimCmdlets\\Get-CimClass", null).AddParameter("Namespace", value).AddParameter("Class", value2);
					Exception ex;
					Collection<PSObject> source = context.Helper.ExecuteCurrentPowerShell(out ex, null);
					foreach (CimClass cimClass in source.Select(new Func<PSObject, object>(PSObject.Base)).OfType<CimClass>())
					{
						list.AddRange((func != null) ? cimClass.CimClassProperties.Where(func) : ((IEnumerable<object>)cimClass.CimClassProperties));
					}
				}
			}
			return list;
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x001E315C File Offset: 0x001E135C
		private static CompletionCompleters.TypeCompletionMapping[][] InitializeTypeCache()
		{
			Dictionary<string, CompletionCompleters.TypeCompletionMapping> dictionary = new Dictionary<string, CompletionCompleters.TypeCompletionMapping>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, Type> keyValuePair in TypeAccelerators.Get)
			{
				CompletionCompleters.TypeCompletion item2 = new CompletionCompleters.TypeCompletion
				{
					Type = keyValuePair.Value
				};
				CompletionCompleters.TypeCompletionMapping typeCompletionMapping;
				if (dictionary.TryGetValue(keyValuePair.Key, out typeCompletionMapping))
				{
					Type acceleratorType = keyValuePair.Value;
					if (typeCompletionMapping.Completions.Any(delegate(CompletionCompleters.TypeCompletionBase item)
					{
						CompletionCompleters.TypeCompletion typeCompletion = item as CompletionCompleters.TypeCompletion;
						return typeCompletion != null && typeCompletion.Type == acceleratorType;
					}))
					{
						continue;
					}
					typeCompletionMapping.Completions.Add(item2);
				}
				else
				{
					dictionary.Add(keyValuePair.Key, new CompletionCompleters.TypeCompletionMapping
					{
						Key = keyValuePair.Key,
						Completions = 
						{
							item2
						}
					});
				}
				string fullName = keyValuePair.Value.FullName;
				if (!dictionary.ContainsKey(fullName))
				{
					dictionary.Add(fullName, new CompletionCompleters.TypeCompletionMapping
					{
						Key = fullName,
						Completions = 
						{
							item2
						}
					});
					string name = keyValuePair.Value.Name;
					if (!keyValuePair.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						if (!dictionary.TryGetValue(name, out typeCompletionMapping))
						{
							typeCompletionMapping = new CompletionCompleters.TypeCompletionMapping
							{
								Key = name
							};
							dictionary.Add(name, typeCompletionMapping);
						}
						typeCompletionMapping.Completions.Add(item2);
					}
				}
			}
			IEnumerable<Assembly> assemblies = ClrFacade.GetAssemblies(null);
			IEnumerable<Type> enumerable = assemblies.SelectMany((Assembly assembly) => assembly.GetTypes().Where(new Func<Type, bool>(TypeResolver.IsPublic)));
			foreach (Type type in enumerable)
			{
				CompletionCompleters.HandleNamespace(dictionary, type.Namespace);
				CompletionCompleters.HandleType(dictionary, type.FullName, type.Name, type);
			}
			IGrouping<int, CompletionCompleters.TypeCompletionMapping>[] array = (from t in dictionary.Values
			group t by t.Key.Count((char c) => c == '.') into g
			orderby g.Key
			select g).ToArray<IGrouping<int, CompletionCompleters.TypeCompletionMapping>>();
			CompletionCompleters.TypeCompletionMapping[][] array2 = new CompletionCompleters.TypeCompletionMapping[array.Last<IGrouping<int, CompletionCompleters.TypeCompletionMapping>>().Key + 1][];
			foreach (IGrouping<int, CompletionCompleters.TypeCompletionMapping> grouping in array)
			{
				array2[grouping.Key] = grouping.ToArray<CompletionCompleters.TypeCompletionMapping>();
			}
			Interlocked.Exchange<CompletionCompleters.TypeCompletionMapping[][]>(ref CompletionCompleters.typeCache, array2);
			return array2;
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x001E3430 File Offset: 0x001E1630
		private static void HandleNamespace(Dictionary<string, CompletionCompleters.TypeCompletionMapping> entryCache, string @namespace)
		{
			if (!string.IsNullOrEmpty(@namespace))
			{
				CompletionCompleters.TypeCompletionMapping typeCompletionMapping;
				if (!entryCache.TryGetValue(@namespace, out typeCompletionMapping))
				{
					typeCompletionMapping = new CompletionCompleters.TypeCompletionMapping
					{
						Key = @namespace,
						Completions = 
						{
							new CompletionCompleters.NamespaceCompletion
							{
								Namespace = @namespace
							}
						}
					};
					entryCache.Add(@namespace, typeCompletionMapping);
					return;
				}
				if (!typeCompletionMapping.Completions.OfType<CompletionCompleters.NamespaceCompletion>().Any<CompletionCompleters.NamespaceCompletion>())
				{
					typeCompletionMapping.Completions.Add(new CompletionCompleters.NamespaceCompletion
					{
						Namespace = @namespace
					});
				}
			}
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x001E34AC File Offset: 0x001E16AC
		private static void HandleType(Dictionary<string, CompletionCompleters.TypeCompletionMapping> entryCache, string fullTypeName, string shortTypeName, Type actualType)
		{
			if (string.IsNullOrEmpty(fullTypeName))
			{
				return;
			}
			int num = fullTypeName.LastIndexOf('`');
			int num2 = fullTypeName.LastIndexOf('+');
			bool flag = num != -1;
			bool flag2 = num2 != -1;
			CompletionCompleters.TypeCompletionBase item;
			if (flag)
			{
				if (flag2)
				{
					return;
				}
				item = ((actualType != null) ? new CompletionCompleters.GenericTypeCompletion
				{
					Type = actualType
				} : new CompletionCompleters.GenericTypeCompletionInStringFormat
				{
					FullTypeName = fullTypeName
				});
				fullTypeName = fullTypeName.Substring(0, num);
				shortTypeName = shortTypeName.Substring(0, shortTypeName.LastIndexOf('`'));
			}
			else
			{
				item = ((actualType != null) ? new CompletionCompleters.TypeCompletion
				{
					Type = actualType
				} : new CompletionCompleters.TypeCompletionInStringFormat
				{
					FullTypeName = fullTypeName
				});
			}
			CompletionCompleters.TypeCompletionMapping typeCompletionMapping;
			if (!entryCache.TryGetValue(fullTypeName, out typeCompletionMapping))
			{
				typeCompletionMapping = new CompletionCompleters.TypeCompletionMapping
				{
					Key = fullTypeName,
					Completions = 
					{
						item
					}
				};
				entryCache.Add(fullTypeName, typeCompletionMapping);
				if (!entryCache.TryGetValue(shortTypeName, out typeCompletionMapping))
				{
					typeCompletionMapping = new CompletionCompleters.TypeCompletionMapping
					{
						Key = shortTypeName
					};
					entryCache.Add(shortTypeName, typeCompletionMapping);
				}
				typeCompletionMapping.Completions.Add(item);
			}
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x001E35D0 File Offset: 0x001E17D0
		private static void UpdateTypeCacheOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			Interlocked.Exchange<CompletionCompleters.TypeCompletionMapping[][]>(ref CompletionCompleters.typeCache, null);
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x001E3628 File Offset: 0x001E1828
		internal static List<CompletionResult> CompleteNamespace(CompletionContext context, string prefix = "", string suffix = "")
		{
			CompletionCompleters.TypeCompletionMapping[][] array = CompletionCompleters.typeCache ?? CompletionCompleters.InitializeTypeCache();
			List<CompletionResult> list = new List<CompletionResult>();
			string wordToComplete = context.WordToComplete;
			int num = wordToComplete.Count((char c) => c == '.');
			if (num >= array.Length || array[num] == null)
			{
				return list;
			}
			WildcardPattern pattern = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);
			foreach (CompletionCompleters.TypeCompletionMapping typeCompletionMapping in from e in array[num]
			where e.Completions.OfType<CompletionCompleters.NamespaceCompletion>().Any<CompletionCompleters.NamespaceCompletion>() && pattern.IsMatch(e.Key)
			select e)
			{
				foreach (CompletionCompleters.TypeCompletionBase typeCompletionBase in typeCompletionMapping.Completions)
				{
					list.Add(typeCompletionBase.GetCompletionResult(typeCompletionMapping.Key, prefix, suffix));
				}
			}
			list.Sort((CompletionResult c1, CompletionResult c2) => string.Compare(c1.ListItemText, c2.ListItemText, StringComparison.OrdinalIgnoreCase));
			return list;
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x001E3768 File Offset: 0x001E1968
		public static IEnumerable<CompletionResult> CompleteType(string typeName)
		{
			PowerShell powershell = (Runspace.DefaultRunspace == null) ? PowerShell.Create() : PowerShell.Create(RunspaceMode.CurrentRunspace);
			CompletionExecutionHelper helper = new CompletionExecutionHelper(powershell);
			return CompletionCompleters.CompleteType(new CompletionContext
			{
				WordToComplete = typeName,
				Helper = helper
			}, "", "");
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x001E380C File Offset: 0x001E1A0C
		internal static List<CompletionResult> CompleteType(CompletionContext context, string prefix = "", string suffix = "")
		{
			CompletionCompleters.TypeCompletionMapping[][] array = CompletionCompleters.typeCache ?? CompletionCompleters.InitializeTypeCache();
			List<CompletionResult> list = new List<CompletionResult>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			string wordToComplete = context.WordToComplete;
			int num = wordToComplete.Count((char c) => c == '.');
			if (num >= array.Length || array[num] == null)
			{
				return list;
			}
			WildcardPattern pattern = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);
			foreach (CompletionCompleters.TypeCompletionMapping typeCompletionMapping in from e in array[num]
			where pattern.IsMatch(e.Key)
			select e)
			{
				foreach (CompletionCompleters.TypeCompletionBase typeCompletionBase in typeCompletionMapping.Completions)
				{
					string namespaceToRemove = CompletionCompleters.GetNamespaceToRemove(context, typeCompletionBase);
					CompletionResult completionResult = typeCompletionBase.GetCompletionResult(typeCompletionMapping.Key, prefix, suffix, namespaceToRemove);
					if (!hashSet.Contains(completionResult.CompletionText))
					{
						list.Add(completionResult);
						hashSet.Add(completionResult.CompletionText);
					}
				}
			}
			ScriptBlockAst scriptBlockAst = (ScriptBlockAst)context.RelatedAsts[0];
			IEnumerable<TypeDefinitionAst> source = scriptBlockAst.FindAll((Ast ast) => ast is TypeDefinitionAst, false).Cast<TypeDefinitionAst>();
			foreach (TypeDefinitionAst typeDefinitionAst in from ast in source
			where pattern.IsMatch(ast.Name)
			select ast)
			{
				string str = string.Empty;
				if (typeDefinitionAst.IsInterface)
				{
					str = "Interface ";
				}
				else if (typeDefinitionAst.IsClass)
				{
					str = "Class ";
				}
				else if (typeDefinitionAst.IsEnum)
				{
					str = "Enum ";
				}
				list.Add(new CompletionResult(prefix + typeDefinitionAst.Name + suffix, typeDefinitionAst.Name, CompletionResultType.Type, str + typeDefinitionAst.Name));
			}
			list.Sort((CompletionResult c1, CompletionResult c2) => string.Compare(c1.ListItemText, c2.ListItemText, StringComparison.OrdinalIgnoreCase));
			return list;
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x001E3AB4 File Offset: 0x001E1CB4
		private static string GetNamespaceToRemove(CompletionContext context, CompletionCompleters.TypeCompletionBase completion)
		{
			if (completion is CompletionCompleters.NamespaceCompletion)
			{
				return null;
			}
			CompletionCompleters.TypeCompletion typeCompletion = completion as CompletionCompleters.TypeCompletion;
			string typeNameSpace = (typeCompletion != null) ? typeCompletion.Type.Namespace : ((CompletionCompleters.TypeCompletionInStringFormat)completion).Namespace;
			ScriptBlockAst scriptBlockAst = (ScriptBlockAst)context.RelatedAsts[0];
			IEnumerable<UsingStatementAst> enumerable = from s in scriptBlockAst.UsingStatements
			where s.UsingStatementKind == UsingStatementKind.Namespace && typeNameSpace != null && typeNameSpace.StartsWith(s.Name.Value, StringComparison.OrdinalIgnoreCase)
			select s;
			string text = string.Empty;
			foreach (UsingStatementAst usingStatementAst in enumerable)
			{
				if (usingStatementAst.Name.Extent.Text.Length > text.Length)
				{
					text = usingStatementAst.Name.Extent.Text;
				}
			}
			return text;
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x001E3B98 File Offset: 0x001E1D98
		internal static List<CompletionResult> CompleteHelpTopics(CompletionContext context)
		{
			List<CompletionResult> list = new List<CompletionResult>();
			string path = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID) + "\\" + CultureInfo.CurrentCulture.Name;
			string searchPattern = context.WordToComplete + "*";
			WildcardPattern wildcardPattern = new WildcardPattern("about_*.help.txt", WildcardOptions.IgnoreCase);
			string[] array = null;
			try
			{
				array = Directory.GetFiles(path, searchPattern);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			if (array != null)
			{
				foreach (string text in array)
				{
					if (text != null)
					{
						try
						{
							string fileName = Path.GetFileName(text);
							if (fileName != null && wildcardPattern.IsMatch(fileName))
							{
								string completionText = fileName.Substring(0, fileName.Length - 9);
								list.Add(new CompletionResult(completionText));
							}
						}
						catch (Exception e2)
						{
							CommandProcessorBase.CheckForSevereException(e2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x001E3CDC File Offset: 0x001E1EDC
		internal static List<CompletionResult> CompleteStatementFlags(TokenKind kind, string wordToComplete)
		{
			if (kind == TokenKind.Switch)
			{
				wordToComplete = wordToComplete.Substring(1);
				bool withColon = wordToComplete.EndsWith(":", StringComparison.Ordinal);
				wordToComplete = (withColon ? wordToComplete.Remove(wordToComplete.Length - 1) : wordToComplete);
				string text = LanguagePrimitives.EnumSingleTypeConverter.EnumValues(typeof(SwitchFlags));
				string listSeparator = CultureInfo.CurrentUICulture.TextInfo.ListSeparator;
				string[] array = text.Split(new string[]
				{
					listSeparator
				}, StringSplitOptions.RemoveEmptyEntries);
				WildcardPattern wildcardPattern = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);
				List<string> list = new List<string>();
				List<CompletionResult> list2 = new List<CompletionResult>();
				CompletionResult completionResult = null;
				foreach (string text2 in array)
				{
					if (!text2.Equals(SwitchFlags.None.ToString(), StringComparison.OrdinalIgnoreCase))
					{
						if (wordToComplete.Equals(text2, StringComparison.OrdinalIgnoreCase))
						{
							string completionText = withColon ? ("-" + text2 + ":") : ("-" + text2);
							completionResult = new CompletionResult(completionText, text2, CompletionResultType.ParameterName, text2);
						}
						else if (wildcardPattern.IsMatch(text2))
						{
							list.Add(text2);
						}
					}
				}
				if (completionResult != null)
				{
					list2.Add(completionResult);
				}
				list.Sort();
				list2.AddRange(from entry in list
				let completionText = withColon ? ("-" + entry + ":") : ("-" + entry)
				select new CompletionResult(completionText, entry, CompletionResultType.ParameterName, entry));
				return list2;
			}
			return null;
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x001E3EB4 File Offset: 0x001E20B4
		internal static List<CompletionResult> CompleteHashtableKeyForDynamicKeyword(CompletionContext completionContext, DynamicKeywordStatementAst ast, HashtableAst hashtableAst)
		{
			List<CompletionResult> list = null;
			Dictionary<string, DynamicKeywordProperty> properties = ast.Keyword.Properties;
			string pattern = completionContext.WordToComplete + "*";
			List<string> propertiesName = new List<string>();
			int offset = completionContext.CursorPosition.Offset;
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
			{
				StringConstantExpressionAst stringConstantExpressionAst = tuple.Item1 as StringConstantExpressionAst;
				if (stringConstantExpressionAst != null && stringConstantExpressionAst.Extent.EndOffset != offset)
				{
					propertiesName.Add(stringConstantExpressionAst.Value);
				}
			}
			if (properties.Count > 0)
			{
				IEnumerable<KeyValuePair<string, DynamicKeywordProperty>> enumerable = from p in properties
				where !propertiesName.Contains(p.Key, StringComparer.OrdinalIgnoreCase)
				select p;
				if (enumerable != null && enumerable.Any<KeyValuePair<string, DynamicKeywordProperty>>())
				{
					list = new List<CompletionResult>();
					WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
					IEnumerable<KeyValuePair<string, DynamicKeywordProperty>> enumerable2 = from p in enumerable
					where wildcardPattern.IsMatch(p.Key)
					select p;
					if (enumerable2 == null || !enumerable2.Any<KeyValuePair<string, DynamicKeywordProperty>>())
					{
						enumerable2 = enumerable;
					}
					foreach (KeyValuePair<string, DynamicKeywordProperty> keyValuePair in enumerable2)
					{
						string text = LanguagePrimitives.ConvertTypeNameToPSTypeName(keyValuePair.Value.TypeConstraint);
						if (text == "[]")
						{
							text = "[" + keyValuePair.Value.TypeConstraint + "]";
						}
						if (string.Equals(text, "[MSFT_Credential]", StringComparison.OrdinalIgnoreCase))
						{
							text = "[pscredential]";
						}
						list.Add(new CompletionResult(keyValuePair.Key + " = ", keyValuePair.Key, CompletionResultType.Property, text));
					}
				}
			}
			return list;
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x001E40B4 File Offset: 0x001E22B4
		internal static List<CompletionResult> CompleteHashtableKey(CompletionContext completionContext, HashtableAst hashtableAst)
		{
			ConvertExpressionAst convertExpressionAst = hashtableAst.Parent as ConvertExpressionAst;
			if (convertExpressionAst != null)
			{
				List<CompletionResult> list = new List<CompletionResult>();
				CompletionCompleters.CompleteMemberByInferredType(completionContext, convertExpressionAst.GetInferredType(completionContext), list, completionContext.WordToComplete + "*", new Func<object, bool>(CompletionCompleters.IsWriteablePropertyMember), false);
				return list;
			}
			Ast parent = hashtableAst.Parent;
			DynamicKeywordStatementAst dynamicKeywordStatementAst = parent as DynamicKeywordStatementAst;
			if (dynamicKeywordStatementAst != null)
			{
				return CompletionCompleters.CompleteHashtableKeyForDynamicKeyword(completionContext, dynamicKeywordStatementAst, hashtableAst);
			}
			if (parent is ArrayLiteralAst)
			{
				parent = parent.Parent;
			}
			if (parent is CommandParameterAst)
			{
				parent = parent.Parent;
			}
			CommandAst commandAst = parent as CommandAst;
			if (commandAst != null)
			{
				PseudoBindingInfo pseudoBindingInfo = new PseudoParameterBinder().DoPseudoParameterBinding(commandAst, null, null, PseudoParameterBinder.BindingType.ArgumentCompletion);
				string text = null;
				foreach (KeyValuePair<string, AstParameterArgumentPair> keyValuePair in pseudoBindingInfo.BoundArguments)
				{
					AstPair astPair = keyValuePair.Value as AstPair;
					if (astPair != null)
					{
						if (astPair.Argument == hashtableAst)
						{
							text = keyValuePair.Key;
							break;
						}
					}
					else
					{
						AstArrayPair astArrayPair = keyValuePair.Value as AstArrayPair;
						if (astArrayPair != null && astArrayPair.Argument.Contains(hashtableAst))
						{
							text = keyValuePair.Key;
							break;
						}
					}
				}
				if (text != null)
				{
					string commandName2;
					if (text.Equals("GroupBy", StringComparison.OrdinalIgnoreCase))
					{
						string commandName;
						if ((commandName = pseudoBindingInfo.CommandName) != null && (commandName == "Format-Table" || commandName == "Format-List" || commandName == "Format-Wide" || commandName == "Format-Custom"))
						{
							return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
							{
								"Expression",
								"FormatString",
								"Label"
							});
						}
						return null;
					}
					else if (text.Equals("Property", StringComparison.OrdinalIgnoreCase) && (commandName2 = pseudoBindingInfo.CommandName) != null)
					{
						if (<PrivateImplementationDetails>{BF058111-6D89-4D83-B656-1DDD29798561}.$$method0x600572d-1 == null)
						{
							<PrivateImplementationDetails>{BF058111-6D89-4D83-B656-1DDD29798561}.$$method0x600572d-1 = new Dictionary<string, int>(7)
							{
								{
									"New-Object",
									0
								},
								{
									"Sort-Object",
									1
								},
								{
									"Group-Object",
									2
								},
								{
									"Format-Table",
									3
								},
								{
									"Format-List",
									4
								},
								{
									"Format-Wide",
									5
								},
								{
									"Format-Custom",
									6
								}
							};
						}
						int num;
						if (<PrivateImplementationDetails>{BF058111-6D89-4D83-B656-1DDD29798561}.$$method0x600572d-1.TryGetValue(commandName2, out num))
						{
							switch (num)
							{
							case 0:
							{
								IEnumerable<PSTypeName> inferredType = commandAst.GetInferredType(completionContext);
								List<CompletionResult> list2 = new List<CompletionResult>();
								CompletionCompleters.CompleteMemberByInferredType(completionContext, inferredType, list2, completionContext.WordToComplete + "*", new Func<object, bool>(CompletionCompleters.IsWriteablePropertyMember), false);
								return list2;
							}
							case 1:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression",
									"Ascending",
									"Descending"
								});
							case 2:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression"
								});
							case 3:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression",
									"FormatString",
									"Label",
									"Width",
									"Alignment"
								});
							case 4:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression",
									"FormatString",
									"Label"
								});
							case 5:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression",
									"FormatString"
								});
							case 6:
								return CompletionCompleters.GetSpecialHashTableKeyMembers(new string[]
								{
									"Expression",
									"Depth"
								});
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x001E447B File Offset: 0x001E267B
		private static List<CompletionResult> GetSpecialHashTableKeyMembers(params string[] keys)
		{
			return (from key in keys
			select new CompletionResult(key, key, CompletionResultType.Property, key)).ToList<CompletionResult>();
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x001E44A8 File Offset: 0x001E26A8
		internal static PowerShell AddCommandWithPreferenceSetting(PowerShell powershell, string command, Type type = null)
		{
			if (type != null)
			{
				CmdletInfo commandInfo = new CmdletInfo(command, type);
				powershell.AddCommand(commandInfo);
			}
			else
			{
				powershell.AddCommand(command);
			}
			powershell.AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false);
			return powershell;
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x001E452C File Offset: 0x001E272C
		internal static bool IsPathSafelyExpandable(ExpandableStringExpressionAst expandableStringAst, string extraText, ExecutionContext executionContext, out string expandedString)
		{
			expandedString = null;
			StringConstantType stringConstantType = expandableStringAst.StringConstantType;
			if (stringConstantType == StringConstantType.DoubleQuotedHereString)
			{
				return false;
			}
			List<string> list = new List<string>();
			foreach (ExpressionAst expressionAst in expandableStringAst.NestedExpressions)
			{
				VariableExpressionAst variableExpressionAst = expressionAst as VariableExpressionAst;
				if (variableExpressionAst == null)
				{
					return false;
				}
				string text = CompletionCompleters.CombineVariableWithPartialPath(variableExpressionAst, null, executionContext);
				if (text == null)
				{
					return false;
				}
				list.Add(text);
			}
			string str = string.Format(CultureInfo.InvariantCulture, expandableStringAst.FormatExpression, list.ToArray());
			string text2 = (stringConstantType == StringConstantType.DoubleQuoted) ? "\"" : string.Empty;
			expandedString = text2 + str + extraText + text2;
			return true;
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x001E45F8 File Offset: 0x001E27F8
		internal static string CombineVariableWithPartialPath(VariableExpressionAst variableAst, string extraText, ExecutionContext executionContext)
		{
			VariablePath variablePath = variableAst.VariablePath;
			if (!variablePath.IsVariable)
			{
				if (!variablePath.DriveName.Equals("env", StringComparison.OrdinalIgnoreCase))
				{
					goto IL_82;
				}
			}
			try
			{
				object variableValue = VariableOps.GetVariableValue(variablePath, executionContext, variableAst);
				string text = (variableValue == null) ? string.Empty : (variableValue as string);
				if (text == null)
				{
					object obj = PSObject.Base(variableValue);
					if (obj is string || obj.GetType().GetTypeInfo().IsPrimitive)
					{
						text = LanguagePrimitives.ConvertTo<string>(variableValue);
					}
				}
				if (text != null)
				{
					return text + extraText;
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			IL_82:
			return null;
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x001E469C File Offset: 0x001E289C
		internal static string HandleDoubleAndSingleQuote(ref string wordToComplete)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(wordToComplete) && (wordToComplete[0].IsSingleQuote() || wordToComplete[0].IsDoubleQuote()))
			{
				char c = wordToComplete[0];
				int length = wordToComplete.Length;
				if (length == 1)
				{
					wordToComplete = string.Empty;
					result = (c.IsSingleQuote() ? "'" : "\"");
				}
				else if (length > 1)
				{
					if ((wordToComplete[length - 1].IsDoubleQuote() && c.IsDoubleQuote()) || (wordToComplete[length - 1].IsSingleQuote() && c.IsSingleQuote()))
					{
						wordToComplete = wordToComplete.Substring(1, length - 2);
						result = (c.IsSingleQuote() ? "'" : "\"");
					}
					else if (!wordToComplete[length - 1].IsDoubleQuote() && !wordToComplete[length - 1].IsSingleQuote())
					{
						wordToComplete = wordToComplete.Substring(1);
						result = (c.IsSingleQuote() ? "'" : "\"");
					}
				}
			}
			return result;
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x001E47B1 File Offset: 0x001E29B1
		internal static bool IsSplattedVariable(Ast targetExpr)
		{
			return targetExpr is VariableExpressionAst && ((VariableExpressionAst)targetExpr).Splatted;
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x001E47CC File Offset: 0x001E29CC
		internal static void CompleteMemberHelper(bool @static, string memberName, ExpressionAst targetExpr, CompletionContext context, List<CompletionResult> results)
		{
			object obj;
			if (SafeExprEvaluator.TrySafeEval(targetExpr, context.ExecutionContext, out obj) && obj != null)
			{
				if (targetExpr is ArrayExpressionAst && !(obj is object[]))
				{
					obj = new object[]
					{
						obj
					};
				}
				PowerShell currentPowerShell = context.Helper.CurrentPowerShell;
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Core\\Where-Object", null).AddParameter("Property", "Name").AddParameter("Like").AddParameter("Value", memberName);
				CompletionCompleters.AddCommandWithPreferenceSetting(currentPowerShell, "Microsoft.PowerShell.Utility\\Sort-Object", null).AddParameter("Property", new object[]
				{
					"MemberType",
					"Name"
				});
				IEnumerable input;
				if (@static)
				{
					Type type = PSObject.Base(obj) as Type;
					if (type == null)
					{
						return;
					}
					input = PSObject.dotNetStaticAdapter.BaseGetMembers<PSMemberInfo>(type);
				}
				else
				{
					input = PSObject.AsPSObject(obj).Members;
				}
				Exception ex;
				Collection<PSObject> collection = context.Helper.ExecuteCurrentPowerShell(out ex, input);
				foreach (PSObject obj2 in collection)
				{
					PSMemberInfo psmemberInfo = (PSMemberInfo)PSObject.Base(obj2);
					if (!psmemberInfo.IsHidden)
					{
						string text = psmemberInfo.Name;
						if (text.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) != -1)
						{
							text = text.Replace("'", "''");
							text = "'" + text + "'";
						}
						bool flag = psmemberInfo is PSMethodInfo;
						if (flag)
						{
							if (psmemberInfo is PSMethod && ((PSMethod)psmemberInfo).IsSpecial)
							{
								continue;
							}
							text += '(';
						}
						string text2 = psmemberInfo.ToString();
						if (text2.IndexOf("),", StringComparison.OrdinalIgnoreCase) != -1)
						{
							string[] array = text2.Split(new string[]
							{
								"),"
							}, StringSplitOptions.RemoveEmptyEntries);
							StringBuilder stringBuilder = new StringBuilder();
							foreach (string text3 in array)
							{
								stringBuilder.Append(text3.Trim() + ")\r\n");
							}
							stringBuilder.Remove(stringBuilder.Length - 3, 3);
							text2 = stringBuilder.ToString();
						}
						results.Add(new CompletionResult(text, psmemberInfo.Name, flag ? CompletionResultType.Method : CompletionResultType.Property, text2));
					}
				}
				IDictionary dictionary = PSObject.Base(obj) as IDictionary;
				if (dictionary != null)
				{
					WildcardPattern wildcardPattern = new WildcardPattern(memberName, WildcardOptions.IgnoreCase);
					foreach (object obj3 in dictionary)
					{
						string text4 = ((DictionaryEntry)obj3).Key as string;
						if (text4 != null && wildcardPattern.IsMatch(text4))
						{
							if (text4.IndexOfAny(CompletionCompleters.CharactersRequiringQuotes) != -1)
							{
								text4 = text4.Replace("'", "''");
								text4 = "'" + text4 + "'";
							}
							results.Add(new CompletionResult(text4, text4, CompletionResultType.Property, text4));
						}
					}
				}
				if (!@static && CompletionCompleters.IsValueEnumerable(PSObject.Base(obj)))
				{
					CompletionCompleters.CompleteExtensionMethods(memberName, results);
				}
			}
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x001E4B44 File Offset: 0x001E2D44
		private static bool IsValueEnumerable(object value)
		{
			object obj = PSObject.Base(value);
			return obj != null && !(obj is string) && !(obj is PSObject) && !(obj is IDictionary) && !(obj is XmlNode) && (obj is IEnumerable || obj is IEnumerator || obj is DataTable);
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x001E4B98 File Offset: 0x001E2D98
		private static bool IsStaticTypeEnumerable(Type type)
		{
			return !type.Equals(typeof(string)) && !typeof(IDictionary).IsAssignableFrom(type) && !typeof(XmlNode).IsAssignableFrom(type) && (typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerator).IsAssignableFrom(type));
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x001E4C10 File Offset: 0x001E2E10
		private static bool CompletionRequiresQuotes(string completion, bool escape)
		{
			Token[] array;
			ParseError[] array2;
			Parser.ParseInput(completion, out array, out array2);
			char[] array3;
			if (!escape)
			{
				array3 = new char[]
				{
					'$',
					'`'
				};
			}
			else
			{
				RuntimeHelpers.InitializeArray(array3 = new char[4], fieldof(<PrivateImplementationDetails>{BF058111-6D89-4D83-B656-1DDD29798561}.$$method0x6005737-1).FieldHandle);
			}
			char[] anyOf = array3;
			bool flag = array2.Length != 0 || array.Length != 2;
			if ((!flag && array[0] is StringToken) || (array.Length == 2 && (array[0].TokenFlags & TokenFlags.Keyword) != TokenFlags.None))
			{
				flag = false;
				string text = array[0].Text;
				if (text.IndexOfAny(anyOf) != -1)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x001E4CA4 File Offset: 0x001E2EA4
		private static bool ProviderSpecified(string path)
		{
			int num = path.IndexOf(':');
			return num != -1 && num + 1 < path.Length && path[num + 1] == ':';
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x001E4CD8 File Offset: 0x001E2ED8
		private static Type GetEffectiveParameterType(Type type)
		{
			Type underlyingType = Nullable.GetUnderlyingType(type);
			return underlyingType ?? type;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x001E4CF4 File Offset: 0x001E2EF4
		private static bool TurnOnLiteralPathOption(CompletionContext completionContext)
		{
			bool result = false;
			if (completionContext.Options == null)
			{
				completionContext.Options = new Hashtable
				{
					{
						"LiteralPaths",
						true
					}
				};
				result = true;
			}
			else if (!completionContext.Options.ContainsKey("LiteralPaths"))
			{
				completionContext.Options.Add("LiteralPaths", true);
				result = true;
			}
			return result;
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x001E4D58 File Offset: 0x001E2F58
		internal static bool IsAmpersandNeeded(CompletionContext context, bool defaultChoice)
		{
			if (context.RelatedAsts != null && !string.IsNullOrEmpty(context.WordToComplete))
			{
				Ast ast = context.RelatedAsts.Last<Ast>();
				CommandAst commandAst = ast.Parent as CommandAst;
				if (commandAst != null && commandAst.CommandElements.Count == 1 && ((!defaultChoice && commandAst.InvocationOperator == TokenKind.Unknown) || (defaultChoice && commandAst.InvocationOperator != TokenKind.Unknown)))
				{
					defaultChoice = !defaultChoice;
				}
			}
			return defaultChoice;
		}

		// Token: 0x04002FF4 RID: 12276
		private const int MAX_PREFERRED_LENGTH = -1;

		// Token: 0x04002FF5 RID: 12277
		private const int NERR_Success = 0;

		// Token: 0x04002FF6 RID: 12278
		private const int ERROR_MORE_DATA = 234;

		// Token: 0x04002FF7 RID: 12279
		private const int STYPE_DISKTREE = 0;

		// Token: 0x04002FF8 RID: 12280
		private const int STYPE_MASK = 255;

		// Token: 0x04002FF9 RID: 12281
		private static readonly HashSet<string> KeywordsToExcludeFromAddingAmpersand = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			TokenKind.InlineScript.ToString(),
			TokenKind.Configuration.ToString()
		};

		// Token: 0x04002FFA RID: 12282
		internal static readonly List<string> PseudoWorkflowCommands = new List<string>
		{
			"Checkpoint-Workflow",
			"Suspend-Workflow",
			"InlineScript"
		};

		// Token: 0x04002FFB RID: 12283
		private static string[] ParameterNamesOfImportDSCResource = new string[]
		{
			"Name",
			"ModuleName",
			"ModuleVersion"
		};

		// Token: 0x04002FFC RID: 12284
		private static ConcurrentDictionary<string, IEnumerable<string>> cimNamespaceAndClassNameToAssociationResultClassNames = new ConcurrentDictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002FFD RID: 12285
		private static ConcurrentDictionary<string, IEnumerable<string>> cimNamespaceToClassNames = new ConcurrentDictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002FFE RID: 12286
		private static readonly string[] VariableScopes = new string[]
		{
			"Global:",
			"Local:",
			"Script:",
			"Private:"
		};

		// Token: 0x04002FFF RID: 12287
		private static readonly char[] CharactersRequiringQuotes = new char[]
		{
			'-',
			'`',
			'&',
			'@',
			'\'',
			'"',
			'#',
			'{',
			'}',
			'(',
			')',
			'$',
			',',
			';',
			'|',
			'<',
			'>',
			' ',
			'.',
			'\\',
			'/',
			'\t',
			'^'
		};

		// Token: 0x04003000 RID: 12288
		private static readonly Lazy<SortedSet<string>> _specialVariablesCache = new Lazy<SortedSet<string>>(new Func<SortedSet<string>>(CompletionCompleters.BuildSpecialVariablesCache));

		// Token: 0x04003001 RID: 12289
		private static readonly List<Tuple<string, string>> ExtensionMethods = new List<Tuple<string, string>>
		{
			new Tuple<string, string>("Where", "Where({ expression } [, mode [, numberToReturn]])"),
			new Tuple<string, string>("ForEach", "ForEach(expression [, arguments...])")
		};

		// Token: 0x04003002 RID: 12290
		private static readonly HashSet<string> DscCollectionVariables = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"SelectedNodes",
			"AllNodes"
		};

		// Token: 0x04003003 RID: 12291
		private static CompletionCompleters.TypeCompletionMapping[][] typeCache;

		// Token: 0x0200097C RID: 2428
		private class FindFunctionsVisitor : AstVisitor
		{
			// Token: 0x060059D5 RID: 22997 RVA: 0x001E4DC0 File Offset: 0x001E2FC0
			public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
			{
				this.FunctionDefinitions.Add(functionDefinitionAst);
				return AstVisitAction.Continue;
			}

			// Token: 0x04003022 RID: 12322
			internal readonly List<FunctionDefinitionAst> FunctionDefinitions = new List<FunctionDefinitionAst>();
		}

		// Token: 0x0200097D RID: 2429
		private sealed class ArgumentLocation
		{
			// Token: 0x170011F5 RID: 4597
			// (get) Token: 0x060059D7 RID: 22999 RVA: 0x001E4DE2 File Offset: 0x001E2FE2
			// (set) Token: 0x060059D8 RID: 23000 RVA: 0x001E4DEA File Offset: 0x001E2FEA
			internal bool IsPositional { get; set; }

			// Token: 0x170011F6 RID: 4598
			// (get) Token: 0x060059D9 RID: 23001 RVA: 0x001E4DF3 File Offset: 0x001E2FF3
			// (set) Token: 0x060059DA RID: 23002 RVA: 0x001E4DFB File Offset: 0x001E2FFB
			internal int Position { get; set; }

			// Token: 0x170011F7 RID: 4599
			// (get) Token: 0x060059DB RID: 23003 RVA: 0x001E4E04 File Offset: 0x001E3004
			// (set) Token: 0x060059DC RID: 23004 RVA: 0x001E4E0C File Offset: 0x001E300C
			internal AstParameterArgumentPair Argument { get; set; }
		}

		// Token: 0x0200097E RID: 2430
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct SHARE_INFO_1
		{
			// Token: 0x04003026 RID: 12326
			public string netname;

			// Token: 0x04003027 RID: 12327
			public int type;

			// Token: 0x04003028 RID: 12328
			public string remark;
		}

		// Token: 0x0200097F RID: 2431
		private class FindVariablesVisitor : AstVisitor
		{
			// Token: 0x060059DE RID: 23006 RVA: 0x001E4E1D File Offset: 0x001E301D
			public override AstVisitAction VisitVariableExpression(VariableExpressionAst variableExpressionAst)
			{
				if (variableExpressionAst != this.CompletionVariableAst)
				{
					this.VariableSources.Add(new Tuple<string, Ast>(variableExpressionAst.VariablePath.UserPath, variableExpressionAst));
				}
				return AstVisitAction.Continue;
			}

			// Token: 0x060059DF RID: 23007 RVA: 0x001E4E48 File Offset: 0x001E3048
			public override AstVisitAction VisitCommand(CommandAst commandAst)
			{
				if (commandAst != this.CompletionVariableAst && !this.CompletionVariableAst.Extent.IsWithin(commandAst.Extent))
				{
					string[] array = new string[]
					{
						"PV",
						"PipelineVariable",
						"OV",
						"OutVariable"
					};
					StaticBindingResult staticBindingResult = StaticParameterBinder.BindCommand(commandAst, false, array);
					if (staticBindingResult != null)
					{
						foreach (string key in array)
						{
							ParameterBindingResult parameterBindingResult;
							if (staticBindingResult.BoundParameters.TryGetValue(key, out parameterBindingResult))
							{
								this.VariableSources.Add(new Tuple<string, Ast>((string)parameterBindingResult.ConstantValue, commandAst));
							}
						}
					}
				}
				return AstVisitAction.Continue;
			}

			// Token: 0x060059E0 RID: 23008 RVA: 0x001E4F00 File Offset: 0x001E3100
			public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
			{
				if (functionDefinitionAst == this.Top)
				{
					return AstVisitAction.Continue;
				}
				return AstVisitAction.SkipChildren;
			}

			// Token: 0x060059E1 RID: 23009 RVA: 0x001E4F0E File Offset: 0x001E310E
			public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
			{
				if (scriptBlockExpressionAst == this.Top)
				{
					return AstVisitAction.Continue;
				}
				return AstVisitAction.SkipChildren;
			}

			// Token: 0x060059E2 RID: 23010 RVA: 0x001E4F1C File Offset: 0x001E311C
			public override AstVisitAction VisitScriptBlock(ScriptBlockAst scriptBlockAst)
			{
				if (scriptBlockAst == this.Top)
				{
					return AstVisitAction.Continue;
				}
				return AstVisitAction.SkipChildren;
			}

			// Token: 0x04003029 RID: 12329
			internal Ast Top;

			// Token: 0x0400302A RID: 12330
			internal Ast CompletionVariableAst;

			// Token: 0x0400302B RID: 12331
			internal readonly List<Tuple<string, Ast>> VariableSources = new List<Tuple<string, Ast>>();
		}

		// Token: 0x02000980 RID: 2432
		private abstract class TypeCompletionBase
		{
			// Token: 0x060059E4 RID: 23012
			internal abstract CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix);

			// Token: 0x060059E5 RID: 23013
			internal abstract CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove);

			// Token: 0x060059E6 RID: 23014 RVA: 0x001E4F40 File Offset: 0x001E3140
			internal static string RemoveBackTick(string typeName)
			{
				int num = typeName.LastIndexOf('`');
				if (num != -1)
				{
					return typeName.Substring(0, num);
				}
				return typeName;
			}
		}

		// Token: 0x02000981 RID: 2433
		private class TypeCompletionInStringFormat : CompletionCompleters.TypeCompletionBase
		{
			// Token: 0x170011F8 RID: 4600
			// (get) Token: 0x060059E8 RID: 23016 RVA: 0x001E4F6C File Offset: 0x001E316C
			internal string ShortTypeName
			{
				get
				{
					if (this.shortTypeName == null)
					{
						int num = this.FullTypeName.LastIndexOf('.');
						int num2 = this.FullTypeName.LastIndexOf('+');
						this.shortTypeName = ((num2 != -1) ? this.FullTypeName.Substring(num2 + 1) : this.FullTypeName.Substring(num + 1));
					}
					return this.shortTypeName;
				}
			}

			// Token: 0x170011F9 RID: 4601
			// (get) Token: 0x060059E9 RID: 23017 RVA: 0x001E4FCC File Offset: 0x001E31CC
			internal string Namespace
			{
				get
				{
					if (this.@namespace == null)
					{
						int length = this.FullTypeName.LastIndexOf('.');
						this.@namespace = this.FullTypeName.Substring(0, length);
					}
					return this.@namespace;
				}
			}

			// Token: 0x060059EA RID: 23018 RVA: 0x001E5008 File Offset: 0x001E3208
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix)
			{
				return this.GetCompletionResult(keyMatched, prefix, suffix, null);
			}

			// Token: 0x060059EB RID: 23019 RVA: 0x001E5014 File Offset: 0x001E3214
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove)
			{
				string str = string.IsNullOrEmpty(namespaceToRemove) ? this.FullTypeName : this.FullTypeName.Substring(namespaceToRemove.Length + 1);
				string listItemText = this.ShortTypeName;
				string fullTypeName = this.FullTypeName;
				return new CompletionResult(prefix + str + suffix, listItemText, CompletionResultType.Type, fullTypeName);
			}

			// Token: 0x0400302C RID: 12332
			internal string FullTypeName;

			// Token: 0x0400302D RID: 12333
			private string shortTypeName;

			// Token: 0x0400302E RID: 12334
			private string @namespace;
		}

		// Token: 0x02000982 RID: 2434
		private class GenericTypeCompletionInStringFormat : CompletionCompleters.TypeCompletionInStringFormat
		{
			// Token: 0x170011FA RID: 4602
			// (get) Token: 0x060059ED RID: 23021 RVA: 0x001E5070 File Offset: 0x001E3270
			private int GenericArgumentCount
			{
				get
				{
					if (this.genericArgumentCount == 0)
					{
						int num = this.FullTypeName.LastIndexOf('`');
						string valueToConvert = this.FullTypeName.Substring(num + 1);
						this.genericArgumentCount = LanguagePrimitives.ConvertTo<int>(valueToConvert);
					}
					return this.genericArgumentCount;
				}
			}

			// Token: 0x060059EE RID: 23022 RVA: 0x001E50B4 File Offset: 0x001E32B4
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix)
			{
				return this.GetCompletionResult(keyMatched, prefix, suffix, null);
			}

			// Token: 0x060059EF RID: 23023 RVA: 0x001E50C0 File Offset: 0x001E32C0
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove)
			{
				string text = CompletionCompleters.TypeCompletionBase.RemoveBackTick(this.FullTypeName);
				string str = string.IsNullOrEmpty(namespaceToRemove) ? text : text.Substring(namespaceToRemove.Length + 1);
				string str2 = CompletionCompleters.TypeCompletionBase.RemoveBackTick(base.ShortTypeName);
				string listItemText = str2 + "<>";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(text);
				stringBuilder.Append('[');
				for (int i = 0; i < this.GenericArgumentCount; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append((this.GenericArgumentCount == 1) ? "T" : string.Format(CultureInfo.InvariantCulture, "T{0}", new object[]
					{
						i + 1
					}));
				}
				stringBuilder.Append(']');
				return new CompletionResult(prefix + str + suffix, listItemText, CompletionResultType.Type, stringBuilder.ToString());
			}

			// Token: 0x0400302F RID: 12335
			private int genericArgumentCount;
		}

		// Token: 0x02000983 RID: 2435
		private class TypeCompletion : CompletionCompleters.TypeCompletionBase
		{
			// Token: 0x060059F1 RID: 23025 RVA: 0x001E51B4 File Offset: 0x001E33B4
			protected string GetTooltipPrefix()
			{
				TypeInfo typeInfo = this.Type.GetTypeInfo();
				if (typeof(Delegate).IsAssignableFrom(this.Type))
				{
					return "Delegate ";
				}
				if (typeInfo.IsInterface)
				{
					return "Interface ";
				}
				if (typeInfo.IsClass)
				{
					return "Class ";
				}
				if (typeInfo.IsEnum)
				{
					return "Enum ";
				}
				if (typeof(ValueType).IsAssignableFrom(this.Type))
				{
					return "Struct ";
				}
				return "";
			}

			// Token: 0x060059F2 RID: 23026 RVA: 0x001E5236 File Offset: 0x001E3436
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix)
			{
				return this.GetCompletionResult(keyMatched, prefix, suffix, null);
			}

			// Token: 0x060059F3 RID: 23027 RVA: 0x001E5244 File Offset: 0x001E3444
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove)
			{
				string text = ToStringCodeMethods.Type(this.Type, false);
				if (keyMatched.IndexOf('.') != -1 && text.IndexOf('.') == -1)
				{
					text = this.Type.FullName;
				}
				if (!string.IsNullOrEmpty(namespaceToRemove) && text.Equals(this.Type.FullName, StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(namespaceToRemove.Length + 1);
				}
				string name = this.Type.Name;
				string toolTip = this.GetTooltipPrefix() + this.Type.FullName;
				return new CompletionResult(prefix + text + suffix, name, CompletionResultType.Type, toolTip);
			}

			// Token: 0x04003030 RID: 12336
			internal Type Type;
		}

		// Token: 0x02000984 RID: 2436
		private class GenericTypeCompletion : CompletionCompleters.TypeCompletion
		{
			// Token: 0x060059F5 RID: 23029 RVA: 0x001E52E9 File Offset: 0x001E34E9
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix)
			{
				return this.GetCompletionResult(keyMatched, prefix, suffix, null);
			}

			// Token: 0x060059F6 RID: 23030 RVA: 0x001E52F8 File Offset: 0x001E34F8
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove)
			{
				string text = CompletionCompleters.TypeCompletionBase.RemoveBackTick(this.Type.FullName);
				string str = string.IsNullOrEmpty(namespaceToRemove) ? text : text.Substring(namespaceToRemove.Length + 1);
				string str2 = CompletionCompleters.TypeCompletionBase.RemoveBackTick(this.Type.Name);
				string listItemText = str2 + "<>";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.GetTooltipPrefix());
				stringBuilder.Append(text);
				stringBuilder.Append('[');
				Type[] genericArguments = this.Type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(genericArguments[i].Name);
				}
				stringBuilder.Append(']');
				return new CompletionResult(prefix + str + suffix, listItemText, CompletionResultType.Type, stringBuilder.ToString());
			}
		}

		// Token: 0x02000985 RID: 2437
		private class NamespaceCompletion : CompletionCompleters.TypeCompletionBase
		{
			// Token: 0x060059F8 RID: 23032 RVA: 0x001E53E4 File Offset: 0x001E35E4
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix)
			{
				string text = this.Namespace;
				int num = text.LastIndexOf('.');
				if (num != -1)
				{
					text = text.Substring(num + 1);
				}
				return new CompletionResult(prefix + this.Namespace + suffix, text, CompletionResultType.Namespace, "Namespace " + this.Namespace);
			}

			// Token: 0x060059F9 RID: 23033 RVA: 0x001E5434 File Offset: 0x001E3634
			internal override CompletionResult GetCompletionResult(string keyMatched, string prefix, string suffix, string namespaceToRemove)
			{
				return this.GetCompletionResult(keyMatched, prefix, suffix);
			}

			// Token: 0x04003031 RID: 12337
			internal string Namespace;
		}

		// Token: 0x02000986 RID: 2438
		private class TypeCompletionMapping
		{
			// Token: 0x04003032 RID: 12338
			internal string Key;

			// Token: 0x04003033 RID: 12339
			internal List<CompletionCompleters.TypeCompletionBase> Completions = new List<CompletionCompleters.TypeCompletionBase>();
		}

		// Token: 0x02000987 RID: 2439
		private class ItemPathComparer : IComparer<PSObject>
		{
			// Token: 0x060059FC RID: 23036 RVA: 0x001E545C File Offset: 0x001E365C
			public int Compare(PSObject x, PSObject y)
			{
				PathInfo pathInfo = PSObject.Base(x) as PathInfo;
				FileSystemInfo fileSystemInfo = PSObject.Base(x) as FileSystemInfo;
				string text = PSObject.Base(x) as string;
				PathInfo pathInfo2 = PSObject.Base(y) as PathInfo;
				FileSystemInfo fileSystemInfo2 = PSObject.Base(y) as FileSystemInfo;
				string text2 = PSObject.Base(y) as string;
				string text3 = null;
				string text4 = null;
				if (pathInfo != null)
				{
					text3 = pathInfo.ProviderPath;
				}
				else if (fileSystemInfo != null)
				{
					text3 = fileSystemInfo.FullName;
				}
				else if (text != null)
				{
					text3 = text;
				}
				if (pathInfo2 != null)
				{
					text4 = pathInfo2.ProviderPath;
				}
				else if (fileSystemInfo2 != null)
				{
					text4 = fileSystemInfo2.FullName;
				}
				else if (text2 != null)
				{
					text4 = text2;
				}
				if (!string.IsNullOrEmpty(text3))
				{
					string.IsNullOrEmpty(text4);
				}
				return string.Compare(text3, text4, StringComparison.CurrentCultureIgnoreCase);
			}
		}

		// Token: 0x02000988 RID: 2440
		private class CommandNameComparer : IComparer<PSObject>
		{
			// Token: 0x060059FE RID: 23038 RVA: 0x001E5520 File Offset: 0x001E3720
			public int Compare(PSObject x, PSObject y)
			{
				object obj = PSObject.Base(x);
				object obj2 = PSObject.Base(y);
				CommandInfo commandInfo = obj as CommandInfo;
				string text = (commandInfo != null) ? commandInfo.Name : (obj as string);
				CommandInfo commandInfo2 = obj2 as CommandInfo;
				string strB = (commandInfo2 != null) ? commandInfo2.Name : (obj2 as string);
				if (text != null)
				{
				}
				return string.Compare(text, strB, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x02000BD6 RID: 3030
		[CompilerGenerated]
		private static class <CompleteModuleName>o__SiteContainere
		{
			// Token: 0x04003C1B RID: 15387
			public static CallSite<Func<CallSite, object, object>> <>p__Sitef;

			// Token: 0x04003C1C RID: 15388
			public static CallSite<Func<CallSite, object, object>> <>p__Site10;

			// Token: 0x04003C1D RID: 15389
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site11;

			// Token: 0x04003C1E RID: 15390
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site12;

			// Token: 0x04003C1F RID: 15391
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site13;

			// Token: 0x04003C20 RID: 15392
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site14;

			// Token: 0x04003C21 RID: 15393
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site15;

			// Token: 0x04003C22 RID: 15394
			public static CallSite<Func<CallSite, object, object>> <>p__Site16;

			// Token: 0x04003C23 RID: 15395
			public static CallSite<Func<CallSite, object, object>> <>p__Site17;

			// Token: 0x04003C24 RID: 15396
			public static CallSite<Func<CallSite, object, object>> <>p__Site18;

			// Token: 0x04003C25 RID: 15397
			public static CallSite<Func<CallSite, object, object>> <>p__Site19;

			// Token: 0x04003C26 RID: 15398
			public static CallSite<Func<CallSite, object, object>> <>p__Site1a;

			// Token: 0x04003C27 RID: 15399
			public static CallSite<Func<CallSite, object, object>> <>p__Site1b;

			// Token: 0x04003C28 RID: 15400
			public static CallSite<Func<CallSite, object, bool>> <>p__Site1c;

			// Token: 0x04003C29 RID: 15401
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Site1d;

			// Token: 0x04003C2A RID: 15402
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Site1e;

			// Token: 0x04003C2B RID: 15403
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site1f;

			// Token: 0x04003C2C RID: 15404
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site20;

			// Token: 0x04003C2D RID: 15405
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site21;

			// Token: 0x04003C2E RID: 15406
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site22;

			// Token: 0x04003C2F RID: 15407
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site23;
		}

		// Token: 0x02000BE1 RID: 3041
		[CompilerGenerated]
		private static class <NativeCompletionEventLogCommands>o__SiteContainer7b
		{
			// Token: 0x04003C55 RID: 15445
			public static CallSite<Func<CallSite, object, object>> <>p__Site7c;

			// Token: 0x04003C56 RID: 15446
			public static CallSite<Func<CallSite, object, object>> <>p__Site7d;

			// Token: 0x04003C57 RID: 15447
			public static CallSite<Func<CallSite, object, bool>> <>p__Site7e;

			// Token: 0x04003C58 RID: 15448
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Site7f;

			// Token: 0x04003C59 RID: 15449
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Site80;

			// Token: 0x04003C5A RID: 15450
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site81;

			// Token: 0x04003C5B RID: 15451
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site82;

			// Token: 0x04003C5C RID: 15452
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site83;

			// Token: 0x04003C5D RID: 15453
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site84;

			// Token: 0x04003C5E RID: 15454
			public static CallSite<Func<CallSite, object, bool>> <>p__Site85;

			// Token: 0x04003C5F RID: 15455
			public static CallSite<Func<CallSite, WildcardPattern, object, object>> <>p__Site86;

			// Token: 0x04003C60 RID: 15456
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site87;
		}

		// Token: 0x02000BE2 RID: 3042
		[CompilerGenerated]
		private static class <NativeCompletionJobCommands>o__SiteContainer88
		{
			// Token: 0x04003C61 RID: 15457
			public static CallSite<Func<CallSite, object, object>> <>p__Site89;

			// Token: 0x04003C62 RID: 15458
			public static CallSite<Func<CallSite, object, object>> <>p__Site8a;

			// Token: 0x04003C63 RID: 15459
			public static CallSite<Func<CallSite, object, bool>> <>p__Site8b;

			// Token: 0x04003C64 RID: 15460
			public static CallSite<Func<CallSite, WildcardPattern, object, object>> <>p__Site8c;

			// Token: 0x04003C65 RID: 15461
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site8d;

			// Token: 0x04003C66 RID: 15462
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site8e;

			// Token: 0x04003C67 RID: 15463
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site8f;

			// Token: 0x04003C68 RID: 15464
			public static CallSite<Func<CallSite, object, object>> <>p__Site90;

			// Token: 0x04003C69 RID: 15465
			public static CallSite<Func<CallSite, object, object>> <>p__Site91;

			// Token: 0x04003C6A RID: 15466
			public static CallSite<Func<CallSite, object, bool>> <>p__Site92;

			// Token: 0x04003C6B RID: 15467
			public static CallSite<Func<CallSite, WildcardPattern, object, object>> <>p__Site93;

			// Token: 0x04003C6C RID: 15468
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site94;

			// Token: 0x04003C6D RID: 15469
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site95;

			// Token: 0x04003C6E RID: 15470
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site96;

			// Token: 0x04003C6F RID: 15471
			public static CallSite<Func<CallSite, object, object>> <>p__Site97;

			// Token: 0x04003C70 RID: 15472
			public static CallSite<Func<CallSite, object, bool>> <>p__Site98;

			// Token: 0x04003C71 RID: 15473
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Site99;

			// Token: 0x04003C72 RID: 15474
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Site9a;

			// Token: 0x04003C73 RID: 15475
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site9b;

			// Token: 0x04003C74 RID: 15476
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site9c;

			// Token: 0x04003C75 RID: 15477
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site9d;

			// Token: 0x04003C76 RID: 15478
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site9e;

			// Token: 0x04003C77 RID: 15479
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site9f;
		}

		// Token: 0x02000BE3 RID: 3043
		[CompilerGenerated]
		private static class <NativeCompletionScheduledJobCommands>o__SiteContainera0
		{
			// Token: 0x04003C78 RID: 15480
			public static CallSite<Func<CallSite, object, object>> <>p__Sitea1;

			// Token: 0x04003C79 RID: 15481
			public static CallSite<Func<CallSite, object, object>> <>p__Sitea2;

			// Token: 0x04003C7A RID: 15482
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitea3;

			// Token: 0x04003C7B RID: 15483
			public static CallSite<Func<CallSite, WildcardPattern, object, object>> <>p__Sitea4;

			// Token: 0x04003C7C RID: 15484
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitea5;

			// Token: 0x04003C7D RID: 15485
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitea6;

			// Token: 0x04003C7E RID: 15486
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitea7;

			// Token: 0x04003C7F RID: 15487
			public static CallSite<Func<CallSite, object, object>> <>p__Sitea8;

			// Token: 0x04003C80 RID: 15488
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitea9;

			// Token: 0x04003C81 RID: 15489
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Siteaa;

			// Token: 0x04003C82 RID: 15490
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Siteab;

			// Token: 0x04003C83 RID: 15491
			public static CallSite<Func<CallSite, object, string, object>> <>p__Siteac;

			// Token: 0x04003C84 RID: 15492
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitead;

			// Token: 0x04003C85 RID: 15493
			public static CallSite<Func<CallSite, object, string, object>> <>p__Siteae;

			// Token: 0x04003C86 RID: 15494
			public static CallSite<Func<CallSite, string, object, object>> <>p__Siteaf;

			// Token: 0x04003C87 RID: 15495
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Siteb0;
		}

		// Token: 0x02000BE4 RID: 3044
		[CompilerGenerated]
		private static class <NativeCompletionProcessCommands>o__SiteContainerb6
		{
			// Token: 0x04003C88 RID: 15496
			public static CallSite<Func<CallSite, object, object>> <>p__Siteb7;

			// Token: 0x04003C89 RID: 15497
			public static CallSite<Func<CallSite, object, object>> <>p__Siteb8;

			// Token: 0x04003C8A RID: 15498
			public static CallSite<Func<CallSite, object, bool>> <>p__Siteb9;

			// Token: 0x04003C8B RID: 15499
			public static CallSite<Func<CallSite, WildcardPattern, object, object>> <>p__Siteba;

			// Token: 0x04003C8C RID: 15500
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitebb;

			// Token: 0x04003C8D RID: 15501
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitebc;

			// Token: 0x04003C8E RID: 15502
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitebd;

			// Token: 0x04003C8F RID: 15503
			public static CallSite<Func<CallSite, object, object>> <>p__Sitebe;

			// Token: 0x04003C90 RID: 15504
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitebf;

			// Token: 0x04003C91 RID: 15505
			public static CallSite<Func<CallSite, HashSet<string>, object, object>> <>p__Sitec0;

			// Token: 0x04003C92 RID: 15506
			public static CallSite<Action<CallSite, HashSet<string>, object>> <>p__Sitec1;

			// Token: 0x04003C93 RID: 15507
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitec2;

			// Token: 0x04003C94 RID: 15508
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Sitec3;

			// Token: 0x04003C95 RID: 15509
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitec4;

			// Token: 0x04003C96 RID: 15510
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitec5;

			// Token: 0x04003C97 RID: 15511
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitec6;

			// Token: 0x04003C98 RID: 15512
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitec7;

			// Token: 0x04003C99 RID: 15513
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitec8;

			// Token: 0x04003C9A RID: 15514
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitec9;
		}

		// Token: 0x02000BE5 RID: 3045
		[CompilerGenerated]
		private static class <NativeCompletionProviderCommands>o__SiteContainerca
		{
			// Token: 0x04003C9B RID: 15515
			public static CallSite<Func<CallSite, object, object>> <>p__Sitecb;

			// Token: 0x04003C9C RID: 15516
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitecc;

			// Token: 0x04003C9D RID: 15517
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Sitecd;

			// Token: 0x04003C9E RID: 15518
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitece;

			// Token: 0x04003C9F RID: 15519
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitecf;

			// Token: 0x04003CA0 RID: 15520
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sited0;

			// Token: 0x04003CA1 RID: 15521
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sited1;

			// Token: 0x04003CA2 RID: 15522
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sited2;

			// Token: 0x04003CA3 RID: 15523
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sited3;
		}

		// Token: 0x02000BE6 RID: 3046
		[CompilerGenerated]
		private static class <NativeCompletionDriveCommands>o__SiteContainerd4
		{
			// Token: 0x04003CA4 RID: 15524
			public static CallSite<Func<CallSite, object, object>> <>p__Sited5;

			// Token: 0x04003CA5 RID: 15525
			public static CallSite<Func<CallSite, object, bool>> <>p__Sited6;

			// Token: 0x04003CA6 RID: 15526
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Sited7;

			// Token: 0x04003CA7 RID: 15527
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sited8;

			// Token: 0x04003CA8 RID: 15528
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sited9;

			// Token: 0x04003CA9 RID: 15529
			public static CallSite<Func<CallSite, string, object, object>> <>p__Siteda;

			// Token: 0x04003CAA RID: 15530
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitedb;

			// Token: 0x04003CAB RID: 15531
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitedc;

			// Token: 0x04003CAC RID: 15532
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitedd;
		}

		// Token: 0x02000BE7 RID: 3047
		[CompilerGenerated]
		private static class <NativeCompletionServiceCommands>o__SiteContainerde
		{
			// Token: 0x04003CAD RID: 15533
			public static CallSite<Func<CallSite, object, object>> <>p__Sitedf;

			// Token: 0x04003CAE RID: 15534
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitee0;

			// Token: 0x04003CAF RID: 15535
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Sitee1;

			// Token: 0x04003CB0 RID: 15536
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitee2;

			// Token: 0x04003CB1 RID: 15537
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitee3;

			// Token: 0x04003CB2 RID: 15538
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitee4;

			// Token: 0x04003CB3 RID: 15539
			public static CallSite<Func<CallSite, object, string, object>> <>p__Sitee5;

			// Token: 0x04003CB4 RID: 15540
			public static CallSite<Func<CallSite, string, object, object>> <>p__Sitee6;

			// Token: 0x04003CB5 RID: 15541
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitee7;

			// Token: 0x04003CB6 RID: 15542
			public static CallSite<Func<CallSite, object, object>> <>p__Sitee8;

			// Token: 0x04003CB7 RID: 15543
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitee9;

			// Token: 0x04003CB8 RID: 15544
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Siteea;

			// Token: 0x04003CB9 RID: 15545
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Siteeb;

			// Token: 0x04003CBA RID: 15546
			public static CallSite<Func<CallSite, object, string, object>> <>p__Siteec;

			// Token: 0x04003CBB RID: 15547
			public static CallSite<Func<CallSite, string, object, object>> <>p__Siteed;

			// Token: 0x04003CBC RID: 15548
			public static CallSite<Func<CallSite, object, string, object>> <>p__Siteee;

			// Token: 0x04003CBD RID: 15549
			public static CallSite<Func<CallSite, string, object, object>> <>p__Siteef;

			// Token: 0x04003CBE RID: 15550
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Sitef0;
		}

		// Token: 0x02000BE8 RID: 3048
		[CompilerGenerated]
		private static class <NativeCompletionVariableCommands>o__SiteContainerf1
		{
			// Token: 0x04003CBF RID: 15551
			public static CallSite<Func<CallSite, object, object>> <>p__Sitef2;

			// Token: 0x04003CC0 RID: 15552
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitef3;

			// Token: 0x04003CC1 RID: 15553
			public static CallSite<Func<CallSite, object, int, object>> <>p__Sitef4;

			// Token: 0x04003CC2 RID: 15554
			public static CallSite<Func<CallSite, object, char[], object>> <>p__Sitef5;

			// Token: 0x04003CC3 RID: 15555
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitef6;

			// Token: 0x04003CC4 RID: 15556
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitef7;

			// Token: 0x04003CC5 RID: 15557
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitef8;

			// Token: 0x04003CC6 RID: 15558
			public static CallSite<Func<CallSite, object, object>> <>p__Sitef9;

			// Token: 0x04003CC7 RID: 15559
			public static CallSite<Func<CallSite, object, string, StringComparison, object>> <>p__Sitefa;

			// Token: 0x04003CC8 RID: 15560
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitefb;

			// Token: 0x04003CC9 RID: 15561
			public static CallSite<Func<CallSite, object, object, object>> <>p__Sitefc;

			// Token: 0x04003CCA RID: 15562
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Sitefd;

			// Token: 0x04003CCB RID: 15563
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Sitefe;

			// Token: 0x04003CCC RID: 15564
			public static CallSite<Func<CallSite, object, string, object>> <>p__Siteff;

			// Token: 0x04003CCD RID: 15565
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site100;

			// Token: 0x04003CCE RID: 15566
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site101;

			// Token: 0x04003CCF RID: 15567
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site102;

			// Token: 0x04003CD0 RID: 15568
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site103;
		}

		// Token: 0x02000BE9 RID: 3049
		[CompilerGenerated]
		private static class <NativeCompletionAliasCommands>o__SiteContainer106
		{
			// Token: 0x04003CD1 RID: 15569
			public static CallSite<Func<CallSite, object, object>> <>p__Site107;

			// Token: 0x04003CD2 RID: 15570
			public static CallSite<Func<CallSite, object, bool>> <>p__Site108;

			// Token: 0x04003CD3 RID: 15571
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Site109;

			// Token: 0x04003CD4 RID: 15572
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Site10a;

			// Token: 0x04003CD5 RID: 15573
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site10b;

			// Token: 0x04003CD6 RID: 15574
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site10c;

			// Token: 0x04003CD7 RID: 15575
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site10d;

			// Token: 0x04003CD8 RID: 15576
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site10e;

			// Token: 0x04003CD9 RID: 15577
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site10f;
		}

		// Token: 0x02000BEA RID: 3050
		[CompilerGenerated]
		private static class <NativeCompletionTraceSourceCommands>o__SiteContainer110
		{
			// Token: 0x04003CDA RID: 15578
			public static CallSite<Func<CallSite, object, object>> <>p__Site111;

			// Token: 0x04003CDB RID: 15579
			public static CallSite<Func<CallSite, object, bool>> <>p__Site112;

			// Token: 0x04003CDC RID: 15580
			public static CallSite<Func<CallSite, Type, object, bool, object>> <>p__Site113;

			// Token: 0x04003CDD RID: 15581
			public static CallSite<Func<CallSite, object, string, string, object>> <>p__Site114;

			// Token: 0x04003CDE RID: 15582
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site115;

			// Token: 0x04003CDF RID: 15583
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site116;

			// Token: 0x04003CE0 RID: 15584
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site117;

			// Token: 0x04003CE1 RID: 15585
			public static CallSite<Func<CallSite, string, object, object>> <>p__Site118;

			// Token: 0x04003CE2 RID: 15586
			public static CallSite<Func<CallSite, Type, object, object, CompletionResultType, object, CompletionResult>> <>p__Site119;
		}

		// Token: 0x02000BEB RID: 3051
		[CompilerGenerated]
		private static class <CompleteFilename>o__SiteContainer124
		{
			// Token: 0x04003CE3 RID: 15587
			public static CallSite<Func<CallSite, object, object>> <>p__Site125;

			// Token: 0x04003CE4 RID: 15588
			public static CallSite<Func<CallSite, object, string>> <>p__Site126;

			// Token: 0x04003CE5 RID: 15589
			public static CallSite<Func<CallSite, object, object>> <>p__Site127;

			// Token: 0x04003CE6 RID: 15590
			public static CallSite<Func<CallSite, object, string>> <>p__Site128;

			// Token: 0x04003CE7 RID: 15591
			public static CallSite<Func<CallSite, object, object>> <>p__Site129;

			// Token: 0x04003CE8 RID: 15592
			public static CallSite<Func<CallSite, object, string>> <>p__Site12a;

			// Token: 0x04003CE9 RID: 15593
			public static CallSite<Func<CallSite, object, object>> <>p__Site12b;

			// Token: 0x04003CEA RID: 15594
			public static CallSite<Func<CallSite, Type, object, object>> <>p__Site12c;

			// Token: 0x04003CEB RID: 15595
			public static CallSite<Func<CallSite, object, object>> <>p__Site12d;

			// Token: 0x04003CEC RID: 15596
			public static CallSite<Func<CallSite, object, bool>> <>p__Site12e;

			// Token: 0x04003CED RID: 15597
			public static CallSite<Func<CallSite, bool, object, object>> <>p__Site12f;

			// Token: 0x04003CEE RID: 15598
			public static CallSite<Func<CallSite, object, object>> <>p__Site130;

			// Token: 0x04003CEF RID: 15599
			public static CallSite<Action<CallSite, PowerShell, string, object>> <>p__Site131;

			// Token: 0x04003CF0 RID: 15600
			public static CallSite<Func<CallSite, object, object>> <>p__Site132;

			// Token: 0x04003CF1 RID: 15601
			public static CallSite<Func<CallSite, object, string>> <>p__Site133;

			// Token: 0x04003CF2 RID: 15602
			public static CallSite<Func<CallSite, object, object>> <>p__Site134;

			// Token: 0x04003CF3 RID: 15603
			public static CallSite<Func<CallSite, object, string>> <>p__Site135;

			// Token: 0x04003CF4 RID: 15604
			public static CallSite<Func<CallSite, object, object>> <>p__Site136;

			// Token: 0x04003CF5 RID: 15605
			public static CallSite<Func<CallSite, object, bool>> <>p__Site137;
		}

		// Token: 0x02000BEC RID: 3052
		[CompilerGenerated]
		private static class <CompleteVariable>o__SiteContainer13c
		{
			// Token: 0x04003CF6 RID: 15606
			public static CallSite<Func<CallSite, object, object>> <>p__Site13d;

			// Token: 0x04003CF7 RID: 15607
			public static CallSite<Func<CallSite, Type, object, object>> <>p__Site13e;

			// Token: 0x04003CF8 RID: 15608
			public static CallSite<Func<CallSite, object, object>> <>p__Site13f;
		}
	}
}
