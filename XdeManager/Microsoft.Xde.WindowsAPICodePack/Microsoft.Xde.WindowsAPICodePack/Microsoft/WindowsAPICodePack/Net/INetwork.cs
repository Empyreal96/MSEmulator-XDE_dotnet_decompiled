using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x0200000F RID: 15
	[TypeLibType(4160)]
	[Guid("DCB00002-570F-4A9B-8D69-199FDBA5723B")]
	[ComImport]
	internal interface INetwork
	{
		// Token: 0x060000C2 RID: 194
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetName();

		// Token: 0x060000C3 RID: 195
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SetName([MarshalAs(UnmanagedType.BStr)] [In] string szNetworkNewName);

		// Token: 0x060000C4 RID: 196
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDescription();

		// Token: 0x060000C5 RID: 197
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SetDescription([MarshalAs(UnmanagedType.BStr)] [In] string szDescription);

		// Token: 0x060000C6 RID: 198
		[MethodImpl(MethodImplOptions.InternalCall)]
		Guid GetNetworkId();

		// Token: 0x060000C7 RID: 199
		[MethodImpl(MethodImplOptions.InternalCall)]
		DomainType GetDomainType();

		// Token: 0x060000C8 RID: 200
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumerable GetNetworkConnections();

		// Token: 0x060000C9 RID: 201
		[MethodImpl(MethodImplOptions.InternalCall)]
		void GetTimeCreatedAndConnected(out uint pdwLowDateTimeCreated, out uint pdwHighDateTimeCreated, out uint pdwLowDateTimeConnected, out uint pdwHighDateTimeConnected);

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CA RID: 202
		bool IsConnectedToInternet { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CB RID: 203
		bool IsConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x060000CC RID: 204
		[MethodImpl(MethodImplOptions.InternalCall)]
		ConnectivityStates GetConnectivity();

		// Token: 0x060000CD RID: 205
		[MethodImpl(MethodImplOptions.InternalCall)]
		NetworkCategory GetCategory();

		// Token: 0x060000CE RID: 206
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SetCategory([In] NetworkCategory NewCategory);
	}
}
