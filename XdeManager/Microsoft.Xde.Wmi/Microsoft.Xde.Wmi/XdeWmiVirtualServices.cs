using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000014 RID: 20
	public class XdeWmiVirtualServices : IXdeWmiVirtualServices, IXdeVirtualServices
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00008529 File Offset: 0x00006729
		public bool CanAccessApi
		{
			get
			{
				return this.GetHyperVManagementService() != null;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008534 File Offset: 0x00006734
		public bool IsGpuSupported
		{
			get
			{
				return WmiUtils.IsRemoteFxAvailable();
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000853B File Offset: 0x0000673B
		public IXdeVirtualMachine GetVirtualMachine(string vmName)
		{
			return this.GetVirtualMachine(vmName, SettingsOptions.None);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008545 File Offset: 0x00006745
		public IXdeVirtualMachine GetVirtualMachine(string vmName, SettingsOptions options)
		{
			return XdeVirtualMachine.GetVirtualMachine(vmName, options);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000854E File Offset: 0x0000674E
		public IXdeVirtualMachine CreateVirtualMachine(string vmName)
		{
			return this.CreateVirtualMachine(vmName, false);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008558 File Offset: 0x00006758
		public IXdeVirtualMachine CreateVirtualMachine(string vmName, bool generation2)
		{
			return XdeVirtualMachine.CreateVirtualMachine(vmName, generation2);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008564 File Offset: 0x00006764
		public ReadOnlyCollection<IXdeVirtualMachine> GetAllXdeVirtualMachines(SettingsOptions settingsOptions)
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_VirtualSystemSettingData where VirtualSystemType='{0}'", new object[]
			{
				"Microsoft:Hyper-V:System:Realized"
			});
			ManagementObjectCollection queryHelper = WmiBaseUtils.GetQueryHelper(WmiBaseUtils.GetVirtualizationV2Scope(), query);
			List<IXdeVirtualMachine> list = new List<IXdeVirtualMachine>();
			foreach (ManagementBaseObject managementBaseObject in queryHelper)
			{
				string[] array = managementBaseObject["Notes"] as string[];
				if (array != null && array.Length != 0 && array[0].StartsWith(XdeVirtualMachine.NotesHeader, StringComparison.OrdinalIgnoreCase))
				{
					IXdeVirtualMachine virtualMachineByUniqueId = XdeVirtualMachine.GetVirtualMachineByUniqueId((string)((ManagementObject)managementBaseObject)["VirtualSystemIdentifier"], settingsOptions);
					list.Add(virtualMachineByUniqueId);
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008628 File Offset: 0x00006828
		public IXdeHyperVManagementService GetHyperVManagementService()
		{
			return XdeHyperVManagementService.GetService();
		}
	}
}
