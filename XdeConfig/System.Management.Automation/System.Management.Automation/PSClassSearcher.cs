using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200009D RID: 157
	internal class PSClassSearcher : IEnumerable<PSClassInfo>, IEnumerable, IEnumerator<PSClassInfo>, IDisposable, IEnumerator
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x000255BC File Offset: 0x000237BC
		internal PSClassSearcher(string className, bool useWildCards, ExecutionContext context)
		{
			this._context = context;
			this._className = className;
			this._useWildCards = useWildCards;
			this.moduleInfoCache = new Dictionary<string, PSModuleInfo>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000255F4 File Offset: 0x000237F4
		public void Reset()
		{
			this._currentMatch = null;
			this.matchingClass = null;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00025604 File Offset: 0x00023804
		public void Dispose()
		{
			this.Reset();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00025612 File Offset: 0x00023812
		IEnumerator<PSClassInfo> IEnumerable<PSClassInfo>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00025615 File Offset: 0x00023815
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00025618 File Offset: 0x00023818
		public bool MoveNext()
		{
			this._currentMatch = this.GetNextClass();
			return this._currentMatch != null;
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00025631 File Offset: 0x00023831
		PSClassInfo IEnumerator<PSClassInfo>.Current
		{
			get
			{
				return this._currentMatch;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00025639 File Offset: 0x00023839
		object IEnumerator.Current
		{
			get
			{
				return ((IEnumerator<PSClassInfo>)this).Current;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00025644 File Offset: 0x00023844
		private PSClassInfo GetNextClass()
		{
			PSClassInfo result = null;
			WildcardPattern classNameMatcher = new WildcardPattern(this._className, WildcardOptions.IgnoreCase);
			if (this.matchingClassList == null)
			{
				this.matchingClassList = new Collection<PSClassInfo>();
				if (!this.FindMatchInCachedTypes(classNameMatcher) && !this.FindTypeByModulePath(classNameMatcher))
				{
					return null;
				}
				this.matchingClass = this.matchingClassList.GetEnumerator();
			}
			if (!this.matchingClass.MoveNext())
			{
				this.matchingClass = null;
			}
			else
			{
				result = this.matchingClass.Current;
			}
			return result;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000256C0 File Offset: 0x000238C0
		private bool FindTypeByModulePath(WildcardPattern classNameMatcher)
		{
			bool result = false;
			List<string> defaultAvailableModuleFiles = ModuleUtils.GetDefaultAvailableModuleFiles(false, false, this._context);
			foreach (string text in defaultAvailableModuleFiles)
			{
				string fullPath = Path.GetFullPath(text);
				HashSet<string> exportedClasses = AnalysisCache.GetExportedClasses(fullPath, this._context);
				if (exportedClasses != null)
				{
					if (!this._useWildCards)
					{
						if (exportedClasses.Contains(this._className))
						{
							PSClassInfo psclassInfo = this.CachedItemToPSClassInfo(classNameMatcher, text);
							if (psclassInfo != null)
							{
								this.matchingClassList.Add(psclassInfo);
								result = true;
							}
						}
					}
					else
					{
						foreach (string input in exportedClasses)
						{
							if (classNameMatcher.IsMatch(input))
							{
								PSClassInfo psclassInfo2 = this.CachedItemToPSClassInfo(classNameMatcher, text);
								if (psclassInfo2 != null)
								{
									this.matchingClassList.Add(psclassInfo2);
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000257D4 File Offset: 0x000239D4
		private bool FindMatchInCachedTypes(WildcardPattern classNameMatcher)
		{
			bool result = false;
			foreach (KeyValuePair<string, HashSet<string>> keyValuePair in AnalysisCache.GetCachedClasses())
			{
				if (!this._useWildCards)
				{
					if (keyValuePair.Value.Contains(this._className))
					{
						PSClassInfo psclassInfo = this.CachedItemToPSClassInfo(classNameMatcher, keyValuePair.Key);
						if (psclassInfo != null)
						{
							this.matchingClassList.Add(psclassInfo);
							result = true;
						}
					}
				}
				else
				{
					foreach (string input in keyValuePair.Value)
					{
						if (classNameMatcher.IsMatch(input))
						{
							PSClassInfo psclassInfo2 = this.CachedItemToPSClassInfo(classNameMatcher, keyValuePair.Key);
							if (psclassInfo2 != null)
							{
								this.matchingClassList.Add(psclassInfo2);
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000258D0 File Offset: 0x00023AD0
		private PSClassInfo CachedItemToPSClassInfo(WildcardPattern classNameMatcher, string modulePath)
		{
			foreach (PSModuleInfo psmoduleInfo in this.GetPSModuleInfo(modulePath))
			{
				ReadOnlyDictionary<string, TypeDefinitionAst> exportedTypeDefinitions = psmoduleInfo.GetExportedTypeDefinitions();
				TypeDefinitionAst typeDefinitionAst = null;
				if (!this._useWildCards)
				{
					if (exportedTypeDefinitions.TryGetValue(this._className, out typeDefinitionAst))
					{
						ScriptBlockAst scriptBlockAst = typeDefinitionAst.Parent.Parent as ScriptBlockAst;
						if (scriptBlockAst != null)
						{
							return this.ConvertToClassInfo(psmoduleInfo, scriptBlockAst, typeDefinitionAst);
						}
					}
				}
				else
				{
					foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in exportedTypeDefinitions)
					{
						if (keyValuePair.Value != null && classNameMatcher.IsMatch(keyValuePair.Value.Name))
						{
							ScriptBlockAst scriptBlockAst = keyValuePair.Value.Parent.Parent as ScriptBlockAst;
							if (scriptBlockAst != null)
							{
								return this.ConvertToClassInfo(psmoduleInfo, scriptBlockAst, keyValuePair.Value);
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000259F4 File Offset: 0x00023BF4
		private Collection<PSModuleInfo> GetPSModuleInfo(string modulePath)
		{
			PSModuleInfo psmoduleInfo = null;
			lock (this.lockObject)
			{
				this.moduleInfoCache.TryGetValue(modulePath, out psmoduleInfo);
			}
			if (psmoduleInfo != null)
			{
				return new Collection<PSModuleInfo>
				{
					psmoduleInfo
				};
			}
			CommandInfo commandInfo = new CmdletInfo("Get-Module", typeof(GetModuleCommand), null, null, this._context);
			Command command = new Command(commandInfo);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(modulePath);
			Collection<PSModuleInfo> collection = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("List", true).AddParameter("Name", fileNameWithoutExtension).AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false).Invoke<PSModuleInfo>();
			lock (this.lockObject)
			{
				foreach (PSModuleInfo psmoduleInfo2 in collection)
				{
					this.moduleInfoCache.Add(psmoduleInfo2.Path, psmoduleInfo2);
				}
			}
			return collection;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00025B7C File Offset: 0x00023D7C
		private PSClassInfo ConvertToClassInfo(PSModuleInfo module, ScriptBlockAst ast, TypeDefinitionAst statement)
		{
			PSClassInfo psclassInfo = new PSClassInfo(statement.Name);
			psclassInfo.Module = module;
			Collection<PSClassMemberInfo> collection = new Collection<PSClassMemberInfo>();
			foreach (MemberAst memberAst in statement.Members)
			{
				PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
				if (propertyMemberAst != null)
				{
					PSClassMemberInfo item = new PSClassMemberInfo(propertyMemberAst.Name, propertyMemberAst.PropertyType.TypeName.FullName, propertyMemberAst.Extent.Text);
					collection.Add(item);
				}
			}
			psclassInfo.UpdateMembers(collection);
			string text = null;
			if (ast.GetHelpContent() != null)
			{
				text = ast.GetHelpContent().MamlHelpFile;
			}
			if (!string.IsNullOrEmpty(text))
			{
				psclassInfo.HelpFile = text;
			}
			return psclassInfo;
		}

		// Token: 0x04000373 RID: 883
		private string _className;

		// Token: 0x04000374 RID: 884
		private ExecutionContext _context;

		// Token: 0x04000375 RID: 885
		private PSClassInfo _currentMatch;

		// Token: 0x04000376 RID: 886
		private IEnumerator<PSClassInfo> matchingClass;

		// Token: 0x04000377 RID: 887
		private Collection<PSClassInfo> matchingClassList;

		// Token: 0x04000378 RID: 888
		private bool _useWildCards;

		// Token: 0x04000379 RID: 889
		private Dictionary<string, PSModuleInfo> moduleInfoCache;

		// Token: 0x0400037A RID: 890
		private object lockObject = new object();
	}
}
