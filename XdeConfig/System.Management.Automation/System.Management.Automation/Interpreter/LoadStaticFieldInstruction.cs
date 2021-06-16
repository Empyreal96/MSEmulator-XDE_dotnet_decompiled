using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006BB RID: 1723
	internal sealed class LoadStaticFieldInstruction : Instruction
	{
		// Token: 0x060047B5 RID: 18357 RVA: 0x0017CBF0 File Offset: 0x0017ADF0
		public LoadStaticFieldInstruction(FieldInfo field)
		{
			this._field = field;
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x0017CBFF File Offset: 0x0017ADFF
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0017CC02 File Offset: 0x0017AE02
		public override int Run(InterpretedFrame frame)
		{
			frame.Push(this._field.GetValue(null));
			return 1;
		}

		// Token: 0x04002311 RID: 8977
		private readonly FieldInfo _field;
	}
}
