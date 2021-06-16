using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.PowerShell.Cmdletization;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000A5 RID: 165
	public class ModuleCmdletBase : PSCmdlet
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00027932 File Offset: 0x00025B32
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x00027929 File Offset: 0x00025B29
		internal string BasePrefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0002793A File Offset: 0x00025B3A
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00027942 File Offset: 0x00025B42
		internal bool BaseForce
		{
			get
			{
				return this._force;
			}
			set
			{
				this._force = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0002794B File Offset: 0x00025B4B
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x00027953 File Offset: 0x00025B53
		internal bool BaseGlobal
		{
			get
			{
				return this._global;
			}
			set
			{
				this._global = value;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0002795C File Offset: 0x00025B5C
		internal SessionState TargetSessionState
		{
			get
			{
				if (this.BaseGlobal)
				{
					return base.Context.TopLevelSessionState.PublicSessionState;
				}
				return base.Context.SessionState;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00027982 File Offset: 0x00025B82
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0002798A File Offset: 0x00025B8A
		internal bool BasePassThru
		{
			get
			{
				return this._passThru;
			}
			set
			{
				this._passThru = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00027993 File Offset: 0x00025B93
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0002799B File Offset: 0x00025B9B
		internal bool BaseAsCustomObject
		{
			get
			{
				return this._baseAsCustomObject;
			}
			set
			{
				this._baseAsCustomObject = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000279A4 File Offset: 0x00025BA4
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x000279AC File Offset: 0x00025BAC
		internal List<WildcardPattern> BaseFunctionPatterns
		{
			get
			{
				return this._functionPatterns;
			}
			set
			{
				this._functionPatterns = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x000279B5 File Offset: 0x00025BB5
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x000279BD File Offset: 0x00025BBD
		internal List<WildcardPattern> BaseCmdletPatterns
		{
			get
			{
				return this._cmdletPatterns;
			}
			set
			{
				this._cmdletPatterns = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x000279C6 File Offset: 0x00025BC6
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x000279CE File Offset: 0x00025BCE
		internal List<WildcardPattern> BaseVariablePatterns
		{
			get
			{
				return this._variablePatterns;
			}
			set
			{
				this._variablePatterns = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x000279D7 File Offset: 0x00025BD7
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x000279DF File Offset: 0x00025BDF
		internal List<WildcardPattern> BaseAliasPatterns
		{
			get
			{
				return this._aliasPatterns;
			}
			set
			{
				this._aliasPatterns = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000279E8 File Offset: 0x00025BE8
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000279F0 File Offset: 0x00025BF0
		internal Version BaseMinimumVersion
		{
			get
			{
				return this._minimumVersion;
			}
			set
			{
				this._minimumVersion = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000279F9 File Offset: 0x00025BF9
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x00027A01 File Offset: 0x00025C01
		internal Version BaseMaximumVersion
		{
			get
			{
				return this._maximumVersion;
			}
			set
			{
				this._maximumVersion = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00027A0A File Offset: 0x00025C0A
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x00027A12 File Offset: 0x00025C12
		internal Version BaseRequiredVersion
		{
			get
			{
				return this._requiredVersion;
			}
			set
			{
				this._requiredVersion = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00027A1B File Offset: 0x00025C1B
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00027A23 File Offset: 0x00025C23
		internal Guid? BaseGuid { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00027A2C File Offset: 0x00025C2C
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00027A34 File Offset: 0x00025C34
		protected object[] BaseArgumentList
		{
			get
			{
				return this._arguments;
			}
			set
			{
				this._arguments = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00027A3D File Offset: 0x00025C3D
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00027A45 File Offset: 0x00025C45
		protected bool BaseDisableNameChecking
		{
			get
			{
				return this._disableNameChecking;
			}
			set
			{
				this._disableNameChecking = value;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00027A4E File Offset: 0x00025C4E
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x00027A56 File Offset: 0x00025C56
		protected bool AddToAppDomainLevelCache
		{
			get
			{
				return this._addToAppDomainLevelCache;
			}
			set
			{
				this._addToAppDomainLevelCache = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00027A5F File Offset: 0x00025C5F
		internal List<WildcardPattern> MatchAll
		{
			get
			{
				if (this._matchAll == null)
				{
					this._matchAll = new List<WildcardPattern>();
					this._matchAll.Add(new WildcardPattern("*", WildcardOptions.IgnoreCase));
				}
				return this._matchAll;
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00027A90 File Offset: 0x00025C90
		internal bool LoadUsingModulePath(bool found, IEnumerable<string> modulePath, string name, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out PSModuleInfo module)
		{
			return this.LoadUsingModulePath(null, found, modulePath, name, ss, options, manifestProcessingFlags, out module);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00027AB0 File Offset: 0x00025CB0
		internal bool LoadUsingModulePath(PSModuleInfo parentModule, bool found, IEnumerable<string> modulePath, string name, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out PSModuleInfo module)
		{
			string text = Path.GetExtension(name);
			module = null;
			string text2;
			if (string.IsNullOrEmpty(text) || !ModuleIntrinsics.IsPowerShellModuleExtension(text))
			{
				text2 = name;
				text = null;
			}
			else
			{
				text2 = name.Substring(0, name.Length - text.Length);
			}
			foreach (string path in modulePath)
			{
				string text3 = Path.Combine(path, text2);
				module = this.LoadUsingMultiVersionModuleBase(text3, options, out found);
				if (!found)
				{
					if (name.IndexOfAny(Utils.DirectorySeparators) == -1)
					{
						text3 = Path.Combine(text3, text2);
					}
					else if (Directory.Exists(text3))
					{
						text3 = Path.Combine(text3, Path.GetFileName(text2));
					}
					module = this.LoadUsingExtensions(parentModule, name, text3, text, null, this.BasePrefix, ss, options, manifestProcessingFlags, out found);
				}
				if (found)
				{
					break;
				}
			}
			if (found && module != null && !module.HadErrorsLoading)
			{
				if (module.ExportedWorkflows != null && module.ExportedWorkflows.Count > 0 && Utils.IsRunningFromSysWOW64())
				{
					throw new NotSupportedException(AutomationExceptions.WorkflowDoesNotSupportWOW64);
				}
				bool flag = (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements;
				bool flag2 = (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.Force) == ModuleCmdletBase.ManifestProcessingFlags.Force;
				bool force = flag || flag2;
				AnalysisCache.CacheExportedCommands(module, force, base.Context);
			}
			return found;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00027C08 File Offset: 0x00025E08
		internal PSModuleInfo LoadUsingMultiVersionModuleBase(string moduleBase, ModuleCmdletBase.ImportModuleOptions importModuleOptions, out bool found)
		{
			PSModuleInfo result = null;
			found = false;
			foreach (Version version in ModuleUtils.GetModuleVersionSubfolders(moduleBase))
			{
				if ((!(this.BaseRequiredVersion != null) || this.BaseRequiredVersion.Equals(version)) && (!(this.BaseMinimumVersion != null) || !(this.BaseRequiredVersion == null) || !(version < this.BaseMinimumVersion)) && (!(this.BaseMaximumVersion != null) || !(this.BaseRequiredVersion == null) || !(version > this.BaseMaximumVersion)))
				{
					string text = Path.Combine(moduleBase, Path.Combine(version.ToString(), Path.GetFileName(moduleBase)));
					string text2 = text + ".psd1";
					bool flag = false;
					if (File.Exists(text2))
					{
						flag = version.Equals(ModuleIntrinsics.GetManifestModuleVersion(text2));
						if (flag)
						{
							result = this.LoadUsingExtensions(null, moduleBase, text, ".psd1", null, this.BasePrefix, null, importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out found);
							if (found)
							{
								break;
							}
						}
					}
					if (!flag)
					{
						base.WriteVerbose(string.Format(CultureInfo.InvariantCulture, Modules.SkippingInvalidModuleVersionFolder, new object[]
						{
							version.ToString(),
							moduleBase
						}));
					}
				}
			}
			return result;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00027D78 File Offset: 0x00025F78
		private Hashtable LoadModuleManifestData(ExternalScriptInfo scriptInfo, string[] validMembers, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, ref bool containedErrors)
		{
			Hashtable result;
			try
			{
				result = this.LoadModuleManifestData(scriptInfo.Path, scriptInfo.ScriptBlock, validMembers, manifestProcessingFlags, ref containedErrors);
			}
			catch (RuntimeException ex)
			{
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					string message = StringUtil.Format(Modules.InvalidModuleManifest, scriptInfo.Path, ex.Message);
					MissingMemberException exception = new MissingMemberException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, scriptInfo.Path);
					base.WriteError(errorRecord);
				}
				containedErrors = true;
				result = null;
			}
			return result;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00027DFC File Offset: 0x00025FFC
		internal Hashtable LoadModuleManifestData(string moduleManifestPath, ScriptBlock scriptBlock, string[] validMembers, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, ref bool containedErrors)
		{
			try
			{
				scriptBlock.CheckRestrictedLanguage(ModuleCmdletBase.PermittedCmdlets, new List<string>
				{
					"PSScriptRoot"
				}, true);
			}
			catch (RuntimeException ex)
			{
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					string message = StringUtil.Format(Modules.InvalidModuleManifest, moduleManifestPath, ex.Message);
					MissingMemberException exception = new MissingMemberException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
					base.WriteError(errorRecord);
				}
				containedErrors = true;
				return null;
			}
			object variableValue = base.Context.GetVariableValue(SpecialVariables.PSScriptRootVarPath);
			object variableValue2 = base.Context.GetVariableValue(SpecialVariables.PSCommandPathVarPath);
			ArrayList arrayList = (ArrayList)base.Context.GetVariableValue(SpecialVariables.ErrorVarPath);
			int count = arrayList.Count;
			object obj;
			try
			{
				base.Context.SetVariable(SpecialVariables.PSScriptRootVarPath, Path.GetDirectoryName(moduleManifestPath));
				base.Context.SetVariable(SpecialVariables.PSCommandPathVarPath, moduleManifestPath);
				obj = PSObject.Base(scriptBlock.InvokeReturnAsIs(new object[0]));
			}
			finally
			{
				base.Context.SetVariable(SpecialVariables.PSScriptRootVarPath, variableValue);
				base.Context.SetVariable(SpecialVariables.PSCommandPathVarPath, variableValue2);
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
				{
					while (arrayList.Count > count)
					{
						arrayList.RemoveAt(0);
					}
				}
			}
			Hashtable hashtable = obj as Hashtable;
			if (hashtable == null)
			{
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					string message = StringUtil.Format(Modules.EmptyModuleManifest, moduleManifestPath);
					ArgumentException exception2 = new ArgumentException(message);
					ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
					base.WriteError(errorRecord2);
				}
				containedErrors = true;
				return null;
			}
			hashtable = new Hashtable(hashtable, StringComparer.OrdinalIgnoreCase);
			if (validMembers != null && !this.ValidateManifestHash(hashtable, validMembers, moduleManifestPath, manifestProcessingFlags))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			return hashtable;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00027FC0 File Offset: 0x000261C0
		private bool ValidateManifestHash(Hashtable data, string[] validMembers, string moduleManifestPath, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags)
		{
			bool result = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in data.Keys)
			{
				string text = (string)obj;
				bool flag = false;
				foreach (string value in validMembers)
				{
					if (text.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("'");
					stringBuilder.Append(text);
					stringBuilder.Append("'");
				}
			}
			if (stringBuilder.Length > 0)
			{
				result = false;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					Version psversion = PSVersionInfo.PSVersion;
					Version version;
					if (this.GetScalarFromData<Version>(data, moduleManifestPath, "PowerShellVersion", manifestProcessingFlags, out version) && psversion < version)
					{
						string message = StringUtil.Format(Modules.ModuleManifestInsufficientPowerShellVersion, new object[]
						{
							psversion,
							moduleManifestPath,
							version
						});
						InvalidOperationException exception = new InvalidOperationException(message);
						ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InsufficientPowerShellVersion", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord);
					}
					else
					{
						StringBuilder stringBuilder2 = new StringBuilder("'");
						stringBuilder2.Append(validMembers[0]);
						for (int j = 1; j < validMembers.Length; j++)
						{
							stringBuilder2.Append("', '");
							stringBuilder2.Append(validMembers[j]);
						}
						stringBuilder2.Append("'");
						string message = StringUtil.Format(Modules.InvalidModuleManifestMember, new object[]
						{
							moduleManifestPath,
							stringBuilder2,
							stringBuilder
						});
						InvalidOperationException exception2 = new InvalidOperationException(message);
						ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_InvalidManifestMember", ErrorCategory.InvalidData, moduleManifestPath);
						base.WriteError(errorRecord2);
					}
				}
			}
			return result;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000281AC File Offset: 0x000263AC
		private PSModuleInfo LoadModuleNamedInManifest(PSModuleInfo parentModule, ModuleSpecification moduleSpecification, string moduleBase, bool searchModulePath, string prefix, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, bool loadTypesFiles, bool loadFormatFiles, object privateData, out bool found, string shortModuleName)
		{
			PSModuleInfo psmoduleInfo = null;
			PSModuleInfo psmoduleInfo2 = null;
			found = false;
			bool flag = false;
			bool flag2 = false;
			Version baseMinimumVersion = this.BaseMinimumVersion;
			Version baseMaximumVersion = this.BaseMaximumVersion;
			Version baseRequiredVersion = this.BaseRequiredVersion;
			Guid? baseGuid = this.BaseGuid;
			string text = ModuleCmdletBase.ResolveRootedFilePath(moduleSpecification.Name, base.Context);
			if (string.IsNullOrEmpty(text))
			{
				text = Path.Combine(moduleBase, moduleSpecification.Name);
			}
			else
			{
				flag2 = true;
			}
			string text2 = Path.GetExtension(moduleSpecification.Name);
			PSModuleInfo result;
			try
			{
				base.Context.Modules.IncrementModuleNestingDepth(this, text);
				this.BaseMinimumVersion = null;
				this.BaseMaximumVersion = null;
				this.BaseRequiredVersion = null;
				this.BaseGuid = null;
				if (!ModuleIntrinsics.IsPowerShellModuleExtension(text2))
				{
					if (File.Exists(text))
					{
						PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(Modules.ManifestMemberNotValid, new object[]
						{
							moduleSpecification.Name,
							"NestedModules",
							parentModule.Path,
							StringUtil.Format(Modules.InvalidModuleExtension, text2, moduleSpecification.Name),
							ModuleIntrinsics.GetModuleName(parentModule.Path)
						});
						ex.SetErrorId("Modules_InvalidModuleExtension");
						throw ex;
					}
					text2 = null;
				}
				if (text2 == null)
				{
					if (this.VerifyIfNestedModuleIsAvailable(moduleSpecification, text, text2, out psmoduleInfo2))
					{
						psmoduleInfo = this.LoadUsingExtensions(parentModule, moduleSpecification.Name, text, text2, moduleBase, prefix, ss, options, manifestProcessingFlags, out found, out flag);
					}
					if (!found && !flag)
					{
						string text3 = Path.Combine(text, moduleSpecification.Name);
						string moduleBase2 = Path.Combine(moduleBase, moduleSpecification.Name);
						if (this.VerifyIfNestedModuleIsAvailable(moduleSpecification, text3, text2, out psmoduleInfo2))
						{
							psmoduleInfo = this.LoadUsingExtensions(parentModule, moduleSpecification.Name, text3, text2, moduleBase2, prefix, ss, options, manifestProcessingFlags, out found, out flag);
						}
					}
				}
				else
				{
					if (this.VerifyIfNestedModuleIsAvailable(moduleSpecification, text, text2, out psmoduleInfo2))
					{
						psmoduleInfo = this.LoadModule(parentModule, text, moduleBase, prefix, ss, privateData, ref options, manifestProcessingFlags, out found, out flag);
					}
					if (!found && !flag)
					{
						string text4 = Path.Combine(text, moduleSpecification.Name);
						string moduleBase3 = Path.Combine(moduleBase, moduleSpecification.Name);
						if (this.VerifyIfNestedModuleIsAvailable(moduleSpecification, text4, text2, out psmoduleInfo2))
						{
							psmoduleInfo = this.LoadModule(parentModule, text4, moduleBase3, prefix, ss, privateData, ref options, manifestProcessingFlags, out found, out flag);
						}
					}
				}
				if (!found && flag2)
				{
					result = null;
				}
				else
				{
					if (searchModulePath && !found && !flag && this.VerifyIfNestedModuleIsAvailable(moduleSpecification, null, null, out psmoduleInfo2))
					{
						IEnumerable<string> modulePath;
						if (psmoduleInfo2 != null)
						{
							string fileName = Path.GetFileName(psmoduleInfo2.ModuleBase);
							Version version;
							if (Version.TryParse(fileName, out version))
							{
								string directoryName = Path.GetDirectoryName(psmoduleInfo2.ModuleBase);
								modulePath = new string[]
								{
									Path.GetDirectoryName(directoryName),
									directoryName
								};
							}
							else
							{
								modulePath = new string[]
								{
									Path.GetDirectoryName(psmoduleInfo2.ModuleBase),
									psmoduleInfo2.ModuleBase
								};
							}
						}
						else
						{
							modulePath = ModuleIntrinsics.GetModulePath(false, base.Context);
						}
						found = this.LoadUsingModulePath(parentModule, found, modulePath, moduleSpecification.Name, ss, options, manifestProcessingFlags, out psmoduleInfo);
					}
					if (!found && moduleSpecification.Guid == null && moduleSpecification.Version == null && moduleSpecification.RequiredVersion == null && moduleSpecification.MaximumVersion == null)
					{
						bool flag3 = true;
						if (parentModule != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements && parentModule.ExportedCmdlets != null && parentModule.ExportedCmdlets.Count > 0)
						{
							flag3 = false;
							foreach (string pattern in parentModule.ExportedCmdlets.Keys)
							{
								if (WildcardPattern.ContainsWildcardCharacters(pattern))
								{
									flag3 = true;
									break;
								}
							}
							found = true;
						}
						if (flag3)
						{
							try
							{
								psmoduleInfo = this.LoadBinaryModule(parentModule, true, moduleSpecification.Name, null, null, moduleBase, ss, options, manifestProcessingFlags, prefix, loadTypesFiles, loadFormatFiles, out found, shortModuleName, false);
							}
							catch (FileNotFoundException)
							{
							}
							if (psmoduleInfo != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
							{
								ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
							}
						}
					}
					result = psmoduleInfo;
				}
			}
			finally
			{
				this.BaseMinimumVersion = baseMinimumVersion;
				this.BaseMaximumVersion = baseMaximumVersion;
				this.BaseRequiredVersion = baseRequiredVersion;
				this.BaseGuid = baseGuid;
				base.Context.Modules.DecrementModuleNestingCount();
			}
			return result;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00028638 File Offset: 0x00026838
		internal List<PSModuleInfo> GetModule(string[] names, bool all, bool refresh)
		{
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			if (names != null)
			{
				foreach (string text in names)
				{
					if (text.IndexOf('\\') != -1 || text.IndexOf('/') != -1)
					{
						list2.Add(text);
					}
					else
					{
						list3.Add(text);
					}
				}
				list.AddRange(this.GetModuleForRootedPaths(list2.ToArray(), all, refresh));
			}
			if (names == null || list3.Count > 0)
			{
				list.AddRange(this.GetModuleForNonRootedPaths(list3.ToArray(), all, refresh));
			}
			return list;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00028968 File Offset: 0x00026B68
		private IEnumerable<PSModuleInfo> GetModuleForNonRootedPaths(string[] names, bool all, bool refresh)
		{
			IEnumerable<WildcardPattern> patternList = SessionStateUtilities.CreateWildcardsFromStrings(names, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			Dictionary<string, List<PSModuleInfo>> availableModules = this.GetAvailableLocallyModulesCore(names, all, refresh);
			foreach (KeyValuePair<string, List<PSModuleInfo>> entry in availableModules)
			{
				KeyValuePair<string, List<PSModuleInfo>> keyValuePair = entry;
				foreach (PSModuleInfo module in keyValuePair.Value)
				{
					if (SessionStateUtilities.MatchesAnyWildcardPattern(module.Name, patternList, true))
					{
						yield return module;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00028DF4 File Offset: 0x00026FF4
		private IEnumerable<PSModuleInfo> GetModuleForRootedPaths(string[] modulePaths, bool all, bool refresh)
		{
			HashSet<string> modules = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			IEnumerable<string> moduleNames = from p in modulePaths
			select Path.GetFileNameWithoutExtension(p);
			foreach (string mp in modulePaths)
			{
				string modulePath = ModuleCmdletBase.ResolveRootedFilePath(mp, base.Context);
				if (string.IsNullOrEmpty(modulePath))
				{
					if (!WildcardPattern.ContainsWildcardCharacters(mp))
					{
						base.WriteError(this.CreateModuleNotFoundError(mp));
					}
				}
				else if (!Utils.NativeDirectoryExists(modulePath))
				{
					PSModuleInfo module = this.CreateModuleInfoForGetModule(modulePath, refresh);
					if (module != null && !modules.Contains(modulePath))
					{
						modules.Add(modulePath);
						yield return module;
					}
				}
				else
				{
					List<string> availableModuleFiles = null;
					if (all)
					{
						availableModuleFiles = ModuleUtils.GetAllAvailableModuleFiles(modulePath);
					}
					else
					{
						availableModuleFiles = ModuleUtils.GetModuleVersionsFromAbsolutePath(modulePath);
					}
					bool foundModule = false;
					foreach (string file in availableModuleFiles)
					{
						PSModuleInfo module2 = this.CreateModuleInfoForGetModule(file, refresh);
						if (module2 != null && SessionStateUtilities.CollectionContainsValue(moduleNames, module2.Name, StringComparer.OrdinalIgnoreCase))
						{
							foundModule = true;
							if (!modules.Contains(modulePath))
							{
								modules.Add(modulePath);
								yield return module2;
							}
						}
					}
					if (!foundModule && !WildcardPattern.ContainsWildcardCharacters(mp))
					{
						base.WriteError(this.CreateModuleNotFoundError(modulePath));
					}
				}
			}
			yield break;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00028E28 File Offset: 0x00027028
		private ErrorRecord CreateModuleNotFoundError(string modulePath)
		{
			string message = StringUtil.Format(Modules.ModuleNotFoundForGetModule, modulePath);
			FileNotFoundException exception = new FileNotFoundException(message);
			return new ErrorRecord(exception, "Modules_ModuleNotFoundForGetModule", ErrorCategory.ResourceUnavailable, modulePath);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00028E60 File Offset: 0x00027060
		private Dictionary<string, List<PSModuleInfo>> GetAvailableLocallyModulesCore(string[] names, bool all, bool refresh)
		{
			Dictionary<string, List<PSModuleInfo>> dictionary = new Dictionary<string, List<PSModuleInfo>>(StringComparer.OrdinalIgnoreCase);
			List<string> modulePaths = (List<string>)ModuleIntrinsics.GetModulePath(false, base.Context);
			bool flag = base.Context.TakeResponsibilityForModuleAnalysisAppDomain();
			try
			{
				foreach (string text in ModuleIntrinsics.GetModulePath(false, base.Context))
				{
					try
					{
						List<PSModuleInfo> source;
						if (all)
						{
							source = this.GetAllAvailableModules(text, refresh);
						}
						else
						{
							source = this.GetDefaultAvailableModules(names, text, modulePaths, refresh);
						}
						if (!dictionary.ContainsKey(text))
						{
							dictionary.Add(text, (from m in source
							orderby m.Name
							select m).ToList<PSModuleInfo>());
						}
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
				}
			}
			finally
			{
				if (flag)
				{
					base.Context.ReleaseResponsibilityForModuleAnalysisAppDomain();
				}
			}
			return dictionary;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00028F70 File Offset: 0x00027170
		private List<PSModuleInfo> GetAllAvailableModules(string directory, bool refresh)
		{
			List<string> allAvailableModuleFiles = ModuleUtils.GetAllAvailableModuleFiles(directory);
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			foreach (string file in allAvailableModuleFiles)
			{
				PSModuleInfo psmoduleInfo = this.CreateModuleInfoForGetModule(file, refresh);
				if (psmoduleInfo != null)
				{
					list.Add(psmoduleInfo);
				}
			}
			return list;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00028FDC File Offset: 0x000271DC
		private List<PSModuleInfo> GetDefaultAvailableModules(string[] name, string directory, List<string> modulePaths, bool refresh)
		{
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			IEnumerable<WildcardPattern> patterns = SessionStateUtilities.CreateWildcardsFromStrings(name, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			List<string> defaultAvailableModuleFiles = ModuleUtils.GetDefaultAvailableModuleFiles(directory, modulePaths);
			foreach (string text in defaultAvailableModuleFiles)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				if (SessionStateUtilities.MatchesAnyWildcardPattern(fileNameWithoutExtension, patterns, true))
				{
					PSModuleInfo psmoduleInfo = this.CreateModuleInfoForGetModule(text, refresh);
					if (psmoduleInfo != null)
					{
						if (!psmoduleInfo.HadErrorsLoading)
						{
							AnalysisCache.CacheExportedCommands(psmoduleInfo, refresh, base.Context);
						}
						else
						{
							ModuleIntrinsics.Tracer.WriteLine("Caching skipped for " + psmoduleInfo.Name + " because it had errors while loading.", new object[0]);
						}
						list.Add(psmoduleInfo);
					}
				}
			}
			this.ClearAnalysisCaches();
			return list;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000290B4 File Offset: 0x000272B4
		internal static Version GetMaximumVersion(string stringVersion)
		{
			Version result;
			if (Version.TryParse(stringVersion, out result))
			{
				return result;
			}
			string text = "999999999";
			if (stringVersion[stringVersion.Length - 1] == '*')
			{
				stringVersion = stringVersion.Substring(0, stringVersion.Length - 1);
				stringVersion += text;
				int num = stringVersion.Count((char x) => x == '.');
				for (int i = 0; i < 3 - num; i++)
				{
					stringVersion = stringVersion + '.' + text;
				}
			}
			if (Version.TryParse(stringVersion, out result))
			{
				return new Version(stringVersion);
			}
			string message = StringUtil.Format(Modules.MaximumVersionFormatIncorrect, stringVersion);
			throw new PSArgumentException(message);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00029168 File Offset: 0x00027368
		private PSModuleInfo CreateModuleInfoForGetModule(string file, bool refresh)
		{
			if (this.currentlyProcessingModules.ContainsKey(file))
			{
				return this.currentlyProcessingModules[file];
			}
			this.currentlyProcessingModules[file] = null;
			PSModuleInfo psmoduleInfo = null;
			string extension = Path.GetExtension(file);
			ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags = ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError;
			manifestProcessingFlags |= ModuleCmdletBase.ManifestProcessingFlags.IgnoreHostNameAndHostVersion;
			if (refresh)
			{
				manifestProcessingFlags |= ModuleCmdletBase.ManifestProcessingFlags.Force;
			}
			try
			{
				if (extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					string text;
					ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(file, out text, true);
					psmoduleInfo = this.LoadModuleManifest(scriptInfoForFile, manifestProcessingFlags, null, null, null, null);
				}
				else
				{
					ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
					bool flag = false;
					psmoduleInfo = this.LoadModule(file, null, string.Empty, null, ref importModuleOptions, manifestProcessingFlags, out flag);
				}
				if (psmoduleInfo == null)
				{
					psmoduleInfo = new PSModuleInfo(file, null, null);
					psmoduleInfo.HadErrorsLoading = true;
				}
				if (extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					if (psmoduleInfo.RootModuleForManifest != null)
					{
						if (psmoduleInfo.RootModuleForManifest.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo.SetModuleType(ModuleType.Binary);
						}
						else if (psmoduleInfo.RootModuleForManifest.EndsWith(".psm1", StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo.SetModuleType(ModuleType.Script);
						}
						else if (psmoduleInfo.RootModuleForManifest.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo.SetModuleType(ModuleType.Workflow);
						}
						else if (psmoduleInfo.RootModuleForManifest.EndsWith(".cdxml", StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo.SetModuleType(ModuleType.Cim);
						}
						else
						{
							psmoduleInfo.SetModuleType(ModuleType.Manifest);
						}
						psmoduleInfo.RootModule = psmoduleInfo.RootModuleForManifest;
					}
					else
					{
						psmoduleInfo.SetModuleType(ModuleType.Manifest);
						psmoduleInfo.RootModule = psmoduleInfo.Path;
					}
				}
				else if (extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
				{
					psmoduleInfo.SetModuleType(ModuleType.Binary);
					psmoduleInfo.RootModule = psmoduleInfo.Path;
				}
				else if (extension.Equals(".xaml", StringComparison.OrdinalIgnoreCase))
				{
					psmoduleInfo.SetModuleType(ModuleType.Workflow);
					psmoduleInfo.RootModule = psmoduleInfo.Path;
				}
				else if (extension.Equals(".cdxml"))
				{
					psmoduleInfo.SetModuleType(ModuleType.Cim);
					string moduleName;
					ExternalScriptInfo scriptInfoForFile2 = this.GetScriptInfoForFile(file, out moduleName, true);
					StringReader cmdletizationXmlReader = new StringReader(scriptInfoForFile2.ScriptContents);
					ScriptWriter scriptWriter = new ScriptWriter(cmdletizationXmlReader, moduleName, "Microsoft.PowerShell.Cmdletization.Cim.CimCmdletAdapter, Microsoft.PowerShell.Commands.Management, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", base.MyInvocation, ScriptWriter.GenerationOptions.HelpXml);
					scriptWriter.PopulatePSModuleInfo(psmoduleInfo);
					psmoduleInfo.RootModule = psmoduleInfo.Path;
				}
				else
				{
					psmoduleInfo.SetModuleType(ModuleType.Script);
					psmoduleInfo.RootModule = psmoduleInfo.Path;
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				if (psmoduleInfo == null)
				{
					psmoduleInfo = new PSModuleInfo(file, null, null);
					psmoduleInfo.HadErrorsLoading = true;
				}
			}
			this.currentlyProcessingModules[file] = psmoduleInfo;
			return psmoduleInfo;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000293D8 File Offset: 0x000275D8
		internal PSModuleInfo LoadModuleManifest(ExternalScriptInfo scriptInfo, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, Version minimumVersion, Version maximumVersion, Version requiredVersion, Guid? requiredModuleGuid)
		{
			ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
			return this.LoadModuleManifest(scriptInfo, manifestProcessingFlags, minimumVersion, maximumVersion, requiredVersion, requiredModuleGuid, ref importModuleOptions);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00029400 File Offset: 0x00027600
		internal PSModuleInfo LoadModuleManifest(ExternalScriptInfo scriptInfo, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, Version minimumVersion, Version maximumVersion, Version requiredVersion, Guid? requiredModuleGuid, ref ModuleCmdletBase.ImportModuleOptions options)
		{
			bool flag = false;
			Hashtable data = null;
			Hashtable localizedData = null;
			if (!this.LoadModuleManifestData(scriptInfo, manifestProcessingFlags, out data, out localizedData, ref flag))
			{
				return null;
			}
			return this.LoadModuleManifest(scriptInfo.Path, scriptInfo, data, localizedData, manifestProcessingFlags, minimumVersion, maximumVersion, requiredVersion, requiredModuleGuid, ref options, ref flag);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00029440 File Offset: 0x00027640
		internal bool LoadModuleManifestData(ExternalScriptInfo scriptInfo, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out Hashtable data, out Hashtable localizedData, ref bool containedErrors)
		{
			localizedData = null;
			data = this.LoadModuleManifestData(scriptInfo, ModuleCmdletBase.ModuleManifestMembers, manifestProcessingFlags, ref containedErrors);
			if (data == null)
			{
				return false;
			}
			ExternalScriptInfo externalScriptInfo = this.FindLocalizedModuleManifest(scriptInfo.Path);
			localizedData = null;
			if (externalScriptInfo != null)
			{
				localizedData = this.LoadModuleManifestData(externalScriptInfo, null, manifestProcessingFlags, ref containedErrors);
				if (localizedData == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00029494 File Offset: 0x00027694
		private ErrorRecord GetErrorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario(Hashtable data, string moduleManifestPath, string rootModulePath)
		{
			if (rootModulePath == null)
			{
				return null;
			}
			if (!rootModulePath.EndsWith(".cdxml", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			if (!data.ContainsKey("NestedModules"))
			{
				return null;
			}
			string text = data.ContainsKey("ModuleToProcess") ? "ModuleToProcess" : "RootModule";
			string message = StringUtil.Format(Modules.CmdletizationDoesSupportRexportingNestedModules, new object[]
			{
				text,
				moduleManifestPath,
				rootModulePath
			});
			InvalidOperationException exception = new InvalidOperationException(message);
			return new ErrorRecord(exception, "Modules_CmdletizationDoesSupportRexportingNestedModules", ErrorCategory.InvalidOperation, moduleManifestPath);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002951C File Offset: 0x0002771C
		internal PSModuleInfo LoadModuleManifest(string moduleManifestPath, ExternalScriptInfo scriptInfo, Hashtable data, Hashtable localizedData, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, Version minimumVersion, Version maximumVersion, Version requiredVersion, Guid? requiredModuleGuid, ref ModuleCmdletBase.ImportModuleOptions options, ref bool containedErrors)
		{
			string directoryName = Path.GetDirectoryName(moduleManifestPath);
			if ((manifestProcessingFlags & (ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.LoadElements | ModuleCmdletBase.ManifestProcessingFlags.WriteWarnings)) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				base.Context.ModuleBeingProcessed = moduleManifestPath;
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string text = null;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "ModuleToProcess", manifestProcessingFlags, out text))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			string text2 = null;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "RootModule", manifestProcessingFlags, out text2))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			if (!string.IsNullOrEmpty(text) && (string.IsNullOrEmpty(base.Context.ModuleBeingProcessed) || !base.Context.ModuleBeingProcessed.Equals(moduleManifestPath, StringComparison.OrdinalIgnoreCase) || !base.Context.ModuleBeingProcessed.Equals(base.Context.PreviousModuleProcessed, StringComparison.OrdinalIgnoreCase)) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteWarnings) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				base.WriteWarning(Modules.ModuleToProcessFieldDeprecated);
			}
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				if ((string.IsNullOrEmpty(base.Context.ModuleBeingProcessed) || !base.Context.ModuleBeingProcessed.Equals(moduleManifestPath, StringComparison.OrdinalIgnoreCase) || !base.Context.ModuleBeingProcessed.Equals(base.Context.PreviousModuleProcessed, StringComparison.OrdinalIgnoreCase)) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					string message = StringUtil.Format(Modules.ModuleManifestCannotContainBothModuleToProcessAndRootModule, moduleManifestPath);
					InvalidOperationException exception = new InvalidOperationException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_ModuleManifestCannotContainBothModuleToProcessAndRootModule", ErrorCategory.InvalidOperation, moduleManifestPath);
					base.WriteError(errorRecord);
				}
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			string text3 = text ?? text2;
			bool flag = false;
			string text4 = text3;
			if (string.Equals(Path.GetExtension(text3), ".xaml", StringComparison.OrdinalIgnoreCase))
			{
				if (WildcardPattern.ContainsWildcardCharacters(text3))
				{
					PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(Modules.WildCardNotAllowedInModuleToProcessAndInNestedModules, new object[]
					{
						moduleManifestPath
					});
					ex.SetErrorId("Modules_WildCardNotAllowedInModuleToProcessAndInNestedModules");
					throw ex;
				}
				list.Add(text3);
				text3 = null;
				flag = true;
			}
			string text5 = null;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "DefaultCommandPrefix", manifestProcessingFlags, out text5))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			string prefix = string.Empty;
			if (!string.IsNullOrEmpty(text5))
			{
				prefix = text5;
			}
			if (!string.IsNullOrEmpty(this.BasePrefix))
			{
				prefix = this.BasePrefix;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				if (WildcardPattern.ContainsWildcardCharacters(text3))
				{
					PSInvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(Modules.WildCardNotAllowedInModuleToProcessAndInNestedModules, new object[]
					{
						moduleManifestPath
					});
					ex2.SetErrorId("Modules_WildCardNotAllowedInModuleToProcessAndInNestedModules");
					throw ex2;
				}
				PSModuleInfo psmoduleInfo = null;
				string text6 = this.FixupFileName(directoryName, text3, null);
				string extension = Path.GetExtension(text6);
				if (!string.IsNullOrEmpty(extension) && ModuleIntrinsics.IsPowerShellModuleExtension(extension))
				{
					base.Context.Modules.ModuleTable.TryGetValue(text6, out psmoduleInfo);
				}
				else
				{
					foreach (string extension2 in ModuleIntrinsics.PSModuleExtensions)
					{
						text6 = this.FixupFileName(directoryName, text3, extension2);
						base.Context.Modules.ModuleTable.TryGetValue(text6, out psmoduleInfo);
						if (psmoduleInfo != null)
						{
							break;
						}
					}
				}
				if (psmoduleInfo != null && (this.BaseRequiredVersion == null || psmoduleInfo.Version.Equals(this.BaseRequiredVersion)) && ((this.BaseMinimumVersion == null && this.BaseMaximumVersion == null) || (this.BaseMaximumVersion != null && this.BaseMinimumVersion == null && psmoduleInfo.Version <= this.BaseMaximumVersion) || (this.BaseMaximumVersion == null && this.BaseMinimumVersion != null && psmoduleInfo.Version >= this.BaseMinimumVersion) || (this.BaseMaximumVersion != null && this.BaseMinimumVersion != null && psmoduleInfo.Version >= this.BaseMinimumVersion && psmoduleInfo.Version <= this.BaseMaximumVersion)) && (this.BaseGuid == null || psmoduleInfo.Guid.Equals(this.BaseGuid)) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
				{
					if (!this.BaseForce)
					{
						ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
						this.ImportModuleMembers(psmoduleInfo, prefix, options);
						return psmoduleInfo;
					}
					if (Utils.NativeFileExists(text6))
					{
						this.RemoveModule(psmoduleInfo);
					}
				}
			}
			string empty = string.Empty;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "Author", manifestProcessingFlags, out empty))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			string empty2 = string.Empty;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "CompanyName", manifestProcessingFlags, out empty2))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			string empty3 = string.Empty;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "Copyright", manifestProcessingFlags, out empty3))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			Guid? guid;
			if (!this.GetScalarFromData<Guid?>(data, moduleManifestPath, "guid", manifestProcessingFlags, out guid))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			Version version;
			if (!this.GetScalarFromData<Version>(data, moduleManifestPath, "ModuleVersion", manifestProcessingFlags, out version))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else
			{
				if (version == null)
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message = StringUtil.Format(Modules.ModuleManifestMissingModuleVersion, moduleManifestPath);
						MissingMemberException exception2 = new MissingMemberException(message);
						ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord2);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
				else if (requiredVersion != null && !version.Equals(requiredVersion))
				{
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
				else if ((version < minimumVersion || (maximumVersion != null && version > maximumVersion)) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
				if (requiredModuleGuid != null && !requiredModuleGuid.Equals(guid) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
				DirectoryInfo directoryInfo = null;
				try
				{
					directoryInfo = ClrFacade.GetParent(moduleManifestPath);
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (ArgumentException)
				{
				}
				Version version2;
				if (directoryInfo != null && Version.TryParse(directoryInfo.Name, out version2) && directoryInfo.Parent != null && directoryInfo.Parent.Name.Equals(Path.GetFileNameWithoutExtension(moduleManifestPath)) && !version2.Equals(version))
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message = StringUtil.Format(Modules.InvalidModuleManifestVersion, new object[]
						{
							moduleManifestPath,
							version.ToString(),
							directoryInfo.FullName
						});
						InvalidOperationException exception3 = new InvalidOperationException(message);
						ErrorRecord errorRecord3 = new ErrorRecord(exception3, "Modules_InvalidModuleManifestVersion", ErrorCategory.InvalidArgument, moduleManifestPath);
						base.WriteError(errorRecord3);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
			}
			Version version3;
			if (!this.GetScalarFromData<Version>(data, moduleManifestPath, "PowerShellVersion", manifestProcessingFlags, out version3))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (version3 != null)
			{
				Version psversion = PSVersionInfo.PSVersion;
				if (psversion < version3)
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message = StringUtil.Format(Modules.ModuleManifestInsufficientPowerShellVersion, new object[]
						{
							psversion,
							moduleManifestPath,
							version3
						});
						InvalidOperationException exception4 = new InvalidOperationException(message);
						ErrorRecord errorRecord4 = new ErrorRecord(exception4, "Modules_InsufficientPowerShellVersion", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord4);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
			}
			string text7;
			if (!this.GetScalarFromData<string>(data, moduleManifestPath, "PowerShellHostName", manifestProcessingFlags, out text7))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.IgnoreHostNameAndHostVersion) == (ModuleCmdletBase.ManifestProcessingFlags)0 && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (text7 != null)
			{
				string name = base.Context.InternalHost.Name;
				if (!string.Equals(name, text7, StringComparison.OrdinalIgnoreCase))
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.IgnoreHostNameAndHostVersion) == (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							string message = StringUtil.Format(Modules.InvalidPowerShellHostName, new object[]
							{
								name,
								moduleManifestPath,
								text7
							});
							InvalidOperationException exception5 = new InvalidOperationException(message);
							ErrorRecord errorRecord5 = new ErrorRecord(exception5, "Modules_InvalidPowerShellHostName", ErrorCategory.ResourceUnavailable, moduleManifestPath);
							base.WriteError(errorRecord5);
						}
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							return null;
						}
					}
				}
			}
			Version version4;
			if (!this.GetScalarFromData<Version>(data, moduleManifestPath, "PowerShellHostVersion", manifestProcessingFlags, out version4))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.IgnoreHostNameAndHostVersion) == (ModuleCmdletBase.ManifestProcessingFlags)0 && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (version4 != null)
			{
				Version version5 = base.Context.InternalHost.Version;
				if (version5 < version4)
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.IgnoreHostNameAndHostVersion) == (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							string name2 = base.Context.InternalHost.Name;
							string message = StringUtil.Format(Modules.InvalidPowerShellHostVersion, new object[]
							{
								name2,
								version5,
								moduleManifestPath,
								version4
							});
							InvalidOperationException exception6 = new InvalidOperationException(message);
							ErrorRecord errorRecord6 = new ErrorRecord(exception6, "Modules_InsufficientPowerShellHostVersion", ErrorCategory.ResourceUnavailable, moduleManifestPath);
							base.WriteError(errorRecord6);
						}
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							return null;
						}
					}
				}
			}
			ProcessorArchitecture processorArchitecture;
			if (!this.GetScalarFromData<ProcessorArchitecture>(data, moduleManifestPath, "ProcessorArchitecture", manifestProcessingFlags, out processorArchitecture))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (processorArchitecture != ProcessorArchitecture.None && processorArchitecture != ProcessorArchitecture.MSIL)
			{
				bool flag2 = false;
				ProcessorArchitecture processorArchitecture2 = PsUtils.GetProcessorArchitecture(out flag2);
				if ((processorArchitecture2 != processorArchitecture && !flag2) || (flag2 && !processorArchitecture.ToString().Equals(PsUtils.ArmArchitecture, StringComparison.OrdinalIgnoreCase)))
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string text8 = flag2 ? PsUtils.ArmArchitecture : processorArchitecture2.ToString();
						string message = StringUtil.Format(Modules.InvalidProcessorArchitecture, new object[]
						{
							text8,
							moduleManifestPath,
							processorArchitecture
						});
						InvalidOperationException exception7 = new InvalidOperationException(message);
						ErrorRecord errorRecord7 = new ErrorRecord(exception7, "Modules_InvalidProcessorArchitecture", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord7);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
			}
			Version version6;
			if (!this.GetScalarFromData<Version>(data, moduleManifestPath, "CLRVersion", manifestProcessingFlags, out version6))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (version6 != null)
			{
				Version version7 = Environment.Version;
				if (version7 < version6)
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message = StringUtil.Format(Modules.ModuleManifestInsufficientCLRVersion, new object[]
						{
							version7,
							moduleManifestPath,
							version6
						});
						InvalidOperationException exception8 = new InvalidOperationException(message);
						ErrorRecord errorRecord8 = new ErrorRecord(exception8, "Modules_InsufficientCLRVersion", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord8);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
			}
			Version version8;
			if (!this.GetScalarFromData<Version>(data, moduleManifestPath, "DotNetFrameworkVersion", manifestProcessingFlags, out version8))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (version8 != null)
			{
				bool flag3 = false;
				if (!Utils.IsNetFrameworkVersionSupported(version8, out flag3))
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message = StringUtil.Format(Modules.InvalidDotNetFrameworkVersion, moduleManifestPath, version8);
						InvalidOperationException exception9 = new InvalidOperationException(message);
						ErrorRecord errorRecord9 = new ErrorRecord(exception9, "Modules_InsufficientDotNetFrameworkVersion", ErrorCategory.ResourceUnavailable, moduleManifestPath);
						base.WriteError(errorRecord9);
					}
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						return null;
					}
				}
				else if (flag3)
				{
					string text9 = StringUtil.Format(Modules.CannotDetectNetFrameworkVersion, version8);
					base.WriteVerbose(text9);
				}
			}
			string text10 = null;
			this.GetScalarFromData<string>(data, moduleManifestPath, "HelpInfoURI", manifestProcessingFlags, out text10);
			List<PSModuleInfo> list3 = new List<PSModuleInfo>();
			List<PSModuleInfo> list4 = new List<PSModuleInfo>();
			ModuleSpecification[] array;
			if (!this.GetScalarFromData<ModuleSpecification[]>(data, moduleManifestPath, "RequiredModules", manifestProcessingFlags, out array))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (array != null)
			{
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					PSModuleInfo psmoduleInfo2 = new PSModuleInfo(moduleManifestPath, base.Context, null);
					if (guid != null)
					{
						psmoduleInfo2.SetGuid(guid.Value);
					}
					if (version != null)
					{
						psmoduleInfo2.SetVersion(version);
					}
					foreach (ModuleSpecification requiredModule in array)
					{
						ErrorRecord errorRecord10 = null;
						PSModuleInfo psmoduleInfo3 = this.LoadRequiredModule(psmoduleInfo2, requiredModule, moduleManifestPath, manifestProcessingFlags, containedErrors, out errorRecord10);
						if (psmoduleInfo3 == null && errorRecord10 != null)
						{
							base.WriteError(errorRecord10);
							return null;
						}
						if (psmoduleInfo3 != null)
						{
							list3.Add(psmoduleInfo3);
						}
					}
				}
				else
				{
					foreach (ModuleSpecification moduleSpecification in array)
					{
						PSModuleInfo psmoduleInfo4 = new PSModuleInfo(moduleSpecification.Name, base.Context, null);
						if (moduleSpecification.Guid != null)
						{
							psmoduleInfo4.SetGuid(moduleSpecification.Guid.Value);
						}
						psmoduleInfo4.SetVersion(moduleSpecification.RequiredVersion ?? moduleSpecification.Version);
						list4.Add(psmoduleInfo4);
					}
				}
			}
			ModuleSpecification[] array3;
			if (!this.GetScalarFromData<ModuleSpecification[]>(data, moduleManifestPath, "NestedModules", manifestProcessingFlags, out array3))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			List<ModuleSpecification> list5 = new List<ModuleSpecification>();
			if (array3 != null && array3.Length > 0)
			{
				foreach (ModuleSpecification moduleSpecification2 in array3)
				{
					if (WildcardPattern.ContainsWildcardCharacters(moduleSpecification2.Name))
					{
						PSInvalidOperationException ex3 = PSTraceSource.NewInvalidOperationException(Modules.WildCardNotAllowedInModuleToProcessAndInNestedModules, new object[]
						{
							moduleManifestPath
						});
						ex3.SetErrorId("Modules_WildCardNotAllowedInModuleToProcessAndInNestedModules");
						throw ex3;
					}
					if (string.Equals(Path.GetExtension(moduleSpecification2.Name), ".xaml", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(moduleSpecification2.Name);
					}
					else
					{
						list5.Add(moduleSpecification2);
					}
				}
				Array.Clear(array3, 0, array3.Length);
			}
			object privateData = null;
			if (data.Contains("PrivateData"))
			{
				privateData = data["PrivateData"];
			}
			List<WildcardPattern> matchAll;
			if (!this.GetListOfWildcardsFromData(data, moduleManifestPath, "FunctionsToExport", manifestProcessingFlags, out matchAll))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			List<WildcardPattern> matchAll2;
			if (!this.GetListOfWildcardsFromData(data, moduleManifestPath, "VariablesToExport", manifestProcessingFlags, out matchAll2))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			List<WildcardPattern> matchAll3;
			if (!this.GetListOfWildcardsFromData(data, moduleManifestPath, "AliasesToExport", manifestProcessingFlags, out matchAll3))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			List<WildcardPattern> matchAll4;
			if (!this.GetListOfWildcardsFromData(data, moduleManifestPath, "CmdletsToExport", manifestProcessingFlags, out matchAll4))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			List<WildcardPattern> matchAll5;
			if (!this.GetListOfWildcardsFromData(data, moduleManifestPath, "DscResourcesToExport", manifestProcessingFlags, out matchAll5))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			InitialSessionState initialSessionState = null;
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				initialSessionState = InitialSessionState.Create();
				if (base.Context.InitialSessionState != null)
				{
					initialSessionState.DisableFormatUpdates = base.Context.InitialSessionState.DisableFormatUpdates;
				}
				initialSessionState.ThrowOnRunspaceOpenError = true;
			}
			bool flag4 = false;
			bool flag5 = false;
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			List<string> list8;
			if (!this.GetListOfStringsFromData(data, moduleManifestPath, "RequiredAssemblies", manifestProcessingFlags, out list8))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else
			{
				if (list8 != null && list8.Count > 0)
				{
					foreach (string text11 in list8)
					{
						if (string.Equals(Path.GetExtension(text11), ".xaml", StringComparison.OrdinalIgnoreCase))
						{
							list2.Add(text11);
						}
						else
						{
							list6.Add(text11);
						}
					}
				}
				if (list6 != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					foreach (string text12 in list6)
					{
						if (WildcardPattern.ContainsWildcardCharacters(text12))
						{
							PSInvalidOperationException ex4 = PSTraceSource.NewInvalidOperationException(Modules.WildCardNotAllowedInRequiredAssemblies, new object[]
							{
								moduleManifestPath
							});
							ex4.SetErrorId("Modules_WildCardNotAllowedInRequiredAssemblies");
							throw ex4;
						}
						string text13 = this.FixupFileName(directoryName, text12, ".dll");
						string text14 = StringUtil.Format(Modules.LoadingFile, "Assembly", text13);
						base.WriteVerbose(text14);
						initialSessionState.Assemblies.Add(new SessionStateAssemblyEntry(text12, text13));
						list7.Add(text13);
						flag4 = true;
					}
				}
			}
			List<string> list9;
			if (!this.GetListOfFilesFromData(data, moduleManifestPath, "TypesToProcess", manifestProcessingFlags, directoryName, ".ps1xml", true, out list9))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (list9 != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				foreach (string text15 in list9)
				{
					string text16 = StringUtil.Format(Modules.LoadingFile, "TypesToProcess", text15);
					base.WriteVerbose(text16);
					if (base.Context.RunspaceConfiguration != null)
					{
						base.Context.RunspaceConfiguration.Types.Append(new TypeConfigurationEntry(text15));
						flag5 = true;
					}
					else
					{
						bool flag6 = false;
						string value = ModuleCmdletBase.ResolveRootedFilePath(text15, base.Context) ?? text15;
						foreach (SessionStateTypeEntry sessionStateTypeEntry in ((IEnumerable<SessionStateTypeEntry>)base.Context.InitialSessionState.Types))
						{
							if (sessionStateTypeEntry.FileName != null)
							{
								string text17 = ModuleCmdletBase.ResolveRootedFilePath(sessionStateTypeEntry.FileName, base.Context) ?? sessionStateTypeEntry.FileName;
								if (text17.Equals(value, StringComparison.OrdinalIgnoreCase))
								{
									flag6 = true;
									break;
								}
							}
						}
						if (!flag6)
						{
							initialSessionState.Types.Add(new SessionStateTypeEntry(text15));
							flag4 = true;
						}
					}
				}
			}
			List<string> list10;
			if (!this.GetListOfFilesFromData(data, moduleManifestPath, "FormatsToProcess", manifestProcessingFlags, directoryName, ".ps1xml", true, out list10))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (list10 != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				foreach (string text18 in list10)
				{
					string text19 = StringUtil.Format(Modules.LoadingFile, "FormatsToProcess", text18);
					base.WriteVerbose(text19);
					if (base.Context.RunspaceConfiguration != null)
					{
						base.Context.RunspaceConfiguration.Formats.Append(new FormatConfigurationEntry(text18));
						flag5 = true;
					}
					else
					{
						bool flag7 = false;
						foreach (SessionStateFormatEntry sessionStateFormatEntry in ((IEnumerable<SessionStateFormatEntry>)base.Context.InitialSessionState.Formats))
						{
							if (sessionStateFormatEntry.FileName != null && sessionStateFormatEntry.FileName.Equals(text18, StringComparison.OrdinalIgnoreCase))
							{
								flag7 = true;
								break;
							}
						}
						if (!flag7)
						{
							initialSessionState.Formats.Add(new SessionStateFormatEntry(text18));
							flag4 = true;
						}
					}
				}
			}
			List<string> list11;
			if (!this.GetListOfFilesFromData(data, moduleManifestPath, "ScriptsToProcess", manifestProcessingFlags, directoryName, ".ps1", true, out list11))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			else if (list11 != null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				foreach (string text20 in list11)
				{
					if (!Path.GetExtension(text20).Equals(".ps1", StringComparison.OrdinalIgnoreCase))
					{
						string message2 = StringUtil.Format(Modules.ScriptsToProcessIncorrectExtension, text20);
						InvalidOperationException e = new InvalidOperationException(message2);
						ModuleCmdletBase.WriteInvalidManifestMemberError(this, "ScriptsToProcess", moduleManifestPath, e, manifestProcessingFlags);
						containedErrors = true;
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							return null;
						}
					}
				}
			}
			string text21 = string.Empty;
			if (data.Contains("Description"))
			{
				if (localizedData != null && localizedData.Contains("Description"))
				{
					text21 = (string)LanguagePrimitives.ConvertTo(localizedData["Description"], typeof(string), CultureInfo.InvariantCulture);
				}
				if (string.IsNullOrEmpty(text21))
				{
					text21 = (string)LanguagePrimitives.ConvertTo(data["Description"], typeof(string), CultureInfo.InvariantCulture);
				}
			}
			List<string> list12;
			if (!this.GetListOfFilesFromData(data, moduleManifestPath, "FileList", manifestProcessingFlags, directoryName, "", false, out list12))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			ModuleSpecification[] array4;
			if (!this.GetScalarFromData<ModuleSpecification[]>(data, moduleManifestPath, "ModuleList", manifestProcessingFlags, out array4))
			{
				containedErrors = true;
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					return null;
				}
			}
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				if (flag4)
				{
					try
					{
						initialSessionState.Bind(base.Context, true);
					}
					catch (Exception ex5)
					{
						CommandProcessorBase.CheckForSevereException(ex5);
						this.RemoveTypesAndFormatting(list10, list9);
						ErrorRecord errorRecord11 = new ErrorRecord(ex5, "FormatXmlUpdateException", ErrorCategory.InvalidOperation, null);
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							base.ThrowTerminatingError(errorRecord11);
						}
						else if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							base.WriteError(errorRecord11);
						}
					}
				}
				if (flag5)
				{
					try
					{
						base.Context.CurrentRunspace.RunspaceConfiguration.Types.Update(true);
						base.Context.CurrentRunspace.RunspaceConfiguration.Formats.Update(true);
					}
					catch (Exception ex6)
					{
						CommandProcessorBase.CheckForSevereException(ex6);
						this.RemoveTypesAndFormatting(list10, list9);
						ErrorRecord errorRecord12 = new ErrorRecord(ex6, "FormatXmlUpdateException", ErrorCategory.InvalidOperation, null);
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							base.ThrowTerminatingError(errorRecord12);
						}
						else if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							base.WriteError(errorRecord12);
						}
					}
				}
			}
			SessionState sessionState;
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				sessionState = new SessionState(base.Context, true, true);
			}
			else
			{
				sessionState = null;
			}
			PSModuleInfo psmoduleInfo5 = new PSModuleInfo(moduleManifestPath, base.Context, sessionState);
			psmoduleInfo5.SetModuleType(ModuleType.Manifest);
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				this.SetModuleLoggingInformation(psmoduleInfo5);
			}
			if (list4 != null && list4.Count > 0)
			{
				foreach (PSModuleInfo requiredModule2 in list4)
				{
					psmoduleInfo5.AddRequiredModule(requiredModule2);
				}
			}
			if (sessionState != null)
			{
				sessionState.Internal.SetVariable(SpecialVariables.PSScriptRootVarPath, Path.GetDirectoryName(moduleManifestPath), true, CommandOrigin.Internal);
				sessionState.Internal.SetVariable(SpecialVariables.PSCommandPathVarPath, moduleManifestPath, true, CommandOrigin.Internal);
				sessionState.Internal.Module = psmoduleInfo5;
				if (matchAll == null)
				{
					matchAll = this.MatchAll;
				}
				if (matchAll4 == null)
				{
					matchAll4 = this.MatchAll;
				}
				if (matchAll2 == null)
				{
					matchAll2 = this.MatchAll;
				}
				if (matchAll3 == null)
				{
					matchAll3 = this.MatchAll;
				}
				if (matchAll5 == null)
				{
					matchAll5 = this.MatchAll;
				}
			}
			psmoduleInfo5.Description = text21;
			psmoduleInfo5.PrivateData = privateData;
			psmoduleInfo5.SetExportedTypeFiles(new ReadOnlyCollection<string>(list9 ?? new List<string>()));
			psmoduleInfo5.SetExportedFormatFiles(new ReadOnlyCollection<string>(list10 ?? new List<string>()));
			psmoduleInfo5.SetVersion(version);
			psmoduleInfo5.Author = empty;
			psmoduleInfo5.CompanyName = empty2;
			psmoduleInfo5.Copyright = empty3;
			psmoduleInfo5.DotNetFrameworkVersion = version8;
			psmoduleInfo5.ClrVersion = version6;
			psmoduleInfo5.PowerShellHostName = text7;
			psmoduleInfo5.PowerShellHostVersion = version4;
			psmoduleInfo5.PowerShellVersion = version3;
			psmoduleInfo5.ProcessorArchitecture = processorArchitecture;
			psmoduleInfo5.Prefix = prefix;
			if (list6 != null)
			{
				foreach (string assembly in list6)
				{
					psmoduleInfo5.AddRequiredAssembly(assembly);
				}
			}
			if (list12 != null)
			{
				foreach (string file in list12)
				{
					psmoduleInfo5.AddToFileList(file);
				}
			}
			if (array4 != null)
			{
				foreach (ModuleSpecification m in array4)
				{
					psmoduleInfo5.AddToModuleList(m);
				}
			}
			if (list11 != null)
			{
				foreach (string s in list11)
				{
					psmoduleInfo5.AddScript(s);
				}
			}
			psmoduleInfo5.RootModule = text4;
			psmoduleInfo5.RootModuleForManifest = text4;
			if (guid != null)
			{
				psmoduleInfo5.SetGuid(guid.Value);
			}
			if (text10 != null)
			{
				psmoduleInfo5.SetHelpInfoUri(text10);
			}
			foreach (PSModuleInfo requiredModule3 in list3)
			{
				psmoduleInfo5.AddRequiredModule(requiredModule3);
			}
			if (array != null)
			{
				foreach (ModuleSpecification requiredModuleSpecification in array)
				{
					psmoduleInfo5.AddRequiredModuleSpecification(requiredModuleSpecification);
				}
			}
			string path = Path.Combine(directoryName, "PSGetModuleInfo.xml");
			if (File.Exists(path))
			{
				base.WriteVerbose(StringUtil.Format(Modules.PopulatingRepositorySourceLocation, psmoduleInfo5.Name));
				try
				{
					using (TextReader textReader = File.OpenText(path))
					{
						PSObject psobject = PSSerializer.Deserialize(textReader.ReadToEnd()) as PSObject;
						if (psobject != null && psobject.Properties["RepositorySourceLocation"] != null)
						{
							string text22 = psobject.Properties["RepositorySourceLocation"].Value.ToString();
							Uri repositorySourceLocation;
							if (!string.IsNullOrWhiteSpace(text22) && Uri.IsWellFormedUriString(text22, UriKind.RelativeOrAbsolute) && Uri.TryCreate(text22, UriKind.RelativeOrAbsolute, out repositorySourceLocation))
							{
								psmoduleInfo5.RepositorySourceLocation = repositorySourceLocation;
							}
						}
					}
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (ArgumentException)
				{
				}
				catch (PathTooLongException)
				{
				}
				catch (NotSupportedException)
				{
				}
				catch (InvalidOperationException)
				{
				}
				catch (XmlException)
				{
				}
			}
			bool flag8 = false;
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				if (matchAll != null)
				{
					psmoduleInfo5.DeclaredFunctionExports = new Collection<string>();
					if (matchAll.Count > 0)
					{
						foreach (WildcardPattern wildcardPattern in matchAll)
						{
							string pattern = wildcardPattern.Pattern;
							if (!WildcardPattern.ContainsWildcardCharacters(pattern))
							{
								psmoduleInfo5.DeclaredFunctionExports.Add(ModuleCmdletBase.AddPrefixToCommandName(pattern, text5));
							}
							else
							{
								flag8 = true;
							}
						}
						if (psmoduleInfo5.DeclaredFunctionExports.Count == 0)
						{
							psmoduleInfo5.DeclaredFunctionExports = null;
						}
					}
					else
					{
						flag8 = true;
					}
				}
				else
				{
					flag8 = true;
				}
				if (matchAll4 != null)
				{
					psmoduleInfo5.DeclaredCmdletExports = new Collection<string>();
					if (matchAll4.Count > 0)
					{
						foreach (WildcardPattern wildcardPattern2 in matchAll4)
						{
							string pattern2 = wildcardPattern2.Pattern;
							if (!WildcardPattern.ContainsWildcardCharacters(pattern2))
							{
								psmoduleInfo5.DeclaredCmdletExports.Add(ModuleCmdletBase.AddPrefixToCommandName(pattern2, text5));
							}
							else
							{
								flag8 = true;
							}
						}
						if (psmoduleInfo5.DeclaredCmdletExports.Count == 0)
						{
							psmoduleInfo5.DeclaredCmdletExports = null;
						}
					}
					else
					{
						flag8 = true;
					}
				}
				else
				{
					flag8 = true;
				}
				if (matchAll3 != null)
				{
					psmoduleInfo5.DeclaredAliasExports = new Collection<string>();
					if (matchAll3.Count > 0)
					{
						foreach (WildcardPattern wildcardPattern3 in matchAll3)
						{
							string pattern3 = wildcardPattern3.Pattern;
							if (!WildcardPattern.ContainsWildcardCharacters(pattern3))
							{
								psmoduleInfo5.DeclaredAliasExports.Add(ModuleCmdletBase.AddPrefixToCommandName(pattern3, text5));
							}
							else
							{
								flag8 = true;
							}
						}
						if (psmoduleInfo5.DeclaredAliasExports.Count == 0)
						{
							psmoduleInfo5.DeclaredAliasExports = null;
						}
					}
					else
					{
						flag8 = true;
					}
				}
				else
				{
					flag8 = true;
				}
				if (matchAll2 != null)
				{
					psmoduleInfo5.DeclaredVariableExports = new Collection<string>();
					if (matchAll2.Count > 0)
					{
						foreach (WildcardPattern wildcardPattern4 in matchAll2)
						{
							string pattern4 = wildcardPattern4.Pattern;
							if (!WildcardPattern.ContainsWildcardCharacters(pattern4))
							{
								psmoduleInfo5.DeclaredVariableExports.Add(pattern4);
							}
						}
						if (psmoduleInfo5.DeclaredVariableExports.Count == 0)
						{
							psmoduleInfo5.DeclaredVariableExports = null;
						}
					}
				}
				if (!flag8)
				{
					return psmoduleInfo5;
				}
			}
			if (list11 != null)
			{
				foreach (string fileName in list11)
				{
					bool flag9 = false;
					PSModuleInfo psmoduleInfo6 = this.LoadModule(fileName, directoryName, string.Empty, null, ref options, manifestProcessingFlags, out flag9);
					if (flag9 && sessionState == null)
					{
						foreach (string cmdlet in psmoduleInfo6.ExportedCmdlets.Keys)
						{
							psmoduleInfo5.AddDetectedCmdletExport(cmdlet);
						}
						foreach (string name3 in psmoduleInfo6.ExportedFunctions.Keys)
						{
							psmoduleInfo5.AddDetectedFunctionExport(name3);
						}
						foreach (string text23 in psmoduleInfo6.ExportedAliases.Keys)
						{
							psmoduleInfo5.AddDetectedAliasExport(text23, psmoduleInfo6.ExportedAliases[text23].Definition);
						}
					}
				}
			}
			if (list5 != null)
			{
				if (sessionState == null && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
				{
					containedErrors = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message3 = StringUtil.Format(Modules.ModuleManifestNestedModulesCantGoWithModuleToProcess, moduleManifestPath);
						ErrorRecord errorRecord13 = new ErrorRecord(new ArgumentException(message3), "Modules_BinaryModuleAndNestedModules", ErrorCategory.InvalidArgument, moduleManifestPath);
						base.WriteError(errorRecord13);
					}
				}
				bool basePassThru = this.BasePassThru;
				this.BasePassThru = false;
				List<WildcardPattern> baseVariablePatterns = this.BaseVariablePatterns;
				this.BaseVariablePatterns = this.MatchAll;
				List<WildcardPattern> baseFunctionPatterns = this.BaseFunctionPatterns;
				this.BaseFunctionPatterns = this.MatchAll;
				List<WildcardPattern> baseAliasPatterns = this.BaseAliasPatterns;
				this.BaseAliasPatterns = this.MatchAll;
				List<WildcardPattern> baseCmdletPatterns = this.BaseCmdletPatterns;
				this.BaseCmdletPatterns = this.MatchAll;
				bool baseDisableNameChecking = this.BaseDisableNameChecking;
				this.BaseDisableNameChecking = true;
				SessionStateInternal engineSessionState = base.Context.EngineSessionState;
				try
				{
					ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags2 = manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements;
					if (sessionState != null)
					{
						base.Context.EngineSessionState = sessionState.Internal;
					}
					ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
					foreach (ModuleSpecification moduleSpecification3 in list5)
					{
						bool flag10 = false;
						bool baseGlobal = this.BaseGlobal;
						this.BaseGlobal = false;
						string shortModuleName = null;
						if (moduleSpecification3.Name == this.ServiceCoreAssemblyFullName)
						{
							shortModuleName = this.ServiceCoreAssemblyShortName;
						}
						PSModuleInfo psmoduleInfo7;
						if (string.Equals(moduleSpecification3.Name, this.ServiceCoreAssemblyFullName, StringComparison.OrdinalIgnoreCase) || string.Equals(moduleSpecification3.Name, this.ServiceCoreAssemblyShortName, StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo7 = this.LoadServiceCoreModule(psmoduleInfo5, directoryName, null, importModuleOptions, manifestProcessingFlags, false, out flag10);
						}
						else
						{
							psmoduleInfo7 = this.LoadModuleNamedInManifest(psmoduleInfo5, moduleSpecification3, directoryName, true, string.Empty, null, importModuleOptions, manifestProcessingFlags, true, true, privateData, out flag10, shortModuleName);
						}
						this.BaseGlobal = baseGlobal;
						if (!flag10)
						{
							containedErrors = true;
							string message4 = StringUtil.Format(Modules.ManifestMemberNotFound, new object[]
							{
								moduleSpecification3.Name,
								"NestedModules",
								moduleManifestPath
							});
							FileNotFoundException innerException = new FileNotFoundException(message4);
							PSInvalidOperationException ex7 = new PSInvalidOperationException(message4, innerException, "Modules_ModuleFileNotFound", ErrorCategory.ResourceUnavailable, ModuleIntrinsics.GetModuleName(moduleManifestPath));
							throw ex7;
						}
						if (sessionState == null && psmoduleInfo7 != null && !string.Equals(psmoduleInfo7.Name, this.ServiceCoreAssemblyShortName, StringComparison.OrdinalIgnoreCase))
						{
							foreach (string cmdlet2 in psmoduleInfo7.ExportedCmdlets.Keys)
							{
								psmoduleInfo5.AddDetectedCmdletExport(cmdlet2);
							}
							foreach (string name4 in psmoduleInfo7.ExportedFunctions.Keys)
							{
								psmoduleInfo5.AddDetectedFunctionExport(name4);
							}
							foreach (string text24 in psmoduleInfo7.ExportedAliases.Keys)
							{
								psmoduleInfo5.AddDetectedAliasExport(text24, psmoduleInfo7.ExportedAliases[text24].Definition);
							}
						}
						if (psmoduleInfo7 != null)
						{
							psmoduleInfo5.AddNestedModule(psmoduleInfo7);
						}
					}
					if (flag)
					{
						psmoduleInfo5.SetModuleType(ModuleType.Workflow);
					}
					if (list != null && list.Count > 0)
					{
						scriptInfo.ValidateScriptInfo(base.Host);
						importModuleOptions.ServiceCoreAutoAdded = true;
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
						{
							this.ProcessWorkflowsToProcess(directoryName, list, new List<string>(), new List<string>(), null, psmoduleInfo5, importModuleOptions);
						}
						else
						{
							bool baseGlobal2 = this.BaseGlobal;
							this.BaseGlobal = false;
							bool flag11 = false;
							foreach (string text25 in list)
							{
								List<string> list13 = new List<string>();
								list13.Add(text25);
								SessionState sessionState2 = new SessionState(base.Context, true, true);
								PSModuleInfo psmoduleInfo8 = new PSModuleInfo(ModuleIntrinsics.GetModuleName(text25), text25, base.Context, sessionState2);
								sessionState2.Internal.Module = psmoduleInfo8;
								psmoduleInfo8.PrivateData = privateData;
								psmoduleInfo8.SetModuleType(ModuleType.Workflow);
								psmoduleInfo8.SetModuleBase(directoryName);
								this.LoadServiceCoreModule(psmoduleInfo8, string.Empty, sessionState2, importModuleOptions, manifestProcessingFlags, true, out flag11);
								this.ProcessWorkflowsToProcess(directoryName, list13, list2, list7, sessionState2, psmoduleInfo8, importModuleOptions);
								if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
								{
									this.ImportModuleMembers(psmoduleInfo8, this.BasePrefix, options);
								}
								ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo8);
								psmoduleInfo5.AddNestedModule(psmoduleInfo8);
							}
							this.BaseGlobal = baseGlobal2;
						}
					}
				}
				catch (Exception)
				{
					this.RemoveTypesAndFormatting(list10, list9);
					throw;
				}
				finally
				{
					base.Context.EngineSessionState = engineSessionState;
					this.BasePassThru = basePassThru;
					this.BaseVariablePatterns = baseVariablePatterns;
					this.BaseFunctionPatterns = baseFunctionPatterns;
					this.BaseAliasPatterns = baseAliasPatterns;
					this.BaseCmdletPatterns = baseCmdletPatterns;
					this.BaseDisableNameChecking = baseDisableNameChecking;
				}
			}
			if (text3 != null)
			{
				bool basePassThru2 = this.BasePassThru;
				this.BasePassThru = false;
				List<WildcardPattern> baseVariablePatterns2 = this.BaseVariablePatterns;
				this.BaseVariablePatterns = new List<WildcardPattern>();
				List<WildcardPattern> baseFunctionPatterns2 = this.BaseFunctionPatterns;
				this.BaseFunctionPatterns = new List<WildcardPattern>();
				List<WildcardPattern> baseAliasPatterns2 = this.BaseAliasPatterns;
				this.BaseAliasPatterns = new List<WildcardPattern>();
				List<WildcardPattern> baseCmdletPatterns2 = this.BaseCmdletPatterns;
				this.BaseCmdletPatterns = new List<WildcardPattern>();
				PSModuleInfo psmoduleInfo9;
				try
				{
					bool flag12 = false;
					psmoduleInfo9 = this.LoadModuleNamedInManifest(null, new ModuleSpecification(text3), directoryName, false, prefix, sessionState, options, manifestProcessingFlags, list9 == null || 0 == list9.Count, list10 == null || 0 == list10.Count, privateData, out flag12, null);
					if (!flag12 || psmoduleInfo9 == null)
					{
						containedErrors = true;
						string message5 = StringUtil.Format(Modules.ManifestMemberNotFound, new object[]
						{
							text3,
							"ModuleToProcess/RootModule",
							moduleManifestPath
						});
						FileNotFoundException innerException2 = new FileNotFoundException(message5);
						PSInvalidOperationException ex8 = new PSInvalidOperationException(message5, innerException2, "Modules_ModuleFileNotFound", ErrorCategory.ResourceUnavailable, ModuleIntrinsics.GetModuleName(moduleManifestPath));
						throw ex8;
					}
					ErrorRecord errorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario = this.GetErrorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario(data, moduleManifestPath, psmoduleInfo9.Path);
					if (errorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario != null)
					{
						containedErrors = true;
						this.RemoveModule(psmoduleInfo9);
						PSInvalidOperationException ex9 = new PSInvalidOperationException(errorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario.Exception.Message, errorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario.Exception, errorRecordIfUnsupportedRootCdxmlAndNestedModuleScenario.FullyQualifiedErrorId, ErrorCategory.InvalidOperation, moduleManifestPath);
						throw ex9;
					}
				}
				catch (Exception)
				{
					this.RemoveTypesAndFormatting(list10, list9);
					throw;
				}
				finally
				{
					this.BasePassThru = basePassThru2;
					this.BaseVariablePatterns = baseVariablePatterns2;
					this.BaseFunctionPatterns = baseFunctionPatterns2;
					this.BaseAliasPatterns = baseAliasPatterns2;
					this.BaseCmdletPatterns = baseCmdletPatterns2;
				}
				if (psmoduleInfo9.SessionState == null && sessionState != null)
				{
					psmoduleInfo9.SessionState = sessionState;
					sessionState.Internal.Module = psmoduleInfo9;
				}
				else if (psmoduleInfo9.SessionState != null && sessionState == null)
				{
					sessionState = psmoduleInfo9.SessionState;
				}
				psmoduleInfo9.SetName(psmoduleInfo5.Name);
				foreach (PSModuleInfo nestedModule in psmoduleInfo5.NestedModules)
				{
					psmoduleInfo9.AddNestedModule(nestedModule);
				}
				foreach (PSModuleInfo requiredModule4 in psmoduleInfo5.RequiredModules)
				{
					psmoduleInfo9.AddRequiredModule(requiredModule4);
				}
				psmoduleInfo9.SetVersion(psmoduleInfo5.Version);
				if (string.IsNullOrEmpty(psmoduleInfo9.Description))
				{
					psmoduleInfo9.Description = text21;
				}
				if (psmoduleInfo9.Version.Equals(new Version(0, 0)))
				{
					psmoduleInfo9.SetVersion(version);
				}
				if (psmoduleInfo9.Guid.Equals(Guid.Empty) && guid != null)
				{
					psmoduleInfo9.SetGuid(guid.Value);
				}
				if (psmoduleInfo9.HelpInfoUri == null && text10 != null)
				{
					psmoduleInfo9.SetHelpInfoUri(text10);
				}
				if (array != null)
				{
					foreach (ModuleSpecification requiredModuleSpecification2 in array)
					{
						psmoduleInfo9.AddRequiredModuleSpecification(requiredModuleSpecification2);
					}
				}
				if (psmoduleInfo9.RootModule == null)
				{
					psmoduleInfo9.RootModule = psmoduleInfo5.RootModule;
				}
				if (psmoduleInfo9.PrivateData == null)
				{
					psmoduleInfo9.PrivateData = psmoduleInfo5.PrivateData;
				}
				foreach (string tag in psmoduleInfo5.Tags)
				{
					psmoduleInfo9.AddToTags(tag);
				}
				psmoduleInfo9.ReleaseNotes = psmoduleInfo5.ReleaseNotes;
				psmoduleInfo9.ProjectUri = psmoduleInfo5.ProjectUri;
				psmoduleInfo9.LicenseUri = psmoduleInfo5.LicenseUri;
				psmoduleInfo9.IconUri = psmoduleInfo5.IconUri;
				psmoduleInfo9.RepositorySourceLocation = psmoduleInfo5.RepositorySourceLocation;
				if (sessionState == null)
				{
					psmoduleInfo9.Path = psmoduleInfo5.Path;
				}
				if (string.IsNullOrEmpty(psmoduleInfo9.Author))
				{
					psmoduleInfo9.Author = empty;
				}
				if (string.IsNullOrEmpty(psmoduleInfo9.CompanyName))
				{
					psmoduleInfo9.CompanyName = empty2;
				}
				if (string.IsNullOrEmpty(psmoduleInfo9.Copyright))
				{
					psmoduleInfo9.Copyright = empty3;
				}
				if (psmoduleInfo9.PowerShellVersion == null)
				{
					psmoduleInfo9.PowerShellVersion = version3;
				}
				if (string.IsNullOrEmpty(psmoduleInfo9.PowerShellHostName))
				{
					psmoduleInfo9.PowerShellHostName = text7;
				}
				if (psmoduleInfo9.PowerShellHostVersion == null)
				{
					psmoduleInfo9.PowerShellHostVersion = version4;
				}
				if (psmoduleInfo9.DotNetFrameworkVersion == null)
				{
					psmoduleInfo9.DotNetFrameworkVersion = version8;
				}
				if (psmoduleInfo9.ClrVersion == null)
				{
					psmoduleInfo9.ClrVersion = version6;
				}
				if (string.IsNullOrEmpty(psmoduleInfo9.Prefix))
				{
					psmoduleInfo9.Prefix = prefix;
				}
				if ((psmoduleInfo9.FileList == null || psmoduleInfo9.FileList.LongCount<string>() == 0L) && list12 != null)
				{
					foreach (string file2 in list12)
					{
						psmoduleInfo9.AddToFileList(file2);
					}
				}
				if ((psmoduleInfo9.ModuleList == null || psmoduleInfo9.ModuleList.LongCount<object>() == 0L) && array4 != null)
				{
					foreach (ModuleSpecification m2 in array4)
					{
						psmoduleInfo9.AddToModuleList(m2);
					}
				}
				if (psmoduleInfo9.ProcessorArchitecture == ProcessorArchitecture.None)
				{
					psmoduleInfo9.ProcessorArchitecture = processorArchitecture;
				}
				if ((psmoduleInfo9.RequiredAssemblies == null || psmoduleInfo9.RequiredAssemblies.LongCount<string>() == 0L) && list6 != null)
				{
					foreach (string assembly2 in list6)
					{
						psmoduleInfo9.AddRequiredAssembly(assembly2);
					}
				}
				if ((psmoduleInfo9.Scripts == null || psmoduleInfo9.Scripts.LongCount<string>() == 0L) && list11 != null)
				{
					foreach (string s2 in list11)
					{
						psmoduleInfo9.AddScript(s2);
					}
				}
				if (psmoduleInfo9.RootModuleForManifest == null)
				{
					psmoduleInfo9.RootModuleForManifest = psmoduleInfo5.RootModuleForManifest;
				}
				if (psmoduleInfo9.DeclaredCmdletExports == null || psmoduleInfo9.DeclaredCmdletExports.Count == 0)
				{
					psmoduleInfo9.DeclaredCmdletExports = psmoduleInfo5.DeclaredCmdletExports;
				}
				if (psmoduleInfo5._detectedCmdletExports != null)
				{
					foreach (string cmdlet3 in psmoduleInfo5._detectedCmdletExports)
					{
						psmoduleInfo9.AddDetectedCmdletExport(cmdlet3);
					}
				}
				if (psmoduleInfo9.DeclaredFunctionExports == null || psmoduleInfo9.DeclaredFunctionExports.Count == 0)
				{
					psmoduleInfo9.DeclaredFunctionExports = psmoduleInfo5.DeclaredFunctionExports;
				}
				if (psmoduleInfo5._detectedFunctionExports != null)
				{
					foreach (string name5 in psmoduleInfo5._detectedFunctionExports)
					{
						psmoduleInfo9.AddDetectedFunctionExport(name5);
					}
				}
				if (psmoduleInfo9.DeclaredAliasExports == null || psmoduleInfo9.DeclaredAliasExports.Count == 0)
				{
					psmoduleInfo9.DeclaredAliasExports = psmoduleInfo5.DeclaredAliasExports;
				}
				if (psmoduleInfo5._detectedAliasExports != null)
				{
					foreach (string text26 in psmoduleInfo5._detectedAliasExports.Keys)
					{
						psmoduleInfo9.AddDetectedAliasExport(text26, psmoduleInfo5._detectedAliasExports[text26]);
					}
				}
				if (psmoduleInfo9.DeclaredVariableExports == null || psmoduleInfo9.DeclaredVariableExports.Count == 0)
				{
					psmoduleInfo9.DeclaredVariableExports = psmoduleInfo5.DeclaredVariableExports;
				}
				if (psmoduleInfo5._detectedWorkflowExports != null)
				{
					foreach (string name6 in psmoduleInfo5._detectedWorkflowExports)
					{
						psmoduleInfo9.AddDetectedWorkflowExport(name6);
					}
				}
				if (psmoduleInfo9.DeclaredWorkflowExports == null || psmoduleInfo9.DeclaredWorkflowExports.Count == 0)
				{
					psmoduleInfo9.DeclaredWorkflowExports = psmoduleInfo5.DeclaredWorkflowExports;
				}
				if (psmoduleInfo5.ExportedTypeFiles.Count > 0)
				{
					psmoduleInfo9.SetExportedTypeFiles(psmoduleInfo5.ExportedTypeFiles);
				}
				if (psmoduleInfo5.ExportedFormatFiles.Count > 0)
				{
					psmoduleInfo9.SetExportedFormatFiles(psmoduleInfo5.ExportedFormatFiles);
				}
				psmoduleInfo5 = psmoduleInfo9;
				if (psmoduleInfo5.ModuleType == ModuleType.Binary)
				{
					if (matchAll4 != null && sessionState != null)
					{
						psmoduleInfo5.ExportedCmdlets.Clear();
						if (sessionState != null)
						{
							ModuleIntrinsics.ExportModuleMembers(this, sessionState.Internal, matchAll, matchAll4, matchAll3, matchAll2, null);
						}
					}
				}
				else
				{
					if (sessionState != null && !sessionState.Internal.UseExportList)
					{
						ModuleIntrinsics.ExportModuleMembers(this, sessionState.Internal, this.MatchAll, this.MatchAll, null, null, options.ServiceCoreAutoAdded ? ModuleCmdletBase.ServiceCoreAssemblyCmdlets : null);
					}
					if (matchAll != null)
					{
						if (sessionState != null)
						{
							ModuleCmdletBase.UpdateCommandCollection<FunctionInfo>(sessionState.Internal.ExportedFunctions, matchAll);
						}
						else
						{
							Collection<string> collection = new Collection<string>();
							if (psmoduleInfo5.DeclaredFunctionExports != null)
							{
								foreach (string item in psmoduleInfo5.DeclaredFunctionExports)
								{
									collection.Add(item);
								}
							}
							ModuleCmdletBase.UpdateCommandCollection(collection, matchAll);
						}
					}
					if (matchAll4 != null)
					{
						if (sessionState != null)
						{
							ModuleCmdletBase.UpdateCommandCollection<CmdletInfo>(psmoduleInfo5.CompiledExports, matchAll4);
						}
						else
						{
							ModuleCmdletBase.UpdateCommandCollection(psmoduleInfo5.DeclaredCmdletExports, matchAll4);
						}
					}
					if (matchAll3 != null)
					{
						if (sessionState != null)
						{
							ModuleCmdletBase.UpdateCommandCollection<AliasInfo>(sessionState.Internal.ExportedAliases, matchAll3);
						}
						else
						{
							ModuleCmdletBase.UpdateCommandCollection(psmoduleInfo5.DeclaredAliasExports, matchAll3);
						}
					}
					if (matchAll2 != null && sessionState != null)
					{
						List<PSVariable> list14 = new List<PSVariable>();
						foreach (PSVariable psvariable in sessionState.Internal.ExportedVariables)
						{
							if (SessionStateUtilities.MatchesAnyWildcardPattern(psvariable.Name, matchAll2, false))
							{
								list14.Add(psvariable);
							}
						}
						sessionState.Internal.ExportedVariables.Clear();
						sessionState.Internal.ExportedVariables.AddRange(list14);
					}
				}
			}
			else if (sessionState != null)
			{
				ModuleIntrinsics.ExportModuleMembers(this, sessionState.Internal, matchAll, matchAll4, matchAll3, matchAll2, options.ServiceCoreAutoAdded ? ModuleCmdletBase.ServiceCoreAssemblyCmdlets : null);
			}
			ModuleCmdletBase.SetDeclaredDscResources(matchAll5, psmoduleInfo5);
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				this.ImportModuleMembers(psmoduleInfo5, prefix, options);
			}
			return psmoduleInfo5;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002C4BC File Offset: 0x0002A6BC
		private static void SetDeclaredDscResources(List<WildcardPattern> exportedDscResources, PSModuleInfo manifestInfo)
		{
			if (exportedDscResources != null)
			{
				manifestInfo._declaredDscResourceExports = new Collection<string>();
				if (exportedDscResources.Count > 0)
				{
					foreach (WildcardPattern wildcardPattern in exportedDscResources)
					{
						string pattern = wildcardPattern.Pattern;
						if (!WildcardPattern.ContainsWildcardCharacters(pattern))
						{
							manifestInfo._declaredDscResourceExports.Add(pattern);
						}
					}
				}
			}
			if (manifestInfo._declaredDscResourceExports != null && manifestInfo._declaredDscResourceExports.Count == 0)
			{
				ReadOnlyDictionary<string, TypeDefinitionAst> exportedTypeDefinitions = manifestInfo.GetExportedTypeDefinitions();
				IEnumerable<TypeDefinitionAst> enumerable = exportedTypeDefinitions.Values.Where(delegate(TypeDefinitionAst typeAst)
				{
					for (int i = 0; i < typeAst.Attributes.Count; i++)
					{
						AttributeAst attributeAst = typeAst.Attributes[i];
						if (attributeAst.TypeName.GetReflectionAttributeType() == typeof(DscResourceAttribute))
						{
							return true;
						}
					}
					return false;
				});
				foreach (TypeDefinitionAst typeDefinitionAst in enumerable)
				{
					manifestInfo._declaredDscResourceExports.Add(typeDefinitionAst.Name);
				}
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002C5C4 File Offset: 0x0002A7C4
		private PSModuleInfo LoadServiceCoreModule(PSModuleInfo parentModule, string moduleBase, SessionState ss, ModuleCmdletBase.ImportModuleOptions nestedModuleOptions, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, bool addToParentModueIfFound, out bool found)
		{
			SessionStateInternal engineSessionState = base.Context.EngineSessionState;
			if (ss != null)
			{
				base.Context.EngineSessionState = ss.Internal;
			}
			PSModuleInfo result;
			try
			{
				found = false;
				bool baseGlobal = this.BaseGlobal;
				this.BaseGlobal = false;
				PSModuleInfo psmoduleInfo = this.LoadBinaryModule(parentModule, false, this.ServiceCoreAssemblyFullName, null, null, moduleBase, ss, nestedModuleOptions, manifestProcessingFlags, string.Empty, true, true, out found, this.ServiceCoreAssemblyShortName, true);
				this.BaseGlobal = baseGlobal;
				if (!found)
				{
					string message = StringUtil.Format(Modules.ManifestMemberNotFound, new object[]
					{
						this.ServiceCoreAssemblyFullName,
						"NestedModules",
						parentModule.Name
					});
					FileNotFoundException innerException = new FileNotFoundException(message);
					PSInvalidOperationException ex = new PSInvalidOperationException(message, innerException, "Modules_ModuleFileNotFound", ErrorCategory.ResourceUnavailable, parentModule.Name);
					throw ex;
				}
				if (addToParentModueIfFound)
				{
					parentModule.AddNestedModule(psmoduleInfo);
				}
				result = psmoduleInfo;
			}
			finally
			{
				base.Context.EngineSessionState = engineSessionState;
			}
			return result;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002C6BC File Offset: 0x0002A8BC
		private void ProcessWorkflowsToProcess(string moduleBase, List<string> workflowsToProcess, List<string> dependentWorkflows, List<string> assemblyList, SessionState ss, PSModuleInfo manifestInfo, ModuleCmdletBase.ImportModuleOptions options)
		{
			if (ss != null)
			{
				if (workflowsToProcess == null || workflowsToProcess.Count <= 0)
				{
					return;
				}
				if ((SystemPolicy.GetSystemLockdownPolicy() == SystemEnforcementMode.Enforce || base.Context.LanguageMode == PSLanguageMode.ConstrainedLanguage) && !SystemPolicy.XamlWorkflowSupported)
				{
					throw new NotSupportedException(Modules.XamlWorkflowsNotSupported);
				}
				SessionStateInternal engineSessionState = base.Context.EngineSessionState;
				try
				{
					base.Context.EngineSessionState = ss.Internal;
					if (dependentWorkflows != null && dependentWorkflows.Count > 0)
					{
						ScriptBlock scriptBlock = ScriptBlock.Create(base.Context, "param($files, $dependentFiles, $assemblyList) Microsoft.PowerShell.Workflow.ServiceCore\\Import-PSWorkflow -Path \"$files\" -DependentWorkflow $dependentFiles -DependentAssemblies $assemblyList -Force:$" + this.BaseForce);
						List<string> list = new List<string>(this.ResolveDependentWorkflowFiles(moduleBase, dependentWorkflows));
						using (IEnumerator<string> enumerator = this.ResolveWorkflowFiles(moduleBase, workflowsToProcess).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string text = enumerator.Current;
								base.WriteVerbose(StringUtil.Format(Modules.LoadingWorkflow, text));
								scriptBlock.Invoke(new object[]
								{
									text,
									list.ToArray(),
									assemblyList.ToArray()
								});
							}
							goto IL_18C;
						}
					}
					ScriptBlock scriptBlock2 = ScriptBlock.Create(base.Context, "param($files, $dependentFiles) Microsoft.PowerShell.Workflow.ServiceCore\\Import-PSWorkflow -Path \"$files\" -Force:$" + this.BaseForce);
					foreach (string text2 in this.ResolveWorkflowFiles(moduleBase, workflowsToProcess))
					{
						base.WriteVerbose(StringUtil.Format(Modules.LoadingWorkflow, text2));
						scriptBlock2.Invoke(new object[]
						{
							text2
						});
					}
					IL_18C:
					ModuleIntrinsics.ExportModuleMembers(this, ss.Internal, this.MatchAll, this.MatchAll, this.MatchAll, this.MatchAll, options.ServiceCoreAutoAdded ? ModuleCmdletBase.ServiceCoreAssemblyCmdlets : null);
					return;
				}
				finally
				{
					base.Context.EngineSessionState = engineSessionState;
				}
			}
			if (workflowsToProcess != null)
			{
				foreach (string path in workflowsToProcess)
				{
					manifestInfo.AddDetectedWorkflowExport(Path.GetFileNameWithoutExtension(path));
				}
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002C93C File Offset: 0x0002AB3C
		private ICollection<string> ResolveWorkflowFiles(string moduleBase, List<string> workflowsToProcess)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in workflowsToProcess)
			{
				if (string.Equals(Path.GetExtension(text), ".xaml", StringComparison.OrdinalIgnoreCase))
				{
					string text2 = text;
					if (!Path.IsPathRooted(text2))
					{
						text2 = Path.Combine(moduleBase, text2);
					}
					using (IEnumerator<string> enumerator2 = base.Context.SessionState.Path.GetResolvedProviderPathFromProviderPath(text2, base.Context.ProviderNames.FileSystem).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							string text3 = enumerator2.Current;
							dictionary[text3] = text3;
						}
						continue;
					}
				}
				PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(Modules.InvalidWorkflowExtensionDuringManifestProcessing, new object[]
				{
					text
				});
				ex.SetErrorId("Modules_InvalidWorkflowExtensionDuringManifestProcessing");
				throw ex;
			}
			return dictionary.Values;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002CA48 File Offset: 0x0002AC48
		private ICollection<string> ResolveDependentWorkflowFiles(string moduleBase, List<string> dependentWorkflowsToProcess)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (dependentWorkflowsToProcess.Count == 1 && string.Equals(Path.GetExtension(dependentWorkflowsToProcess[0]), ".dll", StringComparison.OrdinalIgnoreCase))
			{
				string text = dependentWorkflowsToProcess[0];
				if (!Path.IsPathRooted(text))
				{
					text = Path.Combine(moduleBase, text);
				}
				dictionary[text] = text;
			}
			else
			{
				foreach (string text2 in dependentWorkflowsToProcess)
				{
					if (string.Equals(Path.GetExtension(text2), ".xaml", StringComparison.OrdinalIgnoreCase))
					{
						string text3 = text2;
						if (!Path.IsPathRooted(text3))
						{
							text3 = Path.Combine(moduleBase, text3);
						}
						using (IEnumerator<string> enumerator2 = base.Context.SessionState.Path.GetResolvedProviderPathFromProviderPath(text3, base.Context.ProviderNames.FileSystem).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string text4 = enumerator2.Current;
								dictionary[text4] = text4;
							}
							continue;
						}
					}
					PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(Modules.InvalidWorkflowExtensionDuringManifestProcessing, new object[]
					{
						text2
					});
					ex.SetErrorId("Modules_InvalidWorkflowExtensionDuringManifestProcessing");
					throw ex;
				}
			}
			return dictionary.Values;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		private static void UpdateCommandCollection<T>(List<T> list, List<WildcardPattern> patterns) where T : CommandInfo
		{
			List<T> list2 = new List<T>();
			foreach (T item in list)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(item.Name, patterns, false))
				{
					list2.Add(item);
				}
			}
			list.Clear();
			list.AddRange(list2);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002CC18 File Offset: 0x0002AE18
		private static void UpdateCommandCollection(Collection<string> list, List<WildcardPattern> patterns)
		{
			if (list == null)
			{
				return;
			}
			List<string> list2 = new List<string>();
			foreach (WildcardPattern wildcardPattern in patterns)
			{
				if (!WildcardPattern.ContainsWildcardCharacters(wildcardPattern.Pattern) && !list.Contains(wildcardPattern.Pattern, StringComparer.OrdinalIgnoreCase))
				{
					list.Add(wildcardPattern.Pattern);
				}
			}
			foreach (string text in list)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(text, patterns, false))
				{
					list2.Add(text);
				}
			}
			list.Clear();
			foreach (string item in list2)
			{
				list.Add(item);
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002CD20 File Offset: 0x0002AF20
		private static void WriteInvalidManifestMemberError(PSCmdlet cmdlet, string manifestElement, string moduleManifestPath, Exception e, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags)
		{
			CommandProcessorBase.CheckForSevereException(e);
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
			{
				ErrorRecord errorRecord = ModuleCmdletBase.GenerateInvalidModuleMemberErrorRecord(manifestElement, moduleManifestPath, e);
				cmdlet.WriteError(errorRecord);
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002CD4C File Offset: 0x0002AF4C
		private static ErrorRecord GenerateInvalidModuleMemberErrorRecord(string manifestElement, string moduleManifestPath, Exception e)
		{
			string message = StringUtil.Format(Modules.ModuleManifestInvalidManifestMember, new object[]
			{
				manifestElement,
				e.Message,
				moduleManifestPath
			});
			ArgumentException exception = new ArgumentException(message);
			return new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002CD94 File Offset: 0x0002AF94
		internal static object IsModuleLoaded(ExecutionContext context, ModuleSpecification requiredModule, out bool wrongVersion, out bool wrongGuid, out bool loaded)
		{
			loaded = false;
			object obj = null;
			wrongVersion = false;
			wrongGuid = false;
			string name = requiredModule.Name;
			Guid? guid = requiredModule.Guid;
			foreach (PSModuleInfo psmoduleInfo in context.Modules.GetModules(new string[]
			{
				"*"
			}, false))
			{
				if (name.Equals(psmoduleInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					if (guid == null || guid.Value.Equals(psmoduleInfo.Guid))
					{
						if (requiredModule.RequiredVersion != null)
						{
							if (requiredModule.RequiredVersion.Equals(psmoduleInfo.Version))
							{
								obj = psmoduleInfo;
								loaded = true;
								break;
							}
							wrongVersion = true;
						}
						else if (requiredModule.Version != null)
						{
							if (requiredModule.MaximumVersion != null)
							{
								if (ModuleCmdletBase.GetMaximumVersion(requiredModule.MaximumVersion) >= psmoduleInfo.Version && requiredModule.Version <= psmoduleInfo.Version)
								{
									obj = psmoduleInfo;
									loaded = true;
									break;
								}
								wrongVersion = true;
							}
							else
							{
								if (requiredModule.Version <= psmoduleInfo.Version)
								{
									obj = psmoduleInfo;
									loaded = true;
									break;
								}
								wrongVersion = true;
							}
						}
						else
						{
							if (requiredModule.MaximumVersion == null)
							{
								obj = psmoduleInfo;
								loaded = true;
								break;
							}
							if (ModuleCmdletBase.GetMaximumVersion(requiredModule.MaximumVersion) >= psmoduleInfo.Version)
							{
								obj = psmoduleInfo;
								loaded = true;
								break;
							}
							wrongVersion = true;
						}
					}
					else
					{
						wrongGuid = true;
					}
				}
			}
			if (obj == null && InitialSessionState.IsEngineModule(requiredModule.Name))
			{
				using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
				{
					Collection<PSSnapInInfo> collection = null;
					powerShell.AddCommand("Get-PSSnapin");
					powerShell.AddParameter("Name", requiredModule.Name);
					try
					{
						collection = powerShell.Invoke<PSSnapInInfo>();
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
					if (collection != null && collection.Count > 0)
					{
						obj = collection[0];
						loaded = true;
					}
				}
			}
			return obj;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		internal PSModuleInfo LoadRequiredModule(PSModuleInfo currentModule, ModuleSpecification requiredModule, string moduleManifestPath, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, bool containedErrors, out ErrorRecord error)
		{
			error = null;
			if (!containedErrors)
			{
				return ModuleCmdletBase.LoadRequiredModule(base.Context, currentModule, requiredModule, moduleManifestPath, manifestProcessingFlags, out error);
			}
			return null;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002D004 File Offset: 0x0002B204
		internal static PSModuleInfo LoadRequiredModule(ExecutionContext context, PSModuleInfo currentModule, ModuleSpecification requiredModuleSpecification, string moduleManifestPath, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out ErrorRecord error)
		{
			error = null;
			string name = requiredModuleSpecification.Name;
			Guid? guid = requiredModuleSpecification.Guid;
			PSModuleInfo result = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			object obj = ModuleCmdletBase.IsModuleLoaded(context, requiredModuleSpecification, out flag, out flag2, out flag3);
			if (obj == null)
			{
				Collection<PSModuleInfo> moduleIfAvailable = ModuleCmdletBase.GetModuleIfAvailable(requiredModuleSpecification, null);
				if (moduleIfAvailable != null && moduleIfAvailable.Count > 0)
				{
					PSModuleInfo psmoduleInfo = moduleIfAvailable[0];
					Dictionary<ModuleSpecification, List<ModuleSpecification>> dictionary = new Dictionary<ModuleSpecification, List<ModuleSpecification>>(new ModuleSpecificationComparer());
					if (currentModule != null)
					{
						dictionary.Add(new ModuleSpecification(currentModule), new List<ModuleSpecification>
						{
							requiredModuleSpecification
						});
					}
					if (!ModuleCmdletBase.HasRequiredModulesCyclicReference(psmoduleInfo.Name, new List<ModuleSpecification>(psmoduleInfo.RequiredModulesSpecification), new Collection<PSModuleInfo>
					{
						psmoduleInfo
					}, dictionary, out error))
					{
						result = ModuleCmdletBase.ImportRequiredModule(context, requiredModuleSpecification, out error);
					}
					else if (moduleManifestPath != null)
					{
						MissingMemberException exception = null;
						if (error != null && error.Exception != null)
						{
							exception = new MissingMemberException(error.Exception.Message);
						}
						error = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
					}
				}
				else if (moduleManifestPath != null)
				{
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
					{
						string message;
						if (flag)
						{
							if (requiredModuleSpecification.RequiredVersion != null)
							{
								message = StringUtil.Format(Modules.RequiredModuleNotLoadedWrongVersion, new object[]
								{
									moduleManifestPath,
									name,
									requiredModuleSpecification.RequiredVersion
								});
							}
							else if (requiredModuleSpecification.Version != null && requiredModuleSpecification.MaximumVersion == null)
							{
								message = StringUtil.Format(Modules.RequiredModuleNotLoadedWrongVersion, new object[]
								{
									moduleManifestPath,
									name,
									requiredModuleSpecification.Version
								});
							}
							else if (requiredModuleSpecification.Version == null && requiredModuleSpecification.MaximumVersion != null)
							{
								message = StringUtil.Format(Modules.RequiredModuleNotLoadedWrongMaximumVersion, new object[]
								{
									moduleManifestPath,
									name,
									requiredModuleSpecification.MaximumVersion
								});
							}
							else
							{
								message = StringUtil.Format(Modules.RequiredModuleNotLoadedWrongMinimumVersionAndMaximumVersion, new object[]
								{
									moduleManifestPath,
									name,
									requiredModuleSpecification.Version,
									requiredModuleSpecification.MaximumVersion
								});
							}
						}
						else if (flag2)
						{
							message = StringUtil.Format(Modules.RequiredModuleNotLoadedWrongGuid, new object[]
							{
								moduleManifestPath,
								name,
								guid.Value
							});
						}
						else
						{
							message = StringUtil.Format(Modules.RequiredModuleNotLoaded, moduleManifestPath, name);
						}
						MissingMemberException exception2 = new MissingMemberException(message);
						error = new ErrorRecord(exception2, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
					}
				}
				else
				{
					string message = StringUtil.Format(Modules.RequiredModuleNotFound, requiredModuleSpecification.Name);
					MissingMemberException exception3 = new MissingMemberException(message);
					error = new ErrorRecord(exception3, "Modules_RequiredModuleNotFound", ErrorCategory.ResourceUnavailable, null);
				}
			}
			else if (obj is PSModuleInfo)
			{
				result = (PSModuleInfo)obj;
			}
			return result;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0002D2DC File Offset: 0x0002B4DC
		private static PSModuleInfo ImportRequiredModule(ExecutionContext context, ModuleSpecification requiredModule, out ErrorRecord error)
		{
			error = null;
			PSModuleInfo psmoduleInfo = null;
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				powerShell.AddCommand("Import-Module");
				powerShell.AddParameter("Name", requiredModule.Name);
				if (requiredModule.RequiredVersion != null)
				{
					powerShell.AddParameter("RequiredVersion", requiredModule.RequiredVersion);
				}
				else if (requiredModule.MaximumVersion != null)
				{
					powerShell.AddParameter("MaximumVersion", requiredModule.MaximumVersion);
				}
				else
				{
					powerShell.AddParameter("Version", requiredModule.Version);
				}
				powerShell.Invoke();
				if (powerShell.Streams.Error != null && powerShell.Streams.Error.Count > 0)
				{
					error = powerShell.Streams.Error[0];
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					string text = requiredModule.Name;
					string text2 = string.Empty;
					if (text.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
					{
						text2 = text;
						text = Path.GetFileNameWithoutExtension(text);
					}
					ModuleSpecification moduleSpecification = new ModuleSpecification(text);
					if (requiredModule.Guid != null)
					{
						moduleSpecification.Guid = new Guid?(requiredModule.Guid.Value);
					}
					if (requiredModule.RequiredVersion != null)
					{
						moduleSpecification.RequiredVersion = requiredModule.RequiredVersion;
					}
					if (requiredModule.Version != null)
					{
						moduleSpecification.Version = requiredModule.Version;
					}
					if (requiredModule.MaximumVersion != null)
					{
						moduleSpecification.MaximumVersion = requiredModule.MaximumVersion;
					}
					bool flag3 = false;
					object obj = ModuleCmdletBase.IsModuleLoaded(context, moduleSpecification, out flag, out flag2, out flag3);
					psmoduleInfo = (obj as PSModuleInfo);
					if (psmoduleInfo == null)
					{
						string message = StringUtil.Format(Modules.RequiredModuleNotFound, text);
						if (!string.IsNullOrEmpty(text2))
						{
							MissingMemberException exception = new MissingMemberException(message);
							error = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, text2);
						}
						else
						{
							InvalidOperationException exception2 = new InvalidOperationException(message);
							error = new ErrorRecord(exception2, "Modules_RequiredModuleNotLoadedWithoutManifest", ErrorCategory.InvalidOperation, requiredModule);
						}
					}
				}
			}
			return psmoduleInfo;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0002D4E8 File Offset: 0x0002B6E8
		internal bool VerifyIfNestedModuleIsAvailable(ModuleSpecification nestedModuleSpec, string rootedModulePath, string extension, out PSModuleInfo nestedModuleInfoIfAvailable)
		{
			nestedModuleInfoIfAvailable = null;
			if (nestedModuleSpec.Guid != null || nestedModuleSpec.Version != null || nestedModuleSpec.RequiredVersion != null || nestedModuleSpec.MaximumVersion != null)
			{
				if (!string.IsNullOrEmpty(extension) && !string.Equals(extension, ".psd1"))
				{
					return false;
				}
				string text = rootedModulePath;
				if (string.IsNullOrEmpty(extension))
				{
					text = rootedModulePath + ".psd1";
				}
				ModuleSpecification moduleSpecification = new ModuleSpecification(string.IsNullOrEmpty(rootedModulePath) ? nestedModuleSpec.Name : text);
				if (nestedModuleSpec.Guid != null)
				{
					moduleSpecification.Guid = new Guid?(nestedModuleSpec.Guid.Value);
				}
				if (nestedModuleSpec.Version != null)
				{
					moduleSpecification.Version = nestedModuleSpec.Version;
				}
				if (nestedModuleSpec.RequiredVersion != null)
				{
					moduleSpecification.RequiredVersion = nestedModuleSpec.RequiredVersion;
				}
				if (nestedModuleSpec.MaximumVersion != null)
				{
					moduleSpecification.MaximumVersion = nestedModuleSpec.MaximumVersion;
				}
				Collection<PSModuleInfo> moduleIfAvailable = ModuleCmdletBase.GetModuleIfAvailable(moduleSpecification, null);
				if (moduleIfAvailable.Count < 1)
				{
					return false;
				}
				nestedModuleInfoIfAvailable = moduleIfAvailable[0];
			}
			return true;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0002D608 File Offset: 0x0002B808
		internal static Collection<PSModuleInfo> GetModuleIfAvailable(ModuleSpecification requiredModule, Runspace rsToUse = null)
		{
			Collection<PSModuleInfo> collection = new Collection<PSModuleInfo>();
			Collection<PSModuleInfo> collection2 = null;
			PowerShell powerShell;
			if (rsToUse == null)
			{
				powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace);
			}
			else
			{
				powerShell = PowerShell.Create();
				powerShell.Runspace = rsToUse;
			}
			using (powerShell)
			{
				if (requiredModule.Name.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					powerShell.AddCommand("Test-ModuleManifest");
					powerShell.AddParameter("Path", requiredModule.Name);
				}
				else
				{
					powerShell.AddCommand("Get-Module");
					powerShell.AddParameter("Name", requiredModule.Name);
					powerShell.AddParameter("ListAvailable");
				}
				collection2 = powerShell.Invoke<PSModuleInfo>();
			}
			foreach (PSModuleInfo psmoduleInfo in collection2)
			{
				if (requiredModule.Guid == null || requiredModule.Guid.Value.Equals(psmoduleInfo.Guid))
				{
					if (requiredModule.RequiredVersion != null)
					{
						if (requiredModule.RequiredVersion.Equals(psmoduleInfo.Version))
						{
							collection.Add(psmoduleInfo);
						}
					}
					else if (requiredModule.Version != null)
					{
						if (requiredModule.MaximumVersion != null)
						{
							if (ModuleCmdletBase.GetMaximumVersion(requiredModule.MaximumVersion) >= psmoduleInfo.Version && requiredModule.Version <= psmoduleInfo.Version)
							{
								collection.Add(psmoduleInfo);
							}
						}
						else if (requiredModule.Version <= psmoduleInfo.Version)
						{
							collection.Add(psmoduleInfo);
						}
					}
					else if (requiredModule.MaximumVersion != null)
					{
						if (ModuleCmdletBase.GetMaximumVersion(requiredModule.MaximumVersion) >= psmoduleInfo.Version)
						{
							collection.Add(psmoduleInfo);
						}
					}
					else
					{
						collection.Add(psmoduleInfo);
					}
				}
			}
			return collection;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0002D80C File Offset: 0x0002BA0C
		private static bool HasRequiredModulesCyclicReference(string currentModuleName, List<ModuleSpecification> requiredModules, IEnumerable<PSModuleInfo> moduleInfoList, Dictionary<ModuleSpecification, List<ModuleSpecification>> nonCyclicRequiredModules, out ErrorRecord error)
		{
			error = null;
			if (requiredModules == null || requiredModules.Count == 0)
			{
				return false;
			}
			foreach (ModuleSpecification moduleSpecification in requiredModules)
			{
				if (nonCyclicRequiredModules.ContainsKey(moduleSpecification))
				{
					PSModuleInfo psmoduleInfo = null;
					foreach (PSModuleInfo psmoduleInfo2 in moduleInfoList)
					{
						if (psmoduleInfo2.Name.Equals(currentModuleName, StringComparison.OrdinalIgnoreCase))
						{
							psmoduleInfo = psmoduleInfo2;
							break;
						}
					}
					string message = StringUtil.Format(Modules.RequiredModulesCyclicDependency, new object[]
					{
						currentModuleName,
						moduleSpecification.Name,
						psmoduleInfo.Path
					});
					MissingMemberException exception = new MissingMemberException(message);
					error = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, psmoduleInfo.Path);
					return true;
				}
				Collection<PSModuleInfo> moduleIfAvailable = ModuleCmdletBase.GetModuleIfAvailable(moduleSpecification, null);
				List<ModuleSpecification> list = new List<ModuleSpecification>();
				string currentModuleName2 = null;
				if (moduleIfAvailable.Count == 1)
				{
					currentModuleName2 = moduleIfAvailable[0].Name;
					foreach (ModuleSpecification item in moduleIfAvailable[0].RequiredModulesSpecification)
					{
						list.Add(item);
					}
					if (list.Count > 0)
					{
						nonCyclicRequiredModules.Add(moduleSpecification, list);
					}
				}
				if (ModuleCmdletBase.HasRequiredModulesCyclicReference(currentModuleName2, list, moduleIfAvailable, nonCyclicRequiredModules, out error))
				{
					return true;
				}
			}
			ModuleSpecification key = new ModuleSpecification(currentModuleName);
			if (nonCyclicRequiredModules.ContainsKey(key))
			{
				nonCyclicRequiredModules.Remove(key);
			}
			return false;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0002D9F8 File Offset: 0x0002BBF8
		private ExternalScriptInfo FindLocalizedModuleManifest(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			string fileName = Path.GetFileName(path);
			string text = null;
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			CultureInfo cultureInfo = currentUICulture;
			while (cultureInfo != null && !string.IsNullOrEmpty(cultureInfo.Name))
			{
				StringBuilder stringBuilder = new StringBuilder(directoryName);
				stringBuilder.Append("\\");
				stringBuilder.Append(cultureInfo.Name);
				stringBuilder.Append("\\");
				stringBuilder.Append(fileName);
				string text2 = stringBuilder.ToString();
				if (Utils.NativeFileExists(text2))
				{
					text = text2;
					break;
				}
				cultureInfo = cultureInfo.Parent;
			}
			ExternalScriptInfo result = null;
			if (text != null)
			{
				result = new ExternalScriptInfo(Path.GetFileName(text), text);
			}
			return result;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002DAA4 File Offset: 0x0002BCA4
		internal bool GetListOfStringsFromData(Hashtable data, string moduleManifestPath, string key, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out List<string> list)
		{
			list = null;
			if (data.Contains(key) && data[key] != null)
			{
				try
				{
					string[] collection = (string[])LanguagePrimitives.ConvertTo(data[key], typeof(string[]), CultureInfo.InvariantCulture);
					list = new List<string>(collection);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					ModuleCmdletBase.WriteInvalidManifestMemberError(this, key, moduleManifestPath, e, manifestProcessingFlags);
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002DB1C File Offset: 0x0002BD1C
		private bool GetListOfWildcardsFromData(Hashtable data, string moduleManifestPath, string key, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out List<WildcardPattern> list)
		{
			list = null;
			List<string> list2;
			if (!this.GetListOfStringsFromData(data, moduleManifestPath, key, manifestProcessingFlags, out list2))
			{
				return false;
			}
			if (list2 != null)
			{
				list = new List<WildcardPattern>();
				foreach (string text in list2)
				{
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							list.Add(new WildcardPattern(text, WildcardOptions.IgnoreCase));
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
							list = null;
							ModuleCmdletBase.WriteInvalidManifestMemberError(this, key, moduleManifestPath, e, manifestProcessingFlags);
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002DBC8 File Offset: 0x0002BDC8
		private bool GetListOfFilesFromData(Hashtable data, string moduleManifestPath, string key, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, string moduleBase, string extension, bool verifyFilesExist, out List<string> list)
		{
			list = null;
			List<string> list2;
			if (!this.GetListOfStringsFromData(data, moduleManifestPath, key, manifestProcessingFlags, out list2))
			{
				return false;
			}
			if (list2 != null)
			{
				list = new List<string>();
				foreach (string name in list2)
				{
					try
					{
						string text = this.FixupFileName(moduleBase, name, extension);
						if (verifyFilesExist && !Utils.NativeFileExists(text))
						{
							string message = StringUtil.Format(SessionStateStrings.PathNotFound, text);
							throw new FileNotFoundException(message, text);
						}
						list.Add(text);
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							base.ThrowTerminatingError(ModuleCmdletBase.GenerateInvalidModuleMemberErrorRecord(key, moduleManifestPath, e));
						}
						list = null;
						ModuleCmdletBase.WriteInvalidManifestMemberError(this, key, moduleManifestPath, e, manifestProcessingFlags);
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
		internal void SetModuleLoggingInformation(PSModuleInfo m)
		{
			IEnumerable<string> moduleNames;
			ModuleCmdletBase.ModuleLoggingGroupPolicyStatus moduleLoggingInformation = ModuleCmdletBase.GetModuleLoggingInformation(out moduleNames);
			if (moduleLoggingInformation != ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Undefined)
			{
				this.SetModuleLoggingInformation(moduleLoggingInformation, m, moduleNames);
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0002DCD4 File Offset: 0x0002BED4
		private void SetModuleLoggingInformation(ModuleCmdletBase.ModuleLoggingGroupPolicyStatus status, PSModuleInfo m, IEnumerable<string> moduleNames)
		{
			if ((status & ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Enabled) != ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Undefined && moduleNames != null)
			{
				foreach (string text in moduleNames)
				{
					if (string.Equals(m.Name, text, StringComparison.OrdinalIgnoreCase))
					{
						m.LogPipelineExecutionDetails = true;
					}
					else if (WildcardPattern.ContainsWildcardCharacters(text))
					{
						WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
						if (wildcardPattern.IsMatch(m.Name))
						{
							m.LogPipelineExecutionDetails = true;
						}
					}
				}
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002DD5C File Offset: 0x0002BF5C
		internal static ModuleCmdletBase.ModuleLoggingGroupPolicyStatus GetModuleLoggingInformation(out IEnumerable<string> moduleNames)
		{
			moduleNames = null;
			ModuleCmdletBase.ModuleLoggingGroupPolicyStatus result = ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Undefined;
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("ModuleLogging", new RegistryKey[]
			{
				Registry.LocalMachine,
				Registry.CurrentUser
			});
			if (groupPolicySetting != null)
			{
				object obj = null;
				if (groupPolicySetting.TryGetValue("EnableModuleLogging", out obj))
				{
					if (string.Equals(obj.ToString(), "0", StringComparison.OrdinalIgnoreCase))
					{
						return ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Disabled;
					}
					if (string.Equals(obj.ToString(), "1", StringComparison.OrdinalIgnoreCase))
					{
						result = ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Enabled;
						object obj2 = null;
						if (groupPolicySetting.TryGetValue("ModuleNames", out obj2))
						{
							moduleNames = new List<string>((string[])obj2);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002DDF4 File Offset: 0x0002BFF4
		internal bool GetScalarFromData<T>(Hashtable data, string moduleManifestPath, string key, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out T result)
		{
			object obj = data[key];
			if (obj == null || (obj is string && string.IsNullOrEmpty((string)obj)))
			{
				result = default(T);
				return true;
			}
			bool result2;
			try
			{
				result = (T)((object)LanguagePrimitives.ConvertTo(obj, typeof(T), CultureInfo.InvariantCulture));
				result2 = true;
			}
			catch (PSInvalidCastException ex)
			{
				result = default(T);
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors) != (ModuleCmdletBase.ManifestProcessingFlags)0)
				{
					string message = StringUtil.Format(Modules.ModuleManifestInvalidValue, new object[]
					{
						key,
						ex.Message,
						moduleManifestPath
					});
					ArgumentException exception = new ArgumentException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidManifest", ErrorCategory.ResourceUnavailable, moduleManifestPath);
					base.WriteError(errorRecord);
				}
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002DEC0 File Offset: 0x0002C0C0
		internal string FixupFileName(string moduleBase, string name, string extension)
		{
			string text = ModuleCmdletBase.ResolveRootedFilePath(name, base.Context);
			if (string.IsNullOrEmpty(text))
			{
				text = Path.Combine(moduleBase, name);
			}
			string text2 = ModuleCmdletBase.ResolveRootedFilePath(text, base.Context);
			string extension2 = Path.GetExtension(name);
			string text3 = (!string.IsNullOrEmpty(text2)) ? text2 : text;
			if (string.IsNullOrEmpty(extension2))
			{
				text3 += extension;
			}
			if (!string.IsNullOrEmpty(extension2) && extension2.Equals(".dll", StringComparison.OrdinalIgnoreCase))
			{
				Exception ex = null;
				Assembly assembly = ExecutionContext.LoadAssembly(name, null, out ex);
				if (assembly != null)
				{
					text3 = ClrFacade.GetAssemblyLocation(assembly);
				}
			}
			return text3;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0002DF54 File Offset: 0x0002C154
		internal static bool IsRooted(string filePath)
		{
			return Path.IsPathRooted(filePath) || filePath.StartsWith(".\\", StringComparison.Ordinal) || filePath.StartsWith("./", StringComparison.Ordinal) || filePath.StartsWith("..\\", StringComparison.Ordinal) || filePath.StartsWith("../", StringComparison.Ordinal) || filePath.StartsWith("~/", StringComparison.Ordinal) || filePath.StartsWith("~\\", StringComparison.Ordinal) || filePath.IndexOf(":", StringComparison.Ordinal) >= 0;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0002DFD4 File Offset: 0x0002C1D4
		internal static string ResolveRootedFilePath(string filePath, ExecutionContext context)
		{
			if (!ModuleCmdletBase.IsRooted(filePath))
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			Collection<string> collection = null;
			if (context.EngineSessionState.IsProviderLoaded(context.ProviderNames.FileSystem))
			{
				try
				{
					collection = context.SessionState.Path.GetResolvedProviderPathFromPSPath(filePath, out providerInfo);
				}
				catch (ItemNotFoundException)
				{
					return null;
				}
				if (!providerInfo.NameEquals(context.ProviderNames.FileSystem))
				{
					throw InterpreterError.NewInterpreterException(filePath, typeof(RuntimeException), null, "FileOpenError", ParserStrings.FileOpenError, new object[]
					{
						providerInfo.FullName
					});
				}
			}
			if (collection == null || collection.Count < 1)
			{
				return null;
			}
			if (collection.Count > 1)
			{
				throw InterpreterError.NewInterpreterException(collection, typeof(RuntimeException), null, "AmbiguousPath", ParserStrings.AmbiguousPath, new object[0]);
			}
			return collection[0];
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002E0B8 File Offset: 0x0002C2B8
		internal static string GetResolvedPath(string filePath, ExecutionContext context)
		{
			ProviderInfo providerInfo = null;
			Collection<string> collection;
			if (context != null && context.EngineSessionState != null && context.EngineSessionState.IsProviderLoaded(context.ProviderNames.FileSystem))
			{
				try
				{
					collection = context.SessionState.Path.GetResolvedProviderPathFromPSPath(filePath, true, out providerInfo);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					return null;
				}
				if (providerInfo == null || !providerInfo.NameEquals(context.ProviderNames.FileSystem))
				{
					return null;
				}
				goto IL_6C;
			}
			collection = new Collection<string>();
			collection.Add(filePath);
			IL_6C:
			if (collection == null || collection.Count < 1 || collection.Count > 1)
			{
				return null;
			}
			return collection[0];
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0002E164 File Offset: 0x0002C364
		private void RemoveTypesAndFormatting(IList<string> formatFilesToRemove, IList<string> typeFilesToRemove)
		{
			try
			{
				if (base.Context.InitialSessionState != null)
				{
					if ((formatFilesToRemove == null || formatFilesToRemove.Count <= 0) && (typeFilesToRemove == null || typeFilesToRemove.Count <= 0))
					{
						goto IL_1FB;
					}
					bool refreshTypeAndFormatSetting = base.Context.InitialSessionState.RefreshTypeAndFormatSetting;
					try
					{
						base.Context.InitialSessionState.RefreshTypeAndFormatSetting = true;
						InitialSessionState.RemoveTypesAndFormats(base.Context, formatFilesToRemove, typeFilesToRemove);
						goto IL_1FB;
					}
					finally
					{
						base.Context.InitialSessionState.RefreshTypeAndFormatSetting = refreshTypeAndFormatSetting;
					}
				}
				if (formatFilesToRemove != null && formatFilesToRemove.Count > 0)
				{
					HashSet<string> hashSet = new HashSet<string>(formatFilesToRemove, StringComparer.OrdinalIgnoreCase);
					List<int> list = new List<int>();
					for (int i = 0; i < base.Context.RunspaceConfiguration.Formats.Count; i++)
					{
						string fileName = base.Context.RunspaceConfiguration.Formats[i].FileName;
						if (fileName != null && hashSet.Contains(fileName))
						{
							list.Add(i);
						}
					}
					for (int j = list.Count - 1; j >= 0; j--)
					{
						base.Context.RunspaceConfiguration.Formats.RemoveItem(list[j]);
					}
					base.Context.RunspaceConfiguration.Formats.Update();
				}
				if (typeFilesToRemove != null && typeFilesToRemove.Count > 0)
				{
					HashSet<string> hashSet2 = new HashSet<string>(typeFilesToRemove, StringComparer.OrdinalIgnoreCase);
					List<int> list2 = new List<int>();
					for (int k = 0; k < base.Context.RunspaceConfiguration.Types.Count; k++)
					{
						string fileName2 = base.Context.RunspaceConfiguration.Types[k].FileName;
						if (fileName2 != null && hashSet2.Contains(fileName2))
						{
							list2.Add(k);
						}
					}
					for (int l = list2.Count - 1; l >= 0; l--)
					{
						base.Context.RunspaceConfiguration.Types.RemoveItem(list2[l]);
					}
					base.Context.RunspaceConfiguration.Types.Update();
				}
				IL_1FB:;
			}
			catch (RuntimeException ex)
			{
				string fullyQualifiedErrorId = ex.ErrorRecord.FullyQualifiedErrorId;
				if (!fullyQualifiedErrorId.Equals("ErrorsUpdatingTypes", StringComparison.Ordinal) && !fullyQualifiedErrorId.Equals("ErrorsUpdatingFormats", StringComparison.Ordinal))
				{
					throw;
				}
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0002E3D4 File Offset: 0x0002C5D4
		internal void RemoveModule(PSModuleInfo module)
		{
			this.RemoveModule(module, null);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		internal void RemoveModule(PSModuleInfo module, string moduleNameInRemoveModuleCmdlet)
		{
			bool flag = false;
			bool flag2 = this.ShouldModuleBeRemoved(module, moduleNameInRemoveModuleCmdlet, out flag);
			if (flag2 && base.Context.Modules.ModuleTable.ContainsKey(module.Path))
			{
				if (module.OnRemove != null)
				{
					module.OnRemove.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, new object[]
					{
						module
					});
				}
				if (module.ImplementingAssembly != null && !module.ImplementingAssembly.IsDynamic)
				{
					Type[] assemblyTypes = PSSnapInHelpers.GetAssemblyTypes(module.ImplementingAssembly, module.Name);
					foreach (Type type in assemblyTypes)
					{
						if (typeof(IModuleAssemblyCleanup).IsAssignableFrom(type) && type != typeof(IModuleAssemblyCleanup))
						{
							IModuleAssemblyCleanup moduleAssemblyCleanup = (IModuleAssemblyCleanup)PSSnapInHelpers.CreateModuleInitializerInstance.Target(PSSnapInHelpers.CreateModuleInitializerInstance, type);
							moduleAssemblyCleanup.OnRemove(module);
						}
					}
				}
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, List<CmdletInfo>> keyValuePair in base.Context.EngineSessionState.GetCmdletTable())
				{
					List<CmdletInfo> value = keyValuePair.Value;
					for (int j = value.Count - 1; j >= 0; j--)
					{
						if (value[j].Module != null && value[j].Module.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
						{
							string name = value[j].Name;
							value.RemoveAt(j);
							base.Context.EngineSessionState.RemoveCmdlet(name, j, true);
						}
					}
					if (value.Count == 0)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (string name2 in list)
				{
					base.Context.EngineSessionState.RemoveCmdletEntry(name2, true);
				}
				if (module.ModuleType == ModuleType.Binary)
				{
					Dictionary<string, List<ProviderInfo>> providers = base.Context.TopLevelSessionState.Providers;
					List<string> list2 = new List<string>();
					foreach (KeyValuePair<string, List<ProviderInfo>> keyValuePair2 in providers)
					{
						for (int k = keyValuePair2.Value.Count - 1; k >= 0; k--)
						{
							ProviderInfo providerInfo = keyValuePair2.Value[k];
							string assemblyLocation = ClrFacade.GetAssemblyLocation(providerInfo.ImplementingType.GetTypeInfo().Assembly);
							if (assemblyLocation.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
							{
								InitialSessionState.RemoveAllDrivesForProvider(providerInfo, base.Context.TopLevelSessionState);
								if (base.Context.EngineSessionState != base.Context.TopLevelSessionState)
								{
									InitialSessionState.RemoveAllDrivesForProvider(providerInfo, base.Context.EngineSessionState);
								}
								foreach (PSModuleInfo psmoduleInfo in base.Context.Modules.ModuleTable.Values)
								{
									if (psmoduleInfo.SessionState != null)
									{
										SessionStateInternal @internal = psmoduleInfo.SessionState.Internal;
										if (@internal != base.Context.TopLevelSessionState && @internal != base.Context.EngineSessionState)
										{
											InitialSessionState.RemoveAllDrivesForProvider(providerInfo, base.Context.EngineSessionState);
										}
									}
								}
								keyValuePair2.Value.RemoveAt(k);
							}
						}
						if (keyValuePair2.Value.Count == 0)
						{
							list2.Add(keyValuePair2.Key);
						}
					}
					foreach (string key in list2)
					{
						providers.Remove(key);
					}
				}
				SessionStateInternal engineSessionState = base.Context.EngineSessionState;
				if (module.SessionState != null)
				{
					foreach (object obj in engineSessionState.GetFunctionTable())
					{
						FunctionInfo functionInfo = (FunctionInfo)((DictionaryEntry)obj).Value;
						if (functionInfo.Module != null && functionInfo.Module.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								engineSessionState.RemoveFunction(functionInfo.Name, true);
								string text = StringUtil.Format(Modules.RemovingImportedFunction, functionInfo.Name);
								base.WriteVerbose(text);
							}
							catch (SessionStateUnauthorizedAccessException ex)
							{
								string message = StringUtil.Format(Modules.UnableToRemoveModuleMember, new object[]
								{
									functionInfo.Name,
									module.Name,
									ex.Message
								});
								InvalidOperationException exception = new InvalidOperationException(message, ex);
								ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_MemberNotRemoved", ErrorCategory.PermissionDenied, functionInfo.Name);
								base.WriteError(errorRecord);
							}
						}
					}
					foreach (PSVariable psvariable in module.SessionState.Internal.ExportedVariables)
					{
						PSVariable variable = engineSessionState.GetVariable(psvariable.Name);
						if (variable != null && variable == psvariable)
						{
							engineSessionState.RemoveVariable(variable, this.BaseForce);
							string text2 = StringUtil.Format(Modules.RemovingImportedVariable, variable.Name);
							base.WriteVerbose(text2);
						}
					}
					foreach (KeyValuePair<string, AliasInfo> keyValuePair3 in engineSessionState.GetAliasTable())
					{
						AliasInfo value2 = keyValuePair3.Value;
						if (value2.Module != null && value2.Module.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
						{
							engineSessionState.RemoveAlias(value2.Name, true);
							string text3 = StringUtil.Format(Modules.RemovingImportedAlias, value2.Name);
							base.WriteVerbose(text3);
						}
					}
				}
				this.RemoveTypesAndFormatting(module.ExportedFormatFiles, module.ExportedTypeFiles);
				base.Context.HelpSystem.ResetHelpProviders();
				foreach (KeyValuePair<string, PSModuleInfo> keyValuePair4 in base.Context.Modules.ModuleTable)
				{
					PSModuleInfo value3 = keyValuePair4.Value;
					if (value3.SessionState != null && value3.SessionState.Internal.ModuleTable.ContainsKey(module.Path))
					{
						value3.SessionState.Internal.ModuleTable.Remove(module.Path);
						value3.SessionState.Internal.ModuleTableKeys.Remove(module.Path);
					}
				}
				if (flag)
				{
					base.Context.TopLevelSessionState.ModuleTable.Remove(module.Path);
					base.Context.TopLevelSessionState.ModuleTableKeys.Remove(module.Path);
				}
				base.Context.Modules.ModuleTable.Remove(module.Path);
				PSModuleInfo.RemoveFromAppDomainLevelCache(module.Name);
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002EC28 File Offset: 0x0002CE28
		private bool ShouldModuleBeRemoved(PSModuleInfo module, string moduleNameInRemoveModuleCmdlet, out bool isTopLevelModule)
		{
			isTopLevelModule = false;
			if (base.Context.TopLevelSessionState.ModuleTable.ContainsKey(module.Path))
			{
				isTopLevelModule = true;
				return moduleNameInRemoveModuleCmdlet == null || module.Name.Equals(moduleNameInRemoveModuleCmdlet, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002EC64 File Offset: 0x0002CE64
		internal bool IsModuleAlreadyLoaded(PSModuleInfo alreadyLoadedModule)
		{
			return alreadyLoadedModule != null && (!(this.BaseRequiredVersion != null) || alreadyLoadedModule.Version.Equals(this.BaseRequiredVersion)) && (!(this.BaseMinimumVersion != null) || !(alreadyLoadedModule.Version < this.BaseMinimumVersion)) && (!(this.BaseMaximumVersion != null) || !(alreadyLoadedModule.Version > this.BaseMaximumVersion)) && (this.BaseGuid == null || alreadyLoadedModule.Guid.Equals(this.BaseGuid));
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002ED14 File Offset: 0x0002CF14
		internal PSModuleInfo IsModuleImportUnnecessaryBecauseModuleIsAlreadyLoaded(string modulePath, string prefix, ModuleCmdletBase.ImportModuleOptions options)
		{
			PSModuleInfo psmoduleInfo;
			if (!base.Context.Modules.ModuleTable.TryGetValue(modulePath, out psmoduleInfo) || !this.IsModuleAlreadyLoaded(psmoduleInfo))
			{
				return null;
			}
			if (this.BaseForce)
			{
				this.RemoveModule(psmoduleInfo);
				return null;
			}
			if (string.IsNullOrEmpty(prefix) && Utils.NativeFileExists(psmoduleInfo.Path))
			{
				string defaultPrefix = this.GetDefaultPrefix(psmoduleInfo);
				if (!string.IsNullOrEmpty(defaultPrefix))
				{
					prefix = defaultPrefix;
				}
			}
			ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
			this.ImportModuleMembers(psmoduleInfo, prefix, options);
			if (this.BaseAsCustomObject)
			{
				if (psmoduleInfo.ModuleType != ModuleType.Script)
				{
					string message = StringUtil.Format(Modules.CantUseAsCustomObjectWithBinaryModule, psmoduleInfo.Path);
					InvalidOperationException exception = new InvalidOperationException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_CantUseAsCustomObjectWithBinaryModule", ErrorCategory.PermissionDenied, null);
					base.WriteError(errorRecord);
				}
				else
				{
					base.WriteObject(psmoduleInfo.AsCustomObject());
				}
			}
			else if (this.BasePassThru)
			{
				base.WriteObject(psmoduleInfo);
			}
			return psmoduleInfo;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002EE08 File Offset: 0x0002D008
		internal PSModuleInfo LoadUsingExtensions(PSModuleInfo parentModule, string moduleName, string fileBaseName, string extension, string moduleBase, string prefix, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out bool found)
		{
			bool flag = false;
			return this.LoadUsingExtensions(parentModule, moduleName, fileBaseName, extension, moduleBase, prefix, ss, options, manifestProcessingFlags, out found, out flag);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002EE30 File Offset: 0x0002D030
		internal PSModuleInfo LoadUsingExtensions(PSModuleInfo parentModule, string moduleName, string fileBaseName, string extension, string moduleBase, string prefix, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out bool found, out bool moduleFileFound)
		{
			moduleFileFound = false;
			string[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				array = new string[]
				{
					extension
				};
			}
			else
			{
				array = ModuleIntrinsics.PSModuleExtensions;
			}
			foreach (string str in array)
			{
				string text = fileBaseName + str;
				text = ModuleCmdletBase.GetResolvedPath(text, base.Context);
				if (text != null && (string.IsNullOrEmpty(base.Context.ModuleBeingProcessed) || !base.Context.ModuleBeingProcessed.Equals(text, StringComparison.OrdinalIgnoreCase)))
				{
					PSModuleInfo psmoduleInfo;
					base.Context.Modules.ModuleTable.TryGetValue(text, out psmoduleInfo);
					PSModuleInfo result;
					if (!this.BaseForce && psmoduleInfo != null && (this.BaseRequiredVersion == null || psmoduleInfo.Version.Equals(this.BaseRequiredVersion)) && ((this.BaseMinimumVersion == null && this.BaseMaximumVersion == null) || (this.BaseMaximumVersion != null && this.BaseMinimumVersion == null && psmoduleInfo.Version <= this.BaseMaximumVersion) || (this.BaseMaximumVersion == null && this.BaseMinimumVersion != null && psmoduleInfo.Version >= this.BaseMinimumVersion) || (this.BaseMaximumVersion != null && this.BaseMinimumVersion != null && psmoduleInfo.Version >= this.BaseMinimumVersion && psmoduleInfo.Version <= this.BaseMaximumVersion)) && (this.BaseGuid == null || psmoduleInfo.Guid.Equals(this.BaseGuid)) && (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
					{
						moduleFileFound = true;
						psmoduleInfo = base.Context.Modules.ModuleTable[text];
						if (string.IsNullOrEmpty(prefix))
						{
							string defaultPrefix = this.GetDefaultPrefix(psmoduleInfo);
							if (!string.IsNullOrEmpty(defaultPrefix))
							{
								prefix = defaultPrefix;
							}
						}
						ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
						this.ImportModuleMembers(psmoduleInfo, prefix, options);
						if (this.BaseAsCustomObject)
						{
							if (psmoduleInfo.ModuleType != ModuleType.Script)
							{
								string message = StringUtil.Format(Modules.CantUseAsCustomObjectWithBinaryModule, psmoduleInfo.Path);
								InvalidOperationException exception = new InvalidOperationException(message);
								ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_CantUseAsCustomObjectWithBinaryModule", ErrorCategory.PermissionDenied, null);
								base.WriteError(errorRecord);
							}
							else
							{
								base.WriteObject(psmoduleInfo.AsCustomObject());
							}
						}
						else if (this.BasePassThru)
						{
							base.WriteObject(psmoduleInfo);
						}
						found = true;
						result = psmoduleInfo;
					}
					else
					{
						if (!Utils.NativeFileExists(text))
						{
							goto IL_336;
						}
						moduleFileFound = true;
						if (this.BaseForce && psmoduleInfo != null && (this.BaseRequiredVersion == null || psmoduleInfo.Version.Equals(this.BaseRequiredVersion)) && (this.BaseGuid == null || psmoduleInfo.Guid.Equals(this.BaseGuid)))
						{
							this.RemoveModule(psmoduleInfo);
						}
						psmoduleInfo = this.LoadModule(parentModule, text, moduleBase, prefix, ss, null, ref options, manifestProcessingFlags, out found, out moduleFileFound);
						if (!found)
						{
							goto IL_336;
						}
						result = psmoduleInfo;
					}
					return result;
				}
				IL_336:;
			}
			found = false;
			return null;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002F18C File Offset: 0x0002D38C
		internal string GetDefaultPrefix(PSModuleInfo module)
		{
			string text = string.Empty;
			string extension = Path.GetExtension(module.Path);
			if (!string.IsNullOrEmpty(extension) && extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
			{
				string text2;
				ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(module.Path, out text2, true);
				bool flag = false;
				Hashtable hashtable = null;
				Hashtable hashtable2 = null;
				if (this.LoadModuleManifestData(scriptInfoForFile, ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError, out hashtable, out hashtable2, ref flag) && hashtable.Contains("DefaultCommandPrefix"))
				{
					if (hashtable2 != null && hashtable2.Contains("DefaultCommandPrefix"))
					{
						text = (string)LanguagePrimitives.ConvertTo(hashtable2["DefaultCommandPrefix"], typeof(string), CultureInfo.InvariantCulture);
					}
					if (string.IsNullOrEmpty(text))
					{
						text = (string)LanguagePrimitives.ConvertTo(hashtable["DefaultCommandPrefix"], typeof(string), CultureInfo.InvariantCulture);
					}
				}
			}
			return text;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002F268 File Offset: 0x0002D468
		internal ExternalScriptInfo GetScriptInfoForFile(string fileName, out string scriptName, bool checkExecutionPolicy)
		{
			scriptName = Path.GetFileName(fileName);
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(scriptName, fileName, base.Context);
			if (!scriptName.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
			{
				if (checkExecutionPolicy)
				{
					base.Context.AuthorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Runspace, base.Context.EngineHostInterface);
				}
				else
				{
					base.Context.AuthorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Internal, base.Context.EngineHostInterface);
				}
				if (!scriptName.EndsWith(".cdxml", StringComparison.OrdinalIgnoreCase))
				{
					CommandDiscovery.VerifyScriptRequirements(externalScriptInfo, base.Context);
				}
				externalScriptInfo.SignatureChecked = true;
			}
			return externalScriptInfo;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002F300 File Offset: 0x0002D500
		internal PSModuleInfo LoadModule(string fileName, string moduleBase, string prefix, SessionState ss, ref ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out bool found)
		{
			bool flag = false;
			return this.LoadModule(null, fileName, moduleBase, prefix, ss, null, ref options, manifestProcessingFlags, out found, out flag);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002F324 File Offset: 0x0002D524
		internal PSModuleInfo LoadModule(PSModuleInfo parentModule, string fileName, string moduleBase, string prefix, SessionState ss, object privateData, ref ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out bool found, out bool moduleFileFound)
		{
			if (!Utils.NativeFileExists(fileName))
			{
				found = false;
				moduleFileFound = false;
				return null;
			}
			string extension = Path.GetExtension(fileName);
			if (this.BaseMinimumVersion != null || this.BaseMaximumVersion != null || this.BaseRequiredVersion != null || this.BaseGuid != null)
			{
				if (string.IsNullOrEmpty(extension) || !extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					found = false;
					moduleFileFound = false;
					return null;
				}
				if (base.Context.Modules.ModuleTable.ContainsKey(fileName) && this.BaseMinimumVersion != null && base.Context.Modules.ModuleTable[fileName].Version >= this.BaseMinimumVersion && (this.BaseMaximumVersion == null || base.Context.Modules.ModuleTable[fileName].Version <= this.BaseMaximumVersion))
				{
					found = false;
					moduleFileFound = false;
					return null;
				}
			}
			PSModuleInfo psmoduleInfo = null;
			found = false;
			string moduleBeingProcessed = base.Context.ModuleBeingProcessed;
			try
			{
				base.Context.PreviousModuleProcessed = base.Context.ModuleBeingProcessed;
				base.Context.ModuleBeingProcessed = fileName;
				string text = StringUtil.Format(Modules.LoadingModule, fileName);
				base.WriteVerbose(text);
				moduleFileFound = true;
				if (extension.Equals(".psm1", StringComparison.OrdinalIgnoreCase))
				{
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
					{
						bool flag = ModuleCmdletBase.ShouldProcessScriptModule(parentModule, ref found);
						if (flag)
						{
							bool force = (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.Force) == ModuleCmdletBase.ManifestProcessingFlags.Force;
							psmoduleInfo = this.AnalyzeScriptFile(fileName, force, base.Context);
							found = true;
							goto IL_9DD;
						}
						goto IL_9DD;
					}
					else
					{
						string text2;
						ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(fileName, out text2, true);
						try
						{
							base.Context.Modules.IncrementModuleNestingDepth(this, scriptInfoForFile.Path);
							try
							{
								psmoduleInfo = base.Context.Modules.CreateModule(fileName, scriptInfoForFile, base.MyInvocation.ScriptPosition, ss, privateData, this._arguments);
								psmoduleInfo.SetModuleBase(moduleBase);
								this.SetModuleLoggingInformation(psmoduleInfo);
								if (!psmoduleInfo.SessionState.Internal.UseExportList)
								{
									ModuleIntrinsics.ExportModuleMembers(this, psmoduleInfo.SessionState.Internal, this.MatchAll, this.MatchAll, this.MatchAll, null, options.ServiceCoreAutoAdded ? ModuleCmdletBase.ServiceCoreAssemblyCmdlets : null);
								}
								if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
								{
									this.ImportModuleMembers(psmoduleInfo, prefix, options);
									ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
								}
								found = true;
								if (this.BaseAsCustomObject)
								{
									base.WriteObject(psmoduleInfo.AsCustomObject());
								}
								else if (this.BasePassThru)
								{
									base.WriteObject(psmoduleInfo);
								}
							}
							catch (RuntimeException ex)
							{
								if (ModuleCmdletBase.ManifestProcessingFlags.WriteErrors == (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors))
								{
									ex.ErrorRecord.PreserveInvocationInfoOnce = true;
								}
								if (ex.WasThrownFromThrowStatement)
								{
									base.ThrowTerminatingError(ex.ErrorRecord);
								}
								else
								{
									base.WriteError(ex.ErrorRecord);
								}
							}
							goto IL_9DD;
						}
						finally
						{
							base.Context.Modules.DecrementModuleNestingCount();
						}
					}
				}
				if (extension.Equals(".ps1", StringComparison.OrdinalIgnoreCase))
				{
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
					{
						bool flag2 = ModuleCmdletBase.ShouldProcessScriptModule(parentModule, ref found);
						if (flag2)
						{
							bool force2 = (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.Force) == ModuleCmdletBase.ManifestProcessingFlags.Force;
							psmoduleInfo = this.AnalyzeScriptFile(fileName, force2, base.Context);
							found = true;
							goto IL_9DD;
						}
						goto IL_9DD;
					}
					else
					{
						psmoduleInfo = new PSModuleInfo(ModuleIntrinsics.GetModuleName(fileName), fileName, base.Context, ss);
						string text2;
						ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(fileName, out text2, true);
						text = StringUtil.Format(Modules.DottingScriptFile, fileName);
						base.WriteVerbose(text);
						try
						{
							found = true;
							InvocationInfo value = (InvocationInfo)base.Context.GetVariableValue(SpecialVariables.MyInvocationVarPath);
							object variableValue = base.Context.GetVariableValue(SpecialVariables.PSScriptRootVarPath);
							object variableValue2 = base.Context.GetVariableValue(SpecialVariables.PSCommandPathVarPath);
							try
							{
								InvocationInfo invocationInfo = new InvocationInfo(scriptInfoForFile, scriptInfoForFile.ScriptBlock.Ast.Extent, base.Context);
								scriptInfoForFile.ScriptBlock.InvokeWithPipe(false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, ((MshCommandRuntime)base.CommandRuntime).OutputPipe, invocationInfo, false, null, null, this.BaseArgumentList ?? new object[0]);
							}
							finally
							{
								if (base.Context.EngineSessionState.CurrentScope.LocalsTuple != null)
								{
									base.Context.EngineSessionState.CurrentScope.LocalsTuple.SetAutomaticVariable(AutomaticVariable.PSScriptRoot, variableValue, base.Context);
									base.Context.EngineSessionState.CurrentScope.LocalsTuple.SetAutomaticVariable(AutomaticVariable.PSCommandPath, variableValue2, base.Context);
									base.Context.EngineSessionState.CurrentScope.LocalsTuple.SetAutomaticVariable(AutomaticVariable.MyInvocation, value, base.Context);
								}
							}
							ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
							if (this.BaseAsCustomObject)
							{
								base.WriteObject(psmoduleInfo.AsCustomObject());
							}
							else if (this.BasePassThru)
							{
								base.WriteObject(psmoduleInfo);
							}
							goto IL_9DD;
						}
						catch (RuntimeException ex2)
						{
							if (ModuleCmdletBase.ManifestProcessingFlags.WriteErrors == (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors))
							{
								ex2.ErrorRecord.PreserveInvocationInfoOnce = true;
							}
							if (ex2.WasThrownFromThrowStatement)
							{
								base.ThrowTerminatingError(ex2.ErrorRecord);
							}
							else
							{
								base.WriteError(ex2.ErrorRecord);
							}
							goto IL_9DD;
						}
						catch (ExitException ex3)
						{
							int num = (int)ex3.Argument;
							base.Context.SetVariable(SpecialVariables.LastExitCodeVarPath, num);
							goto IL_9DD;
						}
					}
				}
				if (extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					string text2;
					ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(fileName, out text2, true);
					found = true;
					psmoduleInfo = this.LoadModuleManifest(scriptInfoForFile, manifestProcessingFlags, this.BaseMinimumVersion, this.BaseMaximumVersion, this.BaseRequiredVersion, this.BaseGuid, ref options);
					if (psmoduleInfo != null)
					{
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
						{
							ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
						}
						if (this.BasePassThru)
						{
							base.WriteObject(psmoduleInfo);
						}
					}
					else if (this.BaseMinimumVersion != null || this.BaseRequiredVersion != null || this.BaseGuid != null || this.BaseMaximumVersion != null)
					{
						found = false;
					}
				}
				else if (extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
				{
					psmoduleInfo = this.LoadBinaryModule(false, ModuleIntrinsics.GetModuleName(fileName), fileName, null, moduleBase, ss, options, manifestProcessingFlags, prefix, true, true, out found);
					if (found = (psmoduleInfo != null))
					{
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
						{
							ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
						}
						if (this.BaseAsCustomObject)
						{
							text = StringUtil.Format(Modules.CantUseAsCustomObjectWithBinaryModule, fileName);
							InvalidOperationException exception = new InvalidOperationException(text);
							ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_CantUseAsCustomObjectWithBinaryModule", ErrorCategory.PermissionDenied, null);
							base.WriteError(errorRecord);
						}
						else if (this.BasePassThru)
						{
							base.WriteObject(psmoduleInfo);
						}
					}
				}
				else if (extension.Equals(".xaml", StringComparison.OrdinalIgnoreCase))
				{
					if (Utils.IsRunningFromSysWOW64())
					{
						throw new NotSupportedException(AutomationExceptions.WorkflowDoesNotSupportWOW64);
					}
					string text2;
					ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(fileName, out text2, true);
					ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
					List<string> list = new List<string>();
					list.Add(fileName);
					found = true;
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
					{
						psmoduleInfo = new PSModuleInfo(ModuleIntrinsics.GetModuleName(fileName), fileName, null, null);
						this.ProcessWorkflowsToProcess(moduleBase, list, new List<string>(), new List<string>(), null, psmoduleInfo, importModuleOptions);
					}
					else
					{
						if (ss == null)
						{
							ss = new SessionState(base.Context, true, true);
						}
						psmoduleInfo = new PSModuleInfo(ModuleIntrinsics.GetModuleName(fileName), fileName, base.Context, ss);
						ss.Internal.Module = psmoduleInfo;
						psmoduleInfo.PrivateData = privateData;
						psmoduleInfo.SetModuleType(ModuleType.Workflow);
						psmoduleInfo.SetModuleBase(moduleBase);
						importModuleOptions.ServiceCoreAutoAdded = true;
						this.LoadServiceCoreModule(psmoduleInfo, string.Empty, ss, importModuleOptions, manifestProcessingFlags, true, out found);
						this.ProcessWorkflowsToProcess(moduleBase, list, new List<string>(), new List<string>(), ss, psmoduleInfo, importModuleOptions);
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != (ModuleCmdletBase.ManifestProcessingFlags)0)
						{
							this.ImportModuleMembers(psmoduleInfo, prefix, options);
						}
						ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
					}
					if (this.BaseAsCustomObject)
					{
						base.WriteObject(psmoduleInfo.AsCustomObject());
					}
					else if (this.BasePassThru)
					{
						base.WriteObject(psmoduleInfo);
					}
				}
				else
				{
					if (extension.Equals(".cdxml", StringComparison.OrdinalIgnoreCase))
					{
						found = true;
						try
						{
							string moduleName = ModuleIntrinsics.GetModuleName(fileName);
							string text2;
							ExternalScriptInfo scriptInfoForFile = this.GetScriptInfoForFile(fileName, out text2, true);
							try
							{
								StringReader cmdletizationXmlReader = new StringReader(scriptInfoForFile.ScriptContents);
								StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
								ScriptWriter scriptWriter = new ScriptWriter(cmdletizationXmlReader, moduleName, "Microsoft.PowerShell.Cmdletization.Cim.CimCmdletAdapter, Microsoft.PowerShell.Commands.Management, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", base.MyInvocation, ScriptWriter.GenerationOptions.HelpXml);
								if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) != ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
								{
									psmoduleInfo = new PSModuleInfo(fileName, null, null);
									scriptWriter.PopulatePSModuleInfo(psmoduleInfo);
									scriptWriter.ReportExportedCommands(psmoduleInfo, prefix);
								}
								else
								{
									scriptWriter.WriteScriptModule(stringWriter);
									ScriptBlock scriptBlock = ScriptBlock.Create(base.Context, stringWriter.ToString());
									scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
									List<object> list2;
									psmoduleInfo = base.Context.Modules.CreateModule(moduleName, fileName, scriptBlock, ss, out list2, this._arguments);
									psmoduleInfo.SetModuleBase(moduleBase);
									scriptWriter.PopulatePSModuleInfo(psmoduleInfo);
									scriptWriter.ReportExportedCommands(psmoduleInfo, prefix);
									this.ImportModuleMembers(psmoduleInfo, prefix, options);
									ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
								}
							}
							catch (Exception ex4)
							{
								CommandProcessorBase.CheckForSevereException(ex4);
								string message = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ExportCimCommand_ErrorInCmdletizationXmlFile, new object[]
								{
									fileName,
									ex4.Message
								});
								throw new XmlException(message, ex4);
							}
							if (this.BaseAsCustomObject)
							{
								base.WriteObject(psmoduleInfo.AsCustomObject());
							}
							else if (this.BasePassThru)
							{
								base.WriteObject(psmoduleInfo);
							}
							goto IL_9DD;
						}
						catch (RuntimeException ex5)
						{
							if (ModuleCmdletBase.ManifestProcessingFlags.WriteErrors == (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.WriteErrors))
							{
								ex5.ErrorRecord.PreserveInvocationInfoOnce = true;
								base.WriteError(ex5.ErrorRecord);
							}
							goto IL_9DD;
						}
					}
					found = true;
					text = StringUtil.Format(Modules.InvalidModuleExtension, extension, fileName);
					InvalidOperationException exception2 = new InvalidOperationException(text);
					ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_InvalidModuleExtension", ErrorCategory.PermissionDenied, null);
					base.WriteError(errorRecord2);
				}
				IL_9DD:;
			}
			finally
			{
				base.Context.ModuleBeingProcessed = moduleBeingProcessed;
			}
			if (PSModuleInfo.UseAppDomainLevelModuleCache && psmoduleInfo != null && moduleBase == null && this.AddToAppDomainLevelCache)
			{
				PSModuleInfo.AddToAppDomainLevelModuleCache(psmoduleInfo.Name, fileName, this.BaseForce);
			}
			return psmoduleInfo;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002FE0C File Offset: 0x0002E00C
		private static bool ShouldProcessScriptModule(PSModuleInfo parentModule, ref bool found)
		{
			bool flag = true;
			if (parentModule != null && flag && parentModule.DeclaredFunctionExports != null && parentModule.DeclaredFunctionExports.Count > 0)
			{
				flag = false;
				foreach (string pattern in parentModule.ExportedFunctions.Keys)
				{
					if (WildcardPattern.ContainsWildcardCharacters(pattern))
					{
						flag = true;
						break;
					}
				}
				found = true;
			}
			return flag;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002FE90 File Offset: 0x0002E090
		private void ClearAnalysisCaches()
		{
			lock (ModuleCmdletBase.lockObject)
			{
				ModuleCmdletBase.binaryAnalysisCache.Clear();
				ModuleCmdletBase.scriptAnalysisCache.Clear();
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002FEE0 File Offset: 0x0002E0E0
		private BinaryAnalysisResult GetCmdletsFromBinaryModuleImplementation(string path, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, out Version assemblyVersion)
		{
			Tuple<BinaryAnalysisResult, Version> tuple = null;
			lock (ModuleCmdletBase.lockObject)
			{
				ModuleCmdletBase.binaryAnalysisCache.TryGetValue(path, out tuple);
			}
			if (tuple != null)
			{
				assemblyVersion = tuple.Item2;
				return tuple.Item1;
			}
			assemblyVersion = new Version("0.0.0.0");
			bool flag2 = false;
			AppDomain appDomain = base.Context.AppDomainForModuleAnalysis;
			if (appDomain == null)
			{
				flag2 = base.Context.TakeResponsibilityForModuleAnalysisAppDomain();
				appDomain = (base.Context.AppDomainForModuleAnalysis = AppDomain.CreateDomain("ReflectionDomain"));
			}
			BinaryAnalysisResult result;
			try
			{
				appDomain.SetData("PathToProcess", path);
				appDomain.SetData("IsModuleLoad", (manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements);
				appDomain.SetData("DetectedCmdlets", null);
				appDomain.SetData("DetectedAliases", null);
				appDomain.SetData("AssemblyVersion", assemblyVersion);
				appDomain.DoCallBack(new CrossAppDomainDelegate(ModuleCmdletBase.AnalyzeSnapinDomainHelper));
				List<string> list = (List<string>)appDomain.GetData("DetectedCmdlets");
				List<Tuple<string, string>> detectedAliases = (List<Tuple<string, string>>)appDomain.GetData("DetectedAliases");
				assemblyVersion = (Version)appDomain.GetData("AssemblyVersion");
				if (list.Count == 0 && Path.IsPathRooted(path))
				{
					string fileName = Path.GetFileName(path);
					BinaryAnalysisResult cmdletsFromBinaryModuleImplementation = this.GetCmdletsFromBinaryModuleImplementation(fileName, manifestProcessingFlags, out assemblyVersion);
					list = cmdletsFromBinaryModuleImplementation.DetectedCmdlets;
					detectedAliases = cmdletsFromBinaryModuleImplementation.DetectedAliases;
				}
				BinaryAnalysisResult binaryAnalysisResult = new BinaryAnalysisResult();
				binaryAnalysisResult.DetectedCmdlets = list;
				binaryAnalysisResult.DetectedAliases = detectedAliases;
				lock (ModuleCmdletBase.lockObject)
				{
					ModuleCmdletBase.binaryAnalysisCache[path] = Tuple.Create<BinaryAnalysisResult, Version>(binaryAnalysisResult, assemblyVersion);
				}
				result = binaryAnalysisResult;
			}
			finally
			{
				if (flag2)
				{
					base.Context.ReleaseResponsibilityForModuleAnalysisAppDomain();
				}
			}
			return result;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000300E8 File Offset: 0x0002E2E8
		private static void AnalyzeSnapinDomainHelper()
		{
			string text = (string)AppDomain.CurrentDomain.GetData("PathToProcess");
			bool isModuleLoad = (bool)AppDomain.CurrentDomain.GetData("IsModuleLoad");
			Dictionary<string, SessionStateCmdletEntry> dictionary = null;
			Dictionary<string, List<SessionStateAliasEntry>> dictionary2 = null;
			Dictionary<string, SessionStateProviderEntry> dictionary3 = null;
			string text2 = null;
			Version data = new Version("0.0.0.0");
			try
			{
				Assembly assembly = null;
				try
				{
					if (Path.IsPathRooted(text))
					{
						assembly = InitialSessionState.LoadAssemblyFromFile(text);
					}
					else
					{
						Exception ex = null;
						assembly = ExecutionContext.LoadAssembly(text, null, out ex);
					}
					if (assembly != null)
					{
						data = ModuleCmdletBase.GetAssemblyVersionNumber(assembly);
					}
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
				if (assembly != null)
				{
					PSSnapInHelpers.AnalyzePSSnapInAssembly(assembly, assembly.Location, null, null, isModuleLoad, out dictionary, out dictionary2, out dictionary3, out text2);
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
			List<string> list = new List<string>();
			List<Tuple<string, string>> list2 = new List<Tuple<string, string>>();
			if (dictionary != null)
			{
				foreach (SessionStateCmdletEntry sessionStateCmdletEntry in dictionary.Values)
				{
					list.Add(sessionStateCmdletEntry.Name);
				}
			}
			if (dictionary2 != null)
			{
				foreach (List<SessionStateAliasEntry> list3 in dictionary2.Values)
				{
					foreach (SessionStateAliasEntry sessionStateAliasEntry in list3)
					{
						list2.Add(new Tuple<string, string>(sessionStateAliasEntry.Name, sessionStateAliasEntry.Definition));
					}
				}
			}
			AppDomain.CurrentDomain.SetData("DetectedCmdlets", list);
			AppDomain.CurrentDomain.SetData("DetectedAliases", list2);
			AppDomain.CurrentDomain.SetData("AssemblyVersion", data);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000302E8 File Offset: 0x0002E4E8
		private PSModuleInfo AnalyzeScriptFile(string filename, bool force, ExecutionContext context)
		{
			PSModuleInfo psmoduleInfo = null;
			lock (ModuleCmdletBase.lockObject)
			{
				ModuleCmdletBase.scriptAnalysisCache.TryGetValue(filename, out psmoduleInfo);
			}
			if (psmoduleInfo != null)
			{
				return psmoduleInfo.Clone();
			}
			psmoduleInfo = new PSModuleInfo(filename, null, null);
			if (!force)
			{
				Dictionary<string, List<CommandTypes>> exportedCommands = AnalysisCache.GetExportedCommands(filename, true, context);
				if (exportedCommands != null)
				{
					foreach (KeyValuePair<string, List<CommandTypes>> keyValuePair in exportedCommands)
					{
						string key = keyValuePair.Key;
						Dictionary<string, List<CommandTypes>>.Enumerator enumerator;
						KeyValuePair<string, List<CommandTypes>> keyValuePair2 = enumerator.Current;
						List<CommandTypes> value = keyValuePair2.Value;
						if (key != null && value != null)
						{
							foreach (CommandTypes commandTypes in value)
							{
								if ((commandTypes & CommandTypes.Alias) == CommandTypes.Alias)
								{
									psmoduleInfo.AddDetectedAliasExport(key, null);
								}
								else if ((commandTypes & CommandTypes.Workflow) == CommandTypes.Workflow)
								{
									psmoduleInfo.AddDetectedWorkflowExport(key);
								}
								else if ((commandTypes & CommandTypes.Function) == CommandTypes.Function)
								{
									psmoduleInfo.AddDetectedFunctionExport(key);
								}
								else if ((commandTypes & CommandTypes.Cmdlet) == CommandTypes.Cmdlet)
								{
									psmoduleInfo.AddDetectedCmdletExport(key);
								}
								else
								{
									psmoduleInfo.AddDetectedFunctionExport(key);
								}
							}
						}
					}
					lock (ModuleCmdletBase.lockObject)
					{
						ModuleCmdletBase.scriptAnalysisCache[filename] = psmoduleInfo;
					}
					return psmoduleInfo;
				}
			}
			ScriptAnalysis scriptAnalysis = new ScriptAnalysis(filename, context);
			List<WildcardPattern> list = new List<WildcardPattern>();
			foreach (string pattern in scriptAnalysis.DiscoveredCommandFilters)
			{
				list.Add(new WildcardPattern(pattern));
			}
			foreach (string text in scriptAnalysis.DiscoveredExports)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(text, list, true) && !ModuleCmdletBase.HasInvalidCharacters(text.Replace("-", "")))
				{
					psmoduleInfo.AddDetectedFunctionExport(text);
				}
			}
			foreach (string text2 in scriptAnalysis.DiscoveredAliases.Keys)
			{
				if (!ModuleCmdletBase.HasInvalidCharacters(text2.Replace("-", "")))
				{
					psmoduleInfo.AddDetectedAliasExport(text2, scriptAnalysis.DiscoveredAliases[text2]);
				}
			}
			if (scriptAnalysis.AddsSelfToPath)
			{
				string directoryName = Path.GetDirectoryName(filename);
				try
				{
					foreach (string path in Directory.GetFiles(directoryName, "*.ps1"))
					{
						psmoduleInfo.AddDetectedFunctionExport(Path.GetFileNameWithoutExtension(path));
					}
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			foreach (RequiredModuleInfo requiredModuleInfo in scriptAnalysis.DiscoveredModules)
			{
				string text3 = requiredModuleInfo.Name;
				List<PSModuleInfo> list2 = new List<PSModuleInfo>();
				if (text3.IndexOfAny(Path.GetInvalidPathChars()) == -1 && Path.HasExtension(text3) && !Path.IsPathRooted(text3))
				{
					string directoryName2 = Path.GetDirectoryName(filename);
					text3 = Path.Combine(directoryName2, text3);
					PSModuleInfo psmoduleInfo2 = this.CreateModuleInfoForGetModule(text3, true);
					if (psmoduleInfo2 != null)
					{
						list2.Add(psmoduleInfo2);
					}
				}
				else
				{
					list2.AddRange(this.GetModule(new string[]
					{
						text3
					}, false, true));
				}
				if (list2 != null && list2.Count != 0)
				{
					List<WildcardPattern> list3 = new List<WildcardPattern>();
					foreach (string pattern2 in requiredModuleInfo.CommandsToPostFilter)
					{
						list3.Add(new WildcardPattern(pattern2));
					}
					foreach (PSModuleInfo psmoduleInfo3 in list2)
					{
						foreach (string text4 in psmoduleInfo3.ExportedFunctions.Keys)
						{
							if (SessionStateUtilities.MatchesAnyWildcardPattern(text4, list3, true) && SessionStateUtilities.MatchesAnyWildcardPattern(text4, list, true) && !ModuleCmdletBase.HasInvalidCharacters(text4.Replace("-", "")))
							{
								psmoduleInfo.AddDetectedFunctionExport(text4);
							}
						}
						foreach (string text5 in psmoduleInfo3.ExportedCmdlets.Keys)
						{
							if (SessionStateUtilities.MatchesAnyWildcardPattern(text5, list3, true) && SessionStateUtilities.MatchesAnyWildcardPattern(text5, list, true) && !ModuleCmdletBase.HasInvalidCharacters(text5.Replace("-", "")))
							{
								psmoduleInfo.AddDetectedCmdletExport(text5);
							}
						}
						foreach (string text6 in psmoduleInfo3.ExportedAliases.Keys)
						{
							if (SessionStateUtilities.MatchesAnyWildcardPattern(text6, list3, true) && SessionStateUtilities.MatchesAnyWildcardPattern(text6, list, true))
							{
								psmoduleInfo.AddDetectedAliasExport(text6, psmoduleInfo3.ExportedAliases[text6].Definition);
							}
						}
					}
				}
			}
			if (!psmoduleInfo.HadErrorsLoading)
			{
				AnalysisCache.CacheExportedCommands(psmoduleInfo, true, context);
			}
			else
			{
				ModuleIntrinsics.Tracer.WriteLine("Caching skipped for " + psmoduleInfo.Name + " because it had errors while loading.", new object[0]);
			}
			lock (ModuleCmdletBase.lockObject)
			{
				ModuleCmdletBase.scriptAnalysisCache[filename] = psmoduleInfo;
			}
			return psmoduleInfo;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000309E0 File Offset: 0x0002EBE0
		internal PSModuleInfo LoadBinaryModule(bool trySnapInName, string moduleName, string fileName, Assembly assemblyToLoad, string moduleBase, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, string prefix, bool loadTypes, bool loadFormats, out bool found)
		{
			return this.LoadBinaryModule(null, trySnapInName, moduleName, fileName, assemblyToLoad, moduleBase, ss, options, manifestProcessingFlags, prefix, loadTypes, loadFormats, out found, null, false);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00030A0C File Offset: 0x0002EC0C
		internal PSModuleInfo LoadBinaryModule(PSModuleInfo parentModule, bool trySnapInName, string moduleName, string fileName, Assembly assemblyToLoad, string moduleBase, SessionState ss, ModuleCmdletBase.ImportModuleOptions options, ModuleCmdletBase.ManifestProcessingFlags manifestProcessingFlags, string prefix, bool loadTypes, bool loadFormats, out bool found, string shortModuleName, bool disableFormatUpdates)
		{
			PSModuleInfo psmoduleInfo = null;
			if (string.IsNullOrEmpty(moduleName) && string.IsNullOrEmpty(fileName) && assemblyToLoad == null)
			{
				throw PSTraceSource.NewArgumentNullException("moduleName,fileName,assemblyToLoad");
			}
			InitialSessionState initialSessionState = InitialSessionState.Create();
			List<string> list = null;
			List<Tuple<string, string>> list2 = null;
			Assembly assembly = null;
			Exception ex = null;
			bool flag = false;
			string path = string.Empty;
			Version version = new Version(0, 0, 0, 0);
			if (assemblyToLoad != null)
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					path = fileName;
				}
				else
				{
					path = ClrFacade.GetAssemblyLocation(assemblyToLoad);
				}
				if (string.IsNullOrEmpty(moduleName))
				{
					moduleName = "dynamic_code_module_" + assemblyToLoad.GetName();
				}
				if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
				{
					if (parentModule != null && InitialSessionState.IsEngineModule(parentModule.Name))
					{
						initialSessionState.ImportCmdletsFromAssembly(assemblyToLoad, parentModule);
					}
					else
					{
						initialSessionState.ImportCmdletsFromAssembly(assemblyToLoad, null);
					}
				}
				version = ModuleCmdletBase.GetAssemblyVersionNumber(assemblyToLoad);
				assembly = assemblyToLoad;
			}
			else
			{
				if (moduleName != null && Utils.IsPowerShellAssembly(moduleName))
				{
					trySnapInName = false;
				}
				if (trySnapInName && PSSnapInInfo.IsPSSnapinIdValid(moduleName))
				{
					PSSnapInInfo pssnapInInfo = null;
					try
					{
						if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
						{
							PSSnapInException ex2;
							pssnapInInfo = initialSessionState.ImportPSSnapIn(moduleName, out ex2);
						}
					}
					catch (PSArgumentException)
					{
					}
					if (pssnapInInfo != null)
					{
						flag = true;
						if (string.IsNullOrEmpty(fileName))
						{
							path = pssnapInInfo.AbsoluteModulePath;
						}
						else
						{
							path = fileName;
						}
						version = pssnapInInfo.Version;
						if (!loadTypes)
						{
							initialSessionState.Types.Reset();
						}
						if (!loadFormats)
						{
							initialSessionState.Formats.Reset();
						}
						foreach (Assembly assembly2 in ClrFacade.GetAssemblies(null))
						{
							if (assembly2.GetName().FullName.Equals(pssnapInInfo.AssemblyName, StringComparison.Ordinal))
							{
								assembly = assembly2;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
					{
						assembly = base.Context.AddAssembly(moduleName, fileName, out ex);
						if (assembly == null)
						{
							if (ex != null)
							{
								throw ex;
							}
							found = false;
							return null;
						}
						else
						{
							version = ModuleCmdletBase.GetAssemblyVersionNumber(assembly);
							if (string.IsNullOrEmpty(fileName))
							{
								path = ClrFacade.GetAssemblyLocation(assembly);
							}
							else
							{
								path = fileName;
							}
							if (parentModule != null && InitialSessionState.IsEngineModule(parentModule.Name))
							{
								initialSessionState.ImportCmdletsFromAssembly(assembly, parentModule);
							}
							else
							{
								initialSessionState.ImportCmdletsFromAssembly(assembly, null);
							}
						}
					}
					else
					{
						string text = fileName;
						path = fileName;
						if (text == null)
						{
							text = Path.Combine(moduleBase, moduleName);
						}
						BinaryAnalysisResult cmdletsFromBinaryModuleImplementation = this.GetCmdletsFromBinaryModuleImplementation(text, manifestProcessingFlags, out version);
						list = cmdletsFromBinaryModuleImplementation.DetectedCmdlets;
						list2 = cmdletsFromBinaryModuleImplementation.DetectedAliases;
					}
				}
			}
			found = true;
			if (string.IsNullOrEmpty(shortModuleName))
			{
				psmoduleInfo = new PSModuleInfo(moduleName, path, base.Context, ss);
			}
			else
			{
				psmoduleInfo = new PSModuleInfo(shortModuleName, path, base.Context, ss);
			}
			psmoduleInfo.SetModuleType(ModuleType.Binary);
			psmoduleInfo.SetModuleBase(moduleBase);
			psmoduleInfo.SetVersion(version);
			psmoduleInfo.ImplementingAssembly = (assemblyToLoad ?? assembly);
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
			{
				this.SetModuleLoggingInformation(psmoduleInfo);
			}
			List<string> list3 = new List<string>();
			foreach (SessionStateTypeEntry sessionStateTypeEntry in ((IEnumerable<SessionStateTypeEntry>)initialSessionState.Types))
			{
				list3.Add(sessionStateTypeEntry.FileName);
			}
			if (list3.Count > 0)
			{
				psmoduleInfo.SetExportedTypeFiles(new ReadOnlyCollection<string>(list3));
			}
			List<string> list4 = new List<string>();
			foreach (SessionStateFormatEntry sessionStateFormatEntry in ((IEnumerable<SessionStateFormatEntry>)initialSessionState.Formats))
			{
				list4.Add(sessionStateFormatEntry.FileName);
			}
			if (list4.Count > 0)
			{
				psmoduleInfo.SetExportedFormatFiles(new ReadOnlyCollection<string>(list4));
			}
			foreach (SessionStateProviderEntry sessionStateProviderEntry in ((IEnumerable<SessionStateProviderEntry>)initialSessionState.Providers))
			{
				if (parentModule != null && InitialSessionState.IsEngineModule(parentModule.Name))
				{
					sessionStateProviderEntry.SetModule(parentModule);
				}
				else
				{
					sessionStateProviderEntry.SetModule(psmoduleInfo);
				}
			}
			if (initialSessionState.Commands != null)
			{
				foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
				{
					sessionStateCommandEntry.SetModule(psmoduleInfo);
					SessionStateCmdletEntry sessionStateCmdletEntry = sessionStateCommandEntry as SessionStateCmdletEntry;
					SessionStateAliasEntry sessionStateAliasEntry = null;
					if (sessionStateCmdletEntry == null)
					{
						sessionStateAliasEntry = (sessionStateCommandEntry as SessionStateAliasEntry);
					}
					if (ss != null)
					{
						if (sessionStateCmdletEntry != null)
						{
							ss.Internal.ExportedCmdlets.Add(CommandDiscovery.NewCmdletInfo(sessionStateCmdletEntry, base.Context));
						}
						else if (sessionStateAliasEntry != null)
						{
							ss.Internal.ExportedAliases.Add(CommandDiscovery.NewAliasInfo(sessionStateAliasEntry, base.Context));
						}
					}
					else if (sessionStateCmdletEntry != null)
					{
						psmoduleInfo.AddExportedCmdlet(CommandDiscovery.NewCmdletInfo(sessionStateCmdletEntry, base.Context));
					}
					else if (sessionStateAliasEntry != null)
					{
						psmoduleInfo.AddExportedAlias(CommandDiscovery.NewAliasInfo(sessionStateAliasEntry, base.Context));
					}
				}
			}
			if (list != null)
			{
				foreach (string cmdlet in list)
				{
					psmoduleInfo.AddDetectedCmdletExport(cmdlet);
				}
			}
			if (list2 != null)
			{
				foreach (Tuple<string, string> tuple in list2)
				{
					psmoduleInfo.AddDetectedAliasExport(tuple.Item1, tuple.Item2);
				}
			}
			if (this.BaseCmdletPatterns != null)
			{
				InitialSessionStateEntryCollection<SessionStateCommandEntry> commands = initialSessionState.Commands;
				for (int i = commands.Count - 1; i >= 0; i--)
				{
					SessionStateCommandEntry sessionStateCommandEntry2 = commands[i];
					if (sessionStateCommandEntry2 != null)
					{
						string name = sessionStateCommandEntry2.Name;
						if (!string.IsNullOrEmpty(name) && !SessionStateUtilities.MatchesAnyWildcardPattern(name, this.BaseCmdletPatterns, false))
						{
							commands.RemoveItem(i);
						}
					}
				}
			}
			foreach (SessionStateCommandEntry sessionStateCommandEntry3 in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				sessionStateCommandEntry3.Name = ModuleCmdletBase.AddPrefixToCommandName(sessionStateCommandEntry3.Name, prefix);
			}
			SessionStateInternal engineSessionState = base.Context.EngineSessionState;
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
			{
				try
				{
					if (ss != null)
					{
						base.Context.EngineSessionState = ss.Internal;
					}
					if (disableFormatUpdates)
					{
						initialSessionState.DisableFormatUpdates = true;
					}
					initialSessionState.Bind(base.Context, true, psmoduleInfo, options.NoClobber, options.Local);
					IEnumerable<Type> enumerable = new Type[0];
					if (assembly != null)
					{
						enumerable = assembly.ExportedTypes;
					}
					else if (assemblyToLoad != null)
					{
						enumerable = assemblyToLoad.ExportedTypes;
					}
					foreach (Type type in enumerable)
					{
						if (typeof(JobSourceAdapter).IsAssignableFrom(type) && typeof(JobSourceAdapter) != type && !base.JobManager.IsRegistered(type.Name))
						{
							base.JobManager.RegisterJobSourceAdapter(type);
						}
					}
				}
				finally
				{
					base.Context.EngineSessionState = engineSessionState;
				}
			}
			string str = psmoduleInfo.Name + "\\";
			bool flag2 = !this._disableNameChecking;
			bool flag3 = !this._disableNameChecking;
			foreach (SessionStateCommandEntry sessionStateCommandEntry4 in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry4._isImported)
				{
					try
					{
						if (sessionStateCommandEntry4 is SessionStateCmdletEntry || sessionStateCommandEntry4 is SessionStateFunctionEntry)
						{
							ModuleCmdletBase.ValidateCommandName(this, sessionStateCommandEntry4.Name, psmoduleInfo.Name, ref flag2, ref flag3);
						}
						string commandName = str + sessionStateCommandEntry4.Name;
						CommandInvocationIntrinsics.GetCmdlet(commandName, base.Context);
					}
					catch (CommandNotFoundException ex3)
					{
						base.WriteError(ex3.ErrorRecord);
					}
					if (!string.Equals(sessionStateCommandEntry4.Name, "import-psworkflow", StringComparison.OrdinalIgnoreCase))
					{
						string text2 = StringUtil.Format((sessionStateCommandEntry4.CommandType == CommandTypes.Alias) ? Modules.ImportingAlias : Modules.ImportingCmdlet, sessionStateCommandEntry4.Name);
						base.WriteVerbose(text2);
					}
				}
				else
				{
					string text3 = StringUtil.Format(Modules.ImportModuleNoClobberForCmdlet, sessionStateCommandEntry4.Name);
					base.WriteVerbose(text3);
				}
			}
			if ((manifestProcessingFlags & ModuleCmdletBase.ManifestProcessingFlags.LoadElements) == ModuleCmdletBase.ManifestProcessingFlags.LoadElements)
			{
				ModuleCmdletBase.AddModuleToModuleTables(base.Context, this.TargetSessionState.Internal, psmoduleInfo);
			}
			return psmoduleInfo;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000312B4 File Offset: 0x0002F4B4
		private static Version GetAssemblyVersionNumber(Assembly assemblyToLoad)
		{
			Version result;
			try
			{
				AssemblyName name = assemblyToLoad.GetName();
				result = name.Version;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				result = new Version(0, 0);
			}
			return result;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000312F4 File Offset: 0x0002F4F4
		internal static string AddPrefixToCommandName(string commandName, string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				return commandName;
			}
			string str;
			string str2;
			if (CmdletInfo.SplitCmdletName(commandName, out str, out str2))
			{
				commandName = str + "-" + prefix + str2;
			}
			else
			{
				commandName = prefix + commandName;
			}
			return commandName;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00031334 File Offset: 0x0002F534
		internal static string RemovePrefixFromCommandName(string commandName, string prefix)
		{
			string result = commandName;
			if (string.IsNullOrEmpty(prefix))
			{
				return result;
			}
			string str;
			string text;
			if (CmdletInfo.SplitCmdletName(commandName, out str, out text))
			{
				if (text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					string str2 = text.Substring(prefix.Length, text.Length - prefix.Length);
					result = str + "-" + str2;
				}
			}
			else if (commandName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			{
				result = commandName.Substring(prefix.Length, commandName.Length - prefix.Length);
			}
			return result;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000313B4 File Offset: 0x0002F5B4
		internal static bool IsPrefixedCommand(CommandInfo commandInfo)
		{
			string text;
			string text2;
			return CmdletInfo.SplitCmdletName(commandInfo.Name, out text, out text2) ? text2.StartsWith(commandInfo.Prefix, StringComparison.OrdinalIgnoreCase) : commandInfo.Name.StartsWith(commandInfo.Prefix, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000313F8 File Offset: 0x0002F5F8
		internal static void AddModuleToModuleTables(ExecutionContext context, SessionStateInternal targetSessionState, PSModuleInfo module)
		{
			string text;
			if (module.Path != "")
			{
				text = module.Path;
			}
			else
			{
				text = module.Name;
			}
			if (!context.Modules.ModuleTable.ContainsKey(text))
			{
				context.Modules.ModuleTable.Add(text, module);
			}
			if (context.previousModuleImported.ContainsKey(module.Name))
			{
				context.previousModuleImported.Remove(module.Name);
			}
			context.previousModuleImported.Add(module.Name, module.Path);
			if (!targetSessionState.ModuleTable.ContainsKey(text))
			{
				targetSessionState.ModuleTable.Add(text, module);
				targetSessionState.ModuleTableKeys.Add(text);
			}
			if (targetSessionState.Module != null)
			{
				targetSessionState.Module.AddNestedModule(module);
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000314C4 File Offset: 0x0002F6C4
		protected internal void ImportModuleMembers(PSModuleInfo sourceModule, string prefix)
		{
			ModuleCmdletBase.ImportModuleOptions options = default(ModuleCmdletBase.ImportModuleOptions);
			ModuleCmdletBase.ImportModuleMembers(this, this.TargetSessionState.Internal, sourceModule, prefix, this.BaseFunctionPatterns, this.BaseCmdletPatterns, this.BaseVariablePatterns, this.BaseAliasPatterns, options);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00031508 File Offset: 0x0002F708
		protected internal void ImportModuleMembers(PSModuleInfo sourceModule, string prefix, ModuleCmdletBase.ImportModuleOptions options)
		{
			ModuleCmdletBase.ImportModuleMembers(this, this.TargetSessionState.Internal, sourceModule, prefix, this.BaseFunctionPatterns, this.BaseCmdletPatterns, this.BaseVariablePatterns, this.BaseAliasPatterns, options);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00031544 File Offset: 0x0002F744
		internal static void ImportModuleMembers(ModuleCmdletBase cmdlet, SessionStateInternal targetSessionState, PSModuleInfo sourceModule, string prefix, List<WildcardPattern> functionPatterns, List<WildcardPattern> cmdletPatterns, List<WildcardPattern> variablePatterns, List<WildcardPattern> aliasPatterns, ModuleCmdletBase.ImportModuleOptions options)
		{
			if (sourceModule == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceModule");
			}
			bool flag = cmdlet.CommandInfo.Visibility == SessionStateEntryVisibility.Private || targetSessionState.DefaultCommandVisibility == SessionStateEntryVisibility.Private;
			bool flag2 = !string.IsNullOrEmpty(prefix);
			bool flag3 = !cmdlet.BaseDisableNameChecking;
			bool flag4 = !cmdlet.BaseDisableNameChecking;
			if (targetSessionState.Module != null)
			{
				bool flag5 = false;
				foreach (PSModuleInfo psmoduleInfo in targetSessionState.Module.NestedModules)
				{
					if (psmoduleInfo.Path.Equals(sourceModule.Path, StringComparison.OrdinalIgnoreCase))
					{
						flag5 = true;
					}
				}
				if (!flag5)
				{
					targetSessionState.Module.AddNestedModule(sourceModule);
				}
			}
			SessionStateInternal sessionStateInternal = null;
			if (sourceModule.SessionState != null)
			{
				sessionStateInternal = sourceModule.SessionState.Internal;
			}
			bool flag6 = functionPatterns == null && variablePatterns == null && aliasPatterns == null && cmdletPatterns == null;
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (CmdletInfo cmdletInfo in sourceModule.CompiledExports)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(cmdletInfo.Name, cmdletPatterns, flag6))
				{
					if (options.NoClobber && ModuleCmdletBase.CommandFound(cmdletInfo.Name, targetSessionState))
					{
						string text = StringUtil.Format(Modules.ImportModuleNoClobberForCmdlet, cmdletInfo.Name);
						cmdlet.WriteVerbose(text);
					}
					else
					{
						CmdletInfo cmdletInfo2 = new CmdletInfo(ModuleCmdletBase.AddPrefixToCommandName(cmdletInfo.Name, prefix), cmdletInfo.ImplementingType, cmdletInfo.HelpFile, cmdletInfo.PSSnapIn, cmdlet.Context);
						ModuleCmdletBase.SetCommandVisibility(flag, cmdletInfo2);
						cmdletInfo2.SetModule(sourceModule);
						if (flag2)
						{
							dictionary.Add(cmdletInfo.Name, cmdletInfo2.Name);
							cmdletInfo.Prefix = prefix;
							cmdletInfo2.Prefix = prefix;
						}
						ModuleCmdletBase.ValidateCommandName(cmdlet, cmdletInfo2.Name, sourceModule.Name, ref flag3, ref flag4);
						SessionStateScope sessionStateScope = options.Local ? targetSessionState.CurrentScope : targetSessionState.ModuleScope;
						sessionStateScope.AddCmdletToCache(cmdletInfo2.Name, cmdletInfo2, CommandOrigin.Internal, targetSessionState.ExecutionContext);
						cmdletInfo.IsImported = true;
						string text = StringUtil.Format(Modules.ImportingCmdlet, cmdletInfo2.Name);
						cmdlet.WriteVerbose(text);
					}
				}
			}
			foreach (AliasInfo aliasInfo in sourceModule.CompiledAliasExports)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(aliasInfo.Name, aliasPatterns, flag6))
				{
					string text2 = ModuleCmdletBase.AddPrefixToCommandName(aliasInfo.Name, prefix);
					string definition;
					if (!flag2 || !dictionary.TryGetValue(aliasInfo.Definition, out definition))
					{
						definition = aliasInfo.Definition;
					}
					if (options.NoClobber && ModuleCmdletBase.CommandFound(text2, targetSessionState))
					{
						string text = StringUtil.Format(Modules.ImportModuleNoClobberForAlias, text2);
						cmdlet.WriteVerbose(text);
					}
					else
					{
						AliasInfo aliasInfo2 = new AliasInfo(text2, definition, cmdlet.Context);
						ModuleCmdletBase.SetCommandVisibility(flag, aliasInfo2);
						aliasInfo2.SetModule(sourceModule);
						if (flag2)
						{
							if (!dictionary.ContainsKey(aliasInfo.Name))
							{
								dictionary.Add(aliasInfo.Name, aliasInfo2.Name);
							}
							aliasInfo.Prefix = prefix;
							aliasInfo2.Prefix = prefix;
						}
						SessionStateScope sessionStateScope2 = options.Local ? targetSessionState.CurrentScope : targetSessionState.ModuleScope;
						sessionStateScope2.SetAliasItem(aliasInfo2, false, CommandOrigin.Internal);
						aliasInfo.IsImported = true;
						string text = StringUtil.Format(Modules.ImportingAlias, aliasInfo2.Name);
						cmdlet.WriteVerbose(text);
					}
				}
			}
			if (sessionStateInternal != null)
			{
				foreach (FunctionInfo func in sourceModule.ExportedFunctions.Values)
				{
					ModuleCmdletBase.ImportFunctionsOrWorkflows(func, targetSessionState, sourceModule, functionPatterns, flag6, prefix, options, flag2, ref flag3, ref flag4, dictionary, cmdlet, flag, true);
				}
				foreach (FunctionInfo func2 in sourceModule.ExportedWorkflows.Values)
				{
					ModuleCmdletBase.ImportFunctionsOrWorkflows(func2, targetSessionState, sourceModule, functionPatterns, flag6, prefix, options, flag2, ref flag3, ref flag4, dictionary, cmdlet, flag, false);
				}
				foreach (PSVariable psvariable in sourceModule.ExportedVariables.Values)
				{
					if (SessionStateUtilities.MatchesAnyWildcardPattern(psvariable.Name, variablePatterns, flag6))
					{
						if (options.NoClobber && targetSessionState.ModuleScope.GetVariable(psvariable.Name) != null)
						{
							string text = StringUtil.Format(Modules.ImportModuleNoClobberForVariable, psvariable.Name);
							cmdlet.WriteVerbose(text);
						}
						else
						{
							psvariable.SetModule(sourceModule);
							SessionStateScope sessionStateScope3 = options.Local ? targetSessionState.CurrentScope : targetSessionState.ModuleScope;
							PSVariable psvariable2 = sessionStateScope3.NewVariable(psvariable, true, sessionStateInternal);
							if (flag)
							{
								psvariable2.Visibility = SessionStateEntryVisibility.Private;
							}
							string text = StringUtil.Format(Modules.ImportingVariable, psvariable.Name);
							cmdlet.WriteVerbose(text);
						}
					}
				}
				foreach (AliasInfo aliasInfo3 in sourceModule.ExportedAliases.Values)
				{
					if (SessionStateUtilities.MatchesAnyWildcardPattern(aliasInfo3.Name, aliasPatterns, flag6))
					{
						string text3 = ModuleCmdletBase.AddPrefixToCommandName(aliasInfo3.Name, prefix);
						string definition2;
						if (!flag2 || !dictionary.TryGetValue(aliasInfo3.Definition, out definition2))
						{
							definition2 = aliasInfo3.Definition;
						}
						if (options.NoClobber && ModuleCmdletBase.CommandFound(text3, targetSessionState))
						{
							string text = StringUtil.Format(Modules.ImportModuleNoClobberForAlias, text3);
							cmdlet.WriteVerbose(text);
						}
						else
						{
							AliasInfo aliasInfo4 = new AliasInfo(text3, definition2, cmdlet.Context);
							ModuleCmdletBase.SetCommandVisibility(flag, aliasInfo4);
							aliasInfo4.SetModule(sourceModule);
							if (flag2)
							{
								if (!dictionary.ContainsKey(aliasInfo3.Name))
								{
									dictionary.Add(aliasInfo3.Name, aliasInfo4.Name);
								}
								aliasInfo3.Prefix = prefix;
								aliasInfo4.Prefix = prefix;
							}
							SessionStateScope sessionStateScope4 = options.Local ? targetSessionState.CurrentScope : targetSessionState.ModuleScope;
							sessionStateScope4.SetAliasItem(aliasInfo4, false, CommandOrigin.Internal);
							aliasInfo3.IsImported = true;
							string text = StringUtil.Format(Modules.ImportingAlias, aliasInfo4.Name);
							cmdlet.WriteVerbose(text);
						}
					}
				}
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00031C5C File Offset: 0x0002FE5C
		private static void ImportFunctionsOrWorkflows(FunctionInfo func, SessionStateInternal targetSessionState, PSModuleInfo sourceModule, List<WildcardPattern> functionPatterns, bool noPatternsSpecified, string prefix, ModuleCmdletBase.ImportModuleOptions options, bool usePrefix, ref bool checkVerb, ref bool checkNoun, Dictionary<string, string> original2prefixedName, ModuleCmdletBase cmdlet, bool isImportModulePrivate, bool isFunction)
		{
			if (SessionStateUtilities.MatchesAnyWildcardPattern(func.Name, functionPatterns, noPatternsSpecified))
			{
				string text = ModuleCmdletBase.AddPrefixToCommandName(func.Name, prefix);
				string text2;
				if (options.NoClobber && ModuleCmdletBase.CommandFound(text, targetSessionState))
				{
					text2 = StringUtil.Format(Modules.ImportModuleNoClobberForFunction, func.Name);
					cmdlet.WriteVerbose(text2);
					return;
				}
				SessionStateScope sessionStateScope = options.Local ? targetSessionState.CurrentScope : targetSessionState.ModuleScope;
				FunctionInfo functionInfo = sessionStateScope.SetFunction(text, func.ScriptBlock, func, false, CommandOrigin.Internal, targetSessionState.ExecutionContext);
				ModuleCmdletBase.SetCommandVisibility(isImportModulePrivate, functionInfo);
				functionInfo.SetModule(sourceModule);
				func.IsImported = true;
				if (usePrefix)
				{
					original2prefixedName.Add(func.Name, text);
					func.Prefix = prefix;
					functionInfo.Prefix = prefix;
				}
				ModuleCmdletBase.ValidateCommandName(cmdlet, functionInfo.Name, sourceModule.Name, ref checkNoun, ref checkVerb);
				if (func.CommandType == CommandTypes.Workflow)
				{
					text2 = StringUtil.Format(Modules.ImportingWorkflow, text);
				}
				else
				{
					text2 = StringUtil.Format(Modules.ImportingFunction, text);
				}
				cmdlet.WriteVerbose(text2);
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00031D65 File Offset: 0x0002FF65
		private static void SetCommandVisibility(bool isImportModulePrivate, CommandInfo command)
		{
			if (isImportModulePrivate)
			{
				command.Visibility = SessionStateEntryVisibility.Private;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00031D74 File Offset: 0x0002FF74
		internal static bool CommandFound(string commandName, SessionStateInternal sessionStateInternal)
		{
			EventHandler<CommandLookupEventArgs> commandNotFoundAction = sessionStateInternal.ExecutionContext.EngineIntrinsics.InvokeCommand.CommandNotFoundAction;
			bool result;
			try
			{
				sessionStateInternal.ExecutionContext.EngineIntrinsics.InvokeCommand.CommandNotFoundAction = null;
				CommandSearcher commandSearcher = new CommandSearcher(commandName, SearchResolutionOptions.ResolveAliasPatterns | SearchResolutionOptions.ResolveFunctionPatterns | SearchResolutionOptions.CommandNameIsPattern, CommandTypes.Alias | CommandTypes.Function | CommandTypes.Cmdlet | CommandTypes.Configuration, sessionStateInternal.ExecutionContext);
				if (!commandSearcher.MoveNext())
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			finally
			{
				sessionStateInternal.ExecutionContext.EngineIntrinsics.InvokeCommand.CommandNotFoundAction = commandNotFoundAction;
			}
			return result;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00031DF8 File Offset: 0x0002FFF8
		private static bool HasInvalidCharacters(string commandName)
		{
			int i = 0;
			while (i < commandName.Length)
			{
				char c = commandName[i];
				char c2 = c;
				switch (c2)
				{
				case '"':
				case '#':
				case '$':
				case '%':
				case '&':
				case '\'':
				case '(':
				case ')':
				case '*':
				case '+':
				case ',':
				case '-':
				case '/':
				case ':':
				case ';':
				case '<':
				case '=':
				case '>':
				case '?':
				case '@':
					goto IL_D8;
				case '.':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					break;
				default:
					switch (c2)
					{
					case '[':
					case '\\':
					case ']':
					case '^':
					case '`':
						goto IL_D8;
					case '_':
						break;
					default:
						switch (c2)
						{
						case '{':
						case '|':
						case '}':
						case '~':
							goto IL_D8;
						}
						break;
					}
					break;
				}
				i++;
				continue;
				IL_D8:
				return true;
			}
			return false;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00031EF4 File Offset: 0x000300F4
		private static void ValidateCommandName(ModuleCmdletBase cmdlet, string commandName, string moduleName, ref bool checkVerb, ref bool checkNoun)
		{
			string verb;
			string commandName2;
			if (!CmdletInfo.SplitCmdletName(commandName, out verb, out commandName2))
			{
				return;
			}
			if (!Verbs.IsStandard(verb) && !commandName.Equals("Sort-Object", StringComparison.OrdinalIgnoreCase) && !commandName.Equals("Tee-Object", StringComparison.OrdinalIgnoreCase))
			{
				if (checkVerb)
				{
					checkVerb = false;
					string text = StringUtil.Format(Modules.ImportingNonStandardVerb, moduleName);
					cmdlet.WriteWarning(text);
				}
				string[] array = Verbs.SuggestedAlternates(verb);
				if (array == null)
				{
					string text = StringUtil.Format(Modules.ImportingNonStandardVerbVerbose, commandName, moduleName);
					cmdlet.WriteVerbose(text);
				}
				else
				{
					string text2 = string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, array);
					string text = StringUtil.Format(Modules.ImportingNonStandardVerbVerboseSuggestion, new object[]
					{
						commandName,
						text2,
						moduleName
					});
					cmdlet.WriteVerbose(text);
				}
			}
			if (ModuleCmdletBase.HasInvalidCharacters(commandName2))
			{
				string text;
				if (checkNoun)
				{
					text = Modules.ImportingNonStandardNoun;
					cmdlet.WriteWarning(text);
					checkNoun = false;
				}
				text = StringUtil.Format(Modules.ImportingNonStandardNounVerbose, commandName, moduleName);
				cmdlet.WriteVerbose(text);
			}
		}

		// Token: 0x0400039F RID: 927
		private string _prefix = string.Empty;

		// Token: 0x040003A0 RID: 928
		private bool _force;

		// Token: 0x040003A1 RID: 929
		private bool _global;

		// Token: 0x040003A2 RID: 930
		private bool _passThru;

		// Token: 0x040003A3 RID: 931
		private bool _baseAsCustomObject;

		// Token: 0x040003A4 RID: 932
		private List<WildcardPattern> _functionPatterns;

		// Token: 0x040003A5 RID: 933
		private List<WildcardPattern> _cmdletPatterns;

		// Token: 0x040003A6 RID: 934
		private List<WildcardPattern> _variablePatterns;

		// Token: 0x040003A7 RID: 935
		private List<WildcardPattern> _aliasPatterns;

		// Token: 0x040003A8 RID: 936
		private Version _minimumVersion;

		// Token: 0x040003A9 RID: 937
		private Version _maximumVersion;

		// Token: 0x040003AA RID: 938
		private Version _requiredVersion;

		// Token: 0x040003AB RID: 939
		private object[] _arguments;

		// Token: 0x040003AC RID: 940
		private bool _disableNameChecking = true;

		// Token: 0x040003AD RID: 941
		private bool _addToAppDomainLevelCache;

		// Token: 0x040003AE RID: 942
		private List<WildcardPattern> _matchAll;

		// Token: 0x040003AF RID: 943
		internal static string[] PermittedCmdlets = new string[]
		{
			"Import-LocalizedData",
			"ConvertFrom-StringData",
			"Write-Host",
			"Out-Host",
			"Join-Path"
		};

		// Token: 0x040003B0 RID: 944
		internal static string[] ModuleManifestMembers = new string[]
		{
			"ModuleToProcess",
			"NestedModules",
			"GUID",
			"Author",
			"CompanyName",
			"Copyright",
			"ModuleVersion",
			"Description",
			"PowerShellVersion",
			"PowerShellHostName",
			"PowerShellHostVersion",
			"CLRVersion",
			"DotNetFrameworkVersion",
			"ProcessorArchitecture",
			"RequiredModules",
			"TypesToProcess",
			"FormatsToProcess",
			"ScriptsToProcess",
			"PrivateData",
			"RequiredAssemblies",
			"ModuleList",
			"FileList",
			"FunctionsToExport",
			"VariablesToExport",
			"AliasesToExport",
			"CmdletsToExport",
			"DscResourcesToExport",
			"HelpInfoURI",
			"RootModule",
			"DefaultCommandPrefix"
		};

		// Token: 0x040003B1 RID: 945
		private static string[] ModuleVersionMembers = new string[]
		{
			"ModuleName",
			"GUID",
			"ModuleVersion"
		};

		// Token: 0x040003B2 RID: 946
		private static List<string> ServiceCoreAssemblyCmdlets = new List<string>(new string[]
		{
			"Microsoft.PowerShell.Workflow.ServiceCore\\Import-PSWorkflow",
			"Microsoft.PowerShell.Workflow.ServiceCore\\New-PSWorkflowExecutionOption"
		});

		// Token: 0x040003B3 RID: 947
		private Dictionary<string, PSModuleInfo> currentlyProcessingModules = new Dictionary<string, PSModuleInfo>();

		// Token: 0x040003B4 RID: 948
		private readonly string ServiceCoreAssemblyFullName = "Microsoft.Powershell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";

		// Token: 0x040003B5 RID: 949
		private readonly string ServiceCoreAssemblyShortName = "Microsoft.Powershell.Workflow.ServiceCore";

		// Token: 0x040003B6 RID: 950
		private static object lockObject = new object();

		// Token: 0x040003B7 RID: 951
		private static Dictionary<string, Tuple<BinaryAnalysisResult, Version>> binaryAnalysisCache = new Dictionary<string, Tuple<BinaryAnalysisResult, Version>>();

		// Token: 0x040003B8 RID: 952
		private static Dictionary<string, PSModuleInfo> scriptAnalysisCache = new Dictionary<string, PSModuleInfo>();

		// Token: 0x020000A6 RID: 166
		[Flags]
		internal enum ManifestProcessingFlags
		{
			// Token: 0x040003BF RID: 959
			WriteErrors = 1,
			// Token: 0x040003C0 RID: 960
			NullOnFirstError = 2,
			// Token: 0x040003C1 RID: 961
			LoadElements = 4,
			// Token: 0x040003C2 RID: 962
			WriteWarnings = 8,
			// Token: 0x040003C3 RID: 963
			Force = 16,
			// Token: 0x040003C4 RID: 964
			IgnoreHostNameAndHostVersion = 32
		}

		// Token: 0x020000A7 RID: 167
		protected internal struct ImportModuleOptions
		{
			// Token: 0x040003C5 RID: 965
			internal bool NoClobber;

			// Token: 0x040003C6 RID: 966
			internal bool Local;

			// Token: 0x040003C7 RID: 967
			internal bool ServiceCoreAutoAdded;
		}

		// Token: 0x020000A8 RID: 168
		[Flags]
		internal enum ModuleLoggingGroupPolicyStatus
		{
			// Token: 0x040003C9 RID: 969
			Undefined = 0,
			// Token: 0x040003CA RID: 970
			Enabled = 1,
			// Token: 0x040003CB RID: 971
			Disabled = 2
		}
	}
}
