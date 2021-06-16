using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200008B RID: 139
	internal class NativeCommandParameterBinderController : ParameterBinderController
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0002236B File Offset: 0x0002056B
		internal NativeCommandParameterBinderController(NativeCommand command) : base(command.MyInvocation, command.Context, new NativeCommandParameterBinder(command))
		{
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00022385 File Offset: 0x00020585
		internal string Arguments
		{
			get
			{
				return ((NativeCommandParameterBinder)base.DefaultParameterBinder).Arguments;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00022397 File Offset: 0x00020597
		internal override bool BindParameter(CommandParameterInternal argument, ParameterBindingFlags flags)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002239E File Offset: 0x0002059E
		internal override Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> parameters)
		{
			((NativeCommandParameterBinder)base.DefaultParameterBinder).BindParameters(parameters);
			return NativeCommandParameterBinderController.emptyReturnCollection;
		}

		// Token: 0x04000300 RID: 768
		private static readonly Collection<CommandParameterInternal> emptyReturnCollection = new Collection<CommandParameterInternal>();
	}
}
