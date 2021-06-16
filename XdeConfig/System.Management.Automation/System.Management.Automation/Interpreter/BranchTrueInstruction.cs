using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000683 RID: 1667
	internal sealed class BranchTrueInstruction : OffsetInstruction
	{
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x0017A48E File Offset: 0x0017868E
		public override Instruction[] Cache
		{
			get
			{
				if (BranchTrueInstruction._cache == null)
				{
					BranchTrueInstruction._cache = new Instruction[32];
				}
				return BranchTrueInstruction._cache;
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0017A4A8 File Offset: 0x001786A8
		internal BranchTrueInstruction()
		{
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x0017A4B0 File Offset: 0x001786B0
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x0017A4B3 File Offset: 0x001786B3
		public override int Run(InterpretedFrame frame)
		{
			if ((bool)frame.Pop())
			{
				return this._offset;
			}
			return 1;
		}

		// Token: 0x040022C0 RID: 8896
		private static Instruction[] _cache;
	}
}
