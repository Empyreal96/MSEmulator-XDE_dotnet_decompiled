using System;

namespace System.Management.Automation
{
	// Token: 0x02000072 RID: 114
	internal class MergedCompiledCommandParameter
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x0001D0C2 File Offset: 0x0001B2C2
		internal MergedCompiledCommandParameter(CompiledCommandParameter parameter, ParameterBinderAssociation binderAssociation)
		{
			this.Parameter = parameter;
			this.BinderAssociation = binderAssociation;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x0001D0E0 File Offset: 0x0001B2E0
		internal CompiledCommandParameter Parameter { get; private set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001D0E9 File Offset: 0x0001B2E9
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x0001D0F1 File Offset: 0x0001B2F1
		internal ParameterBinderAssociation BinderAssociation { get; private set; }

		// Token: 0x06000629 RID: 1577 RVA: 0x0001D0FA File Offset: 0x0001B2FA
		public override string ToString()
		{
			return this.Parameter.ToString();
		}
	}
}
