using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xde.Common;
using Microsoft.Xde.Interface;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x0200000D RID: 13
	public abstract class XdeDevice : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x000039A0 File Offset: 0x00001BA0
		public XdeDevice()
		{
			this.relatedDevices = new List<XdeDevice>
			{
				this
			}.AsReadOnly();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000D5 RID: 213
		public abstract bool IsDirty { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000D6 RID: 214
		// (set) Token: 0x060000D7 RID: 215
		public abstract Guid ID { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000D8 RID: 216
		// (set) Token: 0x060000D9 RID: 217
		public abstract string Name { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000DA RID: 218
		// (set) Token: 0x060000DB RID: 219
		public abstract string Vhd { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000DC RID: 220
		public abstract string UapVersion { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000DD RID: 221
		// (set) Token: 0x060000DE RID: 222
		public abstract bool UseCheckpoint { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000DF RID: 223
		// (set) Token: 0x060000E0 RID: 224
		public abstract string Sku { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000E1 RID: 225
		// (set) Token: 0x060000E2 RID: 226
		public abstract string Skin { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000E3 RID: 227
		// (set) Token: 0x060000E4 RID: 228
		public abstract int MemSize { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000E5 RID: 229
		// (set) Token: 0x060000E6 RID: 230
		public abstract bool NoGpu { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000E7 RID: 231
		// (set) Token: 0x060000E8 RID: 232
		public abstract bool ShowDisplayName { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000E9 RID: 233
		// (set) Token: 0x060000EA RID: 234
		public abstract string DisplayName { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000EB RID: 235
		// (set) Token: 0x060000EC RID: 236
		public abstract bool UseWmi { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000ED RID: 237
		// (set) Token: 0x060000EE RID: 238
		public abstract bool DisableStateSep { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000EF RID: 239
		// (set) Token: 0x060000F0 RID: 240
		public abstract bool UseDiffDisk { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000039D7 File Offset: 0x00001BD7
		public virtual bool AppShutsDownForDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000F2 RID: 242
		protected abstract bool UsingOldEmulator { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000039DA File Offset: 0x00001BDA
		public string VmName
		{
			get
			{
				return this.Name + "." + Environment.UserName;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000039F1 File Offset: 0x00001BF1
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003A00 File Offset: 0x00001C00
		public virtual bool VisibleToVisualStudio
		{
			get
			{
				return VisualStudioXdeDevice.AppMadeDeviceExists(this.ID);
			}
			set
			{
				if (value != this.VisibleToVisualStudio)
				{
					if (value)
					{
						throw new NotSupportedException();
					}
					string appMadeDevicePath = VisualStudioXdeDevice.GetAppMadeDevicePath(this.ID);
					if (File.Exists(appMadeDevicePath))
					{
						File.Delete(appMadeDevicePath);
					}
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003A3C File Offset: 0x00001C3C
		private bool IsXdeUIRunningImpl
		{
			get
			{
				object obj = this.objLock;
				bool result;
				lock (obj)
				{
					if (string.IsNullOrEmpty(this.VmName))
					{
						result = false;
					}
					else
					{
						result = XdeDevice.DoesXdeMutexExist(this.VmName);
					}
				}
				return result;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000F7 RID: 247 RVA: 0x00003A94 File Offset: 0x00001C94
		// (remove) Token: 0x060000F8 RID: 248 RVA: 0x00003ADC File Offset: 0x00001CDC
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				object obj = this.objLock;
				lock (obj)
				{
					this.PropertyChangedImpl += value;
					this.UpdateTimer();
				}
			}
			remove
			{
				object obj = this.objLock;
				lock (obj)
				{
					this.PropertyChangedImpl -= value;
					this.UpdateTimer();
				}
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000F9 RID: 249 RVA: 0x00003B24 File Offset: 0x00001D24
		// (remove) Token: 0x060000FA RID: 250 RVA: 0x00003B5C File Offset: 0x00001D5C
		private event PropertyChangedEventHandler PropertyChangedImpl;

		// Token: 0x060000FB RID: 251
		public abstract void Save();

		// Token: 0x060000FC RID: 252
		public abstract Task Delete();

		// Token: 0x060000FD RID: 253 RVA: 0x00003B94 File Offset: 0x00001D94
		public void DeleteVirtualMachine()
		{
			this.DeleteDiffDisk();
			Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "microsoft.xde.common.dll")).GetType("Microsoft.Xde.Common.VirtualMachineHelper").GetMethod("DeleteVirtualMachine").Invoke(null, new object[]
			{
				this.Name
			});
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00003BEF File Offset: 0x00001DEF
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00003C28 File Offset: 0x00001E28
		public bool IsXdeUIRunning
		{
			get
			{
				if (this.isXdeUIRunning != null)
				{
					return this.isXdeUIRunning.Value;
				}
				this.isXdeUIRunning = new bool?(this.IsXdeUIRunningImpl);
				return this.isXdeUIRunning.Value;
			}
			private set
			{
				bool? flag = this.isXdeUIRunning;
				if (!(flag.GetValueOrDefault() == value & flag != null))
				{
					this.isXdeUIRunning = new bool?(value);
					this.OnPropertyChanged("IsXdeUIRunning");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00003C69 File Offset: 0x00001E69
		public bool IsVhdValidForLaunching
		{
			get
			{
				return !string.IsNullOrEmpty(this.Vhd) && File.Exists(this.Vhd);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003C8A File Offset: 0x00001E8A
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003C92 File Offset: 0x00001E92
		public string IpAddress
		{
			get
			{
				return this.ipAddress;
			}
			private set
			{
				if (this.ipAddress != value)
				{
					this.ipAddress = value;
					this.OnPropertyChanged("IpAddress");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000103 RID: 259
		public abstract string MacAddress { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003CB4 File Offset: 0x00001EB4
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00003CBC File Offset: 0x00001EBC
		public bool IsShellReady
		{
			get
			{
				return this.isShellReady;
			}
			private set
			{
				if (this.isShellReady != value)
				{
					this.isShellReady = value;
					this.OnPropertyChanged("IsShellReady");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000106 RID: 262
		public abstract bool CanKernelDebug { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000107 RID: 263
		public abstract string XdeLocation { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000108 RID: 264
		public abstract string FileName { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000109 RID: 265
		public abstract bool CanDelete { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003CD9 File Offset: 0x00001ED9
		public virtual IReadOnlyList<XdeDevice> RelatedDevices
		{
			get
			{
				return this.relatedDevices;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003CE1 File Offset: 0x00001EE1
		public virtual bool CanModifyProperty(string propName)
		{
			return true;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public string DiffDisk
		{
			get
			{
				if (!this.UseDiffDisk || string.IsNullOrEmpty(this.Vhd))
				{
					return null;
				}
				string path = string.Format("{0}.{1}", this.ID, Path.GetFileName(this.Vhd));
				return Path.Combine(XdeManagerSettings.Current.DownloadRoot, "diffdisks", path);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00003D40 File Offset: 0x00001F40
		public string GetCommandLine()
		{
			string unvirtualizedPath = XdeAppUtils.GetUnvirtualizedPath(this.Vhd);
			string str = string.Format("-name \"{0}\" -vhd \"{1}\" -sku {2} -video {3} -memSize {4}", new object[]
			{
				this.VmName,
				unvirtualizedPath,
				this.Sku,
				this.Skin,
				this.MemSize
			});
			if (!string.IsNullOrEmpty(this.DiffDisk))
			{
				string unvirtualizedPath2 = XdeAppUtils.GetUnvirtualizedPath(this.DiffDisk);
				string directoryName = Path.GetDirectoryName(unvirtualizedPath2);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!this.UsingOldEmulator)
				{
					str = str + " -usediffdisk \"" + unvirtualizedPath2 + "\"";
				}
				else
				{
					str = str + " -creatediffdisk \"" + unvirtualizedPath2 + "\" -fastShutdown";
				}
			}
			if (this.UseCheckpoint)
			{
				str += " -snapshot -fastShutdown";
			}
			if (this.NoGpu)
			{
				str += " -nogpu";
			}
			if (this.UseWmi)
			{
				str += " -usewmi";
			}
			if (this.DisableStateSep)
			{
				str += " -disablestatesep";
			}
			if (this.ShowDisplayName)
			{
				str += " -showname";
			}
			string text = this.DisplayName;
			if (string.IsNullOrEmpty(text))
			{
				DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.Vhd);
				if (downloadedVhdInfo != null && !string.IsNullOrEmpty(downloadedVhdInfo.BuildVersion))
				{
					string[] array = downloadedVhdInfo.BuildVersion.Split(new char[]
					{
						'.'
					});
					string text2 = array[0];
					string text3 = array[1];
					string text4 = array[2];
					text = string.Concat(new string[]
					{
						this.Name,
						" - ",
						text2,
						".",
						downloadedVhdInfo.Branch,
						".",
						text4
					});
				}
				else
				{
					text = this.Name;
				}
			}
			return str + " -displayName \"" + text + "\"";
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00003F10 File Offset: 0x00002110
		public XdeDevice.StartKernelDebuggerResult StartKernelDebugger()
		{
			string text = "Kernel debugging VM " + this.VmName;
			IntPtr intPtr = NativeMethods.FindTopLevelWindowWithMatchingText(text);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.ShowWindow(intPtr, 1);
				NativeMethods.SetForegroundWindow(intPtr);
				return XdeDevice.StartKernelDebuggerResult.SkippedAlreadyDebugging;
			}
			string arg;
			int num;
			XdeAppUtils.GetKernelDebuggerSettingsForVmName(this.VmName, out arg, out num);
			foreach (string text2 in new string[]
			{
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DBG\\UI\\WinDbgX.exe"),
				"C:\\Debuggers\\windbg.exe",
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Windows Kits\\10\\Debuggers\\x64\\windbg.exe")
			})
			{
				if (File.Exists(text2))
				{
					string arguments = string.Format("-k net:port={0},key={1} -T \"{2}\"", num, arg, text);
					Process.Start(text2, arguments).Dispose();
					return XdeDevice.StartKernelDebuggerResult.Success;
				}
			}
			return XdeDevice.StartKernelDebuggerResult.NoWindbgFound;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003FE2 File Offset: 0x000021E2
		public XdeInstallation GetXdeInstallation()
		{
			return XdeInstallation.GetXdeInstallations().FirstOrDefault((XdeInstallation i) => StringComparer.OrdinalIgnoreCase.Equals(i.XdePath, this.XdeLocation));
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003FFC File Offset: 0x000021FC
		public void GetSkuAndSkin(out XdeSku sku, out XdeSkin skin)
		{
			sku = null;
			skin = null;
			XdeInstallation xdeInstallation = this.GetXdeInstallation();
			if (xdeInstallation == null)
			{
				return;
			}
			sku = xdeInstallation.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, this.Sku));
			if (sku != null)
			{
				skin = sku.FindSkin(this.Skin);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004046 File Offset: 0x00002246
		public string ResolveXdeLocation()
		{
			XdeInstallation xdeInstallation = XdeInstallation.LoadFromPath(this.XdeLocation);
			if (xdeInstallation == null)
			{
				throw new InvalidOperationException("Install path not found for XdeLocation: " + this.XdeLocation);
			}
			return xdeInstallation.ResolvedXdePath;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004074 File Offset: 0x00002274
		public void DeleteDiffDisk()
		{
			string diffDisk = this.DiffDisk;
			if (!string.IsNullOrEmpty(diffDisk) && File.Exists(diffDisk))
			{
				File.Delete(diffDisk);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000040A0 File Offset: 0x000022A0
		public void Start(bool waitForExit, string additionalArgs)
		{
			if (XdeAppUtils.IsRunningAsPackagedEmulator || !XdeAppUtils.IsPackagedEmulatorInstalled)
			{
				if (!string.IsNullOrEmpty(this.DiffDisk))
				{
					string directoryName = Path.GetDirectoryName(this.DiffDisk);
					if (!Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					try
					{
						FileUtils.GrantHyperVRightsForDirectory(directoryName, true);
					}
					catch (UnauthorizedAccessException)
					{
					}
				}
				try
				{
					FileUtils.GrantHyperVRightsForDirectory(this.Vhd);
				}
				catch (UnauthorizedAccessException)
				{
				}
				using (Process process = new Process())
				{
					string text = this.GetCommandLine();
					if (!string.IsNullOrEmpty(additionalArgs))
					{
						text = text + " " + additionalArgs;
					}
					process.StartInfo = new ProcessStartInfo(this.ResolveXdeLocation(), text);
					process.Start();
					if (waitForExit)
					{
						process.WaitForExit();
					}
				}
				return;
			}
			string text2 = "launch \"" + this.Name + "\"";
			if (!string.IsNullOrEmpty(additionalArgs))
			{
				text2 = text2 + " --args \"" + additionalArgs + "\"";
			}
			if (waitForExit)
			{
				text2 += " --wait";
			}
			string str;
			if (XdeDevice.ExecuteXdeConfig(text2, out str) != 0)
			{
				throw new Exception("Failed to start device. Output from xdeconfig:\r\n\r\n" + str);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000041D8 File Offset: 0x000023D8
		public void WaitForConnectionInformation(TimeSpan timeout)
		{
			Task.Run(delegate()
			{
				this.RefreshSettingsFromConnection(timeout);
			}).Wait();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004202 File Offset: 0x00002402
		public void WaitForConnectionInformation()
		{
			this.WaitForConnectionInformation(TimeSpan.FromSeconds(20.0));
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004218 File Offset: 0x00002418
		public void Start(bool waitForExit)
		{
			this.Start(waitForExit, null);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004222 File Offset: 0x00002422
		public void Start()
		{
			this.Start(false, null);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000422C File Offset: 0x0000242C
		public void Stop()
		{
			object obj = this.objLock;
			lock (obj)
			{
				this.RefreshSettingsFromConnection();
				try
				{
					XdeServiceClient xdeServiceClient = this.xdeServiceClient;
					if (xdeServiceClient != null)
					{
						xdeServiceClient.Close();
					}
				}
				catch (ObjectDisposedException)
				{
					this.DisposeXdeServiceClient();
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004294 File Offset: 0x00002494
		public void Dispose()
		{
			this.DisposeXdeServiceClient();
			this.DisposeTimer();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000042A4 File Offset: 0x000024A4
		public void BringToFront()
		{
			object obj = this.objLock;
			lock (obj)
			{
				this.RefreshSettingsFromConnection();
				try
				{
					XdeServiceClient xdeServiceClient = this.xdeServiceClient;
					if (xdeServiceClient != null)
					{
						xdeServiceClient.BringToFront();
					}
				}
				catch (ObjectDisposedException)
				{
					this.DisposeXdeServiceClient();
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000430C File Offset: 0x0000250C
		private static bool DoesXdeMutexExist(string virtualMachineName)
		{
			Mutex mutex;
			if (Mutex.TryOpenExisting(Utility.GetXdeOwnershipMutexName(virtualMachineName), out mutex))
			{
				mutex.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004334 File Offset: 0x00002534
		private static int ExecuteXdeConfig(string args, out string output)
		{
			output = null;
			int exitCode;
			using (Process process = new Process())
			{
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				string errorText = null;
				process.StartInfo.RedirectStandardError = true;
				process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
				{
					errorText += e.Data;
				};
				process.StartInfo.FileName = "xdeconfig.exe";
				process.StartInfo.Arguments = args;
				process.Start();
				process.BeginErrorReadLine();
				output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				output += errorText;
				exitCode = process.ExitCode;
			}
			return exitCode;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004408 File Offset: 0x00002608
		private void RefreshSettingsFromConnection()
		{
			this.RefreshSettingsFromConnection(TimeSpan.FromSeconds(20.0));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004420 File Offset: 0x00002620
		private void RefreshSettingsFromConnection(TimeSpan timeout)
		{
			bool flag = false;
			string text = null;
			bool flag2 = false;
			object obj = this.objLock;
			lock (obj)
			{
				if (this.IsXdeUIRunningImpl)
				{
					flag = true;
					this.EnsureXdeServiceClient(timeout);
					try
					{
						flag2 = this.xdeServiceClient.IsToolsPipeReady();
						string text2;
						this.xdeServiceClient.GetEndPoint(out text2, out text);
						goto IL_68;
					}
					catch (ObjectDisposedException)
					{
						flag = false;
						text = null;
						flag2 = false;
						this.DisposeXdeServiceClient();
						goto IL_68;
					}
				}
				this.DisposeXdeServiceClient();
			}
			IL_68:
			this.IsXdeUIRunning = flag;
			this.IpAddress = text;
			this.IsShellReady = flag2;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000044C8 File Offset: 0x000026C8
		private void EnsureXdeServiceClient(TimeSpan timeout)
		{
			if (this.xdeServiceClient == null)
			{
				this.xdeServiceClient = new XdeServiceClient();
				this.xdeServiceClient.Connect(this.VmName, timeout);
				this.xdeServiceClient.PipeReady += this.XdeServiceClient_PipeReady;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004508 File Offset: 0x00002708
		private void UpdateTimer()
		{
			if (this.PropertyChangedImpl == null)
			{
				if (this.changeTimer != null)
				{
					this.changeTimer.Enabled = false;
					return;
				}
			}
			else
			{
				if (this.changeTimer == null)
				{
					Task.Run(delegate()
					{
						this.RefreshSettingsFromConnection();
					});
					this.changeTimer = new System.Timers.Timer(5000.0);
					this.changeTimer.Elapsed += delegate(object sender, ElapsedEventArgs e)
					{
						this.HandleTimer();
					};
				}
				this.changeTimer.Enabled = true;
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004583 File Offset: 0x00002783
		private void HandleTimer()
		{
			if (this.IsXdeUIRunningImpl != this.IsXdeUIRunning)
			{
				this.RefreshSettingsFromConnection();
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004599 File Offset: 0x00002799
		private void XdeServiceClient_PipeReady()
		{
			Task.Run(delegate()
			{
				string text;
				string text2;
				this.xdeServiceClient.GetEndPoint(out text, out text2);
				this.IpAddress = text2;
				this.IsShellReady = true;
			});
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000045B0 File Offset: 0x000027B0
		private void DisposeXdeServiceClient()
		{
			object obj = this.objLock;
			lock (obj)
			{
				if (this.xdeServiceClient != null)
				{
					this.xdeServiceClient.PipeReady -= this.XdeServiceClient_PipeReady;
					this.xdeServiceClient.Dispose();
					this.xdeServiceClient = null;
				}
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000461C File Offset: 0x0000281C
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedImpl = this.PropertyChangedImpl;
			if (propertyChangedImpl == null)
			{
				return;
			}
			propertyChangedImpl(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004638 File Offset: 0x00002838
		private void DisposeTimer()
		{
			object obj = this.objLock;
			lock (obj)
			{
				if (this.changeTimer != null)
				{
					this.changeTimer.Dispose();
					this.changeTimer = null;
				}
			}
		}

		// Token: 0x0400003C RID: 60
		private const int LookingForAddrTimerDuration = 1000;

		// Token: 0x0400003D RID: 61
		private const int NormalTimerDuration = 5000;

		// Token: 0x0400003E RID: 62
		private readonly object objLock = new object();

		// Token: 0x0400003F RID: 63
		private System.Timers.Timer changeTimer;

		// Token: 0x04000040 RID: 64
		private XdeServiceClient xdeServiceClient;

		// Token: 0x04000041 RID: 65
		private bool? isXdeUIRunning;

		// Token: 0x04000042 RID: 66
		private string ipAddress;

		// Token: 0x04000043 RID: 67
		private bool isShellReady;

		// Token: 0x04000044 RID: 68
		private IReadOnlyList<XdeDevice> relatedDevices;

		// Token: 0x02000025 RID: 37
		public enum StartKernelDebuggerResult
		{
			// Token: 0x040000AD RID: 173
			Success,
			// Token: 0x040000AE RID: 174
			SkippedAlreadyDebugging,
			// Token: 0x040000AF RID: 175
			NoWindbgFound
		}
	}
}
