using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000667 RID: 1639
	internal sealed class GetArrayItemInstruction<TElement> : Instruction
	{
		// Token: 0x060045EE RID: 17902 RVA: 0x001771B7 File Offset: 0x001753B7
		internal GetArrayItemInstruction()
		{
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x001771BF File Offset: 0x001753BF
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x001771C2 File Offset: 0x001753C2
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x001771C8 File Offset: 0x001753C8
		public override int Run(InterpretedFrame frame)
		{
			int num = (int)frame.Pop();
			TElement[] array = (TElement[])frame.Pop();
			frame.Push(array[num]);
			return 1;
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060045F2 RID: 17906 RVA: 0x00177200 File Offset: 0x00175400
		public override string InstructionName
		{
			get
			{
				return "GetArrayItem";
			}
		}
	}
}
