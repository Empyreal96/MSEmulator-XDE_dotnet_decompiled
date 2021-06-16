using System;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A0 RID: 2464
	public sealed class MethodParameter
	{
		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06005AC7 RID: 23239 RVA: 0x001E8A40 File Offset: 0x001E6C40
		// (set) Token: 0x06005AC8 RID: 23240 RVA: 0x001E8A48 File Offset: 0x001E6C48
		public string Name { get; set; }

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x001E8A51 File Offset: 0x001E6C51
		// (set) Token: 0x06005ACA RID: 23242 RVA: 0x001E8A59 File Offset: 0x001E6C59
		public Type ParameterType { get; set; }

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06005ACB RID: 23243 RVA: 0x001E8A62 File Offset: 0x001E6C62
		// (set) Token: 0x06005ACC RID: 23244 RVA: 0x001E8A6A File Offset: 0x001E6C6A
		public string ParameterTypeName { get; set; }

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x001E8A73 File Offset: 0x001E6C73
		// (set) Token: 0x06005ACE RID: 23246 RVA: 0x001E8A7B File Offset: 0x001E6C7B
		public MethodParameterBindings Bindings { get; set; }

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x001E8A84 File Offset: 0x001E6C84
		// (set) Token: 0x06005AD0 RID: 23248 RVA: 0x001E8A8C File Offset: 0x001E6C8C
		public object Value { get; set; }

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x001E8A95 File Offset: 0x001E6C95
		// (set) Token: 0x06005AD2 RID: 23250 RVA: 0x001E8A9D File Offset: 0x001E6C9D
		public bool IsValuePresent { get; set; }
	}
}
