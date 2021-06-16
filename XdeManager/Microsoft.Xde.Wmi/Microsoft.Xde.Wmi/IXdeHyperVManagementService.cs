using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Management;
using System.Net;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000005 RID: 5
	public interface IXdeHyperVManagementService : IDisposable
	{
		// Token: 0x0600000E RID: 14
		bool InternalNetworkAdapterFixNeeded(string switchFriendlyName);

		// Token: 0x0600000F RID: 15
		bool UnboundNicExists();

		// Token: 0x06000010 RID: 16
		void CreateSwitchesForAllUnboundNics();

		// Token: 0x06000011 RID: 17
		ManagementObject XdeCreateExternalVirtualSwitch(ManagementObject externalNic, string switchFriendlyName);

		// Token: 0x06000012 RID: 18
		ManagementObject XdeCreateInternalVirtualSwitch(string switchFriendlyName);

		// Token: 0x06000013 RID: 19
		void XdeDeleteVirtualSwitch(ManagementObject switchObject);

		// Token: 0x06000014 RID: 20
		void XdeDeleteVirtualSwitch(string switchFriendlyName);

		// Token: 0x06000015 RID: 21
		ManagementObject FindSwitchByUniqueId(string id);

		// Token: 0x06000016 RID: 22
		ManagementObject FindSwitchV2(string switchFriendlyName);

		// Token: 0x06000017 RID: 23
		ManagementObject FindSwitchV2ByUniqueId(string id);

		// Token: 0x06000018 RID: 24
		ManagementObject FindDefaultSwitch();

		// Token: 0x06000019 RID: 25
		ManagementObject FindSwitchConnectedToNicV2(ManagementObject nicPort);

		// Token: 0x0600001A RID: 26
		bool IsExternalSwitchV2(ManagementObject switchObject);

		// Token: 0x0600001B RID: 27
		ManagementObject DefineVirtualSystem(string virtualMachineName);

		// Token: 0x0600001C RID: 28
		ManagementObject DefineVirtualSystem(string virtualMachineName, bool generation2);

		// Token: 0x0600001D RID: 29
		void DestroyVirtualSystem(ManagementObject virtualMachineObj);

		// Token: 0x0600001E RID: 30
		string[] AddVirtualSystemResources(ManagementObject vm, ManagementObject resObj, string resName);

		// Token: 0x0600001F RID: 31
		void DeleteVirtualSystemResource(ManagementObject resObj, string resName);

		// Token: 0x06000020 RID: 32
		void ModifyVirtualSystem(ManagementObject settingsObj);

		// Token: 0x06000021 RID: 33
		void ModifyVirtualSystemResources(ManagementObject resObj, string resName);

		// Token: 0x06000022 RID: 34
		Image GetVirtualSystemThumbnailImage(ManagementObject virtualMachineObj, int width, int height);

		// Token: 0x06000023 RID: 35
		ulong GetSettingsSizeOfSystemFiles(ManagementObject virtualMachineSettingsObj);

		// Token: 0x06000024 RID: 36
		void CreateDifferencingVirtualHardDisk(string diffDiskPath, string parentPath);

		// Token: 0x06000025 RID: 37
		IXdeVirtualSwitchInformation GetSwitchInformation(ManagementObject switchObj);

		// Token: 0x06000026 RID: 38
		ReadOnlyCollection<IXdeVirtualSwitchInformation> GetExternalSwitchInfos();

		// Token: 0x06000027 RID: 39
		string FindNextAvailableInternalMacAddress(string switchFriendlyName);

		// Token: 0x06000028 RID: 40
		ManagementObject FindInternalSwitch(string switchFriendlyName);

		// Token: 0x06000029 RID: 41
		void InitializeMacAddressRange();

		// Token: 0x0600002A RID: 42
		void FixInternalNetworkAdapterSettings(string switchFriendlyName);

		// Token: 0x0600002B RID: 43
		void SetInternalNetworkAdapterStaticIP(ManagementObject internalSwitch, IPAddress staticIP, IPAddress subnetMask);

		// Token: 0x0600002C RID: 44
		VirtualHardDiskSettingData GetVhdInformation(string vhdFileName);

		// Token: 0x0600002D RID: 45
		bool IsNicValidForInternalUse(ManagementObject portSettings, string switchFriendlyName);
	}
}
