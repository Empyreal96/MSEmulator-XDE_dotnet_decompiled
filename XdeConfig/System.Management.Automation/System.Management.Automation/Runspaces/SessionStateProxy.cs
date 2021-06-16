using System;
using System.Collections.Generic;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F7 RID: 503
	public class SessionStateProxy
	{
		// Token: 0x060016F8 RID: 5880 RVA: 0x00090C4B File Offset: 0x0008EE4B
		internal SessionStateProxy()
		{
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00090C53 File Offset: 0x0008EE53
		internal SessionStateProxy(RunspaceBase runspace)
		{
			this._runspace = runspace;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00090C62 File Offset: 0x0008EE62
		public virtual void SetVariable(string name, object value)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			this._runspace.SetVariable(name, value);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00090C7F File Offset: 0x0008EE7F
		public virtual object GetVariable(string name)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (name.Equals(string.Empty))
			{
				return null;
			}
			return this._runspace.GetVariable(name);
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x00090CAA File Offset: 0x0008EEAA
		public virtual List<string> Applications
		{
			get
			{
				return this._runspace.Applications;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00090CB7 File Offset: 0x0008EEB7
		public virtual List<string> Scripts
		{
			get
			{
				return this._runspace.Scripts;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x00090CC4 File Offset: 0x0008EEC4
		public virtual DriveManagementIntrinsics Drive
		{
			get
			{
				return this._runspace.Drive;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x00090CD1 File Offset: 0x0008EED1
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x00090CDE File Offset: 0x0008EEDE
		public virtual PSLanguageMode LanguageMode
		{
			get
			{
				return this._runspace.LanguageMode;
			}
			set
			{
				this._runspace.LanguageMode = value;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x00090CEC File Offset: 0x0008EEEC
		public virtual PSModuleInfo Module
		{
			get
			{
				return this._runspace.Module;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x00090CF9 File Offset: 0x0008EEF9
		public virtual PathIntrinsics Path
		{
			get
			{
				return this._runspace.PathIntrinsics;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x00090D06 File Offset: 0x0008EF06
		public virtual CmdletProviderManagementIntrinsics Provider
		{
			get
			{
				return this._runspace.Provider;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x00090D13 File Offset: 0x0008EF13
		public virtual PSVariableIntrinsics PSVariable
		{
			get
			{
				return this._runspace.PSVariable;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00090D20 File Offset: 0x0008EF20
		public virtual CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				return this._runspace.InvokeCommand;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x00090D2D File Offset: 0x0008EF2D
		public virtual ProviderIntrinsics InvokeProvider
		{
			get
			{
				return this._runspace.InvokeProvider;
			}
		}

		// Token: 0x040009D5 RID: 2517
		private RunspaceBase _runspace;
	}
}
