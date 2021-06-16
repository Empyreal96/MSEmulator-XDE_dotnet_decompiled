using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020000A0 RID: 160
	internal class AnalysisCache
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x00025D20 File Offset: 0x00023F20
		internal static Dictionary<string, List<CommandTypes>> GetExportedCommands(string modulePath, bool testOnly, ExecutionContext context)
		{
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSDisableModuleAutoLoadingMemoryCache")) && AnalysisCache.IsModuleInCacheValid(modulePath))
			{
				lock (AnalysisCache.itemCache)
				{
					return AnalysisCache.itemCache[modulePath].Commands;
				}
			}
			string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(modulePath);
			PSCacheItem pscacheItem = AnalysisCache.Get(basePath, modulePath);
			if (!testOnly && pscacheItem == null)
			{
				try
				{
					if (AnalysisCache.modulesBeingAnalyzed.Contains(modulePath))
					{
						ModuleIntrinsics.Tracer.WriteLine(modulePath + " is already being analyzed. Exiting.", new object[0]);
						return null;
					}
					ModuleIntrinsics.Tracer.WriteLine("Registering " + modulePath + " for analysis.", new object[0]);
					AnalysisCache.modulesBeingAnalyzed.Add(modulePath);
					AnalysisCache.GetPSModuleInfo(context, fileNameWithoutExtension);
				}
				catch (Exception ex)
				{
					ModuleIntrinsics.Tracer.WriteLine("Module analysis generated an exception: " + ex.ToString(), new object[0]);
					CommandProcessorBase.CheckForSevereException(ex);
				}
				finally
				{
					ModuleIntrinsics.Tracer.WriteLine("Unregistering " + modulePath + " for analysis.", new object[0]);
					AnalysisCache.modulesBeingAnalyzed.Remove(modulePath);
				}
				pscacheItem = AnalysisCache.Get(basePath, modulePath);
			}
			if (pscacheItem != null)
			{
				lock (AnalysisCache.itemCache)
				{
					ModuleIntrinsics.Tracer.WriteLine("Caching item exported from module" + fileNameWithoutExtension, new object[0]);
					AnalysisCache.itemCache[modulePath] = pscacheItem;
				}
				if (pscacheItem.Commands != null)
				{
					ModuleIntrinsics.Tracer.WriteLine("Returning " + pscacheItem.Commands.Count + " exported commands.", new object[0]);
				}
				else
				{
					ModuleIntrinsics.Tracer.WriteLine("Returning NULL for exported commands", new object[0]);
				}
				return pscacheItem.Commands;
			}
			ModuleIntrinsics.Tracer.WriteLine("Returning NULL for exported commands", new object[0]);
			return null;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00025F60 File Offset: 0x00024160
		internal static Dictionary<string, HashSet<string>> GetCachedClasses()
		{
			Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
			lock (AnalysisCache.itemCache)
			{
				foreach (KeyValuePair<string, PSCacheItem> keyValuePair in AnalysisCache.itemCache)
				{
					if (keyValuePair.Value.Classes.Count > 0)
					{
						dictionary.Add(keyValuePair.Key, keyValuePair.Value.Classes);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00025FF4 File Offset: 0x000241F4
		internal static HashSet<string> GetExportedClasses(string modulePath, ExecutionContext context)
		{
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSDisableModuleAutoLoadingMemoryCache")) && AnalysisCache.IsModuleInCacheValid(modulePath))
			{
				lock (AnalysisCache.itemCache)
				{
					PSCacheItem pscacheItem = AnalysisCache.itemCache[modulePath];
					if (pscacheItem.Classes.Count > 0)
					{
						return pscacheItem.Classes;
					}
				}
			}
			string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
			PSCacheItem pscacheItem2 = AnalysisCache.Get(basePath, modulePath);
			if (pscacheItem2 == null || !AnalysisCache.modulesWithTypeAnalysis.Contains(modulePath))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(modulePath);
				ModuleIntrinsics.Tracer.WriteLine("Listing modules.", new object[0]);
				Collection<PSModuleInfo> psmoduleInfo = AnalysisCache.GetPSModuleInfo(context, fileNameWithoutExtension);
				foreach (PSModuleInfo module in psmoduleInfo)
				{
					AnalysisCache.CacheExportedPSTypes(module, context);
				}
				pscacheItem2 = AnalysisCache.Get(basePath, modulePath);
			}
			else
			{
				lock (AnalysisCache.itemCache)
				{
					if (!AnalysisCache.itemCache.ContainsKey(modulePath))
					{
						AnalysisCache.itemCache.Add(modulePath, pscacheItem2);
					}
				}
			}
			if (pscacheItem2.Classes.Count > 0)
			{
				return pscacheItem2.Classes;
			}
			return null;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00026168 File Offset: 0x00024368
		private static Mutex GetAnalysisMutex()
		{
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = "Global\\PowerShell_CommandAnalysis_Lock_" + value;
			ModuleIntrinsics.Tracer.WriteLine("Entering mutex " + text, new object[0]);
			Mutex result;
			try
			{
				MutexSecurity mutexSecurity = new MutexSecurity();
				SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				MutexAccessRule rule = new MutexAccessRule(identity, MutexRights.FullControl, AccessControlType.Allow);
				mutexSecurity.AddAccessRule(rule);
				bool flag = false;
				result = new Mutex(false, text, ref flag, mutexSecurity);
			}
			catch (UnauthorizedAccessException ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("UnauthorizedAccessException in creating Mutex " + ex.Message + ". Using Mutex.OpenExisting method to access mutex", new object[0]);
				result = Mutex.OpenExisting(text, MutexRights.FullControl);
			}
			return result;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00026234 File Offset: 0x00024434
		internal static void CacheExportedCommands(PSModuleInfo module, bool force, ExecutionContext context)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
			ModuleIntrinsics.Tracer.WriteLine("Requested caching for " + module.Name + " at " + text, new object[0]);
			if (!force)
			{
				Dictionary<string, List<CommandTypes>> exportedCommands = AnalysisCache.GetExportedCommands(module.Path, true, context);
				if (exportedCommands != null && module.ExportedCommands != null)
				{
					bool flag = false;
					foreach (KeyValuePair<string, CommandInfo> keyValuePair in module.ExportedCommands)
					{
						if (!exportedCommands.ContainsKey(keyValuePair.Key))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						ModuleIntrinsics.Tracer.WriteLine("Existing cached info up-to-date. Skipping.", new object[0]);
					}
					return;
				}
			}
			Mutex mutex = AnalysisCache.GetAnalysisMutex();
			try
			{
				mutex = Utils.SafeWaitMutex(mutex, () => AnalysisCache.GetAnalysisMutex());
				PSCacheItem pscacheItem = null;
				lock (AnalysisCache.itemCache)
				{
					if (!AnalysisCache.itemCache.TryGetValue(module.Path, out pscacheItem))
					{
						pscacheItem = new PSCacheItem();
					}
				}
				if (module.ExportedCommands != null)
				{
					if (pscacheItem.Commands == null)
					{
						pscacheItem.Commands = new Dictionary<string, List<CommandTypes>>(StringComparer.OrdinalIgnoreCase);
					}
					ModuleIntrinsics.Tracer.WriteLine("Caching " + module.ExportedCommands.Count + " commands", new object[0]);
					foreach (CommandInfo commandInfo in module.ExportedCommands.Values)
					{
						List<CommandTypes> list = null;
						if (!pscacheItem.Commands.TryGetValue(commandInfo.Name, out list))
						{
							ModuleIntrinsics.Tracer.WriteLine("Caching " + commandInfo.Name, new object[0]);
							list = new List<CommandTypes>();
							list.Add(commandInfo.CommandType);
						}
						else
						{
							list.Add(commandInfo.CommandType);
						}
						if (!pscacheItem.Commands.ContainsKey(commandInfo.Name))
						{
							pscacheItem.Commands.Add(commandInfo.Name, list);
						}
					}
					AnalysisCache.Cache<PSCacheItem>(text, module.Path, pscacheItem);
					lock (AnalysisCache.itemCache)
					{
						AnalysisCache.itemCache[module.Path] = pscacheItem;
					}
				}
			}
			finally
			{
				ModuleIntrinsics.Tracer.WriteLine("Releasing mutex after caching.", new object[0]);
				mutex.ReleaseMutex();
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002655C File Offset: 0x0002475C
		public static PSCacheItem Get(string basePath, string path)
		{
			AnalysisCacheIndex analysisCacheIndex = null;
			ModuleIntrinsics.Tracer.WriteLine("Getting analysis cache entry for " + path + ".", new object[0]);
			Mutex analysisMutex = AnalysisCache.GetAnalysisMutex();
			try
			{
				Utils.SafeWaitMutex(analysisMutex, () => AnalysisCache.GetAnalysisMutex());
				analysisCacheIndex = AnalysisCache.GetCacheIndex(basePath);
				if (analysisCacheIndex == null)
				{
					ModuleIntrinsics.Tracer.WriteLine("Could not get cache index. Returning.", new object[0]);
					return null;
				}
				AnalysisCacheIndexEntry analysisCacheIndexEntry = null;
				if (!analysisCacheIndex.Entries.TryGetValue(path, out analysisCacheIndexEntry))
				{
					ModuleIntrinsics.Tracer.WriteLine("Returning NULL - not cached.", new object[0]);
					return null;
				}
				ModuleIntrinsics.Tracer.WriteLine("Found cache entry for " + path, new object[0]);
				DateTime lastWriteTime = new FileInfo(path).LastWriteTime;
				if (lastWriteTime == analysisCacheIndexEntry.LastWriteTime)
				{
					ModuleIntrinsics.Tracer.WriteLine("LastWriteTime is current.", new object[0]);
					try
					{
						ModuleIntrinsics.Tracer.WriteLine("Deserializing from " + Path.Combine(basePath, analysisCacheIndexEntry.Path), new object[0]);
						return AnalysisCache.DeserializeFromFile<PSCacheItem>(basePath, analysisCacheIndexEntry.Path);
					}
					catch (Exception ex)
					{
						ModuleIntrinsics.Tracer.WriteLine("Got an exception deserializing: " + ex.ToString(), new object[0]);
						analysisCacheIndex.Entries.Remove(path);
						CommandProcessorBase.CheckForSevereException(ex);
						if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSDisableModuleAutoloadingCacheMaintenance")))
						{
							ModuleIntrinsics.Tracer.WriteLine("Cleaning cache store.", new object[0]);
							AnalysisCache.CleanAnalysisCacheStore(analysisCacheIndex);
						}
						ModuleIntrinsics.Tracer.WriteLine("Returning NULL due to exception.", new object[0]);
						return null;
					}
				}
				ModuleIntrinsics.Tracer.WriteLine("Returning NULL - LastWriteTime does not match.", new object[0]);
			}
			finally
			{
				ModuleIntrinsics.Tracer.WriteLine("Releasing mutex after cache file data.", new object[0]);
				analysisMutex.ReleaseMutex();
			}
			return null;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00026780 File Offset: 0x00024980
		private static void Cache<T>(string basePath, string path, T value)
		{
			bool flag = false;
			ModuleIntrinsics.Tracer.WriteLine("Caching " + value + " to disk.", new object[0]);
			AnalysisCacheIndex analysisCacheIndex = AnalysisCache.GetCacheIndex(basePath);
			if (analysisCacheIndex == null)
			{
				ModuleIntrinsics.Tracer.WriteLine("Could not get cache index. Returning.", new object[0]);
				return;
			}
			ModuleIntrinsics.Tracer.WriteLine("Got cache index.", new object[0]);
			AnalysisCacheIndexEntry analysisCacheIndexEntry;
			if (!analysisCacheIndex.Entries.TryGetValue(path, out analysisCacheIndexEntry))
			{
				analysisCacheIndexEntry = new AnalysisCacheIndexEntry();
				string str = Guid.NewGuid().ToString();
				analysisCacheIndexEntry.Path = "PowerShell_AnalysisCacheEntry_" + str;
				flag = true;
				ModuleIntrinsics.Tracer.WriteLine("Item not already in cache. Caching to " + analysisCacheIndexEntry.Path + ", need to update index.", new object[0]);
			}
			DateTime lastWriteTime = new FileInfo(path).LastWriteTime;
			if (analysisCacheIndexEntry.LastWriteTime != lastWriteTime)
			{
				ModuleIntrinsics.Tracer.WriteLine(string.Concat(new object[]
				{
					"LastWriteTime for ",
					path,
					" + has changed. Old: ",
					analysisCacheIndexEntry.LastWriteTime,
					", new: ",
					lastWriteTime,
					". Need to update index."
				}), new object[0]);
				analysisCacheIndexEntry.LastWriteTime = lastWriteTime;
				flag = true;
			}
			string path2 = analysisCacheIndexEntry.Path;
			ModuleIntrinsics.Tracer.WriteLine("Caching to " + path2, new object[0]);
			if (flag)
			{
				analysisCacheIndex = AnalysisCache.GetCacheIndexFromDisk(basePath);
				analysisCacheIndex.Entries[path] = analysisCacheIndexEntry;
			}
			try
			{
				if (AnalysisCache.savedCacheIndex != null)
				{
					AnalysisCache.savedCacheIndex.Entries[path] = analysisCacheIndexEntry;
				}
				AnalysisCache.SerializeToFile<T>(value, path2);
			}
			catch (IOException ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("Couldn't serialize file due to IOException - " + ex.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
			catch (UnauthorizedAccessException ex2)
			{
				ModuleIntrinsics.Tracer.WriteLine("Couldn't serialize file due to UnauthorizedAccessException - " + ex2.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
			if (flag && !AnalysisCache.disableDiskBasedCache)
			{
				ModuleIntrinsics.Tracer.WriteLine("Serializing index.", new object[0]);
				AnalysisCache.SaveCacheIndex(analysisCacheIndex);
				AnalysisCache.savedCacheIndex = null;
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000269D0 File Offset: 0x00024BD0
		private static AnalysisCacheIndex GetCacheIndex(string basePath)
		{
			AnalysisCacheIndex analysisCacheIndex = null;
			if (AnalysisCache.savedCacheIndex != null)
			{
				ModuleIntrinsics.Tracer.WriteLine("Found in-memory cache entry.", new object[0]);
				if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSDisableModuleAutoLoadingMemoryCache")))
				{
					analysisCacheIndex = AnalysisCache.savedCacheIndex;
				}
			}
			if (analysisCacheIndex == null)
			{
				ModuleIntrinsics.Tracer.WriteLine("No in-memory entry. Getting cache index.", new object[0]);
				analysisCacheIndex = AnalysisCache.GetCacheIndexFromDisk(basePath);
				AnalysisCache.savedCacheIndex = analysisCacheIndex;
			}
			return analysisCacheIndex;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00026A38 File Offset: 0x00024C38
		public static AnalysisCacheIndex GetCacheIndexFromDisk(string basePath)
		{
			AnalysisCacheIndex analysisCacheIndex = null;
			bool flag = false;
			try
			{
				ModuleIntrinsics.Tracer.WriteLine("Deserializing cache index from " + Path.Combine(basePath, "PowerShell_AnalysisCacheIndex"), new object[0]);
				analysisCacheIndex = AnalysisCache.DeserializeFromFile<AnalysisCacheIndex>(basePath, "PowerShell_AnalysisCacheIndex");
			}
			catch (Exception ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("Got an exception deserializing index: " + ex.ToString(), new object[0]);
				CommandProcessorBase.CheckForSevereException(ex);
				flag = true;
			}
			if (analysisCacheIndex == null)
			{
				ModuleIntrinsics.Tracer.WriteLine("Creating new index, couldn't get one from disk.", new object[0]);
				analysisCacheIndex = new AnalysisCacheIndex();
				analysisCacheIndex.LastMaintenance = DateTime.Now;
			}
			if (analysisCacheIndex.Entries == null)
			{
				analysisCacheIndex.Entries = new Dictionary<string, AnalysisCacheIndexEntry>(StringComparer.OrdinalIgnoreCase);
			}
			if (flag || (DateTime.Now - analysisCacheIndex.LastMaintenance).TotalDays > 7.0)
			{
				if (flag)
				{
					ModuleIntrinsics.Tracer.WriteLine("Cleaning analysis store because it was corrupted.", new object[0]);
				}
				else
				{
					ModuleIntrinsics.Tracer.WriteLine("Cleaning analysis store for its 7-day maintenance window. Last maintenance was " + analysisCacheIndex.LastMaintenance, new object[0]);
				}
				if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSDisableModuleAutoloadingCacheMaintenance")))
				{
					AnalysisCache.CleanAnalysisCacheStore(analysisCacheIndex);
				}
			}
			return analysisCacheIndex;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00026B78 File Offset: 0x00024D78
		private static void CleanAnalysisCacheStore(AnalysisCacheIndex cacheIndex)
		{
			try
			{
				ModuleIntrinsics.Tracer.WriteLine("Entering CleanAnalysisCacheStore.", new object[0]);
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
				List<string> list = new List<string>();
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string text2 in cacheIndex.Entries.Keys)
				{
					AnalysisCacheIndexEntry analysisCacheIndexEntry = cacheIndex.Entries[text2];
					string text3 = Path.Combine(text, analysisCacheIndexEntry.Path);
					ModuleIntrinsics.Tracer.WriteLine("Cache index contains " + text3, new object[0]);
					if (!File.Exists(text2))
					{
						ModuleIntrinsics.Tracer.WriteLine("Module + " + text2 + " no longer exists. Deleting its index entry.", new object[0]);
						File.Delete(text3);
						list.Add(text2);
					}
					else
					{
						hashSet.Add(text3);
					}
				}
				foreach (string key in list)
				{
					cacheIndex.Entries.Remove(key);
				}
				ModuleIntrinsics.Tracer.WriteLine("Searching for files with no cache entries.", new object[0]);
				foreach (string text4 in Directory.EnumerateFiles(text, "PowerShell_AnalysisCacheEntry_*"))
				{
					if (!hashSet.Contains(text4))
					{
						ModuleIntrinsics.Tracer.WriteLine("Found stale file: " + text4, new object[0]);
						File.Delete(text4);
					}
				}
				ModuleIntrinsics.Tracer.WriteLine("Saving cache index.", new object[0]);
				cacheIndex.LastMaintenance = DateTime.Now;
				AnalysisCache.SaveCacheIndex(cacheIndex);
			}
			catch (IOException ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("Got an IO exception during cache maintenance: " + ex.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
			catch (UnauthorizedAccessException ex2)
			{
				ModuleIntrinsics.Tracer.WriteLine("Got an UnauthorizedAccessException during cache maintenance: " + ex2.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00026E18 File Offset: 0x00025018
		private static void SaveCacheIndex(AnalysisCacheIndex index)
		{
			try
			{
				ModuleIntrinsics.Tracer.WriteLine("Serializing index to PowerShell_AnalysisCacheIndex", new object[0]);
				AnalysisCache.savedCacheIndex = index;
				AnalysisCache.SerializeToFile<AnalysisCacheIndex>(index, "PowerShell_AnalysisCacheIndex");
			}
			catch (IOException ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("Got an IO exception saving cache index: " + ex.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
			catch (UnauthorizedAccessException ex2)
			{
				ModuleIntrinsics.Tracer.WriteLine("Got an unauthorized access exception saving cache index: " + ex2.ToString(), new object[0]);
				AnalysisCache.disableDiskBasedCache = true;
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00026EBC File Offset: 0x000250BC
		private static T DeserializeFromFile<T>(string basePath, string path)
		{
			T result = default(T);
			if (AnalysisCache.disableDiskBasedCache)
			{
				ModuleIntrinsics.Tracer.WriteLine("Skipping deserialization from file: " + Path.Combine(basePath, path) + " - disk-based caching is diabled.", new object[0]);
				return result;
			}
			ModuleIntrinsics.Tracer.WriteLine("Deserializing " + typeof(T).FullName + " from file: " + Path.Combine(basePath, path), new object[0]);
			if (Directory.Exists(basePath))
			{
				string path2 = Path.Combine(basePath, path);
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
				using (FileStream fileStream = new FileStream(path2, FileMode.Open))
				{
					using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(fileStream, XmlDictionaryReaderQuotas.Max))
					{
						result = (T)((object)dataContractSerializer.ReadObject(xmlDictionaryReader));
					}
				}
			}
			ModuleIntrinsics.Tracer.WriteLine("Deserializing complete.", new object[0]);
			return result;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00026FC4 File Offset: 0x000251C4
		private static void SerializeToFile<T>(T value, string path)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
			string text2 = Path.Combine(text, path);
			if (AnalysisCache.disableDiskBasedCache)
			{
				ModuleIntrinsics.Tracer.WriteLine(string.Concat(new string[]
				{
					"Skipping serialization of ",
					value.ToString(),
					" to file: ",
					text2,
					" - disk-based caching is diabled."
				}), new object[0]);
				return;
			}
			ModuleIntrinsics.Tracer.WriteLine("Serializing " + value.ToString() + " to file: " + text2, new object[0]);
			if (!Directory.Exists(text))
			{
				ModuleIntrinsics.Tracer.WriteLine("Root directory does not exist. Creating.", new object[0]);
				Directory.CreateDirectory(text);
			}
			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
			using (FileStream fileStream = new FileStream(text2, FileMode.OpenOrCreate))
			{
				fileStream.SetLength(0L);
				using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(fileStream))
				{
					dataContractSerializer.WriteObject(xmlDictionaryWriter, value);
					xmlDictionaryWriter.Flush();
				}
			}
			ModuleIntrinsics.Tracer.WriteLine("Serializing complete.", new object[0]);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002711C File Offset: 0x0002531C
		private static void CacheClassesFromModule(string cacheStoreLocation, PSModuleInfo module)
		{
			PSCacheItem pscacheItem = null;
			lock (AnalysisCache.itemCache)
			{
				if (!AnalysisCache.itemCache.TryGetValue(module.Path, out pscacheItem))
				{
					pscacheItem = new PSCacheItem();
				}
			}
			ReadOnlyDictionary<string, TypeDefinitionAst> exportedTypeDefinitions = module.GetExportedTypeDefinitions();
			foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in exportedTypeDefinitions)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.IsClass)
				{
					if (pscacheItem.Classes.Count == 0)
					{
						pscacheItem.Classes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					}
					pscacheItem.Classes.Add(keyValuePair.Value.Name);
				}
			}
			if (pscacheItem.Classes != null && pscacheItem.Classes.Count > 0)
			{
				lock (AnalysisCache.itemCache)
				{
					AnalysisCache.itemCache[module.Path] = pscacheItem;
				}
				AnalysisCache.Cache<PSCacheItem>(cacheStoreLocation, module.Path, pscacheItem);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002725C File Offset: 0x0002545C
		private static Collection<PSModuleInfo> GetPSModuleInfo(ExecutionContext context, string moduleShortName)
		{
			CommandInfo commandInfo = new CmdletInfo("Get-Module", typeof(GetModuleCommand), null, null, context);
			Command command = new Command(commandInfo);
			try
			{
				PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("List", true).AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false);
				if (moduleShortName != null)
				{
					powerShell.AddParameter("Name", moduleShortName);
				}
				return powerShell.Invoke<PSModuleInfo>();
			}
			catch (Exception ex)
			{
				ModuleIntrinsics.Tracer.WriteLine("Module analysis generated an exception: " + ex.ToString(), new object[0]);
				CommandProcessorBase.CheckForSevereException(ex);
			}
			return null;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00027354 File Offset: 0x00025554
		private static void CacheExportedPSTypes(PSModuleInfo module, ExecutionContext context)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\");
			ModuleIntrinsics.Tracer.WriteLine("Requested caching for " + module.Name + " at " + text, new object[0]);
			Mutex mutex = AnalysisCache.GetAnalysisMutex();
			try
			{
				mutex = Utils.SafeWaitMutex(mutex, () => AnalysisCache.GetAnalysisMutex());
				AnalysisCache.CacheClassesFromModule(text, module);
				AnalysisCache.modulesWithTypeAnalysis.Add(module.Path);
			}
			finally
			{
				ModuleIntrinsics.Tracer.WriteLine("Releasing mutex after caching.", new object[0]);
				mutex.ReleaseMutex();
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002740C File Offset: 0x0002560C
		private static bool IsModuleInCacheValid(string modulePath)
		{
			AnalysisCacheIndexEntry analysisCacheIndexEntry = null;
			PSCacheItem pscacheItem = null;
			lock (AnalysisCache.itemCache)
			{
				AnalysisCache.itemCache.TryGetValue(modulePath, out pscacheItem);
			}
			if (pscacheItem == null || AnalysisCache.savedCacheIndex == null || !AnalysisCache.savedCacheIndex.Entries.TryGetValue(modulePath, out analysisCacheIndexEntry) || pscacheItem == null)
			{
				return false;
			}
			DateTime lastWriteTime = new FileInfo(modulePath).LastWriteTime;
			if (lastWriteTime == analysisCacheIndexEntry.LastWriteTime && pscacheItem != null)
			{
				return true;
			}
			ModuleIntrinsics.Tracer.WriteLine(string.Concat(new object[]
			{
				"Cache entry for ",
				modulePath,
				" was out of date. Cached on ",
				analysisCacheIndexEntry.LastWriteTime,
				", last updated on ",
				lastWriteTime,
				". Re-analyzing."
			}), new object[0]);
			lock (AnalysisCache.itemCache)
			{
				AnalysisCache.itemCache.Remove(modulePath);
			}
			return false;
		}

		// Token: 0x04000382 RID: 898
		private const string CommandAnalysisFolder = "Microsoft\\Windows\\PowerShell\\CommandAnalysis\\";

		// Token: 0x04000383 RID: 899
		private const string DataFileBase = "PowerShell_AnalysisCacheEntry_";

		// Token: 0x04000384 RID: 900
		private const string IndexFile = "PowerShell_AnalysisCacheIndex";

		// Token: 0x04000385 RID: 901
		private static Dictionary<string, PSCacheItem> itemCache = new Dictionary<string, PSCacheItem>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000386 RID: 902
		private static AnalysisCacheIndex savedCacheIndex = null;

		// Token: 0x04000387 RID: 903
		private static HashSet<string> modulesBeingAnalyzed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000388 RID: 904
		private static HashSet<string> modulesWithTypeAnalysis = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000389 RID: 905
		private static bool disableDiskBasedCache = false;
	}
}
