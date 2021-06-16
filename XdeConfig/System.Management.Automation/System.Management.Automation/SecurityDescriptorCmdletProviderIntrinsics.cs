using System;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace System.Management.Automation
{
	// Token: 0x020007FC RID: 2044
	public sealed class SecurityDescriptorCmdletProviderIntrinsics
	{
		// Token: 0x06004DCF RID: 19919 RVA: 0x0019865F File Offset: 0x0019685F
		private SecurityDescriptorCmdletProviderIntrinsics()
		{
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x00198667 File Offset: 0x00196867
		internal SecurityDescriptorCmdletProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.sessionState = cmdlet.Context.EngineSessionState;
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x00198695 File Offset: 0x00196895
		internal SecurityDescriptorCmdletProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x001986B2 File Offset: 0x001968B2
		public Collection<PSObject> Get(string path, AccessControlSections includeSections)
		{
			return this.sessionState.GetSecurityDescriptor(path, includeSections);
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x001986C1 File Offset: 0x001968C1
		internal void Get(string path, AccessControlSections includeSections, CmdletProviderContext context)
		{
			this.sessionState.GetSecurityDescriptor(path, includeSections, context);
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x001986D4 File Offset: 0x001968D4
		public Collection<PSObject> Set(string path, ObjectSecurity sd)
		{
			return this.sessionState.SetSecurityDescriptor(path, sd);
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x001986F0 File Offset: 0x001968F0
		internal void Set(string path, ObjectSecurity sd, CmdletProviderContext context)
		{
			this.sessionState.SetSecurityDescriptor(path, sd, context);
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x00198700 File Offset: 0x00196900
		public ObjectSecurity NewFromPath(string path, AccessControlSections includeSections)
		{
			return this.sessionState.NewSecurityDescriptorFromPath(path, includeSections);
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x0019870F File Offset: 0x0019690F
		public ObjectSecurity NewOfType(string providerId, string type, AccessControlSections includeSections)
		{
			return this.sessionState.NewSecurityDescriptorOfType(providerId, type, includeSections);
		}

		// Token: 0x04002838 RID: 10296
		private Cmdlet cmdlet;

		// Token: 0x04002839 RID: 10297
		private SessionStateInternal sessionState;
	}
}
