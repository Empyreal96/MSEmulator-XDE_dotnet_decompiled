using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000159 RID: 345
	internal class ConsolidatedString : Collection<string>
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x000633E8 File Offset: 0x000615E8
		protected override void SetItem(int index, string item)
		{
			if (string.IsNullOrEmpty(item))
			{
				throw PSTraceSource.NewArgumentException("item");
			}
			base.SetItem(index, item);
			this.UpdateKey();
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0006340B File Offset: 0x0006160B
		protected override void ClearItems()
		{
			base.ClearItems();
			this.UpdateKey();
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00063419 File Offset: 0x00061619
		protected override void InsertItem(int index, string item)
		{
			if (string.IsNullOrEmpty(item))
			{
				throw PSTraceSource.NewArgumentException("item");
			}
			base.InsertItem(index, item);
			this.UpdateKey();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0006343C File Offset: 0x0006163C
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			this.UpdateKey();
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x0006344B File Offset: 0x0006164B
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00063453 File Offset: 0x00061653
		internal string Key { get; private set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0006345C File Offset: 0x0006165C
		internal bool IsReadOnly
		{
			get
			{
				return ((ICollection<string>)this).IsReadOnly;
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00063464 File Offset: 0x00061664
		private void UpdateKey()
		{
			this.Key = string.Join("@@@", this);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00063477 File Offset: 0x00061677
		public ConsolidatedString(ConsolidatedString other) : base(new List<string>(other))
		{
			this.Key = other.Key;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00063494 File Offset: 0x00061694
		internal ConsolidatedString(IEnumerable<string> strings, bool interned)
		{
			IList<string> list2;
			if (!interned)
			{
				IList<string> list = strings.ToList<string>();
				list2 = list;
			}
			else
			{
				list2 = new ReadOnlyCollection<string>(strings.ToList<string>());
			}
			base..ctor(list2);
			this.UpdateKey();
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000634C8 File Offset: 0x000616C8
		public ConsolidatedString(IEnumerable<string> strings) : base(strings.ToList<string>())
		{
			for (int i = 0; i < base.Count; i++)
			{
				string value = base[i];
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("strings");
				}
			}
			this.UpdateKey();
		}

		// Token: 0x04000797 RID: 1943
		internal static readonly ConsolidatedString Empty = new ConsolidatedString(new string[0]);
	}
}
