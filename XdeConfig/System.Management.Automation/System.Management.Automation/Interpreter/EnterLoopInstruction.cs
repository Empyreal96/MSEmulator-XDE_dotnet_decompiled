using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Threading;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000690 RID: 1680
	internal sealed class EnterLoopInstruction : Instruction
	{
		// Token: 0x0600470D RID: 18189 RVA: 0x0017AC66 File Offset: 0x00178E66
		internal EnterLoopInstruction(PowerShellLoopExpression loop, LocalVariables locals, int compilationThreshold, int instructionIndex)
		{
			this._loop = loop;
			this._variables = locals.CopyLocals();
			this._closureVariables = locals.ClosureVariables;
			this._compilationThreshold = compilationThreshold;
			this._instructionIndex = instructionIndex;
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x0017AC9C File Offset: 0x00178E9C
		internal void FinishLoop(int loopEnd)
		{
			this._loopEnd = loopEnd;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x0017ACA8 File Offset: 0x00178EA8
		public override int Run(InterpretedFrame frame)
		{
			if (this._compilationThreshold-- == 0)
			{
				if (frame.Interpreter.CompileSynchronously)
				{
					this.Compile(frame);
				}
				else
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.Compile), frame);
				}
			}
			return 1;
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06004710 RID: 18192 RVA: 0x0017ACF2 File Offset: 0x00178EF2
		private bool Compiled
		{
			get
			{
				return this._loop == null;
			}
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x0017AD00 File Offset: 0x00178F00
		private void Compile(object frameObj)
		{
			if (this.Compiled)
			{
				return;
			}
			lock (this)
			{
				if (!this.Compiled)
				{
					InterpretedFrame interpretedFrame = (InterpretedFrame)frameObj;
					LoopCompiler loopCompiler = new LoopCompiler(this._loop, interpretedFrame.Interpreter.LabelMapping, this._variables, this._closureVariables, this._instructionIndex, this._loopEnd);
					Instruction[] instructions = interpretedFrame.Interpreter.Instructions.Instructions;
					instructions[this._instructionIndex] = new CompiledLoopInstruction(loopCompiler.CreateDelegate());
					this._loop = null;
					this._variables = null;
					this._closureVariables = null;
				}
			}
		}

		// Token: 0x040022DE RID: 8926
		private readonly int _instructionIndex;

		// Token: 0x040022DF RID: 8927
		private Dictionary<ParameterExpression, LocalVariable> _variables;

		// Token: 0x040022E0 RID: 8928
		private Dictionary<ParameterExpression, LocalVariable> _closureVariables;

		// Token: 0x040022E1 RID: 8929
		private PowerShellLoopExpression _loop;

		// Token: 0x040022E2 RID: 8930
		private int _loopEnd;

		// Token: 0x040022E3 RID: 8931
		private int _compilationThreshold;
	}
}
