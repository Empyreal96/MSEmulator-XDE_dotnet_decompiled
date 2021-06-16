using System;
using System.Management;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000008 RID: 8
	internal class VirtualEthernetSwitchManagementService : IDisposable
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000233C File Offset: 0x0000053C
		private VirtualEthernetSwitchManagementService(ManagementObject service)
		{
			this.switchMgmtService = service;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000234B File Offset: 0x0000054B
		public void Dispose()
		{
			if (!this.disposed)
			{
				if (this.switchMgmtService != null)
				{
					this.switchMgmtService.Dispose();
				}
				this.switchMgmtService = null;
				this.disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000237C File Offset: 0x0000057C
		internal static VirtualEthernetSwitchManagementService GetService()
		{
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			WmiUtils.DebugAssert(virtualizationV2Scope != null);
			ManagementObject serviceObject = WmiUtils.GetServiceObject(virtualizationV2Scope, "Msvm_VirtualEthernetSwitchManagementService");
			if (serviceObject != null)
			{
				return new VirtualEthernetSwitchManagementService(serviceObject);
			}
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000023B0 File Offset: 0x000005B0
		internal ManagementObject DefineSystem(string systemSettings, string[] resourceSettings)
		{
			ManagementObject result;
			using (ManagementBaseObject methodParameters = this.switchMgmtService.GetMethodParameters("DefineSystem"))
			{
				methodParameters["SystemSettings"] = systemSettings;
				methodParameters["ResourceSettings"] = resourceSettings;
				using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.switchMgmtService, methodParameters, "DefineSystem", XdeVmExceptions.CreateSwitchFailure))
				{
					result = (ManagementObject)managementBaseObject["ResultingSystem"];
				}
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002444 File Offset: 0x00000644
		internal void DestroySystem(ManagementObject switchObj)
		{
			using (ManagementBaseObject methodParameters = this.switchMgmtService.GetMethodParameters("DestroySystem"))
			{
				methodParameters["AffectedSystem"] = switchObj.Path.Path;
				WmiUtils.InvokeMethodWithVerify(this.switchMgmtService, methodParameters, "DestroySystem", XdeVmExceptions.DeleteSwitchFailure);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000024AC File Offset: 0x000006AC
		internal ManagementObject GetVirtualEthernetSwitchSettingData()
		{
			ManagementPath path = new ManagementPath("Msvm_VirtualEthernetSwitchSettingData");
			ManagementObject result;
			using (ManagementClass managementClass = new ManagementClass(this.switchMgmtService.Scope, path, null))
			{
				result = managementClass.CreateInstance();
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000024FC File Offset: 0x000006FC
		private ManagementObject GetVirtualSystemSettingDataInstance()
		{
			ManagementPath path = new ManagementPath("Msvm_VirtualSystemSettingData");
			ManagementObject result;
			using (ManagementClass managementClass = new ManagementClass(this.switchMgmtService.Scope, path, null))
			{
				result = managementClass.CreateInstance();
			}
			return result;
		}

		// Token: 0x04000007 RID: 7
		private ManagementObject switchMgmtService;

		// Token: 0x04000008 RID: 8
		private bool disposed;
	}
}
