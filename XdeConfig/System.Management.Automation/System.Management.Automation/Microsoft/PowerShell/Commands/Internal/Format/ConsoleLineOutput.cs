using System;
using System.Collections.Specialized;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200052E RID: 1326
	internal sealed class ConsoleLineOutput : LineOutput
	{
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x00129DFC File Offset: 0x00127FFC
		internal override int ColumnNumber
		{
			get
			{
				base.CheckStopProcessing();
				PSHostRawUserInterface rawUI = this.console.RawUI;
				try
				{
					return this.forceNewLine ? (rawUI.BufferSize.Width - 1) : rawUI.BufferSize.Width;
				}
				catch (HostException)
				{
				}
				if (!this.forceNewLine)
				{
					return this.fallbackRawConsoleColumnNumber;
				}
				return this.fallbackRawConsoleColumnNumber - 1;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x00129E74 File Offset: 0x00128074
		internal override int RowNumber
		{
			get
			{
				base.CheckStopProcessing();
				PSHostRawUserInterface rawUI = this.console.RawUI;
				try
				{
					return rawUI.WindowSize.Height;
				}
				catch (HostException)
				{
				}
				return this.fallbackRawConsoleRowNumber;
			}
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x00129EC0 File Offset: 0x001280C0
		internal override void WriteLine(string s)
		{
			base.CheckStopProcessing();
			this.writeLineHelper.WriteLine(s, this.ColumnNumber);
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x00129EDA File Offset: 0x001280DA
		internal override DisplayCells DisplayCells
		{
			get
			{
				base.CheckStopProcessing();
				if (this._displayCellsPSHost != null)
				{
					return this._displayCellsPSHost;
				}
				return this._displayCellsPSHost;
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x00129EF8 File Offset: 0x001280F8
		internal ConsoleLineOutput(PSHostUserInterface hostConsole, bool paging, TerminatingErrorContext errorContext)
		{
			if (hostConsole == null)
			{
				throw PSTraceSource.NewArgumentNullException("hostConsole");
			}
			if (errorContext == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorContext");
			}
			this.console = hostConsole;
			this.errorContext = errorContext;
			if (paging)
			{
				ConsoleLineOutput.tracer.WriteLine("paging is needed", new object[0]);
				string s = StringUtil.Format(FormatAndOut_out_xxx.ConsoleLineOutput_PagingPrompt, new object[0]);
				this.prompt = new ConsoleLineOutput.PromptHandler(s, this);
			}
			PSHostRawUserInterface rawUI = this.console.RawUI;
			if (rawUI != null)
			{
				ConsoleLineOutput.tracer.WriteLine("there is a valid raw interface", new object[0]);
				this._displayCellsPSHost = new DisplayCellsPSHost(rawUI);
			}
			WriteLineHelper.WriteCallback wlc = new WriteLineHelper.WriteCallback(this.OnWriteLine);
			WriteLineHelper.WriteCallback wc = new WriteLineHelper.WriteCallback(this.OnWrite);
			if (this.forceNewLine)
			{
				this.writeLineHelper = new WriteLineHelper(false, wlc, null, this.DisplayCells);
				return;
			}
			this.writeLineHelper = new WriteLineHelper(false, wlc, wc, this.DisplayCells);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x0012A000 File Offset: 0x00128200
		private void OnWriteLine(string s)
		{
			this.console.TranscribeResult(s);
			switch (base.WriteStream)
			{
			case WriteStreamType.Error:
				this.console.WriteErrorLine(s);
				break;
			case WriteStreamType.Warning:
				this.console.WriteWarningLine(s);
				break;
			case WriteStreamType.Verbose:
				this.console.WriteVerboseLine(s);
				break;
			case WriteStreamType.Debug:
				this.console.WriteDebugLine(s);
				break;
			default:
				if (!this.console.TranscribeOnly)
				{
					this.console.WriteLine(s);
				}
				break;
			}
			this.LineWrittenEvent();
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x0012A094 File Offset: 0x00128294
		private void OnWrite(string s)
		{
			switch (base.WriteStream)
			{
			case WriteStreamType.Error:
				this.console.WriteErrorLine(s);
				break;
			case WriteStreamType.Warning:
				this.console.WriteWarningLine(s);
				break;
			case WriteStreamType.Verbose:
				this.console.WriteVerboseLine(s);
				break;
			case WriteStreamType.Debug:
				this.console.WriteDebugLine(s);
				break;
			default:
				this.console.Write(s);
				break;
			}
			this.LineWrittenEvent();
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x0012A10C File Offset: 0x0012830C
		private void LineWrittenEvent()
		{
			if (this.disableLineWrittenEvent)
			{
				return;
			}
			if (this.prompt == null)
			{
				return;
			}
			this.linesWritten += 1L;
			if (!this.NeedToPrompt)
			{
				return;
			}
			this.disableLineWrittenEvent = true;
			ConsoleLineOutput.PromptHandler.PromptResponse promptResponse = this.prompt.PromptUser(this.console);
			this.disableLineWrittenEvent = false;
			switch (promptResponse)
			{
			case ConsoleLineOutput.PromptHandler.PromptResponse.NextPage:
				this.linesWritten = 0L;
				return;
			case ConsoleLineOutput.PromptHandler.PromptResponse.NextLine:
				this.linesWritten -= 1L;
				return;
			case ConsoleLineOutput.PromptHandler.PromptResponse.Quit:
				throw new HaltCommandException();
			default:
				return;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x0012A198 File Offset: 0x00128398
		private bool NeedToPrompt
		{
			get
			{
				int rowNumber = this.RowNumber;
				if (rowNumber <= 0)
				{
					return false;
				}
				int num = this.prompt.ComputePromptLines(this.DisplayCells, this.ColumnNumber);
				int num2 = this.RowNumber - num;
				if (num2 <= 0)
				{
					ConsoleLineOutput.tracer.WriteLine("No available Lines; suppress prompting", new object[0]);
					return false;
				}
				return this.linesWritten >= (long)num2;
			}
		}

		// Token: 0x04001C50 RID: 7248
		[TraceSource("ConsoleLineOutput", "ConsoleLineOutput")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("ConsoleLineOutput", "ConsoleLineOutput");

		// Token: 0x04001C51 RID: 7249
		private bool forceNewLine = true;

		// Token: 0x04001C52 RID: 7250
		private int fallbackRawConsoleColumnNumber = 80;

		// Token: 0x04001C53 RID: 7251
		private int fallbackRawConsoleRowNumber = 40;

		// Token: 0x04001C54 RID: 7252
		private WriteLineHelper writeLineHelper;

		// Token: 0x04001C55 RID: 7253
		private ConsoleLineOutput.PromptHandler prompt;

		// Token: 0x04001C56 RID: 7254
		private long linesWritten;

		// Token: 0x04001C57 RID: 7255
		private bool disableLineWrittenEvent;

		// Token: 0x04001C58 RID: 7256
		private PSHostUserInterface console;

		// Token: 0x04001C59 RID: 7257
		private DisplayCells _displayCellsPSHost;

		// Token: 0x04001C5A RID: 7258
		private TerminatingErrorContext errorContext;

		// Token: 0x0200052F RID: 1327
		private class PromptHandler
		{
			// Token: 0x0600375D RID: 14173 RVA: 0x0012A211 File Offset: 0x00128411
			internal PromptHandler(string s, ConsoleLineOutput cmdlet)
			{
				if (string.IsNullOrEmpty(s))
				{
					throw PSTraceSource.NewArgumentNullException("s");
				}
				this.promptString = s;
				this.callingCmdlet = cmdlet;
			}

			// Token: 0x0600375E RID: 14174 RVA: 0x0012A23A File Offset: 0x0012843A
			internal int ComputePromptLines(DisplayCells displayCells, int cols)
			{
				this.actualPrompt = StringManipulationHelper.GenerateLines(displayCells, this.promptString, cols, cols);
				return this.actualPrompt.Count;
			}

			// Token: 0x0600375F RID: 14175 RVA: 0x0012A25C File Offset: 0x0012845C
			internal ConsoleLineOutput.PromptHandler.PromptResponse PromptUser(PSHostUserInterface console)
			{
				for (int i = 0; i < this.actualPrompt.Count; i++)
				{
					if (i < this.actualPrompt.Count - 1)
					{
						console.WriteLine(this.actualPrompt[i]);
					}
					else
					{
						console.Write(this.actualPrompt[i]);
					}
				}
				for (;;)
				{
					this.callingCmdlet.CheckStopProcessing();
					char character = console.RawUI.ReadKey(ReadKeyOptions.NoEcho | ReadKeyOptions.IncludeKeyUp).Character;
					if (character == 'q' || character == 'Q')
					{
						break;
					}
					if (character == ' ')
					{
						goto Block_3;
					}
					if (character == '\r')
					{
						goto Block_4;
					}
				}
				console.WriteLine();
				return ConsoleLineOutput.PromptHandler.PromptResponse.Quit;
				Block_3:
				console.WriteLine();
				return ConsoleLineOutput.PromptHandler.PromptResponse.NextPage;
				Block_4:
				console.WriteLine();
				return ConsoleLineOutput.PromptHandler.PromptResponse.NextLine;
			}

			// Token: 0x04001C5B RID: 7259
			private StringCollection actualPrompt;

			// Token: 0x04001C5C RID: 7260
			private string promptString;

			// Token: 0x04001C5D RID: 7261
			private ConsoleLineOutput callingCmdlet;

			// Token: 0x02000530 RID: 1328
			internal enum PromptResponse
			{
				// Token: 0x04001C5F RID: 7263
				NextPage,
				// Token: 0x04001C60 RID: 7264
				NextLine,
				// Token: 0x04001C61 RID: 7265
				Quit
			}
		}
	}
}
