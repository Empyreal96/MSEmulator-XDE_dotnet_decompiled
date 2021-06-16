using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000A RID: 10
	public class WmiVhdBootSettings
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002F52 File Offset: 0x00001152
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002F5A File Offset: 0x0000115A
		public string BootLanguage { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002F63 File Offset: 0x00001163
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002F6B File Offset: 0x0000116B
		public string VideoResolution { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002F74 File Offset: 0x00001174
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002F7C File Offset: 0x0000117C
		public string ScreenDiagonal { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002F85 File Offset: 0x00001185
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002F8D File Offset: 0x0000118D
		public XdeSensors SensorsEnabled { get; set; }

		// Token: 0x06000059 RID: 89 RVA: 0x00002F98 File Offset: 0x00001198
		public static WmiVhdBootSettings ReadForVhd(string vhdPath)
		{
			string fileNameForVhd = WmiVhdBootSettings.GetFileNameForVhd(vhdPath);
			if (!File.Exists(fileNameForVhd))
			{
				return null;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(WmiVhdBootSettings));
			WmiVhdBootSettings result;
			using (StreamReader streamReader = new StreamReader(fileNameForVhd))
			{
				result = (WmiVhdBootSettings)xmlSerializer.Deserialize(streamReader);
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002FF8 File Offset: 0x000011F8
		public static string GetFileNameForVhd(string vhdPath)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(vhdPath);
			if (string.IsNullOrEmpty(fileNameWithoutExtension))
			{
				return null;
			}
			string text = fileNameWithoutExtension + ".boot.xml";
			string directoryName = Path.GetDirectoryName(vhdPath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				text = Path.Combine(directoryName, text);
			}
			return text;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000303C File Offset: 0x0000123C
		public void WriteForVhd(string vhdPath)
		{
			string fileNameForVhd = WmiVhdBootSettings.GetFileNameForVhd(vhdPath);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(WmiVhdBootSettings));
			using (XmlWriter xmlWriter = XmlWriter.Create(fileNameForVhd))
			{
				xmlSerializer.Serialize(xmlWriter, this);
			}
		}
	}
}
