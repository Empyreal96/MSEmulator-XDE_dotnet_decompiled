using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace System.Management.Automation
{
	// Token: 0x020001CE RID: 462
	internal class MUIFileSearcher
	{
		// Token: 0x0600154B RID: 5451 RVA: 0x00084A68 File Offset: 0x00082C68
		private MUIFileSearcher(string target, Collection<string> searchPaths, SearchMode searchMode)
		{
			this._target = target;
			this._searchPaths = searchPaths;
			this._searchMode = searchMode;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00084A9C File Offset: 0x00082C9C
		private MUIFileSearcher(string target, Collection<string> searchPaths) : this(target, searchPaths, SearchMode.Unique)
		{
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00084AA7 File Offset: 0x00082CA7
		internal string Target
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00084AAF File Offset: 0x00082CAF
		internal Collection<string> SearchPaths
		{
			get
			{
				return this._searchPaths;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x00084AB7 File Offset: 0x00082CB7
		internal SearchMode SearchMode
		{
			get
			{
				return this._searchMode;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x00084ABF File Offset: 0x00082CBF
		internal Collection<string> Result
		{
			get
			{
				if (this._result == null)
				{
					this._result = new Collection<string>();
					this.SearchForFiles();
				}
				return this._result;
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00084AE0 File Offset: 0x00082CE0
		private void SearchForFiles()
		{
			if (string.IsNullOrEmpty(this.Target))
			{
				return;
			}
			string fileName = Path.GetFileName(this.Target);
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}
			Collection<string> collection = MUIFileSearcher.NormalizeSearchPaths(this.Target, this.SearchPaths);
			foreach (string directory in collection)
			{
				this.SearchForFiles(fileName, directory);
				if (this.SearchMode == SearchMode.First && this.Result.Count > 0)
				{
					break;
				}
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00084B78 File Offset: 0x00082D78
		private void AddFiles(string muiDirectory, string directory, string pattern)
		{
			if (Directory.Exists(muiDirectory))
			{
				string[] files = Directory.GetFiles(muiDirectory, pattern);
				if (files == null)
				{
					return;
				}
				foreach (string text in files)
				{
					string item = Path.Combine(muiDirectory, text);
					switch (this.SearchMode)
					{
					case SearchMode.First:
						this._result.Add(item);
						return;
					case SearchMode.All:
						this._result.Add(item);
						break;
					case SearchMode.Unique:
					{
						string fileName = Path.GetFileName(text);
						string key = Path.Combine(directory, fileName);
						if (!this._uniqueMatches.Contains(key))
						{
							this._result.Add(item);
							this._uniqueMatches[key] = true;
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00084C40 File Offset: 0x00082E40
		private void SearchForFiles(string pattern, string directory)
		{
			List<string> list = new List<string>();
			CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
			while (cultureInfo != null && !string.IsNullOrEmpty(cultureInfo.Name))
			{
				list.Add(cultureInfo.Name);
				cultureInfo = cultureInfo.Parent;
			}
			list.Add("");
			if (!list.Contains("en-US"))
			{
				list.Add("en-US");
			}
			if (!list.Contains("en"))
			{
				list.Add("en");
			}
			foreach (string path in list)
			{
				string muiDirectory = Path.Combine(directory, path);
				this.AddFiles(muiDirectory, directory, pattern);
				if (this.SearchMode == SearchMode.First && this.Result.Count > 0)
				{
					break;
				}
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00084D20 File Offset: 0x00082F20
		private static Collection<string> NormalizeSearchPaths(string target, Collection<string> searchPaths)
		{
			Collection<string> collection = new Collection<string>();
			if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(Path.GetDirectoryName(target)))
			{
				string directoryName = Path.GetDirectoryName(target);
				if (Directory.Exists(directoryName))
				{
					collection.Add(Path.GetFullPath(directoryName));
				}
				return collection;
			}
			if (searchPaths != null)
			{
				foreach (string text in searchPaths)
				{
					if (!collection.Contains(text) && Directory.Exists(text))
					{
						collection.Add(text);
					}
				}
			}
			string mshDefaultInstallationPath = MUIFileSearcher.GetMshDefaultInstallationPath();
			if (mshDefaultInstallationPath != null && !collection.Contains(mshDefaultInstallationPath) && Directory.Exists(mshDefaultInstallationPath))
			{
				collection.Add(mshDefaultInstallationPath);
			}
			return collection;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00084DDC File Offset: 0x00082FDC
		private static string GetMshDefaultInstallationPath()
		{
			string text = CommandDiscovery.GetShellPathFromRegistry(Utils.DefaultPowerShellShellID);
			if (text != null)
			{
				text = Path.GetDirectoryName(text);
			}
			return text;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00084DFF File Offset: 0x00082FFF
		internal static Collection<string> SearchFiles(string pattern)
		{
			return MUIFileSearcher.SearchFiles(pattern, new Collection<string>());
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00084E0C File Offset: 0x0008300C
		internal static Collection<string> SearchFiles(string pattern, Collection<string> searchPaths)
		{
			MUIFileSearcher muifileSearcher = new MUIFileSearcher(pattern, searchPaths);
			return muifileSearcher.Result;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00084E27 File Offset: 0x00083027
		internal static string LocateFile(string file)
		{
			return MUIFileSearcher.LocateFile(file, new Collection<string>());
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00084E34 File Offset: 0x00083034
		internal static string LocateFile(string file, Collection<string> searchPaths)
		{
			MUIFileSearcher muifileSearcher = new MUIFileSearcher(file, searchPaths, SearchMode.First);
			if (muifileSearcher.Result == null || muifileSearcher.Result.Count == 0)
			{
				return null;
			}
			return muifileSearcher.Result[0];
		}

		// Token: 0x04000907 RID: 2311
		private string _target;

		// Token: 0x04000908 RID: 2312
		private Collection<string> _searchPaths;

		// Token: 0x04000909 RID: 2313
		private SearchMode _searchMode = SearchMode.Unique;

		// Token: 0x0400090A RID: 2314
		private Collection<string> _result;

		// Token: 0x0400090B RID: 2315
		private Hashtable _uniqueMatches = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}
