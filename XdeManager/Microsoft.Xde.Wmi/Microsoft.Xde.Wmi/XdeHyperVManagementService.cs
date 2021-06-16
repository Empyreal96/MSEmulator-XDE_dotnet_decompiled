using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000D RID: 13
	public sealed class XdeHyperVManagementService : IXdeHyperVManagementService, IDisposable
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00003CAA File Offset: 0x00001EAA
		private XdeHyperVManagementService()
		{
			this.isDisposed = false;
			this.virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003CCF File Offset: 0x00001ECF
		private VirtualSystemManagementService VirtualSystemSvc
		{
			get
			{
				if (!this.isDisposed && this.virtualSystemSvc == null)
				{
					this.virtualSystemSvc = VirtualSystemManagementService.GetService();
				}
				return this.virtualSystemSvc;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003CF2 File Offset: 0x00001EF2
		private VirtualEthernetSwitchManagementService VirtualEthSwitchSvc
		{
			get
			{
				if (!this.isDisposed && this.virtualEthSwitchSvc == null)
				{
					this.virtualEthSwitchSvc = VirtualEthernetSwitchManagementService.GetService();
				}
				return this.virtualEthSwitchSvc;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003D15 File Offset: 0x00001F15
		private ManagementObject ImageManagementService
		{
			get
			{
				if (!this.isDisposed && this.imageManagementService == null)
				{
					this.imageManagementService = WmiUtils.GetServiceObject(this.virtualizationV2Scope, "Msvm_ImageManagementService");
				}
				return this.imageManagementService;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003D44 File Offset: 0x00001F44
		public static IXdeHyperVManagementService GetService()
		{
			XdeHyperVManagementService xdeHyperVManagementService = new XdeHyperVManagementService();
			if (xdeHyperVManagementService.VirtualSystemSvc != null)
			{
				return xdeHyperVManagementService;
			}
			return null;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003D64 File Offset: 0x00001F64
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				if (this.virtualSystemSvc != null)
				{
					this.virtualSystemSvc.Dispose();
					this.virtualSystemSvc = null;
				}
				if (this.virtualEthSwitchSvc != null)
				{
					this.virtualEthSwitchSvc.Dispose();
					this.virtualEthSwitchSvc = null;
				}
				this.isDisposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003DBA File Offset: 0x00001FBA
		public bool InternalNetworkAdapterFixNeeded(string switchFriendlyName)
		{
			return this.NeedNetworkAdapterSettingsFix(switchFriendlyName);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003DC3 File Offset: 0x00001FC3
		public bool UnboundNicExists()
		{
			return WmiUtils.FindAllUnboundNics(this.virtualizationV2Scope).Count > 0 || WmiUtils.FindAllUnboundWiFiNics(this.virtualizationV2Scope).Count > 0;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003DF0 File Offset: 0x00001FF0
		public void CreateSwitchesForAllUnboundNics()
		{
			this.CreateSwitchesForAllUnboundWiredNics();
			this.CreateSwitchesForAllUnboundWiFiNics();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003E00 File Offset: 0x00002000
		public ReadOnlyCollection<IXdeVirtualSwitchInformation> GetExternalSwitchInfos()
		{
			if (this.externalSwitchInfos != null)
			{
				return this.externalSwitchInfos;
			}
			List<IXdeVirtualSwitchInformation> list = new List<IXdeVirtualSwitchInformation>();
			ManagementScope scope = WmiBaseUtils.GetVirtualizationV2Scope();
			foreach (ManagementObject nicPort in WmiUtils.FindAllBoundNics(scope).Cast<ManagementObject>().Union(WmiUtils.FindAllBoundWiFiNics(scope).Cast<ManagementObject>()))
			{
				using (ManagementObject managementObject = this.FindSwitchConnectedToNicV2(nicPort))
				{
					if (managementObject != null)
					{
						XdeHyperVManagementService.XdeVirtualSwitchInformation xdeVirtualSwitchInformation = new XdeHyperVManagementService.XdeVirtualSwitchInformation(managementObject);
						xdeVirtualSwitchInformation.External = true;
						list.Add(xdeVirtualSwitchInformation);
						this.switchInfos[xdeVirtualSwitchInformation.Id] = xdeVirtualSwitchInformation;
					}
				}
			}
			this.externalSwitchInfos = list.AsReadOnly();
			return this.externalSwitchInfos;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003EDC File Offset: 0x000020DC
		public ManagementObject XdeCreateExternalVirtualSwitch(ManagementObject externalNic, string switchFriendlyName)
		{
			ValidationUtilities.CheckNotNull(externalNic, "externalNic");
			ValidationUtilities.CheckNotNull(switchFriendlyName, "switchFriendlyName");
			WmiUtils.CheckValidArgScope(externalNic, "externalNic", this.virtualizationV2Scope);
			ManagementObject result;
			using (ManagementObject virtualEthernetSwitchSettingData = this.VirtualEthSwitchSvc.GetVirtualEthernetSwitchSettingData())
			{
				virtualEthernetSwitchSettingData["VirtualSystemIdentifier"] = Guid.NewGuid().ToString();
				virtualEthernetSwitchSettingData["ElementName"] = switchFriendlyName;
				using (ManagementObject internalPortSettingData = this.GetInternalPortSettingData(switchFriendlyName))
				{
					internalPortSettingData["Address"] = externalNic["PermanentAddress"];
					using (ManagementObject externalPortSettingData = this.GetExternalPortSettingData(externalNic, switchFriendlyName))
					{
						string[] resourceSettings = new string[]
						{
							internalPortSettingData.GetText(TextFormat.CimDtd20),
							externalPortSettingData.GetText(TextFormat.CimDtd20)
						};
						result = this.VirtualEthSwitchSvc.DefineSystem(virtualEthernetSwitchSettingData.GetText(TextFormat.CimDtd20), resourceSettings);
					}
				}
			}
			return result;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003FEC File Offset: 0x000021EC
		public ManagementObject XdeCreateInternalVirtualSwitch(string switchFriendlyName)
		{
			ValidationUtilities.CheckNotBlank(switchFriendlyName, "switchFriendlyName");
			ManagementObject result;
			using (ManagementObject virtualEthernetSwitchSettingData = this.VirtualEthSwitchSvc.GetVirtualEthernetSwitchSettingData())
			{
				virtualEthernetSwitchSettingData["VirtualSystemIdentifier"] = Guid.NewGuid().ToString();
				virtualEthernetSwitchSettingData["ElementName"] = switchFriendlyName;
				using (ManagementObject internalPortSettingData = this.GetInternalPortSettingData(switchFriendlyName))
				{
					string[] resourceSettings = new string[]
					{
						internalPortSettingData.GetText(TextFormat.CimDtd20)
					};
					ManagementObject managementObject = this.VirtualEthSwitchSvc.DefineSystem(virtualEthernetSwitchSettingData.GetText(TextFormat.CimDtd20), resourceSettings);
					if (managementObject == null)
					{
						managementObject = this.FindInternalSwitch(switchFriendlyName);
					}
					result = managementObject;
				}
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000040B0 File Offset: 0x000022B0
		public void XdeDeleteVirtualSwitch(ManagementObject switchObject)
		{
			ValidationUtilities.CheckNotNull(switchObject, "switchObject");
			WmiUtils.CheckValidArgScope(switchObject, "switchObject", this.virtualizationV2Scope);
			this.VirtualEthSwitchSvc.DestroySystem(switchObject);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000040DC File Offset: 0x000022DC
		public void XdeDeleteVirtualSwitch(string switchFriendlyName)
		{
			ValidationUtilities.CheckNotBlank(switchFriendlyName, "switchFriendlyName");
			using (ManagementObject managementObject = this.FindSwitchV2(switchFriendlyName))
			{
				if (managementObject == null)
				{
					throw new XdeVirtualMachineException(XdeVmExceptions.SwitchNotFound);
				}
				this.XdeDeleteVirtualSwitch(managementObject);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004130 File Offset: 0x00002330
		public ManagementObject FindSwitchByUniqueId(string id)
		{
			ValidationUtilities.CheckNotBlank(id, "id");
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_VirtualEthernetSwitch where Name = '{0}'", new object[]
			{
				id
			});
			return WmiBaseUtils.GetInstanceHelper(this.virtualizationV2Scope, query, 0, 100);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000416C File Offset: 0x0000236C
		public ManagementObject FindSwitchV2(string switchFriendlyName)
		{
			ValidationUtilities.CheckNotBlank(switchFriendlyName, "switchFriendlyName");
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_VirtualEthernetSwitch where ElementName = '{0}'", new object[]
			{
				switchFriendlyName
			});
			return WmiBaseUtils.GetInstanceHelper(this.virtualizationV2Scope, query, 0, 100);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000041A8 File Offset: 0x000023A8
		public ManagementObject FindSwitchV2ByUniqueId(string id)
		{
			ValidationUtilities.CheckNotBlank(id, "id");
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_VirtualEthernetSwitch where Name = '{0}'", new object[]
			{
				id
			});
			return WmiBaseUtils.GetInstanceHelper(this.virtualizationV2Scope, query, 0, 100);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000041E4 File Offset: 0x000023E4
		public ManagementObject FindDefaultSwitch()
		{
			return this.FindSwitchByUniqueId("c08cb7b8-9b3c-408e-8e30-5e16a3aeb444");
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000041F4 File Offset: 0x000023F4
		public ManagementObject FindSwitchConnectedToNicV2(ManagementObject nicPort)
		{
			ValidationUtilities.CheckNotNull(nicPort, "nicPort");
			string relatedClass;
			if (nicPort["CreationClassName"].ToString() == "Msvm_ExternalEthernetPort")
			{
				relatedClass = "Msvm_LANEndpoint";
			}
			else
			{
				WmiUtils.CheckValidWmiObjectArgClass(nicPort, "nicPort", "Msvm_WiFiPort");
				relatedClass = "Msvm_WiFiEndpoint";
			}
			foreach (ManagementBaseObject managementBaseObject in nicPort.GetRelated(relatedClass))
			{
				foreach (ManagementBaseObject managementBaseObject2 in ((ManagementObject)managementBaseObject).GetRelated("Msvm_LANEndpoint"))
				{
					foreach (ManagementBaseObject managementBaseObject3 in ((ManagementObject)managementBaseObject2).GetRelated("Msvm_EthernetSwitchPort"))
					{
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = ((ManagementObject)managementBaseObject3).GetRelated("Msvm_VirtualEthernetSwitch").GetEnumerator())
						{
							if (enumerator4.MoveNext())
							{
								return (ManagementObject)enumerator4.Current;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004350 File Offset: 0x00002550
		public bool IsExternalSwitchV2(ManagementObject switchObject)
		{
			ValidationUtilities.CheckNotNull(switchObject, "switchObject");
			foreach (ManagementBaseObject managementBaseObject in switchObject.GetRelated("Msvm_EthernetSwitchPort"))
			{
				foreach (ManagementBaseObject managementBaseObject2 in ((ManagementObject)managementBaseObject).GetRelated("Msvm_LANEndpoint"))
				{
					foreach (ManagementBaseObject managementBaseObject3 in ((ManagementObject)managementBaseObject2).GetRelated("Msvm_LANEndpoint"))
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject3;
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = managementObject.GetRelated("Msvm_ExternalEthernetPort").GetEnumerator())
						{
							if (enumerator4.MoveNext())
							{
								ManagementObject managementObject2 = (ManagementObject)enumerator4.Current;
								return true;
							}
						}
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = managementObject.GetRelated("Msvm_WiFiPort").GetEnumerator())
						{
							if (enumerator4.MoveNext())
							{
								ManagementObject managementObject3 = (ManagementObject)enumerator4.Current;
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000450C File Offset: 0x0000270C
		public void CreateDifferencingVirtualHardDisk(string diffDiskPath, string parentPath)
		{
			ValidationUtilities.CheckNotBlank(diffDiskPath, "diffDiskPath");
			ValidationUtilities.CheckNotBlank(parentPath, "parentPath");
			if (!File.Exists(parentPath))
			{
				throw new FileNotFoundException(XdeVmExceptions.VHDNotFound);
			}
			ManagementBaseObject methodParameters = this.ImageManagementService.GetMethodParameters("CreateDifferencingVirtualHardDisk");
			methodParameters["Path"] = diffDiskPath;
			methodParameters["ParentPath"] = parentPath;
			WmiUtils.InvokeMethodWithVerify(this.ImageManagementService, methodParameters, "CreateDifferencingVirtualHardDisk", XdeVmExceptions.DiffDiskCreateError);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004584 File Offset: 0x00002784
		public VirtualHardDiskSettingData GetVhdInformation(string vhdFileName)
		{
			ManagementBaseObject methodParameters = this.ImageManagementService.GetMethodParameters("GetVirtualHardDiskSettingData");
			methodParameters["Path"] = vhdFileName;
			return VirtualHardDiskSettingData.Parse((string)WmiUtils.InvokeMethodWithVerify(this.ImageManagementService, methodParameters, "GetVirtualHardDiskSettingData", XdeVmExceptions.DiffDiskCreateError)["SettingData"]);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000045D8 File Offset: 0x000027D8
		public IXdeVirtualSwitchInformation GetSwitchInformation(ManagementObject switchObj)
		{
			string key = switchObj["Name"] as string;
			bool flag = false;
			Dictionary<string, XdeHyperVManagementService.XdeVirtualSwitchInformation> obj = this.switchInfos;
			XdeHyperVManagementService.XdeVirtualSwitchInformation xdeVirtualSwitchInformation;
			lock (obj)
			{
				if (!this.switchInfos.TryGetValue(key, out xdeVirtualSwitchInformation))
				{
					int num = 0;
					while (num <= 5 && !flag)
					{
						string query = StringUtilities.InvariantCultureFormat("select * from Msvm_VirtualEthernetSwitch where Name = '{0}'", new object[]
						{
							switchObj["Name"] as string
						});
						ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetVirtualizationV2Scope(), query, 0, 100);
						if (instanceHelper == null)
						{
							return null;
						}
						xdeVirtualSwitchInformation = new XdeHyperVManagementService.XdeVirtualSwitchInformation(switchObj);
						try
						{
							foreach (ManagementBaseObject managementBaseObject in instanceHelper.GetRelated("Msvm_EthernetSwitchPort"))
							{
								foreach (ManagementBaseObject managementBaseObject2 in ((ManagementObject)managementBaseObject).GetRelated("Msvm_LANEndpoint"))
								{
									ManagementObject managementObject = (ManagementObject)managementBaseObject2;
									foreach (ManagementBaseObject managementBaseObject3 in managementObject.GetRelated("Msvm_LANEndpoint"))
									{
										ManagementObject managementObject2 = (ManagementObject)managementBaseObject3;
										using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = managementObject2.GetRelated("Msvm_ExternalEthernetPort").GetEnumerator())
										{
											if (enumerator4.MoveNext())
											{
												ManagementObject managementObject3 = (ManagementObject)enumerator4.Current;
												xdeVirtualSwitchInformation.External = true;
												xdeVirtualSwitchInformation.HostMacAddress = (managementObject3["PermanentAddress"] as string);
												goto IL_28F;
											}
										}
										foreach (ManagementBaseObject managementBaseObject4 in managementObject2.GetRelated("Msvm_InternalEthernetPort"))
										{
											ManagementObject managementObject4 = (ManagementObject)managementBaseObject4;
											xdeVirtualSwitchInformation.HostMacAddress = (managementObject4["PermanentAddress"] as string);
										}
									}
									foreach (ManagementBaseObject managementBaseObject5 in managementObject.GetRelated("Msvm_WiFiEndpoint"))
									{
										using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = ((ManagementObject)managementBaseObject5).GetRelated("Msvm_WiFiPort").GetEnumerator())
										{
											if (enumerator4.MoveNext())
											{
												ManagementObject managementObject5 = (ManagementObject)enumerator4.Current;
												xdeVirtualSwitchInformation.External = true;
												xdeVirtualSwitchInformation.HostMacAddress = (managementObject5["PermanentAddress"] as string);
												goto IL_28F;
											}
										}
									}
								}
							}
						}
						catch (ManagementException)
						{
							if (num < 5)
							{
								goto IL_29E;
							}
							throw;
						}
						goto IL_28F;
						IL_29E:
						num++;
						continue;
						IL_28F:
						this.switchInfos[key] = xdeVirtualSwitchInformation;
						flag = true;
						goto IL_29E;
					}
				}
			}
			return xdeVirtualSwitchInformation;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004984 File Offset: 0x00002B84
		public string FindNextAvailableInternalMacAddress(string switchFriendlyName)
		{
			string @string = this.FindInternalSwitch(switchFriendlyName)["Name"] as string;
			ManagementScope scope = WmiBaseUtils.GetVirtualizationV2Scope();
			string query = "Select * From Msvm_SyntheticEthernetPortSettingData";
			bool[] array = new bool[255];
			string text = string.Empty;
			if (switchFriendlyName == "Microsoft Emulator NAT Switch")
			{
				text = "02DFDFDFDF";
			}
			foreach (ManagementBaseObject managementBaseObject in WmiBaseUtils.GetQueryHelper(scope, query))
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				string text2 = managementObject["Address"] as string;
				if (!string.IsNullOrEmpty(text2) && text2.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					ManagementObject managementObject2;
					WmiUtils.GetSwitchForPortSettingData(managementObject, out managementObject2);
					if (managementObject2 != null)
					{
						string string2 = managementObject2["Name"] as string;
						byte b;
						if (StringUtilities.EqualsNoCase(@string, string2) && byte.TryParse(text2.Substring(text.Length, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
						{
							array[(int)b] = true;
						}
					}
				}
			}
			int num = 2;
			while (num < 255 && array[num])
			{
				num++;
			}
			if (num == 255)
			{
				throw new XdeVirtualMachineException(XdeVmExceptions.NoMacAddressesAvailableForInternalNic);
			}
			return StringUtilities.InvariantCultureFormat("{0}{1:X2}", new object[]
			{
				text,
				num
			});
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004AE4 File Offset: 0x00002CE4
		public bool IsNicValidForInternalUse(ManagementObject portSettings, string switchFriendlyName)
		{
			bool flag = (bool)portSettings["StaticMacAddress"];
			string text = portSettings["Address"] as string;
			string value = string.Empty;
			if (switchFriendlyName == "Microsoft Emulator NAT Switch")
			{
				value = "02DFDFDFDF";
			}
			return flag && !string.IsNullOrEmpty(text) && text.StartsWith(value, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004B41 File Offset: 0x00002D41
		public ManagementObject FindInternalSwitch(string switchFriendlyName)
		{
			return this.FindSwitchV2(switchFriendlyName);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004B4C File Offset: 0x00002D4C
		public void SetInternalNetworkAdapterStaticIP(ManagementObject internalSwitch, IPAddress staticIP, IPAddress subnetMask)
		{
			IXdeVirtualSwitchInformation switchInformation = this.GetSwitchInformation(internalSwitch);
			string query = StringUtilities.InvariantCultureFormat("select * from Win32_NetworkAdapterConfiguration where MACAddress = '{0}' and IPEnabled = 'True'", new object[]
			{
				WmiUtils.NormalizeMac(switchInformation.HostMacAddress)
			});
			using (ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), query, 10, 500))
			{
				if (instanceHelper == null)
				{
					throw new Exception(XdeVmExceptions.IpV4NotFound);
				}
				using (ManagementBaseObject methodParameters = instanceHelper.GetMethodParameters("EnableStatic"))
				{
					methodParameters["IPAddress"] = new string[]
					{
						staticIP.ToString()
					};
					methodParameters["SubnetMask"] = new string[]
					{
						subnetMask.ToString()
					};
					WmiUtils.InvokeMethodWithVerify(instanceHelper, methodParameters, "EnableStatic", string.Empty);
				}
				using (ManagementBaseObject methodParameters2 = instanceHelper.GetMethodParameters("SetIPConnectionMetric"))
				{
					methodParameters2["IPConnectionMetric"] = 9000U;
					WmiUtils.InvokeMethodWithVerify(instanceHelper, methodParameters2, "SetIPConnectionMetric", string.Empty);
				}
				using (ManagementBaseObject methodParameters3 = instanceHelper.GetMethodParameters("SetDynamicDNSRegistration"))
				{
					methodParameters3["FullDNSRegistrationEnabled"] = false;
					methodParameters3["DomainDNSRegistrationEnabled"] = false;
					WmiUtils.InvokeMethodWithVerify(instanceHelper, methodParameters3, "SetDynamicDNSRegistration", string.Empty);
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004D04 File Offset: 0x00002F04
		public void InitializeMacAddressRange()
		{
			if (WmiUtils.GetAllVirtualTargetComputers(this.virtualizationV2Scope).Count != 0)
			{
				return;
			}
			foreach (ManagementBaseObject managementBaseObject in WmiUtils.FindAllEnabledNics(this.virtualizationV2Scope))
			{
				string text = (string)((ManagementObject)managementBaseObject)["PermanentAddress"];
				if (!string.IsNullOrEmpty(text))
				{
					this.SetMacAddressRange(text);
					return;
				}
			}
			foreach (ManagementBaseObject managementBaseObject2 in WmiUtils.FindAllEnabledWiFiNics(this.virtualizationV2Scope))
			{
				string text = (string)((ManagementObject)managementBaseObject2)["PermanentAddress"];
				if (!string.IsNullOrEmpty(text))
				{
					this.SetMacAddressRange(text);
					break;
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public void FixInternalNetworkAdapterSettings(string switchFriendlyName)
		{
			ManagementScope standardCimv2Scope = WmiBaseUtils.GetStandardCimv2Scope();
			string query = StringUtilities.InvariantCultureFormat("select * from MSFT_NetAdapterBindingSettingData where Name like '%" + switchFriendlyName + "%'", Array.Empty<object>());
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(standardCimv2Scope, new ObjectQuery(query)))
			{
				foreach (ManagementObject managementObject in managementObjectSearcher.Get().Cast<ManagementObject>())
				{
					string a = (string)managementObject["ComponentID"];
					if (a == "ms_tcpip6" || a == "ms_tcpip")
					{
						WmiUtils.InvokeMethodWithVerify(managementObject, null, "Enable", string.Empty);
					}
					else
					{
						WmiUtils.InvokeMethodWithVerify(managementObject, null, "Disable", string.Empty);
					}
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004ECC File Offset: 0x000030CC
		public ManagementObject DefineVirtualSystem(string vmName)
		{
			return this.DefineVirtualSystem(vmName, false);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004ED6 File Offset: 0x000030D6
		public ManagementObject DefineVirtualSystem(string vmName, bool generation2)
		{
			ValidationUtilities.CheckNotBlank(vmName, "vmName");
			return this.VirtualSystemSvc.DefineVirtualSystem(vmName, generation2);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004EF0 File Offset: 0x000030F0
		public void DestroyVirtualSystem(ManagementObject vmObj)
		{
			ValidationUtilities.CheckNotNull(vmObj, "vmObj");
			WmiUtils.CheckValidWmiObjectArgClass(vmObj, "vmObj", "Msvm_ComputerSystem");
			this.VirtualSystemSvc.DestroyVirtualSystem(vmObj);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004F19 File Offset: 0x00003119
		public string[] AddVirtualSystemResources(ManagementObject vmSettingsObj, ManagementObject resObj, string resName)
		{
			ValidationUtilities.CheckNotNull(vmSettingsObj, "vmSettingsObj");
			WmiUtils.CheckValidWmiObjectArgClass(vmSettingsObj, "vmSettingsObj", "Msvm_VirtualSystemSettingData");
			ValidationUtilities.CheckNotNull(resObj, "resObj");
			return this.VirtualSystemSvc.AddVirtualSystemResources(vmSettingsObj, resObj, resName);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004F4F File Offset: 0x0000314F
		public void DeleteVirtualSystemResource(ManagementObject resObj, string resName)
		{
			ValidationUtilities.CheckNotNull(resObj, "resObj");
			this.VirtualSystemSvc.DeleteVirtualSystemResource(resObj, resName);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004F69 File Offset: 0x00003169
		public void ModifyVirtualSystem(ManagementObject settingsObj)
		{
			ValidationUtilities.CheckNotNull(settingsObj, "settingsObj");
			this.VirtualSystemSvc.ModifyVirtualSystem(settingsObj.GetText(TextFormat.CimDtd20));
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004F88 File Offset: 0x00003188
		public void ModifyVirtualSystemResources(ManagementObject resObj, string resName)
		{
			ValidationUtilities.CheckNotNull(resObj, "resObj");
			WmiUtils.CheckValidWmiObjectArgSuperClass(resObj, "resObj", "CIM_ResourceAllocationSettingData");
			this.VirtualSystemSvc.ModifyVirtualSystemResources(resObj, resName);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004FB2 File Offset: 0x000031B2
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public Image GetVirtualSystemThumbnailImage(ManagementObject vmObj, int width, int height)
		{
			ValidationUtilities.CheckNotNull(vmObj, "vmObj");
			return this.VirtualSystemSvc.GetVirtualSystemThumbnailImage(vmObj, width, height);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004FCD File Offset: 0x000031CD
		public ulong GetSettingsSizeOfSystemFiles(ManagementObject settingsObj)
		{
			return this.VirtualSystemSvc.GetSettingsSizeOfSystemFiles(settingsObj);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004FDB File Offset: 0x000031DB
		private static string GetExternalSwitchName(ManagementBaseObject unboundNic)
		{
			return StringUtilities.CurrentCultureFormat(Strings.ExternalSwitchNameFormat, new object[]
			{
				unboundNic["Name"]
			});
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004FFC File Offset: 0x000031FC
		private bool NeedNetworkAdapterSettingsFix(string switchFriendlyName)
		{
			bool result = false;
			ManagementObject managementObject = this.FindSwitchV2(switchFriendlyName);
			if (managementObject != null)
			{
				IXdeVirtualSwitchInformation switchInformation = this.GetSwitchInformation(managementObject);
				string value = string.Empty;
				string value2 = string.Empty;
				try
				{
					value = switchInformation.HostIpAddress;
					value2 = switchInformation.HostIpMask;
				}
				catch (Exception)
				{
				}
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value2))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005064 File Offset: 0x00003264
		private ManagementObject GetInternalPortSettingData(string switchFriendlyName)
		{
			ManagementObject result;
			using (ManagementObject resourceAllocationsettingDataDefault = WmiUtils.GetResourceAllocationsettingDataDefault(this.virtualizationV2Scope, XdeWmiTypes.ResourceType.EthernetPort, XdeWmiTypes.ResourceSubType.EthernetConnection, null))
			{
				resourceAllocationsettingDataDefault["Address"] = null;
				resourceAllocationsettingDataDefault["HostResource"] = new string[]
				{
					WmiUtils.GetHostTargetComputer(this.virtualizationV2Scope).Path.Path
				};
				resourceAllocationsettingDataDefault["ElementName"] = XdeVirtualMachineDefaultSettings.GetInternalEthernetPortName(switchFriendlyName);
				result = resourceAllocationsettingDataDefault;
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000050E8 File Offset: 0x000032E8
		private ManagementObject GetExternalPortSettingData(ManagementObject nicObj, string switchFriendlyName)
		{
			ManagementObject result;
			using (ManagementObject resourceAllocationsettingDataDefault = WmiUtils.GetResourceAllocationsettingDataDefault(this.virtualizationV2Scope, XdeWmiTypes.ResourceType.EthernetPort, XdeWmiTypes.ResourceSubType.EthernetConnection, null))
			{
				resourceAllocationsettingDataDefault["Address"] = null;
				resourceAllocationsettingDataDefault["HostResource"] = new string[]
				{
					nicObj.Path.Path
				};
				resourceAllocationsettingDataDefault["ElementName"] = XdeVirtualMachineDefaultSettings.GetExternalSwitchPortName(switchFriendlyName);
				result = resourceAllocationsettingDataDefault;
			}
			return result;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005164 File Offset: 0x00003364
		private void CreateSwitchesForAllUnboundWiredNics()
		{
			foreach (ManagementBaseObject managementBaseObject in WmiUtils.FindAllUnboundNics(this.virtualizationV2Scope))
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				this.XdeCreateExternalVirtualSwitch(managementObject, XdeHyperVManagementService.GetExternalSwitchName(managementObject));
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000051C4 File Offset: 0x000033C4
		private void CreateSwitchesForAllUnboundWiFiNics()
		{
			foreach (ManagementBaseObject managementBaseObject in WmiUtils.FindAllUnboundWiFiNics(this.virtualizationV2Scope))
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				this.XdeCreateExternalVirtualSwitch(managementObject, XdeHyperVManagementService.GetExternalSwitchName(managementObject));
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005224 File Offset: 0x00003424
		private void SetMacAddressRange(string hostMacAddress)
		{
			if (!string.IsNullOrEmpty(hostMacAddress))
			{
				uint hash = this.GetHash24(hostMacAddress);
				WmiUtils.DebugAssert((hash & 4278190080U) == 0U);
				if (hash <= 16776960U)
				{
					string value = "00155D" + hash.ToString("X6");
					string value2 = "00155D" + (hash + 255U).ToString("X6");
					using (ManagementObject virtualSystemManagementServiceSettingData = this.virtualSystemSvc.GetVirtualSystemManagementServiceSettingData())
					{
						virtualSystemManagementServiceSettingData["MinimumMacAddress"] = value;
						virtualSystemManagementServiceSettingData["MaximumMacAddress"] = value2;
						this.virtualSystemSvc.ModifyServiceSettings(virtualSystemManagementServiceSettingData.GetText(TextFormat.CimDtd20));
					}
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000052EC File Offset: 0x000034EC
		private uint GetHash24(string input)
		{
			byte[] bytes = BitConverter.GetBytes(input.GetHashCode());
			WmiUtils.DebugAssert(bytes.Length == 4);
			byte[] array = bytes;
			int num = 0;
			array[num] ^= bytes[3];
			byte[] array2 = bytes;
			int num2 = 1;
			array2[num2] ^= bytes[3];
			byte[] array3 = bytes;
			int num3 = 2;
			array3[num3] ^= bytes[3];
			bytes[3] = 0;
			return BitConverter.ToUInt32(bytes, 0);
		}

		// Token: 0x04000014 RID: 20
		public const string InternalNATSwitchName = "Microsoft Emulator NAT Switch";

		// Token: 0x04000015 RID: 21
		public const string DefaultWindowsSwitchName = "c08cb7b8-9b3c-408e-8e30-5e16a3aeb444";

		// Token: 0x04000016 RID: 22
		public const string XdeNATMacPrefix = "02DFDFDFDF";

		// Token: 0x04000017 RID: 23
		private const string MicrosoftOUI = "00155D";

		// Token: 0x04000018 RID: 24
		private const uint InterfaceMetricValue = 9000U;

		// Token: 0x04000019 RID: 25
		private bool isDisposed;

		// Token: 0x0400001A RID: 26
		private ManagementScope virtualizationV2Scope;

		// Token: 0x0400001B RID: 27
		private VirtualSystemManagementService virtualSystemSvc;

		// Token: 0x0400001C RID: 28
		private VirtualEthernetSwitchManagementService virtualEthSwitchSvc;

		// Token: 0x0400001D RID: 29
		private ManagementObject imageManagementService;

		// Token: 0x0400001E RID: 30
		private ReadOnlyCollection<IXdeVirtualSwitchInformation> externalSwitchInfos;

		// Token: 0x0400001F RID: 31
		private Dictionary<string, XdeHyperVManagementService.XdeVirtualSwitchInformation> switchInfos = new Dictionary<string, XdeHyperVManagementService.XdeVirtualSwitchInformation>();

		// Token: 0x0200001D RID: 29
		private class XdeVirtualSwitchInformation : IXdeVirtualSwitchInformation, IXdeWmiObject
		{
			// Token: 0x06000197 RID: 407 RVA: 0x000088FE File Offset: 0x00006AFE
			public XdeVirtualSwitchInformation(ManagementObject switchObj)
			{
				this.Id = (switchObj["Name"] as string);
				this.Name = (switchObj["ElementName"] as string);
				this.WmiObject = switchObj;
			}

			// Token: 0x1700006B RID: 107
			// (get) Token: 0x06000198 RID: 408 RVA: 0x00008939 File Offset: 0x00006B39
			// (set) Token: 0x06000199 RID: 409 RVA: 0x00008941 File Offset: 0x00006B41
			public ManagementObject WmiObject { get; set; }

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x0600019A RID: 410 RVA: 0x0000894A File Offset: 0x00006B4A
			// (set) Token: 0x0600019B RID: 411 RVA: 0x00008952 File Offset: 0x00006B52
			public bool External { get; set; }

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x0600019C RID: 412 RVA: 0x0000895B File Offset: 0x00006B5B
			public string HostIpAddress
			{
				get
				{
					if (string.IsNullOrEmpty(this.hostIpAddress))
					{
						this.DetectHostIpAddressAndMask();
						return this.hostIpAddress;
					}
					return this.hostIpAddress;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x0600019D RID: 413 RVA: 0x0000897D File Offset: 0x00006B7D
			public string HostIpMask
			{
				get
				{
					if (string.IsNullOrEmpty(this.hostIpMask))
					{
						this.DetectHostIpAddressAndMask();
						return this.hostIpMask;
					}
					return this.hostIpMask;
				}
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x0600019E RID: 414 RVA: 0x0000899F File Offset: 0x00006B9F
			// (set) Token: 0x0600019F RID: 415 RVA: 0x000089A7 File Offset: 0x00006BA7
			public string HostMacAddress { get; set; }

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x060001A0 RID: 416 RVA: 0x000089B0 File Offset: 0x00006BB0
			// (set) Token: 0x060001A1 RID: 417 RVA: 0x000089B8 File Offset: 0x00006BB8
			public string Name { get; set; }

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x000089C1 File Offset: 0x00006BC1
			// (set) Token: 0x060001A3 RID: 419 RVA: 0x000089C9 File Offset: 0x00006BC9
			public string Id { get; set; }

			// Token: 0x060001A4 RID: 420 RVA: 0x000089D4 File Offset: 0x00006BD4
			private void DetectHostIpAddressAndMask()
			{
				if (string.IsNullOrEmpty(this.HostMacAddress))
				{
					return;
				}
				string text = WmiUtils.NormalizeMac(this.HostMacAddress);
				string query = StringUtilities.InvariantCultureFormat("select * from Win32_NetworkAdapterConfiguration where MACAddress = '{0}' and IPEnabled = 'True'", new object[]
				{
					text
				});
				ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), query, 0, 100);
				if (instanceHelper == null)
				{
					return;
				}
				for (int i = 60; i > 0; i--)
				{
					foreach (string address in (string[])instanceHelper["IPAddress"])
					{
						if (ValidationUtilities.IsValidIPv4Format(address))
						{
							this.hostIpAddress = address;
							break;
						}
					}
					foreach (string address2 in (string[])instanceHelper["IPSubnet"])
					{
						if (ValidationUtilities.IsValidIPv4Format(address2))
						{
							this.hostIpMask = address2;
							break;
						}
					}
					if (!string.IsNullOrEmpty(this.hostIpAddress) && !string.IsNullOrEmpty(this.hostIpMask))
					{
						return;
					}
					Thread.Sleep(1000);
					instanceHelper.Get();
				}
				throw new XdeVirtualMachineException(XdeVmExceptions.IpV4NotFound);
			}

			// Token: 0x04000079 RID: 121
			private string hostIpAddress;

			// Token: 0x0400007A RID: 122
			private string hostIpMask;
		}
	}
}
