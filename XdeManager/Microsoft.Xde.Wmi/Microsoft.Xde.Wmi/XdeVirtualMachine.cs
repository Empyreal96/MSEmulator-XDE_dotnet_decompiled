using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Xde.Common;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000E RID: 14
	public sealed class XdeVirtualMachine : IXdeVirtualMachine, IDisposable
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00005348 File Offset: 0x00003548
		private XdeVirtualMachine(string vmName, ManagementObject vmObj, SettingsOptions settingsOptions)
		{
			this.vmName = vmName;
			this.vmComputerSystem = vmObj;
			this.settingsOptions = settingsOptions;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000AF RID: 175 RVA: 0x00005368 File Offset: 0x00003568
		// (remove) Token: 0x060000B0 RID: 176 RVA: 0x000053A0 File Offset: 0x000035A0
		public event EventHandler<EnabledStateChangedEventArgs> EnableStateChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000B1 RID: 177 RVA: 0x000053D8 File Offset: 0x000035D8
		// (remove) Token: 0x060000B2 RID: 178 RVA: 0x00005410 File Offset: 0x00003610
		public event EventHandler<EventArgs> SnapshotsChanged;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005445 File Offset: 0x00003645
		public VirtualMachineEnabledState EnabledState
		{
			get
			{
				if (this.isDisposed)
				{
					return VirtualMachineEnabledState.Unknown;
				}
				this.vmComputerSystem.Get();
				return (VirtualMachineEnabledState)Convert.ToInt32(this.vmComputerSystem["EnabledState"].ToString());
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005478 File Offset: 0x00003678
		public string Guid
		{
			get
			{
				ManagementObject managementObject = this.vmComputerSystem;
				if (managementObject == null)
				{
					return default(Guid).ToString();
				}
				return (string)managementObject["Name"];
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000054B4 File Offset: 0x000036B4
		public string Name
		{
			get
			{
				return this.vmName;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000054BC File Offset: 0x000036BC
		public IXdeVirtualMachineSettings CurrentSettings
		{
			get
			{
				return this.currentSettings;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000054C4 File Offset: 0x000036C4
		public ReadOnlyCollection<IXdeVirtualMachineSettings> SnapshotSettings
		{
			get
			{
				this.EnsureCachedSnapshotSettings();
				return this.cachedSnapshotSettings;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000054D2 File Offset: 0x000036D2
		public IXdeVirtualMachineNicInformation InternalNATNic
		{
			get
			{
				return (from nic in this.CurrentSettings.Nics
				where XdeVirtualMachine.IsGuestInternalNatMAC(nic.GuestMacAddress) && !string.IsNullOrEmpty(nic.SwitchInformation.HostIpAddress)
				select nic).FirstOrDefault<IXdeVirtualMachineNicInformation>();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005508 File Offset: 0x00003708
		public ManagementObject ManagementObject
		{
			get
			{
				return this.vmComputerSystem;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00005510 File Offset: 0x00003710
		private IXdeHyperVManagementService XdeHyperVMgmtSvc
		{
			get
			{
				if (!this.isDisposed && this.xdeHyperVMgmtSvc == null)
				{
					this.xdeHyperVMgmtSvc = XdeHyperVManagementService.GetService();
				}
				return this.xdeHyperVMgmtSvc;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005533 File Offset: 0x00003733
		public static IXdeVirtualMachine GetVirtualMachine(string vmName)
		{
			return XdeVirtualMachine.GetVirtualMachine(vmName, SettingsOptions.None);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000553C File Offset: 0x0000373C
		public static IXdeVirtualMachine GetVirtualMachine(string vmName, SettingsOptions settingsOptions)
		{
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			ManagementObject targetComputer = WmiUtils.GetTargetComputer(vmName, virtualizationV2Scope);
			XdeVirtualMachine xdeVirtualMachine = null;
			if (targetComputer != null)
			{
				xdeVirtualMachine = new XdeVirtualMachine(vmName, targetComputer, settingsOptions);
				xdeVirtualMachine.Initialize();
			}
			return xdeVirtualMachine;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000556C File Offset: 0x0000376C
		public static IXdeVirtualMachine GetVirtualMachineByUniqueId(string vmUniqueId, SettingsOptions settingsOptions)
		{
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			ManagementObject targetComputerByUniqueId = WmiUtils.GetTargetComputerByUniqueId(vmUniqueId, virtualizationV2Scope);
			XdeVirtualMachine xdeVirtualMachine = null;
			if (targetComputerByUniqueId != null)
			{
				xdeVirtualMachine = new XdeVirtualMachine((string)targetComputerByUniqueId["ElementName"], targetComputerByUniqueId, settingsOptions);
				xdeVirtualMachine.Initialize();
			}
			return xdeVirtualMachine;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000055AC File Offset: 0x000037AC
		public static IXdeVirtualMachine CreateVirtualMachine(string vmName, bool generation2)
		{
			ManagementObject managementObject = null;
			XdeVirtualMachine xdeVirtualMachine = null;
			IXdeHyperVManagementService xdeHyperVManagementService = null;
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			using (ManagementObject targetComputer = WmiUtils.GetTargetComputer(vmName, virtualizationV2Scope))
			{
				if (targetComputer != null)
				{
					throw new InvalidOperationException(XdeVmExceptions.CreateVirtualMachineAlreadyExistError);
				}
			}
			try
			{
				xdeHyperVManagementService = XdeHyperVManagementService.GetService();
				managementObject = xdeHyperVManagementService.DefineVirtualSystem(vmName, generation2);
				xdeVirtualMachine = new XdeVirtualMachine(vmName, managementObject, SettingsOptions.None);
				xdeVirtualMachine.Initialize();
				xdeVirtualMachine.CurrentSettings.Notes = XdeVirtualMachine.NotesHeader;
				((XdeVirtualMachineSettings)xdeVirtualMachine.CurrentSettings).ExposeVirtualizationExtensions = true;
			}
			catch
			{
				if (xdeHyperVManagementService != null && managementObject != null)
				{
					if (xdeVirtualMachine != null)
					{
						xdeVirtualMachine.DeleteVirtualMachine();
					}
					else
					{
						xdeHyperVManagementService.DestroyVirtualSystem(managementObject);
					}
					managementObject.Dispose();
				}
				throw;
			}
			finally
			{
				if (xdeHyperVManagementService != null)
				{
					xdeHyperVManagementService.Dispose();
				}
			}
			return xdeVirtualMachine;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005684 File Offset: 0x00003884
		public static bool IsGuestInternalNatMAC(string macAddress)
		{
			return macAddress.Length == 12 && macAddress.StartsWith("02DFDFDFDF", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000056A0 File Offset: 0x000038A0
		public void DeleteVirtualMachine()
		{
			if (this.EnabledState != VirtualMachineEnabledState.Disabled)
			{
				this.Stop();
			}
			if (this.CurrentSettings != null)
			{
				this.CurrentSettings.CleanupForDeletion();
			}
			string createdDiffDisk = this.GetCreatedDiffDisk();
			if (!string.IsNullOrEmpty(createdDiffDisk))
			{
				string fileNameForVhd = WmiVhdBootSettings.GetFileNameForVhd(createdDiffDisk);
				if (File.Exists(fileNameForVhd))
				{
					File.Delete(fileNameForVhd);
				}
			}
			this.XdeHyperVMgmtSvc.DestroyVirtualSystem(this.vmComputerSystem);
			if (!string.IsNullOrEmpty(createdDiffDisk) && File.Exists(createdDiffDisk))
			{
				try
				{
					File.Delete(createdDiffDisk);
				}
				catch (Exception ex)
				{
					throw new Exception(StringUtilities.CurrentCultureFormat(XdeVmExceptions.FailedDeleteDiffDisk, new object[]
					{
						createdDiffDisk,
						ex.Message
					}), ex);
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005754 File Offset: 0x00003954
		public void Start()
		{
			if (this.EnabledState == VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemInUse);
			}
			try
			{
				this.RequestStateChange(VirtualMachineEnabledState.Enabled);
			}
			catch (Exception ex)
			{
				ManagementObject managementObject = this.currentSettings.ManagementObject.GetRelated("Msvm_Synthetic3DDisplayControllerSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
				if (managementObject != null)
				{
					string text = managementObject["InstanceID"].ToString();
					text = text.Substring(text.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
					if (ex.Message.Contains(text))
					{
						this.currentSettings.VGPUInformation = new VGPUInformation(VGPUStatus.VMFailedToStartWithVGPU, ex.Message);
						this.currentSettings.EnsureHasNonGpuDisplayAdapter();
						this.RequestStateChange(VirtualMachineEnabledState.Enabled);
						return;
					}
				}
				throw ex;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005818 File Offset: 0x00003A18
		public void Start(bool bootingToTakeSnapshot)
		{
			this.Start();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005820 File Offset: 0x00003A20
		public void Stop()
		{
			if (this.EnabledState == VirtualMachineEnabledState.Disabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemNotRunning);
			}
			this.RequestStateChange(VirtualMachineEnabledState.Disabled);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000583D File Offset: 0x00003A3D
		public void IntentionalShutdownComing()
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000583F File Offset: 0x00003A3F
		public void TypeKey(Keys key)
		{
			this.SendKeyHelper(key, "TypeKey");
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000584D File Offset: 0x00003A4D
		public void PressKey(Keys key)
		{
			this.SendKeyHelper(key, "PressKey");
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000585B File Offset: 0x00003A5B
		public void ReleaseKey(Keys key)
		{
			this.SendKeyHelper(key, "ReleaseKey");
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000586C File Offset: 0x00003A6C
		public void SendMouseEvent(MouseEventArgs args)
		{
			if (this.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemNotRunning);
			}
			if (this.mouse == null)
			{
				ManagementObjectCollection related = this.vmComputerSystem.GetRelated("Msvm_SyntheticMouse");
				this.mouse = related.Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			if (this.mouse == null)
			{
				throw new XdeVirtualMachineException(XdeVmExceptions.MouseNotFoundError);
			}
			this.mouse.InvokeMethod("SetAbsolutePosition", new object[]
			{
				args.X,
				args.Y
			});
			bool flag = (args.Button & MouseButtons.Left) == MouseButtons.Left;
			this.mouse.InvokeMethod("SetButtonState", new object[]
			{
				1,
				flag
			});
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000593C File Offset: 0x00003B3C
		public Size GetCurrentResolution()
		{
			if (this.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemNotRunning);
			}
			ManagementObject managementObject;
			if (this.currentSettings.Generation >= 2)
			{
				managementObject = this.vmComputerSystem.GetRelated("Msvm_VideoHead").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			else
			{
				ManagementObject managementObject2 = this.vmComputerSystem.GetRelated("Msvm_S3DisplayController").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
				if (managementObject2 == null)
				{
					throw new XdeVirtualMachineException(XdeVmExceptions.DisplayControllerError);
				}
				if (Convert.ToInt32(managementObject2["EnabledState"]) == 3)
				{
					managementObject2 = this.vmComputerSystem.GetRelated("Msvm_SyntheticDisplayController").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
					if (managementObject2 == null)
					{
						managementObject2 = this.vmComputerSystem.GetRelated("Msvm_Synthetic3DDisplayController").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
						if (managementObject2 == null)
						{
							throw new XdeVirtualMachineException(XdeVmExceptions.DisplayControllerError);
						}
					}
				}
				managementObject = managementObject2.GetRelated("Msvm_VideoHead").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			if (managementObject == null)
			{
				throw new XdeVirtualMachineException(XdeVmExceptions.DisplayControllerError);
			}
			return new Size(Convert.ToInt32(managementObject["CurrentHorizontalResolution"]), Convert.ToInt32(managementObject["CurrentVerticalResolution"]));
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005A5C File Offset: 0x00003C5C
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public Image GetScreenShot(int startX, int startY, int width, int height)
		{
			Image image = null;
			if (this.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemNotRunning);
			}
			Size currentResolution = this.GetCurrentResolution();
			if (startX > currentResolution.Width || startY > currentResolution.Height || startX - width > currentResolution.Width || startY - height > currentResolution.Height)
			{
				throw new ArgumentException(XdeVmExceptions.ResolutionOutOfBounds);
			}
			Image virtualSystemThumbnailImage = this.XdeHyperVMgmtSvc.GetVirtualSystemThumbnailImage(this.vmComputerSystem, currentResolution.Width, currentResolution.Height);
			Image result;
			try
			{
				image = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(image);
				Rectangle destRect = new Rectangle(0, 0, width, height);
				graphics.DrawImage(virtualSystemThumbnailImage, destRect, startX, startY, width, height, GraphicsUnit.Pixel);
				result = image;
			}
			catch
			{
				if (image != null)
				{
					image.Dispose();
				}
				throw;
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005B2C File Offset: 0x00003D2C
		public void CreateSnapshot(string snapshotName)
		{
			using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
			{
				service.CreateSnapshot(this.vmComputerSystem, snapshotName);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005B68 File Offset: 0x00003D68
		public void ApplySnapshot(IXdeVirtualMachineSettings snapshotSettings)
		{
			using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
			{
				service.ApplySnapshot(this.vmComputerSystem, ((XdeVirtualMachineSettings)snapshotSettings).ManagementObject);
			}
			this.currentSettings.IsSnapshot = true;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005BBC File Offset: 0x00003DBC
		public void RemoveSnapshot(string snapshotName)
		{
			using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
			{
				service.RemoveSnapshot(this.vmComputerSystem, snapshotName);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public IXdeVirtualMachineSettings FindSnapshotSettings(string snapshotName)
		{
			IXdeVirtualMachineSettings xdeVirtualMachineSettings = (from s in this.SnapshotSettings
			where s.UniqueId == snapshotName
			select s).FirstOrDefault<IXdeVirtualMachineSettings>();
			if (xdeVirtualMachineSettings == null)
			{
				xdeVirtualMachineSettings = (from s in this.SnapshotSettings
				where s.Name == snapshotName
				select s).FirstOrDefault<IXdeVirtualMachineSettings>();
			}
			return xdeVirtualMachineSettings;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005C50 File Offset: 0x00003E50
		public void RevertToStoppedState()
		{
			if (this.EnabledState != VirtualMachineEnabledState.Disabled)
			{
				this.Stop();
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005C61 File Offset: 0x00003E61
		public void TrackCreatedFile(string diffDisk)
		{
			this.TrackCreatedDisk("DiffDisk", diffDisk);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000583D File Offset: 0x00003A3D
		public void WriteSettingsToVhd(WindowsImageVhd windowsImage)
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005C70 File Offset: 0x00003E70
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				XdeVirtualMachine.SafeEventWatcherDispose(this.computerSystemWatcher);
				this.computerSystemWatcher = null;
				XdeVirtualMachine.SafeEventWatcherDispose(this.snapshotAddedWatcher);
				this.snapshotAddedWatcher = null;
				XdeVirtualMachine.SafeEventWatcherDispose(this.snapshotRemovedWatcher);
				this.snapshotRemovedWatcher = null;
				if (this.keyboard != null)
				{
					this.keyboard.Dispose();
					this.keyboard = null;
				}
				if (this.mouse != null)
				{
					this.mouse.Dispose();
					this.mouse = null;
				}
				if (this.currentSettings != null)
				{
					this.currentSettings.Dispose();
					this.currentSettings = null;
				}
				if (this.vmComputerSystem != null)
				{
					this.vmComputerSystem.Dispose();
					this.vmComputerSystem = null;
				}
				if (this.xdeHyperVMgmtSvc != null)
				{
					this.xdeHyperVMgmtSvc.Dispose();
					this.xdeHyperVMgmtSvc = null;
				}
				this.isDisposed = true;
				this.xdeHyperVMgmtSvc = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005D54 File Offset: 0x00003F54
		private static void SafeEventWatcherDispose(ManagementEventWatcher watcher)
		{
			if (watcher != null)
			{
				try
				{
					watcher.Stop();
					watcher.Dispose();
				}
				catch (COMException)
				{
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005D88 File Offset: 0x00003F88
		private void EnsureCachedSnapshotSettings()
		{
			if (this.cachedSnapshotSettings != null)
			{
				return;
			}
			List<IXdeVirtualMachineSettings> list = new List<IXdeVirtualMachineSettings>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (ManagementObject managementObject in from ManagementObject o in this.vmComputerSystem.GetRelated("Msvm_VirtualSystemSettingData")
			where (string)o["VirtualSystemType"] == "Microsoft:Hyper-V:Snapshot:Realized"
			select o)
			{
				if (!hashSet.Contains(managementObject.Path.Path))
				{
					list.Add(new XdeVirtualMachineSettings(this.XdeHyperVMgmtSvc, this, managementObject, this.settingsOptions));
					hashSet.Add(managementObject.Path.Path);
				}
			}
			this.cachedSnapshotSettings = list.AsReadOnly();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005E68 File Offset: 0x00004068
		private void FlushSnapshotCache()
		{
			this.cachedSnapshotSettings = null;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005E74 File Offset: 0x00004074
		private void TrackCreatedDisk(string diskPrefix, string diskPath)
		{
			string text = XdeVirtualMachine.NotesHeader;
			bool flag = false;
			if (!string.IsNullOrEmpty(this.CurrentSettings.Notes))
			{
				string[] array = this.CurrentSettings.Notes.Split(new char[]
				{
					'\r',
					'\n'
				});
				for (int i = 1; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						if (array[i].Split(new char[]
						{
							'|'
						})[0] == diskPrefix)
						{
							text = string.Concat(new string[]
							{
								text,
								"\r\n",
								diskPrefix,
								"|",
								diskPath
							});
							flag = true;
						}
						else
						{
							text = text + "\r\n" + array[i];
						}
					}
				}
			}
			if (!flag)
			{
				text = string.Concat(new string[]
				{
					text,
					"\r\n",
					diskPrefix,
					"|",
					diskPath
				});
			}
			this.CurrentSettings.Notes = text;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005F68 File Offset: 0x00004168
		private string GetCreatedDiskRecord(string diskPrefix)
		{
			if (!string.IsNullOrEmpty(this.CurrentSettings.Notes))
			{
				string[] array = this.CurrentSettings.Notes.Split(new char[]
				{
					'\r',
					'\n'
				});
				for (int i = 1; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						'|'
					});
					if (array2.Length > 1 && array2[0] == diskPrefix)
					{
						return array2[1];
					}
				}
			}
			return null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005FE0 File Offset: 0x000041E0
		private string GetCreatedDiffDisk()
		{
			return this.GetCreatedDiskRecord("DiffDisk");
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005FF0 File Offset: 0x000041F0
		private void RequestStateChange(VirtualMachineEnabledState state)
		{
			ManagementBaseObject methodParameters = this.vmComputerSystem.GetMethodParameters("RequestStateChange");
			string failedTaskDescriptionFormat = (state == VirtualMachineEnabledState.Enabled) ? XdeVmExceptions.StartError : XdeVmExceptions.StopError;
			methodParameters["RequestedState"] = state;
			WmiUtils.InvokeMethodWithVerify(this.vmComputerSystem, methodParameters, "RequestStateChange", failedTaskDescriptionFormat);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006043 File Offset: 0x00004243
		private void LoadCurrentSettings()
		{
			this.currentSettings = new XdeVirtualMachineSettings(this.XdeHyperVMgmtSvc, this, this.GetCurrentSettingData(), this.settingsOptions);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006063 File Offset: 0x00004263
		private void Initialize()
		{
			this.LoadCurrentSettings();
			this.StartWmiNotifications();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006071 File Offset: 0x00004271
		private ManagementObject GetCurrentSettingData()
		{
			return (from ManagementObject o in this.vmComputerSystem.GetRelated("Msvm_VirtualSystemSettingData")
			where (string)o["VirtualSystemType"] == "Microsoft:Hyper-V:System:Realized"
			select o).FirstOrDefault<ManagementObject>();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000060B4 File Offset: 0x000042B4
		private void ComputerSystemChangedWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			if (this.EnableStateChanged != null)
			{
				ManagementBaseObject managementBaseObject = e.NewEvent["TargetInstance"] as ManagementBaseObject;
				if (managementBaseObject != null)
				{
					this.EnableStateChanged(this, new EnabledStateChangedEventArgs((VirtualMachineEnabledState)((ushort)managementBaseObject["EnabledState"])));
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006103 File Offset: 0x00004303
		private void SnapshotsChangedWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			this.FlushSnapshotCache();
			if (this.SnapshotsChanged != null)
			{
				this.SnapshotsChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006124 File Offset: 0x00004324
		private void StartWmiNotifications()
		{
			WqlEventQuery query = new WqlEventQuery(StringUtilities.InvariantCultureFormat("Select * from __instancemodificationevent where targetinstance isa 'msvm_computersystem' and targetinstance.__relpath = 'Msvm_ComputerSystem.CreationClassName=\"Msvm_ComputerSystem\",Name=\"{0}\"'", new object[]
			{
				this.Guid
			}));
			this.computerSystemWatcher = new ManagementEventWatcher(query);
			this.computerSystemWatcher.Scope = this.vmComputerSystem.Scope;
			this.computerSystemWatcher.EventArrived += this.ComputerSystemChangedWatcher_EventArrived;
			this.computerSystemWatcher.Start();
			query = new WqlEventQuery(StringUtilities.InvariantCultureFormat("Select * from __instancemodificationevent where targetinstance isa 'Msvm_VirtualSystemSettingData' and targetinstance.VirtualSystemIdentifier = '{0}'", new object[]
			{
				this.Guid
			}));
			this.snapshotAddedWatcher = new ManagementEventWatcher(query);
			this.snapshotAddedWatcher.Scope = this.vmComputerSystem.Scope;
			this.snapshotAddedWatcher.EventArrived += this.SnapshotsChangedWatcher_EventArrived;
			this.snapshotAddedWatcher.Start();
			query = new WqlEventQuery(StringUtilities.InvariantCultureFormat("Select * from __InstanceDeletionEvent where targetinstance isa 'Msvm_VirtualSystemSettingData' and targetinstance.VirtualSystemIdentifier = '{0}'", new object[]
			{
				this.Guid
			}));
			this.snapshotRemovedWatcher = new ManagementEventWatcher(query);
			this.snapshotRemovedWatcher.Scope = this.vmComputerSystem.Scope;
			this.snapshotRemovedWatcher.EventArrived += this.SnapshotsChangedWatcher_EventArrived;
			this.snapshotRemovedWatcher.Start();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000625C File Offset: 0x0000445C
		private void SendKeyHelper(Keys key, string methodName)
		{
			if (this.EnabledState != VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(XdeVmExceptions.SystemNotRunning);
			}
			if (this.keyboard == null)
			{
				ManagementObjectCollection related = this.vmComputerSystem.GetRelated("Msvm_Keyboard");
				this.keyboard = related.Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			if (this.keyboard == null)
			{
				throw new XdeVirtualMachineException(XdeVmExceptions.KeyboardNotFound);
			}
			this.keyboard.InvokeMethod(methodName, new object[]
			{
				(uint)key
			});
		}

		// Token: 0x04000020 RID: 32
		public const string DiffDiskPrefix = "DiffDisk";

		// Token: 0x04000021 RID: 33
		public static readonly string NotesHeader = "Emulator " + Globals.XdeVersion.ToString();

		// Token: 0x04000022 RID: 34
		private const string SDCardVhdFileName = "sdcard.vhd";

		// Token: 0x04000023 RID: 35
		private bool isDisposed;

		// Token: 0x04000024 RID: 36
		private string vmName;

		// Token: 0x04000025 RID: 37
		private ManagementObject vmComputerSystem;

		// Token: 0x04000026 RID: 38
		private ManagementObject keyboard;

		// Token: 0x04000027 RID: 39
		private ManagementObject mouse;

		// Token: 0x04000028 RID: 40
		private ManagementEventWatcher computerSystemWatcher;

		// Token: 0x04000029 RID: 41
		private ManagementEventWatcher snapshotAddedWatcher;

		// Token: 0x0400002A RID: 42
		private ManagementEventWatcher snapshotRemovedWatcher;

		// Token: 0x0400002B RID: 43
		private IXdeHyperVManagementService xdeHyperVMgmtSvc;

		// Token: 0x0400002C RID: 44
		private XdeVirtualMachineSettings currentSettings;

		// Token: 0x0400002D RID: 45
		private SettingsOptions settingsOptions;

		// Token: 0x0400002E RID: 46
		private ReadOnlyCollection<IXdeVirtualMachineSettings> cachedSnapshotSettings;
	}
}
