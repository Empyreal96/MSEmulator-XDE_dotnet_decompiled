using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000746 RID: 1862
	internal sealed class CreateDelegateInstruction : Instruction
	{
		// Token: 0x06004AAF RID: 19119 RVA: 0x00187EF5 File Offset: 0x001860F5
		internal CreateDelegateInstruction(LightDelegateCreator delegateCreator)
		{
			this._creator = delegateCreator;
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06004AB0 RID: 19120 RVA: 0x00187F04 File Offset: 0x00186104
		public override int ConsumedStack
		{
			get
			{
				return this._creator.Interpreter.ClosureSize;
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06004AB1 RID: 19121 RVA: 0x00187F16 File Offset: 0x00186116
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00187F1C File Offset: 0x0018611C
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object>[] array;
			if (this.ConsumedStack > 0)
			{
				array = new StrongBox<object>[this.ConsumedStack];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					array[i] = (StrongBox<object>)frame.Pop();
				}
			}
			else
			{
				array = null;
			}
			Delegate value = this._creator.CreateDelegate(array);
			frame.Push(value);
			return 1;
		}

		// Token: 0x04002423 RID: 9251
		private readonly LightDelegateCreator _creator;
	}
}
