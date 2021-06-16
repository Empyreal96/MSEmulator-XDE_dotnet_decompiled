using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200014D RID: 333
	public class ReadOnlyPSMemberInfoCollection<T> : IEnumerable<!0>, IEnumerable where T : PSMemberInfo
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x0005F42F File Offset: 0x0005D62F
		internal ReadOnlyPSMemberInfoCollection(PSMemberInfoInternalCollection<T> members)
		{
			if (members == null)
			{
				throw PSTraceSource.NewArgumentNullException("members");
			}
			this.members = members;
		}

		// Token: 0x17000444 RID: 1092
		public T this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				return this.members[name];
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0005F46D File Offset: 0x0005D66D
		public ReadOnlyPSMemberInfoCollection<T> Match(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.members.Match(name);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0005F48E File Offset: 0x0005D68E
		public ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.members.Match(name, memberTypes);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0005F4B8 File Offset: 0x0005D6B8
		public virtual IEnumerator<T> GetEnumerator()
		{
			return this.members.GetEnumerator();
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x0005F4C5 File Offset: 0x0005D6C5
		public int Count
		{
			get
			{
				return this.members.Count;
			}
		}

		// Token: 0x17000446 RID: 1094
		public T this[int index]
		{
			get
			{
				return this.members[index];
			}
		}

		// Token: 0x0400075E RID: 1886
		private PSMemberInfoInternalCollection<T> members;
	}
}
