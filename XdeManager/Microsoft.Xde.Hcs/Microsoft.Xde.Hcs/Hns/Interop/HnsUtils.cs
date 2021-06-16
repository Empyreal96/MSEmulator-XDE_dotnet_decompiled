using System;
using System.Runtime.InteropServices;
using HCS.Config.Containers.HNS;
using Microsoft.Xde.Hcs;

namespace Microsoft.Xde.Hns.Interop
{
	// Token: 0x02000004 RID: 4
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public static class HnsUtils
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000021CC File Offset: 0x000003CC
		public static HNSEndpoint CreateEndpoint(Guid networkId, string macAddress)
		{
			HNSEndpoint endpoint = new HNSEndpoint
			{
				VirtualNetwork = networkId,
				ID = Guid.NewGuid(),
				MacAddress = macAddress
			};
			HNSEndpoint result;
			using (HnsUtils.HcnNetworkSafeHandle hcnNetworkSafeHandle = HnsUtils.OpenNetwork(networkId))
			{
				using (HnsUtils.HcnEndpointSafeHandle hcnEndpointSafeHandle = HnsUtils.CreateEndpointWithNetwork(hcnNetworkSafeHandle, endpoint))
				{
					string json;
					string text;
					int num = HnsUtils.NativeMethods.HcnQueryEndpointProperties(hcnEndpointSafeHandle, string.Empty, out json, out text);
					if (num != 0)
					{
						Marshal.ThrowExceptionForHR(num);
					}
					result = JsonHelper.FromJson<HNSEndpoint>(json);
				}
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002260 File Offset: 0x00000460
		public static void DeleteEndpoint(HNSEndpoint endpoint)
		{
			string text;
			int num = HnsUtils.NativeMethods.HcnDeleteEndpoint(endpoint.ID, out text);
			if (num != 0 && num != -2143617022)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000228C File Offset: 0x0000048C
		public static HNSNetwork GetNetwork(Guid networkId)
		{
			HNSNetwork result;
			using (HnsUtils.HcnNetworkSafeHandle hcnNetworkSafeHandle = HnsUtils.OpenNetwork(networkId))
			{
				string json;
				string text;
				int num = HnsUtils.NativeMethods.HcnQueryNetworkProperties(hcnNetworkSafeHandle, string.Empty, out json, out text);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				result = JsonHelper.FromJson<HNSNetwork>(json);
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022E0 File Offset: 0x000004E0
		private static HnsUtils.HcnNetworkSafeHandle OpenNetwork(Guid networkId)
		{
			HnsUtils.HcnNetworkSafeHandle result;
			string text;
			int num = HnsUtils.NativeMethods.HcnOpenNetwork(networkId, out result, out text);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002304 File Offset: 0x00000504
		private static HnsUtils.HcnEndpointSafeHandle CreateEndpointWithNetwork(HnsUtils.HcnNetworkSafeHandle network, HNSEndpoint endpoint)
		{
			string settings = JsonHelper.ToJson<HNSEndpoint>(endpoint);
			HnsUtils.HcnEndpointSafeHandle result;
			string text;
			int num = HnsUtils.NativeMethods.HcnCreateEndpoint(network, endpoint.ID, settings, out result, out text);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return result;
		}

		// Token: 0x04000003 RID: 3
		public static readonly Guid DefaultSwitchId = new Guid("C08CB7B8-9B3C-408E-8E30-5E16A3AEB444");

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private static class NativeMethods
		{
			// Token: 0x060000C3 RID: 195
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnOpenNetwork([MarshalAs(UnmanagedType.LPStruct)] Guid id, out HnsUtils.HcnNetworkSafeHandle network, [MarshalAs(UnmanagedType.LPWStr)] out string result);

			// Token: 0x060000C4 RID: 196
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			public static extern int HcnQueryNetworkProperties(HnsUtils.HcnNetworkSafeHandle network, [MarshalAs(UnmanagedType.LPWStr)] string query, [MarshalAs(UnmanagedType.LPWStr)] out string properties, [MarshalAs(UnmanagedType.LPWStr)] out string result);

			// Token: 0x060000C5 RID: 197
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnCreateEndpoint(HnsUtils.HcnNetworkSafeHandle network, [MarshalAs(UnmanagedType.LPStruct)] Guid id, [MarshalAs(UnmanagedType.LPWStr)] string settings, out HnsUtils.HcnEndpointSafeHandle endpoint, [MarshalAs(UnmanagedType.LPWStr)] out string result);

			// Token: 0x060000C6 RID: 198
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnDeleteEndpoint([MarshalAs(UnmanagedType.LPStruct)] Guid id, [MarshalAs(UnmanagedType.LPWStr)] out string result);

			// Token: 0x060000C7 RID: 199
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnCloseNetwork([MarshalAs(UnmanagedType.SysUInt)] IntPtr network);

			// Token: 0x060000C8 RID: 200
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnCloseEndpoint([MarshalAs(UnmanagedType.SysUInt)] IntPtr endpoint);

			// Token: 0x060000C9 RID: 201
			[DllImport("computenetwork.dll", CharSet = CharSet.Unicode)]
			internal static extern int HcnQueryEndpointProperties(HnsUtils.HcnEndpointSafeHandle endpoint, [MarshalAs(UnmanagedType.LPWStr)] string query, [MarshalAs(UnmanagedType.LPWStr)] out string properties, [MarshalAs(UnmanagedType.LPWStr)] out string result);
		}

		// Token: 0x0200001B RID: 27
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class HcnNetworkSafeHandle : SafeHandle
		{
			// Token: 0x060000CA RID: 202 RVA: 0x000043CA File Offset: 0x000025CA
			public HcnNetworkSafeHandle() : base(IntPtr.Zero, true)
			{
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000CB RID: 203 RVA: 0x0000429B File Offset: 0x0000249B
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x060000CC RID: 204 RVA: 0x000043D8 File Offset: 0x000025D8
			protected override bool ReleaseHandle()
			{
				if (this.handle != IntPtr.Zero)
				{
					HnsUtils.NativeMethods.HcnCloseNetwork(this.handle);
					this.handle = IntPtr.Zero;
				}
				return true;
			}
		}

		// Token: 0x0200001C RID: 28
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class HcnEndpointSafeHandle : SafeHandle
		{
			// Token: 0x060000CD RID: 205 RVA: 0x000043CA File Offset: 0x000025CA
			public HcnEndpointSafeHandle() : base(IntPtr.Zero, true)
			{
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000CE RID: 206 RVA: 0x0000429B File Offset: 0x0000249B
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x060000CF RID: 207 RVA: 0x00004404 File Offset: 0x00002604
			protected override bool ReleaseHandle()
			{
				if (this.handle != IntPtr.Zero)
				{
					HnsUtils.NativeMethods.HcnCloseEndpoint(this.handle);
					this.handle = IntPtr.Zero;
				}
				return true;
			}
		}
	}
}
