using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using DiscUtils;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace XdeConfig
{
	// Token: 0x02000002 RID: 2
	public class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		private static int Main(string[] args)
		{
			if (args.Length > 2 && args[0] == "/name" && args.Contains("/vhd") && args.Contains("/creatediffdisk"))
			{
				string text = args[1];
				string text2 = "." + Environment.UserName;
				if (text.EndsWith(text2))
				{
					text = text.Substring(0, text.Length - text2.Length);
				}
				return Program.RunLaunchAndReturnExitCode(new Program.LaunchOptions
				{
					Device = text,
					Wait = true
				});
			}
			Console.CancelKeyPress += delegate(object s, ConsoleCancelEventArgs e)
			{
				Console.CursorVisible = true;
			};
			if (args.Length >= 2 && !args[0].StartsWith("-") && !args[1].StartsWith("-") && !StringComparer.OrdinalIgnoreCase.Equals(args[0], "help"))
			{
				List<string> list = new List<string>(args);
				list.Insert(1, "-d");
				args = list.ToArray();
			}
			int result;
			if (XdeAppUtils.IsInternalVersion)
			{
				result = Parser.Default.ParseArguments(args).MapResult((Program.ListOptions opts) => Program.RunListAndReturnExitCode(opts), (Program.RedownloadOptions opts) => Program.RunRedownloadAndReturnExitCode(opts), (Program.ResetOptions opts) => Program.RunResetAndReturnExitCode(opts), (Program.GetLatestOptions opts) => Program.RunGetLatestAndReturnExitCode(opts), (Program.DownloadOptions opts) => Program.RunDownloadAndReturnExitCode(opts), (Program.CreateOptions opts) => Program.RunCreateAndReturnExitCode(opts), (Program.EditOptions opts) => Program.RunEditAndReturnExitCode(opts), (Program.LaunchOptions opts) => Program.RunLaunchAndReturnExitCode(opts), (Program.KDebugOptions opts) => Program.RunKDebugAndReturnExitCode(opts), (Program.DeleteOptions opts) => Program.RunDeleteOptionsAndExitCode(opts), (Program.CleanDeviceListOptions opts) => Program.RunCleanDeviceListAndExitCode(opts), (Program.MountImageOptions opts) => Program.RunMountImageAndReturnExitCode(opts), (Program.DismountImageOptions opts) => Program.RunDismountImageAndReturnExitCode(opts), (Program.InstallPowerShellOptions opts) => Program.RunInstallPowerShellModuleAndReturnExitCode(opts), (Program.XdeAppOptions opts) => Program.RunXdeAppOptionsAndExitCode(opts), (IEnumerable<Error> errs) => 1);
			}
			else
			{
				result = Parser.Default.ParseArguments(args).MapResult((Program.ListOptions opts) => Program.RunListAndReturnExitCode(opts), (Program.ResetOptions opts) => Program.RunResetAndReturnExitCode(opts), (Program.GetLatestOptions opts) => Program.RunGetLatestAndReturnExitCode(opts), (Program.CreateOptions opts) => Program.RunCreateAndReturnExitCode(opts), (Program.EditOptions opts) => Program.RunEditAndReturnExitCode(opts), (Program.LaunchOptions opts) => Program.RunLaunchAndReturnExitCode(opts), (Program.DeleteOptions opts) => Program.RunDeleteOptionsAndExitCode(opts), (Program.CleanDeviceListOptions opts) => Program.RunCleanDeviceListAndExitCode(opts), (Program.InstallPowerShellOptions opts) => Program.RunInstallPowerShellModuleAndReturnExitCode(opts), (Program.XdeAppOptions opts) => Program.RunXdeAppOptionsAndExitCode(opts), (IEnumerable<Error> errs) => 1);
			}
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000024BC File Offset: 0x000006BC
		private static XdeDevice FindDevice(string name)
		{
			return XdeDeviceFactory.GetDevices().FirstOrDefault((XdeDevice d) => StringComparer.OrdinalIgnoreCase.Equals(d.Name, name));
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000024EC File Offset: 0x000006EC
		private static int RunInstallPowerShellModuleAndReturnExitCode(Program.InstallPowerShellOptions options)
		{
			string destDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WindowsPowerShell\\Modules\\XdeUtils");
			return FileUtils.CopyFileWithConsoleOutput(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modules\\XdeUtils\\*"), destDir);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000252C File Offset: 0x0000072C
		private static int RunCleanDeviceListAndExitCode(Program.CleanDeviceListOptions options)
		{
			try
			{
				XdeAppUtils.CleanCoreconDatabase();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to fully clean up the Visual Studio device list:\r\n\r\n" + ex.Message + "\r\nMake sure you shut down any instances of Visual Studio before trying again.");
				return 1;
			}
			Console.WriteLine("Device list successfully cleaned.");
			return 0;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000257C File Offset: 0x0000077C
		private static int RunListAndReturnExitCode(Program.ListOptions options)
		{
			List<XdeDevice> list = new List<XdeDevice>();
			bool flag = false;
			foreach (XdeDevice xdeDevice in XdeDeviceFactory.GetDevices())
			{
				if (options.Device == null || StringComparer.OrdinalIgnoreCase.Equals(xdeDevice.Name, options.Device))
				{
					if (xdeDevice.IsXdeUIRunning)
					{
						xdeDevice.WaitForConnectionInformation();
						flag = true;
					}
					else if (options.RunningOnly)
					{
						continue;
					}
					list.Add(xdeDevice);
				}
			}
			if (options.Csv)
			{
				Console.WriteLine("Name,SKU,Skin,IPAddress,VHD,VHDSource,IsRunning,NoGPU,UseWMI,DisableStateSep,ShowDisplayName,UseDiffDisk,ID,UapVersion,IsShellReady,MACAddress,UseCheckpoint");
			}
			if (options.RunningOnly && !flag)
			{
				if (!options.Csv)
				{
					Console.WriteLine("No devices are currently running.");
				}
				return 0;
			}
			foreach (XdeDevice xdeDevice2 in list)
			{
				bool isXdeUIRunning = xdeDevice2.IsXdeUIRunning;
				string text = isXdeUIRunning ? xdeDevice2.IpAddress : string.Empty;
				string text2 = (xdeDevice2.Vhd != null) ? xdeDevice2.Vhd : "<not set>";
				DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(xdeDevice2.Vhd);
				string text3 = (downloadedVhdInfo != null) ? downloadedVhdInfo.Source : "<unknown>";
				string text4 = (downloadedVhdInfo != null) ? downloadedVhdInfo.Branch : "<unknown>";
				string text5 = (downloadedVhdInfo != null) ? downloadedVhdInfo.BuildVersion : "<unknown>";
				SkuBuildInfo skuBuildInfoForSku = SkuBuildInfo.GetSkuBuildInfoForSku(xdeDevice2.Sku);
				ImageInfo imageInfo = (skuBuildInfoForSku != null) ? skuBuildInfoForSku.FindInfoForVhdFileName(xdeDevice2.Vhd) : null;
				string text6 = (imageInfo != null) ? imageInfo.Name : "<unknown>";
				if (options.Csv)
				{
					Console.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}", new object[]
					{
						xdeDevice2.Name,
						xdeDevice2.Sku,
						xdeDevice2.Skin,
						text,
						text2,
						text3,
						isXdeUIRunning,
						xdeDevice2.NoGpu,
						xdeDevice2.UseWmi,
						xdeDevice2.DisableStateSep,
						xdeDevice2.ShowDisplayName,
						xdeDevice2.UseDiffDisk,
						xdeDevice2.ID,
						xdeDevice2.UapVersion,
						xdeDevice2.IsShellReady,
						xdeDevice2.MacAddress,
						xdeDevice2.UseCheckpoint
					}));
				}
				else
				{
					XdeSku xdeSku;
					XdeSkin xdeSkin;
					xdeDevice2.GetSkuAndSkin(out xdeSku, out xdeSkin);
					ConsoleColor foregroundColor2;
					ConsoleColor foregroundColor = foregroundColor2 = Console.ForegroundColor;
					string text7 = string.Empty;
					if (isXdeUIRunning)
					{
						if (string.IsNullOrEmpty(text))
						{
							foregroundColor2 = ConsoleColor.Blue;
							text7 = "<No IP address found yet>";
						}
						else
						{
							foregroundColor2 = ConsoleColor.DarkGreen;
							text7 = text;
						}
					}
					Console.ForegroundColor = foregroundColor2;
					Console.WriteLine(xdeDevice2.Name);
					Console.ForegroundColor = foregroundColor;
					string text8 = isXdeUIRunning ? "Running" : "Stopped";
					string text9 = (xdeSku != null) ? ((xdeSku.UseHcsIfAvailable && !xdeDevice2.UseWmi) ? "HCS (not visible in Hyper-V Manager)" : "WMI (visible in Hyper-V Manager)") : string.Empty;
					string text10 = (xdeSku != null) ? xdeSku.ProcessorCount.ToString() : string.Empty;
					string text11 = (xdeSku != null) ? xdeSku.UseHcsIfAvailable.ToString() : string.Empty;
					string text12 = (xdeSku != null) ? xdeSku.UseGpu.ToString() : string.Empty;
					string text13 = (xdeSkin != null) ? string.Format("{0} @ {1}x{2}", xdeSkin.DisplayCount, xdeSkin.DisplayWidth, xdeSkin.DisplayHeight) : string.Empty;
					string text14 = (xdeSkin != null) ? string.Format("{0}", xdeSkin.DisplayScaleFactor) : string.Empty;
					Console.WriteLine(string.Format("  Status\r\n    State           : {0}\r\n    Is shell ready  : {1}\r\n    IP Address      : {2}\r\n    MAC Address     : {3}\r\n    VM type         : {4}\r\n                    \r\n  Options           \r\n    VHD             : {5}\r\n      Source        \r\n         Branch     : {6}\r\n         Build      : {7}\r\n         Image type : {8}\r\n      UseDiffDisk   : {9}\r\n      UseCheckpoint : {10}\r\n    Memory          : {11} MB\r\n    SKU             : {12}\r\n       Processors   : {13}\r\n       Use HCS      : {14}\r\n       Use GPU      : {15}\r\n    Skin            : {16}\r\n       Displays     : {17}\r\n       Scale Factor : {18}\r\n    NoGPU           : {19}\r\n    UseWMI          : {20}\r\n    DisableStateSep : {21}\r\n    ShowDisplayName : {22}", new object[]
					{
						text8,
						xdeDevice2.IsShellReady,
						text7,
						xdeDevice2.MacAddress,
						text9,
						xdeDevice2.Vhd,
						text4,
						text5,
						text6,
						xdeDevice2.UseDiffDisk,
						xdeDevice2.UseCheckpoint,
						xdeDevice2.MemSize,
						xdeDevice2.Sku,
						text10,
						text11,
						text12,
						xdeDevice2.Skin,
						text13,
						text14,
						xdeDevice2.NoGpu,
						xdeDevice2.UseWmi,
						xdeDevice2.DisableStateSep,
						xdeDevice2.ShowDisplayName
					}));
				}
			}
			foreach (XdeDevice xdeDevice3 in list)
			{
				xdeDevice3.Dispose();
			}
			return 0;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002AC4 File Offset: 0x00000CC4
		private static void ShowIndexNumbers(string[] paths)
		{
			Console.WriteLine("Set --index to one of the following values:");
			for (int i = 0; i < paths.Length; i++)
			{
				Console.WriteLine(string.Format("{0}: {1}", i, paths[i]));
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002B04 File Offset: 0x00000D04
		private static string EnsureDiffDiskExists(XdeDevice device)
		{
			string result = device.Vhd;
			if (!string.IsNullOrEmpty(device.DiffDisk))
			{
				if (!File.Exists(device.DiffDisk))
				{
					VhdUtils.CreateDiffDisk(device.Vhd, device.DiffDisk);
				}
				result = device.DiffDisk;
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002B4C File Offset: 0x00000D4C
		private static void ShowDirEntries(WindowsImageVhd vhd, string path, string search, bool recurse)
		{
			bool flag = false;
			bool flag2 = search == "*.*";
			if (flag2)
			{
				Console.WriteLine("\r\n Directory of " + path + "\r\n");
				flag = true;
			}
			List<string> list = new List<string>();
			long num = 0L;
			int num2 = 0;
			int num3 = 0;
			foreach (string text in vhd.OSFileSystem.GetDirectories(path))
			{
				list.Add(text);
				DiscDirectoryInfo directoryInfo = vhd.OSFileSystem.GetDirectoryInfo(text);
				if (flag2)
				{
					Console.WriteLine(string.Format("{0} <DIR> {1}", directoryInfo.CreationTime, directoryInfo.Name));
				}
				num2++;
			}
			foreach (string path2 in vhd.OSFileSystem.GetFiles(path, search))
			{
				if (!flag)
				{
					Console.WriteLine("\r\n Directory of " + path + "\r\n");
					flag = true;
				}
				DiscFileInfo fileInfo = vhd.OSFileSystem.GetFileInfo(path2);
				Console.WriteLine(string.Format("{0} {1} {2}", fileInfo.CreationTime, fileInfo.Length, fileInfo.Name));
				num += fileInfo.Length;
				num3++;
			}
			if (flag)
			{
				Console.WriteLine(string.Format("{0} File(s)   {1} bytes", num3, num));
				Console.WriteLine(string.Format("{0} Dir(s)", num2));
			}
			if (recurse)
			{
				foreach (string path3 in list)
				{
					Program.ShowDirEntries(vhd, path3, search, recurse);
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002D08 File Offset: 0x00000F08
		private static int RunDirAndExitCode(Program.DirOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Can't show files in the image file while the device is running.");
				return 1;
			}
			if (!Path.IsPathRooted(options.Path))
			{
				Console.WriteLine("The file path must be rooted (e.g. \\data\\test, not data\\test)");
				return 1;
			}
			using (WindowsImageVhd windowsImageVhd = WindowsImageVhd.OpenVhd(Program.EnsureDiffDiskExists(xdeDevice), FileAccess.ReadWrite))
			{
				string path;
				string search;
				if (windowsImageVhd.OSFileSystem.DirectoryExists(options.Path))
				{
					path = options.Path;
					search = "*.*";
				}
				else
				{
					path = Path.GetDirectoryName(options.Path);
					search = Path.GetFileName(options.Path);
				}
				Program.ShowDirEntries(windowsImageVhd, path, search, options.Recursive);
			}
			return 0;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002DE0 File Offset: 0x00000FE0
		private static int RunDeleteFileAndExitCode(Program.DeleteFileOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Can't delete files in the image file while the device is running.");
				return 1;
			}
			if (!Path.IsPathRooted(options.Path))
			{
				Console.WriteLine("The file path must be rooted (e.g. \\data\\test\\foo.txt, not foo.txt)");
				return 1;
			}
			using (WindowsImageVhd windowsImageVhd = WindowsImageVhd.OpenVhd(Program.EnsureDiffDiskExists(xdeDevice), FileAccess.ReadWrite))
			{
				int deleted = 0;
				windowsImageVhd.DeleteFiles(options.Path, options.Recursive, delegate(string fileName)
				{
					int deleted = deleted;
					deleted++;
				});
				if (deleted == 0)
				{
					Console.WriteLine("Could not find " + options.Path);
				}
			}
			return 0;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002EB8 File Offset: 0x000010B8
		private static int RunCopyToAndExitCode(Program.CopyToOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Can't copy files to the image file while the device is running.");
				return 1;
			}
			if (options.Dest.Contains("*"))
			{
				Console.WriteLine("Destination can't contain wildcards.");
				return 1;
			}
			string text = Program.EnsureDiffDiskExists(xdeDevice);
			Console.WriteLine("Writing to " + text);
			if (!Path.IsPathRooted(options.Src))
			{
				options.Src = Path.Combine(Environment.CurrentDirectory, options.Src);
			}
			if (!Path.IsPathRooted(options.Dest))
			{
				Console.WriteLine("The desitination path must be rooted (e.g. \\data\\test\\bin, not data\\test\\bin)");
				return 1;
			}
			string text2;
			string fileSpec;
			if (Directory.Exists(options.Src))
			{
				text2 = options.Src;
				fileSpec = "*.*";
			}
			else
			{
				text2 = Path.GetDirectoryName(options.Src);
				fileSpec = Path.GetFileName(options.Src);
			}
			text2 = new DirectoryInfo(text2).FullName;
			using (WindowsImageVhd windowsImageVhd = WindowsImageVhd.OpenVhd(text, FileAccess.ReadWrite))
			{
				int copied = 0;
				windowsImageVhd.CopyDirectoryFromLocalToVhd(text2, fileSpec, options.Dest, options.Recursive, false, delegate(string src, string dest)
				{
					int copied = copied;
					copied++;
					Console.WriteLine(src);
				});
				Console.WriteLine(string.Format("{0} file(s) copied.", copied));
			}
			return 0;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000302C File Offset: 0x0000122C
		private static int RunCopyFromAndExitCode(Program.CopyFromOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Can't copy files from the image file while the device is running.");
				return 1;
			}
			if (options.Dest.Contains("*"))
			{
				Console.WriteLine("Destination can't contain wildcards.");
				return 1;
			}
			string vhdPath = Program.EnsureDiffDiskExists(xdeDevice);
			if (!Path.IsPathRooted(options.Dest))
			{
				options.Dest = Path.Combine(Environment.CurrentDirectory, options.Dest);
			}
			if (!Path.IsPathRooted(options.Src))
			{
				Console.WriteLine("The source path must be rooted (e.g. \\data\\test\\bin, not data\\test\\bin)");
				return 1;
			}
			using (WindowsImageVhd windowsImageVhd = WindowsImageVhd.OpenVhd(vhdPath, FileAccess.ReadWrite))
			{
				string vhdDir;
				string fileSpec;
				if (windowsImageVhd.OSFileSystem.DirectoryExists(options.Src))
				{
					vhdDir = options.Src;
					fileSpec = "*.*";
				}
				else
				{
					vhdDir = Path.GetDirectoryName(options.Src);
					fileSpec = Path.GetFileName(options.Src);
				}
				int copied = 0;
				windowsImageVhd.CopyDirectoryFromVhdToLocal(vhdDir, fileSpec, options.Dest, options.Recursive, delegate(string src, string dest)
				{
					int copied = copied;
					copied++;
					Console.WriteLine(src);
				});
				Console.WriteLine(string.Format("{0} file(s) copied.", copied));
			}
			return 0;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003188 File Offset: 0x00001388
		private static int RunLaunchAndReturnExitCode(Program.LaunchOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Device \"" + xdeDevice.Name + "\" is already running.");
				return 1;
			}
			if (string.IsNullOrEmpty(xdeDevice.Vhd))
			{
				Console.WriteLine("The vhd is not set for this this device. Use --edit to set the vhd.");
				return 1;
			}
			if (!File.Exists(xdeDevice.Vhd))
			{
				Console.WriteLine("The vhd \"" + xdeDevice.Vhd + "\" does not exist. Use --edit to set the vhd.");
				return 1;
			}
			if (string.IsNullOrEmpty(xdeDevice.Sku))
			{
				Console.WriteLine("The sku is not set for this this device. Use --edit to set the sku.");
				return 1;
			}
			if (string.IsNullOrEmpty(xdeDevice.Skin))
			{
				Console.WriteLine("The skin is not set for this this device. Use --edit to set the skin.");
				return 1;
			}
			bool wait = options.Wait;
			Console.WriteLine("Launching emulator for \"" + xdeDevice.Name + "\".");
			if (wait)
			{
				Console.WriteLine("Waiting until the emulator exits...");
			}
			string additionalArgs = (options.Args != null) ? options.Args.Replace('*', '-') : null;
			xdeDevice.Start(wait, additionalArgs);
			return 0;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000032AC File Offset: 0x000014AC
		private static int RunDeleteOptionsAndExitCode(Program.DeleteOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (!xdeDevice.CanDelete)
			{
				Console.WriteLine("Deletion of \"" + options.Device + "\" not supported.");
				return 1;
			}
			if (xdeDevice.IsXdeUIRunning)
			{
				Console.WriteLine("Can't delete \"" + options.Device + "\" while it's running.");
				return 1;
			}
			if (!options.Confirm)
			{
				Console.Write("Delete \"" + xdeDevice.Name + "\"? (Yes/No): ");
				if (!Console.ReadLine().StartsWith("Y", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("Device not deleted.");
					return 0;
				}
			}
			xdeDevice.Delete();
			Console.WriteLine("Device \"" + options.Device + "\" deleted.");
			return 0;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003390 File Offset: 0x00001590
		private static int RunXdeAppOptionsAndExitCode(Program.XdeAppOptions options)
		{
			XdeManagerSettings xdeManagerSettings = XdeManagerSettings.Current;
			if (!string.IsNullOrEmpty(options.DownloadRoot))
			{
				xdeManagerSettings.DownloadRoot = options.DownloadRoot;
				xdeManagerSettings.Save();
				Console.WriteLine("Future downloads will be placed in: " + xdeManagerSettings.DownloadRoot);
				return 0;
			}
			Console.WriteLine("Current image download root: " + xdeManagerSettings.DownloadRoot);
			Console.WriteLine("Use --downloadroot to set the root directory where images are downloaded.");
			return 0;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000033FC File Offset: 0x000015FC
		private static string RunPowershell(string command)
		{
			string result = string.Empty;
			using (Process process = new Process())
			{
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.FileName = "powershell";
				process.StartInfo.Arguments = command;
				process.Start();
				result = process.StandardError.ReadToEnd();
				try
				{
					Console.CursorVisible = false;
				}
				catch (Exception)
				{
				}
				process.StandardError.ReadToEnd();
				process.WaitForExit();
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000034A4 File Offset: 0x000016A4
		private static char GetFirstFreeDrive()
		{
			DriveInfo[] drives = DriveInfo.GetDrives();
			for (char c = 'Z'; c >= 'C'; c -= '\u0001')
			{
				string fullDrive = string.Format("{0}:\\", c);
				if (drives.FirstOrDefault((DriveInfo d) => d.Name == fullDrive) == null)
				{
					return c;
				}
			}
			return '\0';
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000034FC File Offset: 0x000016FC
		private static int RunDismountImageAndReturnExitCode(Program.DismountImageOptions options)
		{
			if (!UacSecurity.IsAdmin())
			{
				Console.WriteLine("Must be in elevated command prompt in order to dismount image.");
				return 1;
			}
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			string str = Program.EnsureDiffDiskExists(xdeDevice);
			string text = "$p = Get-PhysicalDisk | Where-Object {$_.PhysicalLocation -eq '" + str + "'} | Get-VirtualDisk | Where-Object {$_.FriendlyName -eq 'MainOSDisk' } | get-Disk | get-partition; $driveLetter = $p.DriveLetter; $p | Remove-PartitionAccessPath -accesspath \"$driveLetter`:\\\"";
			string value = Program.RunPowershell("-EncodedCommand " + Convert.ToBase64String(Encoding.Unicode.GetBytes(text)));
			text = "Dismount-VHD '" + str + "'";
			value = Program.RunPowershell(text);
			if (!string.IsNullOrEmpty(value))
			{
				Console.WriteLine(value);
				return 1;
			}
			Console.WriteLine("Dismounted " + str + ".");
			return 0;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000035C0 File Offset: 0x000017C0
		private static int RunMountImageAndReturnExitCode(Program.MountImageOptions options)
		{
			if (!UacSecurity.IsAdmin())
			{
				Console.WriteLine("Must be in elevated command prompt in order to mount image.");
				return 1;
			}
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (options.Drive == '*')
			{
				options.Drive = Program.GetFirstFreeDrive();
			}
			string text = Program.EnsureDiffDiskExists(xdeDevice);
			string value = Program.RunPowershell("Mount-VHD '" + text + "'");
			if (!string.IsNullOrEmpty(value))
			{
				Console.WriteLine(value);
				return 1;
			}
			value = Program.RunPowershell(string.Format("$vdisk = Get-PhysicalDisk | Where-Object {{$_.PhysicalLocation -eq '{0}'}} | Get-VirtualDisk | Where-Object {{$_.FriendlyName -eq 'MainOSDisk' }}; $vdisk | Connect-VirtualDisk; $vdisk | get-Disk | get-partition | Set-Partition -NewDriveLetter {1}", text, options.Drive));
			if (!string.IsNullOrEmpty(value))
			{
				Console.WriteLine(value);
				return 1;
			}
			Console.WriteLine(string.Format("{0}:\\ mapped for OS disk on {1}.", options.Drive, text));
			return 0;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003698 File Offset: 0x00001898
		private static int RunKDebugAndReturnExitCode(Program.KDebugOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			switch (xdeDevice.StartKernelDebugger())
			{
			case XdeDevice.StartKernelDebuggerResult.SkippedAlreadyDebugging:
				Console.WriteLine("Skipped starting a new debugger because an existing debugger for the VM was found.");
				return 0;
			case XdeDevice.StartKernelDebuggerResult.NoWindbgFound:
				Console.WriteLine("Could not find an installed debugger.");
				return 1;
			}
			Console.WriteLine("Debugger launched for \"" + xdeDevice.Name + "\".");
			return 0;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003720 File Offset: 0x00001920
		private static int RunDownloadAndReturnExitCode(Program.DownloadOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (!xdeDevice.CanModifyProperty("Vhd"))
			{
				Program.ShowCantModifyProperty("Vhd");
				return 1;
			}
			SkuBuildInfo skuBuildInfoForSku = SkuBuildInfo.GetSkuBuildInfoForSku(xdeDevice.Sku);
			if (skuBuildInfoForSku == null)
			{
				Console.WriteLine("No build information found for sku \"" + xdeDevice.Sku + "\".");
				return 1;
			}
			if (skuBuildInfoForSku.ImageInfos == null || skuBuildInfoForSku.ImageInfos.Count == 0)
			{
				Console.WriteLine("No build paths found for sku \"" + xdeDevice.Sku + "\".");
				return 1;
			}
			ImageInfo imageInfo = null;
			if (options.ImageType == null)
			{
				imageInfo = skuBuildInfoForSku.ImageInfos.First<ImageInfo>();
				Console.WriteLine("Parameter 'imagetype' not used, using default: \"" + imageInfo.Name + "\"");
				Console.WriteLine("Possible imagetype values:");
				using (IEnumerator<ImageInfo> enumerator = skuBuildInfoForSku.ImageInfos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ImageInfo imageInfo2 = enumerator.Current;
						Console.WriteLine(imageInfo2.Name);
					}
					goto IL_1D0;
				}
			}
			imageInfo = skuBuildInfoForSku.ImageInfos.FirstOrDefault((ImageInfo i) => StringComparer.OrdinalIgnoreCase.Equals(i.Name, options.ImageType));
			if (imageInfo == null)
			{
				Console.WriteLine(string.Concat(new string[]
				{
					"No image info found for sku \"",
					xdeDevice.Sku,
					"\" and image type \"",
					options.ImageType,
					"\"."
				}));
				Console.WriteLine("Valid image types are:");
				foreach (ImageInfo imageInfo3 in skuBuildInfoForSku.ImageInfos)
				{
					Console.WriteLine(imageInfo3.Name);
				}
				return 1;
			}
			IL_1D0:
			if (options.Index == null)
			{
				Program.ShowIndexNumbers(DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(options.Branch, imageInfo.Location, 10).ToArray<string>());
				return 1;
			}
			int value = options.Index.Value;
			string text;
			if (value == 0)
			{
				text = DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(options.Branch, imageInfo.Location, 1).FirstOrDefault<string>();
				if (text == null)
				{
					Program.ShowNoDownloadPathsFound(options.Branch, imageInfo);
					return 1;
				}
			}
			else
			{
				string[] array = DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(options.Branch, imageInfo.Location, value + 1).ToArray<string>();
				if (array.Length == 0)
				{
					Program.ShowNoDownloadPathsFound(options.Branch, imageInfo);
					return 1;
				}
				int? index = options.Index;
				int num = array.Length;
				if (index.GetValueOrDefault() >= num & index != null)
				{
					Console.WriteLine(string.Format("--index {0} not found on branch {1}, image type \"{2}\"", options.Index, options.Branch, imageInfo.Name));
					Program.ShowIndexNumbers(array);
					return 1;
				}
				text = array[value];
			}
			string path = Path.Combine(XdeManagerSettings.Current.DownloadRoot, xdeDevice.Name);
			xdeDevice.Vhd = Path.Combine(path, Path.GetFileName(text));
			xdeDevice.Save();
			return Program.CopyVhdSkipIfSame(text, xdeDevice.Vhd);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003A88 File Offset: 0x00001C88
		private static void ShowSkus(IEnumerable<XdeSku> skus)
		{
			foreach (XdeSku xdeSku in skus)
			{
				Console.WriteLine(xdeSku.Name);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003AD4 File Offset: 0x00001CD4
		private static void ShowSkins(XdeSku sku)
		{
			foreach (XdeSkin xdeSkin in sku.Skins)
			{
				Console.WriteLine(xdeSkin.Name);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003B24 File Offset: 0x00001D24
		private static void ShowCantModifyProperty(string propertyName)
		{
			Console.WriteLine("This device does not support changing the '" + propertyName + "' property.");
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003B3C File Offset: 0x00001D3C
		private static int RunEditAndReturnExitCode(Program.EditOptions options)
		{
			XdeDevice device = Program.FindDevice(options.Device);
			if (device == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (options.Name != null)
			{
				if (Program.FindDevice(options.Name) != null)
				{
					Console.WriteLine("A device already exists named \"" + options.Name + "\".");
					return 1;
				}
				if (options.Name != device.Name)
				{
					device.Name = options.Name;
				}
			}
			XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
			if (appInstallXdeInstallation == null)
			{
				Console.WriteLine("The app install version of XDE could not be found.");
				return 1;
			}
			if (options.Sku != null)
			{
				if (!device.CanModifyProperty("Sku"))
				{
					Program.ShowCantModifyProperty("Sku");
					return 1;
				}
				device.Sku = options.Sku;
			}
			if (options.Skin != null)
			{
				if (!device.CanModifyProperty("Skin"))
				{
					Program.ShowCantModifyProperty("Skin");
					return 1;
				}
				XdeSku xdeSku = appInstallXdeInstallation.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, device.Sku));
				if (xdeSku == null)
				{
					Console.WriteLine("Sku \"" + device.Sku + "\" could not be found. Possible skus:");
					Program.ShowSkus(appInstallXdeInstallation.Skus);
					return 1;
				}
				if (xdeSku.FindSkin(options.Skin) == null)
				{
					Console.WriteLine(string.Concat(new string[]
					{
						"Skin \"",
						options.Skin,
						"\" for sku \"",
						device.Sku,
						"\" could not be found. Possible skins:"
					}));
					Program.ShowSkins(xdeSku);
					return 1;
				}
				device.Skin = options.Skin;
			}
			if (options.Vhd != null)
			{
				if (!device.CanModifyProperty("Vhd"))
				{
					Program.ShowCantModifyProperty("Vhd");
					return 1;
				}
				device.Vhd = options.Vhd;
			}
			if (options.NoGpu != null)
			{
				if (!device.CanModifyProperty("NoGpu"))
				{
					Program.ShowCantModifyProperty("NoGpu");
					return 1;
				}
				device.NoGpu = options.NoGpu.Value;
			}
			if (options.ShowDisplayName != null)
			{
				if (!device.CanModifyProperty("ShowDisplayName"))
				{
					Program.ShowCantModifyProperty("ShowDisplayName");
					return 1;
				}
				device.ShowDisplayName = options.ShowDisplayName.Value;
			}
			if (options.DisableStateSep != null && XdeAppUtils.IsInternalVersion)
			{
				if (!device.CanModifyProperty("DisableStateSep"))
				{
					Program.ShowCantModifyProperty("DisableStateSep");
					return 1;
				}
				device.DisableStateSep = options.DisableStateSep.Value;
			}
			if (options.UseWmi != null)
			{
				if (!device.CanModifyProperty("UseWmi"))
				{
					Program.ShowCantModifyProperty("UseWmi");
					return 1;
				}
				device.UseWmi = options.UseWmi.Value;
			}
			if (options.UseDiffDisk != null)
			{
				if (!device.CanModifyProperty("UseDiffDisk"))
				{
					Program.ShowCantModifyProperty("UseDiffDisk");
					return 1;
				}
				device.UseDiffDisk = options.UseDiffDisk.Value;
			}
			if (options.UseCheckpoint != null)
			{
				if (!device.CanModifyProperty("UseCheckpoint"))
				{
					Program.ShowCantModifyProperty("UseCheckpoint");
					return 1;
				}
				device.UseCheckpoint = options.UseCheckpoint.Value;
			}
			bool flag = false;
			if (device.IsDirty)
			{
				device.Save();
				Console.WriteLine("Device \"" + device.Name + "\" saved.");
			}
			else if (!flag)
			{
				Console.WriteLine("No properties changed, so the device \"" + device.Name + "\" was not saved.");
			}
			return 0;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003F54 File Offset: 0x00002154
		private static int RunCreateAndReturnExitCode(Program.CreateOptions options)
		{
			if (options.Name != null && Program.FindDevice(options.Name) != null)
			{
				Console.WriteLine("A device already exists named \"" + options.Name + "\".");
				return 1;
			}
			XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
			if (appInstallXdeInstallation == null)
			{
				Console.WriteLine("The app install version of XDE could not be found.");
				return 1;
			}
			if (options.Sku == null)
			{
				Console.WriteLine("--sku is required. Possible skus:");
				Program.ShowSkus(appInstallXdeInstallation.Skus);
				return 1;
			}
			XdeSku xdeSku = appInstallXdeInstallation.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, options.Sku));
			if (xdeSku == null)
			{
				Console.WriteLine("Sku \"" + options.Sku + "\" could not be found. Possible skus:");
				Program.ShowSkus(appInstallXdeInstallation.Skus);
				return 1;
			}
			if (options.Skin != null && xdeSku.FindSkin(options.Skin) == null)
			{
				Console.WriteLine(string.Concat(new string[]
				{
					"Skin \"",
					options.Skin,
					"\" for sku \"",
					xdeSku.Name,
					"\" could not be found. Possible skins:"
				}));
				Program.ShowSkins(xdeSku);
				return 1;
			}
			XdeDevice xdeDevice = appInstallXdeInstallation.CreateDeviceForSku(xdeSku);
			if (options.Name == null)
			{
				Console.WriteLine("--device not specified. Using default name \"" + xdeDevice.Name + "\".");
			}
			else
			{
				xdeDevice.Name = options.Name;
			}
			if (options.Skin == null)
			{
				Console.WriteLine("--skin not specified. Using default skin \"" + xdeDevice.Skin + "\".");
			}
			else
			{
				xdeDevice.Skin = options.Skin;
			}
			if (options.Vhd != null)
			{
				xdeDevice.Vhd = options.Vhd;
			}
			else
			{
				Console.WriteLine("--vhd not specified. Use the 'download' command to get a vhd from a build share.");
			}
			xdeDevice.Save();
			Console.WriteLine("New device \"" + xdeDevice.Name + "\" created.");
			return 0;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000415C File Offset: 0x0000235C
		private static void ShowNoDownloadPathsFound(string branch, ImageInfo imageInfo)
		{
			Console.WriteLine(string.Concat(new string[]
			{
				"No built images found for ",
				branch,
				", image type \"",
				imageInfo.Name,
				"\""
			}));
			Console.WriteLine("Images expected at \\\\winbuilds\\release\\" + branch + "\\<build string>\\" + imageInfo.Location);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000041BC File Offset: 0x000023BC
		private static int CopyVhdSkipIfSame(string source, string dest)
		{
			dest = XdeAppUtils.GetUnvirtualizedPath(dest);
			if (!File.Exists(source))
			{
				Console.WriteLine("The source file \"" + source + "\" does not exist.");
				return 1;
			}
			if (FileUtils.AreFilesSame(source, dest))
			{
				Console.WriteLine("Skipping download of file because sizes and write times are identical.");
				return 0;
			}
			int num = FileUtils.CopyFileWithConsoleOutput(source, Path.GetDirectoryName(dest));
			if (num == 0)
			{
				FileUtils.GrantHyperVRightsForFile(dest);
				DownloadedVhdInfo.RecordDownloadedVhdInfo(source, dest);
			}
			return num;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004224 File Offset: 0x00002424
		private static int RunResetAndReturnExitCode(Program.ResetOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			Program.AskDeleteDeviceFiles(xdeDevice);
			return 0;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00004263 File Offset: 0x00002463
		private static void AskDeleteDeviceFiles(XdeDevice device)
		{
			if (Program.AskQuestion("Delete diff disk and checkpoints for '" + device.Name + "'? (Yes/No): "))
			{
				device.DeleteVirtualMachine();
				Console.WriteLine("Device reset.");
				return;
			}
			Console.WriteLine("Diff disk and checkpoints not deleted.");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000429C File Offset: 0x0000249C
		private static int RunRedownloadAndReturnExitCode(Program.RedownloadOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (!xdeDevice.CanModifyProperty("Vhd"))
			{
				Program.ShowCantModifyProperty("Vhd");
				return 1;
			}
			if (string.IsNullOrEmpty(xdeDevice.Vhd))
			{
				Console.WriteLine("The vhd is not set for this this device. Use --edit to set the vhd.");
				return 1;
			}
			DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(xdeDevice.Vhd);
			if (downloadedVhdInfo == null)
			{
				Console.WriteLine("The VHD can't be re-downloaded because the original source location isn't known.");
				return 1;
			}
			if (Program.CopyVhdSkipIfSame(downloadedVhdInfo.Source, xdeDevice.Vhd) == 0 && FileUtils.AreFilesSame(downloadedVhdInfo.Source, xdeDevice.Vhd))
			{
				Console.WriteLine("You can reset the device so the next boot will start fresh with a new state.");
				Program.AskDeleteDeviceFiles(xdeDevice);
				return 0;
			}
			return 1;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000435C File Offset: 0x0000255C
		private static bool AskQuestion(string text)
		{
			for (;;)
			{
				Console.Write(text);
				string text2 = Console.ReadLine();
				if (text2.Length >= 1)
				{
					char c = text2.ToUpperInvariant()[0];
					if (c == 'Y')
					{
						break;
					}
					if (c == 'N')
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004398 File Offset: 0x00002598
		private static int RunGetLatestAndReturnExitCode(Program.GetLatestOptions options)
		{
			XdeDevice xdeDevice = Program.FindDevice(options.Device);
			if (xdeDevice == null)
			{
				Console.WriteLine("No device found named \"" + options.Device + "\".");
				return 1;
			}
			if (!xdeDevice.CanModifyProperty("Vhd"))
			{
				Program.ShowCantModifyProperty("Vhd");
				return 1;
			}
			if (string.IsNullOrEmpty(xdeDevice.Vhd))
			{
				Console.WriteLine("No VHD has been set for '\"" + options.Device + "\".");
				return 1;
			}
			DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(xdeDevice.Vhd);
			if (downloadedVhdInfo == null)
			{
				Console.WriteLine("The latest build can't be found because the original source location isn't known.");
				return 1;
			}
			string latestVhdFileNameFromSource = downloadedVhdInfo.GetLatestVhdFileNameFromSource();
			string vhd = xdeDevice.Vhd;
			return Program.CopyVhdSkipIfSame(latestVhdFileNameFromSource, vhd);
		}

		// Token: 0x02000003 RID: 3
		[Verb("cleanvsdevicelist", HelpText = "Clean up the Visual Studio device list.\r\nThis is useful if the target device dropdown list is missing emulator devices.")]
		private class CleanDeviceListOptions
		{
		}

		// Token: 0x02000004 RID: 4
		[Verb("installps", HelpText = "Install XDE PowerShell module.")]
		private class InstallPowerShellOptions
		{
		}

		// Token: 0x02000005 RID: 5
		[Verb("mount", HelpText = "Mount device image.")]
		private class MountImageOptions
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000025 RID: 37 RVA: 0x00004458 File Offset: 0x00002658
			// (set) Token: 0x06000026 RID: 38 RVA: 0x00004460 File Offset: 0x00002660
			[Option("drive", HelpText = "Drive to map.", Default = '*')]
			public char Drive { get; set; }

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000027 RID: 39 RVA: 0x00004469 File Offset: 0x00002669
			// (set) Token: 0x06000028 RID: 40 RVA: 0x00004471 File Offset: 0x00002671
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000006 RID: 6
		[Verb("dismount", HelpText = "Dismount device image.")]
		private class DismountImageOptions
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600002A RID: 42 RVA: 0x00004482 File Offset: 0x00002682
			// (set) Token: 0x0600002B RID: 43 RVA: 0x0000448A File Offset: 0x0000268A
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000007 RID: 7
		[Verb("copyto", HelpText = "Copy desktop file(s) to device image.")]
		private class CopyToOptions
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600002D RID: 45 RVA: 0x0000449B File Offset: 0x0000269B
			// (set) Token: 0x0600002E RID: 46 RVA: 0x000044A3 File Offset: 0x000026A3
			[Option("src", Default = false, HelpText = "Source file or directory.", Required = true)]
			public string Src { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600002F RID: 47 RVA: 0x000044AC File Offset: 0x000026AC
			// (set) Token: 0x06000030 RID: 48 RVA: 0x000044B4 File Offset: 0x000026B4
			[Option("dest", Default = false, HelpText = "Destination file or directory.", Required = true)]
			public string Dest { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000031 RID: 49 RVA: 0x000044BD File Offset: 0x000026BD
			// (set) Token: 0x06000032 RID: 50 RVA: 0x000044C5 File Offset: 0x000026C5
			[Option('r', "recursive", Default = false, HelpText = "Recursive copy.")]
			public bool Recursive { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000033 RID: 51 RVA: 0x000044CE File Offset: 0x000026CE
			// (set) Token: 0x06000034 RID: 52 RVA: 0x000044D6 File Offset: 0x000026D6
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000008 RID: 8
		[Verb("copyfrom", HelpText = "Copy image file(s) to desktop.")]
		private class CopyFromOptions
		{
			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000036 RID: 54 RVA: 0x000044E7 File Offset: 0x000026E7
			// (set) Token: 0x06000037 RID: 55 RVA: 0x000044EF File Offset: 0x000026EF
			[Option("src", Default = false, HelpText = "Source file or directory.", Required = true)]
			public string Src { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000038 RID: 56 RVA: 0x000044F8 File Offset: 0x000026F8
			// (set) Token: 0x06000039 RID: 57 RVA: 0x00004500 File Offset: 0x00002700
			[Option("dest", Default = false, HelpText = "Destination file or directory.", Required = true)]
			public string Dest { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x0600003A RID: 58 RVA: 0x00004509 File Offset: 0x00002709
			// (set) Token: 0x0600003B RID: 59 RVA: 0x00004511 File Offset: 0x00002711
			[Option('r', "recursive", Default = false, HelpText = "Recursive copy.")]
			public bool Recursive { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x0600003C RID: 60 RVA: 0x0000451A File Offset: 0x0000271A
			// (set) Token: 0x0600003D RID: 61 RVA: 0x00004522 File Offset: 0x00002722
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000009 RID: 9
		[Verb("dir", HelpText = "Show directories and files on a device image.")]
		private class DirOptions
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600003F RID: 63 RVA: 0x00004533 File Offset: 0x00002733
			// (set) Token: 0x06000040 RID: 64 RVA: 0x0000453B File Offset: 0x0000273B
			[Option("path", HelpText = "Path to view.", Required = true)]
			public string Path { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000041 RID: 65 RVA: 0x00004544 File Offset: 0x00002744
			// (set) Token: 0x06000042 RID: 66 RVA: 0x0000454C File Offset: 0x0000274C
			[Option('r', "recursive", Default = false, HelpText = "Recursive copy.")]
			public bool Recursive { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000043 RID: 67 RVA: 0x00004555 File Offset: 0x00002755
			// (set) Token: 0x06000044 RID: 68 RVA: 0x0000455D File Offset: 0x0000275D
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x0200000A RID: 10
		[Verb("del", HelpText = "Delete files from device image.")]
		private class DeleteFileOptions
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000046 RID: 70 RVA: 0x0000456E File Offset: 0x0000276E
			// (set) Token: 0x06000047 RID: 71 RVA: 0x00004576 File Offset: 0x00002776
			[Option("path", HelpText = "Path to delete.", Required = true)]
			public string Path { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000048 RID: 72 RVA: 0x0000457F File Offset: 0x0000277F
			// (set) Token: 0x06000049 RID: 73 RVA: 0x00004587 File Offset: 0x00002787
			[Option('r', "recursive", Default = false, HelpText = "Recursive copy.")]
			public bool Recursive { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600004A RID: 74 RVA: 0x00004590 File Offset: 0x00002790
			// (set) Token: 0x0600004B RID: 75 RVA: 0x00004598 File Offset: 0x00002798
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x0200000B RID: 11
		[Verb("list", HelpText = "List emulator devices.")]
		private class ListOptions
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600004D RID: 77 RVA: 0x000045A9 File Offset: 0x000027A9
			// (set) Token: 0x0600004E RID: 78 RVA: 0x000045B1 File Offset: 0x000027B1
			[Option('c', "csv", Default = false, HelpText = "Format output as CSV values.")]
			public bool Csv { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600004F RID: 79 RVA: 0x000045BA File Offset: 0x000027BA
			// (set) Token: 0x06000050 RID: 80 RVA: 0x000045C2 File Offset: 0x000027C2
			[Option('r', "running", Default = false, HelpText = "Only show running devices.")]
			public bool RunningOnly { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000051 RID: 81 RVA: 0x000045CB File Offset: 0x000027CB
			// (set) Token: 0x06000052 RID: 82 RVA: 0x000045D3 File Offset: 0x000027D3
			[Option('d', "device", HelpText = "Limit to the emulator device with a specific name.")]
			public string Device { get; set; }
		}

		// Token: 0x0200000C RID: 12
		[Verb("delete", HelpText = "Delete an emulator device.")]
		private class DeleteOptions
		{
			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000054 RID: 84 RVA: 0x000045E4 File Offset: 0x000027E4
			// (set) Token: 0x06000055 RID: 85 RVA: 0x000045EC File Offset: 0x000027EC
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000056 RID: 86 RVA: 0x000045F5 File Offset: 0x000027F5
			// (set) Token: 0x06000057 RID: 87 RVA: 0x000045FD File Offset: 0x000027FD
			[Option('y', "yes", Default = false, HelpText = "Don't prompt for confirmation.")]
			public bool Confirm { get; set; }
		}

		// Token: 0x0200000D RID: 13
		[Verb("redownload", HelpText = "Re-download the currently downloaded image from the original source.")]
		private class RedownloadOptions
		{
			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000059 RID: 89 RVA: 0x0000460E File Offset: 0x0000280E
			// (set) Token: 0x0600005A RID: 90 RVA: 0x00004616 File Offset: 0x00002816
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x0200000E RID: 14
		[Verb("reset", HelpText = "Reset the device state by deleting the diff disk.")]
		private class ResetOptions
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600005C RID: 92 RVA: 0x00004627 File Offset: 0x00002827
			// (set) Token: 0x0600005D RID: 93 RVA: 0x0000462F File Offset: 0x0000282F
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x0200000F RID: 15
		[Verb("getlatest", HelpText = "Get the most recently-built image from the original source.")]
		private class GetLatestOptions
		{
			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600005F RID: 95 RVA: 0x00004640 File Offset: 0x00002840
			// (set) Token: 0x06000060 RID: 96 RVA: 0x00004648 File Offset: 0x00002848
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000010 RID: 16
		[Verb("download", HelpText = "Download a VHD from a build share.")]
		private class DownloadOptions
		{
			// Token: 0x1700001A RID: 26
			// (get) Token: 0x06000062 RID: 98 RVA: 0x00004659 File Offset: 0x00002859
			// (set) Token: 0x06000063 RID: 99 RVA: 0x00004661 File Offset: 0x00002861
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x06000064 RID: 100 RVA: 0x0000466A File Offset: 0x0000286A
			// (set) Token: 0x06000065 RID: 101 RVA: 0x00004672 File Offset: 0x00002872
			[Option('b', "branch", HelpText = "The branch to download from.", Required = true)]
			public string Branch { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000066 RID: 102 RVA: 0x0000467B File Offset: 0x0000287B
			// (set) Token: 0x06000067 RID: 103 RVA: 0x00004683 File Offset: 0x00002883
			[Option('t', "imagetype", HelpText = "The type of image to download for the device's current sku.")]
			public string ImageType { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000068 RID: 104 RVA: 0x0000468C File Offset: 0x0000288C
			// (set) Token: 0x06000069 RID: 105 RVA: 0x00004694 File Offset: 0x00002894
			[Option('i', "index", HelpText = "The index number of the image to download.")]
			public int? Index { get; set; }
		}

		// Token: 0x02000011 RID: 17
		[Verb("create", HelpText = "Create an emulator device.")]
		private class CreateOptions
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x0600006B RID: 107 RVA: 0x000046A5 File Offset: 0x000028A5
			// (set) Token: 0x0600006C RID: 108 RVA: 0x000046AD File Offset: 0x000028AD
			[Option('n', "name", HelpText = "The name to use for the new device.")]
			public string Name { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x0600006D RID: 109 RVA: 0x000046B6 File Offset: 0x000028B6
			// (set) Token: 0x0600006E RID: 110 RVA: 0x000046BE File Offset: 0x000028BE
			[Option('s', "sku", HelpText = "The emulator sku to use.")]
			public string Sku { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x0600006F RID: 111 RVA: 0x000046C7 File Offset: 0x000028C7
			// (set) Token: 0x06000070 RID: 112 RVA: 0x000046CF File Offset: 0x000028CF
			[Option('k', "skin", HelpText = "The skin to use with for the given sku.")]
			public string Skin { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x06000071 RID: 113 RVA: 0x000046D8 File Offset: 0x000028D8
			// (set) Token: 0x06000072 RID: 114 RVA: 0x000046E0 File Offset: 0x000028E0
			[Option('v', "vhd", HelpText = "The vhd to use for the device.")]
			public string Vhd { get; set; }
		}

		// Token: 0x02000012 RID: 18
		[Verb("edit", HelpText = "Edit an emulator device")]
		private class EditOptions
		{
			// Token: 0x17000022 RID: 34
			// (get) Token: 0x06000074 RID: 116 RVA: 0x000046F1 File Offset: 0x000028F1
			// (set) Token: 0x06000075 RID: 117 RVA: 0x000046F9 File Offset: 0x000028F9
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x06000076 RID: 118 RVA: 0x00004702 File Offset: 0x00002902
			// (set) Token: 0x06000077 RID: 119 RVA: 0x0000470A File Offset: 0x0000290A
			[Option('n', "name", HelpText = "The new name to use for the device.")]
			public string Name { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00004713 File Offset: 0x00002913
			// (set) Token: 0x06000079 RID: 121 RVA: 0x0000471B File Offset: 0x0000291B
			[Option('s', "sku", HelpText = "The emulator sku to use.")]
			public string Sku { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x0600007A RID: 122 RVA: 0x00004724 File Offset: 0x00002924
			// (set) Token: 0x0600007B RID: 123 RVA: 0x0000472C File Offset: 0x0000292C
			[Option('k', "skin", HelpText = "The skin to use with for the given sku.")]
			public string Skin { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x0600007C RID: 124 RVA: 0x00004735 File Offset: 0x00002935
			// (set) Token: 0x0600007D RID: 125 RVA: 0x0000473D File Offset: 0x0000293D
			[Option('v', "vhd", HelpText = "The vhd to use for the device.")]
			public string Vhd { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x0600007E RID: 126 RVA: 0x00004746 File Offset: 0x00002946
			// (set) Token: 0x0600007F RID: 127 RVA: 0x0000474E File Offset: 0x0000294E
			[Option("nogpu", HelpText = "Don't use the desktop GPU with this device.")]
			public bool? NoGpu { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000080 RID: 128 RVA: 0x00004757 File Offset: 0x00002957
			// (set) Token: 0x06000081 RID: 129 RVA: 0x0000475F File Offset: 0x0000295F
			[Option("disablestatesep", HelpText = "Disable state separation.")]
			public bool? DisableStateSep { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000082 RID: 130 RVA: 0x00004768 File Offset: 0x00002968
			// (set) Token: 0x06000083 RID: 131 RVA: 0x00004770 File Offset: 0x00002970
			[Option("showdisplayname", HelpText = "Show the display name on the emulator window.")]
			public bool? ShowDisplayName { get; set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000084 RID: 132 RVA: 0x00004779 File Offset: 0x00002979
			// (set) Token: 0x06000085 RID: 133 RVA: 0x00004781 File Offset: 0x00002981
			[Option("usewmi", HelpText = "Use WMI for VM operations.")]
			public bool? UseWmi { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000086 RID: 134 RVA: 0x0000478A File Offset: 0x0000298A
			// (set) Token: 0x06000087 RID: 135 RVA: 0x00004792 File Offset: 0x00002992
			[Option("usediffdisk", HelpText = "Use a diff disk to keep the main VHD from being modified.")]
			public bool? UseDiffDisk { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000088 RID: 136 RVA: 0x0000479B File Offset: 0x0000299B
			// (set) Token: 0x06000089 RID: 137 RVA: 0x000047A3 File Offset: 0x000029A3
			[Option("usecheckpoint", HelpText = "Create or boot to a checkpoint.")]
			public bool? UseCheckpoint { get; set; }
		}

		// Token: 0x02000013 RID: 19
		[Verb("launch", HelpText = "Launch an emulator.")]
		private class LaunchOptions
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600008B RID: 139 RVA: 0x000047B4 File Offset: 0x000029B4
			// (set) Token: 0x0600008C RID: 140 RVA: 0x000047BC File Offset: 0x000029BC
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x0600008D RID: 141 RVA: 0x000047C5 File Offset: 0x000029C5
			// (set) Token: 0x0600008E RID: 142 RVA: 0x000047CD File Offset: 0x000029CD
			[Option('w', "wait", HelpText = "Wait until xde.exe exits.", Default = false)]
			public bool Wait { get; set; }

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x0600008F RID: 143 RVA: 0x000047D6 File Offset: 0x000029D6
			// (set) Token: 0x06000090 RID: 144 RVA: 0x000047DE File Offset: 0x000029DE
			[Option("args", HelpText = "Additional arguments to pass to XDE. (Use '*' instead of '-' for options.)")]
			public string Args { get; set; }
		}

		// Token: 0x02000014 RID: 20
		[Verb("kdebug", HelpText = "Launch the kernel debugger for an emulator.")]
		private class KDebugOptions
		{
			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000092 RID: 146 RVA: 0x000047EF File Offset: 0x000029EF
			// (set) Token: 0x06000093 RID: 147 RVA: 0x000047F7 File Offset: 0x000029F7
			[Option('d', "device", HelpText = "The emulator device name.", Required = true)]
			public string Device { get; set; }
		}

		// Token: 0x02000015 RID: 21
		[Verb("options", HelpText = "Show or set global options.")]
		private class XdeAppOptions
		{
			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000095 RID: 149 RVA: 0x00004808 File Offset: 0x00002A08
			// (set) Token: 0x06000096 RID: 150 RVA: 0x00004810 File Offset: 0x00002A10
			[Option('r', "downloadroot", HelpText = "The directory to use as the base for where images are download to.")]
			public string DownloadRoot { get; set; }
		}
	}
}
