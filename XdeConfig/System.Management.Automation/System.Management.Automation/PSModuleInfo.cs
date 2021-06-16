using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020000B4 RID: 180
	public sealed class PSModuleInfo
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0003A1CE File Offset: 0x000383CE
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0003A1C5 File Offset: 0x000383C5
		private ReadOnlyDictionary<string, TypeDefinitionAst> _exportedTypeDefinitions { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0003A1DF File Offset: 0x000383DF
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0003A1D6 File Offset: 0x000383D6
		private ReadOnlyDictionary<string, TypeDefinitionAst> _exportedTypeDefinitionsIncludeNesting { get; set; }

		// Token: 0x06000992 RID: 2450 RVA: 0x0003A1E8 File Offset: 0x000383E8
		internal static void SetDefaultDynamicNameAndPath(PSModuleInfo module)
		{
			string text = Guid.NewGuid().ToString();
			module._path = text;
			module._name = "__DynamicModule_" + text;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0003A221 File Offset: 0x00038421
		internal PSModuleInfo(string path, ExecutionContext context, SessionState sessionState) : this(null, path, context, sessionState)
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0003A230 File Offset: 0x00038430
		internal PSModuleInfo(string name, string path, ExecutionContext context, SessionState sessionState)
		{
			this._name = string.Empty;
			this._path = string.Empty;
			this._description = string.Empty;
			this._tags = new List<string>();
			this._version = new Version(0, 0);
			this._detectedFunctionExports = new List<string>();
			this._detectedWorkflowExports = new List<string>();
			this._detectedCmdletExports = new List<string>();
			this._compiledExports = new List<CmdletInfo>();
			this._compiledAliasExports = new List<AliasInfo>();
			this._fileList = new List<string>();
			this._moduleList = new Collection<object>();
			this._nestedModules = new List<PSModuleInfo>();
			this._scripts = new List<string>();
			this._requiredAssemblies = new Collection<string>();
			this._requiredModules = new List<PSModuleInfo>();
			this._requiredModulesSpecification = new List<ModuleSpecification>();
			this._detectedAliasExports = new Dictionary<string, string>();
			this._exportedFormatFiles = new ReadOnlyCollection<string>(new List<string>());
			this._exportedTypeFiles = new ReadOnlyCollection<string>(new List<string>());
			base..ctor();
			if (path != null)
			{
				string resolvedPath = ModuleCmdletBase.GetResolvedPath(path, context);
				this._path = (resolvedPath ?? path);
			}
			this._sessionState = sessionState;
			if (sessionState != null)
			{
				sessionState.Internal.Module = this;
			}
			if (name == null)
			{
				this._name = ModuleIntrinsics.GetModuleName(this._path);
				return;
			}
			this._name = name;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003A377 File Offset: 0x00038577
		public PSModuleInfo(bool linkToGlobal) : this(LocalPipeline.GetExecutionContextFromTLS(), linkToGlobal)
		{
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0003A388 File Offset: 0x00038588
		internal PSModuleInfo(ExecutionContext context, bool linkToGlobal)
		{
			this._name = string.Empty;
			this._path = string.Empty;
			this._description = string.Empty;
			this._tags = new List<string>();
			this._version = new Version(0, 0);
			this._detectedFunctionExports = new List<string>();
			this._detectedWorkflowExports = new List<string>();
			this._detectedCmdletExports = new List<string>();
			this._compiledExports = new List<CmdletInfo>();
			this._compiledAliasExports = new List<AliasInfo>();
			this._fileList = new List<string>();
			this._moduleList = new Collection<object>();
			this._nestedModules = new List<PSModuleInfo>();
			this._scripts = new List<string>();
			this._requiredAssemblies = new Collection<string>();
			this._requiredModules = new List<PSModuleInfo>();
			this._requiredModulesSpecification = new List<ModuleSpecification>();
			this._detectedAliasExports = new Dictionary<string, string>();
			this._exportedFormatFiles = new ReadOnlyCollection<string>(new List<string>());
			this._exportedTypeFiles = new ReadOnlyCollection<string>(new List<string>());
			base..ctor();
			if (context == null)
			{
				throw new InvalidOperationException("PSModuleInfo");
			}
			PSModuleInfo.SetDefaultDynamicNameAndPath(this);
			this._sessionState = new SessionState(context, true, linkToGlobal);
			this._sessionState.Internal.Module = this;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0003A4B8 File Offset: 0x000386B8
		public PSModuleInfo(ScriptBlock scriptBlock)
		{
			this._name = string.Empty;
			this._path = string.Empty;
			this._description = string.Empty;
			this._tags = new List<string>();
			this._version = new Version(0, 0);
			this._detectedFunctionExports = new List<string>();
			this._detectedWorkflowExports = new List<string>();
			this._detectedCmdletExports = new List<string>();
			this._compiledExports = new List<CmdletInfo>();
			this._compiledAliasExports = new List<AliasInfo>();
			this._fileList = new List<string>();
			this._moduleList = new Collection<object>();
			this._nestedModules = new List<PSModuleInfo>();
			this._scripts = new List<string>();
			this._requiredAssemblies = new Collection<string>();
			this._requiredModules = new List<PSModuleInfo>();
			this._requiredModulesSpecification = new List<ModuleSpecification>();
			this._detectedAliasExports = new Dictionary<string, string>();
			this._exportedFormatFiles = new ReadOnlyCollection<string>(new List<string>());
			this._exportedTypeFiles = new ReadOnlyCollection<string>(new List<string>());
			base..ctor();
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentException("scriptBlock");
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				throw new InvalidOperationException("PSModuleInfo");
			}
			PSModuleInfo.SetDefaultDynamicNameAndPath(this);
			this._sessionState = new SessionState(executionContextFromTLS, true, true);
			this._sessionState.Internal.Module = this;
			SessionStateInternal engineSessionState = executionContextFromTLS.EngineSessionState;
			try
			{
				executionContextFromTLS.EngineSessionState = this._sessionState.Internal;
				executionContextFromTLS.SetVariable(SpecialVariables.PSScriptRootVarPath, this._path);
				scriptBlock = scriptBlock.Clone(true);
				scriptBlock.SessionState = this._sessionState;
				Pipe outputPipe = new Pipe
				{
					NullPipe = true
				};
				scriptBlock.InvokeWithPipe(false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, outputPipe, null, false, null, null, null);
			}
			finally
			{
				executionContextFromTLS.EngineSessionState = engineSessionState;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0003A680 File Offset: 0x00038880
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0003A688 File Offset: 0x00038888
		internal bool ModuleHasPrivateMembers
		{
			get
			{
				return this._moduleHasPrivateMembers;
			}
			set
			{
				this._moduleHasPrivateMembers = value;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0003A691 File Offset: 0x00038891
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0003A699 File Offset: 0x00038899
		internal bool HadErrorsLoading { get; set; }

		// Token: 0x0600099C RID: 2460 RVA: 0x0003A6A2 File Offset: 0x000388A2
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0003A6AA File Offset: 0x000388AA
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0003A6B2 File Offset: 0x000388B2
		public bool LogPipelineExecutionDetails
		{
			get
			{
				return this._logPipelineExecutionDetails;
			}
			set
			{
				this._logPipelineExecutionDetails = value;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0003A6BB File Offset: 0x000388BB
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003A6C3 File Offset: 0x000388C3
		internal void SetName(string name)
		{
			this._name = name;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0003A6CC File Offset: 0x000388CC
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0003A6D4 File Offset: 0x000388D4
		public string Path
		{
			get
			{
				return this._path;
			}
			internal set
			{
				this._path = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0003A6DD File Offset: 0x000388DD
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0003A6E5 File Offset: 0x000388E5
		public Assembly ImplementingAssembly { get; internal set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0003A6EE File Offset: 0x000388EE
		public string Definition
		{
			get
			{
				if (this._definitionExtent != null)
				{
					return this._definitionExtent.Text;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0003A709 File Offset: 0x00038909
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0003A711 File Offset: 0x00038911
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = (value ?? string.Empty);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0003A723 File Offset: 0x00038923
		public Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003A72B File Offset: 0x0003892B
		internal void SetGuid(Guid guid)
		{
			this._guid = guid;
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0003A734 File Offset: 0x00038934
		public string HelpInfoUri
		{
			get
			{
				return this._helpInfoUri;
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0003A73C File Offset: 0x0003893C
		internal void SetHelpInfoUri(string uri)
		{
			this._helpInfoUri = uri;
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0003A748 File Offset: 0x00038948
		public string ModuleBase
		{
			get
			{
				string result;
				if ((result = this._moduleBase) == null)
				{
					result = (this._moduleBase = ((!string.IsNullOrEmpty(this._path)) ? System.IO.Path.GetDirectoryName(this._path) : string.Empty));
				}
				return result;
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003A787 File Offset: 0x00038987
		internal void SetModuleBase(string moduleBase)
		{
			this._moduleBase = moduleBase;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0003A790 File Offset: 0x00038990
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0003A798 File Offset: 0x00038998
		public object PrivateData
		{
			get
			{
				return this._privateData;
			}
			set
			{
				this._privateData = value;
				this.SetPSDataPropertiesFromPrivateData();
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003A7A8 File Offset: 0x000389A8
		private void SetPSDataPropertiesFromPrivateData()
		{
			this._tags.Clear();
			this.ReleaseNotes = null;
			this.LicenseUri = null;
			this.ProjectUri = null;
			this.IconUri = null;
			Hashtable hashtable = this._privateData as Hashtable;
			if (hashtable != null && hashtable.ContainsKey("PSData"))
			{
				Hashtable hashtable2 = hashtable["PSData"] as Hashtable;
				if (hashtable2 != null)
				{
					if (hashtable2.ContainsKey("Tags"))
					{
						object[] array = hashtable2["Tags"] as object[];
						if (array != null && array.Any<object>())
						{
							using (IEnumerator<string> enumerator = array.OfType<string>().GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									string tag = enumerator.Current;
									this.AddToTags(tag);
								}
								goto IL_D4;
							}
						}
						this.AddToTags(hashtable2["Tags"].ToString());
					}
					IL_D4:
					if (hashtable2.ContainsKey("LicenseUri"))
					{
						string uriString = hashtable2["LicenseUri"] as string;
						this.LicenseUri = PSModuleInfo.GetUriFromString(uriString);
					}
					if (hashtable2.ContainsKey("ProjectUri"))
					{
						string uriString2 = hashtable2["ProjectUri"] as string;
						this.ProjectUri = PSModuleInfo.GetUriFromString(uriString2);
					}
					if (hashtable2.ContainsKey("IconUri"))
					{
						string uriString3 = hashtable2["IconUri"] as string;
						this.IconUri = PSModuleInfo.GetUriFromString(uriString3);
					}
					if (hashtable2.ContainsKey("ReleaseNotes"))
					{
						this.ReleaseNotes = (hashtable2["ReleaseNotes"] as string);
					}
				}
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003A940 File Offset: 0x00038B40
		private static Uri GetUriFromString(string uriString)
		{
			Uri result = null;
			if (uriString != null)
			{
				Uri.TryCreate(uriString, UriKind.Absolute, out result);
			}
			return result;
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0003A95D File Offset: 0x00038B5D
		public IEnumerable<string> Tags
		{
			get
			{
				return this._tags;
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003A965 File Offset: 0x00038B65
		internal void AddToTags(string tag)
		{
			this._tags.Add(tag);
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0003A973 File Offset: 0x00038B73
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0003A97B File Offset: 0x00038B7B
		public Uri ProjectUri { get; internal set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0003A984 File Offset: 0x00038B84
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0003A98C File Offset: 0x00038B8C
		public Uri IconUri { get; internal set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0003A995 File Offset: 0x00038B95
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0003A99D File Offset: 0x00038B9D
		public Uri LicenseUri { get; internal set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0003A9A6 File Offset: 0x00038BA6
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0003A9AE File Offset: 0x00038BAE
		public string ReleaseNotes { get; internal set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0003A9B7 File Offset: 0x00038BB7
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0003A9BF File Offset: 0x00038BBF
		public Uri RepositorySourceLocation { get; internal set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0003A9C8 File Offset: 0x00038BC8
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0003A9D0 File Offset: 0x00038BD0
		internal void SetVersion(Version version)
		{
			this._version = version;
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0003A9D9 File Offset: 0x00038BD9
		public ModuleType ModuleType
		{
			get
			{
				return this._moduleType;
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003A9E1 File Offset: 0x00038BE1
		internal void SetModuleType(ModuleType moduleType)
		{
			this._moduleType = moduleType;
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0003A9EA File Offset: 0x00038BEA
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0003A9F2 File Offset: 0x00038BF2
		public string Author { get; internal set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0003A9FB File Offset: 0x00038BFB
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0003AA03 File Offset: 0x00038C03
		public ModuleAccessMode AccessMode
		{
			get
			{
				return this._accessMode;
			}
			set
			{
				if (this._accessMode == ModuleAccessMode.Constant)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				this._accessMode = value;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003AA1B File Offset: 0x00038C1B
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0003AA23 File Offset: 0x00038C23
		public Version ClrVersion { get; internal set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0003AA2C File Offset: 0x00038C2C
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0003AA34 File Offset: 0x00038C34
		public string CompanyName { get; internal set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0003AA3D File Offset: 0x00038C3D
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0003AA45 File Offset: 0x00038C45
		public string Copyright { get; internal set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0003AA4E File Offset: 0x00038C4E
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0003AA56 File Offset: 0x00038C56
		public Version DotNetFrameworkVersion { get; internal set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0003AA60 File Offset: 0x00038C60
		public Dictionary<string, FunctionInfo> ExportedFunctions
		{
			get
			{
				Dictionary<string, FunctionInfo> dictionary = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.DeclaredFunctionExports != null && this.DeclaredFunctionExports.Count > 0)
				{
					using (IEnumerator<string> enumerator = this.DeclaredFunctionExports.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							FunctionInfo functionInfo = new FunctionInfo(text, ScriptBlock.Create(""), null);
							functionInfo.SetModule(this);
							dictionary[text] = functionInfo;
						}
						return dictionary;
					}
				}
				if (this.DeclaredFunctionExports != null && this.DeclaredFunctionExports.Count == 0)
				{
					return dictionary;
				}
				if (this._sessionState != null)
				{
					if (this._sessionState.Internal.ExportedFunctions == null)
					{
						return dictionary;
					}
					using (List<FunctionInfo>.Enumerator enumerator2 = this._sessionState.Internal.ExportedFunctions.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FunctionInfo functionInfo2 = enumerator2.Current;
							if (!dictionary.ContainsKey(functionInfo2.Name))
							{
								dictionary[ModuleCmdletBase.AddPrefixToCommandName(functionInfo2.Name, functionInfo2.Prefix)] = functionInfo2;
							}
						}
						return dictionary;
					}
				}
				foreach (string text2 in this._detectedFunctionExports)
				{
					if (!dictionary.ContainsKey(text2))
					{
						FunctionInfo functionInfo3 = new FunctionInfo(text2, ScriptBlock.Create(""), null);
						functionInfo3.SetModule(this);
						dictionary[text2] = functionInfo3;
					}
				}
				return dictionary;
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003AC00 File Offset: 0x00038E00
		private bool IsScriptModuleFile(string path)
		{
			string extension = System.IO.Path.GetExtension(path);
			return extension != null && PSModuleInfo.ScriptModuleExtensions.Contains(extension);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003AC24 File Offset: 0x00038E24
		public ReadOnlyDictionary<string, TypeDefinitionAst> GetExportedTypeDefinitions()
		{
			if (this._exportedTypeDefinitionsIncludeNesting == null)
			{
				if (this._exportedTypeDefinitions == null)
				{
					string text = null;
					if (this.RootModule == null)
					{
						if (this.Path != null)
						{
							text = this.Path;
						}
					}
					else
					{
						text = System.IO.Path.Combine(this.ModuleBase, this.RootModule);
					}
					this.CreateExportedTypeDefinitions((text != null && this.IsScriptModuleFile(text) && File.Exists(text)) ? new ExternalScriptInfo(text, text).GetScriptBlockAst() : null);
				}
				Dictionary<string, TypeDefinitionAst> dictionary = new Dictionary<string, TypeDefinitionAst>(StringComparer.OrdinalIgnoreCase);
				if (this.NestedModules != null)
				{
					foreach (PSModuleInfo psmoduleInfo in this.NestedModules)
					{
						if (psmoduleInfo != this)
						{
							foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in psmoduleInfo.GetExportedTypeDefinitions())
							{
								dictionary[keyValuePair.Key] = keyValuePair.Value;
							}
						}
					}
					foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair2 in this._exportedTypeDefinitions)
					{
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				this._exportedTypeDefinitionsIncludeNesting = new ReadOnlyDictionary<string, TypeDefinitionAst>(dictionary);
			}
			return this._exportedTypeDefinitionsIncludeNesting;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		internal void CreateExportedTypeDefinitions(ScriptBlockAst moduleContentScriptBlockAsts)
		{
			if (moduleContentScriptBlockAsts == null)
			{
				this._exportedTypeDefinitions = PSModuleInfo.EmptyTypeDefinitionDictionary;
			}
			else
			{
				this._exportedTypeDefinitions = new ReadOnlyDictionary<string, TypeDefinitionAst>(moduleContentScriptBlockAsts.FindAll((Ast a) => a is TypeDefinitionAst, false).OfType<TypeDefinitionAst>().ToDictionary((TypeDefinitionAst a) => a.Name, StringComparer.OrdinalIgnoreCase));
			}
			this._exportedTypeDefinitionsIncludeNesting = null;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0003AE37 File Offset: 0x00039037
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0003AE3F File Offset: 0x0003903F
		public string Prefix { get; internal set; }

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003AE48 File Offset: 0x00039048
		internal void AddDetectedFunctionExport(string name)
		{
			if (!this._detectedFunctionExports.Contains(name))
			{
				this._detectedFunctionExports.Add(name);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003AE64 File Offset: 0x00039064
		internal void AddDetectedWorkflowExport(string name)
		{
			if (!this._detectedWorkflowExports.Contains(name))
			{
				this._detectedWorkflowExports.Add(name);
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0003AE80 File Offset: 0x00039080
		public Dictionary<string, CmdletInfo> ExportedCmdlets
		{
			get
			{
				Dictionary<string, CmdletInfo> dictionary = new Dictionary<string, CmdletInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.DeclaredCmdletExports != null && this.DeclaredCmdletExports.Count > 0)
				{
					using (IEnumerator<string> enumerator = this.DeclaredCmdletExports.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							CmdletInfo cmdletInfo = new CmdletInfo(text, null, null, null, null);
							cmdletInfo.SetModule(this);
							dictionary[text] = cmdletInfo;
						}
						return dictionary;
					}
				}
				if (this.DeclaredCmdletExports != null && this.DeclaredCmdletExports.Count == 0)
				{
					return dictionary;
				}
				if (this.CompiledExports != null && this.CompiledExports.Count > 0)
				{
					using (List<CmdletInfo>.Enumerator enumerator2 = this.CompiledExports.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CmdletInfo cmdletInfo2 = enumerator2.Current;
							dictionary[cmdletInfo2.Name] = cmdletInfo2;
						}
						return dictionary;
					}
				}
				foreach (string text2 in this._detectedCmdletExports)
				{
					if (!dictionary.ContainsKey(text2))
					{
						CmdletInfo cmdletInfo3 = new CmdletInfo(text2, null, null, null, null);
						cmdletInfo3.SetModule(this);
						dictionary[text2] = cmdletInfo3;
					}
				}
				return dictionary;
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003AFE8 File Offset: 0x000391E8
		internal void AddDetectedCmdletExport(string cmdlet)
		{
			if (!this._detectedCmdletExports.Contains(cmdlet))
			{
				this._detectedCmdletExports.Add(cmdlet);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0003B004 File Offset: 0x00039204
		public Dictionary<string, CommandInfo> ExportedCommands
		{
			get
			{
				Dictionary<string, CommandInfo> dictionary = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase);
				Dictionary<string, CmdletInfo> exportedCmdlets = this.ExportedCmdlets;
				if (exportedCmdlets != null)
				{
					foreach (KeyValuePair<string, CmdletInfo> keyValuePair in exportedCmdlets)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				Dictionary<string, FunctionInfo> exportedFunctions = this.ExportedFunctions;
				if (exportedFunctions != null)
				{
					foreach (KeyValuePair<string, FunctionInfo> keyValuePair2 in exportedFunctions)
					{
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
				Dictionary<string, FunctionInfo> exportedWorkflows = this.ExportedWorkflows;
				if (exportedWorkflows != null)
				{
					foreach (KeyValuePair<string, FunctionInfo> keyValuePair3 in exportedWorkflows)
					{
						dictionary[keyValuePair3.Key] = keyValuePair3.Value;
					}
				}
				Dictionary<string, AliasInfo> exportedAliases = this.ExportedAliases;
				if (exportedAliases != null)
				{
					foreach (KeyValuePair<string, AliasInfo> keyValuePair4 in exportedAliases)
					{
						dictionary[keyValuePair4.Key] = keyValuePair4.Value;
					}
				}
				return dictionary;
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003B180 File Offset: 0x00039380
		internal void AddExportedCmdlet(CmdletInfo cmdlet)
		{
			this._compiledExports.Add(cmdlet);
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0003B190 File Offset: 0x00039390
		internal List<CmdletInfo> CompiledExports
		{
			get
			{
				if (this._sessionState != null && this._sessionState.Internal.ExportedCmdlets != null && this._sessionState.Internal.ExportedCmdlets.Count > 0)
				{
					foreach (CmdletInfo item in this._sessionState.Internal.ExportedCmdlets)
					{
						this._compiledExports.Add(item);
					}
					this._sessionState.Internal.ExportedCmdlets.Clear();
				}
				return this._compiledExports;
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0003B244 File Offset: 0x00039444
		internal void AddExportedAlias(AliasInfo aliasInfo)
		{
			this._compiledAliasExports.Add(aliasInfo);
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0003B252 File Offset: 0x00039452
		internal List<AliasInfo> CompiledAliasExports
		{
			get
			{
				return this._compiledAliasExports;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0003B25A File Offset: 0x0003945A
		public IEnumerable<string> FileList
		{
			get
			{
				return this._fileList;
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0003B262 File Offset: 0x00039462
		internal void AddToFileList(string file)
		{
			this._fileList.Add(file);
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0003B270 File Offset: 0x00039470
		public IEnumerable<object> ModuleList
		{
			get
			{
				return this._moduleList;
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0003B278 File Offset: 0x00039478
		internal void AddToModuleList(object m)
		{
			this._moduleList.Add(m);
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0003B288 File Offset: 0x00039488
		public ReadOnlyCollection<PSModuleInfo> NestedModules
		{
			get
			{
				ReadOnlyCollection<PSModuleInfo> result;
				if ((result = this._readonlyNestedModules) == null)
				{
					result = (this._readonlyNestedModules = new ReadOnlyCollection<PSModuleInfo>(this._nestedModules));
				}
				return result;
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0003B2B3 File Offset: 0x000394B3
		internal void AddNestedModule(PSModuleInfo nestedModule)
		{
			PSModuleInfo.AddModuleToList(nestedModule, this._nestedModules);
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0003B2C1 File Offset: 0x000394C1
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0003B2C9 File Offset: 0x000394C9
		public string PowerShellHostName { get; internal set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0003B2D2 File Offset: 0x000394D2
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0003B2DA File Offset: 0x000394DA
		public Version PowerShellHostVersion { get; internal set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0003B2E3 File Offset: 0x000394E3
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0003B2EB File Offset: 0x000394EB
		public Version PowerShellVersion { get; internal set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0003B2F4 File Offset: 0x000394F4
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0003B2FC File Offset: 0x000394FC
		public ProcessorArchitecture ProcessorArchitecture { get; internal set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0003B305 File Offset: 0x00039505
		public IEnumerable<string> Scripts
		{
			get
			{
				return this._scripts;
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003B30D File Offset: 0x0003950D
		internal void AddScript(string s)
		{
			this._scripts.Add(s);
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0003B31B File Offset: 0x0003951B
		public IEnumerable<string> RequiredAssemblies
		{
			get
			{
				return this._requiredAssemblies;
			}
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003B323 File Offset: 0x00039523
		internal void AddRequiredAssembly(string assembly)
		{
			this._requiredAssemblies.Add(assembly);
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0003B334 File Offset: 0x00039534
		public ReadOnlyCollection<PSModuleInfo> RequiredModules
		{
			get
			{
				ReadOnlyCollection<PSModuleInfo> result;
				if ((result = this._readonlyRequiredModules) == null)
				{
					result = (this._readonlyRequiredModules = new ReadOnlyCollection<PSModuleInfo>(this._requiredModules));
				}
				return result;
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003B35F File Offset: 0x0003955F
		internal void AddRequiredModule(PSModuleInfo requiredModule)
		{
			PSModuleInfo.AddModuleToList(requiredModule, this._requiredModules);
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0003B370 File Offset: 0x00039570
		internal ReadOnlyCollection<ModuleSpecification> RequiredModulesSpecification
		{
			get
			{
				ReadOnlyCollection<ModuleSpecification> result;
				if ((result = this._readonlyRequiredModulesSpecification) == null)
				{
					result = (this._readonlyRequiredModulesSpecification = new ReadOnlyCollection<ModuleSpecification>(this._requiredModulesSpecification));
				}
				return result;
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0003B39B File Offset: 0x0003959B
		internal void AddRequiredModuleSpecification(ModuleSpecification requiredModuleSpecification)
		{
			this._requiredModulesSpecification.Add(requiredModuleSpecification);
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0003B3A9 File Offset: 0x000395A9
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0003B3B1 File Offset: 0x000395B1
		public string RootModule { get; internal set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0003B3BA File Offset: 0x000395BA
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x0003B3C2 File Offset: 0x000395C2
		internal string RootModuleForManifest { get; set; }

		// Token: 0x060009F7 RID: 2551 RVA: 0x0003B3CC File Offset: 0x000395CC
		private static void AddModuleToList(PSModuleInfo module, List<PSModuleInfo> moduleList)
		{
			foreach (PSModuleInfo psmoduleInfo in moduleList)
			{
				if (psmoduleInfo.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			moduleList.Add(module);
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0003B430 File Offset: 0x00039630
		public Dictionary<string, PSVariable> ExportedVariables
		{
			get
			{
				Dictionary<string, PSVariable> dictionary = new Dictionary<string, PSVariable>(StringComparer.OrdinalIgnoreCase);
				if (this.DeclaredVariableExports != null && this.DeclaredVariableExports.Count > 0)
				{
					using (IEnumerator<string> enumerator = this.DeclaredVariableExports.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string key = enumerator.Current;
							dictionary[key] = null;
						}
						return dictionary;
					}
				}
				if (this._sessionState == null || this._sessionState.Internal.ExportedVariables == null)
				{
					return dictionary;
				}
				foreach (PSVariable psvariable in this._sessionState.Internal.ExportedVariables)
				{
					dictionary[psvariable.Name] = psvariable;
				}
				return dictionary;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0003B510 File Offset: 0x00039710
		public Dictionary<string, AliasInfo> ExportedAliases
		{
			get
			{
				Dictionary<string, AliasInfo> dictionary = new Dictionary<string, AliasInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.DeclaredAliasExports != null && this.DeclaredAliasExports.Count > 0)
				{
					using (IEnumerator<string> enumerator = this.DeclaredAliasExports.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							AliasInfo aliasInfo = new AliasInfo(text, null, null);
							aliasInfo.SetModule(this);
							dictionary[text] = aliasInfo;
						}
						return dictionary;
					}
				}
				if (this.CompiledAliasExports != null && this.CompiledAliasExports.Count > 0)
				{
					using (List<AliasInfo>.Enumerator enumerator2 = this.CompiledAliasExports.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							AliasInfo aliasInfo2 = enumerator2.Current;
							dictionary[aliasInfo2.Name] = aliasInfo2;
						}
						return dictionary;
					}
				}
				if (this._sessionState == null)
				{
					if (this._detectedAliasExports.Count > 0)
					{
						using (Dictionary<string, string>.KeyCollection.Enumerator enumerator3 = this._detectedAliasExports.Keys.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								string text2 = enumerator3.Current;
								if (!dictionary.ContainsKey(text2))
								{
									AliasInfo aliasInfo3 = new AliasInfo(text2, this._detectedAliasExports[text2], null);
									aliasInfo3.SetModule(this);
									dictionary[text2] = aliasInfo3;
								}
							}
							return dictionary;
						}
					}
					return dictionary;
				}
				foreach (AliasInfo aliasInfo4 in this._sessionState.Internal.ExportedAliases)
				{
					dictionary[aliasInfo4.Name] = aliasInfo4;
				}
				return dictionary;
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003B6E0 File Offset: 0x000398E0
		internal void AddDetectedAliasExport(string name, string value)
		{
			this._detectedAliasExports[name] = value;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0003B6F0 File Offset: 0x000398F0
		public Dictionary<string, FunctionInfo> ExportedWorkflows
		{
			get
			{
				Dictionary<string, FunctionInfo> dictionary = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
				if (this.DeclaredWorkflowExports != null && this.DeclaredWorkflowExports.Count > 0)
				{
					foreach (string text in this.DeclaredWorkflowExports)
					{
						WorkflowInfo workflowInfo = new WorkflowInfo(text, ScriptBlock.Create(""), null);
						workflowInfo.SetModule(this);
						dictionary[text] = workflowInfo;
					}
				}
				if (this.DeclaredWorkflowExports != null && this.DeclaredWorkflowExports.Count == 0)
				{
					return dictionary;
				}
				if (this._sessionState == null)
				{
					foreach (string text2 in this._detectedWorkflowExports)
					{
						if (!dictionary.ContainsKey(text2))
						{
							WorkflowInfo workflowInfo2 = new WorkflowInfo(text2, ScriptBlock.Create(""), null);
							workflowInfo2.SetModule(this);
							dictionary[text2] = workflowInfo2;
						}
					}
					return dictionary;
				}
				foreach (WorkflowInfo workflowInfo3 in this._sessionState.Internal.ExportedWorkflows)
				{
					dictionary[workflowInfo3.Name] = workflowInfo3;
				}
				return dictionary;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0003B860 File Offset: 0x00039A60
		public ReadOnlyCollection<string> ExportedDscResources
		{
			get
			{
				IList<string> list2;
				if (this._declaredDscResourceExports == null)
				{
					IList<string> list = new string[0];
					list2 = list;
				}
				else
				{
					list2 = this._declaredDscResourceExports;
				}
				return new ReadOnlyCollection<string>(list2);
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0003B88A File Offset: 0x00039A8A
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x0003B892 File Offset: 0x00039A92
		public SessionState SessionState
		{
			get
			{
				return this._sessionState;
			}
			set
			{
				this._sessionState = value;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0003B89C File Offset: 0x00039A9C
		public ScriptBlock NewBoundScriptBlock(ScriptBlock scriptBlockToBind)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			return this.NewBoundScriptBlock(scriptBlockToBind, executionContextFromTLS);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0003B8B8 File Offset: 0x00039AB8
		internal ScriptBlock NewBoundScriptBlock(ScriptBlock scriptBlockToBind, ExecutionContext context)
		{
			if (this._sessionState == null || context == null)
			{
				throw PSTraceSource.NewInvalidOperationException(Modules.InvalidOperationOnBinaryModule, new object[0]);
			}
			ScriptBlock scriptBlock;
			lock (context.EngineSessionState)
			{
				SessionStateInternal engineSessionState2 = context.EngineSessionState;
				try
				{
					context.EngineSessionState = this._sessionState.Internal;
					scriptBlock = scriptBlockToBind.Clone(true);
					scriptBlock.SessionState = this._sessionState;
				}
				finally
				{
					context.EngineSessionState = engineSessionState2;
				}
			}
			return scriptBlock;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0003B954 File Offset: 0x00039B54
		public object Invoke(ScriptBlock sb, params object[] args)
		{
			if (sb == null)
			{
				return null;
			}
			SessionStateInternal sessionStateInternal = sb.SessionStateInternal;
			object result;
			try
			{
				sb.SessionStateInternal = this._sessionState.Internal;
				result = sb.InvokeReturnAsIs(args);
			}
			finally
			{
				sb.SessionStateInternal = sessionStateInternal;
			}
			return result;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0003B9A4 File Offset: 0x00039BA4
		public PSVariable GetVariableFromCallersModule(string variableName)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				throw new ArgumentNullException("variableName");
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			SessionState sessionState = null;
			foreach (CallStackFrame callStackFrame in executionContextFromTLS.Debugger.GetCallStack())
			{
				PSModuleInfo module = callStackFrame.InvocationInfo.MyCommand.Module;
				if (module == null)
				{
					break;
				}
				if (module.SessionState != this._sessionState)
				{
					sessionState = callStackFrame.InvocationInfo.MyCommand.Module.SessionState;
					break;
				}
			}
			if (sessionState != null)
			{
				return sessionState.Internal.GetVariable(variableName);
			}
			return executionContextFromTLS.TopLevelSessionState.GetVariable(variableName);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0003BA68 File Offset: 0x00039C68
		internal void CaptureLocals()
		{
			if (this._sessionState == null)
			{
				throw PSTraceSource.NewInvalidOperationException(Modules.InvalidOperationOnBinaryModule, new object[0]);
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			MutableTuple localsTuple = executionContextFromTLS.EngineSessionState.CurrentScope.LocalsTuple;
			IEnumerable<PSVariable> enumerable = executionContextFromTLS.EngineSessionState.CurrentScope.Variables.Values;
			if (localsTuple != null)
			{
				Dictionary<string, PSVariable> dictionary = new Dictionary<string, PSVariable>();
				localsTuple.GetVariableTable(dictionary, false);
				enumerable = dictionary.Values.Concat(enumerable);
			}
			foreach (PSVariable psvariable in enumerable)
			{
				try
				{
					if (psvariable.Options == ScopedItemOptions.None && !(psvariable is NullVariable))
					{
						PSVariable variable = new PSVariable(psvariable.Name, psvariable.Value, psvariable.Options, psvariable.Attributes, psvariable.Description);
						this._sessionState.Internal.NewVariable(variable, false);
					}
				}
				catch (SessionStateException)
				{
				}
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0003BB74 File Offset: 0x00039D74
		public PSObject AsCustomObject()
		{
			if (this._sessionState == null)
			{
				throw PSTraceSource.NewInvalidOperationException(Modules.InvalidOperationOnBinaryModule, new object[0]);
			}
			PSObject psobject = new PSObject();
			foreach (KeyValuePair<string, FunctionInfo> keyValuePair in this.ExportedFunctions)
			{
				FunctionInfo value = keyValuePair.Value;
				if (value != null)
				{
					PSScriptMethod member = new PSScriptMethod(value.Name, value.ScriptBlock);
					psobject.Members.Add(member);
				}
			}
			foreach (KeyValuePair<string, PSVariable> keyValuePair2 in this.ExportedVariables)
			{
				PSVariable value2 = keyValuePair2.Value;
				if (value2 != null)
				{
					PSVariableProperty member2 = new PSVariableProperty(value2);
					psobject.Members.Add(member2);
				}
			}
			return psobject;
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003BC6C File Offset: 0x00039E6C
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0003BC74 File Offset: 0x00039E74
		public ScriptBlock OnRemove { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0003BC7D File Offset: 0x00039E7D
		public ReadOnlyCollection<string> ExportedFormatFiles
		{
			get
			{
				return this._exportedFormatFiles;
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0003BC85 File Offset: 0x00039E85
		internal void SetExportedFormatFiles(ReadOnlyCollection<string> files)
		{
			this._exportedFormatFiles = files;
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0003BC8E File Offset: 0x00039E8E
		public ReadOnlyCollection<string> ExportedTypeFiles
		{
			get
			{
				return this._exportedTypeFiles;
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003BC96 File Offset: 0x00039E96
		internal void SetExportedTypeFiles(ReadOnlyCollection<string> files)
		{
			this._exportedTypeFiles = files;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0003BCA0 File Offset: 0x00039EA0
		public PSModuleInfo Clone()
		{
			PSModuleInfo psmoduleInfo = (PSModuleInfo)base.MemberwiseClone();
			psmoduleInfo._fileList = new List<string>(this.FileList);
			psmoduleInfo._moduleList = new Collection<object>(this._moduleList);
			foreach (PSModuleInfo nestedModule in this.NestedModules)
			{
				psmoduleInfo.AddNestedModule(nestedModule);
			}
			psmoduleInfo._readonlyNestedModules = new ReadOnlyCollection<PSModuleInfo>(this.NestedModules);
			psmoduleInfo._readonlyRequiredModules = new ReadOnlyCollection<PSModuleInfo>(this.RequiredModules);
			psmoduleInfo._readonlyRequiredModulesSpecification = new ReadOnlyCollection<ModuleSpecification>(this.RequiredModulesSpecification);
			psmoduleInfo._requiredAssemblies = new Collection<string>(this._requiredAssemblies);
			psmoduleInfo._requiredModulesSpecification = new List<ModuleSpecification>();
			psmoduleInfo._requiredModules = new List<PSModuleInfo>();
			foreach (PSModuleInfo requiredModule in this._requiredModules)
			{
				psmoduleInfo.AddRequiredModule(requiredModule);
			}
			foreach (ModuleSpecification requiredModuleSpecification in this._requiredModulesSpecification)
			{
				psmoduleInfo.AddRequiredModuleSpecification(requiredModuleSpecification);
			}
			psmoduleInfo._scripts = new List<string>(this.Scripts);
			psmoduleInfo._sessionState = this.SessionState;
			return psmoduleInfo;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0003BE20 File Offset: 0x0003A020
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0003BE27 File Offset: 0x0003A027
		public static bool UseAppDomainLevelModuleCache { get; set; }

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003BE2F File Offset: 0x0003A02F
		public static void ClearAppDomainLevelModulePathCache()
		{
			PSModuleInfo._appdomainModulePathCache.Clear();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003BE3C File Offset: 0x0003A03C
		internal static string ResolveUsingAppDomainLevelModuleCache(string moduleName)
		{
			string result;
			if (PSModuleInfo._appdomainModulePathCache.TryGetValue(moduleName, out result))
			{
				return result;
			}
			return string.Empty;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0003BE70 File Offset: 0x0003A070
		internal static void AddToAppDomainLevelModuleCache(string moduleName, string path, bool force)
		{
			if (force)
			{
				PSModuleInfo._appdomainModulePathCache.AddOrUpdate(moduleName, path, (string modulename, string oldPath) => path);
				return;
			}
			PSModuleInfo._appdomainModulePathCache.TryAdd(moduleName, path);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0003BEC8 File Offset: 0x0003A0C8
		internal static bool RemoveFromAppDomainLevelCache(string moduleName)
		{
			string text;
			return PSModuleInfo._appdomainModulePathCache.TryRemove(moduleName, out text);
		}

		// Token: 0x04000441 RID: 1089
		internal const string DynamicModulePrefixString = "__DynamicModule_";

		// Token: 0x04000442 RID: 1090
		private static readonly ReadOnlyDictionary<string, TypeDefinitionAst> EmptyTypeDefinitionDictionary = new ReadOnlyDictionary<string, TypeDefinitionAst>(new Dictionary<string, TypeDefinitionAst>(StringComparer.OrdinalIgnoreCase));

		// Token: 0x04000443 RID: 1091
		private static readonly HashSet<string> ScriptModuleExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			".psm1"
		};

		// Token: 0x04000444 RID: 1092
		private bool _moduleHasPrivateMembers;

		// Token: 0x04000445 RID: 1093
		private bool _logPipelineExecutionDetails;

		// Token: 0x04000446 RID: 1094
		private string _name;

		// Token: 0x04000447 RID: 1095
		private string _path;

		// Token: 0x04000448 RID: 1096
		internal IScriptExtent _definitionExtent;

		// Token: 0x04000449 RID: 1097
		private string _description;

		// Token: 0x0400044A RID: 1098
		private Guid _guid;

		// Token: 0x0400044B RID: 1099
		private string _helpInfoUri;

		// Token: 0x0400044C RID: 1100
		private string _moduleBase;

		// Token: 0x0400044D RID: 1101
		private object _privateData;

		// Token: 0x0400044E RID: 1102
		private readonly List<string> _tags;

		// Token: 0x0400044F RID: 1103
		private Version _version;

		// Token: 0x04000450 RID: 1104
		private ModuleType _moduleType;

		// Token: 0x04000451 RID: 1105
		private ModuleAccessMode _accessMode;

		// Token: 0x04000452 RID: 1106
		internal Collection<string> DeclaredFunctionExports;

		// Token: 0x04000453 RID: 1107
		internal List<string> _detectedFunctionExports;

		// Token: 0x04000454 RID: 1108
		internal List<string> _detectedWorkflowExports;

		// Token: 0x04000455 RID: 1109
		internal Collection<string> DeclaredCmdletExports;

		// Token: 0x04000456 RID: 1110
		internal List<string> _detectedCmdletExports;

		// Token: 0x04000457 RID: 1111
		private readonly List<CmdletInfo> _compiledExports;

		// Token: 0x04000458 RID: 1112
		private readonly List<AliasInfo> _compiledAliasExports;

		// Token: 0x04000459 RID: 1113
		private List<string> _fileList;

		// Token: 0x0400045A RID: 1114
		private Collection<object> _moduleList;

		// Token: 0x0400045B RID: 1115
		private ReadOnlyCollection<PSModuleInfo> _readonlyNestedModules;

		// Token: 0x0400045C RID: 1116
		private readonly List<PSModuleInfo> _nestedModules;

		// Token: 0x0400045D RID: 1117
		private List<string> _scripts;

		// Token: 0x0400045E RID: 1118
		private Collection<string> _requiredAssemblies;

		// Token: 0x0400045F RID: 1119
		private ReadOnlyCollection<PSModuleInfo> _readonlyRequiredModules;

		// Token: 0x04000460 RID: 1120
		private List<PSModuleInfo> _requiredModules;

		// Token: 0x04000461 RID: 1121
		private ReadOnlyCollection<ModuleSpecification> _readonlyRequiredModulesSpecification;

		// Token: 0x04000462 RID: 1122
		private List<ModuleSpecification> _requiredModulesSpecification;

		// Token: 0x04000463 RID: 1123
		internal static string[] _builtinVariables = new string[]
		{
			"_",
			"this",
			"input",
			"args",
			"true",
			"false",
			"null",
			"MaximumErrorCount",
			"MaximumVariableCount",
			"MaximumFunctionCount",
			"MaximumAliasCount",
			"PSDefaultParameterValues",
			"MaximumDriveCount",
			"Error",
			"PSScriptRoot",
			"PSCommandPath",
			"MyInvocation",
			"ExecutionContext",
			"StackTrace"
		};

		// Token: 0x04000464 RID: 1124
		internal Collection<string> DeclaredVariableExports;

		// Token: 0x04000465 RID: 1125
		internal Collection<string> DeclaredAliasExports;

		// Token: 0x04000466 RID: 1126
		internal Dictionary<string, string> _detectedAliasExports;

		// Token: 0x04000467 RID: 1127
		internal Collection<string> DeclaredWorkflowExports;

		// Token: 0x04000468 RID: 1128
		internal Collection<string> _declaredDscResourceExports;

		// Token: 0x04000469 RID: 1129
		private SessionState _sessionState;

		// Token: 0x0400046A RID: 1130
		private ReadOnlyCollection<string> _exportedFormatFiles;

		// Token: 0x0400046B RID: 1131
		private ReadOnlyCollection<string> _exportedTypeFiles;

		// Token: 0x0400046C RID: 1132
		private static readonly ConcurrentDictionary<string, string> _appdomainModulePathCache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
