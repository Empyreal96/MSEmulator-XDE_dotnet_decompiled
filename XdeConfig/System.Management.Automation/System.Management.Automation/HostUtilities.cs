using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200024F RID: 591
	public static class HostUtilities
	{
		// Token: 0x06001C24 RID: 7204 RVA: 0x000A3C84 File Offset: 0x000A1E84
		internal static PSObject GetDollarProfile(string allUsersAllHosts, string allUsersCurrentHost, string currentUserAllHosts, string currentUserCurrentHost)
		{
			return new PSObject(currentUserCurrentHost)
			{
				Properties = 
				{
					new PSNoteProperty("AllUsersAllHosts", allUsersAllHosts),
					new PSNoteProperty("AllUsersCurrentHost", allUsersCurrentHost),
					new PSNoteProperty("CurrentUserAllHosts", currentUserAllHosts),
					new PSNoteProperty("CurrentUserCurrentHost", currentUserCurrentHost)
				}
			};
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000A3CF1 File Offset: 0x000A1EF1
		internal static PSCommand[] GetProfileCommands(string shellId)
		{
			return HostUtilities.GetProfileCommands(shellId, false);
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000A3CFA File Offset: 0x000A1EFA
		internal static void GetProfileObjectData(string shellId, bool useTestProfile, out string allUsersAllHosts, out string allUsersCurrentHost, out string currentUserAllHosts, out string currentUserCurrentHost, out PSObject dollarProfile)
		{
			allUsersAllHosts = HostUtilities.GetFullProfileFileName(null, false, useTestProfile);
			allUsersCurrentHost = HostUtilities.GetFullProfileFileName(shellId, false, useTestProfile);
			currentUserAllHosts = HostUtilities.GetFullProfileFileName(null, true, useTestProfile);
			currentUserCurrentHost = HostUtilities.GetFullProfileFileName(shellId, true, useTestProfile);
			dollarProfile = HostUtilities.GetDollarProfile(allUsersAllHosts, allUsersCurrentHost, currentUserAllHosts, currentUserCurrentHost);
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000A3D38 File Offset: 0x000A1F38
		internal static PSCommand[] GetProfileCommands(string shellId, bool useTestProfile)
		{
			List<PSCommand> list = new List<PSCommand>();
			string text;
			string text2;
			string text3;
			string text4;
			PSObject value;
			HostUtilities.GetProfileObjectData(shellId, useTestProfile, out text, out text2, out text3, out text4, out value);
			PSCommand pscommand = new PSCommand();
			pscommand.AddCommand("set-variable");
			pscommand.AddParameter("Name", "profile");
			pscommand.AddParameter("Value", value);
			pscommand.AddParameter("Option", ScopedItemOptions.None);
			list.Add(pscommand);
			string[] array = new string[]
			{
				text,
				text2,
				text3,
				text4
			};
			foreach (string text5 in array)
			{
				if (File.Exists(text5))
				{
					pscommand = new PSCommand();
					pscommand.AddCommand(text5, false);
					list.Add(pscommand);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000A3E14 File Offset: 0x000A2014
		internal static string GetFullProfileFileName(string shellId, bool forCurrentUser)
		{
			return HostUtilities.GetFullProfileFileName(shellId, forCurrentUser, false);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000A3E20 File Offset: 0x000A2020
		internal static string GetFullProfileFileName(string shellId, bool forCurrentUser, bool useTestProfile)
		{
			string text;
			if (forCurrentUser)
			{
				text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				text = Path.Combine(text, Utils.ProductNameForDirectory);
			}
			else
			{
				text = HostUtilities.GetAllUsersFolderPath(shellId);
				if (string.IsNullOrEmpty(text))
				{
					return "";
				}
			}
			string text2 = useTestProfile ? "profile_test.ps1" : "profile.ps1";
			if (!string.IsNullOrEmpty(shellId))
			{
				text2 = shellId + "_" + text2;
			}
			return Path.Combine(text, text2);
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000A3E8C File Offset: 0x000A208C
		private static string GetAllUsersFolderPath(string shellId)
		{
			string result = string.Empty;
			try
			{
				result = Utils.GetApplicationBase(shellId);
			}
			catch (SecurityException)
			{
			}
			return result;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000A3EBC File Offset: 0x000A20BC
		internal static string GetMaxLines(string source, int maxLines)
		{
			if (string.IsNullOrEmpty(source))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int num = 1;
			while (i < source.Length)
			{
				char c = source[i];
				if (c == '\n')
				{
					num++;
				}
				stringBuilder.Append(c);
				if (num == maxLines)
				{
					stringBuilder.Append("...");
					break;
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000A3F24 File Offset: 0x000A2124
		internal static ArrayList GetSuggestion(Runspace runspace)
		{
			LocalRunspace localRunspace = runspace as LocalRunspace;
			if (localRunspace == null)
			{
				return new ArrayList();
			}
			bool questionMarkVariableValue = localRunspace.ExecutionContext.QuestionMarkVariableValue;
			History history = localRunspace.History;
			HistoryInfo[] entries = history.GetEntries(-1L, 1L, true);
			if (entries.Length == 0)
			{
				return new ArrayList();
			}
			HistoryInfo historyInfo = entries[0];
			ArrayList arrayList = (ArrayList)localRunspace.GetExecutionContext.DollarErrorVariable;
			object obj = null;
			if (arrayList.Count > 0)
			{
				obj = (arrayList[0] as Exception);
				ErrorRecord errorRecord = null;
				if (obj == null)
				{
					errorRecord = (arrayList[0] as ErrorRecord);
				}
				else if (obj is RuntimeException)
				{
					errorRecord = ((RuntimeException)obj).ErrorRecord;
				}
				if (errorRecord != null && errorRecord.InvocationInfo != null)
				{
					if (errorRecord.InvocationInfo.HistoryId == historyInfo.Id)
					{
						obj = errorRecord;
					}
					else
					{
						obj = null;
					}
				}
			}
			Runspace defaultRunspace = null;
			bool flag = false;
			if (Runspace.DefaultRunspace != runspace)
			{
				defaultRunspace = Runspace.DefaultRunspace;
				flag = true;
				Runspace.DefaultRunspace = runspace;
			}
			ArrayList result = null;
			try
			{
				result = HostUtilities.GetSuggestion(historyInfo, obj, arrayList);
			}
			finally
			{
				if (flag)
				{
					Runspace.DefaultRunspace = defaultRunspace;
				}
			}
			localRunspace.ExecutionContext.QuestionMarkVariableValue = questionMarkVariableValue;
			return result;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000A4058 File Offset: 0x000A2258
		internal static ArrayList GetSuggestion(HistoryInfo lastHistory, object lastError, ArrayList errorList)
		{
			ArrayList arrayList = new ArrayList();
			PSModuleInfo psmoduleInfo = new PSModuleInfo(true);
			psmoduleInfo.SessionState.PSVariable.Set("lastHistory", lastHistory);
			psmoduleInfo.SessionState.PSVariable.Set("lastError", lastError);
			int num = 0;
			foreach (object obj in HostUtilities.suggestions)
			{
				Hashtable hashtable = (Hashtable)obj;
				num = errorList.Count;
				if (LanguagePrimitives.IsTrue(hashtable["Enabled"]))
				{
					SuggestionMatchType suggestionMatchType = (SuggestionMatchType)LanguagePrimitives.ConvertTo(hashtable["MatchType"], typeof(SuggestionMatchType), CultureInfo.InvariantCulture);
					if (suggestionMatchType == SuggestionMatchType.Dynamic)
					{
						object obj2 = null;
						ScriptBlock scriptBlock = hashtable["Rule"] as ScriptBlock;
						if (scriptBlock == null)
						{
							hashtable["Enabled"] = false;
							throw new ArgumentException(SuggestionStrings.RuleMustBeScriptBlock, "Rule");
						}
						try
						{
							obj2 = psmoduleInfo.Invoke(scriptBlock, null);
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
							hashtable["Enabled"] = false;
							continue;
						}
						if (LanguagePrimitives.IsTrue(obj2))
						{
							string suggestionText = HostUtilities.GetSuggestionText(hashtable["Suggestion"], (object[])hashtable["SuggestionArgs"], psmoduleInfo);
							if (!string.IsNullOrEmpty(suggestionText))
							{
								string value = string.Format(CultureInfo.CurrentCulture, "Suggestion [{0},{1}]: {2}", new object[]
								{
									(int)hashtable["Id"],
									(string)hashtable["Category"],
									suggestionText
								});
								arrayList.Add(value);
							}
						}
					}
					else
					{
						string input = string.Empty;
						if (suggestionMatchType == SuggestionMatchType.Command)
						{
							input = lastHistory.CommandLine;
						}
						else
						{
							if (suggestionMatchType != SuggestionMatchType.Error)
							{
								hashtable["Enabled"] = false;
								throw new ArgumentException(SuggestionStrings.InvalidMatchType, "MatchType");
							}
							if (lastError != null)
							{
								Exception ex = lastError as Exception;
								if (ex != null)
								{
									input = ex.Message;
								}
								else
								{
									input = lastError.ToString();
								}
							}
						}
						if (Regex.IsMatch(input, (string)hashtable["Rule"], RegexOptions.IgnoreCase))
						{
							string suggestionText2 = HostUtilities.GetSuggestionText(hashtable["Suggestion"], (object[])hashtable["SuggestionArgs"], psmoduleInfo);
							if (!string.IsNullOrEmpty(suggestionText2))
							{
								string value2 = string.Format(CultureInfo.CurrentCulture, "Suggestion [{0},{1}]: {2}", new object[]
								{
									(int)hashtable["Id"],
									(string)hashtable["Category"],
									suggestionText2
								});
								arrayList.Add(value2);
							}
						}
					}
					if (errorList.Count != num)
					{
						hashtable["Enabled"] = false;
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000A4378 File Offset: 0x000A2578
		internal static string RemoveGuidFromMessage(string message, out bool matchPattern)
		{
			matchPattern = false;
			if (string.IsNullOrEmpty(message))
			{
				return message;
			}
			Match match = Regex.Match(message, "^([\\d\\w]{8}\\-[\\d\\w]{4}\\-[\\d\\w]{4}\\-[\\d\\w]{4}\\-[\\d\\w]{12}:).*");
			if (match.Success)
			{
				string value = match.Groups[1].Captures[0].Value;
				message = message.Remove(0, value.Length);
				matchPattern = true;
			}
			return message;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000A43D8 File Offset: 0x000A25D8
		internal static string RemoveIdentifierInfoFromMessage(string message, out bool matchPattern)
		{
			matchPattern = false;
			if (string.IsNullOrEmpty(message))
			{
				return message;
			}
			Match match = Regex.Match(message, "^([\\d\\w]{8}\\-[\\d\\w]{4}\\-[\\d\\w]{4}\\-[\\d\\w]{4}\\-[\\d\\w]{12}:\\[.*\\]:).*");
			if (match.Success)
			{
				string value = match.Groups[1].Captures[0].Value;
				message = message.Remove(0, value.Length);
				matchPattern = true;
			}
			return message;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000A4438 File Offset: 0x000A2638
		private static Hashtable NewSuggestion(int id, string category, SuggestionMatchType matchType, string rule, string suggestion, bool enabled)
		{
			Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			hashtable["Id"] = id;
			hashtable["Category"] = category;
			hashtable["MatchType"] = matchType;
			hashtable["Rule"] = rule;
			hashtable["Suggestion"] = suggestion;
			hashtable["Enabled"] = enabled;
			return hashtable;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000A44AC File Offset: 0x000A26AC
		private static Hashtable NewSuggestion(int id, string category, SuggestionMatchType matchType, ScriptBlock rule, ScriptBlock suggestion, bool enabled)
		{
			Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			hashtable["Id"] = id;
			hashtable["Category"] = category;
			hashtable["MatchType"] = matchType;
			hashtable["Rule"] = rule;
			hashtable["Suggestion"] = suggestion;
			hashtable["Enabled"] = enabled;
			return hashtable;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000A4520 File Offset: 0x000A2720
		private static Hashtable NewSuggestion(int id, string category, SuggestionMatchType matchType, ScriptBlock rule, ScriptBlock suggestion, object[] suggestionArgs, bool enabled)
		{
			Hashtable hashtable = HostUtilities.NewSuggestion(id, category, matchType, rule, suggestion, enabled);
			hashtable.Add("SuggestionArgs", suggestionArgs);
			return hashtable;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000A4549 File Offset: 0x000A2749
		private static string GetSuggestionText(object suggestion, PSModuleInfo invocationModule)
		{
			return HostUtilities.GetSuggestionText(suggestion, null, invocationModule);
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000A4554 File Offset: 0x000A2754
		private static string GetSuggestionText(object suggestion, object[] suggestionArgs, PSModuleInfo invocationModule)
		{
			if (suggestion is ScriptBlock)
			{
				ScriptBlock sb = (ScriptBlock)suggestion;
				object valueToConvert = null;
				try
				{
					valueToConvert = invocationModule.Invoke(sb, suggestionArgs);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					return string.Empty;
				}
				return (string)LanguagePrimitives.ConvertTo(valueToConvert, typeof(string), CultureInfo.CurrentCulture);
			}
			return (string)LanguagePrimitives.ConvertTo(suggestion, typeof(string), CultureInfo.CurrentCulture);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000A45D4 File Offset: 0x000A27D4
		internal static PSCredential CredUIPromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options, IntPtr parentHWND)
		{
			if (string.IsNullOrEmpty(caption))
			{
				caption = CredUI.PromptForCredential_DefaultCaption;
			}
			if (string.IsNullOrEmpty(message))
			{
				message = CredUI.PromptForCredential_DefaultMessage;
			}
			if (caption.Length > 128)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, CredUI.PromptForCredential_InvalidCaption, new object[]
				{
					128
				}));
			}
			if (message.Length > 1024)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, CredUI.PromptForCredential_InvalidMessage, new object[]
				{
					1024
				}));
			}
			if (userName != null && userName.Length > 513)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, CredUI.PromptForCredential_InvalidUserName, new object[]
				{
					513
				}));
			}
			HostUtilities.CREDUI_INFO credui_INFO = default(HostUtilities.CREDUI_INFO);
			credui_INFO.pszCaptionText = caption;
			credui_INFO.pszMessageText = message;
			StringBuilder stringBuilder = new StringBuilder(userName, 513);
			StringBuilder stringBuilder2 = new StringBuilder(256);
			bool value = false;
			int num = Convert.ToInt32(value);
			credui_INFO.cbSize = Marshal.SizeOf(credui_INFO);
			credui_INFO.hwndParent = parentHWND;
			HostUtilities.CREDUI_FLAGS credui_FLAGS = HostUtilities.CREDUI_FLAGS.DO_NOT_PERSIST;
			if ((allowedCredentialTypes & PSCredentialTypes.Domain) != PSCredentialTypes.Domain)
			{
				credui_FLAGS |= HostUtilities.CREDUI_FLAGS.GENERIC_CREDENTIALS;
				if ((options & PSCredentialUIOptions.AlwaysPrompt) == PSCredentialUIOptions.AlwaysPrompt)
				{
					credui_FLAGS |= HostUtilities.CREDUI_FLAGS.ALWAYS_SHOW_UI;
				}
			}
			HostUtilities.CredUIReturnCodes credUIReturnCodes = HostUtilities.CredUIReturnCodes.ERROR_INVALID_PARAMETER;
			if (stringBuilder.Length <= 513 && stringBuilder2.Length <= 256)
			{
				credUIReturnCodes = HostUtilities.CredUIPromptForCredentials(ref credui_INFO, targetName, IntPtr.Zero, 0, stringBuilder, 513, stringBuilder2, 256, ref num, credui_FLAGS);
			}
			PSCredential result;
			if (credUIReturnCodes == HostUtilities.CredUIReturnCodes.NO_ERROR)
			{
				string text = null;
				if (stringBuilder != null)
				{
					text = stringBuilder.ToString();
				}
				text = text.TrimStart(new char[]
				{
					'\\'
				});
				SecureString secureString = new SecureString();
				for (int i = 0; i < stringBuilder2.Length; i++)
				{
					secureString.AppendChar(stringBuilder2[i]);
					stringBuilder2[i] = '\0';
				}
				if (!string.IsNullOrEmpty(text))
				{
					result = new PSCredential(text, secureString);
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001C36 RID: 7222
		[DllImport("credui", CharSet = CharSet.Unicode, EntryPoint = "CredUIPromptForCredentialsW")]
		private static extern HostUtilities.CredUIReturnCodes CredUIPromptForCredentials(ref HostUtilities.CREDUI_INFO pUiInfo, string pszTargetName, IntPtr Reserved, int dwAuthError, StringBuilder pszUserName, int ulUserNameMaxChars, StringBuilder pszPassword, int ulPasswordMaxChars, ref int pfSave, HostUtilities.CREDUI_FLAGS dwFlags);

		// Token: 0x06001C37 RID: 7223 RVA: 0x000A47E8 File Offset: 0x000A29E8
		internal static string GetRemotePrompt(RemoteRunspace runspace, string basePrompt)
		{
			if (runspace.ConnectionInfo is NamedPipeConnectionInfo || runspace.ConnectionInfo is VMConnectionInfo || runspace.ConnectionInfo is ContainerConnectionInfo)
			{
				return basePrompt;
			}
			return string.Format(CultureInfo.InvariantCulture, "[{0}]: {1}", new object[]
			{
				runspace.ConnectionInfo.ComputerName,
				basePrompt
			});
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000A4848 File Offset: 0x000A2A48
		internal static bool IsProcessInteractive(InvocationInfo invocationInfo)
		{
			if (invocationInfo.CommandOrigin != CommandOrigin.Runspace)
			{
				return false;
			}
			if (Process.GetCurrentProcess().MainWindowHandle == IntPtr.Zero)
			{
				return false;
			}
			try
			{
				Process currentProcess = Process.GetCurrentProcess();
				TimeSpan t = DateTime.Now - currentProcess.StartTime;
				if ((t - currentProcess.TotalProcessorTime).TotalSeconds > 2.0)
				{
					return true;
				}
			}
			catch (Win32Exception)
			{
				return false;
			}
			return false;
		}

		// Token: 0x04000B83 RID: 2947
		public const string PSEditFunction = "\r\n            param (\r\n                [Parameter(Mandatory=$true)] [String[]] $FileName\r\n            )\r\n\r\n            foreach ($file in $FileName)\r\n            {\r\n                dir $file -File | foreach {\r\n                    $filePathName = $_.FullName\r\n\r\n                    # Get file contents\r\n                    $contentBytes = Get-Content -Path $filePathName -Raw -Encoding Byte\r\n\r\n                    # Notify client for file open.\r\n                    New-Event -SourceIdentifier PSISERemoteSessionOpenFile -EventArguments @($filePathName, $contentBytes) > $null\r\n                }\r\n            }\r\n        ";

		// Token: 0x04000B84 RID: 2948
		public const string CreatePSEditFunction = "\r\n            param (\r\n                [string] $PSEditFunction\r\n            )\r\n\r\n            if ($PSVersionTable.PSVersion -lt ([version] '3.0'))\r\n            {\r\n                throw (new-object System.NotSupportedException)\r\n            }\r\n\r\n            Register-EngineEvent -SourceIdentifier PSISERemoteSessionOpenFile -Forward\r\n\r\n            if ((Test-Path -Path 'function:\\global:PSEdit') -eq $false)\r\n            {\r\n                Set-Item -Path 'function:\\global:PSEdit' -Value $PSEditFunction\r\n            }\r\n        ";

		// Token: 0x04000B85 RID: 2949
		public const string RemovePSEditFunction = "\r\n            if ($PSVersionTable.PSVersion -lt ([version] '3.0'))\r\n            {\r\n                throw (new-object System.NotSupportedException)\r\n            }\r\n\r\n            if ((Test-Path -Path 'function:\\global:PSEdit') -eq $true)\r\n            {\r\n                Remove-Item -Path 'function:\\global:PSEdit' -Force\r\n            }\r\n\r\n            Get-EventSubscriber -SourceIdentifier PSISERemoteSessionOpenFile -EA Ignore | Remove-Event\r\n        ";

		// Token: 0x04000B86 RID: 2950
		public const string RemoteSessionOpenFileEvent = "PSISERemoteSessionOpenFile";

		// Token: 0x04000B87 RID: 2951
		private static string checkForCommandInCurrentDirectoryScript = "\r\n            [System.Diagnostics.DebuggerHidden()]\r\n            param()\r\n\r\n            $foundSuggestion = $false\r\n        \r\n            if($lastError -and\r\n                ($lastError.Exception -is \"System.Management.Automation.CommandNotFoundException\"))\r\n            {\r\n                $escapedCommand = [System.Management.Automation.WildcardPattern]::Escape($lastError.TargetObject)\r\n                $foundSuggestion = @(Get-Command ($ExecutionContext.SessionState.Path.Combine(\".\", $escapedCommand)) -ErrorAction Ignore).Count -gt 0\r\n            }\r\n\r\n            $foundSuggestion\r\n        ";

		// Token: 0x04000B88 RID: 2952
		private static string createCommandExistsInCurrentDirectoryScript = "\r\n            [System.Diagnostics.DebuggerHidden()]\r\n            param([string] $formatString)\r\n\r\n            $formatString -f $lastError.TargetObject,\".\\$($lastError.TargetObject)\"\r\n        ";

		// Token: 0x04000B89 RID: 2953
		private static ArrayList suggestions = new ArrayList(new Hashtable[]
		{
			HostUtilities.NewSuggestion(1, "Transactions", SuggestionMatchType.Command, "^Start-Transaction", SuggestionStrings.Suggestion_StartTransaction, true),
			HostUtilities.NewSuggestion(2, "Transactions", SuggestionMatchType.Command, "^Use-Transaction", SuggestionStrings.Suggestion_UseTransaction, true),
			HostUtilities.NewSuggestion(3, "General", SuggestionMatchType.Dynamic, ScriptBlock.Create(HostUtilities.checkForCommandInCurrentDirectoryScript), ScriptBlock.Create(HostUtilities.createCommandExistsInCurrentDirectoryScript), new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(SuggestionStrings.Suggestion_CommandExistsInCurrentDirectory)
			}, true)
		});

		// Token: 0x02000250 RID: 592
		[Flags]
		private enum CREDUI_FLAGS
		{
			// Token: 0x04000B8B RID: 2955
			INCORRECT_PASSWORD = 1,
			// Token: 0x04000B8C RID: 2956
			DO_NOT_PERSIST = 2,
			// Token: 0x04000B8D RID: 2957
			REQUEST_ADMINISTRATOR = 4,
			// Token: 0x04000B8E RID: 2958
			EXCLUDE_CERTIFICATES = 8,
			// Token: 0x04000B8F RID: 2959
			REQUIRE_CERTIFICATE = 16,
			// Token: 0x04000B90 RID: 2960
			SHOW_SAVE_CHECK_BOX = 64,
			// Token: 0x04000B91 RID: 2961
			ALWAYS_SHOW_UI = 128,
			// Token: 0x04000B92 RID: 2962
			REQUIRE_SMARTCARD = 256,
			// Token: 0x04000B93 RID: 2963
			PASSWORD_ONLY_OK = 512,
			// Token: 0x04000B94 RID: 2964
			VALIDATE_USERNAME = 1024,
			// Token: 0x04000B95 RID: 2965
			COMPLETE_USERNAME = 2048,
			// Token: 0x04000B96 RID: 2966
			PERSIST = 4096,
			// Token: 0x04000B97 RID: 2967
			SERVER_CREDENTIAL = 16384,
			// Token: 0x04000B98 RID: 2968
			EXPECT_CONFIRMATION = 131072,
			// Token: 0x04000B99 RID: 2969
			GENERIC_CREDENTIALS = 262144,
			// Token: 0x04000B9A RID: 2970
			USERNAME_TARGET_CREDENTIALS = 524288,
			// Token: 0x04000B9B RID: 2971
			KEEP_USERNAME = 1048576
		}

		// Token: 0x02000251 RID: 593
		private struct CREDUI_INFO
		{
			// Token: 0x04000B9C RID: 2972
			public int cbSize;

			// Token: 0x04000B9D RID: 2973
			public IntPtr hwndParent;

			// Token: 0x04000B9E RID: 2974
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszMessageText;

			// Token: 0x04000B9F RID: 2975
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszCaptionText;

			// Token: 0x04000BA0 RID: 2976
			public IntPtr hbmBanner;
		}

		// Token: 0x02000252 RID: 594
		private enum CredUIReturnCodes
		{
			// Token: 0x04000BA2 RID: 2978
			NO_ERROR,
			// Token: 0x04000BA3 RID: 2979
			ERROR_CANCELLED = 1223,
			// Token: 0x04000BA4 RID: 2980
			ERROR_NO_SUCH_LOGON_SESSION = 1312,
			// Token: 0x04000BA5 RID: 2981
			ERROR_NOT_FOUND = 1168,
			// Token: 0x04000BA6 RID: 2982
			ERROR_INVALID_ACCOUNT_NAME = 1315,
			// Token: 0x04000BA7 RID: 2983
			ERROR_INSUFFICIENT_BUFFER = 122,
			// Token: 0x04000BA8 RID: 2984
			ERROR_INVALID_PARAMETER = 87,
			// Token: 0x04000BA9 RID: 2985
			ERROR_INVALID_FLAGS = 1004
		}
	}
}
