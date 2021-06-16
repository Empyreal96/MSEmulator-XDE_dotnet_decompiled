using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000017 RID: 23
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public abstract class ValidateArgumentsAttribute : CmdletMetadataAttribute
	{
		// Token: 0x06000114 RID: 276
		protected abstract void Validate(object arguments, EngineIntrinsics engineIntrinsics);

		// Token: 0x06000115 RID: 277 RVA: 0x00005DC5 File Offset: 0x00003FC5
		internal void InternalValidate(object o, EngineIntrinsics engineIntrinsics)
		{
			this.Validate(o, engineIntrinsics);
		}
	}
}
