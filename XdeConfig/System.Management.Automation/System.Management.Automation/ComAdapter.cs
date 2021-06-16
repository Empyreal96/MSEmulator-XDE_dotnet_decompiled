using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000177 RID: 375
	internal class ComAdapter : Adapter
	{
		// Token: 0x060012B1 RID: 4785 RVA: 0x000743C9 File Offset: 0x000725C9
		internal ComAdapter(ComTypeInfo typeinfo)
		{
			this._comTypeInfo = typeinfo;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000743D8 File Offset: 0x000725D8
		internal static string GetComTypeName(string clsid)
		{
			StringBuilder stringBuilder = new StringBuilder("System.__ComObject");
			stringBuilder.Append("#{");
			stringBuilder.Append(clsid);
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000745E0 File Offset: 0x000727E0
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			yield return ComAdapter.GetComTypeName(this._comTypeInfo.Clsid);
			foreach (string baseType in Adapter.GetDotNetTypeNameHierarchy(obj))
			{
				yield return baseType;
			}
			yield break;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00074604 File Offset: 0x00072804
		protected override T GetMember<T>(object obj, string memberName)
		{
			if (this._comTypeInfo.Properties.ContainsKey(memberName))
			{
				ComProperty comProperty = this._comTypeInfo.Properties[memberName];
				if (comProperty.IsParameterized)
				{
					if (typeof(T).IsAssignableFrom(typeof(PSParameterizedProperty)))
					{
						return new PSParameterizedProperty(comProperty.Name, this, obj, comProperty) as T;
					}
				}
				else if (typeof(T).IsAssignableFrom(typeof(PSProperty)))
				{
					return new PSProperty(comProperty.Name, this, obj, comProperty) as T;
				}
			}
			if (typeof(T).IsAssignableFrom(typeof(PSMethod)) && this._comTypeInfo != null && this._comTypeInfo.Methods.ContainsKey(memberName))
			{
				ComMethod comMethod = this._comTypeInfo.Methods[memberName];
				PSMethod psmethod = new PSMethod(comMethod.Name, this, obj, comMethod);
				return psmethod as T;
			}
			return default(T);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00074718 File Offset: 0x00072918
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			bool flag = typeof(T).IsAssignableFrom(typeof(PSProperty));
			bool flag2 = typeof(T).IsAssignableFrom(typeof(PSParameterizedProperty));
			if (flag || flag2)
			{
				foreach (ComProperty comProperty in this._comTypeInfo.Properties.Values)
				{
					if (comProperty.IsParameterized)
					{
						if (flag2)
						{
							psmemberInfoInternalCollection.Add(new PSParameterizedProperty(comProperty.Name, this, obj, comProperty) as T);
						}
					}
					else if (flag)
					{
						psmemberInfoInternalCollection.Add(new PSProperty(comProperty.Name, this, obj, comProperty) as T);
					}
				}
			}
			bool flag3 = typeof(T).IsAssignableFrom(typeof(PSMethod));
			if (flag3)
			{
				foreach (ComMethod comMethod in this._comTypeInfo.Methods.Values)
				{
					if (psmemberInfoInternalCollection[comMethod.Name] == null)
					{
						PSMethod psmethod = new PSMethod(comMethod.Name, this, obj, comMethod);
						psmemberInfoInternalCollection.Add(psmethod as T);
					}
				}
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000748A0 File Offset: 0x00072AA0
		protected override AttributeCollection PropertyAttributes(PSProperty property)
		{
			return new AttributeCollection(new Attribute[0]);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000748B0 File Offset: 0x00072AB0
		protected override object PropertyGet(PSProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.GetValue(property.baseObject);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000748D8 File Offset: 0x00072AD8
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			comProperty.SetValue(property.baseObject, setValue);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00074900 File Offset: 0x00072B00
		protected override bool PropertyIsSettable(PSProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.IsSettable;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00074920 File Offset: 0x00072B20
		protected override bool PropertyIsGettable(PSProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.IsGettable;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00074940 File Offset: 0x00072B40
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			if (!forDisplay)
			{
				return comProperty.Type.FullName;
			}
			return ToStringCodeMethods.Type(comProperty.Type, false);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00074974 File Offset: 0x00072B74
		protected override string PropertyToString(PSProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.ToString();
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00074994 File Offset: 0x00072B94
		protected override object MethodInvoke(PSMethod method, object[] arguments)
		{
			ComMethod comMethod = (ComMethod)method.adapterData;
			return comMethod.InvokeMethod(method, arguments);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x000749B8 File Offset: 0x00072BB8
		protected override Collection<string> MethodDefinitions(PSMethod method)
		{
			ComMethod comMethod = (ComMethod)method.adapterData;
			return comMethod.MethodDefinitions();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x000749D8 File Offset: 0x00072BD8
		protected override string ParameterizedPropertyType(PSParameterizedProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.Type.FullName;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000749FC File Offset: 0x00072BFC
		protected override bool ParameterizedPropertyIsSettable(PSParameterizedProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.IsSettable;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00074A1C File Offset: 0x00072C1C
		protected override bool ParameterizedPropertyIsGettable(PSParameterizedProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.IsGettable;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00074A3C File Offset: 0x00072C3C
		protected override object ParameterizedPropertyGet(PSParameterizedProperty property, object[] arguments)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.GetValue(property.baseObject, arguments);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00074A64 File Offset: 0x00072C64
		protected override void ParameterizedPropertySet(PSParameterizedProperty property, object setValue, object[] arguments)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			comProperty.SetValue(property.baseObject, setValue, arguments);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00074A8C File Offset: 0x00072C8C
		protected override string ParameterizedPropertyToString(PSParameterizedProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return comProperty.ToString();
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00074AAC File Offset: 0x00072CAC
		protected override Collection<string> ParameterizedPropertyDefinitions(PSParameterizedProperty property)
		{
			ComProperty comProperty = (ComProperty)property.adapterData;
			return new Collection<string>
			{
				comProperty.GetDefinition()
			};
		}

		// Token: 0x040007F2 RID: 2034
		private readonly ComTypeInfo _comTypeInfo;
	}
}
