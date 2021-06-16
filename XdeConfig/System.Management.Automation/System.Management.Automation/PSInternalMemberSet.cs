using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000147 RID: 327
	internal class PSInternalMemberSet : PSMemberSet
	{
		// Token: 0x06001110 RID: 4368 RVA: 0x0005ECD6 File Offset: 0x0005CED6
		internal PSInternalMemberSet(string propertyName, PSObject psObject) : base(propertyName)
		{
			this.internalMembers = null;
			this.psObject = psObject;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0005ECF8 File Offset: 0x0005CEF8
		internal override PSMemberInfoInternalCollection<PSMemberInfo> InternalMembers
		{
			get
			{
				if (this.name.Equals("psadapted", StringComparison.OrdinalIgnoreCase))
				{
					return this.GetInternalMembersFromAdapted();
				}
				if (this.internalMembers == null)
				{
					lock (this.syncObject)
					{
						if (this.internalMembers == null)
						{
							this.internalMembers = new PSMemberInfoInternalCollection<PSMemberInfo>();
							string a;
							if ((a = this.name.ToLowerInvariant()) != null)
							{
								if (!(a == "psbase"))
								{
									if (a == "psobject")
									{
										this.GenerateInternalMembersFromPSObject();
									}
								}
								else
								{
									this.GenerateInternalMembersFromBase();
								}
							}
						}
					}
				}
				return this.internalMembers;
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0005EDA8 File Offset: 0x0005CFA8
		private void GenerateInternalMembersFromBase()
		{
			if (this.psObject.isDeserialized)
			{
				if (this.psObject.clrMembers == null)
				{
					return;
				}
				using (IEnumerator<PSPropertyInfo> enumerator = this.psObject.clrMembers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSMemberInfo psmemberInfo = enumerator.Current;
						this.internalMembers.Add(psmemberInfo.Copy());
					}
					return;
				}
			}
			foreach (PSMemberInfo psmemberInfo2 in PSObject.dotNetInstanceAdapter.BaseGetMembers<PSMemberInfo>(this.psObject.ImmediateBaseObject))
			{
				this.internalMembers.Add(psmemberInfo2.Copy());
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005EE78 File Offset: 0x0005D078
		private PSMemberInfoInternalCollection<PSMemberInfo> GetInternalMembersFromAdapted()
		{
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
			if (this.psObject.isDeserialized)
			{
				if (this.psObject.adaptedMembers == null)
				{
					return psmemberInfoInternalCollection;
				}
				using (IEnumerator<PSPropertyInfo> enumerator = this.psObject.adaptedMembers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSMemberInfo psmemberInfo = enumerator.Current;
						psmemberInfoInternalCollection.Add(psmemberInfo.Copy());
					}
					return psmemberInfoInternalCollection;
				}
			}
			foreach (PSMemberInfo psmemberInfo2 in this.psObject.InternalAdapter.BaseGetMembers<PSMemberInfo>(this.psObject.ImmediateBaseObject))
			{
				psmemberInfoInternalCollection.Add(psmemberInfo2.Copy());
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0005EF50 File Offset: 0x0005D150
		private void GenerateInternalMembersFromPSObject()
		{
			PSMemberInfoCollection<PSMemberInfo> psmemberInfoCollection = PSObject.dotNetInstanceAdapter.BaseGetMembers<PSMemberInfo>(this.psObject);
			foreach (PSMemberInfo psmemberInfo in psmemberInfoCollection)
			{
				this.internalMembers.Add(psmemberInfo.Copy());
			}
		}

		// Token: 0x0400075A RID: 1882
		private object syncObject = new object();

		// Token: 0x0400075B RID: 1883
		private PSObject psObject;
	}
}
