using System;

namespace CSharpx
{
	// Token: 0x02000022 RID: 34
	internal sealed class Right<TLeft, TRight> : Either<TLeft, TRight>
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004493 File Offset: 0x00002693
		internal Right(TRight value) : base(EitherType.Right)
		{
			this.value = value;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000044A3 File Offset: 0x000026A3
		public TRight Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0400003C RID: 60
		private readonly TRight value;
	}
}
