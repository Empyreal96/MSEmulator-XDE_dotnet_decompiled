using System;
using System.Xml;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x0200000F RID: 15
	public class XdeSkin
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00004761 File Offset: 0x00002961
		public static XdeSkin LoadFromPath(string path)
		{
			return new XdeSkin
			{
				skinDisplay = SkinDisplay.LoadSkinDisplay(path)
			};
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004774 File Offset: 0x00002974
		private static int GetIntAttributeValue(XmlElement displayNode, string attrName)
		{
			int result = 0;
			int.TryParse(displayNode.GetAttribute(attrName), out result);
			return result;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004793 File Offset: 0x00002993
		public int DisplayWidth
		{
			get
			{
				return this.skinDisplay.DisplayWidth;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000047A0 File Offset: 0x000029A0
		public int DisplayHeight
		{
			get
			{
				return this.skinDisplay.DisplayHeight;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000047AD File Offset: 0x000029AD
		public int DisplayCount
		{
			get
			{
				return this.skinDisplay.DisplayCount;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000047BA File Offset: 0x000029BA
		public bool HasExternalMonitor
		{
			get
			{
				return this.ExternalDisplayHeight > 0 && this.ExternalDisplayWidth > 0;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000047D0 File Offset: 0x000029D0
		public int ExternalDisplayWidth
		{
			get
			{
				return this.skinDisplay.ExternalDisplayWidth;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000047DD File Offset: 0x000029DD
		public int ExternalDisplayHeight
		{
			get
			{
				return this.skinDisplay.ExternalDisplayHeight;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000047EA File Offset: 0x000029EA
		public int DisplayScaleFactor
		{
			get
			{
				return this.skinDisplay.DisplayScaleFactor;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000047F7 File Offset: 0x000029F7
		public string Name
		{
			get
			{
				return this.skinDisplay.Alias;
			}
		}

		// Token: 0x04000047 RID: 71
		private SkinDisplay skinDisplay;
	}
}
