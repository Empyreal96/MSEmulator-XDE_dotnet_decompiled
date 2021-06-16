using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x020000E0 RID: 224
	public class VariableBreakpoint : Breakpoint
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00045E8C File Offset: 0x0004408C
		internal VariableBreakpoint(string script, string variable, VariableAccessMode accessMode, ScriptBlock action) : base(script, action)
		{
			this.Variable = variable;
			this.AccessMode = accessMode;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00045EA5 File Offset: 0x000440A5
		internal VariableBreakpoint(string script, string variable, VariableAccessMode accessMode, ScriptBlock action, int id) : base(script, action, id)
		{
			this.Variable = variable;
			this.AccessMode = accessMode;
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00045EC0 File Offset: 0x000440C0
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x00045EC8 File Offset: 0x000440C8
		public VariableAccessMode AccessMode { get; private set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00045ED1 File Offset: 0x000440D1
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00045ED9 File Offset: 0x000440D9
		public string Variable { get; private set; }

		// Token: 0x06000C9A RID: 3226 RVA: 0x00045EE4 File Offset: 0x000440E4
		public override string ToString()
		{
			if (!base.IsScriptBreakpoint)
			{
				return StringUtil.Format(DebuggerStrings.VariableBreakpointString, this.Variable, this.AccessMode);
			}
			return StringUtil.Format(DebuggerStrings.VariableScriptBreakpointString, new object[]
			{
				base.Script,
				this.Variable,
				this.AccessMode
			});
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00045F48 File Offset: 0x00044148
		internal bool Trigger(string currentScriptFile, bool read)
		{
			return base.Enabled && (this.AccessMode == VariableAccessMode.ReadWrite || this.AccessMode == (read ? VariableAccessMode.Read : VariableAccessMode.Write)) && (base.Script == null || base.Script.Equals(currentScriptFile, StringComparison.OrdinalIgnoreCase)) && base.Trigger() == Breakpoint.BreakpointAction.Break;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00045F9B File Offset: 0x0004419B
		internal override void RemoveSelf(ScriptDebugger debugger)
		{
			debugger.RemoveVariableBreakpoint(this);
		}
	}
}
