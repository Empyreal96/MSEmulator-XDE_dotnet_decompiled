using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000009 RID: 9
	internal class VirtualSystemManagementService : IDisposable
	{
		// Token: 0x0600003C RID: 60 RVA: 0x0000254C File Offset: 0x0000074C
		private VirtualSystemManagementService(ManagementObject service)
		{
			WmiUtils.DebugAssert(service != null);
			this.virtualSystemManagementService = service;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002564 File Offset: 0x00000764
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002574 File Offset: 0x00000774
		internal static VirtualSystemManagementService GetService()
		{
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			WmiUtils.DebugAssert(virtualizationV2Scope != null);
			VirtualSystemManagementService result = null;
			ManagementObject serviceObject = WmiUtils.GetServiceObject(virtualizationV2Scope, "Msvm_VirtualSystemManagementService");
			if (serviceObject != null)
			{
				result = new VirtualSystemManagementService(serviceObject);
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000025A8 File Offset: 0x000007A8
		internal void ModifyVirtualSystemResources(ManagementObject resObj, string resName)
		{
			WmiUtils.DebugAssert(resObj != null);
			WmiUtils.DebugAssert(((string[])resObj["__DERIVATION"]).Any((string c) => StringUtilities.EqualsNoCase(c, "CIM_ResourceAllocationSettingData")));
			string failedTaskDescriptionFormat = StringUtilities.CurrentCultureFormat(XdeVmExceptions.ConfigChange, new object[]
			{
				resName
			});
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("ModifyResourceSettings"))
			{
				methodParameters["ResourceSettings"] = new string[]
				{
					resObj.GetText(TextFormat.CimDtd20)
				};
				WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "ModifyResourceSettings", failedTaskDescriptionFormat);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002668 File Offset: 0x00000868
		internal string[] AddVirtualSystemResources(ManagementObject vmSettings, ManagementObject resObj, string resName)
		{
			WmiUtils.DebugAssert(vmSettings != null);
			WmiUtils.DebugAssert(resObj != null);
			string failedTaskDescriptionFormat = StringUtilities.CurrentCultureFormat(XdeVmExceptions.ConfigChange, new object[]
			{
				resName
			});
			string[] result;
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("AddResourceSettings"))
			{
				methodParameters["AffectedConfiguration"] = vmSettings.Path.Path;
				methodParameters["ResourceSettings"] = new string[]
				{
					resObj.GetText(TextFormat.CimDtd20)
				};
				using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "AddResourceSettings", failedTaskDescriptionFormat))
				{
					WmiUtils.DebugAssert(managementBaseObject != null);
					result = (string[])managementBaseObject["ResultingResourceSettings"];
				}
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002740 File Offset: 0x00000940
		internal void DeleteVirtualSystemResource(ManagementObject resObj, string resName)
		{
			WmiUtils.DebugAssert(resObj != null);
			string failedTaskDescriptionFormat = StringUtilities.CurrentCultureFormat(XdeVmExceptions.ConfigChange, new object[]
			{
				resName
			});
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("RemoveResourceSettings"))
			{
				methodParameters["ResourceSettings"] = new string[]
				{
					resObj.Path.Path
				};
				WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "RemoveResourceSettings", failedTaskDescriptionFormat);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000027CC File Offset: 0x000009CC
		internal ManagementObject DefineVirtualSystem(string vmName, bool generation2)
		{
			WmiUtils.DebugAssert(!string.IsNullOrEmpty(vmName));
			ManagementObject result;
			using (ManagementObject virtualSystemSettingDataInstance = this.GetVirtualSystemSettingDataInstance())
			{
				using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("DefineSystem"))
				{
					virtualSystemSettingDataInstance["ElementName"] = vmName;
					virtualSystemSettingDataInstance["AutomaticStartupAction"] = 2;
					virtualSystemSettingDataInstance["AutomaticShutdownAction"] = 2;
					if (generation2)
					{
						virtualSystemSettingDataInstance["VirtualSystemSubtype"] = "Microsoft:Hyper-V:SubType:2";
						virtualSystemSettingDataInstance["SecureBootEnabled"] = false;
					}
					methodParameters["SystemSettings"] = virtualSystemSettingDataInstance.GetText(TextFormat.CimDtd20);
					using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "DefineSystem", XdeVmExceptions.CreateVmFailure))
					{
						WmiUtils.DebugAssert(managementBaseObject != null);
						result = new ManagementObject(managementBaseObject["ResultingSystem"].ToString());
					}
				}
			}
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000028E4 File Offset: 0x00000AE4
		internal void DestroyVirtualSystem(ManagementObject vmObj)
		{
			WmiUtils.DebugAssert(vmObj != null);
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("DestroySystem"))
			{
				methodParameters["AffectedSystem"] = vmObj.Path.Path;
				WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "DestroySystem", XdeVmExceptions.DeleteVmFailure);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002954 File Offset: 0x00000B54
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		internal Image GetVirtualSystemThumbnailImage(ManagementObject vmObj, int width, int height)
		{
			WmiUtils.DebugAssert(vmObj != null);
			WmiUtils.DebugAssert(vmObj["__CLASS"].ToString() == "Msvm_ComputerSystem" || vmObj["__CLASS"].ToString() == "Msvm_VirtualSystemSettingData");
			ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("GetVirtualSystemThumbnailImage");
			methodParameters["TargetSystem"] = vmObj.Path.Path;
			methodParameters["WidthPixels"] = width;
			methodParameters["HeightPixels"] = height;
			Image result;
			using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "GetVirtualSystemThumbnailImage", XdeVmExceptions.GetThumbnailError))
			{
				WmiUtils.DebugAssert(managementBaseObject != null);
				WmiUtils.DebugAssert(managementBaseObject["ImageData"] != null);
				Bitmap bitmap = null;
				try
				{
					bitmap = new Bitmap(width, height, PixelFormat.Format16bppRgb565);
					Rectangle rect = new Rectangle(0, 0, width, height);
					BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);
					if (bitmapData == null)
					{
						throw new XdeVirtualMachineException(XdeVmExceptions.GetThumbnailError);
					}
					Marshal.Copy((byte[])managementBaseObject["ImageData"], 0, bitmapData.Scan0, width * height * 2);
					bitmap.UnlockBits(bitmapData);
					result = bitmap;
				}
				catch
				{
					if (bitmap != null)
					{
						bitmap.Dispose();
					}
					throw;
				}
			}
			return result;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002AC0 File Offset: 0x00000CC0
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		internal ulong GetSettingsSizeOfSystemFiles(ManagementObject settingsObj)
		{
			WmiUtils.DebugAssert(settingsObj != null);
			ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("GetSizeOfSystemFiles");
			methodParameters["Vssd"] = settingsObj.Path.Path;
			ulong result;
			using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "GetSizeOfSystemFiles", XdeVmExceptions.GetSystemFilesSizeError))
			{
				WmiUtils.DebugAssert(managementBaseObject != null);
				result = (ulong)managementBaseObject["Size"];
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B4C File Offset: 0x00000D4C
		internal void CreateSnapshot(ManagementObject virtualMachine, string snapshotName)
		{
			using (ManagementObject serviceObject = WmiUtils.GetServiceObject(virtualMachine.Scope, "Msvm_VirtualSystemSnapshotService"))
			{
				using (ManagementBaseObject methodParameters = serviceObject.GetMethodParameters("CreateSnapshot"))
				{
					methodParameters["AffectedSystem"] = virtualMachine.Path.Path;
					methodParameters["SnapshotSettings"] = string.Empty;
					methodParameters["SnapshotType"] = 2;
					using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(serviceObject, methodParameters, "CreateSnapshot", XdeVmExceptions.CreateSnapshotFailure))
					{
						WmiUtils.DebugAssert(managementBaseObject != null);
						if (!string.IsNullOrEmpty(snapshotName))
						{
							ManagementObject snapshot = new ManagementObject(managementBaseObject["Job"].ToString()).GetRelated("Msvm_VirtualSystemSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
							this.RenameSnapshot(snapshot, snapshotName);
						}
					}
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C4C File Offset: 0x00000E4C
		internal void DeleteSnapshot(ManagementObject snapshot)
		{
			using (ManagementObject serviceObject = WmiUtils.GetServiceObject(snapshot.Scope, "Msvm_VirtualSystemSnapshotService"))
			{
				using (ManagementBaseObject methodParameters = serviceObject.GetMethodParameters("DestroySnapshot"))
				{
					methodParameters["AffectedSnapshot"] = snapshot.Path.Path;
					WmiUtils.InvokeMethodWithVerify(serviceObject, methodParameters, "DestroySnapshot", XdeVmExceptions.DeleteSnapshotFailure);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002CD0 File Offset: 0x00000ED0
		internal void RenameSnapshot(ManagementObject snapshot, string name)
		{
			snapshot["ElementName"] = name;
			this.ModifyVirtualSystem(snapshot.GetText(TextFormat.CimDtd20));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CEC File Offset: 0x00000EEC
		internal void ModifyVirtualSystem(string data)
		{
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("ModifySystemSettings"))
			{
				methodParameters["SystemSettings"] = data;
				using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "ModifySystemSettings", XdeVmExceptions.ModifyVirtualSystemFailure))
				{
					WmiUtils.DebugAssert(managementBaseObject != null);
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D68 File Offset: 0x00000F68
		internal void ApplySnapshot(ManagementObject virtualMachine, ManagementObject snapshotSettings)
		{
			using (ManagementObject serviceObject = WmiUtils.GetServiceObject(virtualMachine.Scope, "Msvm_VirtualSystemSnapshotService"))
			{
				using (ManagementBaseObject methodParameters = serviceObject.GetMethodParameters("ApplySnapshot"))
				{
					methodParameters["Snapshot"] = snapshotSettings.Path.Path;
					using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(serviceObject, methodParameters, "ApplySnapshot", XdeVmExceptions.ModifyVirtualSystemFailure))
					{
						WmiUtils.DebugAssert(managementBaseObject != null);
					}
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002E0C File Offset: 0x0000100C
		internal void RemoveSnapshot(ManagementObject virtualMachine, string snapshotName)
		{
			ManagementObject snapshot = this.FindSnapshot(virtualMachine, snapshotName);
			this.DeleteSnapshot(snapshot);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002E2C File Offset: 0x0000102C
		internal void ModifyServiceSettings(string settingData)
		{
			using (ManagementBaseObject methodParameters = this.virtualSystemManagementService.GetMethodParameters("ModifyServiceSettings"))
			{
				methodParameters["SettingData"] = settingData;
				using (ManagementBaseObject managementBaseObject = WmiUtils.InvokeMethodWithVerify(this.virtualSystemManagementService, methodParameters, "ModifyServiceSettings", XdeVmExceptions.ModifyVirtualSystemFailure))
				{
					WmiUtils.DebugAssert(managementBaseObject != null);
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002EA8 File Offset: 0x000010A8
		internal ManagementObject GetVirtualSystemManagementServiceSettingData()
		{
			return WmiBaseUtils.CreateManagementObject(this.virtualSystemManagementService.Scope, "Msvm_VirtualSystemManagementServiceSettingData");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002EBF File Offset: 0x000010BF
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing && this.virtualSystemManagementService != null)
				{
					this.virtualSystemManagementService.Dispose();
				}
				this.virtualSystemManagementService = null;
				this.disposed = true;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002EED File Offset: 0x000010ED
		private ManagementObject GetVirtualSystemSettingDataInstance()
		{
			return WmiBaseUtils.CreateManagementObject(this.virtualSystemManagementService.Scope, "Msvm_VirtualSystemSettingData");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F04 File Offset: 0x00001104
		private ManagementObject FindSnapshot(ManagementObject virtualMachine, string snapshotName)
		{
			ManagementObject managementObject = (from ManagementObject o in virtualMachine.GetRelated("Msvm_VirtualSystemSettingData")
			where StringUtilities.EqualsNoCase(o["ElementName"].ToString(), snapshotName)
			select o).FirstOrDefault<ManagementObject>();
			if (managementObject == null)
			{
				throw new ArgumentException(XdeVmExceptions.SnapshotNameNotFound);
			}
			return managementObject;
		}

		// Token: 0x04000009 RID: 9
		private ManagementObject virtualSystemManagementService;

		// Token: 0x0400000A RID: 10
		private bool disposed;
	}
}
