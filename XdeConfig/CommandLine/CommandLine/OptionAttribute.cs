using System;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x02000044 RID: 68
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class OptionAttribute : BaseAttribute
	{
		// Token: 0x0600014E RID: 334 RVA: 0x00005650 File Offset: 0x00003850
		private OptionAttribute(string shortName, string longName)
		{
			if (shortName == null)
			{
				throw new ArgumentNullException("shortName");
			}
			if (longName == null)
			{
				throw new ArgumentNullException("longName");
			}
			this.shortName = shortName;
			this.longName = longName;
			this.setName = string.Empty;
			this.separator = '\0';
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000569F File Offset: 0x0000389F
		public OptionAttribute() : this(string.Empty, string.Empty)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000056B1 File Offset: 0x000038B1
		public OptionAttribute(string longName) : this(string.Empty, longName)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000056BF File Offset: 0x000038BF
		public OptionAttribute(char shortName, string longName) : this(shortName.ToOneCharString(), longName)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000056CE File Offset: 0x000038CE
		public OptionAttribute(char shortName) : this(shortName.ToOneCharString(), string.Empty)
		{
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000056E1 File Offset: 0x000038E1
		public string LongName
		{
			get
			{
				return this.longName;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000056E9 File Offset: 0x000038E9
		public string ShortName
		{
			get
			{
				return this.shortName;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000056F1 File Offset: 0x000038F1
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000056F9 File Offset: 0x000038F9
		public string SetName
		{
			get
			{
				return this.setName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.setName = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00005710 File Offset: 0x00003910
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00005718 File Offset: 0x00003918
		public char Separator
		{
			get
			{
				return this.separator;
			}
			set
			{
				this.separator = value;
			}
		}

		// Token: 0x04000069 RID: 105
		private readonly string longName;

		// Token: 0x0400006A RID: 106
		private readonly string shortName;

		// Token: 0x0400006B RID: 107
		private string setName;

		// Token: 0x0400006C RID: 108
		private char separator;
	}
}
