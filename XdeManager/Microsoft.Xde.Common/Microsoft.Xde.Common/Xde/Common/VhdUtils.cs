using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using DiscUtils;
using DiscUtils.Setup;
using DiscUtils.Vhd;
using DiscUtils.Vhdx;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006F RID: 111
	public static class VhdUtils
	{
		// Token: 0x0600029E RID: 670 RVA: 0x00006EFC File Offset: 0x000050FC
		static VhdUtils()
		{
			SetupHelper.RegisterAssembly(typeof(DiscUtils.Vhd.Disk).Assembly);
			SetupHelper.RegisterAssembly(typeof(DiscUtils.Vhdx.Disk).Assembly);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00006F28 File Offset: 0x00005128
		public static void CreateDiffDisk(string originalVhd, string diffVhd)
		{
			if (!File.Exists(originalVhd))
			{
				throw new FileNotFoundException();
			}
			if (File.Exists(diffVhd))
			{
				File.Delete(diffVhd);
			}
			string directoryName = Path.GetDirectoryName(diffVhd);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			NativeMethods.VIRTUAL_STORAGE_TYPE storageType = VhdUtils.GetStorageType(originalVhd);
			NativeMethods.CREATE_VIRTUAL_DISK_PARAMETERS create_VIRTUAL_DISK_PARAMETERS = default(NativeMethods.CREATE_VIRTUAL_DISK_PARAMETERS);
			IntPtr zero = IntPtr.Zero;
			create_VIRTUAL_DISK_PARAMETERS.Version = NativeMethods.CREATE_VIRTUAL_DISK_VERSION.CREATE_VIRTUAL_DISK_VERSION_1;
			create_VIRTUAL_DISK_PARAMETERS.Version1Data.BlockSizeInBytes = 0U;
			create_VIRTUAL_DISK_PARAMETERS.Version1Data.SectorSizeInBytes = 0U;
			create_VIRTUAL_DISK_PARAMETERS.Version1Data.ParentPath = originalVhd;
			create_VIRTUAL_DISK_PARAMETERS.Version1Data.SourcePath = null;
			int num = NativeMethods.CreateVirtualDisk(ref storageType, diffVhd, NativeMethods.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_CREATE, IntPtr.Zero, NativeMethods.CREATE_VIRTUAL_DISK_FLAG.CREATE_VIRTUAL_DISK_FLAG_NONE, 0U, ref create_VIRTUAL_DISK_PARAMETERS, IntPtr.Zero, ref zero);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			new VhdUtils.SafeDiskHandle(zero).Dispose();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00006FF0 File Offset: 0x000051F0
		private static Guid GetVhdIdInfo(string vhdFileName, NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION version)
		{
			NativeMethods.VIRTUAL_STORAGE_TYPE storageType = VhdUtils.GetStorageType(vhdFileName);
			NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS open_VIRTUAL_DISK_PARAMETERS = default(NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS);
			open_VIRTUAL_DISK_PARAMETERS.Version = NativeMethods.OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_1;
			open_VIRTUAL_DISK_PARAMETERS.Version1.RWDepth = 1U;
			NativeMethods.VIRTUAL_DISK_ACCESS_MASK virtualDiskAccessMask = (NativeMethods.VIRTUAL_DISK_ACCESS_MASK)589824;
			NativeMethods.OPEN_VIRTUAL_DISK_FLAG flags = NativeMethods.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE;
			IntPtr intPtr;
			int num = NativeMethods.OpenVirtualDisk(ref storageType, vhdFileName, virtualDiskAccessMask, flags, ref open_VIRTUAL_DISK_PARAMETERS, out intPtr);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			Guid identifier;
			using (new VhdUtils.SafeDiskHandle(intPtr))
			{
				NativeMethods.GetVirtualDiskInfoIdentifier getVirtualDiskInfoIdentifier = new NativeMethods.GetVirtualDiskInfoIdentifier
				{
					Version = version
				};
				uint num2 = (uint)(Marshal.SizeOf<NativeMethods.GetVirtualDiskInfoIdentifier>(getVirtualDiskInfoIdentifier) + 1024);
				uint num3 = 0U;
				num = NativeMethods.GetVirtualDiskInformation(intPtr, ref num2, ref getVirtualDiskInfoIdentifier, ref num3);
				if (num != 0)
				{
					if (num == -1069940709)
					{
						throw new InvalidOperationException();
					}
					throw new Win32Exception(num);
				}
				else
				{
					identifier = getVirtualDiskInfoIdentifier.Identifier;
				}
			}
			return identifier;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000070C8 File Offset: 0x000052C8
		public static Guid GetVhdId(string vhdFileName)
		{
			return VhdUtils.GetVhdIdInfo(vhdFileName, NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_IDENTIFIER);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000070D1 File Offset: 0x000052D1
		public static Guid GetParentVhdId(string vhdFileName)
		{
			return VhdUtils.GetVhdIdInfo(vhdFileName, NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_PARENT_IDENTIFIER);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000070DC File Offset: 0x000052DC
		public static bool IsVhdChildOfVhd(string baseVhd, string childVhd)
		{
			string vhdParentPathWin;
			try
			{
				vhdParentPathWin = VhdUtils.GetVhdParentPathWin32(childVhd);
			}
			catch (Exception)
			{
				return false;
			}
			Guid vhdId = VhdUtils.GetVhdId(baseVhd);
			Guid parentVhdId = VhdUtils.GetParentVhdId(childVhd);
			return StringComparer.OrdinalIgnoreCase.Equals(vhdParentPathWin, baseVhd) && vhdId == parentVhdId;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00007130 File Offset: 0x00005330
		public static string GetVhdParentPathWin32(string vhdFileName)
		{
			NativeMethods.VIRTUAL_STORAGE_TYPE storageType = VhdUtils.GetStorageType(vhdFileName);
			NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS open_VIRTUAL_DISK_PARAMETERS = default(NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS);
			open_VIRTUAL_DISK_PARAMETERS.Version = NativeMethods.OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_1;
			open_VIRTUAL_DISK_PARAMETERS.Version1.RWDepth = 1U;
			NativeMethods.VIRTUAL_DISK_ACCESS_MASK virtualDiskAccessMask = (NativeMethods.VIRTUAL_DISK_ACCESS_MASK)589824;
			NativeMethods.OPEN_VIRTUAL_DISK_FLAG flags = NativeMethods.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE;
			IntPtr intPtr;
			int num = NativeMethods.OpenVirtualDisk(ref storageType, vhdFileName, virtualDiskAccessMask, flags, ref open_VIRTUAL_DISK_PARAMETERS, out intPtr);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			string result;
			using (new VhdUtils.SafeDiskHandle(intPtr))
			{
				NativeMethods.GetVirtualDiskInfoParentLocation getVirtualDiskInfoParentLocation = new NativeMethods.GetVirtualDiskInfoParentLocation
				{
					Version = NativeMethods.GET_VIRTUAL_DISK_INFO_VERSION.GET_VIRTUAL_DISK_INFO_PARENT_LOCATION
				};
				uint num2 = (uint)Marshal.SizeOf<NativeMethods.GetVirtualDiskInfoParentLocation>(getVirtualDiskInfoParentLocation);
				uint num3 = 0U;
				num = NativeMethods.GetVirtualDiskInformation(intPtr, ref num2, ref getVirtualDiskInfoParentLocation, ref num3);
				if (num != 0)
				{
					if (num != -1069940709)
					{
						throw new Win32Exception(num);
					}
					result = null;
				}
				else
				{
					result = getVirtualDiskInfoParentLocation.ParentLocation.ParentPath;
				}
			}
			return result;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00007208 File Offset: 0x00005408
		public static string GetVhdParentPath(string vhdFileName)
		{
			return VhdUtils.GetParentVhdPaths(vhdFileName).FirstOrDefault<string>();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00007218 File Offset: 0x00005418
		public static string[] GetParentVhdPaths(string vhdFileName)
		{
			List<string> list = new List<string>();
			using (VirtualDisk virtualDisk = VirtualDisk.OpenDisk(vhdFileName, FileAccess.Read))
			{
				VirtualDiskLayer[] array = virtualDisk.Layers.ToArray<VirtualDiskLayer>();
				for (int i = 1; i < array.Length; i++)
				{
					list.Add(array[i].FullPath);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00007280 File Offset: 0x00005480
		public static IDisposable MountVhdWithPartitionAccessPath(string vhdFileName, string partitionName, string emptyNtfsMountFolder)
		{
			return new VhdUtils.VhdMount(vhdFileName, partitionName, emptyNtfsMountFolder);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000728C File Offset: 0x0000548C
		private static NativeMethods.VIRTUAL_STORAGE_TYPE GetStorageType(string vhdFileName)
		{
			bool flag = StringUtilities.EqualsNoCase(Path.GetExtension(vhdFileName), ".vhd");
			return new NativeMethods.VIRTUAL_STORAGE_TYPE
			{
				DeviceId = (flag ? NativeMethods.VHD_STORAGE_TYPE_DEVICE.VIRTUAL_STORAGE_TYPE_DEVICE_VHD : NativeMethods.VHD_STORAGE_TYPE_DEVICE.VIRTUAL_STORAGE_TYPE_DEVICE_VHDX),
				VendorId = NativeMethods.VIRTUAL_STORAGE_TYPE_VENDOR.VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT
			};
		}

		// Token: 0x02000088 RID: 136
		private sealed class VhdMount : IDisposable
		{
			// Token: 0x06000320 RID: 800 RVA: 0x00008084 File Offset: 0x00006284
			public VhdMount(string vhdFileName, string partitionName, string emptyNtfsMountFolder)
			{
				try
				{
					this.AttachVhd(vhdFileName);
					this.FindMountedVhdPartition(partitionName);
					this.AddAccessPath(emptyNtfsMountFolder);
				}
				catch (Exception)
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x06000321 RID: 801 RVA: 0x000080C8 File Offset: 0x000062C8
			public void Dispose()
			{
				this.RemoveAccessPath();
				if (this.virtDisk != null)
				{
					this.virtDisk.Dispose();
				}
			}

			// Token: 0x06000322 RID: 802 RVA: 0x000080E4 File Offset: 0x000062E4
			private void FindMountedVhdPartition(string partitionName)
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new ManagementScope("\\\\.\\root\\microsoft\\windows\\storage"), new ObjectQuery("select * from MSFT_PhysicalDisk")))
				{
					ManagementObject managementObject = (from ManagementObject o in managementObjectSearcher.Get()
					where StringUtilities.EqualsNoCase((string)o["PhysicalLocation"], this.vhdFileName)
					select o).FirstOrDefault<ManagementObject>();
					if (managementObject != null)
					{
						Func<ManagementObject, bool> <>9__1;
						foreach (ManagementObject managementObject2 in managementObject.GetRelated("MSFT_VirtualDisk").Cast<ManagementObject>())
						{
							ManagementObject managementObject3 = managementObject2.GetRelated("MSFT_Disk").Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
							if (managementObject3 != null)
							{
								foreach (ManagementObject managementObject4 in managementObject3.GetRelated("MSFT_Partition").Cast<ManagementObject>())
								{
									IEnumerable<ManagementObject> source = managementObject4.GetRelated("MSFT_Volume").Cast<ManagementObject>();
									Func<ManagementObject, bool> predicate;
									if ((predicate = <>9__1) == null)
									{
										predicate = (<>9__1 = ((ManagementObject v) => StringUtilities.EqualsNoCase((string)v["FileSystemLabel"], partitionName)));
									}
									if (source.Where(predicate).FirstOrDefault<ManagementObject>() != null)
									{
										this.foundPartition = managementObject4;
										return;
									}
								}
							}
						}
					}
				}
				throw new InvalidOperationException(StringUtilities.CurrentCultureFormat(Strings.FailedToFindPartitionInMountedVHD, new object[]
				{
					partitionName,
					this.vhdFileName
				}));
			}

			// Token: 0x06000323 RID: 803 RVA: 0x000082A0 File Offset: 0x000064A0
			private void AttachVhd(string vhdFileName)
			{
				NativeMethods.VIRTUAL_STORAGE_TYPE storageType = VhdUtils.GetStorageType(vhdFileName);
				NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS open_VIRTUAL_DISK_PARAMETERS = default(NativeMethods.OPEN_VIRTUAL_DISK_PARAMETERS);
				open_VIRTUAL_DISK_PARAMETERS.Version = NativeMethods.OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_2;
				NativeMethods.VIRTUAL_DISK_ACCESS_MASK virtualDiskAccessMask = NativeMethods.VIRTUAL_DISK_ACCESS_MASK.VIRTUAL_DISK_ACCESS_NONE;
				NativeMethods.OPEN_VIRTUAL_DISK_FLAG flags = NativeMethods.OPEN_VIRTUAL_DISK_FLAG.OPEN_VIRTUAL_DISK_FLAG_NONE;
				IntPtr intPtr;
				int num = NativeMethods.OpenVirtualDisk(ref storageType, vhdFileName, virtualDiskAccessMask, flags, ref open_VIRTUAL_DISK_PARAMETERS, out intPtr);
				if (num != 0)
				{
					throw new Win32Exception(num);
				}
				this.virtDisk = new VhdUtils.SafeDiskHandle(intPtr);
				NativeMethods.ATTACH_VIRTUAL_DISK_PARAMETERS attach_VIRTUAL_DISK_PARAMETERS = default(NativeMethods.ATTACH_VIRTUAL_DISK_PARAMETERS);
				attach_VIRTUAL_DISK_PARAMETERS.Version = NativeMethods.ATTACH_VIRTUAL_DISK_VERSION.ATTACH_VIRTUAL_DISK_VERSION_1;
				num = NativeMethods.AttachVirtualDisk(intPtr, IntPtr.Zero, NativeMethods.ATTACH_VIRTUAL_DISK_FLAG.ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER, 0U, ref attach_VIRTUAL_DISK_PARAMETERS, IntPtr.Zero);
				if (num != 0)
				{
					throw new Win32Exception(num);
				}
				this.vhdFileName = vhdFileName;
			}

			// Token: 0x06000324 RID: 804 RVA: 0x0000832C File Offset: 0x0000652C
			private void AddAccessPath(string emptyNtfsMountFolder)
			{
				using (ManagementBaseObject methodParameters = this.foundPartition.GetMethodParameters("AddAccessPath"))
				{
					methodParameters["AccessPath"] = emptyNtfsMountFolder;
					ManagementBaseObject managementBaseObject = this.foundPartition.InvokeMethod("AddAccessPath", methodParameters, null);
					int num = (int)((uint)managementBaseObject["ReturnValue"]);
					if (num != 0 && num != 42002)
					{
						string text = (string)managementBaseObject["ExtendedStatus"];
						string text2 = (text != null) ? text : num.ToString();
						throw new InvalidOperationException(StringUtilities.CurrentCultureFormat(Strings.FailedToAddAccessPathToPartition, new object[]
						{
							text2
						}));
					}
					this.addedAccessPath = emptyNtfsMountFolder;
				}
			}

			// Token: 0x06000325 RID: 805 RVA: 0x000083E4 File Offset: 0x000065E4
			private void RemoveAccessPath()
			{
				if (this.foundPartition != null && !string.IsNullOrEmpty(this.addedAccessPath))
				{
					using (ManagementBaseObject methodParameters = this.foundPartition.GetMethodParameters("RemoveAccessPath"))
					{
						methodParameters["AccessPath"] = this.addedAccessPath;
						this.foundPartition.InvokeMethod("RemoveAccessPath", methodParameters, null);
						this.foundPartition = null;
						this.addedAccessPath = null;
					}
				}
			}

			// Token: 0x040001DA RID: 474
			private VhdUtils.SafeDiskHandle virtDisk;

			// Token: 0x040001DB RID: 475
			private string vhdFileName;

			// Token: 0x040001DC RID: 476
			private string addedAccessPath;

			// Token: 0x040001DD RID: 477
			private ManagementObject foundPartition;
		}

		// Token: 0x02000089 RID: 137
		private class SafeDiskHandle : SafeHandle
		{
			// Token: 0x06000326 RID: 806 RVA: 0x00008468 File Offset: 0x00006668
			public SafeDiskHandle(IntPtr handle) : base(handle, true)
			{
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000327 RID: 807 RVA: 0x00008472 File Offset: 0x00006672
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero || this.handle == new IntPtr(-1);
				}
			}

			// Token: 0x06000328 RID: 808 RVA: 0x00008499 File Offset: 0x00006699
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					NativeMethods.CloseHandle(this.handle);
					this.handle = IntPtr.Zero;
				}
				return true;
			}
		}
	}
}
