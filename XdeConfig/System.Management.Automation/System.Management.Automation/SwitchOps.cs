using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x02000634 RID: 1588
	internal static class SwitchOps
	{
		// Token: 0x060044D4 RID: 17620 RVA: 0x00170E18 File Offset: 0x0016F018
		internal static bool ConditionSatisfiedWildcard(bool caseSensitive, object condition, string str, ExecutionContext context)
		{
			WildcardPattern wildcardPattern = condition as WildcardPattern;
			if (wildcardPattern != null)
			{
				if ((wildcardPattern.Options & WildcardOptions.IgnoreCase) == WildcardOptions.None != caseSensitive)
				{
					WildcardOptions options = caseSensitive ? WildcardOptions.None : WildcardOptions.IgnoreCase;
					wildcardPattern = new WildcardPattern(wildcardPattern.Pattern, options);
				}
			}
			else
			{
				WildcardOptions options2 = caseSensitive ? WildcardOptions.None : WildcardOptions.IgnoreCase;
				wildcardPattern = new WildcardPattern(PSObject.ToStringParser(context, condition), options2);
			}
			return wildcardPattern.IsMatch(str);
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x00170E74 File Offset: 0x0016F074
		internal static bool ConditionSatisfiedRegex(bool caseSensitive, object condition, IScriptExtent errorPosition, string str, ExecutionContext context)
		{
			RegexOptions options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
			bool success;
			try
			{
				Regex regex = condition as Regex;
				Match match;
				if (regex != null && (regex.Options & RegexOptions.IgnoreCase) != RegexOptions.None != caseSensitive)
				{
					match = regex.Match(str);
				}
				else
				{
					string text = PSObject.ToStringParser(context, condition);
					match = Regex.Match(str, text, options);
					if (match.Success && match.Groups.Count > 0)
					{
						regex = new Regex(text, options);
					}
				}
				if (match.Success)
				{
					GroupCollection groups = match.Groups;
					if (groups.Count > 0)
					{
						Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
						foreach (string text2 in regex.GetGroupNames())
						{
							Group group = groups[text2];
							if (group.Success)
							{
								int num;
								if (int.TryParse(text2, out num))
								{
									hashtable.Add(num, group.ToString());
								}
								else
								{
									hashtable.Add(text2, group.ToString());
								}
							}
						}
						context.SetVariable(SpecialVariables.MatchesVarPath, hashtable);
					}
				}
				success = match.Success;
			}
			catch (ArgumentException innerException)
			{
				string text = PSObject.ToStringParser(context, condition);
				throw InterpreterError.NewInterpreterExceptionWithInnerException(text, typeof(RuntimeException), errorPosition, "InvalidRegularExpression", ParserStrings.InvalidRegularExpression, innerException, new object[]
				{
					text
				});
			}
			return success;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x00170FE0 File Offset: 0x0016F1E0
		internal static string ResolveFilePath(IScriptExtent errorExtent, object obj, ExecutionContext context)
		{
			string result;
			try
			{
				FileInfo fileInfo = obj as FileInfo;
				string text = (fileInfo != null) ? fileInfo.FullName : PSObject.ToStringParser(context, obj);
				if (string.IsNullOrEmpty(text))
				{
					throw InterpreterError.NewInterpreterException(text, typeof(RuntimeException), errorExtent, "InvalidFilenameOption", ParserStrings.InvalidFilenameOption, new object[0]);
				}
				SessionState sessionState = new SessionState(context.EngineSessionState);
				ProviderInfo providerInfo;
				Collection<string> resolvedProviderPathFromPSPath = sessionState.Path.GetResolvedProviderPathFromPSPath(text, out providerInfo);
				if (!providerInfo.NameEquals(context.ProviderNames.FileSystem))
				{
					throw InterpreterError.NewInterpreterException(text, typeof(RuntimeException), errorExtent, "FileOpenError", ParserStrings.FileOpenError, new object[]
					{
						providerInfo.FullName
					});
				}
				if (resolvedProviderPathFromPSPath == null || resolvedProviderPathFromPSPath.Count < 1)
				{
					throw InterpreterError.NewInterpreterException(text, typeof(RuntimeException), errorExtent, "FileNotFound", ParserStrings.FileNotFound, new object[]
					{
						text
					});
				}
				if (resolvedProviderPathFromPSPath.Count > 1)
				{
					throw InterpreterError.NewInterpreterException(resolvedProviderPathFromPSPath, typeof(RuntimeException), errorExtent, "AmbiguousPath", ParserStrings.AmbiguousPath, new object[0]);
				}
				result = resolvedProviderPathFromPSPath[0];
			}
			catch (RuntimeException ex)
			{
				if (ex.ErrorRecord != null && ex.ErrorRecord.InvocationInfo == null)
				{
					ex.ErrorRecord.SetInvocationInfo(new InvocationInfo(null, errorExtent, context));
				}
				throw;
			}
			return result;
		}
	}
}
