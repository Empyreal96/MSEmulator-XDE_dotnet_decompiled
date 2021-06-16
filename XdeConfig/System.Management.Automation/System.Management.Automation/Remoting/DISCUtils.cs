using System;
using System.Collections;
using System.IO;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000370 RID: 880
	internal static class DISCUtils
	{
		// Token: 0x06002B3F RID: 11071 RVA: 0x000EE560 File Offset: 0x000EC760
		internal static ExternalScriptInfo GetScriptInfoForFile(ExecutionContext context, string fileName, out string scriptName)
		{
			scriptName = Path.GetFileName(fileName);
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(scriptName, fileName, context);
			if (!scriptName.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
			{
				context.AuthorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Internal, context.EngineHostInterface);
				CommandDiscovery.VerifyPSVersion(externalScriptInfo);
				externalScriptInfo.SignatureChecked = true;
			}
			return externalScriptInfo;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000EE5B0 File Offset: 0x000EC7B0
		internal static Hashtable LoadConfigFile(ExecutionContext context, ExternalScriptInfo scriptInfo)
		{
			object variableValue = context.GetVariableValue(SpecialVariables.PSScriptRootVarPath);
			object variableValue2 = context.GetVariableValue(SpecialVariables.PSCommandPathVarPath);
			object obj;
			try
			{
				context.SetVariable(SpecialVariables.PSScriptRootVarPath, Path.GetDirectoryName(scriptInfo.Definition));
				context.SetVariable(SpecialVariables.PSCommandPathVarPath, scriptInfo.Definition);
				obj = PSObject.Base(scriptInfo.ScriptBlock.InvokeReturnAsIs(new object[0]));
			}
			finally
			{
				context.SetVariable(SpecialVariables.PSScriptRootVarPath, variableValue);
				context.SetVariable(SpecialVariables.PSCommandPathVarPath, variableValue2);
			}
			return obj as Hashtable;
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000EE644 File Offset: 0x000EC844
		internal static bool VerifyConfigTable(Hashtable table, PSCmdlet cmdlet, string path)
		{
			bool flag = false;
			foreach (object obj in table)
			{
				DictionaryEntry de = (DictionaryEntry)obj;
				if (!ConfigFileConstants.IsValidKey(de, cmdlet, path))
				{
					return false;
				}
				if (de.Key.ToString().Equals(ConfigFileConstants.SchemaVersion, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCMissingSchemaVersion, path));
				return false;
			}
			try
			{
				DISCUtils.ValidateAbsolutePaths(cmdlet.SessionState, table, path);
				DISCUtils.ValidateExtensions(table, path);
			}
			catch (InvalidOperationException ex)
			{
				cmdlet.WriteVerbose(ex.Message);
				return false;
			}
			return true;
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000EE710 File Offset: 0x000EC910
		private static void ValidatePS1XMLExtension(string key, string[] paths, string filePath)
		{
			if (paths == null)
			{
				return;
			}
			foreach (string path in paths)
			{
				try
				{
					string extension = Path.GetExtension(path);
					if (!extension.Equals(".ps1xml", StringComparison.OrdinalIgnoreCase))
					{
						throw new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.DISCInvalidExtension, new object[]
						{
							key,
							extension,
							".ps1xml"
						}));
					}
				}
				catch (ArgumentException innerException)
				{
					throw new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ErrorParsingTheKeyInPSSessionConfigurationFile, key, filePath), innerException);
				}
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000EE7A4 File Offset: 0x000EC9A4
		private static void ValidatePS1OrPSM1Extension(string key, string[] paths, string filePath)
		{
			if (paths == null)
			{
				return;
			}
			foreach (string path in paths)
			{
				try
				{
					string extension = Path.GetExtension(path);
					if (!extension.Equals(".ps1", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".psm1", StringComparison.OrdinalIgnoreCase))
					{
						throw new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.DISCInvalidExtension, new object[]
						{
							key,
							extension,
							string.Join(", ", new string[]
							{
								".ps1",
								".psm1"
							})
						}));
					}
				}
				catch (ArgumentException innerException)
				{
					throw new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ErrorParsingTheKeyInPSSessionConfigurationFile, key, filePath), innerException);
				}
			}
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000EE86C File Offset: 0x000ECA6C
		internal static void ValidateExtensions(Hashtable table, string filePath)
		{
			if (table.ContainsKey(ConfigFileConstants.TypesToProcess))
			{
				DISCUtils.ValidatePS1XMLExtension(ConfigFileConstants.TypesToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.TypesToProcess]), filePath);
			}
			if (table.ContainsKey(ConfigFileConstants.FormatsToProcess))
			{
				DISCUtils.ValidatePS1XMLExtension(ConfigFileConstants.FormatsToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.FormatsToProcess]), filePath);
			}
			if (table.ContainsKey(ConfigFileConstants.ScriptsToProcess))
			{
				DISCUtils.ValidatePS1OrPSM1Extension(ConfigFileConstants.ScriptsToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.ScriptsToProcess]), filePath);
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000EE8F4 File Offset: 0x000ECAF4
		internal static void ValidateAbsolutePaths(SessionState state, Hashtable table, string filePath)
		{
			if (table.ContainsKey(ConfigFileConstants.TypesToProcess))
			{
				DISCUtils.ValidateAbsolutePath(state, ConfigFileConstants.TypesToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.TypesToProcess]), filePath);
			}
			if (table.ContainsKey(ConfigFileConstants.FormatsToProcess))
			{
				DISCUtils.ValidateAbsolutePath(state, ConfigFileConstants.FormatsToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.FormatsToProcess]), filePath);
			}
			if (table.ContainsKey(ConfigFileConstants.ScriptsToProcess))
			{
				DISCUtils.ValidateAbsolutePath(state, ConfigFileConstants.ScriptsToProcess, DISCPowerShellConfiguration.TryGetStringArray(table[ConfigFileConstants.ScriptsToProcess]), filePath);
			}
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000EE97C File Offset: 0x000ECB7C
		internal static void ValidateAbsolutePath(SessionState state, string key, string[] paths, string filePath)
		{
			if (paths == null)
			{
				return;
			}
			foreach (string text in paths)
			{
				string text2;
				if (!state.Path.IsPSAbsolute(text, out text2))
				{
					throw new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.DISCPathsMustBeAbsolute, new object[]
					{
						key,
						text,
						filePath
					}));
				}
			}
		}

		// Token: 0x040015C2 RID: 5570
		internal static Type ExecutionPolicyType;
	}
}
