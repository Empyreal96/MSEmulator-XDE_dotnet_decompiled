using System;

namespace System.Management.Automation
{
	// Token: 0x02000138 RID: 312
	public abstract class PSPropertyInfo : PSMemberInfo
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001068 RID: 4200
		public abstract bool IsSettable { get; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001069 RID: 4201
		public abstract bool IsGettable { get; }

		// Token: 0x0600106A RID: 4202 RVA: 0x0005C8D8 File Offset: 0x0005AAD8
		internal Exception NewSetValueException(Exception e, string errorId)
		{
			return new SetValueInvocationException(errorId, e, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
			{
				base.Name,
				e.Message
			});
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0005C90C File Offset: 0x0005AB0C
		internal Exception NewGetValueException(Exception e, string errorId)
		{
			return new GetValueInvocationException(errorId, e, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
			{
				base.Name,
				e.Message
			});
		}
	}
}
