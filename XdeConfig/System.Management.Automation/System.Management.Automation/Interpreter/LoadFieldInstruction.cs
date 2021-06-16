using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006BC RID: 1724
	internal sealed class LoadFieldInstruction : Instruction
	{
		// Token: 0x060047B8 RID: 18360 RVA: 0x0017CC17 File Offset: 0x0017AE17
		public LoadFieldInstruction(FieldInfo field)
		{
			this._field = field;
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x0017CC26 File Offset: 0x0017AE26
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x0017CC29 File Offset: 0x0017AE29
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x0017CC2C File Offset: 0x0017AE2C
		public override int Run(InterpretedFrame frame)
		{
			frame.Push(this._field.GetValue(frame.Pop()));
			return 1;
		}

		// Token: 0x04002312 RID: 8978
		private readonly FieldInfo _field;
	}
}
