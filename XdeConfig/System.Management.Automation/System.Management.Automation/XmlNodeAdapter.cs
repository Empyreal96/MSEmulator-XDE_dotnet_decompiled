using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000112 RID: 274
	internal class XmlNodeAdapter : PropertyOnlyAdapter
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x000516CC File Offset: 0x0004F8CC
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			XmlNode node = (XmlNode)obj;
			string nodeNamespace = node.NamespaceURI;
			IEnumerable<string> baseTypeNames = Adapter.GetDotNetTypeNameHierarchy(obj);
			if (string.IsNullOrEmpty(nodeNamespace))
			{
				foreach (string baseType in baseTypeNames)
				{
					yield return baseType;
				}
			}
			else
			{
				StringBuilder firstType = null;
				foreach (string baseType2 in baseTypeNames)
				{
					if (firstType == null)
					{
						firstType = new StringBuilder(baseType2);
						firstType.Append("#");
						firstType.Append(node.NamespaceURI);
						firstType.Append("#");
						firstType.Append(node.LocalName);
						yield return firstType.ToString();
					}
					yield return baseType2;
				}
			}
			yield break;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x000516F0 File Offset: 0x0004F8F0
		protected override void DoAddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members)
		{
			XmlNode xmlNode = (XmlNode)obj;
			Dictionary<string, List<XmlNode>> dictionary = new Dictionary<string, List<XmlNode>>(StringComparer.OrdinalIgnoreCase);
			if (xmlNode.Attributes != null)
			{
				foreach (object obj2 in xmlNode.Attributes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					List<XmlNode> list;
					if (!dictionary.TryGetValue(xmlNode2.LocalName, out list))
					{
						list = new List<XmlNode>();
						dictionary[xmlNode2.LocalName] = list;
					}
					list.Add(xmlNode2);
				}
			}
			if (xmlNode.ChildNodes != null)
			{
				foreach (object obj3 in xmlNode.ChildNodes)
				{
					XmlNode xmlNode3 = (XmlNode)obj3;
					if (!(xmlNode3 is XmlWhitespace))
					{
						List<XmlNode> list2;
						if (!dictionary.TryGetValue(xmlNode3.LocalName, out list2))
						{
							list2 = new List<XmlNode>();
							dictionary[xmlNode3.LocalName] = list2;
						}
						list2.Add(xmlNode3);
					}
				}
			}
			foreach (KeyValuePair<string, List<XmlNode>> keyValuePair in dictionary)
			{
				members.Add(new PSProperty(keyValuePair.Key, this, obj, keyValuePair.Value.ToArray()) as T);
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005187C File Offset: 0x0004FA7C
		protected override PSProperty DoGetProperty(object obj, string propertyName)
		{
			XmlNode[] array = XmlNodeAdapter.FindNodes(obj, propertyName, StringComparison.OrdinalIgnoreCase);
			if (array.Length == 0)
			{
				return null;
			}
			return new PSProperty(array[0].LocalName, this, obj, array);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000518AC File Offset: 0x0004FAAC
		protected override bool PropertyIsSettable(PSProperty property)
		{
			XmlNode[] array = (XmlNode[])property.adapterData;
			if (array.Length != 1)
			{
				return false;
			}
			XmlNode xmlNode = array[0];
			if (xmlNode is XmlText)
			{
				return true;
			}
			if (xmlNode is XmlAttribute)
			{
				return true;
			}
			XmlAttributeCollection attributes = xmlNode.Attributes;
			if (attributes != null && attributes.Count != 0)
			{
				return false;
			}
			XmlNodeList childNodes = xmlNode.ChildNodes;
			return childNodes == null || childNodes.Count == 0 || (childNodes.Count == 1 && childNodes[0].NodeType == XmlNodeType.Text);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00051928 File Offset: 0x0004FB28
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0005192C File Offset: 0x0004FB2C
		private static object GetNodeObject(XmlNode node)
		{
			XmlText xmlText = node as XmlText;
			if (xmlText != null)
			{
				return xmlText.InnerText;
			}
			XmlAttributeCollection attributes = node.Attributes;
			if (attributes != null && attributes.Count != 0)
			{
				return node;
			}
			if (!node.HasChildNodes)
			{
				return node.InnerText;
			}
			XmlNodeList childNodes = node.ChildNodes;
			if (childNodes.Count == 1 && childNodes[0].NodeType == XmlNodeType.Text)
			{
				return node.InnerText;
			}
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			return node;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000519A8 File Offset: 0x0004FBA8
		protected override object PropertyGet(PSProperty property)
		{
			XmlNode[] array = (XmlNode[])property.adapterData;
			if (array.Length == 1)
			{
				return XmlNodeAdapter.GetNodeObject(array[0]);
			}
			object[] array2 = new object[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = XmlNodeAdapter.GetNodeObject(array[i]);
			}
			return array2;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000519F4 File Offset: 0x0004FBF4
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			string text = setValue as string;
			if (text == null)
			{
				throw new SetValueException("XmlNodeSetShouldBeAString", null, ExtendedTypeSystem.XmlNodeSetShouldBeAString, new object[]
				{
					property.Name
				});
			}
			XmlNode[] array = (XmlNode[])property.adapterData;
			if (array.Length > 1)
			{
				throw new SetValueException("XmlNodeSetRestrictionsMoreThanOneNode", null, ExtendedTypeSystem.XmlNodeSetShouldBeAString, new object[]
				{
					property.Name
				});
			}
			XmlNode xmlNode = array[0];
			XmlText xmlText = xmlNode as XmlText;
			if (xmlText != null)
			{
				xmlText.InnerText = text;
				return;
			}
			XmlAttributeCollection attributes = xmlNode.Attributes;
			if (attributes != null && attributes.Count != 0)
			{
				throw new SetValueException("XmlNodeSetRestrictionsNodeWithAttributes", null, ExtendedTypeSystem.XmlNodeSetShouldBeAString, new object[]
				{
					property.Name
				});
			}
			XmlNodeList childNodes = xmlNode.ChildNodes;
			if (childNodes == null || childNodes.Count == 0)
			{
				xmlNode.InnerText = text;
				return;
			}
			if (childNodes.Count == 1 && childNodes[0].NodeType == XmlNodeType.Text)
			{
				xmlNode.InnerText = text;
				return;
			}
			XmlAttribute xmlAttribute = xmlNode as XmlAttribute;
			if (xmlAttribute != null)
			{
				xmlAttribute.Value = text;
				return;
			}
			throw new SetValueException("XmlNodeSetRestrictionsUnknownNodeType", null, ExtendedTypeSystem.XmlNodeSetShouldBeAString, new object[]
			{
				property.Name
			});
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00051B30 File Offset: 0x0004FD30
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

		// Token: 0x06000ECB RID: 3787 RVA: 0x00051B84 File Offset: 0x0004FD84
		private static XmlNode[] FindNodes(object obj, string propertyName, StringComparison comparisonType)
		{
			List<XmlNode> list = new List<XmlNode>();
			XmlNode xmlNode = (XmlNode)obj;
			if (xmlNode.Attributes != null)
			{
				foreach (object obj2 in xmlNode.Attributes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (xmlNode2.LocalName.Equals(propertyName, comparisonType))
					{
						list.Add(xmlNode2);
					}
				}
			}
			if (xmlNode.ChildNodes != null)
			{
				foreach (object obj3 in xmlNode.ChildNodes)
				{
					XmlNode xmlNode3 = (XmlNode)obj3;
					if (!(xmlNode3 is XmlWhitespace) && xmlNode3.LocalName.Equals(propertyName, comparisonType))
					{
						list.Add(xmlNode3);
					}
				}
			}
			return list.ToArray();
		}
	}
}
