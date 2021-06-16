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
using System.Reflection;
using System.Security;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000AA RID: 170
	[OutputType(new Type[]
	{
		typeof(PSModuleInfo)
	})]
	[Cmdlet("Import", "Module", DefaultParameterSetName = "Name", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141553")]
	public sealed class ImportModuleCommand : ModuleCmdletBase, IDisposable
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00032D19 File Offset: 0x00030F19
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00032D0B File Offset: 0x00030F0B
		[Parameter]
		public SwitchParameter Global
		{
			get
			{
				return base.BaseGlobal;
			}
			set
			{
				base.BaseGlobal = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00032D2F File Offset: 0x00030F2F
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00032D26 File Offset: 0x00030F26
		[ValidateNotNull]
		[Parameter]
		public string Prefix
		{
			get
			{
				return base.BasePrefix;
			}
			set
			{
				base.BasePrefix = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00032D40 File Offset: 0x00030F40
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x00032D37 File Offset: 0x00030F37
		[Parameter(ParameterSetName = "CimSession", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		[Parameter(ParameterSetName = "Name", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		[Parameter(ParameterSetName = "PSSession", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		public string[] Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00032D48 File Offset: 0x00030F48
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00032D50 File Offset: 0x00030F50
		[Parameter(ParameterSetName = "FullyQualifiedName", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		[Parameter(ParameterSetName = "FullyQualifiedNameAndPSSession", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		public ModuleSpecification[] FullyQualifiedName { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00032D59 File Offset: 0x00030F59
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00032D61 File Offset: 0x00030F61
		[Parameter(ParameterSetName = "Assembly", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		public Assembly[] Assembly { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00032DBA File Offset: 0x00030FBA
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x00032D6C File Offset: 0x00030F6C
		[Parameter]
		[ValidateNotNull]
		public string[] Function
		{
			get
			{
				return this._functionImportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._functionImportList = value;
				base.BaseFunctionPatterns = new List<WildcardPattern>();
				foreach (string pattern in this._functionImportList)
				{
					base.BaseFunctionPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00032E12 File Offset: 0x00031012
		// (set) Token: 0x06000888 RID: 2184 RVA: 0x00032DC4 File Offset: 0x00030FC4
		[ValidateNotNull]
		[Parameter]
		public string[] Cmdlet
		{
			get
			{
				return this._cmdletImportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._cmdletImportList = value;
				base.BaseCmdletPatterns = new List<WildcardPattern>();
				foreach (string pattern in this._cmdletImportList)
				{
					base.BaseCmdletPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00032E6A File Offset: 0x0003106A
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x00032E1C File Offset: 0x0003101C
		[Parameter]
		[ValidateNotNull]
		public string[] Variable
		{
			get
			{
				return this._variableExportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._variableExportList = value;
				base.BaseVariablePatterns = new List<WildcardPattern>();
				foreach (string pattern in this._variableExportList)
				{
					base.BaseVariablePatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00032EC2 File Offset: 0x000310C2
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x00032E74 File Offset: 0x00031074
		[Parameter]
		[ValidateNotNull]
		public string[] Alias
		{
			get
			{
				return this._aliasExportList;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._aliasExportList = value;
				base.BaseAliasPatterns = new List<WildcardPattern>();
				foreach (string pattern in this._aliasExportList)
				{
					base.BaseAliasPatterns.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00032ECA File Offset: 0x000310CA
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00032ED7 File Offset: 0x000310D7
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return base.BaseForce;
			}
			set
			{
				base.BaseForce = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00032EE5 File Offset: 0x000310E5
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00032EF2 File Offset: 0x000310F2
		[Parameter]
		public SwitchParameter PassThru
		{
			get
			{
				return base.BasePassThru;
			}
			set
			{
				base.BasePassThru = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00032F00 File Offset: 0x00031100
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00032F0D File Offset: 0x0003110D
		[Parameter]
		public SwitchParameter AsCustomObject
		{
			get
			{
				return base.BaseAsCustomObject;
			}
			set
			{
				base.BaseAsCustomObject = value;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00032F1B File Offset: 0x0003111B
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00032F23 File Offset: 0x00031123
		[Parameter(ParameterSetName = "PSSession")]
		[Alias(new string[]
		{
			"Version"
		})]
		[Parameter(ParameterSetName = "CimSession")]
		[Parameter(ParameterSetName = "Name")]
		public Version MinimumVersion
		{
			get
			{
				return base.BaseMinimumVersion;
			}
			set
			{
				base.BaseMinimumVersion = value;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00032F2C File Offset: 0x0003112C
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00032F49 File Offset: 0x00031149
		[Parameter(ParameterSetName = "PSSession")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "CimSession")]
		public string MaximumVersion
		{
			get
			{
				if (base.BaseMaximumVersion == null)
				{
					return null;
				}
				return base.BaseMaximumVersion.ToString();
			}
			set
			{
				if (value == null)
				{
					base.BaseMaximumVersion = null;
					return;
				}
				base.BaseMaximumVersion = ModuleCmdletBase.GetMaximumVersion(value);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00032F62 File Offset: 0x00031162
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00032F6A File Offset: 0x0003116A
		[Parameter(ParameterSetName = "CimSession")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "PSSession")]
		public Version RequiredVersion
		{
			get
			{
				return base.BaseRequiredVersion;
			}
			set
			{
				base.BaseRequiredVersion = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00032F7C File Offset: 0x0003117C
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00032F73 File Offset: 0x00031173
		[Parameter(ParameterSetName = "ModuleInfo", Mandatory = true, ValueFromPipeline = true, Position = 0)]
		public PSModuleInfo[] ModuleInfo
		{
			get
			{
				return this._moduleInfo;
			}
			set
			{
				this._moduleInfo = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00032F84 File Offset: 0x00031184
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00032F8C File Offset: 0x0003118C
		[Alias(new string[]
		{
			"Args"
		})]
		[Parameter]
		public object[] ArgumentList
		{
			get
			{
				return base.BaseArgumentList;
			}
			set
			{
				base.BaseArgumentList = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00032F95 File Offset: 0x00031195
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00032FA2 File Offset: 0x000311A2
		[Parameter]
		public SwitchParameter DisableNameChecking
		{
			get
			{
				return base.BaseDisableNameChecking;
			}
			set
			{
				base.BaseDisableNameChecking = value;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00032FB0 File Offset: 0x000311B0
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00032FB8 File Offset: 0x000311B8
		[Parameter]
		[Alias(new string[]
		{
			"NoOverwrite"
		})]
		public SwitchParameter NoClobber { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00032FC1 File Offset: 0x000311C1
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00032FC9 File Offset: 0x000311C9
		[ValidateSet(new string[]
		{
			"Local",
			"Global"
		})]
		[Parameter]
		public string Scope
		{
			get
			{
				return this._scope;
			}
			set
			{
				this._scope = value;
				this._isScopeSpecified = true;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00032FD9 File Offset: 0x000311D9
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00032FE1 File Offset: 0x000311E1
		[ValidateNotNull]
		[Parameter(ParameterSetName = "PSSession", Mandatory = true)]
		[Parameter(ParameterSetName = "FullyQualifiedNameAndPSSession", Mandatory = true)]
		public PSSession PSSession { get; set; }

		// Token: 0x060008A6 RID: 2214 RVA: 0x00032FEC File Offset: 0x000311EC
		public ImportModuleCommand()
		{
			base.BaseDisableNameChecking = false;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0003304C File Offset: 0x0003124C
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00033054 File Offset: 0x00031254
		[Parameter(ParameterSetName = "CimSession", Mandatory = true)]
		[ValidateNotNull]
		public CimSession CimSession { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0003305D File Offset: 0x0003125D
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00033065 File Offset: 0x00031265
		[Parameter(ParameterSetName = "CimSession", Mandatory = false)]
		[ValidateNotNull]
		public Uri CimResourceUri { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0003306E File Offset: 0x0003126E
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x00033076 File Offset: 0x00031276
		[Parameter(ParameterSetName = "CimSession", Mandatory = false)]
		[ValidateNotNullOrEmpty]
		public string CimNamespace { get; set; }

		// Token: 0x060008AD RID: 2221 RVA: 0x00033080 File Offset: 0x00031280
		private void ImportModule_ViaLocalModuleInfo(ModuleCmdletBase.ImportModuleOptions importModuleOptions, PSModuleInfo module)
		{
			try
			{
				PSModuleInfo psmoduleInfo = null;
				base.Context.Modules.ModuleTable.TryGetValue(module.Path, out psmoduleInfo);
				if (!base.BaseForce && base.IsModuleAlreadyLoaded(psmoduleInfo))
				{
					ModuleCmdletBase.AddModuleToModuleTables(base.Context, base.TargetSessionState.Internal, psmoduleInfo);
					base.ImportModuleMembers(psmoduleInfo, base.BasePrefix, importModuleOptions);
					if (base.BaseAsCustomObject)
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
					else if (base.BasePassThru)
					{
						base.WriteObject(psmoduleInfo);
					}
				}
				else
				{
					PSModuleInfo module2;
					if (base.Context.Modules.ModuleTable.TryGetValue(module.Path, out module2))
					{
						base.RemoveModule(module2);
					}
					try
					{
						if (module.SessionState == null)
						{
							if (File.Exists(module.Path))
							{
								bool flag;
								PSModuleInfo psmoduleInfo2 = base.LoadModule(module.Path, null, base.BasePrefix, null, ref importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out flag);
							}
						}
						else if (!string.IsNullOrEmpty(module.Name))
						{
							ModuleCmdletBase.AddModuleToModuleTables(base.Context, base.TargetSessionState.Internal, module);
							if (module.SessionState != null)
							{
								base.ImportModuleMembers(module, base.BasePrefix, importModuleOptions);
							}
							if (base.BaseAsCustomObject && module.SessionState != null)
							{
								base.WriteObject(module.AsCustomObject());
							}
							else if (base.BasePassThru)
							{
								base.WriteObject(module);
							}
						}
					}
					catch (IOException)
					{
					}
				}
			}
			catch (PSInvalidOperationException ex)
			{
				ErrorRecord errorRecord2 = new ErrorRecord(ex.ErrorRecord, ex);
				base.WriteError(errorRecord2);
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00033278 File Offset: 0x00031478
		private void ImportModule_ViaAssembly(ModuleCmdletBase.ImportModuleOptions importModuleOptions, Assembly suppliedAssembly)
		{
			bool flag = false;
			if (suppliedAssembly != null && base.Context.Modules.ModuleTable != null)
			{
				foreach (KeyValuePair<string, PSModuleInfo> keyValuePair in base.Context.Modules.ModuleTable)
				{
					string value = "dynamic_code_module_" + suppliedAssembly;
					if (keyValuePair.Value.Path == "")
					{
						if (keyValuePair.Key.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							if (base.BasePassThru)
							{
								base.WriteObject(keyValuePair.Value);
								break;
							}
							break;
						}
					}
					else if (keyValuePair.Value.Path.Equals(ClrFacade.GetAssemblyLocation(suppliedAssembly), StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						if (base.BasePassThru)
						{
							base.WriteObject(keyValuePair.Value);
							break;
						}
						break;
					}
				}
			}
			if (!flag)
			{
				bool flag2;
				PSModuleInfo psmoduleInfo = base.LoadBinaryModule(false, null, null, suppliedAssembly, null, null, importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, base.BasePrefix, false, false, out flag2);
				if (flag2 && psmoduleInfo != null)
				{
					ModuleCmdletBase.AddModuleToModuleTables(base.Context, base.TargetSessionState.Internal, psmoduleInfo);
					if (base.BasePassThru)
					{
						base.WriteObject(psmoduleInfo);
					}
				}
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000333C4 File Offset: 0x000315C4
		private PSModuleInfo ImportModule_LocallyViaName(ModuleCmdletBase.ImportModuleOptions importModuleOptions, string name)
		{
			try
			{
				if (name.Equals("PSWorkflow", StringComparison.OrdinalIgnoreCase) && Utils.IsRunningFromSysWOW64())
				{
					throw new NotSupportedException(AutomationExceptions.WorkflowDoesNotSupportWOW64);
				}
				bool flag = false;
				PSModuleInfo result = null;
				string text = null;
				string text2 = null;
				if (this.MinimumVersion == null && this.MaximumVersion == null && this.RequiredVersion == null && PSModuleInfo.UseAppDomainLevelModuleCache && !base.BaseForce)
				{
					text = PSModuleInfo.ResolveUsingAppDomainLevelModuleCache(name);
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (File.Exists(text))
					{
						text2 = text;
					}
					else
					{
						PSModuleInfo.RemoveFromAppDomainLevelCache(name);
					}
				}
				if (text2 == null)
				{
					text2 = ModuleCmdletBase.ResolveRootedFilePath(name, base.Context);
				}
				bool flag2 = false;
				if (!string.IsNullOrEmpty(text2))
				{
					if (!base.BaseForce && base.Context.Modules.ModuleTable.ContainsKey(text2))
					{
						PSModuleInfo psmoduleInfo = base.Context.Modules.ModuleTable[text2];
						if (this.RequiredVersion == null || psmoduleInfo.Version.Equals(this.RequiredVersion) || (base.BaseMinimumVersion == null && base.BaseMaximumVersion == null) || (psmoduleInfo.ModuleType != ModuleType.Manifest || (base.BaseMinimumVersion == null && base.BaseMaximumVersion != null && psmoduleInfo.Version <= base.BaseMaximumVersion)) || (base.BaseMinimumVersion != null && base.BaseMaximumVersion == null && psmoduleInfo.Version >= base.BaseMinimumVersion) || (base.BaseMinimumVersion != null && base.BaseMaximumVersion != null && psmoduleInfo.Version >= base.BaseMinimumVersion && psmoduleInfo.Version <= base.BaseMaximumVersion))
						{
							flag2 = true;
							ModuleCmdletBase.AddModuleToModuleTables(base.Context, base.TargetSessionState.Internal, psmoduleInfo);
							base.ImportModuleMembers(psmoduleInfo, base.BasePrefix, importModuleOptions);
							if (base.BaseAsCustomObject)
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
							else if (base.BasePassThru)
							{
								base.WriteObject(psmoduleInfo);
							}
							flag = true;
							result = psmoduleInfo;
						}
					}
					if (!flag2)
					{
						if (File.Exists(text2))
						{
							PSModuleInfo module;
							if (base.Context.Modules.ModuleTable.TryGetValue(text2, out module))
							{
								base.RemoveModule(module);
							}
							result = base.LoadModule(text2, null, base.BasePrefix, null, ref importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out flag);
						}
						else if (Directory.Exists(text2))
						{
							result = base.LoadUsingMultiVersionModuleBase(text2, importModuleOptions, out flag);
							if (!flag)
							{
								text2 = Path.Combine(text2, Path.GetFileName(text2));
								result = base.LoadUsingExtensions(null, text2, text2, null, null, base.BasePrefix, null, importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out flag);
							}
						}
					}
				}
				else
				{
					if (InitialSessionState.IsEngineModule(name))
					{
						CmdletInfo cmdlet = base.Context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Get-PSSnapIn");
						if (cmdlet != null && cmdlet.Visibility == SessionStateEntryVisibility.Public)
						{
							CommandInfo commandInfo = new CmdletInfo("Get-PSSnapIn", typeof(GetPSSnapinCommand), null, null, base.Context);
							Command command = new Command(commandInfo);
							Collection<PSSnapInInfo> collection = null;
							try
							{
								using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
								{
									powerShell.AddCommand(command).AddParameter("Name", name).AddParameter("ErrorAction", ActionPreference.Ignore);
									collection = powerShell.Invoke<PSSnapInInfo>();
								}
							}
							catch (Exception e)
							{
								CommandProcessorBase.CheckForSevereException(e);
							}
							if (collection != null && collection.Count == 1)
							{
								string text3 = string.Format(CultureInfo.InvariantCulture, Modules.ModuleLoadedAsASnapin, new object[]
								{
									collection[0].Name
								});
								base.WriteWarning(text3);
								flag = true;
								return result;
							}
						}
					}
					if (ModuleCmdletBase.IsRooted(name))
					{
						if (!string.IsNullOrEmpty(Path.GetExtension(name)))
						{
							result = base.LoadModule(name, null, base.BasePrefix, null, ref importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out flag);
						}
						else
						{
							result = base.LoadUsingExtensions(null, name, name, null, null, base.BasePrefix, null, importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out flag);
						}
					}
					else
					{
						IEnumerable<string> modulePath = ModuleIntrinsics.GetModulePath(false, base.Context);
						if (this.MinimumVersion == null && this.RequiredVersion == null && this.MaximumVersion == null)
						{
							base.AddToAppDomainLevelCache = true;
						}
						flag = base.LoadUsingModulePath(flag, modulePath, name, null, importModuleOptions, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out result);
					}
				}
				if (!flag)
				{
					string message2 = null;
					if (base.BaseRequiredVersion != null)
					{
						message2 = StringUtil.Format(Modules.ModuleWithVersionNotFound, name, base.BaseRequiredVersion);
					}
					else if (base.BaseMinimumVersion != null && base.BaseMaximumVersion != null)
					{
						message2 = StringUtil.Format(Modules.MinimumVersionAndMaximumVersionNotFound, new object[]
						{
							name,
							base.BaseMinimumVersion,
							base.BaseMaximumVersion
						});
					}
					else if (base.BaseMinimumVersion != null)
					{
						message2 = StringUtil.Format(Modules.ModuleWithVersionNotFound, name, base.BaseMinimumVersion);
					}
					else if (base.BaseMaximumVersion != null)
					{
						message2 = StringUtil.Format(Modules.MaximumVersionNotFound, name, base.BaseMaximumVersion);
					}
					ErrorRecord errorRecord2;
					if (base.BaseRequiredVersion != null || base.BaseMinimumVersion != null || base.BaseMaximumVersion != null)
					{
						FileNotFoundException exception2 = new FileNotFoundException(message2);
						errorRecord2 = new ErrorRecord(exception2, "Modules_ModuleWithVersionNotFound", ErrorCategory.ResourceUnavailable, name);
					}
					else
					{
						message2 = StringUtil.Format(Modules.ModuleNotFound, name);
						FileNotFoundException exception3 = new FileNotFoundException(message2);
						errorRecord2 = new ErrorRecord(exception3, "Modules_ModuleNotFound", ErrorCategory.ResourceUnavailable, name);
					}
					base.WriteError(errorRecord2);
				}
				return result;
			}
			catch (PSInvalidOperationException ex)
			{
				ErrorRecord errorRecord3 = new ErrorRecord(ex.ErrorRecord, ex);
				base.WriteError(errorRecord3);
			}
			return null;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00033A10 File Offset: 0x00031C10
		private IList<PSModuleInfo> ImportModule_RemotelyViaPsrpSession(ModuleCmdletBase.ImportModuleOptions importModuleOptions, IEnumerable<string> moduleNames, IEnumerable<ModuleSpecification> fullyQualifiedNames, PSSession psSession)
		{
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			if (moduleNames != null)
			{
				foreach (string moduleName in moduleNames)
				{
					IList<PSModuleInfo> collection = this.ImportModule_RemotelyViaPsrpSession(importModuleOptions, moduleName, null, psSession);
					list.AddRange(collection);
				}
			}
			if (fullyQualifiedNames != null)
			{
				foreach (ModuleSpecification fullyQualifiedName in fullyQualifiedNames)
				{
					IList<PSModuleInfo> collection2 = this.ImportModule_RemotelyViaPsrpSession(importModuleOptions, null, fullyQualifiedName, psSession);
					list.AddRange(collection2);
				}
			}
			return list;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00033AC0 File Offset: 0x00031CC0
		private IList<PSModuleInfo> ImportModule_RemotelyViaPsrpSession(ModuleCmdletBase.ImportModuleOptions importModuleOptions, string moduleName, ModuleSpecification fullyQualifiedName, PSSession psSession)
		{
			List<PSObject> list;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = psSession.Runspace;
				powerShell.AddCommand("Import-Module");
				powerShell.AddParameter("DisableNameChecking", this.DisableNameChecking);
				powerShell.AddParameter("PassThru", true);
				if (fullyQualifiedName != null)
				{
					powerShell.AddParameter("FullyQualifiedName", fullyQualifiedName);
				}
				else
				{
					powerShell.AddParameter("Name", moduleName);
					if (this.MinimumVersion != null)
					{
						powerShell.AddParameter("Version", this.MinimumVersion);
					}
					if (this.RequiredVersion != null)
					{
						powerShell.AddParameter("RequiredVersion", this.RequiredVersion);
					}
					if (this.MaximumVersion != null)
					{
						powerShell.AddParameter("MaximumVersion", this.MaximumVersion);
					}
				}
				if (this.ArgumentList != null)
				{
					powerShell.AddParameter("ArgumentList", this.ArgumentList);
				}
				if (base.BaseForce)
				{
					powerShell.AddParameter("Force", true);
				}
				string errorMessageTemplate = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryRemotePsrpCommandFailed, new object[]
				{
					string.Format(CultureInfo.InvariantCulture, "Import-Module -Name '{0}'", new object[]
					{
						moduleName
					})
				});
				list = RemoteDiscoveryHelper.InvokePowerShell(powerShell, this.CancellationToken, this, errorMessageTemplate).ToList<PSObject>();
			}
			List<PSModuleInfo> list2 = new List<PSModuleInfo>();
			foreach (PSObject psobject in list)
			{
				PSPropertyInfo pspropertyInfo = psobject.Properties["Name"];
				if (pspropertyInfo != null)
				{
					string remoteModuleName = (string)LanguagePrimitives.ConvertTo(pspropertyInfo.Value, typeof(string), CultureInfo.InvariantCulture);
					PSPropertyInfo pspropertyInfo2 = psobject.Properties["HelpInfoUri"];
					string text = null;
					if (pspropertyInfo2 != null)
					{
						text = (string)LanguagePrimitives.ConvertTo(pspropertyInfo2.Value, typeof(string), CultureInfo.InvariantCulture);
					}
					PSPropertyInfo pspropertyInfo3 = psobject.Properties["Guid"];
					Guid empty = Guid.Empty;
					if (pspropertyInfo3 != null)
					{
						LanguagePrimitives.TryConvertTo<Guid>(pspropertyInfo3.Value, out empty);
					}
					PSPropertyInfo pspropertyInfo4 = psobject.Properties["Version"];
					Version remoteModuleVersion = null;
					Version version;
					if (pspropertyInfo4 != null && LanguagePrimitives.TryConvertTo<Version>(pspropertyInfo4.Value, CultureInfo.InvariantCulture, out version))
					{
						remoteModuleVersion = version;
					}
					PSModuleInfo psmoduleInfo = this.ImportModule_RemotelyViaPsrpSession_SinglePreimportedModule(importModuleOptions, remoteModuleName, remoteModuleVersion, psSession);
					if (psmoduleInfo != null)
					{
						if (string.IsNullOrEmpty(psmoduleInfo.HelpInfoUri) && !string.IsNullOrEmpty(text))
						{
							psmoduleInfo.SetHelpInfoUri(text);
						}
						if (empty != Guid.Empty)
						{
							psmoduleInfo.SetGuid(empty);
						}
						list2.Add(psmoduleInfo);
					}
				}
			}
			return list2;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00033DB8 File Offset: 0x00031FB8
		private PSModuleInfo ImportModule_RemotelyViaPsrpSession_SinglePreimportedModule(ModuleCmdletBase.ImportModuleOptions importModuleOptions, string remoteModuleName, Version remoteModuleVersion, PSSession psSession)
		{
			string modulePath = RemoteDiscoveryHelper.GetModulePath(remoteModuleName, remoteModuleVersion, psSession.ComputerName, base.Context.CurrentRunspace);
			string value = WildcardPattern.Escape(modulePath);
			PSModuleInfo result;
			try
			{
				string modulePath2 = Path.Combine(modulePath, Path.GetFileName(modulePath) + ".psm1");
				PSModuleInfo psmoduleInfo = base.IsModuleImportUnnecessaryBecauseModuleIsAlreadyLoaded(modulePath2, base.BasePrefix, importModuleOptions);
				if (psmoduleInfo != null)
				{
					result = psmoduleInfo;
				}
				else
				{
					using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
					{
						powerShell.AddCommand("Export-PSSession");
						powerShell.AddParameter("OutputModule", value);
						powerShell.AddParameter("AllowClobber", true);
						powerShell.AddParameter("Module", remoteModuleName);
						powerShell.AddParameter("Force", true);
						powerShell.AddParameter("FormatTypeName", "*");
						powerShell.AddParameter("Session", psSession);
						string errorMessageTemplate = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryFailedToGenerateProxyForRemoteModule, new object[]
						{
							remoteModuleName
						});
						if (RemoteDiscoveryHelper.InvokePowerShell(powerShell, this.CancellationToken, this, errorMessageTemplate).Count<PSObject>() == 0)
						{
							return null;
						}
					}
					string text = Path.Combine(modulePath, remoteModuleName + ".psd1");
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					File.Move(Path.Combine(modulePath, Path.GetFileName(modulePath) + ".psd1"), text);
					string name = WildcardPattern.Escape(text);
					object[] argumentList = this.ArgumentList;
					try
					{
						this.ArgumentList = new object[]
						{
							psSession
						};
						this.ImportModule_LocallyViaName(importModuleOptions, name);
					}
					finally
					{
						this.ArgumentList = argumentList;
					}
					string key = Path.Combine(modulePath, Path.GetFileName(modulePath) + ".psm1");
					PSModuleInfo psmoduleInfo2;
					if (!base.Context.Modules.ModuleTable.TryGetValue(key, out psmoduleInfo2))
					{
						if (Directory.Exists(modulePath))
						{
							Directory.Delete(modulePath, true);
						}
						result = null;
					}
					else
					{
						ScriptBlock scriptBlock = base.Context.Engine.ParseScriptBlock("\r\n                    Microsoft.PowerShell.Management\\Remove-Item `\r\n                        -LiteralPath $temporaryModulePath `\r\n                        -Force `\r\n                        -Recurse `\r\n                        -ErrorAction SilentlyContinue\r\n\r\n                    if ($previousOnRemoveScript -ne $null)\r\n                    {\r\n                        & $previousOnRemoveScript $args\r\n                    }\r\n                    ", false);
						scriptBlock = scriptBlock.GetNewClosure();
						scriptBlock.Module.SessionState.PSVariable.Set("temporaryModulePath", modulePath);
						scriptBlock.Module.SessionState.PSVariable.Set("previousOnRemoveScript", psmoduleInfo2.OnRemove);
						psmoduleInfo2.OnRemove = scriptBlock;
						result = psmoduleInfo2;
					}
				}
			}
			catch
			{
				if (Directory.Exists(modulePath))
				{
					Directory.Delete(modulePath, true);
				}
				throw;
			}
			return result;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00034074 File Offset: 0x00032274
		private static bool IsNonEmptyManifestField(Hashtable manifestData, string key)
		{
			if (!manifestData.ContainsKey(key))
			{
				return false;
			}
			object obj = manifestData[key];
			object[] array;
			return obj != null && (!LanguagePrimitives.TryConvertTo<object[]>(obj, CultureInfo.InvariantCulture, out array) || array.Length != 0);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000340C0 File Offset: 0x000322C0
		private bool IsMixedModePsCimModule(RemoteDiscoveryHelper.CimModule cimModule)
		{
			string modulePath = RemoteDiscoveryHelper.GetModulePath(cimModule.ModuleName, null, string.Empty, base.Context.CurrentRunspace);
			bool flag = false;
			RemoteDiscoveryHelper.CimModuleFile mainManifest = cimModule.MainManifest;
			if (mainManifest == null)
			{
				return true;
			}
			Hashtable hashtable = RemoteDiscoveryHelper.ConvertCimModuleFileToManifestHashtable(mainManifest, modulePath, this, ref flag);
			if (flag || hashtable == null)
			{
				return false;
			}
			if (ImportModuleCommand.IsNonEmptyManifestField(hashtable, "ScriptsToProcess") || ImportModuleCommand.IsNonEmptyManifestField(hashtable, "RequiredAssemblies"))
			{
				return true;
			}
			int num = 0;
			string[] array = null;
			if (LanguagePrimitives.TryConvertTo<string[]>(hashtable["NestedModules"], CultureInfo.InvariantCulture, out array) && array != null)
			{
				num += array.Length;
			}
			if (hashtable.ContainsKey("RootModule") || hashtable.ContainsKey("ModuleToProcess"))
			{
				string value2;
				if (hashtable.ContainsKey("RootModule"))
				{
					string value;
					if (LanguagePrimitives.TryConvertTo<string>(hashtable["RootModule"], CultureInfo.InvariantCulture, out value) && !string.IsNullOrEmpty(value))
					{
						num++;
					}
				}
				else if (hashtable.ContainsKey("ModuleToProcess") && LanguagePrimitives.TryConvertTo<string>(hashtable["ModuleToProcess"], CultureInfo.InvariantCulture, out value2) && !string.IsNullOrEmpty(value2))
				{
					num++;
				}
			}
			int num2 = (from moduleFile in cimModule.ModuleFiles
			where moduleFile.FileCode == RemoteDiscoveryHelper.CimFileCode.CmdletizationV1
			select moduleFile).Count<RemoteDiscoveryHelper.CimModuleFile>();
			return num > num2;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0003424C File Offset: 0x0003244C
		private void ImportModule_RemotelyViaCimSession(ModuleCmdletBase.ImportModuleOptions importModuleOptions, string[] moduleNames, CimSession cimSession, Uri resourceUri, string cimNamespace)
		{
			IEnumerable<RemoteDiscoveryHelper.CimModule> source = RemoteDiscoveryHelper.GetCimModules(cimSession, resourceUri, cimNamespace, moduleNames, false, this, this.CancellationToken).ToList<RemoteDiscoveryHelper.CimModule>();
			IEnumerable<RemoteDiscoveryHelper.CimModule> enumerable = from cimModule in source
			where cimModule.IsPsCimModule
			select cimModule;
			IEnumerable<string> enumerable2 = from cimModule in source
			where !cimModule.IsPsCimModule
			select cimModule.ModuleName;
			foreach (string text in enumerable2)
			{
				string message = string.Format(CultureInfo.InvariantCulture, Modules.PsModuleOverCimSessionError, new object[]
				{
					text
				});
				ErrorRecord errorRecord = new ErrorRecord(new ArgumentException(message), "PsModuleOverCimSessionError", ErrorCategory.InvalidArgument, text);
				base.WriteError(errorRecord);
			}
			IEnumerable<string> source2 = (from cimModule in source
			select cimModule.ModuleName).ToList<string>();
			for (int i = 0; i < moduleNames.Length; i++)
			{
				string text2 = moduleNames[i];
				WildcardPattern wildcardPattern = new WildcardPattern(text2, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
				if (!source2.Any((string foundModuleName) => wildcardPattern.IsMatch(foundModuleName)))
				{
					string message2 = StringUtil.Format(Modules.ModuleNotFound, text2);
					FileNotFoundException exception = new FileNotFoundException(message2);
					ErrorRecord errorRecord2 = new ErrorRecord(exception, "Modules_ModuleNotFound", ErrorCategory.ResourceUnavailable, text2);
					base.WriteError(errorRecord2);
				}
			}
			foreach (RemoteDiscoveryHelper.CimModule remoteCimModule in enumerable)
			{
				this.ImportModule_RemotelyViaCimModuleData(importModuleOptions, remoteCimModule, cimSession);
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00034484 File Offset: 0x00032684
		private bool IsPs1xmlFileHelper_IsPresentInEntries(RemoteDiscoveryHelper.CimModuleFile cimModuleFile, IEnumerable<string> manifestEntries)
		{
			return manifestEntries.Any((string s) => s.EndsWith(cimModuleFile.FileName, StringComparison.OrdinalIgnoreCase)) || manifestEntries.Any((string s) => this.FixupFileName("", s, ".ps1xml").EndsWith(cimModuleFile.FileName, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000344D4 File Offset: 0x000326D4
		private bool IsPs1xmlFileHelper(RemoteDiscoveryHelper.CimModuleFile cimModuleFile, Hashtable manifestData, string goodKey, string badKey)
		{
			if (!Path.GetExtension(cimModuleFile.FileName).Equals(".ps1xml", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			List<string> list;
			if (!base.GetListOfStringsFromData(manifestData, null, goodKey, (ModuleCmdletBase.ManifestProcessingFlags)0, out list))
			{
				list = new List<string>();
			}
			if (list == null)
			{
				list = new List<string>();
			}
			List<string> list2;
			if (!base.GetListOfStringsFromData(manifestData, null, badKey, (ModuleCmdletBase.ManifestProcessingFlags)0, out list2))
			{
				list2 = new List<string>();
			}
			if (list2 == null)
			{
				list2 = new List<string>();
			}
			bool flag = this.IsPs1xmlFileHelper_IsPresentInEntries(cimModuleFile, list);
			bool flag2 = this.IsPs1xmlFileHelper_IsPresentInEntries(cimModuleFile, list2);
			return flag && !flag2;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00034551 File Offset: 0x00032751
		private bool IsTypesPs1XmlFile(RemoteDiscoveryHelper.CimModuleFile cimModuleFile, Hashtable manifestData)
		{
			return this.IsPs1xmlFileHelper(cimModuleFile, manifestData, "TypesToProcess", "FormatsToProcess");
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00034565 File Offset: 0x00032765
		private bool IsFormatPs1XmlFile(RemoteDiscoveryHelper.CimModuleFile cimModuleFile, Hashtable manifestData)
		{
			return this.IsPs1xmlFileHelper(cimModuleFile, manifestData, "FormatsToProcess", "TypesToProcess");
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00034579 File Offset: 0x00032779
		private static bool IsCmdletizationFile(RemoteDiscoveryHelper.CimModuleFile cimModuleFile)
		{
			return cimModuleFile.FileCode == RemoteDiscoveryHelper.CimFileCode.CmdletizationV1;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00034584 File Offset: 0x00032784
		private IEnumerable<string> CreateCimModuleFiles(RemoteDiscoveryHelper.CimModule remoteCimModule, RemoteDiscoveryHelper.CimFileCode fileCode, Func<RemoteDiscoveryHelper.CimModuleFile, bool> filesFilter, string temporaryModuleDirectory)
		{
			string format = null;
			switch (fileCode)
			{
			case RemoteDiscoveryHelper.CimFileCode.TypesV1:
				format = "{0}_{1}.types.ps1xml";
				break;
			case RemoteDiscoveryHelper.CimFileCode.FormatV1:
				format = "{0}_{1}.format.ps1xml";
				break;
			case RemoteDiscoveryHelper.CimFileCode.CmdletizationV1:
				format = "{0}_{1}.cdxml";
				break;
			}
			List<string> list = new List<string>();
			foreach (RemoteDiscoveryHelper.CimModuleFile cimModuleFile in remoteCimModule.ModuleFiles)
			{
				if (filesFilter(cimModuleFile))
				{
					string fileName = Path.GetFileName(cimModuleFile.FileName);
					string text = string.Format(CultureInfo.InvariantCulture, format, new object[]
					{
						fileName.Substring(0, Math.Min(fileName.Length, 20)),
						Path.GetRandomFileName()
					});
					list.Add(text);
					string path = Path.Combine(temporaryModuleDirectory, text);
					File.WriteAllBytes(path, cimModuleFile.RawFileData);
					AlternateDataStreamUtilities.SetZoneOfOrigin(path, SecurityZone.Intranet);
				}
			}
			return list;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000346B0 File Offset: 0x000328B0
		private PSModuleInfo ImportModule_RemotelyViaCimModuleData(ModuleCmdletBase.ImportModuleOptions importModuleOptions, RemoteDiscoveryHelper.CimModule remoteCimModule, CimSession cimSession)
		{
			PSModuleInfo result;
			try
			{
				if (remoteCimModule.MainManifest == null)
				{
					string message = string.Format(CultureInfo.InvariantCulture, Modules.EmptyModuleManifest, new object[]
					{
						remoteCimModule.ModuleName + ".psd1"
					});
					ArgumentException ex = new ArgumentException(message);
					throw ex;
				}
				bool flag = false;
				PSModuleInfo psmoduleInfo = null;
				string modulePath = RemoteDiscoveryHelper.GetModulePath(remoteCimModule.ModuleName, null, cimSession.ComputerName, base.Context.CurrentRunspace);
				string text = Path.Combine(modulePath, remoteCimModule.ModuleName + ".psd1");
				Hashtable data = null;
				Token[] array;
				ParseError[] array2;
				ScriptBlockAst scriptBlockAst = Parser.ParseInput(remoteCimModule.MainManifest.FileData, text, out array, out array2);
				if (scriptBlockAst == null || (array2 != null && array2.Length > 0))
				{
					throw new ParseException(array2);
				}
				ScriptBlock scriptBlock = new ScriptBlock(scriptBlockAst, false);
				data = base.LoadModuleManifestData(text, scriptBlock, ModuleCmdletBase.ModuleManifestMembers, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError, ref flag);
				if (data == null || flag)
				{
					result = null;
				}
				else
				{
					Hashtable hashtable = data;
					Version remoteModuleVersion;
					if (!base.GetScalarFromData<Version>(data, null, "ModuleVersion", (ModuleCmdletBase.ManifestProcessingFlags)0, out remoteModuleVersion))
					{
						remoteModuleVersion = null;
					}
					modulePath = RemoteDiscoveryHelper.GetModulePath(remoteCimModule.ModuleName, remoteModuleVersion, cimSession.ComputerName, base.Context.CurrentRunspace);
					text = Path.Combine(modulePath, remoteCimModule.ModuleName + ".psd1");
					PSModuleInfo psmoduleInfo2 = base.IsModuleImportUnnecessaryBecauseModuleIsAlreadyLoaded(text, base.BasePrefix, importModuleOptions);
					if (psmoduleInfo2 != null)
					{
						result = psmoduleInfo2;
					}
					else
					{
						try
						{
							Directory.CreateDirectory(modulePath);
							IEnumerable<string> typesToProcess = this.CreateCimModuleFiles(remoteCimModule, RemoteDiscoveryHelper.CimFileCode.TypesV1, (RemoteDiscoveryHelper.CimModuleFile cimModuleFile) => this.IsTypesPs1XmlFile(cimModuleFile, data), modulePath);
							IEnumerable<string> formatsToProcess = this.CreateCimModuleFiles(remoteCimModule, RemoteDiscoveryHelper.CimFileCode.FormatV1, (RemoteDiscoveryHelper.CimModuleFile cimModuleFile) => this.IsFormatPs1XmlFile(cimModuleFile, data), modulePath);
							IEnumerable<string> nestedModules = this.CreateCimModuleFiles(remoteCimModule, RemoteDiscoveryHelper.CimFileCode.CmdletizationV1, new Func<RemoteDiscoveryHelper.CimModuleFile, bool>(ImportModuleCommand.IsCmdletizationFile), modulePath);
							data = RemoteDiscoveryHelper.RewriteManifest(data, nestedModules, typesToProcess, formatsToProcess);
							hashtable = RemoteDiscoveryHelper.RewriteManifest(hashtable);
							psmoduleInfo = base.LoadModuleManifest(text, null, data, hashtable, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.NullOnFirstError | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, base.BaseMinimumVersion, base.BaseMaximumVersion, base.BaseRequiredVersion, base.BaseGuid, ref importModuleOptions, ref flag);
							if (psmoduleInfo == null)
							{
								result = null;
							}
							else
							{
								foreach (PSModuleInfo psmoduleInfo3 in psmoduleInfo.NestedModules)
								{
									Type type;
									PSPrimitiveDictionary.TryPathGet<Type>(psmoduleInfo3.PrivateData as IDictionary, out type, new string[]
									{
										"CmdletsOverObjects",
										"CmdletAdapter"
									});
									if (!type.AssemblyQualifiedName.Equals("Microsoft.PowerShell.Cmdletization.Cim.CimCmdletAdapter, Microsoft.PowerShell.Commands.Management, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", StringComparison.OrdinalIgnoreCase))
									{
										string message2 = string.Format(CultureInfo.InvariantCulture, CmdletizationCoreResources.ImportModule_UnsupportedCmdletAdapter, new object[]
										{
											type.FullName
										});
										ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(message2), "UnsupportedCmdletAdapter", ErrorCategory.InvalidData, type);
										base.ThrowTerminatingError(errorRecord);
									}
								}
								if (this.IsMixedModePsCimModule(remoteCimModule))
								{
									string text2 = string.Format(CultureInfo.InvariantCulture, Modules.MixedModuleOverCimSessionWarning, new object[]
									{
										remoteCimModule.ModuleName
									});
									base.WriteWarning(text2);
								}
								foreach (PSModuleInfo psmoduleInfo4 in psmoduleInfo.NestedModules)
								{
									IDictionary dictionary;
									PSPrimitiveDictionary.TryPathGet<IDictionary>(psmoduleInfo4.PrivateData as IDictionary, out dictionary, new string[]
									{
										"CmdletsOverObjects"
									});
									dictionary["DefaultSession"] = cimSession;
								}
								ScriptBlock scriptBlock2 = base.Context.Engine.ParseScriptBlock("\r\n                        Microsoft.PowerShell.Management\\Remove-Item `\r\n                            -LiteralPath $temporaryModulePath `\r\n                            -Force `\r\n                            -Recurse `\r\n                            -ErrorAction SilentlyContinue\r\n\r\n                        if ($previousOnRemoveScript -ne $null)\r\n                        {\r\n                            & $previousOnRemoveScript $args\r\n                        }\r\n                        ", false);
								scriptBlock2 = scriptBlock2.GetNewClosure();
								scriptBlock2.Module.SessionState.PSVariable.Set("temporaryModulePath", modulePath);
								scriptBlock2.Module.SessionState.PSVariable.Set("previousOnRemoveScript", psmoduleInfo.OnRemove);
								psmoduleInfo.OnRemove = scriptBlock2;
								ModuleCmdletBase.AddModuleToModuleTables(base.Context, base.TargetSessionState.Internal, psmoduleInfo);
								if (base.BasePassThru)
								{
									base.WriteObject(psmoduleInfo);
								}
								result = psmoduleInfo;
							}
						}
						catch
						{
							if (Directory.Exists(modulePath))
							{
								Directory.Delete(modulePath, true);
							}
							throw;
						}
						finally
						{
							if (psmoduleInfo == null && Directory.Exists(modulePath))
							{
								Directory.Delete(modulePath, true);
							}
						}
					}
				}
			}
			catch (Exception innerException)
			{
				ErrorRecord errorRecordForProcessingOfCimModule = RemoteDiscoveryHelper.GetErrorRecordForProcessingOfCimModule(innerException, remoteCimModule.ModuleName);
				base.WriteError(errorRecordForProcessingOfCimModule);
				result = null;
			}
			return result;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00034BA8 File Offset: 0x00032DA8
		private CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationTokenSource.Token;
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00034BB5 File Offset: 0x00032DB5
		protected override void StopProcessing()
		{
			this._cancellationTokenSource.Cancel();
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00034BC2 File Offset: 0x00032DC2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00034BD1 File Offset: 0x00032DD1
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._cancellationTokenSource.Dispose();
			}
			this._disposed = true;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00034BF4 File Offset: 0x00032DF4
		protected override void BeginProcessing()
		{
			if (this.Global.IsPresent && this._isScopeSpecified)
			{
				InvalidOperationException exception = new InvalidOperationException(Modules.GlobalAndScopeParameterCannotBeSpecifiedTogether);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_GlobalAndScopeParameterCannotBeSpecifiedTogether", ErrorCategory.InvalidOperation, null);
				base.ThrowTerminatingError(errorRecord);
			}
			if (!string.IsNullOrEmpty(this.Scope) && this.Scope.Equals("GLOBAL", StringComparison.OrdinalIgnoreCase))
			{
				base.BaseGlobal = true;
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00034D54 File Offset: 0x00032F54
		protected override void ProcessRecord()
		{
			if (base.BaseMaximumVersion != null && base.BaseMaximumVersion != null && base.BaseMaximumVersion < base.BaseMinimumVersion)
			{
				string paramName = StringUtil.Format(Modules.MinimumVersionAndMaximumVersionInvalidRange, base.BaseMinimumVersion, base.BaseMaximumVersion);
				throw new PSArgumentOutOfRangeException(paramName);
			}
			ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
			importModuleOptions.NoClobber = this.NoClobber;
			if (!string.IsNullOrEmpty(this.Scope) && this.Scope.Equals("LOCAL", StringComparison.OrdinalIgnoreCase))
			{
				importModuleOptions.Local = true;
			}
			if (base.ParameterSetName.Equals("ModuleInfo", StringComparison.OrdinalIgnoreCase))
			{
				PSModuleInfo[] moduleInfo = this._moduleInfo;
				for (int i = 0; i < moduleInfo.Length; i++)
				{
					PSModuleInfo module = moduleInfo[i];
					RemoteDiscoveryHelper.DispatchModuleInfoProcessing(module, delegate
					{
						this.ImportModule_ViaLocalModuleInfo(importModuleOptions, module);
						this.SetModuleBaseForEngineModules(module.Name, this.Context);
					}, delegate(CimSession cimSession, Uri resourceUri, string cimNamespace)
					{
						this.ImportModule_RemotelyViaCimSession(importModuleOptions, new string[]
						{
							module.Name
						}, cimSession, resourceUri, cimNamespace);
					}, delegate(PSSession psSession)
					{
						this.ImportModule_RemotelyViaPsrpSession(importModuleOptions, new string[]
						{
							module.Path
						}, null, psSession);
					});
				}
				return;
			}
			if (base.ParameterSetName.Equals("Assembly", StringComparison.OrdinalIgnoreCase))
			{
				if (this.Assembly != null)
				{
					foreach (Assembly suppliedAssembly in this.Assembly)
					{
						this.ImportModule_ViaAssembly(importModuleOptions, suppliedAssembly);
					}
					return;
				}
			}
			else
			{
				if (base.ParameterSetName.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					foreach (string name2 in this.Name)
					{
						PSModuleInfo psmoduleInfo = this.ImportModule_LocallyViaName(importModuleOptions, name2);
						if (psmoduleInfo != null)
						{
							this.SetModuleBaseForEngineModules(psmoduleInfo.Name, base.Context);
						}
					}
					return;
				}
				if (base.ParameterSetName.Equals("PSSession", StringComparison.OrdinalIgnoreCase))
				{
					this.ImportModule_RemotelyViaPsrpSession(importModuleOptions, this.Name, null, this.PSSession);
					return;
				}
				if (base.ParameterSetName.Equals("CimSession", StringComparison.OrdinalIgnoreCase))
				{
					this.ImportModule_RemotelyViaCimSession(importModuleOptions, this.Name, this.CimSession, this.CimResourceUri, this.CimNamespace);
					return;
				}
				if (base.ParameterSetName.Equals("FullyQualifiedName", StringComparison.OrdinalIgnoreCase))
				{
					foreach (ModuleSpecification moduleSpecification in this.FullyQualifiedName)
					{
						this.RequiredVersion = moduleSpecification.RequiredVersion;
						this.MinimumVersion = moduleSpecification.Version;
						this.MaximumVersion = moduleSpecification.MaximumVersion;
						base.BaseGuid = moduleSpecification.Guid;
						PSModuleInfo psmoduleInfo2 = this.ImportModule_LocallyViaName(importModuleOptions, moduleSpecification.Name);
						if (psmoduleInfo2 != null)
						{
							this.SetModuleBaseForEngineModules(psmoduleInfo2.Name, base.Context);
						}
					}
					return;
				}
				if (base.ParameterSetName.Equals("FullyQualifiedNameAndPSSession", StringComparison.OrdinalIgnoreCase))
				{
					this.ImportModule_RemotelyViaPsrpSession(importModuleOptions, null, this.FullyQualifiedName, this.PSSession);
				}
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00035084 File Offset: 0x00033284
		private void SetModuleBaseForEngineModules(string moduleName, System.Management.Automation.ExecutionContext context)
		{
			if (InitialSessionState.IsEngineModule(moduleName))
			{
				foreach (PSModuleInfo psmoduleInfo in context.EngineSessionState.ModuleTable.Values)
				{
					if (psmoduleInfo.Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
					{
						psmoduleInfo.SetModuleBase(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID));
						foreach (PSModuleInfo psmoduleInfo2 in psmoduleInfo.NestedModules)
						{
							psmoduleInfo2.SetModuleBase(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID));
						}
					}
				}
				foreach (PSModuleInfo psmoduleInfo3 in context.Modules.ModuleTable.Values)
				{
					if (psmoduleInfo3.Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
					{
						psmoduleInfo3.SetModuleBase(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID));
						foreach (PSModuleInfo psmoduleInfo4 in psmoduleInfo3.NestedModules)
						{
							psmoduleInfo4.SetModuleBase(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID));
						}
					}
				}
			}
		}

		// Token: 0x040003DF RID: 991
		private const string ParameterSet_Name = "Name";

		// Token: 0x040003E0 RID: 992
		private const string ParameterSet_FQName = "FullyQualifiedName";

		// Token: 0x040003E1 RID: 993
		private const string ParameterSet_ModuleInfo = "ModuleInfo";

		// Token: 0x040003E2 RID: 994
		private const string ParameterSet_Assembly = "Assembly";

		// Token: 0x040003E3 RID: 995
		private const string ParameterSet_ViaPsrpSession = "PSSession";

		// Token: 0x040003E4 RID: 996
		private const string ParameterSet_ViaCimSession = "CimSession";

		// Token: 0x040003E5 RID: 997
		private const string ParameterSet_FQName_ViaPsrpSession = "FullyQualifiedNameAndPSSession";

		// Token: 0x040003E6 RID: 998
		private string[] _name = new string[0];

		// Token: 0x040003E7 RID: 999
		private string[] _functionImportList = new string[0];

		// Token: 0x040003E8 RID: 1000
		private string[] _cmdletImportList = new string[0];

		// Token: 0x040003E9 RID: 1001
		private string[] _variableExportList;

		// Token: 0x040003EA RID: 1002
		private string[] _aliasExportList;

		// Token: 0x040003EB RID: 1003
		private PSModuleInfo[] _moduleInfo = new PSModuleInfo[0];

		// Token: 0x040003EC RID: 1004
		private string _scope = string.Empty;

		// Token: 0x040003ED RID: 1005
		private bool _isScopeSpecified;

		// Token: 0x040003EE RID: 1006
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x040003EF RID: 1007
		private bool _disposed;
	}
}
