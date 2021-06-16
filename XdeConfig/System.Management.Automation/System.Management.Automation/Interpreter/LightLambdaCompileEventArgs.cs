using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006ED RID: 1773
	internal sealed class LightLambdaCompileEventArgs : EventArgs
	{
		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x00182496 File Offset: 0x00180696
		// (set) Token: 0x06004941 RID: 18753 RVA: 0x0018249E File Offset: 0x0018069E
		public Delegate Compiled { get; private set; }

		// Token: 0x06004942 RID: 18754 RVA: 0x001824A7 File Offset: 0x001806A7
		internal LightLambdaCompileEventArgs(Delegate compiled)
		{
			this.Compiled = compiled;
		}
	}
}
