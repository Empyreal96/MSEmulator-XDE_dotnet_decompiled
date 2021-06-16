using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D0 RID: 1488
	public class ParameterToken : Token
	{
		// Token: 0x06003FD9 RID: 16345 RVA: 0x00151A16 File Offset: 0x0014FC16
		internal ParameterToken(InternalScriptExtent scriptExtent, string parameterName, bool usedColon) : base(scriptExtent, TokenKind.Parameter, TokenFlags.None)
		{
			this._parameterName = parameterName;
			this._usedColon = usedColon;
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003FDA RID: 16346 RVA: 0x00151A2F File Offset: 0x0014FC2F
		public string ParameterName
		{
			get
			{
				return this._parameterName;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003FDB RID: 16347 RVA: 0x00151A37 File Offset: 0x0014FC37
		public bool UsedColon
		{
			get
			{
				return this._usedColon;
			}
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00151A40 File Offset: 0x0014FC40
		internal override string ToDebugString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}: <-{2}{3}>", new object[]
			{
				new string(' ', indent),
				base.Kind,
				this._parameterName,
				this._usedColon ? ":" : ""
			});
		}

		// Token: 0x04002025 RID: 8229
		private readonly string _parameterName;

		// Token: 0x04002026 RID: 8230
		private readonly bool _usedColon;
	}
}
