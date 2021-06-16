using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HCS.Compute.System;
using HCS.Config.Containers.HNS;
using HCS.Schema;
using HCS.Schema.HvSocket;
using HCS.Schema.Options;
using HCS.Schema.Requests;
using HCS.Schema.Requests.System;
using HCS.Schema.Responses.System;
using HCS.Schema.VirtualMachines;
using HCS.Schema.VirtualMachines.Resources;
using HCS.Schema.VirtualMachines.Resources.Compute;
using HCS.Schema.VirtualMachines.Resources.Gpu;
using HCS.Schema.VirtualMachines.Resources.Network;
using HCS.Schema.VirtualMachines.Resources.Storage;
using Microsoft.Tools.WindowsDevicePortal;
using Microsoft.Xde.Common;
using Microsoft.Xde.Hcs.Interop;
using Microsoft.Xde.Hcs.Schema;
using Microsoft.Xde.Hns.Interop;
using Microsoft.Xde.Interface;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Hcs
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public sealed class XdeHcsVirtualMachine : IXdeVirtualMachine, IDisposable
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002538 File Offset: 0x00000738
		private XdeHcsVirtualMachine(Microsoft.Xde.Hcs.Schema.VirtualMachine data, IXdeConnectionAddressInfo addressInfo)
		{
			this.data = data;
			this.addressInfo = addressInfo;
			foreach (VirtualMachineSettings virtualMachineSettings in this.data.Settings)
			{
				this.settings.Add(new XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings(this, virtualMachineSettings));
			}
			this.currentSettings = this.settings.Find((XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings t) => t.UniqueId == this.data.CurrentSettingsId.ToString());
			if (this.currentSettings == null)
			{
				this.currentSettings = this.settings[0];
				this.data.CurrentSettingsId = this.currentSettings.Id;
			}
			if ((from sys in JsonHelper.FromJson<HCS.Compute.System.Properties[]>(XdeHcsVirtualMachine.hcs.EnumerateComputeSystems(null))
			where sys.Id == this.Name
			select sys).FirstOrDefault<HCS.Compute.System.Properties>() != null)
			{
				try
				{
					this.hcsSystem = XdeHcsVirtualMachine.hcs.OpenComputeSystem(this.Name);
				}
				catch (HcsException)
				{
				}
			}
			if (this.hcsSystem != null)
			{
				HCS.Compute.System.Properties hcsProps = this.GetHcsProps();
				this.data.HcsId = hcsProps.RuntimeId;
				if (this.EnabledState == VirtualMachineEnabledState.Enabled)
				{
					this.StartWatchForExit();
				}
			}
			this.nics.Add(new XdeHcsVirtualMachine.XdeHcsVirtualMachineNicInformation(this));
			this.SerializeToFile();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026A8 File Offset: 0x000008A8
		~XdeHcsVirtualMachine()
		{
			this.Dispose(false);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000020 RID: 32 RVA: 0x000026D8 File Offset: 0x000008D8
		// (remove) Token: 0x06000021 RID: 33 RVA: 0x00002710 File Offset: 0x00000910
		public event EventHandler<EnabledStateChangedEventArgs> EnableStateChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000022 RID: 34 RVA: 0x00002748 File Offset: 0x00000948
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x00002780 File Offset: 0x00000980
		public event EventHandler<EventArgs> SnapshotsChanged;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000027B8 File Offset: 0x000009B8
		public VirtualMachineEnabledState EnabledState
		{
			get
			{
				if (this.hcsSystem == null)
				{
					return VirtualMachineEnabledState.Disabled;
				}
				if (this.shuttingDown)
				{
					return VirtualMachineEnabledState.Shutdown;
				}
				switch (this.GetHcsProps().State)
				{
				case State.Created:
					return VirtualMachineEnabledState.Disabled;
				case State.Running:
					return VirtualMachineEnabledState.Enabled;
				case State.Paused:
					return VirtualMachineEnabledState.Paused;
				}
				return VirtualMachineEnabledState.Unknown;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002810 File Offset: 0x00000A10
		public string XdeVmId
		{
			get
			{
				return this.data.XdeVmId.ToString();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002828 File Offset: 0x00000A28
		public string Guid
		{
			get
			{
				return this.data.HcsId.ToString();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002840 File Offset: 0x00000A40
		public string Name
		{
			get
			{
				return this.data.Name;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000284D File Offset: 0x00000A4D
		public IXdeVirtualMachineSettings CurrentSettings
		{
			get
			{
				return this.currentSettings;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002855 File Offset: 0x00000A55
		public ReadOnlyCollection<IXdeVirtualMachineSettings> SnapshotSettings
		{
			get
			{
				List<IXdeVirtualMachineSettings> list = new List<IXdeVirtualMachineSettings>(this.settings);
				list.RemoveAt(0);
				return list.AsReadOnly();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000286E File Offset: 0x00000A6E
		public IXdeVirtualMachineNicInformation InternalNATNic
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002875 File Offset: 0x00000A75
		private string SerializationFileName
		{
			get
			{
				return Path.Combine(XdeHcsVirtualServices.VmInfoPath, this.data.XdeVmId.ToString("N") + ".vm.json");
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028A0 File Offset: 0x00000AA0
		public static XdeHcsVirtualMachine LoadFromFile(string fileName, IXdeConnectionAddressInfo addressInfo)
		{
			Microsoft.Xde.Hcs.Schema.VirtualMachine virtualMachine = JsonHelper.FromJson<Microsoft.Xde.Hcs.Schema.VirtualMachine>(File.ReadAllText(fileName, Encoding.UTF8));
			foreach (VirtualMachineSettings virtualMachineSettings in virtualMachine.Settings)
			{
				VirtualMachineSettings virtualMachineSettings2 = XdeHcsVirtualMachine.CreateDefaultVMSettings(virtualMachine.Name);
				ComputeSystem computeSystem = virtualMachineSettings.ComputeSystem;
				virtualMachineSettings.ComputeSystem = virtualMachineSettings2.ComputeSystem;
				virtualMachineSettings.ComputeSystem.VirtualMachine.ComputeTopology.Processor.Count = computeSystem.VirtualMachine.ComputeTopology.Processor.Count;
				virtualMachineSettings.ComputeSystem.VirtualMachine.ComputeTopology.Memory.SizeInMB = computeSystem.VirtualMachine.ComputeTopology.Memory.SizeInMB;
				virtualMachineSettings.ComputeSystem.VirtualMachine.Devices.VideoMonitor = computeSystem.VirtualMachine.Devices.VideoMonitor;
				virtualMachineSettings.ComputeSystem.VirtualMachine.Devices.Scsi["Boot Disk Controller"].Attachments[0U].Path = computeSystem.VirtualMachine.Devices.Scsi["Boot Disk Controller"].Attachments[0U].Path;
			}
			return new XdeHcsVirtualMachine(virtualMachine, addressInfo);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A10 File Offset: 0x00000C10
		public void ApplySnapshot(IXdeVirtualMachineSettings snapshotSettings)
		{
			XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings xdeHcsVirtualMachineSettings = (XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings)snapshotSettings;
			string vhdPath = snapshotSettings.VhdPath;
			this.currentSettings.Data.ComputeSystem.VirtualMachine.RestoreState = new RestoreState
			{
				RuntimeStateFilePath = xdeHcsVirtualMachineSettings.SavePath
			};
			this.currentSettings.IsSnapshot = true;
			this.currentSettings.EnsureHasDisplayAdapter();
			string text = Path.ChangeExtension(snapshotSettings.VhdPath, ".dvhdx");
			VhdUtils.CreateDiffDisk(vhdPath, text);
			FileUtils.GrantHyperVRightsForFile(text);
			this.TrackCreatedFile(text);
			this.currentSettings.VhdPath = text;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A9C File Offset: 0x00000C9C
		public void CreateSnapshot(string snapshotName)
		{
			VirtualMachineEnabledState enabledState = this.EnabledState;
			if (enabledState != VirtualMachineEnabledState.Enabled && enabledState != VirtualMachineEnabledState.Paused)
			{
				throw new InvalidOperationException(Strings.VMMustBeRunningForSnapshot);
			}
			if (enabledState == VirtualMachineEnabledState.Enabled)
			{
				this.PauseSystem();
			}
			VirtualMachineSettings newSnapshotSettings = this.GetNewSnapshotSettings(snapshotName);
			XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings xdeHcsVirtualMachineSettings = new XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings(this, newSnapshotSettings);
			xdeHcsVirtualMachineSettings.VGPUInformation = this.currentSettings.VGPUInformation;
			this.SaveSystemToSaveFile(xdeHcsVirtualMachineSettings.SavePath);
			this.shuttingDown = true;
			this.TerminateSystem();
			File.Move(this.currentSettings.VhdPath, xdeHcsVirtualMachineSettings.VhdPath);
			this.TrackCreatedFile(xdeHcsVirtualMachineSettings.VhdPath);
			FileUtils.GrantHyperVRightsForFile(xdeHcsVirtualMachineSettings.VhdPath);
			this.data.Settings.Add(newSnapshotSettings);
			this.settings.Add(xdeHcsVirtualMachineSettings);
			this.SerializeToFile();
			this.ApplySnapshot(xdeHcsVirtualMachineSettings);
			this.Start();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B68 File Offset: 0x00000D68
		public void DeleteVirtualMachine()
		{
			if (this.EnabledState == VirtualMachineEnabledState.Enabled)
			{
				this.Stop();
			}
			string serializationFileName = this.SerializationFileName;
			if (File.Exists(serializationFileName))
			{
				File.Delete(serializationFileName);
			}
			foreach (string path in this.data.Files)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			string searchPattern = string.Format("{0:N}*", this.data.XdeVmId);
			string[] files = Directory.GetFiles(XdeHcsVirtualServices.VmInfoPath, searchPattern);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C30 File Offset: 0x00000E30
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002C40 File Offset: 0x00000E40
		public IXdeVirtualMachineSettings FindSnapshotSettings(string snapshotName)
		{
			return this.settings.Find((XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings s) => s.UniqueId == snapshotName || s.Name == snapshotName);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C74 File Offset: 0x00000E74
		public Size GetCurrentResolution()
		{
			VideoMonitor videoMonitor = this.currentSettings.Data.ComputeSystem.VirtualMachine.Devices.VideoMonitor;
			return new Size((int)videoMonitor.HorizontalResolution, (int)videoMonitor.VerticalResolution);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000286E File Offset: 0x00000A6E
		public Image GetScreenShot(int startX, int startY, int width, int height)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000286E File Offset: 0x00000A6E
		public void PressKey(Keys key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000286E File Offset: 0x00000A6E
		public void ReleaseKey(Keys key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public void RemoveSnapshot(string snapshotName)
		{
			XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings xdeHcsVirtualMachineSettings = this.settings.Find((XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings s) => s.Name == snapshotName);
			if (xdeHcsVirtualMachineSettings == null)
			{
				return;
			}
			xdeHcsVirtualMachineSettings.Delete();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000286E File Offset: 0x00000A6E
		public void RevertToStoppedState()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000286E File Offset: 0x00000A6E
		public void SendMouseEvent(MouseEventArgs args)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002CEF File Offset: 0x00000EEF
		public void Start()
		{
			this.Start(false);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public void Start(bool bootingToTakeSnapshot)
		{
			if (this.EnabledState == VirtualMachineEnabledState.Enabled)
			{
				throw new InvalidOperationException(Strings.VMAlreadyRunning);
			}
			this.bootingToTakeSnapshot = bootingToTakeSnapshot;
			this.CreateSystemFromCurrentSettings();
			this.SerializeToFile();
			this.StartSystem();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D27 File Offset: 0x00000F27
		public void IntentionalShutdownComing()
		{
			this.shuttingDown = true;
			this.FireEnabledStateChange();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D38 File Offset: 0x00000F38
		public void Stop()
		{
			VirtualMachineEnabledState enabledState = this.EnabledState;
			if (enabledState != VirtualMachineEnabledState.Enabled && enabledState != VirtualMachineEnabledState.Paused)
			{
				throw new InvalidOperationException(Strings.VMIsntRunning);
			}
			this.shuttingDown = true;
			this.FireEnabledStateChange();
			this.SerializeToFile();
			XdeHcsVirtualMachine.hcs.TerminateComputeSystemAsync(this.hcsSystem, null).Wait();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D8C File Offset: 0x00000F8C
		public void TrackCreatedFile(string fileName)
		{
			if (!this.data.Files.Contains(fileName))
			{
				this.data.Files.Add(fileName);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000286E File Offset: 0x00000A6E
		public void TypeKey(Keys key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public void WriteSettingsToVhd(WindowsImageVhd windowsImage)
		{
			if (!this.currentSettings.IsUsingGpu)
			{
				return;
			}
			bool flag = false;
			foreach (ManagementObject partGpu in this.currentSettings.PossibleGpus)
			{
				string driverPathForPartionableGpu = XdeHcsVirtualServices.GetDriverPathForPartionableGpu(partGpu);
				if (!string.IsNullOrEmpty(driverPathForPartionableGpu) && Directory.Exists(driverPathForPartionableGpu))
				{
					string destDir = Path.Combine("\\windows\\system32\\HostDriverStore\\FileRepository", Path.GetFileName(driverPathForPartionableGpu));
					windowsImage.CopyDirectoryFromLocalToVhd(driverPathForPartionableGpu, destDir);
					flag = true;
				}
			}
			if (!flag)
			{
				this.currentSettings.VGPUInformation = new VGPUInformation
				{
					Status = VGPUStatus.NoCompatibleHostHardwareFound
				};
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E60 File Offset: 0x00001060
		internal static XdeHcsVirtualMachine CreateXdeHcsVirtualMachine(string name, IXdeConnectionAddressInfo addressInfo)
		{
			Microsoft.Xde.Hcs.Schema.VirtualMachine virtualMachine = new Microsoft.Xde.Hcs.Schema.VirtualMachine
			{
				Name = name,
				XdeVmId = System.Guid.NewGuid(),
				Settings = new List<VirtualMachineSettings>(),
				Files = new List<string>(),
				CurrentSettingsId = System.Guid.NewGuid()
			};
			VirtualMachineSettings virtualMachineSettings = XdeHcsVirtualMachine.CreateDefaultVMSettings(virtualMachine.CurrentSettingsId.ToString());
			virtualMachineSettings.Id = virtualMachine.CurrentSettingsId;
			virtualMachine.Settings.Add(virtualMachineSettings);
			return new XdeHcsVirtualMachine(virtualMachine, addressInfo);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002EDC File Offset: 0x000010DC
		private static VirtualMachineSettings CreateDefaultVMSettings(string name)
		{
			return new VirtualMachineSettings
			{
				Name = name,
				Id = System.Guid.NewGuid(),
				CreatedTime = DateTime.UtcNow,
				ComputeSystem = new ComputeSystem
				{
					Owner = "microsoft.xde",
					SchemaVersion = new HCS.Schema.Version
					{
						Major = 2U,
						Minor = 1U
					},
					VirtualMachine = new HCS.Schema.VirtualMachine
					{
						ComputeTopology = new Topology
						{
							Memory = new Memory
							{
								SizeInMB = 1024UL,
								Backing = MemoryBackingType.Virtual
							},
							Processor = new Processor
							{
								Count = 2U,
								ExposeVirtualizationExtensions = true
							}
						},
						Chipset = new Chipset
						{
							Uefi = new Uefi
							{
								BootThis = new UefiBootEntry
								{
									DeviceType = UefiBootDevice.ScsiDrive,
									DevicePath = "Boot Disk Controller",
									DiskNumber = 0
								}
							}
						},
						Devices = new Devices
						{
							VideoMonitor = new VideoMonitor
							{
								HorizontalResolution = (ushort)XdeHcsVirtualMachine.DefaultResolution.Width,
								VerticalResolution = (ushort)XdeHcsVirtualMachine.DefaultResolution.Height
							},
							Keyboard = new Keyboard(),
							Mouse = new Mouse(),
							ComPorts = new Dictionary<uint, ComPort>
							{
								{
									0U,
									new ComPort()
								},
								{
									1U,
									new ComPort()
								}
							},
							Scsi = new Dictionary<string, Scsi>
							{
								{
									"Boot Disk Controller",
									new Scsi
									{
										Attachments = new Dictionary<uint, Attachment>
										{
											{
												0U,
												new Attachment
												{
													Type = AttachmentType.VirtualDisk
												}
											}
										}
									}
								}
							}
						},
						StopOnReset = true
					}
				}
			};
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003078 File Offset: 0x00001278
		private void Dispose(bool disposing)
		{
			this.FreeEndpoint();
			if (disposing)
			{
				this.DisposeHcsSystem();
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003089 File Offset: 0x00001289
		private void AddressInfo_Ready(object sender, EventArgs e)
		{
			this.StartLookingForGpuDevice();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003094 File Offset: 0x00001294
		private VirtualMachineSettings GetNewSnapshotSettings(string snapshotName)
		{
			VirtualMachineSettings virtualMachineSettings = JsonHelper.FromJson<VirtualMachineSettings>(JsonHelper.ToJson<VirtualMachineSettings>(this.currentSettings.Data));
			virtualMachineSettings.Name = snapshotName;
			virtualMachineSettings.Id = System.Guid.NewGuid();
			virtualMachineSettings.CreatedTime = DateTime.UtcNow;
			string directoryName = Path.GetDirectoryName(this.CurrentSettings.VhdPath);
			string path = string.Format("{0:N}_{1:N}.vhdx", this.data.XdeVmId, virtualMachineSettings.Id);
			string vhd = Path.Combine(directoryName, path);
			virtualMachineSettings.Vhd = vhd;
			return virtualMachineSettings;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000311C File Offset: 0x0000131C
		private void SaveSystemToSaveFile(string saveFile)
		{
			string options = JsonHelper.ToJson<SaveOptions>(new SaveOptions
			{
				RuntimeStateFilePath = saveFile
			});
			XdeHcsVirtualMachine.hcs.SaveComputeSystemAsync(this.hcsSystem, options).Wait();
			try
			{
				FileUtils.GrantHyperVRightsForFile(saveFile);
			}
			catch (UnauthorizedAccessException)
			{
			}
			this.TrackCreatedFile(saveFile);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003174 File Offset: 0x00001374
		private void PauseSystem()
		{
			XdeHcsVirtualMachine.hcs.PauseComputeSystemAsync(this.hcsSystem, null).Wait();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000318C File Offset: 0x0000138C
		private void TerminateSystem()
		{
			XdeHcsVirtualMachine.hcs.TerminateComputeSystemAsync(this.hcsSystem, null).Wait();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000031A4 File Offset: 0x000013A4
		private HCS.Compute.System.Properties GetHcsProps()
		{
			return JsonHelper.FromJson<HCS.Compute.System.Properties>(XdeHcsVirtualMachine.hcs.GetComputeSystemProperties(this.hcsSystem, null));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000031BC File Offset: 0x000013BC
		private void FreeEndpoint()
		{
			if (this.nicEndpoint != null)
			{
				HnsUtils.DeleteEndpoint(this.nicEndpoint);
				this.nicEndpoint = null;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000031D8 File Offset: 0x000013D8
		private void DisposeHcsSystem()
		{
			if (this.hcsSystem != null)
			{
				this.hcsSystem.Dispose();
				this.hcsSystem = null;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000031F4 File Offset: 0x000013F4
		private void StartSystem()
		{
			this.shuttingDown = false;
			try
			{
				FileUtils.GrantHyperVRightsForFile(this.CurrentSettings.VhdPath);
			}
			catch (UnauthorizedAccessException)
			{
			}
			XdeHcsVirtualMachine.hcs.StartComputeSystemAsync(this.hcsSystem, null).Wait();
			Logger.Instance.Log("HCSStartComputeSystemAsync", Logger.Level.Info);
			this.FireEnabledStateChange();
			if (!this.bootingToTakeSnapshot)
			{
				for (int i = 0; i < 3; i++)
				{
					try
					{
						this.AddNICtoVM();
						Logger.Instance.Log("HCSAddNICSucceeded", Logger.Level.Info);
						break;
					}
					catch (Exception ex)
					{
						Thread.Sleep(1000);
						if (i == 2)
						{
							Logger.Instance.LogException("HCSAddNICToVMFailed", ex);
						}
						else
						{
							Logger.Instance.Log("HCSAddNICToVMFailedWillRetry", Logger.Level.Info, new
							{
								ex.Message
							});
						}
					}
				}
			}
			if (this.currentSettings.IsUsingGpu)
			{
				try
				{
					if (!this.bootingToTakeSnapshot)
					{
						if (this.addressInfo != null)
						{
							this.addressInfo.Ready += this.AddressInfo_Ready;
						}
						this.AddGPUtoVM();
						Logger.Instance.Log("HCSAddGPUtoVMSucceeded", Logger.Level.Info);
					}
				}
				catch (Exception ex2)
				{
					Logger.Instance.LogException("AddGpuToHcsVMFailed", ex2);
					this.currentSettings.VGPUInformation = new VGPUInformation
					{
						Status = VGPUStatus.VMFailedToStartWithVGPU,
						AdditionalInfo = ex2.Message
					};
				}
			}
			this.StartWatchForExit();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000336C File Offset: 0x0000156C
		private void StartWatchForExit()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.WatchForExit));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003380 File Offset: 0x00001580
		private void WatchForExit(object unused)
		{
			XdeHcsVirtualMachine.hcs.WaitOnComputeSystemExitAsync(this.hcsSystem).Wait();
			Logger.Instance.Log("HCSWaitOnComputeSystemExitAsyncExited", Logger.Level.Info);
			if (!this.shuttingDown)
			{
				this.shuttingDown = true;
				this.FireEnabledStateChange();
				Logger.Instance.Log("HCSWaitOnComputeSystemRestartNeeded", Logger.Level.Info);
				this.Start();
				return;
			}
			Logger.Instance.Log("HCSWaitOnComputeSystemRestartNotNeeded", Logger.Level.Info);
			this.shuttingDown = false;
			this.FireEnabledStateChange();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033FE File Offset: 0x000015FE
		private void StartLookingForGpuDevice()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.LookForGpuDevice));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003414 File Offset: 0x00001614
		private void LookForGpuDevice(object unused)
		{
			string guestIpAddress = this.addressInfo.GuestIpAddress;
			if (!string.IsNullOrEmpty(guestIpAddress))
			{
				DevicePortal devicePortal = new DevicePortal("http://" + guestIpAddress);
				DevicePortal.Device device = null;
				try
				{
					Task<List<DevicePortal.Device>> deviceListAsync = devicePortal.GetDeviceListAsync();
					deviceListAsync.Wait();
					device = (from d in deviceListAsync.Result
					where d.ID.StartsWith("PCI\\VEN_1414&DEV_008E", StringComparison.OrdinalIgnoreCase)
					select d).FirstOrDefault<DevicePortal.Device>();
				}
				catch (Exception ex)
				{
					this.currentSettings.VGPUInformation = new VGPUInformation
					{
						Status = VGPUStatus.WdpQueryFailed,
						AdditionalInfo = ex.Message
					};
					return;
				}
				if (device != null)
				{
					if (device.ProblemCode == 0)
					{
						this.currentSettings.VGPUInformation = new VGPUInformation
						{
							Status = VGPUStatus.Running,
							GpuAssignmentMode = this.currentSettings.VGPUInformation.GpuAssignmentMode
						};
						return;
					}
					this.currentSettings.VGPUInformation = new VGPUInformation
					{
						Status = VGPUStatus.VGPUDriverInFailedState,
						AdditionalInfo = StringUtilities.CurrentCultureFormat(Strings.VGPUDriverInFailedState, new object[]
						{
							device.ProblemCode
						})
					};
					return;
				}
				else
				{
					this.currentSettings.VGPUInformation = new VGPUInformation
					{
						Status = VGPUStatus.VGPUNotFoundOnGuest
					};
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003570 File Offset: 0x00001770
		private void AddGPUtoVM()
		{
			ModifySettingRequest obj = new ModifySettingRequest
			{
				ResourcePath = "VirtualMachine/ComputeTopology/Gpu",
				RequestType = ModifyRequestType.Update,
				Settings = this.currentSettings.GpuConfiguration
			};
			XdeHcsVirtualMachine.hcs.ModifyComputeSystem(this.hcsSystem, JsonHelper.ToJson<ModifySettingRequest>(obj));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000035C0 File Offset: 0x000017C0
		private void RemoveGPU()
		{
			ModifySettingRequest obj = new ModifySettingRequest
			{
				ResourcePath = "VirtualMachine/ComputeTopology/Gpu",
				RequestType = ModifyRequestType.Update,
				Settings = new GpuConfiguration
				{
					AllowVendorExtension = true,
					AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Disabled
				}
			};
			XdeHcsVirtualMachine.hcs.ModifyComputeSystem(this.hcsSystem, JsonHelper.ToJson<ModifySettingRequest>(obj));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003618 File Offset: 0x00001818
		private void AddNICtoVM()
		{
			this.FreeEndpoint();
			string macAddressForVmName = XdeMacAddressSettings.GetMacAddressForVmName(this.Name, true);
			this.nicEndpoint = HnsUtils.CreateEndpoint(HnsUtils.DefaultSwitchId, macAddressForVmName);
			NetworkAdapter networkAdapter = new NetworkAdapter
			{
				EndpointId = this.nicEndpoint.ID,
				MacAddress = this.nicEndpoint.MacAddress
			};
			string arg = System.Guid.NewGuid().ToString();
			string configuration = JsonHelper.ToJson<ModifySettingRequest>(new ModifySettingRequest
			{
				ResourcePath = string.Format("VirtualMachine/Devices/NetworkAdapters/{0}", arg),
				RequestType = ModifyRequestType.Add,
				Settings = networkAdapter
			});
			XdeHcsVirtualMachine.hcs.ModifyComputeSystem(this.hcsSystem, configuration);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000036C4 File Offset: 0x000018C4
		private void SerializeToFile()
		{
			this.ScrubDataForWrite();
			string contents = JsonHelper.ToJson<Microsoft.Xde.Hcs.Schema.VirtualMachine>(this.data);
			File.WriteAllText(this.SerializationFileName, contents, Encoding.UTF8);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000036F4 File Offset: 0x000018F4
		private void ScrubDataForWrite()
		{
			foreach (XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings xdeHcsVirtualMachineSettings in this.settings)
			{
				xdeHcsVirtualMachineSettings.Data.ComputeSystem.VirtualMachine.Devices.HvSocket = null;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000375C File Offset: 0x0000195C
		private void CreateSystemFromCurrentSettings()
		{
			if (this.hcsSystem != null && this.EnabledState == VirtualMachineEnabledState.Disabled)
			{
				this.TerminateSystem();
			}
			this.DisposeHcsSystem();
			this.AddHyperVSocketConfig();
			string configuration = JsonHelper.ToJson<ComputeSystem>(this.currentSettings.Data.ComputeSystem);
			Task<SafeHcsSystemHandle> task = XdeHcsVirtualMachine.hcs.CreateComputeSystemAsync(this.Name, configuration, null);
			task.Wait();
			this.hcsSystem = task.Result;
			HCS.Compute.System.Properties hcsProps = this.GetHcsProps();
			this.data.HcsId = hcsProps.RuntimeId;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000037E0 File Offset: 0x000019E0
		private void AddHyperVSocketConfig()
		{
			this.currentSettings.Data.ComputeSystem.VirtualMachine.Devices.HvSocket = new HvSocket
			{
				HvSocketConfig = new HvSocketSystemConfig
				{
					ServiceTable = new Dictionary<Guid, HvSocketServiceConfig>()
				}
			};
			string connectSecurityDescriptor = StringUtilities.InvariantCultureFormat("D:P(A;;FA;;;{0})", new object[]
			{
				WindowsIdentity.GetCurrent().User
			});
			this.currentSettings.Data.ComputeSystem.VirtualMachine.Devices.HvSocket.HvSocketConfig.ServiceTable[Globals.XdeServicesId] = new HvSocketServiceConfig
			{
				BindSecurityDescriptor = "D:P(D;;FA;;;WD)",
				ConnectSecurityDescriptor = connectSecurityDescriptor,
				AllowWildcardBinds = true
			};
			this.currentSettings.Data.ComputeSystem.VirtualMachine.Devices.HvSocket.HvSocketConfig.ServiceTable[Globals.XdeExternalMonitorId] = new HvSocketServiceConfig
			{
				BindSecurityDescriptor = "D:P(D;;FA;;;WD)",
				ConnectSecurityDescriptor = connectSecurityDescriptor,
				AllowWildcardBinds = true
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000038E7 File Offset: 0x00001AE7
		private void FireEnabledStateChange()
		{
			EventHandler<EnabledStateChangedEventArgs> enableStateChanged = this.EnableStateChanged;
			if (enableStateChanged == null)
			{
				return;
			}
			enableStateChanged(this, new EnabledStateChangedEventArgs(this.EnabledState));
		}

		// Token: 0x04000006 RID: 6
		public const string ScsiControllerName = "Boot Disk Controller";

		// Token: 0x04000007 RID: 7
		private const int DefaultStartupMemory = 1024;

		// Token: 0x04000008 RID: 8
		private const int DefaultProcessorCount = 2;

		// Token: 0x04000009 RID: 9
		private static readonly Size DefaultResolution = new Size(1024, 768);

		// Token: 0x0400000A RID: 10
		private static IHcs hcs = HcsFactory.GetHcs();

		// Token: 0x0400000B RID: 11
		private readonly List<IXdeVirtualMachineNicInformation> nics = new List<IXdeVirtualMachineNicInformation>();

		// Token: 0x0400000C RID: 12
		private readonly List<XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings> settings = new List<XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings>();

		// Token: 0x0400000D RID: 13
		private Microsoft.Xde.Hcs.Schema.VirtualMachine data;

		// Token: 0x0400000E RID: 14
		private XdeHcsVirtualMachine.XdeHcsVirtualMachineSettings currentSettings;

		// Token: 0x0400000F RID: 15
		private HNSEndpoint nicEndpoint;

		// Token: 0x04000010 RID: 16
		private SafeHcsSystemHandle hcsSystem;

		// Token: 0x04000011 RID: 17
		private IXdeConnectionAddressInfo addressInfo;

		// Token: 0x04000012 RID: 18
		private bool shuttingDown;

		// Token: 0x04000013 RID: 19
		private bool bootingToTakeSnapshot;

		// Token: 0x0200001D RID: 29
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class XdeHcsVirtualSwitchInformation : IXdeVirtualSwitchInformation
		{
			// Token: 0x060000D0 RID: 208 RVA: 0x00004430 File Offset: 0x00002630
			private XdeHcsVirtualSwitchInformation(HNSNetwork network)
			{
				this.network = network;
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000443F File Offset: 0x0000263F
			public string Name
			{
				get
				{
					return this.network.Name;
				}
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000444C File Offset: 0x0000264C
			public string Id
			{
				get
				{
					return this.network.ID;
				}
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004459 File Offset: 0x00002659
			public bool External
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000445C File Offset: 0x0000265C
			public string HostIpAddress
			{
				get
				{
					return this.network.Subnets[0].GatewayAddress;
				}
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004470 File Offset: 0x00002670
			public string HostIpMask
			{
				get
				{
					int num = int.Parse(this.network.Subnets[0].AddressPrefix.Split(new char[]
					{
						'/'
					})[1]);
					return new IPAddress(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(-1 << 32 - num))).ToString();
				}
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000286E File Offset: 0x00000A6E
			public string HostMacAddress
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x000044C4 File Offset: 0x000026C4
			public static XdeHcsVirtualMachine.XdeHcsVirtualSwitchInformation LoadFromDefaultSwitch()
			{
				return new XdeHcsVirtualMachine.XdeHcsVirtualSwitchInformation(HnsUtils.GetNetwork(HnsUtils.DefaultSwitchId));
			}

			// Token: 0x0400004E RID: 78
			private HNSNetwork network;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class XdeHcsVirtualMachineNicInformation : IXdeVirtualMachineNicInformation
		{
			// Token: 0x060000D8 RID: 216 RVA: 0x000044D5 File Offset: 0x000026D5
			public XdeHcsVirtualMachineNicInformation(XdeHcsVirtualMachine parent)
			{
				this.parent = parent;
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000D9 RID: 217 RVA: 0x000044E4 File Offset: 0x000026E4
			public IXdeVirtualSwitchInformation SwitchInformation
			{
				get
				{
					if (this.switchInfo == null)
					{
						this.switchInfo = XdeHcsVirtualMachine.XdeHcsVirtualSwitchInformation.LoadFromDefaultSwitch();
					}
					return this.switchInfo;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000DA RID: 218 RVA: 0x00004500 File Offset: 0x00002700
			public string GuestMacAddress
			{
				get
				{
					string text = (this.parent.nicEndpoint != null) ? this.parent.nicEndpoint.MacAddress : null;
					if (text != null)
					{
						text = text.Replace("-", string.Empty);
					}
					return text;
				}
			}

			// Token: 0x060000DB RID: 219 RVA: 0x00004543 File Offset: 0x00002743
			public void CleanupForDeletion()
			{
			}

			// Token: 0x0400004F RID: 79
			private XdeHcsVirtualMachine.XdeHcsVirtualSwitchInformation switchInfo;

			// Token: 0x04000050 RID: 80
			private XdeHcsVirtualMachine parent;
		}

		// Token: 0x0200001F RID: 31
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private class XdeHcsVirtualMachineSettings : IXdeVirtualMachineSettings, IDisposable, INotifyPropertyChanged
		{
			// Token: 0x060000DC RID: 220 RVA: 0x00004548 File Offset: 0x00002748
			public XdeHcsVirtualMachineSettings(XdeHcsVirtualMachine parent, VirtualMachineSettings data)
			{
				this.parent = parent;
				this.data = data;
			}

			// Token: 0x14000003 RID: 3
			// (add) Token: 0x060000DD RID: 221 RVA: 0x000045A4 File Offset: 0x000027A4
			// (remove) Token: 0x060000DE RID: 222 RVA: 0x000045DC File Offset: 0x000027DC
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000DF RID: 223 RVA: 0x00004611 File Offset: 0x00002811
			public VirtualMachineSettings Data
			{
				get
				{
					return this.data;
				}
			}

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004619 File Offset: 0x00002819
			public int Generation
			{
				get
				{
					return 2;
				}
			}

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000461C File Offset: 0x0000281C
			public DateTime CreationTime
			{
				get
				{
					return this.data.CreatedTime;
				}
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004629 File Offset: 0x00002829
			// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004636 File Offset: 0x00002836
			public string Name
			{
				get
				{
					return this.data.Name;
				}
				set
				{
					this.data.Name = value;
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004644 File Offset: 0x00002844
			public string UniqueId
			{
				get
				{
					return this.data.Id.ToString();
				}
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000465C File Offset: 0x0000285C
			public Guid Id
			{
				get
				{
					return this.data.Id;
				}
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004669 File Offset: 0x00002869
			// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004676 File Offset: 0x00002876
			public string VhdPath
			{
				get
				{
					return this.data.Vhd;
				}
				set
				{
					this.data.Vhd = value;
				}
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004684 File Offset: 0x00002884
			public string SavePath
			{
				get
				{
					return Path.ChangeExtension(this.VhdPath, ".dat");
				}
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004696 File Offset: 0x00002896
			// (set) Token: 0x060000EA RID: 234 RVA: 0x000046B7 File Offset: 0x000028B7
			public int NumProcessors
			{
				get
				{
					return (int)this.data.ComputeSystem.VirtualMachine.ComputeTopology.Processor.Count;
				}
				set
				{
					this.data.ComputeSystem.VirtualMachine.ComputeTopology.Processor.Count = (uint)value;
				}
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x060000EB RID: 235 RVA: 0x000046D9 File Offset: 0x000028D9
			// (set) Token: 0x060000EC RID: 236 RVA: 0x000046FB File Offset: 0x000028FB
			public int RamSize
			{
				get
				{
					return (int)this.data.ComputeSystem.VirtualMachine.ComputeTopology.Memory.SizeInMB;
				}
				set
				{
					this.data.ComputeSystem.VirtualMachine.ComputeTopology.Memory.SizeInMB = (ulong)value;
				}
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x060000ED RID: 237 RVA: 0x0000471E File Offset: 0x0000291E
			// (set) Token: 0x060000EE RID: 238 RVA: 0x00004745 File Offset: 0x00002945
			public string Com1NamedPipe
			{
				get
				{
					return this.data.ComputeSystem.VirtualMachine.Devices.ComPorts[0U].NamedPipe;
				}
				set
				{
					this.data.ComputeSystem.VirtualMachine.Devices.ComPorts[0U].NamedPipe = value;
				}
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x060000EF RID: 239 RVA: 0x0000476D File Offset: 0x0000296D
			// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004794 File Offset: 0x00002994
			public string Com2NamedPipe
			{
				get
				{
					return this.data.ComputeSystem.VirtualMachine.Devices.ComPorts[1U].NamedPipe;
				}
				set
				{
					this.data.ComputeSystem.VirtualMachine.Devices.ComPorts[1U].NamedPipe = value;
				}
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060000F1 RID: 241 RVA: 0x000047BC File Offset: 0x000029BC
			public ReadOnlyCollection<IXdeVirtualMachineNicInformation> Nics
			{
				get
				{
					return this.parent.nics.AsReadOnly();
				}
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060000F2 RID: 242 RVA: 0x000047CE File Offset: 0x000029CE
			public ReadOnlyCollection<ManagementObject> PossibleGpus
			{
				get
				{
					return this.possibleGpus.AsReadOnly();
				}
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x060000F3 RID: 243 RVA: 0x000047DB File Offset: 0x000029DB
			// (set) Token: 0x060000F4 RID: 244 RVA: 0x000047E3 File Offset: 0x000029E3
			public bool IsInvalid { get; private set; }

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004459 File Offset: 0x00002659
			public InvalidSettingsReason InvalidSettingsReason
			{
				get
				{
					return InvalidSettingsReason.NotInvalid;
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x000047EC File Offset: 0x000029EC
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x000047F9 File Offset: 0x000029F9
			public string Notes
			{
				get
				{
					return this.data.Notes;
				}
				set
				{
					this.data.Notes = value;
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000286E File Offset: 0x00000A6E
			public Exception AsyncLoadException
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004807 File Offset: 0x00002A07
			public bool IsUsingGpu
			{
				get
				{
					return this.vgpuInfo.Status == VGPUStatus.Enabled || this.vgpuInfo.Status == VGPUStatus.Running;
				}
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060000FA RID: 250 RVA: 0x00004827 File Offset: 0x00002A27
			// (set) Token: 0x060000FB RID: 251 RVA: 0x0000482F File Offset: 0x00002A2F
			public bool IsSnapshot { get; set; }

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060000FC RID: 252 RVA: 0x00004838 File Offset: 0x00002A38
			public GpuConfiguration GpuConfiguration { get; } = new GpuConfiguration
			{
				AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Default,
				AllowVendorExtension = true
			};

			// Token: 0x060000FD RID: 253 RVA: 0x00004543 File Offset: 0x00002743
			public void CleanupForDeletion()
			{
			}

			// Token: 0x060000FE RID: 254 RVA: 0x00004840 File Offset: 0x00002A40
			public void Delete()
			{
				this.parent.settings.Remove(this);
				this.parent.data.Settings.Remove(this.data);
				if (File.Exists(this.SavePath))
				{
					File.Delete(this.SavePath);
				}
				this.parent.SerializeToFile();
				this.IsInvalid = true;
			}

			// Token: 0x060000FF RID: 255 RVA: 0x00004543 File Offset: 0x00002743
			public void Dispose()
			{
			}

			// Token: 0x06000100 RID: 256 RVA: 0x000048A8 File Offset: 0x00002AA8
			public void EnsureHasDisplayAdapter()
			{
				VideoMonitor videoMonitor = this.Data.ComputeSystem.VirtualMachine.Devices.VideoMonitor;
				Size requestedResolution = new Size((int)videoMonitor.HorizontalResolution, (int)videoMonitor.VerticalResolution);
				this.EnsureHasDisplayAdapter(this.Data.VgpuStatus, requestedResolution, 0, this.Data.GpuAssignmentMode);
			}

			// Token: 0x06000101 RID: 257 RVA: 0x00004904 File Offset: 0x00002B04
			public void EnsureHasDisplayAdapter(VGPUStatus vgpuStatus, Size requestedResolution, int vgpuRamMB, Microsoft.Xde.Common.GpuAssignmentMode gpuAssignmentMode)
			{
				this.data.VgpuStatus = vgpuStatus;
				this.data.GpuAssignmentMode = gpuAssignmentMode;
				this.possibleGpus.Clear();
				switch (gpuAssignmentMode)
				{
				case Microsoft.Xde.Common.GpuAssignmentMode.None:
					this.GpuConfiguration.AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Disabled;
					break;
				case Microsoft.Xde.Common.GpuAssignmentMode.Default:
					this.GpuConfiguration.AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Default;
					this.possibleGpus.AddRange(XdeHcsVirtualServices.GetPartitonableGpus());
					break;
				case Microsoft.Xde.Common.GpuAssignmentMode.SingleMostPerformant:
				{
					ManagementObject mostPerformantPartionableGpu = XdeHcsVirtualServices.GetMostPerformantPartionableGpu();
					if (mostPerformantPartionableGpu != null)
					{
						this.GpuConfiguration.AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.List;
						this.GpuConfiguration.AssignmentRequest = new Dictionary<string, ushort>
						{
							{
								(string)mostPerformantPartionableGpu["ElementName"],
								ushort.MaxValue
							}
						};
						this.possibleGpus.Add(mostPerformantPartionableGpu);
					}
					break;
				}
				case Microsoft.Xde.Common.GpuAssignmentMode.All:
					this.GpuConfiguration.AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Mirror;
					this.possibleGpus.AddRange(XdeHcsVirtualServices.GetPartitonableGpus());
					break;
				}
				if (vgpuStatus == VGPUStatus.Enabled && this.possibleGpus.Count == 0)
				{
					vgpuStatus = VGPUStatus.NoCompatibleHostHardwareFound;
					gpuAssignmentMode = Microsoft.Xde.Common.GpuAssignmentMode.None;
					this.GpuConfiguration.AssignmentMode = HCS.Schema.VirtualMachines.Resources.Gpu.GpuAssignmentMode.Disabled;
				}
				this.VGPUInformation = new VGPUInformation
				{
					Status = vgpuStatus,
					GpuAssignmentMode = gpuAssignmentMode
				};
				VideoMonitor videoMonitor = this.Data.ComputeSystem.VirtualMachine.Devices.VideoMonitor;
				videoMonitor.HorizontalResolution = (ushort)requestedResolution.Width;
				videoMonitor.VerticalResolution = (ushort)requestedResolution.Height;
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000102 RID: 258 RVA: 0x00004A5F File Offset: 0x00002C5F
			// (set) Token: 0x06000103 RID: 259 RVA: 0x00004A67 File Offset: 0x00002C67
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

			// Token: 0x06000104 RID: 260 RVA: 0x00004A7C File Offset: 0x00002C7C
			public ulong GetFilesSize()
			{
				ulong num = (ulong)new FileInfo(this.VhdPath).Length;
				if (!string.IsNullOrEmpty(this.SavePath) && File.Exists(this.SavePath))
				{
					num += (ulong)new FileInfo(this.SavePath).Length;
				}
				return num;
			}

			// Token: 0x06000105 RID: 261 RVA: 0x0000286E File Offset: 0x00000A6E
			public Bitmap GetThumbnail(int width)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000106 RID: 262 RVA: 0x00004AC8 File Offset: 0x00002CC8
			public void UpdateDisplayControllerSettings(IXdeDisplayControllerSettings settings)
			{
				VideoMonitor videoMonitor = this.Data.ComputeSystem.VirtualMachine.Devices.VideoMonitor;
				videoMonitor.HorizontalResolution = (ushort)settings.Resolution.Width;
				videoMonitor.VerticalResolution = (ushort)settings.Resolution.Height;
			}

			// Token: 0x06000107 RID: 263 RVA: 0x00004543 File Offset: 0x00002743
			public void WaitForLoadedSettings()
			{
			}

			// Token: 0x06000108 RID: 264 RVA: 0x00004B18 File Offset: 0x00002D18
			private void OnPropertyChanged(string propName)
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}

			// Token: 0x04000051 RID: 81
			private XdeHcsVirtualMachine parent;

			// Token: 0x04000052 RID: 82
			private VirtualMachineSettings data;

			// Token: 0x04000053 RID: 83
			private List<ManagementObject> possibleGpus = new List<ManagementObject>();

			// Token: 0x04000054 RID: 84
			private VGPUInformation vgpuInfo = new VGPUInformation
			{
				Status = VGPUStatus.Enabled
			};

			// Token: 0x02000027 RID: 39
			[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
			private class DisplayControllerSettings : IXdeDisplayControllerSettings
			{
				// Token: 0x17000047 RID: 71
				// (get) Token: 0x06000136 RID: 310 RVA: 0x00005275 File Offset: 0x00003475
				// (set) Token: 0x06000137 RID: 311 RVA: 0x0000527D File Offset: 0x0000347D
				public ResolutionType ResolutionType { get; set; }

				// Token: 0x17000048 RID: 72
				// (get) Token: 0x06000138 RID: 312 RVA: 0x00005286 File Offset: 0x00003486
				// (set) Token: 0x06000139 RID: 313 RVA: 0x0000528E File Offset: 0x0000348E
				public Size Resolution { get; set; }
			}
		}
	}
}
