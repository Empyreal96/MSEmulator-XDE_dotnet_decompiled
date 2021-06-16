using System;
using System.Collections;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000428 RID: 1064
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateNotNullAttribute : ValidateArgumentsAttribute
	{
		// Token: 0x06002EFA RID: 12026 RVA: 0x00100E10 File Offset: 0x000FF010
		protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
		{
			if (arguments == null || arguments == AutomationNull.Value)
			{
				throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullFailure, new object[0]);
			}
			IEnumerable enumerable;
			if ((enumerable = (arguments as IEnumerable)) != null)
			{
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						if (obj == null || obj == AutomationNull.Value)
						{
							throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullCollectionFailure, new object[0]);
						}
					}
					return;
				}
			}
			IEnumerator enumerator2;
			if ((enumerator2 = (arguments as IEnumerator)) != null)
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == null || enumerator2.Current == AutomationNull.Value)
					{
						throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullCollectionFailure, new object[0]);
					}
				}
			}
		}
	}
}
