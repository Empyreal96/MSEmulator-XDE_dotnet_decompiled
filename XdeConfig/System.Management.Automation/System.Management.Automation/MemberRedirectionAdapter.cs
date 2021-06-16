using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x0200010E RID: 270
	internal abstract class MemberRedirectionAdapter : Adapter
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x00050F8F File Offset: 0x0004F18F
		protected override AttributeCollection PropertyAttributes(PSProperty property)
		{
			return new AttributeCollection(new Attribute[0]);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00050F9C File Offset: 0x0004F19C
		protected override object PropertyGet(PSProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00050FA3 File Offset: 0x0004F1A3
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00050FAA File Offset: 0x0004F1AA
		protected override bool PropertyIsSettable(PSProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00050FB1 File Offset: 0x0004F1B1
		protected override bool PropertyIsGettable(PSProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00050FB8 File Offset: 0x0004F1B8
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00050FBF File Offset: 0x0004F1BF
		protected override string PropertyToString(PSProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00050FC6 File Offset: 0x0004F1C6
		protected override object MethodInvoke(PSMethod method, object[] arguments)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00050FCD File Offset: 0x0004F1CD
		protected override Collection<string> MethodDefinitions(PSMethod method)
		{
			throw PSTraceSource.NewNotSupportedException();
		}
	}
}
