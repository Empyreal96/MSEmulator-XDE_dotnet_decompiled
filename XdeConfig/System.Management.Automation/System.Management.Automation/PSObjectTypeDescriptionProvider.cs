using System;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x02000191 RID: 401
	public class PSObjectTypeDescriptionProvider : TypeDescriptionProvider
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600137C RID: 4988 RVA: 0x00078E08 File Offset: 0x00077008
		// (remove) Token: 0x0600137D RID: 4989 RVA: 0x00078E40 File Offset: 0x00077040
		public event EventHandler<SettingValueExceptionEventArgs> SettingValueException;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600137E RID: 4990 RVA: 0x00078E78 File Offset: 0x00077078
		// (remove) Token: 0x0600137F RID: 4991 RVA: 0x00078EB0 File Offset: 0x000770B0
		public event EventHandler<GettingValueExceptionEventArgs> GettingValueException;

		// Token: 0x06001381 RID: 4993 RVA: 0x00078EF0 File Offset: 0x000770F0
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			PSObject instance2 = instance as PSObject;
			PSObjectTypeDescriptor psobjectTypeDescriptor = new PSObjectTypeDescriptor(instance2);
			psobjectTypeDescriptor.SettingValueException += this.SettingValueException;
			psobjectTypeDescriptor.GettingValueException += this.GettingValueException;
			return psobjectTypeDescriptor;
		}
	}
}
