using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200014C RID: 332
	public abstract class PSMemberInfoCollection<T> : IEnumerable<!0>, IEnumerable where T : PSMemberInfo
	{
		// Token: 0x0600112F RID: 4399
		public abstract void Add(T member);

		// Token: 0x06001130 RID: 4400
		public abstract void Add(T member, bool preValidated);

		// Token: 0x06001131 RID: 4401
		public abstract void Remove(string name);

		// Token: 0x17000443 RID: 1091
		public abstract T this[string name]
		{
			get;
		}

		// Token: 0x06001133 RID: 4403
		public abstract ReadOnlyPSMemberInfoCollection<T> Match(string name);

		// Token: 0x06001134 RID: 4404
		public abstract ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes);

		// Token: 0x06001135 RID: 4405
		internal abstract ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes, MshMemberMatchOptions matchOptions);

		// Token: 0x06001136 RID: 4406 RVA: 0x0005F3D4 File Offset: 0x0005D5D4
		internal static bool IsReservedName(string name)
		{
			return string.Equals(name, "psbase", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "psadapted", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "psextended", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "psobject", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "pstypenames", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0005F427 File Offset: 0x0005D627
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001138 RID: 4408
		public abstract IEnumerator<T> GetEnumerator();
	}
}
