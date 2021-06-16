using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000074 RID: 116
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("D012AE6D-C19A-4BFE-B367-201F8911F134")]
	[ComImport]
	public interface IMsRdpClientShell
	{
		// Token: 0x0600220C RID: 8716
		[DispId(201)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void Launch();

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600220E RID: 8718
		// (set) Token: 0x0600220D RID: 8717
		[DispId(202)]
		string RdpFileContents { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600220F RID: 8719
		[DispId(203)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SetRdpProperty([MarshalAs(UnmanagedType.BStr)] [In] string szProperty, [MarshalAs(UnmanagedType.Struct)] [In] object Value);

		// Token: 0x06002210 RID: 8720
		[DispId(204)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Struct)]
		object GetRdpProperty([MarshalAs(UnmanagedType.BStr)] [In] string szProperty);

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06002211 RID: 8721
		[DispId(205)]
		bool IsRemoteProgramClientInstalled { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06002213 RID: 8723
		// (set) Token: 0x06002212 RID: 8722
		[DispId(211)]
		bool PublicMode { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06002214 RID: 8724
		[DispId(212)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ShowTrustedSitesManagementDialog();
	}
}
