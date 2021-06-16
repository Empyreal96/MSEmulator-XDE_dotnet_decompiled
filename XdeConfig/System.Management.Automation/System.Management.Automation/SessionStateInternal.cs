using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020007FD RID: 2045
	internal sealed class SessionStateInternal
	{
		// Token: 0x06004DD8 RID: 19928 RVA: 0x00198720 File Offset: 0x00196920
		internal static ISecurityDescriptorCmdletProvider GetPermissionProviderInstance(CmdletProvider providerInstance)
		{
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			ISecurityDescriptorCmdletProvider securityDescriptorCmdletProvider = providerInstance as ISecurityDescriptorCmdletProvider;
			if (securityDescriptorCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(ProviderBaseSecurity.ISecurityDescriptorCmdletProvider_NotSupported, new object[0]);
			}
			return securityDescriptorCmdletProvider;
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x00198758 File Offset: 0x00196958
		internal Collection<PSObject> GetSecurityDescriptor(string path, AccessControlSections sections)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			this.GetSecurityDescriptor(path, sections, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			Collection<PSObject> collection = cmdletProviderContext.GetAccumulatedObjects();
			if (collection == null)
			{
				collection = new Collection<PSObject>();
			}
			return collection;
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x001987A0 File Offset: 0x001969A0
		internal void GetSecurityDescriptor(string path, AccessControlSections sections, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, context, out providerInfo, out providerInstance);
			foreach (string path2 in globbedProviderPathsFromMonadPath)
			{
				this.GetSecurityDescriptor(providerInstance, path2, sections, context);
			}
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x00198818 File Offset: 0x00196A18
		private void GetSecurityDescriptor(CmdletProvider providerInstance, string path, AccessControlSections sections, CmdletProviderContext context)
		{
			SessionStateInternal.GetPermissionProviderInstance(providerInstance);
			try
			{
				providerInstance.GetSecurityDescriptor(path, sections, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetSecurityDescriptorProviderException", SessionStateStrings.GetSecurityDescriptorProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x00198898 File Offset: 0x00196A98
		internal Collection<PSObject> SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (securityDescriptor == null)
			{
				throw PSTraceSource.NewArgumentNullException("securityDescriptor");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			this.SetSecurityDescriptor(path, securityDescriptor, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			Collection<PSObject> collection = cmdletProviderContext.GetAccumulatedObjects();
			if (collection == null)
			{
				collection = new Collection<PSObject>();
			}
			return collection;
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x001988F0 File Offset: 0x00196AF0
		internal void SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (securityDescriptor == null)
			{
				throw PSTraceSource.NewArgumentNullException("securityDescriptor");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, context, out providerInfo, out providerInstance);
			foreach (string path2 in globbedProviderPathsFromMonadPath)
			{
				this.SetSecurityDescriptor(providerInstance, path2, securityDescriptor, context);
			}
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x00198974 File Offset: 0x00196B74
		private void SetSecurityDescriptor(CmdletProvider providerInstance, string path, ObjectSecurity securityDescriptor, CmdletProviderContext context)
		{
			SessionStateInternal.GetPermissionProviderInstance(providerInstance);
			try
			{
				providerInstance.SetSecurityDescriptor(path, securityDescriptor, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (PrivilegeNotHeldException ex)
			{
				context.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.PermissionDenied, path));
			}
			catch (UnauthorizedAccessException ex2)
			{
				context.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.PermissionDenied, path));
			}
			catch (NotSupportedException ex3)
			{
				context.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.InvalidOperation, path));
			}
			catch (SystemException ex4)
			{
				CommandProcessorBase.CheckForSevereException(ex4);
				context.WriteError(new ErrorRecord(ex4, ex4.GetType().FullName, ErrorCategory.InvalidOperation, path));
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("SetSecurityDescriptorProviderException", SessionStateStrings.SetSecurityDescriptorProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x00198AA8 File Offset: 0x00196CA8
		internal ObjectSecurity NewSecurityDescriptorFromPath(string path, AccessControlSections sections)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count == 1)
			{
				return this.NewSecurityDescriptorFromPath(providerInstance, globbedProviderPathsFromMonadPath[0], sections);
			}
			throw PSTraceSource.NewArgumentException("path");
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x00198B04 File Offset: 0x00196D04
		private ObjectSecurity NewSecurityDescriptorFromPath(CmdletProvider providerInstance, string path, AccessControlSections sections)
		{
			ObjectSecurity result = null;
			ISecurityDescriptorCmdletProvider permissionProviderInstance = SessionStateInternal.GetPermissionProviderInstance(providerInstance);
			try
			{
				result = permissionProviderInstance.NewSecurityDescriptorFromPath(path, sections);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewSecurityDescriptorProviderException", SessionStateStrings.GetSecurityDescriptorProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x00198B84 File Offset: 0x00196D84
		internal ObjectSecurity NewSecurityDescriptorOfType(string providerId, string type, AccessControlSections sections)
		{
			CmdletProvider providerInstance = this.GetProviderInstance(providerId);
			return this.NewSecurityDescriptorOfType(providerInstance, type, sections);
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x00198BA4 File Offset: 0x00196DA4
		internal ObjectSecurity NewSecurityDescriptorOfType(CmdletProvider providerInstance, string type, AccessControlSections sections)
		{
			ObjectSecurity result = null;
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			ISecurityDescriptorCmdletProvider permissionProviderInstance = SessionStateInternal.GetPermissionProviderInstance(providerInstance);
			try
			{
				result = permissionProviderInstance.NewSecurityDescriptorOfType(type, sections);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewSecurityDescriptorProviderException", SessionStateStrings.GetSecurityDescriptorProviderException, providerInstance.ProviderInfo, type, e);
			}
			return result;
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x00198C40 File Offset: 0x00196E40
		internal SessionStateInternal(ExecutionContext context) : this(null, false, context)
		{
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x00198C4C File Offset: 0x00196E4C
		internal SessionStateInternal(SessionStateInternal parent, bool linkToGlobal, ExecutionContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			this._context = context;
			this.workingLocationStack = new Dictionary<string, Stack<PathInfo>>(StringComparer.OrdinalIgnoreCase);
			this._globalScope = new SessionStateScope(null);
			this._moduleScope = this._globalScope;
			this.currentScope = this._globalScope;
			this.InitializeSessionStateInternalSpecialVariables(false);
			this._globalScope.ScriptScope = this._globalScope;
			if (parent != null)
			{
				this._globalScope.Parent = parent.GlobalScope;
				this.CopyProviders(parent);
				if (this.Providers != null && this.Providers.Count > 0)
				{
					this.CurrentDrive = parent.CurrentDrive;
				}
				if (linkToGlobal)
				{
					this._globalScope = parent.GlobalScope;
					return;
				}
			}
			else
			{
				this.currentScope.LocalsTuple = MutableTuple.MakeTuple(Compiler.DottedLocalsTupleType, Compiler.DottedLocalsNameIndexMap);
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x00198DD8 File Offset: 0x00196FD8
		internal void InitializeSessionStateInternalSpecialVariables(bool clearVariablesTable)
		{
			if (clearVariablesTable)
			{
				this._globalScope.Variables.Clear();
				this._globalScope.AddSessionStateScopeDefaultVariables();
			}
			PSVariable psvariable = new PSVariable("Error", new ArrayList(), ScopedItemOptions.Constant);
			this._globalScope.SetVariable(psvariable.Name, psvariable, false, false, this, CommandOrigin.Internal, true);
			Collection<Attribute> collection = new Collection<Attribute>();
			collection.Add(new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(DefaultParameterDictionary)
			}));
			PSVariable psvariable2 = new PSVariable("PSDefaultParameterValues", new DefaultParameterDictionary(), ScopedItemOptions.None, collection, RunspaceInit.PSDefaultParameterValuesDescription);
			this._globalScope.SetVariable(psvariable2.Name, psvariable2, false, false, this, CommandOrigin.Internal, true);
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x00198E81 File Offset: 0x00197081
		internal LocationGlobber Globber
		{
			get
			{
				if (this.globberPrivate == null)
				{
					this.globberPrivate = this._context.LocationGlobber;
				}
				return this.globberPrivate;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06004DE7 RID: 19943 RVA: 0x00198EA2 File Offset: 0x001970A2
		internal ExecutionContext ExecutionContext
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x00198EAA File Offset: 0x001970AA
		// (set) Token: 0x06004DE9 RID: 19945 RVA: 0x00198EC6 File Offset: 0x001970C6
		internal SessionState PublicSessionState
		{
			get
			{
				if (this._publicSessionState == null)
				{
					this._publicSessionState = new SessionState(this);
				}
				return this._publicSessionState;
			}
			set
			{
				this._publicSessionState = value;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06004DEA RID: 19946 RVA: 0x00198ECF File Offset: 0x001970CF
		internal ProviderIntrinsics InvokeProvider
		{
			get
			{
				if (this._invokeProvider == null)
				{
					this._invokeProvider = new ProviderIntrinsics(this);
				}
				return this._invokeProvider;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x00198EEB File Offset: 0x001970EB
		// (set) Token: 0x06004DEC RID: 19948 RVA: 0x00198EF3 File Offset: 0x001970F3
		internal PSModuleInfo Module
		{
			get
			{
				return this._module;
			}
			set
			{
				this._module = value;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x00198EFC File Offset: 0x001970FC
		internal Dictionary<string, PSModuleInfo> ModuleTable
		{
			get
			{
				return this._moduleTable;
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x00198F04 File Offset: 0x00197104
		// (set) Token: 0x06004DEF RID: 19951 RVA: 0x00198F11 File Offset: 0x00197111
		internal PSLanguageMode LanguageMode
		{
			get
			{
				return this._context.LanguageMode;
			}
			set
			{
				this._context.LanguageMode = value;
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06004DF0 RID: 19952 RVA: 0x00198F1F File Offset: 0x0019711F
		internal bool UseFullLanguageModeInDebugger
		{
			get
			{
				return this._context.UseFullLanguageModeInDebugger;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06004DF1 RID: 19953 RVA: 0x00198F2C File Offset: 0x0019712C
		public List<string> Scripts
		{
			get
			{
				return this._scripts;
			}
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x00198F34 File Offset: 0x00197134
		internal SessionStateEntryVisibility CheckScriptVisibility(string scriptPath)
		{
			return this.checkPathVisibility(this._scripts, scriptPath);
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x00198F43 File Offset: 0x00197143
		public List<string> Applications
		{
			get
			{
				return this._applications;
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x00198F4B File Offset: 0x0019714B
		internal List<CmdletInfo> ExportedCmdlets
		{
			get
			{
				return this._exportedCmdlets;
			}
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x00198F53 File Offset: 0x00197153
		internal void AddSessionStateEntry(SessionStateCmdletEntry entry)
		{
			this.AddSessionStateEntry(entry, false);
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00198F5D File Offset: 0x0019715D
		internal void AddSessionStateEntry(SessionStateCmdletEntry entry, bool local)
		{
			this.ExecutionContext.CommandDiscovery.AddSessionStateCmdletEntryToCache(entry, local);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x00198F71 File Offset: 0x00197171
		internal void AddSessionStateEntry(SessionStateApplicationEntry entry)
		{
			this.Applications.Add(entry.Path);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x00198F84 File Offset: 0x00197184
		internal void AddSessionStateEntry(SessionStateScriptEntry entry)
		{
			this.Scripts.Add(entry.Path);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x00198F98 File Offset: 0x00197198
		internal void InitializeFixedVariables()
		{
			PSVariable psvariable = new PSVariable("Host", this._context.EngineHostInterface, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.PSHostDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			string value = Environment.GetEnvironmentVariable("USERPROFILE") ?? string.Empty;
			psvariable = new PSVariable("HOME", value, ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope, RunspaceInit.HOMEDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			psvariable = new PSVariable("ExecutionContext", this._context.EngineIntrinsics, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.ExecutionContextDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			psvariable = new PSVariable("PSVersionTable", PSVersionInfo.GetPSVersionTable(), ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.PSVersionTableDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			Process currentProcess = Process.GetCurrentProcess();
			psvariable = new PSVariable("PID", currentProcess.Id, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.PIDDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			psvariable = new PSCultureVariable();
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			psvariable = new PSUICultureVariable();
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			string shellID = this._context.ShellID;
			psvariable = new PSVariable("ShellId", shellID, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.MshShellIdDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			string value2 = "";
			try
			{
				value2 = Utils.GetApplicationBase(shellID);
			}
			catch (SecurityException)
			{
			}
			psvariable = new PSVariable("PSHOME", value2, ScopedItemOptions.Constant | ScopedItemOptions.AllScope, RunspaceInit.PSHOMEDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
			this.SetConsoleVariable();
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x00199184 File Offset: 0x00197384
		internal void SetConsoleVariable()
		{
			string value = string.Empty;
			RunspaceConfigForSingleShell runspaceConfigForSingleShell = this._context.RunspaceConfiguration as RunspaceConfigForSingleShell;
			if (runspaceConfigForSingleShell != null && runspaceConfigForSingleShell.ConsoleInfo != null && !string.IsNullOrEmpty(runspaceConfigForSingleShell.ConsoleInfo.Filename))
			{
				value = runspaceConfigForSingleShell.ConsoleInfo.Filename;
			}
			PSVariable psvariable = new PSVariable("ConsoleFileName", value, ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope, RunspaceInit.ConsoleDescription);
			this.GlobalScope.SetVariable(psvariable.Name, psvariable, false, true, this, CommandOrigin.Internal, true);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001991FC File Offset: 0x001973FC
		internal void AddBuiltInEntries(bool addSetStrictMode)
		{
			this.AddBuiltInVariables();
			this.AddBuiltInFunctions();
			this.AddBuiltInAliases();
			if (addSetStrictMode)
			{
				SessionStateFunctionEntry entry = new SessionStateFunctionEntry("Set-StrictMode", "");
				this.AddSessionStateEntry(entry);
			}
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x00199238 File Offset: 0x00197438
		internal void AddBuiltInVariables()
		{
			foreach (SessionStateVariableEntry entry in InitialSessionState.BuiltInVariables)
			{
				this.AddSessionStateEntry(entry);
			}
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x00199264 File Offset: 0x00197464
		internal void AddBuiltInFunctions()
		{
			foreach (SessionStateFunctionEntry entry in InitialSessionState.BuiltInFunctions)
			{
				this.AddSessionStateEntry(entry);
			}
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x00199290 File Offset: 0x00197490
		internal void AddBuiltInAliases()
		{
			foreach (SessionStateAliasEntry entry in InitialSessionState.BuiltInAliases)
			{
				this.AddSessionStateEntry(entry, "GLOBAL");
			}
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x001992C1 File Offset: 0x001974C1
		internal SessionStateEntryVisibility CheckApplicationVisibility(string applicationPath)
		{
			return this.checkPathVisibility(this._applications, applicationPath);
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x001992D0 File Offset: 0x001974D0
		private SessionStateEntryVisibility checkPathVisibility(List<string> list, string path)
		{
			if (list == null || list.Count == 0)
			{
				return SessionStateEntryVisibility.Private;
			}
			if (string.IsNullOrEmpty(path))
			{
				return SessionStateEntryVisibility.Private;
			}
			if (list.Contains("*"))
			{
				return SessionStateEntryVisibility.Public;
			}
			foreach (string text in list)
			{
				if (string.Equals(text, path, StringComparison.OrdinalIgnoreCase))
				{
					return SessionStateEntryVisibility.Public;
				}
				if (WildcardPattern.ContainsWildcardCharacters(text))
				{
					WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
					if (wildcardPattern.IsMatch(path))
					{
						return SessionStateEntryVisibility.Public;
					}
				}
			}
			return SessionStateEntryVisibility.Private;
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0019936C File Offset: 0x0019756C
		internal void RunspaceClosingNotification()
		{
			if (this != this._context.TopLevelSessionState && this.Providers.Count > 0)
			{
				CmdletProviderContext context = new CmdletProviderContext(this.ExecutionContext);
				Collection<string> collection = new Collection<string>();
				foreach (string item in this.Providers.Keys)
				{
					collection.Add(item);
				}
				foreach (string providerName in collection)
				{
					this.RemoveProvider(providerName, true, context);
				}
			}
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x00199438 File Offset: 0x00197638
		internal ProviderInvocationException NewProviderInvocationException(string resourceId, string resourceStr, ProviderInfo provider, string path, Exception e)
		{
			return this.NewProviderInvocationException(resourceId, resourceStr, provider, path, e, true);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x00199448 File Offset: 0x00197648
		internal ProviderInvocationException NewProviderInvocationException(string resourceId, string resourceStr, ProviderInfo provider, string path, Exception e, bool useInnerExceptionErrorMessage)
		{
			ProviderInvocationException ex = e as ProviderInvocationException;
			if (ex != null)
			{
				ex._providerInfo = provider;
				return ex;
			}
			ex = new ProviderInvocationException(resourceId, resourceStr, provider, path, e, useInnerExceptionErrorMessage);
			MshLog.LogProviderHealthEvent(this._context, provider.Name, ex, Severity.Warning);
			return ex;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0019948C File Offset: 0x0019768C
		internal void AddSessionStateEntry(SessionStateAliasEntry entry, string scopeID)
		{
			AliasInfo aliasInfo = new AliasInfo(entry.Name, entry.Definition, this.ExecutionContext, entry.Options);
			aliasInfo.Visibility = entry.Visibility;
			aliasInfo.SetModule(entry.Module);
			if (!string.IsNullOrEmpty(entry.Description))
			{
				aliasInfo.Description = entry.Description;
			}
			this.SetAliasItemAtScope(aliasInfo, scopeID, true, CommandOrigin.Internal);
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x001994F4 File Offset: 0x001976F4
		internal IDictionary<string, AliasInfo> GetAliasTable()
		{
			Dictionary<string, AliasInfo> dictionary = new Dictionary<string, AliasInfo>(StringComparer.OrdinalIgnoreCase);
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				foreach (AliasInfo aliasInfo in sessionStateScope.AliasTable)
				{
					if (!dictionary.ContainsKey(aliasInfo.Name) && ((aliasInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope))
					{
						dictionary.Add(aliasInfo.Name, aliasInfo);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x001995BC File Offset: 0x001977BC
		internal IDictionary<string, AliasInfo> GetAliasTableAtScope(string scopeID)
		{
			Dictionary<string, AliasInfo> dictionary = new Dictionary<string, AliasInfo>(StringComparer.OrdinalIgnoreCase);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			foreach (AliasInfo aliasInfo in scopeByID.AliasTable)
			{
				if ((aliasInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || scopeByID == this.currentScope)
				{
					dictionary.Add(aliasInfo.Name, aliasInfo);
				}
			}
			return dictionary;
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06004E07 RID: 19975 RVA: 0x00199638 File Offset: 0x00197838
		internal List<AliasInfo> ExportedAliases
		{
			get
			{
				return this._exportedAliases;
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x00199640 File Offset: 0x00197840
		internal AliasInfo GetAlias(string aliasName, CommandOrigin origin)
		{
			AliasInfo aliasInfo = null;
			if (string.IsNullOrEmpty(aliasName))
			{
				return aliasInfo;
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				aliasInfo = sessionStateScope.GetAlias(aliasName);
				if (aliasInfo != null)
				{
					SessionState.ThrowIfNotVisible(origin, aliasInfo);
					if ((aliasInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
					{
						break;
					}
					aliasInfo = null;
				}
			}
			return aliasInfo;
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x001996C4 File Offset: 0x001978C4
		internal AliasInfo GetAlias(string aliasName)
		{
			return this.GetAlias(aliasName, CommandOrigin.Internal);
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x001996D0 File Offset: 0x001978D0
		internal AliasInfo GetAliasAtScope(string aliasName, string scopeID)
		{
			AliasInfo aliasInfo = null;
			if (string.IsNullOrEmpty(aliasName))
			{
				return aliasInfo;
			}
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			aliasInfo = scopeByID.GetAlias(aliasName);
			if (aliasInfo != null && (aliasInfo.Options & ScopedItemOptions.Private) != ScopedItemOptions.None && scopeByID != this.currentScope)
			{
				aliasInfo = null;
			}
			return aliasInfo;
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x00199714 File Offset: 0x00197914
		internal AliasInfo SetAliasValue(string aliasName, string value, bool force, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(aliasName))
			{
				throw PSTraceSource.NewArgumentException("aliasName");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw PSTraceSource.NewArgumentException("value");
			}
			return this.currentScope.SetAliasValue(aliasName, value, this.ExecutionContext, force, origin);
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x0019975F File Offset: 0x0019795F
		internal AliasInfo SetAliasValue(string aliasName, string value, bool force)
		{
			return this.SetAliasValue(aliasName, value, force, CommandOrigin.Internal);
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x0019976C File Offset: 0x0019796C
		internal AliasInfo SetAliasValue(string aliasName, string value, ScopedItemOptions options, bool force, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(aliasName))
			{
				throw PSTraceSource.NewArgumentException("aliasName");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw PSTraceSource.NewArgumentException("value");
			}
			return this.currentScope.SetAliasValue(aliasName, value, options, this.ExecutionContext, force, origin);
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x001997B9 File Offset: 0x001979B9
		internal AliasInfo SetAliasValue(string aliasName, string value, ScopedItemOptions options, bool force)
		{
			return this.SetAliasValue(aliasName, value, options, force, CommandOrigin.Internal);
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x001997C8 File Offset: 0x001979C8
		internal AliasInfo SetAliasItem(AliasInfo alias, bool force, CommandOrigin origin)
		{
			if (alias == null)
			{
				throw PSTraceSource.NewArgumentNullException("alias");
			}
			return this.currentScope.SetAliasItem(alias, force, origin);
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x001997F4 File Offset: 0x001979F4
		internal AliasInfo SetAliasItemAtScope(AliasInfo alias, string scopeID, bool force, CommandOrigin origin)
		{
			if (alias == null)
			{
				throw PSTraceSource.NewArgumentNullException("alias");
			}
			if (string.Equals(scopeID, "PRIVATE", StringComparison.OrdinalIgnoreCase))
			{
				alias.Options |= ScopedItemOptions.Private;
			}
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			return scopeByID.SetAliasItem(alias, force, origin);
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0019983F File Offset: 0x00197A3F
		internal AliasInfo SetAliasItemAtScope(AliasInfo alias, string scopeID, bool force)
		{
			return this.SetAliasItemAtScope(alias, scopeID, force, CommandOrigin.Internal);
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0019984C File Offset: 0x00197A4C
		internal void RemoveAlias(string aliasName, bool force)
		{
			if (string.IsNullOrEmpty(aliasName))
			{
				throw PSTraceSource.NewArgumentException("aliasName");
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				AliasInfo alias = sessionStateScope.GetAlias(aliasName);
				if (alias != null)
				{
					if ((alias.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
					{
						sessionStateScope.RemoveAlias(aliasName, force);
						break;
					}
				}
			}
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x00199B14 File Offset: 0x00197D14
		internal IEnumerable<string> GetAliasesByCommandName(string command)
		{
			SessionStateScopeEnumerator scopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope scope in ((IEnumerable<SessionStateScope>)scopeEnumerator))
			{
				foreach (string alias in scope.GetAliasesByCommandName(command))
				{
					yield return alias;
				}
			}
			yield break;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x00199B38 File Offset: 0x00197D38
		internal CmdletInfo GetCmdlet(string cmdletName)
		{
			return this.GetCmdlet(cmdletName, CommandOrigin.Internal);
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x00199B44 File Offset: 0x00197D44
		internal CmdletInfo GetCmdlet(string cmdletName, CommandOrigin origin)
		{
			CmdletInfo cmdletInfo = null;
			if (string.IsNullOrEmpty(cmdletName))
			{
				return cmdletInfo;
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				cmdletInfo = sessionStateScope.GetCmdlet(cmdletName);
				if (cmdletInfo != null)
				{
					SessionState.ThrowIfNotVisible(origin, cmdletInfo);
					if ((cmdletInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
					{
						break;
					}
					cmdletInfo = null;
				}
			}
			return cmdletInfo;
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x00199BC8 File Offset: 0x00197DC8
		internal CmdletInfo GetCmdletAtScope(string cmdletName, string scopeID)
		{
			CmdletInfo cmdletInfo = null;
			if (string.IsNullOrEmpty(cmdletName))
			{
				return cmdletInfo;
			}
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			cmdletInfo = scopeByID.GetCmdlet(cmdletName);
			if (cmdletInfo != null && (cmdletInfo.Options & ScopedItemOptions.Private) != ScopedItemOptions.None && scopeByID != this.currentScope)
			{
				cmdletInfo = null;
			}
			return cmdletInfo;
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x00199C0C File Offset: 0x00197E0C
		internal IDictionary<string, List<CmdletInfo>> GetCmdletTable()
		{
			Dictionary<string, List<CmdletInfo>> dictionary = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				foreach (KeyValuePair<string, List<CmdletInfo>> keyValuePair in sessionStateScope.CmdletTable)
				{
					if (!dictionary.ContainsKey(keyValuePair.Key))
					{
						List<CmdletInfo> list = new List<CmdletInfo>();
						foreach (CmdletInfo cmdletInfo in keyValuePair.Value)
						{
							if ((cmdletInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
							{
								list.Add(cmdletInfo);
							}
						}
						dictionary.Add(keyValuePair.Key, list);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00199D30 File Offset: 0x00197F30
		internal IDictionary<string, List<CmdletInfo>> GetCmdletTableAtScope(string scopeID)
		{
			Dictionary<string, List<CmdletInfo>> dictionary = new Dictionary<string, List<CmdletInfo>>(StringComparer.OrdinalIgnoreCase);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			foreach (KeyValuePair<string, List<CmdletInfo>> keyValuePair in scopeByID.CmdletTable)
			{
				List<CmdletInfo> list = new List<CmdletInfo>();
				foreach (CmdletInfo cmdletInfo in keyValuePair.Value)
				{
					if ((cmdletInfo.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || scopeByID == this.currentScope)
					{
						list.Add(cmdletInfo);
					}
				}
				dictionary.Add(keyValuePair.Key, list);
			}
			return dictionary;
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x00199E00 File Offset: 0x00198000
		internal void RemoveCmdlet(string name, int index, bool force)
		{
			this.RemoveCmdlet(name, index, force, CommandOrigin.Internal);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x00199E0C File Offset: 0x0019800C
		internal void RemoveCmdlet(string name, int index, bool force, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				CmdletInfo cmdlet = sessionStateScope.GetCmdlet(name);
				if (cmdlet != null)
				{
					if ((cmdlet.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
					{
						sessionStateScope.RemoveCmdlet(name, index, force);
						break;
					}
				}
			}
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x00199E9C File Offset: 0x0019809C
		internal void RemoveCmdletEntry(string name, bool force)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				CmdletInfo cmdlet = sessionStateScope.GetCmdlet(name);
				if (cmdlet != null)
				{
					if ((cmdlet.Options & ScopedItemOptions.Private) == ScopedItemOptions.None || sessionStateScope == this.currentScope)
					{
						sessionStateScope.RemoveCmdletEntry(name, force);
						break;
					}
				}
			}
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x00199F28 File Offset: 0x00198128
		internal bool ItemExists(string path, bool force, bool literalPath)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			bool flag = this.ItemExists(path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x00199F94 File Offset: 0x00198194
		internal bool ItemExists(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			bool flag = false;
			try
			{
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, context, out providerInfo, out providerInstance);
				foreach (string path2 in globbedProviderPathsFromMonadPath)
				{
					flag = this.ItemExists(providerInstance, path2, context);
					if (flag)
					{
						break;
					}
				}
			}
			catch (ItemNotFoundException)
			{
				flag = false;
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0019A048 File Offset: 0x00198248
		internal bool ItemExists(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			bool result = false;
			try
			{
				result = itemProviderInstance.ItemExists(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ItemExistsProviderException", SessionStateStrings.ItemExistsProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0019A0C8 File Offset: 0x001982C8
		internal object ItemExistsDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.ItemExistsDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0019A12C File Offset: 0x0019832C
		private object ItemExistsDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.ItemExistsDynamicParameters(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ItemExistsDynamicParametersProviderException", SessionStateStrings.ItemExistsDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0019A1AC File Offset: 0x001983AC
		internal bool IsValidPath(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			bool flag = this.IsValidPath(path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0019A204 File Offset: 0x00198404
		internal bool IsValidPath(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo provider = null;
			PSDriveInfo psdriveInfo = null;
			string providerPath = this.Globber.GetProviderPath(path, context, out provider, out psdriveInfo);
			ItemCmdletProvider itemProviderInstance = this.GetItemProviderInstance(provider);
			bool flag = this.IsValidPath(itemProviderInstance, providerPath, context);
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0019A270 File Offset: 0x00198470
		private bool IsValidPath(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			bool result = false;
			try
			{
				result = itemProviderInstance.IsValidPath(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("IsValidPathProviderException", SessionStateStrings.IsValidPathProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0019A2F0 File Offset: 0x001984F0
		internal bool IsItemContainer(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			bool flag = this.IsItemContainer(path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0019A348 File Offset: 0x00198548
		internal bool IsItemContainer(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			bool flag = false;
			try
			{
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, context, out providerInfo, out providerInstance);
				foreach (string path2 in globbedProviderPathsFromMonadPath)
				{
					flag = this.IsItemContainer(providerInstance, path2, context);
					if (!flag)
					{
						break;
					}
				}
			}
			catch (ItemNotFoundException)
			{
				flag = false;
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0019A3FC File Offset: 0x001985FC
		private bool IsItemContainer(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			bool flag = false;
			NavigationCmdletProvider navigationCmdletProvider = null;
			try
			{
				navigationCmdletProvider = SessionStateInternal.GetNavigationProviderInstance(providerInstance, false);
				try
				{
					flag = navigationCmdletProvider.IsItemContainer(path, context);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					throw this.NewProviderInvocationException("IsItemContainerProviderException", SessionStateStrings.IsItemContainerProviderException, navigationCmdletProvider.ProviderInfo, path, e);
				}
			}
			catch (NotSupportedException)
			{
				try
				{
					SessionStateInternal.GetContainerProviderInstance(providerInstance);
					flag = (path.Length == 0);
				}
				catch (NotSupportedException)
				{
					flag = false;
				}
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0019A4D8 File Offset: 0x001986D8
		internal void RemoveItem(string[] paths, bool recurse, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.RemoveItem(paths, recurse, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x0019A524 File Offset: 0x00198724
		internal void RemoveItem(string[] paths, bool recurse, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.RemoveItem(providerInstance, path, recurse, context);
				}
			}
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0019A5C8 File Offset: 0x001987C8
		internal void RemoveItem(string providerId, string path, bool recurse, CmdletProviderContext context)
		{
			CmdletProvider providerInstance = this.GetProviderInstance(providerId);
			this.RemoveItem(providerInstance, path, recurse, context);
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0019A5E8 File Offset: 0x001987E8
		internal void RemoveItem(CmdletProvider providerInstance, string path, bool recurse, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				if (context.HasIncludeOrExclude)
				{
					int num = 0;
					this.ProcessPathItems(providerInstance, path, recurse, context, out num, ProcessMode.Delete, false);
					if (this.IsItemContainer(providerInstance, path, context))
					{
						string childName = this.GetChildName(path, context, false);
						bool flag = SessionStateUtilities.MatchesAnyWildcardPattern(childName, SessionStateUtilities.CreateWildcardsFromStrings(context.Include, WildcardOptions.IgnoreCase), true);
						if (flag && !SessionStateUtilities.MatchesAnyWildcardPattern(childName, SessionStateUtilities.CreateWildcardsFromStrings(context.Exclude, WildcardOptions.IgnoreCase), false) && num == 0)
						{
							containerProviderInstance.RemoveItem(path, false, context);
						}
					}
				}
				else
				{
					containerProviderInstance.RemoveItem(path, recurse, context);
				}
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemoveItemProviderException", SessionStateStrings.RemoveItemProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0019A6D8 File Offset: 0x001988D8
		internal object RemoveItemDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.RemoveItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], recurse, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x0019A734 File Offset: 0x00198934
		private object RemoveItemDynamicParameters(CmdletProvider providerInstance, string path, bool recurse, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.RemoveItemDynamicParameters(path, recurse, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemoveItemProviderException", SessionStateStrings.RemoveItemProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x0019A7B8 File Offset: 0x001989B8
		internal Collection<PSObject> GetChildItems(string[] paths, bool recurse, uint depth, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				this.GetChildItems(text, recurse, depth, cmdletProviderContext);
			}
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0019A82C File Offset: 0x00198A2C
		internal void GetChildItems(string path, bool recurse, uint depth, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			ProviderInfo provider = null;
			if ((recurse && !context.SuppressWildcardExpansion) || LocationGlobber.ShouldPerformGlobbing(path, context))
			{
				bool flag = false;
				try
				{
					if (recurse && (context.Include == null || context.Include.Count == 0) && !string.IsNullOrEmpty(path) && !this.IsItemContainer(path))
					{
						string childName = this.GetChildName(path, context);
						if (!string.Equals(childName, "*", StringComparison.OrdinalIgnoreCase) && context.Include != null)
						{
							context.Include.Add(childName);
							flag = true;
						}
						string text = path.Substring(0, path.Length - childName.Length);
						path = text;
					}
					Collection<string> include = context.Include;
					Collection<string> exclude = context.Exclude;
					string filter = context.Filter;
					if (recurse)
					{
						context.SetFilters(new Collection<string>(), new Collection<string>(), null);
					}
					CmdletProvider providerInstance = null;
					Collection<string> collection = null;
					try
					{
						collection = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, context, out provider, out providerInstance);
					}
					finally
					{
						context.SetFilters(include, exclude, filter);
					}
					if (recurse)
					{
						this.GetContainerProviderInstance(provider);
					}
					bool flag2 = !LocationGlobber.StringContainsGlobCharacters(path);
					if ((recurse && !flag2 && include != null && include.Count == 0) || (include != null && include.Count > 0) || (exclude != null && exclude.Count > 0))
					{
						using (IEnumerator<string> enumerator = collection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string path2 = enumerator.Current;
								if (context.Stopping)
								{
									break;
								}
								int num = 0;
								this.ProcessPathItems(providerInstance, path2, recurse, context, out num, ProcessMode.Enumerate, false);
							}
							goto IL_205;
						}
					}
					foreach (string path3 in collection)
					{
						if (context.Stopping)
						{
							break;
						}
						if ((flag2 || recurse) && this.IsItemContainer(providerInstance, path3, context))
						{
							this.GetChildItems(providerInstance, path3, recurse, depth, context);
						}
						else
						{
							this.GetItemPrivate(providerInstance, path3, context);
						}
					}
					IL_205:
					return;
				}
				finally
				{
					if (flag)
					{
						context.Include.Clear();
					}
				}
			}
			PSDriveInfo psdriveInfo = null;
			path = this.Globber.GetProviderPath(path, context, out provider, out psdriveInfo);
			if (psdriveInfo != null)
			{
				context.Drive = psdriveInfo;
			}
			ContainerCmdletProvider containerProviderInstance = this.GetContainerProviderInstance(provider);
			if (path == null || !this.ItemExists(containerProviderInstance, path, context))
			{
				ItemNotFoundException ex = new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
				throw ex;
			}
			if (this.IsItemContainer(containerProviderInstance, path, context))
			{
				this.GetChildItems(containerProviderInstance, path, recurse, depth, context);
				return;
			}
			this.GetItemPrivate(containerProviderInstance, path, context);
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0019AB38 File Offset: 0x00198D38
		private void GetChildItems(CmdletProvider providerInstance, string path, bool recurse, uint depth, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				containerProviderInstance.GetChildItems(path, recurse, depth, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetChildrenProviderException", SessionStateStrings.GetChildrenProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0019ABB8 File Offset: 0x00198DB8
		private void ProcessPathItems(CmdletProvider providerInstance, string path, bool recurse, CmdletProviderContext context, out int childrenNotMatchingFilterCriteria, ProcessMode processMode = ProcessMode.Enumerate, bool skipIsItemContainerCheck = false)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			childrenNotMatchingFilterCriteria = 0;
			Collection<WildcardPattern> patterns = SessionStateUtilities.CreateWildcardsFromStrings(context.Include, WildcardOptions.IgnoreCase);
			Collection<WildcardPattern> patterns2 = SessionStateUtilities.CreateWildcardsFromStrings(context.Exclude, WildcardOptions.IgnoreCase);
			if (skipIsItemContainerCheck || this.IsItemContainer(providerInstance, path, context))
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
				Collection<PSObject> collection = null;
				Dictionary<string, bool> dictionary = null;
				try
				{
					this.GetChildNames(providerInstance, path, recurse ? ReturnContainers.ReturnAllContainers : ReturnContainers.ReturnMatchingContainers, cmdletProviderContext);
					cmdletProviderContext.WriteErrorsToContext(context);
					collection = cmdletProviderContext.GetAccumulatedObjects();
					if (recurse && providerInstance.IsFilterSet())
					{
						cmdletProviderContext.RemoveStopReferral();
						cmdletProviderContext = new CmdletProviderContext(context);
						Collection<PSObject> collection2 = new Collection<PSObject>();
						dictionary = new Dictionary<string, bool>();
						this.GetChildNames(providerInstance, path, ReturnContainers.ReturnMatchingContainers, cmdletProviderContext);
						collection2 = cmdletProviderContext.GetAccumulatedObjects();
						foreach (PSObject psobject in collection2)
						{
							string text = psobject.BaseObject as string;
							if (text != null)
							{
								dictionary[text] = true;
							}
						}
					}
				}
				finally
				{
					cmdletProviderContext.RemoveStopReferral();
				}
				for (int i = 0; i < collection.Count; i++)
				{
					if (context.Stopping)
					{
						return;
					}
					string text2 = collection[i].BaseObject as string;
					if (text2 != null)
					{
						string text3 = this.MakePath(providerInstance, path, text2, context);
						if (text3 != null)
						{
							bool flag = !context.SuppressWildcardExpansion && SessionStateUtilities.MatchesAnyWildcardPattern(text2, patterns, true);
							if (flag)
							{
								if (!SessionStateUtilities.MatchesAnyWildcardPattern(text2, patterns2, false))
								{
									bool flag2 = true;
									if (dictionary != null)
									{
										bool flag3 = false;
										flag2 = dictionary.TryGetValue(text2, out flag3);
									}
									if (flag2)
									{
										if (processMode == ProcessMode.Delete)
										{
											containerProviderInstance.RemoveItem(text3, false, context);
										}
										else if (processMode != ProcessMode.Delete)
										{
											this.GetItemPrivate(providerInstance, text3, context);
										}
									}
								}
								else
								{
									childrenNotMatchingFilterCriteria++;
								}
							}
							else
							{
								childrenNotMatchingFilterCriteria++;
							}
							if (recurse && this.IsItemContainer(providerInstance, text3, context))
							{
								if (context.Stopping)
								{
									return;
								}
								this.ProcessPathItems(providerInstance, text3, recurse, context, out childrenNotMatchingFilterCriteria, processMode, true);
							}
						}
					}
				}
				return;
			}
			string childName = this.GetChildName(providerInstance, path, context, true);
			bool flag4 = SessionStateUtilities.MatchesAnyWildcardPattern(childName, patterns, true);
			if (flag4 && !SessionStateUtilities.MatchesAnyWildcardPattern(childName, patterns2, false))
			{
				if (processMode != ProcessMode.Delete)
				{
					this.GetItemPrivate(providerInstance, path, context);
					return;
				}
				containerProviderInstance.RemoveItem(path, recurse, context);
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0019AE10 File Offset: 0x00199010
		internal object GetChildItemsDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider cmdletProvider = null;
			this.Globber.GetProviderPath(path, out providerInfo);
			if (!this.HasGetChildItemDynamicParameters(providerInfo))
			{
				return null;
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> collection = null;
			try
			{
				collection = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out cmdletProvider);
			}
			catch (ItemNotFoundException)
			{
				if (cmdletProvider == null)
				{
					throw;
				}
			}
			if (collection != null && collection.Count > 0)
			{
				return this.GetChildItemsDynamicParameters(cmdletProvider, collection[0], recurse, cmdletProviderContext);
			}
			if (cmdletProvider != null)
			{
				PSDriveInfo psdriveInfo = null;
				string providerPath = this.Globber.GetProviderPath(path, context, out providerInfo, out psdriveInfo);
				if (providerPath != null)
				{
					return this.GetChildItemsDynamicParameters(cmdletProvider, providerPath, recurse, cmdletProviderContext);
				}
			}
			return null;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0019AED0 File Offset: 0x001990D0
		private bool HasGetChildItemDynamicParameters(ProviderInfo providerInfo)
		{
			Type type = providerInfo.ImplementingType;
			MethodInfo method;
			do
			{
				method = type.GetMethod("GetChildItemsDynamicParameters", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
				type = type.GetTypeInfo().BaseType;
			}
			while (method == null && type != null && type != typeof(ContainerCmdletProvider));
			return method != null;
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x0019AF2C File Offset: 0x0019912C
		private object GetChildItemsDynamicParameters(CmdletProvider providerInstance, string path, bool recurse, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.GetChildItemsDynamicParameters(path, recurse, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetChildrenDynamicParametersProviderException", SessionStateStrings.GetChildrenDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x0019AFB0 File Offset: 0x001991B0
		internal Collection<string> GetChildNames(string[] paths, ReturnContainers returnContainers, bool recurse, uint depth, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				this.GetChildNames(text, returnContainers, recurse, depth, cmdletProviderContext);
			}
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			Collection<PSObject> accumulatedObjects = cmdletProviderContext.GetAccumulatedObjects();
			Collection<string> collection = new Collection<string>();
			foreach (PSObject psobject in accumulatedObjects)
			{
				collection.Add(psobject.BaseObject as string);
			}
			return collection;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x0019B084 File Offset: 0x00199284
		internal void GetChildNames(string path, ReturnContainers returnContainers, bool recurse, uint depth, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			Collection<WildcardPattern> collection = SessionStateUtilities.CreateWildcardsFromStrings(context.Include, WildcardOptions.IgnoreCase);
			Collection<WildcardPattern> collection2 = SessionStateUtilities.CreateWildcardsFromStrings(context.Exclude, WildcardOptions.IgnoreCase);
			if (LocationGlobber.ShouldPerformGlobbing(path, context))
			{
				ProviderInfo providerInfo = null;
				CmdletProvider cmdletProvider = null;
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
				cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, cmdletProviderContext, out providerInfo, out cmdletProvider);
				if (cmdletProviderContext.Drive != null)
				{
					context.Drive = cmdletProviderContext.Drive;
				}
				bool flag = LocationGlobber.StringContainsGlobCharacters(path);
				using (IEnumerator<string> enumerator = globbedProviderPathsFromMonadPath.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (context.Stopping)
						{
							break;
						}
						if ((!flag || recurse) && this.IsItemContainer(cmdletProvider, text, context))
						{
							this.DoGetChildNamesManually(cmdletProvider, text, string.Empty, returnContainers, collection, collection2, context, recurse, depth);
						}
						else if (cmdletProvider is NavigationCmdletProvider)
						{
							string childName = this.GetChildName(cmdletProvider, text, context, false);
							bool flag2 = SessionStateUtilities.MatchesAnyWildcardPattern(childName, collection, true);
							bool flag3 = SessionStateUtilities.MatchesAnyWildcardPattern(childName, collection2, false);
							if (flag2 && !flag3)
							{
								context.WriteObject(childName);
							}
						}
						else
						{
							context.WriteObject(text);
						}
					}
					return;
				}
			}
			ProviderInfo provider = null;
			PSDriveInfo psdriveInfo = null;
			string providerPath = this.Globber.GetProviderPath(path, context, out provider, out psdriveInfo);
			ContainerCmdletProvider containerProviderInstance = this.GetContainerProviderInstance(provider);
			if (psdriveInfo != null)
			{
				context.Drive = psdriveInfo;
			}
			if (!containerProviderInstance.ItemExists(providerPath, context))
			{
				ItemNotFoundException ex = new ItemNotFoundException(providerPath, "PathNotFound", SessionStateStrings.PathNotFound);
				throw ex;
			}
			if (recurse)
			{
				this.DoGetChildNamesManually(containerProviderInstance, providerPath, string.Empty, returnContainers, collection, collection2, context, recurse, depth);
				return;
			}
			this.GetChildNames(containerProviderInstance, providerPath, returnContainers, context);
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0019B26C File Offset: 0x0019946C
		private void DoGetChildNamesManually(CmdletProvider providerInstance, string providerPath, string relativePath, ReturnContainers returnContainers, Collection<WildcardPattern> includeMatcher, Collection<WildcardPattern> excludeMatcher, CmdletProviderContext context, bool recurse, uint depth)
		{
			string path = this.MakePath(providerInstance, providerPath, relativePath, context);
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			try
			{
				this.GetChildNames(providerInstance, path, ReturnContainers.ReturnMatchingContainers, cmdletProviderContext);
				Collection<PSObject> accumulatedObjects = cmdletProviderContext.GetAccumulatedObjects();
				foreach (PSObject psobject in accumulatedObjects)
				{
					if (context.Stopping)
					{
						return;
					}
					string text = psobject.BaseObject as string;
					if (text != null)
					{
						bool flag = SessionStateUtilities.MatchesAnyWildcardPattern(text, includeMatcher, true);
						if (flag && !SessionStateUtilities.MatchesAnyWildcardPattern(text, excludeMatcher, false))
						{
							string obj = this.MakePath(providerInstance, relativePath, text, context);
							context.WriteObject(obj);
						}
					}
				}
				if (recurse && depth > 0U)
				{
					this.GetChildNames(providerInstance, path, ReturnContainers.ReturnAllContainers, cmdletProviderContext);
					accumulatedObjects = cmdletProviderContext.GetAccumulatedObjects();
					foreach (PSObject psobject2 in accumulatedObjects)
					{
						if (context.Stopping)
						{
							break;
						}
						string text2 = psobject2.BaseObject as string;
						if (text2 != null)
						{
							string text3 = this.MakePath(providerInstance, relativePath, text2, context);
							string path2 = this.MakePath(providerInstance, providerPath, text3, context);
							if (this.IsItemContainer(providerInstance, path2, context))
							{
								this.DoGetChildNamesManually(providerInstance, providerPath, text3, returnContainers, includeMatcher, excludeMatcher, context, true, depth - 1U);
							}
						}
					}
				}
			}
			finally
			{
				cmdletProviderContext.RemoveStopReferral();
			}
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0019B418 File Offset: 0x00199618
		private void GetChildNames(CmdletProvider providerInstance, string path, ReturnContainers returnContainers, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				containerProviderInstance.GetChildNames(path, returnContainers, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetChildNamesProviderException", SessionStateStrings.GetChildNamesProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x0019B498 File Offset: 0x00199698
		internal object GetChildNamesDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider cmdletProvider = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> collection = null;
			try
			{
				collection = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out cmdletProvider);
			}
			catch (ItemNotFoundException)
			{
				if (cmdletProvider == null)
				{
					throw;
				}
			}
			object result = null;
			if (collection != null && collection.Count > 0)
			{
				result = this.GetChildNamesDynamicParameters(cmdletProvider, collection[0], cmdletProviderContext);
			}
			else if (cmdletProvider != null)
			{
				PSDriveInfo psdriveInfo = null;
				string providerPath = this.Globber.GetProviderPath(path, context, out providerInfo, out psdriveInfo);
				if (providerPath != null)
				{
					result = this.GetChildNamesDynamicParameters(cmdletProvider, providerPath, cmdletProviderContext);
				}
			}
			return result;
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0019B544 File Offset: 0x00199744
		private object GetChildNamesDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.GetChildNamesDynamicParameters(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetChildNamesDynamicParametersProviderException", SessionStateStrings.GetChildNamesDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0019B5C4 File Offset: 0x001997C4
		internal Collection<PSObject> RenameItem(string path, string newName, bool force)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			this.RenameItem(path, newName, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x0019B60C File Offset: 0x0019980C
		internal void RenameItem(string path, string newName, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, context, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count == 1)
			{
				this.RenameItem(providerInstance, globbedProviderPathsFromMonadPath[0], newName, context);
				return;
			}
			ArgumentException exception = PSTraceSource.NewArgumentException("path", SessionStateStrings.RenameMultipleItemError, new object[0]);
			context.WriteError(new ErrorRecord(exception, "RenameMultipleItemError", ErrorCategory.InvalidArgument, globbedProviderPathsFromMonadPath));
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x0019B684 File Offset: 0x00199884
		private void RenameItem(CmdletProvider providerInstance, string path, string newName, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				containerProviderInstance.RenameItem(path, newName, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RenameItemProviderException", SessionStateStrings.RenameItemProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x0019B704 File Offset: 0x00199904
		internal object RenameItemDynamicParameters(string path, string newName, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.RenameItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], newName, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x0019B760 File Offset: 0x00199960
		private object RenameItemDynamicParameters(CmdletProvider providerInstance, string path, string newName, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.RenameItemDynamicParameters(path, newName, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RenameItemDynamicParametersProviderException", SessionStateStrings.RenameItemDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x0019B7E4 File Offset: 0x001999E4
		internal Collection<PSObject> NewItem(string[] paths, string name, string type, object content, bool force)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			this.NewItem(paths, name, type, content, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x0019B830 File Offset: 0x00199A30
		internal void NewItem(string[] paths, string name, string type, object content, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo provider = null;
				CmdletProvider cmdletProvider = null;
				Collection<string> collection = new Collection<string>();
				if (string.IsNullOrEmpty(name))
				{
					PSDriveInfo psdriveInfo;
					string providerPath = this.Globber.GetProviderPath(text, context, out provider, out psdriveInfo);
					cmdletProvider = this.GetProviderInstance(provider);
					collection.Add(providerPath);
				}
				else
				{
					collection = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, true, context, out provider, out cmdletProvider);
				}
				foreach (string text2 in collection)
				{
					string path = text2;
					if (!string.IsNullOrEmpty(name))
					{
						path = this.MakePath(cmdletProvider, text2, name, context);
					}
					if (context.ExecutionContext.HasRunspaceEverUsedConstrainedLanguageMode && cmdletProvider is FunctionProvider && string.Equals(type, "Directory", StringComparison.OrdinalIgnoreCase))
					{
						throw PSTraceSource.NewNotSupportedException(SessionStateStrings.DriveCmdletProvider_NotSupported, new object[0]);
					}
					bool flag = false;
					if (type != null)
					{
						WildcardPattern wildcardPattern = new WildcardPattern(type + "*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
						if (wildcardPattern.IsMatch("symboliclink") || wildcardPattern.IsMatch("junction") || wildcardPattern.IsMatch("hardlink"))
						{
							flag = true;
						}
					}
					if (flag)
					{
						if (content == null)
						{
							throw PSTraceSource.NewArgumentNullException(SessionStateStrings.NewItemValueNotSpecified, text, new object[0]);
						}
						string text3 = content.ToString();
						if (string.IsNullOrEmpty(text3))
						{
							throw PSTraceSource.NewArgumentNullException(SessionStateStrings.PathNotFound, text3, new object[0]);
						}
						ProviderInfo providerInfo = null;
						CmdletProvider cmdletProvider2 = null;
						Collection<string> collection2 = new Collection<string>();
						collection2 = this.Globber.GetGlobbedProviderPathsFromMonadPath(text3, false, context, out providerInfo, out cmdletProvider2);
						if (string.Compare(providerInfo.Name, "filesystem", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw PSTraceSource.NewNotSupportedException(SessionStateStrings.MustBeFileSystemPath, new object[0]);
						}
						if (collection2.Count > 1)
						{
							throw PSTraceSource.NewInvalidOperationException(SessionStateStrings.PathResolvedToMultiple, new object[]
							{
								text3
							});
						}
						if (collection2.Count == 0)
						{
							throw PSTraceSource.NewInvalidOperationException(SessionStateStrings.PathNotFound, new object[]
							{
								text3
							});
						}
						content = collection2[0];
					}
					this.NewItemPrivate(cmdletProvider, path, type, content, context);
				}
			}
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x0019BA98 File Offset: 0x00199C98
		private void NewItemPrivate(CmdletProvider providerInstance, string path, string type, object content, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				containerProviderInstance.NewItem(path, type, content, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewItemProviderException", SessionStateStrings.NewItemProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x0019BB18 File Offset: 0x00199D18
		internal object NewItemDynamicParameters(string path, string type, object newItemValue, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.NewItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], type, newItemValue, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x0019BB78 File Offset: 0x00199D78
		private object NewItemDynamicParameters(CmdletProvider providerInstance, string path, string type, object newItemValue, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.NewItemDynamicParameters(path, type, newItemValue, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewItemDynamicParametersProviderException", SessionStateStrings.NewItemDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x0019BBFC File Offset: 0x00199DFC
		internal bool HasChildItems(string path, bool force, bool literalPath)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			bool flag = this.HasChildItems(path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x0019BC68 File Offset: 0x00199E68
		internal bool HasChildItems(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, false, context, out providerInfo, out providerInstance);
			bool flag = false;
			foreach (string path2 in globbedProviderPathsFromMonadPath)
			{
				flag = this.HasChildItems(providerInstance, path2, context);
				if (flag)
				{
					break;
				}
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x0019BD08 File Offset: 0x00199F08
		internal bool HasChildItems(string providerId, string path)
		{
			if (string.IsNullOrEmpty(providerId))
			{
				throw PSTraceSource.NewArgumentException("providerId");
			}
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			bool flag = this.HasChildItems(providerId, path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x0019BD78 File Offset: 0x00199F78
		internal bool HasChildItems(string providerId, string path, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = this.GetContainerProviderInstance(providerId);
			bool flag = this.HasChildItems(containerProviderInstance, path, context);
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x0019BDB8 File Offset: 0x00199FB8
		private bool HasChildItems(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			bool result = false;
			try
			{
				result = containerProviderInstance.HasChildItems(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("HasChildItemsProviderException", SessionStateStrings.HasChildItemsProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x0019BE38 File Offset: 0x0019A038
		internal Collection<PSObject> CopyItem(string[] paths, string copyPath, bool recurse, CopyContainers copyContainers, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (copyPath == null)
			{
				copyPath = string.Empty;
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.CopyItem(paths, copyPath, recurse, copyContainers, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x0019BE98 File Offset: 0x0019A098
		internal void CopyItem(string[] paths, string copyPath, bool recurse, CopyContainers copyContainers, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (copyPath == null)
			{
				copyPath = string.Empty;
			}
			PSDriveInfo psdriveInfo = null;
			ProviderInfo providerInfo = null;
			CopyItemDynamicParameters copyItemDynamicParameters = context.DynamicParameters as CopyItemDynamicParameters;
			bool flag = false;
			bool flag2 = false;
			PSSession session = null;
			if (copyItemDynamicParameters != null)
			{
				if (copyItemDynamicParameters.FromSession != null)
				{
					flag2 = true;
					session = copyItemDynamicParameters.FromSession;
				}
				if (copyItemDynamicParameters.ToSession != null)
				{
					flag = true;
					session = copyItemDynamicParameters.ToSession;
				}
			}
			if (flag2 && flag)
			{
				context.WriteError(new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemFromSessionToSession, new object[]
				{
					"FromSession",
					"ToSession"
				})), "InvalidInput", ErrorCategory.InvalidArgument, copyItemDynamicParameters));
				return;
			}
			if ((flag2 || flag) && !this.isValidSession(session, context))
			{
				return;
			}
			string text;
			if (!flag)
			{
				text = this.Globber.GetProviderPath(copyPath, context, out providerInfo, out psdriveInfo);
			}
			else
			{
				text = copyPath;
				if (string.IsNullOrEmpty(text))
				{
					context.WriteError(new ErrorRecord(new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemRemotelyPathIsNullOrEmpty, new object[]
					{
						"Destination"
					})), "CopyItemRemoteDestinationIsNullOrEmpty", ErrorCategory.InvalidArgument, text));
					return;
				}
				if (this.ValidateRemotePathAndGetRoot(text, session, context, false) == null)
				{
					return;
				}
			}
			SessionStateInternal.tracer.WriteLine("providerDestinationPath = {0}", new object[]
			{
				text
			});
			ProviderInfo providerInfo2 = null;
			CmdletProvider providerInstance = null;
			foreach (string text2 in paths)
			{
				if (text2 == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> collection;
				if (flag2)
				{
					string text3 = this.ValidateRemotePathAndGetRoot(text2, session, context, true);
					if (text3 == null)
					{
						break;
					}
					this.Globber.GetGlobbedProviderPathsFromMonadPath(text3, true, context, out providerInfo2, out providerInstance);
					collection = new Collection<string>();
					collection.Add(text2);
				}
				else
				{
					collection = this.Globber.GetGlobbedProviderPathsFromMonadPath(text2, false, context, out providerInfo2, out providerInstance);
				}
				if (!flag2 && !flag && providerInfo2 != providerInfo)
				{
					ArgumentException exception = PSTraceSource.NewArgumentException("path", SessionStateStrings.CopyItemSourceAndDestinationNotSameProvider, new object[0]);
					context.WriteError(new ErrorRecord(exception, "CopyItemSourceAndDestinationNotSameProvider", ErrorCategory.InvalidArgument, collection));
					break;
				}
				bool flag3 = false;
				if (!flag)
				{
					flag3 = this.IsItemContainer(providerInstance, text, context);
					SessionStateInternal.tracer.WriteLine("destinationIsContainer = {0}", new object[]
					{
						flag3
					});
				}
				foreach (string text4 in collection)
				{
					if (context.Stopping)
					{
						return;
					}
					if (flag2 || flag)
					{
						this.CopyItem(providerInstance, text4, text, recurse, context);
					}
					else
					{
						bool flag4 = this.IsItemContainer(providerInstance, text4, context);
						SessionStateInternal.tracer.WriteLine("sourcIsContainer = {0}", new object[]
						{
							flag4
						});
						if (flag4)
						{
							if (flag3)
							{
								if (!recurse && copyContainers == CopyContainers.CopyChildrenOfTargetContainer)
								{
									Exception exception2 = PSTraceSource.NewArgumentException("path", SessionStateStrings.CopyContainerToContainerWithoutRecurseOrContainer, new object[0]);
									context.WriteError(new ErrorRecord(exception2, "CopyContainerToContainerWithoutRecurseOrContainer", ErrorCategory.InvalidArgument, text4));
								}
								else if (recurse && copyContainers == CopyContainers.CopyChildrenOfTargetContainer)
								{
									this.CopyRecurseToSingleContainer(providerInstance, text4, text, context);
								}
								else
								{
									this.CopyItem(providerInstance, text4, text, recurse, context);
								}
							}
							else if (this.ItemExists(providerInstance, text, context))
							{
								Exception exception3 = PSTraceSource.NewArgumentException("path", SessionStateStrings.CopyContainerItemToLeafError, new object[0]);
								context.WriteError(new ErrorRecord(exception3, "CopyContainerItemToLeafError", ErrorCategory.InvalidArgument, text4));
							}
							else
							{
								this.CopyItem(providerInstance, text4, text, recurse, context);
							}
						}
						else
						{
							this.CopyItem(providerInstance, text4, text, recurse, context);
						}
					}
				}
			}
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x0019C26C File Offset: 0x0019A46C
		private void CopyItem(CmdletProvider providerInstance, string path, string copyPath, bool recurse, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			try
			{
				containerProviderInstance.CopyItem(path, copyPath, recurse, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("CopyItemProviderException", SessionStateStrings.CopyItemProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x0019C2EC File Offset: 0x0019A4EC
		private void CopyRecurseToSingleContainer(CmdletProvider providerInstance, string sourcePath, string destinationPath, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			Collection<string> childNames = this.GetChildNames(new string[]
			{
				sourcePath
			}, ReturnContainers.ReturnMatchingContainers, true, uint.MaxValue, false, false);
			foreach (string child in childNames)
			{
				if (context.Stopping)
				{
					break;
				}
				string path = this.MakePath(providerInstance.ProviderInfo, sourcePath, child, context);
				this.CopyItem(containerProviderInstance, path, destinationPath, false, context);
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x0019C37C File Offset: 0x0019A57C
		internal object CopyItemDynamicParameters(string path, string destination, bool recurse, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, !this.RemoteCopyOperation(context), cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.CopyItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], destination, recurse, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x0019C3E8 File Offset: 0x0019A5E8
		private object CopyItemDynamicParameters(CmdletProvider providerInstance, string path, string destination, bool recurse, CmdletProviderContext context)
		{
			ContainerCmdletProvider containerProviderInstance = SessionStateInternal.GetContainerProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = containerProviderInstance.CopyItemDynamicParameters(path, destination, recurse, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("CopyItemDynamicParametersProviderException", SessionStateStrings.CopyItemDynamicParametersProviderException, containerProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x0019C46C File Offset: 0x0019A66C
		private string ValidateRemotePathAndGetRoot(string path, PSSession session, CmdletProviderContext context, bool sourceIsRemote)
		{
			string result;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = session.Runspace;
				string script = "\r\n                    # Return hashtable in the following format: \r\n                    #   Exists - boolean to keep track if the given path exists\r\n                    #   Root - the root for the given path. If wildcards are used, it returns the first drive root.\r\n                    #   IsAbsolute - boolean to keep track of whether the given path is absolute\r\n                    param ([string]$path, [switch]$sourceIsRemote)\r\n\r\n                    $result = @{\r\n                        Exists = $null\r\n                        Root = $null\r\n                        IsAbsolute = $null\r\n                    }\r\n\r\n                    # Validate if the path is absolute\r\n                    $result['IsAbsolute'] = (Split-Path $path -IsAbsolute)\r\n                    if (-not $result['IsAbsolute'])\r\n                    {\r\n                        return $result\r\n                    }\r\n\r\n\r\n                    # Check if the given path exists.\r\n                    $result['Exists'] = (Test-Path $path)\r\n\r\n                    # If $path is a remote source, and it does not exist, return.\r\n                    if ($sourceIsRemote -and (-not $result['Exists']))\r\n                    {\r\n                        return $result\r\n                    }\r\n\r\n                    # If the path does not exist, check if we can find its root.\r\n                    if (-not (Test-Path $path))\r\n                    {\r\n                        $possibleRoot = $null\r\n\r\n                        try\r\n                        {\r\n                            $possibleRoot = [System.IO.Path]::GetPathRoot($path)\r\n                        }\r\n                        # Catch everything and ignore the error.\r\n                        catch {}\r\n\r\n                        if (-not $possibleRoot)\r\n                        {\r\n                            return $result\r\n                        }\r\n\r\n                        # Now use this path to find its root.\r\n                        $path = $possibleRoot\r\n                    }\r\n                    \r\n                    # Get the root path using Get-Item\r\n                    $item = Get-Item $path -ea SilentlyContinue\r\n                    if ($item.PSProvider.Name -eq 'FileSystem')\r\n                    {\r\n                        $result['Root'] = $item.PSDrive.Root | Select-Object -First 1\r\n                        return $result\r\n                    }\r\n    \r\n                    # If this fails, try to get them via Get-PSDrive\r\n                    $fileSystemDrives = @(Get-PSDrive -PSProvider FileSystem -ea SilentlyContinue)\r\n        \r\n                    # If this fails, try to get them via Get-PSProvider\r\n                    if ($fileSystemDrives.Count -eq 0)\r\n                    {\r\n                        $fileSystemDrives = @((Get-PSProvider -PSProvider FileSystem -ea SilentlyContinue).Drives)\r\n                    }\r\n\r\n                    foreach ($drive in  $fileSystemDrives)\r\n                    {\r\n                        if ($path.StartsWith($drive.Root))\r\n                        {\r\n                            $result['Root'] = $drive.Root | Select-Object -First 1\r\n                            break\r\n                        }\r\n                    }\r\n                    return $result\r\n                ";
				powerShell.AddScript(script);
				powerShell.AddParameter("path", path);
				if (sourceIsRemote)
				{
					powerShell.AddParameter("sourceIsRemote", true);
				}
				Hashtable hashtable = SafeInvokeCommand.Invoke(powerShell, null, context);
				if (hashtable == null)
				{
					context.WriteError(new ErrorRecord(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemValidateRemotePath, new object[]
					{
						path
					})), "FailedToValidateRemotePath", ErrorCategory.InvalidOperation, path));
					result = null;
				}
				else if (hashtable["IsAbsolute"] != null && !(bool)hashtable["IsAbsolute"])
				{
					context.WriteError(new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemRemotelyPathIsNotAbsolute, new object[]
					{
						path
					})), "RemotePathIsNotAbsolute", ErrorCategory.InvalidArgument, path));
					result = null;
				}
				else
				{
					bool flag = false;
					string text = null;
					if (hashtable["Exists"] != null)
					{
						flag = (bool)hashtable["Exists"];
					}
					if (hashtable["Root"] != null)
					{
						text = (string)hashtable["Root"];
					}
					bool flag2 = sourceIsRemote && !flag;
					bool flag3 = text == null;
					if (flag2 || flag3)
					{
						context.WriteError(new ErrorRecord(new ArgumentException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.PathNotFound, new object[]
						{
							path
						})), "RemotePathNotFound", ErrorCategory.InvalidArgument, path));
						result = null;
					}
					else
					{
						result = text;
					}
				}
			}
			return result;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x0019C62C File Offset: 0x0019A82C
		private bool isValidSession(PSSession session, CmdletProviderContext context)
		{
			if (session.Availability != RunspaceAvailability.Available)
			{
				context.WriteError(new ErrorRecord(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemSessionProperties, new object[]
				{
					"Availability",
					session.Availability
				})), "SessionIsNotAvailable", ErrorCategory.InvalidOperation, session.Availability));
				return false;
			}
			if (session.Runspace.SessionStateProxy.LanguageMode == PSLanguageMode.ConstrainedLanguage || session.Runspace.SessionStateProxy.LanguageMode == PSLanguageMode.NoLanguage)
			{
				context.WriteError(new ErrorRecord(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SessionStateStrings.CopyItemSessionProperties, new object[]
				{
					"LanguageMode",
					session.Runspace.SessionStateProxy.LanguageMode
				})), "SessionIsNotInFullLanguageMode", ErrorCategory.InvalidOperation, session.Availability));
				return false;
			}
			return true;
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x0019C714 File Offset: 0x0019A914
		private bool RemoteCopyOperation(CmdletProviderContext context)
		{
			CopyItemDynamicParameters copyItemDynamicParameters = context.DynamicParameters as CopyItemDynamicParameters;
			return copyItemDynamicParameters != null && (copyItemDynamicParameters.FromSession != null || copyItemDynamicParameters.ToSession != null);
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x0019C748 File Offset: 0x0019A948
		internal Collection<IContentReader> GetContentReader(string[] paths, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			Collection<IContentReader> contentReader = this.GetContentReader(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return contentReader;
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x0019C794 File Offset: 0x0019A994
		internal Collection<IContentReader> GetContentReader(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<IContentReader> collection = new Collection<IContentReader>();
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					IContentReader contentReaderPrivate = this.GetContentReaderPrivate(providerInstance, path, context);
					if (contentReaderPrivate != null)
					{
						collection.Add(contentReaderPrivate);
					}
					context.ThrowFirstErrorOrDoNothing(true);
				}
			}
			return collection;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x0019C854 File Offset: 0x0019AA54
		private IContentReader GetContentReaderPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			IContentReader result = null;
			try
			{
				result = providerInstance.GetContentReader(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetContentReaderProviderException", SessionStateStrings.GetContentReaderProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x0019C8DC File Offset: 0x0019AADC
		internal object GetContentReaderDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.GetContentReaderDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x0019C938 File Offset: 0x0019AB38
		private object GetContentReaderDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.GetContentReaderDynamicParameters(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetContentReaderDynamicParametersProviderException", SessionStateStrings.GetContentReaderDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x0019C9C0 File Offset: 0x0019ABC0
		internal Collection<IContentWriter> GetContentWriter(string[] paths, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			Collection<IContentWriter> contentWriter = this.GetContentWriter(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return contentWriter;
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x0019CA0C File Offset: 0x0019AC0C
		internal Collection<IContentWriter> GetContentWriter(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<IContentWriter> collection = new Collection<IContentWriter>();
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, true, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					IContentWriter contentWriterPrivate = this.GetContentWriterPrivate(providerInstance, path, context);
					if (contentWriterPrivate != null)
					{
						collection.Add(contentWriterPrivate);
					}
				}
			}
			return collection;
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x0019CAC4 File Offset: 0x0019ACC4
		private IContentWriter GetContentWriterPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			IContentWriter result = null;
			try
			{
				result = providerInstance.GetContentWriter(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetContentWriterProviderException", SessionStateStrings.GetContentWriterProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0019CB4C File Offset: 0x0019AD4C
		internal object GetContentWriterDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.GetContentWriterDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x0019CBA8 File Offset: 0x0019ADA8
		private object GetContentWriterDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.GetContentWriterDynamicParameters(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetContentWriterDynamicParametersProviderException", SessionStateStrings.GetContentWriterDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x0019CC30 File Offset: 0x0019AE30
		internal void ClearContent(string[] paths, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.ClearContent(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0019CC78 File Offset: 0x0019AE78
		internal void ClearContent(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			foreach (string text in paths)
			{
				if (text == null)
				{
					PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.ClearContentPrivate(providerInstance, path, context);
				}
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0019CD18 File Offset: 0x0019AF18
		private void ClearContentPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			try
			{
				providerInstance.ClearContent(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearContentProviderException", SessionStateStrings.ClearContentProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x0019CD9C File Offset: 0x0019AF9C
		internal object ClearContentDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.ClearContentDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x0019CDF8 File Offset: 0x0019AFF8
		private object ClearContentDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.ClearContentDynamicParameters(path, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearContentDynamicParametersProviderException", SessionStateStrings.ClearContentDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x0019CE80 File Offset: 0x0019B080
		internal PSDriveInfo NewDrive(PSDriveInfo drive, string scopeID)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			PSDriveInfo result = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			this.NewDrive(drive, scopeID, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			Collection<PSObject> accumulatedObjects = cmdletProviderContext.GetAccumulatedObjects();
			if (accumulatedObjects != null && accumulatedObjects.Count > 0 && !accumulatedObjects[0].immediateBaseObjectIsEmpty)
			{
				result = (PSDriveInfo)accumulatedObjects[0].BaseObject;
			}
			return result;
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0019CEF4 File Offset: 0x0019B0F4
		internal void NewDrive(PSDriveInfo drive, string scopeID, CmdletProviderContext context)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (!SessionStateInternal.IsValidDriveName(drive.Name))
			{
				ArgumentException ex = PSTraceSource.NewArgumentException("drive.Name", SessionStateStrings.DriveNameIllegalCharacters, new object[0]);
				throw ex;
			}
			PSDriveInfo psdriveInfo = this.ValidateDriveWithProvider(drive, context, true);
			if (psdriveInfo == null)
			{
				return;
			}
			if (string.Compare(psdriveInfo.Name, drive.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				try
				{
					SessionStateScope scopeByID = this.currentScope;
					if (!string.IsNullOrEmpty(scopeID))
					{
						scopeByID = this.GetScopeByID(scopeID);
					}
					scopeByID.NewDrive(psdriveInfo);
				}
				catch (ArgumentException exception)
				{
					context.WriteError(new ErrorRecord(exception, "NewDriveError", ErrorCategory.InvalidArgument, psdriveInfo));
					return;
				}
				catch (SessionStateException)
				{
					throw;
				}
				if (this.ProvidersCurrentWorkingDrive[drive.Provider] == null)
				{
					this.ProvidersCurrentWorkingDrive[drive.Provider] = drive;
				}
				context.WriteObject(psdriveInfo);
				return;
			}
			ProviderInvocationException ex2 = this.NewProviderInvocationException("NewDriveProviderFailed", SessionStateStrings.NewDriveProviderFailed, drive.Provider, drive.Root, PSTraceSource.NewArgumentException("root"));
			throw ex2;
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x0019D024 File Offset: 0x0019B224
		private static bool IsValidDriveName(string name)
		{
			bool result = true;
			if (string.IsNullOrEmpty(name))
			{
				result = false;
			}
			else if (name.IndexOfAny(SessionStateInternal._charactersInvalidInDriveName) >= 0)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x0019D050 File Offset: 0x0019B250
		private string GetProviderRootFromSpecifiedRoot(string root, ProviderInfo provider)
		{
			string result = root;
			SessionState sessionState = new SessionState(this._context.TopLevelSessionState);
			ProviderInfo providerInfo = null;
			try
			{
				Collection<string> resolvedProviderPathFromPSPath = sessionState.Path.GetResolvedProviderPathFromPSPath(root, out providerInfo);
				if (resolvedProviderPathFromPSPath != null && resolvedProviderPathFromPSPath.Count == 1 && provider.NameEquals(providerInfo.FullName))
				{
					ProviderIntrinsics providerIntrinsics = new ProviderIntrinsics(this);
					if (providerIntrinsics.Item.Exists(root))
					{
						result = resolvedProviderPathFromPSPath[0];
					}
				}
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (DriveNotFoundException)
			{
			}
			catch (ProviderNotFoundException)
			{
			}
			catch (ItemNotFoundException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (ProviderInvocationException)
			{
			}
			catch (ArgumentException)
			{
			}
			return result;
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x0019D15C File Offset: 0x0019B35C
		internal object NewDriveDynamicParameters(string providerId, CmdletProviderContext context)
		{
			if (providerId == null)
			{
				return null;
			}
			DriveCmdletProvider driveProviderInstance = this.GetDriveProviderInstance(providerId);
			object result = null;
			try
			{
				result = driveProviderInstance.NewDriveDynamicParameters(context);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewDriveDynamicParametersProviderException", SessionStateStrings.NewDriveDynamicParametersProviderException, driveProviderInstance.ProviderInfo, null, e);
			}
			return result;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x0019D1B4 File Offset: 0x0019B3B4
		internal PSDriveInfo GetDrive(string name)
		{
			return this.GetDrive(name, true);
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0019D1C0 File Offset: 0x0019B3C0
		private PSDriveInfo GetDrive(string name, bool automount)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			PSDriveInfo psdriveInfo = null;
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.CurrentScope);
			int num = 0;
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				psdriveInfo = sessionStateScope.GetDrive(name);
				if (psdriveInfo != null)
				{
					if (psdriveInfo.IsAutoMounted && !this.ValidateOrRemoveAutoMountedDrive(psdriveInfo, sessionStateScope))
					{
						psdriveInfo = null;
					}
					if (psdriveInfo != null)
					{
						SessionStateInternal.tracer.WriteLine("Drive found in scope {0}", new object[]
						{
							num
						});
						break;
					}
				}
				num++;
			}
			if (psdriveInfo == null && automount)
			{
				psdriveInfo = this.AutomountBuiltInDrive(name);
			}
			if (psdriveInfo == null && this == this._context.TopLevelSessionState)
			{
				psdriveInfo = this.AutomountFileSystemDrive(name);
			}
			if (psdriveInfo == null)
			{
				DriveNotFoundException ex = new DriveNotFoundException(name, "DriveNotFound", SessionStateStrings.DriveNotFound);
				throw ex;
			}
			return psdriveInfo;
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0019D2D0 File Offset: 0x0019B4D0
		internal PSDriveInfo GetDrive(string name, string scopeID)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			PSDriveInfo psdriveInfo = null;
			if (string.IsNullOrEmpty(scopeID))
			{
				SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.CurrentScope);
				foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
				{
					psdriveInfo = sessionStateScope.GetDrive(name);
					if (psdriveInfo != null)
					{
						if (psdriveInfo.IsAutoMounted && !this.ValidateOrRemoveAutoMountedDrive(psdriveInfo, sessionStateScope))
						{
							psdriveInfo = null;
						}
						if (psdriveInfo != null)
						{
							break;
						}
					}
				}
				if (psdriveInfo == null)
				{
					psdriveInfo = this.AutomountFileSystemDrive(name);
				}
			}
			else
			{
				SessionStateScope scopeByID = this.GetScopeByID(scopeID);
				psdriveInfo = scopeByID.GetDrive(name);
				if (psdriveInfo != null)
				{
					if (psdriveInfo.IsAutoMounted && !this.ValidateOrRemoveAutoMountedDrive(psdriveInfo, scopeByID))
					{
						psdriveInfo = null;
					}
				}
				else if (scopeByID == this._globalScope)
				{
					psdriveInfo = this.AutomountFileSystemDrive(name);
				}
			}
			return psdriveInfo;
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0019D3BC File Offset: 0x0019B5BC
		private PSDriveInfo AutomountFileSystemDrive(string name)
		{
			PSDriveInfo result = null;
			if (name.Length == 1)
			{
				try
				{
					DriveInfo systemDriveInfo = new DriveInfo(name);
					result = this.AutomountFileSystemDrive(systemDriveInfo);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			return result;
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x0019D42C File Offset: 0x0019B62C
		private PSDriveInfo AutomountFileSystemDrive(DriveInfo systemDriveInfo)
		{
			PSDriveInfo psdriveInfo = null;
			if (!this.IsProviderLoaded(this.ExecutionContext.ProviderNames.FileSystem))
			{
				SessionStateInternal.tracer.WriteLine("The {0} provider is not loaded", new object[]
				{
					this.ExecutionContext.ProviderNames.FileSystem
				});
				return psdriveInfo;
			}
			try
			{
				DriveCmdletProvider driveProviderInstance = this.GetDriveProviderInstance(this.ExecutionContext.ProviderNames.FileSystem);
				if (driveProviderInstance != null)
				{
					string name = systemDriveInfo.Name.Substring(0, 1);
					string description = string.Empty;
					string displayRoot = null;
					try
					{
						description = systemDriveInfo.VolumeLabel;
					}
					catch (UnauthorizedAccessException)
					{
					}
					if (systemDriveInfo.DriveType == DriveType.Network)
					{
						try
						{
							displayRoot = FileSystemProvider.GetRootPathForNetworkDriveOrDosDevice(systemDriveInfo);
						}
						catch (Win32Exception)
						{
						}
						catch (InvalidOperationException)
						{
						}
					}
					PSDriveInfo psdriveInfo2 = new PSDriveInfo(name, driveProviderInstance.ProviderInfo, systemDriveInfo.RootDirectory.FullName, description, null, displayRoot);
					psdriveInfo2.IsAutoMounted = true;
					CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
					psdriveInfo2.DriveBeingCreated = true;
					psdriveInfo = this.ValidateDriveWithProvider(driveProviderInstance, psdriveInfo2, cmdletProviderContext, false);
					psdriveInfo2.DriveBeingCreated = false;
					if (psdriveInfo != null && !cmdletProviderContext.HasErrors())
					{
						this._globalScope.NewDrive(psdriveInfo);
					}
				}
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				MshLog.LogProviderHealthEvent(this.ExecutionContext, this.ExecutionContext.ProviderNames.FileSystem, ex, Severity.Warning);
			}
			return psdriveInfo;
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x0019D5D8 File Offset: 0x0019B7D8
		internal PSDriveInfo AutomountBuiltInDrive(string name)
		{
			SessionStateInternal.MountDefaultDrive(name, this._context);
			return this.GetDrive(name, false);
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0019D5FC File Offset: 0x0019B7FC
		internal static void MountDefaultDrive(string name, ExecutionContext context)
		{
			if (CommandDiscovery.GetCommandDiscoveryPreference(context, SpecialVariables.PSModuleAutoLoadingPreferenceVarPath, "PSModuleAutoLoadingPreference") == PSModuleAutoLoadingPreference.None)
			{
				return;
			}
			string text = null;
			if (string.Equals("Cert", name, StringComparison.OrdinalIgnoreCase) || string.Equals("Certificate", name, StringComparison.OrdinalIgnoreCase))
			{
				text = "Microsoft.PowerShell.Security";
			}
			else if (string.Equals("WSMan", name, StringComparison.OrdinalIgnoreCase))
			{
				text = "Microsoft.WSMan.Management";
			}
			if (!string.IsNullOrEmpty(text))
			{
				SessionStateInternal.tracer.WriteLine("Auto-mounting built-in drive: {0}", new object[]
				{
					name
				});
				CommandInfo commandInfo = new CmdletInfo("Import-Module", typeof(ImportModuleCommand), null, null, context);
				Exception ex = null;
				SessionStateInternal.tracer.WriteLine("Attempting to load module: {0}", new object[]
				{
					text
				});
				CommandDiscovery.AutoloadSpecifiedModule(text, context, commandInfo.Visibility, out ex);
				if (ex != null)
				{
					CommandProcessorBase.CheckForSevereException(ex);
				}
			}
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x0019D6D0 File Offset: 0x0019B8D0
		private bool ValidateOrRemoveAutoMountedDrive(PSDriveInfo drive, SessionStateScope scope)
		{
			bool flag = true;
			try
			{
				DriveInfo driveInfo = new DriveInfo(drive.Name);
				flag = (driveInfo.DriveType != DriveType.NoRootDirectory);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				flag = false;
			}
			if (!flag)
			{
				DriveCmdletProvider driveCmdletProvider = null;
				try
				{
					driveCmdletProvider = this.GetDriveProviderInstance(this.ExecutionContext.ProviderNames.FileSystem);
				}
				catch (NotSupportedException)
				{
				}
				catch (ProviderNotFoundException)
				{
				}
				if (driveCmdletProvider != null)
				{
					CmdletProviderContext context = new CmdletProviderContext(this.ExecutionContext);
					try
					{
						driveCmdletProvider.RemoveDrive(drive, context);
					}
					catch (Exception e2)
					{
						CommandProcessorBase.CheckForSevereException(e2);
					}
					scope.RemoveDrive(drive);
				}
			}
			return flag;
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x0019D7BC File Offset: 0x0019B9BC
		private bool IsAStaleVhdMountedDrive(PSDriveInfo drive)
		{
			bool result = false;
			if (drive.Provider != null && !drive.Provider.NameEquals(this.ExecutionContext.ProviderNames.FileSystem))
			{
				return false;
			}
			if (drive != null && !string.IsNullOrEmpty(drive.Name) && drive.Name.Length == 1)
			{
				try
				{
					char c = Convert.ToChar(drive.Name, CultureInfo.InvariantCulture);
					if (char.ToUpperInvariant(c) >= 'A' && char.ToUpperInvariant(c) <= 'Z')
					{
						DriveInfo driveInfo = new DriveInfo(drive.Name);
						if (driveInfo.DriveType == DriveType.NoRootDirectory)
						{
							try
							{
								if (!Utils.NativeDirectoryExists(drive.Root))
								{
									result = true;
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
				}
				catch (ArgumentException)
				{
				}
			}
			return result;
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x0019D898 File Offset: 0x0019BA98
		internal Collection<PSDriveInfo> GetDrivesForProvider(string providerId)
		{
			if (string.IsNullOrEmpty(providerId))
			{
				return this.Drives(null);
			}
			this.GetSingleProvider(providerId);
			Collection<PSDriveInfo> collection = new Collection<PSDriveInfo>();
			foreach (PSDriveInfo psdriveInfo in this.Drives(null))
			{
				if (psdriveInfo != null && psdriveInfo.Provider.NameEquals(providerId))
				{
					collection.Add(psdriveInfo);
				}
			}
			return collection;
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x0019D91C File Offset: 0x0019BB1C
		internal void RemoveDrive(string driveName, bool force, string scopeID)
		{
			if (driveName == null)
			{
				throw PSTraceSource.NewArgumentNullException("driveName");
			}
			PSDriveInfo drive = this.GetDrive(driveName, scopeID);
			if (drive == null)
			{
				DriveNotFoundException ex = new DriveNotFoundException(driveName, "DriveNotFound", SessionStateStrings.DriveNotFound);
				throw ex;
			}
			this.RemoveDrive(drive, force, scopeID);
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x0019D968 File Offset: 0x0019BB68
		internal void RemoveDrive(string driveName, bool force, string scopeID, CmdletProviderContext context)
		{
			if (driveName == null)
			{
				throw PSTraceSource.NewArgumentNullException("driveName");
			}
			PSDriveInfo drive = this.GetDrive(driveName, scopeID);
			if (drive == null)
			{
				DriveNotFoundException ex = new DriveNotFoundException(driveName, "DriveNotFound", SessionStateStrings.DriveNotFound);
				context.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
				return;
			}
			this.RemoveDrive(drive, force, scopeID, context);
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x0019D9C8 File Offset: 0x0019BBC8
		internal void RemoveDrive(PSDriveInfo drive, bool force, string scopeID)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			this.RemoveDrive(drive, force, scopeID, cmdletProviderContext);
			if (cmdletProviderContext.HasErrors() && !force)
			{
				cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			}
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0019DA10 File Offset: 0x0019BC10
		internal void RemoveDrive(PSDriveInfo drive, bool force, string scopeID, CmdletProviderContext context)
		{
			bool flag = false;
			try
			{
				flag = this.CanRemoveDrive(drive, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (ProviderInvocationException)
			{
				if (!force)
				{
					throw;
				}
			}
			if (flag || force)
			{
				if (string.IsNullOrEmpty(scopeID))
				{
					SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.CurrentScope);
					using (IEnumerator<SessionStateScope> enumerator = ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SessionStateScope sessionStateScope = enumerator.Current;
							try
							{
								PSDriveInfo drive2 = sessionStateScope.GetDrive(drive.Name);
								if (drive2 != null)
								{
									sessionStateScope.RemoveDrive(drive);
									if (this.ProvidersCurrentWorkingDrive[drive.Provider] == drive2)
									{
										this.ProvidersCurrentWorkingDrive[drive.Provider] = null;
									}
									break;
								}
							}
							catch (ArgumentException)
							{
							}
						}
						return;
					}
				}
				SessionStateScope scopeByID = this.GetScopeByID(scopeID);
				scopeByID.RemoveDrive(drive);
				if (this.ProvidersCurrentWorkingDrive[drive.Provider] == drive)
				{
					this.ProvidersCurrentWorkingDrive[drive.Provider] = null;
					return;
				}
			}
			else
			{
				PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(SessionStateStrings.DriveRemovalPreventedByProvider, new object[]
				{
					drive.Name,
					drive.Provider
				});
				context.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
			}
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x0019DBA0 File Offset: 0x0019BDA0
		private bool CanRemoveDrive(PSDriveInfo drive, CmdletProviderContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			SessionStateInternal.tracer.WriteLine("Drive name = {0}", new object[]
			{
				drive.Name
			});
			context.Drive = drive;
			DriveCmdletProvider driveProviderInstance = this.GetDriveProviderInstance(drive.Provider);
			bool flag = false;
			PSDriveInfo psdriveInfo = null;
			try
			{
				psdriveInfo = driveProviderInstance.RemoveDrive(drive, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemoveDriveProviderException", SessionStateStrings.RemoveDriveProviderException, driveProviderInstance.ProviderInfo, null, e);
			}
			if (psdriveInfo != null && string.Compare(psdriveInfo.Name, drive.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				flag = true;
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x0019DCB8 File Offset: 0x0019BEB8
		internal Collection<PSDriveInfo> Drives(string scope)
		{
			Dictionary<string, PSDriveInfo> dictionary = new Dictionary<string, PSDriveInfo>();
			SessionStateScope scopeByID = this.currentScope;
			if (!string.IsNullOrEmpty(scope))
			{
				scopeByID = this.GetScopeByID(scope);
			}
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(scopeByID);
			DriveInfo[] drives = DriveInfo.GetDrives();
			Collection<string> collection = new Collection<string>();
			foreach (DriveInfo driveInfo in drives)
			{
				collection.Add(driveInfo.Name.Substring(0, 1));
			}
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				foreach (PSDriveInfo psdriveInfo in sessionStateScope.Drives)
				{
					if (psdriveInfo != null)
					{
						bool flag = true;
						if (psdriveInfo.IsAutoMounted || this.IsAStaleVhdMountedDrive(psdriveInfo))
						{
							flag = this.ValidateOrRemoveAutoMountedDrive(psdriveInfo, sessionStateScope);
						}
						if (psdriveInfo.Name.Length == 1 && !collection.Contains(psdriveInfo.Name))
						{
							dictionary.Remove(psdriveInfo.Name);
						}
						if (flag && !dictionary.ContainsKey(psdriveInfo.Name))
						{
							dictionary[psdriveInfo.Name] = psdriveInfo;
						}
					}
				}
				if (scope != null && scope.Length > 0)
				{
					break;
				}
			}
			try
			{
				foreach (DriveInfo driveInfo2 in drives)
				{
					if (driveInfo2 != null)
					{
						string key = driveInfo2.Name.Substring(0, 1);
						if (!dictionary.ContainsKey(key))
						{
							PSDriveInfo psdriveInfo2 = this.AutomountFileSystemDrive(driveInfo2);
							if (psdriveInfo2 != null)
							{
								dictionary[psdriveInfo2.Name] = psdriveInfo2;
							}
						}
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			Collection<PSDriveInfo> collection2 = new Collection<PSDriveInfo>();
			foreach (PSDriveInfo item in dictionary.Values)
			{
				collection2.Add(item);
			}
			return collection2;
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x0019DF00 File Offset: 0x0019C100
		// (set) Token: 0x06004E77 RID: 20087 RVA: 0x0019DF27 File Offset: 0x0019C127
		internal PSDriveInfo CurrentDrive
		{
			get
			{
				if (this != this._context.TopLevelSessionState)
				{
					return this._context.TopLevelSessionState.CurrentDrive;
				}
				return this.currentDrive;
			}
			set
			{
				if (this != this._context.TopLevelSessionState)
				{
					this._context.TopLevelSessionState.CurrentDrive = value;
					return;
				}
				this.currentDrive = value;
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x0019DF50 File Offset: 0x0019C150
		internal Collection<PSObject> NewProperty(string[] paths, string property, string type, object value, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("property");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.NewProperty(paths, property, type, value, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x0019DFB4 File Offset: 0x0019C1B4
		internal void NewProperty(string[] paths, string property, string type, object value, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("property");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.NewProperty(providerInstance, path, property, type, value, context);
				}
			}
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0019E068 File Offset: 0x0019C268
		private void NewProperty(CmdletProvider providerInstance, string path, string property, string type, object value, CmdletProviderContext context)
		{
			try
			{
				providerInstance.NewProperty(path, property, type, value, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewPropertyProviderException", SessionStateStrings.NewPropertyProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x0019E0F4 File Offset: 0x0019C2F4
		internal object NewPropertyDynamicParameters(string path, string propertyName, string type, object value, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.NewPropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], propertyName, type, value, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x0019E154 File Offset: 0x0019C354
		private object NewPropertyDynamicParameters(CmdletProvider providerInstance, string path, string propertyName, string type, object value, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.NewPropertyDynamicParameters(path, propertyName, type, value, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("NewPropertyDynamicParametersProviderException", SessionStateStrings.NewPropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0019E1E4 File Offset: 0x0019C3E4
		internal void RemoveProperty(string[] paths, string property, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("property");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.RemoveProperty(paths, property, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0019E23C File Offset: 0x0019C43C
		internal void RemoveProperty(string[] paths, string property, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("property");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.RemoveProperty(providerInstance, path, property, context);
				}
			}
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0019E2EC File Offset: 0x0019C4EC
		private void RemoveProperty(CmdletProvider providerInstance, string path, string property, CmdletProviderContext context)
		{
			try
			{
				providerInstance.RemoveProperty(path, property, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemovePropertyProviderException", SessionStateStrings.RemovePropertyProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0019E374 File Offset: 0x0019C574
		internal object RemovePropertyDynamicParameters(string path, string propertyName, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.RemovePropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], propertyName, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0019E3D0 File Offset: 0x0019C5D0
		private object RemovePropertyDynamicParameters(CmdletProvider providerInstance, string path, string propertyName, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.RemovePropertyDynamicParameters(path, propertyName, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RemovePropertyDynamicParametersProviderException", SessionStateStrings.RemovePropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0019E45C File Offset: 0x0019C65C
		internal Collection<PSObject> CopyProperty(string[] sourcePaths, string sourceProperty, string destinationPath, string destinationProperty, bool force, bool literalPath)
		{
			if (sourcePaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePaths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.CopyProperty(sourcePaths, sourceProperty, destinationPath, destinationProperty, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0019E4E0 File Offset: 0x0019C6E0
		internal void CopyProperty(string[] sourcePaths, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			if (sourcePaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePaths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			foreach (string text in sourcePaths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("sourcePaths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath.Count > 0)
				{
					Collection<string> include = context.Include;
					Collection<string> exclude = context.Exclude;
					string filter = context.Filter;
					context.SetFilters(new Collection<string>(), new Collection<string>(), null);
					Collection<string> globbedProviderPathsFromMonadPath2 = this.Globber.GetGlobbedProviderPathsFromMonadPath(destinationPath, false, context, out providerInfo, out providerInstance);
					context.SetFilters(include, exclude, filter);
					foreach (string sourcePath in globbedProviderPathsFromMonadPath)
					{
						foreach (string destinationPath2 in globbedProviderPathsFromMonadPath2)
						{
							this.CopyProperty(providerInstance, sourcePath, sourceProperty, destinationPath2, destinationProperty, context);
						}
					}
				}
			}
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x0019E64C File Offset: 0x0019C84C
		private void CopyProperty(CmdletProvider providerInstance, string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			try
			{
				providerInstance.CopyProperty(sourcePath, sourceProperty, destinationPath, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("CopyPropertyProviderException", SessionStateStrings.CopyPropertyProviderException, providerInstance.ProviderInfo, sourcePath, e);
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0019E6D8 File Offset: 0x0019C8D8
		internal object CopyPropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.CopyPropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], sourceProperty, destinationPath, destinationProperty, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0019E738 File Offset: 0x0019C938
		private object CopyPropertyDynamicParameters(CmdletProvider providerInstance, string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.CopyPropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("CopyPropertyDynamicParametersProviderException", SessionStateStrings.CopyPropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0019E7C8 File Offset: 0x0019C9C8
		internal Collection<PSObject> MoveProperty(string[] sourcePaths, string sourceProperty, string destinationPath, string destinationProperty, bool force, bool literalPath)
		{
			if (sourcePaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePaths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.MoveProperty(sourcePaths, sourceProperty, destinationPath, destinationProperty, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0019E84C File Offset: 0x0019CA4C
		internal void MoveProperty(string[] sourcePaths, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			if (sourcePaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePaths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationPath");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(destinationPath, false, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 1)
			{
				ArgumentException ex = PSTraceSource.NewArgumentException("destinationPath", SessionStateStrings.MovePropertyDestinationResolveToSingle, new object[0]);
				context.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, globbedProviderPathsFromMonadPath));
				return;
			}
			foreach (string text in sourcePaths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("sourcePaths");
				}
				Collection<string> globbedProviderPathsFromMonadPath2 = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string sourcePath in globbedProviderPathsFromMonadPath2)
				{
					this.MoveProperty(providerInstance, sourcePath, sourceProperty, globbedProviderPathsFromMonadPath[0], destinationProperty, context);
				}
			}
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0019E994 File Offset: 0x0019CB94
		private void MoveProperty(CmdletProvider providerInstance, string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			try
			{
				providerInstance.MoveProperty(sourcePath, sourceProperty, destinationPath, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("MovePropertyProviderException", SessionStateStrings.MovePropertyProviderException, providerInstance.ProviderInfo, sourcePath, e);
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x0019EA20 File Offset: 0x0019CC20
		internal object MovePropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.MovePropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], sourceProperty, destinationPath, destinationProperty, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0019EA80 File Offset: 0x0019CC80
		private object MovePropertyDynamicParameters(CmdletProvider providerInstance, string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.MovePropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("MovePropertyDynamicParametersProviderException", SessionStateStrings.MovePropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x0019EB10 File Offset: 0x0019CD10
		internal Collection<PSObject> RenameProperty(string[] sourcePaths, string sourceProperty, string destinationProperty, bool force, bool literalPath)
		{
			if (sourcePaths == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourcePaths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.RenameProperty(sourcePaths, sourceProperty, destinationProperty, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x0019EB80 File Offset: 0x0019CD80
		internal void RenameProperty(string[] paths, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (sourceProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("sourceProperty");
			}
			if (destinationProperty == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationProperty");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string sourcePath in globbedProviderPathsFromMonadPath)
				{
					this.RenameProperty(providerInstance, sourcePath, sourceProperty, destinationProperty, context);
				}
			}
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x0019EC40 File Offset: 0x0019CE40
		private void RenameProperty(CmdletProvider providerInstance, string sourcePath, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			try
			{
				providerInstance.RenameProperty(sourcePath, sourceProperty, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RenamePropertyProviderException", SessionStateStrings.RenamePropertyProviderException, providerInstance.ProviderInfo, sourcePath, e);
			}
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0019ECC8 File Offset: 0x0019CEC8
		internal object RenamePropertyDynamicParameters(string path, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.RenamePropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], sourceProperty, destinationProperty, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x0019ED28 File Offset: 0x0019CF28
		private object RenamePropertyDynamicParameters(CmdletProvider providerInstance, string path, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.RenamePropertyDynamicParameters(path, sourceProperty, destinationProperty, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("RenamePropertyDynamicParametersProviderException", SessionStateStrings.RenamePropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x0019EDB4 File Offset: 0x0019CFB4
		internal void AddSessionStateEntry(SessionStateFunctionEntry entry)
		{
			ScriptBlock scriptBlock = entry.ScriptBlock.Clone(false);
			if (scriptBlock.IsSingleFunctionDefinition(entry.Name))
			{
				throw PSTraceSource.NewArgumentException("entry");
			}
			FunctionInfo functionInfo = this.SetFunction(entry.Name, scriptBlock, null, entry.Options, false, CommandOrigin.Internal, this.ExecutionContext, entry.HelpFile, true);
			functionInfo.Visibility = entry.Visibility;
			functionInfo.SetModule(entry.Module);
			functionInfo.ScriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x0019EE34 File Offset: 0x0019D034
		internal void AddSessionStateEntry(InitialSessionState initialSessionState, SessionStateWorkflowEntry entry)
		{
			IAstToWorkflowConverter astToWorkflowConverterAndEnsureWorkflowModuleLoaded = Utils.GetAstToWorkflowConverterAndEnsureWorkflowModuleLoaded(null);
			WorkflowInfo workflowInfo = entry.WorkflowInfo;
			if (workflowInfo == null)
			{
				workflowInfo = astToWorkflowConverterAndEnsureWorkflowModuleLoaded.CompileWorkflow(entry.Name, entry.Definition, initialSessionState);
			}
			WorkflowInfo workflowInfo2 = new WorkflowInfo(workflowInfo);
			workflowInfo2 = this.SetWorkflowRaw(workflowInfo2, CommandOrigin.Internal);
			workflowInfo2.Visibility = entry.Visibility;
			workflowInfo2.SetModule(entry.Module);
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x0019EE90 File Offset: 0x0019D090
		internal IDictionary GetFunctionTable()
		{
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			Dictionary<string, FunctionInfo> dictionary = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				foreach (FunctionInfo functionInfo in sessionStateScope.FunctionTable.Values)
				{
					if (!dictionary.ContainsKey(functionInfo.Name))
					{
						dictionary.Add(functionInfo.Name, functionInfo);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x0019EF4C File Offset: 0x0019D14C
		internal IDictionary<string, FunctionInfo> GetFunctionTableAtScope(string scopeID)
		{
			Dictionary<string, FunctionInfo> dictionary = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			foreach (FunctionInfo functionInfo in scopeByID.FunctionTable.Values)
			{
				FunctionInfo functionInfo2 = functionInfo;
				ScopedItemOptions options;
				if (functionInfo2 != null)
				{
					options = functionInfo2.Options;
				}
				else
				{
					options = ((FilterInfo)functionInfo).Options;
				}
				if ((options & ScopedItemOptions.Private) == ScopedItemOptions.None || scopeByID == this.currentScope)
				{
					dictionary.Add(functionInfo.Name, functionInfo);
				}
			}
			return dictionary;
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06004E95 RID: 20117 RVA: 0x0019EFEC File Offset: 0x0019D1EC
		internal List<FunctionInfo> ExportedFunctions
		{
			get
			{
				return this._exportedFunctions;
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06004E96 RID: 20118 RVA: 0x0019EFF4 File Offset: 0x0019D1F4
		internal List<WorkflowInfo> ExportedWorkflows
		{
			get
			{
				return this._exportedWorkflows;
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x0019EFFC File Offset: 0x0019D1FC
		// (set) Token: 0x06004E98 RID: 20120 RVA: 0x0019F004 File Offset: 0x0019D204
		internal bool UseExportList
		{
			get
			{
				return this._useExportList;
			}
			set
			{
				this._useExportList = value;
			}
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x0019F010 File Offset: 0x0019D210
		internal FunctionInfo GetFunction(string name, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			FunctionInfo result = null;
			FunctionLookupPath lookupPath = new FunctionLookupPath(name);
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, lookupPath, origin);
			if (functionScopeItemSearcher.MoveNext())
			{
				result = ((IEnumerator<FunctionInfo>)functionScopeItemSearcher).Current;
			}
			return result;
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x0019F052 File Offset: 0x0019D252
		internal FunctionInfo GetFunction(string name)
		{
			return this.GetFunction(name, CommandOrigin.Internal);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x0019F05C File Offset: 0x0019D25C
		internal FunctionInfo SetFunctionRaw(string name, ScriptBlock function, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (function == null)
			{
				throw PSTraceSource.NewArgumentNullException("function");
			}
			string itemName = name;
			FunctionLookupPath functionLookupPath = new FunctionLookupPath(name);
			name = functionLookupPath.UnqualifiedPath;
			if (string.IsNullOrEmpty(name))
			{
				SessionStateException ex = new SessionStateException(itemName, SessionStateCategory.Function, "ScopedFunctionMustHaveName", SessionStateStrings.ScopedFunctionMustHaveName, ErrorCategory.InvalidArgument, new object[0]);
				throw ex;
			}
			ScopedItemOptions scopedItemOptions = ScopedItemOptions.None;
			if (functionLookupPath.IsPrivate)
			{
				scopedItemOptions |= ScopedItemOptions.Private;
			}
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, functionLookupPath, origin);
			FunctionInfo result = functionScopeItemSearcher.InitialScope.SetFunction(name, function, null, scopedItemOptions, false, origin, this.ExecutionContext);
			if (function.AliasAttribute != null)
			{
				foreach (string name2 in function.AliasAttribute.AliasNames)
				{
					functionScopeItemSearcher.InitialScope.SetAliasValue(name2, name, this.ExecutionContext, false, origin);
				}
			}
			return result;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x0019F168 File Offset: 0x0019D368
		internal WorkflowInfo SetWorkflowRaw(WorkflowInfo workflowInfo, CommandOrigin origin)
		{
			string name = workflowInfo.Name;
			string text = name;
			FunctionLookupPath functionLookupPath = new FunctionLookupPath(text);
			text = functionLookupPath.UnqualifiedPath;
			if (string.IsNullOrEmpty(text))
			{
				SessionStateException ex = new SessionStateException(name, SessionStateCategory.Function, "ScopedFunctionMustHaveName", SessionStateStrings.ScopedFunctionMustHaveName, ErrorCategory.InvalidArgument, new object[0]);
				throw ex;
			}
			ScopedItemOptions scopedItemOptions = ScopedItemOptions.None;
			if (functionLookupPath.IsPrivate)
			{
				scopedItemOptions |= ScopedItemOptions.Private;
			}
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, functionLookupPath, origin);
			workflowInfo.ScriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			if (workflowInfo.Module == null && this.Module != null)
			{
				workflowInfo.SetModule(this.Module);
			}
			WorkflowInfo result = (WorkflowInfo)functionScopeItemSearcher.InitialScope.SetFunction(text, workflowInfo.ScriptBlock, null, scopedItemOptions, false, origin, this.ExecutionContext, null, (string arg1, ScriptBlock arg2, FunctionInfo arg3, ScopedItemOptions arg4, ExecutionContext arg5, string arg6) => workflowInfo);
			if (workflowInfo.ScriptBlock.AliasAttribute != null)
			{
				foreach (string name2 in workflowInfo.ScriptBlock.AliasAttribute.AliasNames)
				{
					functionScopeItemSearcher.InitialScope.SetAliasValue(name2, text, this.ExecutionContext, false, origin);
				}
			}
			return result;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x0019F2D4 File Offset: 0x0019D4D4
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin)
		{
			return this.SetFunction(name, function, originalFunction, options, force, origin, this.ExecutionContext, null);
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x0019F2F8 File Offset: 0x0019D4F8
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, string helpFile)
		{
			return this.SetFunction(name, function, originalFunction, options, force, origin, this.ExecutionContext, helpFile, false);
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x0019F320 File Offset: 0x0019D520
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, ExecutionContext context, string helpFile)
		{
			return this.SetFunction(name, function, originalFunction, options, force, origin, context, helpFile, false);
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x0019F344 File Offset: 0x0019D544
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, ScopedItemOptions options, bool force, CommandOrigin origin, ExecutionContext context, string helpFile, bool isPreValidated)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (function == null)
			{
				throw PSTraceSource.NewArgumentNullException("function");
			}
			string itemName = name;
			FunctionLookupPath functionLookupPath = new FunctionLookupPath(name);
			name = functionLookupPath.UnqualifiedPath;
			if (string.IsNullOrEmpty(name))
			{
				SessionStateException ex = new SessionStateException(itemName, SessionStateCategory.Function, "ScopedFunctionMustHaveName", SessionStateStrings.ScopedFunctionMustHaveName, ErrorCategory.InvalidArgument, new object[0]);
				throw ex;
			}
			if (functionLookupPath.IsPrivate)
			{
				options |= ScopedItemOptions.Private;
			}
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, functionLookupPath, origin);
			return functionScopeItemSearcher.InitialScope.SetFunction(name, function, originalFunction, options, force, origin, context, helpFile);
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x0019F3D8 File Offset: 0x0019D5D8
		internal FunctionInfo SetFunction(string name, ScriptBlock function, FunctionInfo originalFunction, bool force, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (function == null)
			{
				throw PSTraceSource.NewArgumentNullException("function");
			}
			string itemName = name;
			FunctionLookupPath functionLookupPath = new FunctionLookupPath(name);
			name = functionLookupPath.UnqualifiedPath;
			if (string.IsNullOrEmpty(name))
			{
				SessionStateException ex = new SessionStateException(itemName, SessionStateCategory.Function, "ScopedFunctionMustHaveName", SessionStateStrings.ScopedFunctionMustHaveName, ErrorCategory.InvalidArgument, new object[0]);
				throw ex;
			}
			ScopedItemOptions scopedItemOptions = ScopedItemOptions.None;
			if (functionLookupPath.IsPrivate)
			{
				scopedItemOptions |= ScopedItemOptions.Private;
			}
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, functionLookupPath, origin);
			SessionStateScope sessionStateScope = functionScopeItemSearcher.InitialScope;
			FunctionInfo result;
			if (functionScopeItemSearcher.MoveNext())
			{
				sessionStateScope = functionScopeItemSearcher.CurrentLookupScope;
				name = functionScopeItemSearcher.Name;
				if (functionLookupPath.IsPrivate)
				{
					FunctionInfo function2 = sessionStateScope.GetFunction(name);
					FunctionInfo functionInfo = function2;
					if (functionInfo != null)
					{
						scopedItemOptions |= functionInfo.Options;
					}
					else
					{
						scopedItemOptions |= ((FilterInfo)function2).Options;
					}
					result = sessionStateScope.SetFunction(name, function, originalFunction, scopedItemOptions, force, origin, this.ExecutionContext);
				}
				else
				{
					result = sessionStateScope.SetFunction(name, function, force, origin, this.ExecutionContext);
				}
			}
			else if (functionLookupPath.IsPrivate)
			{
				result = sessionStateScope.SetFunction(name, function, originalFunction, scopedItemOptions, force, origin, this.ExecutionContext);
			}
			else
			{
				result = sessionStateScope.SetFunction(name, function, force, origin, this.ExecutionContext);
			}
			return result;
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x0019F517 File Offset: 0x0019D717
		internal FunctionInfo SetFunction(string name, ScriptBlock function, bool force)
		{
			return this.SetFunction(name, function, null, force, CommandOrigin.Internal);
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x0019F524 File Offset: 0x0019D724
		internal void RemoveFunction(string name, bool force, CommandOrigin origin)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			SessionStateScope currentLookupScope = this.currentScope;
			FunctionLookupPath lookupPath = new FunctionLookupPath(name);
			FunctionScopeItemSearcher functionScopeItemSearcher = new FunctionScopeItemSearcher(this, lookupPath, origin);
			if (functionScopeItemSearcher.MoveNext())
			{
				currentLookupScope = functionScopeItemSearcher.CurrentLookupScope;
			}
			currentLookupScope.RemoveFunction(name, force);
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x0019F572 File Offset: 0x0019D772
		internal void RemoveFunction(string name, bool force)
		{
			this.RemoveFunction(name, force, CommandOrigin.Internal);
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x0019F580 File Offset: 0x0019D780
		internal void RemoveFunction(string name, PSModuleInfo module)
		{
			FunctionInfo function = this.GetFunction(name);
			if (function != null && function.ScriptBlock != null && function.ScriptBlock.File != null && function.ScriptBlock.File.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
			{
				this.RemoveFunction(name, true);
			}
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x0019F5D0 File Offset: 0x0019D7D0
		internal Collection<PSObject> GetItem(string[] paths, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.GetItem(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x0019F620 File Offset: 0x0019D820
		internal void GetItem(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.GetItemPrivate(providerInstance, path, context);
				}
			}
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x0019F6C0 File Offset: 0x0019D8C0
		private void GetItemPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			try
			{
				itemProviderInstance.GetItem(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetItemProviderException", SessionStateStrings.GetItemProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x0019F73C File Offset: 0x0019D93C
		internal object GetItemDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.GetItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x0019F798 File Offset: 0x0019D998
		private object GetItemDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = itemProviderInstance.GetItemDynamicParameters(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetItemDynamicParametersProviderException", SessionStateStrings.GetItemDynamicParametersProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x0019F818 File Offset: 0x0019DA18
		internal Collection<PSObject> SetItem(string[] paths, object value, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.SetItem(paths, value, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x0019F868 File Offset: 0x0019DA68
		internal void SetItem(string[] paths, object value, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, true, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath != null)
				{
					foreach (string path in globbedProviderPathsFromMonadPath)
					{
						this.SetItem(providerInstance, path, value, context);
					}
				}
			}
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x0019F90C File Offset: 0x0019DB0C
		private void SetItem(CmdletProvider providerInstance, string path, object value, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			try
			{
				itemProviderInstance.SetItem(path, value, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("SetItemProviderException", SessionStateStrings.SetItemProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x0019F98C File Offset: 0x0019DB8C
		internal object SetItemDynamicParameters(string path, object value, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.SetItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], value, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x0019F9E8 File Offset: 0x0019DBE8
		private object SetItemDynamicParameters(CmdletProvider providerInstance, string path, object value, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = itemProviderInstance.SetItemDynamicParameters(path, value, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("SetItemDynamicParametersProviderException", SessionStateStrings.SetItemDynamicParametersProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x0019FA6C File Offset: 0x0019DC6C
		internal Collection<PSObject> ClearItem(string[] paths, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.ClearItem(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x0019FABC File Offset: 0x0019DCBC
		internal void ClearItem(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath != null)
				{
					foreach (string path in globbedProviderPathsFromMonadPath)
					{
						this.ClearItemPrivate(providerInstance, path, context);
					}
				}
			}
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x0019FB60 File Offset: 0x0019DD60
		private void ClearItemPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			try
			{
				itemProviderInstance.ClearItem(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearItemProviderException", SessionStateStrings.ClearItemProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x0019FBDC File Offset: 0x0019DDDC
		internal object ClearItemDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.ClearItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x0019FC38 File Offset: 0x0019DE38
		private object ClearItemDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = itemProviderInstance.ClearItemDynamicParameters(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearItemProviderException", SessionStateStrings.ClearItemProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x0019FCB8 File Offset: 0x0019DEB8
		internal void InvokeDefaultAction(string[] paths, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.InvokeDefaultAction(paths, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x0019FCF4 File Offset: 0x0019DEF4
		internal void InvokeDefaultAction(string[] paths, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath != null)
				{
					foreach (string path in globbedProviderPathsFromMonadPath)
					{
						this.InvokeDefaultActionPrivate(providerInstance, path, context);
					}
				}
			}
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x0019FD98 File Offset: 0x0019DF98
		private void InvokeDefaultActionPrivate(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			try
			{
				itemProviderInstance.InvokeDefaultAction(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("InvokeDefaultActionProviderException", SessionStateStrings.InvokeDefaultActionProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x0019FE14 File Offset: 0x0019E014
		internal object InvokeDefaultActionDynamicParameters(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.InvokeDefaultActionDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x0019FE70 File Offset: 0x0019E070
		private object InvokeDefaultActionDynamicParameters(CmdletProvider providerInstance, string path, CmdletProviderContext context)
		{
			ItemCmdletProvider itemProviderInstance = SessionStateInternal.GetItemProviderInstance(providerInstance);
			object result = null;
			try
			{
				result = itemProviderInstance.InvokeDefaultActionDynamicParameters(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("InvokeDefaultActionDynamicParametersProviderException", SessionStateStrings.InvokeDefaultActionDynamicParametersProviderException, itemProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06004EBA RID: 20154 RVA: 0x0019FEF0 File Offset: 0x0019E0F0
		internal PathInfo CurrentLocation
		{
			get
			{
				if (this.CurrentDrive == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				PathInfo pathInfo = new PathInfo(this.CurrentDrive, this.CurrentDrive.Provider, this.CurrentDrive.CurrentLocation, new SessionState(this));
				SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
				{
					pathInfo
				});
				return pathInfo;
			}
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x0019FF58 File Offset: 0x0019E158
		internal PathInfo GetNamespaceCurrentLocation(string namespaceID)
		{
			if (namespaceID == null)
			{
				throw PSTraceSource.NewArgumentNullException("namespaceID");
			}
			PSDriveInfo psdriveInfo = null;
			if (namespaceID.Length == 0)
			{
				this.ProvidersCurrentWorkingDrive.TryGetValue(this.CurrentDrive.Provider, out psdriveInfo);
			}
			else
			{
				this.ProvidersCurrentWorkingDrive.TryGetValue(this.GetSingleProvider(namespaceID), out psdriveInfo);
			}
			if (psdriveInfo == null)
			{
				DriveNotFoundException ex = new DriveNotFoundException(namespaceID, "DriveNotFound", SessionStateStrings.DriveNotFound);
				throw ex;
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Drive = psdriveInfo;
			string path;
			if (psdriveInfo.Hidden)
			{
				if (LocationGlobber.IsProviderDirectPath(psdriveInfo.CurrentLocation))
				{
					path = psdriveInfo.CurrentLocation;
				}
				else
				{
					path = LocationGlobber.GetProviderQualifiedPath(psdriveInfo.CurrentLocation, psdriveInfo.Provider);
				}
			}
			else
			{
				path = LocationGlobber.GetDriveQualifiedPath(psdriveInfo.CurrentLocation, psdriveInfo);
			}
			PathInfo pathInfo = new PathInfo(psdriveInfo, psdriveInfo.Provider, path, new SessionState(this));
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				pathInfo
			});
			return pathInfo;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x001A0053 File Offset: 0x0019E253
		internal PathInfo SetLocation(string path)
		{
			return this.SetLocation(path, null);
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x001A0060 File Offset: 0x0019E260
		internal PathInfo SetLocation(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			string text = path;
			string name = null;
			string name2 = null;
			PSDriveInfo psdriveInfo = this.CurrentDrive;
			if (LocationGlobber.IsHomePath(path))
			{
				path = this.Globber.GetHomeRelativePath(path);
			}
			if (LocationGlobber.IsProviderDirectPath(path))
			{
				ProviderInfo providerInfo = this.CurrentLocation.Provider;
				this.CurrentDrive = providerInfo.HiddenDrive;
			}
			else if (LocationGlobber.IsProviderQualifiedPath(path, out name2))
			{
				ProviderInfo providerInfo = this.GetSingleProvider(name2);
				this.CurrentDrive = providerInfo.HiddenDrive;
			}
			else if (this.Globber.IsAbsolutePath(path, out name))
			{
				PSDriveInfo drive = this.GetDrive(name);
				this.CurrentDrive = drive;
			}
			if (context == null)
			{
				context = new CmdletProviderContext(this.ExecutionContext);
			}
			if (this.CurrentDrive != null)
			{
				context.Drive = this.CurrentDrive;
			}
			CmdletProvider cmdletProvider = null;
			Collection<PathInfo> collection = null;
			try
			{
				collection = this.Globber.GetGlobbedMonadPathsFromMonadPath(path, false, context, out cmdletProvider);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this.CurrentDrive = psdriveInfo;
				throw;
			}
			if (collection.Count == 0)
			{
				this.CurrentDrive = psdriveInfo;
				throw new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			for (int i = 0; i < collection.Count; i++)
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
				PathInfo pathInfo = collection[i];
				string text2 = path;
				try
				{
					string name3 = null;
					flag4 = LocationGlobber.IsProviderQualifiedPath(pathInfo.Path, out name3);
					if (flag4)
					{
						string path2 = LocationGlobber.RemoveProviderQualifier(pathInfo.Path);
						try
						{
							text2 = this.NormalizeRelativePath(this.GetSingleProvider(name3), path2, string.Empty, cmdletProviderContext);
							goto IL_1DF;
						}
						catch (NotSupportedException)
						{
							goto IL_1DF;
						}
						catch (LoopFlowException)
						{
							throw;
						}
						catch (PipelineStoppedException)
						{
							throw;
						}
						catch (ActionPreferenceStopException)
						{
							throw;
						}
						catch (Exception e2)
						{
							CommandProcessorBase.CheckForSevereException(e2);
							this.CurrentDrive = psdriveInfo;
							throw;
						}
					}
					try
					{
						text2 = this.NormalizeRelativePath(pathInfo.Path, this.CurrentDrive.Root, cmdletProviderContext);
					}
					catch (NotSupportedException)
					{
					}
					catch (LoopFlowException)
					{
						throw;
					}
					catch (PipelineStoppedException)
					{
						throw;
					}
					catch (ActionPreferenceStopException)
					{
						throw;
					}
					catch (Exception e3)
					{
						CommandProcessorBase.CheckForSevereException(e3);
						this.CurrentDrive = psdriveInfo;
						throw;
					}
					IL_1DF:
					if (cmdletProviderContext.HasErrors())
					{
						this.CurrentDrive = psdriveInfo;
						cmdletProviderContext.ThrowFirstErrorOrDoNothing();
					}
				}
				finally
				{
					cmdletProviderContext.RemoveStopReferral();
				}
				bool flag5 = false;
				CmdletProviderContext cmdletProviderContext2 = new CmdletProviderContext(context);
				cmdletProviderContext2.SuppressWildcardExpansion = true;
				try
				{
					flag5 = this.IsItemContainer(pathInfo.Path, cmdletProviderContext2);
					if (cmdletProviderContext2.HasErrors())
					{
						this.CurrentDrive = psdriveInfo;
						cmdletProviderContext2.ThrowFirstErrorOrDoNothing();
					}
				}
				catch (NotSupportedException)
				{
					if (text2.Length == 0)
					{
						flag5 = true;
					}
				}
				finally
				{
					cmdletProviderContext2.RemoveStopReferral();
				}
				if (flag5)
				{
					if (flag)
					{
						this.CurrentDrive = psdriveInfo;
						throw PSTraceSource.NewArgumentException("path", SessionStateStrings.PathResolvedToMultiple, new object[]
						{
							text
						});
					}
					path = text2;
					flag2 = true;
					flag3 = flag4;
					flag = true;
				}
			}
			if (flag2)
			{
				if (!LocationGlobber.IsProviderDirectPath(path) && path.StartsWith('\\'.ToString(), StringComparison.CurrentCulture) && !flag3)
				{
					path = path.Substring(1);
				}
				SessionStateInternal.tracer.WriteLine("New working path = {0}", new object[]
				{
					path
				});
				this.CurrentDrive.CurrentLocation = path;
				this.ProvidersCurrentWorkingDrive[this.CurrentDrive.Provider] = this.CurrentDrive;
				this.SetVariable(SpecialVariables.PWDVarPath, this.CurrentLocation, false, true, CommandOrigin.Internal);
				return this.CurrentLocation;
			}
			this.CurrentDrive = psdriveInfo;
			throw new ItemNotFoundException(text, "PathNotFound", SessionStateStrings.PathNotFound);
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x001A0490 File Offset: 0x0019E690
		internal bool IsCurrentLocationOrAncestor(string path, CmdletProviderContext context)
		{
			bool flag = false;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			PSDriveInfo psdriveInfo = null;
			ProviderInfo providerInfo = null;
			string text = this.Globber.GetProviderPath(path, context, out providerInfo, out psdriveInfo);
			if (psdriveInfo != null)
			{
				SessionStateInternal.tracer.WriteLine("Tracing drive", new object[0]);
				psdriveInfo.Trace();
			}
			if (psdriveInfo != null)
			{
				context.Drive = psdriveInfo;
			}
			if (psdriveInfo == this.CurrentDrive)
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
				try
				{
					text = this.NormalizeRelativePath(path, null, cmdletProviderContext);
				}
				catch (NotSupportedException)
				{
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				finally
				{
					cmdletProviderContext.RemoveStopReferral();
				}
				if (cmdletProviderContext.HasErrors())
				{
					cmdletProviderContext.ThrowFirstErrorOrDoNothing();
				}
				SessionStateInternal.tracer.WriteLine("Provider path = {0}", new object[]
				{
					text
				});
				PSDriveInfo psdriveInfo2 = null;
				ProviderInfo providerInfo2 = null;
				string providerPath = this.Globber.GetProviderPath(".", context, out providerInfo2, out psdriveInfo2);
				SessionStateInternal.tracer.WriteLine("Current working path = {0}", new object[]
				{
					providerPath
				});
				SessionStateInternal.tracer.WriteLine("Comparing {0} to {1}", new object[]
				{
					text,
					providerPath
				});
				if (string.Compare(text, providerPath, StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					SessionStateInternal.tracer.WriteLine("The path is the current working directory", new object[0]);
					flag = true;
				}
				else
				{
					string text2 = providerPath;
					while (text2.Length > 0)
					{
						text2 = this.GetParentPath(psdriveInfo.Provider, text2, string.Empty, context);
						SessionStateInternal.tracer.WriteLine("Comparing {0} to {1}", new object[]
						{
							text2,
							text
						});
						if (string.Compare(text2, text, StringComparison.CurrentCultureIgnoreCase) == 0)
						{
							SessionStateInternal.tracer.WriteLine("The path is a parent of the current working directory: {0}", new object[]
							{
								text2
							});
							flag = true;
							break;
						}
					}
				}
			}
			else
			{
				SessionStateInternal.tracer.WriteLine("Drives are not the same", new object[0]);
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001A06DC File Offset: 0x0019E8DC
		internal void PushCurrentLocation(string stackName)
		{
			if (string.IsNullOrEmpty(stackName))
			{
				stackName = this.defaultStackName;
			}
			ProviderInfo provider = this.CurrentDrive.Provider;
			string mshQualifiedPath = LocationGlobber.GetMshQualifiedPath(this.CurrentDrive.CurrentLocation, this.CurrentDrive);
			PathInfo item = new PathInfo(this.CurrentDrive, provider, mshQualifiedPath, new SessionState(this));
			SessionStateInternal.tracer.WriteLine("Pushing drive: {0} directory: {1}", new object[]
			{
				this.CurrentDrive.Name,
				mshQualifiedPath
			});
			Stack<PathInfo> stack = null;
			if (!this.workingLocationStack.TryGetValue(stackName, out stack))
			{
				stack = new Stack<PathInfo>();
				this.workingLocationStack[stackName] = stack;
			}
			stack.Push(item);
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001A078C File Offset: 0x0019E98C
		internal PathInfo PopLocation(string stackName)
		{
			if (string.IsNullOrEmpty(stackName))
			{
				stackName = this.defaultStackName;
			}
			if (WildcardPattern.ContainsWildcardCharacters(stackName))
			{
				bool flag = false;
				WildcardPattern wildcardPattern = new WildcardPattern(stackName, WildcardOptions.IgnoreCase);
				foreach (string text in this.workingLocationStack.Keys)
				{
					if (wildcardPattern.IsMatch(text))
					{
						if (flag)
						{
							throw PSTraceSource.NewArgumentException("stackName", SessionStateStrings.StackNameResolvedToMultiple, new object[]
							{
								stackName
							});
						}
						flag = true;
						stackName = text;
					}
				}
			}
			PathInfo pathInfo = this.CurrentLocation;
			try
			{
				Stack<PathInfo> stack = null;
				if (!this.workingLocationStack.TryGetValue(stackName, out stack))
				{
					if (!string.Equals(stackName, "default", StringComparison.OrdinalIgnoreCase))
					{
						throw PSTraceSource.NewArgumentException("stackName", SessionStateStrings.StackNotFound, new object[]
						{
							stackName
						});
					}
					return null;
				}
				else
				{
					PathInfo pathInfo2 = stack.Pop();
					string mshQualifiedPath = LocationGlobber.GetMshQualifiedPath(WildcardPattern.Escape(pathInfo2.Path), pathInfo2.GetDrive());
					pathInfo = this.SetLocation(mshQualifiedPath);
					if (stack.Count == 0 && !string.Equals(stackName, "default", StringComparison.OrdinalIgnoreCase))
					{
						this.workingLocationStack.Remove(stackName);
					}
				}
			}
			catch (InvalidOperationException)
			{
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				pathInfo
			});
			return pathInfo;
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001A0900 File Offset: 0x0019EB00
		internal PathInfoStack LocationStack(string stackName)
		{
			if (string.IsNullOrEmpty(stackName))
			{
				stackName = this.defaultStackName;
			}
			Stack<PathInfo> locationStack = null;
			if (!this.workingLocationStack.TryGetValue(stackName, out locationStack))
			{
				if (!string.Equals(stackName, "default", StringComparison.OrdinalIgnoreCase))
				{
					throw PSTraceSource.NewArgumentException("stackName");
				}
				locationStack = new Stack<PathInfo>();
			}
			return new PathInfoStack(stackName, locationStack);
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x001A095C File Offset: 0x0019EB5C
		internal PathInfoStack SetDefaultLocationStack(string stackName)
		{
			if (string.IsNullOrEmpty(stackName))
			{
				stackName = "default";
			}
			if (!this.workingLocationStack.ContainsKey(stackName))
			{
				if (string.Equals(stackName, "default", StringComparison.OrdinalIgnoreCase))
				{
					return new PathInfoStack("default", new Stack<PathInfo>());
				}
				ItemNotFoundException ex = new ItemNotFoundException(stackName, "StackNotFound", SessionStateStrings.PathNotFound);
				throw ex;
			}
			else
			{
				this.defaultStackName = stackName;
				Stack<PathInfo> stack = this.workingLocationStack[this.defaultStackName];
				if (stack != null)
				{
					return new PathInfoStack(this.defaultStackName, stack);
				}
				return null;
			}
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001A09E4 File Offset: 0x0019EBE4
		internal string GetParentPath(string path, string root)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			string parentPath = this.GetParentPath(path, root, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				parentPath
			});
			return parentPath;
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x001A0A37 File Offset: 0x0019EC37
		internal string GetParentPath(string path, string root, CmdletProviderContext context)
		{
			return this.GetParentPath(path, root, context, false);
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001A0A44 File Offset: 0x0019EC44
		internal string GetParentPath(string path, string root, CmdletProviderContext context, bool useDefaultProvider)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			string result;
			try
			{
				PSDriveInfo psdriveInfo = null;
				ProviderInfo provider = null;
				try
				{
					this.Globber.GetProviderPath(path, cmdletProviderContext, out provider, out psdriveInfo);
				}
				catch (DriveNotFoundException)
				{
					if (!useDefaultProvider)
					{
						throw;
					}
					provider = this.PublicSessionState.Internal.GetSingleProvider("FileSystem");
				}
				if (cmdletProviderContext.HasErrors())
				{
					cmdletProviderContext.WriteErrorsToContext(context);
					result = null;
				}
				else
				{
					if (psdriveInfo != null)
					{
						context.Drive = psdriveInfo;
					}
					bool isProviderQualified = false;
					bool isDriveQualified = false;
					string text = null;
					string path2 = this.RemoveQualifier(path, out text, out isProviderQualified, out isDriveQualified);
					string text2 = this.GetParentPath(provider, path2, root, context);
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						text2 = this.AddQualifier(text2, text, isProviderQualified, isDriveQualified);
					}
					SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
					{
						text2
					});
					result = text2;
				}
			}
			finally
			{
				cmdletProviderContext.RemoveStopReferral();
			}
			return result;
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001A0B58 File Offset: 0x0019ED58
		private string AddQualifier(string path, string qualifier, bool isProviderQualified, bool isDriveQualified)
		{
			string format = "{1}";
			if (isProviderQualified)
			{
				format = "{0}::{1}";
			}
			else if (isDriveQualified)
			{
				format = "{0}:{1}";
			}
			return string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				qualifier,
				path
			});
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x001A0BA0 File Offset: 0x0019EDA0
		private string RemoveQualifier(string path, out string qualifier, out bool isProviderQualified, out bool isDriveQualified)
		{
			string text = path;
			qualifier = null;
			isProviderQualified = false;
			isDriveQualified = false;
			if (LocationGlobber.IsProviderQualifiedPath(path, out qualifier))
			{
				isProviderQualified = true;
				int num = path.IndexOf("::", StringComparison.Ordinal);
				if (num != -1)
				{
					text = path.Substring(num + 2);
				}
			}
			else if (this.Globber.IsAbsolutePath(path, out qualifier))
			{
				isDriveQualified = true;
				text = path.Substring(qualifier.Length + 1);
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x001A0C24 File Offset: 0x0019EE24
		internal string GetParentPath(ProviderInfo provider, string path, string root, CmdletProviderContext context)
		{
			CmdletProvider providerInstance = this.GetProviderInstance(provider);
			return this.GetParentPath(providerInstance, path, root, context);
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x001A0C44 File Offset: 0x0019EE44
		internal string GetParentPath(CmdletProvider providerInstance, string path, string root, CmdletProviderContext context)
		{
			NavigationCmdletProvider navigationProviderInstance = SessionStateInternal.GetNavigationProviderInstance(providerInstance, false);
			string result = null;
			try
			{
				result = navigationProviderInstance.GetParentPath(path, root, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetParentPathProviderException", SessionStateStrings.GetParentPathProviderException, navigationProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x001A0CC8 File Offset: 0x0019EEC8
		internal string NormalizeRelativePath(string path, string basePath)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			string text = this.NormalizeRelativePath(path, basePath, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x001A0D1C File Offset: 0x0019EF1C
		internal string NormalizeRelativePath(string path, string basePath, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			string result;
			try
			{
				PSDriveInfo psdriveInfo = null;
				ProviderInfo providerInfo = null;
				string text = this.Globber.GetProviderPath(path, cmdletProviderContext, out providerInfo, out psdriveInfo);
				if (cmdletProviderContext.HasErrors())
				{
					cmdletProviderContext.WriteErrorsToContext(context);
					result = null;
				}
				else if (text == null || providerInfo == null)
				{
					Exception exception = PSTraceSource.NewArgumentException("path");
					context.WriteError(new ErrorRecord(exception, "NormalizePathNullResult", ErrorCategory.InvalidArgument, path));
					result = null;
				}
				else
				{
					if (basePath != null)
					{
						PSDriveInfo psdriveInfo2 = null;
						ProviderInfo providerInfo2 = null;
						this.Globber.GetProviderPath(basePath, cmdletProviderContext, out providerInfo2, out psdriveInfo2);
						if (psdriveInfo != null && psdriveInfo2 != null && !psdriveInfo.Name.Equals(psdriveInfo2.Name, StringComparison.OrdinalIgnoreCase) && !psdriveInfo.Root.StartsWith(psdriveInfo2.Root, StringComparison.OrdinalIgnoreCase) && !psdriveInfo2.Root.StartsWith(psdriveInfo.Root, StringComparison.OrdinalIgnoreCase))
						{
							return path;
						}
					}
					if (psdriveInfo != null)
					{
						context.Drive = psdriveInfo;
						if (this.GetProviderInstance(providerInfo) is NavigationCmdletProvider && !string.IsNullOrEmpty(psdriveInfo.Root) && path.StartsWith(psdriveInfo.Root, StringComparison.OrdinalIgnoreCase))
						{
							bool flag = this.IsPathSeparator(psdriveInfo.Root[psdriveInfo.Root.Length - 1]);
							int length = psdriveInfo.Root.Length;
							bool flag2 = length < path.Length && this.IsPathSeparator(path[length]);
							bool flag3 = psdriveInfo.Root.Length == path.Length;
							if (flag || flag2 || flag3)
							{
								text = path;
							}
						}
					}
					result = this.NormalizeRelativePath(providerInfo, text, basePath, context);
				}
			}
			finally
			{
				cmdletProviderContext.RemoveStopReferral();
			}
			return result;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x001A0EF0 File Offset: 0x0019F0F0
		private bool IsPathSeparator(char c)
		{
			return c == '\\' || c == '/';
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x001A0F00 File Offset: 0x0019F100
		internal string NormalizeRelativePath(ProviderInfo provider, string path, string basePath, CmdletProviderContext context)
		{
			CmdletProvider providerInstance = this.GetProviderInstance(provider);
			NavigationCmdletProvider navigationCmdletProvider = providerInstance as NavigationCmdletProvider;
			if (navigationCmdletProvider != null)
			{
				try
				{
					return navigationCmdletProvider.NormalizeRelativePath(path, basePath, context);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					throw this.NewProviderInvocationException("NormalizeRelativePathProviderException", SessionStateStrings.NormalizeRelativePathProviderException, navigationCmdletProvider.ProviderInfo, path, e);
				}
			}
			if (!(providerInstance is ContainerCmdletProvider))
			{
				throw PSTraceSource.NewNotSupportedException();
			}
			return path;
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x001A0F9C File Offset: 0x0019F19C
		internal string MakePath(string parent, string child)
		{
			CmdletProviderContext context = new CmdletProviderContext(this.ExecutionContext);
			return this.MakePath(parent, child, context);
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x001A0FC0 File Offset: 0x0019F1C0
		internal string MakePath(string parent, string child, CmdletProviderContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (parent == null && child == null)
			{
				throw PSTraceSource.NewArgumentException("parent");
			}
			ProviderInfo providerInfo = null;
			if (this.CurrentDrive != null)
			{
				providerInfo = this.CurrentDrive.Provider;
			}
			string text;
			if (context.Drive == null)
			{
				bool flag = LocationGlobber.IsProviderQualifiedPath(parent);
				bool flag2 = LocationGlobber.IsAbsolutePath(parent);
				if (flag || flag2)
				{
					PSDriveInfo psdriveInfo = null;
					this.Globber.GetProviderPath(parent, context, out providerInfo, out psdriveInfo);
					if (psdriveInfo == null && flag)
					{
						psdriveInfo = providerInfo.HiddenDrive;
					}
					context.Drive = psdriveInfo;
				}
				else
				{
					context.Drive = this.CurrentDrive;
				}
				text = this.MakePath(providerInfo, parent, child, context);
				if (flag2)
				{
					text = LocationGlobber.GetDriveQualifiedPath(text, context.Drive);
				}
				else if (flag)
				{
					text = LocationGlobber.GetProviderQualifiedPath(text, providerInfo);
				}
			}
			else
			{
				providerInfo = context.Drive.Provider;
				text = this.MakePath(providerInfo, parent, child, context);
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x001A10CC File Offset: 0x0019F2CC
		internal string MakePath(ProviderInfo provider, string parent, string child, CmdletProviderContext context)
		{
			CmdletProvider providerInstance = provider.CreateInstance();
			return this.MakePath(providerInstance, parent, child, context);
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x001A10EC File Offset: 0x0019F2EC
		internal string MakePath(CmdletProvider providerInstance, string parent, string child, CmdletProviderContext context)
		{
			string result = null;
			NavigationCmdletProvider navigationCmdletProvider = providerInstance as NavigationCmdletProvider;
			if (navigationCmdletProvider != null)
			{
				try
				{
					return navigationCmdletProvider.MakePath(parent, child, context);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					throw this.NewProviderInvocationException("MakePathProviderException", SessionStateStrings.MakePathProviderException, navigationCmdletProvider.ProviderInfo, parent, e);
				}
			}
			if (!(providerInstance is ContainerCmdletProvider))
			{
				throw PSTraceSource.NewNotSupportedException();
			}
			result = child;
			return result;
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x001A1184 File Offset: 0x0019F384
		internal string GetChildName(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			string childName = this.GetChildName(path, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				childName
			});
			return childName;
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x001A11D6 File Offset: 0x0019F3D6
		internal string GetChildName(string path, CmdletProviderContext context)
		{
			return this.GetChildName(path, context, false);
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001A11E4 File Offset: 0x0019F3E4
		internal string GetChildName(string path, CmdletProviderContext context, bool useDefaultProvider)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			PSDriveInfo psdriveInfo = null;
			ProviderInfo provider = null;
			string text = null;
			try
			{
				text = this.Globber.GetProviderPath(path, context, out provider, out psdriveInfo);
			}
			catch (DriveNotFoundException)
			{
				if (!useDefaultProvider)
				{
					throw;
				}
				provider = this.PublicSessionState.Internal.GetSingleProvider("FileSystem");
				text = path.Replace('/', '\\');
				text = text.TrimEnd(new char[]
				{
					'\\'
				});
			}
			if (psdriveInfo != null)
			{
				context.Drive = psdriveInfo;
			}
			return this.GetChildName(provider, text, context);
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001A1284 File Offset: 0x0019F484
		private string GetChildName(ProviderInfo provider, string path, CmdletProviderContext context)
		{
			CmdletProvider providerInstance = provider.CreateInstance();
			return this.GetChildName(providerInstance, path, context, true);
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001A12A4 File Offset: 0x0019F4A4
		private string GetChildName(CmdletProvider providerInstance, string path, CmdletProviderContext context, bool acceptNonContainerProviders)
		{
			string result = null;
			NavigationCmdletProvider navigationProviderInstance = SessionStateInternal.GetNavigationProviderInstance(providerInstance, acceptNonContainerProviders);
			if (navigationProviderInstance == null)
			{
				return path;
			}
			try
			{
				result = navigationProviderInstance.GetChildName(path, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetChildNameProviderException", SessionStateStrings.GetChildNameProviderException, navigationProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x001A132C File Offset: 0x0019F52C
		internal Collection<PSObject> MoveItem(string[] paths, string destination, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.MoveItem(paths, destination, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x001A137C File Offset: 0x0019F57C
		internal void MoveItem(string[] paths, string destination, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (destination == null)
			{
				throw PSTraceSource.NewArgumentNullException("destination");
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			Collection<PathInfo> globbedMonadPathsFromMonadPath = this.Globber.GetGlobbedMonadPathsFromMonadPath(destination, true, context, out providerInstance);
			if (globbedMonadPathsFromMonadPath.Count > 1)
			{
				ArgumentException ex = PSTraceSource.NewArgumentException("destination", SessionStateStrings.MoveItemOneDestination, new object[0]);
				context.WriteError(new ErrorRecord(ex, ex.GetType().FullName, ErrorCategory.InvalidArgument, destination));
				return;
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath.Count > 1 && globbedMonadPathsFromMonadPath.Count > 0 && !this.IsItemContainer(globbedMonadPathsFromMonadPath[0].Path))
				{
					ArgumentException ex2 = PSTraceSource.NewArgumentException("path", SessionStateStrings.MoveItemPathMultipleDestinationNotContainer, new object[0]);
					context.WriteError(new ErrorRecord(ex2, ex2.GetType().FullName, ErrorCategory.InvalidArgument, globbedMonadPathsFromMonadPath[0]));
				}
				else
				{
					PSDriveInfo psdriveInfo = null;
					ProviderInfo providerInfo2 = null;
					CmdletProviderContext context2 = new CmdletProviderContext(this.ExecutionContext);
					string providerPath;
					if (globbedMonadPathsFromMonadPath.Count > 0)
					{
						providerPath = this.Globber.GetProviderPath(globbedMonadPathsFromMonadPath[0].Path, context2, out providerInfo2, out psdriveInfo);
					}
					else
					{
						providerPath = this.Globber.GetProviderPath(destination, context2, out providerInfo2, out psdriveInfo);
					}
					if (!string.Equals(providerInfo.FullName, providerInfo2.FullName, StringComparison.OrdinalIgnoreCase))
					{
						ArgumentException ex3 = PSTraceSource.NewArgumentException("destination", SessionStateStrings.MoveItemSourceAndDestinationNotSameProvider, new object[0]);
						context.WriteError(new ErrorRecord(ex3, ex3.GetType().FullName, ErrorCategory.InvalidArgument, globbedProviderPathsFromMonadPath));
					}
					else
					{
						foreach (string path in globbedProviderPathsFromMonadPath)
						{
							this.MoveItemPrivate(providerInstance, path, providerPath, context);
						}
					}
				}
			}
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x001A1584 File Offset: 0x0019F784
		private void MoveItemPrivate(CmdletProvider providerInstance, string path, string destination, CmdletProviderContext context)
		{
			NavigationCmdletProvider navigationProviderInstance = SessionStateInternal.GetNavigationProviderInstance(providerInstance, false);
			try
			{
				navigationProviderInstance.MoveItem(path, destination, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("MoveItemProviderException", SessionStateStrings.MoveItemProviderException, navigationProviderInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x001A1604 File Offset: 0x0019F804
		internal object MoveItemDynamicParameters(string path, string destination, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.MoveItemDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], destination, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x001A1660 File Offset: 0x0019F860
		private object MoveItemDynamicParameters(CmdletProvider providerInstance, string path, string destination, CmdletProviderContext context)
		{
			NavigationCmdletProvider navigationProviderInstance = SessionStateInternal.GetNavigationProviderInstance(providerInstance, false);
			object result = null;
			try
			{
				result = navigationProviderInstance.MoveItemDynamicParameters(path, destination, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("MoveItemDynamicParametersProviderException", SessionStateStrings.MoveItemDynamicParametersProviderException, navigationProviderInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x001A16F4 File Offset: 0x0019F8F4
		internal Collection<PSObject> GetProperty(string[] paths, Collection<string> providerSpecificPickList, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.GetProperty(paths, providerSpecificPickList, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x001A173C File Offset: 0x0019F93C
		internal void GetProperty(string[] paths, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.GetPropertyPrivate(providerInstance, path, providerSpecificPickList, context);
				}
			}
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001A17E0 File Offset: 0x0019F9E0
		private void GetPropertyPrivate(CmdletProvider providerInstance, string path, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			try
			{
				providerInstance.GetProperty(path, providerSpecificPickList, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetPropertyProviderException", SessionStateStrings.GetPropertyProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001A1868 File Offset: 0x0019FA68
		internal object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.GetPropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], providerSpecificPickList, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x001A18C4 File Offset: 0x0019FAC4
		private object GetPropertyDynamicParameters(CmdletProvider providerInstance, string path, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.GetPropertyDynamicParameters(path, providerSpecificPickList, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("GetPropertyDynamicParametersProviderException", SessionStateStrings.GetPropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x001A1950 File Offset: 0x0019FB50
		internal Collection<PSObject> SetProperty(string[] paths, PSObject property, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("properties");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.SetProperty(paths, property, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return cmdletProviderContext.GetAccumulatedObjects();
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x001A19B0 File Offset: 0x0019FBB0
		internal void SetProperty(string[] paths, PSObject property, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (property == null)
			{
				throw PSTraceSource.NewArgumentNullException("property");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				if (globbedProviderPathsFromMonadPath != null)
				{
					foreach (string path in globbedProviderPathsFromMonadPath)
					{
						this.SetPropertyPrivate(providerInstance, path, property, context);
					}
				}
			}
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x001A1A64 File Offset: 0x0019FC64
		private void SetPropertyPrivate(CmdletProvider providerInstance, string path, PSObject property, CmdletProviderContext context)
		{
			try
			{
				providerInstance.SetProperty(path, property, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("SetPropertyProviderException", SessionStateStrings.SetPropertyProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x001A1AEC File Offset: 0x0019FCEC
		internal object SetPropertyDynamicParameters(string path, PSObject propertyValue, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.SetPropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], propertyValue, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x001A1B48 File Offset: 0x0019FD48
		private object SetPropertyDynamicParameters(CmdletProvider providerInstance, string path, PSObject propertyValue, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.SetPropertyDynamicParameters(path, propertyValue, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("SetPropertyDynamicParametersProviderException", SessionStateStrings.SetPropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x001A1BD4 File Offset: 0x0019FDD4
		internal void ClearProperty(string[] paths, Collection<string> propertyToClear, bool force, bool literalPath)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (propertyToClear == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyToClear");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
			cmdletProviderContext.Force = force;
			cmdletProviderContext.SuppressWildcardExpansion = literalPath;
			this.ClearProperty(paths, propertyToClear, cmdletProviderContext);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x001A1C2C File Offset: 0x0019FE2C
		internal void ClearProperty(string[] paths, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			if (paths == null)
			{
				throw PSTraceSource.NewArgumentNullException("paths");
			}
			if (propertyToClear == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyToClear");
			}
			foreach (string text in paths)
			{
				if (text == null)
				{
					throw PSTraceSource.NewArgumentNullException("paths");
				}
				ProviderInfo providerInfo = null;
				CmdletProvider providerInstance = null;
				Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(text, false, context, out providerInfo, out providerInstance);
				foreach (string path in globbedProviderPathsFromMonadPath)
				{
					this.ClearPropertyPrivate(providerInstance, path, propertyToClear, context);
				}
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001A1CDC File Offset: 0x0019FEDC
		private void ClearPropertyPrivate(CmdletProvider providerInstance, string path, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			try
			{
				providerInstance.ClearProperty(path, propertyToClear, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearPropertyProviderException", SessionStateStrings.ClearPropertyProviderException, providerInstance.ProviderInfo, path, e);
			}
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x001A1D64 File Offset: 0x0019FF64
		internal object ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			if (path == null)
			{
				return null;
			}
			ProviderInfo providerInfo = null;
			CmdletProvider providerInstance = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), null);
			Collection<string> globbedProviderPathsFromMonadPath = this.Globber.GetGlobbedProviderPathsFromMonadPath(path, true, cmdletProviderContext, out providerInfo, out providerInstance);
			if (globbedProviderPathsFromMonadPath.Count > 0)
			{
				return this.ClearPropertyDynamicParameters(providerInstance, globbedProviderPathsFromMonadPath[0], propertyToClear, cmdletProviderContext);
			}
			return null;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x001A1DC0 File Offset: 0x0019FFC0
		private object ClearPropertyDynamicParameters(CmdletProvider providerInstance, string path, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			object result = null;
			try
			{
				result = providerInstance.ClearPropertyDynamicParameters(path, propertyToClear, context);
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.NewProviderInvocationException("ClearPropertyDynamicParametersProviderException", SessionStateStrings.ClearPropertyDynamicParametersProviderException, providerInstance.ProviderInfo, path, e);
			}
			return result;
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x001A1E4C File Offset: 0x001A004C
		internal Dictionary<string, List<ProviderInfo>> Providers
		{
			get
			{
				if (this == this._context.TopLevelSessionState)
				{
					return this._providers;
				}
				return this._context.TopLevelSessionState.Providers;
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06004EEC RID: 20204 RVA: 0x001A1E73 File Offset: 0x001A0073
		internal Dictionary<ProviderInfo, PSDriveInfo> ProvidersCurrentWorkingDrive
		{
			get
			{
				if (this == this._context.TopLevelSessionState)
				{
					return this._providersCurrentWorkingDrive;
				}
				return this._context.TopLevelSessionState.ProvidersCurrentWorkingDrive;
			}
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x001A1E9C File Offset: 0x001A009C
		internal void UpdateProviders()
		{
			if (this.ExecutionContext.RunspaceConfiguration == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			if (this == this._context.TopLevelSessionState && !this._providersInitialized)
			{
				foreach (ProviderConfigurationEntry providerConfig in ((IEnumerable<ProviderConfigurationEntry>)this.ExecutionContext.RunspaceConfiguration.Providers))
				{
					this.AddProvider(providerConfig);
				}
				this._providersInitialized = true;
				return;
			}
			foreach (ProviderConfigurationEntry providerConfigurationEntry in this.ExecutionContext.RunspaceConfiguration.Providers.UpdateList)
			{
				switch (providerConfigurationEntry.Action)
				{
				case UpdateAction.Add:
					this.AddProvider(providerConfigurationEntry);
					break;
				case UpdateAction.Remove:
					this.RemoveProvider(providerConfigurationEntry);
					break;
				}
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x001A1F94 File Offset: 0x001A0194
		internal void AddSessionStateEntry(SessionStateProviderEntry providerEntry)
		{
			this.AddProvider(providerEntry.ImplementingType, providerEntry.Name, providerEntry.HelpFileName, providerEntry.PSSnapIn, providerEntry.Module);
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x001A1FBB File Offset: 0x001A01BB
		private ProviderInfo AddProvider(ProviderConfigurationEntry providerConfig)
		{
			return this.AddProvider(providerConfig.ImplementingType, providerConfig.Name, providerConfig.HelpFileName, providerConfig.PSSnapIn, null);
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x001A1FDC File Offset: 0x001A01DC
		private ProviderInfo AddProvider(Type implementingType, string name, string helpFileName, PSSnapInInfo psSnapIn, PSModuleInfo module)
		{
			ProviderInfo providerInfo = null;
			try
			{
				providerInfo = new ProviderInfo(new SessionState(this), implementingType, name, helpFileName, psSnapIn);
				providerInfo.SetModule(module);
				this.NewProvider(providerInfo);
				MshLog.LogProviderLifecycleEvent(this.ExecutionContext, providerInfo.Name, ProviderState.Started);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (SessionStateException ex)
			{
				if (ex.GetType() == typeof(SessionStateException))
				{
					throw;
				}
				this.ExecutionContext.ReportEngineStartupError(ex);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this.ExecutionContext.ReportEngineStartupError(e);
			}
			return providerInfo;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x001A20A8 File Offset: 0x001A02A8
		private PSDriveInfo ValidateDriveWithProvider(PSDriveInfo drive, CmdletProviderContext context, bool resolvePathIfPossible)
		{
			DriveCmdletProvider driveProviderInstance = this.GetDriveProviderInstance(drive.Provider);
			return this.ValidateDriveWithProvider(driveProviderInstance, drive, context, resolvePathIfPossible);
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x001A20CC File Offset: 0x001A02CC
		private PSDriveInfo ValidateDriveWithProvider(DriveCmdletProvider driveProvider, PSDriveInfo drive, CmdletProviderContext context, bool resolvePathIfPossible)
		{
			drive.DriveBeingCreated = true;
			if (this.CurrentDrive != null && resolvePathIfPossible)
			{
				string providerRootFromSpecifiedRoot = this.GetProviderRootFromSpecifiedRoot(drive.Root, drive.Provider);
				if (providerRootFromSpecifiedRoot != null)
				{
					drive.SetRoot(providerRootFromSpecifiedRoot);
				}
			}
			PSDriveInfo result = null;
			try
			{
				result = driveProvider.NewDrive(drive, context);
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				ProviderInvocationException ex = this.NewProviderInvocationException("NewDriveProviderException", SessionStateStrings.NewDriveProviderException, driveProvider.ProviderInfo, drive.Root, e);
				context.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
			}
			finally
			{
				drive.DriveBeingCreated = false;
			}
			return result;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x001A21AC File Offset: 0x001A03AC
		internal CmdletProvider GetProviderInstance(string providerId)
		{
			if (providerId == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerId");
			}
			ProviderInfo singleProvider = this.GetSingleProvider(providerId);
			return this.GetProviderInstance(singleProvider);
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x001A21D6 File Offset: 0x001A03D6
		internal CmdletProvider GetProviderInstance(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			return provider.CreateInstance();
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x001A21EC File Offset: 0x001A03EC
		internal static ProviderNameAmbiguousException NewAmbiguousProviderName(string name, Collection<ProviderInfo> matchingProviders)
		{
			string possibleMatches = SessionStateInternal.GetPossibleMatches(matchingProviders);
			return new ProviderNameAmbiguousException(name, "ProviderNameAmbiguous", SessionStateStrings.ProviderNameAmbiguous, matchingProviders, new object[]
			{
				possibleMatches
			});
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x001A2220 File Offset: 0x001A0420
		private static string GetPossibleMatches(Collection<ProviderInfo> matchingProviders)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ProviderInfo providerInfo in matchingProviders)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(providerInfo.FullName);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x001A2288 File Offset: 0x001A0488
		internal DriveCmdletProvider GetDriveProviderInstance(string providerId)
		{
			if (providerId == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerId");
			}
			DriveCmdletProvider driveCmdletProvider = this.GetProviderInstance(providerId) as DriveCmdletProvider;
			if (driveCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.DriveCmdletProvider_NotSupported, new object[0]);
			}
			return driveCmdletProvider;
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x001A22C8 File Offset: 0x001A04C8
		internal DriveCmdletProvider GetDriveProviderInstance(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			DriveCmdletProvider driveCmdletProvider = this.GetProviderInstance(provider) as DriveCmdletProvider;
			if (driveCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.DriveCmdletProvider_NotSupported, new object[0]);
			}
			return driveCmdletProvider;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x001A2308 File Offset: 0x001A0508
		private static DriveCmdletProvider GetDriveProviderInstance(CmdletProvider providerInstance)
		{
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			DriveCmdletProvider driveCmdletProvider = providerInstance as DriveCmdletProvider;
			if (driveCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.DriveCmdletProvider_NotSupported, new object[0]);
			}
			return driveCmdletProvider;
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x001A2340 File Offset: 0x001A0540
		internal ItemCmdletProvider GetItemProviderInstance(string providerId)
		{
			if (providerId == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerId");
			}
			ItemCmdletProvider itemCmdletProvider = this.GetProviderInstance(providerId) as ItemCmdletProvider;
			if (itemCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ItemCmdletProvider_NotSupported, new object[0]);
			}
			return itemCmdletProvider;
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x001A2380 File Offset: 0x001A0580
		internal ItemCmdletProvider GetItemProviderInstance(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			ItemCmdletProvider itemCmdletProvider = this.GetProviderInstance(provider) as ItemCmdletProvider;
			if (itemCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ItemCmdletProvider_NotSupported, new object[0]);
			}
			return itemCmdletProvider;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x001A23C0 File Offset: 0x001A05C0
		private static ItemCmdletProvider GetItemProviderInstance(CmdletProvider providerInstance)
		{
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			ItemCmdletProvider itemCmdletProvider = providerInstance as ItemCmdletProvider;
			if (itemCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ItemCmdletProvider_NotSupported, new object[0]);
			}
			return itemCmdletProvider;
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x001A23F8 File Offset: 0x001A05F8
		internal ContainerCmdletProvider GetContainerProviderInstance(string providerId)
		{
			if (providerId == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerId");
			}
			ContainerCmdletProvider containerCmdletProvider = this.GetProviderInstance(providerId) as ContainerCmdletProvider;
			if (containerCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ContainerCmdletProvider_NotSupported, new object[0]);
			}
			return containerCmdletProvider;
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001A2438 File Offset: 0x001A0638
		internal ContainerCmdletProvider GetContainerProviderInstance(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			ContainerCmdletProvider containerCmdletProvider = this.GetProviderInstance(provider) as ContainerCmdletProvider;
			if (containerCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ContainerCmdletProvider_NotSupported, new object[0]);
			}
			return containerCmdletProvider;
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001A2478 File Offset: 0x001A0678
		private static ContainerCmdletProvider GetContainerProviderInstance(CmdletProvider providerInstance)
		{
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			ContainerCmdletProvider containerCmdletProvider = providerInstance as ContainerCmdletProvider;
			if (containerCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.ContainerCmdletProvider_NotSupported, new object[0]);
			}
			return containerCmdletProvider;
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001A24B0 File Offset: 0x001A06B0
		internal NavigationCmdletProvider GetNavigationProviderInstance(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			NavigationCmdletProvider navigationCmdletProvider = this.GetProviderInstance(provider) as NavigationCmdletProvider;
			if (navigationCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.NavigationCmdletProvider_NotSupported, new object[0]);
			}
			return navigationCmdletProvider;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x001A24F0 File Offset: 0x001A06F0
		private static NavigationCmdletProvider GetNavigationProviderInstance(CmdletProvider providerInstance, bool acceptNonContainerProviders)
		{
			if (providerInstance == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInstance");
			}
			NavigationCmdletProvider navigationCmdletProvider = providerInstance as NavigationCmdletProvider;
			if (navigationCmdletProvider == null && !acceptNonContainerProviders)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.NavigationCmdletProvider_NotSupported, new object[0]);
			}
			return navigationCmdletProvider;
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x001A252C File Offset: 0x001A072C
		internal bool IsProviderLoaded(string name)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			try
			{
				ProviderInfo singleProvider = this.GetSingleProvider(name);
				flag = (singleProvider != null);
			}
			catch (ProviderNotFoundException)
			{
			}
			SessionStateInternal.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x001A2594 File Offset: 0x001A0794
		internal Collection<ProviderInfo> GetProvider(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(name);
			if (instance == null)
			{
				ProviderNotFoundException ex = new ProviderNotFoundException(name, SessionStateCategory.CmdletProvider, "ProviderNotFoundBadFormat", SessionStateStrings.ProviderNotFoundBadFormat, new object[0]);
				throw ex;
			}
			return this.GetProvider(instance);
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x001A25E0 File Offset: 0x001A07E0
		internal ProviderInfo GetSingleProvider(string name)
		{
			Collection<ProviderInfo> provider = this.GetProvider(name);
			if (provider.Count == 1)
			{
				return provider[0];
			}
			if (provider.Count == 0)
			{
				ProviderNotFoundException ex = new ProviderNotFoundException(name, SessionStateCategory.CmdletProvider, "ProviderNotFound", SessionStateStrings.ProviderNotFound, new object[0]);
				throw ex;
			}
			throw SessionStateInternal.NewAmbiguousProviderName(name, provider);
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x001A2630 File Offset: 0x001A0830
		internal Collection<ProviderInfo> GetProvider(PSSnapinQualifiedName providerName)
		{
			Collection<ProviderInfo> collection = new Collection<ProviderInfo>();
			if (providerName == null)
			{
				ProviderNotFoundException ex = new ProviderNotFoundException(providerName.ToString(), SessionStateCategory.CmdletProvider, "ProviderNotFound", SessionStateStrings.ProviderNotFound, new object[0]);
				throw ex;
			}
			List<ProviderInfo> list = null;
			if (!this.Providers.TryGetValue(providerName.ShortName, out list))
			{
				SessionStateInternal.MountDefaultDrive(providerName.ShortName, this._context);
				if (!this.Providers.TryGetValue(providerName.ShortName, out list))
				{
					ProviderNotFoundException ex2 = new ProviderNotFoundException(providerName.ToString(), SessionStateCategory.CmdletProvider, "ProviderNotFound", SessionStateStrings.ProviderNotFound, new object[0]);
					throw ex2;
				}
			}
			if (this.ExecutionContext.IsSingleShell && !string.IsNullOrEmpty(providerName.PSSnapInName))
			{
				using (List<ProviderInfo>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProviderInfo providerInfo = enumerator.Current;
						if (string.Equals(providerInfo.PSSnapInName, providerName.PSSnapInName, StringComparison.OrdinalIgnoreCase) || string.Equals(providerInfo.ModuleName, providerName.PSSnapInName, StringComparison.OrdinalIgnoreCase))
						{
							collection.Add(providerInfo);
						}
					}
					return collection;
				}
			}
			foreach (ProviderInfo item in list)
			{
				collection.Add(item);
			}
			return collection;
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06004F06 RID: 20230 RVA: 0x001A278C File Offset: 0x001A098C
		internal IEnumerable<ProviderInfo> ProviderList
		{
			get
			{
				Collection<ProviderInfo> collection = new Collection<ProviderInfo>();
				foreach (List<ProviderInfo> list in this.Providers.Values)
				{
					foreach (ProviderInfo item in list)
					{
						collection.Add(item);
					}
				}
				return collection;
			}
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x001A2824 File Offset: 0x001A0A24
		internal void CopyProviders(SessionStateInternal ss)
		{
			if (ss == null || ss.Providers == null)
			{
				return;
			}
			this._providers = new Dictionary<string, List<ProviderInfo>>();
			foreach (KeyValuePair<string, List<ProviderInfo>> keyValuePair in ss._providers)
			{
				this._providers.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x001A28A0 File Offset: 0x001A0AA0
		internal void InitializeProvider(CmdletProvider providerInstance, ProviderInfo provider, CmdletProviderContext context)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			if (context == null)
			{
				context = new CmdletProviderContext(this.ExecutionContext);
			}
			List<PSDriveInfo> list = new List<PSDriveInfo>();
			DriveCmdletProvider driveProviderInstance = SessionStateInternal.GetDriveProviderInstance(providerInstance);
			if (driveProviderInstance != null)
			{
				try
				{
					Collection<PSDriveInfo> collection = driveProviderInstance.InitializeDefaultDrives(context);
					if (collection != null && collection.Count > 0)
					{
						list.AddRange(collection);
						this.ProvidersCurrentWorkingDrive[provider] = collection[0];
					}
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					ProviderInvocationException exception = this.NewProviderInvocationException("InitializeDefaultDrivesException", SessionStateStrings.InitializeDefaultDrivesException, provider, string.Empty, e);
					context.WriteError(new ErrorRecord(exception, "InitializeDefaultDrivesException", ErrorCategory.InvalidOperation, provider));
				}
			}
			if (list != null && list.Count > 0)
			{
				foreach (PSDriveInfo psdriveInfo in list)
				{
					if (!(psdriveInfo == null) && provider.NameEquals(psdriveInfo.Provider.FullName))
					{
						try
						{
							PSDriveInfo psdriveInfo2 = this.ValidateDriveWithProvider(driveProviderInstance, psdriveInfo, context, false);
							if (psdriveInfo2 != null)
							{
								this._globalScope.NewDrive(psdriveInfo2);
							}
						}
						catch (SessionStateException ex)
						{
							context.WriteError(ex.ErrorRecord);
						}
					}
				}
			}
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x001A2A2C File Offset: 0x001A0C2C
		internal ProviderInfo NewProvider(ProviderInfo provider)
		{
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			ProviderInfo providerInfo = this.ProviderExists(provider);
			if (providerInfo != null)
			{
				if (providerInfo.ImplementingType == provider.ImplementingType)
				{
					return providerInfo;
				}
				SessionStateException ex = new SessionStateException(provider.Name, SessionStateCategory.CmdletProvider, "CmdletProviderAlreadyExists", SessionStateStrings.CmdletProviderAlreadyExists, ErrorCategory.ResourceExists, new object[0]);
				throw ex;
			}
			else
			{
				CmdletProvider cmdletProvider = provider.CreateInstance();
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
				ProviderInfo providerInfo2 = null;
				try
				{
					providerInfo2 = cmdletProvider.Start(provider, cmdletProviderContext);
					cmdletProvider.SetProviderInformation(providerInfo2);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					throw;
				}
				catch (InvalidOperationException)
				{
					throw;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					throw this.NewProviderInvocationException("ProviderStartException", SessionStateStrings.ProviderStartException, provider, null, e);
				}
				cmdletProviderContext.ThrowFirstErrorOrDoNothing(true);
				if (providerInfo2 == null)
				{
					throw PSTraceSource.NewInvalidOperationException(SessionStateStrings.InvalidProviderInfoNull, new object[0]);
				}
				if (providerInfo2 != provider)
				{
					if (!string.Equals(providerInfo2.Name, provider.Name, StringComparison.OrdinalIgnoreCase))
					{
						throw PSTraceSource.NewInvalidOperationException(SessionStateStrings.InvalidProviderInfo, new object[0]);
					}
					provider = providerInfo2;
				}
				try
				{
					this.NewProviderEntry(provider);
				}
				catch (ArgumentException)
				{
					SessionStateException ex2 = new SessionStateException(provider.Name, SessionStateCategory.CmdletProvider, "CmdletProviderAlreadyExists", SessionStateStrings.CmdletProviderAlreadyExists, ErrorCategory.ResourceExists, new object[0]);
					throw ex2;
				}
				this.ProvidersCurrentWorkingDrive.Add(provider, null);
				bool flag = false;
				try
				{
					this.InitializeProvider(cmdletProvider, provider, cmdletProviderContext);
					cmdletProviderContext.ThrowFirstErrorOrDoNothing(true);
				}
				catch (LoopFlowException)
				{
					throw;
				}
				catch (PipelineStoppedException)
				{
					flag = true;
					throw;
				}
				catch (ActionPreferenceStopException)
				{
					flag = true;
					throw;
				}
				catch (NotSupportedException)
				{
					flag = false;
				}
				catch (SessionStateException)
				{
					flag = true;
					throw;
				}
				finally
				{
					if (flag)
					{
						this.Providers.Remove(provider.Name.ToString());
						this.ProvidersCurrentWorkingDrive.Remove(provider);
						provider = null;
					}
				}
				return provider;
			}
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x001A2C54 File Offset: 0x001A0E54
		private ProviderInfo ProviderExists(ProviderInfo provider)
		{
			List<ProviderInfo> list = null;
			if (this.Providers.TryGetValue(provider.Name, out list))
			{
				foreach (ProviderInfo providerInfo in list)
				{
					if (provider.NameEquals(providerInfo.FullName))
					{
						return providerInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x001A2CC8 File Offset: 0x001A0EC8
		private void NewProviderEntry(ProviderInfo provider)
		{
			bool flag = false;
			if (!this.Providers.ContainsKey(provider.Name))
			{
				this.Providers.Add(provider.Name, new List<ProviderInfo>());
			}
			else
			{
				List<ProviderInfo> list = this.Providers[provider.Name];
				foreach (ProviderInfo providerInfo in list)
				{
					if (string.IsNullOrEmpty(provider.PSSnapInName) && string.Equals(providerInfo.Name, provider.Name, StringComparison.OrdinalIgnoreCase) && providerInfo.GetType().Equals(provider.GetType()))
					{
						flag = true;
					}
					else if (string.Equals(providerInfo.PSSnapInName, provider.PSSnapInName, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.Providers[provider.Name].Add(provider);
			}
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x001A2DBC File Offset: 0x001A0FBC
		private void RemoveProvider(ProviderConfigurationEntry entry)
		{
			try
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
				string providerName = this.GetProviderName(entry);
				this.RemoveProvider(providerName, true, cmdletProviderContext);
				cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this.ExecutionContext.ReportEngineStartupError(e);
			}
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x001A2E40 File Offset: 0x001A1040
		private string GetProviderName(ProviderConfigurationEntry entry)
		{
			string result = entry.Name;
			if (entry.PSSnapIn != null)
			{
				result = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
				{
					entry.PSSnapIn.Name,
					entry.Name
				});
			}
			return result;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x001A2E8C File Offset: 0x001A108C
		internal void RemoveProvider(string providerName, bool force, CmdletProviderContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (string.IsNullOrEmpty(providerName))
			{
				throw PSTraceSource.NewArgumentException("providerName");
			}
			bool flag = false;
			ProviderInfo providerInfo = null;
			try
			{
				providerInfo = this.GetSingleProvider(providerName);
			}
			catch (ProviderNotFoundException)
			{
				return;
			}
			try
			{
				CmdletProvider providerInstance = this.GetProviderInstance(providerInfo);
				if (providerInstance == null)
				{
					ProviderNotFoundException ex = new ProviderNotFoundException(providerName, SessionStateCategory.CmdletProvider, "ProviderNotFound", SessionStateStrings.ProviderNotFound, new object[0]);
					context.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
					flag = true;
				}
				else
				{
					int num = 0;
					foreach (PSDriveInfo drive in this.GetDrivesForProvider(providerName))
					{
						if (drive != null)
						{
							num++;
							break;
						}
					}
					if (num > 0)
					{
						if (force)
						{
							using (IEnumerator<PSDriveInfo> enumerator2 = this.GetDrivesForProvider(providerName).GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									PSDriveInfo psdriveInfo = enumerator2.Current;
									if (psdriveInfo != null)
									{
										this.RemoveDrive(psdriveInfo, true, null);
									}
								}
								goto IL_134;
							}
						}
						flag = true;
						SessionStateException ex2 = new SessionStateException(providerName, SessionStateCategory.CmdletProvider, "RemoveDrivesBeforeRemovingProvider", SessionStateStrings.RemoveDrivesBeforeRemovingProvider, ErrorCategory.InvalidOperation, new object[0]);
						context.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
						return;
					}
					try
					{
						IL_134:
						providerInstance.Stop(context);
					}
					catch (LoopFlowException)
					{
						throw;
					}
					catch (PipelineStoppedException)
					{
						throw;
					}
					catch (ActionPreferenceStopException)
					{
						throw;
					}
				}
			}
			catch (LoopFlowException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (ActionPreferenceStopException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				flag = true;
				context.WriteError(new ErrorRecord(ex3, "RemoveProviderUnexpectedException", ErrorCategory.InvalidArgument, providerName));
			}
			finally
			{
				if (force || !flag)
				{
					MshLog.LogProviderLifecycleEvent(this.ExecutionContext, providerName, ProviderState.Stopped);
					this.RemoveProviderFromCollection(providerInfo);
					this.ProvidersCurrentWorkingDrive.Remove(providerInfo);
				}
			}
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001A3144 File Offset: 0x001A1344
		private void RemoveProviderFromCollection(ProviderInfo provider)
		{
			if (this.Providers.ContainsKey(provider.Name))
			{
				List<ProviderInfo> list = this.Providers[provider.Name];
				if (list.Count == 1 && list[0].NameEquals(provider.FullName))
				{
					this.Providers.Remove(provider.Name);
					return;
				}
				list.Remove(provider);
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06004F10 RID: 20240 RVA: 0x001A31B0 File Offset: 0x001A13B0
		internal int ProviderCount
		{
			get
			{
				int num = 0;
				foreach (List<ProviderInfo> list in this.Providers.Values)
				{
					num += list.Count;
				}
				return num;
			}
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001A3210 File Offset: 0x001A1410
		internal SessionStateScope GetScopeByID(string scopeID)
		{
			SessionStateScope result = this.currentScope;
			if (!string.IsNullOrEmpty(scopeID))
			{
				if (string.Equals(scopeID, "GLOBAL", StringComparison.OrdinalIgnoreCase))
				{
					result = this._globalScope;
				}
				else if (string.Equals(scopeID, "LOCAL", StringComparison.OrdinalIgnoreCase))
				{
					result = this.currentScope;
				}
				else if (string.Equals(scopeID, "PRIVATE", StringComparison.OrdinalIgnoreCase))
				{
					result = this.currentScope;
				}
				else if (string.Equals(scopeID, "SCRIPT", StringComparison.OrdinalIgnoreCase))
				{
					result = this.currentScope.ScriptScope;
				}
				else
				{
					try
					{
						int num = int.Parse(scopeID, CultureInfo.CurrentCulture);
						if (num < 0)
						{
							throw PSTraceSource.NewArgumentOutOfRangeException("scopeID", scopeID);
						}
						result = (this.GetScopeByID(num) ?? this.currentScope);
					}
					catch (FormatException)
					{
						throw PSTraceSource.NewArgumentException("scopeID", AutomationExceptions.InvalidScopeIdArgument, new object[]
						{
							"scopeID"
						});
					}
					catch (OverflowException)
					{
						throw PSTraceSource.NewArgumentOutOfRangeException("scopeID", scopeID);
					}
				}
			}
			return result;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001A3314 File Offset: 0x001A1514
		internal SessionStateScope GetScopeByID(int scopeID)
		{
			SessionStateScope parent = this.currentScope;
			int num = scopeID;
			while (scopeID > 0 && parent != null)
			{
				parent = parent.Parent;
				scopeID--;
			}
			if (parent == null && scopeID >= 0)
			{
				ArgumentOutOfRangeException ex = PSTraceSource.NewArgumentOutOfRangeException("scopeID", num, SessionStateStrings.ScopeIDExceedsAvailableScopes, new object[]
				{
					num
				});
				throw ex;
			}
			return parent;
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06004F13 RID: 20243 RVA: 0x001A3370 File Offset: 0x001A1570
		internal SessionStateScope GlobalScope
		{
			get
			{
				return this._globalScope;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06004F14 RID: 20244 RVA: 0x001A3378 File Offset: 0x001A1578
		internal SessionStateScope ModuleScope
		{
			get
			{
				return this._moduleScope;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x001A3380 File Offset: 0x001A1580
		// (set) Token: 0x06004F16 RID: 20246 RVA: 0x001A3388 File Offset: 0x001A1588
		internal SessionStateScope CurrentScope
		{
			get
			{
				return this.currentScope;
			}
			set
			{
				this.currentScope = value;
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x001A3391 File Offset: 0x001A1591
		internal SessionStateScope ScriptScope
		{
			get
			{
				return this.currentScope.ScriptScope;
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x001A33A0 File Offset: 0x001A15A0
		internal SessionStateScope NewScope(bool isScriptScope)
		{
			SessionStateScope sessionStateScope = new SessionStateScope(this.currentScope);
			if (isScriptScope)
			{
				sessionStateScope.ScriptScope = sessionStateScope;
			}
			return sessionStateScope;
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x001A33C4 File Offset: 0x001A15C4
		internal void RemoveScope(SessionStateScope scope)
		{
			if (scope == this._globalScope)
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException("GLOBAL", SessionStateCategory.Scope, "GlobalScopeCannotRemove", SessionStateStrings.GlobalScopeCannotRemove);
				throw ex;
			}
			foreach (PSDriveInfo psdriveInfo in scope.Drives)
			{
				if (!(psdriveInfo == null))
				{
					CmdletProviderContext context = new CmdletProviderContext(this.ExecutionContext);
					try
					{
						this.CanRemoveDrive(psdriveInfo, context);
					}
					catch (LoopFlowException)
					{
						throw;
					}
					catch (PipelineStoppedException)
					{
						throw;
					}
					catch (ActionPreferenceStopException)
					{
						throw;
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			}
			scope.RemoveAllDrives();
			if (scope == this.currentScope && this.currentScope.Parent != null)
			{
				this.currentScope = this.currentScope.Parent;
			}
			scope.Parent = null;
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x001A34C8 File Offset: 0x001A16C8
		internal void AddSessionStateEntry(SessionStateVariableEntry entry)
		{
			this.SetVariableAtScope(new PSVariable(entry.Name, entry.Value, entry.Options, entry.Attributes, entry.Description)
			{
				Visibility = entry.Visibility
			}, "global", true, CommandOrigin.Internal);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x001A3514 File Offset: 0x001A1714
		internal PSVariable GetVariable(string name, CommandOrigin origin)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name, VariablePathFlags.Variable | VariablePathFlags.Unqualified);
			SessionStateScope sessionStateScope = null;
			return this.GetVariableItem(variablePath, out sessionStateScope, origin);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001A3549 File Offset: 0x001A1749
		internal PSVariable GetVariable(string name)
		{
			return this.GetVariable(name, CommandOrigin.Internal);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x001A3554 File Offset: 0x001A1754
		internal object GetVariableValue(string name)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			CmdletProviderContext cmdletProviderContext = null;
			SessionStateScope sessionStateScope = null;
			return this.GetVariableValue(variablePath, out cmdletProviderContext, out sessionStateScope);
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x001A3588 File Offset: 0x001A1788
		internal object GetVariableValue(string name, object defaultValue)
		{
			object obj = this.GetVariableValue(name);
			if (obj == null)
			{
				obj = defaultValue;
			}
			return obj;
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x001A35A4 File Offset: 0x001A17A4
		internal object GetVariableValue(VariablePath variablePath, out CmdletProviderContext context, out SessionStateScope scope)
		{
			context = null;
			scope = null;
			object result = null;
			if (variablePath.IsVariable)
			{
				PSVariable variableItem = this.GetVariableItem(variablePath, out scope);
				if (variableItem != null)
				{
					result = variableItem.Value;
				}
			}
			else
			{
				result = this.GetVariableValueFromProvider(variablePath, out context, out scope, this.currentScope.ScopeOrigin);
			}
			return result;
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x001A35EC File Offset: 0x001A17EC
		internal object GetVariableValueFromProvider(VariablePath variablePath, out CmdletProviderContext context, out SessionStateScope scope, CommandOrigin origin)
		{
			scope = null;
			if (variablePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("variablePath");
			}
			context = null;
			DriveScopeItemSearcher driveScopeItemSearcher = new DriveScopeItemSearcher(this, variablePath);
			object result = null;
			if (driveScopeItemSearcher.MoveNext())
			{
				PSDriveInfo psdriveInfo = ((IEnumerator<PSDriveInfo>)driveScopeItemSearcher).Current;
				if (!(psdriveInfo == null))
				{
					context = new CmdletProviderContext(this.ExecutionContext, origin);
					context.Drive = psdriveInfo;
					Collection<IContentReader> collection = null;
					try
					{
						collection = this.GetContentReader(new string[]
						{
							variablePath.QualifiedName
						}, context);
					}
					catch (ItemNotFoundException)
					{
						return result;
					}
					catch (DriveNotFoundException)
					{
						return result;
					}
					catch (ProviderNotFoundException)
					{
						return result;
					}
					catch (NotImplementedException e)
					{
						ProviderInfo provider = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider);
						throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider, variablePath.QualifiedName, e, false);
					}
					catch (NotSupportedException e2)
					{
						ProviderInfo provider2 = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider2);
						throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider2, variablePath.QualifiedName, e2, false);
					}
					if (collection != null && collection.Count != 0)
					{
						if (collection.Count > 1)
						{
							foreach (IContentReader contentReader in collection)
							{
								contentReader.Close();
							}
							PSArgumentException e3 = PSTraceSource.NewArgumentException("path", SessionStateStrings.VariablePathResolvedToMultiple, new object[]
							{
								variablePath.QualifiedName
							});
							ProviderInfo provider3 = null;
							this.Globber.GetProviderPath(variablePath.QualifiedName, out provider3);
							throw this.NewProviderInvocationException("ProviderVariableSyntaxInvalid", SessionStateStrings.ProviderVariableSyntaxInvalid, provider3, variablePath.QualifiedName, e3);
						}
						IContentReader contentReader2 = collection[0];
						try
						{
							IList list = contentReader2.Read(-1L);
							if (list != null)
							{
								if (list.Count == 0)
								{
									result = null;
								}
								else if (list.Count == 1)
								{
									result = list[0];
								}
								else
								{
									result = list;
								}
							}
						}
						catch (Exception ex)
						{
							ProviderInfo provider4 = null;
							this.Globber.GetProviderPath(variablePath.QualifiedName, out provider4);
							CommandProcessorBase.CheckForSevereException(ex);
							ProviderInvocationException ex2 = new ProviderInvocationException("ProviderContentReadError", SessionStateStrings.ProviderContentReadError, provider4, variablePath.QualifiedName, ex);
							throw ex2;
						}
						finally
						{
							contentReader2.Close();
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x001A387C File Offset: 0x001A1A7C
		internal PSVariable GetVariableItem(VariablePath variablePath, out SessionStateScope scope, CommandOrigin origin)
		{
			scope = null;
			if (variablePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("variablePath");
			}
			VariableScopeItemSearcher variableScopeItemSearcher = new VariableScopeItemSearcher(this, variablePath, origin);
			PSVariable result = null;
			if (variableScopeItemSearcher.MoveNext())
			{
				result = ((IEnumerator<PSVariable>)variableScopeItemSearcher).Current;
				scope = variableScopeItemSearcher.CurrentLookupScope;
			}
			return result;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001A38BD File Offset: 0x001A1ABD
		internal PSVariable GetVariableItem(VariablePath variablePath, out SessionStateScope scope)
		{
			return this.GetVariableItem(variablePath, out scope, CommandOrigin.Internal);
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x001A38C8 File Offset: 0x001A1AC8
		internal PSVariable GetVariableAtScope(string name, string scopeID)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			PSVariable result = null;
			if (variablePath.IsVariable)
			{
				result = scopeByID.GetVariable(variablePath.QualifiedName);
			}
			return result;
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x001A390C File Offset: 0x001A1B0C
		internal object GetVariableValueAtScope(string name, string scopeID)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			object obj = null;
			if (variablePath.IsVariable)
			{
				obj = scopeByID.GetVariable(variablePath.QualifiedName);
			}
			else
			{
				PSDriveInfo drive = scopeByID.GetDrive(variablePath.DriveName);
				if (drive != null)
				{
					CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
					cmdletProviderContext.Drive = drive;
					Collection<IContentReader> collection = null;
					try
					{
						collection = this.GetContentReader(new string[]
						{
							variablePath.QualifiedName
						}, cmdletProviderContext);
					}
					catch (ItemNotFoundException)
					{
						return null;
					}
					catch (DriveNotFoundException)
					{
						return null;
					}
					catch (ProviderNotFoundException)
					{
						return null;
					}
					catch (NotImplementedException e)
					{
						ProviderInfo provider = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider);
						throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider, variablePath.QualifiedName, e, false);
					}
					catch (NotSupportedException e2)
					{
						ProviderInfo provider2 = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider2);
						throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider2, variablePath.QualifiedName, e2, false);
					}
					if (collection == null || collection.Count == 0)
					{
						return null;
					}
					if (collection.Count > 1)
					{
						foreach (IContentReader contentReader in collection)
						{
							contentReader.Close();
						}
						PSArgumentException e3 = PSTraceSource.NewArgumentException("path", SessionStateStrings.VariablePathResolvedToMultiple, new object[]
						{
							name
						});
						ProviderInfo provider3 = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider3);
						throw this.NewProviderInvocationException("ProviderVariableSyntaxInvalid", SessionStateStrings.ProviderVariableSyntaxInvalid, provider3, variablePath.QualifiedName, e3);
					}
					IContentReader contentReader2 = collection[0];
					try
					{
						IList list = contentReader2.Read(-1L);
						if (list != null)
						{
							if (list.Count == 0)
							{
								obj = null;
							}
							else if (list.Count == 1)
							{
								obj = list[0];
							}
							else
							{
								obj = list;
							}
						}
					}
					catch (Exception ex)
					{
						ProviderInfo provider4 = null;
						this.Globber.GetProviderPath(variablePath.QualifiedName, out provider4);
						CommandProcessorBase.CheckForSevereException(ex);
						ProviderInvocationException ex2 = new ProviderInvocationException("ProviderContentReadError", SessionStateStrings.ProviderContentReadError, provider4, variablePath.QualifiedName, ex);
						throw ex2;
					}
					finally
					{
						contentReader2.Close();
					}
				}
			}
			if (obj != null)
			{
				PSVariable psvariable = obj as PSVariable;
				if (psvariable != null)
				{
					obj = psvariable.Value;
				}
				else
				{
					try
					{
						obj = ((DictionaryEntry)obj).Value;
					}
					catch (InvalidCastException)
					{
					}
				}
			}
			return obj;
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x001A3BF8 File Offset: 0x001A1DF8
		internal object GetAutomaticVariableValue(AutomaticVariable variable)
		{
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.CurrentScope);
			object obj = AutomationNull.Value;
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				obj = sessionStateScope.GetAutomaticVariableValue(variable);
				if (obj != AutomationNull.Value)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001A3C5C File Offset: 0x001A1E5C
		internal void SetVariableValue(string name, object newValue, CommandOrigin origin)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			this.SetVariable(variablePath, newValue, true, origin);
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x001A3C89 File Offset: 0x001A1E89
		internal void SetVariableValue(string name, object newValue)
		{
			this.SetVariableValue(name, newValue, CommandOrigin.Internal);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x001A3C94 File Offset: 0x001A1E94
		internal object SetVariable(PSVariable variable, bool force, CommandOrigin origin)
		{
			if (variable == null || string.IsNullOrEmpty(variable.Name))
			{
				throw PSTraceSource.NewArgumentException("variable");
			}
			VariablePath variablePath = new VariablePath(variable.Name, VariablePathFlags.Variable | VariablePathFlags.Unqualified);
			return this.SetVariable(variablePath, variable, false, force, origin);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x001A3CD8 File Offset: 0x001A1ED8
		internal object SetVariable(VariablePath variablePath, object newValue, bool asValue, CommandOrigin origin)
		{
			return this.SetVariable(variablePath, newValue, asValue, false, origin);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001A3CE8 File Offset: 0x001A1EE8
		internal object SetVariable(VariablePath variablePath, object newValue, bool asValue, bool force, CommandOrigin origin)
		{
			object result = null;
			if (variablePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("variablePath");
			}
			CmdletProviderContext cmdletProviderContext = null;
			SessionStateScope sessionStateScope = null;
			if (variablePath.IsVariable)
			{
				if (variablePath.IsLocal || variablePath.IsUnscopedVariable)
				{
					sessionStateScope = this.currentScope;
				}
				else if (variablePath.IsScript)
				{
					sessionStateScope = this.currentScope.ScriptScope;
				}
				else if (variablePath.IsGlobal)
				{
					sessionStateScope = this._globalScope;
				}
				else if (variablePath.IsPrivate)
				{
					sessionStateScope = this.currentScope;
				}
				PSVariable psvariable = sessionStateScope.SetVariable(variablePath.QualifiedName, newValue, asValue, force, this, origin, false);
				if (variablePath.IsPrivate && psvariable != null)
				{
					psvariable.Options |= ScopedItemOptions.Private;
				}
				result = psvariable;
			}
			else
			{
				this.GetVariableValue(variablePath, out cmdletProviderContext, out sessionStateScope);
				Collection<IContentWriter> collection = null;
				try
				{
					if (cmdletProviderContext != null)
					{
						try
						{
							CmdletProviderContext context = new CmdletProviderContext(cmdletProviderContext);
							this.ClearContent(new string[]
							{
								variablePath.QualifiedName
							}, context);
						}
						catch (NotSupportedException)
						{
						}
						catch (ItemNotFoundException)
						{
						}
						collection = this.GetContentWriter(new string[]
						{
							variablePath.QualifiedName
						}, cmdletProviderContext);
						cmdletProviderContext.ThrowFirstErrorOrDoNothing(true);
					}
					else
					{
						try
						{
							this.ClearContent(new string[]
							{
								variablePath.QualifiedName
							}, false, false);
						}
						catch (NotSupportedException)
						{
						}
						catch (ItemNotFoundException)
						{
						}
						collection = this.GetContentWriter(new string[]
						{
							variablePath.QualifiedName
						}, false, false);
					}
				}
				catch (NotImplementedException e)
				{
					ProviderInfo provider = null;
					this.Globber.GetProviderPath(variablePath.QualifiedName, out provider);
					throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider, variablePath.QualifiedName, e, false);
				}
				catch (NotSupportedException e2)
				{
					ProviderInfo provider2 = null;
					this.Globber.GetProviderPath(variablePath.QualifiedName, out provider2);
					throw this.NewProviderInvocationException("ProviderCannotBeUsedAsVariable", SessionStateStrings.ProviderCannotBeUsedAsVariable, provider2, variablePath.QualifiedName, e2, false);
				}
				if (collection == null || collection.Count == 0)
				{
					ItemNotFoundException ex = new ItemNotFoundException(variablePath.QualifiedName, "PathNotFound", SessionStateStrings.PathNotFound);
					throw ex;
				}
				if (collection.Count > 1)
				{
					foreach (IContentWriter contentWriter in collection)
					{
						contentWriter.Close();
					}
					PSArgumentException e3 = PSTraceSource.NewArgumentException("path", SessionStateStrings.VariablePathResolvedToMultiple, new object[]
					{
						variablePath.QualifiedName
					});
					ProviderInfo provider3 = null;
					this.Globber.GetProviderPath(variablePath.QualifiedName, out provider3);
					throw this.NewProviderInvocationException("ProviderVariableSyntaxInvalid", SessionStateStrings.ProviderVariableSyntaxInvalid, provider3, variablePath.QualifiedName, e3);
				}
				IContentWriter contentWriter2 = collection[0];
				IList list = newValue as IList;
				if (list == null)
				{
					list = new object[]
					{
						newValue
					};
				}
				try
				{
					contentWriter2.Write(list);
				}
				catch (Exception ex2)
				{
					ProviderInfo provider4 = null;
					this.Globber.GetProviderPath(variablePath.QualifiedName, out provider4);
					CommandProcessorBase.CheckForSevereException(ex2);
					ProviderInvocationException ex3 = new ProviderInvocationException("ProviderContentWriteError", SessionStateStrings.ProviderContentWriteError, provider4, variablePath.QualifiedName, ex2);
					throw ex3;
				}
				finally
				{
					contentWriter2.Close();
				}
			}
			return result;
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x001A4054 File Offset: 0x001A2254
		internal object SetVariableAtScope(PSVariable variable, string scopeID, bool force, CommandOrigin origin)
		{
			if (variable == null || string.IsNullOrEmpty(variable.Name))
			{
				throw PSTraceSource.NewArgumentException("variable");
			}
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			return scopeByID.SetVariable(variable.Name, variable, false, force, this, origin, false);
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001A4097 File Offset: 0x001A2297
		internal object NewVariable(PSVariable variable, bool force)
		{
			if (variable == null || string.IsNullOrEmpty(variable.Name))
			{
				throw PSTraceSource.NewArgumentException("variable");
			}
			return this.CurrentScope.NewVariable(variable, force, this);
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001A40C4 File Offset: 0x001A22C4
		internal object NewVariableAtScope(PSVariable variable, string scopeID, bool force)
		{
			if (variable == null || string.IsNullOrEmpty(variable.Name))
			{
				throw PSTraceSource.NewArgumentException("variable");
			}
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			return scopeByID.NewVariable(variable, force, this);
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001A40FD File Offset: 0x001A22FD
		internal void RemoveVariable(string name)
		{
			this.RemoveVariable(name, false);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001A4108 File Offset: 0x001A2308
		internal void RemoveVariable(string name, bool force)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			SessionStateScope sessionStateScope = null;
			if (variablePath.IsVariable)
			{
				if (this.GetVariableItem(variablePath, out sessionStateScope) != null)
				{
					sessionStateScope.RemoveVariable(variablePath.QualifiedName, force);
					return;
				}
			}
			else
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
				cmdletProviderContext.Force = force;
				this.RemoveItem(new string[]
				{
					variablePath.QualifiedName
				}, false, cmdletProviderContext);
				cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			}
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001A4184 File Offset: 0x001A2384
		internal void RemoveVariable(PSVariable variable)
		{
			this.RemoveVariable(variable, false);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001A4190 File Offset: 0x001A2390
		internal void RemoveVariable(PSVariable variable, bool force)
		{
			if (variable == null)
			{
				throw PSTraceSource.NewArgumentNullException("variable");
			}
			VariablePath variablePath = new VariablePath(variable.Name);
			SessionStateScope sessionStateScope = null;
			if (this.GetVariableItem(variablePath, out sessionStateScope) != null)
			{
				sessionStateScope.RemoveVariable(variablePath.QualifiedName, force);
			}
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x001A41D1 File Offset: 0x001A23D1
		internal void RemoveVariableAtScope(string name, string scopeID)
		{
			this.RemoveVariableAtScope(name, scopeID, false);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001A41DC File Offset: 0x001A23DC
		internal void RemoveVariableAtScope(string name, string scopeID, bool force)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			VariablePath variablePath = new VariablePath(name);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			if (variablePath.IsVariable)
			{
				scopeByID.RemoveVariable(variablePath.QualifiedName, force);
				return;
			}
			PSDriveInfo drive = scopeByID.GetDrive(variablePath.DriveName);
			if (drive != null)
			{
				CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.ExecutionContext);
				cmdletProviderContext.Drive = drive;
				cmdletProviderContext.Force = force;
				this.RemoveItem(new string[]
				{
					variablePath.QualifiedName
				}, false, cmdletProviderContext);
				cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			}
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001A427A File Offset: 0x001A247A
		internal void RemoveVariableAtScope(PSVariable variable, string scopeID)
		{
			this.RemoveVariableAtScope(variable, scopeID, false);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001A4288 File Offset: 0x001A2488
		internal void RemoveVariableAtScope(PSVariable variable, string scopeID, bool force)
		{
			if (variable == null)
			{
				throw PSTraceSource.NewArgumentNullException("variable");
			}
			VariablePath variablePath = new VariablePath(variable.Name);
			SessionStateScope scopeByID = this.GetScopeByID(scopeID);
			scopeByID.RemoveVariable(variablePath.QualifiedName, force);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x001A42C4 File Offset: 0x001A24C4
		internal IDictionary<string, PSVariable> GetVariableTable()
		{
			SessionStateScopeEnumerator sessionStateScopeEnumerator = new SessionStateScopeEnumerator(this.currentScope);
			Dictionary<string, PSVariable> result = new Dictionary<string, PSVariable>(StringComparer.OrdinalIgnoreCase);
			foreach (SessionStateScope sessionStateScope in ((IEnumerable<SessionStateScope>)sessionStateScopeEnumerator))
			{
				this.GetScopeVariableTable(sessionStateScope, result, sessionStateScope == this.currentScope);
			}
			return result;
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001A4330 File Offset: 0x001A2530
		private void GetScopeVariableTable(SessionStateScope scope, Dictionary<string, PSVariable> result, bool includePrivate)
		{
			foreach (KeyValuePair<string, PSVariable> keyValuePair in scope.Variables)
			{
				if (!result.ContainsKey(keyValuePair.Key))
				{
					PSVariable value = keyValuePair.Value;
					if (!value.IsPrivate || includePrivate)
					{
						result.Add(keyValuePair.Key, value);
					}
				}
			}
			foreach (MutableTuple mutableTuple in scope.DottedScopes)
			{
				mutableTuple.GetVariableTable(result, includePrivate);
			}
			if (scope.LocalsTuple != null)
			{
				scope.LocalsTuple.GetVariableTable(result, includePrivate);
			}
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001A4400 File Offset: 0x001A2600
		internal IDictionary<string, PSVariable> GetVariableTableAtScope(string scopeID)
		{
			Dictionary<string, PSVariable> result = new Dictionary<string, PSVariable>(StringComparer.OrdinalIgnoreCase);
			this.GetScopeVariableTable(this.GetScopeByID(scopeID), result, true);
			return result;
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06004F39 RID: 20281 RVA: 0x001A4428 File Offset: 0x001A2628
		internal List<PSVariable> ExportedVariables
		{
			get
			{
				return this._exportedVariables;
			}
		}

		// Token: 0x0400283A RID: 10298
		private const string startingDefaultStackName = "default";

		// Token: 0x0400283B RID: 10299
		[TraceSource("SessionState", "SessionState Class")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("SessionState", "SessionState Class");

		// Token: 0x0400283C RID: 10300
		private LocationGlobber globberPrivate;

		// Token: 0x0400283D RID: 10301
		private ExecutionContext _context;

		// Token: 0x0400283E RID: 10302
		private SessionState _publicSessionState;

		// Token: 0x0400283F RID: 10303
		private ProviderIntrinsics _invokeProvider;

		// Token: 0x04002840 RID: 10304
		private PSModuleInfo _module;

		// Token: 0x04002841 RID: 10305
		internal List<string> ModuleTableKeys = new List<string>();

		// Token: 0x04002842 RID: 10306
		private Dictionary<string, PSModuleInfo> _moduleTable = new Dictionary<string, PSModuleInfo>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002843 RID: 10307
		private List<string> _scripts = new List<string>(new string[]
		{
			"*"
		});

		// Token: 0x04002844 RID: 10308
		private List<string> _applications = new List<string>(new string[]
		{
			"*"
		});

		// Token: 0x04002845 RID: 10309
		private List<CmdletInfo> _exportedCmdlets = new List<CmdletInfo>();

		// Token: 0x04002846 RID: 10310
		internal SessionStateEntryVisibility DefaultCommandVisibility;

		// Token: 0x04002847 RID: 10311
		private List<AliasInfo> _exportedAliases = new List<AliasInfo>();

		// Token: 0x04002848 RID: 10312
		private PSDriveInfo currentDrive;

		// Token: 0x04002849 RID: 10313
		private static char[] _charactersInvalidInDriveName = new char[]
		{
			':',
			'/',
			'\\',
			'.',
			'~'
		};

		// Token: 0x0400284A RID: 10314
		private List<FunctionInfo> _exportedFunctions = new List<FunctionInfo>();

		// Token: 0x0400284B RID: 10315
		private List<WorkflowInfo> _exportedWorkflows = new List<WorkflowInfo>();

		// Token: 0x0400284C RID: 10316
		private bool _useExportList;

		// Token: 0x0400284D RID: 10317
		private Dictionary<string, Stack<PathInfo>> workingLocationStack;

		// Token: 0x0400284E RID: 10318
		private string defaultStackName = "default";

		// Token: 0x0400284F RID: 10319
		private Dictionary<string, List<ProviderInfo>> _providers = new Dictionary<string, List<ProviderInfo>>(100, StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002850 RID: 10320
		private Dictionary<ProviderInfo, PSDriveInfo> _providersCurrentWorkingDrive = new Dictionary<ProviderInfo, PSDriveInfo>();

		// Token: 0x04002851 RID: 10321
		private bool _providersInitialized;

		// Token: 0x04002852 RID: 10322
		private SessionStateScope currentScope;

		// Token: 0x04002853 RID: 10323
		private readonly SessionStateScope _globalScope;

		// Token: 0x04002854 RID: 10324
		private readonly SessionStateScope _moduleScope;

		// Token: 0x04002855 RID: 10325
		private List<PSVariable> _exportedVariables = new List<PSVariable>();
	}
}
