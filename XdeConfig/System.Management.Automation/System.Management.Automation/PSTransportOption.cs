using System;
using System.Collections;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020002AC RID: 684
	public abstract class PSTransportOption : ICloneable
	{
		// Token: 0x06002123 RID: 8483 RVA: 0x000BF4D4 File Offset: 0x000BD6D4
		internal virtual string ConstructOptionsAsXmlAttributes()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000BF4DB File Offset: 0x000BD6DB
		internal virtual Hashtable ConstructOptionsAsHashtable()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000BF4E2 File Offset: 0x000BD6E2
		internal virtual string ConstructQuotas()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000BF4E9 File Offset: 0x000BD6E9
		internal virtual Hashtable ConstructQuotasAsHashtable()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000BF4F0 File Offset: 0x000BD6F0
		internal void LoadFromDefaults(PSSessionType sessionType)
		{
			this.LoadFromDefaults(sessionType, false);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000BF4FA File Offset: 0x000BD6FA
		protected internal virtual void LoadFromDefaults(PSSessionType sessionType, bool keepAssigned)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000BF501 File Offset: 0x000BD701
		public object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
