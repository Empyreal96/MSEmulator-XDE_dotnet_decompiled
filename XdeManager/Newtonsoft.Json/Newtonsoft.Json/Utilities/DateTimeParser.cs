using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000044 RID: 68
	internal struct DateTimeParser
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00012812 File Offset: 0x00010A12
		public bool Parse(char[] text, int startIndex, int length)
		{
			this._text = text;
			this._end = startIndex + length;
			return this.ParseDate(startIndex) && this.ParseChar(DateTimeParser.Lzyyyy_MM_dd + startIndex, 'T') && this.ParseTimeAndZoneAndWhitespace(DateTimeParser.Lzyyyy_MM_ddT + startIndex);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00012850 File Offset: 0x00010A50
		private bool ParseDate(int start)
		{
			return this.Parse4Digit(start, out this.Year) && 1 <= this.Year && this.ParseChar(start + DateTimeParser.Lzyyyy, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_, out this.Month) && 1 <= this.Month && this.Month <= 12 && this.ParseChar(start + DateTimeParser.Lzyyyy_MM, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_MM_, out this.Day) && 1 <= this.Day && this.Day <= DateTime.DaysInMonth(this.Year, this.Month);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00012901 File Offset: 0x00010B01
		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			return this.ParseTime(ref start) && this.ParseZone(start);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00012918 File Offset: 0x00010B18
		private bool ParseTime(ref int start)
		{
			if (!this.Parse2Digit(start, out this.Hour) || this.Hour > 24 || !this.ParseChar(start + DateTimeParser.LzHH, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_, out this.Minute) || this.Minute >= 60 || !this.ParseChar(start + DateTimeParser.LzHH_mm, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_mm_, out this.Second) || this.Second >= 60 || (this.Hour == 24 && (this.Minute != 0 || this.Second != 0)))
			{
				return false;
			}
			start += DateTimeParser.LzHH_mm_ss;
			if (this.ParseChar(start, '.'))
			{
				this.Fraction = 0;
				int num = 0;
				for (;;)
				{
					int num2 = start + 1;
					start = num2;
					if (num2 >= this._end || num >= 7)
					{
						break;
					}
					int num3 = (int)(this._text[start] - '0');
					if (num3 < 0 || num3 > 9)
					{
						break;
					}
					this.Fraction = this.Fraction * 10 + num3;
					num++;
				}
				if (num < 7)
				{
					if (num == 0)
					{
						return false;
					}
					this.Fraction *= DateTimeParser.Power10[7 - num];
				}
				if (this.Hour == 24 && this.Fraction != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012A58 File Offset: 0x00010C58
		private bool ParseZone(int start)
		{
			if (start < this._end)
			{
				char c = this._text[start];
				if (c == 'Z' || c == 'z')
				{
					this.Zone = ParserTimeZone.Utc;
					start++;
				}
				else
				{
					if (start + 2 < this._end && this.Parse2Digit(start + DateTimeParser.Lz_, out this.ZoneHour) && this.ZoneHour <= 99)
					{
						if (c != '+')
						{
							if (c == '-')
							{
								this.Zone = ParserTimeZone.LocalWestOfUtc;
								start += DateTimeParser.Lz_zz;
							}
						}
						else
						{
							this.Zone = ParserTimeZone.LocalEastOfUtc;
							start += DateTimeParser.Lz_zz;
						}
					}
					if (start < this._end)
					{
						if (this.ParseChar(start, ':'))
						{
							start++;
							if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
							{
								start += 2;
							}
						}
						else if (start + 1 < this._end && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
						{
							start += 2;
						}
					}
				}
			}
			return start == this._end;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00012B64 File Offset: 0x00010D64
		private bool Parse4Digit(int start, out int num)
		{
			if (start + 3 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				int num4 = (int)(this._text[start + 2] - '0');
				int num5 = (int)(this._text[start + 3] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012BF0 File Offset: 0x00010DF0
		private bool Parse2Digit(int start, out int num)
		{
			if (start + 1 < this._end)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00012C42 File Offset: 0x00010E42
		private bool ParseChar(int start, char ch)
		{
			return start < this._end && this._text[start] == ch;
		}

		// Token: 0x04000190 RID: 400
		public int Year;

		// Token: 0x04000191 RID: 401
		public int Month;

		// Token: 0x04000192 RID: 402
		public int Day;

		// Token: 0x04000193 RID: 403
		public int Hour;

		// Token: 0x04000194 RID: 404
		public int Minute;

		// Token: 0x04000195 RID: 405
		public int Second;

		// Token: 0x04000196 RID: 406
		public int Fraction;

		// Token: 0x04000197 RID: 407
		public int ZoneHour;

		// Token: 0x04000198 RID: 408
		public int ZoneMinute;

		// Token: 0x04000199 RID: 409
		public ParserTimeZone Zone;

		// Token: 0x0400019A RID: 410
		private char[] _text;

		// Token: 0x0400019B RID: 411
		private int _end;

		// Token: 0x0400019C RID: 412
		private static readonly int[] Power10 = new int[]
		{
			-1,
			10,
			100,
			1000,
			10000,
			100000,
			1000000
		};

		// Token: 0x0400019D RID: 413
		private static readonly int Lzyyyy = "yyyy".Length;

		// Token: 0x0400019E RID: 414
		private static readonly int Lzyyyy_ = "yyyy-".Length;

		// Token: 0x0400019F RID: 415
		private static readonly int Lzyyyy_MM = "yyyy-MM".Length;

		// Token: 0x040001A0 RID: 416
		private static readonly int Lzyyyy_MM_ = "yyyy-MM-".Length;

		// Token: 0x040001A1 RID: 417
		private static readonly int Lzyyyy_MM_dd = "yyyy-MM-dd".Length;

		// Token: 0x040001A2 RID: 418
		private static readonly int Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;

		// Token: 0x040001A3 RID: 419
		private static readonly int LzHH = "HH".Length;

		// Token: 0x040001A4 RID: 420
		private static readonly int LzHH_ = "HH:".Length;

		// Token: 0x040001A5 RID: 421
		private static readonly int LzHH_mm = "HH:mm".Length;

		// Token: 0x040001A6 RID: 422
		private static readonly int LzHH_mm_ = "HH:mm:".Length;

		// Token: 0x040001A7 RID: 423
		private static readonly int LzHH_mm_ss = "HH:mm:ss".Length;

		// Token: 0x040001A8 RID: 424
		private static readonly int Lz_ = "-".Length;

		// Token: 0x040001A9 RID: 425
		private static readonly int Lz_zz = "-zz".Length;

		// Token: 0x040001AA RID: 426
		private const short MaxFractionDigits = 7;
	}
}
