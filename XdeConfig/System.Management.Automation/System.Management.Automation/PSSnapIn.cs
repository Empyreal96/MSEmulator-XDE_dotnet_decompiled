using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200085C RID: 2140
	public abstract class PSSnapIn : PSSnapInInstaller
	{
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x001B7B82 File Offset: 0x001B5D82
		public virtual string[] Formats
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x001B7B85 File Offset: 0x001B5D85
		public virtual string[] Types
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x001B7B88 File Offset: 0x001B5D88
		internal override Dictionary<string, object> RegValues
		{
			get
			{
				if (this._regValues == null)
				{
					this._regValues = base.RegValues;
					if (this.Types != null && this.Types.Length > 0)
					{
						this._regValues["Types"] = this.Types;
					}
					if (this.Formats != null && this.Formats.Length > 0)
					{
						this._regValues["Formats"] = this.Formats;
					}
				}
				return this._regValues;
			}
		}

		// Token: 0x04002A3D RID: 10813
		private Dictionary<string, object> _regValues;
	}
}
