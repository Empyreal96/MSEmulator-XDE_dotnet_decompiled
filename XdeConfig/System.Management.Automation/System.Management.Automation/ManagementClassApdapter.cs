using System;

namespace System.Management.Automation
{
	// Token: 0x0200018A RID: 394
	internal class ManagementClassApdapter : BaseWMIAdapter
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00077CA8 File Offset: 0x00075EA8
		protected override void AddAllProperties<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members)
		{
			if (wmiObject.SystemProperties != null)
			{
				foreach (PropertyData propertyData in wmiObject.SystemProperties)
				{
					members.Add(new PSProperty(propertyData.Name, this, wmiObject, propertyData) as T);
				}
			}
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00077D1C File Offset: 0x00075F1C
		protected override PSProperty DoGetProperty(ManagementBaseObject wmiObject, string propertyName)
		{
			if (wmiObject.SystemProperties != null)
			{
				foreach (PropertyData propertyData in wmiObject.SystemProperties)
				{
					if (propertyName.Equals(propertyData.Name, StringComparison.OrdinalIgnoreCase))
					{
						return new PSProperty(propertyData.Name, this, wmiObject, propertyData);
					}
				}
			}
			return null;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00077D94 File Offset: 0x00075F94
		protected override object InvokeManagementMethod(ManagementObject wmiObject, string methodName, ManagementBaseObject inParams)
		{
			Adapter.tracer.WriteLine("Invoking class method: {0}", new object[]
			{
				methodName
			});
			ManagementClass managementClass = wmiObject as ManagementClass;
			object result;
			try
			{
				result = managementClass.InvokeMethod(methodName, inParams, null);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new MethodInvocationException("WMIMethodException", ex, ExtendedTypeSystem.WMIMethodInvocationException, new object[]
				{
					methodName,
					ex.Message
				});
			}
			return result;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00077E14 File Offset: 0x00076014
		protected override void AddAllMethods<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members)
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return;
			}
			CacheTable instanceMethodTable = BaseWMIAdapter.GetInstanceMethodTable(wmiObject, true);
			foreach (object obj in instanceMethodTable.memberCollection)
			{
				BaseWMIAdapter.WMIMethodCacheEntry wmimethodCacheEntry = (BaseWMIAdapter.WMIMethodCacheEntry)obj;
				if (members[wmimethodCacheEntry.Name] == null)
				{
					Adapter.tracer.WriteLine("Adding method {0}", new object[]
					{
						wmimethodCacheEntry.Name
					});
					members.Add(new PSMethod(wmimethodCacheEntry.Name, this, wmiObject, wmimethodCacheEntry) as T);
				}
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00077ED4 File Offset: 0x000760D4
		protected override T GetManagementObjectMethod<T>(ManagementBaseObject wmiObject, string methodName)
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return default(T);
			}
			CacheTable instanceMethodTable = BaseWMIAdapter.GetInstanceMethodTable(wmiObject, true);
			BaseWMIAdapter.WMIMethodCacheEntry wmimethodCacheEntry = (BaseWMIAdapter.WMIMethodCacheEntry)instanceMethodTable[methodName];
			if (wmimethodCacheEntry == null)
			{
				return default(T);
			}
			return new PSMethod(wmimethodCacheEntry.Name, this, wmiObject, wmimethodCacheEntry) as T;
		}
	}
}
