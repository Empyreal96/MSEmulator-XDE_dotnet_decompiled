using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200006C RID: 108
	public sealed class DscResourcePropertyInfo
	{
		// Token: 0x060005D1 RID: 1489 RVA: 0x0001A8B0 File Offset: 0x00018AB0
		internal DscResourcePropertyInfo()
		{
			this.Values = new ReadOnlyCollection<string>(new List<string>());
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001A8C8 File Offset: 0x00018AC8
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0001A8D0 File Offset: 0x00018AD0
		public string Name { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001A8D9 File Offset: 0x00018AD9
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0001A8E1 File Offset: 0x00018AE1
		public string PropertyType { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001A8EA File Offset: 0x00018AEA
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001A8F2 File Offset: 0x00018AF2
		public bool IsMandatory { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001A8FB File Offset: 0x00018AFB
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0001A903 File Offset: 0x00018B03
		public ReadOnlyCollection<string> Values { get; private set; }

		// Token: 0x060005DA RID: 1498 RVA: 0x0001A90C File Offset: 0x00018B0C
		internal void UpdateValues(IList<string> values)
		{
			if (values != null)
			{
				this.Values = new ReadOnlyCollection<string>(values);
			}
		}
	}
}
