using System;

namespace CommandLine
{
	// Token: 0x02000042 RID: 66
	public sealed class NameInfo : IEquatable<NameInfo>
	{
		// Token: 0x06000145 RID: 325 RVA: 0x00005524 File Offset: 0x00003724
		internal NameInfo(string shortName, string longName)
		{
			if (shortName == null)
			{
				throw new ArgumentNullException("shortName");
			}
			if (longName == null)
			{
				throw new ArgumentNullException("longName");
			}
			this.longName = longName;
			this.shortName = shortName;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00005556 File Offset: 0x00003756
		public string ShortName
		{
			get
			{
				return this.shortName;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000555E File Offset: 0x0000375E
		public string LongName
		{
			get
			{
				return this.longName;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00005568 File Offset: 0x00003768
		public string NameText
		{
			get
			{
				if (this.ShortName.Length > 0 && this.LongName.Length > 0)
				{
					return this.ShortName + ", " + this.LongName;
				}
				if (this.ShortName.Length <= 0)
				{
					return this.LongName;
				}
				return this.ShortName;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000055C4 File Offset: 0x000037C4
		public override bool Equals(object obj)
		{
			NameInfo nameInfo = obj as NameInfo;
			if (nameInfo != null)
			{
				return this.Equals(nameInfo);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000055EA File Offset: 0x000037EA
		public override int GetHashCode()
		{
			return new
			{
				this.ShortName,
				this.LongName
			}.GetHashCode();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005602 File Offset: 0x00003802
		public bool Equals(NameInfo other)
		{
			return other != null && this.ShortName.Equals(other.ShortName) && this.LongName.Equals(other.LongName);
		}

		// Token: 0x04000066 RID: 102
		public static readonly NameInfo EmptyName = new NameInfo(string.Empty, string.Empty);

		// Token: 0x04000067 RID: 103
		private readonly string longName;

		// Token: 0x04000068 RID: 104
		private readonly string shortName;
	}
}
