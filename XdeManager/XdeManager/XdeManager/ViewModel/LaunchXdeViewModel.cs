using System;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x02000010 RID: 16
	public class LaunchXdeViewModel : ViewModelBase
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000040F0 File Offset: 0x000022F0
		public LaunchXdeViewModel(XdeDevice xdeDevice)
		{
			this.XdePath = xdeDevice.ResolveXdeLocation();
			this.CommandLineArgs = xdeDevice.GetCommandLine();
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004110 File Offset: 0x00002310
		public string XdePath { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004118 File Offset: 0x00002318
		public string CommandLineArgs { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004120 File Offset: 0x00002320
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004128 File Offset: 0x00002328
		public string AdditionalCommandLineArgs { get; set; }
	}
}
