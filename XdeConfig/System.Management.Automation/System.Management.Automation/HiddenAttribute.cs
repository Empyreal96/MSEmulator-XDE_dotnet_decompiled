using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200041E RID: 1054
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class HiddenAttribute : ParsingBaseAttribute
	{
	}
}
