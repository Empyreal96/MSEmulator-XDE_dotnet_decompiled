using System;
using System.DirectoryServices;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Microsoft.PowerShell
{
	// Token: 0x02000157 RID: 343
	public static class ToStringCodeMethods
	{
		// Token: 0x0600117D RID: 4477 RVA: 0x00060864 File Offset: 0x0005EA64
		private static void AddGenericArguments(StringBuilder sb, Type[] genericArguments, bool dropNamespaces)
		{
			sb.Append('[');
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (i > 0)
				{
					sb.Append(',');
				}
				sb.Append(ToStringCodeMethods.Type(genericArguments[i], dropNamespaces));
			}
			sb.Append(']');
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000608B0 File Offset: 0x0005EAB0
		internal static string Type(Type type, bool dropNamespaces = false)
		{
			if (type == null)
			{
				return string.Empty;
			}
			TypeInfo typeInfo = type.GetTypeInfo();
			string text2;
			if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
			{
				string text = ToStringCodeMethods.Type(type.GetGenericTypeDefinition(), dropNamespaces);
				int length = text.LastIndexOf(typeInfo.IsNested ? '[' : '`');
				StringBuilder stringBuilder = new StringBuilder(text, 0, length, 512);
				ToStringCodeMethods.AddGenericArguments(stringBuilder, type.GetGenericArguments(), dropNamespaces);
				text2 = stringBuilder.ToString();
			}
			else if (typeInfo.IsArray)
			{
				string text3 = ToStringCodeMethods.Type(type.GetElementType(), dropNamespaces);
				StringBuilder stringBuilder2 = new StringBuilder(text3, text3.Length + 10);
				stringBuilder2.Append("[");
				for (int i = 0; i < type.GetArrayRank() - 1; i++)
				{
					stringBuilder2.Append(",");
				}
				stringBuilder2.Append("]");
				text2 = stringBuilder2.ToString();
			}
			else
			{
				text2 = TypeAccelerators.FindBuiltinAccelerator(type);
				if (text2 == null)
				{
					if (dropNamespaces)
					{
						if (typeInfo.IsNested)
						{
							string text4 = type.ToString();
							text2 = ((type.Namespace == null) ? text4 : text4.Substring(type.Namespace.Length + 1));
						}
						else
						{
							text2 = type.Name;
						}
					}
					else
					{
						text2 = type.ToString();
					}
				}
			}
			if (!typeInfo.IsGenericParameter && !typeInfo.ContainsGenericParameters && !dropNamespaces && !typeInfo.Assembly.GetCustomAttributes(typeof(DynamicClassImplementationAssemblyAttribute)).Any<Attribute>())
			{
				Type left;
				TypeResolver.TryResolveType(text2, out left);
				if (left != type)
				{
					text2 = type.AssemblyQualifiedName;
				}
			}
			return text2;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00060A3A File Offset: 0x0005EC3A
		public static string Type(PSObject instance)
		{
			if (instance == null)
			{
				return string.Empty;
			}
			return ToStringCodeMethods.Type((Type)instance.BaseObject, false);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00060A58 File Offset: 0x0005EC58
		public static string XmlNode(PSObject instance)
		{
			if (instance == null)
			{
				return string.Empty;
			}
			XmlNode xmlNode = (XmlNode)instance.BaseObject;
			if (xmlNode == null)
			{
				return string.Empty;
			}
			return xmlNode.LocalName;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00060A8C File Offset: 0x0005EC8C
		public static string XmlNodeList(PSObject instance)
		{
			if (instance == null)
			{
				return string.Empty;
			}
			XmlNodeList xmlNodeList = (XmlNodeList)instance.BaseObject;
			if (xmlNodeList == null)
			{
				return string.Empty;
			}
			if (xmlNodeList.Count != 1)
			{
				return PSObject.ToStringEnumerable(null, xmlNodeList, null, null, null);
			}
			if (xmlNodeList[0] == null)
			{
				return string.Empty;
			}
			return PSObject.AsPSObject(xmlNodeList[0]).ToString();
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00060AEC File Offset: 0x0005ECEC
		public static string PropertyValueCollection(PSObject instance)
		{
			if (instance == null)
			{
				return string.Empty;
			}
			PropertyValueCollection propertyValueCollection = (PropertyValueCollection)instance.BaseObject;
			if (propertyValueCollection == null)
			{
				return string.Empty;
			}
			if (propertyValueCollection.Count != 1)
			{
				return PSObject.ToStringEnumerable(null, propertyValueCollection, null, null, null);
			}
			if (propertyValueCollection[0] == null)
			{
				return string.Empty;
			}
			return PSObject.AsPSObject(propertyValueCollection[0]).ToString();
		}
	}
}
