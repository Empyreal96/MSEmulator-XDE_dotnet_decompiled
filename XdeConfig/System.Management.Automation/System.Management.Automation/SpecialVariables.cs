using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200042D RID: 1069
	internal static class SpecialVariables
	{
		// Token: 0x06002F3D RID: 12093 RVA: 0x00102CA9 File Offset: 0x00100EA9
		internal static bool IsImplicitVariableAccessibleInClassMethod(VariablePath variablePath)
		{
			return SpecialVariables.ClassMethodsAccessibleVariables.Contains(variablePath.UserPath);
		}

		// Token: 0x04001905 RID: 6405
		internal const string HistorySize = "MaximumHistoryCount";

		// Token: 0x04001906 RID: 6406
		internal const string MyInvocation = "MyInvocation";

		// Token: 0x04001907 RID: 6407
		internal const string OFS = "OFS";

		// Token: 0x04001908 RID: 6408
		internal const string OutputEncoding = "OutputEncoding";

		// Token: 0x04001909 RID: 6409
		internal const string VerboseHelpErrors = "VerboseHelpErrors";

		// Token: 0x0400190A RID: 6410
		internal const string LogEngineHealthEvent = "LogEngineHealthEvent";

		// Token: 0x0400190B RID: 6411
		internal const string LogEngineLifecycleEvent = "LogEngineLifecycleEvent";

		// Token: 0x0400190C RID: 6412
		internal const string LogCommandHealthEvent = "LogCommandHealthEvent";

		// Token: 0x0400190D RID: 6413
		internal const string LogCommandLifecycleEvent = "LogCommandLifecycleEvent";

		// Token: 0x0400190E RID: 6414
		internal const string LogProviderHealthEvent = "LogProviderHealthEvent";

		// Token: 0x0400190F RID: 6415
		internal const string LogProviderLifecycleEvent = "LogProviderLifecycleEvent";

		// Token: 0x04001910 RID: 6416
		internal const string LogSettingsEvent = "LogSettingsEvent";

		// Token: 0x04001911 RID: 6417
		internal const string PSLogUserData = "PSLogUserData";

		// Token: 0x04001912 RID: 6418
		internal const string NestedPromptLevel = "NestedPromptLevel";

		// Token: 0x04001913 RID: 6419
		internal const string CurrentlyExecutingCommand = "CurrentlyExecutingCommand";

		// Token: 0x04001914 RID: 6420
		internal const string PSBoundParameters = "PSBoundParameters";

		// Token: 0x04001915 RID: 6421
		internal const string Matches = "Matches";

		// Token: 0x04001916 RID: 6422
		internal const string LastExitCode = "LASTEXITCODE";

		// Token: 0x04001917 RID: 6423
		internal const string PSDebugContext = "PSDebugContext";

		// Token: 0x04001918 RID: 6424
		internal const string StackTrace = "StackTrace";

		// Token: 0x04001919 RID: 6425
		internal const string FirstToken = "^";

		// Token: 0x0400191A RID: 6426
		internal const string LastToken = "$";

		// Token: 0x0400191B RID: 6427
		internal const string PSItem = "PSItem";

		// Token: 0x0400191C RID: 6428
		internal const string Underbar = "_";

		// Token: 0x0400191D RID: 6429
		internal const string Question = "?";

		// Token: 0x0400191E RID: 6430
		internal const string Args = "args";

		// Token: 0x0400191F RID: 6431
		internal const string This = "this";

		// Token: 0x04001920 RID: 6432
		internal const string Input = "input";

		// Token: 0x04001921 RID: 6433
		internal const string PSCmdlet = "PSCmdlet";

		// Token: 0x04001922 RID: 6434
		internal const string Error = "error";

		// Token: 0x04001923 RID: 6435
		internal const string EventError = "error";

		// Token: 0x04001924 RID: 6436
		internal const string PathExt = "env:PATHEXT";

		// Token: 0x04001925 RID: 6437
		internal const string PSEmailServer = "PSEmailServer";

		// Token: 0x04001926 RID: 6438
		internal const string PSDefaultParameterValues = "PSDefaultParameterValues";

		// Token: 0x04001927 RID: 6439
		internal const string PSScriptRoot = "PSScriptRoot";

		// Token: 0x04001928 RID: 6440
		internal const string PSCommandPath = "PSCommandPath";

		// Token: 0x04001929 RID: 6441
		internal const string @foreach = "foreach";

		// Token: 0x0400192A RID: 6442
		internal const string @switch = "switch";

		// Token: 0x0400192B RID: 6443
		internal const string pwd = "PWD";

		// Token: 0x0400192C RID: 6444
		internal const string Null = "null";

		// Token: 0x0400192D RID: 6445
		internal const string True = "true";

		// Token: 0x0400192E RID: 6446
		internal const string False = "false";

		// Token: 0x0400192F RID: 6447
		internal const string PSModuleAutoLoading = "PSModuleAutoLoadingPreference";

		// Token: 0x04001930 RID: 6448
		internal const string DebugPreference = "DebugPreference";

		// Token: 0x04001931 RID: 6449
		internal const string ErrorActionPreference = "ErrorActionPreference";

		// Token: 0x04001932 RID: 6450
		internal const string ProgressPreference = "ProgressPreference";

		// Token: 0x04001933 RID: 6451
		internal const string VerbosePreference = "VerbosePreference";

		// Token: 0x04001934 RID: 6452
		internal const string WarningPreference = "WarningPreference";

		// Token: 0x04001935 RID: 6453
		internal const string WhatIfPreference = "WhatIfPreference";

		// Token: 0x04001936 RID: 6454
		internal const string ConfirmPreference = "ConfirmPreference";

		// Token: 0x04001937 RID: 6455
		internal const string InformationPreference = "InformationPreference";

		// Token: 0x04001938 RID: 6456
		internal const string ErrorView = "ErrorView";

		// Token: 0x04001939 RID: 6457
		internal const string PSSessionConfigurationName = "PSSessionConfigurationName";

		// Token: 0x0400193A RID: 6458
		internal const string PSSessionApplicationName = "PSSessionApplicationName";

		// Token: 0x0400193B RID: 6459
		internal const string ConsoleFileName = "ConsoleFileName";

		// Token: 0x0400193C RID: 6460
		internal const string ExecutionContext = "ExecutionContext";

		// Token: 0x0400193D RID: 6461
		internal const string Home = "HOME";

		// Token: 0x0400193E RID: 6462
		internal const string Host = "Host";

		// Token: 0x0400193F RID: 6463
		internal const string PID = "PID";

		// Token: 0x04001940 RID: 6464
		internal const string PSCulture = "PSCulture";

		// Token: 0x04001941 RID: 6465
		internal const string PSHome = "PSHOME";

		// Token: 0x04001942 RID: 6466
		internal const string PSUICulture = "PSUICulture";

		// Token: 0x04001943 RID: 6467
		internal const string PSVersionTable = "PSVersionTable";

		// Token: 0x04001944 RID: 6468
		internal const string ShellId = "ShellId";

		// Token: 0x04001945 RID: 6469
		internal static readonly VariablePath HistorySizeVarPath = new VariablePath("MaximumHistoryCount");

		// Token: 0x04001946 RID: 6470
		internal static readonly VariablePath MyInvocationVarPath = new VariablePath("MyInvocation");

		// Token: 0x04001947 RID: 6471
		internal static readonly VariablePath OFSVarPath = new VariablePath("OFS");

		// Token: 0x04001948 RID: 6472
		internal static readonly VariablePath OutputEncodingVarPath = new VariablePath("OutputEncoding");

		// Token: 0x04001949 RID: 6473
		internal static readonly VariablePath VerboseHelpErrorsVarPath = new VariablePath("VerboseHelpErrors");

		// Token: 0x0400194A RID: 6474
		internal static readonly VariablePath LogEngineHealthEventVarPath = new VariablePath("LogEngineHealthEvent");

		// Token: 0x0400194B RID: 6475
		internal static readonly VariablePath LogEngineLifecycleEventVarPath = new VariablePath("LogEngineLifecycleEvent");

		// Token: 0x0400194C RID: 6476
		internal static readonly VariablePath LogCommandHealthEventVarPath = new VariablePath("LogCommandHealthEvent");

		// Token: 0x0400194D RID: 6477
		internal static readonly VariablePath LogCommandLifecycleEventVarPath = new VariablePath("LogCommandLifecycleEvent");

		// Token: 0x0400194E RID: 6478
		internal static readonly VariablePath LogProviderHealthEventVarPath = new VariablePath("LogProviderHealthEvent");

		// Token: 0x0400194F RID: 6479
		internal static readonly VariablePath LogProviderLifecycleEventVarPath = new VariablePath("LogProviderLifecycleEvent");

		// Token: 0x04001950 RID: 6480
		internal static readonly VariablePath LogSettingsEventVarPath = new VariablePath("LogSettingsEvent");

		// Token: 0x04001951 RID: 6481
		internal static readonly VariablePath PSLogUserDataPath = new VariablePath("PSLogUserData");

		// Token: 0x04001952 RID: 6482
		internal static readonly VariablePath NestedPromptCounterVarPath = new VariablePath("global:NestedPromptLevel");

		// Token: 0x04001953 RID: 6483
		internal static readonly VariablePath CurrentlyExecutingCommandVarPath = new VariablePath("CurrentlyExecutingCommand");

		// Token: 0x04001954 RID: 6484
		internal static readonly VariablePath PSBoundParametersVarPath = new VariablePath("PSBoundParameters");

		// Token: 0x04001955 RID: 6485
		internal static readonly VariablePath MatchesVarPath = new VariablePath("Matches");

		// Token: 0x04001956 RID: 6486
		internal static readonly VariablePath LastExitCodeVarPath = new VariablePath("global:LASTEXITCODE");

		// Token: 0x04001957 RID: 6487
		internal static readonly VariablePath PSDebugContextVarPath = new VariablePath("PSDebugContext");

		// Token: 0x04001958 RID: 6488
		internal static readonly VariablePath StackTraceVarPath = new VariablePath("global:StackTrace");

		// Token: 0x04001959 RID: 6489
		internal static readonly VariablePath FirstTokenVarPath = new VariablePath("global:^");

		// Token: 0x0400195A RID: 6490
		internal static readonly VariablePath LastTokenVarPath = new VariablePath("global:$");

		// Token: 0x0400195B RID: 6491
		internal static readonly VariablePath UnderbarVarPath = new VariablePath("_");

		// Token: 0x0400195C RID: 6492
		internal static readonly VariablePath QuestionVarPath = new VariablePath("?");

		// Token: 0x0400195D RID: 6493
		internal static readonly VariablePath ArgsVarPath = new VariablePath("local:args");

		// Token: 0x0400195E RID: 6494
		internal static readonly VariablePath ThisVarPath = new VariablePath("this");

		// Token: 0x0400195F RID: 6495
		internal static readonly VariablePath InputVarPath = new VariablePath("local:input");

		// Token: 0x04001960 RID: 6496
		internal static readonly VariablePath PSCmdletVarPath = new VariablePath("PSCmdlet");

		// Token: 0x04001961 RID: 6497
		internal static readonly VariablePath ErrorVarPath = new VariablePath("global:error");

		// Token: 0x04001962 RID: 6498
		internal static readonly VariablePath EventErrorVarPath = new VariablePath("script:error");

		// Token: 0x04001963 RID: 6499
		internal static readonly VariablePath PathExtVarPath = new VariablePath("env:PATHEXT");

		// Token: 0x04001964 RID: 6500
		internal static readonly VariablePath PSEmailServerVarPath = new VariablePath("PSEmailServer");

		// Token: 0x04001965 RID: 6501
		internal static readonly VariablePath PSDefaultParameterValuesVarPath = new VariablePath("PSDefaultParameterValues");

		// Token: 0x04001966 RID: 6502
		internal static readonly VariablePath PSScriptRootVarPath = new VariablePath("PSScriptRoot");

		// Token: 0x04001967 RID: 6503
		internal static readonly VariablePath PSCommandPathVarPath = new VariablePath("PSCommandPath");

		// Token: 0x04001968 RID: 6504
		internal static readonly VariablePath foreachVarPath = new VariablePath("local:foreach");

		// Token: 0x04001969 RID: 6505
		internal static readonly VariablePath switchVarPath = new VariablePath("local:switch");

		// Token: 0x0400196A RID: 6506
		internal static VariablePath PWDVarPath = new VariablePath("global:PWD");

		// Token: 0x0400196B RID: 6507
		internal static VariablePath NullVarPath = new VariablePath("null");

		// Token: 0x0400196C RID: 6508
		internal static VariablePath TrueVarPath = new VariablePath("true");

		// Token: 0x0400196D RID: 6509
		internal static VariablePath FalseVarPath = new VariablePath("false");

		// Token: 0x0400196E RID: 6510
		internal static VariablePath PSModuleAutoLoadingPreferenceVarPath = new VariablePath("global:PSModuleAutoLoadingPreference");

		// Token: 0x0400196F RID: 6511
		internal static readonly VariablePath DebugPreferenceVarPath = new VariablePath("DebugPreference");

		// Token: 0x04001970 RID: 6512
		internal static readonly VariablePath ErrorActionPreferenceVarPath = new VariablePath("ErrorActionPreference");

		// Token: 0x04001971 RID: 6513
		internal static readonly VariablePath ProgressPreferenceVarPath = new VariablePath("ProgressPreference");

		// Token: 0x04001972 RID: 6514
		internal static readonly VariablePath VerbosePreferenceVarPath = new VariablePath("VerbosePreference");

		// Token: 0x04001973 RID: 6515
		internal static readonly VariablePath WarningPreferenceVarPath = new VariablePath("WarningPreference");

		// Token: 0x04001974 RID: 6516
		internal static readonly VariablePath WhatIfPreferenceVarPath = new VariablePath("WhatIfPreference");

		// Token: 0x04001975 RID: 6517
		internal static readonly VariablePath ConfirmPreferenceVarPath = new VariablePath("ConfirmPreference");

		// Token: 0x04001976 RID: 6518
		internal static readonly VariablePath InformationPreferenceVarPath = new VariablePath("InformationPreference");

		// Token: 0x04001977 RID: 6519
		internal static readonly VariablePath ErrorViewVarPath = new VariablePath("ErrorView");

		// Token: 0x04001978 RID: 6520
		internal static readonly VariablePath PSSessionConfigurationNameVarPath = new VariablePath("global:PSSessionConfigurationName");

		// Token: 0x04001979 RID: 6521
		internal static readonly VariablePath PSSessionApplicationNameVarPath = new VariablePath("global:PSSessionApplicationName");

		// Token: 0x0400197A RID: 6522
		internal static List<string> AllScopeSessionVariables = new List<string>
		{
			"ConsoleFileName",
			"ExecutionContext",
			"HOME",
			"Host",
			"PID",
			"PSCulture",
			"PSHOME",
			"PSUICulture",
			"PSVersionTable",
			"ShellId"
		};

		// Token: 0x0400197B RID: 6523
		internal static readonly string[] AutomaticVariables = new string[]
		{
			"_",
			"args",
			"this",
			"input",
			"PSCmdlet",
			"PSBoundParameters",
			"MyInvocation",
			"PSScriptRoot",
			"PSCommandPath"
		};

		// Token: 0x0400197C RID: 6524
		internal static readonly Type[] AutomaticVariableTypes = new Type[]
		{
			typeof(object),
			typeof(object[]),
			typeof(object),
			typeof(object),
			typeof(PSScriptCmdlet),
			typeof(PSBoundParametersDictionary),
			typeof(InvocationInfo),
			typeof(string),
			typeof(string)
		};

		// Token: 0x0400197D RID: 6525
		internal static readonly string[] PreferenceVariables = new string[]
		{
			"DebugPreference",
			"VerbosePreference",
			"ErrorActionPreference",
			"WhatIfPreference",
			"WarningPreference",
			"InformationPreference",
			"ConfirmPreference"
		};

		// Token: 0x0400197E RID: 6526
		internal static readonly Type[] PreferenceVariableTypes = new Type[]
		{
			typeof(ActionPreference),
			typeof(ActionPreference),
			typeof(ActionPreference),
			typeof(SwitchParameter),
			typeof(ActionPreference),
			typeof(ActionPreference),
			typeof(ConfirmImpact)
		};

		// Token: 0x0400197F RID: 6527
		internal static readonly string[] AllScopeVariables = new string[]
		{
			"?",
			"ConsoleFileName",
			"ExecutionContext",
			"false",
			"HOME",
			"Host",
			"PID",
			"PSCulture",
			"PSHOME",
			"PSUICulture",
			"PSVersionTable",
			"ShellId",
			"true"
		};

		// Token: 0x04001980 RID: 6528
		private static readonly HashSet<string> ClassMethodsAccessibleVariables = new HashSet<string>(new string[]
		{
			"LASTEXITCODE",
			"error",
			"StackTrace",
			"OutputEncoding",
			"NestedPromptLevel",
			"PWD",
			"Matches"
		}, StringComparer.OrdinalIgnoreCase);
	}
}
