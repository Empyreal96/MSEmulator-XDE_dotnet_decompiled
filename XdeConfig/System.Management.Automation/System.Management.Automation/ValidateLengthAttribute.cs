using System;

namespace System.Management.Automation
{
	// Token: 0x0200041F RID: 1055
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateLengthAttribute : ValidateEnumeratedArgumentsAttribute
	{
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x001005A8 File Offset: 0x000FE7A8
		public int MinLength
		{
			get
			{
				return this.minLength;
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x001005B0 File Offset: 0x000FE7B0
		public int MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x001005B8 File Offset: 0x000FE7B8
		protected override void ValidateElement(object element)
		{
			string text = element as string;
			if (text == null)
			{
				throw new ValidationMetadataException("ValidateLengthNotString", null, Metadata.ValidateLengthNotString, new object[0]);
			}
			int length = text.Length;
			if (length < this.minLength)
			{
				throw new ValidationMetadataException("ValidateLengthMinLengthFailure", null, Metadata.ValidateLengthMinLengthFailure, new object[]
				{
					this.minLength,
					length
				});
			}
			if (length > this.maxLength)
			{
				throw new ValidationMetadataException("ValidateLengthMaxLengthFailure", null, Metadata.ValidateLengthMaxLengthFailure, new object[]
				{
					this.maxLength,
					length
				});
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00100660 File Offset: 0x000FE860
		public ValidateLengthAttribute(int minLength, int maxLength)
		{
			if (minLength < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("minLength", minLength);
			}
			if (maxLength <= 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("maxLength", maxLength);
			}
			if (maxLength < minLength)
			{
				throw new ValidationMetadataException("ValidateLengthMaxLengthSmallerThanMinLength", null, Metadata.ValidateLengthMaxLengthSmallerThanMinLength, new object[0]);
			}
			this.minLength = minLength;
			this.maxLength = maxLength;
		}

		// Token: 0x040018A3 RID: 6307
		private int minLength;

		// Token: 0x040018A4 RID: 6308
		private int maxLength;
	}
}
