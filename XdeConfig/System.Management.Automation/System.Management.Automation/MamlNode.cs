using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C1 RID: 449
	internal class MamlNode
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x00081D72 File Offset: 0x0007FF72
		internal MamlNode(XmlNode xmlNode)
		{
			this._xmlNode = xmlNode;
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x00081D8C File Offset: 0x0007FF8C
		internal XmlNode XmlNode
		{
			get
			{
				return this._xmlNode;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x00081D94 File Offset: 0x0007FF94
		internal PSObject PSObject
		{
			get
			{
				if (this._mshObject == null)
				{
					this.RemoveUnsupportedNodes(this._xmlNode);
					this._mshObject = this.GetPSObject(this._xmlNode);
				}
				return this._mshObject;
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00081DC4 File Offset: 0x0007FFC4
		private PSObject GetPSObject(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return new PSObject();
			}
			PSObject psobject = null;
			if (MamlNode.IsAtomic(xmlNode))
			{
				psobject = new PSObject(xmlNode.InnerText.Trim());
			}
			else if (MamlNode.IncludeMamlFormatting(xmlNode))
			{
				psobject = new PSObject(this.GetMamlFormattingPSObjects(xmlNode));
			}
			else
			{
				psobject = new PSObject(this.GetInsidePSObject(xmlNode));
				psobject.TypeNames.Clear();
				if (xmlNode.Attributes["type"] != null)
				{
					if (string.Compare(xmlNode.Attributes["type"].Value, "field", StringComparison.OrdinalIgnoreCase) == 0)
					{
						psobject.TypeNames.Add("MamlPSClassHelpInfo#field");
					}
					else if (string.Compare(xmlNode.Attributes["type"].Value, "method", StringComparison.OrdinalIgnoreCase) == 0)
					{
						psobject.TypeNames.Add("MamlPSClassHelpInfo#method");
					}
				}
				psobject.TypeNames.Add("MamlCommandHelpInfo#" + xmlNode.LocalName);
			}
			if (xmlNode.Attributes != null)
			{
				foreach (object obj in xmlNode.Attributes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					psobject.Properties.Add(new PSNoteProperty(xmlNode2.Name, xmlNode2.Value));
				}
			}
			return psobject;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00081F2C File Offset: 0x0008012C
		private PSObject GetInsidePSObject(XmlNode xmlNode)
		{
			Hashtable insideProperties = this.GetInsideProperties(xmlNode);
			PSObject psobject = new PSObject();
			IDictionaryEnumerator enumerator = insideProperties.GetEnumerator();
			while (enumerator.MoveNext())
			{
				psobject.Properties.Add(new PSNoteProperty((string)enumerator.Key, enumerator.Value));
			}
			return psobject;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00081F7C File Offset: 0x0008017C
		private Hashtable GetInsideProperties(XmlNode xmlNode)
		{
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			if (xmlNode == null)
			{
				return hashtable;
			}
			if (xmlNode.ChildNodes != null)
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					MamlNode.AddProperty(hashtable, xmlNode2.LocalName, this.GetPSObject(xmlNode2));
				}
			}
			return MamlNode.SimplifyProperties(hashtable);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00082000 File Offset: 0x00080200
		private void RemoveUnsupportedNodes(XmlNode xmlNode)
		{
			XmlNode xmlNode2 = xmlNode.FirstChild;
			while (xmlNode2 != null)
			{
				if (xmlNode2.NodeType == XmlNodeType.Comment)
				{
					XmlNode oldChild = xmlNode2;
					xmlNode2 = xmlNode2.NextSibling;
					xmlNode.RemoveChild(oldChild);
				}
				else
				{
					this.RemoveUnsupportedNodes(xmlNode2);
					xmlNode2 = xmlNode2.NextSibling;
				}
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00082044 File Offset: 0x00080244
		private static void AddProperty(Hashtable properties, string name, PSObject mshObject)
		{
			ArrayList arrayList = (ArrayList)properties[name];
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				properties[name] = arrayList;
			}
			if (mshObject == null)
			{
				return;
			}
			if (mshObject.BaseObject is PSCustomObject || !mshObject.BaseObject.GetType().Equals(typeof(PSObject[])))
			{
				arrayList.Add(mshObject);
				return;
			}
			PSObject[] array = (PSObject[])mshObject.BaseObject;
			for (int i = 0; i < array.Length; i++)
			{
				arrayList.Add(array[i]);
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000820CC File Offset: 0x000802CC
		private static Hashtable SimplifyProperties(Hashtable properties)
		{
			if (properties == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			IDictionaryEnumerator enumerator = properties.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ArrayList arrayList = (ArrayList)enumerator.Value;
				if (arrayList != null && arrayList.Count != 0)
				{
					if (arrayList.Count == 1 && !MamlNode.IsMamlFormattingPSObject((PSObject)arrayList[0]))
					{
						PSObject value = (PSObject)arrayList[0];
						hashtable[enumerator.Key] = value;
					}
					else
					{
						hashtable[enumerator.Key] = arrayList.ToArray(typeof(PSObject));
					}
				}
			}
			return hashtable;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00082168 File Offset: 0x00080368
		private static bool IsAtomic(XmlNode xmlNode)
		{
			return xmlNode != null && (xmlNode.ChildNodes == null || (xmlNode.ChildNodes.Count <= 1 && (xmlNode.ChildNodes.Count == 0 || xmlNode.ChildNodes[0].GetType().Equals(typeof(XmlText)))));
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000821C8 File Offset: 0x000803C8
		private static bool IncludeMamlFormatting(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.ChildNodes == null || xmlNode.ChildNodes.Count == 0)
			{
				return false;
			}
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (MamlNode.IsMamlFormattingNode(xmlNode2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00082248 File Offset: 0x00080448
		private static bool IsMamlFormattingNode(XmlNode xmlNode)
		{
			return xmlNode.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase) || xmlNode.LocalName.Equals("list", StringComparison.OrdinalIgnoreCase) || xmlNode.LocalName.Equals("definitionList", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00082298 File Offset: 0x00080498
		private static bool IsMamlFormattingPSObject(PSObject mshObject)
		{
			Collection<string> typeNames = mshObject.TypeNames;
			return typeNames != null && typeNames.Count != 0 && typeNames[typeNames.Count - 1].Equals("MamlTextItem", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x000822D4 File Offset: 0x000804D4
		private PSObject[] GetMamlFormattingPSObjects(XmlNode xmlNode)
		{
			ArrayList arrayList = new ArrayList();
			int paraMamlNodeCount = this.GetParaMamlNodeCount(xmlNode.ChildNodes);
			int num = 0;
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase))
				{
					num++;
					PSObject paraPSObject = MamlNode.GetParaPSObject(xmlNode2, num != paraMamlNodeCount);
					if (paraPSObject != null)
					{
						arrayList.Add(paraPSObject);
					}
				}
				else if (xmlNode2.LocalName.Equals("list", StringComparison.OrdinalIgnoreCase))
				{
					ArrayList listPSObjects = this.GetListPSObjects(xmlNode2);
					for (int i = 0; i < listPSObjects.Count; i++)
					{
						arrayList.Add(listPSObjects[i]);
					}
				}
				else if (xmlNode2.LocalName.Equals("definitionList", StringComparison.OrdinalIgnoreCase))
				{
					ArrayList definitionListPSObjects = this.GetDefinitionListPSObjects(xmlNode2);
					for (int j = 0; j < definitionListPSObjects.Count; j++)
					{
						arrayList.Add(definitionListPSObjects[j]);
					}
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			return (PSObject[])arrayList.ToArray(typeof(PSObject));
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00082428 File Offset: 0x00080628
		private int GetParaMamlNodeCount(XmlNodeList nodes)
		{
			int num = 0;
			foreach (object obj in nodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase) && !xmlNode.InnerText.Trim().Equals(string.Empty))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000824A8 File Offset: 0x000806A8
		private void WriteMamlInvalidChildNodeError(XmlNode node, XmlNode childNode)
		{
			ErrorRecord errorRecord = new ErrorRecord(new ParentContainsErrorRecordException("MamlInvalidChildNodeError"), "MamlInvalidChildNodeError", ErrorCategory.SyntaxError, null);
			errorRecord.ErrorDetails = new ErrorDetails(typeof(MamlNode).GetTypeInfo().Assembly, "HelpErrors", "MamlInvalidChildNodeError", new object[]
			{
				node.LocalName,
				childNode.LocalName,
				MamlNode.GetNodePath(node)
			});
			this.Errors.Add(errorRecord);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00082528 File Offset: 0x00080728
		private void WriteMamlInvalidChildNodeCountError(XmlNode node, string childNodeName, int count)
		{
			ErrorRecord errorRecord = new ErrorRecord(new ParentContainsErrorRecordException("MamlInvalidChildNodeCountError"), "MamlInvalidChildNodeCountError", ErrorCategory.SyntaxError, null);
			errorRecord.ErrorDetails = new ErrorDetails(typeof(MamlNode).GetTypeInfo().Assembly, "HelpErrors", "MamlInvalidChildNodeCountError", new object[]
			{
				node.LocalName,
				childNodeName,
				count,
				MamlNode.GetNodePath(node)
			});
			this.Errors.Add(errorRecord);
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000825AC File Offset: 0x000807AC
		private static string GetNodePath(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return "";
			}
			if (xmlNode.ParentNode == null)
			{
				return "\\" + xmlNode.LocalName;
			}
			return MamlNode.GetNodePath(xmlNode.ParentNode) + "\\" + xmlNode.LocalName + MamlNode.GetNodeIndex(xmlNode);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000825FC File Offset: 0x000807FC
		private static string GetNodeIndex(XmlNode xmlNode)
		{
			if (xmlNode == null || xmlNode.ParentNode == null)
			{
				return "";
			}
			int num = 0;
			int num2 = 0;
			foreach (object obj in xmlNode.ParentNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2 == xmlNode)
				{
					num = num2++;
				}
				else if (xmlNode2.LocalName.Equals(xmlNode.LocalName, StringComparison.OrdinalIgnoreCase))
				{
					num2++;
				}
			}
			if (num2 > 1)
			{
				return "[" + num.ToString("d", CultureInfo.CurrentCulture) + "]";
			}
			return "";
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000826BC File Offset: 0x000808BC
		private static PSObject GetParaPSObject(XmlNode xmlNode, bool newLine)
		{
			if (xmlNode == null)
			{
				return null;
			}
			if (!xmlNode.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			PSObject psobject = new PSObject();
			StringBuilder stringBuilder = new StringBuilder();
			if (newLine && !xmlNode.InnerText.Trim().Equals(string.Empty))
			{
				stringBuilder.AppendLine(xmlNode.InnerText.Trim());
			}
			else
			{
				stringBuilder.Append(xmlNode.InnerText.Trim());
			}
			psobject.Properties.Add(new PSNoteProperty("Text", stringBuilder.ToString()));
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add("MamlParaTextItem");
			psobject.TypeNames.Add("MamlTextItem");
			return psobject;
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00082778 File Offset: 0x00080978
		private ArrayList GetListPSObjects(XmlNode xmlNode)
		{
			ArrayList arrayList = new ArrayList();
			if (xmlNode == null)
			{
				return arrayList;
			}
			if (!xmlNode.LocalName.Equals("list", StringComparison.OrdinalIgnoreCase))
			{
				return arrayList;
			}
			if (xmlNode.ChildNodes == null || xmlNode.ChildNodes.Count == 0)
			{
				return arrayList;
			}
			bool ordered = MamlNode.IsOrderedList(xmlNode);
			int num = 1;
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("listItem", StringComparison.OrdinalIgnoreCase))
				{
					PSObject listItemPSObject = this.GetListItemPSObject(xmlNode2, ordered, ref num);
					if (listItemPSObject != null)
					{
						arrayList.Add(listItemPSObject);
					}
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			return arrayList;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00082848 File Offset: 0x00080A48
		private static bool IsOrderedList(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return false;
			}
			if (xmlNode.Attributes == null || xmlNode.Attributes.Count == 0)
			{
				return false;
			}
			foreach (object obj in xmlNode.Attributes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.Name.Equals("class", StringComparison.OrdinalIgnoreCase) && xmlNode2.Value.Equals("ordered", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000828E4 File Offset: 0x00080AE4
		private PSObject GetListItemPSObject(XmlNode xmlNode, bool ordered, ref int index)
		{
			if (xmlNode == null)
			{
				return null;
			}
			if (!xmlNode.LocalName.Equals("listItem", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string value = "";
			if (xmlNode.ChildNodes.Count > 1)
			{
				this.WriteMamlInvalidChildNodeCountError(xmlNode, "para", 1);
			}
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase))
				{
					value = xmlNode2.InnerText.Trim();
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			string text;
			if (ordered)
			{
				text = index.ToString("d2", CultureInfo.CurrentCulture);
				text += ". ";
				index++;
			}
			else
			{
				text = "* ";
			}
			PSObject psobject = new PSObject();
			psobject.Properties.Add(new PSNoteProperty("Text", value));
			psobject.Properties.Add(new PSNoteProperty("Tag", text));
			psobject.TypeNames.Clear();
			if (ordered)
			{
				psobject.TypeNames.Add("MamlOrderedListTextItem");
			}
			else
			{
				psobject.TypeNames.Add("MamlUnorderedListTextItem");
			}
			psobject.TypeNames.Add("MamlTextItem");
			return psobject;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00082A48 File Offset: 0x00080C48
		private ArrayList GetDefinitionListPSObjects(XmlNode xmlNode)
		{
			ArrayList arrayList = new ArrayList();
			if (xmlNode == null)
			{
				return arrayList;
			}
			if (!xmlNode.LocalName.Equals("definitionList", StringComparison.OrdinalIgnoreCase))
			{
				return arrayList;
			}
			if (xmlNode.ChildNodes == null || xmlNode.ChildNodes.Count == 0)
			{
				return arrayList;
			}
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("definitionListItem", StringComparison.OrdinalIgnoreCase))
				{
					PSObject definitionListItemPSObject = this.GetDefinitionListItemPSObject(xmlNode2);
					if (definitionListItemPSObject != null)
					{
						arrayList.Add(definitionListItemPSObject);
					}
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			return arrayList;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00082B08 File Offset: 0x00080D08
		private PSObject GetDefinitionListItemPSObject(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return null;
			}
			if (!xmlNode.LocalName.Equals("definitionListItem", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			string value = null;
			string value2 = null;
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("term", StringComparison.OrdinalIgnoreCase))
				{
					value = xmlNode2.InnerText.Trim();
				}
				else if (xmlNode2.LocalName.Equals("definition", StringComparison.OrdinalIgnoreCase))
				{
					value2 = this.GetDefinitionText(xmlNode2);
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			PSObject psobject = new PSObject();
			psobject.Properties.Add(new PSNoteProperty("Term", value));
			psobject.Properties.Add(new PSNoteProperty("Definition", value2));
			psobject.TypeNames.Clear();
			psobject.TypeNames.Add("MamlDefinitionTextItem");
			psobject.TypeNames.Add("MamlTextItem");
			return psobject;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00082C30 File Offset: 0x00080E30
		private string GetDefinitionText(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return null;
			}
			if (!xmlNode.LocalName.Equals("definition", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			if (xmlNode.ChildNodes == null || xmlNode.ChildNodes.Count == 0)
			{
				return "";
			}
			if (xmlNode.ChildNodes.Count > 1)
			{
				this.WriteMamlInvalidChildNodeCountError(xmlNode, "para", 1);
			}
			string result = "";
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase))
				{
					result = xmlNode2.InnerText.Trim();
				}
				else
				{
					this.WriteMamlInvalidChildNodeError(xmlNode, xmlNode2);
				}
			}
			return result;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00082D04 File Offset: 0x00080F04
		private static string GetPreformattedText(string text)
		{
			string text2 = text.Replace("\t", "    ");
			string[] lines = text2.Split(new char[]
			{
				'\n'
			});
			string[] array = MamlNode.TrimLines(lines);
			if (array == null || array.Length == 0)
			{
				return "";
			}
			int minIndentation = MamlNode.GetMinIndentation(array);
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (MamlNode.IsEmptyLine(array[i]))
				{
					array2[i] = array[i];
				}
				else
				{
					array2[i] = array[i].Remove(0, minIndentation);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.AppendLine(array2[j]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00082DC8 File Offset: 0x00080FC8
		private static string[] TrimLines(string[] lines)
		{
			if (lines == null || lines.Length == 0)
			{
				return null;
			}
			int i = 0;
			while (i < lines.Length && MamlNode.IsEmptyLine(lines[i]))
			{
				i++;
			}
			int num = i;
			if (num == lines.Length)
			{
				return null;
			}
			i = lines.Length - 1;
			while (i >= num && MamlNode.IsEmptyLine(lines[i]))
			{
				i--;
			}
			int num2 = i;
			string[] array = new string[num2 - num + 1];
			for (i = num; i <= num2; i++)
			{
				array[i - num] = lines[i];
			}
			return array;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00082E40 File Offset: 0x00081040
		private static int GetMinIndentation(string[] lines)
		{
			int num = -1;
			for (int i = 0; i < lines.Length; i++)
			{
				if (!MamlNode.IsEmptyLine(lines[i]))
				{
					int indentation = MamlNode.GetIndentation(lines[i]);
					if (num < 0 || indentation < num)
					{
						num = indentation;
					}
				}
			}
			return num;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00082E7C File Offset: 0x0008107C
		private static int GetIndentation(string line)
		{
			if (MamlNode.IsEmptyLine(line))
			{
				return 0;
			}
			string text = line.TrimStart(new char[]
			{
				' '
			});
			return line.Length - text.Length;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00082EB4 File Offset: 0x000810B4
		private static bool IsEmptyLine(string line)
		{
			if (string.IsNullOrEmpty(line))
			{
				return true;
			}
			string value = line.Trim();
			return string.IsNullOrEmpty(value);
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x00082EDD File Offset: 0x000810DD
		internal Collection<ErrorRecord> Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x040008EB RID: 2283
		private XmlNode _xmlNode;

		// Token: 0x040008EC RID: 2284
		private PSObject _mshObject;

		// Token: 0x040008ED RID: 2285
		private Collection<ErrorRecord> _errors = new Collection<ErrorRecord>();
	}
}
