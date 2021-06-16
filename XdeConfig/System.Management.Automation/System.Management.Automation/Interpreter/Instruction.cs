using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000651 RID: 1617
	internal abstract class Instruction
	{
		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x001767BC File Offset: 0x001749BC
		public virtual int ConsumedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060045AD RID: 17837 RVA: 0x001767BF File Offset: 0x001749BF
		public virtual int ProducedStack
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x001767C2 File Offset: 0x001749C2
		public virtual int ConsumedContinuations
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x001767C5 File Offset: 0x001749C5
		public virtual int ProducedContinuations
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x001767C8 File Offset: 0x001749C8
		public int StackBalance
		{
			get
			{
				return this.ProducedStack - this.ConsumedStack;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060045B1 RID: 17841 RVA: 0x001767D7 File Offset: 0x001749D7
		public int ContinuationsBalance
		{
			get
			{
				return this.ProducedContinuations - this.ConsumedContinuations;
			}
		}

		// Token: 0x060045B2 RID: 17842
		public abstract int Run(InterpretedFrame frame);

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060045B3 RID: 17843 RVA: 0x001767E6 File Offset: 0x001749E6
		public virtual string InstructionName
		{
			get
			{
				return base.GetType().Name.Replace("Instruction", "");
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00176802 File Offset: 0x00174A02
		public override string ToString()
		{
			return this.InstructionName + "()";
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00176814 File Offset: 0x00174A14
		public virtual string ToDebugString(int instructionIndex, object cookie, Func<int, int> labelIndexer, IList<object> objects)
		{
			return this.ToString();
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x0017681C File Offset: 0x00174A1C
		public virtual object GetDebugCookie(LightCompiler compiler)
		{
			return null;
		}

		// Token: 0x04002285 RID: 8837
		public const int UnknownInstrIndex = 2147483647;
	}
}
