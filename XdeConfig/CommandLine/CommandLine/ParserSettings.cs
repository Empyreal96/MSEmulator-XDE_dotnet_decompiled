using System;
using System.Globalization;
using System.IO;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x0200004D RID: 77
	public class ParserSettings : IDisposable
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00007AB4 File Offset: 0x00005CB4
		public ParserSettings()
		{
			this.caseSensitive = true;
			this.caseInsensitiveEnumValues = false;
			this.autoHelp = true;
			this.autoVersion = true;
			this.parsingCulture = CultureInfo.InvariantCulture;
			try
			{
				this.maximumDisplayWidth = Console.WindowWidth;
				if (this.maximumDisplayWidth < 1)
				{
					this.maximumDisplayWidth = 80;
				}
			}
			catch (IOException)
			{
				this.maximumDisplayWidth = 80;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007B28 File Offset: 0x00005D28
		~ParserSettings()
		{
			this.Dispose(false);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007B58 File Offset: 0x00005D58
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00007B60 File Offset: 0x00005D60
		public bool CaseSensitive
		{
			get
			{
				return this.caseSensitive;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.caseSensitive, value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007B74 File Offset: 0x00005D74
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007B7C File Offset: 0x00005D7C
		public bool CaseInsensitiveEnumValues
		{
			get
			{
				return this.caseInsensitiveEnumValues;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.caseInsensitiveEnumValues, value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007B90 File Offset: 0x00005D90
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007B98 File Offset: 0x00005D98
		public CultureInfo ParsingCulture
		{
			get
			{
				return this.parsingCulture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				PopsicleSetter.Set<CultureInfo>(this.Consumed, ref this.parsingCulture, value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007BBA File Offset: 0x00005DBA
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00007BC2 File Offset: 0x00005DC2
		public TextWriter HelpWriter
		{
			get
			{
				return this.helpWriter;
			}
			set
			{
				PopsicleSetter.Set<TextWriter>(this.Consumed, ref this.helpWriter, value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007BD6 File Offset: 0x00005DD6
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00007BDE File Offset: 0x00005DDE
		public bool IgnoreUnknownArguments
		{
			get
			{
				return this.ignoreUnknownArguments;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.ignoreUnknownArguments, value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007BF2 File Offset: 0x00005DF2
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00007BFA File Offset: 0x00005DFA
		public bool AutoHelp
		{
			get
			{
				return this.autoHelp;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.autoHelp, value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00007C0E File Offset: 0x00005E0E
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00007C16 File Offset: 0x00005E16
		public bool AutoVersion
		{
			get
			{
				return this.autoVersion;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.autoVersion, value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00007C2A File Offset: 0x00005E2A
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00007C32 File Offset: 0x00005E32
		public bool EnableDashDash
		{
			get
			{
				return this.enableDashDash;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.enableDashDash, value);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00007C46 File Offset: 0x00005E46
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00007C4E File Offset: 0x00005E4E
		public int MaximumDisplayWidth
		{
			get
			{
				return this.maximumDisplayWidth;
			}
			set
			{
				this.maximumDisplayWidth = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00007C57 File Offset: 0x00005E57
		internal StringComparer NameComparer
		{
			get
			{
				if (!this.CaseSensitive)
				{
					return StringComparer.OrdinalIgnoreCase;
				}
				return StringComparer.Ordinal;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007C6C File Offset: 0x00005E6C
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00007C74 File Offset: 0x00005E74
		internal bool Consumed { get; set; }

		// Token: 0x060001B9 RID: 441 RVA: 0x00007C7D File Offset: 0x00005E7D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007C8C File Offset: 0x00005E8C
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				this.disposed = true;
			}
		}

		// Token: 0x04000079 RID: 121
		private const int DefaultMaximumLength = 80;

		// Token: 0x0400007A RID: 122
		private bool disposed;

		// Token: 0x0400007B RID: 123
		private bool caseSensitive;

		// Token: 0x0400007C RID: 124
		private bool caseInsensitiveEnumValues;

		// Token: 0x0400007D RID: 125
		private TextWriter helpWriter;

		// Token: 0x0400007E RID: 126
		private bool ignoreUnknownArguments;

		// Token: 0x0400007F RID: 127
		private bool autoHelp;

		// Token: 0x04000080 RID: 128
		private bool autoVersion;

		// Token: 0x04000081 RID: 129
		private CultureInfo parsingCulture;

		// Token: 0x04000082 RID: 130
		private bool enableDashDash;

		// Token: 0x04000083 RID: 131
		private int maximumDisplayWidth;
	}
}
