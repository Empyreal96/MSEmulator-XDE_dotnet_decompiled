using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A10 RID: 2576
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class GetErrorText
{
	// Token: 0x06006019 RID: 24601 RVA: 0x0020664E File Offset: 0x0020484E
	internal GetErrorText()
	{
	}

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x0600601A RID: 24602 RVA: 0x00206658 File Offset: 0x00204858
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(GetErrorText.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("GetErrorText", typeof(GetErrorText).Assembly);
				GetErrorText.resourceMan = resourceManager;
			}
			return GetErrorText.resourceMan;
		}
	}

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x0600601B RID: 24603 RVA: 0x00206697 File Offset: 0x00204897
	// (set) Token: 0x0600601C RID: 24604 RVA: 0x0020669E File Offset: 0x0020489E
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return GetErrorText.resourceCulture;
		}
		set
		{
			GetErrorText.resourceCulture = value;
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x0600601D RID: 24605 RVA: 0x002066A6 File Offset: 0x002048A6
	internal static string ActionPreferenceStop
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("ActionPreferenceStop", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x0600601E RID: 24606 RVA: 0x002066BC File Offset: 0x002048BC
	internal static string AssemblyNotRegistered
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("AssemblyNotRegistered", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x0600601F RID: 24607 RVA: 0x002066D2 File Offset: 0x002048D2
	internal static string BadTemplate
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("BadTemplate", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014AD RID: 5293
	// (get) Token: 0x06006020 RID: 24608 RVA: 0x002066E8 File Offset: 0x002048E8
	internal static string BlankTemplate
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("BlankTemplate", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014AE RID: 5294
	// (get) Token: 0x06006021 RID: 24609 RVA: 0x002066FE File Offset: 0x002048FE
	internal static string PipelineDepthException
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("PipelineDepthException", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x06006022 RID: 24610 RVA: 0x00206714 File Offset: 0x00204914
	internal static string PipelineStoppedException
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("PipelineStoppedException", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014B0 RID: 5296
	// (get) Token: 0x06006023 RID: 24611 RVA: 0x0020672A File Offset: 0x0020492A
	internal static string ResourceBaseNameFailure
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("ResourceBaseNameFailure", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014B1 RID: 5297
	// (get) Token: 0x06006024 RID: 24612 RVA: 0x00206740 File Offset: 0x00204940
	internal static string ResourceIdFailure
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("ResourceIdFailure", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x170014B2 RID: 5298
	// (get) Token: 0x06006025 RID: 24613 RVA: 0x00206756 File Offset: 0x00204956
	internal static string ScriptCallDepthException
	{
		get
		{
			return GetErrorText.ResourceManager.GetString("ScriptCallDepthException", GetErrorText.resourceCulture);
		}
	}

	// Token: 0x04003217 RID: 12823
	private static ResourceManager resourceMan;

	// Token: 0x04003218 RID: 12824
	private static CultureInfo resourceCulture;
}
