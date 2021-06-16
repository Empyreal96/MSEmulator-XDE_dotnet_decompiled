using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xde.Base.Properties;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Base.Connection
{
	// Token: 0x0200000D RID: 13
	[Export(typeof(IXdeConnectionAddressInfo))]
	public class XdeConnectionAddressInfo : IXdeConnectionAddressInfo, IDisposable
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060000DC RID: 220 RVA: 0x00005198 File Offset: 0x00003398
		// (remove) Token: 0x060000DD RID: 221 RVA: 0x000051D0 File Offset: 0x000033D0
		public event EventHandler Ready;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060000DE RID: 222 RVA: 0x00005208 File Offset: 0x00003408
		// (remove) Token: 0x060000DF RID: 223 RVA: 0x00005240 File Offset: 0x00003440
		public event EventHandler<MessageEventArgs> Failed;

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005275 File Offset: 0x00003475
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000527D File Offset: 0x0000347D
		[Import]
		public IXdeControllerState ControllerState
		{
			get
			{
				return this.contollerState;
			}
			set
			{
				this.contollerState = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005286 File Offset: 0x00003486
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000528E File Offset: 0x0000348E
		[Import(typeof(IXdeAutomationSimpleCommandsPipe))]
		public IXdeAutomationSimpleCommandsPipe SimpleCommandsPipe
		{
			get
			{
				return this.simpleCommandsPipe;
			}
			set
			{
				if (this.simpleCommandsPipe != value)
				{
					this.simpleCommandsPipe = value;
					this.simpleCommandsPipe.PropertyChanged += this.SimpleCommandsPipe_PropertyChanged;
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000052B7 File Offset: 0x000034B7
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000052BF File Offset: 0x000034BF
		public string HostIpAddress { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000052C8 File Offset: 0x000034C8
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000052D0 File Offset: 0x000034D0
		public string GuestIpAddress { get; private set; }

		// Token: 0x060000E8 RID: 232 RVA: 0x000052D9 File Offset: 0x000034D9
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000052F1 File Offset: 0x000034F1
		public Socket CreateSocket()
		{
			return new Socket((AddressFamily)34, SocketType.Stream, ProtocolType.Icmp);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000052FC File Offset: 0x000034FC
		public EndPoint GetEndPoint()
		{
			return new HyperVSocketEndPoint(new Guid(this.contollerState.CurrentVirtualMachine.Guid), Globals.XdeServicesId);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000531D File Offset: 0x0000351D
		private void SimpleCommandsPipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "IsConnected" || !this.simpleCommandsPipe.IsConnected)
			{
				return;
			}
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				if (this.InitializeHostAddressFromWmi())
				{
					this.WaitForValidIPAddressFromGuest();
				}
			});
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005354 File Offset: 0x00003554
		private void WaitForValidIPAddressFromGuest()
		{
			IPAddress subnetMask = IPAddress.Parse(this.hostSubnetMask);
			IPAddress address = IPAddress.Parse(this.HostIpAddress);
			for (int i = 0; i < 60000; i += 500)
			{
				NetworkAdapterInformation[] guestAdapterInformation = this.simpleCommandsPipe.GetGuestAdapterInformation();
				if (guestAdapterInformation != null && guestAdapterInformation.Length != 0)
				{
					NetworkAdapterInformation networkAdapterInformation = guestAdapterInformation[0];
					int j = 0;
					while (j < networkAdapterInformation.IPAddresses.Length)
					{
						NetworkIPAddress networkIPAddress = networkAdapterInformation.IPAddresses[j];
						if (networkIPAddress.IPAddress.AddressFamily == AddressFamily.InterNetwork && networkIPAddress.DadState == IpDadState.Preferred && networkIPAddress.IPAddress.IsInSameSubnet(address, subnetMask))
						{
							this.GuestIpAddress = networkIPAddress.IPAddress.ToString();
							EventHandler ready = this.Ready;
							if (ready == null)
							{
								return;
							}
							ready(this, EventArgs.Empty);
							return;
						}
						else
						{
							j++;
						}
					}
				}
				Thread.Sleep(500);
			}
			EventHandler<MessageEventArgs> failed = this.Failed;
			if (failed == null)
			{
				return;
			}
			failed(this, new MessageEventArgs(Strings.GuestDidntReturnValidIP));
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005454 File Offset: 0x00003654
		private void OnFailed(string message)
		{
			if (this.Failed != null)
			{
				this.Failed(this, new MessageEventArgs(message));
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005470 File Offset: 0x00003670
		private bool InitializeHostAddressFromWmi()
		{
			try
			{
				IXdeVirtualMachineNicInformation xdeVirtualMachineNicInformation = (from s in this.ControllerState.CurrentVirtualMachine.CurrentSettings.Nics
				where !s.SwitchInformation.External
				select s).FirstOrDefault<IXdeVirtualMachineNicInformation>();
				if (xdeVirtualMachineNicInformation != null)
				{
					this.HostIpAddress = xdeVirtualMachineNicInformation.SwitchInformation.HostIpAddress;
					this.hostSubnetMask = xdeVirtualMachineNicInformation.SwitchInformation.HostIpMask;
				}
			}
			catch (Exception ex)
			{
				Logger.Instance.LogException("InitializeHostAddressFromWmi", ex, this.disposed);
				this.OnFailed(ex.Message);
				return false;
			}
			if (this.HostIpAddress == null)
			{
				Logger.Instance.LogError("HostAddressNotFound");
				this.OnFailed(Strings.HostAddressNotFound);
				return false;
			}
			return true;
		}

		// Token: 0x04000050 RID: 80
		private IXdeControllerState contollerState;

		// Token: 0x04000051 RID: 81
		private IXdeAutomationSimpleCommandsPipe simpleCommandsPipe;

		// Token: 0x04000052 RID: 82
		private string hostSubnetMask;

		// Token: 0x04000053 RID: 83
		private bool disposed;
	}
}
