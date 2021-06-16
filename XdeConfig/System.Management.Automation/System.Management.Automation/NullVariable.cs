using System;

namespace System.Management.Automation
{
	// Token: 0x02000843 RID: 2115
	internal class NullVariable : PSVariable
	{
		// Token: 0x06005174 RID: 20852 RVA: 0x001B2316 File Offset: 0x001B0516
		internal NullVariable() : base("null", null, ScopedItemOptions.Constant | ScopedItemOptions.AllScope)
		{
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x001B2326 File Offset: 0x001B0526
		// (set) Token: 0x06005176 RID: 20854 RVA: 0x001B2329 File Offset: 0x001B0529
		public override object Value
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x001B232B File Offset: 0x001B052B
		// (set) Token: 0x06005178 RID: 20856 RVA: 0x001B2346 File Offset: 0x001B0546
		public override string Description
		{
			get
			{
				if (this.description == null)
				{
					this.description = SessionStateStrings.DollarNullDescription;
				}
				return this.description;
			}
			set
			{
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06005179 RID: 20857 RVA: 0x001B2348 File Offset: 0x001B0548
		// (set) Token: 0x0600517A RID: 20858 RVA: 0x001B234B File Offset: 0x001B054B
		public override ScopedItemOptions Options
		{
			get
			{
				return ScopedItemOptions.None;
			}
			set
			{
			}
		}

		// Token: 0x040029C2 RID: 10690
		private string description;
	}
}
