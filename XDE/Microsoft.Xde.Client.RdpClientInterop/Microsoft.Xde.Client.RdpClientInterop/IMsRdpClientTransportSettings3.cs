using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000078 RID: 120
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("3D5B21AC-748D-41DE-8F30-E15169586BD4")]
	[ComImport]
	public interface IMsRdpClientTransportSettings3 : IMsRdpClientTransportSettings2
	{
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06002399 RID: 9113
		// (set) Token: 0x06002398 RID: 9112
		[DispId(210)]
		string GatewayHostname { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x0600239B RID: 9115
		// (set) Token: 0x0600239A RID: 9114
		[DispId(211)]
		uint GatewayUsageMethod { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x0600239D RID: 9117
		// (set) Token: 0x0600239C RID: 9116
		[DispId(212)]
		uint GatewayProfileUsageMethod { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x0600239F RID: 9119
		// (set) Token: 0x0600239E RID: 9118
		[DispId(213)]
		uint GatewayCredsSource { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060023A1 RID: 9121
		// (set) Token: 0x060023A0 RID: 9120
		[DispId(216)]
		uint GatewayUserSelectedCredsSource { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060023A2 RID: 9122
		[DispId(214)]
		int GatewayIsSupported { [DispId(214)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060023A3 RID: 9123
		[DispId(215)]
		uint GatewayDefaultUsageMethod { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060023A5 RID: 9125
		// (set) Token: 0x060023A4 RID: 9124
		[DispId(222)]
		uint GatewayCredSharing { [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060023A7 RID: 9127
		// (set) Token: 0x060023A6 RID: 9126
		[DispId(217)]
		uint GatewayPreAuthRequirement { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060023A9 RID: 9129
		// (set) Token: 0x060023A8 RID: 9128
		[DispId(218)]
		string GatewayPreAuthServerAddr { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060023AB RID: 9131
		// (set) Token: 0x060023AA RID: 9130
		[DispId(219)]
		string GatewaySupportUrl { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060023AD RID: 9133
		// (set) Token: 0x060023AC RID: 9132
		[DispId(220)]
		string GatewayEncryptedOtpCookie { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060023AF RID: 9135
		// (set) Token: 0x060023AE RID: 9134
		[DispId(221)]
		uint GatewayEncryptedOtpCookieSize { [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060023B1 RID: 9137
		// (set) Token: 0x060023B0 RID: 9136
		[DispId(223)]
		string GatewayUsername { [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060023B3 RID: 9139
		// (set) Token: 0x060023B2 RID: 9138
		[DispId(224)]
		string GatewayDomain { [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D1 RID: 4305
		// (set) Token: 0x060023B4 RID: 9140
		[DispId(225)]
		string GatewayPassword { [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060023B6 RID: 9142
		// (set) Token: 0x060023B5 RID: 9141
		[DispId(226)]
		uint GatewayCredSourceCookie { [DispId(226)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(226)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060023B8 RID: 9144
		// (set) Token: 0x060023B7 RID: 9143
		[DispId(227)]
		string GatewayAuthCookieServerAddr { [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060023BA RID: 9146
		// (set) Token: 0x060023B9 RID: 9145
		[DispId(228)]
		string GatewayEncryptedAuthCookie { [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060023BC RID: 9148
		// (set) Token: 0x060023BB RID: 9147
		[DispId(229)]
		uint GatewayEncryptedAuthCookieSize { [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060023BE RID: 9150
		// (set) Token: 0x060023BD RID: 9149
		[DispId(230)]
		string GatewayAuthLoginPage { [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
