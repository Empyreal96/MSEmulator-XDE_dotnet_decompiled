using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000146 RID: 326
	public class PSMemberSet : PSMemberInfo
	{
		// Token: 0x060010FD RID: 4349 RVA: 0x0005E810 File Offset: 0x0005CA10
		internal override void ReplicateInstance(object particularInstance)
		{
			base.ReplicateInstance(particularInstance);
			foreach (PSMemberInfo psmemberInfo in this.Members)
			{
				psmemberInfo.ReplicateInstance(particularInstance);
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0005E864 File Offset: 0x0005CA64
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" {");
			foreach (PSMemberInfo psmemberInfo in this.Members)
			{
				stringBuilder.Append(psmemberInfo.Name);
				stringBuilder.Append(", ");
			}
			if (stringBuilder.Length > 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			stringBuilder.Insert(0, base.Name);
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0005E910 File Offset: 0x0005CB10
		public PSMemberSet(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.internalMembers = new PSMemberInfoInternalCollection<PSMemberInfo>();
			this.members = new PSMemberInfoIntegratingCollection<PSMemberInfo>(this, PSMemberSet.emptyMemberCollection);
			this.properties = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(this, PSMemberSet.emptyPropertyCollection);
			this.methods = new PSMemberInfoIntegratingCollection<PSMethodInfo>(this, PSMemberSet.emptyMethodCollection);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0005E984 File Offset: 0x0005CB84
		public PSMemberSet(string name, IEnumerable<PSMemberInfo> members)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (members == null)
			{
				throw PSTraceSource.NewArgumentNullException("members");
			}
			this.internalMembers = new PSMemberInfoInternalCollection<PSMemberInfo>();
			foreach (PSMemberInfo psmemberInfo in members)
			{
				if (psmemberInfo == null)
				{
					throw PSTraceSource.NewArgumentNullException("members");
				}
				this.internalMembers.Add(psmemberInfo.Copy());
			}
			this.members = new PSMemberInfoIntegratingCollection<PSMemberInfo>(this, PSMemberSet.emptyMemberCollection);
			this.properties = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(this, PSMemberSet.emptyPropertyCollection);
			this.methods = new PSMemberInfoIntegratingCollection<PSMethodInfo>(this, PSMemberSet.emptyMethodCollection);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0005EA58 File Offset: 0x0005CC58
		private static Collection<CollectionEntry<PSMemberInfo>> GetTypeMemberCollection()
		{
			return new Collection<CollectionEntry<PSMemberInfo>>
			{
				new CollectionEntry<PSMemberInfo>(new CollectionEntry<PSMemberInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSMemberInfo>), new CollectionEntry<PSMemberInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSMemberInfo>), true, true, "type table members")
			};
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0005EA98 File Offset: 0x0005CC98
		private static Collection<CollectionEntry<PSMethodInfo>> GetTypeMethodCollection()
		{
			return new Collection<CollectionEntry<PSMethodInfo>>
			{
				new CollectionEntry<PSMethodInfo>(new CollectionEntry<PSMethodInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSMethodInfo>), new CollectionEntry<PSMethodInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSMethodInfo>), true, true, "type table members")
			};
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0005EAD8 File Offset: 0x0005CCD8
		private static Collection<CollectionEntry<PSPropertyInfo>> GetTypePropertyCollection()
		{
			return new Collection<CollectionEntry<PSPropertyInfo>>
			{
				new CollectionEntry<PSPropertyInfo>(new CollectionEntry<PSPropertyInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSPropertyInfo>), new CollectionEntry<PSPropertyInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSPropertyInfo>), true, true, "type table members")
			};
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0005EB18 File Offset: 0x0005CD18
		internal PSMemberSet(string name, PSObject mshObject)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (mshObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("mshObject");
			}
			this.constructorPSObject = mshObject;
			this.internalMembers = mshObject.InstanceMembers;
			this.members = new PSMemberInfoIntegratingCollection<PSMemberInfo>(this, PSMemberSet.typeMemberCollection);
			this.properties = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(this, PSMemberSet.typePropertyCollection);
			this.methods = new PSMemberInfoIntegratingCollection<PSMethodInfo>(this, PSMemberSet.typeMethodCollection);
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0005EBA0 File Offset: 0x0005CDA0
		public bool InheritMembers
		{
			get
			{
				return this.inheritMembers;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0005EBA8 File Offset: 0x0005CDA8
		internal virtual PSMemberInfoInternalCollection<PSMemberInfo> InternalMembers
		{
			get
			{
				return this.internalMembers;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0005EBB0 File Offset: 0x0005CDB0
		public PSMemberInfoCollection<PSMemberInfo> Members
		{
			get
			{
				return this.members;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x0005EBB8 File Offset: 0x0005CDB8
		public PSMemberInfoCollection<PSPropertyInfo> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0005EBC0 File Offset: 0x0005CDC0
		public PSMemberInfoCollection<PSMethodInfo> Methods
		{
			get
			{
				return this.methods;
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0005EBC8 File Offset: 0x0005CDC8
		public override PSMemberInfo Copy()
		{
			if (this.constructorPSObject == null)
			{
				PSMemberSet psmemberSet = new PSMemberSet(this.name);
				foreach (PSMemberInfo member in this.Members)
				{
					psmemberSet.Members.Add(member);
				}
				base.CloneBaseProperties(psmemberSet);
				return psmemberSet;
			}
			return new PSMemberSet(this.name, this.constructorPSObject);
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0005EC48 File Offset: 0x0005CE48
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.MemberSet;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0005EC4F File Offset: 0x0005CE4F
		// (set) Token: 0x0600110D RID: 4365 RVA: 0x0005EC54 File Offset: 0x0005CE54
		public override object Value
		{
			get
			{
				return this;
			}
			set
			{
				throw new ExtendedTypeSystemException("CannotChangePSMemberSetValue", null, ExtendedTypeSystem.CannotSetValueForMemberType, new object[]
				{
					base.GetType().FullName
				});
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0005EC87 File Offset: 0x0005CE87
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(PSMemberSet).FullName;
			}
		}

		// Token: 0x0400074E RID: 1870
		private PSMemberInfoIntegratingCollection<PSMemberInfo> members;

		// Token: 0x0400074F RID: 1871
		private PSMemberInfoIntegratingCollection<PSPropertyInfo> properties;

		// Token: 0x04000750 RID: 1872
		private PSMemberInfoIntegratingCollection<PSMethodInfo> methods;

		// Token: 0x04000751 RID: 1873
		internal PSMemberInfoInternalCollection<PSMemberInfo> internalMembers;

		// Token: 0x04000752 RID: 1874
		private PSObject constructorPSObject;

		// Token: 0x04000753 RID: 1875
		private static Collection<CollectionEntry<PSMemberInfo>> emptyMemberCollection = new Collection<CollectionEntry<PSMemberInfo>>();

		// Token: 0x04000754 RID: 1876
		private static Collection<CollectionEntry<PSMethodInfo>> emptyMethodCollection = new Collection<CollectionEntry<PSMethodInfo>>();

		// Token: 0x04000755 RID: 1877
		private static Collection<CollectionEntry<PSPropertyInfo>> emptyPropertyCollection = new Collection<CollectionEntry<PSPropertyInfo>>();

		// Token: 0x04000756 RID: 1878
		private static Collection<CollectionEntry<PSMemberInfo>> typeMemberCollection = PSMemberSet.GetTypeMemberCollection();

		// Token: 0x04000757 RID: 1879
		private static Collection<CollectionEntry<PSMethodInfo>> typeMethodCollection = PSMemberSet.GetTypeMethodCollection();

		// Token: 0x04000758 RID: 1880
		private static Collection<CollectionEntry<PSPropertyInfo>> typePropertyCollection = PSMemberSet.GetTypePropertyCollection();

		// Token: 0x04000759 RID: 1881
		internal bool inheritMembers = true;
	}
}
