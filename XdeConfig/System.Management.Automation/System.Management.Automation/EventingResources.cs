using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A0D RID: 2573
[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
internal class EventingResources
{
	// Token: 0x06005F78 RID: 24440 RVA: 0x0020587C File Offset: 0x00203A7C
	internal EventingResources()
	{
	}

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x06005F79 RID: 24441 RVA: 0x00205884 File Offset: 0x00203A84
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(EventingResources.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("EventingResources", typeof(EventingResources).Assembly);
				EventingResources.resourceMan = resourceManager;
			}
			return EventingResources.resourceMan;
		}
	}

	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x06005F7A RID: 24442 RVA: 0x002058C3 File Offset: 0x00203AC3
	// (set) Token: 0x06005F7B RID: 24443 RVA: 0x002058CA File Offset: 0x00203ACA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return EventingResources.resourceCulture;
		}
		set
		{
			EventingResources.resourceCulture = value;
		}
	}

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x06005F7C RID: 24444 RVA: 0x002058D2 File Offset: 0x00203AD2
	internal static string ActionAndForwardNotSupported
	{
		get
		{
			return EventingResources.ResourceManager.GetString("ActionAndForwardNotSupported", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x06005F7D RID: 24445 RVA: 0x002058E8 File Offset: 0x00203AE8
	internal static string CouldNotFindEvent
	{
		get
		{
			return EventingResources.ResourceManager.GetString("CouldNotFindEvent", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001411 RID: 5137
	// (get) Token: 0x06005F7E RID: 24446 RVA: 0x002058FE File Offset: 0x00203AFE
	internal static string NonVoidDelegateNotSupported
	{
		get
		{
			return EventingResources.ResourceManager.GetString("NonVoidDelegateNotSupported", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001412 RID: 5138
	// (get) Token: 0x06005F7F RID: 24447 RVA: 0x00205914 File Offset: 0x00203B14
	internal static string RemoteOperationNotSupported
	{
		get
		{
			return EventingResources.ResourceManager.GetString("RemoteOperationNotSupported", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001413 RID: 5139
	// (get) Token: 0x06005F80 RID: 24448 RVA: 0x0020592A File Offset: 0x00203B2A
	internal static string ReservedIdentifier
	{
		get
		{
			return EventingResources.ResourceManager.GetString("ReservedIdentifier", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x06005F81 RID: 24449 RVA: 0x00205940 File Offset: 0x00203B40
	internal static string SubscriberExists
	{
		get
		{
			return EventingResources.ResourceManager.GetString("SubscriberExists", EventingResources.resourceCulture);
		}
	}

	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x06005F82 RID: 24450 RVA: 0x00205956 File Offset: 0x00203B56
	internal static string WinRTEventsNotSupported
	{
		get
		{
			return EventingResources.ResourceManager.GetString("WinRTEventsNotSupported", EventingResources.resourceCulture);
		}
	}

	// Token: 0x04003211 RID: 12817
	private static ResourceManager resourceMan;

	// Token: 0x04003212 RID: 12818
	private static CultureInfo resourceCulture;
}
