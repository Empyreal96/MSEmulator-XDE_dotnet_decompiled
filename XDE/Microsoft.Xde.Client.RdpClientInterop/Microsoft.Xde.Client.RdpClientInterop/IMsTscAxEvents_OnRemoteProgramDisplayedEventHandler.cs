using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000060 RID: 96
	// (Invoke) Token: 0x06001F1F RID: 7967
	[ComVisible(false)]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	public delegate void IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler([In] bool vbDisplayed, [In] uint uDisplayInformation);
}
