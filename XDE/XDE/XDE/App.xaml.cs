using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000012 RID: 18
	public partial class App : System.Windows.Application
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004712 File Offset: 0x00002912
		public static DateTime ProgramStartedAt
		{
			get
			{
				return Process.GetCurrentProcess().StartTime;
			}
		}

		// Token: 0x06000089 RID: 137
		[DllImport("Microsoft.Xde.Client.NativeAudioClient.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern void EnableVisualStyles();

		// Token: 0x0600008A RID: 138 RVA: 0x00004720 File Offset: 0x00002920
		protected override void OnStartup(StartupEventArgs e)
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			base.OnStartup(e);
			using (XdeController xdeController = new XdeController())
			{
				XdeReturnCode exitCode = xdeController.Run(e.Args);
				System.Windows.Application.Current.Shutdown((int)exitCode);
			}
		}
	}
}
