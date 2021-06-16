using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020000F4 RID: 244
	internal sealed class EmbeddedRunspaceDebugger : NestedRunspaceDebugger
	{
		// Token: 0x06000DAB RID: 3499 RVA: 0x0004A8DA File Offset: 0x00048ADA
		public EmbeddedRunspaceDebugger(Runspace runspace, PowerShell command, Debugger rootDebugger, PSMonitorRunspaceType runspaceType, Guid parentDebuggerId) : base(runspace, runspaceType, parentDebuggerId)
		{
			if (rootDebugger == null)
			{
				throw new PSArgumentNullException("rootDebugger");
			}
			this._command = command;
			this._rootDebugger = rootDebugger;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0004A904 File Offset: 0x00048B04
		protected override void HandleDebuggerStop(object sender, DebuggerStopEventArgs e)
		{
			this._sendDebuggerArgs = new DebuggerStopEventArgs(e.InvocationInfo, new Collection<Breakpoint>(e.Breakpoints), e.ResumeAction);
			object runningCmd = null;
			try
			{
				runningCmd = this.DrainAndBlockRemoteOutput();
				base.RaiseDebuggerStopEvent(this._sendDebuggerArgs);
			}
			finally
			{
				this.RestoreRemoteOutput(runningCmd);
				e.ResumeAction = this._sendDebuggerArgs.ResumeAction;
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0004A974 File Offset: 0x00048B74
		protected override DebuggerCommandResults HandleCallStack(PSDataCollection<PSObject> output)
		{
			PSCommand pscommand = new PSCommand();
			pscommand.AddCommand("Get-PSCallStack");
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			this._wrappedDebugger.ProcessCommand(pscommand, psdataCollection);
			PSDataCollection<CallStackFrame> psdataCollection2 = this._rootDebugger.GetCallStack().ToArray<CallStackFrame>();
			foreach (CallStackFrame obj in psdataCollection2)
			{
				psdataCollection.Add(new PSObject(obj));
			}
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("Out-String").AddParameter("Stream", true);
				powerShell.Invoke<PSObject>(psdataCollection, output);
			}
			return new DebuggerCommandResults(null, true);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0004AA5C File Offset: 0x00048C5C
		protected override bool HandleListCommand(PSDataCollection<PSObject> output)
		{
			return this._sendDebuggerArgs != null && this._sendDebuggerArgs.InvocationInfo != null && this._rootDebugger.InternalProcessListCommand(this._sendDebuggerArgs.InvocationInfo.ScriptLineNumber, output);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0004AA94 File Offset: 0x00048C94
		internal override InvocationInfo FixupInvocationInfo(InvocationInfo debugStopInvocationInfo)
		{
			if (debugStopInvocationInfo == null)
			{
				return null;
			}
			int num = debugStopInvocationInfo.ScriptLineNumber;
			CallStackFrame parentStackFrame = null;
			CallStackFrame[] activeDebuggerCallStack = this._rootDebugger.GetActiveDebuggerCallStack();
			if (activeDebuggerCallStack != null && activeDebuggerCallStack.Length > 0)
			{
				parentStackFrame = activeDebuggerCallStack[0];
			}
			else
			{
				CallStackFrame[] array = this._rootDebugger.GetCallStack().ToArray<CallStackFrame>();
				if (array != null && array.Length > 0)
				{
					parentStackFrame = array[0];
					num--;
				}
			}
			InvocationInfo invocationInfo = this.CreateInvocationInfoFromParent(parentStackFrame, num, debugStopInvocationInfo.ScriptPosition.StartColumnNumber, debugStopInvocationInfo.ScriptPosition.EndColumnNumber);
			return invocationInfo ?? debugStopInvocationInfo;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0004AB13 File Offset: 0x00048D13
		public override void Dispose()
		{
			base.Dispose();
			this._rootDebugger = null;
			this._parentScriptBlockAst = null;
			this._command = null;
			this._sendDebuggerArgs = null;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0004ABA4 File Offset: 0x00048DA4
		private InvocationInfo CreateInvocationInfoFromParent(CallStackFrame parentStackFrame, int debugLineNumber, int debugStartColNumber, int debugEndColNumber)
		{
			if (parentStackFrame == null)
			{
				return null;
			}
			if (this._parentScriptBlockAst == null && !string.IsNullOrEmpty(parentStackFrame.ScriptName) && File.Exists(parentStackFrame.ScriptName))
			{
				Token[] array;
				ParseError[] array2;
				this._parentScriptBlockAst = Parser.ParseInput(File.ReadAllText(parentStackFrame.ScriptName), out array, out array2);
			}
			if (this._parentScriptBlockAst != null)
			{
				int callingLineNumber = parentStackFrame.ScriptLineNumber;
				StatementAst statementAst = null;
				StatementAst statementAst2 = this._parentScriptBlockAst.Find((Ast ast) => ast is StatementAst && ast.Extent.StartLineNumber == callingLineNumber, true) as StatementAst;
				if (statementAst2 != null)
				{
					StatementAst statementAst3 = statementAst2.Find((Ast ast) => ast is StatementAst && ast.Extent.StartLineNumber > callingLineNumber, true) as StatementAst;
					if (statementAst3 != null)
					{
						int adjustedLineNumber = statementAst3.Extent.StartLineNumber + debugLineNumber - 1;
						statementAst = (statementAst2.Find((Ast ast) => ast is StatementAst && ast.Extent.StartLineNumber == adjustedLineNumber, true) as StatementAst);
					}
				}
				if (statementAst != null)
				{
					int offsetInLine = debugStartColNumber + (debugEndColNumber - debugStartColNumber) - 2;
					string line = this.FixUpStatementExtent(statementAst.Extent.StartColumnNumber - 1, statementAst.Extent.Text);
					ScriptPosition startPosition = new ScriptPosition(parentStackFrame.ScriptName, statementAst.Extent.StartLineNumber, debugStartColNumber, line);
					ScriptPosition endPosition = new ScriptPosition(parentStackFrame.ScriptName, statementAst.Extent.EndLineNumber, offsetInLine, line);
					return InvocationInfo.Create(parentStackFrame.InvocationInfo.MyCommand, new ScriptExtent(startPosition, endPosition));
				}
			}
			return null;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0004AD20 File Offset: 0x00048F20
		private string FixUpStatementExtent(int startColNum, string stateExtentText)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(' ', startColNum);
			stringBuilder.Append(stateExtentText);
			return stringBuilder.ToString();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0004AD4C File Offset: 0x00048F4C
		private object DrainAndBlockRemoteOutput()
		{
			if (!(this._runspace is RemoteRunspace))
			{
				return null;
			}
			try
			{
				if (this._command != null)
				{
					this._command.WaitForServicingComplete();
					this._command.SuspendIncomingData();
					return this._command;
				}
				Pipeline currentlyRunningPipeline = this._runspace.GetCurrentlyRunningPipeline();
				if (currentlyRunningPipeline != null)
				{
					currentlyRunningPipeline.DrainIncomingData();
					currentlyRunningPipeline.SuspendIncomingData();
					return currentlyRunningPipeline;
				}
			}
			catch (PSNotSupportedException)
			{
			}
			return null;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0004ADC8 File Offset: 0x00048FC8
		private void RestoreRemoteOutput(object runningCmd)
		{
			if (runningCmd == null)
			{
				return;
			}
			PowerShell powerShell = runningCmd as PowerShell;
			if (powerShell != null)
			{
				powerShell.ResumeIncomingData();
				return;
			}
			Pipeline pipeline = runningCmd as Pipeline;
			if (pipeline != null)
			{
				pipeline.ResumeIncomingData();
			}
		}

		// Token: 0x04000607 RID: 1543
		private PowerShell _command;

		// Token: 0x04000608 RID: 1544
		private Debugger _rootDebugger;

		// Token: 0x04000609 RID: 1545
		private ScriptBlockAst _parentScriptBlockAst;

		// Token: 0x0400060A RID: 1546
		private DebuggerStopEventArgs _sendDebuggerArgs;
	}
}
