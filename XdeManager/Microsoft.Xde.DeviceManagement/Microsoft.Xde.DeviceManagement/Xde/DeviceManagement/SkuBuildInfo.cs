using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x0200000B RID: 11
	public class SkuBuildInfo
	{
		// Token: 0x06000097 RID: 151 RVA: 0x0000314C File Offset: 0x0000134C
		public static ReadOnlyCollection<SkuBuildInfo> GetSkuBuildInfos()
		{
			if (SkuBuildInfo.BuildInfos != null)
			{
				return SkuBuildInfo.BuildInfos;
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			List<SkuBuildInfo> list = new List<SkuBuildInfo>();
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Microsoft.Xde.DeviceManagement.skubuildinfo.json"))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					List<SkuBuildInfo.SkuBuildInfoData> list2 = (List<SkuBuildInfo.SkuBuildInfoData>)new JsonSerializer().Deserialize(streamReader, typeof(List<SkuBuildInfo.SkuBuildInfoData>));
					using (List<SkuBuildInfo.SkuBuildInfoData>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SkuBuildInfo.SkuBuildInfoData i = enumerator.Current;
							List<ImageInfo> imageInfos = i.ImageInfos;
							if (i.ImageInfosSameAs != null)
							{
								SkuBuildInfo.SkuBuildInfoData skuBuildInfoData = list2.FirstOrDefault((SkuBuildInfo.SkuBuildInfoData b) => b.SkuName == i.ImageInfosSameAs);
								if (skuBuildInfoData != null)
								{
									imageInfos = skuBuildInfoData.ImageInfos;
								}
							}
							SkuBuildInfo item = new SkuBuildInfo
							{
								SkuName = i.SkuName,
								ImageInfos = imageInfos.AsReadOnly()
							};
							list.Add(item);
						}
					}
				}
			}
			SkuBuildInfo.BuildInfos = list.AsReadOnly();
			return SkuBuildInfo.BuildInfos;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003294 File Offset: 0x00001494
		// (set) Token: 0x06000099 RID: 153 RVA: 0x0000329C File Offset: 0x0000149C
		public string SkuName { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000032A5 File Offset: 0x000014A5
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000032AD File Offset: 0x000014AD
		public ReadOnlyCollection<ImageInfo> ImageInfos { get; private set; }

		// Token: 0x0600009C RID: 156 RVA: 0x000032B6 File Offset: 0x000014B6
		private SkuBuildInfo()
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000032C0 File Offset: 0x000014C0
		public static SkuBuildInfo GetSkuBuildInfoForSku(string sku)
		{
			return SkuBuildInfo.GetSkuBuildInfos().FirstOrDefault((SkuBuildInfo s) => StringComparer.OrdinalIgnoreCase.Equals(s.SkuName, sku));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000032F0 File Offset: 0x000014F0
		private Tuple<DownloadedVhdInfo, ImageInfo> GetSourceImageInfoForVhdFileName(string vhdFileName)
		{
			DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(vhdFileName);
			ImageInfo item = null;
			if (downloadedVhdInfo != null && downloadedVhdInfo.Source != null)
			{
				item = this.ImageInfos.FirstOrDefault((ImageInfo b) => downloadedVhdInfo.Source.EndsWith(b.Location, StringComparison.OrdinalIgnoreCase));
			}
			return new Tuple<DownloadedVhdInfo, ImageInfo>(downloadedVhdInfo, item);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000334A File Offset: 0x0000154A
		public ImageInfo FindInfoForVhdFileName(string vhdFileName)
		{
			return this.GetSourceImageInfoForVhdFileName(vhdFileName).Item2;
		}

		// Token: 0x0400002C RID: 44
		private static ReadOnlyCollection<SkuBuildInfo> BuildInfos;

		// Token: 0x0200001F RID: 31
		private class SkuBuildInfoData
		{
			// Token: 0x1700009C RID: 156
			// (get) Token: 0x0600018E RID: 398 RVA: 0x000060C2 File Offset: 0x000042C2
			// (set) Token: 0x0600018F RID: 399 RVA: 0x000060CA File Offset: 0x000042CA
			public string SkuName { get; set; }

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x06000190 RID: 400 RVA: 0x000060D3 File Offset: 0x000042D3
			// (set) Token: 0x06000191 RID: 401 RVA: 0x000060DB File Offset: 0x000042DB
			public string ImageInfosSameAs { get; set; }

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x06000192 RID: 402 RVA: 0x000060E4 File Offset: 0x000042E4
			// (set) Token: 0x06000193 RID: 403 RVA: 0x000060EC File Offset: 0x000042EC
			public List<ImageInfo> ImageInfos { get; set; }
		}
	}
}
