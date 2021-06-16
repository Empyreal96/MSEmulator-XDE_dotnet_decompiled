using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;

namespace System.Management.Automation
{
	// Token: 0x0200018B RID: 395
	internal class ManagementObjectAdapter : ManagementClassApdapter
	{
		// Token: 0x06001343 RID: 4931 RVA: 0x00077F48 File Offset: 0x00076148
		protected override void AddAllProperties<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members)
		{
			base.AddAllProperties<T>(wmiObject, members);
			if (wmiObject.Properties != null)
			{
				foreach (PropertyData propertyData in wmiObject.Properties)
				{
					members.Add(new PSProperty(propertyData.Name, this, wmiObject, propertyData) as T);
				}
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00077FC4 File Offset: 0x000761C4
		protected override PSProperty DoGetProperty(ManagementBaseObject wmiObject, string propertyName)
		{
			PSProperty psproperty = base.DoGetProperty(wmiObject, propertyName);
			if (psproperty != null)
			{
				return psproperty;
			}
			try
			{
				PropertyData propertyData = wmiObject.Properties[propertyName];
				return new PSProperty(propertyData.Name, this, wmiObject, propertyData);
			}
			catch (ManagementException)
			{
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLogProvider psetwLogProvider = new PSEtwLogProvider();
				psetwLogProvider.WriteEvent(PSEventId.Engine_Health, PSChannel.Analytic, PSOpcode.Exception, PSLevel.Informational, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
				{
					string.Format(CultureInfo.InvariantCulture, "ManagementBaseObjectAdapter::DoGetProperty::PropertyName:{0}, Exception:{1}, StackTrace:{2}", new object[]
					{
						propertyName,
						ex.Message,
						ex.StackTrace
					}),
					string.Empty,
					string.Empty
				});
			}
			return null;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000780A4 File Offset: 0x000762A4
		protected override object InvokeManagementMethod(ManagementObject obj, string methodName, ManagementBaseObject inParams)
		{
			Adapter.tracer.WriteLine("Invoking class method: {0}", new object[]
			{
				methodName
			});
			object result;
			try
			{
				ManagementBaseObject managementBaseObject = obj.InvokeMethod(methodName, inParams, null);
				result = managementBaseObject;
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

		// Token: 0x06001346 RID: 4934 RVA: 0x0007811C File Offset: 0x0007631C
		protected override void AddAllMethods<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members)
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return;
			}
			CacheTable instanceMethodTable = BaseWMIAdapter.GetInstanceMethodTable(wmiObject, false);
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

		// Token: 0x06001347 RID: 4935 RVA: 0x000781DC File Offset: 0x000763DC
		protected override T GetManagementObjectMethod<T>(ManagementBaseObject wmiObject, string methodName)
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return default(T);
			}
			CacheTable instanceMethodTable = BaseWMIAdapter.GetInstanceMethodTable(wmiObject, false);
			BaseWMIAdapter.WMIMethodCacheEntry wmimethodCacheEntry = (BaseWMIAdapter.WMIMethodCacheEntry)instanceMethodTable[methodName];
			if (wmimethodCacheEntry == null)
			{
				return default(T);
			}
			return new PSMethod(wmimethodCacheEntry.Name, this, wmiObject, wmimethodCacheEntry) as T;
		}
	}
}
