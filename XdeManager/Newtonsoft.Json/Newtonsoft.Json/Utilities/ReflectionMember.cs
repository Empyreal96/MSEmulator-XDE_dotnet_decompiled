using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005F RID: 95
	internal class ReflectionMember
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00017E22 File Offset: 0x00016022
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00017E2A File Offset: 0x0001602A
		public Type MemberType { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00017E33 File Offset: 0x00016033
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00017E3B File Offset: 0x0001603B
		public Func<object, object> Getter { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00017E44 File Offset: 0x00016044
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00017E4C File Offset: 0x0001604C
		public Action<object, object> Setter { get; set; }
	}
}
