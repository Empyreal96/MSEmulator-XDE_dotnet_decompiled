using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000014 RID: 20
	[EventData]
	internal class EventCounterPayload : IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00008E39 File Offset: 0x00007039
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00008E41 File Offset: 0x00007041
		public string Name { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00008E4A File Offset: 0x0000704A
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00008E52 File Offset: 0x00007052
		public float Mean { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00008E5B File Offset: 0x0000705B
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00008E63 File Offset: 0x00007063
		public float StandardDerivation { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00008E6C File Offset: 0x0000706C
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00008E74 File Offset: 0x00007074
		public int Count { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00008E7D File Offset: 0x0000707D
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00008E85 File Offset: 0x00007085
		public float Min { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00008E8E File Offset: 0x0000708E
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00008E96 File Offset: 0x00007096
		public float Max { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00008E9F File Offset: 0x0000709F
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00008EA7 File Offset: 0x000070A7
		public float IntervalSec { get; internal set; }

		// Token: 0x060000DD RID: 221 RVA: 0x00008EB0 File Offset: 0x000070B0
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.ForEnumeration.GetEnumerator();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00008EBD File Offset: 0x000070BD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.ForEnumeration.GetEnumerator();
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000090B8 File Offset: 0x000072B8
		private IEnumerable<KeyValuePair<string, object>> ForEnumeration
		{
			get
			{
				yield return new KeyValuePair<string, object>("Name", this.Name);
				yield return new KeyValuePair<string, object>("Mean", this.Mean);
				yield return new KeyValuePair<string, object>("StandardDerivation", this.StandardDerivation);
				yield return new KeyValuePair<string, object>("Count", this.Count);
				yield return new KeyValuePair<string, object>("Min", this.Min);
				yield return new KeyValuePair<string, object>("Max", this.Max);
				yield break;
			}
		}
	}
}
