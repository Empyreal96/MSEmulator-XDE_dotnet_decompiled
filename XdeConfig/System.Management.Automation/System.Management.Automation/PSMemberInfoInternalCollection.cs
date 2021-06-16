using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace System.Management.Automation
{
	// Token: 0x0200014E RID: 334
	internal class PSMemberInfoInternalCollection<T> : PSMemberInfoCollection<T>, IEnumerable<!0>, IEnumerable where T : PSMemberInfo
	{
		// Token: 0x06001141 RID: 4417 RVA: 0x0005F4E0 File Offset: 0x0005D6E0
		internal PSMemberInfoInternalCollection()
		{
			this.members = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0005F4F8 File Offset: 0x0005D6F8
		private void Replace(T oldMember, T newMember)
		{
			this.members[newMember.Name] = newMember;
			if (oldMember.IsHidden)
			{
				this.countHidden--;
			}
			if (newMember.IsHidden)
			{
				this.countHidden++;
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0005F560 File Offset: 0x0005D760
		internal void Replace(T newMember)
		{
			lock (this.members)
			{
				T oldMember = this.members[newMember.Name] as T;
				this.Replace(oldMember, newMember);
			}
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0005F5C8 File Offset: 0x0005D7C8
		public override void Add(T member)
		{
			this.Add(member, false);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0005F5D4 File Offset: 0x0005D7D4
		public override void Add(T member, bool preValidated)
		{
			if (member == null)
			{
				throw PSTraceSource.NewArgumentNullException("member");
			}
			lock (this.members)
			{
				T t = this.members[member.Name] as T;
				if (t != null)
				{
					this.Replace(t, member);
				}
				else
				{
					this.members[member.Name] = member;
					if (member.IsHidden)
					{
						this.countHidden++;
					}
				}
			}
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0005F694 File Offset: 0x0005D894
		public override void Remove(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (PSMemberInfoCollection<T>.IsReservedName(name))
			{
				throw new ExtendedTypeSystemException("PSMemberInfoInternalCollectionRemoveReservedName", null, ExtendedTypeSystem.ReservedMemberName, new object[]
				{
					name
				});
			}
			lock (this.members)
			{
				PSMemberInfo psmemberInfo = this.members[name] as PSMemberInfo;
				if (psmemberInfo != null)
				{
					if (psmemberInfo.IsHidden)
					{
						this.countHidden--;
					}
					this.members.Remove(name);
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		public override T this[string name]
		{
			get
			{
				if (string.IsNullOrEmpty(name))
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				T result;
				lock (this.members)
				{
					result = (this.members[name] as T);
				}
				return result;
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0005F7A8 File Offset: 0x0005D9A8
		public override ReadOnlyPSMemberInfoCollection<T> Match(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.Match(name, PSMemberTypes.All, MshMemberMatchOptions.None);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0005F7CA File Offset: 0x0005D9CA
		public override ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.Match(name, memberTypes, MshMemberMatchOptions.None);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0005F7E8 File Offset: 0x0005D9E8
		internal override ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes, MshMemberMatchOptions matchOptions)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			PSMemberInfoInternalCollection<T> internalMembers = this.GetInternalMembers(matchOptions);
			return new ReadOnlyPSMemberInfoCollection<T>(MemberMatch.Match<T>(internalMembers, name, MemberMatch.GetNamePattern(name), memberTypes));
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0005F824 File Offset: 0x0005DA24
		private PSMemberInfoInternalCollection<T> GetInternalMembers(MshMemberMatchOptions matchOptions)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			lock (this.members)
			{
				foreach (T member in this.members.Values.OfType<T>())
				{
					if (member.MatchesOptions(matchOptions))
					{
						psmemberInfoInternalCollection.Add(member);
					}
				}
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x0005F8C0 File Offset: 0x0005DAC0
		internal int Count
		{
			get
			{
				int count;
				lock (this.members)
				{
					count = this.members.Count;
				}
				return count;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x0005F908 File Offset: 0x0005DB08
		internal int VisibleCount
		{
			get
			{
				int result;
				lock (this.members)
				{
					result = this.members.Count - this.countHidden;
				}
				return result;
			}
		}

		// Token: 0x1700044A RID: 1098
		internal T this[int index]
		{
			get
			{
				T result;
				lock (this.members)
				{
					result = (this.members[index] as T);
				}
				return result;
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0005F9AC File Offset: 0x0005DBAC
		public override IEnumerator<T> GetEnumerator()
		{
			IEnumerator<T> result;
			lock (this.members)
			{
				result = this.members.Values.OfType<T>().ToList<T>().GetEnumerator();
			}
			return result;
		}

		// Token: 0x0400075F RID: 1887
		private readonly OrderedDictionary members;

		// Token: 0x04000760 RID: 1888
		private int countHidden;
	}
}
