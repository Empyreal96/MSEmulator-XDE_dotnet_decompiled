using System;
using System.IO;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x020000DE RID: 222
	public class CommandBreakpoint : Breakpoint
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00045D0A File Offset: 0x00043F0A
		internal CommandBreakpoint(string script, WildcardPattern command, string commandString, ScriptBlock action) : base(script, action)
		{
			this.CommandPattern = command;
			this.Command = commandString;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00045D23 File Offset: 0x00043F23
		internal CommandBreakpoint(string script, WildcardPattern command, string commandString, ScriptBlock action, int id) : base(script, action, id)
		{
			this.CommandPattern = command;
			this.Command = commandString;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00045D3E File Offset: 0x00043F3E
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00045D46 File Offset: 0x00043F46
		public string Command { get; private set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00045D4F File Offset: 0x00043F4F
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00045D57 File Offset: 0x00043F57
		internal WildcardPattern CommandPattern { get; private set; }

		// Token: 0x06000C90 RID: 3216 RVA: 0x00045D60 File Offset: 0x00043F60
		public override string ToString()
		{
			if (!base.IsScriptBreakpoint)
			{
				return StringUtil.Format(DebuggerStrings.CommandBreakpointString, this.Command);
			}
			return StringUtil.Format(DebuggerStrings.CommandScriptBreakpointString, base.Script, this.Command);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00045D91 File Offset: 0x00043F91
		internal override void RemoveSelf(ScriptDebugger debugger)
		{
			debugger.RemoveCommandBreakpoint(this);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00045D9C File Offset: 0x00043F9C
		private bool CommandInfoMatches(CommandInfo commandInfo)
		{
			if (commandInfo == null)
			{
				return false;
			}
			if (this.CommandPattern.IsMatch(commandInfo.Name))
			{
				return true;
			}
			if (!string.IsNullOrEmpty(commandInfo.ModuleName) && this.Command.IndexOf('\\') != -1 && this.CommandPattern.IsMatch(commandInfo.ModuleName + "\\" + commandInfo.Name))
			{
				return true;
			}
			ExternalScriptInfo externalScriptInfo = commandInfo as ExternalScriptInfo;
			if (externalScriptInfo != null)
			{
				if (externalScriptInfo.Path.Equals(this.Command, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (this.CommandPattern.IsMatch(Path.GetFileNameWithoutExtension(externalScriptInfo.Path)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00045E40 File Offset: 0x00044040
		internal bool Trigger(InvocationInfo invocationInfo)
		{
			return (this.CommandPattern.IsMatch(invocationInfo.InvocationName) || this.CommandInfoMatches(invocationInfo.MyCommand)) && (base.Script == null || base.Script.Equals(invocationInfo.ScriptName, StringComparison.OrdinalIgnoreCase));
		}
	}
}
