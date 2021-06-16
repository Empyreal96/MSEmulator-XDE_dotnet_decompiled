using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x020000F6 RID: 246
	internal class DebuggerCommandProcessor
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x0004AE3C File Offset: 0x0004903C
		public DebuggerCommandProcessor()
		{
			this.commandTable = new Dictionary<string, DebuggerCommand>(StringComparer.OrdinalIgnoreCase);
			this.commandTable["stepInto"] = (this.commandTable["s"] = new DebuggerCommand("stepInto", new DebuggerResumeAction?(DebuggerResumeAction.StepInto), true, false));
			this.commandTable["stepOut"] = (this.commandTable["o"] = new DebuggerCommand("stepOut", new DebuggerResumeAction?(DebuggerResumeAction.StepOut), false, false));
			this.commandTable["stepOver"] = (this.commandTable["v"] = new DebuggerCommand("stepOver", new DebuggerResumeAction?(DebuggerResumeAction.StepOver), true, false));
			this.commandTable["continue"] = (this.commandTable["c"] = new DebuggerCommand("continue", new DebuggerResumeAction?(DebuggerResumeAction.Continue), false, false));
			this.commandTable["quit"] = (this.commandTable["q"] = new DebuggerCommand("quit", new DebuggerResumeAction?(DebuggerResumeAction.Stop), false, false));
			this.commandTable["k"] = new DebuggerCommand("get-pscallstack", null, false, false);
			this.commandTable["h"] = (this.commandTable["?"] = (this.helpCommand = new DebuggerCommand("h", null, false, true)));
			this.commandTable["list"] = (this.commandTable["l"] = (this.listCommand = new DebuggerCommand("list", null, true, true)));
			this.commandTable[string.Empty] = new DebuggerCommand(string.Empty, null, false, true);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0004B042 File Offset: 0x00049242
		public void Reset()
		{
			this.lines = null;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0004B04C File Offset: 0x0004924C
		public DebuggerCommand ProcessCommand(PSHost host, string command, InvocationInfo invocationInfo)
		{
			return this.lastCommand = this.DoProcessCommand(host, command, invocationInfo, null);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0004B06C File Offset: 0x0004926C
		public DebuggerCommand ProcessCommand(PSHost host, string command, InvocationInfo invocationInfo, IList<PSObject> output)
		{
			DebuggerCommand debuggerCommand = this.DoProcessCommand(host, command, invocationInfo, output);
			if (debuggerCommand.ExecutedByDebugger || debuggerCommand.ResumeAction != null)
			{
				this.lastCommand = debuggerCommand;
			}
			return debuggerCommand;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0004B0A5 File Offset: 0x000492A5
		public void ProcessListCommand(InvocationInfo invocationInfo, IList<PSObject> output)
		{
			this.DoProcessCommand(null, "list", invocationInfo, output);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0004B0B8 File Offset: 0x000492B8
		public DebuggerCommand ProcessBasicCommand(string command)
		{
			if (command.Length == 0 && this.lastCommand != null && this.lastCommand.RepeatOnEnter)
			{
				return this.lastCommand;
			}
			DebuggerCommand debuggerCommand;
			if (this.commandTable.TryGetValue(command, out debuggerCommand))
			{
				if (debuggerCommand.ExecutedByDebugger || debuggerCommand.ResumeAction != null)
				{
					this.lastCommand = debuggerCommand;
				}
				return debuggerCommand;
			}
			return null;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0004B11C File Offset: 0x0004931C
		private DebuggerCommand DoProcessCommand(PSHost host, string command, InvocationInfo invocationInfo, IList<PSObject> output)
		{
			if (command.Length == 0 && this.lastCommand != null && this.lastCommand.RepeatOnEnter)
			{
				if (this.lastCommand == this.listCommand)
				{
					if (this.lines != null && this.lastLineDisplayed < this.lines.Length)
					{
						this.DisplayScript(host, output, invocationInfo, this.lastLineDisplayed + 1, 16);
					}
					return this.listCommand;
				}
				command = this.lastCommand.Command;
			}
			Regex regex = new Regex("^l(ist)?(\\s+(?<start>\\S+))?(\\s+(?<count>\\S+))?$", RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success)
			{
				this.DisplayScript(host, output, invocationInfo, match);
				return this.listCommand;
			}
			DebuggerCommand debuggerCommand = null;
			if (this.commandTable.TryGetValue(command, out debuggerCommand))
			{
				if (debuggerCommand == this.helpCommand)
				{
					this.DisplayHelp(host, output);
				}
				return debuggerCommand;
			}
			return new DebuggerCommand(command, null, false, false);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0004B1FC File Offset: 0x000493FC
		private void DisplayHelp(PSHost host, IList<PSObject> output)
		{
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.StepHelp, "s", "stepInto"), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.StepOverHelp, "v", "stepOver"), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.StepOutHelp, "o", "stepOut"), host, output);
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.ContinueHelp, "c", "continue"), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.StopHelp, "q", "quit"), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.DetachHelp, "d", "detach"), host, output);
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.GetStackTraceHelp, "k"), host, output);
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.ListHelp, "l", "list"), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.AdditionalListHelp1, new object[0]), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.AdditionalListHelp2, new object[0]), host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.AdditionalListHelp3, new object[0]), host, output);
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.EnterHelp, new object[]
			{
				"stepInto",
				"stepOver",
				"list"
			}), host, output);
			this.WriteLine("", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.HelpCommandHelp, "?", "h"), host, output);
			this.WriteLine("\n", host, output);
			this.WriteLine(StringUtil.Format(DebuggerStrings.PromptHelp, new object[0]), host, output);
			this.WriteLine("", host, output);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0004B3FC File Offset: 0x000495FC
		private void DisplayScript(PSHost host, IList<PSObject> output, InvocationInfo invocationInfo, Match match)
		{
			if (invocationInfo == null)
			{
				return;
			}
			if (this.lines == null)
			{
				string fullScript = invocationInfo.GetFullScript();
				if (string.IsNullOrEmpty(fullScript))
				{
					this.WriteErrorLine(StringUtil.Format(DebuggerStrings.NoSourceCode, new object[0]), host, output);
					return;
				}
				this.lines = fullScript.Split(new string[]
				{
					"\r\n",
					"\r",
					"\n"
				}, StringSplitOptions.None);
			}
			int num = Math.Max(invocationInfo.ScriptLineNumber - 5, 1);
			if (match.Groups["start"].Value.Length <= 0)
			{
				goto IL_107;
			}
			try
			{
				num = int.Parse(match.Groups["start"].Value, CultureInfo.CurrentCulture.NumberFormat);
			}
			catch
			{
				this.WriteErrorLine(StringUtil.Format(DebuggerStrings.BadStartFormat, this.lines.Length), host, output);
				return;
			}
			if (num > 0 && num <= this.lines.Length)
			{
				goto IL_107;
			}
			this.WriteErrorLine(StringUtil.Format(DebuggerStrings.BadStartFormat, this.lines.Length), host, output);
			return;
			IL_107:
			int num2 = 16;
			if (match.Groups["count"].Value.Length > 0)
			{
				try
				{
					num2 = int.Parse(match.Groups["count"].Value, CultureInfo.CurrentCulture.NumberFormat);
				}
				catch
				{
					this.WriteErrorLine(StringUtil.Format(DebuggerStrings.BadCountFormat, this.lines.Length), host, output);
					return;
				}
				num2 = ((num2 > this.lines.Length) ? this.lines.Length : num2);
				if (num2 <= 0)
				{
					this.WriteErrorLine(DebuggerStrings.BadCountFormat, host, output);
					return;
				}
			}
			this.DisplayScript(host, output, invocationInfo, num, num2);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0004B5CC File Offset: 0x000497CC
		private void DisplayScript(PSHost host, IList<PSObject> output, InvocationInfo invocationInfo, int start, int count)
		{
			this.WriteCR(host, output);
			int num = start;
			while (num <= this.lines.Length && num < start + count)
			{
				this.WriteLine((num == invocationInfo.ScriptLineNumber) ? string.Format(CultureInfo.CurrentCulture, "{0,5}:* {1}", new object[]
				{
					num,
					this.lines[num - 1]
				}) : string.Format(CultureInfo.CurrentCulture, "{0,5}:  {1}", new object[]
				{
					num,
					this.lines[num - 1]
				}), host, output);
				this.lastLineDisplayed = num;
				num++;
			}
			this.WriteCR(host, output);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0004B67C File Offset: 0x0004987C
		private void WriteLine(string line, PSHost host, IList<PSObject> output)
		{
			if (host != null)
			{
				host.UI.WriteLine(line);
			}
			if (output != null)
			{
				output.Add(new PSObject(line));
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0004B69C File Offset: 0x0004989C
		private void WriteCR(PSHost host, IList<PSObject> output)
		{
			if (host != null)
			{
				host.UI.WriteLine();
			}
			if (output != null)
			{
				output.Add(new PSObject("\r\n"));
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0004B6BF File Offset: 0x000498BF
		private void WriteErrorLine(string error, PSHost host, IList<PSObject> output)
		{
			if (host != null)
			{
				host.UI.WriteErrorLine(error);
			}
			if (output != null)
			{
				output.Add(new PSObject(new ErrorRecord(new RuntimeException(error), "DebuggerError", ErrorCategory.InvalidOperation, null)));
			}
		}

		// Token: 0x0400060D RID: 1549
		private const string ContinueCommand = "continue";

		// Token: 0x0400060E RID: 1550
		private const string ContinueShortcut = "c";

		// Token: 0x0400060F RID: 1551
		private const string GetStackTraceShortcut = "k";

		// Token: 0x04000610 RID: 1552
		private const string HelpCommand = "h";

		// Token: 0x04000611 RID: 1553
		private const string HelpShortcut = "?";

		// Token: 0x04000612 RID: 1554
		private const string ListCommand = "list";

		// Token: 0x04000613 RID: 1555
		private const string ListShortcut = "l";

		// Token: 0x04000614 RID: 1556
		private const string StepCommand = "stepInto";

		// Token: 0x04000615 RID: 1557
		private const string StepShortcut = "s";

		// Token: 0x04000616 RID: 1558
		private const string StepOutCommand = "stepOut";

		// Token: 0x04000617 RID: 1559
		private const string StepOutShortcut = "o";

		// Token: 0x04000618 RID: 1560
		private const string StepOverCommand = "stepOver";

		// Token: 0x04000619 RID: 1561
		private const string StepOverShortcut = "v";

		// Token: 0x0400061A RID: 1562
		private const string StopCommand = "quit";

		// Token: 0x0400061B RID: 1563
		private const string StopShortcut = "q";

		// Token: 0x0400061C RID: 1564
		private const string DetachCommand = "detach";

		// Token: 0x0400061D RID: 1565
		private const string DetachShortcut = "d";

		// Token: 0x0400061E RID: 1566
		private const int DefaultListLineCount = 16;

		// Token: 0x0400061F RID: 1567
		private const string Crlf = "\r\n";

		// Token: 0x04000620 RID: 1568
		private Dictionary<string, DebuggerCommand> commandTable;

		// Token: 0x04000621 RID: 1569
		private DebuggerCommand helpCommand;

		// Token: 0x04000622 RID: 1570
		private DebuggerCommand listCommand;

		// Token: 0x04000623 RID: 1571
		private DebuggerCommand lastCommand;

		// Token: 0x04000624 RID: 1572
		private string[] lines;

		// Token: 0x04000625 RID: 1573
		private int lastLineDisplayed;
	}
}
