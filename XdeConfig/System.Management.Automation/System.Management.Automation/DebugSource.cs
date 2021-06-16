using System;

namespace System.Management.Automation
{
	// Token: 0x020000E8 RID: 232
	public sealed class DebugSource
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00046503 File Offset: 0x00044703
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x0004650B File Offset: 0x0004470B
		public string Script { get; private set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00046514 File Offset: 0x00044714
		// (set) Token: 0x06000CCD RID: 3277 RVA: 0x0004651C File Offset: 0x0004471C
		public string ScriptFile { get; private set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00046525 File Offset: 0x00044725
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0004652D File Offset: 0x0004472D
		public string XamlDefinition { get; private set; }

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00046536 File Offset: 0x00044736
		public DebugSource(string script, string scriptFile, string xamlDefinition)
		{
			this.Script = script;
			this.ScriptFile = scriptFile;
			this.XamlDefinition = xamlDefinition;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00046553 File Offset: 0x00044753
		private DebugSource()
		{
		}
	}
}
