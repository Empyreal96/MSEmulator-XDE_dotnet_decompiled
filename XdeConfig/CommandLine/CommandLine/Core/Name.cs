using System;

namespace CommandLine.Core
{
	// Token: 0x0200007F RID: 127
	internal class Name : Token, IEquatable<Name>
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000BF87 File Offset: 0x0000A187
		public Name(string text) : base(TokenType.Name, text)
		{
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BF94 File Offset: 0x0000A194
		public override bool Equals(object obj)
		{
			Name name = obj as Name;
			if (name != null)
			{
				return this.Equals(name);
			}
			return base.Equals(obj);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000BFBA File Offset: 0x0000A1BA
		public override int GetHashCode()
		{
			return new
			{
				base.Tag,
				base.Text
			}.GetHashCode();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public bool Equals(Name other)
		{
			return other != null && base.Tag.Equals(other.Tag) && base.Text.Equals(other.Text);
		}
	}
}
