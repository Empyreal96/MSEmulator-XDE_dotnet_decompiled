using System;
using System.Collections.Generic;
using System.Management.Automation.Help;

namespace System.Management.Automation
{
	// Token: 0x020001A8 RID: 424
	internal class MamlUtil
	{
		// Token: 0x060013CC RID: 5068 RVA: 0x00079794 File Offset: 0x00077994
		internal static void OverrideName(PSObject maml1, PSObject maml2)
		{
			MamlUtil.PrependPropertyValue(maml1, maml2, new string[]
			{
				"Name"
			}, true);
			MamlUtil.PrependPropertyValue(maml1, maml2, new string[]
			{
				"Details",
				"Name"
			}, true);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000797DC File Offset: 0x000779DC
		internal static void OverridePSTypeNames(PSObject maml1, PSObject maml2)
		{
			foreach (string text in maml2.TypeNames)
			{
				if (text.StartsWith(DefaultCommandHelpObjectBuilder.TypeNameForDefaultHelp, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			maml1.TypeNames.Clear();
			foreach (string item in maml2.TypeNames)
			{
				maml1.TypeNames.Add(item);
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00079880 File Offset: 0x00077A80
		internal static void AddCommonProperties(PSObject maml1, PSObject maml2)
		{
			if (maml1.Properties["PSSnapIn"] == null)
			{
				PSPropertyInfo pspropertyInfo = maml2.Properties["PSSnapIn"];
				if (pspropertyInfo != null)
				{
					maml1.Properties.Add(new PSNoteProperty("PSSnapIn", pspropertyInfo.Value));
				}
			}
			if (maml1.Properties["ModuleName"] == null)
			{
				PSPropertyInfo pspropertyInfo2 = maml2.Properties["ModuleName"];
				if (pspropertyInfo2 != null)
				{
					maml1.Properties.Add(new PSNoteProperty("ModuleName", pspropertyInfo2.Value));
				}
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00079910 File Offset: 0x00077B10
		internal static void PrependSyntax(PSObject maml1, PSObject maml2)
		{
			MamlUtil.PrependPropertyValue(maml1, maml2, new string[]
			{
				"Syntax",
				"SyntaxItem"
			}, false);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00079940 File Offset: 0x00077B40
		internal static void PrependDetailedDescription(PSObject maml1, PSObject maml2)
		{
			MamlUtil.PrependPropertyValue(maml1, maml2, new string[]
			{
				"Description"
			}, false);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00079968 File Offset: 0x00077B68
		internal static void OverrideParameters(PSObject maml1, PSObject maml2)
		{
			string[] path = new string[]
			{
				"Parameters",
				"Parameter"
			};
			List<object> list = new List<object>();
			PSPropertyInfo properyInfo = MamlUtil.GetProperyInfo(maml2, path);
			Array array = properyInfo.Value as Array;
			if (array != null)
			{
				list.AddRange(array as IEnumerable<object>);
			}
			else
			{
				list.Add(PSObject.AsPSObject(properyInfo.Value));
			}
			MamlUtil.EnsurePropertyInfoPathExists(maml1, path);
			PSPropertyInfo properyInfo2 = MamlUtil.GetProperyInfo(maml1, path);
			List<object> list2 = new List<object>();
			array = (properyInfo2.Value as Array);
			if (array != null)
			{
				list2.AddRange(array as IEnumerable<object>);
			}
			else
			{
				list2.Add(PSObject.AsPSObject(properyInfo2.Value));
			}
			for (int i = 0; i < list.Count; i++)
			{
				PSObject psobject = PSObject.AsPSObject(list[i]);
				string value = "";
				PSPropertyInfo pspropertyInfo = psobject.Properties["Name"];
				if (pspropertyInfo == null || LanguagePrimitives.TryConvertTo<string>(pspropertyInfo.Value, out value))
				{
					bool flag = false;
					foreach (object obj in list2)
					{
						PSObject psobject2 = (PSObject)obj;
						string text = "";
						PSPropertyInfo pspropertyInfo2 = psobject2.Properties["Name"];
						if ((pspropertyInfo2 == null || LanguagePrimitives.TryConvertTo<string>(pspropertyInfo2.Value, out text)) && text.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						list2.Add(list[i]);
					}
				}
			}
			if (list2.Count == 1)
			{
				properyInfo2.Value = list2[0];
				return;
			}
			if (list2.Count >= 2)
			{
				properyInfo2.Value = list2.ToArray();
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00079B3C File Offset: 0x00077D3C
		internal static void PrependNotes(PSObject maml1, PSObject maml2)
		{
			MamlUtil.PrependPropertyValue(maml1, maml2, new string[]
			{
				"AlertSet",
				"Alert"
			}, false);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00079B6C File Offset: 0x00077D6C
		internal static PSPropertyInfo GetProperyInfo(PSObject psObject, string[] path)
		{
			if (path.Length <= 0)
			{
				return null;
			}
			for (int i = 0; i < path.Length; i++)
			{
				string name = path[i];
				PSPropertyInfo pspropertyInfo = psObject.Properties[name];
				if (i == path.Length - 1)
				{
					return pspropertyInfo;
				}
				if (pspropertyInfo == null || !(pspropertyInfo.Value is PSObject))
				{
					return null;
				}
				psObject = (PSObject)pspropertyInfo.Value;
			}
			return null;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00079BCC File Offset: 0x00077DCC
		internal static void PrependPropertyValue(PSObject maml1, PSObject maml2, string[] path, bool shouldOverride)
		{
			List<object> list = new List<object>();
			PSPropertyInfo properyInfo = MamlUtil.GetProperyInfo(maml2, path);
			if (properyInfo != null)
			{
				Array array = properyInfo.Value as Array;
				if (array != null)
				{
					list.AddRange(properyInfo.Value as IEnumerable<object>);
				}
				else
				{
					list.Add(properyInfo.Value);
				}
			}
			MamlUtil.EnsurePropertyInfoPathExists(maml1, path);
			PSPropertyInfo properyInfo2 = MamlUtil.GetProperyInfo(maml1, path);
			if (properyInfo2 != null)
			{
				if (!shouldOverride)
				{
					Array array2 = properyInfo2.Value as Array;
					if (array2 != null)
					{
						list.AddRange(properyInfo2.Value as IEnumerable<object>);
					}
					else
					{
						list.Add(properyInfo2.Value);
					}
				}
				if (list.Count == 1)
				{
					properyInfo2.Value = list[0];
					return;
				}
				if (list.Count >= 2)
				{
					properyInfo2.Value = list.ToArray();
				}
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00079C8C File Offset: 0x00077E8C
		internal static void EnsurePropertyInfoPathExists(PSObject psObject, string[] path)
		{
			if (path.Length <= 0)
			{
				return;
			}
			for (int i = 0; i < path.Length; i++)
			{
				string name = path[i];
				PSPropertyInfo pspropertyInfo = psObject.Properties[name];
				if (pspropertyInfo == null)
				{
					object value = (i < path.Length - 1) ? new PSObject() : null;
					pspropertyInfo = new PSNoteProperty(name, value);
					psObject.Properties.Add(pspropertyInfo);
				}
				if (i == path.Length - 1)
				{
					return;
				}
				if (pspropertyInfo.Value == null || !(pspropertyInfo.Value is PSObject))
				{
					pspropertyInfo.Value = new PSObject();
				}
				psObject = (PSObject)pspropertyInfo.Value;
			}
		}
	}
}
