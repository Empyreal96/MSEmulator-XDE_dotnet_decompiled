using System;
using System.CodeDom.Compiler;

namespace System.Management.Automation
{
	// Token: 0x020005FA RID: 1530
	[GeneratedCode("DLR", "2.0")]
	internal class MutableTuple<T0> : MutableTuple
	{
		// Token: 0x06004200 RID: 16896 RVA: 0x0015D94F File Offset: 0x0015BB4F
		public MutableTuple()
		{
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x0015D957 File Offset: 0x0015BB57
		public MutableTuple(T0 item0)
		{
			this._item0 = item0;
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x0015D966 File Offset: 0x0015BB66
		// (set) Token: 0x06004203 RID: 16899 RVA: 0x0015D96E File Offset: 0x0015BB6E
		public T0 Item000
		{
			get
			{
				return this._item0;
			}
			set
			{
				this._item0 = value;
				this._valuesSet[0] = true;
			}
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x0015D984 File Offset: 0x0015BB84
		protected override object GetValueImpl(int index)
		{
			if (index == 0)
			{
				return this.Item000;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x0015D9B0 File Offset: 0x0015BBB0
		protected override void SetValueImpl(int index, object value)
		{
			if (index == 0)
			{
				this.Item000 = LanguagePrimitives.ConvertTo<T0>(value);
				return;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0015D9DA File Offset: 0x0015BBDA
		public override int Capacity
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0400210A RID: 8458
		private T0 _item0;
	}
}
