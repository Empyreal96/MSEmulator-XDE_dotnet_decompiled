using System;

namespace CommandLine
{
	// Token: 0x02000031 RID: 49
	public abstract class NamedError : Error, IEquatable<NamedError>
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00005240 File Offset: 0x00003440
		protected internal NamedError(ErrorType tag, NameInfo nameInfo) : base(tag)
		{
			this.nameInfo = nameInfo;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005250 File Offset: 0x00003450
		public NameInfo NameInfo
		{
			get
			{
				return this.nameInfo;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005258 File Offset: 0x00003458
		public override bool Equals(object obj)
		{
			NamedError namedError = obj as NamedError;
			if (namedError != null)
			{
				return this.Equals(namedError);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000527E File Offset: 0x0000347E
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				base.StopsProcessing,
				this.NameInfo
			}.GetHashCode();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000529C File Offset: 0x0000349C
		public bool Equals(NamedError other)
		{
			return other != null && base.Tag.Equals(other.Tag) && this.NameInfo.Equals(other.NameInfo);
		}

		// Token: 0x0400005E RID: 94
		private readonly NameInfo nameInfo;
	}
}
