using System;

namespace System.Management.Automation
{
	// Token: 0x020002AB RID: 683
	public abstract class PSSessionTypeOption
	{
		// Token: 0x0600211F RID: 8479 RVA: 0x000BF4B7 File Offset: 0x000BD6B7
		protected internal virtual string ConstructPrivateData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000BF4BE File Offset: 0x000BD6BE
		protected internal virtual PSSessionTypeOption ConstructObjectFromPrivateData(string privateData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000BF4C5 File Offset: 0x000BD6C5
		protected internal virtual void CopyUpdatedValuesFrom(PSSessionTypeOption updated)
		{
			throw new NotImplementedException();
		}
	}
}
