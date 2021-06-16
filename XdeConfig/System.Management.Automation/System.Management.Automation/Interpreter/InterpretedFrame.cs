using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D4 RID: 1748
	internal sealed class InterpretedFrame
	{
		// Token: 0x06004871 RID: 18545 RVA: 0x0017E594 File Offset: 0x0017C794
		internal InterpretedFrame(Interpreter interpreter, StrongBox<object>[] closure)
		{
			this.Interpreter = interpreter;
			this.StackIndex = interpreter.LocalCount;
			this.Data = new object[this.StackIndex + interpreter.Instructions.MaxStackDepth];
			int maxContinuationDepth = interpreter.Instructions.MaxContinuationDepth;
			if (maxContinuationDepth > 0)
			{
				this._continuations = new int[maxContinuationDepth];
			}
			this.Closure = closure;
			this._pendingContinuation = -1;
			this._pendingValue = Interpreter.NoValue;
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x0017E60C File Offset: 0x0017C80C
		public DebugInfo GetDebugInfo(int instructionIndex)
		{
			return DebugInfo.GetMatchingDebugInfo(this.Interpreter._debugInfos, instructionIndex);
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x0017E61F File Offset: 0x0017C81F
		public string Name
		{
			get
			{
				return this.Interpreter._name;
			}
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x0017E62C File Offset: 0x0017C82C
		public void Push(object value)
		{
			this.Data[this.StackIndex++] = value;
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0017E654 File Offset: 0x0017C854
		public void Push(bool value)
		{
			this.Data[this.StackIndex++] = (value ? ScriptingRuntimeHelpers.True : ScriptingRuntimeHelpers.False);
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x0017E688 File Offset: 0x0017C888
		public void Push(int value)
		{
			this.Data[this.StackIndex++] = ScriptingRuntimeHelpers.Int32ToObject(value);
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x0017E6B4 File Offset: 0x0017C8B4
		public object Pop()
		{
			return this.Data[--this.StackIndex];
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x0017E6D9 File Offset: 0x0017C8D9
		internal void SetStackDepth(int depth)
		{
			this.StackIndex = this.Interpreter.LocalCount + depth;
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x0017E6EE File Offset: 0x0017C8EE
		public object Peek()
		{
			return this.Data[this.StackIndex - 1];
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x0017E700 File Offset: 0x0017C900
		public void Dup()
		{
			int stackIndex = this.StackIndex;
			this.Data[stackIndex] = this.Data[stackIndex - 1];
			this.StackIndex = stackIndex + 1;
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x0600487B RID: 18555 RVA: 0x0017E72F File Offset: 0x0017C92F
		public ExecutionContext ExecutionContext
		{
			get
			{
				return (ExecutionContext)this.Data[1];
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x0600487C RID: 18556 RVA: 0x0017E73E File Offset: 0x0017C93E
		public FunctionContext FunctionContext
		{
			get
			{
				return (FunctionContext)this.Data[0];
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x0600487D RID: 18557 RVA: 0x0017E74D File Offset: 0x0017C94D
		public InterpretedFrame Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x0017E755 File Offset: 0x0017C955
		public static bool IsInterpretedFrame(MethodBase method)
		{
			return method.DeclaringType == typeof(Interpreter) && method.Name == "Run";
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x0017E938 File Offset: 0x0017CB38
		public static IEnumerable<StackFrame> GroupStackFrames(IEnumerable<StackFrame> stackTrace)
		{
			bool inInterpretedFrame = false;
			foreach (StackFrame frame in stackTrace)
			{
				if (InterpretedFrame.IsInterpretedFrame(frame.GetMethod()))
				{
					if (inInterpretedFrame)
					{
						continue;
					}
					inInterpretedFrame = true;
				}
				else
				{
					inInterpretedFrame = false;
				}
				yield return frame;
			}
			yield break;
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x0017EA6C File Offset: 0x0017CC6C
		public IEnumerable<InterpretedFrameInfo> GetStackTraceDebugInfo()
		{
			InterpretedFrame frame = this;
			do
			{
				yield return new InterpretedFrameInfo(frame.Name, frame.GetDebugInfo(frame.InstructionIndex));
				frame = frame.Parent;
			}
			while (frame != null);
			yield break;
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x0017EA89 File Offset: 0x0017CC89
		internal void SaveTraceToException(Exception exception)
		{
			if (exception.Data[typeof(InterpretedFrameInfo)] == null)
			{
				exception.Data[typeof(InterpretedFrameInfo)] = new List<InterpretedFrameInfo>(this.GetStackTraceDebugInfo()).ToArray();
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0017EAC7 File Offset: 0x0017CCC7
		public static InterpretedFrameInfo[] GetExceptionStackTrace(Exception exception)
		{
			return exception.Data[typeof(InterpretedFrameInfo)] as InterpretedFrameInfo[];
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0017EAE4 File Offset: 0x0017CCE4
		internal ThreadLocal<InterpretedFrame>.StorageInfo Enter()
		{
			ThreadLocal<InterpretedFrame>.StorageInfo storageInfo = InterpretedFrame.CurrentFrame.GetStorageInfo();
			this._parent = storageInfo.Value;
			storageInfo.Value = this;
			return storageInfo;
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0017EB10 File Offset: 0x0017CD10
		internal void Leave(ThreadLocal<InterpretedFrame>.StorageInfo currentFrame)
		{
			currentFrame.Value = this._parent;
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x0017EB1E File Offset: 0x0017CD1E
		internal bool IsJumpHappened()
		{
			return this._pendingContinuation >= 0;
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x0017EB2C File Offset: 0x0017CD2C
		public void RemoveContinuation()
		{
			this._continuationIndex--;
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x0017EB3C File Offset: 0x0017CD3C
		public void PushContinuation(int continuation)
		{
			this._continuations[this._continuationIndex++] = continuation;
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x0017EB64 File Offset: 0x0017CD64
		public int YieldToCurrentContinuation()
		{
			RuntimeLabel runtimeLabel = this.Interpreter._labels[this._continuations[this._continuationIndex - 1]];
			this.SetStackDepth(runtimeLabel.StackDepth);
			return runtimeLabel.Index - this.InstructionIndex;
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x0017EBB4 File Offset: 0x0017CDB4
		public int YieldToPendingContinuation()
		{
			RuntimeLabel runtimeLabel = this.Interpreter._labels[this._pendingContinuation];
			if (runtimeLabel.ContinuationStackDepth < this._continuationIndex)
			{
				RuntimeLabel runtimeLabel2 = this.Interpreter._labels[this._continuations[this._continuationIndex - 1]];
				this.SetStackDepth(runtimeLabel2.StackDepth);
				return runtimeLabel2.Index - this.InstructionIndex;
			}
			this.SetStackDepth(runtimeLabel.StackDepth);
			if (this._pendingValue != Interpreter.NoValue)
			{
				this.Data[this.StackIndex - 1] = this._pendingValue;
			}
			this._pendingContinuation = -1;
			this._pendingValue = Interpreter.NoValue;
			return runtimeLabel.Index - this.InstructionIndex;
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x0017EC7C File Offset: 0x0017CE7C
		internal void PushPendingContinuation()
		{
			this.Push(this._pendingContinuation);
			this.Push(this._pendingValue);
			this._pendingContinuation = -1;
			this._pendingValue = Interpreter.NoValue;
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x0017ECA8 File Offset: 0x0017CEA8
		internal void PopPendingContinuation()
		{
			this._pendingValue = this.Pop();
			this._pendingContinuation = (int)this.Pop();
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x0017ECC7 File Offset: 0x0017CEC7
		internal static MethodInfo GotoMethod
		{
			get
			{
				MethodInfo result;
				if ((result = InterpretedFrame._Goto) == null)
				{
					result = (InterpretedFrame._Goto = typeof(InterpretedFrame).GetMethod("Goto"));
				}
				return result;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x0017ECEC File Offset: 0x0017CEEC
		internal static MethodInfo VoidGotoMethod
		{
			get
			{
				MethodInfo result;
				if ((result = InterpretedFrame._VoidGoto) == null)
				{
					result = (InterpretedFrame._VoidGoto = typeof(InterpretedFrame).GetMethod("VoidGoto"));
				}
				return result;
			}
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0017ED11 File Offset: 0x0017CF11
		public int VoidGoto(int labelIndex)
		{
			return this.Goto(labelIndex, Interpreter.NoValue, false);
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0017ED20 File Offset: 0x0017CF20
		public int Goto(int labelIndex, object value, bool gotoExceptionHandler)
		{
			RuntimeLabel runtimeLabel = this.Interpreter._labels[labelIndex];
			if (this._continuationIndex == runtimeLabel.ContinuationStackDepth)
			{
				this.SetStackDepth(runtimeLabel.StackDepth);
				if (value != Interpreter.NoValue)
				{
					this.Data[this.StackIndex - 1] = value;
				}
				return runtimeLabel.Index - this.InstructionIndex;
			}
			this._pendingContinuation = labelIndex;
			this._pendingValue = value;
			return this.YieldToCurrentContinuation();
		}

		// Token: 0x04002358 RID: 9048
		public static readonly ThreadLocal<InterpretedFrame> CurrentFrame = new ThreadLocal<InterpretedFrame>();

		// Token: 0x04002359 RID: 9049
		internal readonly Interpreter Interpreter;

		// Token: 0x0400235A RID: 9050
		internal InterpretedFrame _parent;

		// Token: 0x0400235B RID: 9051
		private int[] _continuations;

		// Token: 0x0400235C RID: 9052
		private int _continuationIndex;

		// Token: 0x0400235D RID: 9053
		private int _pendingContinuation;

		// Token: 0x0400235E RID: 9054
		private object _pendingValue;

		// Token: 0x0400235F RID: 9055
		public readonly object[] Data;

		// Token: 0x04002360 RID: 9056
		public readonly StrongBox<object>[] Closure;

		// Token: 0x04002361 RID: 9057
		public int StackIndex;

		// Token: 0x04002362 RID: 9058
		public int InstructionIndex;

		// Token: 0x04002363 RID: 9059
		public ExceptionHandler CurrentAbortHandler;

		// Token: 0x04002364 RID: 9060
		private static MethodInfo _Goto;

		// Token: 0x04002365 RID: 9061
		private static MethodInfo _VoidGoto;
	}
}
