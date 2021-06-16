using System;

namespace CSharpx
{
	// Token: 0x02000027 RID: 39
	internal abstract class Maybe<T>
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00004C72 File Offset: 0x00002E72
		protected Maybe(MaybeType tag)
		{
			this.tag = tag;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004C81 File Offset: 0x00002E81
		public MaybeType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004C8C File Offset: 0x00002E8C
		public bool MatchJust(out T value)
		{
			value = ((this.Tag == MaybeType.Just) ? ((Just<T>)this).Value : default(T));
			return this.Tag == MaybeType.Just;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004CC6 File Offset: 0x00002EC6
		public bool MatchNothing()
		{
			return this.Tag == MaybeType.Nothing;
		}

		// Token: 0x04000041 RID: 65
		private readonly MaybeType tag;
	}
}
