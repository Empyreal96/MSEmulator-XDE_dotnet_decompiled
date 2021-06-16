using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IXdeSkinFactory))]
	public class SkinFactory : IXdeSkinFactory
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000391D File Offset: 0x00001B1D
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003925 File Offset: 0x00001B25
		[Import]
		public IXdeSku Sku { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00003930 File Offset: 0x00001B30
		public static bool TryParseSizeFromResName(string resolutionName, out Size resolution)
		{
			string[] array = resolutionName.Split(new char[]
			{
				'x',
				'X'
			});
			int num;
			int num2;
			if (array.Length == 2 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2) && num > 0 && num2 > 0)
			{
				resolution = new Size(num, num2);
				return true;
			}
			resolution = default(Size);
			return false;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003994 File Offset: 0x00001B94
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IXdeSkin LoadSkinFromResolution(string resolutionName, XdeSensors sensors)
		{
			Skin skin = Skin.LoadSkinInformation(this.GetFullSkinNameFromResolution(resolutionName), resolutionName);
			SkinDisplay display = skin.Display;
			if (display.MappingImage != null)
			{
				return Skin.LoadSkin(this.GetFullSkinNameFromResolution(resolutionName), sensors, skin);
			}
			return Skin.LoadSkin(this.GetBestMatchingSkinForDisplayInfo(display), this.GetFullSkinNameFromResolution(resolutionName));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000039E0 File Offset: 0x00001BE0
		public SkinDisplay GetSkinDisplayInformation(string resolutionName)
		{
			return Skin.LoadSkinInformation(this.GetFullSkinNameFromResolution(resolutionName), resolutionName).Display;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000039F4 File Offset: 0x00001BF4
		public IEnumerable<SkinDisplay> GetAllSkinDisplayInformation()
		{
			foreach (string skinFileName in this.GetAllSkinFiles())
			{
				Skin skin = Skin.LoadSkinInformation(skinFileName, null);
				yield return skin.Display;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003A04 File Offset: 0x00001C04
		public bool IsValidResolutionName(string resolutionName)
		{
			return File.Exists(this.GetFullSkinNameFromResolution(resolutionName));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003A12 File Offset: 0x00001C12
		private string GetSkinFolder()
		{
			return Path.Combine(this.Sku.SkuDirectory, "skins");
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003A29 File Offset: 0x00001C29
		private IEnumerable<string> GetAllSkinFiles()
		{
			string skinFolder = this.GetSkinFolder();
			foreach (string text in Directory.GetFiles(skinFolder, "skin_*.xml"))
			{
				yield return text;
			}
			string[] array = null;
			yield break;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003A3C File Offset: 0x00001C3C
		private string GetBestMatchingSkinForDisplayInfo(SkinDisplay displayInfo)
		{
			int num = 0;
			float num2 = (float)displayInfo.DisplayHeight / (float)displayInfo.DisplayWidth;
			float num3 = float.MaxValue;
			string result = null;
			foreach (string text in this.GetAllSkinFiles())
			{
				Skin skin = Skin.LoadSkinInformation(text, null);
				if (skin.Display.DisplayWidth != displayInfo.DisplayWidth || skin.Display.DisplayHeight != displayInfo.DisplayHeight)
				{
					float num4 = (float)skin.Display.DisplayHeight / (float)skin.Display.DisplayWidth;
					float num5 = Math.Abs(num2 - num4);
					if (num5 <= num3)
					{
						num3 = num5;
						if (skin.Display.DisplayHeight > num)
						{
							result = text;
							num = skin.Display.DisplayHeight;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B2C File Offset: 0x00001D2C
		private string GetFullSkinNameFromResolution(string resolutionName)
		{
			string skinFolder = this.GetSkinFolder();
			string path = StringUtilities.InvariantCultureFormat("skin_{0}.xml", new object[]
			{
				resolutionName
			});
			string text = Path.Combine(skinFolder, path);
			if (!File.Exists(text))
			{
				text = null;
				foreach (string text2 in this.GetAllSkinFiles())
				{
					if (StringUtilities.EqualsNoCase(Skin.LoadSkinInformation(text2, null).Display.Alias, resolutionName))
					{
						text = text2;
						break;
					}
				}
				Size size;
				if (text == null && SkinFactory.TryParseSizeFromResName(resolutionName, out size))
				{
					path = StringUtilities.InvariantCultureFormat("skin_{0}.xml", new object[]
					{
						"auto"
					});
					text = Path.Combine(skinFolder, path);
					if (!File.Exists(text))
					{
						text = null;
					}
				}
			}
			return text;
		}

		// Token: 0x0400002C RID: 44
		private const string SkinPrefix = "skin_";

		// Token: 0x0400002D RID: 45
		private const string SkinWildcard = "skin_*.xml";

		// Token: 0x0400002E RID: 46
		private const string SkinFormat = "skin_{0}.xml";
	}
}
