using System;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal.Host
{
	// Token: 0x0200020C RID: 524
	internal class InternalHostRawUserInterface : PSHostRawUserInterface
	{
		// Token: 0x06001851 RID: 6225 RVA: 0x00094DC8 File Offset: 0x00092FC8
		internal InternalHostRawUserInterface(PSHostRawUserInterface externalRawUI, InternalHost parentHost)
		{
			this.externalRawUI = externalRawUI;
			this.parentHost = parentHost;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00094DE0 File Offset: 0x00092FE0
		internal void ThrowNotInteractive()
		{
			string hostFunctionNotImplemented = HostInterfaceExceptionsStrings.HostFunctionNotImplemented;
			HostException ex = new HostException(hostFunctionNotImplemented, null, "HostFunctionNotImplemented", ErrorCategory.NotImplemented);
			throw ex;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001853 RID: 6227 RVA: 0x00094E04 File Offset: 0x00093004
		// (set) Token: 0x06001854 RID: 6228 RVA: 0x00094E2C File Offset: 0x0009302C
		public override ConsoleColor ForegroundColor
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.ForegroundColor;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.ForegroundColor = value;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x00094E48 File Offset: 0x00093048
		// (set) Token: 0x06001856 RID: 6230 RVA: 0x00094E70 File Offset: 0x00093070
		public override ConsoleColor BackgroundColor
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.BackgroundColor;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.BackgroundColor = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001857 RID: 6231 RVA: 0x00094E8C File Offset: 0x0009308C
		// (set) Token: 0x06001858 RID: 6232 RVA: 0x00094EB4 File Offset: 0x000930B4
		public override Coordinates CursorPosition
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.CursorPosition;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.CursorPosition = value;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001859 RID: 6233 RVA: 0x00094ED0 File Offset: 0x000930D0
		// (set) Token: 0x0600185A RID: 6234 RVA: 0x00094EF8 File Offset: 0x000930F8
		public override Coordinates WindowPosition
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.WindowPosition;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.WindowPosition = value;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x00094F14 File Offset: 0x00093114
		// (set) Token: 0x0600185C RID: 6236 RVA: 0x00094F3C File Offset: 0x0009313C
		public override int CursorSize
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.CursorSize;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.CursorSize = value;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x00094F58 File Offset: 0x00093158
		// (set) Token: 0x0600185E RID: 6238 RVA: 0x00094F80 File Offset: 0x00093180
		public override Size BufferSize
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.BufferSize;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.BufferSize = value;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x00094F9C File Offset: 0x0009319C
		// (set) Token: 0x06001860 RID: 6240 RVA: 0x00094FC4 File Offset: 0x000931C4
		public override Size WindowSize
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.WindowSize;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.WindowSize = value;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x00094FE0 File Offset: 0x000931E0
		public override Size MaxWindowSize
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.MaxWindowSize;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x00095008 File Offset: 0x00093208
		public override Size MaxPhysicalWindowSize
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.MaxPhysicalWindowSize;
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00095030 File Offset: 0x00093230
		public override KeyInfo ReadKey(ReadKeyOptions options)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			KeyInfo result = default(KeyInfo);
			try
			{
				result = this.externalRawUI.ReadKey(options);
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parentHost.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x000950A8 File Offset: 0x000932A8
		public override void FlushInputBuffer()
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			this.externalRawUI.FlushInputBuffer();
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x000950C4 File Offset: 0x000932C4
		public override bool KeyAvailable
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.KeyAvailable;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001866 RID: 6246 RVA: 0x000950EC File Offset: 0x000932EC
		// (set) Token: 0x06001867 RID: 6247 RVA: 0x00095114 File Offset: 0x00093314
		public override string WindowTitle
		{
			get
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				return this.externalRawUI.WindowTitle;
			}
			set
			{
				if (this.externalRawUI == null)
				{
					this.ThrowNotInteractive();
				}
				this.externalRawUI.WindowTitle = value;
			}
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00095130 File Offset: 0x00093330
		public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			this.externalRawUI.SetBufferContents(origin, contents);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0009514D File Offset: 0x0009334D
		public override void SetBufferContents(Rectangle r, BufferCell fill)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			this.externalRawUI.SetBufferContents(r, fill);
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0009516A File Offset: 0x0009336A
		public override BufferCell[,] GetBufferContents(Rectangle r)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			return this.externalRawUI.GetBufferContents(r);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00095186 File Offset: 0x00093386
		public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			this.externalRawUI.ScrollBufferContents(source, destination, clip, fill);
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000951A6 File Offset: 0x000933A6
		public override int LengthInBufferCells(string str)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			return this.externalRawUI.LengthInBufferCells(str);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000951C2 File Offset: 0x000933C2
		public override int LengthInBufferCells(string str, int offset)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			return this.externalRawUI.LengthInBufferCells(str, offset);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000951DF File Offset: 0x000933DF
		public override int LengthInBufferCells(char character)
		{
			if (this.externalRawUI == null)
			{
				this.ThrowNotInteractive();
			}
			return this.externalRawUI.LengthInBufferCells(character);
		}

		// Token: 0x04000A31 RID: 2609
		private PSHostRawUserInterface externalRawUI;

		// Token: 0x04000A32 RID: 2610
		private InternalHost parentHost;
	}
}
