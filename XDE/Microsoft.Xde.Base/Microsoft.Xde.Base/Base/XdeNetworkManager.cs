using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Base.Properties;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;
using Microsoft.Xde.Wmi;

namespace Microsoft.Xde.Base
{
	// Token: 0x0200000A RID: 10
	public class XdeNetworkManager : IXdeNetworkManager, IDisposable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004357 File Offset: 0x00002557
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000435F File Offset: 0x0000255F
		[Import]
		public IXdeMinUiFactory UiFactory { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004368 File Offset: 0x00002568
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004370 File Offset: 0x00002570
		[Import(typeof(IXdeAutomationSimpleCommandsPipe))]
		public IXdeAutomationSimpleCommandsPipe SimpleCommandsFeature
		{
			get
			{
				return this.simpleCommandsFeature;
			}
			set
			{
				if (this.simpleCommandsFeature != value)
				{
					this.simpleCommandsFeature = value;
					if (this.simpleCommandsFeature != null)
					{
						this.simpleCommandsFeature.PropertyChanged += this.SimpleCommandsPipe_PropertyChanged;
					}
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000043A1 File Offset: 0x000025A1
		private void SimpleCommandsPipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected" && this.simpleCommandsFeature.IsConnected)
			{
				this.UpdateGuestNetworkProperties(null);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000043C9 File Offset: 0x000025C9
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000043D1 File Offset: 0x000025D1
		private XdeNatManager NatMgr { get; set; }

		// Token: 0x0600009B RID: 155 RVA: 0x000043DC File Offset: 0x000025DC
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (this.networkAddressChangedHandler != null)
			{
				NetworkChange.NetworkAddressChanged -= this.networkAddressChangedHandler;
				this.networkAddressChangedHandler = null;
			}
			if (this.networkAvailabilityChangedHandler != null)
			{
				NetworkChange.NetworkAvailabilityChanged -= this.networkAvailabilityChangedHandler;
				this.networkAvailabilityChangedHandler = null;
			}
			if (this.dnsTimerEventHandler != null)
			{
				this.dnsTimer.Elapsed -= this.dnsTimerEventHandler;
				this.dnsTimerEventHandler = null;
			}
			if (this.dnsTimer != null)
			{
				this.dnsTimer.Dispose();
			}
			if (this.NatMgr != null)
			{
				this.NatMgr.Dispose();
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000447C File Offset: 0x0000267C
		public void PreInitialize()
		{
			if (!DefaultSettings.NATDisabled)
			{
				if (!DefaultSettings.DefaultSwitchDisabled)
				{
					using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
					{
						if (service.FindDefaultSwitch() != null)
						{
							DefaultSettings.NATDisabled = true;
							DefaultSettings.UseDefaultSwitch = true;
						}
					}
				}
				if (!DefaultSettings.NATDisabled)
				{
					this.NatMgr = new XdeNatManager(this);
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000044E0 File Offset: 0x000026E0
		public bool TryAquireNecessaryPermissions()
		{
			return this.NatMgr == null || this.NatMgr.TryAquireNecessaryPermissions();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000044F8 File Offset: 0x000026F8
		public bool InitializeNetworkConfig()
		{
			if (DefaultSettings.NATDisabled && !DefaultSettings.UseDefaultSwitch && !this.InitializeExternalSwitches())
			{
				return false;
			}
			if (this.NatMgr != null)
			{
				this.InitializeMacAddressRange();
				if (!this.NatMgr.Initialize())
				{
					return false;
				}
			}
			this.networkAddressChangedHandler = new NetworkAddressChangedEventHandler(this.NetworkChangedCallback);
			NetworkChange.NetworkAddressChanged += this.networkAddressChangedHandler;
			this.networkAvailabilityChangedHandler = new NetworkAvailabilityChangedEventHandler(this.NetworkChangedCallback);
			NetworkChange.NetworkAvailabilityChanged += this.networkAvailabilityChangedHandler;
			this.dnsTimer = new Timer(3000.0);
			this.dnsTimer.AutoReset = false;
			this.dnsTimer.Enabled = true;
			this.dnsTimerEventHandler = new ElapsedEventHandler(this.DnsTimerElapsed);
			this.dnsTimer.Elapsed += this.dnsTimerEventHandler;
			return true;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000045C4 File Offset: 0x000027C4
		public void UpdateGuestNetworkProperties(IXdeSimpleCommandsPipe simpleCommandsPipe)
		{
			if (simpleCommandsPipe == null)
			{
				simpleCommandsPipe = (((IXdeFeature)this.simpleCommandsFeature).Connection as IXdeSimpleCommandsPipe);
			}
			simpleCommandsPipe.SetupGuestProxyAndDNSServers();
			if (this.NatMgr != null)
			{
				this.NatMgr.UpdateGuestNetworkProperties(simpleCommandsPipe);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000045FC File Offset: 0x000027FC
		public bool InitializeInternalSwitch(string switchName, string staticIPStr, string subnetMaskStr)
		{
			IPAddress staticIP = IPAddress.Parse(staticIPStr);
			IPAddress subnetMask = IPAddress.Parse(subnetMaskStr);
			return this.InitializeInternalSwitch(switchName, staticIP, subnetMask);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004620 File Offset: 0x00002820
		public bool InitializeInternalSwitch(string switchName, IPAddress staticIP, IPAddress subnetMask)
		{
			bool result;
			try
			{
				using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
				{
					if (service.FindInternalSwitch(switchName) == null)
					{
						this.CreateInternalSwitch(switchName, staticIP, subnetMask);
					}
					else
					{
						this.FixInternalNetworkAdapterSettingsIfNeeded(switchName);
					}
					result = true;
				}
			}
			catch (Exception exception)
			{
				this.UiFactory.ShowErrorMessageForException(Strings.FailedToInitializeInternalSwitchFormat, exception, "InternalSwitchInitialized");
				result = false;
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004694 File Offset: 0x00002894
		public void CreateInternalSwitch(string switchFriendlyName, IPAddress staticIP, IPAddress subnetMask)
		{
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				service.XdeCreateInternalVirtualSwitch(switchFriendlyName);
				try
				{
					this.SetStaticIpOnHostNic(switchFriendlyName, staticIP, subnetMask);
				}
				catch (Exception ex)
				{
					service.XdeDeleteVirtualSwitch(switchFriendlyName);
					throw new Exception(StringUtilities.CurrentCultureFormat("Something happened while creating a switch: {0}", new object[]
					{
						ex.Message
					}), ex);
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004708 File Offset: 0x00002908
		public void SetStaticIpOnHostNic(string switchName, IPAddress staticIP, IPAddress subnetMask)
		{
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				ManagementObject managementObject = service.FindInternalSwitch(switchName);
				if (managementObject == null)
				{
					throw new Exception(Strings.FailedToSetIPAddressOnNatNic);
				}
				if (!UacSecurity.IsHyperVAdmin() && !UacSecurity.IsAdmin())
				{
					this.ShowElevationRequiredForInternalAdapter();
				}
				else
				{
					bool flag = false;
					for (;;)
					{
						try
						{
							service.SetInternalNetworkAdapterStaticIP(managementObject, staticIP, subnetMask);
							break;
						}
						catch (Exception ex)
						{
							if (flag)
							{
								throw new Exception(StringUtilities.CurrentCultureFormat(Strings.FailedToSetIPAddressOnNatNic, new object[]
								{
									ex.Message
								}), ex);
							}
							this.FixInternalNetworkAdapterSettingsIfNeeded(switchName);
							flag = true;
						}
					}
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000047AC File Offset: 0x000029AC
		public void FixInternalNetworkAdapterSettingsIfNeeded(string switchFriendlyName)
		{
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				try
				{
					if (service.InternalNetworkAdapterFixNeeded(switchFriendlyName))
					{
						Logger.Instance.LogError("InteralAdapterRepairAttempted");
						if (this.CheckPrivilegesForFixingAdapter())
						{
							service.FixInternalNetworkAdapterSettings(switchFriendlyName);
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004814 File Offset: 0x00002A14
		private void ShowElevationRequiredForInternalAdapter()
		{
			this.UiFactory.ShowElevationDialog(string.Format(Strings.RetryRunningAsAdmin, Array.Empty<object>()), Strings.CannotModifyInternalAdapter, true);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004838 File Offset: 0x00002A38
		private bool InitializeExternalSwitches()
		{
			bool result;
			try
			{
				using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
				{
					if (service.UnboundNicExists())
					{
						TaskDialogResult taskDialogResult = this.UiFactory.ShowTaskDialog(Strings.ConfigureExternalNicInstruction, Strings.ConfigureExternalNicText, TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No, TaskDialogResult.Yes, TaskDialogStandardIcon.Warning);
						if (taskDialogResult == TaskDialogResult.Yes)
						{
							service.CreateSwitchesForAllUnboundNics();
						}
						Logger.Instance.LogDialogRespondedTo("ConnectExternalSwitches", (uint)taskDialogResult);
					}
				}
				result = true;
			}
			catch (Exception exception)
			{
				this.UiFactory.ShowErrorMessageForException(Strings.FailedToInitializeExternalSwitchesFormat, exception, "ExternalSwitchesInitialized");
				result = false;
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000048D4 File Offset: 0x00002AD4
		private bool CheckPrivilegesForFixingAdapter()
		{
			bool result = false;
			if (!UacSecurity.IsHyperVAdmin() && !UacSecurity.IsAdmin())
			{
				TaskDialogResult taskDialogResult = this.UiFactory.ShowTaskDialog(Strings.FixNetworkAdapterSettingsInstruction, Strings.FixNetworkAdapterSettingsText, TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel, TaskDialogResult.Cancel, TaskDialogStandardIcon.Warning);
				Logger.Instance.LogDialogRespondedTo("FixInternalNetworkAdapterSettings", (uint)taskDialogResult);
				if (taskDialogResult == TaskDialogResult.Ok)
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000492C File Offset: 0x00002B2C
		private void InitializeMacAddressRange()
		{
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				service.InitializeMacAddressRange();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004964 File Offset: 0x00002B64
		private void NetworkChangedCallback(object sender, EventArgs e)
		{
			if (!this.disposed)
			{
				if (this.NatMgr != null)
				{
					this.NatMgr.HandleNetworkChangeEvent();
				}
				this.dnsTimer.Start();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000498C File Offset: 0x00002B8C
		private void DnsTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (!this.disposed)
			{
				this.simpleCommandsFeature.SetupGuestProxyAndDNSServers();
			}
		}

		// Token: 0x0400003F RID: 63
		private const int DnsResolutionDelay = 3000;

		// Token: 0x04000040 RID: 64
		private bool disposed;

		// Token: 0x04000041 RID: 65
		private IXdeAutomationSimpleCommandsPipe simpleCommandsFeature;

		// Token: 0x04000042 RID: 66
		private NetworkAddressChangedEventHandler networkAddressChangedHandler;

		// Token: 0x04000043 RID: 67
		private NetworkAvailabilityChangedEventHandler networkAvailabilityChangedHandler;

		// Token: 0x04000044 RID: 68
		private Timer dnsTimer;

		// Token: 0x04000045 RID: 69
		private ElapsedEventHandler dnsTimerEventHandler;
	}
}
