using System;
using System.CodeDom.Compiler;

namespace System.Management.Automation
{
	// Token: 0x020005FD RID: 1533
	[GeneratedCode("DLR", "2.0")]
	internal class MutableTuple<T0, T1, T2, T3, T4, T5, T6, T7> : MutableTuple<T0, T1, T2, T3>
	{
		// Token: 0x06004217 RID: 16919 RVA: 0x0015DBC8 File Offset: 0x0015BDC8
		public MutableTuple()
		{
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x0015DBD0 File Offset: 0x0015BDD0
		public MutableTuple(T0 item0, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) : base(item0, item1, item2, item3)
		{
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x0015DBFD File Offset: 0x0015BDFD
		// (set) Token: 0x0600421A RID: 16922 RVA: 0x0015DC05 File Offset: 0x0015BE05
		public T4 Item004
		{
			get
			{
				return this._item4;
			}
			set
			{
				this._item4 = value;
				this._valuesSet[4] = true;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0015DC1B File Offset: 0x0015BE1B
		// (set) Token: 0x0600421C RID: 16924 RVA: 0x0015DC23 File Offset: 0x0015BE23
		public T5 Item005
		{
			get
			{
				return this._item5;
			}
			set
			{
				this._item5 = value;
				this._valuesSet[5] = true;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x0015DC39 File Offset: 0x0015BE39
		// (set) Token: 0x0600421E RID: 16926 RVA: 0x0015DC41 File Offset: 0x0015BE41
		public T6 Item006
		{
			get
			{
				return this._item6;
			}
			set
			{
				this._item6 = value;
				this._valuesSet[6] = true;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x0015DC57 File Offset: 0x0015BE57
		// (set) Token: 0x06004220 RID: 16928 RVA: 0x0015DC5F File Offset: 0x0015BE5F
		public T7 Item007
		{
			get
			{
				return this._item7;
			}
			set
			{
				this._item7 = value;
				this._valuesSet[7] = true;
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0015DC78 File Offset: 0x0015BE78
		protected override object GetValueImpl(int index)
		{
			switch (index)
			{
			case 0:
				return base.Item000;
			case 1:
				return base.Item001;
			case 2:
				return base.Item002;
			case 3:
				return base.Item003;
			case 4:
				return this.Item004;
			case 5:
				return this.Item005;
			case 6:
				return this.Item006;
			case 7:
				return this.Item007;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0015DD1C File Offset: 0x0015BF1C
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
				base.Item002 = LanguagePrimitives.ConvertTo<T2>(value);
				return;
			case 3:
				base.Item003 = LanguagePrimitives.ConvertTo<T3>(value);
				return;
			case 4:
				this.Item004 = LanguagePrimitives.ConvertTo<T4>(value);
				return;
			case 5:
				this.Item005 = LanguagePrimitives.ConvertTo<T5>(value);
				return;
			case 6:
				this.Item006 = LanguagePrimitives.ConvertTo<T6>(value);
				return;
			case 7:
				this.Item007 = LanguagePrimitives.ConvertTo<T7>(value);
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x0015DDC5 File Offset: 0x0015BFC5
		public override int Capacity
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0400210E RID: 8462
		private T4 _item4;

		// Token: 0x0400210F RID: 8463
		private T5 _item5;

		// Token: 0x04002110 RID: 8464
		private T6 _item6;

		// Token: 0x04002111 RID: 8465
		private T7 _item7;
	}
}
