using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x020001E3 RID: 483
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public abstract class ArgumentTransformationAttribute : CmdletMetadataAttribute
	{
		// Token: 0x0600162A RID: 5674
		public abstract object Transform(EngineIntrinsics engineIntrinsics, object inputData);

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x0008D654 File Offset: 0x0008B854
		public virtual bool TransformNullOptionalParameters
		{
			get
			{
				return true;
			}
		}
	}
}
