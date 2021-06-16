using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000141 RID: 321
	public abstract class PSMethodInfo : PSMemberInfo
	{
		// Token: 0x060010CD RID: 4301
		public abstract object Invoke(params object[] arguments);

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060010CE RID: 4302
		public abstract Collection<string> OverloadDefinitions { get; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0005DFDE File Offset: 0x0005C1DE
		// (set) Token: 0x060010D0 RID: 4304 RVA: 0x0005DFE4 File Offset: 0x0005C1E4
		public sealed override object Value
		{
			get
			{
				return this;
			}
			set
			{
				throw new ExtendedTypeSystemException("CannotChangePSMethodInfoValue", null, ExtendedTypeSystem.CannotSetValueForMemberType, new object[]
				{
					base.GetType().FullName
				});
			}
		}
	}
}
