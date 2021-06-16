using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000B3 RID: 179
	[OutputType(new Type[]
	{
		typeof(string)
	})]
	[Cmdlet("New", "ModuleManifest", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141555")]
	public sealed class NewModuleManifestCommand : PSCmdlet
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x000383E4 File Offset: 0x000365E4
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x000383EC File Offset: 0x000365EC
		[Parameter(Mandatory = true, Position = 0)]
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x000383F5 File Offset: 0x000365F5
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x000383FD File Offset: 0x000365FD
		[Parameter]
		[AllowEmptyCollection]
		public object[] NestedModules
		{
			get
			{
				return this._nestedModules;
			}
			set
			{
				this._nestedModules = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00038406 File Offset: 0x00036606
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0003840E File Offset: 0x0003660E
		[Parameter]
		public Guid Guid
		{
			get
			{
				return this._guid;
			}
			set
			{
				this._guid = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00038417 File Offset: 0x00036617
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0003841F File Offset: 0x0003661F
		[Parameter]
		[AllowEmptyString]
		public string Author
		{
			get
			{
				return this._author;
			}
			set
			{
				this._author = value;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00038428 File Offset: 0x00036628
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00038430 File Offset: 0x00036630
		[Parameter]
		[AllowEmptyString]
		public string CompanyName
		{
			get
			{
				return this._companyName;
			}
			set
			{
				this._companyName = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00038439 File Offset: 0x00036639
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x00038441 File Offset: 0x00036641
		[AllowEmptyString]
		[Parameter]
		public string Copyright
		{
			get
			{
				return this._copyright;
			}
			set
			{
				this._copyright = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0003844A File Offset: 0x0003664A
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x00038452 File Offset: 0x00036652
		[AllowEmptyString]
		[Parameter]
		[Alias(new string[]
		{
			"ModuleToProcess"
		})]
		public string RootModule
		{
			get
			{
				return this._rootModule;
			}
			set
			{
				this._rootModule = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0003845B File Offset: 0x0003665B
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00038463 File Offset: 0x00036663
		[Parameter]
		[ValidateNotNull]
		public Version ModuleVersion
		{
			get
			{
				return this._moduleVersion;
			}
			set
			{
				this._moduleVersion = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0003846C File Offset: 0x0003666C
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x00038474 File Offset: 0x00036674
		[AllowEmptyString]
		[Parameter]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0003847D File Offset: 0x0003667D
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00038499 File Offset: 0x00036699
		[Parameter]
		public ProcessorArchitecture ProcessorArchitecture
		{
			get
			{
				if (this._processorArchitecture == null)
				{
					return ProcessorArchitecture.None;
				}
				return this._processorArchitecture.Value;
			}
			set
			{
				this._processorArchitecture = new ProcessorArchitecture?(value);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x000384A7 File Offset: 0x000366A7
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x000384AF File Offset: 0x000366AF
		[Parameter]
		public Version PowerShellVersion
		{
			get
			{
				return this._powerShellVersion;
			}
			set
			{
				this._powerShellVersion = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000384B8 File Offset: 0x000366B8
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x000384C0 File Offset: 0x000366C0
		[Parameter]
		public Version ClrVersion
		{
			get
			{
				return this._ClrVersion;
			}
			set
			{
				this._ClrVersion = value;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x000384C9 File Offset: 0x000366C9
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x000384D1 File Offset: 0x000366D1
		[Parameter]
		public Version DotNetFrameworkVersion
		{
			get
			{
				return this._DotNetFrameworkVersion;
			}
			set
			{
				this._DotNetFrameworkVersion = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x000384DA File Offset: 0x000366DA
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x000384E2 File Offset: 0x000366E2
		[Parameter]
		public string PowerShellHostName
		{
			get
			{
				return this._PowerShellHostName;
			}
			set
			{
				this._PowerShellHostName = value;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x000384EB File Offset: 0x000366EB
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x000384F3 File Offset: 0x000366F3
		[Parameter]
		public Version PowerShellHostVersion
		{
			get
			{
				return this._PowerShellHostVersion;
			}
			set
			{
				this._PowerShellHostVersion = value;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x000384FC File Offset: 0x000366FC
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00038504 File Offset: 0x00036704
		[Parameter]
		[ArgumentTypeConverter(new Type[]
		{
			typeof(ModuleSpecification[])
		})]
		public object[] RequiredModules
		{
			get
			{
				return this._requiredModules;
			}
			set
			{
				this._requiredModules = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0003850D File Offset: 0x0003670D
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x00038515 File Offset: 0x00036715
		[Parameter]
		[AllowEmptyCollection]
		public string[] TypesToProcess
		{
			get
			{
				return this._types;
			}
			set
			{
				this._types = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0003851E File Offset: 0x0003671E
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x00038526 File Offset: 0x00036726
		[Parameter]
		[AllowEmptyCollection]
		public string[] FormatsToProcess
		{
			get
			{
				return this._formats;
			}
			set
			{
				this._formats = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0003852F File Offset: 0x0003672F
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00038537 File Offset: 0x00036737
		[AllowEmptyCollection]
		[Parameter]
		public string[] ScriptsToProcess
		{
			get
			{
				return this._scripts;
			}
			set
			{
				this._scripts = value;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00038540 File Offset: 0x00036740
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00038548 File Offset: 0x00036748
		[Parameter]
		[AllowEmptyCollection]
		public string[] RequiredAssemblies
		{
			get
			{
				return this._requiredAssemblies;
			}
			set
			{
				this._requiredAssemblies = value;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00038551 File Offset: 0x00036751
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00038559 File Offset: 0x00036759
		[Parameter]
		[AllowEmptyCollection]
		public string[] FileList
		{
			get
			{
				return this._miscFiles;
			}
			set
			{
				this._miscFiles = value;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00038562 File Offset: 0x00036762
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0003856A File Offset: 0x0003676A
		[AllowEmptyCollection]
		[ArgumentTypeConverter(new Type[]
		{
			typeof(ModuleSpecification[])
		})]
		[Parameter]
		public object[] ModuleList
		{
			get
			{
				return this._moduleList;
			}
			set
			{
				this._moduleList = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00038573 File Offset: 0x00036773
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0003857B File Offset: 0x0003677B
		[AllowEmptyCollection]
		[Parameter]
		public string[] FunctionsToExport
		{
			get
			{
				return this._exportedFunctions;
			}
			set
			{
				this._exportedFunctions = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00038584 File Offset: 0x00036784
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x0003858C File Offset: 0x0003678C
		[Parameter]
		[AllowEmptyCollection]
		public string[] AliasesToExport
		{
			get
			{
				return this._exportedAliases;
			}
			set
			{
				this._exportedAliases = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00038595 File Offset: 0x00036795
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0003859D File Offset: 0x0003679D
		[AllowEmptyCollection]
		[Parameter]
		public string[] VariablesToExport
		{
			get
			{
				return this._exportedVariables;
			}
			set
			{
				this._exportedVariables = value;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x000385A6 File Offset: 0x000367A6
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x000385AE File Offset: 0x000367AE
		[AllowEmptyCollection]
		[Parameter]
		public string[] CmdletsToExport
		{
			get
			{
				return this._exportedCmdlets;
			}
			set
			{
				this._exportedCmdlets = value;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x000385B7 File Offset: 0x000367B7
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x000385BF File Offset: 0x000367BF
		[AllowEmptyCollection]
		[Parameter]
		public string[] DscResourcesToExport
		{
			get
			{
				return this._dscResourcesToExport;
			}
			set
			{
				this._dscResourcesToExport = value;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x000385C8 File Offset: 0x000367C8
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x000385D0 File Offset: 0x000367D0
		[Parameter(Mandatory = false)]
		[AllowNull]
		public object PrivateData
		{
			get
			{
				return this._privateData;
			}
			set
			{
				this._privateData = value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x000385D9 File Offset: 0x000367D9
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x000385E1 File Offset: 0x000367E1
		[Parameter(Mandatory = false)]
		[ValidateNotNullOrEmpty]
		public string[] Tags { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x000385EA File Offset: 0x000367EA
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x000385F2 File Offset: 0x000367F2
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = false)]
		public Uri ProjectUri { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x000385FB File Offset: 0x000367FB
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00038603 File Offset: 0x00036803
		[Parameter(Mandatory = false)]
		[ValidateNotNullOrEmpty]
		public Uri LicenseUri { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0003860C File Offset: 0x0003680C
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00038614 File Offset: 0x00036814
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = false)]
		public Uri IconUri { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0003861D File Offset: 0x0003681D
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x00038625 File Offset: 0x00036825
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = false)]
		public string ReleaseNotes { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0003862E File Offset: 0x0003682E
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00038636 File Offset: 0x00036836
		[Parameter]
		[AllowNull]
		public string HelpInfoUri
		{
			get
			{
				return this._helpInfoUri;
			}
			set
			{
				this._helpInfoUri = value;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0003863F File Offset: 0x0003683F
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0003864C File Offset: 0x0003684C
		[Parameter]
		public SwitchParameter PassThru
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

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0003865A File Offset: 0x0003685A
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x00038662 File Offset: 0x00036862
		[AllowNull]
		[Parameter]
		public string DefaultCommandPrefix
		{
			get
			{
				return this._defaultCommandPrefix;
			}
			set
			{
				this._defaultCommandPrefix = value;
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0003866B File Offset: 0x0003686B
		private string QuoteName(object name)
		{
			if (name == null)
			{
				return "''";
			}
			return "'" + name.ToString().Replace("'", "''") + "'";
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0003869C File Offset: 0x0003689C
		private string QuoteNames(IEnumerable names, StreamWriter streamWriter)
		{
			if (names == null)
			{
				return "@()";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 15;
			bool flag = true;
			foreach (object obj in names)
			{
				string text = (string)obj;
				if (!string.IsNullOrEmpty(text))
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					string text2 = this.QuoteName(text);
					num += text2.Length;
					if (num > 80)
					{
						stringBuilder.Append(streamWriter.NewLine);
						stringBuilder.Append("               ");
						num = 15 + text2.Length;
					}
					stringBuilder.Append(text2);
				}
			}
			if (stringBuilder.Length == 0)
			{
				return "@()";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00038960 File Offset: 0x00036B60
		private IEnumerable PreProcessModuleSpec(IEnumerable moduleSpecs)
		{
			if (moduleSpecs != null)
			{
				foreach (object spec in moduleSpecs)
				{
					if (!(spec is Hashtable))
					{
						yield return spec.ToString();
					}
					else
					{
						yield return spec;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00038984 File Offset: 0x00036B84
		private string QuoteModules(IEnumerable moduleSpecs, StreamWriter streamWriter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("@(");
			if (moduleSpecs != null)
			{
				bool flag = true;
				foreach (object obj in moduleSpecs)
				{
					if (obj != null)
					{
						ModuleSpecification moduleSpecification = (ModuleSpecification)LanguagePrimitives.ConvertTo(obj, typeof(ModuleSpecification), CultureInfo.InvariantCulture);
						if (!flag)
						{
							stringBuilder.Append(", ");
							stringBuilder.Append(streamWriter.NewLine);
							stringBuilder.Append("               ");
						}
						flag = false;
						if (moduleSpecification.Guid == null && moduleSpecification.Version == null && moduleSpecification.MaximumVersion == null && moduleSpecification.RequiredVersion == null)
						{
							stringBuilder.Append(this.QuoteName(moduleSpecification.Name));
						}
						else
						{
							stringBuilder.Append("@{");
							stringBuilder.Append("ModuleName = ");
							stringBuilder.Append(this.QuoteName(moduleSpecification.Name));
							stringBuilder.Append("; ");
							if (moduleSpecification.Guid != null)
							{
								stringBuilder.Append("GUID = ");
								stringBuilder.Append(this.QuoteName(moduleSpecification.Guid.ToString()));
								stringBuilder.Append("; ");
							}
							if (moduleSpecification.Version != null)
							{
								stringBuilder.Append("ModuleVersion = ");
								stringBuilder.Append(this.QuoteName(moduleSpecification.Version.ToString()));
								stringBuilder.Append("; ");
							}
							if (moduleSpecification.MaximumVersion != null)
							{
								stringBuilder.Append("MaximumVersion = ");
								stringBuilder.Append(this.QuoteName(moduleSpecification.MaximumVersion));
								stringBuilder.Append("; ");
							}
							if (moduleSpecification.RequiredVersion != null)
							{
								stringBuilder.Append("RequiredVersion = ");
								stringBuilder.Append(this.QuoteName(moduleSpecification.RequiredVersion.ToString()));
								stringBuilder.Append("; ");
							}
							stringBuilder.Append("}");
						}
					}
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00038BE8 File Offset: 0x00036DE8
		private string QuoteFiles(IEnumerable names, StreamWriter streamWriter)
		{
			List<string> list = new List<string>();
			if (names != null)
			{
				foreach (object obj in names)
				{
					string text = (string)obj;
					if (!string.IsNullOrEmpty(text))
					{
						foreach (string item in this.TryResolveFilePath(text))
						{
							list.Add(item);
						}
					}
				}
			}
			return this.QuoteNames(list, streamWriter);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00038C98 File Offset: 0x00036E98
		private List<string> TryResolveFilePath(string filePath)
		{
			List<string> list = new List<string>();
			ProviderInfo providerInfo = null;
			SessionState sessionState = base.Context.SessionState;
			try
			{
				Collection<string> resolvedProviderPathFromPSPath = sessionState.Path.GetResolvedProviderPathFromPSPath(filePath, out providerInfo);
				if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || resolvedProviderPathFromPSPath == null || resolvedProviderPathFromPSPath.Count < 1)
				{
					list.Add(filePath);
					return list;
				}
				foreach (string path in resolvedProviderPathFromPSPath)
				{
					string text = base.SessionState.Path.NormalizeRelativePath(path, base.SessionState.Path.CurrentLocation.ProviderPath);
					if (text.StartsWith(".\\", StringComparison.OrdinalIgnoreCase) || text.StartsWith("./", StringComparison.OrdinalIgnoreCase))
					{
						text = text.Substring(2);
					}
					list.Add(text);
				}
			}
			catch (ItemNotFoundException)
			{
				list.Add(filePath);
			}
			return list;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00038DAC File Offset: 0x00036FAC
		private string ManifestFragment(string key, string resourceString, string value, StreamWriter streamWriter)
		{
			string newLine = streamWriter.NewLine;
			return string.Format(CultureInfo.InvariantCulture, "{0}# {1}{2}{0}{3:19} = {4}{5}{6}", new object[]
			{
				this._indent,
				resourceString,
				newLine,
				key,
				value,
				newLine,
				newLine
			});
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00038DFC File Offset: 0x00036FFC
		private string ManifestFragmentForNonSpecifiedManifestMember(string key, string resourceString, string value, StreamWriter streamWriter)
		{
			string newLine = streamWriter.NewLine;
			return string.Format(CultureInfo.InvariantCulture, "{0}# {1}{2}{0}{3:19} = {4}{5}{6}", new object[]
			{
				this._indent,
				resourceString,
				newLine,
				"# " + key,
				value,
				newLine,
				newLine
			});
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00038E54 File Offset: 0x00037054
		private string ManifestComment(string insert, StreamWriter streamWriter)
		{
			if (!string.IsNullOrEmpty(insert))
			{
				insert = " " + insert;
			}
			return string.Format(CultureInfo.InvariantCulture, "#{0}{1}", new object[]
			{
				insert,
				streamWriter.NewLine
			});
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00038E9C File Offset: 0x0003709C
		protected override void BeginProcessing()
		{
			if (this.ProcessorArchitecture == ProcessorArchitecture.IA64)
			{
				string message = StringUtil.Format(Modules.InvalidProcessorArchitectureInManifest, this.ProcessorArchitecture);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidProcessorArchitectureInManifest", ErrorCategory.InvalidArgument, this.ProcessorArchitecture);
				base.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00039170 File Offset: 0x00037370
		protected override void ProcessRecord()
		{
			ProviderInfo providerInfo = null;
			PSDriveInfo psdriveInfo;
			string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(this._path, out providerInfo, out psdriveInfo);
			if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || !unresolvedProviderPathFromPSPath.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
			{
				string message = StringUtil.Format(Modules.InvalidModuleManifestPath, this._path);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidModuleManifestPath", ErrorCategory.InvalidArgument, this._path);
				base.ThrowTerminatingError(errorRecord);
			}
			this.ValidateUriParamterValue(this.ProjectUri, "ProjectUri");
			this.ValidateUriParamterValue(this.LicenseUri, "LicenseUri");
			this.ValidateUriParamterValue(this.IconUri, "IconUri");
			string action = StringUtil.Format(Modules.CreatingModuleManifestFile, unresolvedProviderPathFromPSPath);
			if (base.ShouldProcess(unresolvedProviderPathFromPSPath, action))
			{
				if (string.IsNullOrEmpty(this._author))
				{
					this._author = Environment.UserName;
				}
				if (string.IsNullOrEmpty(this._companyName))
				{
					this._companyName = Modules.DefaultCompanyName;
				}
				if (string.IsNullOrEmpty(this._copyright))
				{
					this._copyright = StringUtil.Format(Modules.DefaultCopyrightMessage, DateTime.Now.Year, this._author);
				}
				FileStream fileStream;
				FileInfo fileInfo;
				StreamWriter streamWriter;
				PathUtils.MasterStreamOpen(this, unresolvedProviderPathFromPSPath, "unicode", false, false, false, false, out fileStream, out streamWriter, out fileInfo, false);
				try
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.ManifestComment("", streamWriter));
					stringBuilder.Append(this.ManifestComment(StringUtil.Format(Modules.ManifestHeaderLine1, System.IO.Path.GetFileNameWithoutExtension(unresolvedProviderPathFromPSPath)), streamWriter));
					stringBuilder.Append(this.ManifestComment("", streamWriter));
					stringBuilder.Append(this.ManifestComment(StringUtil.Format(Modules.ManifestHeaderLine2, this._author), streamWriter));
					stringBuilder.Append(this.ManifestComment("", streamWriter));
					stringBuilder.Append(this.ManifestComment(StringUtil.Format(Modules.ManifestHeaderLine3, DateTime.Now.ToString("d", CultureInfo.CurrentCulture)), streamWriter));
					stringBuilder.Append(this.ManifestComment("", streamWriter));
					stringBuilder.Append(streamWriter.NewLine);
					stringBuilder.Append("@{");
					stringBuilder.Append(streamWriter.NewLine);
					stringBuilder.Append(streamWriter.NewLine);
					if (this._rootModule == null)
					{
						this._rootModule = string.Empty;
					}
					this.BuildModuleManifest(stringBuilder, "RootModule", Modules.RootModule, !string.IsNullOrEmpty(this._rootModule), () => this.QuoteName(this._rootModule), streamWriter);
					this.BuildModuleManifest(stringBuilder, "ModuleVersion", Modules.ModuleVersion, this._moduleVersion != null && !string.IsNullOrEmpty(this._moduleVersion.ToString()), () => this.QuoteName(this._moduleVersion.ToString()), streamWriter);
					this.BuildModuleManifest(stringBuilder, "GUID", Modules.GUID, !string.IsNullOrEmpty(this._guid.ToString()), () => this.QuoteName(this._guid.ToString()), streamWriter);
					this.BuildModuleManifest(stringBuilder, "Author", Modules.Author, !string.IsNullOrEmpty(this._author), () => this.QuoteName(this.Author), streamWriter);
					this.BuildModuleManifest(stringBuilder, "CompanyName", Modules.CompanyName, !string.IsNullOrEmpty(this._companyName), () => this.QuoteName(this._companyName), streamWriter);
					this.BuildModuleManifest(stringBuilder, "Copyright", Modules.Copyright, !string.IsNullOrEmpty(this._copyright), () => this.QuoteName(this._copyright), streamWriter);
					this.BuildModuleManifest(stringBuilder, "Description", Modules.Description, !string.IsNullOrEmpty(this._description), () => this.QuoteName(this._description), streamWriter);
					this.BuildModuleManifest(stringBuilder, "PowerShellVersion", Modules.PowerShellVersion, this._powerShellVersion != null && !string.IsNullOrEmpty(this._powerShellVersion.ToString()), () => this.QuoteName(this._powerShellVersion), streamWriter);
					this.BuildModuleManifest(stringBuilder, "PowerShellHostName", Modules.PowerShellHostName, !string.IsNullOrEmpty(this._PowerShellHostName), () => this.QuoteName(this._PowerShellHostName), streamWriter);
					this.BuildModuleManifest(stringBuilder, "PowerShellHostVersion", Modules.PowerShellHostVersion, this._PowerShellHostVersion != null && !string.IsNullOrEmpty(this._PowerShellHostVersion.ToString()), () => this.QuoteName(this._PowerShellHostVersion), streamWriter);
					this.BuildModuleManifest(stringBuilder, "DotNetFrameworkVersion", Modules.DotNetFrameworkVersion, this._DotNetFrameworkVersion != null && !string.IsNullOrEmpty(this._DotNetFrameworkVersion.ToString()), () => this.QuoteName(this._DotNetFrameworkVersion), streamWriter);
					this.BuildModuleManifest(stringBuilder, "CLRVersion", Modules.CLRVersion, this._ClrVersion != null && !string.IsNullOrEmpty(this._ClrVersion.ToString()), () => this.QuoteName(this._ClrVersion), streamWriter);
					this.BuildModuleManifest(stringBuilder, "ProcessorArchitecture", Modules.ProcessorArchitecture, this._processorArchitecture != null, () => this.QuoteName(this._processorArchitecture), streamWriter);
					this.BuildModuleManifest(stringBuilder, "RequiredModules", Modules.RequiredModules, this._requiredModules != null && this._requiredModules.Length > 0, () => this.QuoteModules(this._requiredModules, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "RequiredAssemblies", Modules.RequiredAssemblies, this._requiredAssemblies != null, () => this.QuoteFiles(this._requiredAssemblies, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "ScriptsToProcess", Modules.ScriptsToProcess, this._scripts != null, () => this.QuoteFiles(this._scripts, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "TypesToProcess", Modules.TypesToProcess, this._types != null, () => this.QuoteFiles(this._types, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "FormatsToProcess", Modules.FormatsToProcess, this._formats != null, () => this.QuoteFiles(this._formats, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "NestedModules", Modules.NestedModules, this._nestedModules != null, () => this.QuoteModules(this.PreProcessModuleSpec(this._nestedModules), streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "FunctionsToExport", Modules.FunctionsToExport, this._exportedFunctions != null && this._exportedFunctions.Length > 0, () => this.QuoteNames(this._exportedFunctions, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "CmdletsToExport", Modules.CmdletsToExport, this._exportedCmdlets != null && this._exportedCmdlets.Length > 0, () => this.QuoteNames(this._exportedCmdlets, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "VariablesToExport", Modules.VariablesToExport, this._exportedVariables != null && this._exportedVariables.Length > 0, () => this.QuoteNames(this._exportedVariables, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "AliasesToExport", Modules.AliasesToExport, this._exportedAliases != null && this._exportedAliases.Length > 0, () => this.QuoteNames(this._exportedAliases, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "DscResourcesToExport", Modules.DscResourcesToExport, this._dscResourcesToExport != null && this._dscResourcesToExport.Length > 0, () => this.QuoteNames(this._dscResourcesToExport, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "ModuleList", Modules.ModuleList, this._moduleList != null, () => this.QuoteModules(this._moduleList, streamWriter), streamWriter);
					this.BuildModuleManifest(stringBuilder, "FileList", Modules.FileList, this._miscFiles != null, () => this.QuoteFiles(this._miscFiles, streamWriter), streamWriter);
					this.BuildPrivateDataInModuleManifest(stringBuilder, streamWriter);
					this.BuildModuleManifest(stringBuilder, "HelpInfoURI", Modules.HelpInfoURI, !string.IsNullOrEmpty(this._helpInfoUri), () => this.QuoteName(this._helpInfoUri), streamWriter);
					this.BuildModuleManifest(stringBuilder, "DefaultCommandPrefix", Modules.DefaultCommandPrefix, !string.IsNullOrEmpty(this._defaultCommandPrefix), () => this.QuoteName(this._defaultCommandPrefix), streamWriter);
					stringBuilder.Append("}");
					stringBuilder.Append(streamWriter.NewLine);
					stringBuilder.Append(streamWriter.NewLine);
					string text = stringBuilder.ToString();
					if (this._passThru)
					{
						base.WriteObject(text);
					}
					streamWriter.Write(text);
				}
				finally
				{
					streamWriter.Dispose();
				}
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00039C24 File Offset: 0x00037E24
		private void BuildModuleManifest(StringBuilder result, string key, string keyDescription, bool hasValue, Func<string> action, StreamWriter streamWriter)
		{
			if (hasValue)
			{
				result.Append(this.ManifestFragment(key, keyDescription, action(), streamWriter));
				return;
			}
			result.Append(this.ManifestFragmentForNonSpecifiedManifestMember(key, keyDescription, action(), streamWriter));
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00039CE0 File Offset: 0x00037EE0
		private void BuildPrivateDataInModuleManifest(StringBuilder result, StreamWriter streamWriter)
		{
			Hashtable hashtable = this.PrivateData as Hashtable;
			bool flag = this.Tags != null || this.ReleaseNotes != null || !(this.ProjectUri == null) || !(this.IconUri == null) || !(this.LicenseUri == null);
			if (this._privateData == null || hashtable != null)
			{
				result.Append(this.ManifestComment(Modules.PrivateData, streamWriter));
				result.Append("PrivateData = @{");
				result.Append(streamWriter.NewLine);
				result.Append(streamWriter.NewLine);
				result.Append("    PSData = @{");
				result.Append(streamWriter.NewLine);
				result.Append(streamWriter.NewLine);
				this._indent = "        ";
				this.BuildModuleManifest(result, "Tags", Modules.Tags, this.Tags != null && this.Tags.Length > 0, () => this.QuoteNames(this.Tags, streamWriter), streamWriter);
				this.BuildModuleManifest(result, "LicenseUri", Modules.LicenseUri, this.LicenseUri != null, () => this.QuoteName(this.LicenseUri), streamWriter);
				this.BuildModuleManifest(result, "ProjectUri", Modules.ProjectUri, this.ProjectUri != null, () => this.QuoteName(this.ProjectUri), streamWriter);
				this.BuildModuleManifest(result, "IconUri", Modules.IconUri, this.IconUri != null, () => this.QuoteName(this.IconUri), streamWriter);
				this.BuildModuleManifest(result, "ReleaseNotes", Modules.ReleaseNotes, !string.IsNullOrEmpty(this.ReleaseNotes), () => this.QuoteName(this.ReleaseNotes), streamWriter);
				result.Append("    } ");
				result.Append(this.ManifestComment(StringUtil.Format(Modules.EndOfManifestHashTable, "PSData"), streamWriter));
				result.Append(streamWriter.NewLine);
				this._indent = "    ";
				if (hashtable != null)
				{
					result.Append(streamWriter.NewLine);
					foreach (object obj in hashtable)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						result.Append(this.ManifestFragment(dictionaryEntry.Key.ToString(), dictionaryEntry.Key.ToString(), this.QuoteName((string)LanguagePrimitives.ConvertTo(dictionaryEntry.Value, typeof(string), CultureInfo.InvariantCulture)), streamWriter));
					}
				}
				result.Append("} ");
				result.Append(this.ManifestComment(StringUtil.Format(Modules.EndOfManifestHashTable, "PrivateData"), streamWriter));
				this._indent = "";
				result.Append(streamWriter.NewLine);
				return;
			}
			if (flag)
			{
				InvalidOperationException exception = new InvalidOperationException(Modules.PrivateDataValueTypeShouldBeHashTableError);
				ErrorRecord errorRecord = new ErrorRecord(exception, "PrivateDataValueTypeShouldBeHashTable", ErrorCategory.InvalidArgument, this._privateData);
				base.ThrowTerminatingError(errorRecord);
				return;
			}
			base.WriteWarning(Modules.PrivateDataValueTypeShouldBeHashTableWarning);
			this.BuildModuleManifest(result, "PrivateData", Modules.PrivateData, this._privateData != null, () => this.QuoteName((string)LanguagePrimitives.ConvertTo(this._privateData, typeof(string), CultureInfo.InvariantCulture)), streamWriter);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0003A0D4 File Offset: 0x000382D4
		private void ValidateUriParamterValue(Uri uri, string parameterName)
		{
			if (uri != null && !Uri.IsWellFormedUriString(uri.ToString(), UriKind.Absolute))
			{
				string message = StringUtil.Format(Modules.InvalidParameterValue, uri);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_InvalidUri", ErrorCategory.InvalidArgument, parameterName);
				base.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x0400041C RID: 1052
		private string _path;

		// Token: 0x0400041D RID: 1053
		private object[] _nestedModules;

		// Token: 0x0400041E RID: 1054
		private Guid _guid = Guid.NewGuid();

		// Token: 0x0400041F RID: 1055
		private string _author;

		// Token: 0x04000420 RID: 1056
		private string _companyName = "";

		// Token: 0x04000421 RID: 1057
		private string _copyright;

		// Token: 0x04000422 RID: 1058
		private string _rootModule;

		// Token: 0x04000423 RID: 1059
		private Version _moduleVersion = new Version(1, 0);

		// Token: 0x04000424 RID: 1060
		private string _description;

		// Token: 0x04000425 RID: 1061
		private ProcessorArchitecture? _processorArchitecture = null;

		// Token: 0x04000426 RID: 1062
		private Version _powerShellVersion;

		// Token: 0x04000427 RID: 1063
		private Version _ClrVersion;

		// Token: 0x04000428 RID: 1064
		private Version _DotNetFrameworkVersion;

		// Token: 0x04000429 RID: 1065
		private string _PowerShellHostName;

		// Token: 0x0400042A RID: 1066
		private Version _PowerShellHostVersion;

		// Token: 0x0400042B RID: 1067
		private object[] _requiredModules;

		// Token: 0x0400042C RID: 1068
		private string[] _types;

		// Token: 0x0400042D RID: 1069
		private string[] _formats;

		// Token: 0x0400042E RID: 1070
		private string[] _scripts;

		// Token: 0x0400042F RID: 1071
		private string[] _requiredAssemblies;

		// Token: 0x04000430 RID: 1072
		private string[] _miscFiles;

		// Token: 0x04000431 RID: 1073
		private object[] _moduleList;

		// Token: 0x04000432 RID: 1074
		private string[] _exportedFunctions = new string[]
		{
			"*"
		};

		// Token: 0x04000433 RID: 1075
		private string[] _exportedAliases = new string[]
		{
			"*"
		};

		// Token: 0x04000434 RID: 1076
		private string[] _exportedVariables = new string[]
		{
			"*"
		};

		// Token: 0x04000435 RID: 1077
		private string[] _exportedCmdlets = new string[]
		{
			"*"
		};

		// Token: 0x04000436 RID: 1078
		private string[] _dscResourcesToExport;

		// Token: 0x04000437 RID: 1079
		private object _privateData;

		// Token: 0x04000438 RID: 1080
		private string _helpInfoUri;

		// Token: 0x04000439 RID: 1081
		private bool _passThru;

		// Token: 0x0400043A RID: 1082
		private string _defaultCommandPrefix;

		// Token: 0x0400043B RID: 1083
		private string _indent = "";
	}
}
