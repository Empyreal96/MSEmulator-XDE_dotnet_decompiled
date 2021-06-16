using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200007B RID: 123
	internal class ParameterSetPromptingData
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		internal ParameterSetPromptingData(uint parameterSet, bool isDefaultSet)
		{
			this.parameterSet = parameterSet;
			this.isDefaultSet = isDefaultSet;
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001FA69 File Offset: 0x0001DC69
		internal bool IsDefaultSet
		{
			get
			{
				return this.isDefaultSet;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001FA71 File Offset: 0x0001DC71
		internal uint ParameterSet
		{
			get
			{
				return this.parameterSet;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001FA79 File Offset: 0x0001DC79
		internal bool IsAllSet
		{
			get
			{
				return this.parameterSet == uint.MaxValue;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001FA84 File Offset: 0x0001DC84
		internal Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> PipelineableMandatoryParameters
		{
			get
			{
				return this.pipelineableMandatoryParameters;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001FA8C File Offset: 0x0001DC8C
		internal Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> PipelineableMandatoryByValueParameters
		{
			get
			{
				return this.pipelineableMandatoryByValueParameters;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001FA94 File Offset: 0x0001DC94
		internal Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> PipelineableMandatoryByPropertyNameParameters
		{
			get
			{
				return this.pipelineableMandatoryByPropertyNameParameters;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0001FA9C File Offset: 0x0001DC9C
		internal Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> NonpipelineableMandatoryParameters
		{
			get
			{
				return this.nonpipelineableMandatoryParameters;
			}
		}

		// Token: 0x040002A2 RID: 674
		private bool isDefaultSet;

		// Token: 0x040002A3 RID: 675
		private uint parameterSet;

		// Token: 0x040002A4 RID: 676
		private Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> pipelineableMandatoryParameters = new Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>();

		// Token: 0x040002A5 RID: 677
		private Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> pipelineableMandatoryByValueParameters = new Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>();

		// Token: 0x040002A6 RID: 678
		private Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> pipelineableMandatoryByPropertyNameParameters = new Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>();

		// Token: 0x040002A7 RID: 679
		private Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata> nonpipelineableMandatoryParameters = new Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>();
	}
}
