using System;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation
{
	// Token: 0x0200017F RID: 383
	internal class ComMethodInformation : MethodInformation
	{
		// Token: 0x060012D1 RID: 4817 RVA: 0x00075023 File Offset: 0x00073223
		internal ComMethodInformation(bool hasvarargs, bool hasoptional, ParameterInformation[] arguments, Type returnType, int dispId, INVOKEKIND invokekind) : base(hasvarargs, hasoptional, arguments)
		{
			this.ReturnType = returnType;
			this.DispId = dispId;
			this.InvokeKind = invokekind;
		}

		// Token: 0x04000821 RID: 2081
		internal readonly Type ReturnType;

		// Token: 0x04000822 RID: 2082
		internal readonly int DispId;

		// Token: 0x04000823 RID: 2083
		internal readonly INVOKEKIND InvokeKind;
	}
}
