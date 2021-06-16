using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200036F RID: 879
	internal static class ConfigFileConstants
	{
		// Token: 0x06002B33 RID: 11059 RVA: 0x000ED938 File Offset: 0x000EBB38
		internal static bool IsValidKey(DictionaryEntry de, PSCmdlet cmdlet, string path)
		{
			bool flag = false;
			foreach (ConfigTypeEntry configTypeEntry in ConfigFileConstants.ConfigFileKeys)
			{
				if (string.Equals(configTypeEntry.Key, de.Key.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					if (configTypeEntry.ValidationCallback(de.Key.ToString(), de.Value, cmdlet, path))
					{
						return true;
					}
				}
			}
			if (!flag)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCInvalidKey, de.Key.ToString(), path));
			}
			return false;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000ED9CC File Offset: 0x000EBBCC
		private static bool ISSValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			string value = obj as string;
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					Enum.Parse(typeof(SessionType), value, true);
					return true;
				}
				catch (ArgumentException)
				{
				}
			}
			cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeValidEnum, new object[]
			{
				key,
				typeof(SessionType).FullName,
				LanguagePrimitives.EnumSingleTypeConverter.EnumValues(typeof(SessionType)),
				path
			}));
			return false;
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000EDA58 File Offset: 0x000EBC58
		private static bool LanugageModeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			string value = obj as string;
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					Enum.Parse(typeof(PSLanguageMode), value, true);
					return true;
				}
				catch (ArgumentException)
				{
				}
			}
			cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeValidEnum, new object[]
			{
				key,
				typeof(PSLanguageMode).FullName,
				LanguagePrimitives.EnumSingleTypeConverter.EnumValues(typeof(PSLanguageMode)),
				path
			}));
			return false;
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000EDAE4 File Offset: 0x000EBCE4
		private static bool ExecutionPolicyValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			string value = obj as string;
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					Enum.Parse(DISCUtils.ExecutionPolicyType, value, true);
					return true;
				}
				catch (ArgumentException)
				{
				}
			}
			cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeValidEnum, new object[]
			{
				key,
				DISCUtils.ExecutionPolicyType.FullName,
				LanguagePrimitives.EnumSingleTypeConverter.EnumValues(DISCUtils.ExecutionPolicyType),
				path
			}));
			return false;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000EDB64 File Offset: 0x000EBD64
		private static bool HashtableTypeValiationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			if (!(obj is Hashtable))
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtable, key, path));
				return false;
			}
			return true;
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000EDB90 File Offset: 0x000EBD90
		private static bool AliasDefinitionsTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			Hashtable[] array = DISCPowerShellConfiguration.TryGetHashtableArray(obj);
			if (array == null)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, key, path));
				return false;
			}
			Hashtable[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				Hashtable hashtable = array2[i];
				bool result;
				if (!hashtable.ContainsKey(ConfigFileConstants.AliasNameToken))
				{
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.AliasNameToken,
						path
					}));
					result = false;
				}
				else
				{
					if (hashtable.ContainsKey(ConfigFileConstants.AliasValueToken))
					{
						foreach (object obj2 in hashtable.Keys)
						{
							string text = (string)obj2;
							if (!string.Equals(text, ConfigFileConstants.AliasNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.AliasValueToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.AliasDescriptionToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.AliasOptionsToken, StringComparison.OrdinalIgnoreCase))
							{
								cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
								{
									text,
									key,
									path
								}));
								return false;
							}
						}
						i++;
						continue;
					}
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.AliasValueToken,
						path
					}));
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000EDD14 File Offset: 0x000EBF14
		private static bool FunctionDefinitionsTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			Hashtable[] array = DISCPowerShellConfiguration.TryGetHashtableArray(obj);
			if (array == null)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, key, path));
				return false;
			}
			Hashtable[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				Hashtable hashtable = array2[i];
				bool result;
				if (!hashtable.ContainsKey(ConfigFileConstants.FunctionNameToken))
				{
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.FunctionNameToken,
						path
					}));
					result = false;
				}
				else if (!hashtable.ContainsKey(ConfigFileConstants.FunctionValueToken))
				{
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.FunctionValueToken,
						path
					}));
					result = false;
				}
				else
				{
					if (hashtable[ConfigFileConstants.FunctionValueToken] is ScriptBlock)
					{
						foreach (object obj2 in hashtable.Keys)
						{
							string text = (string)obj2;
							if (!string.Equals(text, ConfigFileConstants.FunctionNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionValueToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionOptionsToken, StringComparison.OrdinalIgnoreCase))
							{
								cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
								{
									text,
									key,
									path
								}));
								return false;
							}
						}
						i++;
						continue;
					}
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCKeyMustBeScriptBlock, new object[]
					{
						ConfigFileConstants.FunctionValueToken,
						key,
						path
					}));
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000EDED0 File Offset: 0x000EC0D0
		private static bool VariableDefinitionsTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			Hashtable[] array = DISCPowerShellConfiguration.TryGetHashtableArray(obj);
			if (array == null)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, key, path));
				return false;
			}
			Hashtable[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				Hashtable hashtable = array2[i];
				bool result;
				if (!hashtable.ContainsKey(ConfigFileConstants.VariableNameToken))
				{
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.VariableNameToken,
						path
					}));
					result = false;
				}
				else
				{
					if (hashtable.ContainsKey(ConfigFileConstants.VariableValueToken))
					{
						foreach (object obj2 in hashtable.Keys)
						{
							string text = (string)obj2;
							if (!string.Equals(text, ConfigFileConstants.VariableNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.VariableValueToken, StringComparison.OrdinalIgnoreCase))
							{
								cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
								{
									text,
									key,
									path
								}));
								return false;
							}
						}
						i++;
						continue;
					}
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
					{
						key,
						ConfigFileConstants.VariableValueToken,
						path
					}));
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000EE038 File Offset: 0x000EC238
		private static bool StringTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			if (!(obj is string))
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeString, key, path));
				return false;
			}
			return true;
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000EE057 File Offset: 0x000EC257
		private static bool StringArrayTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			if (DISCPowerShellConfiguration.TryGetStringArray(obj) == null)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringArray, key, path));
				return false;
			}
			return true;
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000EE078 File Offset: 0x000EC278
		private static bool StringOrHashtableArrayTypeValidationCallback(string key, object obj, PSCmdlet cmdlet, string path)
		{
			if (DISCPowerShellConfiguration.TryGetObjectsOfType<object>(obj, new Type[]
			{
				typeof(string),
				typeof(Hashtable)
			}) == null)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringOrHashtableArrayInFile, key, path));
				return false;
			}
			return true;
		}

		// Token: 0x0400159A RID: 5530
		internal static readonly string AliasDefinitions = "AliasDefinitions";

		// Token: 0x0400159B RID: 5531
		internal static readonly string AliasDescriptionToken = "Description";

		// Token: 0x0400159C RID: 5532
		internal static readonly string AliasNameToken = "Name";

		// Token: 0x0400159D RID: 5533
		internal static readonly string AliasOptionsToken = "Options";

		// Token: 0x0400159E RID: 5534
		internal static readonly string AliasValueToken = "Value";

		// Token: 0x0400159F RID: 5535
		internal static readonly string AssembliesToLoad = "AssembliesToLoad";

		// Token: 0x040015A0 RID: 5536
		internal static readonly string Author = "Author";

		// Token: 0x040015A1 RID: 5537
		internal static readonly string CompanyName = "CompanyName";

		// Token: 0x040015A2 RID: 5538
		internal static readonly string Copyright = "Copyright";

		// Token: 0x040015A3 RID: 5539
		internal static readonly string Description = "Description";

		// Token: 0x040015A4 RID: 5540
		internal static readonly string EnvironmentVariables = "EnvironmentVariables";

		// Token: 0x040015A5 RID: 5541
		internal static readonly string ExecutionPolicy = "ExecutionPolicy";

		// Token: 0x040015A6 RID: 5542
		internal static readonly string FormatsToProcess = "FormatsToProcess";

		// Token: 0x040015A7 RID: 5543
		internal static readonly string FunctionDefinitions = "FunctionDefinitions";

		// Token: 0x040015A8 RID: 5544
		internal static readonly string FunctionNameToken = "Name";

		// Token: 0x040015A9 RID: 5545
		internal static readonly string FunctionOptionsToken = "Options";

		// Token: 0x040015AA RID: 5546
		internal static readonly string FunctionValueToken = "ScriptBlock";

		// Token: 0x040015AB RID: 5547
		internal static readonly string Guid = "GUID";

		// Token: 0x040015AC RID: 5548
		internal static readonly string LanguageMode = "LanguageMode";

		// Token: 0x040015AD RID: 5549
		internal static readonly string ModulesToImport = "ModulesToImport";

		// Token: 0x040015AE RID: 5550
		internal static readonly string PowerShellVersion = "PowerShellVersion";

		// Token: 0x040015AF RID: 5551
		internal static readonly string RoleDefinitions = "RoleDefinitions";

		// Token: 0x040015B0 RID: 5552
		internal static readonly string SchemaVersion = "SchemaVersion";

		// Token: 0x040015B1 RID: 5553
		internal static readonly string ScriptsToProcess = "ScriptsToProcess";

		// Token: 0x040015B2 RID: 5554
		internal static readonly string SessionType = "SessionType";

		// Token: 0x040015B3 RID: 5555
		internal static readonly string RoleCapabilities = "RoleCapabilities";

		// Token: 0x040015B4 RID: 5556
		internal static readonly string RunAsVirtualAccount = "RunAsVirtualAccount";

		// Token: 0x040015B5 RID: 5557
		internal static readonly string RunAsVirtualAccountGroups = "RunAsVirtualAccountGroups";

		// Token: 0x040015B6 RID: 5558
		internal static readonly string TranscriptDirectory = "TranscriptDirectory";

		// Token: 0x040015B7 RID: 5559
		internal static readonly string TypesToProcess = "TypesToProcess";

		// Token: 0x040015B8 RID: 5560
		internal static readonly string VariableDefinitions = "VariableDefinitions";

		// Token: 0x040015B9 RID: 5561
		internal static readonly string VariableNameToken = "Name";

		// Token: 0x040015BA RID: 5562
		internal static readonly string VariableValueToken = "Value";

		// Token: 0x040015BB RID: 5563
		internal static readonly string VisibleAliases = "VisibleAliases";

		// Token: 0x040015BC RID: 5564
		internal static readonly string VisibleCmdlets = "VisibleCmdlets";

		// Token: 0x040015BD RID: 5565
		internal static readonly string VisibleFunctions = "VisibleFunctions";

		// Token: 0x040015BE RID: 5566
		internal static readonly string VisibleProviders = "VisibleProviders";

		// Token: 0x040015BF RID: 5567
		internal static readonly string VisibleExternalCommands = "VisibleExternalCommands";

		// Token: 0x040015C0 RID: 5568
		internal static HashSet<string> DisallowedRoleCapabilityKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			ConfigFileConstants.PowerShellVersion,
			ConfigFileConstants.SessionType,
			ConfigFileConstants.RoleDefinitions,
			ConfigFileConstants.LanguageMode,
			ConfigFileConstants.ExecutionPolicy
		};

		// Token: 0x040015C1 RID: 5569
		internal static ConfigTypeEntry[] ConfigFileKeys = new ConfigTypeEntry[]
		{
			new ConfigTypeEntry(ConfigFileConstants.AliasDefinitions, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.AliasDefinitionsTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.AssembliesToLoad, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.Author, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.CompanyName, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.Copyright, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.Description, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.EnvironmentVariables, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.HashtableTypeValiationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.ExecutionPolicy, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.ExecutionPolicyValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.FormatsToProcess, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.FunctionDefinitions, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.FunctionDefinitionsTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.Guid, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.LanguageMode, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.LanugageModeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.ModulesToImport, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringOrHashtableArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.PowerShellVersion, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.RoleDefinitions, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.HashtableTypeValiationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.SchemaVersion, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.ScriptsToProcess, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.SessionType, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.ISSValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.RoleCapabilities, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.TranscriptDirectory, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.TypesToProcess, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VariableDefinitions, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.VariableDefinitionsTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VisibleAliases, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VisibleCmdlets, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VisibleFunctions, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VisibleProviders, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback)),
			new ConfigTypeEntry(ConfigFileConstants.VisibleExternalCommands, new ConfigTypeEntry.TypeValidationCallback(ConfigFileConstants.StringArrayTypeValidationCallback))
		};
	}
}
