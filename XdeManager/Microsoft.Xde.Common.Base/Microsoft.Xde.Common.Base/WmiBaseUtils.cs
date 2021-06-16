using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000029 RID: 41
	public static class WmiBaseUtils
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00004264 File Offset: 0x00002464
		public static string EscapeWmiStringForQuery(string item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in item)
			{
				if (c != '"')
				{
					if (c == '\\')
					{
						stringBuilder.Append("\\\\");
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				else
				{
					stringBuilder.Append("\\\"");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000042C6 File Offset: 0x000024C6
		public static ManagementScope GetVirtualizationV2Scope()
		{
			return WmiBaseUtils.GetVirtualizationV2Scope(string.Empty);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000042D2 File Offset: 0x000024D2
		public static ManagementScope GetVirtualizationV2Scope(string serverName)
		{
			if (string.IsNullOrEmpty(serverName) || StringComparer.OrdinalIgnoreCase.Equals(serverName, "localhost"))
			{
				serverName = ".";
			}
			return new ManagementScope(StringUtilities.InvariantCultureFormat("\\\\{0}\\root\\virtualization\\v2", new object[]
			{
				serverName
			}));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000430E File Offset: 0x0000250E
		public static ManagementScope GetCimv2Scope()
		{
			return new ManagementScope("\\\\.\\root\\cimv2");
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000431A File Offset: 0x0000251A
		public static ManagementScope GetStandardCimv2Scope()
		{
			return new ManagementScope("\\\\.\\root\\StandardCimv2");
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00004328 File Offset: 0x00002528
		public static ManagementObject GetInstanceHelper(ManagementScope scope, string query, int retries = 0, int waitBeforeRetry = 100)
		{
			for (int i = 0; i <= retries; i++)
			{
				if (i != 0)
				{
					Thread.Sleep(waitBeforeRetry);
				}
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query)))
				{
					ManagementObject managementObject = managementObjectSearcher.Get().Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
					if (managementObject != null)
					{
						return managementObject;
					}
				}
			}
			return null;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00004390 File Offset: 0x00002590
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

		// Token: 0x060001AD RID: 429 RVA: 0x000043DC File Offset: 0x000025DC
		public static ManagementObject CreateManagementObject(ManagementScope scope, string className)
		{
			ManagementPath path = new ManagementPath(className);
			ManagementObject result;
			using (ManagementClass managementClass = new ManagementClass(scope, path, null))
			{
				result = managementClass.CreateInstance();
			}
			return result;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00004420 File Offset: 0x00002620
		public static ManagementObjectCollection GetQueryHelper(ManagementScope scope, string query)
		{
			ManagementObjectCollection result;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query)))
			{
				result = managementObjectSearcher.Get();
			}
			return result;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00004460 File Offset: 0x00002660
		public static IEnumerable<ManagementObject> GetAllInstances(ManagementScope scope, string className)
		{
			IEnumerable<ManagementObject> result;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, new ObjectQuery("select * from " + className)))
			{
				result = managementObjectSearcher.Get().Cast<ManagementObject>();
			}
			return result;
		}
	}
}
