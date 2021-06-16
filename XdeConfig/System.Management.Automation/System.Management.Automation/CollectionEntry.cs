using System;

namespace System.Management.Automation
{
	// Token: 0x0200014F RID: 335
	internal class CollectionEntry<T> where T : PSMemberInfo
	{
		// Token: 0x06001150 RID: 4432 RVA: 0x0005FA08 File Offset: 0x0005DC08
		internal CollectionEntry(CollectionEntry<T>.GetMembersDelegate getMembers, CollectionEntry<T>.GetMemberDelegate getMember, bool shouldReplicateWhenReturning, bool shouldCloneWhenReturning, string collectionNameForTracing)
		{
			this.getMembers = getMembers;
			this.getMember = getMember;
			this.shouldReplicateWhenReturning = shouldReplicateWhenReturning;
			this.shouldCloneWhenReturning = shouldCloneWhenReturning;
			this.collectionNameForTracing = collectionNameForTracing;
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0005FA35 File Offset: 0x0005DC35
		internal CollectionEntry<T>.GetMembersDelegate GetMembers
		{
			get
			{
				return this.getMembers;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x0005FA3D File Offset: 0x0005DC3D
		internal CollectionEntry<T>.GetMemberDelegate GetMember
		{
			get
			{
				return this.getMember;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0005FA45 File Offset: 0x0005DC45
		internal bool ShouldReplicateWhenReturning
		{
			get
			{
				return this.shouldReplicateWhenReturning;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x0005FA4D File Offset: 0x0005DC4D
		internal bool ShouldCloneWhenReturning
		{
			get
			{
				return this.shouldCloneWhenReturning;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0005FA55 File Offset: 0x0005DC55
		internal string CollectionNameForTracing
		{
			get
			{
				return this.collectionNameForTracing;
			}
		}

		// Token: 0x04000761 RID: 1889
		private CollectionEntry<T>.GetMembersDelegate getMembers;

		// Token: 0x04000762 RID: 1890
		private CollectionEntry<T>.GetMemberDelegate getMember;

		// Token: 0x04000763 RID: 1891
		private bool shouldReplicateWhenReturning;

		// Token: 0x04000764 RID: 1892
		private bool shouldCloneWhenReturning;

		// Token: 0x04000765 RID: 1893
		private string collectionNameForTracing;

		// Token: 0x02000150 RID: 336
		// (Invoke) Token: 0x06001157 RID: 4439
		internal delegate PSMemberInfoInternalCollection<T> GetMembersDelegate(PSObject obj);

		// Token: 0x02000151 RID: 337
		// (Invoke) Token: 0x0600115B RID: 4443
		internal delegate T GetMemberDelegate(PSObject obj, string name);
	}
}
