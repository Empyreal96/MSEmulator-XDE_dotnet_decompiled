using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000043 RID: 67
	public interface IXdeNetworkThrottlingConfig : INotifyPropertyChanged
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000180 RID: 384
		// (set) Token: 0x06000181 RID: 385
		bool IsNetworkSimulationEnabled { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000182 RID: 386
		// (set) Token: 0x06000183 RID: 387
		NetworkThrottlingSpeed NetThrottlingSpeed { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000184 RID: 388
		// (set) Token: 0x06000185 RID: 389
		NetworkThrottlingSignalStrength SignalStrength { get; set; }

		// Token: 0x06000186 RID: 390
		ThrottlerParams GetThrottlerParamsForCurrentSettings();
	}
}
