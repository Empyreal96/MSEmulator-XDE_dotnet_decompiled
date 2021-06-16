using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Reflection;
using System.Security;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020003FC RID: 1020
	public abstract class RunspaceConfiguration
	{
		// Token: 0x06002E0B RID: 11787 RVA: 0x000FE2B0 File Offset: 0x000FC4B0
		public static RunspaceConfiguration Create(string assemblyName)
		{
			if (string.IsNullOrEmpty(assemblyName))
			{
				throw PSTraceSource.NewArgumentNullException("assemblyName");
			}
			Assembly assembly = null;
			foreach (Assembly assembly2 in ClrFacade.GetAssemblies(null))
			{
				if (string.Equals(assembly2.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase))
				{
					assembly = assembly2;
					break;
				}
			}
			if (assembly == null)
			{
				assembly = Assembly.Load(new AssemblyName(assemblyName));
			}
			return RunspaceConfiguration.Create(assembly);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000FE340 File Offset: 0x000FC540
		public static RunspaceConfiguration Create(string consoleFilePath, out PSConsoleLoadException warnings)
		{
			return RunspaceConfigForSingleShell.Create(consoleFilePath, out warnings);
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000FE349 File Offset: 0x000FC549
		public static RunspaceConfiguration Create()
		{
			return RunspaceConfigForSingleShell.CreateDefaultConfiguration();
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000FE350 File Offset: 0x000FC550
		private static RunspaceConfiguration Create(Assembly assembly)
		{
			if (assembly == null)
			{
				throw PSTraceSource.NewArgumentNullException("assembly");
			}
			object[] customAttributes = ClrFacade.GetCustomAttributes<RunspaceConfigurationTypeAttribute>(assembly);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				throw new RunspaceConfigurationAttributeException("RunspaceConfigurationAttributeNotExist", assembly.FullName);
			}
			if (customAttributes.Length > 1)
			{
				throw new RunspaceConfigurationAttributeException("RunspaceConfigurationAttributeDuplicate", assembly.FullName);
			}
			RunspaceConfigurationTypeAttribute runspaceConfigurationTypeAttribute = (RunspaceConfigurationTypeAttribute)customAttributes[0];
			RunspaceConfiguration result;
			try
			{
				Type type = assembly.GetType(runspaceConfigurationTypeAttribute.RunspaceConfigurationType, true, false);
				result = RunspaceConfiguration.Create(type);
			}
			catch (SecurityException)
			{
				throw new RunspaceConfigurationTypeException(assembly.FullName, runspaceConfigurationTypeAttribute.RunspaceConfigurationType);
			}
			return result;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000FE3F0 File Offset: 0x000FC5F0
		private static RunspaceConfiguration Create(Type runspaceConfigType)
		{
			MethodInfo method = runspaceConfigType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				return null;
			}
			return (RunspaceConfiguration)method.Invoke(null, null);
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06002E10 RID: 11792
		public abstract string ShellId { get; }

		// Token: 0x06002E11 RID: 11793 RVA: 0x000FE423 File Offset: 0x000FC623
		public PSSnapInInfo AddPSSnapIn(string name, out PSSnapInException warning)
		{
			return this.DoAddPSSnapIn(name, out warning);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000FE42D File Offset: 0x000FC62D
		internal virtual PSSnapInInfo DoAddPSSnapIn(string name, out PSSnapInException warning)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000FE434 File Offset: 0x000FC634
		public PSSnapInInfo RemovePSSnapIn(string name, out PSSnapInException warning)
		{
			return this.DoRemovePSSnapIn(name, out warning);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000FE43E File Offset: 0x000FC63E
		internal virtual PSSnapInInfo DoRemovePSSnapIn(string name, out PSSnapInException warning)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06002E15 RID: 11797 RVA: 0x000FE445 File Offset: 0x000FC645
		public virtual RunspaceConfigurationEntryCollection<CmdletConfigurationEntry> Cmdlets
		{
			get
			{
				if (this._cmdlets == null)
				{
					this._cmdlets = new RunspaceConfigurationEntryCollection<CmdletConfigurationEntry>();
				}
				return this._cmdlets;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002E16 RID: 11798 RVA: 0x000FE460 File Offset: 0x000FC660
		public virtual RunspaceConfigurationEntryCollection<ProviderConfigurationEntry> Providers
		{
			get
			{
				if (this._providers == null)
				{
					this._providers = new RunspaceConfigurationEntryCollection<ProviderConfigurationEntry>();
				}
				return this._providers;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002E17 RID: 11799 RVA: 0x000FE47B File Offset: 0x000FC67B
		internal TypeTable TypeTable
		{
			get
			{
				if (this.typeTable == null)
				{
					this.typeTable = new TypeTable();
				}
				return this.typeTable;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002E18 RID: 11800 RVA: 0x000FE496 File Offset: 0x000FC696
		public virtual RunspaceConfigurationEntryCollection<TypeConfigurationEntry> Types
		{
			get
			{
				if (this._types == null)
				{
					this._types = new RunspaceConfigurationEntryCollection<TypeConfigurationEntry>();
				}
				return this._types;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002E19 RID: 11801 RVA: 0x000FE4B1 File Offset: 0x000FC6B1
		public virtual RunspaceConfigurationEntryCollection<FormatConfigurationEntry> Formats
		{
			get
			{
				if (this._formats == null)
				{
					this._formats = new RunspaceConfigurationEntryCollection<FormatConfigurationEntry>();
				}
				return this._formats;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x000FE4CC File Offset: 0x000FC6CC
		internal TypeInfoDataBaseManager FormatDBManager
		{
			get
			{
				return this.formatDBManger;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x000FE4D4 File Offset: 0x000FC6D4
		public virtual RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> Scripts
		{
			get
			{
				if (this._scripts == null)
				{
					this._scripts = new RunspaceConfigurationEntryCollection<ScriptConfigurationEntry>();
				}
				return this._scripts;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002E1C RID: 11804 RVA: 0x000FE4EF File Offset: 0x000FC6EF
		public virtual RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> InitializationScripts
		{
			get
			{
				if (this._initializationScripts == null)
				{
					this._initializationScripts = new RunspaceConfigurationEntryCollection<ScriptConfigurationEntry>();
				}
				return this._initializationScripts;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000FE50A File Offset: 0x000FC70A
		public virtual RunspaceConfigurationEntryCollection<AssemblyConfigurationEntry> Assemblies
		{
			get
			{
				if (this._assemblies == null)
				{
					this._assemblies = new RunspaceConfigurationEntryCollection<AssemblyConfigurationEntry>();
				}
				return this._assemblies;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06002E1E RID: 11806 RVA: 0x000FE525 File Offset: 0x000FC725
		public virtual AuthorizationManager AuthorizationManager
		{
			get
			{
				if (this._authorizationManager == null)
				{
					this._authorizationManager = new PSAuthorizationManager(this.ShellId);
				}
				return this._authorizationManager;
			}
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000FE548 File Offset: 0x000FC748
		internal void Bind(ExecutionContext executionContext)
		{
			this._host = executionContext.EngineHostInterface;
			this.Initialize(executionContext);
			this.Assemblies.OnUpdate += executionContext.UpdateAssemblyCache;
			RunspaceConfiguration.runspaceInitTracer.WriteLine("initializing assembly list", new object[0]);
			try
			{
				this.Assemblies.Update(true);
			}
			catch (RuntimeException ex)
			{
				RunspaceConfiguration.runspaceInitTracer.WriteLine("assembly list initialization failed", new object[0]);
				MshLog.LogEngineHealthEvent(executionContext, 103, ex, Severity.Error);
				executionContext.ReportEngineStartupError(ex.Message);
				throw;
			}
			if (executionContext.CommandDiscovery != null)
			{
				this.Cmdlets.OnUpdate += executionContext.CommandDiscovery.UpdateCmdletCache;
				RunspaceConfiguration.runspaceInitTracer.WriteLine("initializing cmdlet list", new object[0]);
				try
				{
					this.Cmdlets.Update(true);
				}
				catch (PSNotSupportedException ex2)
				{
					RunspaceConfiguration.runspaceInitTracer.WriteLine("cmdlet list initialization failed", new object[0]);
					MshLog.LogEngineHealthEvent(executionContext, 103, ex2, Severity.Error);
					executionContext.ReportEngineStartupError(ex2.Message);
					throw;
				}
			}
			if (executionContext.EngineSessionState != null)
			{
				this.Providers.OnUpdate += executionContext.EngineSessionState.UpdateProviders;
				RunspaceConfiguration.runspaceInitTracer.WriteLine("initializing provider list", new object[0]);
				try
				{
					this.Providers.Update(true);
				}
				catch (PSNotSupportedException ex3)
				{
					RunspaceConfiguration.runspaceInitTracer.WriteLine("provider list initialization failed", new object[0]);
					MshLog.LogEngineHealthEvent(executionContext, 103, ex3, Severity.Error);
					executionContext.ReportEngineStartupError(ex3.Message);
					throw;
				}
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000FE6E4 File Offset: 0x000FC8E4
		internal void Unbind(ExecutionContext executionContext)
		{
			if (executionContext == null)
			{
				return;
			}
			if (executionContext.CommandDiscovery != null)
			{
				this.Cmdlets.OnUpdate -= executionContext.CommandDiscovery.UpdateCmdletCache;
			}
			if (executionContext.EngineSessionState != null)
			{
				this.Providers.OnUpdate -= executionContext.EngineSessionState.UpdateProviders;
			}
			this.Assemblies.OnUpdate -= executionContext.UpdateAssemblyCache;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000FE754 File Offset: 0x000FC954
		internal void Initialize(ExecutionContext executionContext)
		{
			lock (this._syncObject)
			{
				if (!this._initialized)
				{
					this._initialized = true;
					this.Types.OnUpdate += this.UpdateTypes;
					this.Formats.OnUpdate += this.UpdateFormats;
					RunspaceConfiguration.runspaceInitTracer.WriteLine("initializing types information", new object[0]);
					try
					{
						this.UpdateTypes();
					}
					catch (RuntimeException ex)
					{
						RunspaceConfiguration.runspaceInitTracer.WriteLine("type information initialization failed", new object[0]);
						MshLog.LogEngineHealthEvent(executionContext, 103, ex, Severity.Warning);
						executionContext.ReportEngineStartupError(ex.Message);
					}
					RunspaceConfiguration.runspaceInitTracer.WriteLine("initializing format information", new object[0]);
					try
					{
						this.UpdateFormats(true);
					}
					catch (RuntimeException ex2)
					{
						RunspaceConfiguration.runspaceInitTracer.WriteLine("format information initialization failed", new object[0]);
						MshLog.LogEngineHealthEvent(executionContext, 103, ex2, Severity.Warning);
						executionContext.ReportEngineStartupError(ex2.Message);
					}
				}
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000FE880 File Offset: 0x000FCA80
		internal void UpdateTypes()
		{
			Collection<string> independentErrors = new Collection<string>();
			Collection<int> collection = new Collection<int>();
			Collection<PSSnapInTypeAndFormatErrors> formatAndTypesErrors = FormatAndTypeDataHelper.GetFormatAndTypesErrors(this, this._host, this.Types, RunspaceConfigurationCategory.Types, independentErrors, collection);
			if (collection.Count > 0)
			{
				this.RemoveNeedlessEntries(RunspaceConfigurationCategory.Types, collection);
			}
			this.TypeTable.Update(formatAndTypesErrors, this._authorizationManager, this._host);
			FormatAndTypeDataHelper.ThrowExceptionOnError("ErrorsUpdatingTypes", independentErrors, formatAndTypesErrors, RunspaceConfigurationCategory.Types);
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000FE8E5 File Offset: 0x000FCAE5
		internal void UpdateFormats()
		{
			this.UpdateFormats(false);
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000FE8F0 File Offset: 0x000FCAF0
		private void UpdateFormats(bool preValidated)
		{
			Collection<string> independentErrors = new Collection<string>();
			Collection<int> collection = new Collection<int>();
			Collection<PSSnapInTypeAndFormatErrors> formatAndTypesErrors = FormatAndTypeDataHelper.GetFormatAndTypesErrors(this, this._host, this.Formats, RunspaceConfigurationCategory.Formats, independentErrors, collection);
			if (collection.Count > 0)
			{
				this.RemoveNeedlessEntries(RunspaceConfigurationCategory.Formats, collection);
			}
			this.FormatDBManager.UpdateDataBase(formatAndTypesErrors, this.AuthorizationManager, this._host, preValidated);
			FormatAndTypeDataHelper.ThrowExceptionOnError("ErrorsUpdatingFormats", independentErrors, formatAndTypesErrors, RunspaceConfigurationCategory.Formats);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000FE958 File Offset: 0x000FCB58
		private void RemoveNeedlessEntries(RunspaceConfigurationCategory category, IList<int> entryIndicesToRemove)
		{
			for (int i = entryIndicesToRemove.Count - 1; i >= 0; i--)
			{
				if (category == RunspaceConfigurationCategory.Types)
				{
					this.Types.RemoveItem(entryIndicesToRemove[i]);
				}
				else if (category == RunspaceConfigurationCategory.Formats)
				{
					this.Formats.RemoveItem(entryIndicesToRemove[i]);
				}
			}
		}

		// Token: 0x04001831 RID: 6193
		private RunspaceConfigurationEntryCollection<CmdletConfigurationEntry> _cmdlets;

		// Token: 0x04001832 RID: 6194
		private RunspaceConfigurationEntryCollection<ProviderConfigurationEntry> _providers;

		// Token: 0x04001833 RID: 6195
		private TypeTable typeTable;

		// Token: 0x04001834 RID: 6196
		private RunspaceConfigurationEntryCollection<TypeConfigurationEntry> _types;

		// Token: 0x04001835 RID: 6197
		private RunspaceConfigurationEntryCollection<FormatConfigurationEntry> _formats;

		// Token: 0x04001836 RID: 6198
		private TypeInfoDataBaseManager formatDBManger = new TypeInfoDataBaseManager();

		// Token: 0x04001837 RID: 6199
		private RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> _scripts;

		// Token: 0x04001838 RID: 6200
		private RunspaceConfigurationEntryCollection<ScriptConfigurationEntry> _initializationScripts;

		// Token: 0x04001839 RID: 6201
		private RunspaceConfigurationEntryCollection<AssemblyConfigurationEntry> _assemblies;

		// Token: 0x0400183A RID: 6202
		private AuthorizationManager _authorizationManager;

		// Token: 0x0400183B RID: 6203
		private PSHost _host;

		// Token: 0x0400183C RID: 6204
		private bool _initialized;

		// Token: 0x0400183D RID: 6205
		private object _syncObject = new object();

		// Token: 0x0400183E RID: 6206
		[TraceSource("RunspaceInit", "Initialization code for Runspace")]
		private static PSTraceSource runspaceInitTracer = PSTraceSource.GetTracer("RunspaceInit", "Initialization code for Runspace", false);
	}
}
