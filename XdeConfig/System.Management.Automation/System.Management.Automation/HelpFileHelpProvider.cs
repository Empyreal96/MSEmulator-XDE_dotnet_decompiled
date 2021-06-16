using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;

namespace System.Management.Automation
{
	// Token: 0x020001BD RID: 445
	internal class HelpFileHelpProvider : HelpProviderWithCache
	{
		// Token: 0x060014AC RID: 5292 RVA: 0x000804E4 File Offset: 0x0007E6E4
		internal HelpFileHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x000804F8 File Offset: 0x0007E6F8
		internal override string Name
		{
			get
			{
				return "HelpFile Help Provider";
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x000804FF File Offset: 0x0007E6FF
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.HelpFile;
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000807DC File Offset: 0x0007E9DC
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			int countHelpInfosFound = 0;
			string helpFileName = helpRequest.Target + ".help.txt";
			Collection<string> filesMatched = MUIFileSearcher.SearchFiles(helpFileName, this.GetExtendedSearchPaths());
			foreach (string file in filesMatched)
			{
				if (!this._helpFiles.ContainsKey(file))
				{
					try
					{
						this.LoadHelpFile(file);
					}
					catch (IOException exception)
					{
						base.ReportHelpFileError(exception, helpRequest.Target, file);
					}
					catch (SecurityException exception2)
					{
						base.ReportHelpFileError(exception2, helpRequest.Target, file);
					}
				}
				HelpInfo helpInfo = base.GetCache(file);
				if (helpInfo != null)
				{
					countHelpInfosFound++;
					yield return helpInfo;
					if (countHelpInfosFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
					{
						yield break;
					}
				}
			}
			yield break;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00080BAC File Offset: 0x0007EDAC
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			string target = helpRequest.Target;
			string pattern = target;
			int countOfHelpInfoObjectsFound = 0;
			WildcardPattern wildCardPattern = null;
			if (!searchOnlyContent && !WildcardPattern.ContainsWildcardCharacters(target))
			{
				pattern = "*" + pattern + "*";
			}
			if (searchOnlyContent)
			{
				string text = helpRequest.Target;
				if (!WildcardPattern.ContainsWildcardCharacters(helpRequest.Target))
				{
					text = "*" + text + "*";
				}
				wildCardPattern = new WildcardPattern(text, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
				pattern = "*";
			}
			pattern += ".help.txt";
			Collection<string> files = MUIFileSearcher.SearchFiles(pattern, this.GetExtendedSearchPaths());
			if (files != null)
			{
				foreach (string file in files)
				{
					if (!this._helpFiles.ContainsKey(file))
					{
						try
						{
							this.LoadHelpFile(file);
						}
						catch (IOException exception)
						{
							base.ReportHelpFileError(exception, helpRequest.Target, file);
						}
						catch (SecurityException exception2)
						{
							base.ReportHelpFileError(exception2, helpRequest.Target, file);
						}
					}
					HelpFileHelpInfo helpInfo = base.GetCache(file) as HelpFileHelpInfo;
					if (helpInfo != null && (!searchOnlyContent || helpInfo.MatchPatternInContent(wildCardPattern)))
					{
						countOfHelpInfoObjectsFound++;
						yield return helpInfo;
						if (countOfHelpInfoObjectsFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
						{
							yield break;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00080BD8 File Offset: 0x0007EDD8
		private HelpInfo LoadHelpFile(string path)
		{
			string fileName = Path.GetFileName(path);
			if (!path.EndsWith(".help.txt", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string text = fileName.Substring(0, fileName.Length - 9);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			HelpInfo helpInfo = base.GetCache(path);
			if (helpInfo != null)
			{
				return helpInfo;
			}
			string text2 = null;
			using (TextReader textReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				text2 = textReader.ReadToEnd();
			}
			this._helpFiles[path] = 0;
			helpInfo = HelpFileHelpInfo.GetHelpInfo(text, text2, path);
			base.AddCache(path, helpInfo);
			return helpInfo;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00080C80 File Offset: 0x0007EE80
		internal Collection<string> GetExtendedSearchPaths()
		{
			Collection<string> searchPaths = base.GetSearchPaths();
			string defaultShellSearchPath = base.GetDefaultShellSearchPath();
			int num = searchPaths.IndexOf(defaultShellSearchPath);
			if (num != 0)
			{
				if (num > 0)
				{
					searchPaths.RemoveAt(num);
				}
				searchPaths.Insert(0, defaultShellSearchPath);
			}
			foreach (string path in ModuleIntrinsics.GetModulePath(false, base.HelpSystem.ExecutionContext))
			{
				if (Directory.Exists(path))
				{
					try
					{
						string[] directories = Directory.GetDirectories(path);
						foreach (string text in directories)
						{
							if (Directory.EnumerateFiles(text).Any<string>() && !searchPaths.Contains(text))
							{
								searchPaths.Add(text);
							}
						}
					}
					catch (ArgumentException)
					{
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (SecurityException)
					{
					}
				}
			}
			return searchPaths;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00080D90 File Offset: 0x0007EF90
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x040008E7 RID: 2279
		private Hashtable _helpFiles = new Hashtable();
	}
}
