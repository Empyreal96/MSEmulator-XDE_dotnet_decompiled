using System;

namespace CommandLine
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValueAttribute : BaseAttribute
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x000083BF File Offset: 0x000065BF
		public ValueAttribute(int index)
		{
			this.index = index;
			this.metaName = string.Empty;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x000083D9 File Offset: 0x000065D9
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000083E1 File Offset: 0x000065E1
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x000083E9 File Offset: 0x000065E9
		public string MetaName
		{
			get
			{
				return this.metaName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.metaName = value;
			}
		}

		// Token: 0x0400008A RID: 138
		private readonly int index;

		// Token: 0x0400008B RID: 139
		private string metaName;
	}
}
