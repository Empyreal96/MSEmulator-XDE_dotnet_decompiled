using System;

namespace System.Management.Automation
{
	// Token: 0x02000068 RID: 104
	public class FilterInfo : FunctionInfo
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001A67B File Offset: 0x0001887B
		internal FilterInfo(string name, ScriptBlock filter, ExecutionContext context) : this(name, filter, context, null)
		{
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001A687 File Offset: 0x00018887
		internal FilterInfo(string name, ScriptBlock filter, ExecutionContext context, string helpFile) : base(name, filter, context, helpFile)
		{
			base.SetCommandType(CommandTypes.Filter);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001A69B File Offset: 0x0001889B
		internal FilterInfo(string name, ScriptBlock filter, ScopedItemOptions options, ExecutionContext context) : this(name, filter, options, context, null)
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001A6A9 File Offset: 0x000188A9
		internal FilterInfo(string name, ScriptBlock filter, ScopedItemOptions options, ExecutionContext context, string helpFile) : base(name, filter, options, context, helpFile)
		{
			base.SetCommandType(CommandTypes.Filter);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001A6BF File Offset: 0x000188BF
		internal FilterInfo(FilterInfo other) : base(other)
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001A6C8 File Offset: 0x000188C8
		internal FilterInfo(string name, FilterInfo other) : base(name, other)
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001A6D4 File Offset: 0x000188D4
		internal override CommandInfo CreateGetCommandCopy(object[] arguments)
		{
			return new FilterInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = arguments
			};
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001A6F7 File Offset: 0x000188F7
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Filter;
			}
		}
	}
}
