using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A59 RID: 2649
	internal sealed class ComEventSink : MarshalByRefObject, IReflect, IDisposable
	{
		// Token: 0x060069C6 RID: 27078 RVA: 0x002148B4 File Offset: 0x00212AB4
		private ComEventSink(object rcw, Guid sourceIid)
		{
			this.Initialize(rcw, sourceIid);
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x002148D0 File Offset: 0x00212AD0
		private void Initialize(object rcw, Guid sourceIid)
		{
			this._sourceIid = sourceIid;
			this._adviseCookie = -1;
			IConnectionPointContainer connectionPointContainer = rcw as IConnectionPointContainer;
			if (connectionPointContainer == null)
			{
				throw Error.COMObjectDoesNotSupportEvents();
			}
			connectionPointContainer.FindConnectionPoint(ref this._sourceIid, out this._connectionPoint);
			if (this._connectionPoint == null)
			{
				throw Error.COMObjectDoesNotSupportSourceInterface();
			}
			ComEventSinkProxy comEventSinkProxy = new ComEventSinkProxy(this, this._sourceIid);
			this._connectionPoint.Advise(comEventSinkProxy.GetTransparentProxy(), out this._adviseCookie);
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x00214940 File Offset: 0x00212B40
		public static ComEventSink FromRuntimeCallableWrapper(object rcw, Guid sourceIid, bool createIfNotFound)
		{
			List<ComEventSink> list = ComEventSinksContainer.FromRuntimeCallableWrapper(rcw, createIfNotFound);
			if (list == null)
			{
				return null;
			}
			ComEventSink comEventSink = null;
			lock (list)
			{
				foreach (ComEventSink comEventSink2 in list)
				{
					if (comEventSink2._sourceIid == sourceIid)
					{
						comEventSink = comEventSink2;
						break;
					}
					if (comEventSink2._sourceIid == Guid.Empty)
					{
						comEventSink2.Initialize(rcw, sourceIid);
						comEventSink = comEventSink2;
					}
				}
				if (comEventSink == null && createIfNotFound)
				{
					comEventSink = new ComEventSink(rcw, sourceIid);
					list.Add(comEventSink);
				}
			}
			return comEventSink;
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x00214A04 File Offset: 0x00212C04
		public void AddHandler(int dispid, object func)
		{
			string name = string.Format(CultureInfo.InvariantCulture, "[DISPID={0}]", new object[]
			{
				dispid
			});
			lock (this._lockObject)
			{
				ComEventSink.ComEventSinkMethod comEventSinkMethod = this.FindSinkMethod(name);
				if (comEventSinkMethod == null)
				{
					if (this._comEventSinkMethods == null)
					{
						this._comEventSinkMethods = new List<ComEventSink.ComEventSinkMethod>();
					}
					comEventSinkMethod = new ComEventSink.ComEventSinkMethod();
					comEventSinkMethod._name = name;
					this._comEventSinkMethods.Add(comEventSinkMethod);
				}
				ComEventSink.ComEventSinkMethod comEventSinkMethod2 = comEventSinkMethod;
				comEventSinkMethod2._handlers = (Func<object[], object>)Delegate.Combine(comEventSinkMethod2._handlers, new Func<object[], object>(new SplatCallSite(func).Invoke));
			}
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x00214AC0 File Offset: 0x00212CC0
		public void RemoveHandler(int dispid, object func)
		{
			string name = string.Format(CultureInfo.InvariantCulture, "[DISPID={0}]", new object[]
			{
				dispid
			});
			lock (this._lockObject)
			{
				ComEventSink.ComEventSinkMethod comEventSinkMethod = this.FindSinkMethod(name);
				if (comEventSinkMethod != null)
				{
					Delegate[] invocationList = comEventSinkMethod._handlers.GetInvocationList();
					foreach (Delegate @delegate in invocationList)
					{
						SplatCallSite splatCallSite = @delegate.Target as SplatCallSite;
						if (splatCallSite != null && splatCallSite._callable.Equals(func))
						{
							ComEventSink.ComEventSinkMethod comEventSinkMethod2 = comEventSinkMethod;
							comEventSinkMethod2._handlers = (Func<object[], object>)Delegate.Remove(comEventSinkMethod2._handlers, @delegate as Func<object[], object>);
							break;
						}
					}
					if (comEventSinkMethod._handlers == null)
					{
						this._comEventSinkMethods.Remove(comEventSinkMethod);
					}
					if (this._comEventSinkMethods.Count == 0)
					{
						this.Dispose();
					}
				}
			}
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x00214BC0 File Offset: 0x00212DC0
		public object ExecuteHandler(string name, object[] args)
		{
			ComEventSink.ComEventSinkMethod comEventSinkMethod = this.FindSinkMethod(name);
			if (comEventSinkMethod != null && comEventSinkMethod._handlers != null)
			{
				return comEventSinkMethod._handlers(args);
			}
			return null;
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x00214BEE File Offset: 0x00212DEE
		public FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x00214BF1 File Offset: 0x00212DF1
		public FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x00214BF9 File Offset: 0x00212DF9
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return new MemberInfo[0];
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x00214C01 File Offset: 0x00212E01
		public MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return new MemberInfo[0];
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x00214C09 File Offset: 0x00212E09
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x00214C0C File Offset: 0x00212E0C
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return null;
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x00214C0F File Offset: 0x00212E0F
		public MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return new MethodInfo[0];
		}

		// Token: 0x060069D3 RID: 27091 RVA: 0x00214C17 File Offset: 0x00212E17
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return null;
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x00214C1A File Offset: 0x00212E1A
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x00214C1D File Offset: 0x00212E1D
		public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return new PropertyInfo[0];
		}

		// Token: 0x17001D94 RID: 7572
		// (get) Token: 0x060069D6 RID: 27094 RVA: 0x00214C25 File Offset: 0x00212E25
		public Type UnderlyingSystemType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x00214C31 File Offset: 0x00212E31
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.ExecuteHandler(name, args);
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x00214C3C File Offset: 0x00212E3C
		public void Dispose()
		{
			this.DisposeAll();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x00214C4C File Offset: 0x00212E4C
		~ComEventSink()
		{
			this.DisposeAll();
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x00214C78 File Offset: 0x00212E78
		private void DisposeAll()
		{
			if (this._connectionPoint == null)
			{
				return;
			}
			if (this._adviseCookie == -1)
			{
				return;
			}
			try
			{
				this._connectionPoint.Unadvise(this._adviseCookie);
				Marshal.ReleaseComObject(this._connectionPoint);
			}
			catch (Exception ex)
			{
				COMException ex2 = ex as COMException;
				if (ex2 != null && ex2.ErrorCode == -2147220992)
				{
					throw;
				}
			}
			finally
			{
				this._connectionPoint = null;
				this._adviseCookie = -1;
				this._sourceIid = Guid.Empty;
			}
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x00214D28 File Offset: 0x00212F28
		private ComEventSink.ComEventSinkMethod FindSinkMethod(string name)
		{
			if (this._comEventSinkMethods == null)
			{
				return null;
			}
			return this._comEventSinkMethods.Find((ComEventSink.ComEventSinkMethod element) => element._name == name);
		}

		// Token: 0x040032A3 RID: 12963
		private Guid _sourceIid;

		// Token: 0x040032A4 RID: 12964
		private IConnectionPoint _connectionPoint;

		// Token: 0x040032A5 RID: 12965
		private int _adviseCookie;

		// Token: 0x040032A6 RID: 12966
		private List<ComEventSink.ComEventSinkMethod> _comEventSinkMethods;

		// Token: 0x040032A7 RID: 12967
		private object _lockObject = new object();

		// Token: 0x02000A5A RID: 2650
		private class ComEventSinkMethod
		{
			// Token: 0x040032A8 RID: 12968
			public string _name;

			// Token: 0x040032A9 RID: 12969
			public Func<object[], object> _handlers;
		}
	}
}
