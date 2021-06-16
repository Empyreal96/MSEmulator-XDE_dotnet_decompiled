using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005DF RID: 1503
	public class DynamicKeywordProperty
	{
		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x001522B3 File Offset: 0x001504B3
		// (set) Token: 0x0600402C RID: 16428 RVA: 0x001522BB File Offset: 0x001504BB
		public string Name { get; set; }

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x001522C4 File Offset: 0x001504C4
		// (set) Token: 0x0600402E RID: 16430 RVA: 0x001522CC File Offset: 0x001504CC
		public string TypeConstraint { get; set; }

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x001522D8 File Offset: 0x001504D8
		public List<string> Attributes
		{
			get
			{
				List<string> result;
				if ((result = this._attributes) == null)
				{
					result = (this._attributes = new List<string>());
				}
				return result;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x00152300 File Offset: 0x00150500
		public List<string> Values
		{
			get
			{
				List<string> result;
				if ((result = this._values) == null)
				{
					result = (this._values = new List<string>());
				}
				return result;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x00152328 File Offset: 0x00150528
		public Dictionary<string, string> ValueMap
		{
			get
			{
				Dictionary<string, string> result;
				if ((result = this._valueMap) == null)
				{
					result = (this._valueMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
				}
				return result;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x00152352 File Offset: 0x00150552
		// (set) Token: 0x06004033 RID: 16435 RVA: 0x0015235A File Offset: 0x0015055A
		public bool Mandatory { get; set; }

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x00152363 File Offset: 0x00150563
		// (set) Token: 0x06004035 RID: 16437 RVA: 0x0015236B File Offset: 0x0015056B
		public bool IsKey { get; set; }

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004036 RID: 16438 RVA: 0x00152374 File Offset: 0x00150574
		// (set) Token: 0x06004037 RID: 16439 RVA: 0x0015237C File Offset: 0x0015057C
		public Tuple<int, int> Range { get; set; }

		// Token: 0x0400204E RID: 8270
		private List<string> _attributes;

		// Token: 0x0400204F RID: 8271
		private List<string> _values;

		// Token: 0x04002050 RID: 8272
		private Dictionary<string, string> _valueMap;
	}
}
