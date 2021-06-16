using System;
using System.Collections;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200040F RID: 1039
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public abstract class ValidateEnumeratedArgumentsAttribute : ValidateArgumentsAttribute
	{
		// Token: 0x06002E91 RID: 11921
		protected abstract void ValidateElement(object element);

		// Token: 0x06002E92 RID: 11922 RVA: 0x00100120 File Offset: 0x000FE320
		protected sealed override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
		{
			if (arguments == null || arguments == AutomationNull.Value)
			{
				throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullOrEmptyCollectionFailure, new object[0]);
			}
			IEnumerable enumerable = LanguagePrimitives.GetEnumerable(arguments);
			if (enumerable == null)
			{
				this.ValidateElement(arguments);
				return;
			}
			foreach (object element in enumerable)
			{
				this.ValidateElement(element);
			}
		}
	}
}
