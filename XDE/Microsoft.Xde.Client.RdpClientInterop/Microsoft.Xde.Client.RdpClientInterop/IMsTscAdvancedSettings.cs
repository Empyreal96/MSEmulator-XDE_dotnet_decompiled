using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000046 RID: 70
	[Guid("809945CC-4B3B-4A92-A6B0-DBF9B5F2EF2D")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsTscAdvancedSettings
	{
		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06001EC7 RID: 7879
		// (set) Token: 0x06001EC6 RID: 7878
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06001EC9 RID: 7881
		// (set) Token: 0x06001EC8 RID: 7880
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06001ECB RID: 7883
		// (set) Token: 0x06001ECA RID: 7882
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E5A RID: 3674
		// (set) Token: 0x06001ECC RID: 7884
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E5B RID: 3675
		// (set) Token: 0x06001ECD RID: 7885
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E5C RID: 3676
		// (set) Token: 0x06001ECE RID: 7886
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E5D RID: 3677
		// (set) Token: 0x06001ECF RID: 7887
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06001ED1 RID: 7889
		// (set) Token: 0x06001ED0 RID: 7888
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06001ED3 RID: 7891
		// (set) Token: 0x06001ED2 RID: 7890
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
