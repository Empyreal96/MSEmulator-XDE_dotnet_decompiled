using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000940 RID: 2368
	internal sealed class ViewDefinition
	{
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x001C93B7 File Offset: 0x001C75B7
		internal Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x001C93BF File Offset: 0x001C75BF
		internal ViewDefinition()
		{
			this._instanceId = Guid.NewGuid();
		}

		// Token: 0x04002EDA RID: 11994
		internal DatabaseLoadingInfo loadingInfo;

		// Token: 0x04002EDB RID: 11995
		internal string name;

		// Token: 0x04002EDC RID: 11996
		internal AppliesTo appliesTo = new AppliesTo();

		// Token: 0x04002EDD RID: 11997
		internal GroupBy groupBy;

		// Token: 0x04002EDE RID: 11998
		internal FormatControlDefinitionHolder formatControlDefinitionHolder = new FormatControlDefinitionHolder();

		// Token: 0x04002EDF RID: 11999
		internal ControlBase mainControl;

		// Token: 0x04002EE0 RID: 12000
		internal bool outOfBand;

		// Token: 0x04002EE1 RID: 12001
		private Guid _instanceId;
	}
}
