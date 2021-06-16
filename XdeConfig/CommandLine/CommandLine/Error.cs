using System;

namespace CommandLine
{
	// Token: 0x0200002E RID: 46
	public abstract class Error : IEquatable<Error>
	{
		// Token: 0x0600011A RID: 282 RVA: 0x000050E2 File Offset: 0x000032E2
		protected internal Error(ErrorType tag, bool stopsProcessing)
		{
			this.tag = tag;
			this.stopsProcessing = stopsProcessing;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000050F8 File Offset: 0x000032F8
		protected internal Error(ErrorType tag) : this(tag, false)
		{
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005102 File Offset: 0x00003302
		public ErrorType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000510A File Offset: 0x0000330A
		public bool StopsProcessing
		{
			get
			{
				return this.stopsProcessing;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005114 File Offset: 0x00003314
		public override bool Equals(object obj)
		{
			Error error = obj as Error;
			if (error != null)
			{
				return this.Equals(error);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000513A File Offset: 0x0000333A
		public override int GetHashCode()
		{
			return new
			{
				this.Tag,
				this.StopsProcessing
			}.GetHashCode();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005154 File Offset: 0x00003354
		public bool Equals(Error other)
		{
			return other != null && this.Tag.Equals(other.Tag);
		}

		// Token: 0x0400005B RID: 91
		private readonly ErrorType tag;

		// Token: 0x0400005C RID: 92
		private readonly bool stopsProcessing;
	}
}
