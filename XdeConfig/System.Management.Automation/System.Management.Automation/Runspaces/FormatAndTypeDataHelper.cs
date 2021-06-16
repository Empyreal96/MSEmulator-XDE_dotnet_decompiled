using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Text;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200040D RID: 1037
	internal static class FormatAndTypeDataHelper
	{
		// Token: 0x06002E8A RID: 11914 RVA: 0x000FFC7C File Offset: 0x000FDE7C
		private static string GetBaseFolder(RunspaceConfiguration runspaceConfiguration, Collection<string> independentErrors)
		{
			string text = CommandDiscovery.GetShellPathFromRegistry(runspaceConfiguration.ShellId);
			if (text == null)
			{
				text = Path.GetDirectoryName(PsUtils.GetMainModule(Process.GetCurrentProcess()).FileName);
			}
			else
			{
				text = Path.GetDirectoryName(text);
				if (!Directory.Exists(text))
				{
					string directoryName = Path.GetDirectoryName(ClrFacade.GetAssemblyLocation(typeof(FormatAndTypeDataHelper).GetTypeInfo().Assembly));
					string item = StringUtil.Format(TypesXmlStrings.CannotFindRegistryKeyPath, new object[]
					{
						text,
						Utils.GetRegistryConfigurationPath(runspaceConfiguration.ShellId),
						"\\Path",
						directoryName
					});
					independentErrors.Add(item);
					text = directoryName;
				}
			}
			return text;
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000FFD18 File Offset: 0x000FDF18
		internal static Collection<PSSnapInTypeAndFormatErrors> GetFormatAndTypesErrors(RunspaceConfiguration runspaceConfiguration, PSHost host, IEnumerable configurationEntryCollection, RunspaceConfigurationCategory category, Collection<string> independentErrors, Collection<int> entryIndicesToRemove)
		{
			Collection<PSSnapInTypeAndFormatErrors> collection = new Collection<PSSnapInTypeAndFormatErrors>();
			string baseFolder = FormatAndTypeDataHelper.GetBaseFolder(runspaceConfiguration, independentErrors);
			HashSet<string> fullFileNameSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			int num = -1;
			foreach (object obj in configurationEntryCollection)
			{
				num++;
				string fileName;
				string psSnapinName;
				if (category == RunspaceConfigurationCategory.Types)
				{
					TypeConfigurationEntry typeConfigurationEntry = (TypeConfigurationEntry)obj;
					fileName = typeConfigurationEntry.FileName;
					psSnapinName = ((typeConfigurationEntry.PSSnapIn == null) ? runspaceConfiguration.ShellId : typeConfigurationEntry.PSSnapIn.Name);
					if (fileName == null)
					{
						collection.Add(new PSSnapInTypeAndFormatErrors(psSnapinName, typeConfigurationEntry.TypeData, typeConfigurationEntry.IsRemove));
						continue;
					}
				}
				else
				{
					FormatConfigurationEntry formatConfigurationEntry = (FormatConfigurationEntry)obj;
					fileName = formatConfigurationEntry.FileName;
					psSnapinName = ((formatConfigurationEntry.PSSnapIn == null) ? runspaceConfiguration.ShellId : formatConfigurationEntry.PSSnapIn.Name);
					if (fileName == null)
					{
						collection.Add(new PSSnapInTypeAndFormatErrors(psSnapinName, formatConfigurationEntry.FormatData));
						continue;
					}
				}
				bool flag = false;
				string andCheckFullFileName = FormatAndTypeDataHelper.GetAndCheckFullFileName(psSnapinName, fullFileNameSet, baseFolder, fileName, independentErrors, ref flag);
				if (andCheckFullFileName == null)
				{
					if (flag)
					{
						entryIndicesToRemove.Add(num);
					}
				}
				else
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(psSnapinName, andCheckFullFileName));
				}
			}
			return collection;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000FFE68 File Offset: 0x000FE068
		private static string GetAndCheckFullFileName(string psSnapinName, HashSet<string> fullFileNameSet, string baseFolder, string baseFileName, Collection<string> independentErrors, ref bool needToRemoveEntry)
		{
			string text = Path.IsPathRooted(baseFileName) ? baseFileName : Path.Combine(baseFolder, baseFileName);
			if (!File.Exists(text))
			{
				string item = StringUtil.Format(TypesXmlStrings.FileNotFound, psSnapinName, text);
				independentErrors.Add(item);
				return null;
			}
			if (fullFileNameSet.Contains(text))
			{
				needToRemoveEntry = true;
				return null;
			}
			if (!text.EndsWith(".ps1xml", StringComparison.OrdinalIgnoreCase))
			{
				string item2 = StringUtil.Format(TypesXmlStrings.EntryShouldBeMshXml, psSnapinName, text);
				independentErrors.Add(item2);
				return null;
			}
			fullFileNameSet.Add(text);
			return text;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000FFEE4 File Offset: 0x000FE0E4
		internal static void ThrowExceptionOnError(string errorId, Collection<string> independentErrors, Collection<PSSnapInTypeAndFormatErrors> PSSnapinFilesCollection, RunspaceConfigurationCategory category)
		{
			Collection<string> collection = new Collection<string>();
			if (independentErrors != null)
			{
				foreach (string item in independentErrors)
				{
					collection.Add(item);
				}
			}
			foreach (PSSnapInTypeAndFormatErrors pssnapInTypeAndFormatErrors in PSSnapinFilesCollection)
			{
				foreach (string item2 in pssnapInTypeAndFormatErrors.Errors)
				{
					collection.Add(item2);
				}
			}
			if (collection.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('\n');
			foreach (string value in collection)
			{
				stringBuilder.Append(value);
				stringBuilder.Append('\n');
			}
			string message = "";
			if (category == RunspaceConfigurationCategory.Types)
			{
				message = StringUtil.Format(ExtendedTypeSystem.TypesXmlError, stringBuilder.ToString());
			}
			else if (category == RunspaceConfigurationCategory.Formats)
			{
				message = StringUtil.Format(FormatAndOutXmlLoadingStrings.FormatLoadingErrors, stringBuilder.ToString());
			}
			RuntimeException ex = new RuntimeException(message);
			ex.SetErrorId(errorId);
			throw ex;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x00100060 File Offset: 0x000FE260
		internal static void ThrowExceptionOnError(string errorId, Collection<string> errors, RunspaceConfigurationCategory category)
		{
			if (errors.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('\n');
			foreach (string value in errors)
			{
				stringBuilder.Append(value);
				stringBuilder.Append('\n');
			}
			string message = "";
			if (category == RunspaceConfigurationCategory.Types)
			{
				message = StringUtil.Format(ExtendedTypeSystem.TypesXmlError, stringBuilder.ToString());
			}
			else if (category == RunspaceConfigurationCategory.Formats)
			{
				message = StringUtil.Format(FormatAndOutXmlLoadingStrings.FormatLoadingErrors, stringBuilder.ToString());
			}
			RuntimeException ex = new RuntimeException(message);
			ex.SetErrorId(errorId);
			throw ex;
		}

		// Token: 0x04001878 RID: 6264
		private const string FileNotFound = "FileNotFound";

		// Token: 0x04001879 RID: 6265
		private const string CannotFindRegistryKey = "CannotFindRegistryKey";

		// Token: 0x0400187A RID: 6266
		private const string CannotFindRegistryKeyPath = "CannotFindRegistryKeyPath";

		// Token: 0x0400187B RID: 6267
		private const string EntryShouldBeMshXml = "EntryShouldBeMshXml";

		// Token: 0x0400187C RID: 6268
		private const string DuplicateFile = "DuplicateFile";

		// Token: 0x0400187D RID: 6269
		internal const string ValidationException = "ValidationException";
	}
}
