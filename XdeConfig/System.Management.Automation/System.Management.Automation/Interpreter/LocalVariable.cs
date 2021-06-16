using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000705 RID: 1797
	internal sealed class LocalVariable
	{
		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x00185437 File Offset: 0x00183637
		// (set) Token: 0x060049E2 RID: 18914 RVA: 0x00185447 File Offset: 0x00183647
		public bool IsBoxed
		{
			get
			{
				return (this._flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this._flags |= 1;
					return;
				}
				this._flags &= -2;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x0018546A File Offset: 0x0018366A
		public bool InClosure
		{
			get
			{
				return (this._flags & 2) != 0;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x0018547A File Offset: 0x0018367A
		public bool InClosureOrBoxed
		{
			get
			{
				return this.InClosure | this.IsBoxed;
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x00185489 File Offset: 0x00183689
		internal LocalVariable(int index, bool closure, bool boxed)
		{
			this.Index = index;
			this._flags = ((closure ? 2 : 0) | (boxed ? 1 : 0));
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x001854B0 File Offset: 0x001836B0
		internal Expression LoadFromArray(Expression frameData, Expression closure)
		{
			Expression expression = Expression.ArrayAccess(this.InClosure ? closure : frameData, new Expression[]
			{
				Expression.Constant(this.Index)
			});
			if (!this.IsBoxed)
			{
				return expression;
			}
			return Expression.Convert(expression, typeof(StrongBox<object>));
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00185504 File Offset: 0x00183704
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1} {2}", new object[]
			{
				this.Index,
				this.IsBoxed ? "boxed" : null,
				this.InClosure ? "in closure" : null
			});
		}

		// Token: 0x040023CF RID: 9167
		private const int IsBoxedFlag = 1;

		// Token: 0x040023D0 RID: 9168
		private const int InClosureFlag = 2;

		// Token: 0x040023D1 RID: 9169
		public readonly int Index;

		// Token: 0x040023D2 RID: 9170
		private int _flags;
	}
}
