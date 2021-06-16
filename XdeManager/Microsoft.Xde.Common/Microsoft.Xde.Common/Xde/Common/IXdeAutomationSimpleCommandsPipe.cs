using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000059 RID: 89
	public interface IXdeAutomationSimpleCommandsPipe : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x060001BA RID: 442
		Size GetScreenSize();

		// Token: 0x060001BB RID: 443
		void InitiateSystemShutdown();

		// Token: 0x060001BC RID: 444
		void InitiateSystemReboot();

		// Token: 0x060001BD RID: 445
		void IndicateInternetConnectivityReprobe();

		// Token: 0x060001BE RID: 446
		NetworkAdapterInformation[] GetGuestAdapterInformation();

		// Token: 0x060001BF RID: 447
		void InitiateIPRenew();

		// Token: 0x060001C0 RID: 448
		XdeSensors GetSensorsEnabledStates();

		// Token: 0x060001C1 RID: 449
		void SetSensorsEnabledStates(XdeSensors sensorsEnabledStatesBV);

		// Token: 0x060001C2 RID: 450
		void SetupGuestProxyAndDNSServers();

		// Token: 0x060001C3 RID: 451
		void SetGuestNATSubnet(IPSubnet subnet, IPAddress gateway);

		// Token: 0x060001C4 RID: 452
		void SetGuestSystemTimeAndZone();

		// Token: 0x060001C5 RID: 453
		void SetNetworkThrottlingParams(ThrottlerParams throttlingParams);

		// Token: 0x060001C6 RID: 454
		void SetEnabledMonitors(int monitorsToEnable);
	}
}
