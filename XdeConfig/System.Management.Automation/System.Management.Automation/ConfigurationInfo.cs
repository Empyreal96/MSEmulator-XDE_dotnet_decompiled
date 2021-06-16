using System;

namespace System.Management.Automation
{
	// Token: 0x02000069 RID: 105
	public class ConfigurationInfo : FunctionInfo
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001A6FE File Offset: 0x000188FE
		internal ConfigurationInfo(string name, ScriptBlock configuration, ExecutionContext context) : this(name, configuration, context, null)
		{
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001A70A File Offset: 0x0001890A
		internal ConfigurationInfo(string name, ScriptBlock configuration, ExecutionContext context, string helpFile) : base(name, configuration, context, helpFile)
		{
			base.SetCommandType(CommandTypes.Configuration);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001A722 File Offset: 0x00018922
		internal ConfigurationInfo(string name, ScriptBlock configuration, ScopedItemOptions options, ExecutionContext context) : this(name, configuration, options, context, null)
		{
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001A730 File Offset: 0x00018930
		internal ConfigurationInfo(string name, ScriptBlock configuration, ScopedItemOptions options, ExecutionContext context, string helpFile, bool isMetaConfig) : base(name, configuration, options, context, helpFile)
		{
			base.SetCommandType(CommandTypes.Configuration);
			this.IsMetaConfiguration = isMetaConfig;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001A752 File Offset: 0x00018952
		internal ConfigurationInfo(string name, ScriptBlock configuration, ScopedItemOptions options, ExecutionContext context, string helpFile) : this(name, configuration, options, context, helpFile, false)
		{
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001A762 File Offset: 0x00018962
		internal ConfigurationInfo(ConfigurationInfo other) : base(other)
		{
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001A76B File Offset: 0x0001896B
		internal ConfigurationInfo(string name, ConfigurationInfo other) : base(name, other)
		{
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001A778 File Offset: 0x00018978
		internal override CommandInfo CreateGetCommandCopy(object[] arguments)
		{
			return new ConfigurationInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = arguments
			};
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001A79D File Offset: 0x0001899D
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Configuration;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001A7A4 File Offset: 0x000189A4
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x0001A7AC File Offset: 0x000189AC
		public bool IsMetaConfiguration { get; internal set; }
	}
}
