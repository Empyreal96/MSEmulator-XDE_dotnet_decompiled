using System;

namespace CSharpx
{
	// Token: 0x02000021 RID: 33
	internal sealed class Left<TLeft, TRight> : Either<TLeft, TRight>
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x0000447B File Offset: 0x0000267B
		internal Left(TLeft value) : base(EitherType.Left)
		{
			this.value = value;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000448B File Offset: 0x0000268B
		public TLeft Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0400003B RID: 59
		private readonly TLeft value;
	}
}
