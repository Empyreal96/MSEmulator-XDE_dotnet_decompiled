using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000B RID: 11
	public class WmiUtils
	{
		// Token: 0x0600005D RID: 93 RVA: 0x0000308C File Offset: 0x0000128C
		public static ManagementObjectCollection GetAllVirtualTargetComputers(ManagementScope scope)
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_ComputerSystem Where Name != '{0}'", new object[]
			{
				Environment.MachineName
			});
			ManagementObjectCollection result;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query)))
			{
				result = managementObjectSearcher.Get();
			}
			return result;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000030E4 File Offset: 0x000012E4
		public static ManagementObject GetTargetComputer(string vmName, ManagementScope scope)
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_ComputerSystem Where ElementName = '{0}'", new object[]
			{
				vmName
			});
			return WmiBaseUtils.GetInstanceHelper(scope, query, 0, 100);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003110 File Offset: 0x00001310
		public static ManagementObject GetTargetComputerByUniqueId(string vmUniqueId, ManagementScope scope)
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_ComputerSystem Where Name = '{0}'", new object[]
			{
				vmUniqueId
			});
			return WmiBaseUtils.GetInstanceHelper(scope, query, 0, 100);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000313C File Offset: 0x0000133C
		public static ManagementObject GetHostTargetComputer(ManagementScope scope)
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Msvm_ComputerSystem Where Name = '{0}'", new object[]
			{
				Environment.MachineName
			});
			return WmiBaseUtils.GetInstanceHelper(scope, query, 0, 100);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000316C File Offset: 0x0000136C
		public static void GetSwitchForPortSettingData(ManagementObject portSettingData, out ManagementObject virtualSwitch)
		{
			virtualSwitch = null;
			int num = 0;
			while (num < 50 && virtualSwitch == null)
			{
				if (num != 0)
				{
					Thread.Sleep(100);
				}
				string query = string.Format("select * from Msvm_EthernetPortAllocationSettingData where Parent = \"{0}\"", WmiBaseUtils.EscapeWmiStringForQuery(portSettingData.Path.Path));
				ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(portSettingData.Scope, query, 0, 100);
				if (instanceHelper != null)
				{
					string[] array = instanceHelper["HostResource"] as string[];
					if (array != null && array.Length != 0)
					{
						ManagementObject managementObject = new ManagementObject(array[0]);
						try
						{
							managementObject.Get();
							virtualSwitch = managementObject;
						}
						catch (Exception)
						{
							if (num == 50)
							{
								throw;
							}
						}
					}
				}
				num++;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003210 File Offset: 0x00001410
		public static ManagementBaseObject InvokeMethodWithVerify(ManagementObject obj, ManagementBaseObject inParams, string methodName, string failedTaskDescriptionFormat)
		{
			ManagementBaseObject managementBaseObject = obj.InvokeMethod(methodName, inParams, null);
			string text = null;
			WmiUtils.ReturnCode returnCode = WmiUtils.ReturnCode.Completed;
			bool flag;
			if ((uint)managementBaseObject["ReturnValue"] == 4096U)
			{
				flag = WmiUtils.JobCompleted(obj.Scope, managementBaseObject, out returnCode, out text);
			}
			else if ((uint)managementBaseObject["ReturnValue"] == 0U)
			{
				flag = true;
			}
			else
			{
				uint num = (uint)managementBaseObject["ReturnValue"];
				flag = false;
				text = StringUtilities.InvariantCultureFormat(Strings.ErrorCodeFormat, new object[]
				{
					num
				});
			}
			if (!flag && !string.IsNullOrEmpty(failedTaskDescriptionFormat))
			{
				Exception ex = new Exception(StringUtilities.InvariantCultureFormat(failedTaskDescriptionFormat, new object[]
				{
					text
				}));
				ex.Data["ReturnCode"] = returnCode;
				throw ex;
			}
			return managementBaseObject;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032D4 File Offset: 0x000014D4
		public static bool JobCompleted(ManagementScope scope, ManagementBaseObject outParams, out WmiUtils.ReturnCode returnCode, out string errorDescription)
		{
			bool result = true;
			errorDescription = null;
			returnCode = WmiUtils.ReturnCode.Completed;
			string path = (string)outParams["Job"];
			using (ManagementObject managementObject = new ManagementObject(scope, new ManagementPath(path), null))
			{
				managementObject.Get();
				while ((ushort)managementObject["JobState"] == 3 || (ushort)managementObject["JobState"] == 4)
				{
					Thread.Sleep(100);
					managementObject.Get();
				}
				if ((ushort)managementObject["JobState"] != 7)
				{
					returnCode = (WmiUtils.ReturnCode)((ushort)managementObject["ErrorCode"]);
					errorDescription = (string)managementObject["ErrorDescription"];
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000339C File Offset: 0x0000159C
		public static ManagementObject GetServiceObject(ManagementScope scope, string serviceName)
		{
			ManagementPath path = new ManagementPath(serviceName);
			ManagementObject result;
			using (ManagementClass managementClass = new ManagementClass(scope, path, null))
			{
				result = managementClass.GetInstances().Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000033E8 File Offset: 0x000015E8
		public static ManagementObject GetResourceAllocationsettingDataDefault(ManagementScope scope, XdeWmiTypes.ResourceType resourceType, XdeWmiTypes.ResourceSubType resourceSubType, string otherResourceType = null)
		{
			string query;
			if (resourceType == XdeWmiTypes.ResourceType.Other)
			{
				query = StringUtilities.InvariantCultureFormat("select * from Msvm_ResourcePool where ResourceType = '{0}' and ResourceSubType = null and OtherResourceType = {1}", new object[]
				{
					(ushort)resourceType,
					otherResourceType
				});
			}
			else
			{
				query = StringUtilities.InvariantCultureFormat("select * from Msvm_ResourcePool where ResourceType = '{0}' and ResourceSubType ='{1}' and OtherResourceType = null", new object[]
				{
					(ushort)resourceType,
					resourceSubType.Name()
				});
			}
			ManagementObjectCollection managementObjectCollection;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query)))
			{
				managementObjectCollection = managementObjectSearcher.Get();
			}
			WmiUtils.DebugAssert(managementObjectCollection.Count == 1);
			foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
			{
				foreach (ManagementBaseObject managementBaseObject2 in ((ManagementObject)managementBaseObject).GetRelated("Msvm_AllocationCapabilities"))
				{
					foreach (ManagementBaseObject managementBaseObject3 in ((ManagementObject)managementBaseObject2).GetRelationships("Msvm_SettingsDefineCapabilities"))
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject3;
						if (managementObject["ValueRole"] != null && Convert.ToInt16(managementObject["ValueRole"]) == 0)
						{
							return new ManagementObject(managementObject["PartComponent"].ToString());
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003580 File Offset: 0x00001780
		public static string NormalizeMac(string mac)
		{
			if (!mac.Contains(':'))
			{
				return string.Concat(new string[]
				{
					mac.Substring(0, 2),
					":",
					mac.Substring(2, 2),
					":",
					mac.Substring(4, 2),
					":",
					mac.Substring(6, 2),
					":",
					mac.Substring(8, 2),
					":",
					mac.Substring(10, 2)
				});
			}
			return null;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003614 File Offset: 0x00001814
		public static ManagementObjectCollection FindAllEnabledNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_ExternalEthernetPort WHERE EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003630 File Offset: 0x00001830
		public static ManagementObjectCollection FindAllUnboundNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_ExternalEthernetPort WHERE IsBound=False AND EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000364C File Offset: 0x0000184C
		public static ManagementObjectCollection FindAllEnabledWiFiNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_WiFiPort WHERE EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003668 File Offset: 0x00001868
		public static ManagementObjectCollection FindAllUnboundWiFiNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_WiFiPort WHERE IsBound=False AND EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003684 File Offset: 0x00001884
		public static ManagementObjectCollection FindAllBoundNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_ExternalEthernetPort WHERE IsBound=True AND EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000036A0 File Offset: 0x000018A0
		public static ManagementObjectCollection FindAllBoundWiFiNics(ManagementScope scope)
		{
			string query = "Select * From Msvm_WiFiPort WHERE IsBound=True AND EnabledState=2";
			return WmiBaseUtils.GetQueryHelper(scope, query);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000036BC File Offset: 0x000018BC
		public static void CheckValidWmiObjectArgClass(ManagementObject wmiObj, string argName, string wmiClass)
		{
			if (wmiObj["__CLASS"] == null || wmiObj["__CLASS"].ToString().ToLowerInvariant() != wmiClass.ToLowerInvariant())
			{
				throw new ArgumentNullException(StringUtilities.CurrentCultureFormat(XdeVmExceptions.ArgumentError, new object[]
				{
					argName
				}), argName);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003714 File Offset: 0x00001914
		public static void CheckValidWmiObjectArgSuperClass(ManagementObject wmiObj, string argName, string wmiClass)
		{
			string[] array = wmiObj["__DERIVATION"] as string[];
			if (array != null && !array.Any((string c) => StringUtilities.EqualsNoCase(c, wmiClass)))
			{
				throw new ArgumentException(StringUtilities.CurrentCultureFormat(XdeVmExceptions.ArgumentError, new object[]
				{
					argName
				}), argName);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003774 File Offset: 0x00001974
		public static void CheckValidArgScope(ManagementObject wmiObj, string argName, ManagementScope scope)
		{
			if (wmiObj.Scope == null || !wmiObj.Scope.Path.Path.Equals(scope.Path.Path))
			{
				throw new ArgumentException(StringUtilities.CurrentCultureFormat(XdeVmExceptions.ArgumentError, new object[]
				{
					argName
				}), argName);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000037C6 File Offset: 0x000019C6
		public static void DebugAssert(bool condition)
		{
			Logger.Instance.LogAssertNoPopup(condition);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000037D4 File Offset: 0x000019D4
		[Conditional("DEBUG")]
		public static void DumpProperties(ManagementBaseObject obj)
		{
			foreach (PropertyData propertyData in obj.Properties)
			{
				if (propertyData.Value is Array)
				{
					Array array = (Array)propertyData.Value;
					for (int i = 0; i < array.Length; i++)
					{
					}
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003850 File Offset: 0x00001A50
		public static NetworkAdapterInformation[] GetLocalAdapterInformation()
		{
			ManagementScope cimv2Scope = WmiBaseUtils.GetCimv2Scope();
			List<NetworkAdapterInformation> list = new List<NetworkAdapterInformation>();
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(cimv2Scope, new ObjectQuery("select * from Win32_NetworkAdapterConfiguration where IPEnabled = 'True'")))
			{
				foreach (ManagementObject adapter in managementObjectSearcher.Get().Cast<ManagementObject>())
				{
					NetworkAdapterInformation item = WmiUtils.ConvertWmiNetworkAdapterConfig(adapter);
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000038E0 File Offset: 0x00001AE0
		public static ManagementObject Get3dVideoPool()
		{
			ManagementScope virtualizationV2Scope = WmiBaseUtils.GetVirtualizationV2Scope();
			ManagementObject result;
			try
			{
				result = WmiUtils.GetServiceObject(virtualizationV2Scope, "Msvm_Synth3dVideoPool");
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003918 File Offset: 0x00001B18
		public static bool IsRemoteFxAvailable()
		{
			bool result;
			try
			{
				ManagementObject managementObject = WmiUtils.Get3dVideoPool();
				if (managementObject == null)
				{
					WmiUtils.LogRemoteFxAvailability(false, "VideoPoolNone");
					result = false;
				}
				else if (!(bool)managementObject["IsGPUCapable"])
				{
					WmiUtils.LogRemoteFxAvailability(false, "GpuNotCapable");
					result = false;
				}
				else if (!(bool)managementObject["IsSLATCapable"])
				{
					WmiUtils.LogRemoteFxAvailability(false, "SlatNotCapable");
					result = false;
				}
				else
				{
					ManagementObject serviceObject = WmiUtils.GetServiceObject(WmiBaseUtils.GetCimv2Scope(), "Win32_OperatingSystem");
					if (serviceObject != null && int.Parse(serviceObject["ProductType"].ToString(), CultureInfo.InvariantCulture) != 1 && !WmiUtils.IsRDVHInstalled())
					{
						WmiUtils.LogRemoteFxAvailability(false, "RdvhNotInstalled");
						result = false;
					}
					else
					{
						WmiUtils.LogRemoteFxAvailability(true, string.Empty);
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				WmiUtils.LogRemoteFxAvailability(false, "ExceptionThrown");
				Logger.Instance.LogException("RemoteFxAvailability", e);
				result = false;
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003A0C File Offset: 0x00001C0C
		internal static bool IsDX11Disabled()
		{
			bool result = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\RemoteFX\\Parameters"))
				{
					if (registryKey != null)
					{
						int? num = registryKey.GetValue("DisableDX11") as int?;
						if (num != null && num.Value != 0)
						{
							result = true;
						}
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A88 File Offset: 0x00001C88
		private static bool IsRDVHInstalled()
		{
			string query = StringUtilities.InvariantCultureFormat("select * from Win32_ServerFeature where ID = {0}", new object[]
			{
				322
			});
			return WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), query, 0, 100) != null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003AC4 File Offset: 0x00001CC4
		private static NetworkAdapterInformation ConvertWmiNetworkAdapterConfig(ManagementObject adapter)
		{
			NetworkAdapterInformation result = default(NetworkAdapterInformation);
			result.MacAddress = ((string)adapter["MACAddress"]).Replace(":", string.Empty);
			List<NetworkIPAddress> list = new List<NetworkIPAddress>();
			string[] array = (string[])adapter["IPAddress"];
			string[] array2 = (string[])adapter["IPSubnet"];
			for (int i = 0; i < array.Length; i++)
			{
				NetworkIPAddress item = new NetworkIPAddress
				{
					DadState = IpDadState.Invalid
				};
				item.IPAddress = IPAddress.Parse(array[i]);
				item.IPSubnet = IPAddress.Parse(array2[i]);
				list.Add(item);
			}
			result.IPAddresses = list.ToArray();
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003B83 File Offset: 0x00001D83
		private static void LogRemoteFxAvailability(bool available, string reason)
		{
			Logger.Instance.Log("RemoteFxAvailability", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				available = available,
				reason = reason
			});
		}

		// Token: 0x0400000F RID: 15
		private const string RemoteFxAvailability = "RemoteFxAvailability";

		// Token: 0x02000019 RID: 25
		public enum ReturnCode : ushort
		{
			// Token: 0x0400005A RID: 90
			Completed,
			// Token: 0x0400005B RID: 91
			Started = 4096,
			// Token: 0x0400005C RID: 92
			Failed = 32768,
			// Token: 0x0400005D RID: 93
			AccessDenied,
			// Token: 0x0400005E RID: 94
			NotSupported,
			// Token: 0x0400005F RID: 95
			Unknown,
			// Token: 0x04000060 RID: 96
			Timeout,
			// Token: 0x04000061 RID: 97
			InvalidParameter,
			// Token: 0x04000062 RID: 98
			SystemInUse,
			// Token: 0x04000063 RID: 99
			InvalidState,
			// Token: 0x04000064 RID: 100
			IncorrectDataType,
			// Token: 0x04000065 RID: 101
			SystemNotAvailable,
			// Token: 0x04000066 RID: 102
			OutOfMemory
		}

		// Token: 0x0200001A RID: 26
		private enum JobState
		{
			// Token: 0x04000068 RID: 104
			None,
			// Token: 0x04000069 RID: 105
			New = 2,
			// Token: 0x0400006A RID: 106
			Starting,
			// Token: 0x0400006B RID: 107
			Running,
			// Token: 0x0400006C RID: 108
			Suspended,
			// Token: 0x0400006D RID: 109
			ShuttingDown,
			// Token: 0x0400006E RID: 110
			Completed,
			// Token: 0x0400006F RID: 111
			Terminated,
			// Token: 0x04000070 RID: 112
			Killed,
			// Token: 0x04000071 RID: 113
			Exception,
			// Token: 0x04000072 RID: 114
			Service
		}

		// Token: 0x0200001B RID: 27
		private enum ValueRole
		{
			// Token: 0x04000074 RID: 116
			Default,
			// Token: 0x04000075 RID: 117
			Minimum,
			// Token: 0x04000076 RID: 118
			Maximum,
			// Token: 0x04000077 RID: 119
			Increment
		}
	}
}
