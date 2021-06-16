using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006B RID: 107
	public class RegKey
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000556E File Offset: 0x0000376E
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00005578 File Offset: 0x00003778
		[XmlAttribute]
		public string KeyName
		{
			get
			{
				return this.keyName;
			}
			set
			{
				this.keyName = value;
				this.KeyRootAlias = null;
				this.KeyPath = null;
				if (this.keyName != null)
				{
					int num = this.keyName.IndexOf('\\');
					if (num != -1)
					{
						this.KeyRootAlias = this.keyName.Substring(0, num);
						this.KeyPath = this.keyName.Substring(num + 1);
					}
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000269 RID: 617 RVA: 0x000055DB File Offset: 0x000037DB
		[XmlElement(ElementName = "RegValue")]
		public List<RegValue> Values
		{
			get
			{
				return this.regValues;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000055E3 File Offset: 0x000037E3
		// (set) Token: 0x0600026B RID: 619 RVA: 0x000055EB File Offset: 0x000037EB
		[XmlIgnore]
		public string KeyRootAlias { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000055F4 File Offset: 0x000037F4
		// (set) Token: 0x0600026D RID: 621 RVA: 0x000055FC File Offset: 0x000037FC
		[XmlIgnore]
		public string KeyPath { get; private set; }

		// Token: 0x04000181 RID: 385
		private string keyName;

		// Token: 0x04000182 RID: 386
		private List<RegValue> regValues = new List<RegValue>();
	}
}
