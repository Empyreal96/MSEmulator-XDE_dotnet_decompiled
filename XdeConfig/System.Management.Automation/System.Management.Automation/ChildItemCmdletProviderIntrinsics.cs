using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000003 RID: 3
	public sealed class ChildItemCmdletProviderIntrinsics
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		private ChildItemCmdletProviderIntrinsics()
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E0 File Offset: 0x000002E0
		internal ChildItemCmdletProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.sessionState = cmdlet.Context.EngineSessionState;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000210E File Offset: 0x0000030E
		internal ChildItemCmdletProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000212C File Offset: 0x0000032C
		public Collection<PSObject> Get(string path, bool recurse)
		{
			return this.sessionState.GetChildItems(new string[]
			{
				path
			}, recurse, uint.MaxValue, false, false);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002154 File Offset: 0x00000354
		public Collection<PSObject> Get(string[] path, bool recurse, uint depth, bool force, bool literalPath)
		{
			return this.sessionState.GetChildItems(path, recurse, depth, force, literalPath);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002168 File Offset: 0x00000368
		public Collection<PSObject> Get(string[] path, bool recurse, bool force, bool literalPath)
		{
			return this.Get(path, recurse, uint.MaxValue, force, literalPath);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002176 File Offset: 0x00000376
		internal void Get(string path, bool recurse, uint depth, CmdletProviderContext context)
		{
			this.sessionState.GetChildItems(path, recurse, depth, context);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002188 File Offset: 0x00000388
		internal object GetChildItemsDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			return this.sessionState.GetChildItemsDynamicParameters(path, recurse, context);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002198 File Offset: 0x00000398
		public Collection<string> GetNames(string path, ReturnContainers returnContainers, bool recurse)
		{
			return this.sessionState.GetChildNames(new string[]
			{
				path
			}, returnContainers, recurse, uint.MaxValue, false, false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021C1 File Offset: 0x000003C1
		public Collection<string> GetNames(string[] path, ReturnContainers returnContainers, bool recurse, bool force, bool literalPath)
		{
			return this.sessionState.GetChildNames(path, returnContainers, recurse, uint.MaxValue, force, literalPath);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021D6 File Offset: 0x000003D6
		public Collection<string> GetNames(string[] path, ReturnContainers returnContainers, bool recurse, uint depth, bool force, bool literalPath)
		{
			return this.sessionState.GetChildNames(path, returnContainers, recurse, depth, force, literalPath);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021EC File Offset: 0x000003EC
		internal void GetNames(string path, ReturnContainers returnContainers, bool recurse, uint depth, CmdletProviderContext context)
		{
			this.sessionState.GetChildNames(path, returnContainers, recurse, depth, context);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002200 File Offset: 0x00000400
		internal object GetChildNamesDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetChildNamesDynamicParameters(path, context);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000220F File Offset: 0x0000040F
		public bool HasChild(string path)
		{
			return this.sessionState.HasChildItems(path, false, false);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000221F File Offset: 0x0000041F
		public bool HasChild(string path, bool force, bool literalPath)
		{
			return this.sessionState.HasChildItems(path, force, literalPath);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000222F File Offset: 0x0000042F
		internal bool HasChild(string path, CmdletProviderContext context)
		{
			return this.sessionState.HasChildItems(path, context);
		}

		// Token: 0x04000006 RID: 6
		private Cmdlet cmdlet;

		// Token: 0x04000007 RID: 7
		private SessionStateInternal sessionState;
	}
}
