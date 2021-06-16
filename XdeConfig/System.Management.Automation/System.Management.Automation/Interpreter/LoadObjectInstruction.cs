using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000730 RID: 1840
	internal sealed class LoadObjectInstruction : Instruction
	{
		// Token: 0x06004A71 RID: 19057 RVA: 0x001874EA File Offset: 0x001856EA
		internal LoadObjectInstruction(object value)
		{
			this._value = value;
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x001874F9 File Offset: 0x001856F9
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x001874FC File Offset: 0x001856FC
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex++] = this._value;
			return 1;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x00187528 File Offset: 0x00185728
		public override string ToString()
		{
			return "LoadObject(" + (this._value ?? "null") + ")";
		}

		// Token: 0x0400240F RID: 9231
		private readonly object _value;
	}
}
