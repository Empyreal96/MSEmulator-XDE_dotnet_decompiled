using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Xde.Common;
using Microsoft.Xde.Hcs.Interop;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Hcs
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public sealed class XdeHcsVirtualServices : IXdeVirtualServices
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000395C File Offset: 0x00001B5C
		public static string VmInfoPath
		{
			get
			{
				if (XdeHcsVirtualServices.vmInfoPath == null)
				{
					XdeHcsVirtualServices.vmInfoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\XdeApp\\HCS");
					if (!Directory.Exists(XdeHcsVirtualServices.vmInfoPath))
					{
						Directory.CreateDirectory(XdeHcsVirtualServices.vmInfoPath);
					}
					FileUtils.GrantHyperVRightsForDirectory(XdeHcsVirtualServices.vmInfoPath);
				}
				return XdeHcsVirtualServices.vmInfoPath;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000039AC File Offset: 0x00001BAC
		public bool CanAccessApi
		{
			get
			{
				bool result;
				try
				{
					HcsFactory.GetHcs().EnumerateComputeSystems(null);
					result = true;
				}
				catch (Exception ex) when (ex is HcsException && ex.HResult == -2143878885)
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003A0C File Offset: 0x00001C0C
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00003A14 File Offset: 0x00001C14
		[Import]
		public IXdeConnectionAddressInfo AddressInfo { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003A1D File Offset: 0x00001C1D
		public bool IsGpuSupported
		{
			get
			{
				return XdeHcsVirtualServices.GetPartitonableGpus().FirstOrDefault<ManagementObject>() != null;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003A2C File Offset: 0x00001C2C
		public IXdeVirtualMachine CreateVirtualMachine(string virtualMachineName)
		{
			using (IXdeVirtualMachine virtualMachine = this.GetVirtualMachine(virtualMachineName))
			{
				if (virtualMachine != null)
				{
					throw new ArgumentException(Strings.VMAlreadyExists, "virtualMachineName");
				}
			}
			return XdeHcsVirtualMachine.CreateXdeHcsVirtualMachine(virtualMachineName, this.AddressInfo);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003A7C File Offset: 0x00001C7C
		public IXdeVirtualMachine CreateVirtualMachine(string virtualMachineName, bool generation2)
		{
			if (!generation2)
			{
				throw new ArgumentException(Strings.OnlyGen2Supported, "generation2");
			}
			return this.CreateVirtualMachine(virtualMachineName);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003A98 File Offset: 0x00001C98
		public ReadOnlyCollection<IXdeVirtualMachine> GetAllXdeVirtualMachines(SettingsOptions settingsOptions)
		{
			List<IXdeVirtualMachine> list = new List<IXdeVirtualMachine>();
			foreach (string text in Directory.EnumerateFiles(XdeHcsVirtualServices.VmInfoPath, "*.vm.json"))
			{
				XdeHcsVirtualMachine item = null;
				try
				{
					item = XdeHcsVirtualMachine.LoadFromFile(text, this.AddressInfo);
				}
				catch (Exception)
				{
					File.Delete(text);
					continue;
				}
				list.Add(item);
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003B20 File Offset: 0x00001D20
		public IXdeVirtualMachine GetVirtualMachine(string virtualMachineName)
		{
			return this.GetAllXdeVirtualMachines(SettingsOptions.None).FirstOrDefault((IXdeVirtualMachine v) => v.Name == virtualMachineName);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003B52 File Offset: 0x00001D52
		public IXdeVirtualMachine GetVirtualMachine(string virtualMachineName, SettingsOptions options)
		{
			return this.GetVirtualMachine(virtualMachineName);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003B5C File Offset: 0x00001D5C
		internal static ManagementObject GetMostPerformantPartionableGpu()
		{
			Dictionary<ulong, ManagementObject> dictionary = new Dictionary<ulong, ManagementObject>();
			foreach (ManagementObject managementObject in XdeHcsVirtualServices.GetPartitonableGpus())
			{
				string pDeviceName = (string)managementObject["ElementName"];
				NativeMethods.D3DKMT_OPENADAPTERFROMDEVICENAME d3DKMT_OPENADAPTERFROMDEVICENAME = default(NativeMethods.D3DKMT_OPENADAPTERFROMDEVICENAME);
				d3DKMT_OPENADAPTERFROMDEVICENAME.pDeviceName = pDeviceName;
				if (NativeMethods.D3DKMTOpenAdapterFromDeviceName(ref d3DKMT_OPENADAPTERFROMDEVICENAME) == 0)
				{
					dictionary[d3DKMT_OPENADAPTERFROMDEVICENAME.AdapterLuid.ToULong()] = managementObject;
					NativeMethods.D3DKMT_CLOSEADAPTER d3DKMT_CLOSEADAPTER = new NativeMethods.D3DKMT_CLOSEADAPTER
					{
						hAdapter = d3DKMT_OPENADAPTERFROMDEVICENAME.hAdapter
					};
					NativeMethods.D3DKMTCloseAdapter(ref d3DKMT_CLOSEADAPTER);
				}
			}
			Guid guid = typeof(NativeMethods.IDXGIFactory6).GUID;
			NativeMethods.IDXGIFactory6 idxgifactory;
			NativeMethods.CreateDXGIFactory2(0U, ref guid, out idxgifactory);
			Guid guid2 = typeof(NativeMethods.IDXGIAdapter).GUID;
			uint num = 0U;
			ManagementObject result;
			for (;;)
			{
				NativeMethods.IDXGIAdapter idxgiadapter;
				try
				{
					idxgifactory.EnumAdapterByGpuPreference(num, NativeMethods.DXGI_GPU_PREFERENCE.DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE, ref guid2, out idxgiadapter);
				}
				catch (COMException)
				{
					goto IL_102;
				}
				NativeMethods.DXGI_ADAPTER_DESC dxgi_ADAPTER_DESC;
				idxgiadapter.GetDesc(out dxgi_ADAPTER_DESC);
				if (dictionary.TryGetValue(dxgi_ADAPTER_DESC.AdapterLuid.ToULong(), out result))
				{
					break;
				}
				num += 1U;
			}
			return result;
			IL_102:
			return XdeHcsVirtualServices.GetPartitonableGpus().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003C94 File Offset: 0x00001E94
		internal static IEnumerable<ManagementObject> GetPartitonableGpus()
		{
			ManagementScope scope = new ManagementScope("\\\\.\\root\\virtualization\\v2");
			IEnumerable<ManagementObject> result;
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery("select * from Msvm_PartitionableGpu")))
				{
					result = managementObjectSearcher.Get().Cast<ManagementObject>();
				}
			}
			catch (ManagementException)
			{
				result = new ManagementObject[0];
			}
			return result;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003CFC File Offset: 0x00001EFC
		internal static ManagementObject GetPnpDriverForGpu(ManagementObject partionableGpu)
		{
			string text = (string)partionableGpu["ElementName"];
			if (text.StartsWith("\\\\?\\", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring("\\\\?\\".Length);
			}
			int num = text.IndexOf("#{");
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			text = text.Replace("#", "\\\\");
			string query = "select * from Win32_PnPSignedDriver where DeviceId = \"" + text + "\"";
			ManagementObject result;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new ManagementScope("\\\\.\\root\\cimv2"), new ObjectQuery(query)))
			{
				result = managementObjectSearcher.Get().Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003DBC File Offset: 0x00001FBC
		internal static string GetDriverPathForPartionableGpu(ManagementObject partGpu)
		{
			string path = (string)partGpu["ElementName"];
			ManagementObject pnpDriverForGpu = XdeHcsVirtualServices.GetPnpDriverForGpu(partGpu);
			if (pnpDriverForGpu != null)
			{
				string text = (string)pnpDriverForGpu["InfName"];
				if (text != null)
				{
					string text2 = Registry.GetValue("HKEY_LOCAL_MACHINE\\DRIVERS\\DriverDatabase\\DriverInfFiles\\" + text, "Active", null) as string;
					if (text2 != null)
					{
						return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "DriverStore\\FileRepository", text2);
					}
					Logger.Instance.LogError("HcsGpuDidntFindInfDirName", new
					{
						path
					});
				}
				else
				{
					Logger.Instance.LogError("HcsGpuDidntFindInfName", new
					{
						path
					});
				}
			}
			else
			{
				Logger.Instance.LogError("HcsGpuCouldntFindDriver", new
				{
					path
				});
			}
			return null;
		}

		// Token: 0x04000016 RID: 22
		private static string vmInfoPath;
	}
}
