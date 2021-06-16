using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001B9 RID: 441
	internal class UserDefinedHelpData
	{
		// Token: 0x06001492 RID: 5266 RVA: 0x0007F8A0 File Offset: 0x0007DAA0
		private UserDefinedHelpData()
		{
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0007F8B8 File Offset: 0x0007DAB8
		internal Dictionary<string, string> Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0007F8C0 File Offset: 0x0007DAC0
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0007F8C8 File Offset: 0x0007DAC8
		internal static UserDefinedHelpData Load(XmlNode dataNode)
		{
			if (dataNode == null)
			{
				return null;
			}
			UserDefinedHelpData userDefinedHelpData = new UserDefinedHelpData();
			for (int i = 0; i < dataNode.ChildNodes.Count; i++)
			{
				XmlNode xmlNode = dataNode.ChildNodes[i];
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					userDefinedHelpData.Properties[xmlNode.Name] = xmlNode.InnerText.Trim();
				}
			}
			if (!userDefinedHelpData.Properties.ContainsKey("name"))
			{
				return null;
			}
			userDefinedHelpData._name = userDefinedHelpData.Properties["name"];
			if (string.IsNullOrEmpty(userDefinedHelpData.Name))
			{
				return null;
			}
			return userDefinedHelpData;
		}

		// Token: 0x040008DB RID: 2267
		private readonly Dictionary<string, string> _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040008DC RID: 2268
		private string _name;
	}
}
