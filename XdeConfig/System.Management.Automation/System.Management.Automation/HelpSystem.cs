using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020001AF RID: 431
	internal class HelpSystem
	{
		// Token: 0x06001420 RID: 5152 RVA: 0x0007B018 File Offset: 0x00079218
		internal HelpSystem(ExecutionContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("ExecutionContext");
			}
			this._executionContext = context;
			this.Initialize();
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0007B068 File Offset: 0x00079268
		internal ExecutionContext ExecutionContext
		{
			get
			{
				return this._executionContext;
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001422 RID: 5154 RVA: 0x0007B070 File Offset: 0x00079270
		// (remove) Token: 0x06001423 RID: 5155 RVA: 0x0007B0A8 File Offset: 0x000792A8
		internal event HelpSystem.HelpProgressHandler OnProgress;

		// Token: 0x06001424 RID: 5156 RVA: 0x0007B0DD File Offset: 0x000792DD
		internal void Initialize()
		{
			this._verboseHelpErrors = LanguagePrimitives.IsTrue(this._executionContext.GetVariableValue(SpecialVariables.VerboseHelpErrorsVarPath, false));
			this._helpErrorTracer = new HelpErrorTracer(this);
			this.InitializeHelpProviders();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0007B112 File Offset: 0x00079312
		internal IEnumerable<HelpInfo> GetHelp(HelpRequest helpRequest)
		{
			if (helpRequest == null)
			{
				return null;
			}
			helpRequest.Validate();
			this.ValidateHelpCulture();
			return this.DoGetHelp(helpRequest);
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0007B12C File Offset: 0x0007932C
		internal Collection<ErrorRecord> LastErrors
		{
			get
			{
				return this._lastErrors;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0007B134 File Offset: 0x00079334
		internal HelpCategory LastHelpCategory
		{
			get
			{
				return this._lastHelpCategory;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0007B13C File Offset: 0x0007933C
		internal bool VerboseHelpErrors
		{
			get
			{
				return this._verboseHelpErrors;
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0007B144 File Offset: 0x00079344
		internal Collection<string> GetSearchPaths()
		{
			if (this._searchPaths != null)
			{
				return this._searchPaths;
			}
			this._searchPaths = new Collection<string>();
			RunspaceConfigForSingleShell runspaceConfigForSingleShell = this.ExecutionContext.RunspaceConfiguration as RunspaceConfigForSingleShell;
			if (runspaceConfigForSingleShell != null)
			{
				MshConsoleInfo consoleInfo = runspaceConfigForSingleShell.ConsoleInfo;
				if (consoleInfo == null || consoleInfo.ExternalPSSnapIns == null)
				{
					return this._searchPaths;
				}
				foreach (PSSnapInInfo pssnapInInfo in consoleInfo.ExternalPSSnapIns)
				{
					this._searchPaths.Add(pssnapInInfo.ApplicationBase);
				}
			}
			if (this.ExecutionContext.Modules != null)
			{
				foreach (PSModuleInfo psmoduleInfo in this.ExecutionContext.Modules.ModuleTable.Values)
				{
					if (!this._searchPaths.Contains(psmoduleInfo.ModuleBase))
					{
						this._searchPaths.Add(psmoduleInfo.ModuleBase);
					}
				}
			}
			return this._searchPaths;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0007B608 File Offset: 0x00079808
		private IEnumerable<HelpInfo> DoGetHelp(HelpRequest helpRequest)
		{
			this._lastErrors.Clear();
			this._searchPaths = null;
			this._lastHelpCategory = helpRequest.HelpCategory;
			if (string.IsNullOrEmpty(helpRequest.Target))
			{
				HelpInfo helpInfo = this.GetDefaultHelp();
				if (helpInfo != null)
				{
					yield return helpInfo;
				}
				yield return null;
			}
			else
			{
				bool isMatchFound = false;
				if (!WildcardPattern.ContainsWildcardCharacters(helpRequest.Target))
				{
					foreach (HelpInfo helpInfo2 in this.ExactMatchHelp(helpRequest))
					{
						isMatchFound = true;
						yield return helpInfo2;
					}
				}
				if (!isMatchFound)
				{
					foreach (HelpInfo helpInfo3 in this.SearchHelp(helpRequest))
					{
						isMatchFound = true;
						yield return helpInfo3;
					}
					if (!isMatchFound && !WildcardPattern.ContainsWildcardCharacters(helpRequest.Target) && this.LastErrors.Count == 0)
					{
						Exception exception = new HelpNotFoundException(helpRequest.Target);
						ErrorRecord item = new ErrorRecord(exception, "HelpNotFound", ErrorCategory.ResourceUnavailable, null);
						this.LastErrors.Add(item);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0007B908 File Offset: 0x00079B08
		internal IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			bool isHelpInfoFound = false;
			for (int i = 0; i < this.HelpProviders.Count; i++)
			{
				HelpProvider helpProvider = (HelpProvider)this.HelpProviders[i];
				if ((helpProvider.HelpCategory & helpRequest.HelpCategory) > HelpCategory.None)
				{
					foreach (HelpInfo helpInfo in helpProvider.ExactMatchHelp(helpRequest))
					{
						isHelpInfoFound = true;
						foreach (HelpInfo fwdHelpInfo in this.ForwardHelp(helpInfo, helpRequest))
						{
							yield return fwdHelpInfo;
						}
					}
				}
				if (isHelpInfoFound && !(helpProvider is ScriptCommandHelpProvider))
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0007BC9C File Offset: 0x00079E9C
		private IEnumerable<HelpInfo> ForwardHelp(HelpInfo helpInfo, HelpRequest helpRequest)
		{
			new Collection<HelpInfo>();
			if (helpInfo.ForwardHelpCategory == HelpCategory.None && string.IsNullOrEmpty(helpInfo.ForwardTarget))
			{
				yield return helpInfo;
			}
			else
			{
				HelpCategory forwardHelpCategory = helpInfo.ForwardHelpCategory;
				bool isHelpInfoProcessed = false;
				for (int i = 0; i < this.HelpProviders.Count; i++)
				{
					HelpProvider helpProvider = (HelpProvider)this.HelpProviders[i];
					if ((helpProvider.HelpCategory & forwardHelpCategory) != HelpCategory.None)
					{
						isHelpInfoProcessed = true;
						using (IEnumerator<HelpInfo> enumerator = helpProvider.ProcessForwardedHelp(helpInfo, helpRequest).GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								HelpInfo fwdResult = enumerator.Current;
								foreach (HelpInfo fHelpInfo in this.ForwardHelp(fwdResult, helpRequest))
								{
									yield return fHelpInfo;
								}
								yield break;
							}
						}
					}
				}
				if (!isHelpInfoProcessed)
				{
					yield return helpInfo;
				}
			}
			yield break;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0007BCC8 File Offset: 0x00079EC8
		private HelpInfo GetDefaultHelp()
		{
			HelpRequest helpRequest = new HelpRequest("default", HelpCategory.DefaultHelp);
			using (IEnumerator<HelpInfo> enumerator = this.ExactMatchHelp(helpRequest).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0007C118 File Offset: 0x0007A318
		private IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest)
		{
			int countOfHelpInfosFound = 0;
			bool searchInHelpContent = false;
			bool shouldBreak = false;
			HelpProgressInfo progress = new HelpProgressInfo();
			progress.Activity = StringUtil.Format(HelpDisplayStrings.SearchingForHelpContent, helpRequest.Target);
			progress.Completed = false;
			progress.PercentComplete = 0;
			try
			{
				this.OnProgress(this, progress);
				for (;;)
				{
					if (searchInHelpContent)
					{
						shouldBreak = true;
					}
					for (int i = 0; i < this.HelpProviders.Count; i++)
					{
						HelpProvider helpProvider = (HelpProvider)this.HelpProviders[i];
						if ((helpProvider.HelpCategory & helpRequest.HelpCategory) > HelpCategory.None)
						{
							foreach (HelpInfo helpInfo in helpProvider.SearchHelp(helpRequest, searchInHelpContent))
							{
								if (this._executionContext.CurrentPipelineStopping)
								{
									yield break;
								}
								countOfHelpInfosFound++;
								yield return helpInfo;
								if (countOfHelpInfosFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
								{
									yield break;
								}
							}
						}
					}
					if (countOfHelpInfosFound > 0)
					{
						break;
					}
					searchInHelpContent = true;
					if (this.HelpProviders.Count > 0)
					{
						progress.PercentComplete += 100 / this.HelpProviders.Count;
						this.OnProgress(this, progress);
					}
					if (shouldBreak)
					{
						goto Block_9;
					}
				}
				yield break;
				Block_9:;
			}
			finally
			{
				progress.Completed = true;
				progress.PercentComplete = 100;
				this.OnProgress(this, progress);
			}
			yield break;
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0007C13C File Offset: 0x0007A33C
		internal ArrayList HelpProviders
		{
			get
			{
				return this._helpProviders;
			}
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0007C144 File Offset: 0x0007A344
		private void InitializeHelpProviders()
		{
			HelpProvider value = new AliasHelpProvider(this);
			this._helpProviders.Add(value);
			value = new ScriptCommandHelpProvider(this);
			this._helpProviders.Add(value);
			value = new CommandHelpProvider(this);
			this._helpProviders.Add(value);
			value = new ProviderHelpProvider(this);
			this._helpProviders.Add(value);
			value = new PSClassHelpProvider(this);
			this._helpProviders.Add(value);
			value = new HelpFileHelpProvider(this);
			this._helpProviders.Add(value);
			value = new FaqHelpProvider(this);
			this._helpProviders.Add(value);
			value = new GlossaryHelpProvider(this);
			this._helpProviders.Add(value);
			value = new GeneralHelpProvider(this);
			this._helpProviders.Add(value);
			value = new DefaultHelpProvider(this);
			this._helpProviders.Add(value);
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0007C21B File Offset: 0x0007A41B
		internal HelpErrorTracer HelpErrorTracer
		{
			get
			{
				return this._helpErrorTracer;
			}
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0007C223 File Offset: 0x0007A423
		internal IDisposable Trace(string helpFile)
		{
			if (this._helpErrorTracer == null)
			{
				return null;
			}
			return this._helpErrorTracer.Trace(helpFile);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0007C23B File Offset: 0x0007A43B
		internal void TraceError(ErrorRecord errorRecord)
		{
			if (this._helpErrorTracer == null)
			{
				return;
			}
			this._helpErrorTracer.TraceError(errorRecord);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0007C252 File Offset: 0x0007A452
		internal void TraceErrors(Collection<ErrorRecord> errorRecords)
		{
			if (this._helpErrorTracer == null || errorRecords == null)
			{
				return;
			}
			this._helpErrorTracer.TraceErrors(errorRecords);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0007C26C File Offset: 0x0007A46C
		private void ValidateHelpCulture()
		{
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			if (this._culture == null)
			{
				this._culture = currentUICulture;
				return;
			}
			if (this._culture.Equals(currentUICulture))
			{
				return;
			}
			this._culture = currentUICulture;
			this.ResetHelpProviders();
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0007C2AC File Offset: 0x0007A4AC
		internal void ResetHelpProviders()
		{
			if (this._helpProviders == null)
			{
				return;
			}
			for (int i = 0; i < this._helpProviders.Count; i++)
			{
				HelpProvider helpProvider = (HelpProvider)this._helpProviders[i];
				helpProvider.Reset();
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0007C2F0 File Offset: 0x0007A4F0
		internal Dictionary<Ast, Token[]> ScriptBlockTokenCache
		{
			get
			{
				return this.scriptBlockTokenCache.Value;
			}
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0007C2FD File Offset: 0x0007A4FD
		internal void ClearScriptBlockTokenCache()
		{
			if (this.scriptBlockTokenCache.IsValueCreated)
			{
				this.scriptBlockTokenCache.Value.Clear();
			}
		}

		// Token: 0x040008AB RID: 2219
		private ExecutionContext _executionContext;

		// Token: 0x040008AD RID: 2221
		private Collection<ErrorRecord> _lastErrors = new Collection<ErrorRecord>();

		// Token: 0x040008AE RID: 2222
		private HelpCategory _lastHelpCategory;

		// Token: 0x040008AF RID: 2223
		private bool _verboseHelpErrors;

		// Token: 0x040008B0 RID: 2224
		private Collection<string> _searchPaths;

		// Token: 0x040008B1 RID: 2225
		private ArrayList _helpProviders = new ArrayList();

		// Token: 0x040008B2 RID: 2226
		private HelpErrorTracer _helpErrorTracer;

		// Token: 0x040008B3 RID: 2227
		private CultureInfo _culture;

		// Token: 0x040008B4 RID: 2228
		private readonly Lazy<Dictionary<Ast, Token[]>> scriptBlockTokenCache = new Lazy<Dictionary<Ast, Token[]>>(true);

		// Token: 0x020001B0 RID: 432
		// (Invoke) Token: 0x0600143A RID: 5178
		internal delegate void HelpProgressHandler(object sender, HelpProgressInfo arg);
	}
}
