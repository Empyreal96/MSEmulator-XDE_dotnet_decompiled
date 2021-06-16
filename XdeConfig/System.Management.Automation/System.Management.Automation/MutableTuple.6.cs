using System;
using System.CodeDom.Compiler;

namespace System.Management.Automation
{
	// Token: 0x020005FE RID: 1534
	[GeneratedCode("DLR", "2.0")]
	internal class MutableTuple<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : MutableTuple<T0, T1, T2, T3, T4, T5, T6, T7>
	{
		// Token: 0x06004224 RID: 16932 RVA: 0x0015DDC8 File Offset: 0x0015BFC8
		public MutableTuple()
		{
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0015DDD0 File Offset: 0x0015BFD0
		public MutableTuple(T0 item0, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15) : base(item0, item1, item2, item3, item4, item5, item6, item7)
		{
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
			this._item12 = item12;
			this._item13 = item13;
			this._item14 = item14;
			this._item15 = item15;
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x0015DE30 File Offset: 0x0015C030
		// (set) Token: 0x06004227 RID: 16935 RVA: 0x0015DE38 File Offset: 0x0015C038
		public T8 Item008
		{
			get
			{
				return this._item8;
			}
			set
			{
				this._item8 = value;
				this._valuesSet[8] = true;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06004228 RID: 16936 RVA: 0x0015DE4E File Offset: 0x0015C04E
		// (set) Token: 0x06004229 RID: 16937 RVA: 0x0015DE56 File Offset: 0x0015C056
		public T9 Item009
		{
			get
			{
				return this._item9;
			}
			set
			{
				this._item9 = value;
				this._valuesSet[9] = true;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x0015DE6D File Offset: 0x0015C06D
		// (set) Token: 0x0600422B RID: 16939 RVA: 0x0015DE75 File Offset: 0x0015C075
		public T10 Item010
		{
			get
			{
				return this._item10;
			}
			set
			{
				this._item10 = value;
				this._valuesSet[10] = true;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x0600422C RID: 16940 RVA: 0x0015DE8C File Offset: 0x0015C08C
		// (set) Token: 0x0600422D RID: 16941 RVA: 0x0015DE94 File Offset: 0x0015C094
		public T11 Item011
		{
			get
			{
				return this._item11;
			}
			set
			{
				this._item11 = value;
				this._valuesSet[11] = true;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x0015DEAB File Offset: 0x0015C0AB
		// (set) Token: 0x0600422F RID: 16943 RVA: 0x0015DEB3 File Offset: 0x0015C0B3
		public T12 Item012
		{
			get
			{
				return this._item12;
			}
			set
			{
				this._item12 = value;
				this._valuesSet[12] = true;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x0015DECA File Offset: 0x0015C0CA
		// (set) Token: 0x06004231 RID: 16945 RVA: 0x0015DED2 File Offset: 0x0015C0D2
		public T13 Item013
		{
			get
			{
				return this._item13;
			}
			set
			{
				this._item13 = value;
				this._valuesSet[13] = true;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x0015DEE9 File Offset: 0x0015C0E9
		// (set) Token: 0x06004233 RID: 16947 RVA: 0x0015DEF1 File Offset: 0x0015C0F1
		public T14 Item014
		{
			get
			{
				return this._item14;
			}
			set
			{
				this._item14 = value;
				this._valuesSet[14] = true;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x0015DF08 File Offset: 0x0015C108
		// (set) Token: 0x06004235 RID: 16949 RVA: 0x0015DF10 File Offset: 0x0015C110
		public T15 Item015
		{
			get
			{
				return this._item15;
			}
			set
			{
				this._item15 = value;
				this._valuesSet[15] = true;
			}
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0015DF28 File Offset: 0x0015C128
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
				return base.Item004;
			case 5:
				return base.Item005;
			case 6:
				return base.Item006;
			case 7:
				return base.Item007;
			case 8:
				return this.Item008;
			case 9:
				return this.Item009;
			case 10:
				return this.Item010;
			case 11:
				return this.Item011;
			case 12:
				return this.Item012;
			case 13:
				return this.Item013;
			case 14:
				return this.Item014;
			case 15:
				return this.Item015;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0015E04C File Offset: 0x0015C24C
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
				base.Item004 = LanguagePrimitives.ConvertTo<T4>(value);
				return;
			case 5:
				base.Item005 = LanguagePrimitives.ConvertTo<T5>(value);
				return;
			case 6:
				base.Item006 = LanguagePrimitives.ConvertTo<T6>(value);
				return;
			case 7:
				base.Item007 = LanguagePrimitives.ConvertTo<T7>(value);
				return;
			case 8:
				this.Item008 = LanguagePrimitives.ConvertTo<T8>(value);
				return;
			case 9:
				this.Item009 = LanguagePrimitives.ConvertTo<T9>(value);
				return;
			case 10:
				this.Item010 = LanguagePrimitives.ConvertTo<T10>(value);
				return;
			case 11:
				this.Item011 = LanguagePrimitives.ConvertTo<T11>(value);
				return;
			case 12:
				this.Item012 = LanguagePrimitives.ConvertTo<T12>(value);
				return;
			case 13:
				this.Item013 = LanguagePrimitives.ConvertTo<T13>(value);
				return;
			case 14:
				this.Item014 = LanguagePrimitives.ConvertTo<T14>(value);
				return;
			case 15:
				this.Item015 = LanguagePrimitives.ConvertTo<T15>(value);
				return;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x0015E180 File Offset: 0x0015C380
		public override int Capacity
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x04002112 RID: 8466
		private T8 _item8;

		// Token: 0x04002113 RID: 8467
		private T9 _item9;

		// Token: 0x04002114 RID: 8468
		private T10 _item10;

		// Token: 0x04002115 RID: 8469
		private T11 _item11;

		// Token: 0x04002116 RID: 8470
		private T12 _item12;

		// Token: 0x04002117 RID: 8471
		private T13 _item13;

		// Token: 0x04002118 RID: 8472
		private T14 _item14;

		// Token: 0x04002119 RID: 8473
		private T15 _item15;
	}
}
