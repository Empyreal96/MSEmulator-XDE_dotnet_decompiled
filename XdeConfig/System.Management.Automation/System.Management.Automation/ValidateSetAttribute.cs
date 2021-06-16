using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000424 RID: 1060
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateSetAttribute : ValidateEnumeratedArgumentsAttribute
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x00100CF6 File Offset: 0x000FEEF6
		// (set) Token: 0x06002EF2 RID: 12018 RVA: 0x00100CFE File Offset: 0x000FEEFE
		public bool IgnoreCase
		{
			get
			{
				return this.ignoreCase;
			}
			set
			{
				this.ignoreCase = value;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x00100D07 File Offset: 0x000FEF07
		public IList<string> ValidValues
		{
			get
			{
				return this.validValues;
			}
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x00100D10 File Offset: 0x000FEF10
		protected override void ValidateElement(object element)
		{
			if (element == null)
			{
				throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullFailure, new object[0]);
			}
			string @string = element.ToString();
			for (int i = 0; i < this.validValues.Length; i++)
			{
				string string2 = this.validValues[i];
				if (CultureInfo.InvariantCulture.CompareInfo.Compare(string2, @string, this.ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0)
				{
					return;
				}
			}
			throw new ValidationMetadataException("ValidateSetFailure", null, Metadata.ValidateSetFailure, new object[]
			{
				element.ToString(),
				this.SetAsString()
			});
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x00100DA4 File Offset: 0x000FEFA4
		private string SetAsString()
		{
			return string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, this.validValues);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00100DC0 File Offset: 0x000FEFC0
		public ValidateSetAttribute(params string[] validValues)
		{
			if (validValues == null)
			{
				throw PSTraceSource.NewArgumentNullException("validValues");
			}
			if (validValues.Length == 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("validValues", validValues);
			}
			this.validValues = validValues;
		}

		// Token: 0x040018AF RID: 6319
		private string[] validValues;

		// Token: 0x040018B0 RID: 6320
		private bool ignoreCase = true;
	}
}
