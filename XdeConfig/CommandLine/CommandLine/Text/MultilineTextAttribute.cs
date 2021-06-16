using System;
using System.Linq;
using System.Text;

namespace CommandLine.Text
{
	// Token: 0x0200005A RID: 90
	public abstract class MultilineTextAttribute : Attribute
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00009C81 File Offset: 0x00007E81
		protected MultilineTextAttribute(string line1) : this(line1, string.Empty, string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009C9E File Offset: 0x00007E9E
		protected MultilineTextAttribute(string line1, string line2) : this(line1, line2, string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009CB7 File Offset: 0x00007EB7
		protected MultilineTextAttribute(string line1, string line2, string line3) : this(line1, line2, line3, string.Empty, string.Empty)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009CCC File Offset: 0x00007ECC
		protected MultilineTextAttribute(string line1, string line2, string line3, string line4) : this(line1, line2, line3, line4, string.Empty)
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009CE0 File Offset: 0x00007EE0
		protected MultilineTextAttribute(string line1, string line2, string line3, string line4, string line5)
		{
			if (line1 == null)
			{
				throw new ArgumentException("line1");
			}
			if (line2 == null)
			{
				throw new ArgumentException("line2");
			}
			if (line3 == null)
			{
				throw new ArgumentException("line3");
			}
			if (line4 == null)
			{
				throw new ArgumentException("line4");
			}
			if (line5 == null)
			{
				throw new ArgumentException("line5");
			}
			this.line1 = line1;
			this.line2 = line2;
			this.line3 = line3;
			this.line4 = line4;
			this.line5 = line5;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00009D60 File Offset: 0x00007F60
		public virtual string Value
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(string.Empty);
				string[] array = new string[]
				{
					this.line1,
					this.line2,
					this.line3,
					this.line4,
					this.line5
				};
				for (int i = 0; i < this.GetLastLineWithText(array); i++)
				{
					stringBuilder.AppendLine(array[i]);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009DCE File Offset: 0x00007FCE
		public string Line1
		{
			get
			{
				return this.line1;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00009DD6 File Offset: 0x00007FD6
		public string Line2
		{
			get
			{
				return this.line2;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009DDE File Offset: 0x00007FDE
		public string Line3
		{
			get
			{
				return this.line3;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00009DE6 File Offset: 0x00007FE6
		public string Line4
		{
			get
			{
				return this.line4;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00009DEE File Offset: 0x00007FEE
		public string Line5
		{
			get
			{
				return this.line5;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009DF8 File Offset: 0x00007FF8
		internal HelpText AddToHelpText(HelpText helpText, Func<string, HelpText> func)
		{
			string[] array = new string[]
			{
				this.line1,
				this.line2,
				this.line3,
				this.line4,
				this.line5
			};
			return array.Take(this.GetLastLineWithText(array)).Aggregate(helpText, (HelpText current, string line) => func(line));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009E65 File Offset: 0x00008065
		internal HelpText AddToHelpText(HelpText helpText, bool before)
		{
			if (!before)
			{
				return this.AddToHelpText(helpText, new Func<string, HelpText>(helpText.AddPostOptionsLine));
			}
			return this.AddToHelpText(helpText, new Func<string, HelpText>(helpText.AddPreOptionsLine));
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009E91 File Offset: 0x00008091
		protected virtual int GetLastLineWithText(string[] value)
		{
			return Array.FindLastIndex<string>(value, (string str) => !string.IsNullOrEmpty(str)) + 1;
		}

		// Token: 0x040000B6 RID: 182
		private readonly string line1;

		// Token: 0x040000B7 RID: 183
		private readonly string line2;

		// Token: 0x040000B8 RID: 184
		private readonly string line3;

		// Token: 0x040000B9 RID: 185
		private readonly string line4;

		// Token: 0x040000BA RID: 186
		private readonly string line5;
	}
}
