using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x0200000E RID: 14
	public static class XdeDeviceFactory
	{
		// Token: 0x0600012B RID: 299 RVA: 0x000046F6 File Offset: 0x000028F6
		public static IEnumerable<XdeDevice> GetDevices()
		{
			foreach (OptionalPackageDevice optionalPackageDevice in OptionalPackageDevice.LoadDevices())
			{
				yield return optionalPackageDevice;
			}
			IEnumerator<OptionalPackageDevice> enumerator = null;
			foreach (AppXdeDevice appXdeDevice in AppXdeDevice.LoadDevices())
			{
				yield return appXdeDevice;
			}
			IEnumerator<AppXdeDevice> enumerator2 = null;
			foreach (VisualStudioXdeDevice visualStudioXdeDevice in VisualStudioXdeDevice.LoadDevices())
			{
				yield return visualStudioXdeDevice;
			}
			IEnumerator<VisualStudioXdeDevice> enumerator3 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004700 File Offset: 0x00002900
		public static XdeDevice FindDevice(string name)
		{
			return XdeDeviceFactory.GetDevices().FirstOrDefault((XdeDevice d) => StringComparer.OrdinalIgnoreCase.Equals(d.Name, name));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004730 File Offset: 0x00002930
		public static XdeDevice InitNewDevice()
		{
			if (!XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				throw new InvalidOperationException("The packaged emulator is not installed.");
			}
			return AppXdeDevice.InitNewDevice(null);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000474A File Offset: 0x0000294A
		internal static void FireDeviceListChanged()
		{
			EventHandler deviceListChanged = XdeDeviceFactory.DeviceListChanged;
			if (deviceListChanged == null)
			{
				return;
			}
			deviceListChanged(null, EventArgs.Empty);
		}

		// Token: 0x04000046 RID: 70
		public static EventHandler DeviceListChanged;
	}
}
