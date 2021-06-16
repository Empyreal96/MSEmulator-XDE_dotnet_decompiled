using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000012 RID: 18
	public sealed class XdeVirtualMachineSettings : IXdeVirtualMachineSettings, IDisposable, INotifyPropertyChanged
	{
		// Token: 0x060000EB RID: 235 RVA: 0x0000637C File Offset: 0x0000457C
		public XdeVirtualMachineSettings(IXdeHyperVManagementService xdeHyperVMgmtSvc, XdeVirtualMachine virtualMachine, ManagementObject settings, SettingsOptions options)
		{
			this.xdeHyperVMgmtSvc = xdeHyperVMgmtSvc;
			this.virtualMachine = virtualMachine;
			this.settings = settings;
			this.options = options;
			if (this.SettingsType != SettingsType.Current)
			{
				this.options |= (SettingsOptions.ReadOnly | SettingsOptions.DontCreateSwitches);
			}
			if (!DefaultSettings.NATDisabled || DefaultSettings.UseDefaultSwitch)
			{
				this.maxExternalsAllowed = 0;
			}
			if ((this.options & SettingsOptions.LoadSettingsAsync) == SettingsOptions.None)
			{
				this.LoadSettings();
				return;
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadSettingsAsync));
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000EC RID: 236 RVA: 0x0000643C File Offset: 0x0000463C
		// (remove) Token: 0x060000ED RID: 237 RVA: 0x00006474 File Offset: 0x00004674
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000064A9 File Offset: 0x000046A9
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000064B1 File Offset: 0x000046B1
		public VGPUInformation VGPUInformation
		{
			get
			{
				return this.vgpuInfo;
			}
			set
			{
				this.vgpuInfo = value;
				this.OnPropertyChanged("VGPUInformation");
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000064C5 File Offset: 0x000046C5
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000064CD File Offset: 0x000046CD
		public ManagementObject ManagementObject
		{
			get
			{
				return this.settings;
			}
			private set
			{
				this.settings = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000064D8 File Offset: 0x000046D8
		public int Generation
		{
			get
			{
				string text = this.settings["VirtualSystemSubType"] as string;
				if (!string.IsNullOrEmpty(text) && text.StartsWith("Microsoft:Hyper-V:SubType:", StringComparison.OrdinalIgnoreCase))
				{
					return int.Parse(text.Substring("Microsoft:Hyper-V:SubType:".Length));
				}
				return 1;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00006528 File Offset: 0x00004728
		public SettingsType SettingsType
		{
			get
			{
				string a = (string)this.settings["VirtualSystemType"];
				if (a == "Microsoft:Hyper-V:System:Realized")
				{
					return SettingsType.Current;
				}
				if (!(a == "Microsoft:Hyper-V:Snapshot:Realized"))
				{
					return SettingsType.Unknown;
				}
				return SettingsType.Snapshot;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000656C File Offset: 0x0000476C
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x000065B8 File Offset: 0x000047B8
		public int NumProcessors
		{
			get
			{
				using (ManagementObject processorSettings = this.GetProcessorSettings())
				{
					if (processorSettings != null)
					{
						return (int)((ulong)processorSettings["VirtualQuantity"]);
					}
				}
				return 0;
			}
			set
			{
				using (ManagementObject processorSettings = this.GetProcessorSettings())
				{
					if (processorSettings != null && value != (int)((ulong)processorSettings["VirtualQuantity"]))
					{
						processorSettings["VirtualQuantity"] = (ulong)((long)value);
						this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(processorSettings, XdeVmExceptions.Processor);
					}
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006624 File Offset: 0x00004824
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00006659 File Offset: 0x00004859
		public string Notes
		{
			get
			{
				string[] array = this.settings["Notes"] as string[];
				if (array != null && array.Length == 1)
				{
					return array[0];
				}
				return string.Empty;
			}
			set
			{
				this.settings["Notes"] = new string[]
				{
					value
				};
				this.xdeHyperVMgmtSvc.ModifyVirtualSystem(this.settings);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006688 File Offset: 0x00004888
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000066E4 File Offset: 0x000048E4
		public int RamSize
		{
			get
			{
				int result;
				using (ManagementObject memorySettings = this.GetMemorySettings())
				{
					if (memorySettings == null || memorySettings["Reservation"] == null)
					{
						result = 0;
					}
					else
					{
						result = Convert.ToInt32(memorySettings["Reservation"].ToString());
					}
				}
				return result;
			}
			set
			{
				using (ManagementObject memorySettings = this.GetMemorySettings())
				{
					memorySettings["Reservation"] = value.ToString();
					memorySettings["VirtualQuantity"] = value.ToString();
					memorySettings["DynamicMemoryEnabled"] = false;
					this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(memorySettings, XdeVmExceptions.Memory);
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000675C File Offset: 0x0000495C
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00006765 File Offset: 0x00004965
		public string VhdPath
		{
			get
			{
				return this.GetVhdPath(0);
			}
			set
			{
				this.SetVhdPath(value, 0);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000676F File Offset: 0x0000496F
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00006778 File Offset: 0x00004978
		public string Com1NamedPipe
		{
			get
			{
				return this.GetSerialPortConnection(XdeWmiTypes.SerialPort.COM1);
			}
			set
			{
				this.SetSerialPortConnection(XdeWmiTypes.SerialPort.COM1, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00006782 File Offset: 0x00004982
		// (set) Token: 0x060000FF RID: 255 RVA: 0x0000678B File Offset: 0x0000498B
		public string Com2NamedPipe
		{
			get
			{
				return this.GetSerialPortConnection(XdeWmiTypes.SerialPort.COM2);
			}
			set
			{
				this.SetSerialPortConnection(XdeWmiTypes.SerialPort.COM2, value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006795 File Offset: 0x00004995
		public ReadOnlyCollection<IXdeVirtualMachineNicInformation> Nics
		{
			get
			{
				return this.nicInfos.AsReadOnly();
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000067A2 File Offset: 0x000049A2
		public bool IsInvalid
		{
			get
			{
				return this.InvalidSettingsReason > InvalidSettingsReason.NotInvalid;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000067AD File Offset: 0x000049AD
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000067B5 File Offset: 0x000049B5
		public InvalidSettingsReason InvalidSettingsReason { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000067BE File Offset: 0x000049BE
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000067C6 File Offset: 0x000049C6
		public Exception AsyncLoadException { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000067CF File Offset: 0x000049CF
		public DateTime CreationTime
		{
			get
			{
				return ManagementDateTimeConverter.ToDateTime((string)this.ManagementObject["CreationTime"]);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000067EB File Offset: 0x000049EB
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00006804 File Offset: 0x00004A04
		public string Name
		{
			get
			{
				return this.settings["ElementName"] as string;
			}
			set
			{
				using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
				{
					service.RenameSnapshot(this.settings, value);
				}
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000684C File Offset: 0x00004A4C
		public string UniqueId
		{
			get
			{
				return this.settings["InstanceID"] as string;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00006863 File Offset: 0x00004A63
		public bool IsUsingGpu
		{
			get
			{
				return this.GetWmiRemoteFxDisplayControllerSettings() != null;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000686E File Offset: 0x00004A6E
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00006876 File Offset: 0x00004A76
		public bool IsSnapshot { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006880 File Offset: 0x00004A80
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000068CC File Offset: 0x00004ACC
		public bool ExposeVirtualizationExtensions
		{
			get
			{
				using (ManagementObject processorSettings = this.GetProcessorSettings())
				{
					if (processorSettings != null)
					{
						return (bool)processorSettings["ExposeVirtualizationExtensions"];
					}
				}
				return false;
			}
			set
			{
				using (ManagementObject processorSettings = this.GetProcessorSettings())
				{
					if (processorSettings != null && value != (bool)processorSettings["ExposeVirtualizationExtensions"])
					{
						processorSettings["ExposeVirtualizationExtensions"] = value;
						this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(processorSettings, XdeVmExceptions.Processor);
					}
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006934 File Offset: 0x00004B34
		private bool ReadOnly
		{
			get
			{
				return (this.options & SettingsOptions.ReadOnly) == SettingsOptions.ReadOnly;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00006941 File Offset: 0x00004B41
		private bool CreateSwitches
		{
			get
			{
				return (this.options & SettingsOptions.DontCreateSwitches) == SettingsOptions.None;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000694E File Offset: 0x00004B4E
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006958 File Offset: 0x00004B58
		public ulong GetFilesSize()
		{
			ulong settingsSizeOfSystemFiles;
			using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
			{
				settingsSizeOfSystemFiles = service.GetSettingsSizeOfSystemFiles(this.ManagementObject);
			}
			return settingsSizeOfSystemFiles;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006998 File Offset: 0x00004B98
		public Bitmap GetThumbnail(int width)
		{
			Size currentResolution = this.virtualMachine.GetCurrentResolution();
			int height = currentResolution.Height * width / currentResolution.Width;
			Bitmap result;
			using (IXdeHyperVManagementService service = XdeHyperVManagementService.GetService())
			{
				result = (Bitmap)service.GetVirtualSystemThumbnailImage(this.ManagementObject, width, height);
			}
			return result;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000069FC File Offset: 0x00004BFC
		public void CleanupForDeletion()
		{
			foreach (IXdeVirtualMachineNicInformation xdeVirtualMachineNicInformation in this.nicInfos)
			{
				((XdeVirtualMachineSettings.XdeVirtualMachineNicInformation)xdeVirtualMachineNicInformation).CleanupForDeletion();
			}
			if (!string.IsNullOrEmpty(this.VhdPath))
			{
				string fileNameForVhd = WmiVhdBootSettings.GetFileNameForVhd(this.VhdPath);
				if (File.Exists(fileNameForVhd))
				{
					File.Delete(fileNameForVhd);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006A78 File Offset: 0x00004C78
		public void Delete()
		{
			if (this.SettingsType != SettingsType.Snapshot)
			{
				throw new InvalidOperationException();
			}
			foreach (IXdeVirtualMachineNicInformation xdeVirtualMachineNicInformation in this.nicInfos)
			{
				((XdeVirtualMachineSettings.XdeVirtualMachineNicInformation)xdeVirtualMachineNicInformation).CleanupForDeletion();
			}
			using (VirtualSystemManagementService service = VirtualSystemManagementService.GetService())
			{
				service.DeleteSnapshot(this.ManagementObject);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006B08 File Offset: 0x00004D08
		public void WaitForLoadedSettings()
		{
			this.settingsLoaded.WaitOne();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006B18 File Offset: 0x00004D18
		public void EnsureHasDisplayAdapter(VGPUStatus vgpuStatus, Size requestedResolution, int vgpuRamMB, GpuAssignmentMode gpuAssignmentMode)
		{
			this.requestedResolution = requestedResolution;
			this.VGPUInformation = new VGPUInformation(vgpuStatus);
			if (this.VGPUInformation.Status == VGPUStatus.Enabled || this.VGPUInformation.Status == VGPUStatus.Running)
			{
				if (this.EnsureHasGpuAdapter(requestedResolution, vgpuRamMB))
				{
					return;
				}
				this.VGPUInformation = new VGPUInformation
				{
					Status = VGPUStatus.NoCompatibleHostHardwareFound
				};
			}
			this.EnsureHasNonGpuDisplayAdapter();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006B7C File Offset: 0x00004D7C
		public void EnsureHasNonGpuDisplayAdapter()
		{
			this.EnsureHasDisplayAdapter();
			this.UpdateDisplayControllerSettings(this.requestedResolution);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006B90 File Offset: 0x00004D90
		private bool EnsureHasGpuAdapter(Size requestedResolution, int vgpuRamMB)
		{
			ManagementObject wmiRemoteFxDisplayControllerSettings = this.GetWmiRemoteFxDisplayControllerSettings();
			byte remoteFxMaxScreenResolutionValue = this.GetRemoteFxMaxScreenResolutionValue(requestedResolution);
			if (vgpuRamMB <= 0)
			{
				vgpuRamMB = 256;
			}
			ulong num = (ulong)((long)(vgpuRamMB * 1048576));
			if (wmiRemoteFxDisplayControllerSettings == null)
			{
				string query = string.Format("SELECT * FROM Msvm_AllocationCapabilities WHERE ResourceSubType = '{0}'", XdeWmiTypes.ResourceSubType.RemoteFx.Name());
				ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(this.settings.Scope, query, 0, 100);
				if (instanceHelper == null)
				{
					Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
					{
						reason = "Msvm_AllocationCapabilities"
					});
					return false;
				}
				ManagementObject managementObject = (from ManagementObject s in instanceHelper.GetRelated("CIM_ResourceAllocationSettingData")
				where ((string)s["InstanceID"]).Contains("Default")
				select s).FirstOrDefault<ManagementObject>();
				if (managementObject == null)
				{
					Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
					{
						reason = "CIM_ResourceAllocationSettingData"
					});
					return false;
				}
				managementObject["Weight"] = 0;
				managementObject["MaximumScreenResolution"] = remoteFxMaxScreenResolutionValue;
				managementObject["VRAMSizeBytes"] = num;
				if (new ManagementObject(this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, managementObject, XdeVmExceptions.RemoteFXAdapter)[0]) == null)
				{
					Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
					{
						reason = "AddVirtualSystemResources"
					});
					return false;
				}
				this.RemoveDisplayAdapter();
				ManagementObject managementObject2 = (from ManagementObject s in this.settings.GetRelated("Msvm_ResourceAllocationSettingData")
				where (string)s["ResourceSubType"] == XdeWmiTypes.ResourceSubType.S3Controller.Name()
				select s).FirstOrDefault<ManagementObject>();
				if (managementObject2 == null)
				{
					Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
					{
						reason = "Msvm_ResourceAllocationSettingData"
					});
					return false;
				}
				string text = (string)managementObject2["Address"];
				if (text == null)
				{
					Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
					{
						reason = "NoS3Address"
					});
					return false;
				}
				string text2 = text.ToUpper(CultureInfo.InvariantCulture);
				if (!text2.Equals("02C1,00000000,00") && !text2.Equals("02C1,00000000,01"))
				{
					bool flag = WmiUtils.IsDX11Disabled();
					ManagementObject managementObject3 = WmiUtils.Get3dVideoPool();
					if (managementObject3 == null)
					{
						Logger.Instance.LogError("InitRemoteFxAdapterFailed", new
						{
							reason = "No3DVideoPool"
						});
						return false;
					}
					if (Convert.ToSingle((string)managementObject3["DirectXVersion"], CultureInfo.InvariantCulture) >= 11f && !flag)
					{
						managementObject2["Address"] = "02C1,00000000,01";
					}
					else
					{
						managementObject2["Address"] = "02C1,00000000,00";
					}
					this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(managementObject2, XdeVmExceptions.S3DisplayAdapter);
				}
			}
			else if ((byte)wmiRemoteFxDisplayControllerSettings["MaximumScreenResolution"] != remoteFxMaxScreenResolutionValue || (ulong)wmiRemoteFxDisplayControllerSettings["VRAMSizeBytes"] != num)
			{
				wmiRemoteFxDisplayControllerSettings["MaximumScreenResolution"] = remoteFxMaxScreenResolutionValue;
				wmiRemoteFxDisplayControllerSettings["VRAMSizeBytes"] = num;
				this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(wmiRemoteFxDisplayControllerSettings, XdeVmExceptions.RemoteFXAdapter);
			}
			return true;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006E8C File Offset: 0x0000508C
		private void EnsureHasDisplayAdapter()
		{
			ManagementObject managementObject = this.GetWmiDisplayControllerSettings();
			if (managementObject == null)
			{
				managementObject = (from ManagementObject s in this.settings.GetRelated("CIM_ResourceAllocationSettingData")
				where (string)s["ResourceSubType"] == XdeWmiTypes.ResourceSubType.VideoSynthetic.Name()
				select s).FirstOrDefault<ManagementObject>();
			}
			if (managementObject != null)
			{
				return;
			}
			if (this.IsUsingGpu)
			{
				this.RemoveRemoteFxDisplayAdapter();
			}
			string query = string.Format("SELECT * FROM Msvm_AllocationCapabilities WHERE ResourceSubType = '{0}'", XdeWmiTypes.ResourceSubType.VideoSynthetic.Name());
			ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(this.settings.Scope, query, 0, 100);
			if (instanceHelper != null)
			{
				ManagementObject resObj = (from ManagementObject s in instanceHelper.GetRelated("CIM_ResourceAllocationSettingData")
				where ((string)s["InstanceID"]).Contains("Default")
				select s).FirstOrDefault<ManagementObject>();
				this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, resObj, XdeVmExceptions.DisplayAdapter);
				if (this.Generation == 1)
				{
					ManagementObject managementObject2 = (from ManagementObject s in this.settings.GetRelated("Msvm_ResourceAllocationSettingData")
					where (string)s["ResourceSubType"] == XdeWmiTypes.ResourceSubType.S3Controller.Name()
					select s).First<ManagementObject>();
					if (!((string)managementObject2["Address"]).ToUpper(CultureInfo.InvariantCulture).Equals("5353,00000000,00"))
					{
						managementObject2["Address"] = "5353,00000000,00";
						this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(managementObject2, XdeVmExceptions.S3DisplayAdapter);
					}
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000700C File Offset: 0x0000520C
		private IXdeDisplayControllerSettings GetDisplayControllerSettings()
		{
			ManagementObject wmiDisplayControllerSettings = this.GetWmiDisplayControllerSettings();
			if (wmiDisplayControllerSettings != null)
			{
				return new XdeDisplayControllerSettings(wmiDisplayControllerSettings);
			}
			return null;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000702C File Offset: 0x0000522C
		private void UpdateDisplayControllerSettings(Size requestedResolution)
		{
			XdeDisplayControllerSettings xdeDisplayControllerSettings = (XdeDisplayControllerSettings)this.GetDisplayControllerSettings();
			xdeDisplayControllerSettings.ResolutionType = ResolutionType.Single;
			xdeDisplayControllerSettings.Resolution = requestedResolution;
			this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(xdeDisplayControllerSettings.ManagementObject, XdeVmExceptions.DisplayControllerSettings);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007069 File Offset: 0x00005269
		public void Dispose()
		{
			this.WaitForLoadedSettings();
			this.settings.Dispose();
			this.settingsLoaded.Dispose();
			this.StopWmiWatcher();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007094 File Offset: 0x00005294
		private void RemoveDisplayAdapter()
		{
			ManagementObject wmiDisplayControllerSettings = this.GetWmiDisplayControllerSettings();
			if (wmiDisplayControllerSettings != null)
			{
				this.virtualMachine.RevertToStoppedState();
				this.xdeHyperVMgmtSvc.DeleteVirtualSystemResource(wmiDisplayControllerSettings, XdeVmExceptions.DisplayAdapter);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000070C8 File Offset: 0x000052C8
		private void RemoveRemoteFxDisplayAdapter()
		{
			ManagementObject wmiRemoteFxDisplayControllerSettings = this.GetWmiRemoteFxDisplayControllerSettings();
			if (wmiRemoteFxDisplayControllerSettings != null)
			{
				this.virtualMachine.RevertToStoppedState();
				this.xdeHyperVMgmtSvc.DeleteVirtualSystemResource(wmiRemoteFxDisplayControllerSettings, XdeVmExceptions.DisplayAdapter);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000070FB File Offset: 0x000052FB
		private void StopWmiWatcher()
		{
			if (this.wmiChangeWatcher != null)
			{
				this.wmiChangeWatcher.EventArrived -= this.WmiChangeWatcher_EventArrived;
				this.wmiChangeWatcher.Stop();
				this.wmiChangeWatcher.Dispose();
				this.wmiChangeWatcher = null;
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000713C File Offset: 0x0000533C
		private void StartWmiWatcher()
		{
			WqlEventQuery query = new WqlEventQuery(StringUtilities.InvariantCultureFormat("Select * from __instancemodificationevent where targetinstance isa 'Msvm_VirtualSystemSettingData' and targetinstance.__relpath = '{0}'", new object[]
			{
				this.ManagementObject.Path.RelativePath
			}));
			this.wmiChangeWatcher = new ManagementEventWatcher(query);
			this.wmiChangeWatcher.Scope = this.ManagementObject.Scope;
			this.wmiChangeWatcher.EventArrived += this.WmiChangeWatcher_EventArrived;
			this.wmiChangeWatcher.Start();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000071B8 File Offset: 0x000053B8
		private void WmiChangeWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			ManagementBaseObject managementBaseObject = e.NewEvent["TargetInstance"] as ManagementBaseObject;
			ManagementBaseObject managementBaseObject2 = e.NewEvent["PreviousInstance"] as ManagementBaseObject;
			if (managementBaseObject != null && managementBaseObject2 != null)
			{
				this.ManagementObject.Get();
				if ((string)managementBaseObject["ElementName"] != (string)managementBaseObject2["ElementName"])
				{
					this.OnPropertyChanged("Name");
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007234 File Offset: 0x00005434
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
				propertyChanged(this, e);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000725C File Offset: 0x0000545C
		private string GetVhdPath(int controllerAddress)
		{
			using (ManagementObject diskSettings = this.GetDiskSettings(controllerAddress))
			{
				if (diskSettings != null && diskSettings["HostResource"] != null)
				{
					return ((string[])diskSettings["HostResource"])[0];
				}
			}
			return null;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000072B8 File Offset: 0x000054B8
		private void SetVhdPath(string vhdPath, int controllerAddress)
		{
			if (string.IsNullOrEmpty(vhdPath))
			{
				this.RemoveHardDrive(controllerAddress);
				return;
			}
			if (!File.Exists(vhdPath))
			{
				throw new XdeVirtualMachineException(XdeVmExceptions.VHDNotFound);
			}
			using (ManagementObject diskSettings = this.GetDiskSettings(controllerAddress))
			{
				if (diskSettings == null)
				{
					this.AddVHD(vhdPath, controllerAddress);
				}
				else
				{
					diskSettings["HostResource"] = new string[]
					{
						vhdPath
					};
					this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(diskSettings, XdeVmExceptions.Vhd);
				}
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007340 File Offset: 0x00005540
		private bool IsXdeInternalSwitch(ManagementObject switchObj)
		{
			return this.IsXdeInternalNATSwitch(switchObj) || XdeVirtualMachineSettings.IsDefaultWindowsSwitch(switchObj);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007353 File Offset: 0x00005553
		private bool IsXdeInternalNATSwitch(ManagementObject switchObj)
		{
			return "Microsoft Emulator NAT Switch" == switchObj["ElementName"].ToString();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000736F File Offset: 0x0000556F
		private static bool IsDefaultWindowsSwitch(ManagementObject switchObj)
		{
			return StringComparer.OrdinalIgnoreCase.Equals((string)switchObj["Name"], "c08cb7b8-9b3c-408e-8e30-5e16a3aeb444");
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007390 File Offset: 0x00005590
		private void AddDefaultSwitch()
		{
			ManagementObject managementObject = this.xdeHyperVMgmtSvc.FindDefaultSwitch();
			if (managementObject != null)
			{
				this.EnsureNicForSwitch(managementObject, "Default switch adapter", true);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000073BC File Offset: 0x000055BC
		private void EnsureProperNicsExist()
		{
			if (this.SettingsType == SettingsType.Current)
			{
				if (!DefaultSettings.NATDisabled)
				{
					this.InitializeInternalNic("Microsoft Emulator NAT Switch", XdeVirtualMachineDefaultSettings.GetNatNicName(this.virtualMachine.Name));
				}
				if (DefaultSettings.UseDefaultSwitch)
				{
					this.AddDefaultSwitch();
				}
				this.InitializeAllExternalNics();
				if (this.refreshNicsNeeded)
				{
					this.LoadBoundNicsIntoCache();
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007415 File Offset: 0x00005615
		private ManagementObject GetProcessorSettings()
		{
			return this.settings.GetRelated("Msvm_ProcessorSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007434 File Offset: 0x00005634
		private void AddNicInfo(ManagementObject portSettingData, IXdeVirtualSwitchInformation switchInfo)
		{
			XdeVirtualMachineSettings.XdeVirtualMachineNicInformation item = new XdeVirtualMachineSettings.XdeVirtualMachineNicInformation(this, portSettingData, switchInfo);
			if (switchInfo.External)
			{
				this.exernalNicCount++;
			}
			this.nicInfos.Add(item);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000746C File Offset: 0x0000566C
		private void DeletePortSettingData(ManagementObject portSettingData)
		{
			this.virtualMachine.RevertToStoppedState();
			this.xdeHyperVMgmtSvc.DeleteVirtualSystemResource(portSettingData, XdeVmExceptions.VirtualNetworkAdapter);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000748A File Offset: 0x0000568A
		private ManagementObject GetMemorySettings()
		{
			return this.settings.GetRelated("Msvm_MemorySettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000074A6 File Offset: 0x000056A6
		private ManagementObject GetWmiDisplayControllerSettings()
		{
			return this.settings.GetRelated("Msvm_SyntheticDisplayControllerSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000074C2 File Offset: 0x000056C2
		private ManagementObject GetWmiRemoteFxDisplayControllerSettings()
		{
			return this.settings.GetRelated("Msvm_Synthetic3DDisplayControllerSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000074E0 File Offset: 0x000056E0
		private void AddVHD(string vhd, int addressOfController)
		{
			string addressToUse = addressOfController.ToString();
			ManagementObject managementObject = this.settings;
			ManagementObject managementObject2 = (from ManagementObject s in managementObject.GetRelated("Msvm_ResourceAllocationSettingData")
			where (string)s["ResourceSubType"] == XdeWmiTypes.ResourceSubType.IDEController.Name() && (string)s["Address"] == addressToUse
			select s).FirstOrDefault<ManagementObject>();
			string query;
			if (managementObject2 == null)
			{
				managementObject2 = (from ManagementObject s in managementObject.GetRelated("Msvm_ResourceAllocationSettingData")
				where (string)s["ResourceSubType"] == XdeWmiTypes.ResourceSubType.ParallelSCSIHBA.Name()
				select s).FirstOrDefault<ManagementObject>();
				if (managementObject2 == null)
				{
					query = string.Format("select * from Msvm_ResourceAllocationSettingData where ResourceType = {0} and InstanceId like '%default%'", 6);
					ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(this.settings.Scope, query, 0, 100);
					managementObject2 = new ManagementObject(this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, instanceHelper, XdeVmExceptions.SyntheticDiskDrive)[0]);
				}
			}
			query = string.Format("SELECT * FROM Msvm_AllocationCapabilities WHERE ResourceSubType = '{0}'", XdeWmiTypes.ResourceSubType.DiskSynthetic.Name());
			ManagementObject managementObject3 = (from ManagementObject s in WmiBaseUtils.GetInstanceHelper(this.settings.Scope, query, 0, 100).GetRelated("Msvm_ResourceAllocationSettingData")
			where ((string)s["InstanceID"]).Contains("Default")
			select s).FirstOrDefault<ManagementObject>();
			managementObject3["Parent"] = managementObject2.Path.Path;
			managementObject3["AddressOnParent"] = addressToUse;
			string[] array = this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, managementObject3, XdeVmExceptions.SyntheticDiskDrive);
			query = string.Format("SELECT * FROM Msvm_AllocationCapabilities WHERE ResourceSubType = '{0}'", XdeWmiTypes.ResourceSubType.VHD.Name());
			ManagementObject managementObject4 = (from ManagementObject s in WmiBaseUtils.GetInstanceHelper(this.settings.Scope, query, 0, 100).GetRelated("Msvm_StorageAllocationSettingData")
			where ((string)s["InstanceID"]).Contains("Default")
			select s).FirstOrDefault<ManagementObject>();
			managementObject4["Parent"] = array[0];
			managementObject4["HostResource"] = new string[]
			{
				vhd
			};
			bool flag = addressOfController == 0;
			try
			{
				managementObject4["IgnoreFlushes"] = flag;
			}
			catch (ManagementException)
			{
			}
			this.virtualMachine.RevertToStoppedState();
			this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, managementObject4, XdeVmExceptions.Vhd);
			this.EnsureBootOrderIsFirstHardDrive();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007738 File Offset: 0x00005938
		private void EnsureBootOrderIsFirstHardDrive()
		{
			string[] array = this.settings["BootSourceOrder"] as string[];
			if (array != null)
			{
				List<string> list = new List<string>();
				foreach (string text in array)
				{
					ManagementObject managementObject = new ManagementObject(text).GetRelated("Msvm_ResourceAllocationSettingData").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
					if (managementObject != null && managementObject["ResourceType"].ToString() == 17.ToString())
					{
						list.Add(text);
						break;
					}
				}
				if (list.Count != 0)
				{
					this.settings["BootSourceOrder"] = list.ToArray();
					this.xdeHyperVMgmtSvc.ModifyVirtualSystem(this.settings);
				}
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000077FC File Offset: 0x000059FC
		private ManagementObject GetDiskSettings(int addressOnParent)
		{
			foreach (ManagementObject managementObject in from ManagementObject o in this.settings.GetRelated("Msvm_StorageAllocationSettingData")
			where (ushort)o["ResourceType"] == 31
			select o)
			{
				ManagementObject managementObject2 = new ManagementObject((string)managementObject["Parent"]);
				managementObject2.Get();
				if (managementObject2["AddressOnParent"].ToString() == addressOnParent.ToString())
				{
					return managementObject;
				}
			}
			return null;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000078B4 File Offset: 0x00005AB4
		private string GetSerialPortConnection(XdeWmiTypes.SerialPort port)
		{
			using (ManagementObject serialPortSettings = this.GetSerialPortSettings(port))
			{
				if (serialPortSettings != null && serialPortSettings["Connection"] != null)
				{
					return ((string[])serialPortSettings["Connection"])[0];
				}
			}
			return null;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007910 File Offset: 0x00005B10
		private ManagementObject GetSerialPortSettings(XdeWmiTypes.SerialPort port)
		{
			return (from ManagementObject s in this.settings.GetRelated("CIM_ResourceAllocationSettingData")
			where (ushort)s["ResourceType"] == 21 && (string)s["ElementName"] == port.Name()
			select s).FirstOrDefault<ManagementObject>();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007958 File Offset: 0x00005B58
		private void SetSerialPortConnection(XdeWmiTypes.SerialPort port, string value)
		{
			using (ManagementObject serialPortSettings = this.GetSerialPortSettings(port))
			{
				if (serialPortSettings == null)
				{
					throw new XdeVirtualMachineException(StringUtilities.InvariantCultureFormat(XdeVmExceptions.SerialControllerNotFound, new object[]
					{
						port.Name()
					}));
				}
				string[] value2 = new string[]
				{
					value
				};
				serialPortSettings["Connection"] = value2;
				this.xdeHyperVMgmtSvc.ModifyVirtualSystemResources(serialPortSettings, XdeVmExceptions.SerialPort);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000079D4 File Offset: 0x00005BD4
		private bool IsVmConnectedToSwitch(ManagementObject virtSwitch)
		{
			return this.switchIdsToPortSettingsData.ContainsKey(virtSwitch["Name"] as string);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000079F4 File Offset: 0x00005BF4
		private void RemoveInvalidAdapters()
		{
			foreach (ManagementBaseObject managementBaseObject in this.settings.GetRelated("Msvm_EmulatedEthernetPortSettingData"))
			{
				ManagementObject portSettingData = (ManagementObject)managementBaseObject;
				if (this.ReadOnly)
				{
					this.InvalidSettingsReason = InvalidSettingsReason.InvalidPortSettings;
					break;
				}
				this.DeletePortSettingData(portSettingData);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007A64 File Offset: 0x00005C64
		private void LoadBoundNicsIntoCache()
		{
			this.refreshNicsNeeded = false;
			this.nicInfos.Clear();
			this.exernalNicCount = 0;
			int num = 0;
			bool flag = !this.ReadOnly;
			foreach (ManagementBaseObject managementBaseObject in this.settings.GetRelated("Msvm_SyntheticEthernetPortSettingData"))
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				ManagementObject managementObject2;
				WmiUtils.GetSwitchForPortSettingData(managementObject, out managementObject2);
				bool flag2 = false;
				IXdeVirtualSwitchInformation xdeVirtualSwitchInformation = null;
				InvalidSettingsReason invalidSettingsReason = InvalidSettingsReason.NotInvalid;
				if (managementObject2 == null)
				{
					flag2 = true;
					invalidSettingsReason = InvalidSettingsReason.NicAttachedToDeletedSwitch;
				}
				else
				{
					xdeVirtualSwitchInformation = this.xdeHyperVMgmtSvc.GetSwitchInformation(managementObject2);
					if (!xdeVirtualSwitchInformation.External)
					{
						if (!this.IsXdeInternalSwitch(((IXdeWmiObject)xdeVirtualSwitchInformation).WmiObject))
						{
							flag2 = true;
							invalidSettingsReason = InvalidSettingsReason.ExtraSwitchFound;
						}
						else
						{
							if (DefaultSettings.NATDisabled && (string)managementObject2["ElementName"] == "Microsoft Emulator NAT Switch")
							{
								flag2 = true;
								invalidSettingsReason = InvalidSettingsReason.InvalidNATSwitchFound;
							}
							if (XdeVirtualMachineSettings.IsDefaultWindowsSwitch(managementObject2))
							{
								if (!DefaultSettings.UseDefaultSwitch)
								{
									flag2 = true;
									invalidSettingsReason = InvalidSettingsReason.InvalidNATSwitchFound;
								}
							}
							else if (!this.xdeHyperVMgmtSvc.IsNicValidForInternalUse(managementObject, (string)managementObject2["ElementName"]))
							{
								flag2 = true;
								invalidSettingsReason = InvalidSettingsReason.InvalidSettingsForInternalNic;
							}
						}
					}
					else
					{
						num++;
						if (num > this.maxExternalsAllowed)
						{
							flag2 = true;
							invalidSettingsReason = InvalidSettingsReason.TooManySwitchesFound;
						}
					}
				}
				if (flag2)
				{
					if (flag)
					{
						this.DeletePortSettingData(managementObject);
					}
					else
					{
						this.InvalidSettingsReason = invalidSettingsReason;
					}
				}
				else
				{
					string key = managementObject2["Name"] as string;
					XdeVirtualMachineSettings.SwitchSettingsInfo value = new XdeVirtualMachineSettings.SwitchSettingsInfo(managementObject, xdeVirtualSwitchInformation);
					this.switchIdsToPortSettingsData[key] = value;
					this.AddNicInfo(managementObject, xdeVirtualSwitchInformation);
				}
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007C18 File Offset: 0x00005E18
		private void EnsureNicForSwitch(ManagementObject virtualSwitch, string adapterName, bool staticAddress)
		{
			if (virtualSwitch == null)
			{
				this.InvalidSettingsReason = InvalidSettingsReason.NicAttachedToDeletedSwitch;
				return;
			}
			if (this.IsVmConnectedToSwitch(virtualSwitch))
			{
				return;
			}
			if (this.ReadOnly)
			{
				this.InvalidSettingsReason = InvalidSettingsReason.NicNotFoundForSwitch;
				return;
			}
			ManagementObject resourceAllocationsettingDataDefault = WmiUtils.GetResourceAllocationsettingDataDefault(this.settings.Scope, XdeWmiTypes.ResourceType.EthernetAdapter, XdeWmiTypes.ResourceSubType.EthernetSynthetic, null);
			ManagementObject resourceAllocationsettingDataDefault2 = WmiUtils.GetResourceAllocationsettingDataDefault(this.settings.Scope, XdeWmiTypes.ResourceType.EthernetPort, XdeWmiTypes.ResourceSubType.EthernetConnection, null);
			resourceAllocationsettingDataDefault["StaticMacAddress"] = staticAddress;
			if (staticAddress)
			{
				resourceAllocationsettingDataDefault["Address"] = XdeMacAddressSettings.GetMacAddressForVmName(this.virtualMachine.Name, true).Replace("-", string.Empty);
			}
			resourceAllocationsettingDataDefault["ElementName"] = adapterName;
			resourceAllocationsettingDataDefault["VirtualSystemIdentifiers"] = new string[]
			{
				string.Format("{{{0}}}", Guid.NewGuid())
			};
			string[] array = this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, resourceAllocationsettingDataDefault, XdeVmExceptions.VirtualNetworkAdapter);
			resourceAllocationsettingDataDefault2["HostResource"] = new string[]
			{
				virtualSwitch.Path.Path
			};
			resourceAllocationsettingDataDefault2["Parent"] = array[0];
			this.virtualMachine.RevertToStoppedState();
			this.xdeHyperVMgmtSvc.AddVirtualSystemResources(this.settings, resourceAllocationsettingDataDefault2, XdeVmExceptions.VirtualNetworkAdapter);
			IXdeVirtualSwitchInformation switchInformation = this.xdeHyperVMgmtSvc.GetSwitchInformation(virtualSwitch);
			this.AddNicInfo(resourceAllocationsettingDataDefault, switchInformation);
			this.refreshNicsNeeded = true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007D70 File Offset: 0x00005F70
		private void InitializeInternalNic(string switchFriendlyName, string adapterName)
		{
			ManagementObject virtualSwitch = this.xdeHyperVMgmtSvc.FindInternalSwitch(switchFriendlyName);
			this.EnsureNicForSwitch(virtualSwitch, adapterName, true);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007D94 File Offset: 0x00005F94
		private void InitializeAllExternalNics()
		{
			ReadOnlyCollection<IXdeVirtualSwitchInformation> externalSwitchInfos = this.xdeHyperVMgmtSvc.GetExternalSwitchInfos();
			int num = 0;
			while (this.exernalNicCount < this.maxExternalsAllowed && num < externalSwitchInfos.Count)
			{
				this.InitializeExternalNic(((IXdeWmiObject)externalSwitchInfos[num]).WmiObject);
				num++;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007DE4 File Offset: 0x00005FE4
		private void InitializeExternalNic(ManagementObject virtSwitch)
		{
			string externalNicName = XdeVirtualMachineDefaultSettings.GetExternalNicName(this.virtualMachine.Name);
			this.EnsureNicForSwitch(virtSwitch, externalNicName, false);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007E0C File Offset: 0x0000600C
		private void RemoveHardDrive(int addressOfController)
		{
			using (ManagementObject diskSettings = this.GetDiskSettings(addressOfController))
			{
				if (diskSettings != null)
				{
					this.virtualMachine.RevertToStoppedState();
					this.xdeHyperVMgmtSvc.DeleteVirtualSystemResource(diskSettings, XdeVmExceptions.Vhd);
				}
			}
			using (ManagementObject managementObject = this.FindResourceSettingsByTypeAndAddress(XdeWmiTypes.ResourceType.StorageExtent, addressOfController))
			{
				if (managementObject != null)
				{
					this.virtualMachine.RevertToStoppedState();
					this.xdeHyperVMgmtSvc.DeleteVirtualSystemResource(managementObject, XdeVmExceptions.Vhd);
				}
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007EA0 File Offset: 0x000060A0
		private ManagementObject FindResourceSettingsByTypeAndAddress(XdeWmiTypes.ResourceType resourceType, int addressOfParent)
		{
			foreach (ManagementObject managementObject in from ManagementObject o in this.settings.GetRelated("Msvm_ResourceAllocationSettingData")
			where (ushort)o["ResourceType"] == (ushort)resourceType
			select o)
			{
				if (addressOfParent.ToString() == (string)managementObject["AddressOnParent"])
				{
					return managementObject;
				}
			}
			return null;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007F38 File Offset: 0x00006138
		private void LoadSettingsAsync(object unused)
		{
			try
			{
				this.LoadSettings();
			}
			catch (Exception asyncLoadException)
			{
				this.AsyncLoadException = asyncLoadException;
				this.InvalidSettingsReason = InvalidSettingsReason.FailedToLoadSettings;
				this.settingsLoaded.Set();
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007F7C File Offset: 0x0000617C
		private void LoadSettings()
		{
			string name = this.Name;
			this.StartWmiWatcher();
			this.ManagementObject.Get();
			if (name != this.Name)
			{
				this.OnPropertyChanged("Name");
			}
			if (this.SettingsType == SettingsType.Current)
			{
				this.RemoveInvalidAdapters();
				this.LoadBoundNicsIntoCache();
				if (this.CreateSwitches)
				{
					this.EnsureProperNicsExist();
				}
			}
			else
			{
				this.LoadBoundNicsIntoCache();
			}
			this.settingsLoaded.Set();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007FF0 File Offset: 0x000061F0
		private byte GetRemoteFxMaxScreenResolutionValue(Size requestedResolution)
		{
			int width = requestedResolution.Width;
			int height = requestedResolution.Height;
			byte result;
			if (width <= 1024 && height <= 768)
			{
				result = 0;
			}
			else if (width <= 1280 && height <= 1024)
			{
				result = 1;
			}
			else if (width <= 1600 && height <= 1200)
			{
				result = 2;
			}
			else if (width <= 1920 && height <= 1200)
			{
				result = 3;
			}
			else if (width <= 2560 && height <= 1600)
			{
				result = 4;
			}
			else
			{
				result = 5;
			}
			return result;
		}

		// Token: 0x04000037 RID: 55
		public const string SystemSettingsTypeName = "Microsoft:Hyper-V:System:Realized";

		// Token: 0x04000038 RID: 56
		public const string SnapshotSettingsTypeName = "Microsoft:Hyper-V:Snapshot:Realized";

		// Token: 0x04000039 RID: 57
		internal const string ValidPortSettingName = "Msvm_SyntheticEthernetPortSettingData";

		// Token: 0x0400003A RID: 58
		internal const string InvalidPortSettingName = "Msvm_EmulatedEthernetPortSettingData";

		// Token: 0x0400003B RID: 59
		internal const XdeWmiTypes.ResourceSubType NicResourceSubType = XdeWmiTypes.ResourceSubType.EthernetSynthetic;

		// Token: 0x0400003C RID: 60
		private const int MaxSyntheticNics = 4;

		// Token: 0x0400003D RID: 61
		private const int NumInternalNicsRequired = 1;

		// Token: 0x0400003E RID: 62
		private const int PrimaryDiskControllerAddress = 0;

		// Token: 0x0400003F RID: 63
		private const int SDCardDiskControllerAddress = 1;

		// Token: 0x04000040 RID: 64
		private int maxExternalsAllowed = 3;

		// Token: 0x04000041 RID: 65
		private XdeVirtualMachine virtualMachine;

		// Token: 0x04000042 RID: 66
		private ManagementObject settings;

		// Token: 0x04000043 RID: 67
		private IXdeHyperVManagementService xdeHyperVMgmtSvc;

		// Token: 0x04000044 RID: 68
		private List<IXdeVirtualMachineNicInformation> nicInfos = new List<IXdeVirtualMachineNicInformation>();

		// Token: 0x04000045 RID: 69
		private Dictionary<string, XdeVirtualMachineSettings.SwitchSettingsInfo> switchIdsToPortSettingsData = new Dictionary<string, XdeVirtualMachineSettings.SwitchSettingsInfo>();

		// Token: 0x04000046 RID: 70
		private int exernalNicCount;

		// Token: 0x04000047 RID: 71
		private SettingsOptions options;

		// Token: 0x04000048 RID: 72
		private ManualResetEvent settingsLoaded = new ManualResetEvent(false);

		// Token: 0x04000049 RID: 73
		private bool refreshNicsNeeded;

		// Token: 0x0400004A RID: 74
		private ManagementEventWatcher wmiChangeWatcher;

		// Token: 0x0400004B RID: 75
		private object propertyChangedEventLock = new object();

		// Token: 0x0400004C RID: 76
		private VGPUInformation vgpuInfo = new VGPUInformation(VGPUStatus.Enabled);

		// Token: 0x0400004D RID: 77
		private Size requestedResolution;

		// Token: 0x02000020 RID: 32
		private class XdeVirtualMachineNicInformation : IXdeVirtualMachineNicInformation
		{
			// Token: 0x060001AD RID: 429 RVA: 0x00008B77 File Offset: 0x00006D77
			public XdeVirtualMachineNicInformation(XdeVirtualMachineSettings owner, ManagementObject portSettingData, IXdeVirtualSwitchInformation switchInfo)
			{
				this.owner = owner;
				this.macAddress = (portSettingData["Address"] as string);
				this.SwitchInformation = switchInfo;
			}

			// Token: 0x17000072 RID: 114
			// (get) Token: 0x060001AE RID: 430 RVA: 0x00008BA3 File Offset: 0x00006DA3
			// (set) Token: 0x060001AF RID: 431 RVA: 0x00008BAB File Offset: 0x00006DAB
			public IXdeVirtualSwitchInformation SwitchInformation { get; private set; }

			// Token: 0x17000073 RID: 115
			// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008BB4 File Offset: 0x00006DB4
			public string GuestMacAddress
			{
				get
				{
					return this.macAddress;
				}
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x0000583D File Offset: 0x00003A3D
			public void CleanupForDeletion()
			{
			}

			// Token: 0x04000085 RID: 133
			private XdeVirtualMachineSettings owner;

			// Token: 0x04000086 RID: 134
			private string macAddress;
		}

		// Token: 0x02000021 RID: 33
		private class SwitchSettingsInfo
		{
			// Token: 0x060001B2 RID: 434 RVA: 0x00008BBC File Offset: 0x00006DBC
			public SwitchSettingsInfo(ManagementObject portSettingsData, IXdeVirtualSwitchInformation switchInfo)
			{
				this.PortSettingsData = portSettingsData;
				this.SwitchInfo = switchInfo;
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x060001B3 RID: 435 RVA: 0x00008BD2 File Offset: 0x00006DD2
			// (set) Token: 0x060001B4 RID: 436 RVA: 0x00008BDA File Offset: 0x00006DDA
			public ManagementObject PortSettingsData { get; private set; }

			// Token: 0x17000075 RID: 117
			// (get) Token: 0x060001B5 RID: 437 RVA: 0x00008BE3 File Offset: 0x00006DE3
			// (set) Token: 0x060001B6 RID: 438 RVA: 0x00008BEB File Offset: 0x00006DEB
			public IXdeVirtualSwitchInformation SwitchInfo { get; private set; }
		}
	}
}
