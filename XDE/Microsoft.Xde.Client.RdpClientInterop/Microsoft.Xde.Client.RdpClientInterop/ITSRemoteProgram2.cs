using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200007A RID: 122
	[Guid("92C38A7D-241A-418C-9936-099872C9AF20")]
	[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface ITSRemoteProgram2 : ITSRemoteProgram
	{
		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060023CC RID: 9164
		// (set) Token: 0x060023CB RID: 9163
		[DispId(200)]
		bool RemoteProgramMode { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060023CD RID: 9165
		[DispId(201)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ServerStartProgram([MarshalAs(UnmanagedType.BStr)] [In] string bstrExecutablePath, [MarshalAs(UnmanagedType.BStr)] [In] string bstrFilePath, [MarshalAs(UnmanagedType.BStr)] [In] string bstrWorkingDirectory, [In] bool vbExpandEnvVarInWorkingDirectoryOnServer, [MarshalAs(UnmanagedType.BStr)] [In] string bstrArguments, [In] bool vbExpandEnvVarInArgumentsOnServer);

		// Token: 0x170010DE RID: 4318
		// (set) Token: 0x060023CE RID: 9166
		[DispId(202)]
		string RemoteApplicationName { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010DF RID: 4319
		// (set) Token: 0x060023CF RID: 9167
		[DispId(203)]
		string RemoteApplicationProgram { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010E0 RID: 4320
		// (set) Token: 0x060023D0 RID: 9168
		[DispId(204)]
		string RemoteApplicationArgs { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
