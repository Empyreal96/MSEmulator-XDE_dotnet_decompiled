using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A09 RID: 2569
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class EnumExpressionEvaluatorStrings
{
	// Token: 0x06005EE4 RID: 24292 RVA: 0x00204BCA File Offset: 0x00202DCA
	internal EnumExpressionEvaluatorStrings()
	{
	}

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x06005EE5 RID: 24293 RVA: 0x00204BD4 File Offset: 0x00202DD4
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(EnumExpressionEvaluatorStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("EnumExpressionEvaluatorStrings", typeof(EnumExpressionEvaluatorStrings).Assembly);
				EnumExpressionEvaluatorStrings.resourceMan = resourceManager;
			}
			return EnumExpressionEvaluatorStrings.resourceMan;
		}
	}

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x06005EE6 RID: 24294 RVA: 0x00204C13 File Offset: 0x00202E13
	// (set) Token: 0x06005EE7 RID: 24295 RVA: 0x00204C1A File Offset: 0x00202E1A
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return EnumExpressionEvaluatorStrings.resourceCulture;
		}
		set
		{
			EnumExpressionEvaluatorStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x06005EE8 RID: 24296 RVA: 0x00204C22 File Offset: 0x00202E22
	internal static string EmptyInputString
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("EmptyInputString", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x06005EE9 RID: 24297 RVA: 0x00204C38 File Offset: 0x00202E38
	internal static string EmptyTokenString
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("EmptyTokenString", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x06005EEA RID: 24298 RVA: 0x00204C4E File Offset: 0x00202E4E
	internal static string InvalidGenericType
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("InvalidGenericType", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x06005EEB RID: 24299 RVA: 0x00204C64 File Offset: 0x00202E64
	internal static string MultipleEnumNameMatch
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("MultipleEnumNameMatch", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x06005EEC RID: 24300 RVA: 0x00204C7A File Offset: 0x00202E7A
	internal static string NoEnumNameMatch
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("NoEnumNameMatch", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001388 RID: 5000
	// (get) Token: 0x06005EED RID: 24301 RVA: 0x00204C90 File Offset: 0x00202E90
	internal static string NoIdentifierGroupingAllowed
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("NoIdentifierGroupingAllowed", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x06005EEE RID: 24302 RVA: 0x00204CA6 File Offset: 0x00202EA6
	internal static string SyntaxErrorBinaryOperatorExpected
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("SyntaxErrorBinaryOperatorExpected", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x06005EEF RID: 24303 RVA: 0x00204CBC File Offset: 0x00202EBC
	internal static string SyntaxErrorIdentifierExpected
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("SyntaxErrorIdentifierExpected", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x06005EF0 RID: 24304 RVA: 0x00204CD2 File Offset: 0x00202ED2
	internal static string SyntaxErrorUnexpectedBinaryOperator
	{
		get
		{
			return EnumExpressionEvaluatorStrings.ResourceManager.GetString("SyntaxErrorUnexpectedBinaryOperator", EnumExpressionEvaluatorStrings.resourceCulture);
		}
	}

	// Token: 0x04003209 RID: 12809
	private static ResourceManager resourceMan;

	// Token: 0x0400320A RID: 12810
	private static CultureInfo resourceCulture;
}
