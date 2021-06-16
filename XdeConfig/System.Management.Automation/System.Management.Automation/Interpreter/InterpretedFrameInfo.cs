using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006EA RID: 1770
	[Serializable]
	internal struct InterpretedFrameInfo
	{
		// Token: 0x060048E0 RID: 18656 RVA: 0x0017FAE8 File Offset: 0x0017DCE8
		public InterpretedFrameInfo(string methodName, DebugInfo info)
		{
			this.MethodName = methodName;
			this.DebugInfo = info;
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x0017FAF8 File Offset: 0x0017DCF8
		public override string ToString()
		{
			return this.MethodName + ((this.DebugInfo != null) ? (": " + this.DebugInfo) : null);
		}

		// Token: 0x040023A3 RID: 9123
		public readonly string MethodName;

		// Token: 0x040023A4 RID: 9124
		public readonly DebugInfo DebugInfo;
	}
}
