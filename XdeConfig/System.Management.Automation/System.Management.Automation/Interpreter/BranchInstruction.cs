using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000685 RID: 1669
	internal class BranchInstruction : OffsetInstruction
	{
		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x0017A504 File Offset: 0x00178704
		public override Instruction[] Cache
		{
			get
			{
				if (BranchInstruction._caches == null)
				{
					BranchInstruction._caches = new Instruction[][][]
					{
						new Instruction[2][],
						new Instruction[2][]
					};
				}
				Instruction[] result;
				if ((result = BranchInstruction._caches[this.ConsumedStack][this.ProducedStack]) == null)
				{
					result = (BranchInstruction._caches[this.ConsumedStack][this.ProducedStack] = new Instruction[32]);
				}
				return result;
			}
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0017A56B File Offset: 0x0017876B
		internal BranchInstruction() : this(false, false)
		{
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x0017A575 File Offset: 0x00178775
		public BranchInstruction(bool hasResult, bool hasValue)
		{
			this._hasResult = hasResult;
			this._hasValue = hasValue;
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x060046D3 RID: 18131 RVA: 0x0017A58B File Offset: 0x0017878B
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

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x060046D4 RID: 18132 RVA: 0x0017A598 File Offset: 0x00178798
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

		// Token: 0x060046D5 RID: 18133 RVA: 0x0017A5A5 File Offset: 0x001787A5
		public override int Run(InterpretedFrame frame)
		{
			return this._offset;
		}

		// Token: 0x040022C2 RID: 8898
		private static Instruction[][][] _caches;

		// Token: 0x040022C3 RID: 8899
		internal readonly bool _hasResult;

		// Token: 0x040022C4 RID: 8900
		internal readonly bool _hasValue;
	}
}
