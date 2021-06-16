using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Text
{
	// Token: 0x02000054 RID: 84
	public class CopyrightInfo
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00008526 File Offset: 0x00006726
		public static CopyrightInfo Empty
		{
			get
			{
				return new CopyrightInfo("author", 1);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008533 File Offset: 0x00006733
		public CopyrightInfo(string author, int year) : this(true, author, new int[]
		{
			year
		})
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008547 File Offset: 0x00006747
		public CopyrightInfo(string author, params int[] years) : this(true, author, years)
		{
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008554 File Offset: 0x00006754
		public CopyrightInfo(bool isSymbolUpper, string author, params int[] copyrightYears)
		{
			if (string.IsNullOrWhiteSpace(author))
			{
				throw new ArgumentException("author");
			}
			if (copyrightYears.Length == 0)
			{
				throw new ArgumentOutOfRangeException("copyrightYears");
			}
			this.isSymbolUpper = isSymbolUpper;
			this.author = author;
			this.copyrightYears = copyrightYears;
			this.builderSize = 12 + author.Length + 4 * copyrightYears.Length + 10;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000085B6 File Offset: 0x000067B6
		protected CopyrightInfo()
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000085BE File Offset: 0x000067BE
		private CopyrightInfo(AssemblyCopyrightAttribute attribute)
		{
			this.attribute = attribute;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000085D0 File Offset: 0x000067D0
		public static CopyrightInfo Default
		{
			get
			{
				Maybe<AssemblyCopyrightAttribute> maybe = ReflectionHelper.GetAttribute<AssemblyCopyrightAttribute>();
				if (maybe.Tag == MaybeType.Just)
				{
					return new CopyrightInfo(maybe.FromJustOrFail(null));
				}
				return new CopyrightInfo(ReflectionHelper.GetAttribute<AssemblyCompanyAttribute>().FromJustOrFail(new InvalidOperationException("CopyrightInfo::Default requires that you define AssemblyCopyrightAttribute or AssemblyCompanyAttribute.")).Company, DateTime.Now.Year);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008623 File Offset: 0x00006823
		protected virtual string CopyrightWord
		{
			get
			{
				return "Copyright";
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000862A File Offset: 0x0000682A
		public static implicit operator string(CopyrightInfo info)
		{
			return info.ToString();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008634 File Offset: 0x00006834
		public override string ToString()
		{
			if (this.attribute != null)
			{
				return this.attribute.Copyright;
			}
			return new StringBuilder(this.builderSize).Append(this.CopyrightWord).Append(' ').Append(this.isSymbolUpper ? "(C)" : "(c)").Append(' ').Append(this.FormatYears(this.copyrightYears)).Append(' ').Append(this.author).ToString();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000086BC File Offset: 0x000068BC
		protected virtual string FormatYears(int[] years)
		{
			if (years.Length == 1)
			{
				return years[0].ToString(CultureInfo.InvariantCulture);
			}
			StringBuilder stringBuilder = new StringBuilder(years.Length * 6);
			for (int i = 0; i < years.Length; i++)
			{
				stringBuilder.Append(years[i].ToString(CultureInfo.InvariantCulture));
				int num = i + 1;
				if (num < years.Length)
				{
					stringBuilder.Append((years[num] - years[i] > 1) ? " - " : ", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000090 RID: 144
		private const string DefaultCopyrightWord = "Copyright";

		// Token: 0x04000091 RID: 145
		private const string SymbolLower = "(c)";

		// Token: 0x04000092 RID: 146
		private const string SymbolUpper = "(C)";

		// Token: 0x04000093 RID: 147
		private readonly AssemblyCopyrightAttribute attribute;

		// Token: 0x04000094 RID: 148
		private readonly bool isSymbolUpper;

		// Token: 0x04000095 RID: 149
		private readonly int[] copyrightYears;

		// Token: 0x04000096 RID: 150
		private readonly string author;

		// Token: 0x04000097 RID: 151
		private readonly int builderSize;
	}
}
