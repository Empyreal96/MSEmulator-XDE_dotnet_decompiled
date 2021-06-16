using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000076 RID: 118
	[Guid("67341688-D606-4C73-A5D2-2E0489009319")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientTransportSettings2 : IMsRdpClientTransportSettings
	{
		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x060022C1 RID: 8897
		// (set) Token: 0x060022C0 RID: 8896
		[DispId(210)]
		string GatewayHostname { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x060022C3 RID: 8899
		// (set) Token: 0x060022C2 RID: 8898
		[DispId(211)]
		uint GatewayUsageMethod { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x060022C5 RID: 8901
		// (set) Token: 0x060022C4 RID: 8900
		[DispId(212)]
		uint GatewayProfileUsageMethod { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x060022C7 RID: 8903
		// (set) Token: 0x060022C6 RID: 8902
		[DispId(213)]
		uint GatewayCredsSource { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x060022C9 RID: 8905
		// (set) Token: 0x060022C8 RID: 8904
		[DispId(216)]
		uint GatewayUserSelectedCredsSource { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x060022CA RID: 8906
		[DispId(214)]
		int GatewayIsSupported { [DispId(214)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x060022CB RID: 8907
		[DispId(215)]
		uint GatewayDefaultUsageMethod { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x060022CD RID: 8909
		// (set) Token: 0x060022CC RID: 8908
		[DispId(222)]
		uint GatewayCredSharing { [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x060022CF RID: 8911
		// (set) Token: 0x060022CE RID: 8910
		[DispId(217)]
		uint GatewayPreAuthRequirement { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x060022D1 RID: 8913
		// (set) Token: 0x060022D0 RID: 8912
		[DispId(218)]
		string GatewayPreAuthServerAddr { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x060022D3 RID: 8915
		// (set) Token: 0x060022D2 RID: 8914
		[DispId(219)]
		string GatewaySupportUrl { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x060022D5 RID: 8917
		// (set) Token: 0x060022D4 RID: 8916
		[DispId(220)]
		string GatewayEncryptedOtpCookie { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x060022D7 RID: 8919
		// (set) Token: 0x060022D6 RID: 8918
		[DispId(221)]
		uint GatewayEncryptedOtpCookieSize { [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x060022D9 RID: 8921
		// (set) Token: 0x060022D8 RID: 8920
		[DispId(223)]
		string GatewayUsername { [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x060022DB RID: 8923
		// (set) Token: 0x060022DA RID: 8922
		[DispId(224)]
		string GatewayDomain { [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700105F RID: 4191
		// (set) Token: 0x060022DC RID: 8924
		[DispId(225)]
		string GatewayPassword { [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
