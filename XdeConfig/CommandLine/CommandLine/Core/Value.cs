using System;

namespace CommandLine.Core
{
	// Token: 0x02000080 RID: 128
	internal class Value : Token, IEquatable<Value>
	{
		// Token: 0x06000306 RID: 774 RVA: 0x0000C01A File Offset: 0x0000A21A
		public Value(string text) : this(text, false)
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000C024 File Offset: 0x0000A224
		public Value(string text, bool explicitlyAssigned) : base(TokenType.Value, text)
		{
			this.explicitlyAssigned = explicitlyAssigned;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000C035 File Offset: 0x0000A235
		public bool ExplicitlyAssigned
		{
			get
			{
				return this.explicitlyAssigned;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C040 File Offset: 0x0000A240
		public override bool Equals(object obj)
		{
			Value value = obj as Value;
			if (value != null)
			{
				return this.Equals(value);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000C066 File Offset: 0x0000A266
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				base.Text
			}.GetHashCode();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000C080 File Offset: 0x0000A280
		public bool Equals(Value other)
		{
			return other != null && base.Tag.Equals(other.Tag) && base.Text.Equals(other.Text);
		}

		// Token: 0x040000E7 RID: 231
		private readonly bool explicitlyAssigned;
	}
}
