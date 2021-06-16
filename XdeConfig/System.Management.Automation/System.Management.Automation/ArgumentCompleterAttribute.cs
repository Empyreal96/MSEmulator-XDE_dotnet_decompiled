using System;
using System.Linq;

namespace System.Management.Automation
{
	// Token: 0x0200098D RID: 2445
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ArgumentCompleterAttribute : Attribute
	{
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06005A57 RID: 23127 RVA: 0x001E5DAC File Offset: 0x001E3FAC
		// (set) Token: 0x06005A58 RID: 23128 RVA: 0x001E5DB4 File Offset: 0x001E3FB4
		public Type Type { get; private set; }

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06005A59 RID: 23129 RVA: 0x001E5DBD File Offset: 0x001E3FBD
		// (set) Token: 0x06005A5A RID: 23130 RVA: 0x001E5DC5 File Offset: 0x001E3FC5
		public ScriptBlock ScriptBlock { get; private set; }

		// Token: 0x06005A5B RID: 23131 RVA: 0x001E5DE0 File Offset: 0x001E3FE0
		public ArgumentCompleterAttribute(Type type)
		{
			if (!(type == null))
			{
				if (!type.GetInterfaces().All((Type t) => t != typeof(IArgumentCompleter)))
				{
					this.Type = type;
					return;
				}
			}
			throw PSTraceSource.NewArgumentException("type");
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x001E5E38 File Offset: 0x001E4038
		public ArgumentCompleterAttribute(ScriptBlock scriptBlock)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			this.ScriptBlock = scriptBlock;
		}
	}
}
