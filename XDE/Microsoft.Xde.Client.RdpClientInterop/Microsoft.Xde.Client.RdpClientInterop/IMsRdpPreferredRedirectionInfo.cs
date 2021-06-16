using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000036 RID: 54
	[Guid("FDD029F9-9574-4DEF-8529-64B521CCCAA4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IMsRdpPreferredRedirectionInfo
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06001444 RID: 5188
		// (set) Token: 0x06001443 RID: 5187
		[DispId(1)]
		bool UseRedirectionServerName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
