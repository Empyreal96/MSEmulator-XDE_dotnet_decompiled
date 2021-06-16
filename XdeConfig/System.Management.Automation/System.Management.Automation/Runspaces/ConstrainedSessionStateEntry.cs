using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000826 RID: 2086
	public abstract class ConstrainedSessionStateEntry : InitialSessionStateEntry
	{
		// Token: 0x06004FFB RID: 20475 RVA: 0x001A7BB8 File Offset: 0x001A5DB8
		protected ConstrainedSessionStateEntry(string name, SessionStateEntryVisibility visibility) : base(name)
		{
			this._visibility = visibility;
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004FFC RID: 20476 RVA: 0x001A7BC8 File Offset: 0x001A5DC8
		// (set) Token: 0x06004FFD RID: 20477 RVA: 0x001A7BD0 File Offset: 0x001A5DD0
		public SessionStateEntryVisibility Visibility
		{
			get
			{
				return this._visibility;
			}
			set
			{
				this._visibility = value;
			}
		}

		// Token: 0x040028EA RID: 10474
		private SessionStateEntryVisibility _visibility;
	}
}
