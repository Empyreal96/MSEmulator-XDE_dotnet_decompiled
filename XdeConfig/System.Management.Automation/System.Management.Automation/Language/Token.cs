using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005CE RID: 1486
	public class Token
	{
		// Token: 0x06003FCC RID: 16332 RVA: 0x001518C3 File Offset: 0x0014FAC3
		internal Token(InternalScriptExtent scriptExtent, TokenKind kind, TokenFlags tokenFlags)
		{
			this._scriptExtent = scriptExtent;
			this._kind = kind;
			this._tokenFlags = (tokenFlags | kind.GetTraits());
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x001518E7 File Offset: 0x0014FAE7
		internal void SetIsCommandArgument()
		{
			if (this._kind != TokenKind.Identifier)
			{
				this._kind = TokenKind.Generic;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x001518F9 File Offset: 0x0014FAF9
		public string Text
		{
			get
			{
				return this._scriptExtent.Text;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00151906 File Offset: 0x0014FB06
		// (set) Token: 0x06003FD0 RID: 16336 RVA: 0x0015190E File Offset: 0x0014FB0E
		public TokenFlags TokenFlags
		{
			get
			{
				return this._tokenFlags;
			}
			internal set
			{
				this._tokenFlags = value;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x00151917 File Offset: 0x0014FB17
		public TokenKind Kind
		{
			get
			{
				return this._kind;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x0015191F File Offset: 0x0014FB1F
		public bool HasError
		{
			get
			{
				return (this._tokenFlags & TokenFlags.TokenInError) != TokenFlags.None;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x00151933 File Offset: 0x0014FB33
		public IScriptExtent Extent
		{
			get
			{
				return this._scriptExtent;
			}
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0015193B File Offset: 0x0014FB3B
		public override string ToString()
		{
			if (this._kind != TokenKind.EndOfInput)
			{
				return this.Text;
			}
			return "<eof>";
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00151954 File Offset: 0x0014FB54
		internal virtual string ToDebugString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}: <{2}>", new object[]
			{
				new string(' ', indent),
				this._kind,
				this.Text
			});
		}

		// Token: 0x04002021 RID: 8225
		private TokenKind _kind;

		// Token: 0x04002022 RID: 8226
		private TokenFlags _tokenFlags;

		// Token: 0x04002023 RID: 8227
		private readonly InternalScriptExtent _scriptExtent;
	}
}
