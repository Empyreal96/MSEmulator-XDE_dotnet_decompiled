using System;

namespace System.Management.Automation
{
	// Token: 0x0200081E RID: 2078
	internal class CustomShellProviderNames : ProviderNames
	{
		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x001A77A1 File Offset: 0x001A59A1
		internal override string Environment
		{
			get
			{
				return "Environment";
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x001A77A8 File Offset: 0x001A59A8
		internal override string Certificate
		{
			get
			{
				return "Certificate";
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x001A77AF File Offset: 0x001A59AF
		internal override string Variable
		{
			get
			{
				return "Variable";
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x001A77B6 File Offset: 0x001A59B6
		internal override string Alias
		{
			get
			{
				return "Alias";
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06004FD1 RID: 20433 RVA: 0x001A77BD File Offset: 0x001A59BD
		internal override string Function
		{
			get
			{
				return "Function";
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x001A77C4 File Offset: 0x001A59C4
		internal override string FileSystem
		{
			get
			{
				return "FileSystem";
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06004FD3 RID: 20435 RVA: 0x001A77CB File Offset: 0x001A59CB
		internal override string Registry
		{
			get
			{
				return "Registry";
			}
		}
	}
}
