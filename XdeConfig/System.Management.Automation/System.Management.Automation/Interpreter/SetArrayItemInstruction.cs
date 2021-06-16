using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000668 RID: 1640
	internal sealed class SetArrayItemInstruction<TElement> : Instruction
	{
		// Token: 0x060045F3 RID: 17907 RVA: 0x00177207 File Offset: 0x00175407
		internal SetArrayItemInstruction()
		{
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x0017720F File Offset: 0x0017540F
		public override int ConsumedStack
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x00177212 File Offset: 0x00175412
		public override int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00177218 File Offset: 0x00175418
		public override int Run(InterpretedFrame frame)
		{
			TElement telement = (TElement)((object)frame.Pop());
			int num = (int)frame.Pop();
			TElement[] array = (TElement[])frame.Pop();
			array[num] = telement;
			return 1;
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x00177252 File Offset: 0x00175452
		public override string InstructionName
		{
			get
			{
				return "SetArrayItem";
			}
		}
	}
}
