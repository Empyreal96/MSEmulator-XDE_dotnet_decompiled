using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200051A RID: 1306
	internal sealed class IndentationManager
	{
		// Token: 0x060036EE RID: 14062 RVA: 0x00128B8E File Offset: 0x00126D8E
		internal void Clear()
		{
			this._frameInfoStack.Clear();
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x00128B9C File Offset: 0x00126D9C
		internal IDisposable StackFrame(FrameInfo frameInfo)
		{
			IndentationManager.IndentationStackFrame result = new IndentationManager.IndentationStackFrame(this);
			this._frameInfoStack.Push(frameInfo);
			return result;
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x00128BBD File Offset: 0x00126DBD
		private void RemoveStackFrame()
		{
			this._frameInfoStack.Pop();
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x00128BCB File Offset: 0x00126DCB
		internal int RightIndentation
		{
			get
			{
				return this.ComputeRightIndentation();
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x00128BD3 File Offset: 0x00126DD3
		internal int LeftIndentation
		{
			get
			{
				return this.ComputeLeftIndentation();
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060036F3 RID: 14067 RVA: 0x00128BDB File Offset: 0x00126DDB
		internal int FirstLineIndentation
		{
			get
			{
				if (this._frameInfoStack.Count == 0)
				{
					return 0;
				}
				return this._frameInfoStack.Peek().firstLine;
			}
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00128BFC File Offset: 0x00126DFC
		private int ComputeRightIndentation()
		{
			int num = 0;
			foreach (FrameInfo frameInfo in this._frameInfoStack)
			{
				num += frameInfo.rightIndentation;
			}
			return num;
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00128C54 File Offset: 0x00126E54
		private int ComputeLeftIndentation()
		{
			int num = 0;
			foreach (FrameInfo frameInfo in this._frameInfoStack)
			{
				num += frameInfo.leftIndentation;
			}
			return num;
		}

		// Token: 0x04001C1E RID: 7198
		private Stack<FrameInfo> _frameInfoStack = new Stack<FrameInfo>();

		// Token: 0x0200051B RID: 1307
		private sealed class IndentationStackFrame : IDisposable
		{
			// Token: 0x060036F7 RID: 14071 RVA: 0x00128CBF File Offset: 0x00126EBF
			internal IndentationStackFrame(IndentationManager mgr)
			{
				this._mgr = mgr;
			}

			// Token: 0x060036F8 RID: 14072 RVA: 0x00128CCE File Offset: 0x00126ECE
			public void Dispose()
			{
				if (this._mgr != null)
				{
					this._mgr.RemoveStackFrame();
				}
			}

			// Token: 0x04001C1F RID: 7199
			private IndentationManager _mgr;
		}
	}
}
