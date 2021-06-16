using System;

namespace CommandLine
{
	// Token: 0x0200004A RID: 74
	public sealed class Parsed<T> : ParserResult<T>, IEquatable<Parsed<T>>
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00006496 File Offset: 0x00004696
		internal Parsed(T value, TypeInfo typeInfo) : base(ParserResultType.Parsed, typeInfo)
		{
			this.value = value;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000064A7 File Offset: 0x000046A7
		internal Parsed(T value) : this(value, TypeInfo.Create(value.GetType()))
		{
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000185 RID: 389 RVA: 0x000064C2 File Offset: 0x000046C2
		public T Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000064CC File Offset: 0x000046CC
		public override bool Equals(object obj)
		{
			Parsed<T> parsed = obj as Parsed<T>;
			if (parsed != null)
			{
				return this.Equals(parsed);
			}
			return base.Equals(obj);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000064F2 File Offset: 0x000046F2
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				this.Value
			}.GetHashCode();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000650C File Offset: 0x0000470C
		public bool Equals(Parsed<T> other)
		{
			if (other == null)
			{
				return false;
			}
			if (base.Tag.Equals(other.Tag))
			{
				T t = this.Value;
				return t.Equals(other.Value);
			}
			return false;
		}

		// Token: 0x04000077 RID: 119
		private readonly T value;
	}
}
