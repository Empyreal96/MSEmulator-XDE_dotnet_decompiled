using System;
using System.Globalization;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000669 RID: 1641
	internal struct RuntimeLabel
	{
		// Token: 0x060045F8 RID: 17912 RVA: 0x00177259 File Offset: 0x00175459
		public RuntimeLabel(int index, int continuationStackDepth, int stackDepth)
		{
			this.Index = index;
			this.ContinuationStackDepth = continuationStackDepth;
			this.StackDepth = stackDepth;
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x00177270 File Offset: 0x00175470
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "->{0} C({1}) S({2})", new object[]
			{
				this.Index,
				this.ContinuationStackDepth,
				this.StackDepth
			});
		}

		// Token: 0x04002299 RID: 8857
		public readonly int Index;

		// Token: 0x0400229A RID: 8858
		public readonly int StackDepth;

		// Token: 0x0400229B RID: 8859
		public readonly int ContinuationStackDepth;
	}
}
