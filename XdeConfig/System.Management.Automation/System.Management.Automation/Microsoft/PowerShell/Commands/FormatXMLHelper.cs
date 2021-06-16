using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Xml;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020004B4 RID: 1204
	internal static class FormatXMLHelper
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x00123C3C File Offset: 0x00121E3C
		internal static void WriteToXML(XmlWriter _writer, IEnumerable<ExtendedTypeDefinition> _typeDefinitions, bool exportScriptBlock)
		{
			_writer.WriteStartElement(FormatXMLHelper._tagConfiguration);
			_writer.WriteStartElement(FormatXMLHelper._tagViewDefinitions);
			Dictionary<Guid, List<ExtendedTypeDefinition>> dictionary = new Dictionary<Guid, List<ExtendedTypeDefinition>>();
			Dictionary<Guid, FormatViewDefinition> dictionary2 = new Dictionary<Guid, FormatViewDefinition>();
			foreach (ExtendedTypeDefinition extendedTypeDefinition in _typeDefinitions)
			{
				foreach (FormatViewDefinition formatViewDefinition in extendedTypeDefinition.FormatViewDefinition)
				{
					if (!dictionary.ContainsKey(formatViewDefinition.InstanceId))
					{
						dictionary.Add(formatViewDefinition.InstanceId, new List<ExtendedTypeDefinition>());
					}
					if (!dictionary2.ContainsKey(formatViewDefinition.InstanceId))
					{
						dictionary2.Add(formatViewDefinition.InstanceId, formatViewDefinition);
					}
					dictionary[formatViewDefinition.InstanceId].Add(extendedTypeDefinition);
				}
			}
			foreach (Guid key in dictionary2.Keys)
			{
				FormatViewDefinition formatViewDefinition2 = dictionary2[key];
				_writer.WriteStartElement(FormatXMLHelper._tagView);
				_writer.WriteElementString(FormatXMLHelper._tagName, formatViewDefinition2.Name);
				_writer.WriteStartElement(FormatXMLHelper._tagViewSelectedBy);
				foreach (ExtendedTypeDefinition extendedTypeDefinition2 in dictionary[key])
				{
					_writer.WriteElementString(FormatXMLHelper._tagTypeName, extendedTypeDefinition2.TypeName);
				}
				_writer.WriteEndElement();
				formatViewDefinition2.Control.WriteToXML(_writer, exportScriptBlock);
				_writer.WriteEndElement();
			}
			_writer.WriteEndElement();
			_writer.WriteEndElement();
		}

		// Token: 0x04001B48 RID: 6984
		private static string _tagConfiguration = "Configuration";

		// Token: 0x04001B49 RID: 6985
		private static string _tagViewDefinitions = "ViewDefinitions";

		// Token: 0x04001B4A RID: 6986
		private static string _tagView = "View";

		// Token: 0x04001B4B RID: 6987
		private static string _tagName = "Name";

		// Token: 0x04001B4C RID: 6988
		private static string _tagViewSelectedBy = "ViewSelectedBy";

		// Token: 0x04001B4D RID: 6989
		private static string _tagTypeName = "TypeName";
	}
}
