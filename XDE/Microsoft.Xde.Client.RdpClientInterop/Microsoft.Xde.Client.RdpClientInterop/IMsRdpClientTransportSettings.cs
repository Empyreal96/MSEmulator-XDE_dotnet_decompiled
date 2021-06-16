using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000071 RID: 113
	[Guid("720298C0-A099-46F5-9F82-96921BAE4701")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientTransportSettings
	{
		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06002162 RID: 8546
		// (set) Token: 0x06002161 RID: 8545
		[DispId(210)]
		string GatewayHostname { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06002164 RID: 8548
		// (set) Token: 0x06002163 RID: 8547
		[DispId(211)]
		uint GatewayUsageMethod { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06002166 RID: 8550
		// (set) Token: 0x06002165 RID: 8549
		[DispId(212)]
		uint GatewayProfileUsageMethod { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06002168 RID: 8552
		// (set) Token: 0x06002167 RID: 8551
		[DispId(213)]
		uint GatewayCredsSource { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x0600216A RID: 8554
		// (set) Token: 0x06002169 RID: 8553
		[DispId(216)]
		uint GatewayUserSelectedCredsSource { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x0600216B RID: 8555
		[DispId(214)]
		int GatewayIsSupported { [DispId(214)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x0600216C RID: 8556
		[DispId(215)]
		uint GatewayDefaultUsageMethod { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
