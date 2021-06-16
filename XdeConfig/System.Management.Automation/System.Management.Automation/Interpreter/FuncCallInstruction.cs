using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000677 RID: 1655
	internal sealed class FuncCallInstruction<TRet> : CallInstruction
	{
		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x00179631 File Offset: 0x00177831
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x0017963E File Offset: 0x0017783E
		public override int ArgumentCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00179641 File Offset: 0x00177841
		public FuncCallInstruction(Func<TRet> target)
		{
			this._target = target;
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x00179650 File Offset: 0x00177850
		public FuncCallInstruction(MethodInfo target)
		{
			this._target = (Func<TRet>)target.CreateDelegate(typeof(Func<TRet>));
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x00179673 File Offset: 0x00177873
		public override object Invoke()
		{
			return this._target();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x00179685 File Offset: 0x00177885
		public override int Run(InterpretedFrame frame)
		{
			frame.Data[frame.StackIndex] = this._target();
			frame.StackIndex -= -1;
			return 1;
		}

		// Token: 0x040022B2 RID: 8882
		private readonly Func<TRet> _target;
	}
}
