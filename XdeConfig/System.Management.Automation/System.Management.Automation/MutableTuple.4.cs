using System;
using System.CodeDom.Compiler;

namespace System.Management.Automation
{
	// Token: 0x020005FC RID: 1532
	[GeneratedCode("DLR", "2.0")]
	internal class MutableTuple<T0, T1, T2, T3> : MutableTuple<T0, T1>
	{
		// Token: 0x0600420E RID: 16910 RVA: 0x0015DA9E File Offset: 0x0015BC9E
		public MutableTuple()
		{
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x0015DAA6 File Offset: 0x0015BCA6
		public MutableTuple(T0 item0, T1 item1, T2 item2, T3 item3) : base(item0, item1)
		{
			this._item2 = item2;
			this._item3 = item3;
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x0015DABF File Offset: 0x0015BCBF
		// (set) Token: 0x06004211 RID: 16913 RVA: 0x0015DAC7 File Offset: 0x0015BCC7
		public T2 Item002
		{
			get
			{
				return this._item2;
			}
			set
			{
				this._item2 = value;
				this._valuesSet[2] = true;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x0015DADD File Offset: 0x0015BCDD
		// (set) Token: 0x06004213 RID: 16915 RVA: 0x0015DAE5 File Offset: 0x0015BCE5
		public T3 Item003
		{
			get
			{
				return this._item3;
			}
			set
			{
				this._item3 = value;
				this._valuesSet[3] = true;
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0015DAFC File Offset: 0x0015BCFC
		protected override object GetValueImpl(int index)
		{
			switch (index)
			{
			case 0:
				return base.Item000;
			case 1:
				return base.Item001;
			case 2:
				return this.Item002;
			case 3:
				return this.Item003;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0015DB60 File Offset: 0x0015BD60
		protected override void SetValueImpl(int index, object value)
		{
			switch (index)
			{
			case 0:
				base.Item000 = LanguagePrimitives.ConvertTo<T0>(value);
				return;
			case 1:
				base.Item001 = LanguagePrimitives.ConvertTo<T1>(value);
				return;
			case 2:
				this.Item002 = LanguagePrimitives.ConvertTo<T2>(value);
				return;
			case 3:
				this.Item003 = LanguagePrimitives.ConvertTo<T3>(value);
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x0015DBC5 File Offset: 0x0015BDC5
		public override int Capacity
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0400210C RID: 8460
		private T2 _item2;

		// Token: 0x0400210D RID: 8461
		private T3 _item3;
	}
}
