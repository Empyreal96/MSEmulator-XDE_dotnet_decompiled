using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006FB RID: 1787
	internal sealed class AssignLocalToClosureInstruction : LocalAccessInstruction
	{
		// Token: 0x060049BF RID: 18879 RVA: 0x001851DB File Offset: 0x001833DB
		internal AssignLocalToClosureInstruction(int index) : base(index)
		{
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x001851E4 File Offset: 0x001833E4
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x001851E7 File Offset: 0x001833E7
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x001851EC File Offset: 0x001833EC
		public override int Run(InterpretedFrame frame)
		{
			StrongBox<object> strongBox = frame.Closure[this._index];
			strongBox.Value = frame.Peek();
			return 1;
		}
	}
}
