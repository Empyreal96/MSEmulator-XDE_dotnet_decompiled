using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006BD RID: 1725
	internal sealed class StoreFieldInstruction : Instruction
	{
		// Token: 0x060047BC RID: 18364 RVA: 0x0017CC46 File Offset: 0x0017AE46
		public StoreFieldInstruction(FieldInfo field)
		{
			this._field = field;
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060047BD RID: 18365 RVA: 0x0017CC55 File Offset: 0x0017AE55
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x0017CC58 File Offset: 0x0017AE58
		public override int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0017CC5C File Offset: 0x0017AE5C
		public override int Run(InterpretedFrame frame)
		{
			object value = frame.Pop();
			object obj = frame.Pop();
			this._field.SetValue(obj, value);
			return 1;
		}

		// Token: 0x04002313 RID: 8979
		private readonly FieldInfo _field;
	}
}
