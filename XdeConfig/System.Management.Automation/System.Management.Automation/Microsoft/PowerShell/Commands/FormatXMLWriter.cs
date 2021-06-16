using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Xml;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020004B3 RID: 1203
	internal static class FormatXMLWriter
	{
		// Token: 0x06003585 RID: 13701 RVA: 0x00123BF4 File Offset: 0x00121DF4
		internal static void WriteToPS1XML(PSCmdlet cmdlet, List<ExtendedTypeDefinition> typeDefinitions, string filepath, bool force, bool noclobber, bool writeScritBlock, bool isLiteralPath)
		{
			FileStream fileStream;
			StreamWriter streamWriter;
			FileInfo fileInfo;
			PathUtils.MasterStreamOpen(cmdlet, filepath, "ascii", true, false, force, noclobber, out fileStream, out streamWriter, out fileInfo, isLiteralPath);
			XmlWriter xmlWriter = XmlWriter.Create(streamWriter);
			FormatXMLHelper.WriteToXML(xmlWriter, typeDefinitions, writeScritBlock);
			xmlWriter.Dispose();
			streamWriter.Dispose();
			fileStream.Dispose();
		}
	}
}
