using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068E RID: 1678
	internal sealed class ThrowInstruction : Instruction
	{
		// Token: 0x06004704 RID: 18180 RVA: 0x0017ABA6 File Offset: 0x00178DA6
		private ThrowInstruction(bool hasResult, bool isRethrow)
		{
			this._hasResult = hasResult;
			this._rethrow = isRethrow;
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06004705 RID: 18181 RVA: 0x0017ABBC File Offset: 0x00178DBC
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

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06004706 RID: 18182 RVA: 0x0017ABC9 File Offset: 0x00178DC9
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0017ABCC File Offset: 0x00178DCC
		public override int Run(InterpretedFrame frame)
		{
			Exception ex = (Exception)frame.Pop();
			if (this._rethrow)
			{
				throw new RethrowException();
			}
			throw ex;
		}

		// Token: 0x040022D7 RID: 8919
		internal static readonly ThrowInstruction Throw = new ThrowInstruction(true, false);

		// Token: 0x040022D8 RID: 8920
		internal static readonly ThrowInstruction VoidThrow = new ThrowInstruction(false, false);

		// Token: 0x040022D9 RID: 8921
		internal static readonly ThrowInstruction Rethrow = new ThrowInstruction(true, true);

		// Token: 0x040022DA RID: 8922
		internal static readonly ThrowInstruction VoidRethrow = new ThrowInstruction(false, true);

		// Token: 0x040022DB RID: 8923
		private readonly bool _hasResult;

		// Token: 0x040022DC RID: 8924
		private readonly bool _rethrow;
	}
}
