using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000682 RID: 1666
	internal sealed class BranchFalseInstruction : OffsetInstruction
	{
		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x0017A452 File Offset: 0x00178652
		public override Instruction[] Cache
		{
			get
			{
				if (BranchFalseInstruction._cache == null)
				{
					BranchFalseInstruction._cache = new Instruction[32];
				}
				return BranchFalseInstruction._cache;
			}
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x0017A46C File Offset: 0x0017866C
		internal BranchFalseInstruction()
		{
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x0017A474 File Offset: 0x00178674
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0017A477 File Offset: 0x00178677
		public override int Run(InterpretedFrame frame)
		{
			if (!(bool)frame.Pop())
			{
				return this._offset;
			}
			return 1;
		}

		// Token: 0x040022BF RID: 8895
		private static Instruction[] _cache;
	}
}
