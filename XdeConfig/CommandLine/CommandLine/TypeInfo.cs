using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine
{
	// Token: 0x02000047 RID: 71
	public sealed class TypeInfo
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00006434 File Offset: 0x00004634
		private TypeInfo(Type current, IEnumerable<Type> choices)
		{
			this.current = current;
			this.choices = choices;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000644A File Offset: 0x0000464A
		public Type Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006452 File Offset: 0x00004652
		public IEnumerable<Type> Choices
		{
			get
			{
				return this.choices;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000645A File Offset: 0x0000465A
		internal static TypeInfo Create(Type current)
		{
			return new TypeInfo(current, Enumerable.Empty<Type>());
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006467 File Offset: 0x00004667
		internal static TypeInfo Create(Type current, IEnumerable<Type> choices)
		{
			return new TypeInfo(current, choices);
		}

		// Token: 0x04000070 RID: 112
		private readonly Type current;

		// Token: 0x04000071 RID: 113
		private readonly IEnumerable<Type> choices;
	}
}
