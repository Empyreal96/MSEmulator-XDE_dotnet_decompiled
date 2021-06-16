using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066D RID: 1645
	internal sealed class ActionCallInstruction : CallInstruction
	{
		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06004645 RID: 17989 RVA: 0x001789D7 File Offset: 0x00176BD7
		public override MethodInfo Info
		{
			get
			{
				return this._target.GetMethodInfo();
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x001789E4 File Offset: 0x00176BE4
		public override int ArgumentCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x001789E7 File Offset: 0x00176BE7
		public ActionCallInstruction(Action target)
		{
			this._target = target;
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x001789F6 File Offset: 0x00176BF6
		public ActionCallInstruction(MethodInfo target)
		{
			this._target = (Action)target.CreateDelegate(typeof(Action));
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00178A19 File Offset: 0x00176C19
		public override object Invoke()
		{
			this._target();
			return null;
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00178A27 File Offset: 0x00176C27
		public override int Run(InterpretedFrame frame)
		{
			this._target();
			frame.StackIndex = frame.StackIndex;
			return 1;
		}

		// Token: 0x040022A8 RID: 8872
		private readonly Action _target;
	}
}
