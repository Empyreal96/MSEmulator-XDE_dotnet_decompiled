using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine
{
	// Token: 0x0200004B RID: 75
	public sealed class NotParsed<T> : ParserResult<T>, IEquatable<NotParsed<T>>
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00006560 File Offset: 0x00004760
		internal NotParsed(TypeInfo typeInfo, IEnumerable<Error> errors) : base(ParserResultType.NotParsed, typeInfo)
		{
			this.errors = errors;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006571 File Offset: 0x00004771
		public IEnumerable<Error> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000657C File Offset: 0x0000477C
		public override bool Equals(object obj)
		{
			NotParsed<T> notParsed = obj as NotParsed<T>;
			if (notParsed != null)
			{
				return this.Equals(notParsed);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000065A2 File Offset: 0x000047A2
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				this.Errors
			}.GetHashCode();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000065BC File Offset: 0x000047BC
		public bool Equals(NotParsed<T> other)
		{
			return other != null && base.Tag.Equals(other.Tag) && this.Errors.SequenceEqual(other.Errors);
		}

		// Token: 0x04000078 RID: 120
		private readonly IEnumerable<Error> errors;
	}
}
