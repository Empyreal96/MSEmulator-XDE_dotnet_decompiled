using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x020009FF RID: 2559
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class AutomationExceptions
{
	// Token: 0x06005E1D RID: 24093 RVA: 0x00203AB8 File Offset: 0x00201CB8
	internal AutomationExceptions()
	{
	}

	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x06005E1E RID: 24094 RVA: 0x00203AC0 File Offset: 0x00201CC0
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(AutomationExceptions.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("AutomationExceptions", typeof(AutomationExceptions).Assembly);
				AutomationExceptions.resourceMan = resourceManager;
			}
			return AutomationExceptions.resourceMan;
		}
	}

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x06005E1F RID: 24095 RVA: 0x00203AFF File Offset: 0x00201CFF
	// (set) Token: 0x06005E20 RID: 24096 RVA: 0x00203B06 File Offset: 0x00201D06
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return AutomationExceptions.resourceCulture;
		}
		set
		{
			AutomationExceptions.resourceCulture = value;
		}
	}

	// Token: 0x170012D0 RID: 4816
	// (get) Token: 0x06005E21 RID: 24097 RVA: 0x00203B0E File Offset: 0x00201D0E
	internal static string Argument
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("Argument", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x06005E22 RID: 24098 RVA: 0x00203B24 File Offset: 0x00201D24
	internal static string ArgumentNull
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("ArgumentNull", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D2 RID: 4818
	// (get) Token: 0x06005E23 RID: 24099 RVA: 0x00203B3A File Offset: 0x00201D3A
	internal static string ArgumentOutOfRange
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("ArgumentOutOfRange", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x06005E24 RID: 24100 RVA: 0x00203B50 File Offset: 0x00201D50
	internal static string CanConvertOneClauseOnly
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CanConvertOneClauseOnly", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06005E25 RID: 24101 RVA: 0x00203B66 File Offset: 0x00201D66
	internal static string CanConvertOneOutputErrorRedir
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CanConvertOneOutputErrorRedir", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06005E26 RID: 24102 RVA: 0x00203B7C File Offset: 0x00201D7C
	internal static string CanOnlyConvertOnePipeline
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CanOnlyConvertOnePipeline", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06005E27 RID: 24103 RVA: 0x00203B92 File Offset: 0x00201D92
	internal static string CantConvertEmptyPipeline
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertEmptyPipeline", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06005E28 RID: 24104 RVA: 0x00203BA8 File Offset: 0x00201DA8
	internal static string CantConvertPipelineStartsWithExpression
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertPipelineStartsWithExpression", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D8 RID: 4824
	// (get) Token: 0x06005E29 RID: 24105 RVA: 0x00203BBE File Offset: 0x00201DBE
	internal static string CantConvertScriptBlockToOpenGenericType
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertScriptBlockToOpenGenericType", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x06005E2A RID: 24106 RVA: 0x00203BD4 File Offset: 0x00201DD4
	internal static string CantConvertScriptBlockWithNoContext
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertScriptBlockWithNoContext", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x06005E2B RID: 24107 RVA: 0x00203BEA File Offset: 0x00201DEA
	internal static string CantConvertScriptBlockWithTrap
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertScriptBlockWithTrap", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x06005E2C RID: 24108 RVA: 0x00203C00 File Offset: 0x00201E00
	internal static string CantConvertWithCommandInvocations
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithCommandInvocations", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x06005E2D RID: 24109 RVA: 0x00203C16 File Offset: 0x00201E16
	internal static string CantConvertWithDotSourcing
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithDotSourcing", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x06005E2E RID: 24110 RVA: 0x00203C2C File Offset: 0x00201E2C
	internal static string CantConvertWithDynamicExpression
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithDynamicExpression", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06005E2F RID: 24111 RVA: 0x00203C42 File Offset: 0x00201E42
	internal static string CantConvertWithNonConstantExpression
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithNonConstantExpression", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06005E30 RID: 24112 RVA: 0x00203C58 File Offset: 0x00201E58
	internal static string CantConvertWithScriptBlockInvocation
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithScriptBlockInvocation", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E0 RID: 4832
	// (get) Token: 0x06005E31 RID: 24113 RVA: 0x00203C6E File Offset: 0x00201E6E
	internal static string CantConvertWithScriptBlocks
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithScriptBlocks", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E1 RID: 4833
	// (get) Token: 0x06005E32 RID: 24114 RVA: 0x00203C84 File Offset: 0x00201E84
	internal static string CantConvertWithUndeclaredVariables
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantConvertWithUndeclaredVariables", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E2 RID: 4834
	// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00203C9A File Offset: 0x00201E9A
	internal static string CantGetUsingExpressionValueWithSpecifiedVariableDictionary
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantGetUsingExpressionValueWithSpecifiedVariableDictionary", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E3 RID: 4835
	// (get) Token: 0x06005E34 RID: 24116 RVA: 0x00203CB0 File Offset: 0x00201EB0
	internal static string CantLoadWorkflowType
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("CantLoadWorkflowType", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E4 RID: 4836
	// (get) Token: 0x06005E35 RID: 24117 RVA: 0x00203CC6 File Offset: 0x00201EC6
	internal static string DynamicParametersWrongType
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("DynamicParametersWrongType", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E5 RID: 4837
	// (get) Token: 0x06005E36 RID: 24118 RVA: 0x00203CDC File Offset: 0x00201EDC
	internal static string HaltCommandException
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("HaltCommandException", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E6 RID: 4838
	// (get) Token: 0x06005E37 RID: 24119 RVA: 0x00203CF2 File Offset: 0x00201EF2
	internal static string InvalidOperation
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("InvalidOperation", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E7 RID: 4839
	// (get) Token: 0x06005E38 RID: 24120 RVA: 0x00203D08 File Offset: 0x00201F08
	internal static string InvalidScopeIdArgument
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("InvalidScopeIdArgument", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E8 RID: 4840
	// (get) Token: 0x06005E39 RID: 24121 RVA: 0x00203D1E File Offset: 0x00201F1E
	internal static string NotImplemented
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("NotImplemented", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012E9 RID: 4841
	// (get) Token: 0x06005E3A RID: 24122 RVA: 0x00203D34 File Offset: 0x00201F34
	internal static string NotSupported
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("NotSupported", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x06005E3B RID: 24123 RVA: 0x00203D4A File Offset: 0x00201F4A
	internal static string ObjectDisposed
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("ObjectDisposed", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x06005E3C RID: 24124 RVA: 0x00203D60 File Offset: 0x00201F60
	internal static string ScriptBlockInvokeOnOneClauseOnly
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("ScriptBlockInvokeOnOneClauseOnly", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x06005E3D RID: 24125 RVA: 0x00203D76 File Offset: 0x00201F76
	internal static string UsingVariableIsUndefined
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("UsingVariableIsUndefined", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x170012ED RID: 4845
	// (get) Token: 0x06005E3E RID: 24126 RVA: 0x00203D8C File Offset: 0x00201F8C
	internal static string WorkflowDoesNotSupportWOW64
	{
		get
		{
			return AutomationExceptions.ResourceManager.GetString("WorkflowDoesNotSupportWOW64", AutomationExceptions.resourceCulture);
		}
	}

	// Token: 0x040031F5 RID: 12789
	private static ResourceManager resourceMan;

	// Token: 0x040031F6 RID: 12790
	private static CultureInfo resourceCulture;
}
