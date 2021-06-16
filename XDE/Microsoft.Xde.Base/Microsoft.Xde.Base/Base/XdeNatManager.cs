using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Threading;
using Microsoft.Xde.Base.Properties;
using Microsoft.Xde.Common;
using Microsoft.Xde.Wmi;

namespace Microsoft.Xde.Base
{
	// Token: 0x0200000B RID: 11
	public class XdeNatManager : IDisposable
	{
		// Token: 0x060000AB RID: 171 RVA: 0x000049A4 File Offset: 0x00002BA4
		public XdeNatManager(XdeNetworkManager xdeNetworkMgr)
		{
			this.networkMgr = xdeNetworkMgr;
			this.StandardIPPrefixes = new List<IPSubnet>();
			this.StandardIPPrefixes.Add(new IPSubnet("172.16.80.0", 24));
			this.StandardIPPrefixes.Add(new IPSubnet("192.168.80.0", 24));
			this.StandardIPPrefixes.Add(new IPSubnet("10.80.80.0", 24));
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004A20 File Offset: 0x00002C20
		public IXdeMinUiFactory UiFactory
		{
			get
			{
				return this.networkMgr.UiFactory;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004A2D File Offset: 0x00002C2D
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004A35 File Offset: 0x00002C35
		public List<IPSubnet> StandardIPPrefixes { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004A3E File Offset: 0x00002C3E
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00004A46 File Offset: 0x00002C46
		public IPSubnet NATIPSubnet
		{
			get
			{
				return this.natSubnet;
			}
			set
			{
				this.natSubnet = value;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004A4F File Offset: 0x00002C4F
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.networkMgr = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004A70 File Offset: 0x00002C70
		public bool TryAquireNecessaryPermissions()
		{
			ManagementObject managementObject;
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				managementObject = service.FindInternalSwitch("Microsoft Emulator NAT Switch");
			}
			if (managementObject == null && !UacSecurity.IsAdmin())
			{
				this.ShowElevationRequiredForInternalAdapter();
				return false;
			}
			NetNat.GetNatInstance("Microsoft Emulator Nat Instance");
			if (this.natInstance == null && !UacSecurity.IsHyperVAdmin() && !UacSecurity.IsAdmin())
			{
				this.ShowElevationRequiredForInternalAdapter();
				return false;
			}
			return true;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004AE8 File Offset: 0x00002CE8
		public bool Initialize()
		{
			if (!this.networkMgr.InitializeInternalSwitch("Microsoft Emulator NAT Switch", this.GetHostIpForSubnet(this.NATIPSubnet), this.NATIPSubnet.IPMask))
			{
				return false;
			}
			try
			{
				this.UpdateNATProperties();
			}
			catch (Exception ex)
			{
				this.UiFactory.ShowErrorMessageForException(ex.StackTrace, ex, "FailedToInitNatConfig");
				return false;
			}
			return true;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004B58 File Offset: 0x00002D58
		public void UpdateGuestNetworkProperties(IXdeSimpleCommandsPipe simpleCommandsPipe)
		{
			if (simpleCommandsPipe != null && simpleCommandsPipe.IsConnected)
			{
				simpleCommandsPipe.SetGuestNATSubnet(this.NATIPSubnet, this.GetHostIpForSubnet(this.NATIPSubnet));
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004B80 File Offset: 0x00002D80
		public IPSubnet GetNextAvailableIPPrefix()
		{
			NetworkAdapterInformation[] localAdapterInformation = WmiUtils.GetLocalAdapterInformation();
			return this.GetNextAvailableIPPrefix(localAdapterInformation);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004B9A File Offset: 0x00002D9A
		public IPSubnet GetNextAvailableIPPrefix(NetworkAdapterInformation[] hostAdapters)
		{
			return this.GetNextAvailableIPPrefix(hostAdapters, this.NATIPSubnet);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004BAC File Offset: 0x00002DAC
		public IPSubnet GetNextAvailableIPPrefix(IPSubnet currentIPSubnet)
		{
			NetworkAdapterInformation[] localAdapterInformation = WmiUtils.GetLocalAdapterInformation();
			return this.GetNextAvailableIPPrefix(localAdapterInformation, currentIPSubnet);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public IPSubnet GetNextAvailableIPPrefix(NetworkAdapterInformation[] hostAdapters, IPSubnet currentIPSubnet)
		{
			IPSubnet ipsubnet = null;
			if (!this.IsNatSubnetInValidConfigs(currentIPSubnet))
			{
				currentIPSubnet = this.StandardIPPrefixes[0];
			}
			if (hostAdapters == null || hostAdapters.Length == 0)
			{
				return currentIPSubnet;
			}
			IPAddress hostIPAddress = this.GetHostIPAddress();
			if (this.HasConflictWithNetworkAdapters(hostAdapters, currentIPSubnet, hostIPAddress))
			{
				using (List<IPSubnet>.Enumerator enumerator = this.StandardIPPrefixes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IPSubnet ipsubnet2 = enumerator.Current;
						if (!this.HasConflictWithNetworkAdapters(hostAdapters, ipsubnet2, hostIPAddress))
						{
							ipsubnet = ipsubnet2;
							break;
						}
					}
					goto IL_74;
				}
			}
			ipsubnet = currentIPSubnet;
			IL_74:
			if (ipsubnet == null)
			{
				throw new Exception(Strings.FailedToResolveIPConflict);
			}
			return ipsubnet;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004C68 File Offset: 0x00002E68
		public void HandleNetworkChangeEvent()
		{
			this.UpdateNATProperties();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004C70 File Offset: 0x00002E70
		private void UpdateNATProperties()
		{
			using (Mutex mutex = new Mutex(false, "Microsoft.WindowsPhone.XDE.NAT"))
			{
				try
				{
					mutex.WaitOne();
				}
				catch (AbandonedMutexException)
				{
				}
				try
				{
					this.natInstance = NetNat.GetNatInstance("Microsoft Emulator Nat Instance");
					IPSubnet ipsubnet;
					if (this.natInstance == null)
					{
						ipsubnet = null;
					}
					else
					{
						ipsubnet = this.natInstance.Subnet;
					}
					this.NATIPSubnet = this.GetNextAvailableIPPrefix(ipsubnet);
					if (ipsubnet == null || !ipsubnet.Equals(this.NATIPSubnet))
					{
						this.networkMgr.SetStaticIpOnHostNic("Microsoft Emulator NAT Switch", this.GetHostIpForSubnet(this.NATIPSubnet), this.NATIPSubnet.IPMask);
						this.UpdateGuestNetworkProperties(null);
						this.InitializeNatInstance(this.NATIPSubnet);
					}
				}
				catch (Exception exception)
				{
					this.UiFactory.ShowErrorMessageForException(Strings.FailedToInitNatConfig, exception, "FailedToInitNatConfig");
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004D78 File Offset: 0x00002F78
		private bool HasConflictWithNetworkAdapters(NetworkAdapterInformation[] hostAdapters, IPSubnet ipSubnet, IPAddress hostIPaddress)
		{
			if (hostAdapters == null || hostAdapters.Length == 0)
			{
				return false;
			}
			foreach (NetworkAdapterInformation networkAdapterInformation in hostAdapters)
			{
				foreach (NetworkIPAddress networkIPAddress in networkAdapterInformation.IPAddresses)
				{
					if (ValidationUtilities.IsValidIPv4Format(networkIPAddress.IPAddress) && (hostIPaddress == null || !networkIPAddress.IPAddress.Equals(hostIPaddress)))
					{
						IPSubnet ipSubnet2 = new IPSubnet(networkIPAddress.IPAddress, networkIPAddress.IPSubnet);
						if (ipSubnet.Overlaps(ipSubnet2))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004E08 File Offset: 0x00003008
		private IPAddress GetHostIPAddress()
		{
			bool flag = false;
			IPAddress result = null;
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				ManagementObject managementObject = service.FindInternalSwitch("Microsoft Emulator NAT Switch");
				if (managementObject != null)
				{
					flag = IPAddress.TryParse(service.GetSwitchInformation(managementObject).HostIpAddress, out result);
				}
				if (managementObject == null || !flag)
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004E68 File Offset: 0x00003068
		private void InitializeNatInstance(IPSubnet natIPSubnet)
		{
			this.natInstance = NetNat.GetNatInstance("Microsoft Emulator Nat Instance");
			if (this.natInstance == null || !this.natInstance.AreNatPropertiesValid(natIPSubnet))
			{
				this.natInstance = NetNat.CreateNatInstance("Microsoft Emulator Nat Instance", natIPSubnet);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004EA1 File Offset: 0x000030A1
		private void ShowElevationRequiredForInternalAdapter()
		{
			this.UiFactory.ShowElevationDialog(string.Format(Strings.RetryRunningAsAdmin, Array.Empty<object>()), Strings.CannotModifyInternalAdapter, true);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004EC4 File Offset: 0x000030C4
		private IPAddress GetHostIpForSubnet(IPSubnet subnet)
		{
			byte[] addressBytes = subnet.IPPrefix.GetAddressBytes();
			if (addressBytes.Length != 4)
			{
				throw new ArgumentOutOfRangeException("subnet");
			}
			addressBytes[3] = 1;
			return new IPAddress(addressBytes);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004EEC File Offset: 0x000030EC
		private bool IsNatSubnetInValidConfigs(IPSubnet ipSubnet)
		{
			if (ipSubnet == null)
			{
				return false;
			}
			if (this.StandardIPPrefixes.Contains(ipSubnet))
			{
				return true;
			}
			foreach (IPSubnet stdIPSubnet in this.StandardIPPrefixes)
			{
				if (ipSubnet.IsSubset(stdIPSubnet))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000048 RID: 72
		private const int BitsInInt = 32;

		// Token: 0x04000049 RID: 73
		private IPSubnet natSubnet = new IPSubnet("172.16.80.0", 24);

		// Token: 0x0400004A RID: 74
		private bool disposed;

		// Token: 0x0400004B RID: 75
		private NetNat natInstance;

		// Token: 0x0400004C RID: 76
		private XdeNetworkManager networkMgr;
	}
}
