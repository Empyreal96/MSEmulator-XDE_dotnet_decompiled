using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005D1 RID: 1489
	public class VariableToken : Token
	{
		// Token: 0x06003FDD RID: 16349 RVA: 0x00151A9D File Offset: 0x0014FC9D
		internal VariableToken(InternalScriptExtent scriptExtent, VariablePath path, TokenFlags tokenFlags, bool splatted) : base(scriptExtent, splatted ? TokenKind.SplattedVariable : TokenKind.Variable, tokenFlags)
		{
			this.VariablePath = path;
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x00151AB6 File Offset: 0x0014FCB6
		public string Name
		{
			get
			{
				return this.VariablePath.UnqualifiedPath;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x00151AC3 File Offset: 0x0014FCC3
		// (set) Token: 0x06003FE0 RID: 16352 RVA: 0x00151ACB File Offset: 0x0014FCCB
		public VariablePath VariablePath { get; private set; }

		// Token: 0x06003FE1 RID: 16353 RVA: 0x00151AD4 File Offset: 0x0014FCD4
		internal override string ToDebugString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}: <{2}> Name:<{3}>", new object[]
			{
				new string(' ', indent),
				base.Kind,
				base.Text,
				this.Name
			});
		}
	}
}
