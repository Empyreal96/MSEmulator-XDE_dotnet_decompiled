using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager
{
	// Token: 0x02000008 RID: 8
	public partial class App : Application
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000028C8 File Offset: 0x00000AC8
		protected override void OnStartup(StartupEventArgs e)
		{
			string deviceName = (e.Args.Length != 0) ? e.Args[0] : null;
			if (deviceName != null)
			{
				XdeDevice xdeDevice = XdeDeviceFactory.GetDevices().FirstOrDefault((XdeDevice d) => StringComparer.OrdinalIgnoreCase.Equals(d.Name, deviceName));
				if (xdeDevice != null)
				{
					xdeDevice.Start(true);
					base.Shutdown();
					return;
				}
			}
			base.OnStartup(e);
		}
	}
}
