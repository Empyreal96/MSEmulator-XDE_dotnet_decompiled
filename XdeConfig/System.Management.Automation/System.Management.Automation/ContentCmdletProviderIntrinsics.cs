using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Provider;

namespace System.Management.Automation
{
	// Token: 0x02000019 RID: 25
	public sealed class ContentCmdletProviderIntrinsics
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00005E39 File Offset: 0x00004039
		private ContentCmdletProviderIntrinsics()
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005E41 File Offset: 0x00004041
		internal ContentCmdletProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.sessionState = cmdlet.Context.EngineSessionState;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005E6F File Offset: 0x0000406F
		internal ContentCmdletProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005E8C File Offset: 0x0000408C
		public Collection<IContentReader> GetReader(string path)
		{
			return this.sessionState.GetContentReader(new string[]
			{
				path
			}, false, false);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005EB2 File Offset: 0x000040B2
		public Collection<IContentReader> GetReader(string[] path, bool force, bool literalPath)
		{
			return this.sessionState.GetContentReader(path, force, literalPath);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005EC4 File Offset: 0x000040C4
		internal Collection<IContentReader> GetReader(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetContentReader(new string[]
			{
				path
			}, context);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005EE9 File Offset: 0x000040E9
		internal object GetContentReaderDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetContentReaderDynamicParameters(path, context);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005EF8 File Offset: 0x000040F8
		public Collection<IContentWriter> GetWriter(string path)
		{
			return this.sessionState.GetContentWriter(new string[]
			{
				path
			}, false, false);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005F1E File Offset: 0x0000411E
		public Collection<IContentWriter> GetWriter(string[] path, bool force, bool literalPath)
		{
			return this.sessionState.GetContentWriter(path, force, literalPath);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005F30 File Offset: 0x00004130
		internal Collection<IContentWriter> GetWriter(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetContentWriter(new string[]
			{
				path
			}, context);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005F55 File Offset: 0x00004155
		internal object GetContentWriterDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetContentWriterDynamicParameters(path, context);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005F64 File Offset: 0x00004164
		public void Clear(string path)
		{
			this.sessionState.ClearContent(new string[]
			{
				path
			}, false, false);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005F8A File Offset: 0x0000418A
		public void Clear(string[] path, bool force, bool literalPath)
		{
			this.sessionState.ClearContent(path, force, literalPath);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005F9C File Offset: 0x0000419C
		internal void Clear(string path, CmdletProviderContext context)
		{
			this.sessionState.ClearContent(new string[]
			{
				path
			}, context);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005FC1 File Offset: 0x000041C1
		internal object ClearContentDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.ClearContentDynamicParameters(path, context);
		}

		// Token: 0x0400005F RID: 95
		private Cmdlet cmdlet;

		// Token: 0x04000060 RID: 96
		private SessionStateInternal sessionState;
	}
}
