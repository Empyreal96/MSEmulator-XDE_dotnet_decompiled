using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A23 RID: 2595
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class PSDataBufferStrings
{
	// Token: 0x06006378 RID: 25464 RVA: 0x0020B068 File Offset: 0x00209268
	internal PSDataBufferStrings()
	{
	}

	// Token: 0x170017E1 RID: 6113
	// (get) Token: 0x06006379 RID: 25465 RVA: 0x0020B070 File Offset: 0x00209270
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(PSDataBufferStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("PSDataBufferStrings", typeof(PSDataBufferStrings).Assembly);
				PSDataBufferStrings.resourceMan = resourceManager;
			}
			return PSDataBufferStrings.resourceMan;
		}
	}

	// Token: 0x170017E2 RID: 6114
	// (get) Token: 0x0600637A RID: 25466 RVA: 0x0020B0AF File Offset: 0x002092AF
	// (set) Token: 0x0600637B RID: 25467 RVA: 0x0020B0B6 File Offset: 0x002092B6
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return PSDataBufferStrings.resourceCulture;
		}
		set
		{
			PSDataBufferStrings.resourceCulture = value;
		}
	}

	// Token: 0x170017E3 RID: 6115
	// (get) Token: 0x0600637C RID: 25468 RVA: 0x0020B0BE File Offset: 0x002092BE
	internal static string CannotConvertToGenericType
	{
		get
		{
			return PSDataBufferStrings.ResourceManager.GetString("CannotConvertToGenericType", PSDataBufferStrings.resourceCulture);
		}
	}

	// Token: 0x170017E4 RID: 6116
	// (get) Token: 0x0600637D RID: 25469 RVA: 0x0020B0D4 File Offset: 0x002092D4
	internal static string IndexOutOfRange
	{
		get
		{
			return PSDataBufferStrings.ResourceManager.GetString("IndexOutOfRange", PSDataBufferStrings.resourceCulture);
		}
	}

	// Token: 0x170017E5 RID: 6117
	// (get) Token: 0x0600637E RID: 25470 RVA: 0x0020B0EA File Offset: 0x002092EA
	internal static string SerializationNotSupported
	{
		get
		{
			return PSDataBufferStrings.ResourceManager.GetString("SerializationNotSupported", PSDataBufferStrings.resourceCulture);
		}
	}

	// Token: 0x170017E6 RID: 6118
	// (get) Token: 0x0600637F RID: 25471 RVA: 0x0020B100 File Offset: 0x00209300
	internal static string ValueNullReference
	{
		get
		{
			return PSDataBufferStrings.ResourceManager.GetString("ValueNullReference", PSDataBufferStrings.resourceCulture);
		}
	}

	// Token: 0x170017E7 RID: 6119
	// (get) Token: 0x06006380 RID: 25472 RVA: 0x0020B116 File Offset: 0x00209316
	internal static string WriteToClosedBuffer
	{
		get
		{
			return PSDataBufferStrings.ResourceManager.GetString("WriteToClosedBuffer", PSDataBufferStrings.resourceCulture);
		}
	}

	// Token: 0x0400323D RID: 12861
	private static ResourceManager resourceMan;

	// Token: 0x0400323E RID: 12862
	private static CultureInfo resourceCulture;
}
