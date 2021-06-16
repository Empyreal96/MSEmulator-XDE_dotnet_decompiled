using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Help;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Net;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001D6 RID: 470
	public class UpdatableHelpCommandBase : PSCmdlet
	{
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x00088CB0 File Offset: 0x00086EB0
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x00088CFC File Offset: 0x00086EFC
		[ValidateNotNull]
		[Parameter(Position = 2)]
		public CultureInfo[] UICulture
		{
			get
			{
				CultureInfo[] array = null;
				if (this._language != null)
				{
					array = new CultureInfo[this._language.Length];
					for (int i = 0; i < this._language.Length; i++)
					{
						array[i] = new CultureInfo(this._language[i]);
					}
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._language = new string[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					this._language[i] = value[i].Name;
				}
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x00088D39 File Offset: 0x00086F39
		// (set) Token: 0x060015A3 RID: 5539 RVA: 0x00088D41 File Offset: 0x00086F41
		[Credential]
		[Parameter]
		public PSCredential Credential
		{
			get
			{
				return this._credential;
			}
			set
			{
				this._credential = value;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00088D4A File Offset: 0x00086F4A
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x00088D57 File Offset: 0x00086F57
		[Parameter]
		public SwitchParameter UseDefaultCredentials
		{
			get
			{
				return this._useDefaultCredentials;
			}
			set
			{
				this._useDefaultCredentials = value;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00088D65 File Offset: 0x00086F65
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x00088D72 File Offset: 0x00086F72
		[Parameter]
		public SwitchParameter Force
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

		// Token: 0x060015A8 RID: 5544 RVA: 0x00088D80 File Offset: 0x00086F80
		private void HandleProgressChanged(object sender, UpdatableHelpProgressEventArgs e)
		{
			string formatSpec = (e.CommandType == UpdatableHelpCommandType.UpdateHelpCommand) ? HelpDisplayStrings.UpdateProgressActivityForModule : HelpDisplayStrings.SaveProgressActivityForModule;
			base.WriteProgress(new ProgressRecord(this.activityId, StringUtil.Format(formatSpec, e.ModuleName), e.ProgressStatus)
			{
				PercentComplete = e.ProgressPercent
			});
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00088DD4 File Offset: 0x00086FD4
		static UpdatableHelpCommandBase()
		{
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Diagnostics", "http://go.microsoft.com/fwlink/?linkid=390783");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Core", "http://go.microsoft.com/fwlink/?linkid=390782");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Utility", "http://go.microsoft.com/fwlink/?linkid=390787");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Host", "http://go.microsoft.com/fwlink/?linkid=390784");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Management", " http://go.microsoft.com/fwlink/?linkid=390785");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.PowerShell.Security", " http://go.microsoft.com/fwlink/?linkid=390786");
			UpdatableHelpCommandBase.metadataCache.Add("Microsoft.WSMan.Management", "http://go.microsoft.com/fwlink/?linkid=390788");
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00088E7C File Offset: 0x0008707C
		internal static bool IsSystemModule(string module)
		{
			return UpdatableHelpCommandBase.metadataCache.ContainsKey(module);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00088E8C File Offset: 0x0008708C
		internal UpdatableHelpCommandBase(UpdatableHelpCommandType commandType)
		{
			this._commandType = commandType;
			this._helpSystem = new UpdatableHelpSystem(this, this._useDefaultCredentials);
			this.exceptions = new Dictionary<string, UpdatableHelpExceptionContext>();
			this._helpSystem.OnProgressChanged += this.HandleProgressChanged;
			Random random = new Random();
			this.activityId = random.Next();
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00088EEC File Offset: 0x000870EC
		private void ProcessSingleModuleObject(PSModuleInfo module, ExecutionContext context, Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> helpModules, bool noErrors)
		{
			if (InitialSessionState.IsEngineModule(module.Name) && !InitialSessionState.IsNestedEngineModule(module.Name))
			{
				base.WriteDebug(StringUtil.Format("Found engine module: {0}, {1}.", module.Name, module.Guid));
				Tuple<string, Version> key = new Tuple<string, Version>(module.Name, module.Version);
				if (!helpModules.ContainsKey(key))
				{
					helpModules.Add(key, new UpdatableHelpModuleInfo(module.Name, module.Guid, Utils.GetApplicationBase(context.ShellID), UpdatableHelpCommandBase.metadataCache[module.Name]));
				}
				return;
			}
			if (InitialSessionState.IsNestedEngineModule(module.Name))
			{
				return;
			}
			if (string.IsNullOrEmpty(module.HelpInfoUri))
			{
				if (!noErrors)
				{
					this.ProcessException(module.Name, null, new UpdatableHelpSystemException("HelpInfoUriNotFound", StringUtil.Format(HelpDisplayStrings.HelpInfoUriNotFound, new object[0]), ErrorCategory.NotSpecified, new Uri("HelpInfoUri", UriKind.Relative), null));
				}
				return;
			}
			if (!module.HelpInfoUri.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
			{
				if (!noErrors)
				{
					this.ProcessException(module.Name, null, new UpdatableHelpSystemException("InvalidHelpInfoUriFormat", StringUtil.Format(HelpDisplayStrings.InvalidHelpInfoUriFormat, module.HelpInfoUri), ErrorCategory.NotSpecified, new Uri("HelpInfoUri", UriKind.Relative), null));
				}
				return;
			}
			Tuple<string, Version> key2 = new Tuple<string, Version>(module.Name, module.Version);
			if (!helpModules.ContainsKey(key2))
			{
				helpModules.Add(key2, new UpdatableHelpModuleInfo(module.Name, module.Guid, module.ModuleBase, module.HelpInfoUri));
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00089064 File Offset: 0x00087264
		private Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> GetModuleInfo(ExecutionContext context, string pattern, ModuleSpecification fullyQualifiedName, bool noErrors)
		{
			List<PSModuleInfo> list = null;
			string text = null;
			if (pattern != null)
			{
				text = pattern;
				list = Utils.GetModules(pattern, context);
			}
			else if (fullyQualifiedName != null)
			{
				text = fullyQualifiedName.Name;
				list = Utils.GetModules(fullyQualifiedName, context);
			}
			Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> dictionary = new Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo>();
			if (list != null)
			{
				foreach (PSModuleInfo module in list)
				{
					this.ProcessSingleModuleObject(module, context, dictionary, noErrors);
				}
			}
			WildcardOptions options = WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant;
			IEnumerable<WildcardPattern> patterns = SessionStateUtilities.CreateWildcardsFromStrings(new string[]
			{
				text
			}, options);
			foreach (KeyValuePair<string, string> keyValuePair in UpdatableHelpCommandBase.metadataCache)
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(keyValuePair.Key, patterns, true))
				{
					if (!keyValuePair.Key.Equals(InitialSessionState.CoreSnapin, StringComparison.OrdinalIgnoreCase))
					{
						Tuple<string, Version> key = new Tuple<string, Version>(keyValuePair.Key, new Version("1.0"));
						if (dictionary.ContainsKey(key))
						{
							continue;
						}
						List<PSModuleInfo> modules = Utils.GetModules(keyValuePair.Key, context);
						if (modules == null)
						{
							continue;
						}
						using (List<PSModuleInfo>.Enumerator enumerator3 = modules.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								PSModuleInfo psmoduleInfo = enumerator3.Current;
								key = new Tuple<string, Version>(psmoduleInfo.Name, psmoduleInfo.Version);
								if (!dictionary.ContainsKey(key))
								{
									base.WriteDebug(StringUtil.Format("Found engine module: {0}, {1}.", psmoduleInfo.Name, psmoduleInfo.Guid));
									dictionary.Add(key, new UpdatableHelpModuleInfo(psmoduleInfo.Name, psmoduleInfo.Guid, Utils.GetApplicationBase(context.ShellID), UpdatableHelpCommandBase.metadataCache[psmoduleInfo.Name]));
								}
							}
							continue;
						}
					}
					Tuple<string, Version> key2 = new Tuple<string, Version>(keyValuePair.Key, new Version("1.0"));
					if (!dictionary.ContainsKey(key2))
					{
						dictionary.Add(key2, new UpdatableHelpModuleInfo(keyValuePair.Key, Guid.Empty, Utils.GetApplicationBase(context.ShellID), keyValuePair.Value));
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000892D4 File Offset: 0x000874D4
		protected override void StopProcessing()
		{
			this._stopping = true;
			this._helpSystem.CancelDownload();
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x000892E8 File Offset: 0x000874E8
		protected override void EndProcessing()
		{
			foreach (UpdatableHelpExceptionContext updatableHelpExceptionContext in this.exceptions.Values)
			{
				UpdatableHelpExceptionContext updatableHelpExceptionContext2 = updatableHelpExceptionContext;
				if (updatableHelpExceptionContext.Exception.FullyQualifiedErrorId == "HelpCultureNotSupported" && ((updatableHelpExceptionContext.Cultures != null && updatableHelpExceptionContext.Cultures.Count > 1) || (updatableHelpExceptionContext.Modules != null && updatableHelpExceptionContext.Modules.Count > 1)))
				{
					updatableHelpExceptionContext2 = new UpdatableHelpExceptionContext(new UpdatableHelpSystemException("HelpCultureNotSupported", StringUtil.Format(HelpDisplayStrings.CannotMatchUICulturePattern, string.Join(", ", updatableHelpExceptionContext.Cultures)), ErrorCategory.InvalidArgument, updatableHelpExceptionContext.Cultures, null));
					updatableHelpExceptionContext2.Modules = updatableHelpExceptionContext.Modules;
					updatableHelpExceptionContext2.Cultures = updatableHelpExceptionContext.Cultures;
				}
				base.WriteError(updatableHelpExceptionContext2.CreateErrorRecord(this._commandType));
				LogContext logContext = MshLog.GetLogContext(base.Context, base.MyInvocation);
				logContext.Severity = "Error";
				PSEtwLog.LogOperationalError(PSEventId.Pipeline_Detail, PSOpcode.Exception, PSTask.ExecutePipeline, logContext, updatableHelpExceptionContext2.GetExceptionMessage(this._commandType));
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00089418 File Offset: 0x00087618
		internal void Process(IEnumerable<string> moduleNames, IEnumerable<ModuleSpecification> fullyQualifiedNames)
		{
			this._helpSystem.WebClient.UseDefaultCredentials = this._useDefaultCredentials;
			if (moduleNames != null)
			{
				using (IEnumerator<string> enumerator = moduleNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						if (this._stopping)
						{
							break;
						}
						this.ProcessModuleWithGlobbing(name);
					}
					return;
				}
			}
			if (fullyQualifiedNames != null)
			{
				using (IEnumerator<ModuleSpecification> enumerator2 = fullyQualifiedNames.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ModuleSpecification fullyQualifiedName = enumerator2.Current;
						if (this._stopping)
						{
							break;
						}
						this.ProcessModuleWithGlobbing(fullyQualifiedName);
					}
					return;
				}
			}
			foreach (KeyValuePair<Tuple<string, Version>, UpdatableHelpModuleInfo> keyValuePair in this.GetModuleInfo("*", null, true))
			{
				if (this._stopping)
				{
					break;
				}
				this.ProcessModule(keyValuePair.Value);
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00089524 File Offset: 0x00087724
		internal void Process(IEnumerable<PSModuleInfo> modules)
		{
			if (modules == null || !modules.Any<PSModuleInfo>())
			{
				return;
			}
			Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> dictionary = new Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo>();
			foreach (PSModuleInfo module in modules)
			{
				this.ProcessSingleModuleObject(module, base.Context, dictionary, false);
			}
			foreach (KeyValuePair<Tuple<string, Version>, UpdatableHelpModuleInfo> keyValuePair in dictionary)
			{
				this.ProcessModule(keyValuePair.Value);
			}
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x000895CC File Offset: 0x000877CC
		private void ProcessModuleWithGlobbing(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				PSArgumentException ex = new PSArgumentException(StringUtil.Format(HelpDisplayStrings.ModuleNameNullOrEmpty, new object[0]));
				base.WriteError(ex.ErrorRecord);
				return;
			}
			foreach (KeyValuePair<Tuple<string, Version>, UpdatableHelpModuleInfo> keyValuePair in this.GetModuleInfo(name, null, false))
			{
				this.ProcessModule(keyValuePair.Value);
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00089654 File Offset: 0x00087854
		private void ProcessModuleWithGlobbing(ModuleSpecification fullyQualifiedName)
		{
			foreach (KeyValuePair<Tuple<string, Version>, UpdatableHelpModuleInfo> keyValuePair in this.GetModuleInfo(null, fullyQualifiedName, false))
			{
				this.ProcessModule(keyValuePair.Value);
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000896B0 File Offset: 0x000878B0
		private void ProcessModule(UpdatableHelpModuleInfo module)
		{
			this._helpSystem.CurrentModule = module.ModuleName;
			if (this is UpdateHelpCommand && !Directory.Exists(module.ModuleBase))
			{
				this.ProcessException(module.ModuleName, null, new UpdatableHelpSystemException("ModuleBaseMustExist", StringUtil.Format(HelpDisplayStrings.ModuleBaseMustExist, new object[0]), ErrorCategory.InvalidOperation, null, null));
				return;
			}
			IEnumerable<string> enumerable;
			if (this._language == null)
			{
				enumerable = this._helpSystem.GetCurrentUICulture();
			}
			else
			{
				enumerable = this._language;
			}
			foreach (string culture in enumerable)
			{
				bool flag = true;
				if (this._stopping)
				{
					break;
				}
				try
				{
					this.ProcessModuleWithCulture(module, culture);
				}
				catch (IOException ex)
				{
					this.ProcessException(module.ModuleName, culture, new UpdatableHelpSystemException("FailedToCopyFile", ex.Message, ErrorCategory.InvalidOperation, null, ex));
				}
				catch (UnauthorizedAccessException ex2)
				{
					this.ProcessException(module.ModuleName, culture, new UpdatableHelpSystemException("AccessIsDenied", ex2.Message, ErrorCategory.PermissionDenied, null, ex2));
				}
				catch (WebException ex3)
				{
					if (ex3.InnerException != null && ex3.InnerException is UnauthorizedAccessException)
					{
						this.ProcessException(module.ModuleName, culture, new UpdatableHelpSystemException("AccessIsDenied", ex3.InnerException.Message, ErrorCategory.PermissionDenied, null, ex3));
					}
					else
					{
						this.ProcessException(module.ModuleName, culture, ex3);
					}
				}
				catch (UpdatableHelpSystemException ex4)
				{
					if (ex4.FullyQualifiedErrorId == "HelpCultureNotSupported")
					{
						flag = false;
						if (this._language != null)
						{
							this.ProcessException(module.ModuleName, culture, ex4);
						}
					}
					else
					{
						this.ProcessException(module.ModuleName, culture, ex4);
					}
				}
				catch (Exception e)
				{
					this.ProcessException(module.ModuleName, culture, e);
				}
				finally
				{
					if (this._helpSystem.Errors.Count != 0)
					{
						foreach (Exception e2 in this._helpSystem.Errors)
						{
							this.ProcessException(module.ModuleName, culture, e2);
						}
						this._helpSystem.Errors.Clear();
					}
				}
				if (this._language == null && flag)
				{
					break;
				}
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000899A4 File Offset: 0x00087BA4
		internal virtual bool ProcessModuleWithCulture(UpdatableHelpModuleInfo module, string culture)
		{
			return false;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000899A8 File Offset: 0x00087BA8
		internal Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> GetModuleInfo(string pattern, ModuleSpecification fullyQualifiedName, bool noErrors)
		{
			Dictionary<Tuple<string, Version>, UpdatableHelpModuleInfo> moduleInfo = this.GetModuleInfo(base.Context, pattern, fullyQualifiedName, noErrors);
			if (moduleInfo.Count == 0 && this.exceptions.Count == 0 && !noErrors)
			{
				string message = (fullyQualifiedName != null) ? StringUtil.Format(HelpDisplayStrings.ModuleNotFoundWithFullyQualifiedName, fullyQualifiedName) : StringUtil.Format(HelpDisplayStrings.CannotMatchModulePattern, pattern);
				ErrorRecord errorRecord = new ErrorRecord(new Exception(message), "ModuleNotFound", ErrorCategory.InvalidArgument, pattern);
				base.WriteError(errorRecord);
			}
			return moduleInfo;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00089A14 File Offset: 0x00087C14
		internal bool IsUpdateNecessary(UpdatableHelpModuleInfo module, UpdatableHelpInfo currentHelpInfo, UpdatableHelpInfo newHelpInfo, CultureInfo culture, bool force)
		{
			if (newHelpInfo == null)
			{
				throw new UpdatableHelpSystemException("UnableToRetrieveHelpInfoXml", StringUtil.Format(HelpDisplayStrings.UnableToRetrieveHelpInfoXml, culture.Name), ErrorCategory.ResourceUnavailable, null, null);
			}
			if (!newHelpInfo.IsCultureSupported(culture))
			{
				throw new UpdatableHelpSystemException("HelpCultureNotSupported", StringUtil.Format(HelpDisplayStrings.HelpCultureNotSupported, culture.Name, newHelpInfo.GetSupportedCultures()), ErrorCategory.InvalidOperation, null, null);
			}
			return force || currentHelpInfo == null || currentHelpInfo.IsNewerVersion(newHelpInfo, culture);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x00089A88 File Offset: 0x00087C88
		internal bool CheckOncePerDayPerModule(string moduleName, string path, string filename, DateTime time, bool force)
		{
			if (force)
			{
				return true;
			}
			string path2 = base.SessionState.Path.Combine(path, filename);
			if (!File.Exists(path2))
			{
				return true;
			}
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path2);
			if ((time - lastWriteTimeUtc).Days >= 1)
			{
				return true;
			}
			if (this._commandType == UpdatableHelpCommandType.UpdateHelpCommand)
			{
				base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.UseForceToUpdateHelp, moduleName));
			}
			else if (this._commandType == UpdatableHelpCommandType.SaveHelpCommand)
			{
				base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.UseForceToSaveHelp, moduleName));
			}
			return false;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00089EC0 File Offset: 0x000880C0
		internal IEnumerable<string> ResolvePath(string path, bool recurse, bool isLiteralPath)
		{
			List<string> resolvedPaths = new List<string>();
			if (isLiteralPath)
			{
				string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(path);
				if (!Directory.Exists(unresolvedProviderPathFromPSPath))
				{
					throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, path), ErrorCategory.InvalidArgument, null, new ItemNotFoundException());
				}
				resolvedPaths.Add(unresolvedProviderPathFromPSPath);
			}
			else
			{
				Collection<PathInfo> resolvedPSPathFromPSPath = base.SessionState.Path.GetResolvedPSPathFromPSPath(path);
				foreach (PathInfo pathInfo in resolvedPSPathFromPSPath)
				{
					this.ValidatePathProvider(pathInfo);
					resolvedPaths.Add(pathInfo.ProviderPath);
				}
			}
			foreach (string resolvedPath in resolvedPaths)
			{
				if (recurse)
				{
					foreach (string innerResolvedPath in this.RecursiveResolvePathHelper(resolvedPath))
					{
						yield return innerResolvedPath;
					}
				}
				else
				{
					CmdletProviderContext context = new CmdletProviderContext(base.Context);
					context.SuppressWildcardExpansion = true;
					if (isLiteralPath || base.InvokeProvider.Item.IsContainer(resolvedPath, context))
					{
						yield return resolvedPath;
					}
				}
			}
			yield break;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0008A160 File Offset: 0x00088360
		private IEnumerable<string> RecursiveResolvePathHelper(string path)
		{
			if (Directory.Exists(path))
			{
				yield return path;
				foreach (string subDirectory in Directory.GetDirectories(path))
				{
					foreach (string subDirectory2 in this.RecursiveResolvePathHelper(subDirectory))
					{
						yield return subDirectory2;
					}
				}
			}
			yield break;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0008A184 File Offset: 0x00088384
		internal void ValidatePathProvider(PathInfo path)
		{
			if (path.Provider == null || path.Provider.Name != "FileSystem")
			{
				throw new PSArgumentException(StringUtil.Format(HelpDisplayStrings.ProviderIsNotFileSystem, path.Path));
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0008A1BC File Offset: 0x000883BC
		internal void LogMessage(string message)
		{
			List<string> list = new List<string>();
			list.Add(message);
			PSEtwLog.LogPipelineExecutionDetailEvent(MshLog.GetLogContext(base.Context, base.Context.CurrentCommandProcessor.Command.MyInvocation), list);
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0008A1FC File Offset: 0x000883FC
		internal void ProcessException(string moduleName, string culture, Exception e)
		{
			UpdatableHelpSystemException ex;
			if (e is UpdatableHelpSystemException)
			{
				ex = (UpdatableHelpSystemException)e;
			}
			else if (e is WebException)
			{
				ex = new UpdatableHelpSystemException("UnableToConnect", StringUtil.Format(HelpDisplayStrings.UnableToConnect, new object[0]), ErrorCategory.InvalidOperation, null, e);
			}
			else if (e is PSArgumentException)
			{
				ex = new UpdatableHelpSystemException("InvalidArgument", e.Message, ErrorCategory.InvalidArgument, null, e);
			}
			else
			{
				ex = new UpdatableHelpSystemException("UnknownErrorId", e.Message, ErrorCategory.InvalidOperation, null, e);
			}
			if (!this.exceptions.ContainsKey(ex.FullyQualifiedErrorId))
			{
				this.exceptions.Add(ex.FullyQualifiedErrorId, new UpdatableHelpExceptionContext(ex));
			}
			this.exceptions[ex.FullyQualifiedErrorId].Modules.Add(moduleName);
			if (culture != null)
			{
				this.exceptions[ex.FullyQualifiedErrorId].Cultures.Add(culture);
			}
		}

		// Token: 0x0400092A RID: 2346
		internal const string PathParameterSetName = "Path";

		// Token: 0x0400092B RID: 2347
		internal const string LiteralPathParameterSetName = "LiteralPath";

		// Token: 0x0400092C RID: 2348
		internal UpdatableHelpCommandType _commandType;

		// Token: 0x0400092D RID: 2349
		internal UpdatableHelpSystem _helpSystem;

		// Token: 0x0400092E RID: 2350
		internal bool _stopping;

		// Token: 0x0400092F RID: 2351
		internal int activityId;

		// Token: 0x04000930 RID: 2352
		private Dictionary<string, UpdatableHelpExceptionContext> exceptions;

		// Token: 0x04000931 RID: 2353
		internal string[] _language;

		// Token: 0x04000932 RID: 2354
		internal PSCredential _credential;

		// Token: 0x04000933 RID: 2355
		internal bool _useDefaultCredentials;

		// Token: 0x04000934 RID: 2356
		internal bool _force;

		// Token: 0x04000935 RID: 2357
		private static Dictionary<string, string> metadataCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
