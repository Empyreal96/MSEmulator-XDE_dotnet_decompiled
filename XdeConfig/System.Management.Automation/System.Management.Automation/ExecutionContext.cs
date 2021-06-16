using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x020000C7 RID: 199
	internal class ExecutionContext
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0004043B File Offset: 0x0003E63B
		internal PSLocalEventManager Events
		{
			get
			{
				return this.eventManager;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00040443 File Offset: 0x0003E643
		internal HashSet<string> AutoLoadingModuleInProgress
		{
			get
			{
				return this._autoLoadingModuleInProgress;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0004044B File Offset: 0x0003E64B
		internal ScriptDebugger Debugger
		{
			get
			{
				return this._debugger;
			}
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00040454 File Offset: 0x0003E654
		internal void ResetManagers()
		{
			if (this._debugger != null)
			{
				this._debugger.ResetDebugger();
			}
			if (this.eventManager != null)
			{
				this.eventManager.Dispose();
			}
			this.eventManager = new PSLocalEventManager(this);
			if (this.transactionManager != null)
			{
				this.transactionManager.Dispose();
			}
			this.transactionManager = new PSTransactionManager();
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x000404B1 File Offset: 0x0003E6B1
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x000404C3 File Offset: 0x0003E6C3
		internal int PSDebugTraceLevel
		{
			get
			{
				if (!this.ignoreScriptDebug)
				{
					return this.debugTraceLevel;
				}
				return 0;
			}
			set
			{
				this.debugTraceLevel = value;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000404CC File Offset: 0x0003E6CC
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x000404DE File Offset: 0x0003E6DE
		internal bool PSDebugTraceStep
		{
			get
			{
				return !this.ignoreScriptDebug && this.debugTraceStep;
			}
			set
			{
				this.debugTraceStep = value;
			}
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000404E7 File Offset: 0x0003E6E7
		internal static bool IsStrictVersion(ExecutionContext context, int majorVersion)
		{
			if (context == null)
			{
				context = LocalPipeline.GetExecutionContextFromTLS();
			}
			return context != null && context.IsStrictVersion(majorVersion);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00040500 File Offset: 0x0003E700
		internal bool IsStrictVersion(int majorVersion)
		{
			for (SessionStateScope sessionStateScope = this.EngineSessionState.CurrentScope; sessionStateScope != null; sessionStateScope = sessionStateScope.Parent)
			{
				if (sessionStateScope.StrictModeVersion != null)
				{
					return sessionStateScope.StrictModeVersion.Major >= majorVersion;
				}
				if (sessionStateScope == this.EngineSessionState.ModuleScope)
				{
					break;
				}
			}
			return false;
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00040554 File Offset: 0x0003E754
		internal bool ShouldTraceStatement
		{
			get
			{
				return !this.ignoreScriptDebug && (this.debugTraceLevel > 0 || this.debugTraceStep);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00040571 File Offset: 0x0003E771
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x00040579 File Offset: 0x0003E779
		internal bool ScriptCommandProcessorShouldRethrowExit
		{
			get
			{
				return this._scriptCommandProcessorShouldRethrowExit;
			}
			set
			{
				this._scriptCommandProcessorShouldRethrowExit = value;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0004058B File Offset: 0x0003E78B
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00040582 File Offset: 0x0003E782
		internal bool IgnoreScriptDebug
		{
			get
			{
				return this.ignoreScriptDebug;
			}
			set
			{
				this.ignoreScriptDebug = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00040593 File Offset: 0x0003E793
		internal AutomationEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0004059B File Offset: 0x0003E79B
		internal RunspaceConfiguration RunspaceConfiguration
		{
			get
			{
				return this._runspaceConfiguration;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x000405A3 File Offset: 0x0003E7A3
		internal InitialSessionState InitialSessionState
		{
			get
			{
				return this._initialSessionState;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000405AC File Offset: 0x0003E7AC
		internal bool IsSingleShell
		{
			get
			{
				return this.RunspaceConfiguration is RunspaceConfigForSingleShell || this.InitialSessionState != null;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x000405D6 File Offset: 0x0003E7D6
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x000405DE File Offset: 0x0003E7DE
		internal string PreviousModuleProcessed
		{
			get
			{
				return this._previousModuleProcessed;
			}
			set
			{
				this._previousModuleProcessed = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x000405E7 File Offset: 0x0003E7E7
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x000405EF File Offset: 0x0003E7EF
		internal Hashtable previousModuleImported
		{
			get
			{
				return this._previousModuleImported;
			}
			set
			{
				this._previousModuleImported = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x000405F8 File Offset: 0x0003E7F8
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x00040600 File Offset: 0x0003E800
		internal string ModuleBeingProcessed
		{
			get
			{
				return this._moduleBeingProcessed;
			}
			set
			{
				this._moduleBeingProcessed = value;
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00040609 File Offset: 0x0003E809
		internal bool TakeResponsibilityForModuleAnalysisAppDomain()
		{
			if (this._responsibilityForModuleAnalysisAppDomainOwned)
			{
				return false;
			}
			this._responsibilityForModuleAnalysisAppDomainOwned = true;
			return true;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0004061D File Offset: 0x0003E81D
		internal void ReleaseResponsibilityForModuleAnalysisAppDomain()
		{
			if (this.AppDomainForModuleAnalysis != null)
			{
				AppDomain.Unload(this.AppDomainForModuleAnalysis);
				this.AppDomainForModuleAnalysis = null;
			}
			this._responsibilityForModuleAnalysisAppDomainOwned = false;
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00040640 File Offset: 0x0003E840
		// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x00040648 File Offset: 0x0003E848
		internal AppDomain AppDomainForModuleAnalysis { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00040651 File Offset: 0x0003E851
		internal AuthorizationManager AuthorizationManager
		{
			get
			{
				return this._authorizationManager;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00040659 File Offset: 0x0003E859
		internal ProviderNames ProviderNames
		{
			get
			{
				if (this.providerNames == null)
				{
					if (this.IsSingleShell)
					{
						this.providerNames = new SingleShellProviderNames();
					}
					else
					{
						this.providerNames = new CustomShellProviderNames();
					}
				}
				return this.providerNames;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00040689 File Offset: 0x0003E889
		internal ModuleIntrinsics Modules
		{
			get
			{
				return this._modules;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00040694 File Offset: 0x0003E894
		internal string ShellID
		{
			get
			{
				if (this._shellId == null)
				{
					if (this._authorizationManager is PSAuthorizationManager && !string.IsNullOrEmpty(this._authorizationManager.ShellId))
					{
						this._shellId = this._authorizationManager.ShellId;
					}
					else if (this._runspaceConfiguration != null && !string.IsNullOrEmpty(this._runspaceConfiguration.ShellId))
					{
						this._shellId = this._runspaceConfiguration.ShellId;
					}
					else
					{
						this._shellId = Utils.DefaultPowerShellShellID;
					}
				}
				return this._shellId;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00040719 File Offset: 0x0003E919
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x00040721 File Offset: 0x0003E921
		internal SessionStateInternal EngineSessionState
		{
			get
			{
				return this._engineSessionState;
			}
			set
			{
				this._engineSessionState = value;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0004072A File Offset: 0x0003E92A
		internal SessionStateInternal TopLevelSessionState
		{
			get
			{
				return this._topLevelSessionState;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00040732 File Offset: 0x0003E932
		internal SessionState SessionState
		{
			get
			{
				return this._engineSessionState.PublicSessionState;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0004073F File Offset: 0x0003E93F
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x00040748 File Offset: 0x0003E948
		internal PSLanguageMode LanguageMode
		{
			get
			{
				return this._languageMode;
			}
			set
			{
				if (value == PSLanguageMode.ConstrainedLanguage)
				{
					ExecutionContext.HasEverUsedConstrainedLanguage = true;
					this.HasRunspaceEverUsedConstrainedLanguageMode = true;
					PSSetMemberBinder.InvalidateCache();
					PSInvokeMemberBinder.InvalidateCache();
					PSConvertBinder.InvalidateCache();
					PSBinaryOperationBinder.InvalidateCache();
					PSGetIndexBinder.InvalidateCache();
					PSSetIndexBinder.InvalidateCache();
					PSCreateInstanceBinder.InvalidateCache();
				}
				LanguagePrimitives.RebuildConversionCache();
				this._languageMode = value;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00040795 File Offset: 0x0003E995
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x0004079D File Offset: 0x0003E99D
		internal bool HasRunspaceEverUsedConstrainedLanguageMode { get; private set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000407A6 File Offset: 0x0003E9A6
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x000407AD File Offset: 0x0003E9AD
		internal static bool HasEverUsedConstrainedLanguage { get; private set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x000407B5 File Offset: 0x0003E9B5
		internal bool UseFullLanguageModeInDebugger
		{
			get
			{
				return this._initialSessionState != null && this._initialSessionState.UseFullLanguageModeInDebugger;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x000407CC File Offset: 0x0003E9CC
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x000407D4 File Offset: 0x0003E9D4
		internal bool IsModuleWithJobSourceAdapterLoaded { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x000407DD File Offset: 0x0003E9DD
		internal LocationGlobber LocationGlobber
		{
			get
			{
				this._locationGlobber = new LocationGlobber(this.SessionState);
				return this._locationGlobber;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x000407F6 File Offset: 0x0003E9F6
		internal Dictionary<string, Assembly> AssemblyCache
		{
			get
			{
				return this._assemblyCache;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x000407FE File Offset: 0x0003E9FE
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x00040806 File Offset: 0x0003EA06
		internal EngineState EngineState
		{
			get
			{
				return this._engineState;
			}
			set
			{
				this._engineState = value;
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00040810 File Offset: 0x0003EA10
		internal object GetVariableValue(VariablePath path)
		{
			CmdletProviderContext cmdletProviderContext;
			SessionStateScope sessionStateScope;
			return this._engineSessionState.GetVariableValue(path, out cmdletProviderContext, out sessionStateScope);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00040830 File Offset: 0x0003EA30
		internal object GetVariableValue(VariablePath path, object defaultValue)
		{
			CmdletProviderContext cmdletProviderContext;
			SessionStateScope sessionStateScope;
			return this._engineSessionState.GetVariableValue(path, out cmdletProviderContext, out sessionStateScope) ?? defaultValue;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00040852 File Offset: 0x0003EA52
		internal void SetVariable(VariablePath path, object newValue)
		{
			this._engineSessionState.SetVariable(path, newValue, true, CommandOrigin.Internal);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00040864 File Offset: 0x0003EA64
		internal T GetEnumPreference<T>(VariablePath preferenceVariablePath, T defaultPref, out bool defaultUsed)
		{
			CmdletProviderContext cmdletProviderContext = null;
			SessionStateScope sessionStateScope = null;
			object variableValue = this.EngineSessionState.GetVariableValue(preferenceVariablePath, out cmdletProviderContext, out sessionStateScope);
			if (variableValue is T)
			{
				if (variableValue is ActionPreference)
				{
					ActionPreference actionPreference = (ActionPreference)variableValue;
					if (actionPreference == ActionPreference.Ignore || actionPreference == ActionPreference.Suspend)
					{
						this.EngineSessionState.SetVariableValue(preferenceVariablePath.UserPath, defaultPref);
						string message = StringUtil.Format(ErrorPackage.UnsupportedPreferenceError, actionPreference);
						throw new NotSupportedException(message);
					}
				}
				T result = (T)((object)variableValue);
				defaultUsed = false;
				return result;
			}
			defaultUsed = true;
			T result2 = defaultPref;
			if (variableValue != null)
			{
				try
				{
					string text = variableValue as string;
					if (text != null)
					{
						result2 = (T)((object)Enum.Parse(typeof(T), text, true));
						defaultUsed = false;
					}
					else
					{
						result2 = (T)((object)PSObject.Base(variableValue));
						defaultUsed = false;
					}
				}
				catch (InvalidCastException)
				{
				}
				catch (ArgumentException)
				{
				}
			}
			return result2;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00040950 File Offset: 0x0003EB50
		internal bool GetBooleanPreference(VariablePath preferenceVariablePath, bool defaultPref, out bool defaultUsed)
		{
			CmdletProviderContext cmdletProviderContext = null;
			SessionStateScope sessionStateScope = null;
			object variableValue = this.EngineSessionState.GetVariableValue(preferenceVariablePath, out cmdletProviderContext, out sessionStateScope);
			if (variableValue == null)
			{
				defaultUsed = true;
				return defaultPref;
			}
			bool result = defaultPref;
			defaultUsed = !LanguagePrimitives.TryConvertTo<bool>(variableValue, out result);
			if (!defaultUsed)
			{
				return result;
			}
			return defaultPref;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00040990 File Offset: 0x0003EB90
		internal HelpSystem HelpSystem
		{
			get
			{
				if (this._helpSystem == null)
				{
					this._helpSystem = new HelpSystem(this);
				}
				return this._helpSystem;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x000409AC File Offset: 0x0003EBAC
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x000409B4 File Offset: 0x0003EBB4
		internal object FormatInfo
		{
			get
			{
				return this._formatInfo;
			}
			set
			{
				this._formatInfo = value;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x000409BD File Offset: 0x0003EBBD
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x000409C5 File Offset: 0x0003EBC5
		internal Dictionary<string, ScriptBlock> CustomArgumentCompleters { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x000409CE File Offset: 0x0003EBCE
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x000409D6 File Offset: 0x0003EBD6
		internal Dictionary<string, ScriptBlock> NativeArgumentCompleters { get; set; }

		// Token: 0x06000B08 RID: 2824 RVA: 0x000409E0 File Offset: 0x0003EBE0
		internal CommandProcessorBase CreateCommand(string command, bool dotSource)
		{
			if (this.commandFactory == null)
			{
				this.commandFactory = new CommandFactory(this);
			}
			CommandProcessorBase commandProcessorBase = this.commandFactory.CreateCommand(command, this.EngineSessionState.CurrentScope.ScopeOrigin, new bool?(!dotSource));
			if (commandProcessorBase != null && commandProcessorBase is ScriptCommandProcessorBase)
			{
				commandProcessorBase.Command.CommandOriginInternal = CommandOrigin.Internal;
			}
			return commandProcessorBase;
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00040A3F File Offset: 0x0003EC3F
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x00040A47 File Offset: 0x0003EC47
		internal CommandProcessorBase CurrentCommandProcessor
		{
			get
			{
				return this.currentCommandProcessor;
			}
			set
			{
				this.currentCommandProcessor = value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00040A50 File Offset: 0x0003EC50
		internal CommandDiscovery CommandDiscovery
		{
			get
			{
				return this._engine.CommandDiscovery;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00040A5D File Offset: 0x0003EC5D
		internal InternalHost EngineHostInterface
		{
			get
			{
				return this.myHostInterface;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00040A65 File Offset: 0x0003EC65
		internal InternalHost InternalHost
		{
			get
			{
				return this.myHostInterface;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00040A6D File Offset: 0x0003EC6D
		internal EngineIntrinsics EngineIntrinsics
		{
			get
			{
				if (this._engineIntrinsics == null)
				{
					this._engineIntrinsics = new EngineIntrinsics(this);
				}
				return this._engineIntrinsics;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00040A89 File Offset: 0x0003EC89
		internal LogContextCache LogContextCache
		{
			get
			{
				return this.logContextCache;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00040A91 File Offset: 0x0003EC91
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00040A99 File Offset: 0x0003EC99
		internal PipelineWriter ExternalSuccessOutput
		{
			get
			{
				return this.externalSuccessOutput;
			}
			set
			{
				this.externalSuccessOutput = value;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00040AA2 File Offset: 0x0003ECA2
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00040AAA File Offset: 0x0003ECAA
		internal PipelineWriter ExternalErrorOutput
		{
			get
			{
				return this._externalErrorOutput;
			}
			set
			{
				this._externalErrorOutput = value;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00040AB3 File Offset: 0x0003ECB3
		// (set) Token: 0x06000B15 RID: 2837 RVA: 0x00040ABB File Offset: 0x0003ECBB
		internal PipelineWriter ExternalProgressOutput
		{
			get
			{
				return this._externalProgressOutput;
			}
			set
			{
				this._externalProgressOutput = value;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00040AC4 File Offset: 0x0003ECC4
		internal ExecutionContext.SavedContextData SaveContextData()
		{
			return new ExecutionContext.SavedContextData(this);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00040ACC File Offset: 0x0003ECCC
		internal void ResetShellFunctionErrorOutputPipe()
		{
			this.shellFunctionErrorOutputPipe = null;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00040AD8 File Offset: 0x0003ECD8
		internal Pipe RedirectErrorPipe(Pipe newPipe)
		{
			Pipe result = this.shellFunctionErrorOutputPipe;
			this.ShellFunctionErrorOutputPipe = newPipe;
			return result;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00040AF4 File Offset: 0x0003ECF4
		internal void RestoreErrorPipe(Pipe pipe)
		{
			this.shellFunctionErrorOutputPipe = pipe;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00040AFD File Offset: 0x0003ECFD
		internal void ResetRedirection()
		{
			this.shellFunctionErrorOutputPipe = null;
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00040B06 File Offset: 0x0003ED06
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x00040B0E File Offset: 0x0003ED0E
		internal Pipe ShellFunctionErrorOutputPipe
		{
			get
			{
				return this.shellFunctionErrorOutputPipe;
			}
			set
			{
				this.shellFunctionErrorOutputPipe = value;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00040B17 File Offset: 0x0003ED17
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00040B1F File Offset: 0x0003ED1F
		internal Pipe ExpressionWarningOutputPipe
		{
			get
			{
				return this.expressionWarningOutputPipe;
			}
			set
			{
				this.expressionWarningOutputPipe = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00040B28 File Offset: 0x0003ED28
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x00040B30 File Offset: 0x0003ED30
		internal Pipe ExpressionVerboseOutputPipe
		{
			get
			{
				return this.expressionVerboseOutputPipe;
			}
			set
			{
				this.expressionVerboseOutputPipe = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00040B39 File Offset: 0x0003ED39
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x00040B41 File Offset: 0x0003ED41
		internal Pipe ExpressionDebugOutputPipe
		{
			get
			{
				return this.expressionDebugOutputPipe;
			}
			set
			{
				this.expressionDebugOutputPipe = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00040B4A File Offset: 0x0003ED4A
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x00040B52 File Offset: 0x0003ED52
		internal Pipe ExpressionInformationOutputPipe
		{
			get
			{
				return this.expressionInformationOutputPipe;
			}
			set
			{
				this.expressionInformationOutputPipe = value;
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00040B5C File Offset: 0x0003ED5C
		internal void AppendDollarError(object obj)
		{
			ErrorRecord errorRecord = obj as ErrorRecord;
			if (errorRecord == null && !(obj is Exception))
			{
				return;
			}
			object dollarErrorVariable = this.DollarErrorVariable;
			ArrayList arrayList = dollarErrorVariable as ArrayList;
			if (arrayList == null)
			{
				return;
			}
			if (arrayList.Count > 0)
			{
				if (arrayList[0] == obj)
				{
					return;
				}
				ErrorRecord errorRecord2 = arrayList[0] as ErrorRecord;
				if (errorRecord2 != null && errorRecord != null && errorRecord2.Exception == errorRecord.Exception)
				{
					return;
				}
			}
			object obj2 = this.EngineSessionState.CurrentScope.ErrorCapacity.FastValue;
			if (obj2 != null)
			{
				try
				{
					obj2 = LanguagePrimitives.ConvertTo(obj2, typeof(int), CultureInfo.InvariantCulture);
				}
				catch (PSInvalidCastException)
				{
				}
				catch (OverflowException)
				{
				}
				catch (Exception)
				{
					throw;
				}
			}
			int num = (obj2 is int) ? ((int)obj2) : 256;
			if (0 > num)
			{
				num = 0;
			}
			else if (32768 < num)
			{
				num = 32768;
			}
			if (0 >= num)
			{
				arrayList.Clear();
				return;
			}
			int num2 = arrayList.Count - (num - 1);
			if (0 < num2)
			{
				arrayList.RemoveRange(num - 1, num2);
			}
			arrayList.Insert(0, obj);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00040C94 File Offset: 0x0003EE94
		internal static void CheckStackDepth()
		{
			try
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
			}
			catch (InsufficientExecutionStackException)
			{
				throw new ScriptCallDepthException();
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00040CC0 File Offset: 0x0003EEC0
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00040CC8 File Offset: 0x0003EEC8
		internal Runspace CurrentRunspace
		{
			get
			{
				return this.currentRunspace;
			}
			set
			{
				this.currentRunspace = value;
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00040CD4 File Offset: 0x0003EED4
		internal void PushPipelineProcessor(PipelineProcessor pp)
		{
			if (this.currentRunspace == null)
			{
				return;
			}
			LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.currentRunspace).GetCurrentlyRunningPipeline();
			if (localPipeline == null)
			{
				return;
			}
			localPipeline.Stopper.Push(pp);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00040D10 File Offset: 0x0003EF10
		internal void PopPipelineProcessor(bool fromSteppablePipeline)
		{
			if (this.currentRunspace == null)
			{
				return;
			}
			LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.currentRunspace).GetCurrentlyRunningPipeline();
			if (localPipeline == null)
			{
				return;
			}
			localPipeline.Stopper.Pop(fromSteppablePipeline);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00040D4C File Offset: 0x0003EF4C
		internal bool CurrentPipelineStopping
		{
			get
			{
				if (this.currentRunspace == null)
				{
					return false;
				}
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.currentRunspace).GetCurrentlyRunningPipeline();
				return localPipeline != null && localPipeline.IsStopping;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00040D84 File Offset: 0x0003EF84
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x00040D8C File Offset: 0x0003EF8C
		internal bool PropagateExceptionsToEnclosingStatementBlock { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00040D95 File Offset: 0x0003EF95
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x00040D9D File Offset: 0x0003EF9D
		internal RuntimeException CurrentExceptionBeingHandled { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00040DA6 File Offset: 0x0003EFA6
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00040DAE File Offset: 0x0003EFAE
		internal bool QuestionMarkVariableValue
		{
			get
			{
				return this._questionMarkVariableValue;
			}
			set
			{
				this._questionMarkVariableValue = value;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00040DB8 File Offset: 0x0003EFB8
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00040E05 File Offset: 0x0003F005
		internal object DollarErrorVariable
		{
			get
			{
				CmdletProviderContext cmdletProviderContext = null;
				SessionStateScope sessionStateScope = null;
				object variableValue;
				if (!this.eventManager.IsExecutingEventAction)
				{
					variableValue = this.EngineSessionState.GetVariableValue(SpecialVariables.ErrorVarPath, out cmdletProviderContext, out sessionStateScope);
				}
				else
				{
					variableValue = this.EngineSessionState.GetVariableValue(SpecialVariables.EventErrorVarPath, out cmdletProviderContext, out sessionStateScope);
				}
				return variableValue;
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.ErrorVarPath, value, true, CommandOrigin.Internal);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00040E1C File Offset: 0x0003F01C
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x00040E39 File Offset: 0x0003F039
		internal ActionPreference DebugPreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ActionPreference>(SpecialVariables.DebugPreferenceVarPath, ActionPreference.SilentlyContinue, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.DebugPreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ActionPreference), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00040E68 File Offset: 0x0003F068
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x00040E85 File Offset: 0x0003F085
		internal ActionPreference VerbosePreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ActionPreference>(SpecialVariables.VerbosePreferenceVarPath, ActionPreference.SilentlyContinue, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.VerbosePreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ActionPreference), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00040EB4 File Offset: 0x0003F0B4
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00040ED1 File Offset: 0x0003F0D1
		internal ActionPreference ErrorActionPreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ActionPreference>(SpecialVariables.ErrorActionPreferenceVarPath, ActionPreference.Continue, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.ErrorActionPreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ActionPreference), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00040F00 File Offset: 0x0003F100
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00040F1D File Offset: 0x0003F11D
		internal ActionPreference WarningActionPreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ActionPreference>(SpecialVariables.WarningPreferenceVarPath, ActionPreference.Continue, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.WarningPreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ActionPreference), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00040F4C File Offset: 0x0003F14C
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00040F69 File Offset: 0x0003F169
		internal ActionPreference InformationActionPreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ActionPreference>(SpecialVariables.InformationPreferenceVarPath, ActionPreference.SilentlyContinue, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.InformationPreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ActionPreference), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00040F98 File Offset: 0x0003F198
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x00040FBF File Offset: 0x0003F1BF
		internal object WhatIfPreferenceVariable
		{
			get
			{
				CmdletProviderContext cmdletProviderContext = null;
				SessionStateScope sessionStateScope = null;
				return this.EngineSessionState.GetVariableValue(SpecialVariables.WhatIfPreferenceVarPath, out cmdletProviderContext, out sessionStateScope);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.WhatIfPreferenceVarPath, value, true, CommandOrigin.Internal);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00040FD8 File Offset: 0x0003F1D8
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00040FF5 File Offset: 0x0003F1F5
		internal ConfirmImpact ConfirmPreferenceVariable
		{
			get
			{
				bool flag = false;
				return this.GetEnumPreference<ConfirmImpact>(SpecialVariables.ConfirmPreferenceVarPath, ConfirmImpact.High, out flag);
			}
			set
			{
				this.EngineSessionState.SetVariable(SpecialVariables.ConfirmPreferenceVarPath, LanguagePrimitives.ConvertTo(value, typeof(ConfirmImpact), CultureInfo.InvariantCulture), true, CommandOrigin.Internal);
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00041024 File Offset: 0x0003F224
		internal void RunspaceClosingNotification()
		{
			if (this.RunspaceConfiguration != null)
			{
				this.RunspaceConfiguration.Unbind(this);
			}
			this.EngineSessionState.RunspaceClosingNotification();
			if (this._debugger != null)
			{
				this._debugger.Dispose();
			}
			if (this.eventManager != null)
			{
				this.eventManager.Dispose();
			}
			this.eventManager = null;
			if (this.transactionManager != null)
			{
				this.transactionManager.Dispose();
			}
			this.transactionManager = null;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00041097 File Offset: 0x0003F297
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x000410D3 File Offset: 0x0003F2D3
		internal TypeTable TypeTable
		{
			get
			{
				if (this.RunspaceConfiguration != null && this.RunspaceConfiguration.TypeTable != null)
				{
					return this.RunspaceConfiguration.TypeTable;
				}
				if (this._typeTable == null)
				{
					this._typeTable = new TypeTable();
				}
				return this._typeTable;
			}
			set
			{
				if (this.RunspaceConfiguration != null)
				{
					throw new NotImplementedException("set_TypeTable()");
				}
				this._typeTable = value;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x000410F0 File Offset: 0x0003F2F0
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0004116C File Offset: 0x0003F36C
		internal TypeInfoDataBaseManager FormatDBManager
		{
			get
			{
				if (this.RunspaceConfiguration != null && this.RunspaceConfiguration.FormatDBManager != null)
				{
					return this.RunspaceConfiguration.FormatDBManager;
				}
				if (this._formatDBManager == null)
				{
					this._formatDBManager = new TypeInfoDataBaseManager();
					this._formatDBManager.Update(this.AuthorizationManager, this.EngineHostInterface);
					if (this.InitialSessionState != null)
					{
						this._formatDBManager.DisableFormatTableUpdates = this.InitialSessionState.DisableFormatUpdates;
					}
				}
				return this._formatDBManager;
			}
			set
			{
				if (this.RunspaceConfiguration != null)
				{
					throw new NotImplementedException("set_FormatDBManager()");
				}
				this._formatDBManager = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00041188 File Offset: 0x0003F388
		internal PSTransactionManager TransactionManager
		{
			get
			{
				return this.transactionManager;
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00041190 File Offset: 0x0003F390
		internal void UpdateAssemblyCache()
		{
			string text = "";
			if (this.RunspaceConfiguration != null)
			{
				if (!this._assemblyCacheInitialized)
				{
					foreach (AssemblyConfigurationEntry assemblyConfigurationEntry in ((IEnumerable<AssemblyConfigurationEntry>)this.RunspaceConfiguration.Assemblies))
					{
						Exception ex = null;
						this.AddAssembly(assemblyConfigurationEntry.Name, assemblyConfigurationEntry.FileName, out ex);
						if (ex != null)
						{
							text = text + "\n" + ex.Message;
						}
					}
					this._assemblyCacheInitialized = true;
				}
				else
				{
					foreach (AssemblyConfigurationEntry assemblyConfigurationEntry2 in this.RunspaceConfiguration.Assemblies.UpdateList)
					{
						switch (assemblyConfigurationEntry2.Action)
						{
						case UpdateAction.Add:
						{
							Exception ex2 = null;
							this.AddAssembly(assemblyConfigurationEntry2.Name, assemblyConfigurationEntry2.FileName, out ex2);
							if (ex2 != null)
							{
								text = text + "\n" + ex2.Message;
							}
							break;
						}
						case UpdateAction.Remove:
							this.RemoveAssembly(assemblyConfigurationEntry2.Name);
							break;
						}
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					string message = StringUtil.Format(MiniShellErrors.UpdateAssemblyErrors, text);
					throw new RuntimeException(message);
				}
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000412E8 File Offset: 0x0003F4E8
		internal Assembly AddAssembly(string name, string filename, out Exception error)
		{
			Assembly assembly = ExecutionContext.LoadAssembly(name, filename, out error);
			if (assembly == null)
			{
				return null;
			}
			if (this._assemblyCache.ContainsKey(assembly.FullName))
			{
				return assembly;
			}
			this._assemblyCache.Add(assembly.FullName, assembly);
			if (this._assemblyCache.ContainsKey(assembly.GetName().Name))
			{
				return assembly;
			}
			this._assemblyCache.Add(assembly.GetName().Name, assembly);
			return assembly;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00041364 File Offset: 0x0003F564
		internal void RemoveAssembly(string name)
		{
			if (!this._assemblyCache.ContainsKey(name))
			{
				return;
			}
			Assembly assembly = this._assemblyCache[name];
			if (assembly != null)
			{
				this._assemblyCache.Remove(name);
				this._assemblyCache.Remove(assembly.GetName().Name);
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000413BC File Offset: 0x0003F5BC
		internal static Assembly LoadAssembly(string name, string filename, out Exception error)
		{
			Assembly assembly = null;
			error = null;
			string text = null;
			if (!string.IsNullOrEmpty(name))
			{
				text = (name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? Path.GetFileNameWithoutExtension(name) : name);
				string assemblyName = Utils.IsPowerShellAssembly(text) ? Utils.GetPowerShellAssemblyStrongName(text) : text;
				try
				{
					assembly = Assembly.Load(new AssemblyName(assemblyName));
				}
				catch (FileNotFoundException ex)
				{
					error = ex;
				}
				catch (FileLoadException ex2)
				{
					error = ex2;
					return null;
				}
				catch (BadImageFormatException ex3)
				{
					error = ex3;
					return null;
				}
				catch (SecurityException ex4)
				{
					error = ex4;
					return null;
				}
			}
			if (assembly != null)
			{
				return assembly;
			}
			if (!string.IsNullOrEmpty(filename))
			{
				error = null;
				try
				{
					assembly = ClrFacade.LoadFrom(filename);
					return assembly;
				}
				catch (FileNotFoundException ex5)
				{
					error = ex5;
				}
				catch (FileLoadException ex6)
				{
					error = ex6;
					return null;
				}
				catch (BadImageFormatException ex7)
				{
					error = ex7;
					return null;
				}
				catch (SecurityException ex8)
				{
					error = ex8;
					return null;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					assembly = Assembly.LoadWithPartialName(text);
					if (assembly != null)
					{
						error = null;
					}
					return assembly;
				}
				catch (BadImageFormatException ex9)
				{
					error = ex9;
				}
			}
			return null;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00041524 File Offset: 0x0003F724
		internal void ReportEngineStartupError(string resourceString, params object[] arguments)
		{
			try
			{
				Cmdlet cmdlet;
				string resourceIdAndErrorId;
				if (this.IsModuleCommandCurrentlyRunning(out cmdlet, out resourceIdAndErrorId))
				{
					RuntimeException ex = InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, resourceIdAndErrorId, resourceString, arguments);
					cmdlet.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
				}
				else
				{
					PSHost engineHostInterface = this.EngineHostInterface;
					if (engineHostInterface != null)
					{
						PSHostUserInterface ui = engineHostInterface.UI;
						if (ui != null)
						{
							ui.WriteErrorLine(StringUtil.Format(resourceString, arguments));
						}
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000415AC File Offset: 0x0003F7AC
		internal void ReportEngineStartupError(string error)
		{
			try
			{
				Cmdlet cmdlet;
				string resourceIdAndErrorId;
				if (this.IsModuleCommandCurrentlyRunning(out cmdlet, out resourceIdAndErrorId))
				{
					RuntimeException ex = InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, resourceIdAndErrorId, "{0}", new object[]
					{
						error
					});
					cmdlet.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
				}
				else
				{
					PSHost engineHostInterface = this.EngineHostInterface;
					if (engineHostInterface != null)
					{
						PSHostUserInterface ui = engineHostInterface.UI;
						if (ui != null)
						{
							ui.WriteErrorLine(error);
						}
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00041640 File Offset: 0x0003F840
		internal void ReportEngineStartupError(Exception e)
		{
			try
			{
				Cmdlet cmdlet;
				string errorId;
				if (this.IsModuleCommandCurrentlyRunning(out cmdlet, out errorId))
				{
					RuntimeException ex = e as RuntimeException;
					ErrorRecord errorRecord = (ex != null) ? new ErrorRecord(ex.ErrorRecord, ex) : new ErrorRecord(e, errorId, ErrorCategory.OperationStopped, null);
					cmdlet.WriteError(errorRecord);
				}
				else
				{
					PSHost engineHostInterface = this.EngineHostInterface;
					if (engineHostInterface != null)
					{
						PSHostUserInterface ui = engineHostInterface.UI;
						if (ui != null)
						{
							ui.WriteErrorLine(e.Message);
						}
					}
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000416D0 File Offset: 0x0003F8D0
		internal void ReportEngineStartupError(ErrorRecord errorRecord)
		{
			try
			{
				Cmdlet cmdlet;
				string text;
				if (this.IsModuleCommandCurrentlyRunning(out cmdlet, out text))
				{
					cmdlet.WriteError(errorRecord);
				}
				else
				{
					PSHost engineHostInterface = this.EngineHostInterface;
					if (engineHostInterface != null)
					{
						PSHostUserInterface ui = engineHostInterface.UI;
						if (ui != null)
						{
							ui.WriteErrorLine(errorRecord.ToString());
						}
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00041734 File Offset: 0x0003F934
		private bool IsModuleCommandCurrentlyRunning(out Cmdlet command, out string errorId)
		{
			command = null;
			errorId = null;
			bool result = false;
			if (this.CurrentCommandProcessor != null)
			{
				CommandInfo commandInfo = this.CurrentCommandProcessor.CommandInfo;
				if ((string.Equals(commandInfo.Name, "Import-Module", StringComparison.OrdinalIgnoreCase) || string.Equals(commandInfo.Name, "Remove-Module", StringComparison.OrdinalIgnoreCase)) && commandInfo.CommandType.Equals(CommandTypes.Cmdlet) && InitialSessionState.CoreModule.Equals(commandInfo.ModuleName, StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					command = (Cmdlet)this.CurrentCommandProcessor.Command;
					errorId = (string.Equals(commandInfo.Name, "Import-Module", StringComparison.OrdinalIgnoreCase) ? "Module_ImportModuleError" : "Module_RemoveModuleError");
				}
			}
			return result;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x000417E8 File Offset: 0x0003F9E8
		internal ExecutionContext(AutomationEngine engine, PSHost hostInterface, RunspaceConfiguration runspaceConfiguration)
		{
			this._runspaceConfiguration = runspaceConfiguration;
			this._authorizationManager = runspaceConfiguration.AuthorizationManager;
			this.InitializeCommon(engine, hostInterface);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0004184C File Offset: 0x0003FA4C
		internal ExecutionContext(AutomationEngine engine, PSHost hostInterface, InitialSessionState initialSessionState)
		{
			this._initialSessionState = initialSessionState;
			this._authorizationManager = initialSessionState.AuthorizationManager;
			this.InitializeCommon(engine, hostInterface);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000418B0 File Offset: 0x0003FAB0
		private void InitializeCommon(AutomationEngine engine, PSHost hostInterface)
		{
			this._engine = engine;
			if (!ExecutionContext._assemblyEventHandlerSet)
			{
				lock (ExecutionContext.lockObject)
				{
					if (!ExecutionContext._assemblyEventHandlerSet)
					{
						AppDomain currentDomain = AppDomain.CurrentDomain;
						currentDomain.AssemblyResolve += ExecutionContext.PowerShellAssemblyResolveHandler;
						ExecutionContext._assemblyEventHandlerSet = true;
					}
				}
			}
			this.eventManager = new PSLocalEventManager(this);
			this.transactionManager = new PSTransactionManager();
			this._debugger = new ScriptDebugger(this);
			this.myHostInterface = (hostInterface as InternalHost);
			if (this.myHostInterface == null)
			{
				this.myHostInterface = new InternalHost(hostInterface, this);
			}
			this._assemblyCache = new Dictionary<string, Assembly>();
			this._topLevelSessionState = (this._engineSessionState = new SessionStateInternal(this));
			if (this._authorizationManager == null)
			{
				this._authorizationManager = new AuthorizationManager(null);
			}
			this._modules = new ModuleIntrinsics(this);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000419A0 File Offset: 0x0003FBA0
		private static Assembly PowerShellAssemblyResolveHandler(object sender, ResolveEventArgs args)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null && executionContextFromTLS._assemblyCache != null && executionContextFromTLS._assemblyCache.ContainsKey(args.Name))
			{
				return executionContextFromTLS._assemblyCache[args.Name];
			}
			return null;
		}

		// Token: 0x040004C6 RID: 1222
		private PSLocalEventManager eventManager;

		// Token: 0x040004C7 RID: 1223
		private HashSet<string> _autoLoadingModuleInProgress = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040004C8 RID: 1224
		private ScriptDebugger _debugger;

		// Token: 0x040004C9 RID: 1225
		internal int _debuggingMode;

		// Token: 0x040004CA RID: 1226
		private int debugTraceLevel;

		// Token: 0x040004CB RID: 1227
		private bool debugTraceStep;

		// Token: 0x040004CC RID: 1228
		private bool _scriptCommandProcessorShouldRethrowExit;

		// Token: 0x040004CD RID: 1229
		private bool ignoreScriptDebug = true;

		// Token: 0x040004CE RID: 1230
		private AutomationEngine _engine;

		// Token: 0x040004CF RID: 1231
		private RunspaceConfiguration _runspaceConfiguration;

		// Token: 0x040004D0 RID: 1232
		private InitialSessionState _initialSessionState;

		// Token: 0x040004D1 RID: 1233
		private string _previousModuleProcessed;

		// Token: 0x040004D2 RID: 1234
		private Hashtable _previousModuleImported = new Hashtable();

		// Token: 0x040004D3 RID: 1235
		private string _moduleBeingProcessed;

		// Token: 0x040004D4 RID: 1236
		private bool _responsibilityForModuleAnalysisAppDomainOwned;

		// Token: 0x040004D5 RID: 1237
		private AuthorizationManager _authorizationManager;

		// Token: 0x040004D6 RID: 1238
		private ProviderNames providerNames;

		// Token: 0x040004D7 RID: 1239
		private ModuleIntrinsics _modules;

		// Token: 0x040004D8 RID: 1240
		private string _shellId;

		// Token: 0x040004D9 RID: 1241
		private SessionStateInternal _engineSessionState;

		// Token: 0x040004DA RID: 1242
		private SessionStateInternal _topLevelSessionState;

		// Token: 0x040004DB RID: 1243
		private PSLanguageMode _languageMode;

		// Token: 0x040004DC RID: 1244
		internal static List<string> ModulesWithJobSourceAdapters = new List<string>
		{
			"PSWorkflow",
			"PSScheduledJob"
		};

		// Token: 0x040004DD RID: 1245
		private LocationGlobber _locationGlobber;

		// Token: 0x040004DE RID: 1246
		private Dictionary<string, Assembly> _assemblyCache;

		// Token: 0x040004DF RID: 1247
		private EngineState _engineState;

		// Token: 0x040004E0 RID: 1248
		private HelpSystem _helpSystem;

		// Token: 0x040004E1 RID: 1249
		private object _formatInfo;

		// Token: 0x040004E2 RID: 1250
		private CommandFactory commandFactory;

		// Token: 0x040004E3 RID: 1251
		private CommandProcessorBase currentCommandProcessor;

		// Token: 0x040004E4 RID: 1252
		private InternalHost myHostInterface;

		// Token: 0x040004E5 RID: 1253
		private EngineIntrinsics _engineIntrinsics;

		// Token: 0x040004E6 RID: 1254
		private LogContextCache logContextCache = new LogContextCache();

		// Token: 0x040004E7 RID: 1255
		private PipelineWriter externalSuccessOutput;

		// Token: 0x040004E8 RID: 1256
		private PipelineWriter _externalErrorOutput;

		// Token: 0x040004E9 RID: 1257
		private PipelineWriter _externalProgressOutput;

		// Token: 0x040004EA RID: 1258
		private Pipe shellFunctionErrorOutputPipe;

		// Token: 0x040004EB RID: 1259
		private Pipe expressionWarningOutputPipe;

		// Token: 0x040004EC RID: 1260
		private Pipe expressionVerboseOutputPipe;

		// Token: 0x040004ED RID: 1261
		private Pipe expressionDebugOutputPipe;

		// Token: 0x040004EE RID: 1262
		private Pipe expressionInformationOutputPipe;

		// Token: 0x040004EF RID: 1263
		private Runspace currentRunspace;

		// Token: 0x040004F0 RID: 1264
		private bool _questionMarkVariableValue = true;

		// Token: 0x040004F1 RID: 1265
		private TypeTable _typeTable;

		// Token: 0x040004F2 RID: 1266
		private TypeInfoDataBaseManager _formatDBManager;

		// Token: 0x040004F3 RID: 1267
		internal PSTransactionManager transactionManager;

		// Token: 0x040004F4 RID: 1268
		private bool _assemblyCacheInitialized;

		// Token: 0x040004F5 RID: 1269
		private static bool _assemblyEventHandlerSet = false;

		// Token: 0x040004F6 RID: 1270
		private static object lockObject = new object();

		// Token: 0x020000C8 RID: 200
		internal class SavedContextData
		{
			// Token: 0x06000B56 RID: 2902 RVA: 0x00041A23 File Offset: 0x0003FC23
			public SavedContextData(ExecutionContext context)
			{
				this.StepScript = context.PSDebugTraceStep;
				this.IgnoreScriptDebug = context.IgnoreScriptDebug;
				this.PSDebug = context.PSDebugTraceLevel;
				this.ShellFunctionErrorOutputPipe = context.ShellFunctionErrorOutputPipe;
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x00041A5B File Offset: 0x0003FC5B
			public void RestoreContextData(ExecutionContext context)
			{
				context.PSDebugTraceStep = this.StepScript;
				context.IgnoreScriptDebug = this.IgnoreScriptDebug;
				context.PSDebugTraceLevel = this.PSDebug;
				context.ShellFunctionErrorOutputPipe = this.ShellFunctionErrorOutputPipe;
			}

			// Token: 0x040004FF RID: 1279
			private bool StepScript;

			// Token: 0x04000500 RID: 1280
			private bool IgnoreScriptDebug;

			// Token: 0x04000501 RID: 1281
			private int PSDebug;

			// Token: 0x04000502 RID: 1282
			private Pipe ShellFunctionErrorOutputPipe;
		}
	}
}
