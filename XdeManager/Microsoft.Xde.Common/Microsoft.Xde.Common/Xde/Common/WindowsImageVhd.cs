using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using DiscUtils;
using DiscUtils.Ntfs;
using DiscUtils.Partitions;
using DiscUtils.Raw;
using DiscUtils.Registry;
using DiscUtils.Setup;
using DiscUtils.Streams;
using DiscUtils.Vhd;
using DiscUtils.Vhdx;
using Microsoft.Spaces.Diskstream;
using Microsoft.Win32;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006E RID: 110
	public class WindowsImageVhd : IDisposable
	{
		// Token: 0x0600027C RID: 636 RVA: 0x00005738 File Offset: 0x00003938
		static WindowsImageVhd()
		{
			SetupHelper.RegisterAssembly(typeof(NtfsFileSystem).Assembly);
			SetupHelper.RegisterAssembly(typeof(DiscUtils.Vhd.Disk).Assembly);
			SetupHelper.RegisterAssembly(typeof(DiscUtils.Vhdx.Disk).Assembly);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00005890 File Offset: 0x00003A90
		private WindowsImageVhd(string vhdPath, FileAccess fileAccess)
		{
			this.mainfileSystem = this.GetOSPartitionFileSystemViaDiscUtils(vhdPath, fileAccess);
			if (this.mainfileSystem == null)
			{
				this.GetOSPartitionFileSystemViaStorageSpaces(vhdPath, out this.mainfileSystem, out this.bootFileSystem);
			}
			if (this.mainfileSystem == null)
			{
				throw new ObjectNotFoundException("The OS partition could not be found in the VHD. The VHD could be invalid or corrupt.");
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000058FF File Offset: 0x00003AFF
		public IFileSystem OSFileSystem
		{
			get
			{
				return this.mainfileSystem;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00005907 File Offset: 0x00003B07
		public static WindowsImageVhd OpenVhd(string vhdPath, FileAccess fileAccess)
		{
			return new WindowsImageVhd(vhdPath, fileAccess);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00005910 File Offset: 0x00003B10
		public void ApplyPackage(TextReader packageReader)
		{
			this.ApplyPackage(packageReader, null);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000591C File Offset: 0x00003B1C
		public void ApplyPackage(TextReader packageReader, string languageFilter)
		{
			XContainer xcontainer = XDocument.Load(packageReader);
			string xelementName = WindowsImageVhd.GetXElementName("RegKeys", "urn:Microsoft.WindowsPhone/PackageSchema.v8.00");
			string lookingFor = (languageFilter == null) ? null : ("(" + languageFilter + ")");
			XElement xelement = (from item in xcontainer.Descendants(xelementName)
			where lookingFor == null || item.Attribute("Language").Value == lookingFor
			select item).FirstOrDefault<XElement>();
			if (xelement != null)
			{
				foreach (RegKey regKey in from key in ((RegKeys)new XmlSerializer(typeof(RegKeys), "urn:Microsoft.WindowsPhone/PackageSchema.v8.00").Deserialize(xelement.CreateReader())).Keys
				where key.Values != null
				select key)
				{
					DiscUtils.Registry.RegistryKey registryKey = this.OpenOrCreateRegistryKey(regKey, false);
					foreach (RegValue regValue in regKey.Values)
					{
						registryKey.SetValue(regValue.RegValueName, regValue.TypedValue);
					}
				}
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00005A68 File Offset: 0x00003C68
		public void ApplySku(IXdeSku sku, SkinDisplay displayInfo, bool ignoreScaleDirectives)
		{
			if (sku.Keys != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["$DisplayScaleFactor"] = displayInfo.DisplayScaleFactor.ToString("X2");
				dictionary["$SingleDisplay"] = ((displayInfo.DisplayCount == 1) ? "1" : "0");
				foreach (SkuRegKey skuRegKey in from s in sku.Keys
				where s.Values != null
				select s)
				{
					RegKey regKey = new RegKey();
					regKey.KeyName = Path.Combine(this.GetAliasFromRoot(skuRegKey.Root), skuRegKey.Path);
					if (!ignoreScaleDirectives || !regKey.KeyName.EndsWith("Microsoft\\Windows\\CurrentVersion\\Explorer\\Scaling", StringComparison.OrdinalIgnoreCase))
					{
						foreach (object obj in Enum.GetValues(typeof(ContainerFlags)))
						{
							ContainerFlags containerFlags = (ContainerFlags)obj;
							if ((skuRegKey.ContainerFlags & containerFlags) != ContainerFlags.None && (containerFlags != ContainerFlags.Container || this.containerSystemBaseDir != null))
							{
								DiscUtils.Registry.RegistryKey registryKey;
								try
								{
									registryKey = this.OpenOrCreateRegistryKey(regKey, containerFlags == ContainerFlags.Container);
								}
								catch (Exception)
								{
									continue;
								}
								foreach (SkuRegValue skuRegValue in skuRegKey.Values)
								{
									RegValue regValue = new RegValue();
									regValue.Name = skuRegValue.Name;
									regValue.Type = skuRegValue.Type;
									regValue.Value = skuRegValue.Data;
									string value;
									if (regValue.Value.StartsWith("$") && dictionary.TryGetValue(regValue.Value, out value))
									{
										regValue.Value = value;
									}
									if (skuRegValue.OverrideFromKey != null)
									{
										object value2 = Registry.GetValue(skuRegValue.OverrideFromKey, skuRegValue.Name, skuRegValue.Data);
										if (value2 != null)
										{
											regValue.Value = value2.ToString();
										}
									}
									registryKey.SetValue(regValue.RegValueName, regValue.TypedValue);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public string GetAliasFromRoot(SkuRegRoots root)
		{
			switch (root)
			{
			case SkuRegRoots.HKCU:
				return "$(hkcu.root)";
			case SkuRegRoots.HKU_DEFAULT:
				return "$(hkuser.default)";
			case SkuRegRoots.HKLM_MICROSOFT:
				return "$(hklm.microsoft)";
			case SkuRegRoots.HKLM_SYSTEM:
				return "$(hklm.system)";
			case SkuRegRoots.HKLM_SOFTWARE:
				return "$(hklm.software)";
			default:
				throw new ArgumentException(Strings.InvalidParameterValue, "root");
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00005D4C File Offset: 0x00003F4C
		public DiscUtils.Registry.RegistryKey GetRegistryKeyFromAlias(string aliasName, bool container)
		{
			WindowsImageVhd.RegistryAliasData registryAliasData = (from a in WindowsImageVhd.RegistryAliasInfos
			where a.Alias == aliasName
			select a).FirstOrDefault<WindowsImageVhd.RegistryAliasData>();
			if (registryAliasData == null)
			{
				throw new ArgumentOutOfRangeException("aliasName");
			}
			string fileName;
			if (container)
			{
				if (string.IsNullOrEmpty(this.containerSystemBaseDir))
				{
					return null;
				}
				fileName = this.containerSystemBaseDir + registryAliasData.FileName;
			}
			else
			{
				fileName = registryAliasData.FileName;
			}
			DiscUtils.Registry.RegistryHive registryHiveFromFileName = this.GetRegistryHiveFromFileName(fileName);
			DiscUtils.Registry.RegistryKey registryKey;
			if (registryAliasData.KeyPath == null)
			{
				registryKey = registryHiveFromFileName.Root;
			}
			else
			{
				registryKey = registryHiveFromFileName.Root.OpenSubKey(registryAliasData.KeyPath);
				if (registryKey == null)
				{
					registryKey = registryHiveFromFileName.Root.CreateSubKey(registryAliasData.KeyPath);
				}
			}
			return registryKey;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00005E04 File Offset: 0x00004004
		public void DeleteFiles(string fileName, bool recursive, WindowsImageVhd.SingleFileFunc fileFunc)
		{
			string directoryName = Path.GetDirectoryName(fileName);
			string fileName2 = Path.GetFileName(fileName);
			foreach (string text in this.OSFileSystem.GetFiles(directoryName, fileName2, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
				this.OSFileSystem.DeleteFile(text);
				fileFunc(text);
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00005E5C File Offset: 0x0000405C
		public void CopyDirectoryFromLocalToVhd(string localDir, string fileSpec, string destDir, bool recursive, bool useHashFile, WindowsImageVhd.CopyFileFunc copyFileFunc)
		{
			this.CopyDirectoryFromLocalToVhdImpl(localDir, fileSpec, destDir, true, recursive, useHashFile, copyFileFunc);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00005E6E File Offset: 0x0000406E
		public void CopyDirectoryFromVhdToLocal(string vhdDir, string fileSpec, string localDir, bool recursive, WindowsImageVhd.CopyFileFunc copyFileFunc)
		{
			this.CopyDirectoryFromVhdToLocalImpl(vhdDir, fileSpec, localDir, recursive, copyFileFunc);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00005E7D File Offset: 0x0000407D
		public void CopyDirectoryFromLocalToVhd(string localDir, string destDir)
		{
			this.CopyDirectoryFromLocalToVhdImpl(localDir, "*.*", destDir, true, true, true, null);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00005E90 File Offset: 0x00004090
		public bool EnableKernelDebugger(IXdeVirtualMachine vm)
		{
			if (this.bootFileSystem == null)
			{
				return false;
			}
			string path = "\\EFI\\Microsoft\\Boot\\BCD";
			if (!this.bootFileSystem.Exists(path))
			{
				path = "\\Boot\\BCD";
				if (!this.bootFileSystem.Exists(path))
				{
					return false;
				}
			}
			string value;
			int num;
			XdeAppUtils.GetKernelDebuggerSettingsForVmName(vm.Name, out value, out num);
			using (TempFile tempFile = new TempFile())
			{
				using (Stream stream = new FileStream(tempFile.FileName, FileMode.Truncate, FileAccess.ReadWrite))
				{
					using (Stream stream2 = this.bootFileSystem.OpenFile(path, FileMode.Open, FileAccess.Read))
					{
						stream2.CopyTo(stream);
						using (DiscUtils.Registry.RegistryHive registryHive = new DiscUtils.Registry.RegistryHive(stream, Ownership.None))
						{
							string localIPAddress = WindowsImageVhd.GetLocalIPAddress();
							registryHive.Root.CreateSubKey("Objects\\{7619dcc9-fafe-11d9-b411-000476eba25f}\\Elements\\260000a0").SetValue("Element", WindowsImageVhd.TrueBin);
							registryHive.Root.CreateSubKey("Objects\\{7619dcc9-fafe-11d9-b411-000476eba25f}\\Elements\\16000049").SetValue("Element", WindowsImageVhd.TrueBin);
							registryHive.Root.CreateSubKey("Objects\\{4636856e-540f-4170-a130-a84776f4c654}\\Elements\\1600001c").SetValue("Element", WindowsImageVhd.TrueBin);
							registryHive.Root.CreateSubKey("Objects\\{4636856e-540f-4170-a130-a84776f4c654}\\Elements\\15000011").SetValue("Element", WindowsImageVhd.GetLongBytes(3L));
							registryHive.Root.CreateSubKey("Objects\\{4636856e-540f-4170-a130-a84776f4c654}\\Elements\\1200001d").SetValue("Element", value);
							registryHive.Root.CreateSubKey("Objects\\{4636856e-540f-4170-a130-a84776f4c654}\\Elements\\1500001a").SetValue("Element", WindowsImageVhd.GetIPAddressBytes(localIPAddress));
							registryHive.Root.CreateSubKey("Objects\\{4636856e-540f-4170-a130-a84776f4c654}\\Elements\\1500001b").SetValue("Element", WindowsImageVhd.GetLongBytes((long)num));
						}
					}
				}
				using (Stream stream3 = new FileStream(tempFile.FileName, FileMode.Open, FileAccess.Read))
				{
					using (Stream stream4 = this.bootFileSystem.OpenFile(path, FileMode.Truncate))
					{
						stream3.CopyTo(stream4);
					}
				}
			}
			return true;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00006108 File Offset: 0x00004308
		public void DisableStateSeparation(bool disabled)
		{
			RegKey regKey = new RegKey
			{
				KeyName = "$(hklm.control)\\StateSeparation\\Policy"
			};
			this.OpenOrCreateRegistryKey(regKey, false).SetValue("DevelopmentMode", disabled ? 1 : 0);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00006144 File Offset: 0x00004344
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.DisposeHives();
			DiscFileSystem discFileSystem = this.mainfileSystem;
			if (discFileSystem != null)
			{
				discFileSystem.Dispose();
			}
			DiscFileSystem discFileSystem2 = this.bootFileSystem;
			if (discFileSystem2 != null)
			{
				discFileSystem2.Dispose();
			}
			VirtualDisk virtualDisk = this.disk;
			if (virtualDisk != null)
			{
				virtualDisk.Dispose();
			}
			IDisposable disposable = this.vhdMount;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			this.DisposeStorageSpaces();
			if (this.tempPathToMount != null && Directory.Exists(this.tempPathToMount))
			{
				Directory.Delete(this.tempPathToMount);
				this.tempPathToMount = null;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000061CE File Offset: 0x000043CE
		private static byte[] GetLongBytes(long value)
		{
			return BitConverter.GetBytes(value).ToArray<byte>();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000061DC File Offset: 0x000043DC
		private static byte[] GetIPAddressBytes(string value)
		{
			byte[] array = new byte[8];
			if (!string.IsNullOrEmpty(value))
			{
				byte[] array2 = IPAddress.Parse(value).GetAddressBytes().Reverse<byte>().ToArray<byte>();
				Array.Copy(array2, 0, array, 0, array2.Length);
			}
			return array;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000621B File Offset: 0x0000441B
		private static string GetXElementName(string localName, string namespaceName)
		{
			return string.Format("{{{0}}}{1}", namespaceName, localName);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000622C File Offset: 0x0000442C
		private static string CreateMd5ForFolder(string path)
		{
			List<string> list = (from p in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
			orderby p
			select p).ToList<string>();
			MD5 md = MD5.Create();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				string arg = text.Substring(path.Length + 1).ToLowerInvariant();
				FileInfo fileInfo = new FileInfo(text);
				string s = string.Format("{0}.{1:x}.{2:x}", arg, fileInfo.Length, fileInfo.LastWriteTimeUtc.Ticks);
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				if (i < list.Count - 1)
				{
					md.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
				}
				else
				{
					md.TransformFinalBlock(bytes, 0, bytes.Length);
				}
			}
			return BitConverter.ToString(md.Hash).Replace("-", "").ToLower();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00006338 File Offset: 0x00004538
		private static string GetLocalIPAddress()
		{
			IPAddress ipaddress = (from a in Dns.GetHostEntry(Dns.GetHostName()).AddressList
			where a.AddressFamily == AddressFamily.InterNetwork
			select a).FirstOrDefault<IPAddress>();
			if (ipaddress != null)
			{
				return ipaddress.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000638D File Offset: 0x0000458D
		private static string GetWofStreamName(string vhdFileName)
		{
			return vhdFileName + ":WofCompressedData";
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000639C File Offset: 0x0000459C
		private void CopyDirectoryFromLocalToVhdImpl(string localDir, string fileSpec, string destDir, bool topLevel, bool recursive, bool useHashFile, WindowsImageVhd.CopyFileFunc copyFileFunc)
		{
			NtfsFileSystem ntfsFileSystem = this.OSFileSystem as NtfsFileSystem;
			string text = null;
			string path = Path.Combine(destDir, "xde.dirhash.txt");
			if (topLevel && useHashFile)
			{
				text = WindowsImageVhd.CreateMd5ForFolder(localDir);
				if (this.mainfileSystem.FileExists(path))
				{
					using (Stream stream = this.mainfileSystem.OpenFile(path, FileMode.Open))
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							string b = streamReader.ReadToEnd();
							if (text == b)
							{
								return;
							}
						}
					}
				}
			}
			if (!this.mainfileSystem.DirectoryExists(destDir))
			{
				if (ntfsFileSystem != null)
				{
					NewFileOptions newFileOptions = new NewFileOptions();
					for (string directoryName = Path.GetDirectoryName(destDir); directoryName != null; directoryName = Path.GetDirectoryName(directoryName))
					{
						if (this.mainfileSystem.DirectoryExists(directoryName))
						{
							newFileOptions.SecurityDescriptor = ntfsFileSystem.GetSecurity(directoryName);
							break;
						}
					}
					ntfsFileSystem.CreateDirectory(destDir, newFileOptions);
				}
				else
				{
					this.mainfileSystem.CreateDirectory(destDir);
				}
			}
			RawSecurityDescriptor securityDescriptor = (ntfsFileSystem != null && ntfsFileSystem.FileExists("\\windows\\system32\\wininet.dll")) ? ntfsFileSystem.GetSecurity("\\windows\\system32\\wininet.dll") : null;
			foreach (string text2 in Directory.GetFiles(localDir, fileSpec))
			{
				string fileName = Path.GetFileName(text2);
				string text3 = Path.Combine(destDir, fileName);
				bool flag = this.mainfileSystem.FileExists(text3);
				using (Stream stream2 = new FileStream(text2, FileMode.Open, FileAccess.Read))
				{
					using (Stream stream3 = this.mainfileSystem.OpenFile(text3, FileMode.Create, FileAccess.Write))
					{
						stream2.CopyTo(stream3);
					}
				}
				if (!flag && ntfsFileSystem != null)
				{
					ntfsFileSystem.SetSecurity(text3, securityDescriptor);
				}
				FileInfo fileInfo = new FileInfo(text2);
				this.mainfileSystem.SetCreationTimeUtc(text3, fileInfo.CreationTimeUtc);
				this.mainfileSystem.SetLastWriteTimeUtc(text3, fileInfo.LastWriteTimeUtc);
				if (copyFileFunc != null)
				{
					copyFileFunc(text2, text3);
				}
			}
			if (recursive)
			{
				foreach (string text4 in Directory.GetDirectories(localDir))
				{
					string fileName2 = Path.GetFileName(text4);
					string destDir2 = Path.Combine(destDir, fileName2);
					this.CopyDirectoryFromLocalToVhdImpl(text4, fileSpec, destDir2, false, recursive, useHashFile, copyFileFunc);
				}
			}
			if (topLevel && useHashFile)
			{
				using (Stream stream4 = this.mainfileSystem.OpenFile(path, FileMode.Create))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream4))
					{
						streamWriter.Write(text);
					}
				}
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00006660 File Offset: 0x00004860
		private void CopyDirectoryFromVhdToLocalImpl(string vhdDir, string fileSpec, string localDir, bool recursive, WindowsImageVhd.CopyFileFunc copyFileFunc)
		{
			if (!Directory.Exists(localDir))
			{
				Directory.CreateDirectory(localDir);
			}
			foreach (string text in this.OSFileSystem.GetFiles(vhdDir, fileSpec))
			{
				string fileName = Path.GetFileName(text);
				string text2 = Path.Combine(localDir, fileName);
				using (Stream stream = this.mainfileSystem.OpenFile(text, FileMode.Open, FileAccess.Read))
				{
					using (Stream stream2 = new FileStream(text2, FileMode.Create))
					{
						stream.CopyTo(stream2);
					}
				}
				if (copyFileFunc != null)
				{
					copyFileFunc(text, text2);
				}
			}
			if (recursive)
			{
				foreach (string text3 in this.mainfileSystem.GetDirectories(vhdDir))
				{
					string fileName2 = Path.GetFileName(text3);
					string localDir2 = Path.Combine(localDir, fileName2);
					this.CopyDirectoryFromVhdToLocalImpl(text3, fileSpec, localDir2, recursive, copyFileFunc);
				}
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000675C File Offset: 0x0000495C
		private DiscFileSystem GetOSPartitionFileSystemViaMounting(string vhdPath)
		{
			do
			{
				this.tempPathToMount = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			}
			while (Directory.Exists(this.tempPathToMount));
			Directory.CreateDirectory(this.tempPathToMount);
			this.vhdMount = VhdUtils.MountVhdWithPartitionAccessPath(vhdPath, "MainOS", this.tempPathToMount);
			return new NativeFileSystem(this.tempPathToMount, false);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000067C8 File Offset: 0x000049C8
		private DiscFileSystem GetOSPartitionFileSystemViaDiscUtils(string vhdPath, FileAccess fileAccess)
		{
			VolumeManager volumeManager = new VolumeManager();
			this.disk = VirtualDisk.OpenDisk(vhdPath, fileAccess);
			volumeManager.AddDisk(this.disk);
			foreach (LogicalVolumeInfo volume in volumeManager.GetLogicalVolumes())
			{
				DiscUtils.FileSystemInfo fileSystemInfo = FileSystemManager.DetectFileSystems(volume).FirstOrDefault<DiscUtils.FileSystemInfo>();
				if (fileSystemInfo != null)
				{
					DiscFileSystem discFileSystem = fileSystemInfo.Open(volume);
					if (discFileSystem.DirectoryExists("\\windows\\system32\\config") && discFileSystem is NtfsFileSystem)
					{
						return discFileSystem;
					}
					discFileSystem.Dispose();
				}
			}
			this.disk.Dispose();
			this.disk = null;
			return null;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00006858 File Offset: 0x00004A58
		private NtfsFileSystem FindFileSystemInSpaces(string spaceName, string[] dirsToCheck)
		{
			Space space = (from s in this.pool.Spaces
			where s.Name == spaceName
			select s).FirstOrDefault<Space>();
			if (space == null)
			{
				return null;
			}
			PartitionInfo partitionInfo = new DiscUtils.Raw.Disk(space, Ownership.None, this.spacesGeometry).Partitions.Partitions.FirstOrDefault<PartitionInfo>();
			if (partitionInfo == null)
			{
				return null;
			}
			SparseStream stream = partitionInfo.Open();
			if (!NtfsFileSystem.Detect(stream))
			{
				return null;
			}
			NtfsFileSystem ntfsFileSystem = new NtfsFileSystem(stream);
			foreach (string path in dirsToCheck)
			{
				if (ntfsFileSystem.DirectoryExists(path))
				{
					return ntfsFileSystem;
				}
			}
			ntfsFileSystem.Dispose();
			return null;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00006908 File Offset: 0x00004B08
		private void GetOSPartitionFileSystemViaStorageSpaces(string vhdPath, out DiscFileSystem mainFileSystem, out DiscFileSystem bootFileSystem)
		{
			bool flag = false;
			List<string> list = new List<string>(VhdUtils.GetParentVhdPaths(vhdPath));
			list.Insert(0, vhdPath);
			mainFileSystem = null;
			bootFileSystem = null;
			try
			{
				this.openVhds = Vhd.Open(list, false);
				Vhd vhd = this.openVhds.FirstOrDefault<Vhd>();
				if (vhd != null)
				{
					this.pool = Pool.Open(vhd);
					this.spacesGeometry = new Geometry((int)vhd.Cylinders, vhd.TracksPerCylinder, vhd.SectorsPerTrack, vhd.BytesPerSector);
					this.mainfileSystem = this.FindFileSystemInSpaces("MainOSDisk", new string[]
					{
						"\\windows\\system32\\config"
					});
					if (this.mainfileSystem != null)
					{
						flag = true;
						this.bootFileSystem = this.FindFileSystemInSpaces("VIRT_EFIESPDisk", new string[]
						{
							"\\EFI\\Microsoft\\Boot\\",
							"\\Boot\\BCD"
						});
						if (this.mainfileSystem.Exists("\\Windows\\Containers\\Vail\\BaseLayer\\Files"))
						{
							this.containerSystemBaseDir = "\\Windows\\Containers\\Vail\\BaseLayer\\Files";
						}
					}
				}
			}
			finally
			{
				if (!flag)
				{
					this.DisposeStorageSpaces();
				}
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00006A0C File Offset: 0x00004C0C
		private void DisposeStorageSpaces()
		{
			Pool pool = this.pool;
			if (pool != null)
			{
				pool.Dispose();
			}
			if (this.openVhds != null)
			{
				foreach (Vhd vhd in this.openVhds)
				{
					vhd.Dispose();
				}
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00006A78 File Offset: 0x00004C78
		private DiscUtils.Registry.RegistryKey OpenOrCreateRegistryKey(RegKey regKey, bool container)
		{
			return this.GetRegistryKeyFromAlias(regKey.KeyRootAlias, container).CreateSubKey(regKey.KeyPath);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00006A94 File Offset: 0x00004C94
		private void CopyLocalToVhdFile(string localFileName, string vhdFileName)
		{
			if ((this.mainfileSystem.GetFileInfo(vhdFileName).Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
			{
				NtfsFileSystem ntfsFileSystem = (NtfsFileSystem)this.mainfileSystem;
				if (!ntfsFileSystem.GetAlternateDataStreams(vhdFileName).Contains("WofCompressedData"))
				{
					throw new Exception("No compressed data found for file marked as reparse point");
				}
				string wofStreamName = WindowsImageVhd.GetWofStreamName(vhdFileName);
				using (TempFile tempFile = new TempFile())
				{
					using (Stream stream = File.OpenRead(localFileName))
					{
						using (Stream stream2 = new FileStream(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
						{
							try
							{
								FileUtils.WofCompress(stream, stream2);
							}
							catch (Exception)
							{
								return;
							}
						}
					}
					using (Stream stream3 = File.OpenRead(tempFile.FileName))
					{
						using (Stream stream4 = ntfsFileSystem.OpenFile(wofStreamName, FileMode.Create, FileAccess.ReadWrite))
						{
							stream3.CopyTo(stream4);
							return;
						}
					}
				}
			}
			using (Stream stream5 = File.OpenRead(localFileName))
			{
				using (Stream stream6 = this.mainfileSystem.OpenFile(vhdFileName, FileMode.Create, FileAccess.ReadWrite))
				{
					stream5.CopyTo(stream6);
				}
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00006C1C File Offset: 0x00004E1C
		private void CopyFileVhdToLocal(string vhdFileName, string localFileName)
		{
			DiscFileInfo fileInfo = this.mainfileSystem.GetFileInfo(vhdFileName);
			if ((fileInfo.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
			{
				NtfsFileSystem ntfsFileSystem = (NtfsFileSystem)this.mainfileSystem;
				if (!ntfsFileSystem.GetAlternateDataStreams(vhdFileName).Contains("WofCompressedData"))
				{
					throw new Exception("No compressed data found for file marked as reparse point");
				}
				string wofStreamName = WindowsImageVhd.GetWofStreamName(vhdFileName);
				using (TempFile tempFile = new TempFile())
				{
					using (Stream stream = ntfsFileSystem.OpenFile(wofStreamName, FileMode.Open, FileAccess.Read))
					{
						using (Stream stream2 = File.OpenWrite(tempFile.FileName))
						{
							stream.CopyTo(stream2);
						}
					}
					using (Stream stream3 = File.OpenRead(tempFile.FileName))
					{
						using (Stream stream4 = File.OpenWrite(localFileName))
						{
							FileUtils.WofDecompress(stream3, stream4, (uint)fileInfo.Length);
							return;
						}
					}
				}
			}
			using (Stream stream5 = File.OpenWrite(localFileName))
			{
				using (Stream stream6 = this.mainfileSystem.OpenFile(vhdFileName, FileMode.Open, FileAccess.Read))
				{
					stream6.CopyTo(stream5);
				}
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00006D9C File Offset: 0x00004F9C
		private DiscUtils.Registry.RegistryHive GetRegistryHiveFromFileName(string fileName)
		{
			DiscUtils.Registry.RegistryHive registryHive;
			if (!this.fileNamesToHives.TryGetValue(fileName, out registryHive))
			{
				TempFile tempFile = new TempFile();
				this.CopyFileVhdToLocal(fileName, tempFile.FileName);
				Stream stream = new FileStream(tempFile.FileName, FileMode.Open, FileAccess.ReadWrite);
				try
				{
					registryHive = new DiscUtils.Registry.RegistryHive(stream, Ownership.Dispose);
				}
				catch (Exception e)
				{
					Logger.Instance.LogException("FailedToOpenRegHive", e);
					stream.Dispose();
					tempFile.Dispose();
					throw;
				}
				this.fileNamesToHives[fileName] = registryHive;
				this.vhdHiveFileNamesToLocalTempFiles[fileName] = tempFile;
			}
			return registryHive;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00006E30 File Offset: 0x00005030
		private void DisposeHives()
		{
			foreach (DiscUtils.Registry.RegistryHive registryHive in this.fileNamesToHives.Values)
			{
				registryHive.Dispose();
			}
			foreach (KeyValuePair<string, TempFile> keyValuePair in this.vhdHiveFileNamesToLocalTempFiles)
			{
				TempFile value = keyValuePair.Value;
				string key = keyValuePair.Key;
				this.CopyLocalToVhdFile(value.FileName, key);
				value.Dispose();
			}
			this.vhdHiveFileNamesToLocalTempFiles.Clear();
			this.fileNamesToHives.Clear();
		}

		// Token: 0x0400018A RID: 394
		public const string HkuserDefault = "$(hkuser.default)";

		// Token: 0x0400018B RID: 395
		public const string HklmMicrosoft = "$(hklm.microsoft)";

		// Token: 0x0400018C RID: 396
		public const string HkcuRoot = "$(hkcu.root)";

		// Token: 0x0400018D RID: 397
		public const string HklmControl = "$(hklm.control)";

		// Token: 0x0400018E RID: 398
		public const string HklmServices = "$(hklm.services)";

		// Token: 0x0400018F RID: 399
		public const string HklmSystem = "$(hklm.system)";

		// Token: 0x04000190 RID: 400
		public const string HklmSoftware = "$(hklm.software)";

		// Token: 0x04000191 RID: 401
		private const string WofCompressedDataName = "WofCompressedData";

		// Token: 0x04000192 RID: 402
		private static readonly WindowsImageVhd.RegistryAliasData[] RegistryAliasInfos = new WindowsImageVhd.RegistryAliasData[]
		{
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hkuser.default)",
				FileName = "\\Windows\\System32\\config\\default"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hklm.microsoft)",
				FileName = "\\Windows\\System32\\config\\software",
				KeyPath = "Microsoft"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hkcu.root)",
				FileName = "\\Users\\Default\\ntuser.dat"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hklm.control)",
				FileName = "\\Windows\\System32\\config\\system",
				KeyPath = "ControlSet001\\Control"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hklm.services)",
				FileName = "\\Windows\\System32\\config\\system",
				KeyPath = "ControlSet001\\services"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hklm.system)",
				FileName = "\\Windows\\System32\\config\\system"
			},
			new WindowsImageVhd.RegistryAliasData
			{
				Alias = "$(hklm.software)",
				FileName = "\\Windows\\System32\\config\\software"
			}
		};

		// Token: 0x04000193 RID: 403
		private static readonly byte[] TrueBin = new byte[]
		{
			1
		};

		// Token: 0x04000194 RID: 404
		private VirtualDisk disk;

		// Token: 0x04000195 RID: 405
		private List<Vhd> openVhds;

		// Token: 0x04000196 RID: 406
		private Pool pool;

		// Token: 0x04000197 RID: 407
		private Geometry spacesGeometry;

		// Token: 0x04000198 RID: 408
		private Dictionary<string, TempFile> vhdHiveFileNamesToLocalTempFiles = new Dictionary<string, TempFile>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000199 RID: 409
		private DiscFileSystem mainfileSystem;

		// Token: 0x0400019A RID: 410
		private DiscFileSystem bootFileSystem;

		// Token: 0x0400019B RID: 411
		private string containerSystemBaseDir;

		// Token: 0x0400019C RID: 412
		private Dictionary<string, DiscUtils.Registry.RegistryHive> fileNamesToHives = new Dictionary<string, DiscUtils.Registry.RegistryHive>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400019D RID: 413
		private IDisposable vhdMount;

		// Token: 0x0400019E RID: 414
		private string tempPathToMount;

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x06000306 RID: 774
		public delegate void CopyFileFunc(string source, string dest);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x0600030A RID: 778
		public delegate void SingleFileFunc(string fileName);

		// Token: 0x02000083 RID: 131
		private class RegistryAliasData
		{
			// Token: 0x1700010C RID: 268
			// (get) Token: 0x0600030D RID: 781 RVA: 0x00007FCC File Offset: 0x000061CC
			// (set) Token: 0x0600030E RID: 782 RVA: 0x00007FD4 File Offset: 0x000061D4
			public string Alias { get; set; }

			// Token: 0x1700010D RID: 269
			// (get) Token: 0x0600030F RID: 783 RVA: 0x00007FDD File Offset: 0x000061DD
			// (set) Token: 0x06000310 RID: 784 RVA: 0x00007FE5 File Offset: 0x000061E5
			public string FileName { get; set; }

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000311 RID: 785 RVA: 0x00007FEE File Offset: 0x000061EE
			// (set) Token: 0x06000312 RID: 786 RVA: 0x00007FF6 File Offset: 0x000061F6
			public string KeyPath { get; set; }
		}
	}
}
