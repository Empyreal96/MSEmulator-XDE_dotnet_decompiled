using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;
using System.Text;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation
{
	// Token: 0x02000062 RID: 98
	public class ExternalScriptInfo : CommandInfo, IScriptCommandInfo
	{
		// Token: 0x0600053E RID: 1342 RVA: 0x000193D2 File Offset: 0x000175D2
		internal ExternalScriptInfo(string name, string path, ExecutionContext context) : base(name, CommandTypes.ExternalScript, context)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			this._path = System.IO.Path.GetFullPath(path);
			this.CommonInitialization();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001940E File Offset: 0x0001760E
		internal ExternalScriptInfo(string name, string path) : base(name, CommandTypes.ExternalScript)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			this._path = System.IO.Path.GetFullPath(path);
			this.CommonInitialization();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00019449 File Offset: 0x00017649
		internal ExternalScriptInfo(ExternalScriptInfo other) : base(other)
		{
			this._path = other._path;
			this.CommonInitialization();
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00019470 File Offset: 0x00017670
		private void CommonInitialization()
		{
			if (SystemPolicy.GetSystemLockdownPolicy() != SystemEnforcementMode.None)
			{
				SystemEnforcementMode lockdownPolicy = SystemPolicy.GetLockdownPolicy(this._path, null);
				if (lockdownPolicy != SystemEnforcementMode.Enforce)
				{
					base.DefiningLanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
					return;
				}
				base.DefiningLanguageMode = new PSLanguageMode?(PSLanguageMode.ConstrainedLanguage);
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000194B0 File Offset: 0x000176B0
		internal override CommandInfo CreateGetCommandCopy(object[] argumentList)
		{
			return new ExternalScriptInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = argumentList
			};
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x000194D5 File Offset: 0x000176D5
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.ExternalScript;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000194DC File Offset: 0x000176DC
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x000194E4 File Offset: 0x000176E4
		public override string Definition
		{
			get
			{
				return this.Path;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000194EC File Offset: 0x000176EC
		public override string Source
		{
			get
			{
				return this.Definition;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000194F4 File Offset: 0x000176F4
		internal override string Syntax
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (CommandParameterSetInfo commandParameterSetInfo in base.ParameterSets)
				{
					stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0} {1}", new object[]
					{
						base.Name,
						commandParameterSetInfo
					}));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00019574 File Offset: 0x00017774
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00019596 File Offset: 0x00017796
		public override SessionStateEntryVisibility Visibility
		{
			get
			{
				if (base.Context == null)
				{
					return SessionStateEntryVisibility.Public;
				}
				return base.Context.EngineSessionState.CheckScriptVisibility(this._path);
			}
			set
			{
				throw PSTraceSource.NewNotImplementedException();
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000195A0 File Offset: 0x000177A0
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x000195F3 File Offset: 0x000177F3
		public ScriptBlock ScriptBlock
		{
			get
			{
				if (this._scriptBlock == null)
				{
					if (!this._path.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
					{
						this.ValidateScriptInfo(null);
					}
					ScriptBlock scriptBlock = ScriptBlock.Create(new Parser(), this._path, this.ScriptContents);
					this.ScriptBlock = scriptBlock;
				}
				return this._scriptBlock;
			}
			private set
			{
				this._scriptBlock = value;
				if (value != null)
				{
					this._scriptBlock.LanguageMode = base.DefiningLanguageMode;
				}
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00019610 File Offset: 0x00017810
		internal ScriptBlockAst GetScriptBlockAst()
		{
			string scriptContents = this.ScriptContents;
			if (this._scriptBlock == null)
			{
				this.ScriptBlock = ScriptBlock.TryGetCachedScriptBlock(this._path, scriptContents);
			}
			if (this._scriptBlock != null)
			{
				return (ScriptBlockAst)this._scriptBlock.Ast;
			}
			if (this._scriptBlockAst == null)
			{
				Parser parser = new Parser();
				ParseError[] array;
				this._scriptBlockAst = parser.Parse(this._path, this.ScriptContents, null, out array);
				if (array.Length == 0)
				{
					this.ScriptBlock = new ScriptBlock(this._scriptBlockAst, false);
					ScriptBlock.CacheScriptBlock(this._scriptBlock.Clone(false), this._path, scriptContents);
				}
			}
			return this._scriptBlockAst;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000196B8 File Offset: 0x000178B8
		public void ValidateScriptInfo(PSHost host)
		{
			if (!this._signatureChecked)
			{
				ExecutionContext executionContext = base.Context ?? LocalPipeline.GetExecutionContextFromTLS();
				this.ReadScriptContents();
				if (executionContext != null)
				{
					CommandDiscovery.ShouldRun(executionContext, host, this, CommandOrigin.Internal);
					this._signatureChecked = true;
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000196F6 File Offset: 0x000178F6
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				return this.ScriptBlock.OutputType;
			}
		}

		// Token: 0x17000159 RID: 345
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00019703 File Offset: 0x00017903
		internal bool SignatureChecked
		{
			set
			{
				this._signatureChecked = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001970C File Offset: 0x0001790C
		internal override CommandMetadata CommandMetadata
		{
			get
			{
				CommandMetadata result;
				if ((result = this._commandMetadata) == null)
				{
					result = (this._commandMetadata = new CommandMetadata(this.ScriptBlock, base.Name, LocalPipeline.GetExecutionContextFromTLS()));
				}
				return result;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00019744 File Offset: 0x00017944
		internal override bool ImplementsDynamicParameters
		{
			get
			{
				try
				{
					return this.ScriptBlock.HasDynamicParameters;
				}
				catch (ParseException)
				{
				}
				catch (ScriptRequiresException)
				{
				}
				this._scriptBlock = null;
				this._scriptContents = null;
				return false;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00019794 File Offset: 0x00017994
		private ScriptRequirements GetRequiresData()
		{
			return this.GetScriptBlockAst().ScriptRequirements;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000197A4 File Offset: 0x000179A4
		internal string RequiresApplicationID
		{
			get
			{
				ScriptRequirements requiresData = this.GetRequiresData();
				if (requiresData != null)
				{
					return requiresData.RequiredApplicationId;
				}
				return null;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000197C3 File Offset: 0x000179C3
		internal uint ApplicationIDLineNumber
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x000197C8 File Offset: 0x000179C8
		internal Version RequiresPSVersion
		{
			get
			{
				ScriptRequirements requiresData = this.GetRequiresData();
				if (requiresData != null)
				{
					return requiresData.RequiredPSVersion;
				}
				return null;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000197E8 File Offset: 0x000179E8
		internal IEnumerable<ModuleSpecification> RequiresModules
		{
			get
			{
				ScriptRequirements requiresData = this.GetRequiresData();
				if (requiresData != null)
				{
					return requiresData.RequiredModules;
				}
				return null;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00019808 File Offset: 0x00017A08
		internal bool RequiresElevation
		{
			get
			{
				ScriptRequirements requiresData = this.GetRequiresData();
				return requiresData != null && requiresData.IsElevationRequired;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00019827 File Offset: 0x00017A27
		internal uint PSVersionLineNumber
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0001982C File Offset: 0x00017A2C
		internal IEnumerable<PSSnapInSpecification> RequiresPSSnapIns
		{
			get
			{
				ScriptRequirements requiresData = this.GetRequiresData();
				if (requiresData != null)
				{
					return requiresData.RequiresPSSnapIns;
				}
				return null;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001984B File Offset: 0x00017A4B
		public string ScriptContents
		{
			get
			{
				if (this._scriptContents == null)
				{
					this.ReadScriptContents();
				}
				return this._scriptContents;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00019861 File Offset: 0x00017A61
		public Encoding OriginalEncoding
		{
			get
			{
				if (this._scriptContents == null)
				{
					this.ReadScriptContents();
				}
				return this._originalEncoding;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00019878 File Offset: 0x00017A78
		private void ReadScriptContents()
		{
			if (this._scriptContents == null)
			{
				try
				{
					using (FileStream fileStream = new FileStream(this._path, FileMode.Open, FileAccess.Read))
					{
						Encoding defaultEncoding = ClrFacade.GetDefaultEncoding();
						SafeFileHandle safeFileHandle = fileStream.SafeFileHandle;
						using (StreamReader streamReader = new StreamReader(fileStream, defaultEncoding))
						{
							this._scriptContents = streamReader.ReadToEnd();
							this._originalEncoding = streamReader.CurrentEncoding;
							if (SystemPolicy.GetSystemLockdownPolicy() != SystemEnforcementMode.None)
							{
								SystemEnforcementMode lockdownPolicy = SystemPolicy.GetLockdownPolicy(this._path, safeFileHandle);
								if (lockdownPolicy != SystemEnforcementMode.Enforce)
								{
									base.DefiningLanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
								}
								else
								{
									base.DefiningLanguageMode = new PSLanguageMode?(PSLanguageMode.ConstrainedLanguage);
								}
							}
							else if (base.Context != null)
							{
								base.DefiningLanguageMode = new PSLanguageMode?(base.Context.LanguageMode);
							}
						}
					}
				}
				catch (ArgumentException innerException)
				{
					ExternalScriptInfo.ThrowCommandNotFoundException(innerException);
				}
				catch (IOException innerException2)
				{
					ExternalScriptInfo.ThrowCommandNotFoundException(innerException2);
				}
				catch (NotSupportedException innerException3)
				{
					ExternalScriptInfo.ThrowCommandNotFoundException(innerException3);
				}
				catch (UnauthorizedAccessException innerException4)
				{
					ExternalScriptInfo.ThrowCommandNotFoundException(innerException4);
				}
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000199B4 File Offset: 0x00017BB4
		private static void ThrowCommandNotFoundException(Exception innerException)
		{
			CommandNotFoundException ex = new CommandNotFoundException(innerException.Message, innerException);
			throw ex;
		}

		// Token: 0x04000223 RID: 547
		private readonly string _path = string.Empty;

		// Token: 0x04000224 RID: 548
		private ScriptBlock _scriptBlock;

		// Token: 0x04000225 RID: 549
		private ScriptBlockAst _scriptBlockAst;

		// Token: 0x04000226 RID: 550
		private bool _signatureChecked;

		// Token: 0x04000227 RID: 551
		private CommandMetadata _commandMetadata;

		// Token: 0x04000228 RID: 552
		private string _scriptContents;

		// Token: 0x04000229 RID: 553
		private Encoding _originalEncoding;
	}
}
