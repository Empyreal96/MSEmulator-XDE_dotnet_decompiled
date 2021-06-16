using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000827 RID: 2087
	public abstract class SessionStateCommandEntry : ConstrainedSessionStateEntry
	{
		// Token: 0x06004FFE RID: 20478 RVA: 0x001A7BD9 File Offset: 0x001A5DD9
		protected SessionStateCommandEntry(string name) : base(name, SessionStateEntryVisibility.Public)
		{
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x001A7BEA File Offset: 0x001A5DEA
		protected internal SessionStateCommandEntry(string name, SessionStateEntryVisibility visibility) : base(name, visibility)
		{
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06005000 RID: 20480 RVA: 0x001A7BFB File Offset: 0x001A5DFB
		public CommandTypes CommandType
		{
			get
			{
				return this._commandType;
			}
		}

		// Token: 0x040028EB RID: 10475
		internal CommandTypes _commandType;

		// Token: 0x040028EC RID: 10476
		internal bool _isImported = true;
	}
}
