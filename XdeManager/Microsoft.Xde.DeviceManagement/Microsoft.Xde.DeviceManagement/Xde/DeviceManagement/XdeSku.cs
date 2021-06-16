using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000010 RID: 16
	public class XdeSku
	{
		// Token: 0x0600013A RID: 314 RVA: 0x0000480C File Offset: 0x00002A0C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004814 File Offset: 0x00002A14
		public static XdeSku LoadFromPath(string path)
		{
			string text = System.IO.Path.Combine(path, "xdesku.xml");
			if (!File.Exists(text))
			{
				return null;
			}
			XdeSku xdeSku = new XdeSku
			{
				skuData = SkuData.LoadSkuInformation(text)
			};
			string path2 = System.IO.Path.Combine(path, "skins");
			List<XdeSkin> list = new List<XdeSkin>();
			string[] files = Directory.GetFiles(path2, "*.xml");
			for (int i = 0; i < files.Length; i++)
			{
				XdeSkin xdeSkin = XdeSkin.LoadFromPath(files[i]);
				if (xdeSkin != null)
				{
					list.Add(xdeSkin);
				}
			}
			xdeSku.Path = path;
			xdeSku.Name = System.IO.Path.GetFileName(path);
			xdeSku.Skins = list;
			return xdeSku;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000048AC File Offset: 0x00002AAC
		public XdeSkin FindSkin(string name)
		{
			return this.Skins.FirstOrDefault((XdeSkin s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, name) || StringComparer.OrdinalIgnoreCase.Equals(string.Format("{0}x{1}", s.DisplayWidth, s.DisplayHeight), name));
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000048DD File Offset: 0x00002ADD
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000048E5 File Offset: 0x00002AE5
		public string Path { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000048EE File Offset: 0x00002AEE
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000048F6 File Offset: 0x00002AF6
		public string Name { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000048FF File Offset: 0x00002AFF
		public int ProcessorCount
		{
			get
			{
				return this.skuData.Options.ProcessorCount;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00004911 File Offset: 0x00002B11
		public bool UseHcsIfAvailable
		{
			get
			{
				return this.skuData.Options.UseHCSIfAvailable;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00004923 File Offset: 0x00002B23
		public bool UseGpu
		{
			get
			{
				return this.UseHcsIfAvailable && this.skuData.Options.ValidSensors.Contains("DesktopGPU");
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004949 File Offset: 0x00002B49
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00004951 File Offset: 0x00002B51
		public IReadOnlyCollection<XdeSkin> Skins { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000495A File Offset: 0x00002B5A
		public int MinMemSize
		{
			get
			{
				return this.skuData.Options.MinMemSize;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000496C File Offset: 0x00002B6C
		public int MaxMemSize
		{
			get
			{
				return this.skuData.Options.MaxMemSize;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000497E File Offset: 0x00002B7E
		public SkuData GetSkuData()
		{
			return SkuData.LoadSkuInformation(System.IO.Path.Combine(this.Path, "xdesku.xml"));
		}

		// Token: 0x04000048 RID: 72
		private SkuData skuData;
	}
}
