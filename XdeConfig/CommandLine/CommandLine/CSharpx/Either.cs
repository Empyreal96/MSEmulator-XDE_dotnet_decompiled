using System;

namespace CSharpx
{
	// Token: 0x02000020 RID: 32
	internal abstract class Either<TLeft, TRight>
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000043EB File Offset: 0x000025EB
		protected Either(EitherType tag)
		{
			this.tag = tag;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000043FA File Offset: 0x000025FA
		public EitherType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004404 File Offset: 0x00002604
		public bool MatchLeft(out TLeft value)
		{
			value = ((this.Tag == EitherType.Left) ? ((Left<TLeft, TRight>)this).Value : default(TLeft));
			return this.Tag == EitherType.Left;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004440 File Offset: 0x00002640
		public bool MatchRight(out TRight value)
		{
			value = ((this.Tag == EitherType.Right) ? ((Right<TLeft, TRight>)this).Value : default(TRight));
			return this.Tag == EitherType.Right;
		}

		// Token: 0x0400003A RID: 58
		private readonly EitherType tag;
	}
}
