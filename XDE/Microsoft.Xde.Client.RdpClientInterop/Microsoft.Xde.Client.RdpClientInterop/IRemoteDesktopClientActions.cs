using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200007E RID: 126
	[Guid("7D54BC4E-1028-45D4-8B0A-B9B6BFFBA176")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IRemoteDesktopClientActions
	{
		// Token: 0x06002492 RID: 9362
		[DispId(730)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SuspendScreenUpdates();

		// Token: 0x06002493 RID: 9363
		[DispId(731)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ResumeScreenUpdates();

		// Token: 0x06002494 RID: 9364
		[DispId(732)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ExecuteRemoteAction([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RemoteActionType")] [In] RemoteActionType remoteAction);

		// Token: 0x06002495 RID: 9365
		[DispId(733)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ExecuteLocalAction([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LocalActionType")] [In] LocalActionType localAction);

		// Token: 0x06002496 RID: 9366
		[DispId(734)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSnapshot([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.SnapshotEncodingType")] [In] SnapshotEncodingType snapshotEncoding, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.SnapshotFormatType")] [In] SnapshotFormatType snapshotFormat, [In] uint snapshotWidth, [In] uint snapshotHeight);
	}
}
