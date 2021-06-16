using System;
using System.Collections;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000429 RID: 1065
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateNotNullOrEmptyAttribute : ValidateArgumentsAttribute
	{
		// Token: 0x06002EFC RID: 12028 RVA: 0x00100EF8 File Offset: 0x000FF0F8
		protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
		{
			if (arguments == null || arguments == AutomationNull.Value)
			{
				throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullOrEmptyFailure, new object[0]);
			}
			string value;
			IEnumerable enumerable;
			IEnumerator enumerator2;
			if ((value = (arguments as string)) != null)
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullOrEmptyFailure, new object[0]);
				}
			}
			else if ((enumerable = (arguments as IEnumerable)) != null)
			{
				int num = 0;
				foreach (object obj in enumerable)
				{
					num++;
					if (obj == null || obj == AutomationNull.Value)
					{
						throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullOrEmptyCollectionFailure, new object[0]);
					}
					string text = obj as string;
					if (text != null && string.IsNullOrEmpty(text))
					{
						throw new ValidationMetadataException("ArgumentCollectionContainsEmpty", null, Metadata.ValidateNotNullOrEmptyFailure, new object[0]);
					}
				}
				if (num == 0)
				{
					throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullOrEmptyCollectionFailure, new object[0]);
				}
			}
			else if ((enumerator2 = (arguments as IEnumerator)) != null)
			{
				int num2 = 0;
				while (enumerator2.MoveNext())
				{
					num2++;
					if (enumerator2.Current == null || enumerator2.Current == AutomationNull.Value)
					{
						throw new ValidationMetadataException("ArgumentIsNull", null, Metadata.ValidateNotNullOrEmptyCollectionFailure, new object[0]);
					}
				}
				if (num2 == 0)
				{
					throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullOrEmptyCollectionFailure, new object[0]);
				}
			}
		}
	}
}
