using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000747 RID: 1863
	internal sealed class NewInstruction : Instruction
	{
		// Token: 0x06004AB3 RID: 19123 RVA: 0x00187F75 File Offset: 0x00186175
		public NewInstruction(ConstructorInfo constructor)
		{
			this._constructor = constructor;
			this._argCount = constructor.GetParameters().Length;
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x00187F92 File Offset: 0x00186192
		public override int ConsumedStack
		{
			get
			{
				return this._argCount;
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x00187F9A File Offset: 0x0018619A
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x00187FA0 File Offset: 0x001861A0
		public override int Run(InterpretedFrame frame)
		{
			object[] array = new object[this._argCount];
			for (int i = this._argCount - 1; i >= 0; i--)
			{
				array[i] = frame.Pop();
			}
			object value;
			try
			{
				value = this._constructor.Invoke(array);
			}
			catch (TargetInvocationException ex)
			{
				ExceptionHelpers.UpdateForRethrow(ex.InnerException);
				throw ex.InnerException;
			}
			frame.Push(value);
			return 1;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00188010 File Offset: 0x00186210
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"New ",
				this._constructor.DeclaringType.Name,
				"(",
				this._constructor,
				")"
			});
		}

		// Token: 0x04002424 RID: 9252
		private readonly ConstructorInfo _constructor;

		// Token: 0x04002425 RID: 9253
		private readonly int _argCount;
	}
}
