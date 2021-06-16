using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace System.Management.Automation
{
	// Token: 0x0200009C RID: 156
	internal class DscResourceSearcher : IEnumerable<DscResourceInfo>, IEnumerable, IEnumerator<DscResourceInfo>, IDisposable, IEnumerator
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00024A90 File Offset: 0x00022C90
		internal DscResourceSearcher(string resourceName, ExecutionContext context)
		{
			this._resourceName = resourceName;
			this._context = context;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00024AA6 File Offset: 0x00022CA6
		public void Reset()
		{
			this._currentMatch = null;
			this.matchingResource = null;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00024AB6 File Offset: 0x00022CB6
		public void Dispose()
		{
			this.Reset();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00024AC4 File Offset: 0x00022CC4
		IEnumerator<DscResourceInfo> IEnumerable<DscResourceInfo>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00024AC7 File Offset: 0x00022CC7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00024ACA File Offset: 0x00022CCA
		public bool MoveNext()
		{
			this._currentMatch = this.GetNextDscResource();
			return this._currentMatch != null;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00024AE3 File Offset: 0x00022CE3
		DscResourceInfo IEnumerator<DscResourceInfo>.Current
		{
			get
			{
				return this._currentMatch;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00024AEB File Offset: 0x00022CEB
		object IEnumerator.Current
		{
			get
			{
				return ((IEnumerator<DscResourceInfo>)this).Current;
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00024AF4 File Offset: 0x00022CF4
		private DscResourceInfo GetNextDscResource()
		{
			DscResourceInfo result = null;
			PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand("Get-DscResource");
			WildcardPattern wildcardPattern = new WildcardPattern(this._resourceName, WildcardOptions.IgnoreCase);
			if (this.matchingResourceList == null)
			{
				Collection<PSObject> collection = powerShell.Invoke();
				this.matchingResourceList = new Collection<DscResourceInfo>();
				bool flag = false;
				foreach (object arg in collection)
				{
					if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1 == null)
					{
						DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> target = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1.Target;
					CallSite <>p__Site = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1;
					if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site2 == null)
					{
						DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, object, object> target2 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site2.Target;
					CallSite <>p__Site2 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site2;
					if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site3 == null)
					{
						DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (target(<>p__Site, target2(<>p__Site2, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site3.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site3, arg), null)))
					{
						if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site4 == null)
						{
							DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DscResourceSearcher)));
						}
						Func<CallSite, object, string> target3 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site4.Target;
						CallSite <>p__Site3 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site4;
						if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site5 == null)
						{
							DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						string text = target3(<>p__Site3, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site5.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site5, arg));
						if (wildcardPattern.IsMatch(text))
						{
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site6 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site6 = CallSite<Func<CallSite, Type, string, object, object, object, ExecutionContext, DscResourceInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							Func<CallSite, Type, string, object, object, object, ExecutionContext, DscResourceInfo> target4 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site6.Target;
							CallSite <>p__Site4 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site6;
							Type typeFromHandle = typeof(DscResourceInfo);
							string arg2 = text;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site7 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResourceType", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							object arg3 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site7.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site7, arg);
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site8 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Path", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							object arg4 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site8.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site8, arg);
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site9 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ParentPath", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							DscResourceInfo dscResourceInfo = target4(<>p__Site4, typeFromHandle, arg2, arg3, arg4, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site9.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site9, arg), this._context);
							DscResourceInfo dscResourceInfo2 = dscResourceInfo;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitea == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitea = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DscResourceSearcher)));
							}
							Func<CallSite, object, string> target5 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitea.Target;
							CallSite <>p__Sitea = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitea;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Siteb == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Siteb = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FriendlyName", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							dscResourceInfo2.FriendlyName = target5(<>p__Sitea, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Siteb.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Siteb, arg));
							DscResourceInfo dscResourceInfo3 = dscResourceInfo;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitec == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitec = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DscResourceSearcher)));
							}
							Func<CallSite, object, string> target6 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitec.Target;
							CallSite <>p__Sitec = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitec;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sited == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sited = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CompanyName", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							dscResourceInfo3.CompanyName = target6(<>p__Sitec, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sited.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sited, arg));
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitee == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Module", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							PSModuleInfo psmoduleInfo = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitee.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitee, arg) as PSModuleInfo;
							if (psmoduleInfo != null)
							{
								dscResourceInfo.Module = psmoduleInfo;
							}
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitef == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitef = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							Func<CallSite, object, bool> target7 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitef.Target;
							CallSite <>p__Sitef = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Sitef;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site10 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
								}));
							}
							Func<CallSite, object, object, object> target8 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site10.Target;
							CallSite <>p__Site5 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site10;
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site11 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImplementedAs", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							if (target7(<>p__Sitef, target8(<>p__Site5, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site11.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site11, arg), null)))
							{
								if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site12 == null)
								{
									DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								Func<CallSite, object, bool> target9 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site12.Target;
								CallSite <>p__Site6 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site12;
								if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site14 == null)
								{
									DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site14 = CallSite<DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>q__SiteDelegate13>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryParse", new Type[]
									{
										typeof(ImplementedAsType)
									}, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, null)
									}));
								}
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>q__SiteDelegate13 target10 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site14.Target;
								CallSite <>p__Site7 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site14;
								Type typeFromHandle2 = typeof(Enum);
								if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site15 == null)
								{
									DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								Func<CallSite, object, object> target11 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site15.Target;
								CallSite <>p__Site8 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site15;
								if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site16 == null)
								{
									DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImplementedAs", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								ImplementedAsType implementedAs;
								if (target9(<>p__Site6, target10(<>p__Site7, typeFromHandle2, target11(<>p__Site8, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site16.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site16, arg)), out implementedAs)))
								{
									dscResourceInfo.ImplementedAs = implementedAs;
								}
							}
							if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site17 == null)
							{
								DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Properties", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
								}));
							}
							IList list = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site17.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site17, arg) as IList;
							if (list != null)
							{
								List<DscResourcePropertyInfo> list2 = new List<DscResourcePropertyInfo>();
								foreach (object arg5 in list)
								{
									DscResourcePropertyInfo dscResourcePropertyInfo = new DscResourcePropertyInfo();
									DscResourcePropertyInfo dscResourcePropertyInfo2 = dscResourcePropertyInfo;
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site18 == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DscResourceSearcher)));
									}
									Func<CallSite, object, string> target12 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site18.Target;
									CallSite <>p__Site9 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site18;
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site19 == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									dscResourcePropertyInfo2.Name = target12(<>p__Site9, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site19.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site19, arg5));
									DscResourcePropertyInfo dscResourcePropertyInfo3 = dscResourcePropertyInfo;
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1a == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1a = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DscResourceSearcher)));
									}
									Func<CallSite, object, string> target13 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1a.Target;
									CallSite <>p__Site1a = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1a;
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1b == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1b = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PropertyType", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									dscResourcePropertyInfo3.PropertyType = target13(<>p__Site1a, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1b.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1b, arg5));
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1c == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1c = CallSite<Action<CallSite, DscResourcePropertyInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "UpdateValues", null, typeof(DscResourceSearcher), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									Action<CallSite, DscResourcePropertyInfo, object> target14 = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1c.Target;
									CallSite <>p__Site1c = DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1c;
									DscResourcePropertyInfo arg6 = dscResourcePropertyInfo;
									if (DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1d == null)
									{
										DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1d = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Values", typeof(DscResourceSearcher), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
										}));
									}
									target14(<>p__Site1c, arg6, DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1d.Target(DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>p__Site1d, arg5));
									list2.Add(dscResourcePropertyInfo);
								}
								dscResourceInfo.UpdateProperties(list2);
							}
							this.matchingResourceList.Add(dscResourceInfo);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					return result;
				}
				this.matchingResource = this.matchingResourceList.GetEnumerator();
			}
			if (!this.matchingResource.MoveNext())
			{
				this.matchingResource = null;
			}
			else
			{
				result = this.matchingResource.Current;
			}
			return result;
		}

		// Token: 0x0400036E RID: 878
		private string _resourceName;

		// Token: 0x0400036F RID: 879
		private ExecutionContext _context;

		// Token: 0x04000370 RID: 880
		private DscResourceInfo _currentMatch;

		// Token: 0x04000371 RID: 881
		private IEnumerator<DscResourceInfo> matchingResource;

		// Token: 0x04000372 RID: 882
		private Collection<DscResourceInfo> matchingResourceList;

		// Token: 0x02000AAC RID: 2732
		[CompilerGenerated]
		private static class <GetNextDscResource>o__SiteContainer0
		{
			// Token: 0x040033DB RID: 13275
			public static CallSite<Func<CallSite, object, bool>> <>p__Site1;

			// Token: 0x040033DC RID: 13276
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site2;

			// Token: 0x040033DD RID: 13277
			public static CallSite<Func<CallSite, object, object>> <>p__Site3;

			// Token: 0x040033DE RID: 13278
			public static CallSite<Func<CallSite, object, string>> <>p__Site4;

			// Token: 0x040033DF RID: 13279
			public static CallSite<Func<CallSite, object, object>> <>p__Site5;

			// Token: 0x040033E0 RID: 13280
			public static CallSite<Func<CallSite, Type, string, object, object, object, ExecutionContext, DscResourceInfo>> <>p__Site6;

			// Token: 0x040033E1 RID: 13281
			public static CallSite<Func<CallSite, object, object>> <>p__Site7;

			// Token: 0x040033E2 RID: 13282
			public static CallSite<Func<CallSite, object, object>> <>p__Site8;

			// Token: 0x040033E3 RID: 13283
			public static CallSite<Func<CallSite, object, object>> <>p__Site9;

			// Token: 0x040033E4 RID: 13284
			public static CallSite<Func<CallSite, object, string>> <>p__Sitea;

			// Token: 0x040033E5 RID: 13285
			public static CallSite<Func<CallSite, object, object>> <>p__Siteb;

			// Token: 0x040033E6 RID: 13286
			public static CallSite<Func<CallSite, object, string>> <>p__Sitec;

			// Token: 0x040033E7 RID: 13287
			public static CallSite<Func<CallSite, object, object>> <>p__Sited;

			// Token: 0x040033E8 RID: 13288
			public static CallSite<Func<CallSite, object, object>> <>p__Sitee;

			// Token: 0x040033E9 RID: 13289
			public static CallSite<Func<CallSite, object, bool>> <>p__Sitef;

			// Token: 0x040033EA RID: 13290
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site10;

			// Token: 0x040033EB RID: 13291
			public static CallSite<Func<CallSite, object, object>> <>p__Site11;

			// Token: 0x040033EC RID: 13292
			public static CallSite<Func<CallSite, object, bool>> <>p__Site12;

			// Token: 0x040033ED RID: 13293
			public static CallSite<DscResourceSearcher.<GetNextDscResource>o__SiteContainer0.<>q__SiteDelegate13> <>p__Site14;

			// Token: 0x040033EE RID: 13294
			public static CallSite<Func<CallSite, object, object>> <>p__Site15;

			// Token: 0x040033EF RID: 13295
			public static CallSite<Func<CallSite, object, object>> <>p__Site16;

			// Token: 0x040033F0 RID: 13296
			public static CallSite<Func<CallSite, object, object>> <>p__Site17;

			// Token: 0x040033F1 RID: 13297
			public static CallSite<Func<CallSite, object, string>> <>p__Site18;

			// Token: 0x040033F2 RID: 13298
			public static CallSite<Func<CallSite, object, object>> <>p__Site19;

			// Token: 0x040033F3 RID: 13299
			public static CallSite<Func<CallSite, object, string>> <>p__Site1a;

			// Token: 0x040033F4 RID: 13300
			public static CallSite<Func<CallSite, object, object>> <>p__Site1b;

			// Token: 0x040033F5 RID: 13301
			public static CallSite<Action<CallSite, DscResourcePropertyInfo, object>> <>p__Site1c;

			// Token: 0x040033F6 RID: 13302
			public static CallSite<Func<CallSite, object, object>> <>p__Site1d;

			// Token: 0x02000AAD RID: 2733
			// (Invoke) Token: 0x06006C01 RID: 27649
			public delegate object <>q__SiteDelegate13(CallSite param0, Type param1, dynamic param2, out ImplementedAsType param3);
		}
	}
}
