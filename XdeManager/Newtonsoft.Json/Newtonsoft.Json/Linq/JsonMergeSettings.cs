using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BB RID: 187
	public class JsonMergeSettings
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x0002AEF9 File Offset: 0x000290F9
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002AF08 File Offset: 0x00029108
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0002AF10 File Offset: 0x00029110
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002AF2C File Offset: 0x0002912C
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0002AF34 File Offset: 0x00029134
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0002AF50 File Offset: 0x00029150
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x0002AF58 File Offset: 0x00029158
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x0400036D RID: 877
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x0400036E RID: 878
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x0400036F RID: 879
		private StringComparison _propertyNameComparison;
	}
}
