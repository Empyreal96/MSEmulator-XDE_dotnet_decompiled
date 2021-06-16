using System;
using System.Linq.Expressions;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000706 RID: 1798
	internal struct LocalDefinition
	{
		// Token: 0x060049E8 RID: 18920 RVA: 0x0018555C File Offset: 0x0018375C
		internal LocalDefinition(int localIndex, ParameterExpression parameter)
		{
			this._index = localIndex;
			this._parameter = parameter;
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x0018556C File Offset: 0x0018376C
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x00185574 File Offset: 0x00183774
		public ParameterExpression Parameter
		{
			get
			{
				return this._parameter;
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x0018557C File Offset: 0x0018377C
		public override bool Equals(object obj)
		{
			if (obj is LocalDefinition)
			{
				LocalDefinition localDefinition = (LocalDefinition)obj;
				return localDefinition.Index == this.Index && localDefinition.Parameter == this.Parameter;
			}
			return false;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x001855BC File Offset: 0x001837BC
		public override int GetHashCode()
		{
			if (this._parameter == null)
			{
				return 0;
			}
			return this._parameter.GetHashCode() ^ this._index.GetHashCode();
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x001855ED File Offset: 0x001837ED
		public static bool operator ==(LocalDefinition self, LocalDefinition other)
		{
			return self.Index == other.Index && self.Parameter == other.Parameter;
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00185611 File Offset: 0x00183811
		public static bool operator !=(LocalDefinition self, LocalDefinition other)
		{
			return self.Index != other.Index || self.Parameter != other.Parameter;
		}

		// Token: 0x040023D3 RID: 9171
		private readonly int _index;

		// Token: 0x040023D4 RID: 9172
		private readonly ParameterExpression _parameter;
	}
}
