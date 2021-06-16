using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000691 RID: 1681
	internal sealed class CompiledLoopInstruction : Instruction
	{
		// Token: 0x06004712 RID: 18194 RVA: 0x0017ADBC File Offset: 0x00178FBC
		public CompiledLoopInstruction(Func<object[], StrongBox<object>[], InterpretedFrame, int> compiledLoop)
		{
			this._compiledLoop = compiledLoop;
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x0017ADCB File Offset: 0x00178FCB
		public override int Run(InterpretedFrame frame)
		{
			return this._compiledLoop(frame.Data, frame.Closure, frame);
		}

		// Token: 0x040022E4 RID: 8932
		private readonly Func<object[], StrongBox<object>[], InterpretedFrame, int> _compiledLoop;
	}
}
