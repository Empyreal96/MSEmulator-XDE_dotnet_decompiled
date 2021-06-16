using System;

namespace CSharpx
{
	// Token: 0x02000029 RID: 41
	internal sealed class Just<T> : Maybe<T>
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00004CDA File Offset: 0x00002EDA
		internal Just(T value) : base(MaybeType.Just)
		{
			this.value = value;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004CEA File Offset: 0x00002EEA
		public T Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000042 RID: 66
		private readonly T value;
	}
}
