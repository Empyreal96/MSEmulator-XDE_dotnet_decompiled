using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000153 RID: 339
	internal class PSMemberInfoIntegratingCollection<T> : PSMemberInfoCollection<T>, IEnumerable<!0>, IEnumerable where T : PSMemberInfo
	{
		// Token: 0x06001165 RID: 4453 RVA: 0x0005FBB4 File Offset: 0x0005DDB4
		private void GenerateAllReservedMembers()
		{
			if (!this.mshOwner.hasGeneratedReservedMembers)
			{
				this.mshOwner.hasGeneratedReservedMembers = true;
				ReservedNameMembers.GeneratePSExtendedMemberSet(this.mshOwner);
				ReservedNameMembers.GeneratePSBaseMemberSet(this.mshOwner);
				ReservedNameMembers.GeneratePSObjectMemberSet(this.mshOwner);
				ReservedNameMembers.GeneratePSAdaptedMemberSet(this.mshOwner);
				ReservedNameMembers.GeneratePSTypeNames(this.mshOwner);
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0005FC15 File Offset: 0x0005DE15
		internal Collection<CollectionEntry<T>> Collections
		{
			get
			{
				return this.collections;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0005FC20 File Offset: 0x0005DE20
		internal PSMemberInfoIntegratingCollection(object owner, Collection<CollectionEntry<T>> collections)
		{
			if (owner == null)
			{
				throw PSTraceSource.NewArgumentNullException("owner");
			}
			this.mshOwner = (owner as PSObject);
			this.memberSetOwner = (owner as PSMemberSet);
			if (this.mshOwner == null && this.memberSetOwner == null)
			{
				throw PSTraceSource.NewArgumentException("owner");
			}
			if (collections == null)
			{
				throw PSTraceSource.NewArgumentNullException("collections");
			}
			this.collections = collections;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0005FC89 File Offset: 0x0005DE89
		public override void Add(T member)
		{
			this.Add(member, false);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0005FC94 File Offset: 0x0005DE94
		public override void Add(T member, bool preValidated)
		{
			if (member == null)
			{
				throw PSTraceSource.NewArgumentNullException("member");
			}
			if (!preValidated)
			{
				if (member.MemberType == PSMemberTypes.Property || member.MemberType == PSMemberTypes.Method)
				{
					throw new ExtendedTypeSystemException("CannotAddMethodOrProperty", null, ExtendedTypeSystem.CannotAddPropertyOrMethod, new object[0]);
				}
				if (this.memberSetOwner != null && this.memberSetOwner.IsReservedMember)
				{
					throw new ExtendedTypeSystemException("CannotAddToReservedNameMemberset", null, ExtendedTypeSystem.CannotChangeReservedMember, new object[]
					{
						this.memberSetOwner.Name
					});
				}
			}
			this.AddToReservedMemberSet(member, preValidated);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0005FD34 File Offset: 0x0005DF34
		internal void AddToReservedMemberSet(T member, bool preValidated)
		{
			if (!preValidated && this.memberSetOwner != null && !this.memberSetOwner.IsInstance)
			{
				throw new ExtendedTypeSystemException("RemoveMemberFromStaticMemberSet", null, ExtendedTypeSystem.ChangeStaticMember, new object[]
				{
					member.Name
				});
			}
			this.AddToTypesXmlCache(member, preValidated);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0005FD8C File Offset: 0x0005DF8C
		internal void AddToTypesXmlCache(T member, bool preValidated)
		{
			if (member == null)
			{
				throw PSTraceSource.NewArgumentNullException("member");
			}
			if (!preValidated && PSMemberInfoCollection<T>.IsReservedName(member.Name))
			{
				throw new ExtendedTypeSystemException("PSObjectMembersMembersAddReservedName", null, ExtendedTypeSystem.ReservedMemberName, new object[]
				{
					member.Name
				});
			}
			PSMemberInfo psmemberInfo = member.Copy();
			if (this.mshOwner != null)
			{
				if (!preValidated)
				{
					TypeTable typeTable = this.mshOwner.GetTypeTable();
					if (typeTable != null)
					{
						PSMemberInfoInternalCollection<T> members = typeTable.GetMembers<T>(this.mshOwner.InternalTypeNames);
						if (members[member.Name] != null)
						{
							throw new ExtendedTypeSystemException("AlreadyPresentInTypesXml", null, ExtendedTypeSystem.MemberAlreadyPresentFromTypesXml, new object[]
							{
								member.Name
							});
						}
					}
				}
				psmemberInfo.ReplicateInstance(this.mshOwner);
				this.mshOwner.InstanceMembers.Add(psmemberInfo, preValidated);
				PSGetMemberBinder.SetHasInstanceMember(psmemberInfo.Name);
				PSVariableAssignmentBinder.NoteTypeHasInstanceMemberOrTypeName(PSObject.Base(this.mshOwner).GetType());
				return;
			}
			this.memberSetOwner.InternalMembers.Add(psmemberInfo, preValidated);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0005FEC0 File Offset: 0x0005E0C0
		public override void Remove(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (this.mshOwner != null)
			{
				this.mshOwner.InstanceMembers.Remove(name);
				return;
			}
			if (!this.memberSetOwner.IsInstance)
			{
				throw new ExtendedTypeSystemException("AddMemberToStaticMemberSet", null, ExtendedTypeSystem.ChangeStaticMember, new object[]
				{
					name
				});
			}
			if (PSMemberInfoCollection<T>.IsReservedName(this.memberSetOwner.Name))
			{
				throw new ExtendedTypeSystemException("CannotRemoveFromReservedNameMemberset", null, ExtendedTypeSystem.CannotChangeReservedMember, new object[]
				{
					this.memberSetOwner.Name
				});
			}
			this.memberSetOwner.InternalMembers.Remove(name);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0005FF70 File Offset: 0x0005E170
		private void EnsureReservedMemberIsLoaded(string name)
		{
			string a;
			if (name.Length >= 6 && (name[0] == 'p' || name[0] == 'P') && (name[1] == 's' || name[1] == 'S') && (a = name.ToLowerInvariant()) != null)
			{
				if (a == "psbase")
				{
					ReservedNameMembers.GeneratePSBaseMemberSet(this.mshOwner);
					return;
				}
				if (a == "psadapted")
				{
					ReservedNameMembers.GeneratePSAdaptedMemberSet(this.mshOwner);
					return;
				}
				if (a == "psextended")
				{
					ReservedNameMembers.GeneratePSExtendedMemberSet(this.mshOwner);
					return;
				}
				if (a == "psobject")
				{
					ReservedNameMembers.GeneratePSObjectMemberSet(this.mshOwner);
					return;
				}
				if (!(a == "pstypenames"))
				{
					return;
				}
				ReservedNameMembers.GeneratePSTypeNames(this.mshOwner);
			}
		}

		// Token: 0x17000451 RID: 1105
		public override T this[string name]
		{
			get
			{
				T result;
				using (PSObject.memberResolution.TraceScope("Lookup", new object[0]))
				{
					if (string.IsNullOrEmpty(name))
					{
						throw PSTraceSource.NewArgumentException("name");
					}
					object obj;
					if (this.mshOwner != null)
					{
						this.EnsureReservedMemberIsLoaded(name);
						obj = this.mshOwner;
						PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
						if (PSObject.HasInstanceMembers(this.mshOwner, out psmemberInfoInternalCollection))
						{
							PSMemberInfo psmemberInfo = psmemberInfoInternalCollection[name];
							T t = psmemberInfo as T;
							if (t != null)
							{
								PSObject.memberResolution.WriteLine("Found PSObject instance member: {0}.", new object[]
								{
									name
								});
								return t;
							}
						}
					}
					else
					{
						PSMemberInfo psmemberInfo = this.memberSetOwner.InternalMembers[name];
						obj = this.memberSetOwner.instance;
						T t2 = psmemberInfo as T;
						if (t2 != null)
						{
							PSObject.memberResolution.WriteLine("Found PSMemberSet member: {0}.", new object[]
							{
								name
							});
							psmemberInfo.ReplicateInstance(obj);
							return t2;
						}
					}
					if (obj == null)
					{
						result = default(T);
					}
					else
					{
						obj = PSObject.AsPSObject(obj);
						foreach (CollectionEntry<T> collectionEntry in this.collections)
						{
							T t3 = collectionEntry.GetMember((PSObject)obj, name);
							if (t3 != null)
							{
								if (collectionEntry.ShouldCloneWhenReturning)
								{
									t3 = (T)((object)t3.Copy());
								}
								if (collectionEntry.ShouldReplicateWhenReturning)
								{
									t3.ReplicateInstance(obj);
								}
								return t3;
							}
						}
						result = default(T);
					}
				}
				return result;
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00060244 File Offset: 0x0005E444
		private PSMemberInfoInternalCollection<T> GetIntegratedMembers(MshMemberMatchOptions matchOptions)
		{
			PSMemberInfoInternalCollection<T> result;
			using (PSObject.memberResolution.TraceScope("Generating the total list of members", new object[0]))
			{
				PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
				object obj;
				if (this.mshOwner != null)
				{
					obj = this.mshOwner;
					using (IEnumerator<PSMemberInfo> enumerator = this.mshOwner.InstanceMembers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PSMemberInfo psmemberInfo = enumerator.Current;
							if (psmemberInfo.MatchesOptions(matchOptions))
							{
								T t = psmemberInfo as T;
								if (t != null)
								{
									psmemberInfoInternalCollection.Add(t);
								}
							}
						}
						goto IL_F4;
					}
				}
				obj = this.memberSetOwner.instance;
				foreach (PSMemberInfo psmemberInfo2 in this.memberSetOwner.InternalMembers)
				{
					if (psmemberInfo2.MatchesOptions(matchOptions))
					{
						T t2 = psmemberInfo2 as T;
						if (t2 != null)
						{
							psmemberInfo2.ReplicateInstance(obj);
							psmemberInfoInternalCollection.Add(t2);
						}
					}
				}
				IL_F4:
				if (obj == null)
				{
					result = psmemberInfoInternalCollection;
				}
				else
				{
					obj = PSObject.AsPSObject(obj);
					foreach (CollectionEntry<T> collectionEntry in this.collections)
					{
						PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection2 = collectionEntry.GetMembers((PSObject)obj);
						foreach (T t3 in psmemberInfoInternalCollection2)
						{
							PSMemberInfo psmemberInfo3 = psmemberInfoInternalCollection[t3.Name];
							if (psmemberInfo3 != null)
							{
								PSObject.memberResolution.WriteLine("Member \"{0}\" of type \"{1}\" has been ignored because a member with the same name and type \"{2}\" is already present.", new object[]
								{
									t3.Name,
									t3.MemberType,
									psmemberInfo3.MemberType
								});
							}
							else if (!t3.MatchesOptions(matchOptions))
							{
								PSObject.memberResolution.WriteLine("Skipping hidden member \"{0}\".", new object[]
								{
									t3.Name
								});
							}
							else
							{
								T member;
								if (collectionEntry.ShouldCloneWhenReturning)
								{
									member = (T)((object)t3.Copy());
								}
								else
								{
									member = t3;
								}
								if (collectionEntry.ShouldReplicateWhenReturning)
								{
									member.ReplicateInstance(obj);
								}
								psmemberInfoInternalCollection.Add(member);
							}
						}
					}
					result = psmemberInfoInternalCollection;
				}
			}
			return result;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00060550 File Offset: 0x0005E750
		public override ReadOnlyPSMemberInfoCollection<T> Match(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.Match(name, PSMemberTypes.All, MshMemberMatchOptions.None);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00060572 File Offset: 0x0005E772
		public override ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			return this.Match(name, memberTypes, MshMemberMatchOptions.None);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00060590 File Offset: 0x0005E790
		internal override ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes, MshMemberMatchOptions matchOptions)
		{
			ReadOnlyPSMemberInfoCollection<T> result;
			using (PSObject.memberResolution.TraceScope("Matching \"{0}\"", new object[]
			{
				name
			}))
			{
				if (string.IsNullOrEmpty(name))
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				if (this.mshOwner != null)
				{
					this.GenerateAllReservedMembers();
				}
				WildcardPattern namePattern = MemberMatch.GetNamePattern(name);
				PSMemberInfoInternalCollection<T> integratedMembers = this.GetIntegratedMembers(matchOptions);
				ReadOnlyPSMemberInfoCollection<T> readOnlyPSMemberInfoCollection = new ReadOnlyPSMemberInfoCollection<T>(MemberMatch.Match<T>(integratedMembers, name, namePattern, memberTypes));
				PSObject.memberResolution.WriteLine("{0} total matches.", new object[]
				{
					readOnlyPSMemberInfoCollection.Count
				});
				result = readOnlyPSMemberInfoCollection;
			}
			return result;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00060648 File Offset: 0x0005E848
		public override IEnumerator<T> GetEnumerator()
		{
			return new PSMemberInfoIntegratingCollection<T>.Enumerator<T>(this);
		}

		// Token: 0x04000766 RID: 1894
		private Collection<CollectionEntry<T>> collections;

		// Token: 0x04000767 RID: 1895
		private PSObject mshOwner;

		// Token: 0x04000768 RID: 1896
		private PSMemberSet memberSetOwner;

		// Token: 0x02000154 RID: 340
		internal struct Enumerator<S> : IEnumerator<S>, IDisposable, IEnumerator where S : PSMemberInfo
		{
			// Token: 0x06001174 RID: 4468 RVA: 0x00060658 File Offset: 0x0005E858
			internal Enumerator(PSMemberInfoIntegratingCollection<S> integratingCollection)
			{
				using (PSObject.memberResolution.TraceScope("Enumeration Start", new object[0]))
				{
					this.currentIndex = -1;
					this.current = default(S);
					this.allMembers = integratingCollection.GetIntegratedMembers(MshMemberMatchOptions.None);
					if (integratingCollection.mshOwner != null)
					{
						integratingCollection.GenerateAllReservedMembers();
						PSObject.memberResolution.WriteLine("Enumerating PSObject with type \"{0}\".", new object[]
						{
							integratingCollection.mshOwner.ImmediateBaseObject.GetType().FullName
						});
						PSObject.memberResolution.WriteLine("PSObject instance members: {0}", new object[]
						{
							this.allMembers.VisibleCount
						});
					}
					else
					{
						PSObject.memberResolution.WriteLine("Enumerating PSMemberSet \"{0}\".", new object[]
						{
							integratingCollection.memberSetOwner.Name
						});
						PSObject.memberResolution.WriteLine("MemberSet instance members: {0}", new object[]
						{
							this.allMembers.VisibleCount
						});
					}
				}
			}

			// Token: 0x06001175 RID: 4469 RVA: 0x00060774 File Offset: 0x0005E974
			public bool MoveNext()
			{
				this.currentIndex++;
				S s = default(S);
				while (this.currentIndex < this.allMembers.Count)
				{
					s = this.allMembers[this.currentIndex];
					if (!s.IsHidden)
					{
						break;
					}
					this.currentIndex++;
				}
				if (this.currentIndex < this.allMembers.Count)
				{
					this.current = s;
					return true;
				}
				this.current = default(S);
				return false;
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x06001176 RID: 4470 RVA: 0x00060804 File Offset: 0x0005EA04
			S IEnumerator<!1>.Current
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					return this.current;
				}
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x06001177 RID: 4471 RVA: 0x0006081B File Offset: 0x0005EA1B
			object IEnumerator.Current
			{
				get
				{
					return ((IEnumerator<S>)this).Current;
				}
			}

			// Token: 0x06001178 RID: 4472 RVA: 0x00060832 File Offset: 0x0005EA32
			void IEnumerator.Reset()
			{
				this.currentIndex = -1;
				this.current = default(S);
			}

			// Token: 0x06001179 RID: 4473 RVA: 0x00060847 File Offset: 0x0005EA47
			public void Dispose()
			{
			}

			// Token: 0x04000769 RID: 1897
			private S current;

			// Token: 0x0400076A RID: 1898
			private int currentIndex;

			// Token: 0x0400076B RID: 1899
			private PSMemberInfoInternalCollection<S> allMembers;
		}
	}
}
