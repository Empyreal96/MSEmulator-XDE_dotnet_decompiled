using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200007D RID: 125
	internal class PositionalCommandParameter
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x0001FCC4 File Offset: 0x0001DEC4
		internal PositionalCommandParameter(MergedCompiledCommandParameter parameter)
		{
			this.parameter = parameter;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001FCDE File Offset: 0x0001DEDE
		internal MergedCompiledCommandParameter Parameter
		{
			get
			{
				return this.parameter;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001FCE6 File Offset: 0x0001DEE6
		internal Collection<ParameterSetSpecificMetadata> ParameterSetData
		{
			get
			{
				return this.parameterSetData;
			}
		}

		// Token: 0x040002B3 RID: 691
		private MergedCompiledCommandParameter parameter;

		// Token: 0x040002B4 RID: 692
		private Collection<ParameterSetSpecificMetadata> parameterSetData = new Collection<ParameterSetSpecificMetadata>();
	}
}
