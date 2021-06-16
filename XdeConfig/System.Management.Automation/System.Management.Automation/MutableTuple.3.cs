using System;
using System.CodeDom.Compiler;

namespace System.Management.Automation
{
	// Token: 0x020005FB RID: 1531
	[GeneratedCode("DLR", "2.0")]
	internal class MutableTuple<T0, T1> : MutableTuple<T0>
	{
		// Token: 0x06004207 RID: 16903 RVA: 0x0015D9DD File Offset: 0x0015BBDD
		public MutableTuple()
		{
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0015D9E5 File Offset: 0x0015BBE5
		public MutableTuple(T0 item0, T1 item1) : base(item0)
		{
			this._item1 = item1;
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x0015D9F5 File Offset: 0x0015BBF5
		// (set) Token: 0x0600420A RID: 16906 RVA: 0x0015D9FD File Offset: 0x0015BBFD
		public T1 Item001
		{
			get
			{
				return this._item1;
			}
			set
			{
				this._item1 = value;
				this._valuesSet[1] = true;
			}
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x0015DA14 File Offset: 0x0015BC14
		protected override object GetValueImpl(int index)
		{
			switch (index)
			{
			case 0:
				return base.Item000;
			case 1:
				return this.Item001;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0015DA58 File Offset: 0x0015BC58
		protected override void SetValueImpl(int index, object value)
		{
			switch (index)
			{
			case 0:
				base.Item000 = LanguagePrimitives.ConvertTo<T0>(value);
				return;
			case 1:
				this.Item001 = LanguagePrimitives.ConvertTo<T1>(value);
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x0015DA9B File Offset: 0x0015BC9B
		public override int Capacity
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0400210B RID: 8459
		private T1 _item1;
	}
}
