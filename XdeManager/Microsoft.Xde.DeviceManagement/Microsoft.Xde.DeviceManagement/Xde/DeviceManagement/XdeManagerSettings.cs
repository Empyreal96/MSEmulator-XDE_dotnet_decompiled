using System;
using System.IO;
using Microsoft.Xde.Common;
using Newtonsoft.Json;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000012 RID: 18
	public class XdeManagerSettings
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00004CEF File Offset: 0x00002EEF
		private XdeManagerSettings(XdeManagerSettings.SettingsData data)
		{
			this.data = data;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00004CFE File Offset: 0x00002EFE
		public static XdeManagerSettings Current
		{
			get
			{
				if (XdeManagerSettings.currentSettings == null)
				{
					XdeManagerSettings.currentSettings = XdeManagerSettings.Load();
				}
				return XdeManagerSettings.currentSettings;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004D16 File Offset: 0x00002F16
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00004D45 File Offset: 0x00002F45
		public string DownloadRoot
		{
			get
			{
				if (string.IsNullOrEmpty(this.data.DownloadRoot))
				{
					return Path.Combine(XdeAppUtils.AppDataFolder, "images");
				}
				return this.data.DownloadRoot;
			}
			set
			{
				this.data.DownloadRoot = value;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004D53 File Offset: 0x00002F53
		private static string GetFileName()
		{
			return Path.Combine(XdeAppUtils.AppDataFolder, "settings.json");
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004D64 File Offset: 0x00002F64
		private static XdeManagerSettings Load()
		{
			XdeManagerSettings.SettingsData settingsData;
			if (File.Exists(XdeManagerSettings.GetFileName()))
			{
				using (StreamReader streamReader = File.OpenText(XdeManagerSettings.GetFileName()))
				{
					JsonSerializer jsonSerializer = new JsonSerializer();
					try
					{
						settingsData = (XdeManagerSettings.SettingsData)jsonSerializer.Deserialize(streamReader, typeof(XdeManagerSettings.SettingsData));
						goto IL_4F;
					}
					catch (Exception)
					{
						settingsData = new XdeManagerSettings.SettingsData();
						goto IL_4F;
					}
				}
			}
			settingsData = new XdeManagerSettings.SettingsData();
			IL_4F:
			return new XdeManagerSettings(settingsData);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004DE4 File Offset: 0x00002FE4
		public void Save()
		{
			string fileName = XdeManagerSettings.GetFileName();
			string directoryName = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = File.CreateText(fileName))
			{
				new JsonSerializer().Serialize(streamWriter, this.data);
			}
		}

		// Token: 0x04000051 RID: 81
		private static XdeManagerSettings currentSettings;

		// Token: 0x04000052 RID: 82
		private XdeManagerSettings.SettingsData data;

		// Token: 0x0200002D RID: 45
		private class SettingsData
		{
			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x060001D2 RID: 466 RVA: 0x00006DE3 File Offset: 0x00004FE3
			// (set) Token: 0x060001D3 RID: 467 RVA: 0x00006DEB File Offset: 0x00004FEB
			public string DownloadRoot { get; set; }
		}
	}
}
