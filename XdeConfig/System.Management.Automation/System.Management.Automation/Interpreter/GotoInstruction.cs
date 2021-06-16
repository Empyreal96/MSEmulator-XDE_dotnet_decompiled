using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000687 RID: 1671
	internal sealed class GotoInstruction : IndexedBranchInstruction
	{
		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x060046DA RID: 18138 RVA: 0x0017A661 File Offset: 0x00178861
		public override int ConsumedContinuations
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x060046DB RID: 18139 RVA: 0x0017A664 File Offset: 0x00178864
		public override int ProducedContinuations
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x060046DC RID: 18140 RVA: 0x0017A667 File Offset: 0x00178867
		public override int ConsumedStack
		{
			get
			{
				if (!this._hasValue)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x0017A674 File Offset: 0x00178874
		public override int ProducedStack
		{
			get
			{
				if (!this._hasResult)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x0017A681 File Offset: 0x00178881
		private GotoInstruction(int targetIndex, bool hasResult, bool hasValue) : base(targetIndex)
		{
			this._hasResult = hasResult;
			this._hasValue = hasValue;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x0017A698 File Offset: 0x00178898
		internal static GotoInstruction Create(int labelIndex, bool hasResult, bool hasValue)
		{
			if (labelIndex < 32)
			{
				int num = 4 * labelIndex | (hasResult ? 2 : 0) | (hasValue ? 1 : 0);
				GotoInstruction result;
				if ((result = GotoInstruction.Cache[num]) == null)
				{
					result = (GotoInstruction.Cache[num] = new GotoInstruction(labelIndex, hasResult, hasValue));
				}
				return result;
			}
			return new GotoInstruction(labelIndex, hasResult, hasValue);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x0017A6E4 File Offset: 0x001788E4
		public override int Run(InterpretedFrame frame)
		{
			Interpreter.AbortThreadIfRequested(frame, this._labelIndex);
			return frame.Goto(this._labelIndex, this._hasValue ? frame.Pop() : Interpreter.NoValue, false);
		}

		// Token: 0x040022C7 RID: 8903
		private const int Variants = 4;

		// Token: 0x040022C8 RID: 8904
		private static readonly GotoInstruction[] Cache = new GotoInstruction[128];

		// Token: 0x040022C9 RID: 8905
		private readonly bool _hasResult;

		// Token: 0x040022CA RID: 8906
		private readonly bool _hasValue;
	}
}
