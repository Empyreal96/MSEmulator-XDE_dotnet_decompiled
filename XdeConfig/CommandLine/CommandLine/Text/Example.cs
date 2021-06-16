using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Text
{
	// Token: 0x02000055 RID: 85
	public sealed class Example : IEquatable<Example>
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x00008740 File Offset: 0x00006940
		public Example(string helpText, IEnumerable<UnParserSettings> formatStyles, object sample)
		{
			if (string.IsNullOrEmpty(helpText))
			{
				throw new ArgumentException("helpText can't be null or empty", "helpText");
			}
			if (formatStyles == null)
			{
				throw new ArgumentNullException("formatStyles");
			}
			if (sample == null)
			{
				throw new ArgumentNullException("sample");
			}
			this.helpText = helpText;
			this.formatStyles = formatStyles;
			this.sample = sample;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000879C File Offset: 0x0000699C
		public Example(string helpText, UnParserSettings formatStyle, object sample) : this(helpText, new UnParserSettings[]
		{
			formatStyle
		}, sample)
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000087B0 File Offset: 0x000069B0
		public Example(string helpText, object sample) : this(helpText, Enumerable.Empty<UnParserSettings>(), sample)
		{
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000087BF File Offset: 0x000069BF
		public string HelpText
		{
			get
			{
				return this.helpText;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000087C7 File Offset: 0x000069C7
		public IEnumerable<UnParserSettings> FormatStyles
		{
			get
			{
				return this.formatStyles;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000087CF File Offset: 0x000069CF
		public object Sample
		{
			get
			{
				return this.sample;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000087D8 File Offset: 0x000069D8
		public override bool Equals(object obj)
		{
			Example example = obj as Example;
			if (example != null)
			{
				return this.Equals(example);
			}
			return base.Equals(obj);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000087FE File Offset: 0x000069FE
		public override int GetHashCode()
		{
			return new
			{
				this.HelpText,
				this.FormatStyles,
				this.Sample
			}.GetHashCode();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000881C File Offset: 0x00006A1C
		public bool Equals(Example other)
		{
			return other != null && (this.HelpText.Equals(other.HelpText) && this.FormatStyles.SequenceEqual(other.FormatStyles)) && this.Sample.Equals(other.Sample);
		}

		// Token: 0x04000098 RID: 152
		private readonly string helpText;

		// Token: 0x04000099 RID: 153
		private readonly IEnumerable<UnParserSettings> formatStyles;

		// Token: 0x0400009A RID: 154
		private readonly object sample;
	}
}
