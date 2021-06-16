using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006BE RID: 1726
	internal sealed class StoreStaticFieldInstruction : Instruction
	{
		// Token: 0x060047C0 RID: 18368 RVA: 0x0017CC85 File Offset: 0x0017AE85
		public StoreStaticFieldInstruction(FieldInfo field)
		{
			this._field = field;
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060047C1 RID: 18369 RVA: 0x0017CC94 File Offset: 0x0017AE94
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060047C2 RID: 18370 RVA: 0x0017CC97 File Offset: 0x0017AE97
		public override int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x0017CC9C File Offset: 0x0017AE9C
		public override int Run(InterpretedFrame frame)
		{
			object value = frame.Pop();
			this._field.SetValue(null, value);
			return 1;
		}

		// Token: 0x04002314 RID: 8980
		private readonly FieldInfo _field;
	}
}
