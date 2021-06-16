using System;

namespace System.Management.Automation
{
	// Token: 0x020000DC RID: 220
	public abstract class Breakpoint
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00045BD0 File Offset: 0x00043DD0
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x00045BD8 File Offset: 0x00043DD8
		public ScriptBlock Action { get; private set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00045BE1 File Offset: 0x00043DE1
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x00045BE9 File Offset: 0x00043DE9
		public bool Enabled { get; private set; }

		// Token: 0x06000C7E RID: 3198 RVA: 0x00045BF2 File Offset: 0x00043DF2
		internal void SetEnabled(bool value)
		{
			this.Enabled = value;
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00045BFB File Offset: 0x00043DFB
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00045C03 File Offset: 0x00043E03
		public int HitCount { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00045C0C File Offset: 0x00043E0C
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x00045C14 File Offset: 0x00043E14
		public int Id { get; private set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00045C1D File Offset: 0x00043E1D
		internal bool IsScriptBreakpoint
		{
			get
			{
				return this.Script != null;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00045C2B File Offset: 0x00043E2B
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x00045C33 File Offset: 0x00043E33
		public string Script { get; private set; }

		// Token: 0x06000C86 RID: 3206 RVA: 0x00045C3C File Offset: 0x00043E3C
		internal Breakpoint(string script, ScriptBlock action)
		{
			this.Enabled = true;
			this.Script = script;
			this.Id = Breakpoint._lastID++;
			this.Action = action;
			this.HitCount = 0;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00045C73 File Offset: 0x00043E73
		internal Breakpoint(string script, ScriptBlock action, int id)
		{
			this.Enabled = true;
			this.Script = script;
			this.Id = id;
			this.Action = action;
			this.HitCount = 0;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00045CA0 File Offset: 0x00043EA0
		internal Breakpoint.BreakpointAction Trigger()
		{
			this.HitCount++;
			if (this.Action == null)
			{
				return Breakpoint.BreakpointAction.Break;
			}
			try
			{
				this.Action.DoInvoke(this, null, new object[0]);
			}
			catch (BreakException)
			{
				return Breakpoint.BreakpointAction.Break;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return Breakpoint.BreakpointAction.Continue;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00045D08 File Offset: 0x00043F08
		internal virtual void RemoveSelf(ScriptDebugger debugger)
		{
		}

		// Token: 0x0400058A RID: 1418
		private static int _lastID;

		// Token: 0x020000DD RID: 221
		internal enum BreakpointAction
		{
			// Token: 0x04000591 RID: 1425
			Continue,
			// Token: 0x04000592 RID: 1426
			Break
		}
	}
}
