using System;
using System.Management.Automation.Language;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200072E RID: 1838
	internal class UpdatePositionInstruction : Instruction
	{
		// Token: 0x06004A69 RID: 19049 RVA: 0x00187443 File Offset: 0x00185643
		private UpdatePositionInstruction(bool checkBreakpoints, int sequencePoint)
		{
			this._checkBreakpoints = checkBreakpoints;
			this._sequencePoint = sequencePoint;
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0018745C File Offset: 0x0018565C
		public override int Run(InterpretedFrame frame)
		{
			FunctionContext functionContext = frame.FunctionContext;
			ExecutionContext executionContext = frame.ExecutionContext;
			functionContext._currentSequencePointIndex = this._sequencePoint;
			if (this._checkBreakpoints && executionContext._debuggingMode > 0)
			{
				executionContext.Debugger.OnSequencePointHit(functionContext);
			}
			return 1;
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x001874A1 File Offset: 0x001856A1
		public static Instruction Create(int sequencePoint, bool checkBreakpoints)
		{
			return new UpdatePositionInstruction(checkBreakpoints, sequencePoint);
		}

		// Token: 0x0400240C RID: 9228
		private readonly int _sequencePoint;

		// Token: 0x0400240D RID: 9229
		private readonly bool _checkBreakpoints;
	}
}
