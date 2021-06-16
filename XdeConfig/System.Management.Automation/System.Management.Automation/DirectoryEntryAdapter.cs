using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000186 RID: 390
	internal class DirectoryEntryAdapter : DotNetAdapter
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00076473 File Offset: 0x00074673
		internal override bool SiteBinderCanOptimize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00076478 File Offset: 0x00074678
		protected override T GetMember<T>(object obj, string memberName)
		{
			DirectoryEntry directoryEntry = (DirectoryEntry)obj;
			PropertyValueCollection propertyValueCollection = directoryEntry.Properties[memberName];
			object obj2 = propertyValueCollection;
			PSProperty psproperty;
			try
			{
				object obj3 = directoryEntry.InvokeGet(memberName);
				if (propertyValueCollection == null || (propertyValueCollection.Value == null && obj3 != null))
				{
					obj2 = obj3;
				}
				psproperty = new PSProperty(propertyValueCollection.PropertyName, this, obj, obj2);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				psproperty = null;
			}
			if (obj2 == null)
			{
				psproperty = null;
			}
			if (typeof(T).IsAssignableFrom(typeof(PSProperty)) && psproperty != null)
			{
				return psproperty as T;
			}
			if (typeof(T).IsAssignableFrom(typeof(PSMethod)) && psproperty == null && base.GetDotNetProperty<T>(obj, memberName) == null)
			{
				return new PSMethod(memberName, this, obj, null) as T;
			}
			return default(T);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00076560 File Offset: 0x00074760
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			DirectoryEntry directoryEntry = (DirectoryEntry)obj;
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			if (directoryEntry.Properties == null || directoryEntry.Properties.PropertyNames == null)
			{
				return null;
			}
			int num = 0;
			try
			{
				num = directoryEntry.Properties.PropertyNames.Count;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			if (num > 0)
			{
				foreach (object obj2 in directoryEntry.Properties)
				{
					PropertyValueCollection propertyValueCollection = (PropertyValueCollection)obj2;
					psmemberInfoInternalCollection.Add(new PSProperty(propertyValueCollection.PropertyName, this, obj, propertyValueCollection) as T);
				}
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00076630 File Offset: 0x00074830
		protected override object PropertyGet(PSProperty property)
		{
			return property.adapterData;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00076638 File Offset: 0x00074838
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			PropertyValueCollection propertyValueCollection = property.adapterData as PropertyValueCollection;
			if (propertyValueCollection != null)
			{
				try
				{
					propertyValueCollection.Clear();
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147467259 || setValue == null)
					{
						throw;
					}
				}
				IEnumerable enumerable = LanguagePrimitives.GetEnumerable(setValue);
				if (enumerable == null)
				{
					propertyValueCollection.Add(setValue);
					return;
				}
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object value = enumerator.Current;
						propertyValueCollection.Add(value);
					}
					return;
				}
			}
			DirectoryEntry directoryEntry = (DirectoryEntry)property.baseObject;
			List<object> list = new List<object>();
			IEnumerable enumerable2 = LanguagePrimitives.GetEnumerable(setValue);
			if (enumerable2 == null)
			{
				list.Add(setValue);
			}
			else
			{
				foreach (object item in enumerable2)
				{
					list.Add(item);
				}
			}
			directoryEntry.InvokeSet(property.name, list.ToArray());
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00076764 File Offset: 0x00074964
		protected override bool PropertyIsSettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00076767 File Offset: 0x00074967
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0007676C File Offset: 0x0007496C
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			object obj = null;
			try
			{
				obj = base.BasePropertyGet(property);
			}
			catch (GetValueException)
			{
			}
			Type type = (obj == null) ? typeof(object) : obj.GetType();
			if (!forDisplay)
			{
				return type.FullName;
			}
			return ToStringCodeMethods.Type(type, false);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000767C0 File Offset: 0x000749C0
		protected override object MethodInvoke(PSMethod method, PSMethodInvocationConstraints invocationConstraints, object[] arguments)
		{
			return this.MethodInvoke(method, arguments);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x000767CC File Offset: 0x000749CC
		protected override object MethodInvoke(PSMethod method, object[] arguments)
		{
			ParameterInformation[] array = new ParameterInformation[arguments.Length];
			for (int i = 0; i < arguments.Length; i++)
			{
				array[i] = new ParameterInformation(typeof(object), false, null, false);
			}
			MethodInformation[] methods = new MethodInformation[]
			{
				new MethodInformation(false, false, array)
			};
			object[] args;
			Adapter.GetBestMethodAndArguments(method.Name, methods, arguments, out args);
			DirectoryEntry directoryEntry = (DirectoryEntry)method.baseObject;
			Exception ex2;
			try
			{
				return directoryEntry.Invoke(method.Name, args);
			}
			catch (DirectoryServicesCOMException ex)
			{
				ex2 = ex;
			}
			catch (TargetInvocationException ex3)
			{
				ex2 = ex3;
			}
			catch (COMException ex4)
			{
				ex2 = ex4;
			}
			PSMethod dotNetMethod = DirectoryEntryAdapter.dotNetAdapter.GetDotNetMethod<PSMethod>(method.baseObject, method.name);
			if (dotNetMethod != null)
			{
				return dotNetMethod.Invoke(arguments);
			}
			throw ex2;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000768B0 File Offset: 0x00074AB0
		protected override string MethodToString(PSMethod method)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.MethodDefinitions(method))
			{
				stringBuilder.Append(value);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return stringBuilder.ToString();
		}

		// Token: 0x04000839 RID: 2105
		private static readonly DotNetAdapter dotNetAdapter = new DotNetAdapter();
	}
}
