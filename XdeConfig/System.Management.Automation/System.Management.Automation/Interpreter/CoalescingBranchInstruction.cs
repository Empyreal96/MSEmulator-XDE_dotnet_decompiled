using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000684 RID: 1668
	internal sealed class CoalescingBranchInstruction : OffsetInstruction
	{
		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x0017A4CA File Offset: 0x001786CA
		public override Instruction[] Cache
		{
			get
			{
				if (CoalescingBranchInstruction._cache == null)
				{
					CoalescingBranchInstruction._cache = new Instruction[32];
				}
				return CoalescingBranchInstruction._cache;
			}
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x0017A4E4 File Offset: 0x001786E4
		internal CoalescingBranchInstruction()
		{
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x0017A4EC File Offset: 0x001786EC
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x0017A4EF File Offset: 0x001786EF
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0017A4F2 File Offset: 0x001786F2
		public override int Run(InterpretedFrame frame)
		{
			if (frame.Peek() != null)
			{
				return this._offset;
			}
			return 1;
		}

		// Token: 0x040022C1 RID: 8897
		private static Instruction[] _cache;
	}
}
