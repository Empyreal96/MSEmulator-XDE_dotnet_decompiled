using System;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x02000421 RID: 1057
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidatePatternAttribute : ValidateEnumeratedArgumentsAttribute
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x00100A09 File Offset: 0x000FEC09
		public string RegexPattern
		{
			get
			{
				return this.regexPattern;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002EE7 RID: 12007 RVA: 0x00100A1A File Offset: 0x000FEC1A
		// (set) Token: 0x06002EE6 RID: 12006 RVA: 0x00100A11 File Offset: 0x000FEC11
		public RegexOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00100A24 File Offset: 0x000FEC24
		protected override void ValidateElement(object element)
		{
			if (element == null)
			{
				throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullFailure, new object[0]);
			}
			string text = element.ToString();
			Regex regex = new Regex(this.regexPattern, this.options);
			Match match = regex.Match(text);
			if (!match.Success)
			{
				throw new ValidationMetadataException("ValidatePatternFailure", null, Metadata.ValidatePatternFailure, new object[]
				{
					text,
					this.regexPattern
				});
			}
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00100A9C File Offset: 0x000FEC9C
		public ValidatePatternAttribute(string regexPattern)
		{
			if (string.IsNullOrEmpty(regexPattern))
			{
				throw PSTraceSource.NewArgumentException("regexPattern");
			}
			this.regexPattern = regexPattern;
		}

		// Token: 0x040018AA RID: 6314
		private string regexPattern;

		// Token: 0x040018AB RID: 6315
		private RegexOptions options = RegexOptions.IgnoreCase;
	}
}
