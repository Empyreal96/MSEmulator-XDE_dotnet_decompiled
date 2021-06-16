using System;

namespace CommandLine
{
	// Token: 0x0200002F RID: 47
	public abstract class TokenError : Error, IEquatable<TokenError>
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00005185 File Offset: 0x00003385
		protected internal TokenError(ErrorType tag, string token) : base(tag)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			this.token = token;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000051A3 File Offset: 0x000033A3
		public string Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000051AC File Offset: 0x000033AC
		public override bool Equals(object obj)
		{
			TokenError tokenError = obj as TokenError;
			if (tokenError != null)
			{
				return this.Equals(tokenError);
			}
			return base.Equals(obj);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000051D2 File Offset: 0x000033D2
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				base.StopsProcessing,
				this.Token
			}.GetHashCode();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000051F0 File Offset: 0x000033F0
		public bool Equals(TokenError other)
		{
			return other != null && base.Tag.Equals(other.Tag) && this.Token.Equals(other.Token);
		}

		// Token: 0x0400005D RID: 93
		private readonly string token;
	}
}
